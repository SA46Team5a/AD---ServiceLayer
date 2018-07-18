using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    interface ICreateStockManagementService
    {

        // Create
        void addStockTransaction(StockTransaction st);
        void addStockVoucher(StockVoucher sv);
        // for when stock is rejected at reimbursement. A StockTransaction
        // is required to add the stock back into the inventory, and a 
        // subsequent StockVoucher needs to be raised

        void submitStockCountItems(int empId);
        void submitVouchers();


    }
}
