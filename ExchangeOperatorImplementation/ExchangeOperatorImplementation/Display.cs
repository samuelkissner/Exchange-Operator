using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOperatorImplementation
{
    class Display
    {
        //purpose: display query results
        public static void DisplayQueryResults(string title, List<string[]> queryResults)
        {
            //string[] columnNames = { "LineItem_ID", "Tool_ID", "ToolName", "Price", "Purchase_Quantity", "Inventory_Quantity" };
            //string[] spacingInfo

            Console.WriteLine(title + "\n");
            Console.WriteLine("{0,9} {1,8} {2, 27} {3, 10:N2} {4, 9} {5,9}\n", "LnItm_ID", "Tool_ID", "ToolName", "Price", "Purch_Qty", "Inv_Qty");
            foreach (string[] row in queryResults)
                Console.WriteLine("{0,9} {1,8} {2, 27} {3, 10:N2} {4,9} {5,9}", row[0], row[1], row[2], row[3], row[4], row[5]);

            Console.WriteLine();
        }

        //purpose: display tool table records
        public static void DisplayToolRecords(string title, List<string[]> toolRecords)
        {
            Console.WriteLine(title + "\n");
            Console.WriteLine("{0,8} {1, 27} {2,9}\n", "Tool_ID", "ToolName", "Inv_Qty");
            foreach (string[] row in toolRecords)
                Console.WriteLine(" {0,8} {1, 27} {2,9}", row[0], row[1], row[2]);

            Console.WriteLine();
        }
        //purpose: display line item reocrds{
        public static void DisplayLineItemRecords(string title, List<string[]> lineItemRecords)
        {
            Console.WriteLine(title + "\n");
            Console.WriteLine("{0,9} {1,8} {2, 10:N2} {3, 9}\n", "LnItm_ID", "Tool_ID", "Price", "Purch_Qty");
            foreach (string[] row in lineItemRecords)
                Console.WriteLine("{0,9} {1,8} {2, 10:N2} {3,9} ", row[0], row[1], row[2], row[3]);

            Console.WriteLine();
        }

        
        

    }
}
