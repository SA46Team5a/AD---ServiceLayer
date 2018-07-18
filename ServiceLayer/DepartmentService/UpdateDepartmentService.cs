using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;


namespace ServiceLayer
{
    //Authour: Divyashree
    partial class UpdateDepartmentService:IUpdateDepartmentService
    {
        static private StationeryStoreEntities context = new StationeryStoreEntities();

        //updating the authority table
        public void updateAuthority(Authority auth)
        {
            Authority A = context.Authorities.Where(a => a.Employee.EmployeeID == auth.EmployeeID).First();
            A.EmployeeID = auth.EmployeeID;
            A.StartDate = auth.StartDate;
            A.EndDate = auth.EndDate;
            context.SaveChanges();

            //Authority A = context.Authorities.Where(a => a.AuthorityID == auth.AuthorityID).First();

        }

        public void rescindAuthority(Authority auth)
        {
            
            auth.EndDate = DateTime.Today;
            //string getDepartmentID(Employee emp);
            
           
            context.SaveChanges();

        }

        //update old representative enddate and add new the departmentRepresentative
        public void updateDepartmentRepresentative(Employee oldemp, Employee newemp)
        {
            DepartmentRepresentative DR = context.DepartmentRepresentatives.Where(d => d.EmployeeID == oldemp.EmployeeID).First();
            DR.EndDate = DateTime.Today;
            DepartmentRepresentative deprep = new DepartmentRepresentative();
            deprep.EmployeeID = newemp.EmployeeID;
            deprep.StartDate = DateTime.Today;           
            context.SaveChanges();
        }

        public void updateCollectionPoint(Department dep, CollectionPoint cp)
        {
            Department CP = context.Departments.Where(c => c.CollectionPointID == dep.CollectionPointID).First();
            dep.CollectionPointID = cp.CollectionPointID;
            context.SaveChanges();


        }

        public void generateNewPasscode(Department dep)
        {

        }
    }
}
