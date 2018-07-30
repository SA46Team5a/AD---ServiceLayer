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

    public class StockVoucherPayload : ItemPayload
    {
        public int DiscrepancyID { get; set; }
        public int ActualCount { get; set; }
        public decimal UnitCost { get; set; }
        public string VoucherRaiserID { get; set; }
        public string VoucherApproverID { get; set; }
        public string Reason { get; set; }

        public StockVoucherPayload(StockVoucher sv)
        {
            DiscrepancyID = sv.DiscrepancyID;
            ItemID = sv.ItemID;
            ItemName = sv.Item.ItemName;
            UnitOfMeasure = sv.Item.UnitOfMeasure;
            QtyInStock = sv.OriginalCount;
            UnitCost = sv.ItemCost;
            VoucherRaiserID = sv.VoucherRaiser.EmployeeID;
            Reason = sv.Reason;
        }

        public static List<StockVoucherPayload> ConvertEntityToPayload(List<StockVoucher> stockVouchers)
        {
            List<StockVoucherPayload> payload = new List<StockVoucherPayload>();
            if (stockVouchers != null && stockVouchers.Count > 0)
                stockVouchers.ForEach(sv => payload.Add(new StockVoucherPayload(sv)));
            return payload;
        }
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
