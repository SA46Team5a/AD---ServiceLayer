using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;


namespace ServiceLayer
{
    //Authour: Divyashree
    public class DepartmentService : IDepartmentService
    {
        static StationeryStoreEntities context = StationeryStoreEntities.Instance;
        // Retrieve
        //to get current authorized employee when dept ID is passed
        public Authority getCurrentAuthority(string dept)
        {
            DateTime today = DateTime.Today;
            return context.Authorities
                .OrderBy(a => a.StartDate)
                .First(a => a.Employee.DepartmentID == dept
                && (a.EndDate >= today || a.EndDate == null));           
        }

        //To get current the departmentrepresentative for the particular department
        public DepartmentRepresentative getCurrentDepartmentRepresentative(string dept)
        {           
            Department department = context.Departments.First(d => d.DepartmentID == dept);       
            return context.DepartmentRepresentatives.
                Where(r => r.EndDate == null || r.EndDate >= DateTime.Today)
                .First(r => r.Employee.DepartmentID == department.DepartmentID);            
        }

       
        //To get the collection point object for particular employee of department
        public CollectionPoint getCollectionPointOfEmployee(string emp)
        {
            Employee E = context.Employees.Where(e => e.EmployeeID == emp).First();
            Department D = context.Departments.Where(d => d.DepartmentID == E.DepartmentID).First();
            return D.CollectionPoint;
        }
        public bool verifyPassCode(string passcode,string dep)
        {
            DepartmentRepresentative depRep = context.DepartmentRepresentatives
                .First(dr => dr.Employee.DepartmentID == dep);
            Console.WriteLine(depRep.Passcode);           
            return depRep.Passcode == passcode;
        }

        //To get employee object of the particular employee Id given
        public Employee getEmployeeById(string emp)
        {           
            return context.Employees.First(e => e.EmployeeID == emp);
        }

        //To get department for particular employee
        public string getDepartmentID(string emp)
        {
            Employee employee = context.Employees.First(e => e.EmployeeID == emp);
            Department empDepartment = context.Departments
                .Where(e => e.DepartmentID == employee.DepartmentID).First();            
            return empDepartment.DepartmentID;
        }


        // Create
        public void addAuthority(Employee emp, DateTime startDate, DateTime endDate)
        {
            // 1 Update Dept Head's Authority with End Date          
           
            Employee deptHead = emp.Department.DepartmentHead;
            Authority authority = context.Authorities.Where(a => a.EmployeeID == deptHead.EmployeeID && a.EndDate == null).First();
            Console.WriteLine(authority.AuthorityID);
            authority.EndDate = startDate.AddDays(-1);
 
            // 2 Creating new Authority Object
            Authority auth = new Authority();
            auth.EmployeeID = emp.EmployeeID;
            auth.StartDate = startDate;
            auth.EndDate = endDate;
            context.Authorities.Add(auth);

            // 3 Reopen Dept Head's authority after endDate 
            Authority autho = new Authority();
            autho.EmployeeID = deptHead.EmployeeID;
            autho.StartDate = endDate.AddDays(1);
            context.Authorities.Add(autho);             
            context.SaveChanges();

        }

        // Update
        //updating the authority table
        public void updateAuthority(Authority auth)
       
        {
            Authority A = context.Authorities.First(a => a.Employee.EmployeeID == auth.EmployeeID);
            A.EmployeeID = auth.EmployeeID;
            A.StartDate = auth.StartDate;
            A.EndDate = auth.EndDate;
            context.SaveChanges();
            Console.WriteLine(A.AuthorityID);

        }


        public void rescindAuthority(Authority auth)
        {
            // 1 Change end date of current auth to today

            Authority authority = context.Authorities
                .Where(a => a.EmployeeID == auth.EmployeeID & auth.EndDate >= DateTime.Today).First();
                auth.EndDate = DateTime.Today;

            // 2 Change start date of the next auth (which is dept head) to tmr

            Authority autho = context.Authorities.Where(a => a.EndDate == null).First();
            autho.StartDate = DateTime.Today.AddDays(1);
            context.SaveChanges();
        }

        //update old representative enddate and add new the departmentRepresentative
        public void updateDepartmentRepresentative(int currentDeptRepId, string newRepEmpId)
        {
            string dep = "CHEM";
            DepartmentRepresentative deptRepresentative = context.DepartmentRepresentatives
                .Where(d => d.DeptRepID == currentDeptRepId).First();
            deptRepresentative.EndDate = DateTime.Today;
            DepartmentRepresentative deprep = new DepartmentRepresentative();
            deprep.EmployeeID = newRepEmpId;            
           // Console.WriteLine(deprep.DeptRepID);
            deprep.StartDate = DateTime.Today.AddDays(1);            
            deprep.Passcode= generateNewPasscode(dep);
            context.DepartmentRepresentatives.Add(deprep);
            context.SaveChanges();
           // Console.WriteLine(deprep.StartDate); Console.WriteLine(deprep.EmployeeID);
        }

        public void updateCollectionPoint(string dep, int cp)
        {
            Department department = context.Departments.Where(d => d.DepartmentID == dep).First();
           // Console.WriteLine(department.CollectionPointID);
            //CollectionPoint collectionPoint = context.CollectionPoints
            //    .Where(c => c.CollectionPointID == department.CollectionPointID).First();
            //Console.WriteLine(department.CollectionPointID);
            department.CollectionPointID = cp;          
             context.SaveChanges();
          

        }
        //To generate new passcode
        public string generateNewPasscode(string dep)
        {
            DepartmentRepresentative depRep = context.DepartmentRepresentatives
                .First(dr => dr.Employee.DepartmentID == dep);            
            Random num = new Random();
            int passcode = num.Next(1000,9999);// Console.Write(depRep.Passcode);
            return depRep.Passcode = Convert.ToString(passcode);         
            
           
        }

    }
}
