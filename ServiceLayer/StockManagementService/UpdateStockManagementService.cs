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

        public void rejectStock(StockTransaction st, StockVoucher sv)
        {
            return;
        }

        public void closeVoucher(StockVoucher sv)
        {
            return;
        }
        public void submitRetrievalForm()
        {

        }
    }
}
