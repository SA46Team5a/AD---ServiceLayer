using ServiceLayer.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class ItemPayload
    {
        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public string UnitOfMeasure { get; set; }
        public int QtyInStock { get; set; }
    }

    public class StockCountPayload : ItemPayload
    {
        public int ActualStock { get; set; }
    }

    public class StockVoucherPayload
    {
        public string ItemID { get; set; }
        public int ActualCount { get; set; }
        public string EmployeeID { get; set; }
        public string Reason { get; set; }
    }

    public class OrderDetailsPayload : ItemPayload
    {
        public string CategoryName { get; set; }
        public int OrderSupplierDetailId { get; set; }
        private readonly IStockManagementService stockManagementService;

        public OrderDetailsPayload(OrderSupplierDetail orderSupplierDetail, IStockManagementService stockManagementService)
        {
            this.stockManagementService = stockManagementService;
            ItemID = orderSupplierDetail.ItemID;
            CategoryName = orderSupplierDetail.Item.Category.CategoryName;
            ItemName = orderSupplierDetail.Item.ItemName;
            UnitOfMeasure = orderSupplierDetail.Item.UnitOfMeasure;
            QtyInStock = stockManagementService.getStockCountOfItem(ItemID);
            OrderSupplierDetailId = orderSupplierDetail.OrderSupplierDetailsID;
        }

        public static List<OrderDetailsPayload> ConvertEntityToPayload(List<OrderSupplierDetail> o, IStockManagementService stockManagementService)
        {
            List<OrderDetailsPayload> payload = new List<OrderDetailsPayload>();
            o.ForEach(osd => payload.Add(new OrderDetailsPayload(osd, stockManagementService)));
            return payload;    
        }
    }
}
