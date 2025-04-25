using ACS.AMS.DAL;
using ACS.AMS.DAL.DBContext;
using ACS.AMS.DAL.DBModel;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.AMS.WebApp
{
    public class DynamicReportHelper
    {
        private static SqlConnection GetConnection()
        {
            return new SqlConnection(AMSContext.ConnectionString);
        }
        public static QueryParameterCollection GetProcedureParameters(string procedureName)
        {
            using SqlConnection sqlConnection = GetConnection();
            using SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlConnection.Open();
            sqlCommand.CommandText = procedureName;
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            SqlCommandBuilder.DeriveParameters(sqlCommand);
            QueryParameterCollection queryParameterCollection = new QueryParameterCollection();
            foreach (SqlParameter parameter in sqlCommand.Parameters)
            {
                if (parameter.Direction == ParameterDirection.Input)
                {
                    queryParameterCollection.Add(new QueryParameter(parameter.ParameterName, parameter.DbType));
                }
            }

            return queryParameterCollection;
        }

        string[] dynamicQuery1 = null;
        public static object GetParameterValue(IFormCollection Request,string name, ParameterDataType dataType)
        {
            string res = "";
            string value = Request[name];
            if (string.IsNullOrEmpty(value) && name.Equals("query", StringComparison.OrdinalIgnoreCase))
            {
                value = Request["ReportTemplate"];
            }

            switch (name.ToUpper())
            {
               
                case "DATEFORMAT":
                    return (CultureHelper.ConfigureDateFormat);

                case "DATETIMEFORMAT":
                    return (CultureHelper.DateTimeFormat);
            }

            if (value == "<--All-->") return DBNull.Value;

            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    if (dataType == ParameterDataType.DateTime)
                        return DateTime.ParseExact(value, CultureHelper.ConfigureDateFormat, CultureInfo.InvariantCulture);

                    if (string.Equals(name, "fromdate", StringComparison.OrdinalIgnoreCase) || string.Equals(name, "todate", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(value))
                        {
                            value = null;
                        }
                        else
                        {
                            return DateTime.ParseExact(value, CultureHelper.ConfigureDateFormat, CultureInfo.InvariantCulture);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //throw new Exception("Invalid Date");                   
                }

                switch (name.ToUpper())
                {
                    case "ITEMCODE":
                        return value;
                    case "ITEMID":
                        return value;
                }
                return value;
            }
            else
                return DBNull.Value;
        }


    }
}
