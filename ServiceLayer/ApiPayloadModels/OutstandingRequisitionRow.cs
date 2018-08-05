using ServiceLayer.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class OutstandingRequisitionRow
    {
        public OutstandingRequisitionView OutstandingRequisitionView { get; set; }
        public RequisitionDetail RequisitionDetail { get; set; }
        public OrderSupplierDetail OrderSupplierDetail { get; set; }
    }
}
