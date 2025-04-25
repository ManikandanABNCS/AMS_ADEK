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
using System.Net;
using NPOI.POIFS.Storage;
using DocumentFormat.OpenXml.Drawing.Diagrams;

namespace ACS.AMS.WebApp.Controllers
{
    public class AssetRetirementController : ACSBaseController
    {
        private static IWebHostEnvironment WebHostEnvironment;
        public AssetRetirementController(IWebHostEnvironment _environment)
        {
            WebHostEnvironment = _environment;
        }
        public ActionResult Index()
        {
            if (!base.HasRights(RightNames.AssetRetirement, UserRightValue.Create))
                return GotoUnauthorizedPage();
            string pageName = "AssetRetirement";
            this.TraceLog("Index", $"{SessionUser.Current.Username} - {pageName} Index page requested");
            Type entityType = GetEntityObjectType("Transaction", true);
            var obj = Activator.CreateInstance(entityType) as BaseEntityObject;
            SystemDatabaseHelper.GenerateMasterGridColumns(entityType, "Transaction");

            return PartialView("Index",
                new IndexPageModel()
                {
                    PageTitle = pageName,
                    PageName = pageName,
                    EntityInstance = obj,
                    ControllerName = this.GetType().Name.Replace("Controller", ""),
                });
            //base.TraceLog("Asset Retirement Index", $"{SessionUser.Current.Username} -Asset Retirement Index page requested");
            //return PartialView();
        }
        public IActionResult _Index([DataSourceRequest] DataSourceRequest request, int currentPageID)
        {
            try
            {

                if (!base.HasRights(RightNames.AssetRetirement, UserRightValue.View))
                    return RedirectToAction("UnauthorizedPage");
                IQueryable<TransactionView> result = TransactionView.GetAllItems(_db).Where(a => a.TransactionTypeName == "AssetRetirement");

                var line = TransactionTable.GetUserValidationResult(_db, SessionUser.Current.UserID, (int)TransactionTypeValue.AssetRetirement); //TransactionLineItemTable.GetUserBasedTransactionID(_db,SessionUser.Current.UserID,(int)TransactionTypeValue.AssetTransfer);
                if (line.Result.Count() > 0)
                {
                    var ids = line.Result.AsQueryable();
                    var id = (from b in ids select b.TransactionID).ToList();
                    result = result.Where(a => id.Contains(a.TransactionID));
                    // var dsResult = result.ToDataSourceResult(request);
                    var dsResult = request.ToDataSourceResult(result, "AssetRetirementIndex", "TransactionID");
                    base.TraceLog("TransactionList Index", $"{SessionUser.Current.Username} -TransactionList Index page Data Fetch");
                    return Json(dsResult);
                }
                else
                {
                    return Json(new { });
                }
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }
        public ActionResult Create()
        {
            if (!base.HasRights(RightNames.AssetRetirement, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");
            IndexPageModel indexPage = new IndexPageModel();
            indexPage.EntityInstance = new BaseEntityObject();
            indexPage.EntityInstance.CurrentPageID = SessionUser.Current.GetNextPageID();
            RetirementDataModel.RemoveModel(indexPage.EntityInstance.CurrentPageID);

            base.TraceLog("Asset Retirement Index", $"{SessionUser.Current.Username} -Asset Retirement Index page requested");
            return PartialView(indexPage);

        }
        public IActionResult _lineItems([DataSourceRequest] DataSourceRequest request, int currentPageID, int? transactionID = null)
        {
            if (!base.HasRights(RightNames.AssetRetirement, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");

            //  IQueryable<AssetTable> query = AssetTable.GetAllItems(_db);
            IQueryable<AssetNewView> query = AssetNewView.GetAllItems(_db);
            RetirementDataModel model = RetirementDataModel.GetModel(currentPageID);
            if (transactionID.HasValue)
            {
                var lineitem = TransactionLineItemTable.TransactionLineItems(_db, (int)transactionID);
                var doc = DocumentTable.GetDocumentDetails(AMSContext.CreateNewContext(), (int)transactionID, "AssetRetirement");
                foreach (var line in lineitem.ToList())
                {
                    RetirementData item = new RetirementData();
                    var asset = AssetTable.GetItem(_db, line.AssetID);
                    item.Asset = asset;
                    model.LineItems.Add(item);
                }
                foreach (var documnet in doc.ToList())
                {
                    DocumentModel document1 = new DocumentModel();
                    document1.DocumentName = documnet.DocumentName;
                    document1.FileName = documnet.FileName;
                    document1.DocumentID = documnet.DocumentID;
                    document1.CurrentPageID = currentPageID;
                    document1.FullPath = documnet.FilePath;
                    document1.FileExtension = documnet.FileExtension;
                    model.Documents.Add(document1);
                }

            }

            if (model != null)
            {
                List<int> ids = model.LineItems.Select(a => a.Asset.AssetID).ToList();
                query = query.Where(a => ids.Contains(a.AssetID));

                //var dsResult = query.ToDataSourceResult(request);
                var dsResult = request.ToDataSourceResult(query, "AssetRetirement", "AssetID");
                base.TraceLog("Asset Retirement Index", $"{SessionUser.Current.Username} -Asset Retirement Index page Data Fetch");
                return Json(dsResult);
            }
            else
            {
                return Json("");

            }

        }
        //public IActionResult Index()
        //{
        //    if (!base.HasRights(RightNames.AssetRetirement, UserRightValue.View))
        //        return RedirectToAction("UnauthorizedPage");
        //    IndexPageModel indexPage = new IndexPageModel();
        //    indexPage.EntityInstance = new BaseEntityObject();
        //    indexPage.EntityInstance.CurrentPageID = SessionUser.Current.GetNextPageID();
        //    RetirementDataModel.RemoveModel(indexPage.EntityInstance.CurrentPageID);
        //    base.TraceLog("Asset Transfer Index", $"{SessionUser.Current.Username} -Asset Transfer Index page requested");
        //    return PartialView(indexPage);
        //}
        //public IActionResult _Index([DataSourceRequest] DataSourceRequest request, int currentPageID)
        //{
        //    if (!base.HasRights(RightNames.AssetRetirement, UserRightValue.View))
        //        return RedirectToAction("UnauthorizedPage");

        //    IQueryable<AssetTable> query = AssetTable.GetAllItems(_db);
        //    RetirementDataModel model = RetirementDataModel.GetModel(currentPageID);
        //    if (model != null)
        //    {
        //        List<int> ids = model.LineItems.Select(a => a.Asset.AssetID).ToList();
        //        query = query.Where(a => ids.Contains(a.AssetID));
        //        var dsResult = query.ToDataSourceResult(request);
        //        base.TraceLog("UserMaster Index", $"{SessionUser.Current.Username} -UserMaster Index page Data Fetch");
        //        return Json(dsResult);
        //    }
        //    else
        //    {
        //        return Json("");

        //    }

        //}
        [HttpPost]
        public IActionResult Create(IFormCollection data)
        {
            if (!base.HasRights(RightNames.AssetRetirement, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");
            try
            {
                base.TraceLog("AssetRetirement Create-post", $"{SessionUser.Current.Username} -AssetRetirement Create call");
                int currentPageID = int.Parse(data["CurrentPageID"] + "");
                RetirementDataModel model = RetirementDataModel.GetModel(currentPageID);
                if(model.LineItems.Count()>0)
                {
                    var assets = model.LineItems.Select(a => a.Asset.AssetID).ToList();
                    string assetID =  System.String.Join(",", assets);
                    //Session["AssetRetirementID"]
                    //if (!string.IsNullOrEmpty(data["AssetIDS"]))
                    //{
                    //    string assetID = (string)data["AssetIDS"];//Session["AssetRetirementID"]
                    //    string[] arryAssetID = assetID.Split(',');
                    //    int[] intIDS = Array.ConvertAll(arryAssetID, s => int.Parse(s));

                    var allvalid = TransactionTable.GetTransactionResult(_db, assetID, null, (int)ApproveModuleValue.AssetRetirement, RightNames.AssetRetirement);
                    if (!string.IsNullOrEmpty(allvalid.Result))
                    {
                        return base.ErrorActionResult(allvalid.Result);
                    }
                   // bool flag = AppConfigurationManager.GetValue<bool>(AppConfigurationManager.BulkDisposeAssetApproval);
                 
                  
                    TransactionTable tran = new TransactionTable();
                    tran.CreatedBy = SessionUser.Current.UserID;
                    tran.CreatedFrom = "Web";
                    tran.CreatedDateTime = System.DateTime.Now;
                    tran.TransactionTypeID = (int)TransactionTypeValue.AssetRetirement;
                    tran.TransactionNo = CodeGenerationHelper.GetNextCode("AssetRetirement");
                    tran.Remarks = data["Remarks"];
                    tran.StatusID = (int)StatusValue.WaitingForApproval;
                    tran.PostingStatusID = (int)PostingStatusValue.WorkInProgress;
                    tran.TransactionDate = System.DateTime.Now;
                    _db.Add(tran);


                
                    foreach (var asetIDs in model.LineItems.ToList())
                    {
                        var checkAssets = AssetTable.GetItem(_db, asetIDs.Asset.AssetID);
                        TransactionLineItemTable lineitem = new TransactionLineItemTable();
                        lineitem.AssetID = asetIDs.Asset.AssetID;
                        lineitem.Transaction = tran;
                        lineitem.CreatedBy = SessionUser.Current.UserID;
                        lineitem.CreatedDateTime = System.DateTime.Now;
                        lineitem.FromLocationID = checkAssets.LocationID;
                        lineitem.StatusID = (int)StatusValue.WaitingForApproval;
                        _db.Add(lineitem);
                      
                        AssetTable oldasset = AssetTable.GetItem(_db, asetIDs.Asset.AssetID);
                        oldasset.StatusID = (int)StatusValue.WaitingForApproval;
                        // checkAssets.LocationID = int.Parse(data["LocationID"] + "");
                    }

                    _db.SaveChanges();
                    _db.Entry(tran).Reload();
                    Tuple<int?, int?, int?, int?> historyData = TransactionTable.GetApprovalHistoryList(_db, assets, null);

                    ApproveWorkflowLineItemTable.ApproveHistoryMaintanance(_db, (int)historyData.Item2, tran.TransactionID, (int)ApproveModuleValue.AssetRetirement, SessionUser.Current.UserID, (int)historyData.Item1, null, null, historyData.Item4);

                    if (model.Documents.Count > 0)
                    {
                        foreach (var item in model.Documents.Distinct().ToList())
                        {
                            DocumentTable document = new DocumentTable();
                            document.FileName = item.FileName;
                            document.DocumentName = item.DocumentName;
                            document.FilePath = item.FullPath;
                            document.FileExtension = item.FileExtension;
                            document.TransactionType = "AssetRetirement";
                            document.ObjectKeyID = tran.TransactionID;
                            _db.Add(document);
                        }
                        _db.SaveChanges();
                    }
                    RetirementDataModel.RemoveModel(currentPageID);

                    base.TraceLog("Asset Retirement Create Post", $"{SessionUser.Current.Username} -Asset Retirement  details saved to db ");
                    return PartialView("SuccessAction");
                    //}
                    //else
                    //{
                    //    return base.ErrorActionResult("Selected Location Type not created workflow . please create the workflow");
                    //}


                }
                else
                {

                    ModelState.AddModelError("Asset", "Select any asset to Retirement ");
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
            return PartialView(data);
        }

        public async Task<ActionResult> DocumentUpload(IEnumerable<IFormFile> UploadDoc, int currentPageID)
        {
            try
            {
                base.TraceLog("AssetRetirement DocumentUpload", $"{SessionUser.Current.Username} -AssetRetirement DocumentUpload call");
                //string absolutePath = DateTime.Now.ToString("yyyy_MM_dd");
                string rootPath = string.Concat(WebHostEnvironment.WebRootPath, "\\");
                string fullPath = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/TransactionDocument");
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }

                RetirementDataModel model = RetirementDataModel.GetModel(currentPageID);

                // The Name of the Upload component is "files".
                if (UploadDoc != null)
                {

                    foreach (var file in UploadDoc)
                    {
                        DocumentModel document = new DocumentModel();
                        var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

                        // Some browsers send file names with full path.
                        // The sample is interested only in the file name.
                        //var fileName = Path.GetFileName(fileContent.FileName.ToString().Trim('"'));
                        //string fileExtension = System.IO.Path.GetExtension(fileName);
                        //var newFileName = Guid.NewGuid().ToString() + fileExtension;
                        //fullPath = Path.Combine(fullPath, newFileName);
                        string fileExtension = System.IO.Path.GetExtension(Path.GetFileName(fileContent.FileName.ToString().Trim('"')));
                        string time = ComboBoxHelper.RemoveSpecialCharacters(String.Format("{0:" + CultureHelper.DateTimeFormatForGrid + "}", DateTime.Now));
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(fileContent.FileName.ToString().Trim('"').Replace(" ", ""));
                        //  var fileName = Path.GetFileName(fileContent.FileName.ToString().Trim('"'));


                        //var newFileName = fileName + fileExtension; //Guid.NewGuid().ToString() + fileExtension;
                        //string newFileName = fileName + "" + time + "" + fileExtension;
                        string newFileName = Guid.NewGuid() + "" + fileExtension;//fileName + "" + time + "" + fileExtension;
                        fullPath = Path.Combine(fullPath, newFileName).Replace(" ", "");
                        using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        // data."ImagePath" + i = fullPath;
                        //document.CurrentPageID = currentPageID;
                        document.FileName = newFileName.Replace(" ", "");
                        document.DocumentName = Path.GetFileName(fileContent.FileName.ToString().Trim('"')).Replace(" ", "");//newFileName;
                        document.FullPath = fullPath.Replace(rootPath, "").Replace(" ", "");
                        document.FileExtension = fileExtension;
                        document.CurrentPageID = currentPageID;
                        model.Documents.Add(document);
                    }
                }
                base.TraceLog("AssetRetirement DocumentUpload", $"{SessionUser.Current.Username} -AssetRetirement DocumentUpload call done");
                // Return an empty string to signify success.
                return Content("");
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }

        }

        public ActionResult DocumentRemove(string[] fileNames, int currentPageID)
        {
            base.TraceLog("AssetRetirement DocumentRemove", $"{SessionUser.Current.Username} -AssetRetirement DocumentRemove call");
            // The parameter of the Remove action must be called "fileNames".
            RetirementDataModel model = RetirementDataModel.GetModel(currentPageID);
            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    if (model.Documents.Count > 0)
                    {
                        foreach (var item in model.Documents.Distinct().ToList())
                        {
                            var fileName = Path.GetFileName(fullName);
                            if (string.Compare(fileName, item.DocumentName) == 0)
                            {
                                var query = model.Documents.Where(a => a.DocumentName == item.DocumentName).FirstOrDefault();

                                var physicalPath = item.FullPath; //Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/TransferAsset", fileName);

                                // TODO: Verify user permissions.

                                if (System.IO.File.Exists(physicalPath))
                                {
                                    System.IO.File.Delete(physicalPath);
                                    model.Documents.Remove(query);
                                }
                            }
                        }
                    }
                }


            }
            base.TraceLog("AssetRetirement DocumentRemove", $"{SessionUser.Current.Username} -AssetRetirement DocumentRemove call done");
            // Return an empty string to signify success.
            return Content("");
        }
        public FileResult DownloadFile(string fileName)
        {
            base.TraceLog("AssetRetirement DownloadFile", $"{SessionUser.Current.Username} -AssetRetirement DownloadFile call");
            //Build the File Path.
            string path = Path.Combine(WebHostEnvironment.WebRootPath, "ExcelTemplate\\") + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            base.TraceLog("AssetRetirement DownloadFile", $"{SessionUser.Current.Username} -AssetRetirement DownloadFile call done");
            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }
        public FileResult DownloadAttachedFile(string fileName)
        {
            base.TraceLog("AssetRetirement DownloadAttachedFile", $"{SessionUser.Current.Username} -AssetRetirement DownloadAttachedFile call");
            //Build the File Path.
            string path = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath\\") + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            base.TraceLog("AssetRetirement DownloadAttachedFile", $"{SessionUser.Current.Username} -AssetRetirement DownloadAttachedFile call done");
            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }
        [HttpPost]
        public async Task<ActionResult> ImportUpload(IFormFile fileNames, int currentPageID)
        {
            try
            {
                base.TraceLog("AssetRetirement ImportUpload", $"{SessionUser.Current.Username} -AssetRetirement ImportUpload call");
                var filePath = await ReadDocument(fileNames);
                string filePaths = filePath.Item1;
                DataTable dt = filePath.Item2;
                RetirementDataModel model = RetirementDataModel.GetModel(currentPageID);
                List<string> barcodesList = new List<string>();
                List<string> MissingbarcodesList = new List<string>();
                List<string> AvaliablebarcodesList = new List<string>();
                List<string> ActivebarcodesList = new List<string>();
                List<string> UnMappedbarcodesList = new List<string>();
                List<string> SameLocMapping = new List<string>();
                bool flag = AppConfigurationManager.GetValue<bool>(AppConfigurationManager.IsMandatoryLocationType);
                int checkCnt = 0;
                string validation = "";
                if (dt.Rows.Count == 0)
                {
                    validation = validation + " Please fill the excel and upload the file";
                }
                else
                {
                    validation = ComboBoxHelper.ValidateUnique(dt);
                }

                List<string> Barcodelist = dt.Rows.OfType<DataRow>().Select(dr => (string)dr["Barcode"]).Distinct().ToList();
              
                var barcodeResult = System.String.Join(",", Barcodelist);
                var allvalid = TransactionTable.GetTransactionRetriementResult(_db, barcodeResult, SessionUser.Current.UserID);
                if (!string.IsNullOrEmpty(allvalid.Result))
                {
                    base.TraceLog("AssetRetirement ImportUpload", $"{SessionUser.Current.Username} -AssetRetirement ImportUpload call done-onError");
                    RetirementDataModel.RemoveModel(currentPageID);
                   
                    return Json(new { Result = "false", Type = "Upload", FilePath = "", error = validation });
                    //return base.ErrorActionResult(allvalid.Result);
                }
                var assetsDetails = AssetTable.GetAllItems(_db).Where(a => Barcodelist.Contains(a.Barcode));

                foreach(var res in  assetsDetails) {
                    RetirementData item = new RetirementData();
                    item.Asset = res;
                    item.Asset.Attribute40 = "Import Asset Transfer";
                    model.LineItems.Add(item);
                }

               
                //    foreach (DataRow row in dt.Rows)
                //{
                //    //if (checkCnt == 0)
                //    //{
                //    //    checkCnt++;
                //    //    continue;
                //    //}
                //    RetirementData item = new RetirementData();
                //    AssetTable asset = AssetTable.GetAssetBarcode(_db, row["Barcode"] + "");
                //    if (asset != null)
                //    {
                //        if (asset.StatusID == (int)StatusValue.Active)
                //        {
                //            IQueryable<AssetNewView> views = AssetNewView.GetAllUserItem(_db, SessionUser.Current.UserID, true).Where(a => a.StatusID == (int)StatusValue.Active);
                //            var mapping = views.Where(a => a.AssetID == asset.AssetID).FirstOrDefault();
                //            if (mapping != null)
                //            {

                //                var query1 = views.Where(a => a.StatusID == (int)StatusValue.Active);// && a.AssetID == asset.AssetID).FirstOrDefault();
                //                if(flag)
                //                {
                //                    query1 = query1.Where(a => a.LocationType != null);
                //                }
                //                var query = query1.Where(a => a.AssetID == asset.AssetID).FirstOrDefault();
                //                if (query != null)
                //                {
                //                    var userBasedLocation = PersonTable.GetUserBasedLocationList(_db, SessionUser.Current.UserID).Select(a => a.LocationID + "").ToList();
                //                    if (userBasedLocation.Count() > 0)
                //                    {
                //                        if (userBasedLocation.Contains(query.LocationL2))
                //                        {
                //                            if (model.LineItems.Count() > 0)
                //                            {
                //                                List<int> ids = model.LineItems.Select(a => a.Asset.AssetID).ToList();
                //                                if (flag)
                //                                {
                //                                    var list = AssetNewView.GetAllItems(_db).Where(a => ids.Contains(a.AssetID)).Select(a => a.LocationType).Distinct().ToList();
                //                                    var res = AssetNewView.GetItem(_db, asset.AssetID);
                //                                    if (list.Contains(res.LocationType))
                //                                    {
                //                                        item.Asset = asset;
                //                                        item.Asset.Attribute40 = "Import Asset Transfer";
                //                                        model.LineItems.Add(item);
                //                                    }
                //                                    else
                //                                    {
                //                                        SameLocMapping.Add(asset.Barcode);
                //                                    }
                //                                }
                //                                else
                //                                {
                //                                    item.Asset = asset;
                //                                    item.Asset.Attribute40 = "Import Asset Transfer";
                //                                    model.LineItems.Add(item);
                //                                }
                //                            }
                //                            else
                //                            {
                //                                item.Asset = asset;
                //                                item.Asset.Attribute40 = "Import Asset Transfer";
                //                                model.LineItems.Add(item);
                //                            }
                //                        }
                //                        else
                //                        {
                //                            MissingbarcodesList.Add(asset.Barcode);
                //                        }
                //                    }
                //                }
                //                else
                //                {
                //                    barcodesList.Add(asset.Barcode);
                //                }
                //            }
                //            else
                //            {
                //                UnMappedbarcodesList.Add(row["Barcode"] + "");
                //            }
                //        }
                //        else
                //        {
                //            ActivebarcodesList.Add(asset.Barcode);
                //        }



                //    }
                //    else
                //    {
                //        AvaliablebarcodesList.Add(row["Barcode"] + "");
                //    }
                //}
                ////string validation = "";
                //foreach (var bar in AvaliablebarcodesList)
                //{
                //    validation = validation + bar + " Excel given barcode not available in db.";
                //}
               
                //foreach (var bar in barcodesList)
                //{
                //    validation = validation + bar + " Excel given barcode Location Type is Empty in db.";
                //}
                //foreach (var bar in UnMappedbarcodesList)
                //{
                //    validation = validation + bar + " Excel given barcode not matched with configured user Mapping.";
                //}
                //foreach (var bar in SameLocMapping)
                //{
                //    validation = validation + bar + " Excel given barcode different Locationtype.";
                //}
                //foreach (var bar in MissingbarcodesList)
                //{
                //    validation = validation + bar + " Excel given barcode Location not Mapped in db. ";
                //}
                //foreach (var bar in ActivebarcodesList)
                //{
                //    validation = validation + bar + " Excel given barcode Not in Active Status. ";
                //}
                ////    if (asset != null)
                ////    {
                ////        if (model != null)
                ////        {
                ////            List<int> ids = model.LineItems.Select(a => a.Asset.AssetID).ToList();
                ////            var list = AssetNewView.GetAllItems(_db).Where(a => ids.Contains(a.AssetID)).Select(a => a.LocationType).Distinct().ToList();
                ////            var res = AssetNewView.GetItem(_db, asset.AssetID);
                ////            if (list.Contains(res.LocationType))
                ////            {
                ////                item.Asset = asset;
                ////                item.Asset.Attribute40 = "Import Asset Retirement";
                ////                model.LineItems.Add(item);
                ////            }


                ////        }
                ////        else
                ////        {
                ////            item.Asset = asset;
                ////            item.Asset.Attribute40 = "Import Asset Retirement";
                ////            model.LineItems.Add(item);
                ////        }
                ////    }
                ////}

                //if (validation == "")
                //{
                    base.TraceLog("AssetRetirement ImportUpload", $"{SessionUser.Current.Username} -AssetRetirement ImportUpload call done");
                    return Json(new { Result = "success", Type = "Upload", FilePath = filePaths, error = "" });
                //}

                //else
                //{
                //    base.TraceLog("AssetRetirement ImportUpload", $"{SessionUser.Current.Username} -AssetRetirement ImportUpload call done-onError");
                //    RetirementDataModel.RemoveModel(currentPageID);
                //    //Response.StatusCode = (int)HttpStatusCode.BadRequest;
                //    //return Content(validation);
                //    return Json(new { Result = "false", Type = "Upload", FilePath = "", error = validation });
                //}
            }
            catch (Exception ex)
            {
                //AppErrorLogTable.SaveException(WasNowContext.CreateNewContext(), ex);
                ErrorActionResult(ex);
                //RetirementDataModel.RemoveModel(currentPageID);
                //Response.StatusCode = (int)HttpStatusCode.BadRequest;
                //return Content(ex);
                return Json(new { Result = "false", Type = "Upload", FilePath = "", error = ex.Message });

            }
        }
        public async Task<Tuple<string, DataTable>> ReadDocument(IFormFile files)
        {
            base.TraceLog("AssetRetirement ReadDocument", $"{SessionUser.Current.Username} -AssetRetirement ReadDocument call");
            string path = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/AssetRetirementImport");
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
                    //excelTable = ImportExcel.ExcelHeaderName(excelTable, workbook, _db);
                    excelTable = ImportExcel.Import(excelTable, workbook, _db);
                    //return excelTable;
                }
                else
                {
                    XSSFWorkbook workbooks = new XSSFWorkbook(stream);
                    //excelTable = ImportExcel.ExcelHeaderName(excelTable, workbooks, _db);
                    excelTable = ImportExcel.Import(excelTable, workbooks, _db);
                    //return excelTable;
                }
            }

            Tuple<string, DataTable> obj = new Tuple<string, DataTable>(filePath, excelTable);
            base.TraceLog("AssetRetirement ReadDocument", $"{SessionUser.Current.Username} -AssetRetirement ReadDocument call done");
            return obj;

        }
        public ActionResult ImportRemove(string[] fileNames, int currentPageID)
        {
            base.TraceLog("AssetRetirement ImportRemove", $"{SessionUser.Current.Username} -AssetRetirement ImportRemove call");
            // The parameter of the Remove action must be called "fileNames".
            // UniformRequestLineItemDetailsModel.ClearCurrentModel(currentPageID);
            RetirementDataModel model = RetirementDataModel.GetModel(currentPageID);
            if (fileNames != null)
            {
                if (model.LineItems.Count > 0)
                {
                    RetirementDataModel.RemoveModel(currentPageID);
                }
            }
            base.TraceLog("AssetRetirement ImportRemove", $"{SessionUser.Current.Username} -AssetRetirement ImportRemove call done");
            // Return an empty string to signify success.
            return Json(new { Result = "success", Type = "Remove", FilePath = "" });
        }
        public IActionResult _deleteItem(int id, int currentPageID)
        {
            base.TraceLog("AssetRetirement _deleteItem", $"{SessionUser.Current.Username} -AssetRetirement _deleteItem call");
            RetirementDataModel model = RetirementDataModel.GetModel(currentPageID);

            var query = model.LineItems.Where(b => b.Asset.AssetID == id).FirstOrDefault();
            if (query != null)
                model.LineItems.Remove(query);
            base.TraceLog("AssetRetirement _deleteItem", $"{SessionUser.Current.Username} -AssetRetirement _deleteItem call done");
            return Content("");
        }

        // VaildateDoc
        [HttpPost]
        public IActionResult DraftMode(IFormCollection data)
        {
            if (!base.HasRights(RightNames.AssetRetirement, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");
            try
            {
                base.TraceLog("AssetRetirement DraftMode", $"{SessionUser.Current.Username} -AssetRetirement DraftMode call");
                if (!string.IsNullOrEmpty(data["AssetIDS"]))
                {
                    string assetID = (string)data["AssetIDS"];//Session["AssetRetirementID"]
                    string[] arryAssetID = assetID.Split(',');
                    int[] intIDS = Array.ConvertAll(arryAssetID, s => int.Parse(s));
                    var list = AssetNewView.GetAllItems(_db).Where(a => intIDS.Contains(a.AssetID)).Select(a => a.LocationType).Distinct().FirstOrDefault();
                    var listID = AssetNewView.GetAllItems(_db).Where(a => intIDS.Contains(a.AssetID)).Select(a => a.LocationL2).Distinct().FirstOrDefault();
                    int fromID = LocationTable.GetLocationType(_db, list);

                    var approvalworkflow = (from b in _db.ApproveWorkflowTable
                                            where b.FromLocationTypeID == fromID
                                            && b.ApproveModuleID == (int)ApproveModuleValue.AssetRetirement
                                            select b).FirstOrDefault();
                    if (approvalworkflow != null)
                    {
                        int currentPageID = int.Parse(data["CurrentPageID"] + "");
                        TransactionTable tran = new TransactionTable();
                        tran.CreatedBy = SessionUser.Current.UserID;
                        tran.CreatedFrom = "Web";
                        tran.CreatedDateTime = System.DateTime.Now;
                        tran.TransactionTypeID = (int)TransactionTypeValue.AssetRetirement;
                        tran.TransactionNo = CodeGenerationHelper.GetNextCode("AssetRetirement");
                        tran.Remarks = data["Remarks"];
                        tran.StatusID = (int)StatusValue.Draft;
                        tran.PostingStatusID = (int)PostingStatusValue.WorkInProgress;
                        tran.TransactionDate = System.DateTime.Now;
                        _db.Add(tran);


                        RetirementDataModel model = RetirementDataModel.GetModel(currentPageID);
                        foreach (var asetIDs in intIDS)
                        {
                            var checkAssets = AssetTable.GetItem(_db, asetIDs);
                            TransactionLineItemTable lineitem = new TransactionLineItemTable();
                            lineitem.AssetID = asetIDs;
                            lineitem.Transaction = tran;
                            lineitem.CreatedBy = SessionUser.Current.UserID;
                            lineitem.CreatedDateTime = System.DateTime.Now;
                            lineitem.FromLocationID = checkAssets.LocationID;

                            lineitem.StatusID = (int)StatusValue.WaitingForApproval;
                            _db.Add(lineitem);



                        }

                        _db.SaveChanges();
                        _db.Entry(tran).Reload();

                        if (model.Documents.Count > 0)
                        {
                            foreach (var item in model.Documents.Distinct().ToList())
                            {
                                DocumentTable document = new DocumentTable();
                                document.FileName = item.FileName;
                                document.DocumentName = item.DocumentName;
                                document.FilePath = item.FullPath;
                                document.FileExtension = item.FileExtension;
                                document.TransactionType = "AssetRetirement";
                                document.ObjectKeyID = tran.TransactionID;
                                _db.Add(document);
                            }
                            _db.SaveChanges();
                        }
                        RetirementDataModel.RemoveModel(currentPageID);

                        base.TraceLog("Asset Retirement Draft Post", $"{SessionUser.Current.Username} -Asset Retirement  details saved to db ");

                        return Json(new { Result = "Success", message = " Asset Retirement Draft Save Successfully" });
                    }
                    else
                    {
                        return Json(new { Result = "Error", message = "Selected Location Type not created workflow . please create the workflow" });
                        // return base.ErrorActionResult("Selected Location Type not created workflow . please create the workflow");
                    }
                }
                else
                {
                    return Json(new { Result = "Error", message = "Asset\", \"Select any asset to Retirement" });
                    //ModelState.AddModelError("Asset", "Select any asset to Retirement ");
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
            return PartialView(data);
        }

        public IActionResult Edit(int id)
        {
            if (!base.HasRights(RightNames.AssetRetirement, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");

            IndexPageModel indexPage = new IndexPageModel();
            indexPage.EntityInstance = new BaseEntityObject();
            indexPage.EntityInstance.CurrentPageID = SessionUser.Current.GetNextPageID();
            indexPage.PageTitle = id + "";

            ViewBag.TransactionID = id;

            TransactionTable old = TransactionTable.GetItem(_db, id);
            if (old.StatusID == (int)StatusValue.Draft)
            {
                return PartialView(indexPage);
            }
            else if (old.StatusID == (int)StatusValue.Active || old.StatusID == (int)StatusValue.WaitingForApproval)
            {
                return PartialView("Details", indexPage);
            }
            base.TraceLog("Asset Retirement Edit", $"{SessionUser.Current.Username} -Asset Retirement Edit page requested");
            return PartialView(indexPage);
        }

        public IActionResult Details(int id)
        {
            if (!base.HasRights(RightNames.AssetRetirement, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");
            IndexPageModel indexPage = new IndexPageModel();
            indexPage.EntityInstance = new BaseEntityObject();
            indexPage.EntityInstance.CurrentPageID = SessionUser.Current.GetNextPageID();
            indexPage.PageTitle = id + "";

            ViewBag.TransactionID = id;

            base.TraceLog("Asset Retirement Details", $"{SessionUser.Current.Username} -Asset Retirement Details page requested");
            return PartialView(indexPage);
        }

        [HttpPost]
        public IActionResult Edit(IFormCollection data)
        {
            if (!base.HasRights(RightNames.AssetRetirement, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");
            try
            {
                base.TraceLog("Asset Retirement Edit Post", $"{SessionUser.Current.Username} -Asset Retirement  details will save to db ");
                if (!string.IsNullOrEmpty(data["AssetIDS"]))
                {
                    string assetID = (string)data["AssetIDS"];//Session["AssetRetirementID"]
                    string[] arryAssetID = assetID.Split(',');
                    int[] intIDS = Array.ConvertAll(arryAssetID, s => int.Parse(s));
                    //var list = AssetNewView.GetAllItems(_db).Where(a => intIDS.Contains(a.AssetID)).Select(a => a.LocationType).Distinct().FirstOrDefault();
                    //var listID = AssetNewView.GetAllItems(_db).Where(a => intIDS.Contains(a.AssetID)).Select(a => a.LocationL2).Distinct().FirstOrDefault();
                    //int fromID = LocationTable.GetLocationType(_db, list);

                    //int? categoryTypeID = null;
                    //var category = AssetNewView.GetAllItems(_db).Where(a => intIDS.Contains(a.AssetID)).Select(a => a.CategoryType).Distinct().ToList();
                    //if (category.Count() == 1)
                    //{
                    //    foreach (var str in category)
                    //    {
                    //        categoryTypeID = CategoryTypeTable.GetCategoryTypeDetails(_db, str).CategoryTypeID;
                    //    }

                    //}
                    //else
                    //{
                    //    var res = CategoryTypeTable.GetAllItems(_db).Where(a => a.IsAllCategoryType == true).FirstOrDefault();
                    //    if (res == null)
                    //    {
                    //        return base.ErrorActionResult("Selected Assets have both IT and NON-IT so CategoryType-All Should be created");
                    //    }
                    //    categoryTypeID = res.CategoryTypeID;

                    //}

                    //var approvalworkflow = (from b in _db.ApproveWorkflowTable
                    //                        where b.FromLocationTypeID == fromID
                    //                && b.ApproveModuleID == (int)ApproveModuleValue.AssetRetirement
                    //                        select b).FirstOrDefault();
                    //int validation = 0;

                    ////string errorMsg = string.Empty;
                    //if (approvalworkflow != null)
                    //{

                    var allvalid = TransactionTable.GetTransactionResult(_db, assetID, null, (int)ApproveModuleValue.AssetRetirement, RightNames.AssetRetirement);
                    if (!string.IsNullOrEmpty(allvalid.Result))
                    {
                        return base.ErrorActionResult(allvalid.Result);
                    }

                    // }
                    //if (approvalworkflow != null)
                    //{
                    int currentPageID = int.Parse(data["CurrentPageID"] + "");
                    int transaction = int.Parse(data["TransactionID"] + "");
                    TransactionTable tran = TransactionTable.GetItem(_db, transaction);
                    tran.StatusID = (int)StatusValue.WaitingForApproval;

                    RetirementDataModel model = RetirementDataModel.GetModel(currentPageID);
                    foreach (var asetIDs in intIDS)
                    {
                        var oldlineitem = TransactionLineItemTable.LineItems(_db, transaction, asetIDs);
                        if (oldlineitem == null)
                        {
                            var checkAssets = AssetTable.GetItem(_db, asetIDs);

                            TransactionLineItemTable lineitem = new TransactionLineItemTable();
                            lineitem.AssetID = asetIDs;
                            lineitem.Transaction = tran;
                            lineitem.CreatedBy = SessionUser.Current.UserID;
                            lineitem.CreatedDateTime = System.DateTime.Now;
                            lineitem.FromLocationID = checkAssets.LocationID;

                            lineitem.StatusID = (int)StatusValue.WaitingForApproval;
                            _db.Add(lineitem);
                            AssetTable oldasset = AssetTable.GetItem(_db, asetIDs);
                            oldasset.StatusID = (int)StatusValue.WaitingForApproval;

                            //AssetTransferHistoryTable historyTable = new AssetTransferHistoryTable();
                            //historyTable.AssetID = asetIDs;
                            //historyTable.OldLocationID = int.Parse(listID);//checkAssets.LocationID;
                            //historyTable.NewLocationID = int.Parse(data["LocationID"] + "");
                            //historyTable.TransferFrom = "Web";
                            //historyTable.TransferDate = System.DateTime.Now;
                            //historyTable.TransferRemarks = data["Remarks"];
                            //historyTable.TransferBy = SessionUser.Current.UserID;
                            //historyTable.StatusID = (int)StatusValue.WaitingForApproval;//(int)StatusValue.Active;
                            //historyTable.DueDate = System.DateTime.Now;
                            //_db.Add(historyTable);
                        }
                    }

                    _db.SaveChanges();
                    _db.Entry(tran).Reload();
                    Tuple<int?, int?, int?, int?> historyData = TransactionTable.GetApprovalHistoryList(_db, intIDS.ToList(), null);

                    ApproveWorkflowLineItemTable.ApproveHistoryMaintanance(_db, (int)historyData.Item2, tran.TransactionID, (int)ApproveModuleValue.AssetRetirement, SessionUser.Current.UserID, (int)historyData.Item1, null, null, historyData.Item4);
                    if (model.Documents.Count > 0)
                    {
                        DocumentTable.deleteDocument(_db, transaction, "AssetRetirement");
                        foreach (var item in model.Documents.Distinct().ToList())
                        {
                            DocumentTable document = new DocumentTable();
                            document.FileName = item.FileName;
                            document.DocumentName = item.DocumentName;
                            document.FilePath = item.FullPath;
                            document.FileExtension = item.FileExtension;
                            document.TransactionType = "AssetRetirement";
                            document.ObjectKeyID = tran.TransactionID;
                            _db.Add(document);
                        }
                        _db.SaveChanges();
                    }
                    TransferAssetDataModel.RemoveModel(currentPageID);

                    base.TraceLog("Asset Retirement Edir Post", $"{SessionUser.Current.Username} -Asset Retirement  details saved to db ");
                    return PartialView("SuccessAction");
                    //}
                    //else
                    //{
                    //    return base.ErrorActionResult("Selected Location Type not created workflow . please create the workflow");
                    //}
                }
                else
                {
                    ModelState.AddModelError("Asset", "Select any asset to transfers ");
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
            return PartialView(data);
        }

        [HttpPost]
        public IActionResult EditDraftMode(IFormCollection data)
        {
            if (!base.HasRights(RightNames.AssetRetirement, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");
            try
            {
                base.TraceLog("Asset Retirement EditDraftMode", $"{SessionUser.Current.Username} -Asset Retirement  details will save to db ");
                if (!string.IsNullOrEmpty(data["AssetIDS"]))
                {
                    string assetID = (string)data["AssetIDS"];//Session["AssetRetirementID"]
                    string[] arryAssetID = assetID.Split(',');
                    int[] intIDS = Array.ConvertAll(arryAssetID, s => int.Parse(s));
                    var list = AssetNewView.GetAllItems(_db).Where(a => intIDS.Contains(a.AssetID)).Select(a => a.LocationType).Distinct().FirstOrDefault();
                    var listID = AssetNewView.GetAllItems(_db).Where(a => intIDS.Contains(a.AssetID)).Select(a => a.LocationL2).Distinct().FirstOrDefault();
                    int fromID = LocationTable.GetLocationType(_db, list);

                    if (int.Parse(listID) == int.Parse(data["LocationID"] + ""))
                    {
                        return base.ErrorActionResult("From and To Location are the Same , Please select different Location");
                    }

                    var approvalworkflow = (from b in _db.ApproveWorkflowTable
                                            where b.FromLocationTypeID == fromID && b.ApproveModuleID == (int)ApproveModuleValue.AssetRetirement
                                            select b).FirstOrDefault();

                    if (approvalworkflow != null)
                    {
                        int currentPageID = int.Parse(data["CurrentPageID"] + "");
                        int transaction = int.Parse(data["TransactionID"] + "");
                        TransactionTable tran = TransactionTable.GetItem(_db, transaction);
                        tran.StatusID = (int)StatusValue.Draft;

                        RetirementDataModel model = RetirementDataModel.GetModel(currentPageID);
                        foreach (var asetIDs in intIDS)
                        {
                            var oldlineitem = TransactionLineItemTable.LineItems(_db, transaction, asetIDs);
                            if (oldlineitem == null)
                            {
                                var checkAssets = AssetTable.GetItem(_db, asetIDs);

                                TransactionLineItemTable lineitem = new TransactionLineItemTable();
                                lineitem.AssetID = asetIDs;
                                lineitem.Transaction = tran;
                                lineitem.CreatedBy = SessionUser.Current.UserID;
                                lineitem.CreatedDateTime = System.DateTime.Now;
                                lineitem.FromLocationID = checkAssets.LocationID;

                                lineitem.StatusID = (int)StatusValue.WaitingForApproval;
                                _db.Add(lineitem);
                            }
                        }

                        _db.SaveChanges();
                        _db.Entry(tran).Reload();

                        if (model.Documents.Count > 0)
                        {
                            DocumentTable.deleteDocument(_db, transaction, "AssetRetirement");
                            foreach (var item in model.Documents.Distinct().ToList())
                            {
                                DocumentTable document = new DocumentTable();
                                document.FileName = item.FileName;
                                document.DocumentName = item.DocumentName;
                                document.FilePath = item.FullPath;
                                document.FileExtension = item.FileExtension;
                                document.TransactionType = "AssetRetirement";
                                document.ObjectKeyID = tran.TransactionID;
                                _db.Add(document);
                            }
                            _db.SaveChanges();
                        }

                        TransferAssetDataModel.RemoveModel(currentPageID);

                        base.TraceLog("Asset Retirement Edir draft Post", $"{SessionUser.Current.Username} -Asset Retirement  details saved to db ");
                        return Json(new { Result = "Success", message = " Asset Retirement Saved Successfully" });
                    }
                    else
                    {
                        base.TraceLog("Asset Retirement Edir draft Post", $"{SessionUser.Current.Username} -Asset Retirement  error ");
                        return Json(new { Result = "Error", message = "Selected Location Type not created workflow . please create the workflow" });
                        // return base.ErrorActionResult("Selected Location Type not created workflow . please create the workflow");
                    }
                }
                else
                {
                    return Json(new { Result = "Error", message = "Select any asset to transfers" });
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
            return PartialView(data);
        }
        
        public IActionResult _LineItemApproval([DataSourceRequest] DataSourceRequest request, int id)
        {
            base.TraceLog("Asset Retirement LineItemApproval", $"{SessionUser.Current.Username} -Asset Retirement  LineItemApproval call ");
            // var query = uniformRequestDetailsDataModel;
            var transID = TransactionTable.GetItem(_db, id);
            var history = ApprovalHistoryTable.GetTransaction(_db, id, (int)ApproveModuleValue.AssetRetirement);
            var query = ApprovalHistoryView.GetAllItems(_db, true).Where(a => a.TransactionID == transID.TransactionID && a.ApproveModuleID == history.ApproveModuleID).OrderBy(a => a.CreatedDateTime).ThenBy(a => a.OrderNo);

            var dsResult = query.ToDataSourceResult(request);

            // var dsResult = request.ToDataSourceResult(query.AsQueryable(), "UniformRequestUniformItemEntry", "UniformRequestLineItemID");
            this.TraceLog("Index", $"{SessionUser.Current.Username} - Index page Data Fetch");
            base.TraceLog("Asset Retirement LineItemApproval", $"{SessionUser.Current.Username} -Asset Retirement  LineItemApproval call Done ");
            return Json(dsResult);
        }

        public ActionResult DownloadApprovalFile(string fileName, int id)
        {
            base.TraceLog("Asset Transfer DownloadApprovalFile", $"{SessionUser.Current.Username} -DownloadApprovalFile call .id- {id}");
            //Build the File Path.
            var history = ApprovalHistoryTable.GetApproval(_db, id);

            string moduleName = string.Concat(history.ApproveModule.ModuleName, "Approval");
            if (history.ApproveModuleID == (int)ApproveModuleValue.InternalAssetTransfer)
            {
                moduleName = "AssetTransferApproval";
            }
            var document = (from b in _db.DocumentTable
                            where b.ObjectKeyID == id
                          && b.TransactionType == moduleName && b.DocumentName == fileName.Trim()
                            select b).FirstOrDefault();

            string path = document.FilePath;
            base.TraceLog("Asset Transfer DownloadApprovalFile", $"{SessionUser.Current.Username} -DownloadApprovalFile call Done");
            //////Read the File data into Byte Array.
            //byte[] bytes = System.IO.File.ReadAllBytes(path);

            ////Send the File to Download.
            //return File(bytes, "application/octet-stream", document.FileName);
            return Json(new { Result = "Success", type = document.TransactionType, fileName = document.FileName });
        }
    }
}