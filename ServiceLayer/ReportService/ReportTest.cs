using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    // Author: Jack,Bhat Pavana
    class ReportTest
    {
        static void Main(string[] args)
        {
            IReportService reportService = new ReportService();
            StationeryStoreEntities context = StationeryStoreEntities.Instance;

           // Test 1
            // Reorder Cost Report last 3 months
            Console.WriteLine("=== Test generateReorderCostReport ===");
         //   Console.WriteLine("1. PastThreeMonths report");

            // set up reorder request payload
            ReorderRequestPayload reqPayload = new ReorderRequestPayload();
            //reqPayload.category = 1;
            //reqPayload.department = new List<string> { "ALPA" };
            //reqPayload.duration = "PastThreeMonths";
            //reqPayload.option = null;

            //// Test
           // ReportResponsePayload respPayload = reportService.generateReorderCostReport(reqPayload);
            //Console.WriteLine(respPayload.labels[0]);
            //Console.WriteLine(respPayload.datasets[0].label);
            //Console.WriteLine(respPayload.datasets[0].backgroundColor);
            //respPayload.datasets[0].data.ForEach(d => Console.WriteLine(d));

            //Test for one month report

            Console.WriteLine("1. One month report");
            reqPayload.category = 1;
            reqPayload.department = new List<string> { "ALPA" };
            reqPayload.duration = "OneMonth";
            reqPayload.option = new List<int> { 0 };

            ReportResponsePayload respPayload = reportService.generateReorderCostReport(reqPayload);
            Console.WriteLine(respPayload.labels[0]);

            Console.WriteLine(respPayload.datasets[0].label);
            Console.WriteLine(respPayload.datasets[0].backgroundColor);
            respPayload.datasets[0].data.ForEach(d => Console.WriteLine(d));

            // Test 2
            // Requisition Cost Report last 3 months
            // Console.WriteLine("=== Test generateRequisitionCostReport ===");
            ////  Console.WriteLine("1. PastThreeMonths report");

            //  // set up requistion request payload
            // RequisitionRequestPayload reqPayload1 = new RequisitionRequestPayload();

            //reqPayload1.category = 2;
            //reqPayload1.department = new List<string> { "CHEM" };
            //reqPayload1.duration = "PastThreeMonths";
            //reqPayload1.option = null;


            //// Test
            //ReportResponsePayload respPayload1 = reportService.generateRequisitionCostReport(reqPayload1);
            //Console.WriteLine(respPayload1.labels[0]);
            //Console.WriteLine(respPayload1.datasets[0].label);
            //Console.WriteLine(respPayload1.datasets[0].backgroundColor);
            //respPayload1.datasets[0].data.ForEach(d => Console.WriteLine(d));

            //Console.WriteLine("2. One month report");
            //RequisitionRequestPayload reqPayload1 = new RequisitionRequestPayload();

            //reqPayload1.category = 2;
            //reqPayload1.department = new List<string> { "CHEM" };
            //reqPayload1.duration = "OneMonth";
            //reqPayload1.option = new List<int> { 7 };


            // Test

            //ReportResponsePayload respPayload1 = reportService.generateRequisitionCostReport(reqPayload1);            
            //Console.WriteLine(respPayload1.labels[0]);
            //Console.WriteLine(respPayload1.datasets[0].label);
            //Console.WriteLine(respPayload1.datasets[0].backgroundColor);
            //respPayload1.datasets[0].data.ForEach(d => Console.WriteLine(d));

            // Test 3
            // Requisition Quantity Report last 3 months
            Console.WriteLine("=== Test generateRequisitionQuantityReport ===");
           // Console.WriteLine("1. PastThreeMonths report");
            RequisitionItemPayload reqPayload2 = new RequisitionItemPayload();
            //reqPayload2.item = "T100";
            //reqPayload2.department = new List<string> { "CHEM" };
            //reqPayload2.duration = "PastThreeMonths";
            //reqPayload2.option = new List<int> { 7 };
            //reqPayload2.compareElement = "1";

            //ReportResponsePayload respPayload2 = reportService.generateRequisitionItemReport(reqPayload2);
            //Console.WriteLine(respPayload2.labels[0]);
            //Console.WriteLine(respPayload2.datasets[0].label);
            //Console.WriteLine(respPayload2.datasets[0].backgroundColor);
            //respPayload2.datasets[0].data.ForEach(d => Console.WriteLine(d));

            //Console.WriteLine("1. Onemonth report");
            //reqPayload2.item = "E006";
            //reqPayload2.department = new List<string> { "CHEM" };
            //reqPayload2.duration = "OneMonth";
            //reqPayload2.option = new List<int> { 8 };
            //reqPayload2.compareElement = "Costs";
            //ReportResponsePayload respPayload2 = reportService.generateRequisitionItemReport(reqPayload2);
            //Console.WriteLine(respPayload2.labels[0]);
            //Console.WriteLine(respPayload2.datasets[0].label);
            //Console.WriteLine(respPayload2.datasets[0].backgroundColor);
            //respPayload2.datasets[0].data.ForEach(d => Console.WriteLine(d));

        }
    }
}
