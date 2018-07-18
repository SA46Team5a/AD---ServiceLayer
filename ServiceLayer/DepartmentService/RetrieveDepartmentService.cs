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
        static private StationeryStoreEntities context = new StationeryStoreEntities();

        //To list all the collectionpoints
        public List<CollectionPoint> getCollectionPoints()
        {
           return context.CollectionPoints.ToList();
        }
        //to get current authorized employee when dept ID is passed
        public string getAuthority(string dept)
        {
            string authorisedempname=null;
            Department D = context.Departments.Where(d => d.DepartmentID == dept).First();
            List<Employee> E = context.Employees.Where(e => e.DepartmentID == D.DepartmentID).ToList();
            foreach (Employee e in E)
            {
                Authority A = context.Authorities.Where(a => a.EmployeeID == e.EmployeeID).First();
                if(A.ToString() != null)
                {
                    authorisedempname= e.EmployeeName;
                }
            }
            return authorisedempname;
        }
        //To list all the employees for the particular department
        public List<Employee> getDepartmentRepresentative(string dept)
        {
            List<Employee> E=context.Employees.Where(e => e.DepartmentID == dept).ToList();
            return E;
        }

        public CollectionPoint getCollectionPointOfDepartment(Department dep)
        {
            return null;
        }
        //To get the collection point object for particular department of the employeeID passed
        public CollectionPoint getCollectionPointOfDepartment(string emp)
        {
            Employee E = context.Employees.Where(e => e.EmployeeID == emp).First();
            Department D = context.Departments.Where(d => d.DepartmentID == E.DepartmentID).First();
            CollectionPoint C=context.CollectionPoints.Where(c => c.CollectionPointID == D.CollectionPointID).First();
            return C;
        }
        public Boolean verifyPassCode(int passcode)
        {
            return true;
        }
        //To get employee object of the particular employee Id given
        public Employee getEmployeeById(string emp)
        {
            Employee E = context.Employees.Where(e => e.EmployeeID == emp).First();
            return E;

        }
        public string getDepartmentID(Employee emp)
        {
            Department E = context.Departments.Where(e => e.DepartmentID == emp.DepartmentID).First();
            
            return E.DepartmentID;

        }
    }
}
