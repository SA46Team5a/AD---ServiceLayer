using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    partial class UpdateStockManagementService:IUpdateStockManagementService
    {
        StationeryStoreEntities context = new StationeryStoreEntities();
       
        //close the voucher by updating the employeeId of the person who approved and update the ApprovedDate to today's date
        public void closeVoucher(StockVoucher sv,string approvedBy)
        {
            sv.ApprovedBy = approvedBy;
            sv.ApprovedDate = DateTime.Today;            
            context.SaveChanges();
            return;
        }
        public void submitRetrievalForm()
        {

        }
    }
}
