using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using NLog;

namespace AliseCofeemaker.Modules
{
    public interface IDBProcCaller
    {
        SqlConnection connection { get; set; }
        DataSet Call(string proc, Dictionary<string, object> parameters = null);
    }

    public class DBProcCaller : IDBProcCaller
    {
        private SqlConnection conn;

        public DBProcCaller()
        {
            conn = new SqlConnection();
            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
            sb.InitialCatalog = "master";
            sb.DataSource = "(local)/SQLEXPRESS";
            sb.UserID = "sa";
            sb.Password = "123";
            conn.ConnectionString = sb.ConnectionString;
            try
            {
                conn.Open();
            } catch (Exception e)
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Debug(e.Message);
            }
        }

        public SqlConnection connection { get => conn; set => conn = value; }


        public DataSet Call(string proc, Dictionary<string, object> parameters = null)
        {
            var command = new SqlCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "CofeeMashineStatus";
            command.Connection = conn;
            var dataAdapter = new SqlDataAdapter(command);
            var dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            return dataSet;
        }
    }
}
