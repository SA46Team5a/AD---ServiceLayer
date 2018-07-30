using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;


namespace ServiceLayer
{
    // Author: Jack,Pavana
    public class ReportService : IReportService
    {
        List<string> colors = new List<string> { "Blue", "BlueViolet", "Brown", "BurlyWood", "CadetBlue", "Chartreuse", "Chocolate", "Coral", "CornflowerBlue", "DarkGoldenRod", "Crimson", "Cyan", "DarkBlue", "DarkCyan" };
        static StationeryStoreEntities context = StationeryStoreEntities.Instance;

        public ReportResponsePayload generateReorderCostReport(ReorderRequestPayload payload)
        {
            List<OrderSupplierDetail> orderSupplierDetails;
            if (payload.department != null)
            {
                 orderSupplierDetails = context.OrderSupplierDetails
                    .Where(s => payload.department.Contains(s.OrderSupplier.SupplierID)
                    && s.Item.CategoryID == payload.category)
                    .ToList();
            }
            else
            {
                orderSupplierDetails = context.OrderSupplierDetails.Where(s => s.Item.CategoryID == payload.category).ToList();
            }
            switch (payload.duration)
            {
                case ("PastThreeMonths"):
                    return pastThreeMonthsReport(orderSupplierDetails);
                case ("OneMonth"):                    
                    return oneMonthReport(orderSupplierDetails, payload.option[0]);
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
            List<RequisitionDetail> requisitionDetails = new List<RequisitionDetail>();
            if (payload.department != null)
            {
                requisitionDetails = context.RequisitionDetails
                  .Where(rd => payload.department.Contains(rd.Requisition.Requester.DepartmentID) && rd.Item.CategoryID == payload.category)
                  .ToList();
            }
            else {
                requisitionDetails = context.RequisitionDetails.Where(rd => rd.Item.CategoryID == payload.category).ToList();
            }


            switch (payload.duration)
            {
                case ("PastThreeMonths"):
                    return pastThreeMonthsReport(requisitionDetails);

                case ("OneMonth"):
                    return oneMonthReport(requisitionDetails,payload.option[0]);
                    
                case ("CompareMonths"):
                    break;
                    //return compareMonthReport(requisitionDetails, payload.option);
            }
            return new ReportResponsePayload();
        }
        public ReportResponsePayload pastThreeMonthsReport(List<RequisitionDetail> requisitionDetails)
        {
            ReportResponsePayload reportResponsePayload = new ReportResponsePayload();
           
            //Distinct list of departments
            List<Department> departments = requisitionDetails
                                            .Select(d => d.Requisition.Requester.Department)
                                            .Distinct().OrderBy(d => d.DepartmentName)
                                            .ToList();

            reportResponsePayload.labels = new List<string>();
            for (int i = 3; i >= 0; i--)
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
               

                for (int i = 3; i >= 0; i--)
                {
                    int j = 0;
                    int month = DateTime.Today.AddMonths(-i).Month;
                    List<RequisitionDetail> newList = requisitionDetails
                        .Where(r => r.Requisition.RequestedDate.Value.Month == month && r.Requisition.Requester.DepartmentID == d.DepartmentID)
                        .ToList();
                    var count = newList.Where(ri => ri.Quantity > 0).Count();                  

                    int[] quantity = new int[count];
                    decimal?[] cost = new decimal?[count];
                    SupplierItem supItem = new SupplierItem();
                    foreach (RequisitionDetail rdetail in newList)
                    {
                        quantity[j] = rdetail.Quantity;
                        supItem = context.SupplierItems.Where(s => s.Rank == 1 && s.ItemID == rdetail.ItemID).FirstOrDefault();
                        cost[j] = (supItem.Cost == null) ? (decimal?)null : Convert.ToDecimal(supItem.Cost);

                        j++;

                    }
                   
                    Console.WriteLine(j);
                    decimal? sum = 0;
                    for (int m = 0; m < newList.Count; m++)
                    {
                        sum = sum + cost[m] * quantity[m];
                    }
                    chartValues.Add(sum == null ? 0 : (decimal)sum);
                }
                reportDataPayload.data = chartValues;
                reportResponsePayload.datasets.Add(reportDataPayload);
            }
            return reportResponsePayload;
        }

        public ReportResponsePayload generateRequisitionItemReport(RequisitionItemPayload payload)
        {
            List<RequisitionDetail> requisitionDetails;
            if (payload.department != null)
            {
                requisitionDetails = context.RequisitionDetails
                   .Where(rd => payload.department.Contains(rd.Requisition.Requester.DepartmentID)
                   && rd.Item.ItemID == payload.item)
                   .ToList();
            }
            else
            {
                requisitionDetails = context.RequisitionDetails.Where(rd => rd.Item.ItemID == payload.item).ToList();
            }

            if (payload.compareElement == "Quantity")
            {
                switch (payload.duration)
                {
                    case ("PastThreeMonths"):
                        return pastThreeMonthsReportQuantity(requisitionDetails);
                    case ("OneMonth"):
                        return oneMonthReportQuantity(requisitionDetails, payload.option[0]);
                    case ("CompareMonths"):
                        break;
                }
            }
            if (payload.compareElement == "Costs")
            {
                switch (payload.duration)
                {
                    case ("PastThreeMonths"):
                        return pastThreeMonthsReport(requisitionDetails);
                    case ("OneMonth"):
                        return oneMonthReport(requisitionDetails, payload.option[0]);
                    case ("CompareMonths"):
                        break;
                       
                }
            }
            return new ReportResponsePayload();
        }


        public ReportResponsePayload oneMonthReport(List<OrderSupplierDetail> orderSupplierDetails, int month)
        {
            
            ReportResponsePayload reportResponsePayload = new ReportResponsePayload();
            month++; // month integer in months is indexed from 0

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

        //One month report for requisition cost analysis
        public ReportResponsePayload oneMonthReport(List<RequisitionDetail> requisitionDetails,int month)
        {
            ReportResponsePayload reportResponsePayload = new ReportResponsePayload();
            month++; // month integer in months is indexed from 0

            // Get list of departments from requisition details
            List<Department> departments = requisitionDetails
            .Select(d => d.Requisition.Requester.Department)
            .Distinct()
            .OrderBy(d => d.DepartmentName)
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
            foreach (Department d in departments)
            {
                reportDataPayload = new ReportDataPayload();
                reportDataPayload.label = d.DepartmentName;
                reportDataPayload.backgroundColor = colors[departments.IndexOf(d)];

                List<decimal> chartValues = new List<decimal>();

                for (int i = 3; i >= 0; i--)
                {
                    int year = DateTime.Today.AddYears(-i - excludeCurrentYear).Year;
                    int j = 0;
                    List<RequisitionDetail> newList = requisitionDetails
                       .Where(r => r.Requisition.RequestedDate.Value.Month == month && r.Requisition.Requester.DepartmentID == d.DepartmentID
                       &&r.Requisition.RequestedDate.Value.Year==year)
                       .ToList();
                    var count = newList.Where(ri => ri.Quantity > 0).Count();

                    int[] quantity = new int[count];
                    decimal?[] cost = new decimal?[count];
                    SupplierItem supItem = new SupplierItem();
                    foreach (RequisitionDetail rdetail in newList)
                    {
                        quantity[j] = rdetail.Quantity;
                        supItem = context.SupplierItems.Where(s => s.Rank == 1 && s.ItemID == rdetail.ItemID).FirstOrDefault();
                        cost[j] = (supItem.Cost == null) ? (decimal?)null : Convert.ToDecimal(supItem.Cost);
                        j++;
                    }
                   
                    decimal? sum = 0;
                    for (int m = 0; m < newList.Count; m++)
                    {
                        sum = sum + cost[m] * quantity[m];
                    }                   

                    chartValues.Add(sum == null ? 0 : (decimal)sum);
                }
                reportDataPayload.data = chartValues;
                reportResponsePayload.datasets.Add(reportDataPayload);
            }

            return reportResponsePayload;
        }
        public ReportResponsePayload pastThreeMonthsReportQuantity(List<RequisitionDetail> requisitionDetails)
        {
            ReportResponsePayload reportResponsePayload = new ReportResponsePayload();

            //Distinct list of departments
            List<Department> departments = requisitionDetails
                                            .Select(d => d.Requisition.Requester.Department)
                                            .Distinct().OrderBy(d => d.DepartmentName)
                                            .ToList();

            reportResponsePayload.labels = new List<string>();
            for (int i = 3; i >= 0; i--)
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


                for (int i = 3; i >= 0; i--)
                {
                    
                    int month = DateTime.Today.AddMonths(-i).Month;
                    //summing up the quantitities filtered based on department id,month chosen
                    decimal? sum = requisitionDetails
                        .Where(rd => rd.Requisition.Requester.DepartmentID == d.DepartmentID
                        && rd.Requisition.RequestedDate.Value.Month == month)
                        .Sum(rd => rd.Quantity);

                    chartValues.Add(sum == null ? 0 : (decimal)sum);
                }
                reportDataPayload.data = chartValues;
                reportResponsePayload.datasets.Add(reportDataPayload);
            }
            return reportResponsePayload;
        }


        public ReportResponsePayload oneMonthReportQuantity(List<RequisitionDetail> requisitionDetails, int month)
        {
            ReportResponsePayload reportResponsePayload = new ReportResponsePayload();
            month++; // month integer in months is indexed from 0

            // Get list of departments from requisition details
            List<Department> departments = requisitionDetails
            .Select(d => d.Requisition.Requester.Department)
            .Distinct()
            .OrderBy(d => d.DepartmentName)
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
            foreach (Department d in departments)
            {
                reportDataPayload = new ReportDataPayload();
                reportDataPayload.label = d.DepartmentName;
                reportDataPayload.backgroundColor = colors[departments.IndexOf(d)];

                List<decimal> chartValues = new List<decimal>();

                for (int i = 3; i >= 0; i--)
                {
                    int year = DateTime.Today.AddYears(-i - excludeCurrentYear).Year;

                    decimal? sum = requisitionDetails
                        .Where(rd => rd.Requisition.Requester.DepartmentID == d.DepartmentID
                        && rd.Requisition.RequestedDate.Value.Month == month
                        && rd.Requisition.RequestedDate.Value.Year==year)
                        .Sum(rd => rd.Quantity);

                    
                    chartValues.Add(sum == null ? 0 : (decimal)sum);
                }
                reportDataPayload.data = chartValues;
                reportResponsePayload.datasets.Add(reportDataPayload);
            }

            return reportResponsePayload;
        }

       
    }
}

