using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;
namespace ServiceLayer
{
    //Author: Benedict
    class RequisitionTest
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing ");
            IRequisitionService rService = new RequisitionService();

            Console.WriteLine("Testing Requisition getUnsubmittedRequisitionOfEmployee(string empId)");
            Requisition r1a = rService.getUnsubmittedRequisitionOfEmployee("E035");
            Console.WriteLine("The reqID of E035 is {0}", r1a.RequisitionID);
            Requisition r1b = rService.getUnsubmittedRequisitionOfEmployee("E026");
            Console.WriteLine("The reqID of E026 is {0}", r1b.RequisitionID);
            //expected new reqID of E026 is 92.

            Console.WriteLine("Testing Requisition getRequisitionById(int reqId)");
            Requisition r2 = rService.getRequisitionById(72);
            Console.WriteLine("The reqID of E035 is {0}", r2.RequisitionID);

            Console.WriteLine("Testing List<Requisition> getRequisitionsOfEmployee(string empId)");
            List<Requisition> rlist1 = rService.getRequisitionsOfEmployee("E035");
            Console.WriteLine("The requisitions of E035 are");
            foreach (Requisition r in rlist1)
            {
                Console.WriteLine("{0}", r.RequestedDate);
            }

            Console.WriteLine("Testing List<Requisition> getPendingRequisitionsOfDep(string depId)");
            List<Requisition> rlist2 = rService.getPendingRequisitionsOfDep("CPSC");
            Console.WriteLine("The pending requisitions of department CPSC are");
            foreach (Requisition r in rlist2)
            {
                Console.WriteLine("{0}", r.RequestedDate);
            }

            Console.WriteLine("Testing void addNewRequisitionDetail(int reqId, string itemId, int qtyRequested)");
            rService.addNewRequisitionDetail(14, "R002" , 2);
            //check RequisitionDetailsTable entry 271 // no additional row added on each test run

            Console.WriteLine("Testing void editRequisitionDetailQty(int reqDetailId, int qty)");
            rService.editRequisitionDetailQty(271, 5);
            //check RequisitionDetailsTable entry 271 // expected qty = 5

            Console.WriteLine("Testing void submitRequisition(int reqId, int authId)");
            rService.submitRequisition(90);
            //check RequisitionsTable entry 90/91 // expected approvalStatus = 2

            Console.WriteLine("Testing void approveRequisition(int reqId, int authId)");
            rService.approveRequisition(90, 12);
            //check RequisitionsTable entry 90 // expected approvalStatus = 3 with dateApprove, AuthorityID

            Console.WriteLine("Testing void rejectRequisition(int reqId, int authId");
            rService.rejectRequisition(90, 12);
            //check RequisitionsTable entry 90 // expected approvalStatus = 4 with dateApprove, AuthorityID

            Console.WriteLine("Testing void deleteRequisition(int reqId)");
            rService.deleteRequisition(90);
            //check RequisitionsTable entry 90 // expected approvalStatus = 4 with dateApprove, AuthorityID

            Console.WriteLine("Testing void deleteRequisitionDetail(int reqDetailId)");
            rService.deleteRequisitionDetail(271);
            //check RequisitionDetailsTable // remove entry 271
        }
    }
}
