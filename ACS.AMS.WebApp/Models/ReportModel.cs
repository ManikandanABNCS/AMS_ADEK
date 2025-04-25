using System.Data;

namespace ACS.AMS.WebApp.Models
{
    public class ReportModel
    {
        public string ReportName { get; set; }
        public List<Telerik.Reporting.Parameter> ParameterData { get; set; } = new List<Telerik.Reporting.Parameter>();

        //Not working for multiple param 
        //public string GetParameterHTML()
        //{
        //    string prm = "";

        //    foreach (var item in ParameterData)
        //    {
        //        prm = $"{item.Name}: '{item.Value}'";
        //    }

        //    return "{" + prm + "}";
        //}


        public string GetParameterHTML()
        {
            string prm = "";
            string Main = "{";
            foreach (var item in ParameterData)
            {
                prm = $"{item.Name}: '{item.Value}'";
                Main = Main + prm +',';
            }
            Main = Main + "}";
            return Main;
        }
    }
}

public class ParameterList
{  
    public string ParamName { get; set; }
    public string ParamValue { get; set; }

}
public class ReportOrderbyColumns
{
    public string OrderbyColumnValue { get; set; }
    public string OrderbyColumnName { get; set; }
    public string OrderbyType { get; set; }
}
public class QueryParameterCollection : List<QueryParameter>
{
}
public class QueryParameter
{
    public string Name { get; set; }

    public DbType DataType { get; set; }

    public object DefaultValue { get; set; }

    public QueryParameter(string name, DbType dataType, object defaultValue = null)
    {
        Name = name;
        DataType = dataType;
        DefaultValue = defaultValue;
    }
}

public enum ParameterDataType
{
    //
    // Summary:
    //     A Boolean data type that represents a true or false condition.
    Boolean,
    //
    // Summary:
    //     A DateTime data type that represents the date and time.
    DateTime,
    //
    // Summary:
    //     A Float data type that represents a floating point decimal value.
    Float,
    //
    // Summary:
    //     An Integer data type.
    Integer,
    //
    // Summary:
    //     A String data type that represents an array of characters.
    String
}