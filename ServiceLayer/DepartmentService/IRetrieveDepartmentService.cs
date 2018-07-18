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
        List<CollectionPoint> getCollectionPoints();
        string getAuthority(string dept);
        List<Employee> getDepartmentRepresentative(string dept);
        CollectionPoint getCollectionPointOfDepartment(Department dep);
        CollectionPoint getCollectionPointOfDepartment(string emp);
        Boolean verifyPassCode(int passcode);
        Employee getEmployeeById(string emp);
    }
}
