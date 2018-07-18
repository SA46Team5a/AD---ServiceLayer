using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;
namespace ServiceLayer
{
    interface IDisbursementService
    {
        // Retrieve
        List<Disbursement> getDisbursementsByDep(string depId);

        // Create
        void addDisbursementDuty(string empId);

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
