using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    // Author: Jack
    public class ReportService : IReportService
    {
        List<string> colors = new List<string> {"Blue","BlueViolet","Brown","BurlyWood","CadetBlue","Chartreuse","Chocolate","Coral","CornflowerBlue","DarkGoldenRod","Crimson","Cyan","DarkBlue","DarkCyan" };
        static StationeryStoreEntities context = StationeryStoreEntities.Instance;

        public ReportResponsePayload generateReorderCostReport(ReorderRequestPayload payload)
        {
            List<OrderSupplierDetail> orderSupplierDetails = context.OrderSupplierDetails
                .Where(s => payload.department.Contains(s.OrderSupplier.SupplierID)
                && s.Item.CategoryID == payload.category)
                .ToList();

            switch (payload.duration)
            {
                case ("PastThreeMonths"):
                    return pastThreeMonthsReport(orderSupplierDetails);
                case ("OneMonth"):
                    return OneMonthReport(orderSupplierDetails, payload.option);
                case ("CompareMonths"):
                    break;
            }
            return new ReportResponsePayload();
        }

        public ReportResponsePayload pastThreeMonthsReport(List<OrderSupplierDetail> orderSupplierDetails)
        {
            ReportResponsePayload reportResponsePayload = new ReportResponsePayload();

            // get distinct list of suppliers from orderSupplierDetails
            List<Supplier> suppliers = orderSupplierDetails
                .Select(s => s.OrderSupplier.Supplier)
                .Distinct()
                .OrderBy(s => s.SupplierName)
                .ToList();

            // Prepare the list of months to generate data for
            reportResponsePayload.labels = new List<string>();
            for (int i = 3; i >= 0; i--)
            {
                reportResponsePayload.labels.Add(DateTime.Today.AddMonths(-i).ToString("MMMM"));
            }

            // Prepare the dataset to be displayed on web
            reportResponsePayload.datasets = new List<ReportDataPayload>();

            ReportDataPayload reportDataPayload;
            foreach (Supplier s in suppliers)
            {
                reportDataPayload = new ReportDataPayload();
                reportDataPayload.label = s.SupplierName;
                reportDataPayload.backgroundColor = colors[suppliers.IndexOf(s)];

                List<decimal> chartValues = new List<decimal>();

                for (int i = 3; i >= 0; i--)
                {
                    int month = DateTime.Today.AddMonths(-i).Month;

                    decimal? sum = orderSupplierDetails
                        .Where(si => si.OrderSupplier.SupplierID == s.SupplierID
                        && si.OrderSupplier.Order.OrderDate.Month == month)
                        .Sum(si => si.UnitCost * si.ActualQuantityReceived);

                    context.RequisitionDetails
                        .Where(rd => rd.Item.CategoryID == categoryID && rd.Requisition.RequestedDate.Value.Month == month && rd.Requisition.Requester.DepartmentID == deptID)
                        .ToList()
                        .Sum(rd => rd.)

                    chartValues.Add(sum == null ? 0 : (decimal) sum);
                }
                reportDataPayload.data = chartValues;
                reportResponsePayload.datasets.Add(reportDataPayload);
            }

            return reportResponsePayload;
        }

        public ReportResponsePayload OneMonthReport(List<OrderSupplierDetail> orderSupplierDetails, List<int> months)
        {
            ReportResponsePayload reportResponsePayload = new ReportResponsePayload();
            int month = months[0] + 1; // month integer in months is indexed from 0

            // Get list of suppliers from orderSupplierDetails
            List<Supplier> suppliers = orderSupplierDetails
                .Select(s => s.OrderSupplier.Supplier)
                .Distinct()
                .OrderBy(s => s.SupplierName)
                .ToList();

            // Get years required for data generation. If month has not past for current year, start 
            // from 4 years ago, if month has past for current year, start for month of this year
            reportResponsePayload.labels = new List<string>();

            int excludeCurrentYear = month > DateTime.Today.Month ? 1 : 0;
            for (int i = 3; i >= 0; i--)
            {
                reportResponsePayload.labels.Add(DateTime.Today.AddYears(-i - excludeCurrentYear).Year.ToString());
            }
            reportResponsePayload.datasets = new List<ReportDataPayload>();

            ReportDataPayload reportDataPayload;
            foreach (Supplier s in suppliers)
            {
                reportDataPayload = new ReportDataPayload();
                reportDataPayload.label = s.SupplierName;
                reportDataPayload.backgroundColor = colors[suppliers.IndexOf(s)];

                List<decimal> chartValues = new List<decimal>();

                for (int i = 3; i >= 0; i--)
                {
                    int year = DateTime.Today.AddYears(-i - excludeCurrentYear).Year;

                    decimal? sum = orderSupplierDetails
                        .Where(si => si.OrderSupplier.SupplierID == s.SupplierID
                        && si.OrderSupplier.Order.OrderDate.Year == year
                        && si.OrderSupplier.Order.OrderDate.Month == month)
                        .Sum(si => si.UnitCost * si.ActualQuantityReceived);

                    chartValues.Add(sum == null ? 0 : (decimal)sum);
                }
                reportDataPayload.data = chartValues;
                reportResponsePayload.datasets.Add(reportDataPayload);
            }

            return reportResponsePayload;
        }
    }
}
