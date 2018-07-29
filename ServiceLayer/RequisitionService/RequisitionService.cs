using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    //Author: Benedict
    public class RequisitionService : IRequisitionService
    {
        static StationeryStoreEntities context = StationeryStoreEntities.Instance;
        //Retreive
        //To get the unique unsubmitted form of an employee
        public Requisition getUnsubmittedRequisitionOfEmployee(string empId)
        {
            Requisition r = context.Requisitions.
                FirstOrDefault(rq => rq.EmployeeID.Equals(empId)
                && rq.ApprovalStatusID==1);

            if (r == null)
                return createNewRequsitionForEmployee(empId);
            else
                return r;
        }

        public List<RequisitionDetail> getRequisitionDetails(int reqId)
        {
            return context.RequisitionDetails.Where(r => r.RequisitionID == reqId).ToList();
        }

        public RequisitionDetail getRequisitionDetailById(int reqDetailId)
        {
            return context.RequisitionDetails.Where(r => r.RequisitionDetailsID == reqDetailId).FirstOrDefault();
        }

        public Requisition getRequisitionById(int reqId) => 
            context.Requisitions.FirstOrDefault(r => r.RequisitionID == reqId);

        
        public List<Requisition> getRequisitionsOfEmployee(string empId)
        {
            return context.Requisitions.
                Where(r => r.EmployeeID == empId 
                && r.ApprovalStatusID != 5).
                OrderByDescending(r => r.RequestedDate).ToList();
        }

        public List<Requisition> getPendingRequisitionsOfDep(string depId)
        {            
            return context.Requisitions.
                Where(r => r.ApprovalStatus.ApprovalStatusID == 2 
                && r.Requester.DepartmentID == depId).
                OrderByDescending(r => r.RequestedDate).ToList();
        }

        public List<OutstandingRequisitionView> getOutstandingRequisitionDetails()
            => context.OutstandingRequisitionViews.ToList();

        int getCountOfOutstandingRequisitions()
            => getOutstandingRequisitionDetails().Select(o => o.RequisitionID).Distinct().ToList().Count;

        //Create
        public Requisition createNewRequsitionForEmployee(string empId)
        {
            // insert new Requisition object with 
            // 1. Employee ID or Employee
            Requisition r = new Requisition();
            r.EmployeeID = empId;

            // 2. Approval status set to Unsubmitted
            r.ApprovalStatusID = 1;

            // 3. insert to DB
            context.Requisitions.Add(r);
            context.SaveChanges();
            return r;
        }

        public void addNewRequisitionDetail(Requisition req, RequisitionDetail rd)
        {
            rd.Requisition = req;
            context.RequisitionDetails.Add(rd);
            context.SaveChanges();
        }

        private bool isItemInRequisitionForm(int reqId, string itemId)
        {
            int count = context.RequisitionDetails.
                Count(rd => rd.RequisitionID == reqId && rd.ItemID == itemId);

            return count > 0;
        }

        public void addNewRequisitionDetail(int reqId, string itemId, int qtyRequested)
        {
            //guard against double entry
            if (!isItemInRequisitionForm(reqId, itemId))
            {
                RequisitionDetail rd = new RequisitionDetail();
                rd.RequisitionID = reqId;
                rd.ItemID = itemId;
                rd.Quantity = qtyRequested;
                context.RequisitionDetails.Add(rd);
                context.SaveChanges();
            }
        }

        public void addNewRequisitionsDetails(Requisition req, Dictionary<string, int> itemAndQty)
        {
            foreach (KeyValuePair<string, int> entry in itemAndQty)
            {
                addNewRequisitionDetail(req.RequisitionID, entry.Key, entry.Value);
            }
        }

        //Update
        // for editing of requisition quantities
        public void editRequisitionDetailQty(int reqDetailId, int qty)
        {
            RequisitionDetail rd = context.RequisitionDetails.First(rdt => rdt.RequisitionDetailsID == reqDetailId);
            rd.Quantity = qty;
            context.SaveChanges();

        }

        public void editRequisitionDetailQtys(Dictionary<int, int> reqDetailIdAndQtys)
        {
            foreach (KeyValuePair<int, int> entry in reqDetailIdAndQtys)
            {
                editRequisitionDetailQty(entry.Key, entry.Value);
            }
        }

        // 1. Change approvalStatus from unsubmitted to pending. 
        // 2. Set RequestedDate to Today
        // 3. Create new Requsition object for employee
        public void submitRequisition(int reqId)
        {
            Requisition r = context.Requisitions.First(rq => rq.RequisitionID == reqId);
            r.ApprovalStatusID = 2;
            r.RetrievalStatusID = 1;
            r.RequestedDate = DateTime.Today;
            //guard against double entry during testing
            if(context.Requisitions.FirstOrDefault(rq => rq.EmployeeID == r.EmployeeID && rq.ApprovalStatusID == 1) == null)
                createNewRequsitionForEmployee(r.EmployeeID);
            context.SaveChanges();
        }

        public void processRequisition(int reqId, string empId, bool toApprove)
        {
            int authId = context.Authorities
                .OrderBy(a => a.StartDate)
                .ToList()
                .Last(a => a.EmployeeID == empId && a.StartDate < DateTime.Today)
                .AuthorityID;

            if (toApprove)
                approveRequisition(reqId, authId);
            else
                rejectRequisition(reqId, authId);
        }

        public void approveRequisition(int reqId, int authId)
        {
            Requisition r = context.Requisitions.First(rq => rq.RequisitionID == reqId);
            r.ApprovalStatusID = 3;
            r.ApproveDate = DateTime.Today;
            r.AuthorityID = authId;
            context.SaveChanges();
        }

        public void rejectRequisition(int reqId, int authId)
        {
            Requisition r = context.Requisitions.First(rq => rq.RequisitionID == reqId);
            r.ApprovalStatusID = 4;
            r.ApproveDate = DateTime.Today;
            r.AuthorityID = authId;
            context.SaveChanges();
        }

        //Delete
        public void deleteRequisition(int reqId)
        {
            Requisition r = context.Requisitions.First(rq => rq.RequisitionID == reqId);
            r.ApprovalStatusID = 5;
            r.ApproveDate = DateTime.Today;
            context.SaveChanges();
        }

        public void deleteRequisitionDetail(int reqDetailId)
        {
            RequisitionDetail rd = context.RequisitionDetails.First(rdt => rdt.RequisitionDetailsID == reqDetailId);
            context.RequisitionDetails.Remove(rd);
            context.SaveChanges();
        }
        
    }
}
