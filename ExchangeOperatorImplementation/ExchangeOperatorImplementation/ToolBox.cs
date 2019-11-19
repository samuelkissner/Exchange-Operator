using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOperatorImplementation
{
    class ToolBox
    {
        List<ToolModel> tools = new List<ToolModel>();

        public  ToolBox(String id)
        {
            //AddTools(id);
            LoadToolBox(id);
        }

        private void LoadToolBox(String id)
        {
            tools = SqliteDataAccess.LoadTool(id); 

            PrintToolBoxContents(tools);
        }

       

        public static void PrintToolBoxContents(List<ToolModel> tools)
        {
            foreach (ToolModel tool in tools)
            {
                Console.WriteLine(tool.ToolSummary);
               
            }

            Console.Read();
        }

        private void AddTools(String id)
        {
            if (id == "Node1_DB")
            {
                SqliteDataAccess.SaveTool(new ToolModel { ToolName = "Hammer", Quantity = 3 }, id);
                SqliteDataAccess.SaveTool(new ToolModel { ToolName = "Phillips-head Screwdriver", Quantity = 4 }, id);
                SqliteDataAccess.SaveTool(new ToolModel { ToolName = "Flat-head Screwdriver", Quantity = 1 }, id);
                SqliteDataAccess.SaveTool(new ToolModel { ToolName = "Allen Wrench", Quantity = 30 }, id);
                SqliteDataAccess.SaveTool(new ToolModel { ToolName = "Pliers", Quantity = 5 }, id);
            }
            else if (id == "Node2_DB")
            {
                SqliteDataAccess.SaveTool(new ToolModel { ToolName = "Level", Quantity = 5 }, id);
                SqliteDataAccess.SaveTool(new ToolModel { ToolName = "Saw", Quantity = 2 }, id);
                SqliteDataAccess.SaveTool(new ToolModel { ToolName = "Socket Wrench", Quantity = 0 }, id);
                SqliteDataAccess.SaveTool(new ToolModel { ToolName = "Drill", Quantity = 9 }, id);
                SqliteDataAccess.SaveTool(new ToolModel { ToolName = "Goggles", Quantity = 9 }, id);
            }
        }

    }

    public class ToolModel
    {
        public int ID { get; set; }
        public string ToolName { get; set; }
        public int Quantity { get; set; }

        public string ToolSummary
        {
            get
            {
                return $"{ID} {ToolName} {Quantity}";
            }
        }
    }
}
