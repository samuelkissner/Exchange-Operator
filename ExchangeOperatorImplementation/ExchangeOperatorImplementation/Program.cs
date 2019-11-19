using System;
using System.Collections.Generic;

namespace ExchangeOperatorImplementation
{
    class Program
    {
        static void Main(string[] args)
        {
            String[] DatabaseConnectionIDs = { "Node1_DB", "Node2_DB" };
            ExchangeOperator xcgh = new ExchangeOperator(DatabaseConnectionIDs);

        }

    }

  
   
    class ExchangeOperator
    {
    
        String[] DatabaseConnectionIDs;
        List<ToolModel> tools = new List<ToolModel>();


        public ExchangeOperator(String[] DatabaseConnectionIDs)
        {
            this.DatabaseConnectionIDs = DatabaseConnectionIDs;

            foreach (String DBConnectionID in DatabaseConnectionIDs)
            {
                tools.AddRange(SqliteDataAccess.LoadTool(DBConnectionID));
            }

            ToolBox.PrintToolBoxContents(tools);

        }

        //purpose - repartition data in nodes based on hash function   
        public void HashPartition()
        {

     
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
