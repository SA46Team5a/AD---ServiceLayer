using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    // Author: Bhat Pavana
    public class ClassificationService : IClassificationService
    {
        static StationeryStoreEntities context = StationeryStoreEntities.Instance;
        public List<ApprovalStatus> GetApprovalStatus()
        {
            return context.ApprovalStatus1.Where(a => a.ApprovalStatusID < 5).ToList();
        }

        public List<string> GetApprovalStatusNames()
        {
            List<string> approvalStatuses = new List<string>() { "All " };
            approvalStatuses.AddRange(GetApprovalStatus().Select(a => a.ApprovalStatusName).ToList());
            return approvalStatuses;
        }

        public List<Category> GetCategories()
        {
            return context.Categories.ToList();            
        }

        public List<CollectionPoint> GetCollectionPoints()
        {
            return context.CollectionPoints.ToList();
        }

        public List<RetrievalStatus> GetRetrievalStatus()
        {
            return context.RetrievalStatus1.ToList();
        }

        public List<Item> GetItems()
        {
            return context.Items.ToList();
        }
    }
}
