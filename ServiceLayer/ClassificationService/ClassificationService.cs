using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    class ClassificationService : IClassificationService
    {
        public List<ApprovalStatus> GetApprovalStatus()
        {
            return new List<ApprovalStatus>();
        }

        public List<Category> GetCategories()
        {
            return new List<Category>();
        }

        public List<CollectionPoint> GetCollectionPoints()
        {
            return new List<CollectionPoint>();
        }

        public List<RetrievalStatus> GetRetrievalStatus()
        {
            return new List<RetrievalStatus>();
        }
    }
}
