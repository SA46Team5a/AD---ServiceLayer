using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    interface IUpdateDepartmentService
    {
        void updateAuthority(Authority auth);
        void rescindAuthority(Authority auth);
        void updateDepartmentRepresentative(Employee oldemp, Employee newemp);
        void updateCollectionPoint(Department dep, CollectionPoint cp);
        void generateNewPasscode(Department dep);

    }
}
