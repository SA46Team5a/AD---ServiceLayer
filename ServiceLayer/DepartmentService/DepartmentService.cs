using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;


namespace ServiceLayer
{
    //Authour: Divyashree
    partial class DepartmentService : IDepartmentService
    {
        static StationeryStoreEntities context = StationeryStoreEntities.Instance;
        // Retrieve
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
        public string getDepartmentID(string emp)
        {
            Employee employee = context.Employees.First(e => e.EmployeeID == emp);
            Department E = context.Departments.Where(e => e.DepartmentID == employee.DepartmentID).First();
            
            return E.DepartmentID;
        }

        // Create
        public void addAuthority(Employee emp, DateTime startDate, DateTime endDate)
        {
            // 1 Update Dept Head's Authority with End Date
            Employee deptHead = emp.Department.DepartmentHead;


            // 2 Creating new Authority Object
            Authority auth = new Authority();
            auth.EmployeeID = emp.EmployeeID;
            auth.StartDate = startDate;
            auth.EndDate = endDate;
            context.Authorities.Add(auth);

            // 3 Reopen Dept Head's authority after end of first 
            context.SaveChanges();



        }

        // Update
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
