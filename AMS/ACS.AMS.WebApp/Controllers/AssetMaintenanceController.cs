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
using ACS.WebAppPageGenerator.Models.SystemModels;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using ACS.AMS.WebApp.Models;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using ACS.AMS.WebApp.Classes;
using System.Configuration;
using System.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
//using Telerik.SvgIcons;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using System.Net.NetworkInformation;
using ACS.WebAppPageGenerator.Models.SystemModels;
using ACS.WebAppPageGenerator.Models.SystemModels;

namespace ACS.AMS.WebApp.Controllers
{
    public class AssetMaintenanceController : ACSBaseController
    {
        private static IWebHostEnvironment WebHostEnvironment;

        public AssetMaintenanceController(IWebHostEnvironment _environment)
        {
            WebHostEnvironment = _environment;
        }

        private PageFieldModel GetTransactionScheduleAgaistAMC()
        {
            return new PageFieldModel()
            {
                FieldName = "SourceTransactionScheduleID",
                DisplayLabel = "Activity",
                ControlType = PageControlTypes.MultiColumnComboBox,

                ControlName = "TransactionScheduleSelection",
                CascadeFrom = "SourceTransactionID",
                DataReadScriptFunctionName = "ActivityCascadeFromAMCSchedule",
                //ChangeMethodName = "ItemSelectionWithOutUnitPriceLoad",
            };
        }

        private TransactionEntryPageModel GetTransactionEntryPageModel(string pageName, BaseEntityObject obj)
        {
            TransactionEntryPageModel newModel = new TransactionEntryPageModel()
            {
                EntityLineItemInstance = Activator.CreateInstance<TransactionLineItemTable>(),
                LineItemPageName = pageName + "LineItem",
                PageTitle = pageName,
                PageName = pageName,
                ControllerName = this.GetType().Name.Substring(0, this.GetType().Name.Length - "Controller".Length),
                EntityInstance = obj,
            };

            SystemDatabaseHelper.GenerateMasterGridColumns(typeof(TransactionLineItemData), newModel.LineItemPageName);

            switch (pageName)
            {
                case "AssetMaintenance":
                    {
                        newModel.PageFields.Add(new PageFieldModel() { FieldName = "TransactionNo", DisplayLabel = "TransactionNo" });
                        newModel.PageFields.Add(new PageFieldModel() { FieldName = "ReferenceNo", DisplayLabel = "ReferenceNo" });
                        newModel.PageFields.Add(new PageFieldModel() { FieldName = "TransactionDate", DisplayLabel = "TransactionDate" });
                        newModel.PageFields.Add(new PageFieldModel() { FieldName = "Remarks", DisplayLabel = "Remarks" });
                        newModel.PageFields.Add(new PageFieldModel() { FieldName = "VendorID", DisplayLabel = "Vendor" });
                        newModel.PageFields.Add(new PageFieldModel() { FieldName = "ServiceDoneBy", DisplayLabel = "ServiceDoneBy" });
                        newModel.PageFields.Add(new PageFieldModel()
                        {
                            FieldName = "SourceTransactionID",
                            DisplayLabel = "AMCSchedule",
                            ControlType = PageControlTypes.MultiColumnComboBox,
                            ControlName = "AMCScheduleSelection",
                            ChangeMethodName = "OnAMCScheduleChange"
                        });
                        newModel.PageFields.Add(GetTransactionScheduleAgaistAMC());
                    }
                    break;

                default:
                    {
                        newModel.PageFields.Add(new PageFieldModel() { FieldName = "TransactionNo", DisplayLabel = "DocumentNo" });
                        newModel.PageFields.Add(new PageFieldModel() { FieldName = "TransactionDateTime", DisplayLabel = "Date" });
                    }
                    break;
            }

            //create the page fields
            var pageFields = obj.GetCreateScreenControls("", SessionUser.Current.UserID);
            PageGenerationHelper.CopyFieldsProperties(pageFields, newModel.PageFields);

            return newModel;
        }

        protected virtual EntryPageModel GetCreateEntryPageModel(string pageName, BaseEntityObject obj, IFormCollection collection = null)
        {
            var pageModel = GetTransactionEntryPageModel(pageName, obj);

            pageModel.FormCollection = collection;
            pageModel.ControllerName = this.GetType().Name.Substring(0, this.GetType().Name.Length - "Controller".Length);

            return pageModel;
        }

        public virtual IActionResult Index(string pageName)
        {
            if (!this.HasRights(RightNames.AssetMaintenance, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");

            this.TraceLog("Index", $"{SessionUser.Current.Username} - {pageName} Index page requested");
            Type entityType = GetEntityObjectType("Transaction", true);
            var obj = Activator.CreateInstance(entityType) as BaseEntityObject;
            SystemDatabaseHelper.GenerateMasterGridColumns(entityType, "AssetMaintenance");

            return PartialView("Index",
                new IndexPageModel()
                {
                    PageTitle = pageName,
                    PageName = pageName,
                    EntityInstance = obj,
                    ControllerName = this.GetType().Name.Replace("Controller", ""),
                });
        }

        public virtual IActionResult _Index([DataSourceRequest] DataSourceRequest request, string pageName)
        {
            try
            {
                if (!this.HasRights(RightNames.AssetMaintenance, UserRightValue.View))
                    return RedirectToAction("UnauthorizedPage");

                var query = TransactionTable.GetAllUserItems(_db, SessionUser.Current.UserID).Where(c => c.TransactionTypeID == (int)TransactionTypeValue.AssetMaintenance);
                var dsResult = request.ToDataSourceResult(query, "AssetMaintenance", "TransactionID");
                this.TraceLog("Index", $"{SessionUser.Current.Username} - {pageName} Index page Data Fetch");

                return Json(dsResult);
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }

        public IActionResult Create()
        {
            string pageName = "AssetMaintenance";
            if (!base.HasRights(RightNames.AssetMaintenance, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("Create", $"{SessionUser.Current.Username} - {pageName} Create page requested");
            var obj = Activator.CreateInstance(GetEntityObjectType("Transaction")) as BaseEntityObject;
            int currentPageID = SessionUser.Current.GetNextPageID();

            obj.CurrentPageID = currentPageID;
            TransactionLineItemDetailsModel.ClearCurrentModel(currentPageID);
            TransactionLineItemDetailsModel.GetCurrentModel(currentPageID);
            return PartialView("CreatePage", GetCreateEntryPageModel(pageName, obj));
            //return PartialView("CreatePage",
            //    new EntryPageModel()
            //    {
            //        PageTitle = pageName,
            //        PageName = pageName,
            //        EntityInstance = obj
            //    });
        }

        [HttpPost]
        public async Task<ActionResult> AssetMaintenanceCreate(IFormCollection collection, string pageName, int CurrentPageID)
        {
            return await ProcessDataCreation(collection, pageName, CurrentPageID);
        }

        protected async Task<ActionResult> ProcessDataCreation(IFormCollection collection, string pageName, int currentPageID)
        {
            if (!base.HasRights(RightNames.AssetMaintenance, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");

            var modelType = GetEntityObjectType("Transaction");
            var item = Activator.CreateInstance(modelType) as BaseEntityObject;

            try
            {
                base.TraceLog("Create Post", $"{SessionUser.Current.Username} - {pageName} details will add to db ");
                string ObjTransactionNo = collection["TransactionNo"];
                string ObjAssetReferenceNo = collection["ReferenceNo"];
                string Remarks = collection["Remarks"];
                string transactionDate = collection["TransactionDate"];
                string VendorData = collection["VendorID"] + "";
                string ServiceDoneBy = collection["ServiceDoneBy"];
                string sourceTransactionData = collection["SourceTransactionID"];
                string sourceTransactionScheduleData = collection["SourceTransactionScheduleID"];

                item.SetFieldValue("TransactionNo", "AUTO");
                item.SetFieldValue("ReferenceNo", ObjAssetReferenceNo);
                item.SetFieldValue("Remarks", Remarks);
                item.SetFieldValue("TransactionDate", transactionDate != null ? DateTime.ParseExact(transactionDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) : DateTime.Now);
                int? vendorID = String.IsNullOrEmpty(VendorData) ? null : int.Parse(VendorData);
                item.SetFieldValue("VendorID", vendorID);
                item.SetFieldValue("ServiceDoneBy", ServiceDoneBy);
                int? sourceTransactionID = String.IsNullOrEmpty(sourceTransactionData) ? null : int.Parse(sourceTransactionData);
                item.SetFieldValue("SourceTransactionID", sourceTransactionID);
                int? sourceTransactionScheduleID = String.IsNullOrEmpty(sourceTransactionScheduleData) ? null : int.Parse(sourceTransactionScheduleData);
                item.SetFieldValue("SourceTransactionScheduleID", sourceTransactionScheduleID);

                if (ModelState.IsValid)
                {
                    var AssetTransactionDetailsDataModel = TransactionLineItemDetailsModel.GetCurrentModel(currentPageID).LineItems;
                    if (AssetTransactionDetailsDataModel.Count == 0)
                        return base.ErrorActionResult("Atleast One LineItem Required");

                    string transactionNo = CodeGenerationHelper.GetNextCode("AssetMaintenance");
                    item.SetFieldValue("TransactionNo", transactionNo);
                    item.SetFieldValue("TransactionTypeID", (int)TransactionTypeValue.AssetMaintenance);
                    item.SetFieldValue("CreatedFrom", "Web");
                    item.SetFieldValue("PostingStatusID", (int)PostingStatusValue.CompletedByEndUser);
                    item.ValidateUniqueKey(_db);
                    _db.Add(item);

                    //add line items
                    foreach (var entry in AssetTransactionDetailsDataModel)
                    {
                        TransactionLineItemTable litem = new TransactionLineItemTable();
                        litem.Transaction = item as TransactionTable;
                        litem.AssetID = entry.AssetID;
                        litem.FromLocationID = entry.FromLocationID;
                        litem.Remarks = entry.MaintenanceRemarks;
                        litem.AdjustmentValue = entry.AdjustmentValue;
                        litem.IsAdjustmentNetBook = entry.IsNetBookAdjustment;
                        _db.Add(litem);
                    }

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
                return base.ErrorActionResult(ex);
            }

            item.CurrentPageID = currentPageID;
            return PartialView("CreatePage", GetCreateEntryPageModel(pageName, item));
        }

        public IActionResult Edit(int id)
        {
            string pageName = "AssetMaintenance";
            if (!base.HasRights(RightNames.AssetMaintenance, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("Edit", $"{SessionUser.Current.Username} - {pageName} Edit page requested.id-{id}");
            var obj = Activator.CreateInstance(GetEntityObjectType("Transaction")) as BaseEntityObject;
            var objInstance = obj as IACSDBObject;
            var model = GetTransactionEntryPageModel(pageName, obj);
            model.PageTitle = pageName;
            model.PageName = pageName;
            model.EntityInstance = objInstance.GetItemByID(_db, id) as BaseEntityObject;
            //BaseEntityObject EntityInstance = objInstance.GetItemByID(_db, id);
            //TransactionTable itm = EntityInstance as TransactionTable;

            int currentPageID = SessionUser.Current.GetNextPageID();
            ViewBag.CurrentPageID = currentPageID;
            TransactionLineItemDetailsModel.ClearCurrentModel(currentPageID);
            var AssetTransactionDetailsDataModel = TransactionLineItemDetailsModel.GetCurrentModel(currentPageID);

            AssetTransactionDetailsDataModel.LineItems = (from b in TransactionLineItemTable.GetAllItems(_db)
                                                          where b.TransactionID == id
                                                          select new TransactionLineItemModel()
                                                          {
                                                              TransactionLineItemID = b.TransactionLineItemID,
                                                              TransactionID = b.TransactionID,
                                                              AssetID = b.AssetID,
                                                              FromLocationID = b.FromLocationID,
                                                              AssetCode = b.Asset.AssetCode,
                                                              Barcode = b.Asset.Barcode,
                                                              MaintenanceRemarks = b.Remarks,
                                                              AdjustmentValue = b.AdjustmentValue != null ? (int)b.AdjustmentValue : 0,
                                                              IsNetBookAdjustment = b.IsAdjustmentNetBook
                                                          }).ToList();
            return PartialView("EditPage", model);
            //return PartialView("EditPage",
            //    new EntryPageModel()
            //    {
            //        PageTitle = pageName,
            //        PageName = pageName,
            //        EntityInstance = objInstance.GetItemByID(_db, id)
            //    });

        }

        protected async Task<ActionResult> ProcessDataUpdation(IFormCollection collection, long primaryKeyID, string pageName, int CurrentPageID)
        {
            ViewBag.CurrentPageID = CurrentPageID;
            if (!base.HasRights(RightNames.AssetMaintenance, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");

            var modelType = GetEntityObjectType("Transaction");
            var item = Activator.CreateInstance(modelType) as BaseEntityObject;
            try
            {
                base.TraceLog("Edit Post", $"{pageName} details will modify to db : Entity id- {primaryKeyID}");
                var res = await base.TryUpdateModelAsync(item, modelType, "");

                var objInstance = item as IACSDBObject;
                var oldItem = objInstance.GetItemByID(_db, (int)primaryKeyID) as BaseEntityObject;
                var olditemdata = oldItem as TransactionTable;
                item.SetFieldValue("TransactionNo", olditemdata.TransactionNo);

                //if (ModelState.IsValid)
                {
                    var AssetTransactionDetailsDataModel = TransactionLineItemDetailsModel.GetCurrentModel(CurrentPageID).LineItems;
                    if (AssetTransactionDetailsDataModel.Count == 0)
                        return base.ErrorActionResult("Atleast one lineItem required");

                    string transactionDate = collection["TransactionDate"];
                    item.SetFieldValue("TransactionDate", transactionDate != null ? DateTime.ParseExact(transactionDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) : DateTime.Now);

                    string ObjAssetReferenceNo = collection["ReferenceNo"];
                    string Remarks = collection["Remarks"];
                    string VendorData = collection["VendorID"];
                    int? vendorID = String.IsNullOrEmpty(VendorData) ? null : int.Parse(VendorData);
                    string ServiceDoneBy = collection["ServiceDoneBy"];

                    string sourceTransactionData = collection["SourceTransactionID"];
                    string sourceTransactionScheduleData = collection["SourceTransactionScheduleID"];

                    int? sourceTransactionID = String.IsNullOrEmpty(sourceTransactionData) ? null : int.Parse(sourceTransactionData);
                    item.SetFieldValue("SourceTransactionID", sourceTransactionID);
                    int? sourceTransactionScheduleID = String.IsNullOrEmpty(sourceTransactionScheduleData) ? null : int.Parse(sourceTransactionScheduleData);
                    item.SetFieldValue("SourceTransactionScheduleID", sourceTransactionScheduleID);

                    item.SetFieldValue("ReferenceNo", ObjAssetReferenceNo);
                    item.SetFieldValue("Remarks", Remarks);
                    item.SetFieldValue("VendorID", vendorID);
                    item.SetFieldValue("ServiceDoneBy", ServiceDoneBy);
                    item.SetFieldValue("TransactionTypeID", (int)TransactionTypeValue.AssetMaintenance);
                    item.SetFieldValue("CreatedFrom", "Web");
                    item.SetFieldValue("PostingStatusID", (int)PostingStatusValue.CompletedByEndUser);

                    DataUtilities.CopyObject(item, oldItem);

                    MasterTableQuery.deleteTransactionLineItemTable(_db, (int)primaryKeyID);
                    foreach (var entry in AssetTransactionDetailsDataModel)
                    {
                        TransactionLineItemTable litem = new TransactionLineItemTable();
                        litem.TransactionID = (int)primaryKeyID;
                        litem.AssetID = entry.AssetID;
                        litem.FromLocationID = entry.FromLocationID;
                        litem.Remarks = entry.MaintenanceRemarks;
                        litem.AdjustmentValue = entry.AdjustmentValue;
                        litem.IsAdjustmentNetBook = entry.IsNetBookAdjustment;
                        _db.Add(litem);
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
                return base.ErrorActionResult(ex);
            }
            return PartialView("EditPage", GetCreateEntryPageModel(pageName, item, collection));
        }

        [HttpPost]
        public async Task<ActionResult> AssetMaintenanceEdit(IFormCollection data, long primaryKeyID, string PageName, int CurrentPageID)
        {
            return await ProcessDataUpdation(data, primaryKeyID, PageName, CurrentPageID);
        }

        public IActionResult Details(int id)
        {
            var pageName = "AssetMaintenance";

            if (!base.HasRights(RightNames.AssetMaintenance, UserRightValue.Details))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("Details", $"{SessionUser.Current.Username} - {pageName} Detail page requested.id-{id}");
            var obj = Activator.CreateInstance(GetEntityObjectType("Transaction")) as BaseEntityObject;
            var objInstance = obj as IACSDBObject;
            var model = GetTransactionEntryPageModel(pageName, obj);
            model.PageTitle = pageName;
            model.PageName = pageName;

            model.EntityInstance = objInstance.GetItemByID(_db, id) as BaseEntityObject;

            int currentPageID = SessionUser.Current.GetNextPageID();
            ViewBag.CurrentPageID = currentPageID;
            TransactionLineItemDetailsModel.ClearCurrentModel(currentPageID);
            var AssetTransactionDetailsDataModel = TransactionLineItemDetailsModel.GetCurrentModel(currentPageID);
            //AssetTransactionDetailsDataModel = new AssetTransactionDetailsDataModel();

            AssetTransactionDetailsDataModel.LineItems = (from b in TransactionLineItemTable.GetAllItems(_db)
                                                          where b.TransactionID == id
                                                          select new TransactionLineItemModel()
                                                          {
                                                              TransactionLineItemID = b.TransactionLineItemID,
                                                              TransactionID = b.TransactionID,
                                                              AssetID = b.AssetID,
                                                              FromLocationID = b.FromLocationID,
                                                              AssetCode = b.Asset.AssetCode,
                                                              Barcode = b.Asset.Barcode,
                                                              MaintenanceRemarks = b.Remarks,
                                                              AdjustmentValue = b.AdjustmentValue != null ? (int)b.AdjustmentValue : 0,
                                                              IsNetBookAdjustment = b.IsAdjustmentNetBook

                                                          }).ToList();
            //AssetTransactionDetailsDataModel.AddRange(lines.ToArray());
            return PartialView("DetailsPage", model);
            //return PartialView("DetailsPage",
            //    new EntryPageModel()
            //    {
            //        PageTitle = pageName,
            //        PageName = pageName,
            //        EntityInstance = objInstance.GetItemByID(_db, id) as BaseEntityObject
            //    });
        }

        #region Line Item Method

        // public static AssetTransactionDetailsDataModel AssetTransactionDetailsDataModel;

        [HttpPost]
        public async Task<ActionResult> AddLineItem(IFormCollection collection, int? id, int currentPageID, string pageName, int assetID, string maintenanceRemarks, int adjustmentValue, bool isNetBookAdjustment)
        {
            try
            {
                base.TraceLog("AddLineItem", $"{SessionUser.Current.Username} - {pageName} lineitem details will add to model");
                if (adjustmentValue == 0)
                    return base.ErrorJsonResult("adjustmentValue should not be zero");
                var AssetTransactionDetailsDataModel = TransactionLineItemDetailsModel.GetCurrentModel(currentPageID);


                if (id != null && id != 0)
                {
                    if (AssetTransactionDetailsDataModel.LineItems.Where(c => c.AssetID == assetID && c.ID != id).Any())
                    {
                        return base.ErrorJsonResult("Asset already exists");
                        //return Content("Asset already exists");
                    }
                    var query = AssetTransactionDetailsDataModel.LineItems.Where(b => b.ID == id).FirstOrDefault();
                    if (query != null)
                    {
                        var UniformItems = (from b in _db.AssetNewView
                                            where b.AssetID == assetID
                                            select new TransactionLineItemModel()
                                            {
                                                AssetID = b.AssetID,
                                                AssetCode = b.AssetCode,
                                                Barcode = b.Barcode,
                                                FromLocationID = b.MappedLocationID
                                            }).FirstOrDefault();
                        query.AssetID = assetID;
                        query.AssetCode = UniformItems.AssetCode;
                        query.Barcode = UniformItems.Barcode;
                        query.FromLocationID = UniformItems.FromLocationID;
                        query.MaintenanceRemarks = maintenanceRemarks;
                        query.AdjustmentValue = adjustmentValue;
                        query.IsNetBookAdjustment = isNetBookAdjustment;
                    }
                }
                else
                {
                    if (AssetTransactionDetailsDataModel.LineItems.Where(c => c.AssetID == assetID).Any())
                    {
                        return base.ErrorActionResult("Asset already exists");
                        //return Content("Asset already exists");
                    }
                    var UniformItems = (from b in _db.AssetNewView
                                        where b.AssetID == assetID
                                        select new TransactionLineItemModel()
                                        {
                                            AssetID = b.AssetID,
                                            AssetCode = b.AssetCode,
                                            Barcode = b.Barcode,
                                            MaintenanceRemarks = maintenanceRemarks,
                                            AdjustmentValue = adjustmentValue,
                                            IsNetBookAdjustment = isNetBookAdjustment,
                                            FromLocationID = b.MappedLocationID
                                        }).FirstOrDefault();

                    AssetTransactionDetailsDataModel.LineItems.Add(UniformItems);
                }

                base.TraceLog("AddLineItem", $"{SessionUser.Current.Username} - {pageName} lineitem details added to model");

                return Content("");
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }
        public ActionResult LoadEditData(int currentPageID, int id)
        {
            try
            {
                base.TraceLog("LoadEditData", $"{SessionUser.Current.Username} - Asset Maintenance lineitem details fetch from model.id-{id}");
                var AssetTransactionDetailsDataModel = TransactionLineItemDetailsModel.GetCurrentModel(currentPageID);

                var target = AssetTransactionDetailsDataModel.LineItems.Where(p => p.ID == id);
                return Json(new { Success = true, result = target });
            }
            catch (Exception ex)
            {
                return base.ErrorActionResult(ex);
            }

        }
        public IActionResult _LineItemindex([DataSourceRequest] DataSourceRequest request, int currentPageID)
        {
            base.TraceLog("LineItemindex", $"{SessionUser.Current.Username} - Asset Maintenance lineitem details fetch for grid");
            var query = TransactionLineItemDetailsModel.GetCurrentModel(currentPageID).LineItems;
            var dsResult = query.ToDataSourceResult(request);
            // var dsResult = request.ToDataSourceResult(query.AsQueryable(), "AssetTransactionUniformItemEntry", "AssetTransactionLineItemID");
            this.TraceLog("Index", $"{SessionUser.Current.Username} - Index page Data Fetch");

            return Json(dsResult);
        }
        public IActionResult DeleteAllLineItem(string toBeDeleteIds, string pageName, int currentPageID)
        {

            if (!base.HasRights(RightNames.AssetMaintenance, UserRightValue.Delete))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("DeleteAll", $"{SessionUser.Current.Username} - {pageName} Delete action requested");

            var obj = Activator.CreateInstance(GetEntityObjectType("Transaction")) as IACSDBObject;


            string checkdItems = toBeDeleteIds;
            string[] arrRequestID = checkdItems.Split(',');
            int[] requestID = Array.ConvertAll(arrRequestID, s => int.Parse(s));
            var requestID1 = requestID.Distinct().ToList();
            int requestCount = requestID1.Count();
            var AssetTransactionDetailsDataModel = TransactionLineItemDetailsModel.GetCurrentModel(currentPageID);
            if (requestCount > 0)
            {
                foreach (var id in requestID1)
                {

                    var query = AssetTransactionDetailsDataModel.LineItems.Where(b => b.ID == id).FirstOrDefault();
                    if (query != null)
                        AssetTransactionDetailsDataModel.LineItems.Remove(query);

                }
            }
            base.TraceLog("DeleteAll", $"{pageName}  page deleted successfully");

            return Json("Success");
        }
        public IActionResult _DeleteLineItem(int id, int currentPageID)
        {
            base.TraceLog("DeleteLineItem", $"Asset Maintenance  page deleted clicked. id-{id}");
            var AssetTransactionDetailsDataModel = TransactionLineItemDetailsModel.GetCurrentModel(currentPageID);
            var query = AssetTransactionDetailsDataModel.LineItems.Where(b => b.ID == id).FirstOrDefault();
            if (query != null)
                AssetTransactionDetailsDataModel.LineItems.Remove(query);
            base.TraceLog("DeleteLineItem", $"Asset Maintenance  page deleted completed. id-{id}");
            return Content("");
        }


        #endregion Line Item Method

        public IActionResult Delete(int id)
        {
            try
            {


                if (!base.HasRights(RightNames.AssetMaintenance, UserRightValue.Delete))
                    return RedirectToAction("UnauthorizedPage");

                base.TraceLog("Delete", $"{SessionUser.Current.Username} - {"AssetMaintenance"} Delete action requested.id-{id}");
                var obj = Activator.CreateInstance(GetEntityObjectType("Transaction")) as IACSDBObject;
                var oldItem = obj.GetItemByID(_db, id) as BaseEntityObject;
                var objInstance = oldItem as IACSDBObject;

                oldItem.UpdateUniqueKey(_db);

                objInstance.DeleteObject();
                _db.SaveChanges();
                ViewData["pageName"] = "AssetMaintenance";
                base.TraceLog("Delete", $"AssetMaintenance details page deleted successfully {oldItem.GetPrimaryKeyFieldName()} {id}");
                return PartialView("SuccessAction", new { pageName = "AssetMaintenance" });
            }
            catch (ValidationException ex)
            {
                ViewData["FocusControl"] = ex.FieldName;
                ModelState.AddModelError("12", ex.Message);
                return base.ErrorActionResult(ex.Message);
            }
            catch (Exception ex)
            {
                return base.ErrorActionResult(ex);
            }

        }

        public IActionResult DeleteAll(string toBeDeleteIds)
        {
            try
            {

                if (!base.HasRights(RightNames.AssetMaintenance, UserRightValue.Delete))
                    return RedirectToAction("UnauthorizedPage");
                base.TraceLog("DeleteAll", $"{SessionUser.Current.Username} - {"AssetMaintenance"} Delete action requested");

                var obj = Activator.CreateInstance(GetEntityObjectType("Transaction")) as IACSDBObject;

                string checkdItems = toBeDeleteIds;
                string[] arrRequestID = checkdItems.Split(',');
                int[] requestID = Array.ConvertAll(arrRequestID, s => int.Parse(s));
                int requestCount = requestID.Count();

                if (requestCount > 0)
                {
                    foreach (var id in requestID)
                    {
                        var oldItem = obj.GetItemByID(_db, id) as BaseEntityObject;
                        var objInstance = oldItem as IACSDBObject;

                        oldItem.UpdateUniqueKey(_db);

                        objInstance.DeleteObject();
                        _db.SaveChanges();
                    }
                }

                base.TraceLog("DeleteAll", $"AssetMaintenance details page deleted successfully");
                return RedirectToAction("SuccessAction", new { pageName = "AssetMaintenance" });
            }
            catch (ValidationException ex)
            {
                ViewData["FocusControl"] = ex.FieldName;
                ModelState.AddModelError("12", ex.Message);
                return base.ErrorActionResult(ex.Message);
            }
            catch (Exception ex)
            {
                return base.ErrorActionResult(ex);
            }
        }

        #region Import excel
        [HttpPost]
        public async Task<ActionResult> ImportUpload(IFormFile fileNames, int currentPageID, int? sourceTransactionID)
        {
            try
            {
                base.TraceLog("ImportUpload", $"AssetMaintenance ImportUpload clicked");
                if (sourceTransactionID == null)
                    sourceTransactionID = 0;

                var filePath = await ReadDocument(fileNames);
                string filePaths = filePath.Item1;
                DataTable dt = filePath.Item2;
                TransactionLineItemDetailsModel model = TransactionLineItemDetailsModel.GetCurrentModel(currentPageID);
                List<string> barcodesList = new List<string>();
                List<string> MissingbarcodesList = new List<string>();
                List<string> AvaliablebarcodesList = new List<string>();
                List<string> ActivebarcodesList = new List<string>();
                List<string> UnMappedbarcodesList = new List<string>();
                List<string> SameLocMapping = new List<string>();
                List<string> MaintenanceRemarksEmptyList = new List<string>();
                List<string> AdjustmentValueList = new List<string>();
                List<string> NotMatchestheAMCScheduleAsset = new List<string>();
                int checkCnt = 0;
                var amcscheduleassets = (from b in _db.TransactionLineItemTable
                                         where b.TransactionID == (int)sourceTransactionID && b.StatusID == (int)StatusValue.Active
                                         select b.AssetID).ToList();
                foreach (DataRow row in dt.Rows)
                {
                    //if (checkCnt == 0)
                    //{
                    //    checkCnt++;
                    //    continue;
                    //}

                    TransactionLineItemModel item = new TransactionLineItemModel();
                    AssetTable asset = AssetTable.GetAssetBarcode(_db, row["Barcode"] + "");
                    bool canproceed = true;
                    if (asset != null)
                    {
                        if ((amcscheduleassets.Count() > 0 && !amcscheduleassets.Any(c => c == asset.AssetID)))
                        {
                            NotMatchestheAMCScheduleAsset.Add(asset.Barcode);
                            canproceed = false;
                        }
                        if (canproceed)
                        {
                            if (asset.StatusID == (int)StatusValue.Active)
                            {
                                if (!String.IsNullOrEmpty(row["MaintenanceRemarks"] + ""))
                                {
                                    if (!String.IsNullOrEmpty(row["MaintenanceRemarks"] + ""))
                                    {
                                        bool isNetBookAdjustment = String.IsNullOrEmpty(row["IsNetBookAdjustment"] + "") ? false : (row["IsNetBookAdjustment"] + "" == "true" ? true : false);
                                        int adjustmentValue = String.IsNullOrEmpty(row["AdjustmentValue"] + "") ? 0 : int.Parse(row["AdjustmentValue"] + "");
                                        var assetDetails = AssetNewView.GetItem(_db, (int)asset.AssetID);
                                        item.AssetID = assetDetails.AssetID;
                                        item.AssetCode = assetDetails.AssetCode;
                                        item.Barcode = assetDetails.Barcode;
                                        item.MaintenanceRemarks = row["MaintenanceRemarks"] + "";
                                        item.AdjustmentValue = adjustmentValue;
                                        item.IsNetBookAdjustment = isNetBookAdjustment;
                                        item.FromLocationID = assetDetails.MappedLocationID;
                                        model.LineItems.Add(item);
                                    }
                                    else
                                    {
                                        AdjustmentValueList.Add(asset.Barcode);
                                    }
                                }
                                else
                                {
                                    MaintenanceRemarksEmptyList.Add(asset.Barcode);
                                }

                            }
                            else
                            {
                                ActivebarcodesList.Add(asset.Barcode);
                            }
                        }
                    }
                    else
                    {
                        AvaliablebarcodesList.Add(row["Barcode"] + "");
                    }
                }
                string validation = "";
                foreach (var bar in NotMatchestheAMCScheduleAsset)
                {
                    validation = validation + bar + " Excel given barcode should be under selected AMC Schedule";
                }
                foreach (var bar in AvaliablebarcodesList)
                {
                    validation = validation + bar + " Excel given barcode not available in db";
                }
                foreach (var bar in barcodesList)
                {
                    validation = validation + bar + " Excel given barcode Location Type is Empty in db";
                }
                foreach (var bar in UnMappedbarcodesList)
                {
                    validation = validation + bar + " Excel given barcode not matched with configured user Mapping";
                }
                foreach (var bar in SameLocMapping)
                {
                    validation = validation + bar + " Excel given barcode different Locationtype";
                }
                foreach (var bar in MissingbarcodesList)
                {
                    validation = validation + bar + " Excel given barcode Location not Mapped in db ";
                }
                foreach (var bar in ActivebarcodesList)
                {
                    validation = validation + bar + " Excel given barcode Not in Active Status ";
                }
                foreach (var bar in MaintenanceRemarksEmptyList)
                {
                    validation = validation + bar + " Excel given barcode Not have Maintenance remarks ";
                }
                foreach (var bar in AdjustmentValueList)
                {
                    validation = validation + bar + " Excel given barcode either not have Adjustment value or it have 0 ";
                }

                if (validation == "")
                {
                    base.TraceLog("ImportUpload", $"AssetMaintenance ImportUpload completed data load to model");
                    return Json(new { Result = "success", Type = "Upload", FilePath = filePaths, error = "" });
                }
                else
                {
                    base.TraceLog("ImportUpload", $"AssetMaintenance ImportUpload completed data error");
                    TransactionLineItemDetailsModel.ClearCurrentModel(currentPageID);
                    //Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    //return Content(validation);
                    return Json(new { Result = "false", Type = "Upload", FilePath = "", error = validation });
                }
            }
            catch (Exception ex)
            {
                //AppErrorLogTable.SaveException(WasNowContext.CreateNewContext(), ex);
                ErrorActionResult(ex);
                //Response.StatusCode = (int)HttpStatusCode.BadRequest;
                //return Content(ex);
                return Json(new { Result = "false", Type = "Upload", FilePath = "", error = ex.Message });

            }
        }
        public async Task<Tuple<string, DataTable>> ReadDocument(IFormFile files)
        {
            base.TraceLog("ReadDocument", $"AssetMaintenance Read Document called");
            string path = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/AssetMaintenance");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var newFileName = Guid.NewGuid().ToString() + files.FileName;
            string fileName = Path.GetFileName(files.FileName);
            string NameTrimmed = String.Concat(newFileName.Where(c => !Char.IsWhiteSpace(c)));
            string filePath = Path.Combine(path, NameTrimmed);
            DataTable excelTable = new DataTable();

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                files.CopyTo(stream);
                stream.Position = 0;
                string FileName = Path.GetExtension(filePath);

                if (FileName == ".xls")
                {
                    HSSFWorkbook workbook = new HSSFWorkbook(stream);
                    excelTable = ImportExcel.Import(excelTable, workbook, _db);
                    //return excelTable;
                }
                else
                {
                    XSSFWorkbook workbooks = new XSSFWorkbook(stream);
                    excelTable = ImportExcel.Import(excelTable, workbooks, _db);
                    //return excelTable;
                }
            }

            Tuple<string, DataTable> obj = new Tuple<string, DataTable>(filePath, excelTable);
            base.TraceLog("ReadDocument", $"AssetMaintenance Read Document call completed");
            return obj;

        }

        public ActionResult ImportRemove(string[] fileNames, int currentPageID)
        {

            base.TraceLog("ImportRemove", $"AssetMaintenance ImportRemove clicked");
            TransactionLineItemDetailsModel model = TransactionLineItemDetailsModel.GetCurrentModel(currentPageID);
            if (fileNames != null)
            {
                if (model.LineItems.Count > 0)
                {
                    TransactionLineItemDetailsModel.ClearCurrentModel(currentPageID);
                }
            }
            return Json(new { Result = "success", Type = "Remove", FilePath = "" });
        }


        public FileResult DownloadFile(string fileName)
        {
            base.TraceLog("DownloadFile", $"AssetMaintenance DownloadFile clicked");
            //Build the File Path.
            string path = Path.Combine(WebHostEnvironment.WebRootPath, "ExcelTemplate\\") + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }
        #endregion
    }
}