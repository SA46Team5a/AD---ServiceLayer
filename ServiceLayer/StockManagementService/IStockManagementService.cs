using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;
namespace ServiceLayer
{
    interface IStockManagementService
    {
        // Retrieve
        List<Item> getAllItems();
        int getStockCountOfItem(int itemId);
        List<Item> getItemsOfCategory(int categoryId);
        List<StockVoucher> getOpenVouchers();
        List<Supplier> getFirstSupplierOfItem();
        List<Supplier> getSupplierOfItem(int itemId);

        // Create
        void addStockTransaction(StockTransaction st);
        void addStockVoucher(StockVoucher sv);
        // for when stock is rejected at reimbursement. A StockTransaction
        // is required to add the stock back into the inventory, and a 
        // subsequent StockVoucher needs to be raised
        void rejectStock(StockTransaction st, StockVoucher sv);

        // Update
        void closeVoucher(StockVoucher sv);

        // Delete
    }
}
