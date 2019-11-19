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
        public static List<ToolModel> LoadTool(string id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString(id)))
            {
                var output = cnn.Query<ToolModel>("select * from Tool", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void SaveTool(ToolModel tool, string id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString(id)))
            {
                cnn.Execute("insert into Tool (ToolName, Quantity) values (@ToolName, @Quantity)", tool);
            }
        }

        private static string LoadConnectionString(string id)
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
