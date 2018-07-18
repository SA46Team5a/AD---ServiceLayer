using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;
namespace ServiceLayer
{
    interface IOrderService
    {
        // Retrieve
        // RequisitionTracking
        // Get first order where the item is ordered from at least one supplier
        Order getFirstOrderAfterDateWithItem(
            DateTime date, 
            int itemId
            );
        
        //
        List<ReorderDetail> getReorderDetails();

        // Create
        // Create new order based on 
        // 1. required quantity
        // 2. stock avaiable from supplier
        // createNewOrder will take both inputs, 
        // allocate the qty across suppliers, and 
        // insert data using addOrder, addOrderSupplier
        // and addOrderSupplierDetail
        void createNewOrder(
            Dictionary<Supplier, Dictionary<Item, int>> availableStockFromSupplier,
            Dictionary<Item, int> reorderQtys
            );
        void addOrder(Order o);
        void addOrderSupplier(OrderSupplier os);
        void addOrderSupplierDetails(OrderSupplierDetail osd);

        // Update
        void updateOrderStatus(int orderSupplierId, DeliveryStatus status);
        void updateInvoiceUploadstatus(int orderSupplierId, InvoiceUploadStatus status);
        void updateOrderSupplierDetailsQtyReceived(int orderSupplierDetailId, int qty);
    }
}
