using ACS.AMS.DAL;
using ACS.AMS.DAL.DBModel;
using ACS.AMS.WebApp.Classes;
using ACS.AMS.WebApp.Models;
using ACS.WebAppPageGenerator.Models.SystemModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
//using Telerik.SvgIcons;
using System.Globalization;

namespace ACS.AMS.WebApp.Controllers
{
    public class AMCScheduleController : ACSBaseController
    {
        private static IWebHostEnvironment WebHostEnvironment;

        public AMCScheduleController(IWebHostEnvironment _environment)
        {
            WebHostEnvironment = _environment;
        }
        public virtual IActionResult Index(string pageName)
        {
       
            if (!this.HasRights(RightNames.AMCSchedule, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");

            this.TraceLog("Index", $"{SessionUser.Current.Username} - {pageName} Index page requested");
            Type entityType = GetEntityObjectType("Transaction", true);
            var obj = Activator.CreateInstance(entityType) as BaseEntityObject;
            SystemDatabaseHelper.GenerateMasterGridColumns(entityType, "AMCSchedule");

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
                if (!this.HasRights(RightNames.AMCSchedule, UserRightValue.View))
                    return RedirectToAction("UnauthorizedPage");

                    var query = TransactionTable.GetAllUserItems(_db, SessionUser.Current.UserID).Where(c=>c.TransactionTypeID==(int)TransactionTypeValue.AMCSchedule);
                    var dsResult = request.ToDataSourceResult(query, "AMCSchedule", "TransactionID");
                    this.TraceLog("_Index", $"{SessionUser.Current.Username} - {pageName} Index page Data Fetch");

                    return Json(dsResult);
                

            }
            catch (Exception ex)
            {
                return base.ErrorActionResult(ex);
            }
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
                EntityInstance = obj
            };

            SystemDatabaseHelper.GenerateMasterGridColumns(typeof(TransactionLineItemData), newModel.LineItemPageName);

            switch (pageName)
            {
                case "AMCSchedule":
                    {
                        newModel.PageFields.Add(new PageFieldModel() { FieldName = "TransactionNo", DisplayLabel = "TransactionNo" });
                        newModel.PageFields.Add(new PageFieldModel() { FieldName = "ReferenceNo", DisplayLabel = "ReferenceNo" });
                        newModel.PageFields.Add(new PageFieldModel() { FieldName = "TransactionDate", DisplayLabel = "TransactionDate" });
                        newModel.PageFields.Add(new PageFieldModel() { FieldName = "Remarks", DisplayLabel = "Remarks" });
                        newModel.PageFields.Add(new PageFieldModel() { FieldName = "VendorID", DisplayLabel = "Vendor" });
                        newModel.PageFields.Add(new PageFieldModel() { FieldName = "TransactionStartDate", DisplayLabel = "ScheduleStartDate" });
                        newModel.PageFields.Add(new PageFieldModel() { FieldName = "TransactionEndDate", DisplayLabel = "ScheduleEndDate" });
                        newModel.PageFields.Add(new PageFieldModel() { FieldName = "TransactionValue", DisplayLabel = "TotalAMCValue" });
                    }
                    break;

                default:
                    {
                        newModel.PageFields.Add(new PageFieldModel() { FieldName = "TransactionNo", DisplayLabel = "DocumentNo" });
                        newModel.PageFields.Add(new PageFieldModel() { FieldName = "TransactionDateTime", DisplayLabel = "Date" });
                    }
                    break;
            }

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
        public IActionResult Create()
        {
            string pageName = "AMCSchedule";
            if (!base.HasRights(RightNames.AMCSchedule, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("Create", $"{SessionUser.Current.Username} - {pageName} Create page requested");
            var obj = Activator.CreateInstance(GetEntityObjectType("Transaction")) as BaseEntityObject;
            int currentPageID = SessionUser.Current.GetNextPageID();

            obj.CurrentPageID = currentPageID;
            TransactionLineItemDetailsModel.ClearCurrentModel(currentPageID);
            TransactionLineItemDetailsModel.GetCurrentModel(currentPageID);

            return PartialView("CreatePage", GetCreateEntryPageModel(pageName, obj));
        }
       
        [HttpPost]
        public async Task<ActionResult> AMCScheduleCreate(IFormCollection collection, string pageName, int CurrentPageID)
        {
            return await ProcessDataCreation(collection, pageName, CurrentPageID);
        }

        protected async Task<ActionResult> ProcessDataCreation(IFormCollection collection, string pageName, int currentPageID)
        {
            

            if (!base.HasRights(RightNames.AMCSchedule, UserRightValue.Create))
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
                string VendorData = collection["VendorID"]+"";
                string ServiceDoneBy = collection["ServiceDoneBy"];


                item.SetFieldValue("TransactionNo", "AUTO");
                item.SetFieldValue("ReferenceNo", ObjAssetReferenceNo);
                item.SetFieldValue("Remarks", Remarks);
                item.SetFieldValue("TransactionDate", transactionDate != null ? DateTime.ParseExact(transactionDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) : DateTime.Now);
                int? vendorID= String.IsNullOrEmpty(VendorData) ? null : int.Parse(VendorData);
                item.SetFieldValue("VendorID", vendorID);
                item.SetFieldValue("SerivceDoneBy", ServiceDoneBy);
                item.SetFieldValue("StatusID", (int)StatusValue.WaitingForApproval);
                if (ModelState.IsValid)
                {
                    var AssetTransactionDetailsDataModel = TransactionLineItemDetailsModel.GetCurrentModel(currentPageID).LineItems;
                    if (AssetTransactionDetailsDataModel.Count == 0)
                        return base.ErrorActionResult("Atleast One Asset Required");
                    var TransactionScheduleListDataModelList = TransactionScheduleListDataModel.GetCurrentModel(currentPageID).LineItems;
                    if (TransactionScheduleListDataModelList.Count == 0)
                        return base.ErrorActionResult("Atleast One Activity Required");

                    var assetIDs = String.Join(",", AssetTransactionDetailsDataModel.Select(u => u.AssetID));

                    var allvalid = TransactionTable.GetTransactionResult(_db, assetIDs, null, (int)ApproveModuleValue.AMCSchedule, RightNames.AMCSchedule);
                    if (!string.IsNullOrEmpty(allvalid.Result))
                    {
                        return base.ErrorActionResult(allvalid.Result);
                    }

                    string transactionNo = CodeGenerationHelper.GetNextCode("AMCSchedule");
                    item.SetFieldValue("TransactionNo", transactionNo);
                    item.SetFieldValue("TransactionTypeID", (int)TransactionTypeValue.AMCSchedule);
                    item.SetFieldValue("CreatedFrom", "Web");
                    item.SetFieldValue("PostingStatusID",(int)PostingStatusValue.WorkInProgress);
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

                    foreach (var entry in TransactionScheduleListDataModelList)
                    {
                        TransactionScheduleTable litem = new TransactionScheduleTable();
                        litem.Transaction = item as TransactionTable;
                        litem.Activity = entry.Activity;
                        litem.ActivityEndDate = entry.ActivityEndDate;
                        litem.ActivityStartDate = entry.ActivityStartDate;
                        _db.Add(litem);
                    }

                    _db.SaveChanges();
                    _db.Entry(item).Reload();
                    TransactionTable tran = (TransactionTable)item;
                    Tuple<int?, int?, int?, int?> historyData = TransactionTable.GetApprovalHistoryList(_db, AssetTransactionDetailsDataModel.Select(u => u.AssetID).ToList(), null);
                    ApproveWorkflowLineItemTable.ApproveHistoryMaintanance(_db, (int)historyData.Item2, tran.TransactionID, (int)ApproveModuleValue.AMCSchedule, SessionUser.Current.UserID, (int)historyData.Item1, null, null, historyData.Item4);


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
            return PartialView("CreatePage", GetCreateEntryPageModel(pageName, item, collection));
           
        }

        public IActionResult Edit(int id)
        {
            string pageName = "AMCSchedule";
            if (!base.HasRights(RightNames.AMCSchedule, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("Edit", $"{SessionUser.Current.Username} - {pageName} Edit page requested ");           

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
                                                            AssetCode = b.Asset.AssetCode,
                                                           Barcode = b.Asset.Barcode,
                                                           MaintenanceRemarks = b.Remarks,
                                                           AdjustmentValue=b.AdjustmentValue!=null? (int)b.AdjustmentValue:0,
                                                           IsNetBookAdjustment=b.IsAdjustmentNetBook
                                                        }).ToList();
           

            TransactionScheduleListDataModel.ClearCurrentModel(currentPageID);
            var TransactionScheduleListDataModelList = TransactionScheduleListDataModel.GetCurrentModel(currentPageID);


            TransactionScheduleListDataModelList.LineItems = (from b in TransactionScheduleTable.GetAllItems(_db)
                                                          where b.TransactionID == id
                                                          select new TransactionScheduleModel()
                                                          {
                                                              TransactionScheduleID = b.TransactionScheduleID,
                                                              TransactionID =(int) b.TransactionID,
                                                             
                                                              Activity = b.Activity,
                                                              ActivityStartDate = b.ActivityStartDate,
                                                              ActivityEndDate = b.ActivityEndDate,
                                                          }).ToList();
            return PartialView("EditPage", model);
            //return PartialView("EditPage",
            //        new EntryPageModel()
            //        {
            //            PageTitle = pageName,
            //            PageName = pageName,
            //            EntityInstance = objInstance.GetItemByID(_db, id)
            //        });
           
        }

        protected async Task<ActionResult> ProcessDataUpdation(IFormCollection collection, long primaryKeyID, string pageName, int CurrentPageID)
        {
            ViewBag.CurrentPageID = CurrentPageID;
            if (!base.HasRights(RightNames.AMCSchedule, UserRightValue.Edit))
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
                        return base.ErrorActionResult("Atleast one Asset required");

                    var TransactionScheduleListDataModelList = TransactionScheduleListDataModel.GetCurrentModel(CurrentPageID).LineItems;
                    if (TransactionScheduleListDataModelList.Count == 0)
                        return base.ErrorActionResult("Atleast One Activity Required");

                    string transactionDate = collection["TransactionDate"];
                    item.SetFieldValue("TransactionDate", transactionDate != null ? DateTime.ParseExact(transactionDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) : DateTime.Now);

                    string ObjAssetReferenceNo = collection["ReferenceNo"];
                    string Remarks = collection["Remarks"];
                    string VendorData = collection["VendorID"];
                    int? vendorID = String.IsNullOrEmpty(VendorData) ? null : int.Parse(VendorData);
                    string ServiceDoneBy = collection["ServiceDoneBy"];
                    //string PostingStatusID = collection["PostingStatusID"];

                    item.SetFieldValue("ReferenceNo", ObjAssetReferenceNo);
                    item.SetFieldValue("Remarks", Remarks);
                    item.SetFieldValue("VendorID", vendorID);
                    item.SetFieldValue("ServiceDoneBy", ServiceDoneBy);
                    item.SetFieldValue("TransactionTypeID", (int)TransactionTypeValue.AMCSchedule);
                    item.SetFieldValue("CreatedFrom", "Web");
                    item.SetFieldValue("PostingStatusID", (int)PostingStatusValue.WorkInProgress);

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
                    MasterTableQuery.deleteTransactionScheduleTable(_db, (int)primaryKeyID);
                    foreach (var entry in TransactionScheduleListDataModelList)
                    {
                        TransactionScheduleTable litem = new TransactionScheduleTable();
                        litem.TransactionID = (int)primaryKeyID;
                        litem.Activity = entry.Activity;
                        litem.ActivityEndDate = entry.ActivityEndDate;
                        litem.ActivityStartDate = entry.ActivityStartDate;
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
            //return PartialView("EditPage",
            //    new EntryPageModel()
            //    {
            //        PageTitle = pageName,
            //        PageName = pageName,
            //        EntityInstance = item
            //    });
        }

        [HttpPost]
        public async Task<ActionResult> AMCScheduleEdit(IFormCollection data, long primaryKeyID, string PageName, int CurrentPageID)
        {
            return await ProcessDataUpdation(data, primaryKeyID, PageName, CurrentPageID);
        }

        public IActionResult Details(int id)
        {
            var pageName = "AMCSchedule";

            if (!base.HasRights(RightNames.AMCSchedule, UserRightValue.Details))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("Details", $"{SessionUser.Current.Username} - {pageName} details page requested Entity id- {id}");
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

            AssetTransactionDetailsDataModel.LineItems = (from b in TransactionLineItemTable.GetAllItems(_db)
                                                        where b.TransactionID == id
                                                        select new TransactionLineItemModel()
                                                        {
                                                           TransactionLineItemID = b.TransactionLineItemID,
                                                           TransactionID = b.TransactionID,
                                                            AssetID = b.AssetID,
                                                            AssetCode = b.Asset.AssetCode,
                                                            Barcode = b.Asset.Barcode,
                                                            MaintenanceRemarks = b.Remarks,
                                                            AdjustmentValue = b.AdjustmentValue !=null?(int)b.AdjustmentValue:0,
                                                            IsNetBookAdjustment=b.IsAdjustmentNetBook

                                                        }).ToList();

            TransactionScheduleListDataModel.ClearCurrentModel(currentPageID);
            var TransactionScheduleListDataModelList = TransactionScheduleListDataModel.GetCurrentModel(currentPageID);


            TransactionScheduleListDataModelList.LineItems = (from b in TransactionScheduleTable.GetAllItems(_db)
                                                              where b.TransactionID == id
                                                              select new TransactionScheduleModel()
                                                              {
                                                                  TransactionScheduleID = b.TransactionScheduleID,
                                                                  TransactionID = (int)b.TransactionID,

                                                                  Activity = b.Activity,
                                                                  ActivityStartDate = b.ActivityStartDate,
                                                                  ActivityEndDate = b.ActivityEndDate,
                                                              }).ToList();
            return PartialView("DetailsPage", model);
        }

        #region Line Item Method

      

        [HttpPost]
        public async Task<ActionResult> AddLineItem(IFormCollection collection, int? id,int currentPageID, string pageName,  int assetID, string maintenanceRemarks, int adjustmentValue, bool isNetBookAdjustment)
        {
            try
            {
                base.TraceLog("AddLineItem", $"{SessionUser.Current.Username} - {pageName} details will add to model ");
                if (adjustmentValue == 0)
                    return base.ErrorActionResult("adjustmentValue should not be zero");
                var AssetTransactionDetailsDataModel = TransactionLineItemDetailsModel.GetCurrentModel(currentPageID);
               
       
                if (id!=null && id!=0)
                {
                    if (AssetTransactionDetailsDataModel.LineItems.Where(c => c.AssetID == assetID && c.ID != id).Any())
                    {
                        return base.ErrorActionResult("Asset already exists");
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
                                                LocationType=b.LocationType,
                                                FromLocationID = b.MappedLocationID
                                            }).FirstOrDefault();

                        query.AssetID = UniformItems.AssetID;
                        query.AssetCode = UniformItems.AssetCode;
                        query.Barcode = UniformItems.Barcode;
                        query.LocationType = UniformItems.LocationType;
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
                                            LocationType = b.LocationType,
                                            FromLocationID = b.MappedLocationID,
                                            MaintenanceRemarks = maintenanceRemarks,
                                            AdjustmentValue = adjustmentValue,
                                            IsNetBookAdjustment = isNetBookAdjustment,
                                        }).FirstOrDefault();
                    
                    

                    AssetTransactionDetailsDataModel.LineItems.Add(UniformItems);
                }
                base.TraceLog("AddLineItem", $"{SessionUser.Current.Username} - {pageName} details added to model. id-{id} ");


                return Content("");
            }
            catch (Exception ex)
            {
                return base.ErrorActionResult(ex);
            }
        }
        public ActionResult LoadEditData(int currentPageID, int id)
        {
            try
            {
                var AssetTransactionDetailsDataModel = TransactionLineItemDetailsModel.GetCurrentModel(currentPageID);

                var target = AssetTransactionDetailsDataModel.LineItems.Where(p => p.ID == id);
                base.TraceLog("EditLineItem", $"{SessionUser.Current.Username} - AMCSchedule details fetched from model to edit.id-{id}");
                return Json(new { Success = true, result = target });
            }
            catch (Exception ex)
            {
                return base.ErrorActionResult(ex);
            }

        }
        public IActionResult _LineItemindex([DataSourceRequest] DataSourceRequest request, int currentPageID)
        {

            var query = TransactionLineItemDetailsModel.GetCurrentModel(currentPageID).LineItems;
            foreach (var line in query.ToList())
            {
               
                var asset = AssetNewView.GetItem(_db, line.AssetID);
                line.AssetCode = asset.AssetCode;
                line.Barcode = asset.Barcode;
                line.CategoryHierarchy = asset.CategoryHierarchy;
                line.LocationType = asset.LocationType;
                line.LocationHierarchy = asset.LocationHierarchy;
                line.AssetDescription = asset.AssetDescription;
            }

            var dsResult = query.ToDataSourceResult(request);
            // var dsResult = request.ToDataSourceResult(query.AsQueryable(), "AssetTransactionUniformItemEntry", "AssetTransactionLineItemID");
            this.TraceLog("LineItemindex", $"{SessionUser.Current.Username} - LineItemindex page Data Fetch");

            return Json(dsResult);
        }
        public IActionResult DeleteAllLineItem(string toBeDeleteIds, string pageName,int currentPageID)
        {
            try
            {
                if (!base.HasRights(RightNames.AMCSchedule, UserRightValue.Delete))
                    return RedirectToAction("UnauthorizedPage");
                base.TraceLog("DeleteAll", $"{SessionUser.Current.Username} - {pageName} Delete All Lineitem action requested");

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
                base.TraceLog("DeleteAll", $"{SessionUser.Current.UserID}- {pageName}  page deleted successfully");

                return Json("Success");
            }
            catch (ValidationException ex)
            {

                return base.ErrorActionResult(ex);
            }
            catch (Exception ex)
            {

                return base.ErrorActionResult(ex);
            }
        }
        public IActionResult _DeleteLineItem(int id, int currentPageID)
        {
            base.TraceLog("DeleteLineItem", $"{SessionUser.Current.Username} - AMcSchedule Delete Lineitem action requested.id-{id}");
            var AssetTransactionDetailsDataModel = TransactionLineItemDetailsModel.GetCurrentModel(currentPageID);
            var query = AssetTransactionDetailsDataModel.LineItems.Where(b => b.ID == id).FirstOrDefault();
            if (query != null)
                AssetTransactionDetailsDataModel.LineItems.Remove(query);
            base.TraceLog("DeleteLineItem", $"{SessionUser.Current.Username} - AMcSchedule Delete Lineitem done id-{id}");
            return Content("");
        }


        #endregion Line Item Method

        #region Activity Line Item Method

      
        [HttpPost]
        public async Task<ActionResult> AddActivity(IFormCollection collection, int? id, int currentPageID, string pageName, string activityRemarks, string activityStartDate, string activityEndDate)
        {
            try
            {
                base.TraceLog("AddActivity", $"{SessionUser.Current.Username} - AMCSchedule AddActivity method requested");

                var TransactionScheduleListModel = TransactionScheduleListDataModel.GetCurrentModel(currentPageID);


                if (id != null && id != 0)
                {
                    
                    var query = TransactionScheduleListModel.LineItems.Where(b => b.ID == id).FirstOrDefault();

                    if (query != null)
                    {
                       
                        query.Activity = activityRemarks;
                        query.ActivityStartDate = DateTime.ParseExact(activityStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        query.ActivityEndDate = DateTime.ParseExact(activityEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                }
                else
                {
                   
                    var activity = new TransactionScheduleModel()
                    {
                        Activity = activityRemarks,
                        ActivityStartDate = DateTime.ParseExact(activityStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ActivityEndDate = DateTime.ParseExact(activityEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    };
                    TransactionScheduleListModel.LineItems.Add(activity);
                }
                base.TraceLog("AddActivity", $"{SessionUser.Current.Username} - {pageName} details added to model ");


                return Content("");
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }

        public ActionResult LoadActivityEditData(int currentPageID, int id)
        {
            try
            {
                var transactionScheduleListDataModel = TransactionScheduleListDataModel.GetCurrentModel(currentPageID);

                var target = transactionScheduleListDataModel.LineItems.Where(p => p.ID == id).Select(c=>new
                {
                    ID=c.ID,
                    Activity =c.Activity ,
                    ActivityStartDate = c.ActivityStartDate.ToString("dd/MM/yyyy"),
                    ActivityEndDate = c.ActivityEndDate.ToString("dd/MM/yyyy"),
                });
                base.TraceLog("EditActivity", $"{SessionUser.Current.Username} - AMCSchedule details fetched from model. id-{id} ");
                return Json(new { Success = true, result = target });
            }
            catch (Exception ex)
            {
                var erroID = ApplicationErrorLogTable.SaveException(ex);
                return ErrorActionResult(ex);
            }

        }
        public IActionResult _ActivityLineItemindex([DataSourceRequest] DataSourceRequest request, int currentPageID)
        {
         
            var query = TransactionScheduleListDataModel.GetCurrentModel(currentPageID).LineItems;
            var dsResult = query.ToDataSourceResult(request);
            this.TraceLog("ActivityLineItem", $"{SessionUser.Current.Username} - ActivityLineItem page Data Fetch");

            return Json(dsResult);
        }
        public IActionResult DeleteAllActivity(string toBeDeleteIds, string pageName, int currentPageID)
        {

            if (!base.HasRights(RightNames.AMCSchedule, UserRightValue.Delete))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("DeleteAllActivity", $"{SessionUser.Current.Username} - {pageName} Delete action requested");

            string checkdItems = toBeDeleteIds;
            string[] arrRequestID = checkdItems.Split(',');
            int[] requestID = Array.ConvertAll(arrRequestID, s => int.Parse(s));
            var requestID1 = requestID.Distinct().ToList();
            int requestCount = requestID1.Count();
            var transactionScheduleListDataModel = TransactionScheduleListDataModel.GetCurrentModel(currentPageID);
            if (requestCount > 0)
            {
                foreach (var id in requestID1)
                {

                    var query = transactionScheduleListDataModel.LineItems.Where(b => b.ID == id).FirstOrDefault();
                    if (query != null)
                        transactionScheduleListDataModel.LineItems.Remove(query);

                }
            }
            base.TraceLog("DeleteAllActivity", $"{pageName}  page deleted successfully");

            return Json("Success");
        }
        public IActionResult _DeleteActivity(int id, int currentPageID)
        {
            var transactionScheduleListDataModel = TransactionScheduleListDataModel.GetCurrentModel(currentPageID);
            var query = transactionScheduleListDataModel.LineItems.Where(b => b.ID == id).FirstOrDefault();
            if (query != null)
                transactionScheduleListDataModel.LineItems.Remove(query);
            this.TraceLog("DeleteActivity", $"{SessionUser.Current.Username} - ActivityLineItem deleted. id-{id}");
            return Content("");
        }



        #endregion
        public IActionResult Delete(int id)
        {
            try
            {


                if (!base.HasRights(RightNames.AMCSchedule, UserRightValue.Delete))
                    return RedirectToAction("UnauthorizedPage");

                base.TraceLog("Delete", $"{SessionUser.Current.Username} - {"AMCSchedule"} Delete action requested.id-{id}");
                var obj = Activator.CreateInstance(GetEntityObjectType("Transaction")) as IACSDBObject;
                var oldItem = obj.GetItemByID(_db, id) as BaseEntityObject;
                var objInstance = oldItem as IACSDBObject;

                oldItem.UpdateUniqueKey(_db);

                objInstance.DeleteObject();
                _db.SaveChanges();
                ViewData["pageName"] = "AMCSchedule";
                base.TraceLog("Delete", $"AMCSchedule details page deleted successfully {oldItem.GetPrimaryKeyFieldName()} {id}");
                return PartialView("SuccessAction", new { pageName = "AMCSchedule" });
            }
            catch (ValidationException ex)
            {
                
                return base.ErrorActionResult(ex);
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

                if (!base.HasRights(RightNames.AMCSchedule, UserRightValue.Delete))
                    return RedirectToAction("UnauthorizedPage");
                base.TraceLog("DeleteAll", $"{SessionUser.Current.Username} - {"AMCSchedule"} Delete action requested");

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

                base.TraceLog("DeleteAll", $"AMCSchedule details page deleted successfully");
                return RedirectToAction("SuccessAction", new { pageName = "AMCSchedule" });
            }
            catch (ValidationException ex)
            {
                return base.ErrorActionResult(ex);
            }
            catch (Exception ex)
            {
                return base.ErrorActionResult(ex);
            }
        }


        #region Import excel
        [HttpPost]
        public async Task<ActionResult> ImportUpload(IFormFile fileNames, int currentPageID)
        {
            try
            {
                base.TraceLog("ImportUpload", $"AMCSchedule ImportUpload method clicked");
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
                int checkCnt = 0;
                foreach (DataRow row in dt.Rows)
                {
                    //if (checkCnt == 0)
                    //{
                    //    checkCnt++;
                    //    continue;
                    //}
                    TransactionLineItemModel item = new TransactionLineItemModel();
                    AssetTable asset = AssetTable.GetAssetBarcode(_db, row["Barcode"] + "");
                    if (asset != null)
                    {
                        if (asset.StatusID == (int)StatusValue.Active)
                        {
                            IQueryable<AssetNewView> views = AssetNewView.GetAllUserItem(_db, SessionUser.Current.UserID, true).Where(a => a.StatusID == (int)StatusValue.Active);
                            var mapping = views.Where(a => a.AssetID == asset.AssetID).FirstOrDefault();
                            if (mapping != null)
                            {
                                var query = views.Where(a => /*a.LocationType != null &&*/ a.StatusID == (int)StatusValue.Active && a.AssetID == asset.AssetID).FirstOrDefault();
                                if (query != null)
                                {
                                    var userBasedLocation = PersonTable.GetUserBasedLocationList(_db, SessionUser.Current.UserID).Select(a => a.LocationID + "").ToList();
                                    if (userBasedLocation.Count() > 0)
                                    {
                                        if (userBasedLocation.Contains(query.LocationL2))
                                        {
                                            if (model.LineItems.Count() > 0)
                                            {
                                                List<int> ids = model.LineItems.Select(a => a.Asset.AssetID).ToList();
                                                var list = AssetNewView.GetAllItems(_db).Where(a => ids.Contains(a.AssetID)).Select(a => a.LocationType).Distinct().ToList();
                                                var res = AssetNewView.GetItem(_db, asset.AssetID);
                                                if (list.Contains(res.LocationType))
                                                {
                                                    item.Asset = asset;
                                                    item.AssetID = asset.AssetID;
                                                    model.LineItems.Add(item);
                                                }
                                                else
                                                {
                                                    SameLocMapping.Add(asset.Barcode);
                                                }
                                            }
                                            else
                                            {
                                                item.Asset = asset;
                                                item.AssetID = asset.AssetID;
                                                model.LineItems.Add(item);
                                            }
                                        }
                                        else
                                        {
                                            MissingbarcodesList.Add(asset.Barcode);
                                        }
                                    }
                                }
                                else
                                {
                                    barcodesList.Add(asset.Barcode);
                                }
                            }
                            else
                            {
                                UnMappedbarcodesList.Add(mapping.Barcode);
                            }
                        }
                        else
                        {
                            ActivebarcodesList.Add(asset.Barcode);
                        }



                    }
                    else
                    {
                        AvaliablebarcodesList.Add(row["Barcode"] + "");
                    }
                }
                string validation = "";
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
               

                if (validation == "")
                {
                    base.TraceLog("ImportUpload", $"AMCSchedule ImportUpload method loaded data to model");
                    return Json(new { Result = "success", Type = "Upload", FilePath = filePaths, error = "" });
                }
                    
                else
                {
                    base.TraceLog("ImportUpload error", $"AMCSchedule Import Upload data error");
                    TransactionLineItemDetailsModel.ClearCurrentModel(currentPageID);
                    //Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    //return Content(validation);
                    return Json(new { Result = "false", Type = "Upload", FilePath = "", error = validation });
                }
            }
            catch (Exception ex)
            {
                
                ErrorJsonResult(ex);
                return Json(new { Result = "false", Type = "Upload", FilePath = "", error = ex.Message });

            }
        }
        public async Task<Tuple<string, DataTable>> ReadDocument(IFormFile files)
        {
            base.TraceLog("ReadDocument", $"AMCSchedule ReadDocument ");
            string path = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/AMCSchedule");
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
            return obj;

        }

        public ActionResult ImportRemove(string[] fileNames, int currentPageID)
        {
            base.TraceLog("ImportRemove", $"AMCSchedule ImportRemove ");
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
            base.TraceLog("DownloadFile", $"AMCSchedule DownloadSampleFile clicked");
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