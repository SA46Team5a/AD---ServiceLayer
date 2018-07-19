using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;
namespace ServiceLayer
{
    interface IRequisitionService
    {
        // Retrieve
        Requisition getUnsubmittedRequisitionOfEmployee(string empId);
        Requisition getRequisitionById(int reqId);
        List<Requisition> getRequisitionsOfEmployee(string empId);
        List<Requisition> getPendingRequisitionsOfDep(string depId);

        // Create
        void createNewRequsitionForEmployee(string empId); 
        void addNewRequisitionsDetails(Requisition req, Dictionary<string, int> itemAndQty);
        void addNewRequisitionDetail(Requisition req, RequisitionDetail rd);
        void addNewRequisitionDetail(int reqId, string itemId, int qtyRequested);

        // Update        
        void editRequisitionDetailQtys(Dictionary<int, int> reqDetailIdAndQtys);
        void editRequisitionDetailQty(int reqDetailId, int qty);

        void submitRequisition(int reqId); 
        void rejectRequisition(int reqId); 
        void approveRequisition(int reqId);

        // Delete
        void deleteRequisition(int reqId);
        void deleteRequisitionDetail(int reqDetailId);
    }
}
