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

        public DisbursementDetailPayload(Item i)
        {
            ItemId = i.ItemID;
            ItemName = i.ItemName;
            UnitOfMeasure = i.UnitOfMeasure;
        }

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
            List<Item> items = disbursementDetails.Select(d => d.RequisitionDetail.Item).Distinct().OrderBy(i => i.ItemID).ToList();
            List<DisbursementDetail> details;
            DisbursementDetailPayload payload;

            foreach (Item item in items)
            {
                details = disbursementDetails.Where(d => d.RequisitionDetail.ItemID == item.ItemID).ToList();
                payload = new DisbursementDetailPayload(item);
                details.ForEach(d => payload.DisbursedQuantity += d.Quantity);
                payload.DisbursementDutyIds = details.Select(d => d.Disbursement.DisbursementDutyID).Distinct().ToList();
                disbursementDetailPayloads.Add(payload);

            }
            
            return disbursementDetailPayloads;
        }
    }
}
