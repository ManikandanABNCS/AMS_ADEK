using ACS.AMS.DAL;
using ACS.AMS.DAL.DBContext;
using ACS.AMS.DAL.DBModel;
using ACS.AMS.WebApp.Models;
using ACS.WebAppPageGenerator.Models.SystemModels;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
//using Telerik.SvgIcons;

namespace ACS.AMS.WebApp
{
    public class DashboardPageHelper
    {
        private DashboardQueryResultFieldCollection fieldList;
        IFormCollection ReportRequest;
        private List<DashboardOrderbyColumns> sortbyFields;
        //  private DashboardQueryResultFieldCollection fieldList;
        public DashboardPageHelper(DashboardPageModel model)
        {
            Model = model;
        }
        public DashboardSeriesModel series { get; set; }
        public string ChartName { get; set; }
        public string ChartTitle { get; set; }
        public DashboardPageModel Model { get; set; }

        public void AddChart(IHtmlHelper<DashboardPageModel> htmlHelper, RazorPage page, DashboardMappingTable item, System.Data.DataTable dt, IQueryable<DashboardFieldMappingTable> fieldDetails)
        {
           
           
            var htmlAttr = new { maxlength = 100, style = "width: 100%;" };
            //var lst = dt.AsEnumerable()
            //              .Select(r => r.Table.Columns.Cast<DataColumn>()
            //              .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
            //                ).ToDictionary(z => z.Key, z => z.Value)
            //                ).ToList();
            var list = fieldDetails.FirstOrDefault();
            

            switch (item.DashboardType.DashboardType)
            {
               
                case "PieChart":
                    page.WriteLiteral($"<div class=\"col-xl-6\">");
                    var lst = dt.AsEnumerable()
                                  .Select(r => r.Table.Columns.Cast<DataColumn>()
                                  .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
                                    ).ToDictionary(z => z.Key, z => z.Value)
                                    ).ToList();
                    page.Write(htmlHelper.Kendo().Chart(lst)
                              .Name(item.DashboardTemplate.DashboardTemplateName)
                                .Title(t => t.Text(item.DasboardMappingName).Position(ChartTitlePosition.Top))
                                .Series(series => series.Donut(list.YAxisField, list.XAxisField)
                                .Labels(labels => labels.Visible(true)))
                                .Tooltip(tooltip => tooltip
                                .Visible(true).Template("#=category# - #= kendo.format('{0}', value)#").Color("#000"))
                                 .Zoomable(zoomable => zoomable
                                .Mousewheel(mousewheel => mousewheel.Lock(ChartAxisLock.Y))
                                .Selection(selection => selection.Lock(ChartAxisLock.Y)))
                                 .Pannable(pannable => pannable
                                .Lock(ChartAxisLock.Y)
                                        )
                                 // .ChartArea(chartt => chartt.Background(item.DashboardBackGrounndColors))
                                 .HtmlAttributes(new { style = " height: 280px;" })

                                );
                page.WriteLiteral($"</div>");
                break;
                case "BarChart-Withoutseries":
                    page.WriteLiteral($"<div class=\"col-xl-6 dashboard_chartdiv\" style=\"background:" + item.DashboardBackGrounndColors+ "\">");
                        List<BarcharModel> model = new List<BarcharModel>();
                        model = (from DataRow dr in dt.Rows
                                 select new BarcharModel()
                                 {
                                     Name = dr[list.XAxisField].ToString(),
                                     Quantity = int.Parse(dr[list.YAxisField].ToString()),
                                 }).ToList();
                        int maxlength = 0;
                        if (model.Count() != 0)
                        {
                            maxlength = (int)model.Select(a => a.Quantity).Max();
                        }
                    page.Write(htmlHelper.Kendo().Chart(model)
                             .Name(item.DashboardTemplate.DashboardTemplateName)
                               .Title(t => t.Text(item.DasboardMappingName).Position(ChartTitlePosition.Top))
                               .Legend(legend => legend
                                    .Visible(false)
                                        )
                                 .ChartArea(chartArea => chartArea
                                    .Background("transparent"))
                               .SeriesDefaults(seriesdefaults => seriesdefaults.Column())
                                .Series(series =>
                                {
                                    series.Column(model => model.Quantity).Labels(o => o.Rotation("-90")).Border(p => p.Width(0));
                                })
                                 .CategoryAxis(axis => axis.Labels(p => p.Color("#000").Rotation(-40))
                                        .Categories(model => model.Name)
                                        .Title(Language.GetString(item.XAxisTitle))
                                .MajorGridLines(lines => lines.Visible(false)).Color("#000")
                                            )
                                  .ValueAxis(axis => axis
                                        .Numeric()
                                        .Max(maxlength == 0 ? 10 : maxlength)
                                        .Title(Language.GetString(item.YAxisTitle))
                                        .Line(line => line.Visible(false))
                                        .MajorGridLines(lines => lines.Visible(true)).Color("#000")
                                    )
                            .Tooltip(tooltip => tooltip
                                .Visible(true).Color("#000")
                               .Template(" #= value #")
                            )
       
                        .HtmlAttributes(new { style = "height: 280px;" })

                               );
                    page.WriteLiteral($"</div>");
                    break;
                case "BarChart-Withseries":
                    page.WriteLiteral($"<div class=\"col-xl-6 dashboard_chartdiv\" style=\"background:" + item.DashboardBackGrounndColors + "\">");
                    List<BarcharSeriesModel> models = new List<BarcharSeriesModel>();
                    models = (from DataRow dr in dt.Rows
                              select new BarcharSeriesModel()
                              {
                                  Name = dr[list.XAxisField].ToString(),
                                  Quantity = int.Parse(dr[list.YAxisField].ToString()),
                                  Categories = dr[list.CategoriesField].ToString(),
                              }).ToList();

                    var seriess = new List<ACS.AMS.WebApp.Models.DashboardSeriesModel>();
                    List<string> categoriesSeries = new List<string>();
                    var result = from expiry in models group expiry by expiry.Categories;
                    foreach (var data in result)
                    {
                        categoriesSeries.AddRange(from Stockdate in data select Stockdate.Name);
                        seriess.Add(new ACS.AMS.WebApp.Models.DashboardSeriesModel
                        {
                            Name = data.Key,
                            Data = from dataPoint in data select dataPoint.Quantity
                        });
                    }
                    var names = categoriesSeries.Distinct().ToArray();

                    page.Write(htmlHelper.Kendo().Chart(models)
                            .Name(item.DashboardTemplate.DashboardTemplateName)
                              .Title(t => t.Text(item.DasboardMappingName).Position(ChartTitlePosition.Top))
                              .Legend(legend => legend
                                   .Visible(false)
                                       )
                                    .ChartArea(chartt => chartt.Background("transparent"))
                                        .SeriesDefaults(seriesDefaults => seriesDefaults
                                            .Column()
                                            .Stack(false)
                                        )
                                        .Series(series =>
                                        {
                                            foreach (var store in seriess.Distinct())
                                            {
                                                series.Column(store.Data).Name(store.Name).Border(p => p.Width(0));//.CategoryField(list.CategoriesField);
                                            }
                                        })
                                           .Pannable(pannable => pannable
                                                .Lock(ChartAxisLock.Y)
                                                 )
                                        .Zoomable(zoomable => zoomable
                                            .Mousewheel(mousewheel => mousewheel.Lock(ChartAxisLock.Y))
                                            .Selection(selection => selection.Lock(ChartAxisLock.Y))
                                        )
                   
                                    .CategoryAxis(axis => axis
                                    .Categories(names)

                                    .AxisCrossingValue(0, 3)
                                    .Labels(a => a.Rotation(-40))
                                            //.Title(Language.GetString(reportTitle)).Color("#666")
                                            .MajorGridLines(lines => lines.Visible(true))
                                                             .Title(Language.GetString(item.XAxisTitle))
                                            .Line(line => line.Visible(true))
                                    )
                                            .Panes(panes => panes.Add())
                                            .ValueAxis(axis => axis
                                            .Numeric()
                                                    .Title(Language.GetString(item.YAxisTitle)).Color("#000")
                                            .Line(line => line.Visible(true))
                                            )
                                            .Tooltip(tooltip => tooltip
                                          .Template("#= series.name #: #=value#")
                                            .Visible(true)//.Color("#fff")
                                                 ).HtmlAttributes(new { style = "height:550px;" })

                                                      );

                       page.WriteLiteral($"</div>");
                    break;
                case "LineChart":
                    page.WriteLiteral($"<div class=\"col-xl-6 dashboard_chartdiv\" style=\"background:" + item.DashboardBackGrounndColors + "\">");
                    List<BarcharSeriesModel> LineModel = new List<BarcharSeriesModel>();
                    models = (from DataRow dr in dt.Rows
                              select new BarcharSeriesModel()
                              {
                                  Name = dr[list.XAxisField].ToString(),
                                  Quantity = int.Parse(dr[list.YAxisField].ToString()),
                                  Categories = dr[list.CategoriesField].ToString(),
                              }).ToList();
                    int maxlengths = 0;
                    if (models.Count() != 0)
                    {
                        maxlengths = (int)models.Select(a => a.Quantity).Max();
                    }
                    var seriesLine = new List<ACS.AMS.WebApp.Models.DashboardSeriesModel>();
                    List<string> lineSeries = new List<string>();
                    var results = from expiry in models group expiry by expiry.Categories;
                    foreach (var data in results)
                    {
                        lineSeries.AddRange(from Stockdate in data select Stockdate.Name);
                        seriesLine.Add(new ACS.AMS.WebApp.Models.DashboardSeriesModel
                        {
                            Name = data.Key,
                            Data = from dataPoint in data select dataPoint.Quantity
                        });
                    }
                    var namesline = lineSeries.Distinct().ToArray();

                    page.Write(htmlHelper.Kendo().Chart(LineModel)
                            .Name(item.DashboardTemplate.DashboardTemplateName)
                              .Title(t => t.Text(item.DasboardMappingName).Position(ChartTitlePosition.Top))
                              .Legend(legend => legend
                                   .Visible(false)
                                       )
                                    .ChartArea(chartt => chartt.Background("transparent"))
                                        .SeriesDefaults(seriesDefaults => seriesDefaults
                                            .Line()
                                            .Stack(false)
                                        )
                                        .Series(series =>
                                        {
                                            foreach (var store in seriesLine.Distinct())
                                            {
                                                series.Line(store.Data).Name(store.Name).Border(p => p.Width(0));//.CategoryField(list.CategoriesField);
                                            }
                                        })
                                         .CategoryAxis(axis => axis
                                                .Labels(labels => labels.Rotation(-90))
                                                .Categories(namesline)
                                                .Crosshair(c => c.Visible(true))
                                                  .Title(Language.GetString(item.XAxisTitle))
                                            )
                                            .ValueAxis(axis => axis.Numeric()
                                                .Labels(labels => labels.Format("{0:N0}"))
                                                .MajorUnit(maxlengths == 0 ? 10 : maxlengths)
                                                  .Title(Language.GetString(item.YAxisTitle)).Color("#000")
                                            )
                                            .Tooltip(tooltip => tooltip
                                                .Visible(true)
                                                .Shared(true)
                                                .Format("{0:N0}")
                                            )
                                           

                                                      );

                    page.WriteLiteral($"</div>");
                    break;
                case "Count":

                  

                    foreach (var field in fieldDetails)
                    {
                        var query = dt.AsEnumerable().Select(r => r.Field<int>(field.FieldName)).FirstOrDefault();
                        int value = query;//.ToString("#,###,###");
                        page.WriteLiteral($"<div class=\"col-xl-3 dash_tab\">");
                        page.WriteLiteral($"<div class=\"col-xl-3 dashboard_countdiv\" style=\"background-color: " + field.ColorCode + "\">");
                        page.WriteLiteral($"<div class=\"dashboard_countimg\" >");
                        if (!string.IsNullOrEmpty(field.IconPath))
                        {
                            string[] arryDocID1 = field.IconPath.ToString().Split("FileStoragePath");
                            var url = "/FileStoragePath" + arryDocID1[1];
                            page.WriteLiteral("<img id='StampImage' src='" + url + "' alt='No Image'>");
                        }
                        else
                        {
                            //page.WriteLiteral("<i class=\"fa fa - user fa - 2x\" ></i>");
                            page.WriteLiteral("<img id='StampImage' src='/css/images/DashboardCountDummy.png' alt='No Image'>");
                        }
                        page.WriteLiteral($"</div>");
                        page.WriteLiteral($"<div class=\"dashboard_countText\">");
                        page.WriteLiteral("<h1>" + value + "</h1>");
                        page.WriteLiteral("<p>" + Language.GetString(field.DisplayTitle) + "</p>");
                        page.WriteLiteral($"</div>");
                        //page.WriteLiteral($"<div class=\"dashboard_countValue\">");
                        //page.WriteLiteral("" + value + "");
                        ////page.WriteLiteral($"<table>");
                        ////page.WriteLiteral($"<tr>");
                        ////page.WriteLiteral($"<td>");
                        ////page.WriteLiteral($"<div class=\"dashtab_img panel1\">");


                        ////page.WriteLiteral($"</div>");
                        ////page.WriteLiteral($"</td>");
                        ////page.WriteLiteral($"<td>");
                        ////page.WriteLiteral("<h4>" + value+ "</h4>");
                        ////page.WriteLiteral("<p>'" + Language.GetString(field.DisplayTitle)+ "'</p>");

                        ////page.WriteLiteral($"</td>");
                        ////page.WriteLiteral($"</tr>");
                        ////page.WriteLiteral($"</table>");
                        
                        page.WriteLiteral($"</div></div>");
                    }

                    break;
            }

            //page.Write(htmlHelper.Kendo().Chart()
            //                    .Name(item.DashboardTemplate.DashboardTemplateName)
            //                      .Title(t => t.Text(item.DasboardMappingName).Position(ChartTitlePosition.Top))
            //                      .SeriesDefaults(series => series.Pie())
            //                      );
        }

        public virtual void CreateDashboardControl(IHtmlHelper<DashboardPageModel> htmlHelper, RazorPage page)
        {
            try
            {
                page.WriteLiteral($"<div class=\"row\">");
                var mapping = DashboardMappingTable.GetAllActivItem(AMSContext.CreateNewContext());
                    foreach (var item in mapping)
                    {
                   
                    var fieldDetails = DashboardFieldMappingTable.GetMappingDetails(AMSContext.CreateNewContext(), item.DashboardMappingID);
                        
                        var dashboardFields = DashboardTemplateTable.AllDashboardFields(AMSContext.CreateNewContext(), item.DashboardTemplateID);
                        var fl = (from b in dashboardFields
                                  select new
                                  {
                                      b.FieldName,
                                      b.FieldDataType
                                  }).AsEnumerable().Select(x => new DashboardQueryResultField(x.FieldName, Type.GetType(x.FieldDataType))).ToList();
                        fieldList = new DashboardQueryResultFieldCollection();
                        fieldList.AddRange(fl);
                        var queryType = DashboardTemplateTable.GetQueryType(AMSContext.CreateNewContext(), item.DashboardTemplate.QueryString);
                        var parameterList = new QueryParameterCollection();
                        if (!item.DashboardTemplate.QueryString.ToUpper().StartsWith("RVW_") && !item.DashboardTemplate.QueryString.ToUpper().StartsWith("NVW"))
                        {
                            parameterList = DynamicReportHelper.GetProcedureParameters(item.DashboardTemplate.QueryString);
                        }
                        var reportdetailDataSet = GetDataSetFromDB(queryType, item.DashboardTemplate.QueryString, parameterList);
                    if (reportdetailDataSet != null)
                    {
                        if (reportdetailDataSet.Rows.Count == 0)
                        {
                            //return Content("No Data Found For Given Selection Criteria");
                        }

                        //var listtoRemove = fl.Select(c => c.ReportFieldName).ToList().Except(selectedFieldList.Select(c => c.ReportTemplateField.FieldName).ToList()).ToList();
                        //var listtoRemove = reportdetailDataSet.Columns.Cast<System.Data.DataColumn>().Select(dc => dc.ColumnName).ToList().Except(fieldDetails.Select(c => c.DashboardTemplateField.FieldName).ToList()).ToList();
                        //foreach (string ColName in listtoRemove)
                        //{
                        //    if (reportdetailDataSet.Columns.Contains(ColName))
                        //        reportdetailDataSet.Columns.Remove(ColName);
                        //}

                        AddChart(htmlHelper, page, item, reportdetailDataSet, fieldDetails);
                    }
                   
                }
                page.WriteLiteral($"</div>");

            }
            catch (Exception ex)
            {
                ApplicationErrorLogTable.SaveException(ex);
                throw new Exception("Internal Server Error");
            }


        }
        public System.Data.DataTable GetDataSetFromDB(string queryType, string queryString, QueryParameterCollection parameterList)
        {
            SqlConnection conn = new SqlConnection(AMSContext.ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = conn.CreateCommand();

            try
            {

                if (queryType == "View")
                {
                    var condition = GetViewQuery(cmd);
                    var Orderby = GetOrderByQuery(cmd);
                    if (!string.IsNullOrEmpty(condition))
                        condition = " WHERE " + condition;
                    if (!string.IsNullOrEmpty(Orderby))
                        Orderby = " ORDER BY  " + Orderby;

                    cmd.CommandText = $"SELECT * FROM {queryString} {condition} {Orderby}";
                    cmd.CommandType = System.Data.CommandType.Text;
                }
                else
                {
                    cmd.CommandText = queryString;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var condition = GetProcedureQuery(cmd, parameterList);
                }


                da.SelectCommand = cmd;
                System.Data.DataTable ds = new System.Data.DataTable();

                ///conn.Open();
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogTable.SaveException(new Exception("Query: " + cmd.CommandText, ex));
                return null;
            }
        }

        private string GetProcedureQuery(SqlCommand cmd, QueryParameterCollection parameterList)
        {
            StringBuilder query = new StringBuilder();
            //Get all the fields from filters "selectedReportID"
            foreach (QueryParameter parameters in parameterList)
            {
                string fieldName = parameters.Name;
                if (fieldName.StartsWith("@") || fieldName.StartsWith("?"))
                {
                    fieldName = fieldName.Substring(1);
                }

                if (parameters.DataType == DbType.DateTime || parameters.DataType == DbType.Date || parameters.DataType == DbType.DateTime2 || parameters.DataType == DbType.DateTimeOffset)
                {
                    AddDateCondition(cmd, fieldName, fieldName, query);
                }
                else
                {
                    AddCondition(cmd, fieldName, fieldName, query);
                }

            }
            return query.ToString();
        }
        private string GetViewQuery(SqlCommand cmd)
        {
            StringBuilder query = new StringBuilder();
            //Get all the fields from filters "selectedReportID"
            foreach (var itm in fieldList)
            {
                if (itm.DataType == typeof(System.DateTime))
                {
                    AddDateCondition(cmd, itm.DashboardFieldName, itm.DashboardFieldName, query);
                }
                else
                {
                    AddCondition(cmd, itm.DashboardFieldName, itm.DashboardFieldName, query);
                }

            }
            return query.ToString();
        }
        private string GetOrderByQuery(SqlCommand cmd)
        {
            StringBuilder query = new StringBuilder();
            //order by asc/desc
            if (sortbyFields != null && sortbyFields.Any())
            {
                foreach (var itm in sortbyFields)
                {
                    ADDSorting(cmd, itm.OrderbyColumnName, itm.OrderbyType, query);
                }
            }

            return query.ToString();
        }
        private void ADDSorting(SqlCommand cmd, string fieldName, string Orderby, StringBuilder query)
        {
            if (!string.IsNullOrEmpty(fieldName))
            {
                if (query.Length > 0)
                    query.Append(" , ");

                switch (Orderby.Trim().ToUpper())
                {
                    case "ASC":
                        query.Append($" {fieldName} ASC ");
                        break;
                    case "DESC":
                        query.Append($" {fieldName} DESC ");
                        break;
                    default:
                        query.Append($" {fieldName} ASC ");
                        break;
                }
            }
        }
        private void AddCondition(SqlCommand cmd, string fieldName, string columnName, StringBuilder query)
        {
            var res =string.Empty;

            if (string.Compare(columnName.ToUpper(), "USERID", true) == 0)
                res = SessionUser.Current.UserID + "";
            if (string.Compare(columnName, "LANGUAGEID", true) == 0)
                res = SessionUser.Current.LanguageID + "";
            if (string.Compare(columnName, "LANGAUGEID", true) == 0)
                res = SessionUser.Current.LanguageID + "";
            if (string.Compare(columnName, "COMPANYID", true) == 0)
                res = SessionUser.Current.CompanyID + "";
            if (string.Compare(columnName, "TYPE", true) == 0)
                res = 1 + "";
            if (ReportRequest != null)
            {
                res = ReportRequest[fieldName];
            }
            if (!string.IsNullOrEmpty(res))
            {
                cmd.Parameters.AddWithValue($"@{columnName}", res.ToString());

                if (query.Length > 0)
                    query.Append(" AND ");

                query.Append($" {columnName} = @{columnName} ");
            }
            else
            {
                var from = ReportRequest[fieldName + "_Start"];
                var dt_to = ReportRequest[fieldName + "_End"];

                if (!string.IsNullOrEmpty(from))
                {
                    if (query.Length > 0)
                        query.Append(" AND ");

                    query.Append($"\"{columnName}\" >= @{columnName}From");
                    cmd.Parameters.AddWithValue($"@{columnName}From", from.ToString());
                }

                if (!string.IsNullOrEmpty(dt_to))
                {
                    if (query.Length > 0)
                        query.Append(" AND ");

                    query.Append($"\"{columnName}\" <= @{columnName}To");
                    cmd.Parameters.AddWithValue($"@{columnName}To", dt_to.ToString());
                }
            }
        }
        private void AddDateCondition(SqlCommand cmd, string fieldName, string columnName, StringBuilder query)
        {
            var res = ReportRequest[fieldName];

            if (!string.IsNullOrEmpty(res))
            {
                DateTime dtValue;
                if (DateTime.TryParseExact(res, CultureHelper.ConfigureDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtValue))
                {
                    cmd.Parameters.AddWithValue($"@{columnName}", dtValue);

                    if (query.Length > 0)
                        query.Append(" AND ");

                    query.Append($" {columnName} = @{columnName} ");
                }
            }
            else
            {
                var from = ReportRequest[fieldName + "_Start"];
                var dt_to = ReportRequest[fieldName + "_End"];

                DateTime dtValue;
                if (DateTime.TryParseExact(from, CultureHelper.ConfigureDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtValue))
                {
                    if (query.Length > 0)
                        query.Append(" AND ");

                    query.Append($"\"{columnName}\" >= @{columnName}From");
                    cmd.Parameters.AddWithValue($"@{columnName}From", dtValue);
                }

                if (DateTime.TryParseExact(dt_to, CultureHelper.ConfigureDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtValue))
                {
                    if (query.Length > 0)
                        query.Append(" AND ");

                    dtValue = dtValue.AddDays(1);
                    query.Append($"\"{columnName}\" < @{columnName}To");
                    cmd.Parameters.AddWithValue($"@{columnName}To", dtValue);
                }
            }
        }

    }
}