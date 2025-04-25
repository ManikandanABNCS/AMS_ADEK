using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ACS.AMS.ServicePlugin
{
    public static class DBHandler
    {
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString);
        }

        public static DataTable GetDataTable(string queryText, bool isProcedure, Dictionary<string, object> parameters)
        {
            using (SqlConnection cn = GetConnection())
            {
                return GetDataTable(cn, queryText, isProcedure, parameters);
            }
        }

        public static DataTable GetDataTable(SqlConnection cn, string queryText, bool isProcedure, Dictionary<string, object> parameters)
        {
            SqlCommand cmd = new SqlCommand(queryText, cn);
            if (isProcedure)
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> param in parameters)
                {
                    if (param.Key.StartsWith("@"))
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    else
                        cmd.Parameters.AddWithValue("@" + param.Key, param.Value);
                }
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable tbl = new DataTable();

            da.Fill(tbl);

            return tbl;
        }
    }
}
