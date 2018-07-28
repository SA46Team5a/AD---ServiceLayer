using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;
namespace ServiceLayer
{

    public interface IClassificationService
    {
        List<Category> GetCategories();
        List<ApprovalStatus> GetApprovalStatus();
        List<RetrievalStatus> GetRetrievalStatus();
        List<CollectionPoint> GetCollectionPoints();
        List<Item> GetItems();
    }
}
