using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer.RequisitionService
{
    //Author: Benedict
    public class RequisitionService : IRequisitionService
    {
        static StationeryStoreEntities context = StationeryStoreEntities.Instance;
        //Retreive
        //To get the unique unsubmitted form of an employee
        public Requisition getUnsubmittedRequisitionOfEmployee(string empId)
        {
            return context.Requisitions.
                First(r=>r.EmployeeID == empId 
                && r.ApprovalStatusID==1);
        }

        public Requisition getRequisitionById(int reqId)
        {
            return context.Requisitions.First(r => r.RequisitionID == reqId);
        }

        public List<Requisition> getRequisitionsOfEmployee(string empId)
        {
            return context.Requisitions.
                Where(r => r.EmployeeID == empId 
                && r.ApprovalStatusID != 5).ToList();
        }

        public List<Requisition> getPendingRequisitionsOfDep(string depId)
        {            
            return context.Requisitions.
                Where(r => r.ApprovalStatus.ApprovalStatusID == 2 
                && r.Requester.DepartmentID == depId).ToList();
        }

        //Create
        public void createNewRequsitionForEmployee(string empId)
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
        }

        public void addNewRequisitionDetail(Requisition req, RequisitionDetail rd)
        {
            rd.Requisition = req;
            context.RequisitionDetails.Add(rd);
            context.SaveChanges();
        }

        public void addNewRequisitionDetail(int reqId, string itemId, int qtyRequested)
        {
            RequisitionDetail rd = new RequisitionDetail();
            rd.RequisitionID = reqId;
            rd.ItemID = itemId;
            rd.Quantity = qtyRequested;
            context.RequisitionDetails.Add(rd);
            context.SaveChanges();
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
            r.RequestedDate = DateTime.Today;
            context.SaveChanges();
        }

        public void approveRequisition(int reqId)
        {
            Requisition r = context.Requisitions.First(rq => rq.RequisitionID == reqId);
            r.ApprovalStatusID = 3;
            r.ApproveDate = DateTime.Today;
            context.SaveChanges();
        }

        public void rejectRequisition(int reqId)
        {
            Requisition r = context.Requisitions.First(rq => rq.RequisitionID == reqId);
            r.ApprovalStatusID = 4;
            r.ApproveDate = DateTime.Today;
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
            RequisitionDetail rd = context.RequisitionDetails.First(rdt => rdt.RequisitionID == reqDetailId);
            context.RequisitionDetails.Remove(rd);
            context.SaveChanges();
        }             
    }
}
