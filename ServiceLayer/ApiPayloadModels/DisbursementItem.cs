using ServiceLayer.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class DisbursementDetailPayload
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string UnitOfMeasure { get; set; }
        public string Reason { get; set; }
        public List<int> DisbursementDutyIds { get; set; }
        public int DisbursedQuantity { get; set; }
        public int? CollectedQuantity { get; set; }
        public int? RejectedQuantity { get { return CollectedQuantity is null ? null : DisbursedQuantity - CollectedQuantity; } }

        public DisbursementDetailPayload(DisbursementDetail d)
        {
            ItemId = d.RequisitionDetail.ItemID;
            ItemName = d.RequisitionDetail.Item.ItemName;
            UnitOfMeasure = d.RequisitionDetail.Item.UnitOfMeasure;
            DisbursementDutyIds = new List<int>();
            Reason = d.Reason;
            DisbursedQuantity = d.Quantity;
            CollectedQuantity = d.CollectedQty;
        }

        public DisbursementDetailPayload(string itemId, string reason, int disbursedQuantity, int collectedQuantity)
        {
            ItemId = itemId;
            Reason = reason;
            DisbursedQuantity = disbursedQuantity;
            CollectedQuantity = collectedQuantity;
        }

        public static List<DisbursementDetailPayload> ConvertEntityToPayload(List<DisbursementDetail> disbursementDetails)
        {
            List<DisbursementDetailPayload> disbursementDetailPayloads = new List<DisbursementDetailPayload>();
            foreach (DisbursementDetail disbursementDetail in disbursementDetails)
            {
                DisbursementDetailPayload disbursementDetailPayload = disbursementDetailPayloads.FirstOrDefault(d => d.ItemId == disbursementDetail.RequisitionDetail.ItemID);
                if (disbursementDetailPayload is null)
                {
                    // create and add a new payload to payload list
                    disbursementDetailPayload = new DisbursementDetailPayload(disbursementDetail);
                    disbursementDetailPayloads.Add(disbursementDetailPayload);
                }
                // add disbursement duty id
                disbursementDetailPayload.DisbursementDutyIds.Add(disbursementDetail.Disbursement.DisbursementDutyID);
            }
            return disbursementDetailPayloads;
        }
    }
}
