using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;
namespace ServiceLayer
{
    // Author: Jack
    public interface IDisbursementService
    {
        // Retrieve
        RetrievalFormPayload getRetrievalForm(string empId);
        RetrievalFormPayload generateNewRetrievalForm(string empId);
        RetrievalFormPayload generateRetrievalFormFromDisbursementDuty(int disDutyId);
        DisbursementDuty getDisbursementDutyById(int disDutyId);
        DisbursementDuty getDisbursementDutyByStoreClerkEmpId(string empId);
        Disbursement getDisbursementById(int disId);
        List<Disbursement> getUncollectedDisbursementsByDep(string depId);
        List<DisbursementDetailPayload> getUncollectedDisbursementDetailsByDep(string depId);
        List<DisbursementDetail> getDisbursementDetailsByReqId(int reqId);
        int getTotalCountOfItemDisbursedForReqDetailId(int reqId);
        List<Department> getDepartmentsWithDisbursements();

        // Create
        int addDisbursementDuty(string empId);
        int addDisbursementFromRequisition(int reqId, int disDutyId);
        void addDisbursementDetailFromRequsitionDetail(int reqDetailId, int disId, int quantity);

        // Update
        void allocateRetrievalToDisbursementDetails(List<RequisitionDetail> requisitionDetails, DisbursementDuty disDuty, int qty);

        // creates a stock adjustment voucher if 
        // quantity collected != quantity issued
        void submitRetrievalForm(int disDutyId, Dictionary<string, int> itemsAndQtys);
        void submitDisbursementOfDep(string depId, List<DisbursementDetailPayload> items, string empId);
        void adjustStockFromRejectedDisbursement(DisbursementDetailPayload di, string empId);
        void allocateCollectedQuantityToDisbursementDetails(List<DisbursementDetail> disbursementDetails, int collectedQty, string reason);
        void updateRequsitionRetrievalStatusBasedOnTotalDisbursed(int disDutyId);


    }
}
