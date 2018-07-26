using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;
namespace ServiceLayer
{
    public interface IReportService
    {
        ReportResponsePayload generateReorderCostReport(ReorderRequestPayload payload);
        ReportResponsePayload pastThreeMonthsReport(List<OrderSupplierDetail> orderSupplierDetails);
    }
}
