using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    public class SupplierPayload
    {
        public string SupplierID { get; set; }
        public string SupplierName { get; set; }

        public SupplierPayload(){ }
        public SupplierPayload(Supplier s)
        {
            SupplierID = s.SupplierID;
            SupplierName = s.SupplierName;
        }

        public static List<SupplierPayload> ConvertEntityToPayload(List<Supplier> suppliers)
        {
            List<SupplierPayload> payload = new List<SupplierPayload>();
            suppliers.ForEach(s => payload.Add(new SupplierPayload(s)));
            return payload;
        }
    }

    public class OrderPayload
    {
        public int OrderID { get; set; }

        public OrderPayload(Order o)
        {
            OrderID = o.OrderID;
        }

        public static List<OrderPayload> ConvertEntityToPayload(List<Order> orders)
        {
            List<OrderPayload> payload = new List<OrderPayload>();
            orders.ForEach(o => payload.Add(new OrderPayload(o)));
            return payload;
        }
    }
}
