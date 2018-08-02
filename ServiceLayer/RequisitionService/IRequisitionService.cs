using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;
namespace ServiceLayer
{
    public interface IRequisitionService
    {
        // Retrieve
        Requisition getUnsubmittedRequisitionOfEmployee(string empId);
        Requisition getRequisitionById(int reqId);
        RequisitionDetail getRequisitionDetailById(int reqDetailId);
        List<Requisition> getRequisitionsOfEmployee(string empId);
        List<Requisition> getPendingRequisitionsOfDep(string depId);
        List<RequisitionDetail> getRequisitionDetails(int reqId);
        List<OutstandingRequisitionView> getOutstandingRequisitionDetails();
        int getCountOfOutstandingRequisitions();
        List<Requisition> getRequisitionsByEmpIdAndStatus(string empId, string statusName);

        // Create
        Requisition createNewRequsitionForEmployee(string empId); 
        void addNewRequisitionsDetails(Requisition req, Dictionary<string, int> itemAndQty);
        void addNewRequisitionDetail(Requisition req, RequisitionDetail rd);
        void addNewRequisitionDetail(int reqId, string itemId, int qtyRequested);

        // Update        
        void editRequisitionDetailQtys(Dictionary<int, int> reqDetailIdAndQtys);
        void editRequisitionDetailQty(int reqDetailId, int qty);

        void submitRequisition(int reqId);
        void processRequisition(int reqId, string empId, bool toApprove, IDepartmentService departmentService);
        void rejectRequisition(int reqId, int authId); 
        void approveRequisition(int reqId, int authId);

        // Delete
        void deleteRequisition(int reqId);
        void deleteRequisitionDetail(int reqDetailId);
    }
}
