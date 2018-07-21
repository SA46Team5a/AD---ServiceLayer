using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    // Author: Jack
    public class DisbursementService : IDisbursementService
    {
        StationeryStoreEntities context = StationeryStoreEntities.Instance;
        IStockManagementService stockManagementService = new StockManagementService();
        IDepartmentService departmentService = new DepartmentService();

        // Retrieve
        public List<RetrievalItem> generateRetrievalForm(string empId)
        {
            // Get list of items to retrieve
            List<RetrievalItem> retrievalItems = context.RetrievalItems.ToList();

            // Change status of requsitions to retrieveing so that they won't be 
            // doubly selected for retrieval
            List<Requisition> requisitions = context.Requisitions
                .Where(r => r.RetrievalStatusID == 1 || r.RetrievalStatusID == 3)
                .ToList();
            requisitions.ForEach(r => r.RetrievalStatusID = 2);
            context.SaveChanges();

            // Assign duty to employee
            int disDutyId = addDisbursementDuty(empId);

            // Record requisitions into disbursements
            requisitions.ForEach(r => addDisbursementFromRequisition(r.RequisitionID, disDutyId));

            return new List<RetrievalItem>();
        }

        public DisbursementDuty getDisbursementDutyById(int disDutyId)
            => context.DisbursementDuties.First(dd => dd.DisbursementDutyID == disDutyId);

        public Disbursement getDisbursementById(int disId)
            =>  context.Disbursements.First(dd => dd.DisbursementID == disId);

        public List<Disbursement> getUncollectedDisbursementsByDep(string depId)
            =>  context.Disbursements
            .Where(d => d.Requisition.Requester.DepartmentID == depId && d.CollectedBy == null && d.DisbursementDuty.isRetrieved)
            .ToList();

        public List<DisbursementDetail> getUncollectedDisbursementDetailsByDep(string depId)
            =>  context.DisbursementDetails
            .Where(d => d.Disbursement.Requisition.Requester.DepartmentID == depId && d.Disbursement.CollectedBy == null && d.Disbursement.DisbursementDuty.isRetrieved)
            .ToList();

        public List<DisbursementDetail> getDisbursementDetailsByReqId(int reqId)
            => context.DisbursementDetails.Where(d => d.RequisitionDetailsID == reqId).ToList();

        public int getTotalCountOfItemDisbursedForReqId(int reqId)
            => (int) context.DisbursementDetails
            .Where(dd => dd.Disbursement.RequisitionID == reqId)
            .Select(dd => dd.CollectedQty)
            .Sum();

        // Create
        public int addDisbursementDuty(string empId)
        {
            DisbursementDuty disbursementDuty = new DisbursementDuty();
            disbursementDuty.DisbursementDate = DateTime.Now;
            disbursementDuty.isRetrieved = false;
            disbursementDuty.StoreClerkID = empId;

            context.DisbursementDuties.Add(disbursementDuty);
            context.SaveChanges();

            return disbursementDuty.DisbursementDutyID;
        }

        public int addDisbursementFromRequisition(int reqId, int disDutyId)
        {
            Disbursement disbursement = new Disbursement();
            disbursement.RequisitionID = reqId;
            disbursement.DisbursementDutyID = disDutyId;

            context.Disbursements.Add(disbursement);
            context.SaveChanges();

            return disbursement.DisbursementID;
        }

        public void addDisbursementDetailFromRequsitionDetail(int reqDetailId, int disId, int quantity)
        {
            DisbursementDetail disbursementDetail = new DisbursementDetail();
            disbursementDetail.DisbursementID = disId;
            disbursementDetail.RequisitionDetailsID = reqDetailId;
            disbursementDetail.Quantity = quantity;

            context.DisbursementDetails.Add(disbursementDetail);
            context.SaveChanges();
        }

        // Allocate retrieved quantities 
        public void allocateRetrievalToDisbursementDetails(List<RequisitionDetail> requisitionDetails, DisbursementDuty disDuty, int qty)
        {
            requisitionDetails = requisitionDetails.OrderBy(rd => rd.Requisition.RequestedDate).ToList();
            foreach (RequisitionDetail rd in requisitionDetails)
            {
                int outstandingQty = rd.Quantity - getTotalCountOfItemDisbursedForReqId(rd.RequisitionID);
                if (qty > 0 && outstandingQty > 0)
                {
                    int qtyToDisburse = Math.Min(qty, outstandingQty);
                    int disId = disDuty.Disbursements.First(d => d.Requisition.RequisitionID == rd.RequisitionID).DisbursementID;
                    addDisbursementDetailFromRequsitionDetail(rd.RequisitionDetailsID, disId, qtyToDisburse);
                    qty -= qtyToDisburse;
                }
                else if (qty <= 0)
                    break;
            }
        }

        // Update
        // Store clerk presses submit on the stationery retrieval form screen
        public void submitRetrievalForm(int disDutyId, Dictionary<string, int> itemsAndQtys) 
        {
            DisbursementDuty disbursementDuty = getDisbursementDutyById(disDutyId);
            // Allocate the retrieved items into respective Disbursements. Priority is on a first-come-first-serve basis.
            foreach (KeyValuePair<string, int> item in itemsAndQtys)
            {
                // Get all requisition details served by disbursement duty
                List<Disbursement> disbursements = disbursementDuty.Disbursements.ToList();
                List<RequisitionDetail> requisitionDetails = new List<RequisitionDetail>();
                disbursements.ForEach(d => requisitionDetails
                    .AddRange(d.Requisition.RequisitionDetails.Where(rd => rd.ItemID == item.Key).ToList()));

                allocateRetrievalToDisbursementDetails(requisitionDetails, disbursementDuty, item.Value);
            }

            // update retrieval status of each requisition within disbursement duty to retrieved
            updateRequsitionRetrievalStatusBasedOnTotalDisbursed(disDutyId);
            disbursementDuty.Disbursements.ToList().ForEach(d => d.Requisition.RetrievalStatusID = 4);
            disbursementDuty.isRetrieved = true;
            context.SaveChanges();
        }

        public void submitDisbursementOfDep(List<int> disDutyIds, string depId, List<DisbursementItem> items, string empId)
        {
            List<DisbursementDetail> disbursementDetails = context.DisbursementDetails
                .Where(d => disDutyIds.Contains(d.Disbursement.DisbursementDutyID))
                .OrderBy(d => d.Disbursement.Requisition.RequestedDate)
                .ToList();
 
            // If there are rejected items, adjust stock and submit stock voucher.
            foreach (DisbursementItem item in items)
            {
                List<DisbursementDetail> disDetailsOfItemId = disbursementDetails.Where(d => d.RequisitionDetail.ItemID == item.ItemId).ToList();
                if (item.RejectedQuantity > 0)
                {
                    adjustStockFromRejectedDisbursement(item, empId);
                    allocateCollectedQuantityToDisbursementDetails(
                        disDetailsOfItemId,
                        item.CollectedQuantity,
                        item.Reason);
                }
                else
                    disDetailsOfItemId.ForEach(d => d.CollectedQty = d.Quantity);
                context.SaveChanges();
            }

            // Update disbursement with department rep
            List<Disbursement> disbursements = disbursementDetails
                .Where(d => d.RequisitionDetail.Requisition.Requester.DepartmentID == depId)
                .Select(d => d.Disbursement)
                .Distinct()
                .ToList();

            foreach (Disbursement d in disbursements)
            {
                d.CollectedBy = departmentService.getCurrentDepartmentRepresentative(depId).DeptRepID;
                d.CollectionDate = DateTime.Now;
            }
            context.SaveChanges();

            // Update retrieval status based on collected quantity
            disDutyIds.ForEach(d => updateRequsitionRetrievalStatusBasedOnTotalDisbursed(d));
        }

        public void adjustStockFromRejectedDisbursement(DisbursementItem di, string empId)
        {
            // do a stock transaction to add qty - qtyCollected back to stock
            stockManagementService.addStockTransaction(di.ItemId, di.Reason, empId, di.RejectedQuantity);

            // raise a stock voucher
            int actualStockCount = stockManagementService.getStockCountOfItem(di.ItemId) - di.RejectedQuantity;
            stockManagementService.addStockVoucher(di.ItemId, actualStockCount, empId, di.Reason);
            
            context.SaveChanges();
        }

        public void allocateCollectedQuantityToDisbursementDetails(List<DisbursementDetail> disbursementDetails, int collectedQty, string reason)
        {
            disbursementDetails = disbursementDetails.OrderBy(d => d.RequisitionDetail.Requisition.RequestedDate).ToList();

            foreach (DisbursementDetail dd in disbursementDetails)
            {
                if (collectedQty > 0)
                {
                    dd.CollectedQty = Math.Min(dd.Quantity, collectedQty);
                    collectedQty -= dd.Quantity;
                    dd.Reason = reason;
                }
                else
                    break;
            }
            context.SaveChanges();
        }

        public void updateRequsitionRetrievalStatusBasedOnTotalDisbursed(int disDutyId)
        {
            DisbursementDuty disbursementDuty = getDisbursementDutyById(disDutyId);
            // update retrieval status of each requisition within disbursement duty
            foreach (Disbursement disbursement in disbursementDuty.Disbursements)
            {
                bool fullyRetrieved = true;
                foreach (RequisitionDetail reqDetail in disbursement.Requisition.RequisitionDetails)
                {
                    int qtyDisbursed = getTotalCountOfItemDisbursedForReqId(reqDetail.RequisitionDetailsID);
                    if (qtyDisbursed < reqDetail.Quantity)
                    {
                        fullyRetrieved = false;
                        break;
                    }
                }

                int reqId = disbursement.Requisition.RequisitionID;
                if (fullyRetrieved)
                    context.Requisitions.First(r => r.RequisitionID == reqId).RetrievalStatusID = 4;
                else
                    context.Requisitions.First(r => r.RequisitionID == reqId).RetrievalStatusID = 3;
            }
            context.SaveChanges();
        }
    }
}
