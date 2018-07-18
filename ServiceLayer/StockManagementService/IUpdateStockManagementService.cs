using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    interface IUpdateStockManagementService
    {

        // for when stock is rejected at reimbursement. A StockTransaction
        // is required to add the stock back into the inventory, and a 
        // subsequent StockVoucher needs to be raised
        void rejectStock(StockTransaction st, StockVoucher sv);
        
        void closeVoucher(StockVoucher sv);

        void submitRetrievalForm();
        
    }
}
