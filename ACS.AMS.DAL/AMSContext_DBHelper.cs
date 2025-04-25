using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.AMS.DAL.DBContext
{
    public partial class AMSContext
    {
        public static int CommandTimeout { get; set; } = 60 * 2;

        public static SqlConnection GetSqlConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public static DataTable GetDataTable(string procedureName, Dictionary<string, object> parameters, bool isProcedure = true)
        {
            using (SqlConnection cn = GetSqlConnection())
            {
                var cmd = cn.CreateCommand();
                cmd.CommandText = procedureName;
                if (isProcedure)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    cmd.CommandType = CommandType.Text;
                }
                cmd.CommandTimeout = CommandTimeout;
                foreach (string key in parameters.Keys)
                {
                    if (key.StartsWith("@"))
                        cmd.Parameters.AddWithValue(key, parameters[key]);
                    else
                        cmd.Parameters.AddWithValue("@" + key, parameters[key]);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                var ds = new DataTable();
                da.Fill(ds);

                return ds;
            }
        }

    }
}
