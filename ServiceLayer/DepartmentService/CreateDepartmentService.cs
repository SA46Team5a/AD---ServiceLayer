using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    //Authour: Divyashree
    partial class CreateDepartmentService:ICreateDepartmentService
    {
        static private StationeryStoreEntities context = new StationeryStoreEntities();
        public void addAuthority(Employee emp, DateTime startDate, DateTime endDate)
        {
            // 1 Update Dept Head's Authority with End Date

            // 2 Creating new Authority Object
            Authority auth = new Authority();
            auth.EmployeeID = emp.EmployeeID;
            auth.StartDate = startDate;
            auth.EndDate = endDate;
            context.Authorities.Add(auth);

            // 3 Reopen Dept Head's authority after end of first 
            context.SaveChanges();
            


        }
    }
}
