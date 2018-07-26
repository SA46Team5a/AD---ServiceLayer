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
        List<string> colors = new List<string> { "Blue", "BlueViolet", "Brown", "BurlyWood", "CadetBlue", "Chartreuse", "Chocolate", "Coral", "CornflowerBlue", "DarkGoldenRod", "Crimson", "Cyan", "DarkBlue", "DarkCyan" };
        static StationeryStoreEntities context = StationeryStoreEntities.Instance;
        IDepartmentService iDepartmentService;
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

                    chartValues.Add(sum == null ? 0 : (decimal)sum);
                }
                reportDataPayload.data = chartValues;
                reportResponsePayload.datasets.Add(reportDataPayload);
            }

            return reportResponsePayload;
        }

        public ReportResponsePayload generateRequisitionCostReport(RequisitionRequestPayload payload)
        {
            List<Department> depList = context.Departments.Where(d => payload.department.Contains(d.DepartmentName)).ToList();
            List<string> depIds = depList.Select(d => d.DepartmentID).ToList();
            List<Employee> empList = context.Employees.Where(e => depIds.Contains(e.EmployeeID)).ToList();
            List<string> empIds = empList.Select(e => e.EmployeeID).ToList();

            List<RequisitionDetail> requisitionDetails = context.RequisitionDetails
               .Where(s => empIds.Contains(s.Requisition.EmployeeID) && s.Item.CategoryID == payload.category)
               .ToList();
            
            switch (payload.duration)
            {
                case ("PastThreeMonths"):
                    return pastThreeMonthsReport(requisitionDetails);
                case ("OneMonth"):
                    break;
                case ("CompareMonths"):
                    break;
            }
            return new ReportResponsePayload();
        }
        public ReportResponsePayload pastThreeMonthsReport(List<RequisitionDetail> requisitionDetails)
        {
            ReportResponsePayload reportResponsePayload = new ReportResponsePayload();
            List<string> empIds = requisitionDetails.Select(r => r.Requisition.EmployeeID).ToList();
            List<string> depList=new List<string>();
            foreach(string empId in empIds)
            {
               depList.Add(iDepartmentService.getDepartmentID(empId));
            }
            depList.Distinct();
            List<Department> departments=context.Departments.Where(d => depList.Contains(d.DepartmentID)).OrderBy(d => d.DepartmentName).ToList();

            //List<Supplier> suppliers = orderSupplierDetails
            //   .Select(s => s.OrderSupplier.Supplier)
            //   .Distinct()
            //   .OrderBy(s => s.SupplierName)
            //   .ToList();

            reportResponsePayload.labels = new List<string>();
            for (int i = 3; i > 0; i--)
            {

                reportResponsePayload.labels.Add(DateTime.Today.AddMonths(-i).ToString("MMMM"));
            }
            reportResponsePayload.datasets = new List<ReportDataPayload>();

            ReportDataPayload reportDataPayload;
            foreach (Department d in departments)
            {
                reportDataPayload = new ReportDataPayload();
                reportDataPayload.label = d.DepartmentName;
                reportDataPayload.backgroundColor = colors[departments.IndexOf(d)];

                List<decimal> chartValues = new List<decimal>();
                List<RequisitionDetail> rd = new List<RequisitionDetail>();
                List<SupplierItem> si = new List<SupplierItem>();

                for (int i = 3; i > 0; i--)
                {
                    int month = DateTime.Today.AddMonths(-i).Month;
                    rd=requisitionDetails.Where(ri => iDepartmentService.getDepartmentID(ri.Requisition.EmployeeID) == d.DepartmentID && ri.Requisition.RequestedDate.Value.Month == month).ToList();
                    int[] quantity = new int[rd.Count];
                    decimal[] cost = new decimal[rd.Count];
                    int j = 0,k=0;
                    string[] item = new string[rd.Count];
                    foreach(RequisitionDetail r in rd)
                    {
                        item[k++] = r.ItemID;
                        quantity[j] = r.Quantity;
                        j++;
                    }
                 
                    //    && 
                    //decimal? sum = orderSupplierDetails
                    //    .Where(si => si.OrderSupplier.SupplierID == s.SupplierID
                    //    && si.OrderSupplier.Order.OrderDate.Month == month)
                    //    .Sum(si => si.UnitCost * si.ActualQuantityReceived);


                   // chartValues.Add(sum == null ? 0 : (decimal)sum);
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
