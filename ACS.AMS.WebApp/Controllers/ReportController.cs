using Kendo.Mvc.UI;

using Kendo.Mvc.Extensions;
using Kendo.Mvc;
using System.Xml.Serialization;
using ACS.AMS.DAL;
using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using Microsoft.AspNetCore.Mvc;
using ACS.AMS.WebApp.Models.ReportTemplate;
using ACS.AMS.DAL.Models.ReportTemplate;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace ACS.AMS.WebApp.Controllers
{
    public class ReportController : ACSBaseController
    {
        public ReportController()
        {
            _RightName = RightNames.Report;
        }

        private DataSourceResult GetAllReports([DataSourceRequest] DataSourceRequest request)
        {
            var query = ReportTable.GetAllItems(_db, true);

            return request.ToDataSourceResult(query);
        }

        // GET: /Report/
        public ActionResult Index()
        {
            if (!base.HasRights(RightNames.Report, UserRightValue.View))
                return GotoUnauthorizedPage();

            base.TraceLog("Report ", $"{SessionUser.Current.Username} -Index page request");
            return PartialView();
        }

        public ActionResult _Index([DataSourceRequest] DataSourceRequest request, string gridState)
        {
            try
            {
                if (!base.HasRights(RightNames.Report, UserRightValue.View))
                    return GotoUnauthorizedPage();

                base.TraceLog("Report ", $"{SessionUser.Current.Username} -_Index request to fetch data");
                // UserScreenDataTable.SaveScreenData(SessionUser.Current.UserID, "ReportIndex", gridState, 1);
                return Json(GetAllReports(request));
            }
            catch (ValidationException ex)
            {
                return base.ErrorActionResult(ex.Message);
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }

        // GET: /Report/Create
        public ActionResult Create()
        {
            if (!base.HasRights(RightNames.Report, UserRightValue.Create))
                return GotoUnauthorizedPage();

            base.TraceLog("Report ", $"{SessionUser.Current.Username} -Create page request");
            List<ReportColumnModel> obj = new List<ReportColumnModel>();
                HttpContext.Session.SetComplexData("ReportColumnModel", obj);
            ReportTable model = new ReportTable();
            model.ReportLeftMargin = 1;
            model.ReportRightMargin = 1;
            model.ReportTopMargin = 1;
            model.ReportBottomMargin = 1;

            return PartialView(model);
        }

        // POST: /Report/Create
        [HttpPost]
        public ActionResult Create(ReportTable item, IFormCollection data)
        {
            if (!base.HasRights(RightNames.Report, UserRightValue.Create))
                return GotoUnauthorizedPage();
            try
            {
                base.TraceLog("Report ", $"{SessionUser.Current.Username} -Create-post  request");
               
                //item.ReportPageHeight = 0.000000M;
                //item.ReportPageWidth = 0.000000M;
                item.CreatedBy = SessionUser.Current.UserID;
                item.CreatedDateTime = System.DateTime.Now;
                item.StatusID = (int)StatusValue.Active;
                string FieldId = data["hdReportSelectedFieldItemsIDS"];
                string[] fieldIDs = FieldId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string fields = data["hdReportSelectedFieldItems"];
                string[] fieldName = fields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);  //int cnt=1;

                List<ReportTemplateFieldTable> templateFields = ReportTemplateFieldTable.GetAllReportFields(_db, (int)item.ReportTemplateID);

                if (ModelState.IsValid)
                {
                    _db.Add(item);
                    string columnIds = data["hdReportSelectedItemsIDS"];
                    string[] ColumnID = columnIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string Columns = data["hdReportSelectedItems"];
                    string[] columnName = Columns.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);  //int cnt=1;

                    int i = 1;

                    foreach (string str in ColumnID)
                    {
                        ReportGroupFieldTable reportGroup = new ReportGroupFieldTable();
                        reportGroup.Report = item;

                        var data1 = (from d in templateFields
                                     where d.ReportTemplateFieldID == int.Parse(str)
                                     select d).FirstOrDefault();

                        reportGroup.ReportTemplateFieldID = int.Parse(str);
                        reportGroup.DisplayTitle = data1.DisplayTitle;
                        reportGroup.FieldFormat = data1.DefaultFormat;
                        reportGroup.FieldWidth = data1.DefaultWidth;
                        reportGroup.DisplayOrderID = i;
                        reportGroup.GroupLevel = (byte)i;
                        reportGroup.StatusID = (int)StatusValue.Active;
                        reportGroup.CreatedBy = SessionUser.Current.UserID;
                        reportGroup.CreatedDateTime = System.DateTime.Now;

                        _db.Add(reportGroup);
                        i++;
                    }

                    if (fieldIDs.Count() > 0)
                    {
                        i = 0;

                        foreach (string str in fieldIDs)
                        {
                            ReportFieldTable fieldTable = new ReportFieldTable();
                            fieldTable.Report = item;

                            var data1 = (from d in templateFields
                                         where d.ReportTemplateFieldID == int.Parse(str)
                                         select d).FirstOrDefault();
                            IList<ReportColumnModel> list = HttpContext.Session.GetComplexData<List<ReportColumnModel>>("ReportColumnModel");//ReportColumnModel.All(_db);
                            decimal result = list.Where(a => a.ColumnID == int.Parse(str)).Select(a => a.Width).FirstOrDefault();

                            fieldTable.ReportTemplateFieldID = int.Parse(str);
                            fieldTable.DisplayTitle = data1.DisplayTitle;
                            fieldTable.FieldFormat = data1.DefaultFormat;
                            fieldTable.FieldWidth = result == 0 ? data1.DefaultWidth : result;
                            fieldTable.DisplayOrderID = ++i;
                            fieldTable.CreatedBy = SessionUser.Current.UserID;
                            fieldTable.CreatedDateTime = System.DateTime.Now;
                            fieldTable.StatusID = (int)StatusValue.Active;
                            _db.Add(fieldTable);
                        }

                        _db.SaveChanges();
                        base.TraceLog("Report ", $"{SessionUser.Current.Username} -Create-post  request done");
                    }
                    else
                    {
                        return base.ErrorActionResult($"Please Add the Display Order for all Column Details");
                    }

                }
                else
                {
                    return base.ErrorActionResult($"Please Add the Report Column Details");
                }
                HttpContext.Session.SetComplexData("ReportColumnModel",null);
                //ReportColumnModel.All(_db).Clear();

                return PartialView("SuccessAction");
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

            return PartialView(item);

        }

        public ActionResult Edit(int id)
        {
            if (!base.HasRights(RightNames.Report, UserRightValue.Edit))
                return GotoUnauthorizedPage();

            base.TraceLog("Report ", $"{SessionUser.Current.Username} -Edit  request");
            List<ReportColumnModel> obj = new List<ReportColumnModel>();
            HttpContext.Session.SetComplexData("ReportColumnModel", obj);
            //Session["ReportID"] = id; 

            ReportTable excel = ReportTemplateTable.GetReport(_db, id);
            //Session["UserReportID"] = excel.ReportID;

            return PartialView(excel);
        }

        [HttpPost]
        public ActionResult Edit(ReportTable item, IFormCollection data)
        {
            if (!base.HasRights(RightNames.Report, UserRightValue.Edit))
                return GotoUnauthorizedPage();

            try
            {
                base.TraceLog("Report ", $"{SessionUser.Current.Username} -Edit-post  request.id-{item.ReportID}");
                var oldItm = ReportTable.GetItem(_db, item.ReportID);
                item.LastModifiedBy = SessionUser.Current.UserID;
                // item.LastModifyBy = SessionUser.Current.UserID;
                item.LastModifiedDateTime = System.DateTime.Now;
                item.StatusID = (int)StatusValue.Active;
                string FieldId = data["hdReportSelectedFieldItemsIDS"];
                string[] fieldIDs = FieldId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string fields = data["hdReportSelectedFieldItems"];
                string[] fieldName = fields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);  //int cnt=1;

                List<ReportTemplateFieldTable> templateFields = ReportTemplateFieldTable.GetAllReportFields(_db, (int)item.ReportTemplateID);

                if (ModelState.IsValid)
                {
                    string columnIds = data["hdReportSelectedItemsIDS"];
                    string[] ColumnID = columnIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string Columns = data["hdReportSelectedItems"];
                    string[] columnName = Columns.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);  //int cnt=1;

                    var grouptoDelete = ReportTemplateFieldTable.GetLineItemFromGroupTable(_db, item.ReportID);
                    if (grouptoDelete.Count() > 0)
                    {
                        ReportTemplateFieldTable.DeleteExistingGrouplineItem(_db, grouptoDelete);
                    }

                    var fieldtoDelete = ReportTemplateFieldTable.GetColumnLineItemFromFieldTable(_db, item.ReportID);
                    if (fieldtoDelete.Count() > 0)
                    {
                        ReportTemplateFieldTable.DeleteExistingFieldlineItem(_db, fieldtoDelete);
                    }

                    int i = 1;
                    foreach (string str in ColumnID)
                    {
                        ReportGroupFieldTable reportGroup = new ReportGroupFieldTable();

                        var data1 = (from d in templateFields
                                     where d.ReportTemplateFieldID == int.Parse(str)
                                     select d).FirstOrDefault();

                        reportGroup.ReportTemplateFieldID = int.Parse(str);
                        reportGroup.DisplayTitle = data1.DisplayTitle;
                        reportGroup.FieldFormat = data1.DefaultFormat;
                        reportGroup.FieldWidth = data1.DefaultWidth;
                        reportGroup.DisplayOrderID = i;
                        reportGroup.GroupLevel = (byte)i;
                        reportGroup.ReportID = item.ReportID;
                        reportGroup.StatusID = (int)StatusValue.Active;
                        reportGroup.CreatedBy = SessionUser.Current.UserID;
                        reportGroup.CreatedDateTime = System.DateTime.Now;

                        _db.Add(reportGroup);

                        i++;
                    }

                    if (fieldIDs.Count() > 0)
                    {
                        i = 0;
                        foreach (string str in fieldIDs)
                        {
                            //Get the TemplateFieldID & width detail
                            string[] templateFieldID = str.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
                            ReportFieldTable fieldTable = new ReportFieldTable();

                            var data1 = (from d in templateFields
                                         where d.ReportTemplateFieldID == int.Parse(templateFieldID[0])
                                         select d).FirstOrDefault();
                            IList<ReportColumnModel> list = HttpContext.Session.GetComplexData<List<ReportColumnModel>>("ReportColumnModel");
                            decimal result = list.Where(a => a.ColumnID == int.Parse(templateFieldID[0])).Select(a => a.Width).FirstOrDefault();

                            fieldTable.ReportTemplateFieldID = int.Parse(templateFieldID[0]);
                            fieldTable.DisplayTitle = data1.DisplayTitle;
                            fieldTable.FieldFormat = data1.DefaultFormat;
                            fieldTable.FieldWidth = result == 0 ? data1.DefaultWidth : result;
                            fieldTable.DisplayOrderID = ++i;
                            fieldTable.ReportID = item.ReportID;
                            fieldTable.CreatedBy = SessionUser.Current.UserID;
                            fieldTable.CreatedDateTime = System.DateTime.Now;
                            fieldTable.StatusID = (int)StatusValue.Active;
                            _db.Add(fieldTable);
                        }
                    }


                    DataUtilities.CopyObject(item, oldItm);
                    _db.SaveChanges();
                    base.TraceLog("Report ", $"{SessionUser.Current.Username} -Edit-post  request done");
                }
                else
                {
                    return base.ErrorActionResult($"Please Add the Report Column Details");
                   
                }
              HttpContext.Session.SetComplexData("ReportColumnModel",null);

                return PartialView("SuccessAction");
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

            return PartialView(item);

        }

        //// GET: /Report/Details/5
        public ActionResult Details(int id)
        {
            if (!base.HasRights(RightNames.Report, UserRightValue.Details))
                return GotoUnauthorizedPage();

            base.TraceLog("Report ", $"{SessionUser.Current.Username} -Details  request");
            //Session["ReportColumnID"] = id;
            ReportTable excel = ReportTemplateTable.GetReport(_db, id);
            //Session["ReportID"] = excel.ReportID;
            var query = ReportTemplateFieldTable.GetColumnLineItemFromFieldTable(_db, excel.ReportID);

            //int pageID = UserReportTable.GetPageUnits(_db, id, excel.ReportID);
            //ViewData["PageUnits"] = pageID == 1 ? "Inches" : "Centimeters";
            List<ReportColumnModel> list = new List<ReportColumnModel>();
            foreach (var itm in query)
            {
                ReportColumnModel model = new ReportColumnModel();
                model.ReportColumnLineItemID = itm.ReportFieldID;
                model.ColumnID = itm.ReportTemplateFieldID;
                model.ColumName = itm.DisplayTitle;
                model.Width = itm.FieldWidth;
                //model.Type = itm.DisplayOrderID;
                //model.isSumField = itm.isSumField;
                list.Add(model);
            }
            HttpContext.Session.SetComplexData("ReportColumnModel", list);
            //Session[SessionUser.Current.UserID + "_ReportColumnsDetils"] = (IList<ReportColumnModel>)list;
            return PartialView(excel);
        }

        public ActionResult EditingReportInline_Read([DataSourceRequest] DataSourceRequest request, int reportID)
        {
            return Json(GetcolumnDetails(request, reportID));
        }

        private object GetcolumnDetails([DataSourceRequest] DataSourceRequest request, int reportID)
        {
            var query = ReportTemplateFieldTable.GetAllColumns(_db, reportID);
            query = query.OrderBy(b => b.DisplayOrder);

            return request.ToDataSourceResult(query);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                if (!base.HasRights(RightNames.Report, UserRightValue.Delete))
                    return GotoUnauthorizedPage();

                base.TraceLog("Report ", $"{SessionUser.Current.Username} -Delete request.id-{id}");
                ReportTable item = ReportTable.GetItem(_db, id);
                if (item != null)
                {
                    item.Delete();
                    this._db.SaveChanges();
                }
                base.TraceLog("Report ", $"{SessionUser.Current.Username} -Delete request done.id-{id}");
                return Content("");
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }

        public IActionResult DeleteAll(string toBeDeleteIds)
        {
            var pageName = "Report";

            base.TraceLog("DeleteAll", $"{SessionUser.Current.Username} - {pageName} Delete action requested");
            if (!base.HasRights(RightNames.UserMaster, UserRightValue.Delete))
                return RedirectToAction("UnauthorizedPage");

            string checkdItems = toBeDeleteIds;
            string[] arrRequestID = checkdItems.Split(',');
            int[] requestID = Array.ConvertAll(arrRequestID, s => int.Parse(s));
            int requestCount = requestID.Count();
            if (requestCount > 0)
            {
                foreach (var id in requestID)
                {
                    ReportTable item = ReportTable.GetItem(_db, id);

                    if (item != null)
                        item.Delete();

                    item.UpdateUniqueKey(_db);
                    _db.SaveChanges();
                }
            }
            base.TraceLog("Delete", $"{pageName} details page deleted successfully");
            return RedirectToAction("SuccessAction", new { pageName = pageName });
        }

        [HttpPost]
        public ActionResult AddDataToReportColumnModel(ReportColumnModel[] Item)
        {
            base.TraceLog("Report ", $"{SessionUser.Current.Username} -AddDataToReportColumnModel request");
            List<ReportColumnModel> query = HttpContext.Session.GetComplexData<List<ReportColumnModel>>("ReportColumnModel");

            int cnt = 1;
            foreach (var itm in Item)
            {
                ReportColumnModel mod = new ReportColumnModel();
                mod.ColumName = itm.ColumName;
                mod.ColumnID = itm.ColumnID;
                mod.DisplayOrder = cnt + "";
                mod.isSumField = itm.isSumField;
                mod.Type = itm.Type;
                if (itm.Width == 0)
                {
                    mod.Width = 0;
                }
                else
                {
                    mod.Width = itm.Width;
                }
                query.Add(mod);
                cnt = cnt + 1;
            }
            HttpContext.Session.SetComplexData("ReportColumnModel", query);

            return Json(new { result = true });
        }
    }
}
