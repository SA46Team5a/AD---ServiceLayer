using ServiceLayer.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    // Author: Jack
    public class RequisitionDetailPayload
    {
        public int RequisitionDetailID { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public string UnitOfMeasure { get; set; }

        public RequisitionDetailPayload(){}
        public RequisitionDetailPayload(RequisitionDetail rq)
        {
            RequisitionDetailID = rq.RequisitionDetailsID;
            ItemName = rq.Item.ItemName;
            Quantity = rq.Quantity;
            UnitOfMeasure = rq.Item.UnitOfMeasure;
        }

        public static List<RequisitionDetailPayload> ConvertEntityToPayload(List<RequisitionDetail> requisitionDetails)
        {
            List<RequisitionDetailPayload> payload = new List<RequisitionDetailPayload>();
            if (requisitionDetails != null && requisitionDetails.Count > 0)
            {
                requisitionDetails.ForEach(rq => payload.Add(new RequisitionDetailPayload(rq)));
            }
            return payload;
        }
    }

    public class RequisitionPayload
    {
        public int RequisitionID { get; set; }
        public string RequesterName { get; set; }
        public DateTime RequestDate { get; set; }
        public List<RequisitionDetailPayload> RequisitionDetails { get; set; }
        
        public RequisitionPayload(Requisition r)
        {
            RequisitionID = r.RequisitionID;
            RequesterName = r.Requester.EmployeeName;
            RequestDate = r.RequestedDate is null ? DateTime.Today : (DateTime) r.RequestedDate;
            RequisitionDetails = RequisitionDetailPayload.ConvertEntityToPayload(r.RequisitionDetails.ToList());
        }

        public static List<RequisitionPayload> ConvertEntityToPayload(List<Requisition> requisitions)
        {
            List<RequisitionPayload> payload = new List<RequisitionPayload>();
            if (requisitions != null && requisitions.Count > 0)
            {
                requisitions.ForEach(r => payload.Add(new RequisitionPayload(r)));
            }
            return payload;
        }
    }
}
