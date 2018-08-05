using System.Collections.Generic;

namespace ServiceLayer
{
    public class RetrievalItemPayload : ItemPayload
    {
       public RetrievalItemPayload(){}
       public int QtyToRetrieve{ get; set; }
    }

    public class RetrievalFormPayload
    {
        public RetrievalFormPayload() { }
        public int disDutyId { get; set; }
        public List<RetrievalItemPayload> retrievalItemPayloads { get; set; }
    }
}
