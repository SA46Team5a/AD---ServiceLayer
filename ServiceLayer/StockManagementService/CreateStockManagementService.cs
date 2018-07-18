using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    // Author: Bhat Pavana
    partial class CreateStockManagementService:ICreateStockManagementService
    {
        StationeryStoreEntities context = new StationeryStoreEntities();
        RetrieveStockManagementService rsms = new RetrieveStockManagementService();
        public void addStockTransaction(string itemId,string description,string employeeId,int adjustment)
        {
            //creating a new stocktransaction record
            StockTransaction st = new StockTransaction();
            //setting the value of itemId,description,employeeId of the person who created and adjustment.
            //adjustment can be positive or negative value based on adding the item to the stock or removing item from the stock
            st.ItemID =itemId;
            if(description!=null)
            st.Description =description;
            st.EmployeeID = employeeId;            
            st.Adjustment = adjustment;
            context.StockTransactions.Add(st);
            context.SaveChanges();

        }

        public void addStockVoucher(string itemId,int actualcount,string employeeId,string reason)
        {
            StockVoucher sv = new StockVoucher();
            //Setting the values received by the method to the object 
            sv.ItemID = itemId;
            //Gettin goriginal count of the item according to the store records
            sv.OriginalCount =rsms.getStockCountOfItem(itemId);
            //entered by the clerk while taking stock
            sv.ActualCount = actualcount;
            //reason in case discrepancy
            sv.Reason = reason;
            sv.ItemCost =(decimal) rsms.getItemCost(itemId);
            sv.RaisedBy = employeeId;
            sv.RaisedByDate = DateTime.Today;
            context.StockVouchers.Add(sv);
            context.SaveChanges();
        }

        public void rejectStock(string itemId, string reason,int count,string employeeId)
        {
            //when the item gets added after  rejection at disbursement actual count will increase
            int actualcount = rsms.getStockCountOfItem(itemId) + count;
            addStockTransaction(itemId, reason, employeeId,count);
            addStockVoucher(itemId,actualcount,employeeId, reason);                
            return;
        }

        public  void submitStockCountItems(int empId)
        {

        }
        public  void submitVouchers()
        {

        }
    }
}
