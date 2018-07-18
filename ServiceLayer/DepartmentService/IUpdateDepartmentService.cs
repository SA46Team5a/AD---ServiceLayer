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
        void updateDepartmentRepresentative(Department dep, Employee emp);
        void updateCollectionPoint(Department dep, CollectionPoint cp);
        void generateNewPasscode(Department dep);

    }
}
