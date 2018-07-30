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

        ReportResponsePayload oneMonthReport(List<OrderSupplierDetail> orderSupplierDetails,int month);
        ReportResponsePayload generateRequisitionCostReport(RequisitionRequestPayload payload);

        ReportResponsePayload pastThreeMonthsReport(List<RequisitionDetail> requisitionDetails);

        ReportResponsePayload oneMonthReport(List<RequisitionDetail> requisitionDetails, int month);

        ReportResponsePayload generateRequisitionItemReport(RequisitionItemPayload payload);
    }
}
