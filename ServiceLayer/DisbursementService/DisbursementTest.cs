using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    // Author: Jack
    class DisbursementTest
    {
        static StationeryStoreEntities context = StationeryStoreEntities.Instance;

        static void Main(string[] args)
        {
            IDisbursementService disbursementService = new DisbursementService();
            IRequisitionService requisitionService = new RequisitionService();
            clearTables();


        }

        static void clearTables()
        {
            // Clear tables for testing
            context.Requisitions.RemoveRange(context.Requisitions);
            context.RequisitionDetails.RemoveRange(context.RequisitionDetails);
            context.DisbursementDuties.RemoveRange(context.DisbursementDuties);
            context.Disbursements.RemoveRange(context.Disbursements);
            context.DisbursementDetails.RemoveRange(context.DisbursementDetails);
            context.SaveChanges();
        }
    }
}
