using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    // Author: Jack
    class OrderTest
    {
        static void Main(string[] args)
        {
            IOrderService OrderService = new OrderService();
            StationeryStoreEntities context = StationeryStoreEntities.Instance;

            // Test 1
            Console.WriteLine("Test createOrderAndGetOrderID() and allocateQtyToSupplier() and create methods");

            // Prepare Required Quantity Data
            List<string> itemIds = new List<string>() { "C001" };
            Dictionary<string, int> itemAndQty = new Dictionary<string, int>();
            itemIds.ForEach(i => itemAndQty.Add(i, 10));

            // Prepare Stock Available at Supplier
            List<int> supplierItemIds = new List<int>() { 1, 15, 29 };
            Dictionary<int, int> supplierItemsAndQty = new Dictionary<int, int>();

            Console.WriteLine("Scenario 1: First supplier has sufficient stock");
            supplierItemIds.ForEach(si => supplierItemsAndQty.Add(si, 15));
            int orderId = OrderService.createOrderAndGetOrderId(itemAndQty, supplierItemsAndQty);
            Order order = context.Orders.First(o => o.OrderID == orderId);
            Console.WriteLine("Number of order suppliers should be 1 : {0}", order.OrderSuppliers.Count());
            OrderSupplierDetail orderSupplierDetail = order.OrderSuppliers.First().OrderSupplierDetails.First();
            Console.WriteLine("Qty ordered should be 10 : {0}", orderSupplierDetail.Quantity);
            Console.WriteLine();

            Console.WriteLine("Scenario 2: First supplier and second supplier combined have enough stock");
            supplierItemsAndQty = new Dictionary<int, int>();
            supplierItemIds.ForEach(si => supplierItemsAndQty.Add(si, 6));
            orderId = OrderService.createOrderAndGetOrderId(itemAndQty, supplierItemsAndQty);
            int test2OrderId = orderId;
            order = context.Orders.First(o => o.OrderID == orderId);
            Console.WriteLine("Number of order suppliers should be 2 : {0}", order.OrderSuppliers.Count());
            List<int> orderedQuantities = context.OrderSupplierDetails.Where(osd => osd.OrderSupplier.OrderID == orderId).OrderBy(osd => osd.OrderSupplierDetailsID).Select(osd => osd.Quantity).ToList();
            Console.WriteLine("Qty ordered should be [6, 4] : [{0}]", String.Join(", ", orderedQuantities));
            Console.WriteLine();
                
            Console.WriteLine("Scenario 3: First, second and third supplier combined have enough stock");
            supplierItemsAndQty = new Dictionary<int, int>();
            supplierItemIds.ForEach(si => supplierItemsAndQty.Add(si, 4));
            orderId = OrderService.createOrderAndGetOrderId(itemAndQty, supplierItemsAndQty);
            order = context.Orders.First(o => o.OrderID == orderId);
            Console.WriteLine("Number of order suppliers should be 3 : {0}", order.OrderSuppliers.Count());
            orderedQuantities = context.OrderSupplierDetails.Where(osd => osd.OrderSupplier.OrderID == orderId).OrderBy(osd => osd.OrderSupplierDetailsID).Select(osd => osd.Quantity).ToList();
            Console.WriteLine("Qty ordered should be [4, 4, 2] : [{0}]", String.Join(", ", orderedQuantities));
            Console.WriteLine();

            Console.WriteLine("Scenario 4: First, second and third supplier combined do not have enough stock");
            supplierItemsAndQty = new Dictionary<int, int>();
            supplierItemIds.ForEach(si => supplierItemsAndQty.Add(si, 3));
            orderId = OrderService.createOrderAndGetOrderId(itemAndQty, supplierItemsAndQty);
            order = context.Orders.First(o => o.OrderID == orderId);
            Console.WriteLine("Number of order suppliers should be 3 : {0}", order.OrderSuppliers.Count());
            orderedQuantities = context.OrderSupplierDetails.Where(osd => osd.OrderSupplier.OrderID == orderId).OrderBy(osd => osd.OrderSupplierDetailsID).Select(osd => osd.Quantity).ToList();
            Console.WriteLine("Qty ordered should be [3, 3, 3] : [{0}]", String.Join(", ", orderedQuantities));
            Console.WriteLine();
            
            Console.WriteLine("Scenario 5: First no stock, second and third supplier combined do not have enough stock");
            supplierItemsAndQty = new Dictionary<int, int>();
            int count = 0;
            supplierItemIds.ForEach(si => supplierItemsAndQty.Add(si, count++));
            orderId = OrderService.createOrderAndGetOrderId(itemAndQty, supplierItemsAndQty);
            order = context.Orders.First(o => o.OrderID == orderId);
            Console.WriteLine("Number of order suppliers should be 2 : {0}", order.OrderSuppliers.Count());
            orderedQuantities = context.OrderSupplierDetails.Where(osd => osd.OrderSupplier.OrderID == orderId).OrderBy(osd => osd.OrderSupplierDetailsID).Select(osd => osd.Quantity).ToList();
            Console.WriteLine("Qty ordered should be [1, 2] : [{0}]", String.Join(", ", orderedQuantities));
            Console.WriteLine();

            Console.WriteLine("Scenario 6: Provided qty <= 0");
            itemAndQty = new Dictionary<string, int>();
            itemIds.ForEach(i => itemAndQty.Add(i, 0));
            supplierItemsAndQty = new Dictionary<int, int>();
            supplierItemIds.ForEach(si => supplierItemsAndQty.Add(si, count++));
            orderId = OrderService.createOrderAndGetOrderId(itemAndQty, supplierItemsAndQty);
            order = context.Orders.First(o => o.OrderID == orderId);
            Console.WriteLine("Number of order suppliers should be 0 : {0}", order.OrderSuppliers.Count());
            orderedQuantities = context.OrderSupplierDetails.Where(osd => osd.OrderSupplier.OrderID == orderId).OrderBy(osd => osd.OrderSupplierDetailsID).Select(osd => osd.Quantity).ToList();
            Console.WriteLine("Qty ordered should be : [{0}]", String.Join(", ", orderedQuantities));
            Console.WriteLine();

            // Test 2
            Console.WriteLine("Test Retrieve methods");

            Console.WriteLine("Test getOrder");
            order = OrderService.getOrder(test2OrderId);
            Console.WriteLine("OrderId should be {0} : {1}", test2OrderId, order.OrderID);

            Console.WriteLine("Test getOrderSupplier");
            int orderSupplierId = order.OrderSuppliers.First().OrderSupplierID;
            OrderSupplier orderSupplier = OrderService.getOrderSupplier(orderSupplierId);
            Console.WriteLine("OrderSupplierId should be {0} : {1}", orderSupplierId, orderSupplier.OrderSupplierID);

            Console.WriteLine("Test getOrderSupplierDetails");
            int orderSupplierDetailId = orderSupplier.OrderSupplierDetails.First().OrderSupplierDetailsID;
            orderSupplierDetail = OrderService.getOrderSupplierDetail(orderSupplierDetailId);
            Console.WriteLine("OrderSupplierDetailId should be {0} : {1}", orderSupplierDetailId, orderSupplierDetail.OrderSupplierDetailsID);

            Console.WriteLine("Test getReorderDetails");
            foreach (ReorderDetail rd in OrderService.getReorderDetails())
            {
                Console.WriteLine("{0} \t {1} \t {2}", rd.ItemID, rd.ItemName, rd.ReorderLevel);
            }
            Console.WriteLine();

            Console.WriteLine("test getSupplierItemsOfItemId");
            foreach (SupplierItem si in OrderService.getSupplierItemsOfItemIds(new List<string>() { "C001", "C002"}))
            {
                Console.WriteLine("{0} \t {1} \t {2} \t {3}", si.SupplierItemID, si.Supplier.SupplierName, si.ItemID, si.Cost);
            }
            Console.WriteLine();

            // Test 3
            Console.WriteLine("Test update methods");
            Console.WriteLine("Test updateQtyReeivedOfOrderSupplierDetail");
            Console.WriteLine("Scenario 1: When received qty > ordered");
            OrderService.updateQtyRecievedOfOrderSupplierDetail(orderSupplierDetailId, 8, "E001");
            orderSupplierDetail = OrderService.getOrderSupplierDetail(orderSupplierDetail.OrderSupplierDetailsID);
            Console.WriteLine("OrderSupplierDetail {0}'s qty recieved should be 6 : {1}", orderSupplierDetailId, orderSupplierDetail.ActualQuantityReceived);
            Console.WriteLine();

            Console.WriteLine("Scenario 2: When received qty < ordered");
            OrderService.updateQtyRecievedOfOrderSupplierDetail(orderSupplierDetailId, 4, "E001");
            Console.WriteLine("OrderSupplierDetail {0}'s qty recieved should be 4 : {1}", orderSupplierDetailId, orderSupplierDetail.ActualQuantityReceived);
            Console.WriteLine();

            Console.WriteLine("Test confirmDeliveryOfOrderSupplier");
            Console.WriteLine("OrderSupplierDetail {0}'s delivery status recieved should be 2 : {1}", orderSupplierId, orderSupplier.DeliveryStatusID);
            OrderService.confirmDeliveryOfOrderSupplier(orderSupplierId);
            Console.WriteLine("OrderSupplierDetail {0}'s delivery status recieved should be 1 : {1}", orderSupplierId, orderSupplier.DeliveryStatusID);
            Console.WriteLine();

            Console.WriteLine("Test confirmInvoiceUploadStatus");
            Console.WriteLine("OrderSupplierDetail {0}'s invoice upload status recieved should be 2 : {1}", orderSupplierId, orderSupplier.InvoiceUploadStatusID);
            OrderService.confirmInvoiceUploadStatus(orderSupplierId);
            Console.WriteLine("OrderSupplierDetail {0}'s invoice upload status recieved should be 1 : {1}", orderSupplierId, orderSupplier.InvoiceUploadStatusID);
            Console.WriteLine(); 
        }
    }
}
