using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;
namespace ServiceLayer
{
    // Author: Jack
    public interface IOrderService
    {
        // Retrieve
        Order getOrder(int orderId);
        List<Order> getOutstandingOrders();
        OrderSupplier getOrderSupplier(int orderSupplierId);
        OrderSupplierDetail getOrderSupplierDetail(int orderSupplierDetailId);
        List<ReorderDetail> getReorderDetails();
        List<Supplier> getSuppliers();
        List<SupplierItem> getSupplierItemsOfItemIds(List<String> itemIds);
        List<OrderSupplier> getOrderSuppliersOfOrder(int orderId);
        List<OrderSupplierDetail> getOrdersServingOutstandingRequisitions(int reqDetailId);
        List<OrderSupplierDetail> getOrderDetailsOfOrderIdAndSupplier(int orderId, string supplierId);
        List<Supplier> getSuppliersOfOrderIdWithOutstandingInvoice(int orderId);

        // Create
        int createOrderAndGetOrderId(Dictionary<string, int> itemAndQty, Dictionary<int, int> supplierItemsAndQty);
        OrderSupplier createOrderSupplier(int orderId, string supplierId);
        OrderSupplierDetail createOrderSupplierDetail(int orderSupplierId, SupplierItem si, int qty);
        void allocateQtyToSuppliers(Order order, Dictionary<int, int> supplierItemAndqty, int qty);

        // Update
        void updateQtyRecievedOfOrderSupplierDetail(int orderSupplierDetailId, int qty, string empId);
        void confirmDeliveryOfOrderSupplier(int orderSupplierId);
        void confirmInvoiceUploadStatus(int orderSupplierId);
    }
}
