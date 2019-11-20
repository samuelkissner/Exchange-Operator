using System;
using System.Collections.Generic;

namespace ExchangeOperatorImplementation
{
    class Program
    {
        static void Main(string[] args)
        {
            //ToolBox tb1 = new ToolBox("Node1_DB");
            //ToolBox tb2 = new ToolBox("Node2_DB");

            
            String[] DatabaseConnectionIDs = { "Node1_DB", "Node2_DB" };
            ExchangeOperator xchg = new ExchangeOperator(DatabaseConnectionIDs);
            xchg.HashPartition();
           
        }

    }

  
   
    class ExchangeOperator
    {
        //ids for the database connections listed in App.config
        String[] DatabaseConnectionIDs;
        //temporary data structure to hold all records retrieved from the various databases (the nodes) prior to repartition
        List<ToolModel> tools = new List<ToolModel>();

        //constructor
        public ExchangeOperator(String[] DatabaseConnectionIDs)
        {   
            this.DatabaseConnectionIDs = DatabaseConnectionIDs;

            //retrieve all records from databases and add to tools. 
            foreach (String DBConnectionID in DatabaseConnectionIDs)
            {
                tools.AddRange(SqliteDataAccess.LoadTool(DBConnectionID));
                SqliteDataAccess.ClearTable(DBConnectionID);
            }

            ToolBox.PrintToolBoxContents(tools);

        }

        //purpose - repartition data in nodes based on hash function h = id mod n
        public void HashPartition()
        {
            //number of nodes/databases
            int n = DatabaseConnectionIDs.Length;
            //go through each tool and add to appropirate partition/database/node
            foreach (ToolModel tool in tools)
            {
                SqliteDataAccess.SaveTool(tool, this.DatabaseConnectionIDs[tool.ID % n]);
            }
        }

        /*
        //purpose - repartition data in nodes based on data range
        public void RangePartition(s)
        {
            //add all node data to a single temporary list
            foreach (Node node in nodes)
                temp.AddRange(node.data);

            //sort the list based on the partitioning attribute
            temp.Sort();

            //determine the ideal distribution of data between nodes (to avoid skew)
            int idealDistribution = temp.Count / nodes.Length;

            //partition data evenly between each node
            foreach (Node node in nodes) {
                node.data.Clear();

            }
        }

      */
        //purpose - copy data from one node to other nodes
        public void Broadcasting()
        {

        }

        //purpose - send data from all nodes to a single node
        public void SendAllData()
        {

        }


    }
    
}
