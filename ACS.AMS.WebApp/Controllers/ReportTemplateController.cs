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
    public class ReportTemplateController : ACSBaseController
    {
        public ReportTemplateController()
        {
            _RightName = RightNames.ReportTemplate;
        }

        protected virtual IQueryable<BaseEntityObject> GetAllItemsForIndex()
        {
            return ReportTemplateTable.GetAllItems(_db);
        }

        public IActionResult Index()
        {
            if (!base.HasRights(RightNames.ReportTemplate, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("ReportTemplate Index", $"{SessionUser.Current.Username} -ReportTemplate Index page requested");

            return PartialView();
        }
        
        public IActionResult _Index([DataSourceRequest] DataSourceRequest request)
        {
            if (!base.HasRights(RightNames.ReportTemplate, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");
            IQueryable<ReportTemplateTable> query = ReportTemplateTable.GetAllItems(_db);
            query = query.Where(a => a.QueryString != null);

            var dsResult = request.ToDataSourceResult(query);
            base.TraceLog("ReportTemplate Index", $"{SessionUser.Current.Username} -ReportTemplate Index page Data Fetch");
            return Json(dsResult);
        }

        //
        // GET: /ReportTemplate/Create
        public ActionResult Create()
        {
            if (!base.HasRights(_RightName, UserRightValue.Create))
                return GotoUnauthorizedPage();
            base.TraceLog("ReportTemplate create", $"{SessionUser.Current.Username} -ReportTemplate create page request");
            ReportTemplateTable model = new ReportTemplateTable();
            model.CurrentPageID = SessionUser.Current.GetNextPageID();
            return PartialView(model);
        }

        // POST: /ReportTemplate/Create
        [HttpPost]
        public ActionResult Create(ReportTemplateTable item, IFormCollection data)
        {
            if (!base.HasRights(_RightName, UserRightValue.Create))
                return GotoUnauthorizedPage();

            try
            {
                base.TraceLog("ReportTemplate create-post", $"{SessionUser.Current.Username} -ReportTemplate create-post request");
                //if (ModelState.IsValid)
                //{
                if(item.ObjectID==0)
                {
                    return base.ErrorActionResult("Query object Required ");
                }
                var obj = vwReportTemplateObjects.GetItem(_db, item.ObjectID);
                    var type = obj.ObjectType;
                    var procedureName = obj.ObjectName;

                    //make sure template name doesn't exists
                    var model = FilterFieldDataModel.GetCurrentModel(item.CurrentPageID);
                    item.QueryType = type;
                    item.QueryString = procedureName;
                item.ProcedureName = procedureName;
                    item.ReportTemplateFile = "Template1.rdl";
                    _db.Add(item);

                    //Query object should not be added already
                    if (ReportTemplateTable.IsQueryTextAlreadyExists(_db, item.QueryString, item.QueryType))
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
                        val.ReportTemplate = item;

                        _db.Add(val);
                    }

                    ScreenFilterTable sfItem = new ScreenFilterTable();
                    sfItem.ScreenFilterName = "Report_" + item.ReportTemplateName;
                    sfItem.ShowDynamicFields = false;
                    sfItem.CreatedDateTime = DateTime.Now;
                    sfItem.StatusID = (int)StatusValue.Active;
                sfItem.ReportTemplate = item as ReportTemplateTable;
                    _db.Add(sfItem);

                    var filters = model.FilterFields.Where(b => b.EnableFilter == true && b.FieldTypeID != null).ToList();
                var validate = model.FilterFields.Where(b => b.EnableFilter == true && b.FieldTypeID==null).ToList();
                if(validate.Count()>0)
                {
                    return base.ErrorActionResult($"Some enabled fields not select the field type .");
                }

                foreach (var modelLi in filters)
                    {
                        modelLi.CreatedDateTime = DateTime.Now;
                    modelLi.CreatedBy = SessionUser.Current.UserID;
                    if (modelLi.FieldTypeID.HasValue)
                    {
                        var datas = AFieldTypeTable.GetItem(_db, (int)modelLi.FieldTypeID);
                        if(datas!=null)
                        {
                            if(!string.IsNullOrEmpty(datas.ValueFieldName))
                            {
                                modelLi.FieldName = datas.ValueFieldName;
                                modelLi.QueryField = datas.ValueFieldName;
                            }
                            else
                            {
                                modelLi.FieldName = modelLi.DisplayName;
                            }
                        }
                        
                    }
                    
                   // modelLi = DateTime.Now;
                    modelLi.ScreenFilter = sfItem;
                        modelLi.ScreenFilterTypeID = 1;
                  

                        _db.Add(modelLi);
                    }

                    _db.SaveChanges();

                    //update Report Template IDs
                    sfItem.ParentID = item.ReportTemplateID;
                    sfItem.ParentType = "ReportTemplate";
                    _db.SaveChanges();
                base.TraceLog("ReportTemplate create-post", $"{SessionUser.Current.Username} -ReportTemplate create-post request done");
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

        // GET: /ReportTemplate/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                if (!base.HasRights(_RightName, UserRightValue.Edit))
                    return GotoUnauthorizedPage();

                base.TraceLog("ReportTemplate Edit", $"{SessionUser.Current.Username} -ReportTemplate Edit page request");
                ReportTemplateTable dept = ReportTemplateTable.GetItem(_db, id);
                dept.CurrentPageID = SessionUser.Current.GetNextPageID();

                LoadQueryObjectFields("Edit", dept.QueryString, dept.QueryType, dept.CurrentPageID, dept.ReportTemplateID, dept.ReportTemplateName);

                return PartialView(dept);
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }

        //// POST: /ReportTemplate/Edit/5
        [HttpPost]
        public ActionResult Edit(ReportTemplateTable item, IFormCollection data)
        {
            if (!base.HasRights(_RightName, UserRightValue.Edit))
                return GotoUnauthorizedPage();

            try
            {
                if (ModelState.IsValid)
                {
                    base.TraceLog("ReportTemplate edit-post", $"{SessionUser.Current.Username} -ReportTemplate edit-post request.id-{item.ReportTemplateID}");
                    ReportTemplateTable oldItm = ReportTemplateTable.GetItem(_db, item.ReportTemplateID);
                    string oldTemplateName = oldItm.ReportTemplateName;

                    DataUtilities.CopyObject(item, oldItm);

                    var model = FilterFieldDataModel.GetCurrentModel(item.CurrentPageID);

                    //Atleat one field is required
                    if (model.Fields.Count == 0)
                    {
                        return base.ErrorActionResult($"No field found on query object or session expired.");
                    }

                    var allDBTemplateFields = (from b in _db.ReportTemplateFieldTable
                                               where b.ReportTemplateID == item.ReportTemplateID
                                               select b).ToList();
                    if (allDBTemplateFields.Count > 0)
                    {
                        allDBTemplateFields.ForEach(e => e.StatusID = (int)StatusValue.Deleted);
                        _db.SaveChanges();
                    }

                    var fields = model.Fields.ToList();
                    foreach (var modelLi in fields)
                    {
                        ReportTemplateFieldTable screeItm = allDBTemplateFields.Where(m => m.ReportTemplateFieldID == modelLi.ReportTemplateFieldID).FirstOrDefault();
                        modelLi.ReportTemplateID = item.ReportTemplateID;

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

                    var sFItem = (from vv in _db.ScreenFilterTable
                                  where vv.ParentID == item.ReportTemplateID
                                        && vv.ParentType == "ReportTemplate"
                                  select vv).FirstOrDefault();
                    if (sFItem == null)
                    {
                        //create the filter object
                        var newID = ScreenFilterTable.CreateScreenFilterTable(null, "Report_" + item.ReportTemplateName, item.ReportTemplateID, "ReportTemplate");
                        sFItem = (from vv in _db.ScreenFilterTable
                                  where vv.ParentID == item.ReportTemplateID
                                        && vv.ParentType == "ReportTemplate"
                                  select vv).FirstOrDefault();
                    }

                    sFItem.ScreenFilterName = "Report_" + item.ReportTemplateName;
                   
                    //fetch all items including delete one
                    var allscreenFilters = from b in _db.ScreenFilterLineItemTable
                                           where b.ScreenFilterID == sFItem.ScreenFilterID
                                           select b;

                    foreach (var filterLI in allscreenFilters)
                    {
                        filterLI.StatusID = (int)StatusValue.Deleted;
                    }

                    var selectedFilters = model.FilterFields.Where(b => b.EnableFilter == true);
                    foreach (var modelLi in selectedFilters)
                    {
                        ScreenFilterLineItemTable screeItm = allscreenFilters.Where(q => q.QueryField == modelLi.QueryField).FirstOrDefault();
                        modelLi.FieldName = modelLi.DisplayName;
                       
                        if (screeItm != null)
                        {
                            //Already this line exists, just enable and assign its field details
                            var id = screeItm.ScreenFilterLineItemID;
                            DataUtilities.CopyObject(modelLi, screeItm);

                            screeItm.ScreenFilterLineItemID = id;
                            screeItm.ScreenFilterID = sFItem.ScreenFilterID;
                            screeItm.FieldName = modelLi.DisplayName;
                            screeItm.ScreenFilterTypeID = modelLi.ScreenFilterTypeID;
                            screeItm.StatusID = (byte)StatusValue.Active;
                            screeItm.CreatedDateTime = DateTime.Now;
                            if (screeItm.FieldTypeID.HasValue)
                            {
                                var datas = AFieldTypeTable.GetItem(_db, (int)screeItm.FieldTypeID);
                                if (datas != null)
                                {
                                    if (!string.IsNullOrEmpty(datas.ValueFieldName))
                                    {
                                        screeItm.FieldName = datas.ValueFieldName;
                                        screeItm.QueryField = datas.ValueFieldName;
                                    }
                                    else
                                    {
                                        screeItm.FieldName = modelLi.DisplayName;
                                    }
                                }

                            }
                        }
                        else
                        {
                            screeItm = new ScreenFilterLineItemTable();
                            _db.Add(screeItm);

                            DataUtilities.CopyObject(modelLi, screeItm);

                            screeItm.ScreenFilterID = sFItem.ScreenFilterID;
                            screeItm.CreatedDateTime = DateTime.Now;

                            if (screeItm.FieldTypeID.HasValue)
                            {
                                var datas = AFieldTypeTable.GetItem(_db, (int)screeItm.FieldTypeID);
                                if (datas != null)
                                {
                                    if (!string.IsNullOrEmpty(datas.ValueFieldName))
                                    {
                                        screeItm.FieldName = datas.ValueFieldName;
                                        screeItm.QueryField = datas.ValueFieldName;
                                    }
                                    else
                                    {
                                        screeItm.FieldName = modelLi.DisplayName;
                                    }
                                }

                            }

                        }
                    }

                    _db.SaveChanges();
                    base.TraceLog("ReportTemplate edit-post", $"{SessionUser.Current.Username} -ReportTemplate edit-post request done.id-{item.ReportTemplateID}");
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

        //GET: /ReportTemplate/Details/5
        public ActionResult Details(int id)
        {
            if (!base.HasRights(_RightName, UserRightValue.View))
                return GotoUnauthorizedPage();
            base.TraceLog("ReportTemplate details", $"{SessionUser.Current.Username} -ReportTemplate details request.id-{id}");
            ReportTemplateTable dept = ReportTemplateTable.GetItem(_db, id);
            dept.CurrentPageID = SessionUser.Current.GetNextPageID();

            LoadQueryObjectFields("View", dept.QueryString, dept.QueryType, dept.CurrentPageID, dept.ReportTemplateID, dept.ReportTemplateName);

            return PartialView(dept);
        }

        //ReportTemplate/Delete
       [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                if (!base.HasRights(_RightName, UserRightValue.Delete))
                    return ErrorActionResult(Language.GetErrorMessage("UnauthorizedAction"));
                base.TraceLog("ReportTemplate delete", $"{SessionUser.Current.Username} -ReportTemplate delete request.id-{id}");
                ReportTemplateTable item = ReportTemplateTable.GetItem(_db, id);

                if (item != null)
                {

                    var sFItem = (from vv in _db.ScreenFilterTable
                                  where vv.ScreenFilterName == "Report_" + item.ReportTemplateName
                                  select vv).FirstOrDefault();

                    if (sFItem != null)
                    {
                        var sFLineItem = (from vv in _db.ScreenFilterLineItemTable
                                          where vv.ScreenFilterID == sFItem.ScreenFilterID
                                          select vv);

                        foreach (ScreenFilterLineItemTable it in sFLineItem)
                        {
                            _db.Remove(it);
                        }
                        _db.Remove(sFItem);
                    }

                    item.Delete();
                    this._db.SaveChanges();
                }
                base.TraceLog("ReportTemplate delete", $"{SessionUser.Current.Username} -ReportTemplate delete request done.id-{id}");
                return Content("");
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }

        public IActionResult DeleteAll(string toBeDeleteIds)
        {
            var pageName = "ReportTemplate";

            base.TraceLog("DeleteAll", $"{SessionUser.Current.Username} - {pageName} Delete action requested");
            if (!base.HasRights(RightNames.ReportTemplate, UserRightValue.Delete))
                return RedirectToAction("UnauthorizedPage");

            string checkdItems = toBeDeleteIds;
            string[] arrRequestID = checkdItems.Split(',');
            int[] requestID = Array.ConvertAll(arrRequestID, s => int.Parse(s));
            int requestCount = requestID.Count();
            if (requestCount > 0)
            {
                foreach (var id in requestID)
                {
                    ReportTemplateTable item = ReportTemplateTable.GetItem(_db, id);

                    if (item != null)
                    {

                        var sFItem = (from vv in _db.ScreenFilterTable
                                      where vv.ScreenFilterName == "Report_" + item.ReportTemplateName
                                      select vv).FirstOrDefault();

                        if (sFItem != null)
                        {
                            var sFLineItem = (from vv in _db.ScreenFilterLineItemTable
                                              where vv.ScreenFilterID == sFItem.ScreenFilterID
                                              select vv);

                            foreach (ScreenFilterLineItemTable it in sFLineItem)
                            {
                                _db.Remove(it);
                            }
                            _db.Remove(sFItem);
                        }

                        item.Delete();
                        item.UpdateUniqueKey(_db);
                        this._db.SaveChanges();
                    }
                   
                }
            }
            base.TraceLog("Delete", $"{pageName} details page deleted successfully");
            return RedirectToAction("SuccessAction", new { pageName = pageName });
        }
        #region Load Field columns into data model

        #endregion

        public void LoadQueryObjectFields(string screen, string queryName, string queryType, int currentPageID, int? reportTemplateID = null, string templateName = null)
        {
            base.TraceLog("ReportTemplate LoadQueryObjectFields", $"{SessionUser.Current.Username} -LoadQueryObjectFields  request.queryName={queryName}");
            var filterDatamodel = FilterFieldDataModel.GetCurrentModel(currentPageID);
            string ConnectionString = AMSContext.ConnectionString;

            var dbObjectSchema = DBHelper.GetAllFields(queryName, queryType);

            filterDatamodel.Fields.Clear();
            filterDatamodel.FilterFields.Clear();

            bool isView = string.Compare(queryType, "View", true) == 0;
            if (!reportTemplateID.HasValue)
                reportTemplateID = -1;

            var templateFields = from b in _db.ReportTemplateFieldTable
                                 where b.ReportTemplateID == reportTemplateID.Value
                                 //&& b.ReportTemplateTable.StatusID == (int)StatusValue.Active
                                 select b;

            var allFields = (from col in dbObjectSchema.Columns
                                 //Join with the current availalbe fields
                             join p in templateFields
                             on col.Name equals p.FieldName into ps
                             from p in ps.DefaultIfEmpty()
                             where (!col.Name.EndsWith("ID") && !col.Name.Contains("Attribute") && !col.Name.Contains("CategoryL") && !col.Name.Contains("LocationL")
                             && col.DataType.ToString()!= "System.Int32" && col.DataType.ToString() != "System.Boolean" && !col.Name.Contains("DOF") 
                             && !col.Name.Contains("ERP") && !col.Name.Contains("QF") && !col.Name.Contains("Path")
                             )
                             orderby col.Name
                             select new ReportTemplateFieldTable
                             {
                                 DisplayTitle = (p == null) ? col.Name : p.DisplayTitle,
                                 FieldName = col.Name,
                                 QueryFieldName = col.Name,

                                 DefaultWidth = (p == null) ? 1 : p.DefaultWidth,
                                 FieldDataType = col.DataType.ToString(),
                                 DefaultFormat = (p == null) ? GetDefaultFormat(col.DataType) : p.DefaultFormat,

                                 FooterSumRequired = (p == null) ? IsNumericType(col.DataType) : p.FooterSumRequired,
                                 GroupSumRequired = (p == null) ? IsNumericType(col.DataType) : p.GroupSumRequired,

                                 ReportTemplateFieldID = (p == null) ? 0 : p.ReportTemplateFieldID,
                                 //this line available and mark it as active
                                 StatusID = (byte)StatusValue.Active
                             }).ToList();

            filterDatamodel.Fields.AddRange(allFields);

            var filterLineQry = from b in _db.ScreenFilterLineItemTable
                                where b.ScreenFilter.ParentID == reportTemplateID.Value
                                        && b.ScreenFilter.ParentType == "ReportTemplate"
                                select b;

            List<ScreenFilterLineItemTable> allFilterFields = null;

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
         
                               where ( isView==false || (!f.Name.EndsWith("ID") && !f.Name.Contains("Attribute") && !f.Name.Contains("CategoryL") && !f.Name.Contains("LocationL")
                                    && f.DataType.ToString() != "System.Int32" && f.DataType.ToString() != "System.Boolean" && !f.Name.Contains("DOF")
                             && !f.Name.Contains("ERP") && !f.Name.Contains("QF") && !f.Name.Contains("Path")
                                    ))
            select new ScreenFilterLineItemTable
            {
                                   DisplayName = f.Name,
                                   QueryField = f.Name,
                                   FieldDataType = f.DataType.ToString(),

                                   StatusID = p != null ? p.StatusID : (byte)StatusValue.Active,
                                   EnableFilter = p != null ? p.StatusID == (byte)StatusValue.Active ? true : false : false,

                                   ScreenFilterTypeID = (byte)(p == null ? 1 : p.ScreenFilterTypeID),
                                   FieldTypeID = p == null ? null : p.FieldTypeID,
                                   //For procedure all parameters are fixed one.
                                   IsFixedFilter = p == null ? (!isView) : p.IsFixedFilter,
                                   IsMandatory = p == null ? false : p.IsMandatory,

                                   ScreenFilterID = p != null ? p.ScreenFilterID : 0,
                                   ScreenFilterLineItemID = p != null ? p.ScreenFilterLineItemID : 0,
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
                    if (f.ScreenFilterLineItemID == 0)
                    {
                        f.EnableFilter = f.IsFixedFilter = true;
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
                        if (f.ScreenFilterLineItemID == 0)
                        {
                            f.EnableFilter = f.IsFixedFilter = true;
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

        public ActionResult ReportTemplateFilterFields(int currentPageID, int pageNumber = 0, bool detailsPage = false)
        {
            base.TraceLog("ReportTemplate ReportTemplateFilterFields", $"{SessionUser.Current.Username} -ReportTemplateFilterFields request.");
            ViewBag.CurrentPageID = currentPageID;
            ViewBag.PageNumber = pageNumber;
            ViewBag.DetailsPage = detailsPage;

            var filterDatamodel = FilterFieldDataModel.GetCurrentModel(currentPageID);

            ViewBag.FilterDataModel = filterDatamodel;

            if (detailsPage)
            {
                return PartialView("ReportTemplateFilterFieldsDetails");
            }

            return PartialView();
        }

        public ActionResult ReportTemplateFields(int currentPageID, int pageNumber = 0, bool detailsPage = false)
        {
            base.TraceLog("ReportTemplate ReportTemplateFields", $"{SessionUser.Current.Username} -ReportTemplateFields request.");
            ViewBag.CurrentPageID = currentPageID;
            ViewBag.PageNumber = pageNumber;

            var filterDatamodel = FilterFieldDataModel.GetCurrentModel(currentPageID);

            ViewBag.FilterDataModel = filterDatamodel;

            if (detailsPage)
            {
                return PartialView("ReportTemplateFieldsDetails");
            }

            return PartialView();
        }

        public ActionResult UpdateFilterField(string fieldName, int currentPageID, bool isEnabled, bool isMandatory, int? filterFieldID, byte filterTypeID)
        {
            try
            {
                base.TraceLog("ReportTemplate UpdateFilterField", $"{SessionUser.Current.Username} -UpdateFilterField request.fieldName-{fieldName}");
                var filterDetailsmodel = FilterFieldDataModel.GetCurrentModel(currentPageID);
                var lineItem = filterDetailsmodel.FilterFields.Where(b => b.QueryField == fieldName).FirstOrDefault();

                if (lineItem != null)
                {
                    lineItem.FieldTypeID = filterFieldID;
                    lineItem.EnableFilter = isEnabled;
                    lineItem.IsMandatory = isMandatory;
                    lineItem.ScreenFilterTypeID = filterTypeID;
                }
                else
                {
                    return ErrorActionResult("Invalid Field - " + fieldName);
                }

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
                base.TraceLog("ReportTemplate UpdateTemplateField", $"{SessionUser.Current.Username} -UpdateTemplateField request.fieldName-{fieldName}");
                var model = FilterFieldDataModel.GetCurrentModel(currentPageID);
                var lineItem = model.Fields.Where(b => b.FieldName == fieldName).FirstOrDefault();

                if (lineItem != null)
                {
                    lineItem.DefaultWidth = defaultWidth;
                    lineItem.DefaultFormat = defaultFormat;
                    lineItem.FooterSumRequired = footerSumRequired;
                }
                else
                {
                    return ErrorActionResult("Invalid Field - " + fieldName);
                }

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
                base.TraceLog("ReportTemplate MoveUpField", $"{SessionUser.Current.Username} -MoveUpField request.fieldName-{fieldName}");
                var filterDetailsmodel = FilterFieldDataModel.GetCurrentModel(currentPageID);
                var lineItem = filterDetailsmodel.FilterFields.Where(b => b.QueryField == fieldName).FirstOrDefault();

                var lineItemtoIncrease = default(ScreenFilterLineItemTable);

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
                    return ErrorActionResult("OrderNo already set as first Field - " + fieldName);
                }
                else
                {
                    return ErrorActionResult("Invalid Field - " + fieldName);
                }

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
                base.TraceLog("ReportTemplate MoveDownField", $"{SessionUser.Current.Username} -MoveDownField request.fieldName-{fieldName}");
                var filterDetailsmodel = FilterFieldDataModel.GetCurrentModel(currentPageID);
                var lineItem = filterDetailsmodel.FilterFields.Where(b => b.QueryField == fieldName).FirstOrDefault();
                //var lineItemtoDecrease = filterDetailsmodel.FilterFields.Where(b => b.QueryField != fieldName && b.OrderNo == (lineItem.OrderNo + 1)).FirstOrDefault();

                var lineItemtoDecrease = default(ScreenFilterLineItemTable);
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
                    return ErrorActionResult("Invalid Field - " + fieldName);
                }

                return Content("");
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }

    }
}
