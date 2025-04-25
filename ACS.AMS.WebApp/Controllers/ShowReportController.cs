using ACS.AMS.DAL;
using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACS.AMS.DAL.DBModel;
using ACS.AMS.WebApp.Models;
using Telerik.Reporting;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Office.Word;
using System.Xml;
using Telerik.Reporting.Processing;
using SQLite;
using Microsoft.AspNetCore.Components;
using DocumentFormat.OpenXml.Bibliography;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System.Globalization;
using System.Text;
using ReportLibrary;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace ACS.AMS.WebApp.Controllers
{
    public class ShowReportController : ACSBaseController
    {
        //private readonly IReportServices _reportService;
        private Stream reportDefinition;
        private ReportTable selectedReport;
        private List<ReportOrderbyColumns> sortbyFields;
        private QueryResultFieldCollection fieldList;
        IFormCollection ReportRequest;
        private readonly IWebHostEnvironment _env;
        public ShowReportController()
        {
            //IWebHostEnvironment env
            // _reportService = reportService;
            //_env = env;
        }

        #region Static Report
        public IActionResult AuditLogReport()
        {
            base.TraceLog("ShowReport Index", $"{SessionUser.Current.Username} -StaticReportIndex request");
            //return PartialView();
            return PartialView("AuditLogReport");
        }
        [HttpPost]
        public IActionResult AuditLogReport(IFormCollection request)
        {
            base.TraceLog("ShowReport AuditLogReport", $"{SessionUser.Current.Username} -AuditLogReport reportName");
            ReportModel reportModel = new ReportModel();
            reportModel.ReportName = request["ReportName"];

            var paramters = Request.Query.Keys.Where(b => b.ToUpper().StartsWith("RPARAM_"));
            var paramters1 = request.Keys.Where(b => b.ToUpper().StartsWith("RPARAM_"));
            paramters1 = paramters1.Where(c => !c.EndsWith("_input"));
            //reportModel.ParameterData.Add(new Telerik.Reporting.Parameter("TestVal", "0"));
            foreach (var prm in paramters)
            {
                var pName = prm.Substring("RPARAM_".Length);
                reportModel.ParameterData.Add(new Telerik.Reporting.Parameter(pName, Request.Query[prm]));
            }

            var paramtersPost = request.Keys.Where(b => b.ToUpper().StartsWith("RPARAM_"));
            paramtersPost = paramtersPost.Where(c => !c.EndsWith("_input"));
            //reportModel.ParameterData.Add(new Telerik.Reporting.Parameter("TestVal", "0"));
            foreach (var prm in paramtersPost)
            {
                var pName = prm.Substring("RPARAM_".Length);
                if (reportModel.ParameterData.Where(b => string.Compare(b.Name, pName, true) == 0).Any() == false)
                    reportModel.ParameterData.Add(new Telerik.Reporting.Parameter(pName, request[prm]));
            }

            //ExportToHtml(reportModel);
            return PartialView("ShowStaticReport", reportModel);
        }
        public IActionResult StaticReportIndex(string reportName)
        {
            base.TraceLog("ShowReport Index", $"{SessionUser.Current.Username} -StaticReportIndex request");
            //return PartialView();
            return PartialView("StaticReportIndex", reportName);
        }
        //public IActionResult ShowStaticReport(IFormCollection request, string reportName)
        //{
        //    return PartialView();
        //}
        public IActionResult ShowStaticReport(IFormCollection request, string reportName)
        {
            base.TraceLog("ShowReport ShowStaticReport", $"{SessionUser.Current.Username} -ShowStaticReport reportName-{reportName}");
            ReportModel reportModel = new ReportModel();
            reportModel.ReportName = reportName;

            var paramters = Request.Query.Keys.Where(b => b.ToUpper().StartsWith("RPARAM_"));
            var paramters1 = request.Keys.Where(b => b.ToUpper().StartsWith("RPARAM_"));
            paramters1 = paramters1.Where(c => !c.EndsWith("_input"));
            //reportModel.ParameterData.Add(new Telerik.Reporting.Parameter("TestVal", "0"));
            foreach (var prm in paramters)
            {
                var pName = prm.Substring("RPARAM_".Length);
                reportModel.ParameterData.Add(new Telerik.Reporting.Parameter(pName, Request.Query[prm]));
            }

            var paramtersPost = request.Keys.Where(b => b.ToUpper().StartsWith("RPARAM_"));
            paramtersPost = paramtersPost.Where(c => !c.EndsWith("_input"));
            //reportModel.ParameterData.Add(new Telerik.Reporting.Parameter("TestVal", "0"));
            foreach (var prm in paramtersPost)
            {
                var pName = prm.Substring("RPARAM_".Length);
                if (reportModel.ParameterData.Where(b => string.Compare(b.Name, pName, true) == 0).Any() == false)
                    reportModel.ParameterData.Add(new Telerik.Reporting.Parameter(pName, request[prm]));
            }

            //ExportToHtml(reportModel);
            return PartialView("ShowStaticReport", reportModel);
        }
        public IActionResult ShowReceiptReport(string paramName,string paramVal, string reportName)
        {
            base.TraceLog("ShowReport ShowReceiptReport", $"{SessionUser.Current.Username} -ShowReceiptReport reportName-{reportName}");
            ReportModel reportModel = new ReportModel();
            reportModel.ReportName = reportName;
            reportModel.ParameterData.Add(new Telerik.Reporting.Parameter(paramName, paramVal));
           
            //ExportToHtml(reportModel);
            return PartialView("ShowStaticReport", reportModel);
        }
        public IActionResult ShowAuditReport(string fromDate,string toDate,string actionName, int companyID,int userID,int languageID,string reportName)
        {
            base.TraceLog("ShowReport ShowReceiptReport", $"{SessionUser.Current.Username} -ShowReceiptReport reportName-{reportName}");
            ReportModel reportModel = new ReportModel();
            reportModel.ReportName = reportName;
            reportModel.ParameterData.Add(new Telerik.Reporting.Parameter("FromDate", fromDate));
            reportModel.ParameterData.Add(new Telerik.Reporting.Parameter("ToDate", toDate));
            reportModel.ParameterData.Add(new Telerik.Reporting.Parameter("ActionName", actionName));
            reportModel.ParameterData.Add(new Telerik.Reporting.Parameter("CompanyID", companyID));
            reportModel.ParameterData.Add(new Telerik.Reporting.Parameter("UserID", userID));
            reportModel.ParameterData.Add(new Telerik.Reporting.Parameter("LanguageID", languageID));
            //ExportToHtml(reportModel);
            return PartialView("ShowStaticReport", reportModel);
        }
        public IActionResult ShowSingleAuditLogReport(string reportName,string entityName,int primaryKeyID, int languageID, int? userID)
        {
            base.TraceLog("ShowReport ShowReceiptReport", $"{SessionUser.Current.Username} -ShowReceiptReport reportName-{reportName}");
            ReportModel reportModel = new ReportModel();
            reportModel.ReportName = reportName;
            reportModel.ParameterData.Add(new Telerik.Reporting.Parameter("EntityName", entityName));
            reportModel.ParameterData.Add(new Telerik.Reporting.Parameter("PrimaryID", primaryKeyID));
            reportModel.ParameterData.Add(new Telerik.Reporting.Parameter("LanguageID", languageID));
            reportModel.ParameterData.Add(new Telerik.Reporting.Parameter("UserID", userID));
            return PartialView("ShowStaticReport", reportModel);
        }
        public IActionResult ShowCatalogueReport(string reportName, int CategoryID)
        {
            base.TraceLog("ShowReport ShowReceiptReport", $"{SessionUser.Current.Username} -ShowReceiptReport reportName-{reportName}");
            ReportModel reportModel = new ReportModel();
            reportModel.ReportName = reportName;
            reportModel.ParameterData.Add(new Telerik.Reporting.Parameter("CultureID", CategoryID));
           
            return PartialView("ShowStaticReport", reportModel);
        }
        System.Collections.Generic.List<System.IO.Stream> streams = new System.Collections.Generic.List<System.IO.Stream>();
        private void ExportToHtml(ReportModel reportModel)
        {
            base.TraceLog("ShowReport ExportToHtml", $"{SessionUser.Current.Username} -ExportToHtml reportName-{reportModel.ReportName}");
            var report = new Telerik.Reporting.Report();
            Telerik.Reporting.SqlDataSource sqlDataSource = new Telerik.Reporting.SqlDataSource("", "", "", SqlDataSourceCommandType.StoredProcedure);
            Telerik.Reporting.DetailSection detail = new Telerik.Reporting.DetailSection();

            detail.Height = new Telerik.Reporting.Drawing.Unit(3.0, Telerik.Reporting.Drawing.UnitType.Inch);
            detail.Name = "detail";
            report.Items.Add((Telerik.Reporting.ReportItemBase)detail);

            Telerik.Reporting.Processing.ReportProcessor reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();

            string documentName = "";

            // specify the output format of the produced image.
            var deviceInfo = new System.Collections.Hashtable();

            deviceInfo["OutputFormat"] = "JPEG";


            // Depending on the report definition choose ONE of the following REPORT SOURCES
            //                  -1-
            // ***CLR (CSharp) report definitions***
            var reportSource = new Telerik.Reporting.UriReportSource();//.TypeReportSource();

            // reportName is the Assembly Qualified Name of the report
            //reportSource.TypeName = reportModel.ReportName;
            //                  -1-

            ////                  -2-
            //// ***Declarative (TRDP/TRDX) report definitions***
            //var reportSource = new Telerik.Reporting.UriReportSource();

            //// reportName is the path to the TRDP/TRDX file
            reportSource.Uri = @"D:\PrgsDrive\Projects\ACS\BarcodeGulf\Dubai\DPWorld\Apps\BGS_DPWorld_LMS_Web\ACS.Smartidfy.WebApp\wwwroot\Reports\" + reportModel.ReportName + ".trdp";
            ////                  -2-

            ////                  -3-
            //// ***Instance of the report definition***
            //var reportSource = new Telerik.Reporting.InstanceReportSource();

            //// Report1 is the class of the report. It should inherit Telerik.Reporting.Report class
            //reportSource.ReportDocument = new Report1();
            ////                  -3-

            // Pass parameter value with the Report Source if necessary
            foreach(var pm in reportModel.ParameterData)
            {
                reportSource.Parameters.Add(pm.Name, pm.Value);
            }
            //object parameterValue = "Some Parameter Value";
            //reportSource.Parameters.Add("ParameterName", parameterValue);

            bool result = reportProcessor.RenderReport("IMAGE", reportSource, deviceInfo, this.CreateStream, out documentName);

            this.CloseStreams();
        }

        void CloseStreams()
        {
            foreach (System.IO.Stream stream in this.streams)
            {
                stream.Close();
            }
            this.streams.Clear();
        }

        System.IO.Stream CreateStream(string name, string extension, System.Text.Encoding encoding, string mimeType)
        {
            string path = @"D:\Shared\ExportFormats";
            string filePath = System.IO.Path.Combine(path, name + "." + extension);

            System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create);
            this.streams.Add(fs);
            return fs;
        }




        public IActionResult LaundryCollectFromEmpIndex(string reportName)
        {
            return PartialView("LaundryCollectFromEmpIndex", reportName);
        }

        public IActionResult LaundryCollectFromEmpReport(IFormCollection request, string reportName)
        {
            base.TraceLog("ShowReport LaundryCollectFromEmpReport", $"{SessionUser.Current.Username} -LaundryCollectFromEmpReport reportName-{reportName}");
            ReportModel reportModel = new ReportModel();
            reportModel.ReportName = reportName;

            var paramters = Request.Query.Keys.Where(b => b.ToUpper().StartsWith("RPARAM_"));
            var paramters1 = request.Keys.Where(b => b.ToUpper().StartsWith("RPARAM_"));
            paramters1 = paramters1.Where(c => !c.EndsWith("_input"));
            foreach (var prm in paramters)
            {
                var pName = prm.Substring("RPARAM_".Length);
                reportModel.ParameterData.Add(new Telerik.Reporting.Parameter(pName, Request.Query[prm]));
            }

            var paramtersPost = request.Keys.Where(b => b.ToUpper().StartsWith("RPARAM_"));
            paramtersPost = paramtersPost.Where(c => !c.EndsWith("_input"));
            //reportModel.ParameterData.Add(new Telerik.Reporting.Parameter("TestVal", "0"));
            foreach (var prm in paramtersPost)
            {
                var pName = prm.Substring("RPARAM_".Length);
                if (reportModel.ParameterData.Where(b => string.Compare(b.Name, pName, true) == 0).Any() == false)
                {
                    if (!string.IsNullOrEmpty(request[prm]))
                        reportModel.ParameterData.Add(new Telerik.Reporting.Parameter(pName, request[prm]));
                }
            }

            return PartialView("LaundryCollectFromEmpReport", reportModel);
        }

        #endregion

        #region Dynamic Report Methods

        [HttpGet]
        public IActionResult DynamicFieldControls(int screenFilterLineItemID, int rowCount)
        {
            base.TraceLog("ShowReport DynamicFieldControls", $"{SessionUser.Current.Username} -DynamicFieldControls click");
            ViewData.Add("ScreenFilterLineItemID", screenFilterLineItemID);
            ViewData.Add("RowCount", rowCount);
            return PartialView();
        }

        public ActionResult DynamicReport(int id)
        {
            //if (!base.HasRights(RightNames.PostingHistoryReport, UserRightValue.View))
            //    return GotoUnauthorizedPage();
            base.TraceLog("ShowReport DynamicReport", $"{SessionUser.Current.Username} -DynamicReport click.id-{id}");
            var reportTemplate = ReportTemplateTable.GetItem(_db, id);
            ViewBag.ReportTemplateID = reportTemplate.ReportTemplateID;
            ViewBag.ReportTemplateName = reportTemplate.ReportTemplateName;

            return PartialView("DynamicReport", reportTemplate.ReportTemplateID);
        }

        //[HttpPost]
        //public IActionResult GenerateReport(IFormCollection Request)
        //{
        //    try
        //    {
        //        string type = Request["TypeofReport"];
        //        string reportName = Request["reportName"];
        //        if (type == "S")
        //            reportName += "_Summary";
        //        if (type == "D")
        //            reportName += "_Detailed";
        //        string format = Request["ReportFormat"];

        //        string jsonSorting = Request["hdnlstSelectOrderby"];
        //        if (!string.IsNullOrEmpty(jsonSorting))
        //        {
        //            var model = JsonConvert.DeserializeObject<List<ReportOrderbyColumns>>(jsonSorting);
        //            if (model.Any())
        //            {
        //                sortbyFields = model;
        //            }
        //        }
        //        int selectedReportID = 0;
        //        string reportteplate = Request["ReportTemplate"];
        //        if (reportteplate != null)
        //        {
        //            string templateID = Request["ReportTemplate"];
        //            if (string.IsNullOrEmpty(templateID))
        //            {
        //                throw new ValidationException("Report template required");
        //            }

        //            selectedReportID = int.Parse(Request["ReportTemplate"]);
        //        }
        //        if (selectedReportID != 0)
        //        {
        //            selectedReport = (from b in _db.ReportTable.Include("ReportTemplate")
        //                              where b.ReportID == selectedReportID
        //                              select b).FirstOrDefault();


        //            var parameterList = new QueryParameterCollection();
        //            if (!selectedReport.ReportTemplate.QueryString.ToUpper().StartsWith("RVW_") && !selectedReport.ReportTemplate.QueryString.ToUpper().StartsWith("RVW"))
        //            {
        //                parameterList = DynamicReportHelper.GetProcedureParameters(selectedReport.ReportTemplate.QueryString);
        //            }

        //            ReportModel reportModel1 = new ReportModel();

        //            reportModel1.ReportName = selectedReport.ReportName;

        //            foreach (var prm in parameterList)
        //            {
        //                string name = prm.Name.Split('@')[1];
        //                if (reportModel1.ParameterData.Where(b => string.Compare(b.Name, name, true) == 0).Any() == false)
        //                {
        //                    if (!string.IsNullOrEmpty(Request[name]))
        //                        reportModel1.ParameterData.Add(new Telerik.Reporting.Parameter(name, Request[name]));
        //                }
        //            }


        //            return PartialView("ShowStaticReport", reportModel1);
        //        }
        //        else
        //        {

        //        }
        //    }
        //    catch (ValidationException ex)
        //    {
        //        ViewData["FocusControl"] = ex.FieldName;
        //        ModelState.AddModelError("12", ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogTable.SaveException(ex);
        //        ModelState.AddModelError("123", ex.Message);
        //    }
        //    ReportModel reportModel = new ReportModel()
        //    {
        //        ReportName = "UniformRequestReport"
        //    };
        //    return PartialView("ShowStaticReport", reportModel);
        //}

        [HttpPost]
        public IActionResult GenerateReport(IFormCollection Request)
        {
            try
            {
                base.TraceLog("ShowReport GenerateReport", $"{SessionUser.Current.Username} -GenerateReport click.id");
                ReportRequest = Request;
                string format = Request["ReportFormat"];
                int selectedReportID = 0;
                string reportteplate = Request["ReportTemplate"];
                if (reportteplate != null)
                {
                    string templateID = Request["ReportTemplate"];
                    if (string.IsNullOrEmpty(templateID))
                    {
                        return base.ErrorActionResult("Report template required");
                    }

                    selectedReportID = int.Parse(Request["ReportTemplate"]);
                }
                
                if (selectedReportID != 0)
                {
                    selectedReport = (from b in _db.ReportTable.Include("ReportTemplate")
                                      where b.ReportID == selectedReportID
                                      select b).FirstOrDefault();

                    var parameterList = new QueryParameterCollection();
                    if (!selectedReport.ReportTemplate.QueryString.ToUpper().StartsWith("RVW_") && !selectedReport.ReportTemplate.QueryString.ToUpper().StartsWith("NVW"))
                    {
                        parameterList = DynamicReportHelper.GetProcedureParameters(selectedReport.ReportTemplate.QueryString);
                    }
                    var selectedFieldList = MasterTable.GetAllReportFields(_db, selectedReport.ReportID);
                    var reportFields = ReportTemplateFieldTable.GetAllReportFields(_db, selectedReport.ReportTemplateID);
                    var fl = (from b in reportFields
                              select new
                              {
                                  b.FieldName,
                                  b.FieldDataType,
                                 
                              }).AsEnumerable().Select(x => new QueryResultField(x.FieldName, Type.GetType(x.FieldDataType)));

                    var selectedGroupList = (from b in _db.ReportGroupFieldTable
                                             where b.StatusID == 1 && b.ReportID == selectedReport.ReportID
                                             select b);
                    fieldList = new QueryResultFieldCollection();
                    fieldList.AddRange(fl);
                    
                    RDLReport rdl = new RDLReport();
                    if (selectedGroupList.Count() > 0)
                    {
                        rdl.DetailSectionFields.Add(new ReportTableField()
                        {
                            FieldName = Language.GetString("SNo"),
                            Expression = "=RowNumber(\"Group1\")",
                            Title = "SNo",
                            Width = 0.35,
                            FieldFormat = "",
                            SumRequired = false
                        });
                    }
                    else
                    {
                        rdl.DetailSectionFields.Add(new ReportTableField()
                        {
                            FieldName = Language.GetString("SNo"),
                            Expression = "=RowNumber(\"DataSet1\")",
                            Title = "SNo",
                            Width = 0.35,
                            FieldFormat = "",
                            SumRequired = false
                        });
                    }

                    foreach (var field in selectedFieldList)
                    {
                        rdl.DetailSectionFields.Add(new ReportTableField()
                        {
                            FieldName = field.ReportTemplateField.FieldName,
                            Title = Language.GetString(field.DisplayTitle),
                            Width = (double)field.FieldWidth,
                            FieldFormat = field.FieldFormat,
                            SumRequired = field.ReportTemplateField.FooterSumRequired,
                            GroupSumRequired = field.ReportTemplateField.GroupSumRequired
                        });
                    }
                    
                    foreach (var field in selectedGroupList)
                    {
                        rdl.GroupSectionFields.Add(new ReportTableField()
                        {
                            FieldName = field.ReportTemplateField.FieldName,
                            Title = Language.GetString(field.DisplayTitle),
                            Width = (double)field.FieldWidth,
                            FieldFormat = field.FieldFormat,
                            SumRequired = field.ReportTemplateField.FooterSumRequired,
                            GroupSumRequired = field.ReportTemplateField.GroupSumRequired
                        });
                    }

                    var reportdetailDataSet = GetDataSetFromDB(selectedReport.ReportTemplate.QueryType, selectedReport.ReportTemplate.QueryString, parameterList, selectedReportID);
                    if (reportdetailDataSet.Rows.Count == 0)
                    {
                        return base.ErrorActionResult("No Data Found For Given Selection Criteria");
                    }                    

                    //var listtoRemove = fl.Select(c => c.ReportFieldName).ToList().Except(selectedFieldList.Select(c => c.ReportTemplateField.FieldName).ToList()).ToList();
                    var listtoRemove= reportdetailDataSet.Columns.Cast<System.Data.DataColumn>()
                                    .Select(dc => dc.ColumnName).ToList()
                                    .Except(selectedFieldList.Select(c => c.ReportTemplateField.FieldName)
                                    .ToList()).ToList();
                    foreach (string ColName in listtoRemove)
                    {
                        if (reportdetailDataSet.Columns.Contains(ColName))
                            reportdetailDataSet.Columns.Remove(ColName);
                    }

                    var report = GenericReport.GenericDynamicReport(reportdetailDataSet);
                    format = format != "" ? format : "PDF";

                    #region Export 
                    Telerik.Reporting.Processing.ReportProcessor reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();
                    // set any deviceInfo settings if necessary
                    var deviceInfo = new System.Collections.Hashtable();

                    // Depending on the report definition choose ONE of the following REPORT SOURCES
                    //                  -1-
                    // ***CLR (CSharp) report definitions***
                    var reportSource = new Telerik.Reporting.InstanceReportSource();
                    reportSource.ReportDocument = report;
                    reportSource.ReportDocument.DocumentName = selectedReport.ReportName;

                    Telerik.Reporting.Processing.RenderingResult result = reportProcessor.RenderReport(format, reportSource, deviceInfo);

                    string fileName = result.DocumentName + "." + result.Extension;
                    string path = System.IO.Path.GetTempPath();
                    string filePath = System.IO.Path.Combine(path, fileName);
                    base.TraceLog("ShowReport GenerateReport", $"{SessionUser.Current.Username} -GenerateReport click done");
                    return File(result.DocumentBytes, result.MimeType, fileName);
                    //if (string.IsNullOrEmpty(format))
                    //{
                    //    ReportModel reportModel = new ReportModel();
                    //    reportModel.ReportName = selectedReport.ReportName;
                    //    return PartialView("GenerateReport", reportModel);
                    //}
                    //else
                    //{
                    //    #region Export 
                    //    Telerik.Reporting.Processing.ReportProcessor reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();
                    //    // set any deviceInfo settings if necessary
                    //    var deviceInfo = new System.Collections.Hashtable();

                    //    // Depending on the report definition choose ONE of the following REPORT SOURCES
                    //    //                  -1-
                    //    // ***CLR (CSharp) report definitions***
                    //    var reportSource = new Telerik.Reporting.InstanceReportSource();
                    //    reportSource.ReportDocument = report;
                    //    reportSource.ReportDocument.DocumentName = selectedReport.ReportName;

                    //    Telerik.Reporting.Processing.RenderingResult result = reportProcessor.RenderReport(format, reportSource, deviceInfo);

                    //    string fileName = result.DocumentName + "." + result.Extension;
                    //    string path = System.IO.Path.GetTempPath();
                    //    string filePath = System.IO.Path.Combine(path, fileName);
                    //    return File(result.DocumentBytes, result.MimeType, fileName);

                    //   // #endregion
                    //}
                    #endregion
                    //var listtoRemove = fl.Select(c => c.ReportFieldName).ToList().Except(selectedFieldList.Select(c => c.ReportTemplateField.FieldName).ToList()).ToList();

                    //foreach (string ColName in listtoRemove)
                    //{
                    //    if (reportdetailDataSet.Columns.Contains(ColName))
                    //        reportdetailDataSet.Columns.Remove(ColName);
                    //}
                    //var listtoRemove = reportdetailDataSet.Columns.Cast<System.Data.DataColumn>().Select(dc => dc.ColumnName).ToList().Except(selectedFieldList.Select(c => c.ReportTemplateField.FieldName).ToList()).ToList();
                    //foreach (string ColName in listtoRemove)
                    //{
                    //    if (reportdetailDataSet.Columns.Contains(ColName))
                    //        reportdetailDataSet.Columns.Remove(ColName);
                    //}
                    //reportdetailDataSet = CleanDataSet(reportdetailDataSet);

                    //var report = GenericReport.GenericDynamicReport(_env, reportdetailDataSet);

                    //var reportPackager = new ReportPackager();
                    ////SAVE THE REPORT AS A FILE
                    ////using (System.Xml.XmlWriter xmlWriter = System.Xml.XmlWriter.Create("D:\\SerializedReport.xml"))
                    ////{
                    ////    Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializer =
                    ////        new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();

                    ////    xmlSerializer.Serialize(xmlWriter, report);
                    ////}
                    //string downloadfileName = Path.Combine(_env.WebRootPath, "Reports") + "\\" + selectedReport.ReportName + ".trdp";
                    //using (var sourceStream = new System.IO.FileStream(downloadfileName, FileMode.OpenOrCreate))
                    //{
                    //    reportPackager.Package(report, sourceStream);
                    //}

                    ////format = format != "" ? format : "PDF";
                    //if (string.IsNullOrEmpty(format))
                    //{
                    //    ReportModel reportModel = new ReportModel();
                    //    reportModel.ReportName = selectedReport.ReportName;
                    //    base.TraceLog("Show Report GenerateReport", $"{SessionUser.Current.Username} -GenerateReport request done.");
                    //    return PartialView("GenerateReport", reportModel);
                    //}
                    //else
                    //{
                    //    #region Export 

                    //    Telerik.Reporting.Processing.ReportProcessor reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();
                    //    // set any deviceInfo settings if necessary
                    //    var deviceInfo = new System.Collections.Hashtable();

                    //    // Depending on the report definition choose ONE of the following REPORT SOURCES
                    //    //                  -1-
                    //    // ***CLR (CSharp) report definitions***
                    //    var reportSource = new Telerik.Reporting.InstanceReportSource();
                    //    reportSource.ReportDocument = report;
                    //    reportSource.ReportDocument.DocumentName = selectedReport.ReportName;

                    //    Telerik.Reporting.Processing.RenderingResult result = reportProcessor.RenderReport(format, reportSource, deviceInfo);

                    //    string fileName = result.DocumentName + "." + result.Extension;
                    //    string path = System.IO.Path.GetTempPath();
                    //    string filePath = System.IO.Path.Combine(path, fileName);
                    //    return File(result.DocumentBytes, result.MimeType, fileName);

                    //    #endregion
                    //}
                }
                else
                {

                }
            }
            catch (ValidationException ex)
            {
                ViewData["FocusControl"] = ex.FieldName;
                ModelState.AddModelError("12", ex.Message);
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
            OutsideRequestMessageModel outsideRequestMessageModel = new OutsideRequestMessageModel()
            {
                Message = "Error",
                Status = "Fail"
            };
            
            return PartialView("OutsideRequestMessagePage", outsideRequestMessageModel);
        }

        public DataTable GetDataSetFromDB(string queryType, string queryString, QueryParameterCollection parameterList, int selectedReportID)
        {
            base.TraceLog("ShowReport GetDataSetFromDB", $"{SessionUser.Current.Username} -GetDataSetFromDB click done");
            SqlConnection conn = new SqlConnection(AMSContext.ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = conn.CreateCommand();

            try
            {
                if (queryType == "View")
                {
                    var condition = GetViewQuery(cmd,selectedReportID);
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
                    var condition = GetProcedureQuery(cmd, parameterList, selectedReportID);
                }


                da.SelectCommand = cmd;
                DataTable ds = new DataTable();

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


        public void ExportTheReport(Telerik.Reporting.Report report, string format, string reportName)
        {
            base.TraceLog("ShowReport ExportTheReport", $"{SessionUser.Current.Username} -ExportTheReport click done");
            Telerik.Reporting.Processing.ReportProcessor reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();
            // set any deviceInfo settings if necessary
            var deviceInfo = new System.Collections.Hashtable();

            // Depending on the report definition choose ONE of the following REPORT SOURCES
            //                  -1-
            // ***CLR (CSharp) report definitions***
            var reportSource = new Telerik.Reporting.InstanceReportSource();
            reportSource.ReportDocument = report;
            reportSource.ReportDocument.DocumentName = reportName;

            Telerik.Reporting.Processing.RenderingResult result = reportProcessor.RenderReport(format, reportSource, deviceInfo);
            if (!result.HasErrors)
            {
                string fileName = result.DocumentName + "." + result.Extension;
                string path = System.IO.Path.GetTempPath();
                string filePath = System.IO.Path.Combine(path, fileName);

                using (System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                {
                    fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
                }
            }
        }

        private string GetProcedureQuery(SqlCommand cmd, QueryParameterCollection parameterList,int selectedReportID)
        {
            base.TraceLog("ShowReport GetProcedureQuery", $"{SessionUser.Current.Username} -GetProcedureQuery click done");
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
                    AddDateCondition(cmd, fieldName, fieldName, query, selectedReportID);
                }
                else
                {
                    AddCondition(cmd, fieldName, fieldName, query, selectedReportID);
                }

            }
            return query.ToString();
        }
        private string GetViewQuery(SqlCommand cmd,int selectedReportID)
        {
            base.TraceLog("ShowReport GetViewQuery", $"{SessionUser.Current.Username} -GetViewQuery click done");
            StringBuilder query = new StringBuilder();
            //Get all the fields from filters "selectedReportID"
            foreach (var itm in fieldList)
            {
                if (itm.DataType == typeof(System.DateTime))
                {
                    AddDateCondition(cmd, itm.ReportFieldName, itm.ReportFieldName, query, selectedReportID);
                }
                else
                {
                    AddCondition(cmd, itm.ReportFieldName, itm.ReportFieldName, query, selectedReportID);
                }

            }
            return query.ToString();
        }
        private string GetOrderByQuery(SqlCommand cmd)
        {
            base.TraceLog("ShowReport GetOrderByQuery", $"{SessionUser.Current.Username} -GetOrderByQuery click done");
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
            base.TraceLog("ShowReport ADDSorting", $"{SessionUser.Current.Username} -ADDSorting click done");
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
        private object GetdefaultParamValue(string name)
        {

            string value = ReportRequest[name];
            if (string.Compare(name, "UserID", true) == 0)
                return (SessionUser.Current.UserID);
            if (string.Compare(name, "LanguageID", true) == 0)
                return (SessionUser.Current.LanguageID);
            if (string.Compare(name, "CompanyID", true) == 0)
                return (SessionUser.Current.CompanyID);
            return value;
        }

        private void AddCondition(SqlCommand cmd, string fieldName, string columnName, StringBuilder query,int selectedReportID)
        {
            base.TraceLog("ShowReport AddCondition", $"{SessionUser.Current.Username} -AddCondition click done");
            var screenfilter = (from b in _db.ScreenFilterTable
                                where b.ReportTemplateID == selectedReportID && b.StatusID == (int)StatusValue.Active
                                select b).FirstOrDefault();
            if(screenfilter!=null)
            {
                var lineitem = (from b in _db.ScreenFilterLineItemTable
                                where
                              b.ScreenFilterID == screenfilter.ScreenFilterID && b.DisplayName == fieldName
                                select b).FirstOrDefault();
                if(lineitem!=null)
                {
                    fieldName = lineitem.QueryField;
                    columnName = lineitem.QueryField;
                }
            }
            //  var res = ReportRequest[fieldName];
            var defaultRes = GetdefaultParamValue(fieldName);

            var res = GetdefaultParamValue(fieldName);//ReportRequest[fieldName];

            if (res != null) //(!string.IsNullOrEmpty(res))
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
        private void AddDateCondition(SqlCommand cmd, string fieldName, string columnName, StringBuilder query, int selectedReportID)
        {
            base.TraceLog("ShowReport AddDateCondition", $"{SessionUser.Current.Username} -AddDateCondition click done");
            var res = GetdefaultParamValue(fieldName);//ReportRequest[fieldName];

            if (res != null) //(!string.IsNullOrEmpty(res))
            {
                DateTime dtValue;
                if (DateTime.TryParseExact(res.ToString(), CultureHelper.ConfigureDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtValue))
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
        #endregion
    }
}