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
using Microsoft.Extensions.Hosting;
using System.Net.Http.Headers;
using ACS.AMS.WebApp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace ACS.AMS.WebApp.Controllers
{
    public class DashboardMappingController : ACSBaseController
    {
        private static IWebHostEnvironment WebHostEnvironment;
        public DashboardMappingController(IWebHostEnvironment _environment)
        {
            _RightName = RightNames.DashboardMapping;
            WebHostEnvironment = _environment;
        }

        protected virtual IQueryable<BaseEntityObject> GetAllItemsForIndex()
        {
            return DashboardMappingTable.GetAllItems(_db);
        }

        public IActionResult Index()
        {
            if (!base.HasRights(RightNames.DashboardMapping, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("DashboardMapping Index", $"{SessionUser.Current.Username} -DashboardMapping Index page requested");

            return PartialView();
        }

        public IActionResult _Index([DataSourceRequest] DataSourceRequest request)
        {
            if (!base.HasRights(RightNames.DashboardMapping, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");
            IQueryable<DashboardMappingTable> query = DashboardMappingTable.GetAllItems(_db);
           
            var dsResult = request.ToDataSourceResult(query);
            base.TraceLog("DashboardMapping Index", $"{SessionUser.Current.Username} -DashboardMapping Index page Data Fetch");
            return Json(dsResult);
        }

        //
        // GET: /ReportTemplate/Create
        public ActionResult Create()
        {
            if (!base.HasRights(_RightName, UserRightValue.Create))
                return GotoUnauthorizedPage();
            base.TraceLog("DashboardMapping Create", $"{SessionUser.Current.Username} - create page request");
            DashboardMappingTable model = new DashboardMappingTable();
            model.DashboardWeight = "300px";
            model.DashboardHeight = "300px";

            model.CurrentPageID = SessionUser.Current.GetNextPageID();
            return PartialView(model);
        }
        [HttpPost]
        public IActionResult Create(DashboardMappingTable item, IFormCollection data)
        {
            if (!base.HasRights(_RightName, UserRightValue.Create))
                return GotoUnauthorizedPage();
            int currentPageID = int.Parse(data["CurrentPageID"] + "");
            try
            {
                base.TraceLog("DashboardMapping Create-Post", $"{SessionUser.Current.Username} - Dashboardmapping details will save to db");
                if (ModelState.IsValid)
                {
                    item.StatusID = (int)StatusValue.Active;
                    item.CreatedBy = SessionUser.Current.UserID;
                    item.CreatedDateTime = System.DateTime.Now;

                    _db.Add(item);
                    var lineitem = DashboardFieldMappingDataModel.GetModel(currentPageID);
                    if (lineitem.LineItems.Count > 0)
                    {
                        foreach (var lst in lineitem.LineItems)
                        {
                            DashboardFieldMappingTable field = new DashboardFieldMappingTable();
                            field.DashboardMapping = item;
                            field.DashboardTemplateFieldID = lst.DashboardTemplateFieldID;
                            field.FieldName = lst.FieldName;
                            field.DisplayTitle = lst.DisplayTitle;
                            field.XAxisField = lst.XAxisField;
                            field.YAxisField= lst.YAxisField;
                            field.IconPath = lst.IconPath;
                            field.RedirectPageName = lst.RedirectPageName;
                            field.CategoriesField = lst.CategoriesField;
                            field.StatusID = (int)StatusValue.Active;
                            field.CreatedBy = SessionUser.Current.UserID;
                            field.CreatedDateTime = System.DateTime.Now;
                            field.ColorCode = lst.ColorCode;
                            _db.Add(field);
                        }
                    }
                    _db.SaveChanges();

                    DashboardFieldMappingDataModel.RemoveModel(currentPageID);
                    base.TraceLog("DashboardMapping Create-Post", $"{SessionUser.Current.Username} - Dashboardmapping details saved to db");
                    return PartialView("SuccessAction");
                }
            }
            catch (ValidationException ex)
            {
                ViewData["FocusControl"] = ex.FieldName;
                DashboardFieldMappingDataModel.RemoveModel(currentPageID);
                ModelState.AddModelError("12", ex.Message);
            }
            catch (Exception ex)
            {
                DashboardFieldMappingDataModel.RemoveModel(currentPageID);
                return ErrorActionResult(ex);
            }

            return PartialView(item);
        }
        public async Task<ActionResult> IconPathUpload(IFormFile fileNames, int currentPageID)
        {
            try
            {
                base.TraceLog("DashboardMapping IconPathUpload", $"{SessionUser.Current.Username} - IconPathUpload call");
                //string absolutePath = DateTime.Now.ToString("yyyy_MM_dd");
                string fullPath = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/IconPath");
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }


                // The Name of the Upload component is "files".
                if (fileNames != null)
                {
                    var fileContent = ContentDispositionHeaderValue.Parse(fileNames.ContentDisposition);

                    // Some browsers send file names with full path.
                    // The sample is interested only in the file name.
                    string fileExtension = System.IO.Path.GetExtension(Path.GetFileName(fileContent.FileName.ToString().Trim('"')));
                    string time = ComboBoxHelper.RemoveSpecialCharacters(String.Format("{0:" + CultureHelper.DateTimeFormatForGrid + "}", DateTime.Now));
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(fileContent.FileName.ToString().Trim('"').Replace(" ", ""));

                    string newFileName = fileName + "" + time + "" + fileExtension;
                    fullPath = Path.Combine(fullPath, newFileName);

                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await fileNames.CopyToAsync(fileStream);
                    }
                    TempData["UploadedFile"] = fullPath;
                    return Json(new { ImageName = fullPath, fileName = newFileName });
                }
                base.TraceLog("DashboardMapping IconPathUpload", $"{SessionUser.Current.Username} - IconPathUpload call done");
                // Return an empty string to signify success.
                return Content("");
                //return Json(new { ImageName = newFileName }, "text/plain");

            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }

        }

        public ActionResult IconRemove(string fileNames)
        {
            // The parameter of the Remove action must be called "fileNames".
            base.TraceLog("DashboardMapping IconRemove", $"{SessionUser.Current.Username} - IconRemove call");
            if (fileNames != null)
            {

                var fileName = Path.GetFileName(fileNames);
                var physicalPath = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/IconPath", fileName);

                // TODO: Verify user permissions.

                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }

            }
            base.TraceLog("DashboardMapping IconRemove", $"{SessionUser.Current.Username} - IconRemove call done");
            // Return an empty string to signify success.
            return Content("");
        }
        [HttpPost]
        public ActionResult AddLineItems(IFormCollection collection, DashboardFieldMappingModel item, int currentPageID)
        {
            try
            {
                base.TraceLog("DashboardMapping AddLineItems", $"{SessionUser.Current.Username} - AddLineItems call");
                DashboardFieldMappingDataModel model = DashboardFieldMappingDataModel.GetModel(currentPageID);
               
                if (model.LineItems.Where(c => c.DashboardTemplateID == item.DashboardTemplateID && c.DashboardTypeID == item.DashboardTypeID
                            && c.FieldName == item.FieldName).Any())
                {
                    return ErrorActionResult(new Exception($"Asset ({item.FieldName} - {item.DashboardTypeID}) already added"));
                }

                model.LineItems.Add(item);

                base.TraceLog("DashboardMapping AddLineItems", $"{SessionUser.Current.Username} - AddLineItems call done");
                return Content("");
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }

        public IActionResult DeleteModelData(int templateID, int currentPageID, string dashboarTypeID, string fieldName)
        {
            base.TraceLog("DashboardMapping DeleteModelData", $"{SessionUser.Current.Username} - DeleteModelData call.templateID-{templateID}");
            DashboardFieldMappingDataModel model = DashboardFieldMappingDataModel.GetModel(currentPageID);

            var query = model.LineItems.Where(b => b.DashboardTemplateID == templateID && b.DashboardTypeID == templateID && b.FieldName == fieldName ).FirstOrDefault();
            if (query != null)
            {
                model.LineItems.Remove(query);
            }
            base.TraceLog("DashboardMapping DeleteModelData", $"{SessionUser.Current.Username} - DeleteModelData call done");
            return Content("");
        }
        public IActionResult GetItemList([DataSourceRequest] DataSourceRequest request, int currentPageID,int? mappingID=null)
        {
            base.TraceLog("DashboardMapping GetItemList", $"{SessionUser.Current.Username} - GetItemList call");
            DashboardFieldMappingDataModel model = DashboardFieldMappingDataModel.GetModel(currentPageID);
            //if (mappingID.HasValue)
            //{
               
            //    var dsResult = request.ToDataSourceResult(model.LineItems.AsQueryable());
            //    this.TraceLog("Index", $"{SessionUser.Current.Username} - Index page Data Fetch");

            //    return Json(dsResult);
            //}
            //else
            //{
                var dsResult = request.ToDataSourceResult(model.LineItems.AsQueryable());
                this.TraceLog("Index", $"{SessionUser.Current.Username} - Index page Data Fetch");

                return Json(dsResult);
           // }
        }
        public ActionResult Edit(int id)
        {
            try
            {
                if (!base.HasRights(_RightName, UserRightValue.Edit))
                    return GotoUnauthorizedPage();

                base.TraceLog("DashboardMapping Edit", $"{SessionUser.Current.Username} - Edit page requested.id- {id}");
                DashboardMappingTable dept = DashboardMappingTable.GetMapping(_db, id);
                dept.CurrentPageID = SessionUser.Current.GetNextPageID();
                DashboardFieldMappingDataModel model = DashboardFieldMappingDataModel.GetModel(dept.CurrentPageID);
                var lineitem = DashboardFieldMappingTable.GetMappingDetails(_db,id);
                foreach (var line in lineitem.ToList())
                {
                    DashboardFieldMappingModel item = new DashboardFieldMappingModel();
                    item.DisplayTitle = line.DisplayTitle;
                    item.XAxisField = line.XAxisField;
                    item.YAxisField = line.YAxisField;
                    item.CategoriesField = line.CategoriesField;
                    item.FieldName = line.FieldName;
                    item.ColorCode = line.ColorCode;
                    item.IconPath = line.IconPath;
                    item.RedirectPageName = line.RedirectPageName;
                    item.DashboardFieldMappingID = id;
                    item.DashboardTemplateFieldID = line.DashboardTemplateFieldID;
                    item.DashboardMappingID = line.DashboardMappingID;
                    item.DashboardTypeID = line.DashboardMapping.DashboardTypeID;
                    model.LineItems.Add(item);
                }
                return PartialView(dept);
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }
        [HttpPost]
        public IActionResult Edit(DashboardMappingTable item, IFormCollection data)
        {
            if (!base.HasRights(_RightName, UserRightValue.Edit))
                return GotoUnauthorizedPage();
            int currentPageID = int.Parse(data["CurrentPageID"] + "");
            try
            {
                base.TraceLog("DashboardMapping Edit-Post", $"{SessionUser.Current.Username} -Dashboard mapping details will modify to db.id- {item.DashboardMappingID}");
                var oldItm = DashboardMappingTable.GetItem(_db, item.DashboardMappingID);
                item.LastModifiedBy = SessionUser.Current.UserID;
                // item.LastModifyBy = SessionUser.Current.UserID;
                item.LastModifiedDateTime = System.DateTime.Now;
                item.StatusID = (int)StatusValue.Active;
                var fieldtoDelete = DashboardFieldMappingTable.GetColumnLineItemFromFieldTable(_db, item.DashboardMappingID);
                if (fieldtoDelete.Count() > 0)
                {
                    DashboardFieldMappingTable.DeleteExistingMappingItem(_db, fieldtoDelete);
                }
                var lineitem = DashboardFieldMappingDataModel.GetModel(currentPageID);
                if (lineitem.LineItems.Count > 0)
                {
                    foreach (var lst in lineitem.LineItems)
                    {
                        DashboardFieldMappingTable field = new DashboardFieldMappingTable();
                        field.DashboardMappingID = item.DashboardMappingID;
                        field.DashboardTemplateFieldID = lst.DashboardTemplateFieldID;
                        field.FieldName = lst.FieldName;
                        field.DisplayTitle = lst.DisplayTitle;
                        field.XAxisField = lst.XAxisField;
                        field.YAxisField = lst.YAxisField;
                        field.CategoriesField = lst.CategoriesField;
                        field.IconPath = lst.IconPath;
                        field.RedirectPageName = lst.RedirectPageName;
                        field.StatusID = (int)StatusValue.Active;
                        field.CreatedBy = SessionUser.Current.UserID;
                        field.CreatedDateTime = System.DateTime.Now;
                        field.ColorCode = lst.ColorCode;
                        _db.Add(field);
                    }
                }
                DataUtilities.CopyObject(item, oldItm);
                _db.SaveChanges();
                DashboardFieldMappingDataModel.RemoveModel(currentPageID);
                return PartialView("SuccessAction");
            }

            catch (ValidationException ex)
            {
                ViewData["FocusControl"] = ex.FieldName;
                DashboardFieldMappingDataModel.RemoveModel(currentPageID);

                ModelState.AddModelError("12", ex.Message);
            }
            catch (Exception ex)
            {
                DashboardFieldMappingDataModel.RemoveModel(currentPageID);
                return ErrorActionResult(ex);
            }

            return PartialView(item);
        }
        public ActionResult Details(int id)
        {
            try
            {
                if (!base.HasRights(_RightName, UserRightValue.Details))
                    return GotoUnauthorizedPage();
                base.TraceLog("DashboardMapping Details", $"{SessionUser.Current.Username} - Details page request.id- {id}");
                DashboardMappingTable dept = DashboardMappingTable.GetMapping(_db, id);
                dept.CurrentPageID = SessionUser.Current.GetNextPageID();
                return PartialView(dept);
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

                base.TraceLog("DashboardMapping Delete", $"{SessionUser.Current.Username} - delete request. id-{id}");
                DashboardMappingTable item = DashboardMappingTable.GetItem(_db, id);

                if (item != null)
                {

                    var sFItem = (from vv in _db.DashboardFieldMappingTable
                                  where vv.DashboardMappingID == id
                                  select vv);
                    foreach (DashboardFieldMappingTable it in sFItem)
                    {
                        it.StatusID = (int)StatusValue.Deleted;
                    }
                 
                  //  _db.Remove(sFItem);


                    item.Delete();
                    item.UpdateUniqueKey(_db);
                    this._db.SaveChanges();
                    base.TraceLog("DashboardMapping Delete", $"{SessionUser.Current.Username} - delete request completed. id-{id}");
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
            var pageName = "DashboardMapping";

            base.TraceLog("DeleteAll", $"{SessionUser.Current.Username} - {pageName} Delete action requested");
            if (!base.HasRights(RightNames.DashboardMapping, UserRightValue.Delete))
                return RedirectToAction("UnauthorizedPage");

            string checkdItems = toBeDeleteIds;
            string[] arrRequestID = checkdItems.Split(',');
            int[] requestID = Array.ConvertAll(arrRequestID, s => int.Parse(s));
            int requestCount = requestID.Count();
            if (requestCount > 0)
            {
                foreach (var id in requestID)
                {
                    DashboardMappingTable item = DashboardMappingTable.GetItem(_db, id);

                    if (item != null)
                    {

                        var sFItem = (from vv in _db.DashboardFieldMappingTable
                                      where vv.DashboardMappingID == id
                                      select vv);
                        foreach (DashboardFieldMappingTable it in sFItem)
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
        public IActionResult DeleteModelData(int DashboardTemplateID, int currentPageID, int DashboardTypeID, string FieldName)
        {
            base.TraceLog("DashboardMapping DeleteModelData", $"{SessionUser.Current.Username} - delete request. DashboardTemplateID-{DashboardTemplateID}");

            DashboardFieldMappingDataModel model = DashboardFieldMappingDataModel.GetModel(currentPageID);

            var query = model.LineItems.Where(b => b.DashboardTemplateID == DashboardTemplateID && b.DashboardTypeID == DashboardTypeID && b.FieldName == FieldName ).FirstOrDefault();
            if (query != null)
            {

                model.LineItems.Remove(query);

            }
            base.TraceLog("DashboardMapping DeleteModelData", $"{SessionUser.Current.Username} - delete request completed. DashboardTemplateID-{DashboardTemplateID}");
            return Content("");
        }
    }
}
