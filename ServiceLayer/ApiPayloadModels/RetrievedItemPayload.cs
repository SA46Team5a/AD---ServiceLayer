using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class ItemAndQtyPayload
    {
        public string ItemId { get; set; }
        public int Quantity { get; set; }

        public static Dictionary<string, int> ConvertListToDictionary(List<ItemAndQtyPayload> list)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            list.ForEach(l => dict.Add(l.ItemId, l.Quantity));
            return dict;
        }
    }
}
