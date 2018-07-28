using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;
namespace ServiceLayer
{
    class StockManagementTest
    {
        public static void main(string[] args) { 
            Console.WriteLine("Testing ");
            IStockManagementService iService = new StockManagementService();


            List<Item> itemList = iService.getAllItems();
            foreach(Item i in itemList)
            {
                Console.Write(i.ItemName + "\t");
                Console.WriteLine(iService.getStockCountOfItem(i.ItemID));
            }

            itemList = iService.getItemsOfCategory(1);
            foreach (Item i in itemList)
            {
                Console.WriteLine(i.ItemName);
            }

            List<StockVoucher> openVoucherList = iService.getOpenVouchers(true);
            foreach(StockVoucher sv in openVoucherList)
            {
                Console.WriteLine(sv.ItemID);
            }

            Console.WriteLine("Supplier name:"+iService.getFirstSupplierOfItem("C001").SupplierName);

            List<Supplier> supplierList = iService.getSupplierOfItem("C001");
            Console.Write("Suppliers are:");
            foreach (Supplier s in supplierList)
            {
                Console.Write(s.SupplierName);
            }

            List<StockCountItem> sciList = iService.getStockCountItemsByCategory(1);
            Console.WriteLine("Stock count item list");
            foreach(StockCountItem sc in sciList)
            {
                Console.WriteLine("Item name ,Quantity in  stock" + sc.ItemName + "   " + sc.QtyInStock);
            }

            Console.Write("Cost of item C001 is:" + iService.getItemCost("C001"));

            Console.WriteLine("Adding stock transaction testing");
            iService.addStockTransaction("C001", "new items received from the supplier", "E012", 200);

            Console.WriteLine("adding stock vouchers");
            iService.addStockVoucher("C001", 720, "E001", "broken");

            Console.WriteLine("Rejecting the stock");
            iService.rejectStock("C001", "opened items", 10, "E001");

            Console.WriteLine("Closing the vouchers:");      
            iService.closeVoucher(11, "E014","broken");

        }
    }
}
