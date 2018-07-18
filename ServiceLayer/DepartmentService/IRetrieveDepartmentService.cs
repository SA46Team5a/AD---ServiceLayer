using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    interface IRetrieveDepartmentService
    {
        Authority getAuthority();
        DepartmentRepresentative getDepartmentRepresentative(Department dep);
        CollectionPoint getCollectionPointOfDepartment(Department dep);
        Boolean verifyPassCode(int passcode);
        Employee getEmployeeById(string emp);
    }
}
