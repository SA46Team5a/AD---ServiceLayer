using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    // Author: Jack
    public class OrderService
    {
        StationeryStoreEntities context = StationeryStoreEntities.Instance;

        // Retrieve
        public List<ReorderDetail> getReorderDetails()
            => context.ReorderDetails.ToList();

        public List<SupplierItem> getSupplierItemsOfItemIds(List<string> itemIds)
            => context.SupplierItems
            .Where(si => itemIds.Contains(si.ItemID))
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

            foreach (SupplierItem si in supplierItems)
            {
                if (qty > 0 && supplierItemAndQty[si.SupplierItemID] > 0)
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
                    osd.OrderSupplier = os;
                    osd.ItemID = si.ItemID;
                    osd.Quantity = orderQty;
                    osd.UnitCost = (decimal) si.Cost;
                    os.OrderSupplierDetails.Add(osd);
                    qty -= orderQty;
                }
                else
                    break;
            }
            context.SaveChanges();
        }
    }
}
