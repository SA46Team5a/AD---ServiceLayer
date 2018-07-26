using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    public class ReportDataPayload
    {
        public string label { get; set; }
        public string backgroundColor { get; set; }
        public List<decimal> data { get; set; }
    }

    public class ReorderReportingModel
    {
        public List<Category> categories { get; set; }
        public List<Supplier> suppliers { get; set; }

        public List<string> month = new List<string> { "January", "February", "March", "April", "May",
            "June", "July", "August", "September", "October", "November", "December" };
    }

    public class ReorderRequestPayload
    {
        public int category { get; set; }
        public List<string> department { get; set; }
        public string duration { get; set; }
        public List<int> option { get; set; }
    }

    public class ReportResponsePayload
    {
        public List<string> labels { get; set; }
        public List<ReportDataPayload> datasets { get; set; }
    }
}
