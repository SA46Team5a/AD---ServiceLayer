using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    // Author: Bhat Pavana
    interface ICreateStockManagementService
    {

        // Create Transaction record
        void addStockTransaction(string itemId, string description, string employeeId, int adjustment);

        //create stock voucher record
        void addStockVoucher(string itemId, int actualcount, string employeeId,string reason);

        /*for when stock is rejected at reimbursement. A StockTransaction is required to add the stock back into the inventory, and a         
        subsequent StockVoucher needs to be raised */       
        void rejectStock(string itemId, string reason, int count, string employeeId);

        void submitStockCountItems(int empId);
        void submitVouchers();


    }
}
