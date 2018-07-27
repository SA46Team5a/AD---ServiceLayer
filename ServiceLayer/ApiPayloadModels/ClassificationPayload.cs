using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    public class CategoryPayload
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        public CategoryPayload() { }
        public CategoryPayload(Category category)
        {
            CategoryID = category.CategoryID;
            CategoryName = category.CategoryName;
        }

        public static List<CategoryPayload> ConvertEntityToPayload(List<Category> categories)
        {
            List<CategoryPayload> payload = new List<CategoryPayload>();
            categories.ForEach(c => payload.Add(new CategoryPayload(c)));
            return payload;
        }
    }

    public class CollectionPointPayload
    {
        public int CollectionPointID { get; set; }
        public string CollectionPointDetails { get; set; }

        public CollectionPointPayload() { }
        public CollectionPointPayload(CollectionPoint cp)
        {
            CollectionPointID = cp.CollectionPointID;
            CollectionPointDetails = cp.CollectionPointDetails;
        }

        public static List<CollectionPointPayload> ConvertEntityToPayload(List<CollectionPoint> categories)
        {
            List<CollectionPointPayload> payload = new List<CollectionPointPayload>();
            categories.ForEach(c => payload.Add(new CollectionPointPayload(c)));
            return payload;
        }
    }
}
