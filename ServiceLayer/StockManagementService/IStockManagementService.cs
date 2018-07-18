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
        // Retrieves list of items
        List<Item> getAllItems();
        // Retrieves stock count of the item 
        int getStockCountOfItem(string itemId);
        List<Item> getItemsOfCategory(int categoryId);
        // Returns a list of vouchers that have not been approved by supervisor or manager
        List<StockVoucher> getOpenVouchers();               
        //Gets the first supplier for the specified item in the parameter.
        Supplier getFirstSupplierOfItem(string itemId);
        //List<Supplier> getFirstSupplierOfItem();
        //Retrieves list of all the suppliers for the item
        List<Supplier> getSupplierOfItem(string itemId);
        List<StockCountItem> getStockCountItemsByCategory(int cat);
        float getItemCost(string itemID);

        // Create Transaction record
        void addStockTransaction(string itemId, string description, string employeeId, int adjustment);

        //create stock voucher record
        void addStockVoucher(string itemId, int actualcount, string employeeId, string reason);

        /*for when stock is rejected at reimbursement. A StockTransaction is required to add the stock back into the inventory, and a         
        subsequent StockVoucher needs to be raised */
        void rejectStock(string itemId, string reason, int count, string employeeId);

        void submitStockCountItems(int empId);
        void submitVouchers();

        void closeVoucher(StockVoucher sv, string approvedBy);

        void submitRetrievalForm();



    }
}
