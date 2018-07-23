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
        public string Reason { get; set; }
        public int DisbursementDutyId { get; set; }
        public int DisbursedQuantity { get; set; }
        public int? CollectedQuantity { get; set; }
        public int? RejectedQuantity { get { return CollectedQuantity is null ? null : DisbursedQuantity - CollectedQuantity; } }

        public DisbursementDetailPayload(DisbursementDetail d)
        {
            ItemId = d.RequisitionDetail.ItemID;
            Reason = d.Reason;
            DisbursementDutyId = d.Disbursement.DisbursementDutyID;
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
            List<DisbursementDetailPayload> payload = new List<DisbursementDetailPayload>();
            disbursementDetails.ForEach(d => payload.Add(new DisbursementDetailPayload(d)));
            return payload;
        }
    }
}
