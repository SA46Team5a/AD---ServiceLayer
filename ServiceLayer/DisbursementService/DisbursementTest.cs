using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DataAccess;

namespace ServiceLayer
{
    // Author: Jack
    class DisbursementTest
    {
        static StationeryStoreEntities context = StationeryStoreEntities.Instance;
        static IDisbursementService disbursementService = new DisbursementService();
        static IRequisitionService requisitionService = new RequisitionService();
        static IDepartmentService departmentService = new DepartmentService();
        static IStockManagementService stockManagementService = new StockManagementService();

        static void Main(string[] args)
        {
            clearTables();

            Console.WriteLine("-- Test Retrieval to disbursement process--");
            Console.WriteLine("Scenario 1 : Shortage in store, 2 disbursements made to complete the whole requisition");
            Console.WriteLine("Part 1: Newly approved requisition");
            Requisition req = setupRequisition(10);
            checkStock();
            List<RetrievalItem> retrievalItems = disbursementService.getRetrievalForm("E015");
            Console.WriteLine("1.1 Retrieval item count should be 3 : {0}", retrievalItems.Count());
            Console.WriteLine("1.2 Qty to retrieve for each item should be 10");
            retrievalItems.ForEach(r => Console.WriteLine(r.ItemID + "\t" + r.QtyToRetrieve + "\t" + r.QtyInStock));
            Console.WriteLine();

            Console.WriteLine("Part 2: Partially disbursed requisition");
            DisbursementDuty disDuty = disbursementService.getDisbursementDutyByStoreClerkEmpId("E015");
            Dictionary<string, int> itemsAndQtys = generateItemAndQtys(5);
            disbursementService.submitRetrievalForm(disDuty.DisbursementDutyID, itemsAndQtys);
            disbursementService.submitDisbursementOfDep(new List<int>() { disDuty.DisbursementDutyID}, "CHEM" , genDisbursementItems(5, 5, null), "E015");
            retrievalItems = disbursementService.getRetrievalForm("E015");
            Console.WriteLine("2.1 Retrieval item count should be 3 : {0}", retrievalItems.Count());
            Console.WriteLine("2.2 Qty to retrieve for each item should be 5, and stock count should decrease by 5");
            retrievalItems.ForEach(r => Console.WriteLine(r.ItemID + "\t" + r.QtyToRetrieve + "\t" + r.QtyInStock));
            Console.WriteLine();

            Console.WriteLine("Part 3: fully disbursed requisition");
            disDuty = disbursementService.getDisbursementDutyByStoreClerkEmpId("E015");
            disbursementService.submitRetrievalForm(disDuty.DisbursementDutyID, itemsAndQtys);
            disbursementService.submitDisbursementOfDep(new List<int>() { disDuty.DisbursementDutyID}, "CHEM" , genDisbursementItems(5, 5, null), "E015");
            retrievalItems = disbursementService.getRetrievalForm("E015");
            Console.WriteLine("3.1 Retrieval item count should be 0 : {0}", retrievalItems.Count());
            Console.WriteLine("3.2 Stock count should decrease by 5");
            retrievalItems.ForEach(r => Console.WriteLine(r.ItemID + "\t" + r.QtyToRetrieve + "\t" + r.QtyInStock));
            checkStock();
            Console.WriteLine();

            clearTables();

            Console.WriteLine("Scenario 2 : Dsbursement partially rejected at collection");
            Console.WriteLine("Part 1: Goods retrieved from store");
            req = setupRequisition(10);
            retrievalItems = disbursementService.getRetrievalForm("E015");
            Console.WriteLine("1.1 Retrieval item count should be 3 : {0}", retrievalItems.Count());
            Console.WriteLine("1.2 Qty to retrieve for each item should be 10");
            retrievalItems.ForEach(r => Console.WriteLine(r.ItemID + "\t" + r.QtyToRetrieve + "\t" + r.QtyInStock));
            Console.WriteLine();

            Console.WriteLine("Part 2: Goods rejected at collection");
            disDuty = disbursementService.getDisbursementDutyByStoreClerkEmpId("E015");
            disbursementService.submitRetrievalForm(disDuty.DisbursementDutyID, itemsAndQtys);
            checkStock();
            disbursementService.submitDisbursementOfDep(new List<int>() { disDuty.DisbursementDutyID}, "CHEM" , genDisbursementItems(10,5,"broken"), "E015");
            Console.WriteLine("2.1 Retrieval item count should be 3 : {0}", retrievalItems.Count());
            Console.WriteLine("2.2 Stock count should increase by 5");
            retrievalItems = disbursementService.getRetrievalForm("E015");
            retrievalItems.ForEach(r => Console.WriteLine(r.ItemID + "\t" + r.QtyToRetrieve + "\t" + r.QtyInStock));
            checkStock();
            Console.WriteLine();

            clearTables();
            Console.WriteLine("Scenario 3 : shortage in store");
            Console.WriteLine("Part 1: Goods retrieved from store");
            req = setupRequisition(10000);
            retrievalItems = disbursementService.getRetrievalForm("E015");
            Console.WriteLine("1.1 Retrieval item count should be 3 : {0}", retrievalItems.Count());
            Console.WriteLine("1.2 Qty to retrieve for each item should be 10000");
            retrievalItems.ForEach(r => Console.WriteLine(r.ItemID + "\t" + r.QtyToRetrieve + "\t" + r.QtyInStock));
            Console.WriteLine();

            Console.WriteLine("Part 2: Goods at collection");
            disDuty = disbursementService.getDisbursementDutyByStoreClerkEmpId("E015");
            itemsAndQtys["C001"] = stockManagementService.getStockCountOfItem("C001");
            itemsAndQtys["C002"] = stockManagementService.getStockCountOfItem("C002");
            itemsAndQtys["C003"] = stockManagementService.getStockCountOfItem("C003");
            disbursementService.submitRetrievalForm(disDuty.DisbursementDutyID, itemsAndQtys);
            checkStock();
            disbursementService.submitDisbursementOfDep(new List<int>() { disDuty.DisbursementDutyID}, "CHEM" , genDisbursementItems(0,0,"broken"), "E015");
            Console.WriteLine("2.1 Retrieval item count should be 3 : {0}", retrievalItems.Count());
            Console.WriteLine("2.2 Stock count should increase by 5");
            retrievalItems = disbursementService.getRetrievalForm("E015");
            retrievalItems.ForEach(r => Console.WriteLine(r.ItemID + "\t" + r.QtyToRetrieve + "\t" + r.QtyInStock));
            checkStock();
            Console.WriteLine();
        }

        static Requisition setupRequisition(int qty)
        {
            requisitionService.createNewRequsitionForEmployee("E026");
            Requisition req = requisitionService.getUnsubmittedRequisitionOfEmployee("E026");
            Dictionary<string, int> itemAndQty = generateItemAndQtys(qty);
            requisitionService.addNewRequisitionsDetails(req, itemAndQty);
            requisitionService.submitRequisition(req.RequisitionID);
            requisitionService.approveRequisition(req.RequisitionID, departmentService.getCurrentAuthority("CHEM").AuthorityID);
            return req;
        }

        static Dictionary<string, int> generateItemAndQtys(int qty)
        {
            Dictionary<string, int> itemAndQty = new Dictionary<string, int>();
            List<string> itemIds = new List<string>() { "C001", "C002", "C003" };
            itemIds.ForEach(id => itemAndQty.Add(id, qty));
            return itemAndQty;
        }

        static void checkStock()
        {
            Console.WriteLine("--- Check Stock ---");
            foreach (string item in new List<string> { "C001", "C002", "C003"} )
            {
                Console.WriteLine(item + " " + stockManagementService.getStockCountOfItem(item));
            }
            Console.WriteLine("-------------------");
        }

        static List<DisbursementItem> genDisbursementItems(int disbursedQty, int collectedQty, string reason)
        {
            List<string> itemIds = new List<string>() { "C001", "C002", "C003" };
            List<DisbursementItem> disbursementItems = new List<DisbursementItem>();
            itemIds.ForEach(id => disbursementItems.Add(
                new DisbursementItem(id, reason, disbursedQty, collectedQty)
                ));
            return disbursementItems;
        }

        static void clearTables()
        {
            // Clear tables for testing
            context.Requisitions.RemoveRange(context.Requisitions);
            context.RequisitionDetails.RemoveRange(context.RequisitionDetails);
            context.DisbursementDuties.RemoveRange(context.DisbursementDuties);
            context.Disbursements.RemoveRange(context.Disbursements);
            context.DisbursementDetails.RemoveRange(context.DisbursementDetails);
            context.SaveChanges();
        }
    }
}
