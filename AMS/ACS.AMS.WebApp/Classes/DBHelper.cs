using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.AMS.WebApp.Classes
{
    public class DBHelper
    {
        private static string ConnectionString;
        private static Dictionary<string, Type> DataTypeMappings;

        static DBHelper()
        {
            ConnectionString = AMSContext.ConnectionString;

            DataTypeMappings = new Dictionary<string, Type>();
            DataTypeMappings.Add("bigint", typeof(Int64));
            DataTypeMappings.Add("binary", typeof(Byte[]));
            DataTypeMappings.Add("bit", typeof(Boolean));
            DataTypeMappings.Add("char", typeof(String));
            DataTypeMappings.Add("date", typeof(DateTime));
            DataTypeMappings.Add("datetime", typeof(DateTime));
            DataTypeMappings.Add("datetime2", typeof(DateTime));
            DataTypeMappings.Add("datetimeoffset", typeof(DateTimeOffset));
            DataTypeMappings.Add("decimal", typeof(Decimal));
            DataTypeMappings.Add("float", typeof(Double));
            DataTypeMappings.Add("image", typeof(Byte[]));
            DataTypeMappings.Add("int", typeof(Int32));
            DataTypeMappings.Add("money", typeof(Decimal));
            DataTypeMappings.Add("nchar", typeof(String));
            DataTypeMappings.Add("ntext", typeof(String));
            DataTypeMappings.Add("numeric", typeof(Decimal));
            DataTypeMappings.Add("nvarchar", typeof(String));
            DataTypeMappings.Add("real", typeof(Single));
            DataTypeMappings.Add("rowversion", typeof(Byte[]));
            DataTypeMappings.Add("smalldatetime", typeof(DateTime));
            DataTypeMappings.Add("smallint", typeof(Int16));
            DataTypeMappings.Add("smallmoney", typeof(Decimal));
            DataTypeMappings.Add("text", typeof(String));
            DataTypeMappings.Add("time", typeof(TimeSpan));
            DataTypeMappings.Add("timestamp", typeof(Byte[]));
            DataTypeMappings.Add("tinyint", typeof(Byte));
            DataTypeMappings.Add("uniqueidentifier", typeof(Guid));
            DataTypeMappings.Add("varbinary", typeof(Byte[]));
            DataTypeMappings.Add("varchar", typeof(String));
        }

        public static DBObjectSchema GetAllFields(string queryString, string queryType)
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = cn.CreateCommand())
                {
                    cn.Open();

                    List<SchemaFieldInfo> objectParameters = new List<SchemaFieldInfo>();
                    command.CommandTimeout = 300;
                    if (queryType == "View")
                    {
                        command.CommandText = "SELECT * FROM " + queryString;
                        command.CommandType = CommandType.Text;
                    }
                    else
                    {
                        command.CommandText = queryString;
                        command.CommandType = CommandType.StoredProcedure;

                        //Pass null for all its parameters
                        {
                            var parameterCommand = cn.CreateCommand();

                            parameterCommand.CommandText = "SELECT PARAMETER_NAME,Data_Type DataType FROM INFORMATION_SCHEMA.PARAMETERS WHERE SPECIFIC_NAME = @ProcedureName ORDER BY PARAMETER_NAME";
                            parameterCommand.CommandType = CommandType.Text;

                            parameterCommand.Parameters.AddWithValue("@ProcedureName", queryString);

                            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(parameterCommand);
                            DataTable parameters = new DataTable();

                            sqlDataAdapter.Fill(parameters);

                            foreach (DataRow r in parameters.Rows)
                            {
                                command.Parameters.AddWithValue(r["PARAMETER_NAME"] + "", DBNull.Value);

                                objectParameters.Add(new SchemaFieldInfo()
                                {
                                    Name = (r["PARAMETER_NAME"] + "").Replace("@", ""),
                                    DataType = DataTypeMappings["" + r["DataType"]]
                                });
                            }
                        }
                    }

                    SqlDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly);
                    DataTable tbl = reader.GetSchemaTable();

                    List<SchemaFieldInfo> objectSchemas = new List<SchemaFieldInfo>();

                    //sort the columns by column name
                    var sortedTbl = new DataView(tbl, "", "columnName", DataViewRowState.CurrentRows).ToTable();
                    foreach (DataRow r in sortedTbl.Rows)
                    {
                        objectSchemas.Add(new SchemaFieldInfo()
                        {
                            Name = r["columnName"] + "",
                            DataType = (Type)r["DataType"]
                        });
                    }

                    DBObjectSchema dBObjectSchema = new DBObjectSchema()
                    {
                        Parameters = objectParameters,
                        Columns = objectSchemas
                    };

                    return dBObjectSchema;
                }
            }
        }
    }

    public class DBObjectSchema
    {
        public List<SchemaFieldInfo> Columns { get; set; }

        public List<SchemaFieldInfo> Parameters { get; set; }
    }

    public class SchemaFieldInfo
    {
        public string Name { get; set; }

        public Type DataType { get; set; }
    }
}
