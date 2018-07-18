using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    // Author: Bhat Pavana
    interface IUpdateStockManagementService
    {
        void closeVoucher(StockVoucher sv, string approvedBy);

        void submitRetrievalForm();
        
    }
}
