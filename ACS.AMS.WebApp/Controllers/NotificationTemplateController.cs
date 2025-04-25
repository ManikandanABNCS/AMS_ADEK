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
using ACS.AMS.WebApp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using NPOI.POIFS.FileSystem;
using ACS.WebAppPageGenerator.Models.SystemModels;
using System.Net.Http.Headers;
using ACS.AMS.WebApp.Classes;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using System.Web;
using System.Net.Mail;
using ACS.WebAppPageGenerator.Models.SystemModels;

namespace ACS.AMS.WebApp.Controllers
{
    public class NotificationTemplateController : MasterPageController
    {
        public NotificationTemplateController(IWebHostEnvironment _environment)
         : base(_environment)
        {
        }
        //public IActionResult Index()
        //{
        //    if (!base.HasRights(RightNames.NotificationTemplate, UserRightValue.View))
        //        return RedirectToAction("UnauthorizedPage");
        //    base.TraceLog("NotificationTemplate Index", $"{SessionUser.Current.Username} -NotificationTemplate Index page requested");
        //    return PartialView();
        //}
        //public IActionResult _Index([DataSourceRequest] DataSourceRequest request)
        //{
        //    if (!base.HasRights(RightNames.NotificationTemplate, UserRightValue.View))
        //        return RedirectToAction("UnauthorizedPage");

        //    var query = NotificationTemplateTable.GetAllItems(_db);
        //    var dsResult = query.ToDataSourceResult(request);
        //    base.TraceLog("NotificationTemplate Index", $"{SessionUser.Current.Username} -NotificationTemplate Index page Data Fetch");
        //    return Json(dsResult);
        //}
        protected override ActionResult DoCreatePage(string pageName, bool isPopupCreation = false)
        {
            var rightName = pageName;

            if (!base.HasRights(rightName, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("Create", $"{SessionUser.Current.Username} - {pageName} Create page requested");
            var obj = Activator.CreateInstance(GetEntityObjectType(pageName)) as BaseEntityObject;

            //var pageModel = GetTransactionEntryPageModel(pageName);
            //pageModel.EntityInstance = obj;

            //obj.CurrentPageID = SessionUser.Current.GetNextPageID();
            //pageModel.EntityLineItemInstance.CurrentPageID = obj.CurrentPageID;

            return PartialView("CreatePage",
                new EntryPageModel()
                {
                    PageTitle = pageName,
                    PageName = pageName,
                    EntityInstance = obj
                });
        }

        
        //    [HttpPost]
        //public async Task<ActionResult> NotificationTemplateCreate(IFormCollection collection, string pageName)
        //{
        //    return await ProcessDataCreation(collection, pageName);
        //}
        protected async override Task<ActionResult> ProcessDataCreation(IFormCollection collection, string pageName)
        {
            var rightName = pageName;

            if (!base.HasRights(rightName, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");

            var modelType = GetEntityObjectType(pageName);

            var item = Activator.CreateInstance(modelType) as BaseEntityObject;
            try
            {
                base.TraceLog("Create Post", $"{SessionUser.Current.Username} - {pageName} details will add to db ");

                var res = await base.TryUpdateModelAsync(item, modelType, "");
                string templateHeaderBodyContent = Convert.ToString(item.GetFieldValue("TemplateHeaderBodyContent"));
                item.SetFieldValue("TemplateHeaderBodyContent", HttpUtility.HtmlDecode(templateHeaderBodyContent));
                string templateDetailsBodyContent = Convert.ToString(item.GetFieldValue("TemplateDetailsBodyContent"));
                item.SetFieldValue("TemplateDetailsBodyContent", HttpUtility.HtmlDecode(templateDetailsBodyContent));
                string templateFooterBodyContent = Convert.ToString(item.GetFieldValue("TemplateFooterBodyContent"));
                item.SetFieldValue("TemplateFooterBodyContent", HttpUtility.HtmlDecode(templateFooterBodyContent));
                if (ModelState.IsValid)
                {
                    //Assign the values from posted data
                    //get the list of fields requires controls
                    item.ValidateUniqueKey(_db);
                    _db.Add(item);

                    //var NotificationType = collection["NotificationType"] + "";
                    //string[] arrRequestID = NotificationType.Split(',');
                    //arrRequestID = arrRequestID.Where(w => w != "").ToArray();
                    //int[] requestID = Array.ConvertAll(arrRequestID, s => int.Parse(s));
                    //int i = 0;
                    //foreach (int I in requestID)
                    //{
                    //    if (i == 0)
                    //    {
                    //        item.SetFieldValue("NotificationTypeID", I);
                    //    }
                    //    NotificationTemplateNotificationTypeTable notificationtemplatetype = new NotificationTemplateNotificationTypeTable();
                    //    notificationtemplatetype.NotificationTemplate = item as NotificationTemplateTable;
                    //    notificationtemplatetype.NotificationTypeID = I;
                    //    notificationtemplatetype.StatusID = (int)StatusValue.Active;
                    //    _db.Add(notificationtemplatetype);
                    //}

                    string selectedList = collection["hdTemplateSelectedItems"];
                    string[] templatefields = selectedList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (templatefields.Length > 0)
                    {
                        //MasterTable.deleteNotificationTemplateFieldTable(_db, int.Parse(user));
                        foreach (string fields in templatefields)
                        {
                            if (fields != string.Empty)
                            {
                                NotificationTemplateFieldTable TemplateFields = new NotificationTemplateFieldTable();
                                TemplateFields.NotificationTemplate = item as NotificationTemplateTable;
                                TemplateFields.NotificationFieldID = int.Parse(fields);
                                _db.Add(TemplateFields);
                            }
                        }
                    }

                    var ReportFormat = collection["AttachmentFormatID"] + "";
                    var Report = collection["ReportID"] + "";
                    var SourceField1 = collection["SourceField1"] + "";
                    var SourceField2 = collection["SourceField2"] + "";
                    var SourceField3 = collection["SourceField3"] + "";

                    var DestinationField1 = collection["DestinationField1"] + "";
                    var DestinationField2 = collection["DestinationField2"] + "";
                    var DestinationField3 = collection["DestinationField3"] + "";
                    if(!string.IsNullOrEmpty(ReportFormat) && !string.IsNullOrEmpty(Report))
                    {
                        NotificationReportAttachmentTable attachment = new NotificationReportAttachmentTable();
                        attachment.ReportID = int.Parse(Report);
                        attachment.AttachmentFormatID = int.Parse(ReportFormat);
                        attachment.NotificationTemplate = item as NotificationTemplateTable;
                        attachment.SourceField1 = SourceField1;
                        attachment.SourceField2 = SourceField2;
                        attachment.SourceField3= SourceField3;
                        attachment.DestinationField1= DestinationField1;
                        attachment.DestinationField2 = DestinationField2;
                        attachment.DestinationField3= DestinationField3;
                        _db.Add(attachment);

                    }
                    
                    //_db.SaveChanges();
                    var data = item as NotificationTemplateTable;
                   // item.SetFieldValue("NotificationTypeID", data.NotificationTemplateID);

                 _db.SaveChanges();
                    ViewData["pageName"] = pageName;
                    base.TraceLog("Create Post", $"{SessionUser.Current.Username} - {pageName} details saved to db ");
                    return PartialView("SuccessAction", new { pageName = pageName });
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

            return PartialView("CreatePage",
                new EntryPageModel()
                {
                    PageTitle = pageName,
                    PageName = pageName,
                    EntityInstance = item,
                    FormCollection = collection
                });
        }
        protected override ActionResult DoEditPage(int id, string pageName)
        {
            var rightName = pageName;

            if (!base.HasRights(rightName, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("Edit", $"{SessionUser.Current.Username} - {pageName} Edit page requested");
            var obj = Activator.CreateInstance(GetEntityObjectType(pageName)) as BaseEntityObject;
            var objInstance = obj as IACSDBObject;
            //var objName = Activator.CreateInstance(GetEntityObjectType("NotificationReportAttachmentTable")) as BaseEntityObject;
            //var objNameInstance = objName as IACSDBObject;

            var reportAttachment = NotificationReportAttachmentTable.GetReportAttachment(_db, id);

            return PartialView("EditPage",
                new EntryPageModel()
                {
                    PageTitle = pageName,
                    PageName = pageName,
                    EntityInstance = objInstance.GetItemByID(_db, id) as BaseEntityObject,
                    //EntityInstance1=objNameInstance.GetItemByID(_db, reportAttachment.NotificationReportAttachmentID) as BaseEntityObject
                });
        }
        protected async override Task<ActionResult> ProcessDataUpdation(IFormCollection data, long primaryKeyID, string pageName)
        {
            var rightName = pageName;

            if (!base.HasRights(rightName, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");

            var modelType = GetEntityObjectType(pageName);
            var item = Activator.CreateInstance(modelType) as BaseEntityObject;
            try
            {
                base.TraceLog("Edit Post", $"{pageName} details will modify to db : Entity id- {primaryKeyID}");
                var res = await base.TryUpdateModelAsync(item, modelType, "");
                var objInstance = item as IACSDBObject;
                var oldItem = objInstance.GetItemByID(_db, primaryKeyID) as BaseEntityObject;
                string templateHeaderBodyContent = Convert.ToString(item.GetFieldValue("TemplateHeaderBodyContent"));
                item.SetFieldValue("TemplateHeaderBodyContent", HttpUtility.HtmlDecode(templateHeaderBodyContent));
                string templateDetailsBodyContent = Convert.ToString(item.GetFieldValue("TemplateDetailsBodyContent"));
                item.SetFieldValue("TemplateDetailsBodyContent", HttpUtility.HtmlDecode(templateDetailsBodyContent));
                string templateFooterBodyContent = Convert.ToString(item.GetFieldValue("TemplateFooterBodyContent"));
                item.SetFieldValue("TemplateFooterBodyContent", HttpUtility.HtmlDecode(templateFooterBodyContent));
                if (ModelState.IsValid)
                {
                    DataUtilities.CopyObject(item, oldItem);
                    oldItem.ValidateUniqueKey(_db);

                    //var NotificationTemplateData = item as NotificationTemplateTable;
                    //var NotificationType = data["NotificationType"] + "";
                    //string[] arrRequestID = NotificationType.Split(',');
                    //arrRequestID = arrRequestID.Where(w => w != "").ToArray();
                    //int[] requestID = Array.ConvertAll(arrRequestID, s => int.Parse(s));
                    //var notificationtypes = NotificationTypeTable.GetAllItems(_db).ToList();

                    //foreach (NotificationTypeTable I in notificationtypes)
                    //{
                    //    var isselected = requestID.Any(c => c == I.NotificationTypeID);
                    //    var notificationtemplatetype = (from b in NotificationTemplateNotificationTypeTable.GetAllItems(_db)
                    //                                    where b.StatusID == (int)StatusValue.Active && b.NotificationTemplateID == NotificationTemplateData.NotificationTemplateID
                    //                                    && b.NotificationTypeID == I.NotificationTypeID
                    //                                    select b).FirstOrDefault();
                    //    if (isselected)
                    //    {

                    //        if (notificationtemplatetype == null)
                    //        {
                    //            NotificationTemplateNotificationTypeTable notitemplatetype = new NotificationTemplateNotificationTypeTable();
                    //            notitemplatetype.NotificationTemplateID = NotificationTemplateData.NotificationTemplateID;
                    //            notitemplatetype.NotificationTypeID = I.NotificationTypeID;
                    //            notitemplatetype.StatusID = (int)StatusValue.Active;
                    //            _db.Add(notitemplatetype);
                    //            //_db.SaveChanges();
                    //        }
                    //    }
                    //    else
                    //    {

                    //        if (notificationtemplatetype != null)
                    //        {
                    //            notificationtemplatetype.StatusID = (int)StatusValue.Inactive;
                    //            //_db.SaveChanges();
                    //        }
                    //    }
                    //}
                    string selectedList = data["hdTemplateSelectedItems"];
                    string[] templatefields = selectedList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (templatefields.Length > 0)
                    {
                        MasterTable.deleteNotificationTemplateFieldTable(_db, (int)primaryKeyID);
                        foreach (string fields in templatefields)
                        {
                            if (fields != string.Empty)
                            {
                                NotificationTemplateFieldTable TemplateFields = new NotificationTemplateFieldTable();
                                TemplateFields.NotificationTemplateID = (int)primaryKeyID;
                                TemplateFields.NotificationFieldID = int.Parse(fields);
                                _db.Add(TemplateFields);
                            }
                        }
                    }

                    var oldAttchment = NotificationReportAttachmentTable.GetAllItems(_db).Where(a => a.NotificationTemplateID == primaryKeyID);
                    var ReportFormat = data["AttachmentFormatID"] + "";
                    var Report = data["ReportID"] + "";
                    var SourceField1 = data["SourceField1"] + "";
                    var SourceField2 = data["SourceField2"] + "";
                    var SourceField3 = data["SourceField3"] + "";

                    var DestinationField1 = data["DestinationField1"] + "";
                    var DestinationField2 = data["DestinationField2"] + "";
                    var DestinationField3 = data["DestinationField3"] + "";
                    if (oldAttchment.Count()>0)
                    {
                        if (!string.IsNullOrEmpty(ReportFormat) && !string.IsNullOrEmpty(Report))
                        {
                            foreach (var line in oldAttchment)
                            {
                              
                                line.AttachmentFormatID = int.Parse(ReportFormat);
                                line.SourceField1 = SourceField1;
                                line.SourceField2 = SourceField2;
                                line.SourceField3 = SourceField3;
                                line.DestinationField1 = DestinationField1;
                                line.DestinationField2 = DestinationField2;
                                line.DestinationField3 = DestinationField3;
                                line.ReportID = int.Parse(Report);


                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(ReportFormat) && !string.IsNullOrEmpty(Report))
                        {
                            NotificationReportAttachmentTable attachment = new NotificationReportAttachmentTable();
                            attachment.ReportID = int.Parse(Report);
                            attachment.AttachmentFormatID = int.Parse(ReportFormat);
                            attachment.NotificationTemplateID =(int)primaryKeyID;
                            attachment.SourceField1 = SourceField1;
                            attachment.SourceField2 = SourceField2;
                            attachment.SourceField3 = SourceField3;
                            attachment.DestinationField1 = DestinationField1;
                            attachment.DestinationField2 = DestinationField2;
                            attachment.DestinationField3 = DestinationField3;
                            _db.Add(attachment);

                        }
                    }
                        _db.SaveChanges();

                        base.TraceLog("Edit Post", $"{pageName} details modified to db : Entity id- {primaryKeyID}");
                    ViewData["pageName"] = pageName;
                    return PartialView("SuccessAction", new { pageName = pageName });
                    
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

            return PartialView("EditPage",
                new EntryPageModel()
                {
                    PageTitle = pageName,
                    PageName = pageName,
                    EntityInstance = item
                });
        }
        protected override ActionResult DoDetailsPage(int id, string pageName)
        {
            var rightName = pageName;

            if (!base.HasRights(rightName, UserRightValue.Details))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("Details", $"{SessionUser.Current.Username} - {pageName} Details page requested");
            var obj = Activator.CreateInstance(GetEntityObjectType(pageName)) as BaseEntityObject;
            var objInstance = obj as IACSDBObject;

            return PartialView("DetailsPage",
                new EntryPageModel()
                {
                    PageTitle = pageName,
                    PageName = pageName,
                    EntityInstance = objInstance.GetItemByID(_db, id) as BaseEntityObject
                });
        }
        public IActionResult TemplateTableSelectionEntry(int notificationModuleID, int? NotificationTemplateID)
        {
            base.TraceLog("NotificationTemplate", $"{SessionUser.Current.Username} -  TemplateTableSelectionEntry requested");
            if (!base.HasRights(RightNames.NotificationTemplate, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");
            NotificationTemplateModel model = new NotificationTemplateModel();
            model.NotificationModuleID = notificationModuleID;
            model.NotificationTemplateID = NotificationTemplateID;
            return PartialView("TemplateTableSelectionEntry", model);
        }

        private TransactionEntryPageModel GetTransactionEntryPageModel(string pageName)
        {
            TransactionEntryPageModel newModel = new TransactionEntryPageModel()
            {
                EntityLineItemInstance = Activator.CreateInstance<TransactionLineItemTable>(),
                LineItemPageName = pageName + "LineItem",
                PageTitle = pageName,
                PageName = pageName,
                ControllerName = this.GetType().Name.Substring(0, this.GetType().Name.Length - "Controller".Length)
            };

            SystemDatabaseHelper.GenerateMasterGridColumns(typeof(TransactionLineItemData), newModel.LineItemPageName);

            

            return newModel;
        }
    }
}
