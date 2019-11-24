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

        public static List<string[]> ExecuteQuery(string database_id, string sql, string[] columnNames)
        {
            List<string[]> resultList = new List<string[]>();
            
            using (SQLiteConnection cnn = new SQLiteConnection(LoadConnectionString(database_id)))
            {
                cnn.Open();
                using (SQLiteCommand fmd = cnn.CreateCommand())
                {
                    fmd.CommandText =sql;
                    fmd.CommandType = CommandType.Text;
                    SQLiteDataReader r = fmd.ExecuteReader();

                    //add column names to list
                    //resultList.Add(columnNames);

                    //retrieve row data and add to list
                    while (r.Read())
                    {

                        string[] row = new string[columnNames.Length];
                        for(int i = 0; i < columnNames.Length; i++)
                        {
                            row[i] = r[columnNames[i]].ToString();
                        }

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
