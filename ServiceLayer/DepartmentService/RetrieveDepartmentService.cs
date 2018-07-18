using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;


namespace ServiceLayer
{
    //Authour: Divyashree
    partial class RetrieveDepartmentService:IRetrieveDepartmentService
    {
        public Authority getAuthority()
        {
            return null;
        }

        public DepartmentRepresentative getDepartmentRepresentative(Department dep)
        {
            return null;
        }

        public CollectionPoint getCollectionPointOfDepartment(Department dep)
        {
            return null;
        }

        public Boolean verifyPassCode(int passcode)
        {
            return true;
        }
        public Employee getEmployeeById(string emp)
        {
            return null;

        }
    }
}
