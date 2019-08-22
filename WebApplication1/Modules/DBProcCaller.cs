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
            //подключение к базе данных
            conn = new SqlConnection();

            //формируется строка подключения
            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
            sb.InitialCatalog = "master";
            sb.DataSource = "(local)/SQLEXPRESS";
            sb.UserID = "sa";
            sb.Password = "123";
            conn.ConnectionString = sb.ConnectionString;

            //проверка подключения
            try
            {
                conn.Open();
                conn.Close();
            } catch (Exception e)
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Debug(e.Message);
            }
        }

        //
        public SqlConnection connection { get => conn; set => conn = value; }


        /// <summary>
        /// Вызов хранимой процедуры
        /// </summary>
        /// <param name="proc">Имя хранимой процедуры</param>
        /// <param name="parameters">список параметров параметр-значение</param>
        /// <returns>DataSet Результат выполнения хранимой процедуры</returns>
        public DataSet Call(string proc, Dictionary<string, object> parameters = null)
        {
            conn.Open();
            var command = new SqlCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = proc;
            command.Connection = conn;

            if (parameters != null)
            {
                foreach (var parameter in parameters.Keys)
                {
                    command.Parameters.AddWithValue(parameter, parameters[parameter]);
                }
            }

            var dataAdapter = new SqlDataAdapter(command);
            var dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            conn.Close();
            return dataSet;
        }
    }
}
