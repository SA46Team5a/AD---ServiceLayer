using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    public class DepartmentPayload
    {
        public string DepartmentID { get; set; }
        public string DepartmentName { get; set; }

        public DepartmentPayload() { }
        public DepartmentPayload(Department d)
        {
            DepartmentID = d.DepartmentID;
            DepartmentName = d.DepartmentName;
        }

        public static List<DepartmentPayload> ConvertEntityToPayload(List<Department> categories)
        {
            List<DepartmentPayload> payload = new List<DepartmentPayload>();
            categories.ForEach(c => payload.Add(new DepartmentPayload(c)));
            return payload;
        }
    }

    public class EmployeePayload
    {
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public EmployeePayload() { }
        public EmployeePayload(Employee e)
        {
            EmployeeID = e.EmployeeID;
            EmployeeName = e.EmployeeName;
            DepartmentName = e.Department.DepartmentName;
            Email = e.EmailID;
            PhoneNumber = e.PhoneNumber;
        }

        public static List<EmployeePayload> ConvertEntityToPayload(List<Employee> employees)
        {
            List<EmployeePayload> payload = new List<EmployeePayload>();
            employees.ForEach(e => payload.Add(new EmployeePayload(e)));
            return payload;
        }
    }

    public class AuthorityPayload
    {
        public int AuthorityID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public AuthorityPayload() { }
        public AuthorityPayload(Authority a)
        {
            AuthorityID = a.AuthorityID;
            EmployeeID = a.EmployeeID;
            EmployeeName = a.Employee.EmployeeName;
            StartDate = a.StartDate;
            EndDate = a.EndDate;
        }
    }

    public class DepartmentRepresentativePayload
    {
        public int DeptRepID { get; set; }
        public string EmployeeName { get; set; }

        public DepartmentRepresentativePayload() { }
        public DepartmentRepresentativePayload(DepartmentRepresentative d)
        {
            DeptRepID = d.DeptRepID;
            EmployeeName = d.Employee.EmployeeName;
        }

        public static DepartmentRepresentativePayload ConvertToDepartmentRepresentativePayload(DepartmentRepresentative d)
        {
            if (d != null)
                return new DepartmentRepresentativePayload(d);
            else
                return null;
        }
    }
}
