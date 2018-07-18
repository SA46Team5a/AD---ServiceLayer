using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    interface ICreateDepartmentService
    {
        void addAuthority(Employee emp);
    }
}
