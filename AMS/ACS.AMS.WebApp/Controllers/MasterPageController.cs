using ACS.AMS.DAL;
using ACS.AMS.DAL.DBContext;
using ACS.AMS.DAL.DBModel;
using ACS.AMS.WebApp.Classes;
using ACS.WebAppPageGenerator.Models.SystemModels;
using DocumentFormat.OpenXml.Office2010.Excel;
using Kendo.Core.Export;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.Infrastructure;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Web;
using Telerik.Documents.SpreadsheetStreaming;

namespace ACS.AMS.WebApp.Controllers
{
	public class MasterPageController : ACSBaseController
    {
        private static IWebHostEnvironment WebHostEnvironment;
        public MasterPageController(IWebHostEnvironment _environment)
        {
            WebHostEnvironment = _environment;
        }

        public virtual IActionResult IndexPageInLineEdit(string pageName)
        {
            //check the rights
            var rightName = pageName;
            if (!this.HasRights(rightName, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");

            this.TraceLog("Index", $"{SessionUser.Current.Username} - {pageName} Index page requested");

            Type entityType = GetEntityObjectType(pageName);
            var obj = Activator.CreateInstance(entityType) as BaseEntityObject;

            SystemDatabaseHelper.GenerateMasterGridColumns(entityType, pageName);

            return PartialView("BaseViews/IndexPageInLineEdit",
                new IndexPageModel()
                {
                    PageTitle = pageName,
                    PageName = pageName,
                    EntityInstance = obj,
                    ControllerName = this.GetType().Name.Replace("Controller", ""),
                });
        }

        public virtual IActionResult Index(string pageName)
        {
            try
            {
                //check the rights
                var rightName = pageName;
                if (!this.HasRights(rightName, UserRightValue.View))
                    return RedirectToAction("UnauthorizedPage");

                this.TraceLog("Index", $"{SessionUser.Current.Username} - {pageName} Index page requested");
                Type entityType = GetEntityObjectType(pageName, true);
                var obj = Activator.CreateInstance(entityType) as BaseEntityObject;
                SystemDatabaseHelper.GenerateMasterGridColumns(entityType, pageName);

                string pageViewName = "BaseViews/IndexPage";

                switch (pageName.ToUpper())
                {
                    case "CATEGORY":
                        pageViewName = "BaseViews/TreeListPage";
                        break;
                    case "LOCATION":
                        pageViewName = "BaseViews/TreeIndexPage";
                        break;
                }

                return PartialView(pageViewName,
                    new IndexPageModel()
                    {
                        PageTitle = pageName,
                        PageName = pageName,
                        EntityInstance = obj,
                        ControllerName = this.GetType().Name.Replace("Controller", ""),
                    });
            }
            catch(Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }

        public virtual IActionResult _Index([DataSourceRequest] DataSourceRequest request, string pageName)
        {
            try
            {
                var rightName = pageName;
                if (!this.HasRights(rightName, UserRightValue.View))
                    return RedirectToAction("UnauthorizedPage");

                //get the 
                Type languageEntityType = null;// EntityHelper.GetLanguageTable(pageName);
                Type entityType = GetEntityObjectType(pageName, true);

                IACSDBObject languageObj = null;
                if (languageEntityType != null)
                    languageObj = Activator.CreateInstance(languageEntityType) as IACSDBObject;
                var obj = Activator.CreateInstance(entityType) as IACSDBObject;

                if (languageObj == null) languageObj = obj;

                var baseEntityInstance = obj as BaseEntityObject;

                //fetch the records from language table

                if (string.Compare(pageName, "Asset") == 0)
                {
                    //_db.EnableInstanceQueryLog = true;
                    
                    var query = AssetNewView.GetAllUserItem(_db, SessionUser.Current.UserID);
                    var dsResult = request.ToDataSourceResultForAsset(query, pageName, baseEntityInstance.GetPrimaryKeyFieldName());

                    //var totalRecs = query.Take(request.PageSize).ToList();
                    //_db.EnableInstanceQueryLog = false;

                    this.TraceLog("Index", $"{SessionUser.Current.Username} - {pageName} Index page Data Fetch");

                    return Json(dsResult);
                }
                else
                {
                    var query = languageObj.GetAllUserItems(_db, SessionUser.Current.UserID);
                    var dsResult = request.ToDataSourceResult(query, pageName, baseEntityInstance.GetPrimaryKeyFieldName());
                    this.TraceLog("Index", $"{SessionUser.Current.Username} - {pageName} Index page Data Fetch");

                    return Json(dsResult);
                }
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }

        protected string GetPageName(string pageName)
        {
            int index = pageName.IndexOf(':');
            if(index > 0)
                return pageName.Substring(0, index);

            return pageName;
        }

        protected string GetSubPageName(string pageName)
        {
            int index = pageName.IndexOf(':');
            if (index > 0)
                return pageName.Substring(index + 1);

            return "";
        }

        protected virtual ActionResult DoCreatePage(string pageName,bool isPopupCreation=false)
        {
            var realEntityPageName = GetPageName(pageName);
            var subPageName = GetSubPageName(pageName);
            var rightName = realEntityPageName;

            if (string.IsNullOrEmpty(rightName))
            {
                base.SaveExceptionAndTraceLog("Missing PageName for the request");
            }

            if (!base.HasRights(realEntityPageName, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("Create", $"{SessionUser.Current.Username} - {pageName} Create page requested");
            var obj = Activator.CreateInstance(GetEntityObjectType(realEntityPageName)) as BaseEntityObject;
            string viewName = "BaseViews/CreatePage";

            if(isPopupCreation)
            {
                viewName = "BaseViews/QuickCreatePage";
                ViewBag.ParentControlName = string.Concat(pageName, "ID");
            }

            if((realEntityPageName == "Asset") && (subPageName == ""))
                viewName = "BaseViews/SubMenuPageCreate";

            return PartialView(viewName, GetCreateEntryPageModel(pageName, obj));
        }

        protected virtual BasePageModel GetCreateEntryPageModel(string pageName, BaseEntityObject obj, IFormCollection collection = null)
        {
            var realPageName = GetPageName(pageName);
            var subPageName = GetSubPageName(pageName);

            BasePageModel newModel = PageGenerationDescriptor.GetPage(realPageName, subPageName, PageTypes.Create, SessionUser.Current.UserID, obj);
            newModel.ControllerName = this.GetType().Name.Substring(0, this.GetType().Name.Length - "Controller".Length);
            newModel.FormCollection = collection;

            //EntryPageModel newModel = new EntryPageModel()
            //{
            //    PageTitle = pageName,
            //    PageName = pageName,
            //    EntityInstance = obj,
            //    FormCollection = collection,
            //    ControllerName = this.GetType().Name.Substring(0, this.GetType().Name.Length - "Controller".Length),
            //};

            //switch (pageName)
            //{
            //    case "CurrencyConversion":
            //        {
            //            newModel.TransactionFields.Add(new PageFieldModel()
            //            {
            //                FieldName = "FromCurrencyID",
            //                DisplayLabel = "FromCurrency",
            //                ControlType = PageControlTypes.MultiColumnComboBox,
            //                ControlName = "CurrencySelection"
            //            });
            //            newModel.TransactionFields.Add(new PageFieldModel()
            //            {
            //                FieldName = "ToCurrencyID",
            //                DisplayLabel = "ToCurrency",
            //                ControlType = PageControlTypes.MultiColumnComboBox,
            //                ControlName = "CurrencySelection"
            //            });
            //        }
            //        break;
            //}

            return newModel;
        }

        public virtual ActionResult Create(string pageName,bool isPopupCreation=false)
        {
            return DoCreatePage(pageName,isPopupCreation);
        }

        protected async virtual Task<ActionResult> ProcessDataCreation(IFormCollection collection, string pageName)
        {
            var realPageName = GetPageName(pageName);
            var subPageName = GetSubPageName(pageName);
            var rightName = realPageName;

            if (!base.HasRights(rightName, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");

            var modelType = GetEntityObjectType(realPageName);
            Type languageEntityType = null;// EntityHelper.GetLanguageTable(modelType);

            var item = Activator.CreateInstance(modelType) as BaseEntityObject;
            BaseEntityObject languageItem = null;

            try
            {
                base.TraceLog("Create Post", $"{SessionUser.Current.Username} - {pageName} details will add to db ");

                if (languageEntityType != null)
                {
                    languageItem = Activator.CreateInstance(languageEntityType) as BaseEntityObject;
                    var res1 = await base.TryUpdateModelAsync(languageItem, languageEntityType, "");
                }

                var res = await base.TryUpdateModelAsync(item, modelType, "");
                if (languageItem != null)
                {
                    //Update Parent entity into description table
                    var propertyname = modelType.Name;
                    propertyname = propertyname.Substring(0, propertyname.Length - 5);
                    languageItem.SetFieldValue(propertyname, item);
                    languageItem.SetFieldValue("LanguageID", 1);

                    _db.Add(languageItem);
                }
                if (modelType.Name == "AssetTable")
                {
                    ModelState.Remove("DOF_MASS_SPLIT_EXECUTED");
                }

                if (ModelState.IsValid)
                {
                    item.SetFieldValue("StatusID", (int)StatusValue.Active);
                    item.SetFieldValue("PostingStatusID", (int)PostingStatusValue.WorkInProgress);
                    

                    UpdateNewEntityObject(item, collection, realPageName);

                    //Assign the values from posted data
                    //get the list of fields requires controls
                    item.ValidateUniqueKey(_db);
                    if (string.Compare(realPageName, "Asset") == 0)
                    {
                        item.SetFieldValue("DOF_MASS_SPLIT_EXECUTED", false);
                        AssetTable asset = (AssetTable)item;

                        var standtandValidate = TransactionTable.AssetValidationResult(_db, SessionUser.Current.UserID, "WebAssetCreate", asset.CategoryID,
                                            null, null, asset.LocationID, asset.DepartmentID, null, asset.SerialNo, null, asset.ManufacturerID);
                        if (!string.IsNullOrEmpty(standtandValidate.Result))
                        {
                            return base.ErrorActionResult(standtandValidate.Result);
                        }

                        if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.AssetApprovalBasedOnWorkFlow))
                        {
                            var validationError = TransactionTable.GetAssetResult(AMSContext.CreateNewContext(), (int)asset.LocationID, asset.CategoryID);
                            if (!string.IsNullOrEmpty(validationError.Result))
                            {
                                return base.ErrorActionResult(validationError.Result);
                            }
                        }
                    }

                    _db.Add(item);
                    _db.SaveChanges();

                    if (string.Compare(realPageName, "Asset") == 0)
                    {
                        if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.AssetApprovalBasedOnWorkFlow))
                        {
                            _db.Entry(item).Reload();
                            AssetTable asset = (AssetTable)item;

                            List<string> BarcodeList = new List<string>() { asset.Barcode };
                            TransactionTable.SaveTransactiondata(_db, BarcodeList, SessionUser.Current.UserID, (int)ApproveModuleValue.AssetAddition, CodeGenerationHelper.GetNextCode("AssetAddition"));
                        }
                    }

                    if (string.Compare(realPageName, "Location") == 0)
                    {
                        _db.Entry(item).Reload();
                        LocationTable datas = (LocationTable)item;
                        if (!string.IsNullOrEmpty(datas.StampPath))
                        {
                            var rootpath = string.Concat(WebHostEnvironment.WebRootPath, "\\");
                            if (!String.IsNullOrEmpty(rootpath))
                            {
                                var newpath = RenameFileWithFileInfoClass(datas.StampPath, string.Concat(datas.LocationCode, '_', datas.LocationID.ToString()), rootpath,realPageName);
                                datas.StampPath = newpath;
                                _db.SaveChanges();
                            }
                        }
                    }
                    if (string.Compare(realPageName, "Category") == 0)
                    {
                        _db.Entry(item).Reload();
                        CategoryTable datas = (CategoryTable)item;
                        if (!string.IsNullOrEmpty(datas.CatalogueImage))
                        {
                            var rootpath = string.Concat(WebHostEnvironment.WebRootPath, "\\");
                            if (!String.IsNullOrEmpty(rootpath))
                            {
                                var newpath = RenameFileWithFileInfoClass(datas.CatalogueImage, string.Concat(datas.CategoryCode, '_', datas.CategoryID.ToString()), rootpath, realPageName);
                                datas.CatalogueImage = newpath;
                                _db.SaveChanges();
                            }
                        }
                    }
                    if (string.Compare(realPageName, "Product") == 0)
                    {
                        _db.Entry(item).Reload();
                        ProductTable datas = (ProductTable)item;
                        if (!string.IsNullOrEmpty(datas.CatalogueImage))
                        {
                            var rootpath = string.Concat(WebHostEnvironment.WebRootPath, "\\");
                            if (!String.IsNullOrEmpty(rootpath))
                            {
                                var newpath = RenameFileWithFileInfoClass(datas.CatalogueImage, string.Concat(datas.ProductCode, '_', datas.ProductID.ToString()), rootpath, realPageName);
                                datas.CatalogueImage = newpath;
                                _db.SaveChanges();
                            }
                        }
                    }


                    base.TraceLog("Create Post", $"{SessionUser.Current.Username} - {realPageName} details saved to db ");

                    if (string.IsNullOrEmpty(subPageName))
                    {
                        ViewData["pageName"] = realPageName;
                        return PartialView("SuccessAction", new { pageName = realPageName });
                    }
                    else
                    {
                        //its a subpage, move to edit screen for further data edit
                        ViewData["pageName"] = realPageName;
                        _db.Entry(item).Reload();

                        ViewData["URL"] = $"{Url.Action("Edit")}/{item .GetPrimaryKeyValue()}?pageName={realPageName}";
                        return PartialView("SuccessAction", new { pageName = realPageName });
                    }
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

            string viewName = "BaseViews/CreatePage";
            return PartialView(viewName, GetCreateEntryPageModel(pageName, item, collection));
        }

        protected virtual void UpdateNewEntityObject(BaseEntityObject entity, IFormCollection collection, string pageName)
        {

        }

        [HttpPost]
        public async Task<ActionResult> Create(IFormCollection collection, string pageName)
        {
            return await ProcessDataCreation(collection, pageName);
        }
        [HttpPost]
        public async Task<ActionResult> EditInLine(IFormCollection data, long primaryKeyID, string pageName)
        {
            return await ProcessDataUpdation(data, primaryKeyID, pageName);
        }

        protected virtual ActionResult DoEditPage(int id, string pageName)
        {
            var realEntityPageName = GetPageName(pageName);
            var subPageName = GetSubPageName(pageName);
            var rightName = realEntityPageName;

            if (!base.HasRights(rightName, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("Edit", $"{SessionUser.Current.Username} - {realEntityPageName} Edit page requested");
            var obj = Activator.CreateInstance(GetEntityObjectType(realEntityPageName)) as BaseEntityObject;
            var objInstance = obj as IACSDBObject;
            string viewName = "BaseViews/EditPage";

            if ((realEntityPageName == "Asset") && (subPageName == ""))
                viewName = "BaseViews/SubMenuPageEdit";

            var editObject = objInstance.GetItemByID(_db, id) as BaseEntityObject;
            var objects = GetCreateEntryPageModel(pageName, editObject);

            if (string.Compare(realEntityPageName, "Asset") == 0)
            {
                AssetTable asset = editObject as AssetTable;
                if (asset.StatusID == (int)StatusValue.WaitingForApproval || asset.StatusID == (int)StatusValue.Disposed)
                {
                    viewName = "BaseViews/SubMenuPageDetails";
                    return PartialView(viewName, objects);
                }
            }

            return PartialView(viewName, objects);
            //return PartialView(viewName, 
            //    new EntryPageModel()
            //    {
            //        PageTitle = pageName,
            //        PageName = pageName,
            //        EntityInstance = objInstance.GetItemByID(_db, id) as BaseEntityObject,
            //        ControllerName = this.GetType().Name.Substring(0, this.GetType().Name.Length - "Controller".Length),
            //    });
        }

        public virtual ActionResult Edit(int id, string pageName)
        {
            return DoEditPage(id, pageName);
        }

        protected async virtual Task<ActionResult> ProcessDataUpdation(IFormCollection collection, long primaryKeyID, string pageName)
        {
            var realPageName = GetPageName(pageName);
            var subPageName = GetSubPageName(pageName);
            var rightName = realPageName;

            if (!base.HasRights(rightName, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");

            var modelType = GetEntityObjectType(realPageName);
            var item = Activator.CreateInstance(modelType) as BaseEntityObject;

            try
            {
                base.TraceLog("Edit Post", $"{realPageName} details will modify to db : Entity id- {primaryKeyID}");
                var res = await base.TryUpdateModelAsync(item, modelType, "");

                var objInstance = item as IACSDBObject;
                var oldItem = objInstance.GetItemByID(_db, primaryKeyID) as BaseEntityObject;
                if (modelType.Name == "AssetTable")
                {
                    ModelState.Remove("DOF_MASS_SPLIT_EXECUTED");
                }

                if (ModelState.IsValid)
                {
                    bool validation = true;

                    if (string.Compare(realPageName, "Location") == 0)
                    {
                        if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.IsMandatoryLocationType))
                        {
                            int level = AppConfigurationManager.GetValue<int>(AppConfigurationManager.PreferredLevelLocationMapping);

                            LocationTable datas = (LocationTable)item;

                            if (datas.ParentLocationID == null && datas.LocationTypeID != null && level > 1)
                            {
                                validation = false;
                                ModelState.AddModelError("12", "Location Type can be changed only at the Preferred Level");
                            }
                            if (datas.ParentLocationID != null)
                            {
                                var loc = (from b in _db.LocationNewView where b.LocationID == datas.ParentLocationID select b).FirstOrDefault();
                                if (loc.ParentLocationID != null && datas.LocationTypeID != null)
                                {
                                    if ((level - 1) != loc.Level)
                                    {
                                        validation = false;
                                        ModelState.AddModelError("12", "Location Type can be changed only at the Preferred Level");
                                    }
                                }
                                else if (loc.ParentLocationID == null && datas.LocationTypeID == null)
                                {
                                    if ((level - 1) == loc.Level)
                                    {
                                        validation = false;
                                        ModelState.AddModelError("12", "Location Type mandatory for Preferred Level Location ");
                                    }
                                }
                            }
                        }
                    }

                    if (string.Compare(realPageName, "Category") == 0)
                    {
                        if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.IsMandatoryCategoryType))
                        {
                            int level = AppConfigurationManager.GetValue<int>(AppConfigurationManager.PreferredLevelCategoryMapping);

                            CategoryTable datas = (CategoryTable)item;

                            if (datas.ParentCategoryID == null && datas.CategoryTypeID != null && level > 1)
                            {
                                validation = false;
                                ModelState.AddModelError("12", "Category Type can be changed only at the Preferred Level");
                            }
                            if (datas.ParentCategoryID != null)
                            {
                                var loc = (from b in _db.CategoryNewView where b.CategoryID == datas.ParentCategoryID select b).FirstOrDefault();
                                if (loc.ParentCategoryID != null && datas.CategoryTypeID != null)
                                {
                                    if ((level - 1) != loc.Level)
                                    {
                                        validation = false;
                                        ModelState.AddModelError("12", "Category Type can be changed only at the Preferred Level");
                                    }
                                }
                                else if (loc.ParentCategoryID == null && datas.CategoryTypeID == null)
                                {
                                    if ((level - 1) == loc.Level)
                                    {
                                        validation = false;
                                        ModelState.AddModelError("12", "Category Type mandatory for Preferred Level Category ");
                                    }
                                }
                            }
                        }

                        string CatalogueImage = Convert.ToString(item.GetFieldValue("CatalogueImage"));
                        string CategoryCode = Convert.ToString(item.GetFieldValue("CategoryCode"));
                        if (!string.IsNullOrEmpty(CatalogueImage))
                        {
                            var rootpath = string.Concat(WebHostEnvironment.WebRootPath, "\\");
                            if (!string.IsNullOrEmpty(rootpath))
                            {
                                var newpath = RenameFileWithFileInfoClass(CatalogueImage, string.Concat(CategoryCode, '_', primaryKeyID.ToString()), rootpath, realPageName);
                                item.SetFieldValue("CatalogueImage", newpath);
                            }
                        }
                        

                    }
                    if(string.Compare(realPageName,"Product")==0)
                    {
                        string CatalogueImage = Convert.ToString(item.GetFieldValue("CatalogueImage"));
                        string productCode = Convert.ToString(item.GetFieldValue("ProductCode"));
                        if (!string.IsNullOrEmpty(CatalogueImage))
                        {
                            var rootpath = string.Concat(WebHostEnvironment.WebRootPath, "\\");
                            if (!string.IsNullOrEmpty(rootpath))
                            {
                                var newpath = RenameFileWithFileInfoClass(CatalogueImage, string.Concat(productCode, '_', primaryKeyID.ToString()), rootpath, realPageName);
                                item.SetFieldValue("CatalogueImage", newpath);
                            }
                        }
                    }
                    if (string.Compare(realPageName, "Asset") == 0)
                    {
                        item.SetFieldValue("DOF_MASS_SPLIT_EXECUTED", false);
                        AssetTable asset = (AssetTable)item;

                        var standtandValidate = TransactionTable.AssetModificationValidationResult(_db, SessionUser.Current.UserID, asset.AssetID, "WebAssetModify", asset.CategoryID,
                                           null, null, asset.LocationID, asset.DepartmentID, null, asset.SerialNo, null, asset.ManufacturerID);
                        if (!string.IsNullOrEmpty(standtandValidate.Result))
                        {
                            return base.ErrorActionResult(standtandValidate.Result);
                        }
                    }

                    {
                        DataUtilities.CopyObject(item, oldItem);
                        oldItem.ValidateUniqueKey(_db);

                        _db.SaveChanges();

                        if (string.Compare(realPageName, "Location") == 0)
                        {
                            LocationTable datas = (LocationTable)item;

                            if (!string.IsNullOrEmpty(datas.StampPath))
                            {
                                var rootpath =  string.Concat(WebHostEnvironment.WebRootPath, "\\"); 
                                if (!string.IsNullOrEmpty(rootpath))
                                {
                                    var newpath = RenameFileWithFileInfoClass(datas.StampPath, string.Concat(datas.LocationCode, '_', datas.LocationID.ToString()), rootpath, realPageName);
                                    datas.StampPath = newpath;

                                    _db.SaveChanges();
                                }
                            }
                        }
                        
                        base.TraceLog("Edit Post", $"{realPageName} details modified to db : Entity id- {primaryKeyID}");

                        if (string.IsNullOrEmpty(subPageName))
                        {
                            ViewData["pageName"] = realPageName;
                            return PartialView("SuccessAction", new { pageName = realPageName });
                        }
                        else
                        {
                            //its a subpage, move to edit screen for further data edit
                            ViewData["URLControl"] = "childWorkingArea";
                            
                            ViewData["URL"] = $"{Url.Action("Edit")}?pageName={pageName}"; //For Edit URL action ID will come by default, so no need for passing it explicitly
                            return PartialView("SuccessAction", new { pageName = realPageName });
                        }
                    }
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
            string viewName = "BaseViews/EditPage";

            return PartialView(viewName, GetCreateEntryPageModel(pageName, item, collection));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(IFormCollection data, long primaryKeyID, string pageName)
        {
            return await ProcessDataUpdation(data, primaryKeyID, pageName);
        }

        protected virtual ActionResult DoDetailsPage(int id, string pageName)
        {
            var realEntityPageName = GetPageName(pageName);
            var subPageName = GetSubPageName(pageName);
            var rightName = realEntityPageName;

            if (!base.HasRights(rightName, UserRightValue.Details))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("Details", $"{SessionUser.Current.Username} - {pageName} Details page requested");
            var obj = Activator.CreateInstance(GetEntityObjectType(realEntityPageName)) as BaseEntityObject;
            var objInstance = obj as IACSDBObject;
            
            string viewName = "BaseViews/DetailsPage";

            if ((realEntityPageName == "Asset") && (subPageName == ""))
                viewName = "BaseViews/SubMenuPageDetails";
            var editObject = objInstance.GetItemByID(_db, id) as BaseEntityObject;

            //if (string.Compare(realEntityPageName, "Asset") == 0)
            //{
            //    AssetTable asset = editObject as AssetTable;
            //    if (asset.StatusID == (int)StatusValue.WaitingForApproval || asset.StatusID == (int)StatusValue.Disposed)
            //    {
            //        viewName = "BaseViews/DetailsPage";
            //        return PartialView(viewName, GetCreateEntryPageModel(pageName, editObject));
            //    }
            //}

            return PartialView(viewName, GetCreateEntryPageModel(pageName, editObject));

            //return PartialView(viewName,
            //    new EntryPageModel()
            //    {
            //        PageTitle = pageName,
            //        PageName = pageName,
            //        EntityInstance = objInstance.GetItemByID(_db, id),
            //        ControllerName = this.GetType().Name.Substring(0, this.GetType().Name.Length - "Controller".Length),
            //    });
        }

        public virtual ActionResult Details(int id, string pageName)
        {
            try
            {
                return DoDetailsPage(id, pageName);
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }

        public virtual IActionResult Delete(int id, string pageName)
        {
            try
            {
                var rightName = pageName;

                if (!base.HasRights(rightName, UserRightValue.Delete))
                    return RedirectToAction("UnauthorizedPage");

                base.TraceLog("Delete", $"{SessionUser.Current.Username} - {pageName} Delete action requested");
                var obj = Activator.CreateInstance(GetEntityObjectType(pageName)) as IACSDBObject;
                var oldItem = obj.GetItemByID(_db, id) as BaseEntityObject;
                var objInstance = oldItem as IACSDBObject;

                oldItem.UpdateUniqueKey(_db);

                objInstance.DeleteObject();
                _db.SaveChanges();
                
                ViewData["pageName"] = pageName;

                base.TraceLog("Delete", $"{pageName} details page deleted successfully {oldItem.GetPrimaryKeyFieldName()} {id}");
                return PartialView("SuccessAction", new { pageName = pageName });
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

        public IActionResult DeleteAll(string toBeDeleteIds, string pageName)
        {
            try
            {
                var rightName = pageName;
                if (!base.HasRights(rightName, UserRightValue.Delete))
                    return RedirectToAction("UnauthorizedPage");
                base.TraceLog("DeleteAll", $"{SessionUser.Current.Username} - {pageName} Delete action requested");

                var obj = Activator.CreateInstance(GetEntityObjectType(pageName)) as IACSDBObject;

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

                base.TraceLog("DeleteAll", $"{pageName} details page deleted successfully");
                return RedirectToAction("SuccessAction", new { pageName = pageName });
            }
            catch (ValidationException ex)
            {
                return base.ErrorActionResult(ex.Message);
            }
            catch (Exception ex)
            {
                return base.ErrorActionResult(ex);
            }
        }

        [HttpPost]
        public FileStreamResult ExportServer(IList<ExportColumnSettings> columnSettings, string format, string title, string filter, int[] selectedIds)
        {
            base.TraceLog("ExportServer", $"ExportServer click");
            SpreadDocumentFormat exportFormat = format == "csv" ? exportFormat = SpreadDocumentFormat.Csv : exportFormat = SpreadDocumentFormat.Xlsx;
            Action<ExportCellStyle> cellStyle = new Action<ExportCellStyle>(ChangeCellStyle);
            Action<ExportRowStyle> rowStyle = new Action<ExportRowStyle>(ChangeRowStyle);
            Action<ExportColumnStyle> columnStyle = new Action<ExportColumnStyle>(ChangeColumnStyle);
            //var query = PersonView.GetAllItems(_db);

            var filters = FilterDescriptorFactory.Create(filter);

            DataSourceRequest req = new DataSourceRequest()
            {
                Filters = filters,
            };

            var result = req.ToDataSourceResult(AssetNewView.GetAllUserItem(_db, SessionUser.Current.UserID), "Asset", "");
            IEnumerable assetData = result.Data;

            //if (selectedIds != null && selectedIds.Length > 0)
            //{
            //    rowsData = GetOrders().Where(x => selectedIds.Contains(x.OrderID)).ToList();
            //}
            //else
            //{
            //    rowsData = GetOrders();
            //}

            string fileName = string.Format("{0}.{1}", title, format);
            string mimeType = Helpers.GetMimeType(exportFormat);

            Stream exportStream = exportFormat == SpreadDocumentFormat.Xlsx ?
                assetData.ToXlsxStream(columnSettings, title, cellStyleAction: cellStyle, rowStyleAction: rowStyle, columnStyleAction: columnStyle) :
                assetData.ToCsvStream(columnSettings);

            var fileStreamResult = new FileStreamResult(exportStream, mimeType);
            fileStreamResult.FileDownloadName = fileName;

            fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin);

            return fileStreamResult;
        }

        [HttpGet]
        [HttpPost]
        public FileStreamResult ExportTreeViewServer(string format, string title, string pageName, string filterData)
        {
            base.TraceLog("ExportTreeViewServer", $"ExportServer click");
            SpreadDocumentFormat exportFormat = format == "csv" ? exportFormat = SpreadDocumentFormat.Csv : exportFormat = SpreadDocumentFormat.Xlsx;
            Action<ExportCellStyle> cellStyle = new Action<ExportCellStyle>(ChangeCellStyle);
            Action<ExportRowStyle> rowStyle = new Action<ExportRowStyle>(ChangeRowStyle);
            Action<ExportColumnStyle> columnStyle = new Action<ExportColumnStyle>(ChangeColumnStyle);
            string fileName = string.Format("{0}.{1}", title, format);
            string mimeType = Helpers.GetMimeType(exportFormat);
            List<ExportColumnSettings> columnSettings = GetColumnsSettings(_db, pageName);

            switch (pageName)
            {
                case "Location":
                    var list = LocationTable.GetExportExceo(_db, SessionUser.Current.UserID, filterData);
                    var data = list.Result.AsEnumerable();

                    Stream exportStream = exportFormat == SpreadDocumentFormat.Xlsx ?
                        data.ToXlsxStream(columnSettings, title, cellStyleAction: cellStyle, rowStyleAction: rowStyle, columnStyleAction: columnStyle) :
                        data.ToCsvStream(columnSettings);

                    var fileStreamResult = new FileStreamResult(exportStream, mimeType);
                    fileStreamResult.FileDownloadName = fileName;
                    fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin);

                    //  return fileStreamResult;
                    return File(exportStream, "application/excel", "LocationList.xlsx");
                    break;

                case "Category":
                    var Catgorylist = LocationTable.GetExportCategoryExceo(_db, SessionUser.Current.UserID, filterData);
                    var datas = Catgorylist.Result.AsEnumerable();

                    Stream exportStreamCategory = exportFormat == SpreadDocumentFormat.Xlsx ?
                        datas.ToXlsxStream(columnSettings, title, cellStyleAction: cellStyle, rowStyleAction: rowStyle, columnStyleAction: columnStyle) :
                        datas.ToCsvStream(columnSettings);

                    var fileStreamCategoryResult = new FileStreamResult(exportStreamCategory, mimeType);
                    fileStreamCategoryResult.FileDownloadName = fileName;
                    fileStreamCategoryResult.FileStream.Seek(0, SeekOrigin.Begin);

                    //  return fileStreamCategoryResult;
                    return File(exportStreamCategory, "application/excel", "CategoryList.xlsx");
                    break;

            }
            return null;
        }

        public static List<ExportColumnSettings> GetColumnsSettings(AMSContext _db, string pageName)
        {
            var list = MasterGridNewLineItemTable.GetAllFields(_db, pageName);
            List<ExportColumnSettings> colu = new List<ExportColumnSettings>();
            foreach (var field in list)
            {
                ExportColumnSettings data = new ExportColumnSettings();
                data.Width = "20px";
                data.Field = field.FieldName;
                data.Title = field.DisplayName;
                data.Format = null;
                data.Hidden = false;
                colu.Add(data);
            }
            return colu;
        }
        private void ChangeCellStyle(ExportCellStyle e)
        {
            bool isHeader = e.Row == 0;
            SpreadCellFormat format = new SpreadCellFormat
            {
                ForeColor = isHeader ? SpreadThemableColor.FromRgb(255, 255, 255) : SpreadThemableColor.FromRgb(0, 0, 0),
                IsItalic = true,
                IsBold = isHeader,
                VerticalAlignment = SpreadVerticalAlignment.Center,
                WrapText = true,
                Fill = SpreadPatternFill.CreateSolidFill(isHeader ? new SpreadColor(0, 0, 0) : new SpreadColor(255, 255, 255))
            };
            e.Cell.SetFormat(format);
        }

        private void ChangeRowStyle(ExportRowStyle e)
        {
            e.Row.SetHeightInPixels(e.Index == 0 ? 80 : 30);
        }

        private void ChangeColumnStyle(ExportColumnStyle e)
        {
            double width = e.Name == "Product name" || e.Name == "Category Name" ? 250 : 100;
            e.Column.SetWidthInPixels(width);
        }

        public static string RenameFileWithFileInfoClass(string oldFile, string newFile, string rootpath,string pageName)
        {
            var oldPath = System.IO.Path.Combine(rootpath, oldFile);
            var oldfiles = System.IO.Path.GetFileName(oldPath);
            var extension = System.IO.Path.GetExtension(oldPath);
            string folderName = string.Empty;
            if (string.Compare(pageName,"Location")==0)
            {
                folderName = "FileStoragePath/StampPath";
            }
            if (string.Compare(pageName, "Category") == 0)
            {
                folderName = "FileStoragePath/UploadImage/Category";

            }
            if (string.Compare(pageName, "Product") == 0)
            {
                folderName = "FileStoragePath/UploadImage/Product";

            }
            string result = Regex.Replace(newFile, @"[^a-zA-Z0-9\s]", "");
            var newphysicalpth = System.IO.Path.Combine(folderName, string.Concat(result.Replace(" ",""), extension));
            var newpath = System.IO.Path.Combine(rootpath, newphysicalpth);
            if (string.Compare(oldPath, newpath) != 0)
            {
                var file = new FileInfo(oldPath);
                if (System.IO.File.Exists(newpath))
                {
                    System.IO.File.Delete(newpath);
                }
                file.MoveTo(newpath);
            }
            return newphysicalpth;

        }
        public async Task<ActionResult> DocumentUpload(IFormFile UploadDoc, int currentPageID,  int userID,string PageName)
        {
            try
            {

                base.TraceLog("DocmentUpload DocumentUpload", $"{SessionUser.Current.Username}  DocumentUpload request.");

                string rootPath = string.Concat(WebHostEnvironment.WebRootPath, "\\");
                string fullPath = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/UploadDocument/"+PageName);
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                if (UploadDoc != null)
                {
                    var fileContent = ContentDispositionHeaderValue.Parse(UploadDoc.ContentDisposition);


                    string fileExtension = System.IO.Path.GetExtension(Path.GetFileName(fileContent.FileName.ToString().Trim('"')));
                    string time = ComboBoxHelper.RemoveSpecialCharacters(String.Format("{0:" + CultureHelper.DateTimeFormatForGrid + "}", DateTime.Now));
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(fileContent.FileName.ToString().Trim('"').Replace(" ",""));

                    string newFileName = fileName + "" + time + "" + fileExtension;
                    fullPath = Path.Combine(fullPath, newFileName);

                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await UploadDoc.CopyToAsync(fileStream);
                    }



                    TempData["UploadDocFile"] = fullPath;
                    //return Content("");
                    var physicalPath = fullPath.Replace(rootPath, "").Replace(" ", "").Replace('/', '\\');
                    base.TraceLog("AttachmentPath", $"{SessionUser.Current.Username} -  AttachmentPathUpload request done");
                    return Json(new { ImageName = physicalPath, fileName = newFileName, rootpath = string.Concat(rootPath, "\\").Replace(" ", "").Replace('/', '\\') });
                }


                return Content("");
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }

        }
        public IActionResult DocumentRemove(string fileNames,string PageName)
        {
            // The parameter of the Remove action must be called "fileNames".
            base.TraceLog("DocumentRemove", $"{SessionUser.Current.Username} -  DocumentRemove requested");
            if (fileNames != null)
            {

                var fileName = Path.GetFileName(fileNames);
                var physicalPath = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/UploadDocument/"+PageName, fileName);

                // TODO: Verify user permissions.

                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }

            }
            base.TraceLog("DocumentRemove", $"{SessionUser.Current.Username} -  DocumentRemove request done");
            // Return an empty string to signify success.
            return Content("");
        }

        public async Task<ActionResult> ImageUpload(IFormFile ImageDoc, int currentPageID, int userID,string PageName)
        {
            try
            {

                base.TraceLog("ImageUpload DocumentUpload", $"{SessionUser.Current.Username}  ImageUpload request.");

                string rootPath = string.Concat(WebHostEnvironment.WebRootPath, "\\");
                string fullPath = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/UploadImage/" + PageName);
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                if (ImageDoc != null)
                {
                    var fileContent = ContentDispositionHeaderValue.Parse(ImageDoc.ContentDisposition);


                    string fileExtension = System.IO.Path.GetExtension(Path.GetFileName(fileContent.FileName.ToString().Trim('"').Replace(" ", "")));
                    string time = ComboBoxHelper.RemoveSpecialCharacters(String.Format("{0:" + CultureHelper.DateTimeFormatForGrid + "}", DateTime.Now));
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(fileContent.FileName.ToString().Trim('"').Replace(" ", ""));

                    string newFileName = fileName + "" + time + "" + fileExtension;
                    fullPath = Path.Combine(fullPath, newFileName.Replace(" ",""));

                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await ImageDoc.CopyToAsync(fileStream);
                    }



                    TempData["UploadDocFile"] = fullPath;
                    //return Content("");
                    var physicalPath = fullPath.Replace(rootPath, "").Replace(" ", "").Replace('/', '\\');
                    base.TraceLog("AttachmentPath", $"{SessionUser.Current.Username} -  AttachmentPathUpload request done");
                    return Json(new { ImageName = physicalPath, fileName = newFileName, rootpath = string.Concat(rootPath, "\\").Replace(" ", "").Replace('/', '\\') });
                }


                return Content("");
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }

        }
        public IActionResult ImageRemove(string fileNames,string PageName)
        {
            // The parameter of the Remove action must be called "fileNames".
            base.TraceLog("ImageRemove", $"{SessionUser.Current.Username} -  ImageRemove requested");
            if (fileNames != null)
            {
                var fileName = Path.GetFileName(fileNames);
                var physicalPath = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/UploadImage/" + PageName, fileName);

                // TODO: Verify user permissions.

                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }

            }
            base.TraceLog("ImageRemove", $"{SessionUser.Current.Username} -  ImageRemove request done");
            // Return an empty string to signify success.
            return Content("");
        }
        public FileResult DownloadFile(string fileName, bool finalLevelDoc = false)
        {

            base.TraceLog(" DownloadFile", $"{SessionUser.Current.Username}  DownloadFile request.");
            //Build the File Path.
            string path = string.Empty;

            path = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/") + fileName;


            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }

        [HttpPost]
        public ActionResult Excel_Export_TreeSave(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
        public JsonResult _TreeIndex([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                base.TraceLog("_TreeIndex", $"{SessionUser.Current.Username} - _TreeIndex  requested");
                
                var data = CategoryListView.GetAllItems(_db);
                var result = data.ToList();
                var list = result.ToTreeDataSourceResult(request, e => e.CategoryID, e => e.ParentCategoryID, e => e);

                return Json(list);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ex);
            }
        }
        [HttpPost]
        public IActionResult _deleteCategory(int categoryID)
        {
            try
            {
                base.TraceLog("Category _deleteCategory", $"{SessionUser.Current.Username} -Category _deleteCategory page requested");
                var data = CategoryTable.GetItem(_db, categoryID);
                if (data != null)
                {
                    data.StatusID = (int)StatusValue.Deleted;
                }
                this._db.SaveChanges();
                return Json(new { Result = "Success" });
            }
            catch (ValidationException ex)
            {
                ViewData["FocusControl"] = ex.FieldName;
                ModelState.AddModelError("12", ex.Message);
            }
            catch (Exception ex)
            {
                return base.ErrorActionResult(ex);
                return Json(new { Result = "failed" });
            }
            return Json(new { Result = "failed" });
        }
        [HttpPost]
        public async Task<ActionResult> QuickCreate(IFormCollection collection, string pageName)
        {
            return await ProcessDataQuickCreation(collection, pageName);
        }

        protected async virtual Task<ActionResult> ProcessDataQuickCreation(IFormCollection collection, string pageName)
        {
            var realPageName = GetPageName(pageName);
            var subPageName = GetSubPageName(pageName);
            var rightName = realPageName;

            if (!base.HasRights(rightName, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");

            var modelType = GetEntityObjectType(realPageName);
            Type languageEntityType = null;// EntityHelper.GetLanguageTable(modelType);

            var item = Activator.CreateInstance(modelType) as BaseEntityObject;
            BaseEntityObject languageItem = null;

            try
            {
                base.TraceLog("Create Post", $"{SessionUser.Current.Username} - {pageName} details will add to db ");

                if (languageEntityType != null)
                {
                    languageItem = Activator.CreateInstance(languageEntityType) as BaseEntityObject;
                    var res1 = await base.TryUpdateModelAsync(languageItem, languageEntityType, "");
                }

                var res = await base.TryUpdateModelAsync(item, modelType, "");
                if (languageItem != null)
                {
                    //Update Parent entity into description table
                    var propertyname = modelType.Name;
                    propertyname = propertyname.Substring(0, propertyname.Length - 5);
                    languageItem.SetFieldValue(propertyname, item);
                    languageItem.SetFieldValue("LanguageID", 1);

                    _db.Add(languageItem);
                }
                if (modelType.Name == "AssetTable")
                {
                    ModelState.Remove("DOF_MASS_SPLIT_EXECUTED");
                }

                if (ModelState.IsValid)
                {
                    item.SetFieldValue("StatusID", (int)StatusValue.Active);
                    item.SetFieldValue("PostingStatusID", (int)PostingStatusValue.WorkInProgress);


                    UpdateNewEntityObject(item, collection, realPageName);

                    //Assign the values from posted data
                    //get the list of fields requires controls
                    item.ValidateUniqueKey(_db);
                    if (string.Compare(realPageName, "Asset") == 0)
                    {
                        item.SetFieldValue("DOF_MASS_SPLIT_EXECUTED", false);
                        AssetTable asset = (AssetTable)item;

                        var standtandValidate = TransactionTable.AssetValidationResult(_db, SessionUser.Current.UserID, "WebAssetCreate", asset.CategoryID,
                                            null, null, asset.LocationID, asset.DepartmentID, null, asset.SerialNo, null, asset.ManufacturerID);
                        if (!string.IsNullOrEmpty(standtandValidate.Result))
                        {
                            return base.ErrorActionResult(standtandValidate.Result);
                        }

                        if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.AssetApprovalBasedOnWorkFlow))
                        {
                            var validationError = TransactionTable.GetAssetResult(AMSContext.CreateNewContext(), (int)asset.LocationID, asset.CategoryID);
                            if (!string.IsNullOrEmpty(validationError.Result))
                            {
                                return base.ErrorActionResult(validationError.Result);
                            }
                        }
                    }

                    _db.Add(item);
                    _db.SaveChanges();

                    if (string.Compare(realPageName, "Asset") == 0)
                    {
                        if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.AssetApprovalBasedOnWorkFlow))
                        {
                            _db.Entry(item).Reload();
                            AssetTable asset = (AssetTable)item;

                            List<string> BarcodeList = new List<string>() { asset.Barcode };
                            TransactionTable.SaveTransactiondata(_db, BarcodeList, SessionUser.Current.UserID, (int)ApproveModuleValue.AssetAddition, CodeGenerationHelper.GetNextCode("AssetAddition"));
                        }
                    }

                    if (string.Compare(realPageName, "Location") == 0)
                    {
                        _db.Entry(item).Reload();
                        LocationTable datas = (LocationTable)item;
                        if (!string.IsNullOrEmpty(datas.StampPath))
                        {
                            var rootpath = string.Concat(WebHostEnvironment.WebRootPath, "\\");
                            if (!String.IsNullOrEmpty(rootpath))
                            {
                                var newpath = RenameFileWithFileInfoClass(datas.StampPath, string.Concat(datas.LocationCode, '_', datas.LocationID.ToString()), rootpath, realPageName);
                                datas.StampPath = newpath;
                                _db.SaveChanges();
                            }
                        }
                    }
                    if (string.Compare(realPageName, "Category") == 0)
                    {
                        _db.Entry(item).Reload();
                        CategoryTable datas = (CategoryTable)item;
                        if (!string.IsNullOrEmpty(datas.CatalogueImage))
                        {
                            var rootpath = string.Concat(WebHostEnvironment.WebRootPath, "\\");
                            if (!String.IsNullOrEmpty(rootpath))
                            {
                                var newpath = RenameFileWithFileInfoClass(datas.CatalogueImage, string.Concat(datas.CategoryCode, '_', datas.CategoryID.ToString()), rootpath, realPageName);
                                datas.CatalogueImage = newpath;
                                _db.SaveChanges();
                            }
                        }
                    }
                    if (string.Compare(realPageName, "Product") == 0)
                    {
                        _db.Entry(item).Reload();
                        ProductTable datas = (ProductTable)item;
                        if (!string.IsNullOrEmpty(datas.CatalogueImage))
                        {
                            var rootpath = string.Concat(WebHostEnvironment.WebRootPath, "\\");
                            if (!String.IsNullOrEmpty(rootpath))
                            {
                                var newpath = RenameFileWithFileInfoClass(datas.CatalogueImage, string.Concat(datas.ProductCode, '_', datas.ProductID.ToString()), rootpath, realPageName);
                                datas.CatalogueImage = newpath;
                                _db.SaveChanges();
                            }
                        }
                    }


                    base.TraceLog("Create Post", $"{SessionUser.Current.Username} - {realPageName} details saved to db ");
                    ViewData["ClosePopup"] = "Yes";
                    ViewData["RefereshID"] = string.Concat(pageName, "ID");
                    if (string.IsNullOrEmpty(subPageName))
                    {
                        ViewData["pageName"] = realPageName;
                        return PartialView("SuccessAction", new { pageName = realPageName });
                    }
                    else
                    {
                        //its a subpage, move to edit screen for further data edit
                        ViewData["pageName"] = realPageName;
                        _db.Entry(item).Reload();

                        ViewData["URL"] = $"{Url.Action("Edit")}/{item.GetPrimaryKeyValue()}?pageName={realPageName}";
                        return PartialView("SuccessAction", new { pageName = realPageName });
                    }
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

            string viewName = "BaseViews/CreatePage";
            return PartialView(viewName, GetCreateEntryPageModel(pageName, item, collection));
        }

        public IActionResult RemoveFile(string fileNames)
        {
            // The parameter of the Remove action must be called "fileNames".
            base.TraceLog("ImageRemove", $"{SessionUser.Current.Username} -  ImageRemove requested");
            string[] removeValue=fileNames.Split("FileStoragePath/");
            if (fileNames != null)
            {
                var fileName = Path.GetFileName(fileNames);
                var physicalPath = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath", removeValue[1]);

                // TODO: Verify user permissions.

                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }

            }
            base.TraceLog("ImageRemove", $"{SessionUser.Current.Username} -  ImageRemove request done");
            // Return an empty string to signify success.
            return Json(new { success = true, message = "File deleted successfully." });
        }

        public IActionResult CatalogueListView(int id)
        {
            try
            {
                //check the rights
                var rightName = "Product";
                var pageName = "Product";
                if (!this.HasRights(rightName, UserRightValue.View))
                    return RedirectToAction("UnauthorizedPage");
                ViewBag.CategoryID = id;
                var category = CategoryTable.GetItem(_db, id);
                ViewBag.CategoryName = category.CategoryName;
                this.TraceLog("Index", $"{SessionUser.Current.Username} - {pageName} Index page requested");
                Type entityType = GetEntityObjectType(pageName, true);
                var obj = Activator.CreateInstance(entityType) as BaseEntityObject;
                SystemDatabaseHelper.GenerateMasterGridColumns(entityType, pageName);

                string pageViewName = "BaseViews/CatalogueListview";

               

                return PartialView(pageViewName,
                    new IndexPageModel()
                    {
                        PageTitle = pageName,
                        PageName = pageName,
                        EntityInstance = obj,
                        ControllerName = this.GetType().Name.Replace("Controller", ""),
                    });
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }
        public IActionResult _ProductCatalogue([DataSourceRequest] DataSourceRequest request, int categoryID,string virtualBarcode=null)
        {
            var query = ProductTable.GetAllItems(_db).Where(a => a.CatalogueImage != null && a.CategoryID == categoryID);
            if(!string.IsNullOrEmpty(virtualBarcode))
            {
                query = query.Where(a => a.VirtualBarcode.Contains(virtualBarcode));
            }
            var dsResult = request.ToDataSourceResult(query, "Product", "ProductID");
            base.TraceLog("Category Search Catalogue Index", $"{SessionUser.Current.Username} -Category Search Catalogue Index page Data Fetch");
            return Json(dsResult);
        }
        public IActionResult CatalogueProductDetails(int id)
        {
            string pageViewName = "MasterViews/CatalogueProductDetails";
            return PartialView(pageViewName, id);
        }
    }
}