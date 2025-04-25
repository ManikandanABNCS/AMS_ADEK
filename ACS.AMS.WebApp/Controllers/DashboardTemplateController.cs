using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

using Kendo.Mvc.UI;

using Kendo.Mvc.Extensions;
using Kendo.Mvc;
using System.Xml.Serialization;
using ACS.AMS.DAL;
using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using Microsoft.AspNetCore.Mvc;
using ACS.AMS.WebApp.Models.ReportTemplate;
using NPOI.SS.Formula.Functions;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace ACS.AMS.WebApp.Controllers
{
    public class DashboardTemplateController : ACSBaseController
    {
        public DashboardTemplateController()
        {
            _RightName = RightNames.DashboardTemplate;
        }

        protected virtual IQueryable<BaseEntityObject> GetAllItemsForIndex()
        {
            return DashboardTemplateTable.GetAllItems(_db);
        }

        public IActionResult Index()
        {
            if (!base.HasRights(RightNames.DashboardTemplate, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("DashboardTemplate Index", $"{SessionUser.Current.Username} -DashboardTemplate Index page requested");

            return PartialView();
        }

        public IActionResult _Index([DataSourceRequest] DataSourceRequest request)
        {
            if (!base.HasRights(RightNames.DashboardTemplate, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");
            IQueryable<DashboardTemplateTable> query = DashboardTemplateTable.GetAllItems(_db);
           
            var dsResult = request.ToDataSourceResult(query);
            base.TraceLog("DashboardTemplate Index", $"{SessionUser.Current.Username} -DashboardTemplate Index page Data Fetch");
            return Json(dsResult);
        }

        //
        // GET: /ReportTemplate/Create
        public ActionResult Create()
        {
            if (!base.HasRights(_RightName, UserRightValue.Create))
                return GotoUnauthorizedPage();
            base.TraceLog("DashboardTemplate create", $"{SessionUser.Current.Username} -DashboardTemplate create page request");
            DashboardTemplateTable model = new DashboardTemplateTable();
            model.CurrentPageID = SessionUser.Current.GetNextPageID();
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult Create(DashboardTemplateTable item, IFormCollection data)
        {
            if (!base.HasRights(_RightName, UserRightValue.Create))
                return GotoUnauthorizedPage();

            try
            {
                base.TraceLog("DashboardTemplate create-post", $"{SessionUser.Current.Username} -DashboardTemplate will save to db");
                //if (ModelState.IsValid)
                //{
                var obj = vwDashboardTemplateObjects.GetItem(_db, item.ObjectID);
                var type = obj.ObjectType;
                var procedureName = obj.ObjectName;

                //make sure template name doesn't exists
                var model = FilterFieldDashBoardDataModel.GetCurrentModel(item.CurrentPageID);
                
                item.QueryString = procedureName;

                _db.Add(item);

                //Query object should not be added already
                if (DashboardTemplateTable.IsQueryTextAlreadyExists(_db, item.QueryString))
                {
                    return base.ErrorActionResult($"Query object '{item.QueryString}' already exists");
                }

                //Atleat one field is required
                if (model.Fields.Count == 0)
                {
                    return base.ErrorActionResult($"No field found on query object or session expired.");
                }

                foreach (var val in model.Fields)
                {
                    val.CreatedDateTime = DateTime.Now;
                    val.DashboardTemplate = item;

                    _db.Add(val);
                }

        

                var filters = model.FilterFields.Where(b => b.EnableFilter == true).ToList();
                foreach (var modelLi in filters)
                {
                    modelLi.CreatedDateTime = DateTime.Now;
                    modelLi.DashboardTemplate = item;
                    modelLi.ScreenFilterTypeID = 1;

                    _db.Add(modelLi);
                }

                _db.SaveChanges();
                base.TraceLog("DashboardTemplate create-post", $"{SessionUser.Current.Username} -DashboardTemplate details saved to db");

                return PartialView("SuccessAction");
            }
            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
            //        }
            //    }
            //}
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
            try
            {
                if (!base.HasRights(_RightName, UserRightValue.Edit))
                    return GotoUnauthorizedPage();

                base.TraceLog("DashboardTemplate Edit", $"{SessionUser.Current.Username} -DashboardTemplate Edit page request.id-{id}");
                DashboardTemplateTable dept = DashboardTemplateTable.GetItem(_db, id);
                dept.CurrentPageID = SessionUser.Current.GetNextPageID();

                var queryType = DashboardTemplateTable.GetQueryType(_db, dept.QueryString);
                LoadQueryObjectFields("Edit", dept.QueryString, queryType, dept.CurrentPageID, dept.DashboardTemplateID, dept.DashboardTemplateName);

                return PartialView(dept);
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }
        public ActionResult Details(int id)
        {
            if (!base.HasRights(_RightName, UserRightValue.View))
                return GotoUnauthorizedPage();

            base.TraceLog("DashboardTemplate Details", $"{SessionUser.Current.Username} -DashboardTemplate Details page request. id-{id}");
            DashboardTemplateTable dept = DashboardTemplateTable.GetItem(_db, id);
            dept.CurrentPageID = SessionUser.Current.GetNextPageID();

            var queryType = DashboardTemplateTable.GetQueryType(_db, dept.QueryString);

            LoadQueryObjectFields("View", dept.QueryString, queryType, dept.CurrentPageID, dept.DashboardTemplateID, dept.DashboardTemplateName);

            return PartialView(dept);
        }
        [HttpPost]
        public ActionResult Edit(DashboardTemplateTable item, IFormCollection data)
        {
            if (!base.HasRights(_RightName, UserRightValue.Edit))
                return GotoUnauthorizedPage();

            try
            {
                if (ModelState.IsValid)
                {
                    base.TraceLog("DashboardTemplate Edit-Post", $"{SessionUser.Current.Username} -DashboardTemplate details will modify to db.id- {item.DashboardTemplateID}");
                    DashboardTemplateTable oldItm = DashboardTemplateTable.GetItem(_db, item.DashboardTemplateID);
                    string oldTemplateName = oldItm.DashboardTemplateName;

                    DataUtilities.CopyObject(item, oldItm);

                    var model = FilterFieldDashBoardDataModel.GetCurrentModel(item.CurrentPageID);

                    //Atleat one field is required
                    if (model.Fields.Count == 0)
                    {
                        return base.ErrorActionResult($"No field found on query object or session expired.");
                    }

                    var allDBTemplateFields = (from b in _db.DashboardTemplateFieldTable
                                               where b.DashboardTemplateID == item.DashboardTemplateID
                                               select b).ToList();
                    if (allDBTemplateFields.Count > 0)
                    {
                        allDBTemplateFields.ForEach(e => e.StatusID = (int)StatusValue.Deleted);
                        _db.SaveChanges();
                    }

                    var fields = model.Fields.ToList();
                    foreach (var modelLi in fields)
                    {
                        DashboardTemplateFieldTable screeItm = allDBTemplateFields.Where(m => m.DashboardTemplateFieldID == modelLi.DashboardTemplateFieldID).FirstOrDefault();
                        modelLi.DashboardTemplateID = item.DashboardTemplateID;

                        if (screeItm != null)
                        {
                            screeItm.StatusID = (int)StatusValue.Active;

                            //modelLi.StatusID = (int)StatusValue.Active;
                            //DataUtilities.CopyObject(modelLi, screeItm);

                        }
                        else
                        {
                            //this is a new field add it
                            _db.Add(modelLi);
                        }
                    }

                 


                    //fetch all items including delete one
                    var allscreenFilters = from b in _db.DashboardTemplateFilterFieldTable
                                           where b.DashboardTemplateID == item.DashboardTemplateID
                                           select b;

                    foreach (var filterLI in allscreenFilters)
                    {
                        filterLI.StatusID = (int)StatusValue.Deleted;
                    }

                    var selectedFilters = model.FilterFields.Where(b => b.EnableFilter == true);
                    foreach (var modelLi in selectedFilters)
                    {
                        DashboardTemplateFilterFieldTable screeItm = allscreenFilters.Where(q => q.QueryField == modelLi.QueryField).FirstOrDefault();

                        if (screeItm != null)
                        {
                            //Already this line exists, just enable and assign its field details
                            var id = screeItm.DashboardTemplateFilterFieldID;
                            DataUtilities.CopyObject(modelLi, screeItm);

                            screeItm.DashboardTemplateFilterFieldID = id;
                            screeItm.DashboardTemplateID = item.DashboardTemplateID;

                            screeItm.ScreenFilterTypeID = modelLi.ScreenFilterTypeID;
                            screeItm.StatusID = (byte)StatusValue.Active;
                            screeItm.CreatedDateTime = DateTime.Now;
                        }
                        else
                        {
                            screeItm = new DashboardTemplateFilterFieldTable();
                            _db.Add(screeItm);
                            DataUtilities.CopyObject(modelLi, screeItm);

                            screeItm.DashboardTemplateID = item.DashboardTemplateID;
                            screeItm.CreatedDateTime = DateTime.Now;
                        }
                    }

                    _db.SaveChanges();
                    base.TraceLog("DashboardTemplate create", $"{SessionUser.Current.Username} -DashboardTemplate modified to db. id-{item.DashboardTemplateID}");
                    return PartialView("SuccessAction");
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

            return PartialView(item);
        }
        public void LoadQueryObjectFields(string screen, string queryName, string queryType, int currentPageID, int? reportTemplateID = null, string templateName = null)
        {
            base.TraceLog("DashboardTemplate LoadQueryObjectFields", $"{SessionUser.Current.Username} -LoadQueryObjectFields call queryName-{queryName}");
            var filterDatamodel = FilterFieldDashBoardDataModel.GetCurrentModel(currentPageID);
            string ConnectionString = AMSContext.ConnectionString;

            var dbObjectSchema = DBHelper.GetAllFields(queryName, queryType);

            filterDatamodel.Fields.Clear();
            filterDatamodel.FilterFields.Clear();

            bool isView = string.Compare(queryType, "View", true) == 0;
            if (!reportTemplateID.HasValue)
                reportTemplateID = -1;

            var templateFields = from b in _db.DashboardTemplateFieldTable
                                 where b.DashboardTemplateID == reportTemplateID.Value
                                 //&& b.ReportTemplateTable.StatusID == (int)StatusValue.Active
                                 select b;

            var allFields = (from col in dbObjectSchema.Columns
                                 //Join with the current availalbe fields
                             join p in templateFields
                             on col.Name equals p.FieldName into ps
                             from p in ps.DefaultIfEmpty()
                             where (!col.Name.EndsWith("ID") && !col.Name.Contains("Attribute") && !col.Name.Contains("CategoryL") && !col.Name.Contains("LocationL")
                             && col.DataType.ToString() != "System.Boolean" && !col.Name.Contains("DOF")
                             && !col.Name.Contains("ERP") && !col.Name.Contains("QF") && !col.Name.Contains("Path")
                             )// col.DataType.ToString() != "System.Int32" && 
                             orderby col.Name
                             select new DashboardTemplateFieldTable
                             {
                                 DisplayTitle = (p == null) ? col.Name : p.DisplayTitle,
                                 FieldName = col.Name,
                                 QueryFieldName = col.Name,

                                
                                 FieldDataType = col.DataType.ToString(),
                                 DefaultFormat = (p == null) ? GetDefaultFormat(col.DataType) : p.DefaultFormat,

                                 //FooterSumRequired = (p == null) ? IsNumericType(col.DataType) : p.FooterSumRequired,
                                 //GroupSumRequired = (p == null) ? IsNumericType(col.DataType) : p.GroupSumRequired,

                                 DashboardTemplateFieldID = (p == null) ? 0 : p.DashboardTemplateFieldID,
                                 //this line available and mark it as active
                                 StatusID = (byte)StatusValue.Active
                             }).ToList();

            filterDatamodel.Fields.AddRange(allFields);

            var filterLineQry = from b in _db.DashboardTemplateFilterFieldTable
                                where b.DashboardTemplateID == reportTemplateID.Value
                                       
                                select b;

            List<DashboardTemplateFilterFieldTable> allFilterFields = null;

            var availableFilterFields = dbObjectSchema.Columns;

            if (!isView)
            {
                availableFilterFields = dbObjectSchema.Parameters;
            }

            //Parameters are there for this object, so use it
            allFilterFields = (from f in availableFilterFields
                               join p in filterLineQry
                               on f.Name equals p.QueryField into ps
                               from p in ps.DefaultIfEmpty()
                                   //orderby p.OrderNo
                               where (!f.Name.Contains("Attribute") && !f.Name.Contains("CategoryL") && !f.Name.Contains("LocationL")
                                    && f.DataType.ToString() != "System.Boolean" && !f.Name.Contains("DOF")
                             && !f.Name.Contains("ERP") && !f.Name.Contains("QF") && !f.Name.Contains("Path")
                                    )
                               select new DashboardTemplateFilterFieldTable
                               {
                                   DisplayTitle = f.Name,
                                   QueryField = f.Name,
                                   FieldDataType = f.DataType.ToString(),
                                   FieldName=f.Name,
                                   StatusID = p != null ? p.StatusID : (byte)StatusValue.Active,
                                   EnableFilter = p != null ? p.StatusID == (byte)StatusValue.Active ? true : false : false,

                                   ScreenFilterTypeID = (byte)(p == null ? 1 : p.ScreenFilterTypeID),
                                   FieldTypeID = p == null ? 0 : p.FieldTypeID,
                                   //For procedure all parameters are fixed one.
                                   IsFixedFilter = p == null ? (!isView) :(bool) p.IsFixedFilter,
                                   IsMandatory = p == null ? false : (bool)p.IsMandatory,

                                 
                                   DashboardTemplateFilterFieldID = p != null ? p.DashboardTemplateFilterFieldID : 0,
                                   OrderNo = p != null ? p.OrderNo : 0
                               }).ToList();

            //Update OrderNumber for other fields
            if (allFilterFields.Any(x => x.OrderNo == 0))
            {
                var lstfields = allFilterFields.Where(x => x.OrderNo == 0);
                int maxOrderNumber = allFilterFields.Max(x => x.OrderNo);
                foreach (var item in lstfields)
                {
                    maxOrderNumber++;
                    item.OrderNo = maxOrderNumber;
                }
            }

            filterDatamodel.FilterFields.AddRange(allFilterFields);

            var allFieldMaps1 = AFieldTypeTable.GetAllItems(_db).Where(b => b.FieldTypeCode == 99).ToList();
            var dateField = AFieldTypeTable.GetAllItems(_db).Where(b => b.FieldTypeDesc == "Date").FirstOrDefault();

            //map the fields with filters. Only for the new fields
            foreach (var f in allFilterFields)
            {
                var found = (from b in allFieldMaps1
                             where f.QueryField.ToUpper().EndsWith(b.FieldTypeDesc.ToUpper())
                             select b).FirstOrDefault();

                if (found != null)
                {
                    if (f.DashboardTemplateFilterFieldID == 0)
                    {
                        f.EnableFilter = true;
                        f.IsFixedFilter = true;
                    }

                    if (f.FieldTypeID == null)
                    {
                        f.FilterTypeDesc = found.FieldTypeDesc;
                        f.FieldTypeID = found.FieldTypeID;
                    }
                }
                else
                {
                    if ((string.Compare(f.FieldDataType, "System.DateTime", true) == 0) && (dateField != null))
                    {
                        if (f.DashboardTemplateFilterFieldID == 0)
                        {
                            f.EnableFilter = true;
                            f.IsFixedFilter = true;
                        }

                        if (f.FieldTypeID == null)
                        {
                            f.FilterTypeDesc = dateField.FieldTypeDesc;
                            f.FieldTypeID = dateField.FieldTypeID;

                            f.ScreenFilterTypeID = (byte)ScreenFilterTypeValue.RangeFilter;
                        }
                    }
                }
            }
        }

        public ActionResult DashboardTemplateFilterFields(int currentPageID, int pageNumber = 0, bool detailsPage = false)
        {
            base.TraceLog("DashboardTemplate DashboardTemplateFilterFields", $"{SessionUser.Current.Username} -DashboardTemplateFilterFields call");
            ViewBag.CurrentPageID = currentPageID;
            ViewBag.PageNumber = pageNumber;
            ViewBag.DetailsPage = detailsPage;

            var filterDatamodel = FilterFieldDashBoardDataModel.GetCurrentModel(currentPageID);

            ViewBag.FilterDataModel = filterDatamodel;

            if (detailsPage)
            {
                return PartialView("DashboardTemplateFilterFieldsDetails");
            }

            return PartialView();
        }

        public ActionResult DashboardTemplateFields(int currentPageID, int pageNumber = 0, bool detailsPage = false)
        {
            base.TraceLog("DashboardTemplate DashboardTemplateFields", $"{SessionUser.Current.Username} -DashboardTemplateFields call");
            ViewBag.CurrentPageID = currentPageID;
            ViewBag.PageNumber = pageNumber;

            var filterDatamodel = FilterFieldDashBoardDataModel.GetCurrentModel(currentPageID);

            ViewBag.FilterDataModel = filterDatamodel;

            if (detailsPage)
            {
                return PartialView("DashboardTemplateFieldsDetails");
            }

            return PartialView();
        }

        public ActionResult UpdateFilterField(string fieldName, int currentPageID, bool isEnabled, bool isMandatory, int? filterFieldID, byte filterTypeID)
        {
            try
            {
                base.TraceLog("DashboardTemplate UpdateFilterField", $"{SessionUser.Current.Username} -UpdateFilterField call");
                var filterDetailsmodel = FilterFieldDashBoardDataModel.GetCurrentModel(currentPageID);
                var lineItem = filterDetailsmodel.FilterFields.Where(b => b.QueryField == fieldName).FirstOrDefault();

                if (lineItem != null)
                {
                    lineItem.FieldTypeID = filterFieldID.HasValue ? (int)filterFieldID : 3;
                    lineItem.EnableFilter = isEnabled;
                    lineItem.IsMandatory = isMandatory;
                    lineItem.ScreenFilterTypeID = filterTypeID;
                }
                else
                {
                    return base.ErrorJsonResult("Invalid Field - " + fieldName);
                }
                base.TraceLog("DashboardTemplate UpdateFilterField", $"{SessionUser.Current.Username} -UpdateFilterField call done");
                return Content("");
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }

        public ActionResult UpdateTemplateField(string fieldName, int currentPageID, decimal defaultWidth, string defaultFormat, bool footerSumRequired)
        {
            try
            {
                base.TraceLog("DashboardTemplate UpdateTemplateField", $"{SessionUser.Current.Username} -UpdateTemplateField call");
                var model = FilterFieldDashBoardDataModel.GetCurrentModel(currentPageID);
                var lineItem = model.Fields.Where(b => b.FieldName == fieldName).FirstOrDefault();

                if (lineItem != null)
                {
                   
                    lineItem.DefaultFormat = defaultFormat;
                   
                }
                else
                {
                    return base.ErrorJsonResult("Invalid Field - " + fieldName);
                }
                base.TraceLog("DashboardTemplate UpdateTemplateField", $"{SessionUser.Current.Username} -UpdateTemplateField call done");
                return Content("");
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }

        public string GetDefaultFormat(Type dataType)
        {
            return (dataType == typeof(System.DateTime)) ? "=Parameters!DateFormat.Value" : "";
        }

        public static bool IsNumericType(Type t)
        {
            switch (Type.GetTypeCode(t))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Up arrow click event
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="currentPageID"></param>
        /// <param name="isEnabled"></param>
        /// <param name="isMandatory"></param>
        /// <param name="filterFieldID"></param>
        /// <param name="filterTypeID"></param>
        /// <returns></returns>
        public ActionResult MoveUpField(string fieldName, int currentPageID, bool isEnabled, bool isMandatory, int? filterFieldID, byte filterTypeID)
        {
            try
            {
                base.TraceLog("DashboardTemplate MoveUpField", $"{SessionUser.Current.Username} -MoveUpField call.Field Name-{fieldName}");
                var filterDetailsmodel = FilterFieldDashBoardDataModel.GetCurrentModel(currentPageID);
                var lineItem = filterDetailsmodel.FilterFields.Where(b => b.QueryField == fieldName).FirstOrDefault();

                var lineItemtoIncrease = default(DashboardTemplateFilterFieldTable);

                if (filterDetailsmodel.FilterFields.Any(b => b.QueryField != fieldName && b.OrderNo < lineItem.OrderNo))
                {
                    lineItemtoIncrease = filterDetailsmodel.FilterFields.Where(b => b.QueryField != fieldName && b.OrderNo < lineItem.OrderNo).OrderBy(x => x.OrderNo).LastOrDefault();
                }
                var t = filterDetailsmodel.FilterFields.Where(b => b.QueryField != fieldName && b.OrderNo < 9);
                if (lineItem != null)
                {
                    if (lineItemtoIncrease != null)
                    {
                        int currRecOrderNo = lineItem.OrderNo;
                        int aboveRecOrderNo = lineItemtoIncrease.OrderNo;

                        lineItem.OrderNo = aboveRecOrderNo;
                        lineItemtoIncrease.OrderNo = currRecOrderNo;
                    }
                }
                else if (lineItem.OrderNo == 0)
                {
                    return base.ErrorJsonResult("OrderNo already set as first Field - " + fieldName);
                }
                else
                {
                    return base.ErrorJsonResult("Invalid Field - " + fieldName);
                }
                base.TraceLog("DashboardTemplate MoveUpField", $"{SessionUser.Current.Username} -MoveUpField call done.Field Name-{fieldName}");
                return Content("");
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }

        /// <summary>
        /// Down arrow click event
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="currentPageID"></param>
        /// <param name="isEnabled"></param>
        /// <param name="isMandatory"></param>
        /// <param name="filterFieldID"></param>
        /// <param name="filterTypeID"></param>
        /// <returns></returns>
        public ActionResult MoveDownField(string fieldName, int currentPageID, bool isEnabled, bool isMandatory, int? filterFieldID, byte filterTypeID)
        {
            try
            {
                base.TraceLog("DashboardTemplate MoveDownField", $"{SessionUser.Current.Username} -MoveDownField call.Field Name-{fieldName}");
                var filterDetailsmodel = FilterFieldDashBoardDataModel.GetCurrentModel(currentPageID);
                var lineItem = filterDetailsmodel.FilterFields.Where(b => b.QueryField == fieldName).FirstOrDefault();
                //var lineItemtoDecrease = filterDetailsmodel.FilterFields.Where(b => b.QueryField != fieldName && b.OrderNo == (lineItem.OrderNo + 1)).FirstOrDefault();

                var lineItemtoDecrease = default(DashboardTemplateFilterFieldTable);
                if (filterDetailsmodel.FilterFields.Any(b => b.QueryField != fieldName && b.OrderNo > lineItem.OrderNo))
                {
                    lineItemtoDecrease = filterDetailsmodel.FilterFields.Where(b => b.QueryField != fieldName && b.OrderNo > lineItem.OrderNo).OrderBy(x => x.OrderNo).FirstOrDefault();
                }

                if (lineItem != null)
                {
                    if (lineItemtoDecrease != null)
                    {
                        int currRecOrderNo = lineItem.OrderNo;
                        int belRecOrderNo = lineItemtoDecrease.OrderNo;

                        lineItem.OrderNo = belRecOrderNo;
                        lineItemtoDecrease.OrderNo = currRecOrderNo;
                    }
                }
                else
                {
                    return base.ErrorJsonResult("Invalid Field - " + fieldName);
                }
                base.TraceLog("DashboardTemplate MoveDownField", $"{SessionUser.Current.Username} -MoveDownField call done.Field Name-{fieldName}");
                return Content("");
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                if (!base.HasRights(_RightName, UserRightValue.Delete))
                    return base.ErrorJsonResult(Language.GetErrorMessage("UnauthorizedAction"));
                base.TraceLog("DashboardTemplate Delete", $"{SessionUser.Current.Username} -Delete request.id-{id}");
                DashboardTemplateTable item = DashboardTemplateTable.GetItem(_db, id);

                if (item != null)
                {

                    var sFItem = (from vv in _db.DashboardTemplateFieldTable
                                  where vv.DashboardTemplateID == id
                                  select vv);
                    foreach (DashboardTemplateFieldTable it in sFItem)
                    {
                        it.StatusID = (int)StatusValue.Deleted;
                    }
                    var sFieldItem = (from vv in _db.DashboardTemplateFilterFieldTable
                                  where vv.DashboardTemplateID == id
                                  select vv);
                    foreach (DashboardTemplateFilterFieldTable it in sFieldItem)
                    {
                        it.StatusID = (int)StatusValue.Deleted;
                    }
                    //_db.Remove(sFItem);


                    item.Delete();
                    item.UpdateUniqueKey(_db);
                    this._db.SaveChanges();
                    base.TraceLog("DashboardTemplate Delete", $"{SessionUser.Current.Username} -Delete request done.id-{id}");
                }

                return Content("");
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }

        public IActionResult DeleteAll(string toBeDeleteIds)
        {
            var pageName = "DashboardTemplate";

            base.TraceLog("DeleteAll", $"{SessionUser.Current.Username} - {pageName} Delete action requested");
            if (!base.HasRights(RightNames.DashboardTemplate, UserRightValue.Delete))
                return RedirectToAction("UnauthorizedPage");

            string checkdItems = toBeDeleteIds;
            string[] arrRequestID = checkdItems.Split(',');
            int[] requestID = Array.ConvertAll(arrRequestID, s => int.Parse(s));
            int requestCount = requestID.Count();
            if (requestCount > 0)
            {
                foreach (var id in requestID)
                {
                    DashboardTemplateTable item = DashboardTemplateTable.GetItem(_db, id);

                    if (item != null)
                    {

                        var sFItem = (from vv in _db.DashboardTemplateFieldTable
                                      where vv.DashboardTemplateID == id
                                      select vv);
                        foreach (DashboardTemplateFieldTable it in sFItem)
                        {
                            it.StatusID = (int)StatusValue.Deleted;
                        }
                        var sFieldItem = (from vv in _db.DashboardTemplateFilterFieldTable
                                          where vv.DashboardTemplateID == id
                                          select vv);
                        foreach (DashboardTemplateFilterFieldTable it in sFieldItem)
                        {
                            it.StatusID = (int)StatusValue.Deleted;
                        }
                        //_db.Remove(sFItem);


                        item.Delete();
                        item.UpdateUniqueKey(_db);
                        this._db.SaveChanges();
                    }


                }
            }
            base.TraceLog("Delete", $"{pageName} details page deleted successfully");
            return RedirectToAction("SuccessAction", new { pageName = pageName });
        }
    }
}
