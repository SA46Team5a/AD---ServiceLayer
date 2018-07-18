using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    // Author: Bhat Pavana
    partial class RetrieveStockManagementService : IRetrieveStockManagementService
    {

        public  List<Item> getAllItems()
        {
            return new List<Item>();
        }

        public int getStockCountOfItem(string itemId)
        {
            return 0;
        }

        public List<Item> getItemsOfCategory(int categoryId)
        {
            return new List<Item>();
        }

        public List<StockVoucher> getOpenVouchers()
        {
            return new List<StockVoucher>(); 
        }

        public Supplier getFirstSupplierOfItem(string itemId)
        {
            return new Supplier();
        }

        public List<Supplier> getSupplierOfItem(string itemId)
        {
            return new List<Supplier>();
        }

        public List<StockCountItem> getStockCountItemsByCategory(int cat)
        {
            return new List<StockCountItem>();
        }

    }
}
