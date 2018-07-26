using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    // Author: Jack
    class ReportTest
    {
        static void Main(string[] args)
        {
            IReportService reportService = new ReportService();
            StationeryStoreEntities context = StationeryStoreEntities.Instance;

            // Test 1
            // Reorder Cost Report last 3 months
            Console.WriteLine("=== Test generateReorderCostReport ===");
            Console.WriteLine("1. PastThreeMonths report");

            // set up reorder request payload
            ReorderRequestPayload reqPayload = new ReorderRequestPayload();
            reqPayload.category = 1;
            reqPayload.department = new List<string> { "ALPA" };
            reqPayload.duration = "PastThreeMonths";
            reqPayload.option = null;

            // Test
            ReportResponsePayload respPayload = reportService.generateReorderCostReport(reqPayload);
            Console.WriteLine(respPayload.labels[0]);
            Console.WriteLine(respPayload.datasets[0].label);
            Console.WriteLine(respPayload.datasets[0].backgroundColor);
            respPayload.datasets[0].data.ForEach(d => Console.WriteLine(d));
        }
    }
}
