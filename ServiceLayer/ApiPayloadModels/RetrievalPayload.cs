using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class RetrievalItemPayload
    {
        public string ItemID;
        public string ItemName;
        public string UnitOfMeasure;
        public int QtyInStock;
        public int QtyToRetrieve;
    }

    public class RetrievalFormPayload
    {
        public int disDutyId { get; set; }
        public List<RetrievalItemPayload> retrievalItemPayloads { get; set; }
    }
}
