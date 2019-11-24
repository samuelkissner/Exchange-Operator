using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeOperatorImplementation
{
    class Program
    {
        static void Main(string[] args)
        {
            //add sample data to the two database nodes
            new SampleRecords().createSampleRecords();


            //execute query on the two nodes, via the exchange operator, and combine the resuts
            String[] DatabaseConnectionIDs = { "Node1_DB", "Node2_DB" };
            ExchangeOperator xchg = new ExchangeOperator(DatabaseConnectionIDs);

            //display database nodes prior to partitioning
            string[] ToolTable_columnNames = { "Tool_ID", "ToolName", "Inventory_Quantity"};
            xchg.ExecuteQuery("select * from Tool",ToolTable_columnNames );
            Display.DisplayToolRecords("STATE OF NODE 1 TOOL RECORDS BEFORE PARTITIONING", xchg.QueryResults[0]);
            Display.DisplayToolRecords("STATE OF NODE 2 TOOL RECORDS BEFORE PARTITIONING", xchg.QueryResults[1]);
            string [] LineItemTable_columnNames = { "LineItem_ID", "Tool_ID", "Price", "Purchase_Quantity" };
            xchg.ExecuteQuery("select * from LineItem", LineItemTable_columnNames);
            Display.DisplayLineItemRecords("STATE OF NODE 1 LINE_ITEM RECORDS BEFORE PARTITIONING", xchg.QueryResults[0]);
            Display.DisplayLineItemRecords("STATE OF NODE 2 LINE_ITEM RECORDS BEFORE PARTITIONING", xchg.QueryResults[1]);

            //hash partition Tool records
            IRecordRepository<Tool> toolRepository = new ToolServiceRepository();
            xchg.HashPartition<Tool>(toolRepository);
            IRecordRepository<LineItem> lineItemRepository = new LineItemServiceRepository();
            xchg.HashPartition<LineItem>(lineItemRepository);

            //display database nodes after paritioning
            xchg.ExecuteQuery("select * from Tool", ToolTable_columnNames);
            Display.DisplayToolRecords("STATE OF NODE 1 TOOL RECORDS AFTER HASH PARTITIONING (H = TOOL_ID % N)", xchg.QueryResults[0]);
            Display.DisplayToolRecords("STATE OF NODE 2 TOOL RECORDS AFTER HASH PARTITIONING (H = TOOL_ID % N)", xchg.QueryResults[1]);
            xchg.ExecuteQuery("select * from LineItem", LineItemTable_columnNames);
            Display.DisplayLineItemRecords("STATE OF NODE 1 LINE_ITEM RECORDS AFTER HASH PARTITIONING (H = TOOL_ID % N)", xchg.QueryResults[0]);
            Display.DisplayLineItemRecords("STATE OF NODE 2 LINE_ITEM RECORDS AFTER HASH PARTITIONING (H = TOOL_ID % N)", xchg.QueryResults[1]);

            //execute query concurrently on two nodes
            string sql = "select * from Tool, LineItem where Tool.Tool_ID = LineItem.Tool_ID";
            string [] QueryResults_columnNames = { "LineItem_ID", "Tool_ID", "ToolName", "Price", "Purchase_Quantity", "Inventory_Quantity" };
            xchg.ExecuteQuery(sql, QueryResults_columnNames);
            Console.WriteLine("THE FOLLOWING QUERY IS BEING EXECUTED CONCURRENTLY ON THE TWO DATABASE NODES VIA SEPARATE THREADS: " + sql + "\n");
            
            //display separate results from the two nodes
            Display.DisplayQueryResults("NODE 1 RESULTS FROM QUERY", xchg.QueryResults[0]);
            Display.DisplayQueryResults("NODE 2 RESULTS FROM QUERY", xchg.QueryResults[1]);

            //combine the results and display
            List<string[]> CombinedResults = xchg.CombineQueryResults();
            Display.DisplayQueryResults("COMBINED RESULTS FROM QUERY", CombinedResults);


            Console.Read();
        }
    }

    class ExchangeOperator
    {
        //ids for the database connections listed in App.config
        string[] DatabaseConnectionIDs;
        //array of threads
        Thread[] Threads;
        //query results from the various partitions
        public List<string[]>[] QueryResults;
       
        //constructor
        public ExchangeOperator(string[] DatabaseConnectionIDs)
        {   
            this.DatabaseConnectionIDs = DatabaseConnectionIDs;
            this.QueryResults = new List<string[]>[DatabaseConnectionIDs.Length];
            this.Threads = new Thread[DatabaseConnectionIDs.Length];
        }

        //purpose - repartition data in nodes based on hash function h = id mod n
        public void HashPartition<T>(IRecordRepository<T> repository )
        {
            List<T> recordList = new List<T>();
            //retrieve all records from databases and add recordList. 
            foreach (string database_id in DatabaseConnectionIDs)
            {
                recordList.AddRange(repository.GetRecords<T>(database_id));
                repository.ClearTable<T>(database_id);
            }

            //number of nodes/databases
            int n = DatabaseConnectionIDs.Length;
            //go through each tool and add to appropirate partition/database/node

            foreach (T record in recordList)
            {
                int PartitionKey = GetPartitionKey<T>(record);
                repository.SaveRecord<T>(record, this.DatabaseConnectionIDs[ PartitionKey % n]);
            }
        }

        //purpose: return value of the key used to partition the record
        public static int GetPartitionKey<T>(T record)
        {
            //get parition key from record and distribute to the appropriate database
            var PartitionKey = typeof(T).GetProperty("Tool_ID").GetValue(record);
            return (int)PartitionKey;
        }

        //purpose: combine query results produced from different threads in a single data structure
        public List<string[]> CombineQueryResults()
        {
            List<string[]> CombinedQueryResults = new List<string[]>();
            foreach (List<string[]> QueryResult in this.QueryResults)
            {
                CombinedQueryResults.AddRange(QueryResult);
            }

            return CombinedQueryResults;
        }


        //purpose: partition data, then execute query (as a separate thread) on each parition
        //resuts:
        public void ExecuteQuery(string qry, string[] columnNames)
        {
            
         
            //create a separate thread to execute the query on each data partition
            for(int i = 0; i < this.DatabaseConnectionIDs.Length; i++)
            {
                int index = i;
                this.Threads[i]= new Thread(() => { QueryResults[index] = ExecuteQueryOnPartition(this.DatabaseConnectionIDs[index], qry, columnNames); });
                Threads[i].Start();
                
            }

            //wait for all the child threads to finish before continuing
            for (int i = 0; i < this.DatabaseConnectionIDs.Length; i++)
            {
                Threads[i].Join();
            }
        }

        //Purpose: execute a qry on partition and store the results
        // each parition will be executed in its own thread
        public List<string[]> ExecuteQueryOnPartition(string database_id, string qry, string[] columnNames)
        {
          return SqliteDataAccess.ExecuteQuery(database_id, qry, columnNames);
        }

        
        
    }
}
