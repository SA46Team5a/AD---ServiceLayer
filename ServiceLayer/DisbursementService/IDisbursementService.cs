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
        DisbursementDuty getDisbursementDutyById(int disDutyId);
        Disbursement getDisbursementById(int disId);
        List<Disbursement> getDisbursementsByDep(string depId);

        // Create
        void addDisbursementDuty(string empId);
        void addDisbursement(Dictionary<string, int> itemsAndQty, int disDutyId);
        void addDisbursementDetail(string itemId, int qty, int reqId);

        // Update
        void confirmDisbursements(List<int> disbursementIds);

        // creates a stock adjustment voucher if 
        // quantity collected != quantity issued
        void confirmDisbursementDetail(
            List<int> disbursementDetailIds,
            List<int> quantitiesCollected,
            List<String> reasons
            );
        
        // Delete

    }
}
