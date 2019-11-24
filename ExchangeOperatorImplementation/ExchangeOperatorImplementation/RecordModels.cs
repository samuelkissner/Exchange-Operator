using System;
using System.Collections.Generic;


namespace ExchangeOperatorImplementation
{
    class SampleRecords
    {
        IRecordRepository<Tool> repository = new ToolServiceRepository();
        IRecordRepository<LineItem> repository2 = new LineItemServiceRepository();
        

        public void RemoveSampleRecords(String database_id)
        {
            repository.ClearTable<Tool>(database_id);
            repository2.ClearTable<LineItem>(database_id);
        }

        public void AddSampleRecords(String database_id)
        {
            if (database_id == "Node1_DB")
            {
                //add sample Tool records to the first database
                repository.SaveRecord<Tool>(new Tool { Tool_ID = 1, ToolName = "Hammer", Inventory_Quantity = 3 }, database_id);
                repository.SaveRecord<Tool>(new Tool { Tool_ID = 2, ToolName = "Phillips-head Screwdriver", Inventory_Quantity = 4 }, database_id);
                repository.SaveRecord<Tool>(new Tool { Tool_ID = 3, ToolName = "Flat-head Screwdriver", Inventory_Quantity = 1 }, database_id);
                repository.SaveRecord<Tool>(new Tool { Tool_ID = 4, ToolName = "Allen Wrench", Inventory_Quantity = 30 }, database_id);
                repository.SaveRecord<Tool>(new Tool { Tool_ID = 5, ToolName = "Pliers", Inventory_Quantity = 5 }, database_id);

                //add sample LineItem records to the first database
                repository2.SaveRecord<LineItem>(new LineItem { LineItem_ID = 1, Tool_ID = 7, Price = 19.95, Purchase_Quantity = 3 }, database_id);
                repository2.SaveRecord<LineItem>(new LineItem { LineItem_ID = 2, Tool_ID = 2, Price = 1.50, Purchase_Quantity = 10 }, database_id);
                repository2.SaveRecord<LineItem>(new LineItem { LineItem_ID = 3, Tool_ID = 10, Price = 10.00, Purchase_Quantity = 1 }, database_id);
            }
            else if (database_id == "Node2_DB")
            {
                //add sample Tool records to the second database
                repository.SaveRecord<Tool>(new Tool { Tool_ID = 6, ToolName = "Level", Inventory_Quantity = 5 }, database_id);
                repository.SaveRecord<Tool>(new Tool { Tool_ID = 7, ToolName = "Saw", Inventory_Quantity = 2 }, database_id);
                repository.SaveRecord<Tool>(new Tool { Tool_ID = 8, ToolName = "Socket Wrench", Inventory_Quantity = 0 }, database_id);
                repository.SaveRecord<Tool>(new Tool { Tool_ID = 9, ToolName = "Drill", Inventory_Quantity = 9 }, database_id);
                repository.SaveRecord<Tool>(new Tool { Tool_ID = 10, ToolName = "Goggles", Inventory_Quantity = 9 }, database_id);

                //add sample LineItem records to the second database
                repository2.SaveRecord<LineItem>(new LineItem { LineItem_ID = 4, Tool_ID = 5, Price = 3.03, Purchase_Quantity = 5 }, database_id);
                repository2.SaveRecord<LineItem>(new LineItem { LineItem_ID = 5, Tool_ID = 4, Price = 4.87, Purchase_Quantity = 33 }, database_id);
                repository2.SaveRecord<LineItem>(new LineItem { LineItem_ID = 6, Tool_ID = 3, Price = 2.01, Purchase_Quantity = 2 }, database_id);
            }

        }

    }

    public interface IRecordRepository<T>
    {
       
        List<T> GetRecords<T>(string database_id);
        void SaveRecord<T>(T record, string database_id);
        //purpose: remove all records from a table.
        void ClearTable<T>(string id);
    }

    public class ToolServiceRepository : IRecordRepository<Tool>
    {
     

        public List<Tool> GetRecords<Tool>(string database_id)
        {
            return SqliteDataAccess.LoadRecords<Tool>(database_id, "select * from Tool");
        }
        public void SaveRecord<Tool>(Tool record, string database_id)
        {
            SqliteDataAccess.SaveRecord(record, database_id, "insert into Tool (Tool_ID,ToolName, Inventory_Quantity) values (@Tool_ID, @ToolName, @Inventory_Quantity)");
        }
        //purpose: remove all records from a table.
        public void ClearTable<Tool>(string database_id)
        {
            SqliteDataAccess.ClearTable(database_id, "delete from Tool");
        }
    }

    public class LineItemServiceRepository : IRecordRepository<LineItem>
    {
       

        public List<LineItem> GetRecords<LineItem>(string database_id)
        {
            return SqliteDataAccess.LoadRecords<LineItem>(database_id, "select * from LineItem");
        }
        public void SaveRecord<LineItem>(LineItem record, string database_id)
        {
            SqliteDataAccess.SaveRecord(record, database_id, "insert into LineItem (LineItem_ID, Tool_ID,Price, Purchase_Quantity) values (@LineItem_ID, @Tool_ID, @Price, @Purchase_Quantity)");
        }
        //purpose: remove all records from a table.
        public void ClearTable<LineItem>(string database_id)
        {
            SqliteDataAccess.ClearTable(database_id, "delete from LineItem");
        }
    }



    public class Tool 
    {
        public int Tool_ID { get; set; }
        public string ToolName { get; set; }
        public int Inventory_Quantity { get; set; }

      
       

        public string Summary
        {
            get
            {
                return $"{Tool_ID} {ToolName} {Inventory_Quantity}";
            }
        }
    }
    
    public class LineItem { 


        public int LineItem_ID { get; set; }
        public int Tool_ID { get; set; }
        public double Price { get; set; }
        public int Purchase_Quantity { get; set; }


        public string Summary
        {
            get
            {
                return $"{LineItem_ID} {Tool_ID} {Price} {Purchase_Quantity}";
            }
        }
    }
}
