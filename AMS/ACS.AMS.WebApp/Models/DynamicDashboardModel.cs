using System.Data;

namespace ACS.AMS.WebApp.Models
{
    public class DashboardTableField
    {
        public string DashboardFieldName => FieldName.Replace(" ", "_");

        public string Expression { get; set; }

        public string FieldName { get; set; }

        public string Title { get; set; }

        public string FieldFormat { get; set; }

        public double Width { get; set; }

        public bool SumRequired { get; set; }

        public bool GroupSumRequired { get; set; }

        public Type FieldDataType { get; set; }

        public DashboardTableField()
        {
            Width = 1.0;
        }

        public string GetFieldExpression()
        {
            if (string.IsNullOrEmpty(Expression))
            {
                return $"=Fields!{DashboardFieldName}.Value";
            }

            return Expression;
        }
    }
    public class DashboardQueryResultField
    {
        public string DashboardFieldName { get; private set; }

        public string Name { get; set; }

        public Type DataType { get; set; }

        public object DefaultValue { get; set; }

        public DashboardQueryResultField(string name, Type dataType, object defaultValue = null)
        {
            Name = name;
            DataType = dataType;
            DefaultValue = defaultValue;
            DashboardFieldName = name.Replace(" ", "_");
        }
    }
    public class DashboardQueryResultFieldCollection : List<DashboardQueryResultField>
    {
    }

    public class DashboardTableFieldCollection : List<DashboardTableField>
    {
    }
   
    //public class RDLReport
    //{
    //    private Telerik.Reporting.Report report = new Telerik.Reporting.Report();

    //    private static double textboxHeight = 0.2;

    //    public ReportTableFieldCollection DetailSectionFields { get; private set; }

    //    public ReportTableGroupFieldCollection GroupSectionFields { get; private set; }

    //    public int FilterParametersCount { get; set; }

    //    public RDLReport()
    //    {
    //        DetailSectionFields = new ReportTableFieldCollection();
    //        GroupSectionFields = new ReportTableGroupFieldCollection();
    //        FilterParametersCount = 0;
    //    }

    //    public void PageSize(string width, string height)
    //    {
           
    //    }
    //}
 }

