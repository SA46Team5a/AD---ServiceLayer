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
        public List<RetrievalItem> getRetrievalForm(string empId)
        {
            bool outstandingRetrievalForm = context.DisbursementDuties
                .Count(d => d.StoreClerkID == empId && d.isRetrieved == false) > 0 ;

            if (outstandingRetrievalForm)
            {
               DisbursementDuty disDuty = context.DisbursementDuties
                   .First(d => d.StoreClerkID == empId && d.isRetrieved == false);
     
               return generateRetrievalFormFromDisbursementDuty(disDuty.DisbursementDutyID);
            }
            else
                return generateNewRetrievalForm(empId);
       }

        public List<RetrievalItem> generateNewRetrievalForm(string empId)
        {
            // Get list of items to retrieve
            List<RequisitionDetail> requisitionDetails = context.RequisitionDetails
                .Where(rd => (rd.Requisition.RetrievalStatusID == 1 || rd.Requisition.RetrievalStatusID == 3))
                .ToList();

            List<string> itemsToRetrieve = requisitionDetails.Select(rd => rd.ItemID).Distinct().ToList();

            List<RetrievalItem> retrievalItems = new List<RetrievalItem>();           
            foreach (string item in itemsToRetrieve)
            {
                RetrievalItem retrievalItem = new RetrievalItem();
                retrievalItem.ItemID = item;
                retrievalItem.QtyInStock = stockManagementService.getStockCountOfItem(item);
                int qtyRequested = requisitionDetails
                    .Where(rd => rd.ItemID == item)
                    .Select(rd => rd.Quantity)
                    .Sum();
                int qtyCollected = requisitionDetails
                    .Where(rd => rd.ItemID == item)
                    .Select(rd => (int) rd.DisbursementDetails.Select(dd => dd.CollectedQty).Sum())
                    .Sum();

                retrievalItem.QtyToRetrieve = qtyRequested - qtyCollected;
                retrievalItems.Add(retrievalItem);
            }

            if (retrievalItems.Count > 0)
            {
                // Change status of requsitions to retrieveing so that they won't be 
                // doubly selected for retrieval
     
                List<Requisition> requisitions = context.Requisitions
                    .Where(r => r.RetrievalStatusID == 1 || r.RetrievalStatusID == 3)
                    .ToList();
     
                requisitions.ForEach(r => r.RetrievalStatusID = 2);
                context.SaveChanges();

                // Assign duty to employee
                int disDutyId = addDisbursementDuty(empId);

                foreach (Requisition req in requisitions)
                {
                    // Record requisitions into disbursements
                    int disId = addDisbursementFromRequisition(req.RequisitionID, disDutyId);
                    // Record requisition details into disbursement details
                    req.RequisitionDetails.ToList().ForEach(r => addDisbursementDetailFromRequsitionDetail(r.RequisitionDetailsID, disId, r.Quantity));
                }
            }
            return retrievalItems;
        }

        public List<RetrievalItem> generateRetrievalFormFromDisbursementDuty(int disDutyId)
        {
            List<DisbursementDetail> disbursementDetails = context.DisbursementDetails
                .Where(d => d.Disbursement.DisbursementDutyID == disDutyId)
                .ToList();

            List<string> itemIds = disbursementDetails
                .Select(d => d.RequisitionDetail.ItemID)
                .Distinct()
                .ToList();

            List<RetrievalItem> retrievalItems = new List<RetrievalItem>();

            RetrievalItem retrievalItem;
            foreach (string itemId in itemIds)
            {
                retrievalItem = new RetrievalItem();
                retrievalItem.ItemID = itemId;
                retrievalItem.QtyToRetrieve = disbursementDetails
                    .Where(d => d.RequisitionDetail.ItemID == itemId)
                    .Sum(d => d.Quantity);
                retrievalItem.QtyInStock = stockManagementService.getStockCountOfItem(itemId);
                retrievalItems.Add(retrievalItem);
            }

            return retrievalItems;
        }

        public DisbursementDuty getDisbursementDutyById(int disDutyId)
            => context.DisbursementDuties.First(dd => dd.DisbursementDutyID == disDutyId);

        public DisbursementDuty getDisbursementDutyByStoreClerkEmpId(string empId)
            => context.DisbursementDuties.First(d => !d.isRetrieved && d.StoreClerkID == empId);

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

        public int getTotalCountOfItemDisbursedForReqDetailId(int reqId)
        {
            int? count = context.DisbursementDetails
                .Where(dd => dd.RequisitionDetailsID == reqId)
                .Select(dd => dd.CollectedQty)
                .Sum();

            return count is null ? 0 : (int) count;

        }

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
                int outstandingQty = rd.Quantity - getTotalCountOfItemDisbursedForReqDetailId(rd.RequisitionID);
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
                int qty = Math.Min(item.Value, stockManagementService.getStockCountOfItem(item.Key));
                // Get all requisition details served by disbursement duty
                List<Disbursement> disbursements = disbursementDuty.Disbursements.ToList();
                List<RequisitionDetail> requisitionDetails = new List<RequisitionDetail>();
                disbursements.ForEach(d => requisitionDetails
                    .AddRange(d.Requisition.RequisitionDetails.Where(rd => rd.ItemID == item.Key).ToList()));

                stockManagementService.addStockTransaction(item.Key, "Retrieval" , disbursementDuty.StoreClerkID, -qty);
            }

            // update retrieval status of each requisition within disbursement duty to retrieved
            disbursementDuty.Disbursements.ToList().ForEach(d => d.Requisition.RetrievalStatusID = 4);
            disbursementDuty.isRetrieved = true;
            context.SaveChanges();
        }

        public void submitDisbursementOfDep(List<int> disDutyIds, string depId, List<DisbursementDetailPayload> items, string empId)
        {
            List<DisbursementDetail> disbursementDetails = context.DisbursementDetails
                .Where(d => disDutyIds.Contains(d.Disbursement.DisbursementDutyID))
                .OrderBy(d => d.Disbursement.Requisition.RequestedDate)
                .ToList();

            // If there are rejected items, adjust stock and submit stock voucher.
            foreach (DisbursementDetailPayload item in items)
            {
                List<DisbursementDetail> disDetailsOfItemId = disbursementDetails.Where(d => d.RequisitionDetail.ItemID == item.ItemId).ToList();
                if (item.RejectedQuantity > 0)
                {
                    adjustStockFromRejectedDisbursement(item, empId);
                    allocateCollectedQuantityToDisbursementDetails(
                        disDetailsOfItemId,
                        (int) item.CollectedQuantity,
                        item.Reason);
                }
                else
                {
                    allocateCollectedQuantityToDisbursementDetails(
                        disDetailsOfItemId,
                        (int) item.CollectedQuantity,
                        item.Reason);
                 }
                context.SaveChanges();
            }

            // Update disbursement with department rep
            Department department = context.Departments.First(d => d.DepartmentID == depId);
            List<Disbursement> disbursements = disbursementDetails
                .Where(d => department.Employees.Select(dep => dep.EmployeeID).Contains(d.RequisitionDetail.Requisition.EmployeeID))
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

        public void adjustStockFromRejectedDisbursement(DisbursementDetailPayload di, string empId)
        {
            // do a stock transaction to add qty - qtyCollected back to stock
            stockManagementService.addStockTransaction(di.ItemId, di.Reason, empId, (int) di.RejectedQuantity);

            // raise a stock voucher
            int actualStockCount = stockManagementService.getStockCountOfItem(di.ItemId) - (int) di.RejectedQuantity;
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
                    int qtyDisbursed = getTotalCountOfItemDisbursedForReqDetailId(reqDetail.RequisitionDetailsID);
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
