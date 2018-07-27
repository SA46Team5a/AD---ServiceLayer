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
}
