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
using ACS.AMS.WebApp.Models.NotificationModule;

namespace ACS.AMS.WebApp.Controllers
{
    public class NotificationModuleController : MasterPageController
    {
        public NotificationModuleController(IWebHostEnvironment _environment):base(_environment)
        {
            _RightName = RightNames.NotificationModule;
        }

        protected override ActionResult DoCreatePage(string pageName, bool isPopupCreation = false)
        {
            if (!base.HasRights(pageName, UserRightValue.Create))
                return GotoUnauthorizedPage();
            base.TraceLog("NotificationModule Create", $"{SessionUser.Current.UserID}-Create page request");
            NotificationModuleTable model = new NotificationModuleTable();
            model.CurrentPageID = SessionUser.Current.GetNextPageID();
            ViewBag.PageName = pageName;
            return PartialView("Create", model);
           
        }
        [HttpPost]
        public async Task<ActionResult> NotificationModuleCreate(IFormCollection collection, NotificationModuleTable item, string pageName, int CurrentPageID)
        {
            return await ProcessDataCreation(collection, item, pageName, CurrentPageID);
        }


        protected async Task<ActionResult> ProcessDataCreation(IFormCollection collection, NotificationModuleTable item, string pageName, int currentPageID)
        {
            if (!base.HasRights(pageName, UserRightValue.Create))
                return GotoUnauthorizedPage();

            try
            {
                ViewBag.PageName = pageName;
                if (string.IsNullOrEmpty(item.QueryString))
                {
                    ModelState.AddModelError("QueryString", "QueryString is required");
                }
                if (ModelState.IsValid)
                {
                    base.TraceLog("NotificationModule Create-post", $"{SessionUser.Current.UserID}-details will save to db");
                    var obj = vwNotificationTemplateObjects.GetItem(_db, Convert.ToInt32(item.QueryString));
                    var type = obj.ObjectType;
                    var procedureName = obj.ObjectName;

                    //make sure template name doesn't exists
                    var model = NotificationFieldDataModel.GetCurrentModel(item.CurrentPageID);
                    item.QueryType = type;
                    item.QueryString = procedureName;

                    _db.Add(item);

                    //Atleat one field is required
                    if (model.Fields.Where(b => b.EnableField == true).ToList().Count == 0)
                    {
                        return base.ErrorActionResult($"No field found on query object or session expired.");
                    }
                    var Fields = model.Fields.Where(b => b.EnableField == true).ToList();
                    foreach (var val in Fields)
                    {
                        val.CreatedDateTime = DateTime.Now;
                        val.NotificationModule = item;

                        _db.Add(val);
                    }
                    _db.SaveChanges();
                    NotificationFieldDataModel.RemoveModel(item.CurrentPageID);
                    ViewData["pageName"] = pageName;
                    base.TraceLog("NotificationModule Create-post", $"{SessionUser.Current.UserID}-details  saved to db");
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

            return PartialView("Create",item);
        }

       
        protected override ActionResult DoEditPage(int id, string pageName)
        {
            try
            {
                if (!base.HasRights(pageName, UserRightValue.Edit))
                    return GotoUnauthorizedPage();
                base.TraceLog("NotificationModule Edit", $"{SessionUser.Current.UserID}-Edit page request");
                NotificationModuleTable dept = NotificationModuleTable.GetItem(_db, id);
                dept.CurrentPageID = SessionUser.Current.GetNextPageID();
                ViewBag.PageName = pageName;
                LoadQueryObjectFields("Edit", dept.QueryString, dept.QueryType, dept.CurrentPageID, dept.NotificationModuleID);

                return PartialView("Edit", dept);
                
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> NotificationModuleEdit(IFormCollection data, NotificationModuleTable item, string pageName, int CurrentPageID)
        {
            return await ProcessDataUpdation(data, item, pageName, CurrentPageID);
        }

        [HttpPost]
        protected async Task<ActionResult> ProcessDataUpdation(IFormCollection collection, NotificationModuleTable item, string pageName, int currentPageID)
       //public ActionResult Edit(ReportTemplateTable item, IFormCollection data)
        {
            if (!base.HasRights(_RightName, UserRightValue.Edit))
                return GotoUnauthorizedPage();

            try
            {
                ViewBag.PageName = pageName;
                if (ModelState.IsValid)
                {
                    base.TraceLog("NotificationModule Edit-post", $"{SessionUser.Current.UserID}-details  will modify to db");
                    NotificationModuleTable oldItm = NotificationModuleTable.GetItem(_db, item.NotificationModuleID);
                    string NotificationModule = oldItm.NotificationModule;

                    DataUtilities.CopyObject(item, oldItm);

                    var model = NotificationFieldDataModel.GetCurrentModel(item.CurrentPageID);

                    //Atleat one field is required
                    if (model.Fields.Where(b => b.EnableField == true).ToList().Count == 0)
                    {
                        return base.ErrorActionResult($"No field found on query object or session expired.");
                    }
                    MasterTable.deleteNotificationModuleFieldTable(_db,item.NotificationModuleID);
                    //var allDBTemplateFields = (from b in _db.NotificationModuleFieldTable
                    //                           where b.NotificationModuleID == item.NotificationModuleID 
                    //                           select b).ToList();
                    //allDBTemplateFields.ForEach(e => e.StatusID = (int)StatusValue.Deleted);

                    var fields = model.Fields.Where(c=>c.EnableField==true).ToList();
                    
                    foreach (var val in fields)
                    {
                        var notificationModuleFieldTable =(from b in _db.NotificationModuleFieldTable
                            where b.NotificationModuleID == item.NotificationModuleID && b.NotificationFieldID==val.NotificationFieldID
                                                           select b).FirstOrDefault();
                        if (notificationModuleFieldTable!=null)
                        {
                            notificationModuleFieldTable.StatusID = (byte)StatusValue.Active;
                        }
                        else
                        {
                            val.CreatedDateTime = DateTime.Now;
                            val.NotificationModuleID = item.NotificationModuleID;
                            val.NotificationFieldID = 0;
                            _db.Add(val);
                        }
                      
                    }
                    _db.SaveChanges();
                    NotificationFieldDataModel.RemoveModel(item.CurrentPageID);
                    ViewData["pageName"] = pageName;
                    base.TraceLog("NotificationModule Edit-post", $"{SessionUser.Current.UserID}-details  modified to db");
                    return PartialView("SuccessAction");
                    //return RedirectToAction("SuccessAction", new { pageName = pageName });
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

            return PartialView("Edit",item);
        }

        protected override ActionResult DoDetailsPage(int id, string pageName)
        {
            if (!base.HasRights(pageName, UserRightValue.View))
                return GotoUnauthorizedPage();
            base.TraceLog("NotificationModule Details", $"{SessionUser.Current.UserID}-details  page request");
            NotificationModuleTable dept = NotificationModuleTable.GetItem(_db, id);
            dept.CurrentPageID = SessionUser.Current.GetNextPageID();
            ViewBag.PageName = pageName;
            LoadQueryObjectFields("View", dept.QueryString, dept.QueryType, dept.CurrentPageID, dept.NotificationModuleID);

            return PartialView("Details", dept);
        }

      
        #region Load Field columns into data model

        public void LoadQueryObjectFields(string screen, string queryName, string queryType, int currentPageID, int? notificationModuleID = null)
        {
            base.TraceLog("NotificationModule LoadQueryObjectFields", $"{SessionUser.Current.UserID}-LoadQueryObjectFields request queryName-{queryName}");
            var filterDatamodel = NotificationFieldDataModel.GetCurrentModel(currentPageID);
            string ConnectionString = AMSContext.ConnectionString;

            var dbObjectSchema = DBHelper.GetAllFields(queryName, queryType);

            filterDatamodel.Fields.Clear();
         
            bool isView = string.Compare(queryType, "View", true) == 0;
            if (!notificationModuleID.HasValue)
                notificationModuleID = -1;

            var templateFields = from b in _db.NotificationModuleFieldTable
                                 where b.NotificationModuleID == notificationModuleID.Value
                                 && b.StatusID == (int)StatusValue.Active
                                 select b;

            var allFields = (from col in dbObjectSchema.Columns
                                 //Join with the current availalbe fields
                             join p in templateFields
                             on col.Name equals p.FieldName into ps
                             from p in ps.DefaultIfEmpty()
                             where (!col.Name.EndsWith("ID") && !col.Name.Contains("Attribute") && !col.Name.Contains("CategoryL") && !col.Name.Contains("LocationL")
                             && col.DataType.ToString() != "System.Int32" && col.DataType.ToString() != "System.Boolean" && !col.Name.Contains("DOF")
                             && !col.Name.Contains("ERP") && !col.Name.Contains("QF") && !col.Name.Contains("Path")
                             )
                             orderby col.Name
                             select new NotificationModuleFieldTable
                             {
                                 NotificationModuleID= (p == null) ? 0 : p.NotificationModuleID,
                                 DisplayName = (p == null) ? col.Name : p.DisplayName,
                                 FieldName = col.Name,
                                 NotificationFieldID = (p == null) ? 0 : p.NotificationFieldID,
                                 StatusID = (int)StatusValue.Active,
                                 EnableField  = p != null ? p.StatusID == (int)StatusValue.Active ? true : false : false,
                             }).ToList();

            filterDatamodel.Fields.AddRange(allFields);

            base.TraceLog("NotificationModule LoadQueryObjectFields", $"{SessionUser.Current.UserID}-LoadQueryObjectFields request queryName-{queryName} done");


        }
        public ActionResult UpdateFieldData(string fieldName, int currentPageID, bool isEnabled)
        {
            try
            {
                base.TraceLog("NotificationModule UpdateFieldData", $"{SessionUser.Current.UserID}-UpdateFieldData request fieldName-{fieldName}");
                var filterDetailsmodel = NotificationFieldDataModel.GetCurrentModel(currentPageID);
                var lineItem = filterDetailsmodel.Fields.Where(b => b.FieldName == fieldName).FirstOrDefault();

                if (lineItem != null)
                {
                  
                    lineItem.EnableField = isEnabled;
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


        public ActionResult NotificationModuleFields(int currentPageID, int pageNumber = 0, bool detailsPage = false)
        {
            base.TraceLog("NotificationModule NotificationModuleFields", $"{SessionUser.Current.UserID}-NotificationModuleFields request ");
            ViewBag.CurrentPageID = currentPageID;
            ViewBag.PageNumber = pageNumber;

            var Datamodel = NotificationFieldDataModel.GetCurrentModel(currentPageID);

            ViewBag.Datamodel = Datamodel;

            if (detailsPage)
            {
                return PartialView("NotificationModuleFieldsDetails");
            }

            return PartialView();
        }

        #endregion
    }
}
