using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
            xchg.ExecuteQuery("select * from Tool");
            
        }

    }

  
   
    class ExchangeOperator
    {
        //ids for the database connections listed in App.config
        String[] DatabaseConnectionIDs;
        //array of threads
        Thread[] Threads;
        //query results from the various partitions
        List<ToolModel>[] QueryResults;
        //temporary data structure to hold all records retrieved from the various databases (the nodes) prior to repartition
        List<ToolModel> tools = new List<ToolModel>();


        //constructor
        public ExchangeOperator(String[] DatabaseConnectionIDs)
        {   
            this.DatabaseConnectionIDs = DatabaseConnectionIDs;
            this.QueryResults = new List<ToolModel>[DatabaseConnectionIDs.Length];
            this.Threads = new Thread[DatabaseConnectionIDs.Length];

            //retrieve all records from databases and add to tools. 
            foreach (String DBConnectionID in DatabaseConnectionIDs)
            {
                tools.AddRange(SqliteDataAccess.LoadTool(DBConnectionID));
                SqliteDataAccess.ClearTable(DBConnectionID);
            }

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

        
        //purpose - repartition data in nodes based on data range
        public void RangePartition()
        {
           

        }
        //purpose - copy data from one node to other nodes
        public void Broadcasting()
        {

        }

        //purpose - send data from all nodes to a single node
        public void SendAll()
        {

        }

        //purpose: partition data, then execute query (as a separate thread) on each parition
        //resuts:
        public void ExecuteQuery(string qry)
        {
            //partition data
            HashPartition();
            for(int i = 0; i < this.DatabaseConnectionIDs.Length; i++)
            {
                int index = i;
                this.Threads[i]= new Thread(() => { QueryResults[index] = ExecuteQueryOnPartition(this.DatabaseConnectionIDs[index], qry); });
                Threads[i].Start();
                
            }

            for (int i = 0; i < this.DatabaseConnectionIDs.Length; i++)
            {
              
                Threads[i].Join();

            }

            Console.WriteLine("Donzo");
        }

        //Purpose: execute a qry on partition and store the results
        // each parition will be executed in its own thread
        public List<ToolModel> ExecuteQueryOnPartition(string id, string qry)
        {

          return SqliteDataAccess.ExecuteQuery(id, qry);
            
        }

    }
    
}
