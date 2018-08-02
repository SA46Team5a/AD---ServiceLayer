using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;


namespace ServiceLayer
{
    // Author: Divyashree, Jack
    public class DepartmentService : IDepartmentService
    {
        static StationeryStoreEntities context = StationeryStoreEntities.Instance;
        // Retrieve
        //to get current or upcoming authorized employee when dept ID is passed, if no one is assigned, return null
        public Authority getDelegatedAuthority(string dept)
        {
            DateTime today = DateTime.Today;
            String deptHeadId = context.Departments.First(d => d.DepartmentID == dept).DepartmentHeadID;
            return context.Authorities
                .OrderBy(a => a.EndDate)
                .Where(a => a.Employee.DepartmentID == dept && a.EmployeeID != deptHeadId && a.EndDate > DateTime.Today)
                .FirstOrDefault();
        }

        // gets current authority
        public Authority getCurrentAuthority(string dept)
            => context.Authorities
                .Where(a => a.Employee.DepartmentID == dept)
                .OrderBy(a => a.StartDate)
                .First(a => a.StartDate <= DateTime.Today && (a.EndDate == null || a.EndDate >= a.StartDate) );

        //To get current the departmentrepresentative for the particular department
        public DepartmentRepresentative getCurrentDepartmentRepresentative(string dept)
        {           
            Department department = context.Departments.First(d => d.DepartmentID == dept);       
            return context.DepartmentRepresentatives
                .Where(r => r.EndDate == null)
                .ToList()
                .OrderBy(d => d.DeptRepID)
                .Last(r => r.Employee.DepartmentID == department.DepartmentID);            
        }

        //To get the collection point object for particular employee of department
        public CollectionPoint getCollectionPointOfEmployee(string emp)
        {
            Employee E = context.Employees.Where(e => e.EmployeeID == emp).First();
            Department D = context.Departments.Where(d => d.DepartmentID == E.DepartmentID).First();
            return D.CollectionPoint;
        }

        // Get collection point for department
        public CollectionPoint getCollectionPointOfDepartment(string depId)
            => context.Departments.Where(d => d.DepartmentID == depId).First().CollectionPoint;

        public bool verifyPassCode(string passcode,string dep)
        {
            DepartmentRepresentative depRep = context.DepartmentRepresentatives
                .First(dr => dr.Employee.DepartmentID == dep);                  
            return depRep.Passcode == passcode;
        }

        // get employees of department
        public List<Employee> getEmployeesOfDepartment(string depId)
           => context.Departments.First(d => d.DepartmentID == depId).Employees.ToList();
       

        //To get employee object of the particular employee Id given
        public Employee getEmployeeById(string emp)
        {           
            return context.Employees.First(e => e.EmployeeID == emp);
        }

        //To get employee object of the partcular employee name
        public Employee getEmployeeObject(String empName)
        {
            return context.Employees.Where(x => x.EmployeeName == empName).FirstOrDefault();
        }

        //To get department for particular employee
        public string getDepartmentID(string emp)
        {
            Employee employee = context.Employees.First(e => e.EmployeeID == emp);
            Department empDepartment = context.Departments
                .Where(e => e.DepartmentID == employee.DepartmentID).First();            
            return empDepartment.DepartmentID;
        }

        // get all departments
        public List<Department> getDepartments()
            => context.Departments.ToList();

        // Create
        public void addAuthority(Employee emp, DateTime startDate, DateTime endDate)
        {
            // 1 Update Dept Head's Authority with End Date          
            Employee deptHead = emp.Department.DepartmentHead;
            Authority authority = context.Authorities.Where(a => a.EmployeeID == deptHead.EmployeeID && a.EndDate == null).First();
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

        public List<Employee> getEligibleDepartmentRepresentatives(string deptId)
        {
            List<Employee> employees = getEmployeesOfDepartment(deptId);
            List<string> ineligibleEmployees = new List<string>();
            ineligibleEmployees.Add(context.Departments.First(d => d.DepartmentID == deptId).DepartmentHeadID);
            ineligibleEmployees.Add(getDelegatedAuthority(deptId).EmployeeID);
            return employees.Where(e => !ineligibleEmployees.Contains(e.EmployeeID)).ToList();
        }

        public List<Employee> getEligibleDelegatedAuthority(string deptId)
        {
            List<Employee> employees = getEmployeesOfDepartment(deptId);
            List<string> ineligibleEmployees = new List<string>();
            ineligibleEmployees.Add(context.Departments.First(d => d.DepartmentID == deptId).DepartmentHeadID);
            ineligibleEmployees.Add(getCurrentDepartmentRepresentative(deptId).EmployeeID);
            return employees.Where(e => !ineligibleEmployees.Contains(e.EmployeeID)).ToList();
        }

        // Update
        //updating the authority table
        public void updateAuthority(Authority auth)
        {
            Authority delegatedAuthority = context.Authorities.First(a => a.Employee.EmployeeID == auth.EmployeeID);
            delegatedAuthority.EmployeeID = auth.EmployeeID;
            delegatedAuthority.StartDate = auth.StartDate < DateTime.Today ? delegatedAuthority.StartDate : auth.StartDate;
            delegatedAuthority.EndDate = auth.EndDate;
            context.SaveChanges();         
        }

        public void rescindAuthority(string empId)
        {
            // 1 Change end date of current auth to today
            String deptId = context.Employees.First(e => e.EmployeeID == empId).DepartmentID;
            Authority delegatedAuthority = getDelegatedAuthority(deptId);
            delegatedAuthority.EndDate = DateTime.Today;

            // 2 Change start date of the next auth (which is dept head) to tmr
            Authority deptHead = context.Authorities.First(a => a.EndDate == null);
            deptHead.StartDate = DateTime.Today.AddDays(1);
            context.SaveChanges();
        }

        //update old representative enddate and add new departmentRepresentative
        public void updateDepartmentRepresentative(int currentDeptRepId, string newRepEmpId)
        {         
            DepartmentRepresentative currentDeptRepresentative = context.DepartmentRepresentatives
                .Where(d => d.DeptRepID == currentDeptRepId).FirstOrDefault();
            if (currentDeptRepresentative != null)
                currentDeptRepresentative.EndDate = DateTime.Today;

            DepartmentRepresentative newDepRep = new DepartmentRepresentative();

            newDepRep.EmployeeID = newRepEmpId;
            String deptID=getDepartmentID(newDepRep.EmployeeID);
            newDepRep.StartDate = DateTime.Today.AddDays(1);            
            newDepRep.Passcode= generateNewPasscode(deptID);
            context.DepartmentRepresentatives.Add(newDepRep);
            context.SaveChanges();           
        }

        public void updateCollectionPoint(string dep, int cp)
        {
            Department department = context.Departments.Where(d => d.DepartmentID == dep).First();          
            department.CollectionPointID = cp;          
             context.SaveChanges();        
        }

        //To generate new passcode
        public string generateNewPasscode(string dep)
        {
            //DepartmentRepresentative depRep = context.DepartmentRepresentatives
            //    .First(dr => dr.Employee.DepartmentID == dep);
            DepartmentRepresentative currentRepresentative=getCurrentDepartmentRepresentative(dep);
            DepartmentRepresentative depRep = context.DepartmentRepresentatives.Where(x => x.DeptRepID == currentRepresentative.DeptRepID).First();
            Random num = new Random();
            int passcode = num.Next(1000,9999);
            return Convert.ToString(passcode);                
        }
    }
}
