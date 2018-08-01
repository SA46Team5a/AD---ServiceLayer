using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    // Author: Bhat Pavana
    public class StockManagementService : IStockManagementService
    {
         
        static StationeryStoreEntities context = StationeryStoreEntities.Instance;

        public Item getItemById(string itemId)
            => context.Items.First(i => i.ItemID == itemId);

        public  List<Item> getAllItems()
        {
            //List all the items in the store
            return context.Items.ToList();         
        }

        public List<StockCountItem> getAllStockCountItem()
        {
            //List all the items and stock count   in the store
            return context.StockCountItems.ToList();
        }

        public string getItemName(string itemId)
        {
            return context.Items.First(i => i.ItemID == itemId).ItemName;
        }

        public int getStockCountOfItem(string itemId)
        {
            StockCountItem item = context.StockCountItems.First(i => i.ItemID == itemId);
            context.Entry<StockCountItem>(item).Reload();
            return (int) item.QtyInStock;
        }

        public List<Item> getItemsOfCategory(int categoryId)
        {
            List<Item> itemList = context.Items.Where(i => i.CategoryID == categoryId).ToList();
            return itemList;
        }

        public List<StockVoucher> getOpenVouchers(bool isStoreManager)
        {
            List<StockVoucher> openVouchers = context.StockVouchers
                .Where(sv => sv.ApprovedBy == null).ToList();

            if (isStoreManager)
                return openVouchers.Where(sv => (sv.ActualCount - sv.OriginalCount) * sv.ItemCost > 250).ToList();
            else
                return openVouchers.Where(sv => (sv.ActualCount - sv.OriginalCount) * sv.ItemCost <= 250).ToList();
        }

        public Supplier getFirstSupplierOfItem(string itemId)
        {
            //Retrieving the Supplier of the given itemId having Rank 1
            return context.SupplierItems
                .First(si => si.ItemID == itemId && si.Rank == 1)
                .Supplier;
        }

        public List<Supplier> getSupplierOfItem(string itemId)
        {
            //retrieving list of SupplierItem records matching with the itemId
            List<SupplierItem> supplierItems = context.SupplierItems.Where(si => si.ItemID == itemId).ToList();

            //Creating a new empty list of suppliers
            List<Supplier> supplierList = new List<Supplier>();
            supplierItems.ForEach(si => supplierList.Add(si.Supplier));
            return supplierList;
        }

        public List<StockCountItem> getStockCountItemsByCategory(int categoryId)
        {
            List<string> itemIds = getItemsOfCategory(categoryId).Select(i => i.ItemID).ToList();
            return context.StockCountItems.Where(s => itemIds.Contains(s.ItemID)).ToList(); ;
        }

        //Method to retrieve the cost of the item
        public float getItemCost(string itemId)
        {
            var supplierItem = context.SupplierItems.First(si => si.ItemID == itemId && si.Rank == 1);
            return (float) supplierItem.Cost;
        }

        public void addStockTransaction(string itemId, string description, string employeeId, int adjustment)
        {
            //creating a new stocktransaction record
            StockTransaction st = new StockTransaction();
            //setting the value of itemId,description,employeeId of the person who created and adjustment.
            //adjustment can be positive or negative value based on adding the item to the stock or removing item from the stock
            st.ItemID = itemId;
            st.Description = description;
            st.EmployeeID = employeeId;
            st.Adjustment = adjustment;
            context.StockTransactions.Add(st);
            context.SaveChanges();            
            // test if st.Item is auto populated after saving changes
        }

        public void addStockVoucher(string itemId, int actualcount, string employeeId, string reason)
        {
            StockVoucher sv = new StockVoucher();
            //Setting the values received by the method to the object 
            sv.ItemID = itemId;
            //Gettin goriginal count of the item according to the store records
            sv.OriginalCount = getStockCountOfItem(itemId);
            //entered by the clerk while taking stock
            sv.ActualCount = actualcount;
            //reason in case discrepancy
            sv.Reason = reason;
            sv.ItemCost = (decimal)getItemCost(itemId);
            sv.RaisedBy = employeeId;
            sv.RaisedByDate = DateTime.Today;
            context.StockVouchers.Add(sv);
            context.SaveChanges();
        }

        public void rejectStock(string itemId, string reason, int count, string employeeId)
        {
            //when the item gets added after  rejection at disbursement actual count will increase
            int actualcount = getStockCountOfItem(itemId) + count;
            addStockTransaction(itemId, reason, employeeId, count);
            addStockVoucher(itemId, actualcount, employeeId, reason);
            return;
        }


        public void submitStockCountItems(List<StockVoucherPayload> vouchers, string empId)
        {
            foreach (StockVoucherPayload voucher in vouchers)
            {
                if (voucher.ActualCount != getStockCountOfItem(voucher.ItemID))
                    addStockVoucher(voucher.ItemID, voucher.ActualCount, empId, voucher.Reason == null ? "" : voucher.Reason);
            }
        }

        public void submitVouchers(List<StockVoucherPayload> stockVoucherPayloads)
        {
            foreach (StockVoucherPayload sv in stockVoucherPayloads)
                closeVoucher(sv.DiscrepancyID, sv.VoucherApproverID, sv.Reason);
        }

        public void closeVoucher(int discrepancyId, string approvedBy, string reason)
        {
            //Retrieve the record from the stock voucher with discrepancy id passed as parameter
            StockVoucher sv = context.StockVouchers.First(i => i.DiscrepancyID == discrepancyId);
            sv.ApprovedBy = approvedBy;
            sv.ApprovedDate = DateTime.Today;
            sv.Reason = reason;
            context.SaveChanges();
            return;
        }
    }
}
