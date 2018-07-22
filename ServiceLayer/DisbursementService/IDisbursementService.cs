﻿using System;
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
        List<RetrievalItem> generateRetrievalForm(string empId);
        DisbursementDuty getDisbursementDutyById(int disDutyId);
        Disbursement getDisbursementById(int disId);
        List<Disbursement> getUncollectedDisbursementsByDep(string depId);
        List<DisbursementDetail> getUncollectedDisbursementDetailsByDep(string depId);
        List<DisbursementDetail> getDisbursementDetailsByReqId(int reqId);
        int getTotalCountOfItemDisbursedForReqId(int reqId);

        // Create
        int addDisbursementDuty(string empId);
        int addDisbursementFromRequisition(int reqId, int disDutyId);
        void addDisbursementDetailFromRequsitionDetail(int reqDetailId, int disId, int quantity);

        // Update
        void allocateRetrievalToDisbursementDetails(List<RequisitionDetail> requisitionDetails, DisbursementDuty disDuty, int qty);

        // creates a stock adjustment voucher if 
        // quantity collected != quantity issued
        void submitRetrievalForm(int disDutyId, Dictionary<string, int> itemsAndQtys);
        void submitDisbursementOfDep(List<int> disDutyIds, string depId, List<DisbursementItem> items, string empId);
        void adjustStockFromRejectedDisbursement(DisbursementItem di, string empId);
        void allocateCollectedQuantityToDisbursementDetails(List<DisbursementDetail> disbursementDetails, int collectedQty, string reason);
        void updateRequsitionRetrievalStatusBasedOnTotalDisbursed(int disDutyId);


    }
}
