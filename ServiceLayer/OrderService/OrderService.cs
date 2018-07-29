using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    // Author: Jack
    public class OrderService : IOrderService
    {
        StationeryStoreEntities context = StationeryStoreEntities.Instance;
        IStockManagementService stockManagementService = new StockManagementService();

        // Retrieve
        public Order getOrder(int orderId)
            => context.Orders.First(o => o.OrderID == orderId);

        public List<Order> getOutstandingOrders()
            => context.OrderSuppliers.Where(os => os.DeliveryStatusID == 2).Select(os => os.Order).Distinct().ToList();

        public OrderSupplier getOrderSupplier(int orderSupplierId)
            => context.OrderSuppliers.First(os => os.OrderSupplierID == orderSupplierId);

        public OrderSupplierDetail getOrderSupplierDetail(int orderSupplierDetailId)
            =>  context.OrderSupplierDetails.First(osd => osd.OrderSupplierDetailsID== orderSupplierDetailId);

        public List<ReorderDetail> getReorderDetails()
            => context.ReorderDetails.ToList();

        public List<Supplier> getSuppliers()
            => context.Suppliers.ToList();

        public List<SupplierItem> getSupplierItemsOfItemIds(List<string> itemIds)
            => context.SupplierItems
            .Where(si => itemIds.Contains(si.ItemID))
            .ToList();

        public List<OrderSupplier> getOrderSuppliersOfOrder(int orderId)
            => context.OrderSuppliers.Where(o => o.OrderID == orderId).ToList();

        public List<Supplier> getSuppliersOfOrderIdWithOutstandingInvoice(int orderId)
            => context.OrderSuppliers
            .Where(o => o.OrderID == orderId && o.InvoiceUploadStatusID == 2)
            .Select(o => o.Supplier)
            .Distinct()
            .ToList();

        public List<OrderSupplierDetail> getOrdersServingOutstandingRequisitions(int reqDetailId)
        {
            // reqDetailId should come from requsition details that are outstanding
            RequisitionDetail requisitionDetail = context.RequisitionDetails.First(r => r.RequisitionDetailsID == reqDetailId);

            // get the date of latest disbursement associated with outstanding requisition
            DateTime date = context.DisbursementDetails
                .OrderBy(d => d.Disbursement.DisbursementDuty.DisbursementDate)
                .ToList()
                .Last()
                .Disbursement
                .DisbursementDuty
                .DisbursementDate;

            // get the orderSupplierDetails that contains the outstanding item
            List<OrderSupplierDetail> orderSupplierDetails = context.OrderSupplierDetails
                .Where(o => o.ItemID == requisitionDetail.ItemID && o.OrderSupplier.Order.OrderDate> date)
                .ToList();

            if (orderSupplierDetails.Count > 0)
            {
                // get the order associated with the first orderSupplierDetail after the disbursement date
                Order order = orderSupplierDetails
                    .OrderBy(o => o.OrderSupplier.Order.OrderDate)
                    .First()
                    .OrderSupplier.Order;

                // get all order supplier details in the same order that contains the outstanding item
                List<OrderSupplierDetail> ordersForOutstandingItem = context.OrderSupplierDetails
                    .Where(o => o.OrderSupplier.OrderID == order.OrderID && o.ItemID == requisitionDetail.ItemID)
                    .ToList();

                return ordersForOutstandingItem;
            }
            else
            {
                return  new List<OrderSupplierDetail>(1) { new OrderSupplierDetail()};
            }
           
        }

        public List<OrderSupplierDetail> getOrderDetailsOfOrderIdAndSupplier(int orderId, string supplierId)
            => context.OrderSupplierDetails
            .Where(o => o.OrderSupplier.OrderID == orderId && o.OrderSupplier.SupplierID == supplierId)
            .ToList();

        // Create
        public int createOrderAndGetOrderId(Dictionary<string, int> itemAndQty, Dictionary<int, int> supplierItemsAndQty)
        {
            Order order = new Order() { OrderDate = DateTime.Now };
            context.Orders.Add(order);
            context.SaveChanges();
            foreach (KeyValuePair<string, int> item in itemAndQty)
            {
                List<int> supplierItems = context.SupplierItems
                    .Where(si => si.ItemID == item.Key)
                    .Select(si => si.SupplierItemID)
                    .ToList();
                Dictionary<int, int> filtered = new Dictionary<int, int>();
                foreach (int id in supplierItems)
                {
                    filtered.Add(id, supplierItemsAndQty[id]);
                }
                allocateQtyToSuppliers(order, filtered, item.Value);
            }            

            return order.OrderID;
        }

        public OrderSupplier createOrderSupplier(int orderId, string supplierId)
        {
            OrderSupplier os = new OrderSupplier();
            os.SupplierID = supplierId;
            os.OrderID = orderId;
            os.DeliveryStatusID = 2;
            os.InvoiceUploadStatusID = 2;
            return os;
        }

        public OrderSupplierDetail createOrderSupplierDetail(
            int orderSupplierId, SupplierItem si, int qty)
        {
            OrderSupplierDetail osd = new OrderSupplierDetail();
            osd.OrderSupplierID = orderSupplierId;
            osd.ItemID = si.ItemID;
            osd.Quantity = qty;
            osd.UnitCost = (decimal) si.Cost;
            return osd;
        }

        public void allocateQtyToSuppliers(Order order, Dictionary<int, int> supplierItemAndQty, int qty)
        {
            List<OrderSupplierDetail> details = new List<OrderSupplierDetail>();
            List<SupplierItem> supplierItems = context.SupplierItems
                .Where(si => supplierItemAndQty.Keys.Contains(si.SupplierItemID))
                .OrderBy(si => si.Rank)
                .ToList();

            int totalAvailableQuantity = supplierItemAndQty.Values.Sum();
            foreach (SupplierItem si in supplierItems)
            {
                if (qty > 0 && totalAvailableQuantity > 0)
                {
                    if (supplierItemAndQty[si.SupplierItemID] > 0)
                    {
                        int availableQty = supplierItemAndQty[si.SupplierItemID];
                        OrderSupplier os;
                        if (order.OrderSuppliers.Count(o => o.Supplier == si.Supplier) == 0)
                        {
                            os = createOrderSupplier(order.OrderID, si.Supplier.SupplierID);
                            order.OrderSuppliers.Add(os);
                            context.SaveChanges();
                        }
                        else
                            os = order.OrderSuppliers.First(o => o.Supplier == si.Supplier);

                        OrderSupplierDetail osd = new OrderSupplierDetail();
                        int orderQty = Math.Min(qty, availableQty);
                        osd = createOrderSupplierDetail(os.OrderSupplierID, si, orderQty);
                        os.OrderSupplierDetails.Add(osd);
                        qty -= orderQty;
                    }
                }
                else
                    break;
            }
            context.SaveChanges();
        }

        // Update
        public void updateQtyRecievedOfOrderSupplierDetail(int orderSupplierDetailId, int qty, string empId)
        {
            OrderSupplierDetail orderSupplierDetail = getOrderSupplierDetail(orderSupplierDetailId);
            orderSupplierDetail.ActualQuantityReceived = Math.Min(qty, orderSupplierDetail.Quantity); // excess rejected
            stockManagementService.addStockTransaction(orderSupplierDetail.ItemID, "restock", empId, qty);

            if (context.OrderSupplierDetails
                .Count(o => o.OrderSupplierID == orderSupplierDetail.OrderSupplierID && o.ActualQuantityReceived == null) == 0)
                confirmDeliveryOfOrderSupplier(orderSupplierDetail.OrderSupplierID);

            context.SaveChanges();
        }

        public void confirmDeliveryOfOrderSupplier(int orderSupplierId)
        {
            OrderSupplier orderSupplier = getOrderSupplier(orderSupplierId);
            orderSupplier.DeliveryStatusID = 1; // Delivered
            context.SaveChanges();
        }

        public void confirmInvoiceUploadStatus(int orderSupplierId)
        {
            OrderSupplier orderSupplier = getOrderSupplier(orderSupplierId);
            orderSupplier.InvoiceUploadStatusID = 1; // Delivered
            context.SaveChanges();
        }
    }
}
