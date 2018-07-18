using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    // Author: Bhat Pavana
    partial class RetrieveStockManagementService : IStockManagementService
    {
         private StationeryStoreEntities context = new StationeryStoreEntities();
       // static StationeryStoreEntities context = StationeryStoreEntities.Instance;

        public  List<Item> getAllItems()
        {
            //List all the items in the store
            List<Item> itemList = context.Items.ToList();         
            return itemList;
        }
        public List<Category> getAllCategories()
        {
            //List all the categories
            List<Category> categoryList= context.Categories.ToList();
            return categoryList;

        }

        public  int getStockCountOfItem(string itemId)
        {           
            //Get the list of items in Stocktransaction 
            List<StockTransaction> stList= context.StockTransactions.Where(st => st.ItemID == itemId).ToList();
            int sumItem = 0;
            foreach (StockTransaction st in stList)
            {             
                sumItem+=(int)(st.Adjustment is null ?0:st.Adjustment);                    
            }
            return sumItem;
        }

        public List<Item> getItemsOfCategory(int categoryId)
        {
            List<Item> itemList = context.Items.Where(i => i.CategoryID == categoryId).ToList();
            return itemList;
        }

        public List<StockVoucher> getOpenVouchers()
        {
            return context.StockVouchers.Where(sv => sv.ApprovedBy == null).ToList();            
        }

        public Supplier getFirstSupplierOfItem(string itemId)
        {
            //Retrieving the Supplier of the given itemId having Rank 1
            var sItem = context.SupplierItems.Where(si => si.ItemID == itemId).Where(si => si.Rank == 1).First();
            return context.Suppliers.Where(s => s.SupplierID == sItem.SupplierID).First();
            
        }

        public List<Supplier> getSupplierOfItem(string itemId)
        {
            //retrieving list of SupplierItem records matching with the itemId
            List<SupplierItem> listOfSupplier = context.SupplierItems.Where(si => si.ItemID == itemId).ToList();
            
            //Creating a new empty list of suppliers
            List<Supplier> supplierList = new List<Supplier>();
            
            //Adding the record of matched supplier by comparing SupplierId of Supplier entity and listOfSupplier fetched
            foreach (SupplierItem si in listOfSupplier)
            {
                supplierList.Add(context.Suppliers.Where(s => s.SupplierID == si.SupplierID).First());
            }
            
            return supplierList;
        }

        public List<StockCountItem> getStockCountItemsByCategory(int categoryId)
        {
            //List<Item> itemList=context.Items.Where(i => i.CategoryID == categoryId).ToList();
            return null;
            
        }

        //Method to retrieve the cost of the item
        public float getItemCost(string itemId)
        {
            var supplierItem = context.SupplierItems.Where(si => si.ItemID == itemId).Where(si => si.Rank == 1).First();
            return (float)supplierItem.Cost;

        }

        RetrieveStockManagementService rsms = new RetrieveStockManagementService();
        public void addStockTransaction(string itemId, string description, string employeeId, int adjustment)
        {
            //creating a new stocktransaction record
            StockTransaction st = new StockTransaction();
            //setting the value of itemId,description,employeeId of the person who created and adjustment.
            //adjustment can be positive or negative value based on adding the item to the stock or removing item from the stock
            st.ItemID = itemId;
            if (description != null)
                st.Description = description;
            st.EmployeeID = employeeId;
            st.Adjustment = adjustment;
            context.StockTransactions.Add(st);
            context.SaveChanges();

        }

        public void addStockVoucher(string itemId, int actualcount, string employeeId, string reason)
        {
            StockVoucher sv = new StockVoucher();
            //Setting the values received by the method to the object 
            sv.ItemID = itemId;
            //Gettin goriginal count of the item according to the store records
            sv.OriginalCount = rsms.getStockCountOfItem(itemId);
            //entered by the clerk while taking stock
            sv.ActualCount = actualcount;
            //reason in case discrepancy
            sv.Reason = reason;
            sv.ItemCost = (decimal)rsms.getItemCost(itemId);
            sv.RaisedBy = employeeId;
            sv.RaisedByDate = DateTime.Today;
            context.StockVouchers.Add(sv);
            context.SaveChanges();
        }

        public void rejectStock(string itemId, string reason, int count, string employeeId)
        {
            //when the item gets added after  rejection at disbursement actual count will increase
            int actualcount = rsms.getStockCountOfItem(itemId) + count;
            addStockTransaction(itemId, reason, employeeId, count);
            addStockVoucher(itemId, actualcount, employeeId, reason);
            return;
        }

        public void submitStockCountItems(int empId)
        {

        }
        public void submitVouchers()
        {

        }

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
