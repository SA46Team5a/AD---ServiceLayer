using ServiceLayer.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    [Serializable]
    public class DisbursementItem
    {
        public DisbursementItem(string itemId, string reason, int disbursedQuantity, int collectedQuantity)
        {
            ItemId = itemId;
            Reason = reason;
            DisbursedQuantity = disbursedQuantity;
            CollectedQuantity = collectedQuantity;
        }

        public string ItemId { get; set; }
        public string Reason { get; set; }
        public int DisbursedQuantity { get; set; }
        public int CollectedQuantity { get; set; }
        public int RejectedQuantity { get { return DisbursedQuantity - CollectedQuantity; } }
    }
}
