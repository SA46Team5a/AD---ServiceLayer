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
        static private StationeryStoreEntities context = new StationeryStoreEntities();
            
        public  List<Item> getAllItems()
        {
            List<Item> itemList = context.Items.ToList();         
            return itemList;
        }
        public List<Category> getAllCategories()
        {
            List<Category> categoryList= context.Categories.ToList();
            return categoryList;

        }

        public int getStockCountOfItem(string itemId)
        {           
            
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

    }
}
