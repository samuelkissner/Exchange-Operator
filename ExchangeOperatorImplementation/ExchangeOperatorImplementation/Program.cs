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
            
            SampleRecords sr = new SampleRecords();
            sr.RemoveSampleRecords("Node1_DB");
            sr.RemoveSampleRecords("Node2_DB");
            sr.AddSampleRecords("Node1_DB");
            sr.AddSampleRecords("Node2_DB");
            

            
            String[] DatabaseConnectionIDs = { "Node1_DB", "Node2_DB" };
            ExchangeOperator xchg = new ExchangeOperator(DatabaseConnectionIDs);
            //hash partition Tool records
            IRecordRepository<Tool> toolRepository = new ToolServiceRepository();
            xchg.HashPartition<Tool>(toolRepository);
            IRecordRepository<LineItem> lineItemRepository = new LineItemServiceRepository();
            xchg.HashPartition<LineItem>(lineItemRepository);
            xchg.ExecuteQuery("");
            
        }

    }

  
   
    class ExchangeOperator
    {
        //ids for the database connections listed in App.config
        string[] DatabaseConnectionIDs;
        //array of threads
        Thread[] Threads;
        //query results from the various partitions
        List<string[]>[] QueryResults;
       


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
          
            //create a separate thread to execute the query on each data partition
            for(int i = 0; i < this.DatabaseConnectionIDs.Length; i++)
            {
                int index = i;
                this.Threads[i]= new Thread(() => { QueryResults[index] = ExecuteQueryOnPartition(this.DatabaseConnectionIDs[index], qry); });
                Threads[i].Start();
                
            }

            //wait for all the child threads to finish before continuing
            for (int i = 0; i < this.DatabaseConnectionIDs.Length; i++)
            {
                Threads[i].Join();
            }

            Console.WriteLine("Donzo");
        }

        //Purpose: execute a qry on partition and store the results
        // each parition will be executed in its own thread
        public List<string[]> ExecuteQueryOnPartition(string database_id, string qry)
        {
          return SqliteDataAccess.ExecuteQuery(database_id, qry);
        }
        
    }
}
