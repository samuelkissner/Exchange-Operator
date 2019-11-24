using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOperatorImplementation
{
    public class SqliteDataAccess
    {


        public static List<string[]> ExecuteQuery(string database_id, string sql)
        {
            List<string[]> resultList = new List<string[]>();
            
            using (SQLiteConnection cnn = new SQLiteConnection(LoadConnectionString(database_id)))
            {
                cnn.Open();
                using (SQLiteCommand fmd = cnn.CreateCommand())
                {
                    fmd.CommandText = "select * from Tool, LineItem where Tool.Tool_ID = LineItem.Tool_ID";
                    fmd.CommandType = CommandType.Text;
                    SQLiteDataReader r = fmd.ExecuteReader();
                    while (r.Read())
                    {

                        string[] row = new string[6];
                        row[0] = r["LineItem_ID"].ToString();
                        row[1] = r["Tool_ID"].ToString();
                        row[2] = r["ToolName"].ToString();
                        row[3] = r["Price"].ToString();
                        row[4] = r["Purchase_Quantity"].ToString();
                        row[5] = r["Inventory_Quantity"].ToString();
                        resultList.Add(row);
                    }
                }
            }
            return resultList;
        }
                
            
            

            

        public static List<T> LoadRecords<T>(string database_id, string sql)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString(database_id)))
            {
                var output = cnn.Query<T>(sql, new DynamicParameters());
                return output.ToList();
            }
        }

        public static void SaveRecord<T>(T record, string database_id, string sql)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString(database_id)))
            {
                cnn.Execute(sql, record);
            }
        }

        private static string LoadConnectionString(string id)
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        //purpose: remove all records from a table.
        public static void ClearTable(string id, string sql)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString(id)))
            {
                cnn.Execute(sql, new DynamicParameters());
            }
        }
    }
}
