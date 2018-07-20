using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    class DepartmentTest
    {
        static void Main(string[] args)
        {
            IDepartmentService IdepartmentService = new DepartmentService();

            //Employee e = IdepartmentService.getEmployeeById("E017");
            //Console.WriteLine("Add authority");
            //IdepartmentService.addAuthority(e, Convert.ToDateTime("19/7/2018 2:11:35 PM"), Convert.ToDateTime("24/7/2018 2:11:35 PM"));
            //Console.WriteLine("getCollectionPointOfEmployee");
            //Console.WriteLine(IdepartmentService.getCollectionPointOfEmployee("E002").CollectionPointDetails);
            //Console.WriteLine("getDepartmentID");
            //Console.WriteLine(IdepartmentService.getDepartmentID("E004"));
            //Console.WriteLine("getEmployeeById");
            //Console.WriteLine(IdepartmentService.getEmployeeById("E010").EmployeeName);
            //Console.WriteLine("getcurrentAuthority");
            //Console.WriteLine(IdepartmentService.getCurrentAuthority("CHEM").EmployeeID);
            //Console.WriteLine("getcurrentRep");
            //Console.WriteLine(IdepartmentService.getCurrentDepartmentRepresentative("CHEM").EmployeeID);
            //Console.WriteLine("getpasscode");
            //IdepartmentService.generateNewPasscode("CHEM");
            //Console.WriteLine("verifyPassCode");
            //IdepartmentService.verifyPassCode("2222", "CHEM");
            //Authority a = new Authority();
            //a.AuthorityID = 17;
            //a.EmployeeID = "E017";
            //a.StartDate = Convert.ToDateTime("19/7/2018 2:11:35 PM");
            //a.EndDate = Convert.ToDateTime("20/7/2018 2:11:35 PM");
            //Console.WriteLine("updateAuthority");
            //IdepartmentService.updateAuthority(a);
            //Console.WriteLine("updateDepartmentRepresentative");
            //IdepartmentService.updateDepartmentRepresentative(16, "E020");
            //Console.WriteLine("rescindAuthority");
            //IdepartmentService.rescindAuthority(IdepartmentService.getCurrentAuthority("CHEM"));


            CollectionPoint c = new CollectionPoint();
            Console.WriteLine("updateCollectionPoint");
            IdepartmentService.updateCollectionPoint("ARCH", 2);









        }
    }
}
