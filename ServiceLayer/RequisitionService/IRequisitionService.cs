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
        Requisition getUnsbumittedRequisitionOfEmployee(string empId);
        Requisition getRequisitionById(int reqId);
        List<Requisition> getRequisitionsOfEmployee(string empId);
        List<Requisition> getSubmittedRequisitionsOfDep(string depId);

        // Create
        Requisition createNewRequsitionForEmployee(string empId);
        void addNewRequisitionsDetails(Requisition req, Dictionary<int, int> itemAndQty);
        void addNewRequisitionDetail(Requisition req, RequisitionDetail rd);

        // Update
        // for editing of requisition quantities
        void editRequisitionDetailQtys(List<int> reqDetailIds, List<int> qtys);
        void editRequisitionDetailQty(int reqDetailId, int qty);

        void submitRequisition(int reqId);
        void rejectRequisition(int reqId);
        void approveRequisition(int reqId);

        // Delete
        void deleteRequisition(int reqId);
        void deleteRequisitionDetail(int reqDetailId);
    }
}
