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
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using Org.BouncyCastle.Crypto;
using DocumentFormat.OpenXml.Math;

namespace ACS.AMS.WebApp.Controllers
{
    public class AssetTransferController : ACSBaseController
    {
        private static IWebHostEnvironment WebHostEnvironment;
        public AssetTransferController(IWebHostEnvironment _environment)
        {
            WebHostEnvironment = _environment;
        }

        //public IActionResult Index()
        //{
        //    if (!base.HasRights(RightNames.AssetTransfer, UserRightValue.View))
        //        return RedirectToAction("UnauthorizedPage");
        //    IndexPageModel indexPage = new IndexPageModel();
        //    indexPage.EntityInstance = new BaseEntityObject();
        //    indexPage.EntityInstance.CurrentPageID = SessionUser.Current.GetNextPageID();
        //    TransferAssetDataModel.RemoveModel(indexPage.EntityInstance.CurrentPageID);

        //    base.TraceLog("Asset Transfer Index", $"{SessionUser.Current.Username} -Asset Transfer Index page requested");
        //    return PartialView(indexPage);
        //}
        public ActionResult Index()
        {
            if (!base.HasRights(RightNames.AssetTransfer, UserRightValue.Create))
                return GotoUnauthorizedPage();

            string pageName = "AssetTransfer";
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
        }
        public IActionResult _Index([DataSourceRequest] DataSourceRequest request, int currentPageID)
        {
            if (!base.HasRights(RightNames.AssetTransfer, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");
            IQueryable<TransactionView> result = TransactionView.GetAllItems(_db).Where(a => a.TransactionTypeName == "AssetTransfer");
            var line = TransactionTable.GetUserValidationResult(_db, SessionUser.Current.UserID, (int)TransactionTypeValue.AssetTransfer); //TransactionLineItemTable.GetUserBasedTransactionID(_db,SessionUser.Current.UserID,(int)TransactionTypeValue.AssetTransfer);
            if (line.Result.Count() > 0)
            {
                var ids = line.Result.AsQueryable();
                var id = (from b in ids select b.TransactionID).ToList();
                result = result.Where(a => id.Contains(a.TransactionID));
                // var dsResult = result.ToDataSourceResult(request);
                var dsResult = request.ToDataSourceResult(result, "AssetTransferIndex", "TransactionID");
                base.TraceLog("Asset Transfer Index", $"{SessionUser.Current.Username} -Asset Transfer Index page Data Fetch");
                return Json(dsResult);
            }
            else
            {
                return Json(new { });
            }
        }

        public ActionResult Create()
        {
            if (!base.HasRights(RightNames.AssetTransfer, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");
            IndexPageModel indexPage = new IndexPageModel();
            indexPage.EntityInstance = new BaseEntityObject();
            indexPage.EntityInstance.CurrentPageID = SessionUser.Current.GetNextPageID();
            TransferAssetDataModel.RemoveModel(indexPage.EntityInstance.CurrentPageID);

            base.TraceLog("Asset Transfer Index", $"{SessionUser.Current.Username} -Asset Transfer Index page requested");
            return PartialView(indexPage);
        }

        public IActionResult _lineItems([DataSourceRequest] DataSourceRequest request, int currentPageID, int? transactionID = null)
        {
            if (!base.HasRights(RightNames.AssetTransfer, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");

            IQueryable<AssetNewView> query = AssetNewView.GetAllUserItems(_db, SessionUser.Current.UserID, false);
            // IQueryable<AssetTable> query = AssetTable.GetAllAssets(_db);
            TransferAssetDataModel model = TransferAssetDataModel.GetModel(currentPageID);
            if (transactionID.HasValue)
            {
                var lineitem = TransactionLineItemTable.TransactionLineItems(_db, (int)transactionID);
                var doc = DocumentTable.GetDocumentDetails(AMSContext.CreateNewContext(), (int)transactionID, "AssetTransfer");
                foreach (var line in lineitem.ToList())
                {
                    TransferAssetData item = new TransferAssetData();
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
                var dsResult = request.ToDataSourceResult(query, "AssetTransfer", "AssetID");
                //var dsResult = query.ToDataSourceResult(request);
                base.TraceLog("AssetTransfer Index", $"{SessionUser.Current.Username} -AssetTransfer Index page Data Fetch");
                return Json(dsResult);
            }
            else
            {
                return Json("");
            }
        }

        [HttpPost]
        public IActionResult Create(IFormCollection data)
        {
            if (!base.HasRights(RightNames.AssetTransfer, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");

            try
            {
                base.TraceLog("AssetTransfer Create-Post", $"{SessionUser.Current.Username} -AssetTransfer details will save to db");
                if (!string.IsNullOrEmpty(data["AssetIDS"]))
                {
                    string assetID = (string)data["AssetIDS"];
                    string[] arryAssetID = assetID.Split(',');
                    int[] intIDS = Array.ConvertAll(arryAssetID, s => int.Parse(s));
                    int toLocationID = int.Parse(data["LocationID"] + "");
                    //  var list = AssetNewView.GetAllItems(_db).Where(a => intIDS.Contains(a.AssetID)).Select(a => a.LocationType).Distinct().FirstOrDefault();
                    //  var listID = AssetNewView.GetAllItems(_db).Where(a => intIDS.Contains(a.AssetID)).Select(a => a.LocationL2).Distinct().FirstOrDefault();
                    //  int fromID = LocationTable.GetLocationType(_db, list);
                    //  int? categoryTypeID = null;
                    //  bool approval = AppConfigurationManager.GetValue<bool>(AppConfigurationManager.TransferAssetApproval);
                    //  if(approval)
                    //  { }
                    //  var category = AssetNewView.GetAllItems(_db).Where(a => intIDS.Contains(a.AssetID)).Select(a => a.CategoryType).Distinct().ToList();
                    //  if (category.Count() == 1)
                    //  {
                    //      foreach (var str in category)
                    //      {
                    //          categoryTypeID = CategoryTypeTable.GetCategoryTypeDetails(_db, str).CategoryTypeID;
                    //      }

                    //  }
                    //  else
                    //  {
                    //      var res = CategoryTypeTable.GetAllItems(_db).Where(a => a.IsAllCategoryType == true).FirstOrDefault();
                    //      if (res == null)
                    //      {
                    //          return base.ErrorActionResult("Selected Assets have both IT and NON-IT so CategoryType-All Should be created");
                    //      }
                    //      categoryTypeID = res.CategoryTypeID;

                    //  }

                    // 
                    //  if (int.Parse(listID) == toLocationID)
                    //  {
                    //      return base.ErrorActionResult("From and To Location are the Same , Please select different Location");
                    //  }

                    //  var loclist = LocationTable.GetItem(_db, toLocationID).LocationTypeID;
                    //  var approvalworkflow = (from b in _db.ApproveWorkflowTable where b.FromLocationTypeID == fromID && b.ToLocationTypeID == loclist 
                    //                              && b.ApproveModuleID==(int)ApproveModuleValue.AssetTransfer 
                    //                              && b.StatusID==(int)StatusValue.Active select b).FirstOrDefault();
                    //  int validation = 0;
                    ////  string errorMsg = string.Empty;

                    //  if (approvalworkflow != null)
                    //  {

                    var allvalid = TransactionTable.GetTransactionResult(_db, assetID, toLocationID, (int)ApproveModuleValue.AssetTransfer, RightNames.AssetTransfer);
                    if (!string.IsNullOrEmpty(allvalid.Result))
                    {
                        return base.ErrorActionResult(allvalid.Result);
                    }


                    //  }
                    //if (approvalworkflow != null)
                    //{
                    int currentPageID = int.Parse(data["CurrentPageID"] + "");
                    TransactionTable tran = new TransactionTable();
                    tran.CreatedBy = SessionUser.Current.UserID;
                    tran.CreatedFrom = "Web";
                    tran.CreatedDateTime = System.DateTime.Now;
                    tran.TransactionTypeID = (int)TransactionTypeValue.AssetTransfer;
                    tran.TransactionNo = CodeGenerationHelper.GetNextCode("AssetTransfer");
                    tran.Remarks = data["Remarks"];
                    tran.StatusID = (int)StatusValue.WaitingForApproval;
                    tran.PostingStatusID = (int)PostingStatusValue.WorkInProgress;
                    tran.TransactionDate = System.DateTime.Now;
                    _db.Add(tran);


                    TransferAssetDataModel model = TransferAssetDataModel.GetModel(currentPageID);
                    foreach (var asetIDs in intIDS)
                    {
                        var checkAssets = AssetTable.GetItem(_db, asetIDs);
                        TransactionLineItemTable lineitem = new TransactionLineItemTable();
                        lineitem.AssetID = asetIDs;
                        lineitem.Transaction = tran;
                        lineitem.CreatedBy = SessionUser.Current.UserID;
                        lineitem.CreatedDateTime = System.DateTime.Now;
                        lineitem.FromLocationID = checkAssets.LocationID;
                        lineitem.ToLocationID = int.Parse(data["LocationID"] + "");
                        lineitem.StatusID = (int)StatusValue.WaitingForApproval;
                        if (!string.IsNullOrEmpty(data["TransferTypeID"]))
                        {
                            lineitem.TransferTypeID = int.Parse(data["TransferTypeID"]);
                        }
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

                    _db.SaveChanges();
                    _db.Entry(tran).Reload();
                    Tuple<int?, int?, int?, int?> historyData = TransactionTable.GetApprovalHistoryList(_db, intIDS.ToList(), toLocationID);
                    ApproveWorkflowLineItemTable.ApproveHistoryMaintanance(_db, (int)historyData.Item2, tran.TransactionID, (int)ApproveModuleValue.AssetTransfer, SessionUser.Current.UserID, (int)historyData.Item1, int.Parse(data["LocationID"] + ""), (int)historyData.Item3, historyData.Item4);
                    if (model.Documents.Count > 0)
                    {
                        foreach (var item in model.Documents.Distinct().ToList())
                        {
                            DocumentTable document = new DocumentTable();
                            document.FileName = item.FileName;
                            document.DocumentName = item.DocumentName;
                            document.FilePath = item.FullPath;
                            document.FileExtension = item.FileExtension;
                            document.TransactionType = "AssetTransfer";
                            document.ObjectKeyID = tran.TransactionID;
                            _db.Add(document);
                        }
                        _db.SaveChanges();
                    }
                    TransferAssetDataModel.RemoveModel(currentPageID);

                    base.TraceLog("Asset Transfer Create Post", $"{SessionUser.Current.Username} -Asset Transfer  details saved to db ");
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
        public IActionResult Edit(int id)
        {
            if (!base.HasRights(RightNames.AssetTransfer, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("AssetTransfer Edit", $"{SessionUser.Current.Username} -AssetTransfer Edit page request.id- {id}");
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
            else if (old.StatusID == (int)StatusValue.Active || old.StatusID == (int)StatusValue.WaitingForApproval || old.StatusID == (int)StatusValue.Rejected)
            {
                return PartialView("Details", indexPage);
            }
            base.TraceLog("Asset Transfer Index", $"{SessionUser.Current.Username} -Asset Transfer Edit page requested.id- {id} ");
            return PartialView(indexPage);

        }

        public IActionResult Details(int id)
        {
            if (!base.HasRights(RightNames.AssetTransfer, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("AssetTransfer Details", $"{SessionUser.Current.Username} -AssetTransfer Details page request.id- {id}");
            IndexPageModel indexPage = new IndexPageModel();
            indexPage.EntityInstance = new BaseEntityObject();
            indexPage.EntityInstance.CurrentPageID = SessionUser.Current.GetNextPageID();
            indexPage.PageTitle = id + "";

            ViewBag.TransactionID = id;

            base.TraceLog("Asset Transfer Details", $"{SessionUser.Current.Username} -Asset Transfer Details page requested");
            return PartialView(indexPage);

        }
        public IActionResult _LineItemApproval([DataSourceRequest] DataSourceRequest request, int id)
        {
            // var query = uniformRequestDetailsDataModel;
            var transID = TransactionTable.GetItem(_db, id);
            var history = ApprovalHistoryTable.GetTransaction(_db, id, (int)ApproveModuleValue.AssetTransfer);
            var query = ApprovalHistoryView.GetAllItems(_db, true).Where(a => a.TransactionID == transID.TransactionID && a.ApproveModuleID == history.ApproveModuleID).OrderBy(a => a.CreatedDateTime).ThenBy(a => a.OrderNo);

            var dsResult = query.ToDataSourceResult(request);

            // var dsResult = request.ToDataSourceResult(query.AsQueryable(), "UniformRequestUniformItemEntry", "UniformRequestLineItemID");
            this.TraceLog("Asset Transfer _LineItemApproval", $"{SessionUser.Current.Username} - Index page Data Fetch.id- {id}");

            return Json(dsResult);
        }
        public async Task<ActionResult> DocumentUpload(IEnumerable<IFormFile> UploadDoc, int currentPageID)
        {
            try
            {
                this.TraceLog("Asset Transfer DocumentUpload", $"{SessionUser.Current.Username} - DocumentUpload call");
                // string absolutePath = DateTime.Now.ToString("yyyy_MM_dd");
                string rootPath = string.Concat(WebHostEnvironment.WebRootPath, "\\");
                string fullPath = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/TransactionDocument");
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                //DocumentDataModel.RemoveModel(currentPageID);
                //DocumentDataModel detailsDocuments = DocumentDataModel.GetModel(currentPageID);
                TransferAssetDataModel model = TransferAssetDataModel.GetModel(currentPageID);

                // The Name of the Upload component is "files".
                if (UploadDoc != null)
                {

                    foreach (var file in UploadDoc)
                    {
                        DocumentModel document = new DocumentModel();
                        var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

                        // Some browsers send file names with full path.
                        // The sample is interested only in the file name.
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
                this.TraceLog("Asset Transfer DocumentUpload", $"{SessionUser.Current.Username} - DocumentUpload call Done");
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
            this.TraceLog("Asset Transfer DocumentRemove", $"{SessionUser.Current.Username} - DocumentRemove call");
            // The parameter of the Remove action must be called "fileNames".
            TransferAssetDataModel model = TransferAssetDataModel.GetModel(currentPageID);
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
                                //query.ModelRemoved = true;



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
            this.TraceLog("Asset Transfer DocumentRemove", $"{SessionUser.Current.Username} - DocumentRemove call Done");
            // Return an empty string to signify success.
            return Content("");
        }
        public FileResult DownloadFile(string fileName)
        {
            this.TraceLog("Asset Transfer DownloadFile", $"{SessionUser.Current.Username} - DownloadFile call");
            //Build the File Path.
            string path = Path.Combine(WebHostEnvironment.WebRootPath, "ExcelTemplate/") + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            this.TraceLog("Asset Transfer DownloadFile", $"{SessionUser.Current.Username} - DownloadFile call Done");
            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }
        public FileResult DownloadAttachedFile(string fileName)
        {
            this.TraceLog("Asset Transfer DownloadAttachedFile", $"{SessionUser.Current.Username} - DownloadAttachedFile call");
            //Build the File Path.
            string path = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath\\") + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            this.TraceLog("Asset Transfer DownloadAttachedFile", $"{SessionUser.Current.Username} - DownloadAttachedFile call Done");
            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }
        [HttpPost]
        public async Task<ActionResult> ImportUpload(IFormFile fileNames, int currentPageID)
        {
            try
            {
                this.TraceLog("Asset Transfer ImportUpload", $"{SessionUser.Current.Username} - ImportUpload call");
                var filePath = await ReadDocument(fileNames);
                string filePaths = filePath.Item1;
                DataTable dt = filePath.Item2;

                TransferAssetDataModel model = TransferAssetDataModel.GetModel(currentPageID);
                List<string> barcodesList = new List<string>();
                List<string> MissingbarcodesList = new List<string>();
                List<string> AvaliablebarcodesList = new List<string>();
                List<string> ActivebarcodesList = new List<string>();
                List<string> UnMappedbarcodesList = new List<string>();
                List<string> SameLocMapping = new List<string>();
                //if (model.LineItems.Count() > 0)
                //{
                //    var validation1 = model.LineItems.Select(a => a.Asset.AssetID);
                //    var loc = AssetNewView.GetAllItems(_db).Where(a => validation1.Contains(a.AssetID)).Select(a => a.LocationType).Distinct().ToList();
                //    if (loc.Count() > 1)
                //    {
                //        return base.ErrorActionResult("Same Type of Location Type should be choosen");
                //    }

                //}
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
                foreach (DataRow row in dt.Rows)
                {
                    //if (checkCnt == 0)
                    //{
                    //    checkCnt++;
                    //    continue;
                    //}
                    TransferAssetData item = new TransferAssetData();
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
                                                var list = views.Where(a => ids.Contains(a.AssetID)).Select(a => a.LocationType).Distinct().ToList();
                                                var res = AssetNewView.GetItem(_db, asset.AssetID);
                                                if (list.Contains(res.LocationType))
                                                {
                                                    item.Asset = asset;
                                                    item.Asset.Attribute40 = "Import Asset Transfer";
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
                                                item.Asset.Attribute40 = "Import Asset Transfer";
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

                foreach (var bar in AvaliablebarcodesList)
                {
                    validation = validation + bar + " Excel given barcode not available in db  .";
                }
                foreach (var bar in UnMappedbarcodesList)
                {
                    validation = validation + bar + " Excel given barcode not matched with configured user Mapping.";
                }
                foreach (var bar in SameLocMapping)
                {
                    validation = validation + bar + " Excel given barcode different Locationtype.";
                }
                foreach (var bar in barcodesList)
                {
                    validation = validation + bar + " Excel given barcode Location Type is Empty in db.";
                }
                foreach (var bar in MissingbarcodesList)
                {
                    validation = validation + bar + " Excel given barcode Location not Mapped in db. ";
                }
                foreach (var bar in ActivebarcodesList)
                {
                    validation = validation + bar + " Excel given barcode Not in Active Status. ";
                }
                if (validation == "")
                {
                    this.TraceLog("Asset Transfer ImportUpload", $"{SessionUser.Current.Username} - ImportUpload call Done");
                    return Json(new { Result = "success", Type = "Upload", FilePath = filePaths, error = "" });
                }

                else
                {

                    TransferAssetDataModel.RemoveModel(currentPageID);
                    // Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    //// string err = "given Location code not matched with transfer Location Type please check it";
                    // return Content(validation);
                    this.TraceLog("Asset Transfer ImportUpload", $"{SessionUser.Current.Username} - ImportUpload call Done on Error");
                    return Json(new { Result = "false", Type = "Upload", FilePath = "", error = validation });
                }
            }
            catch (Exception ex)
            {
                //AppErrorLogTable.SaveException(WasNowContext.CreateNewContext(), ex);
                ErrorActionResult(ex);
                return Json(new { Result = "false", Type = "Upload", FilePath = "", error = ex.Message });

            }
        }
        public async Task<Tuple<string, DataTable>> ReadDocument(IFormFile files)
        {
            this.TraceLog("Asset Transfer ReadDocument", $"{SessionUser.Current.Username} - ReadDocument call");
            string path = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/TransferAssetImport");
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
            this.TraceLog("Asset Transfer ReadDocument", $"{SessionUser.Current.Username} - ReadDocument call Done");
            return obj;

        }
        public ActionResult ImportRemove(string[] fileNames, int currentPageID)
        {
            this.TraceLog("Asset Transfer ImportRemove", $"{SessionUser.Current.Username} - ImportRemove call");
            // The parameter of the Remove action must be called "fileNames".
            // UniformRequestLineItemDetailsModel.ClearCurrentModel(currentPageID);
            TransferAssetDataModel model = TransferAssetDataModel.GetModel(currentPageID);
            if (fileNames != null)
            {
                if (model.LineItems.Count > 0)
                {
                    TransferAssetDataModel.RemoveModel(currentPageID);
                }
            }
            this.TraceLog("Asset Transfer ImportRemove", $"{SessionUser.Current.Username} - ImportRemove call Done");
            // Return an empty string to signify success.
            return Json(new { Result = "success", Type = "Remove", FilePath = "" });
        }
        public IActionResult _deleteItem(int id, int currentPageID)
        {
            this.TraceLog("Asset Transfer _deleteItem", $"{SessionUser.Current.Username} - _deleteItem call.id- {id}");
            TransferAssetDataModel model = TransferAssetDataModel.GetModel(currentPageID);

            var query = model.LineItems.Where(b => b.Asset.AssetID == id).FirstOrDefault();
            if (query != null)
                model.LineItems.Remove(query);

            this.TraceLog("Asset Transfer _deleteItem", $"{SessionUser.Current.Username} - _deleteItem call Done");
            return Content("");
        }
        //}
        [HttpPost]
        public IActionResult DraftMode(IFormCollection data)
        {
            if (!base.HasRights(RightNames.AssetTransfer, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");
            try
            {
                this.TraceLog("Asset Transfer DraftMode", $"{SessionUser.Current.Username} - DraftMode call");
                if (!string.IsNullOrEmpty(data["AssetIDS"]))
                {
                    string assetID = (string)data["AssetIDS"];
                    string[] arryAssetID = assetID.Split(',');
                    int[] intIDS = Array.ConvertAll(arryAssetID, s => int.Parse(s));
                    var list = AssetNewView.GetAllItems(_db).Where(a => intIDS.Contains(a.AssetID)).Select(a => a.LocationType).Distinct().FirstOrDefault();
                    var listID = AssetNewView.GetAllItems(_db).Where(a => intIDS.Contains(a.AssetID)).Select(a => a.LocationL2).Distinct().FirstOrDefault();
                    int fromID = LocationTable.GetLocationType(_db, list);

                    if (int.Parse(listID) == int.Parse(data["LocationID"] + ""))
                    {
                        return base.ErrorActionResult("From and To Location are the Same , Please select different Location");
                    }
                    var loclist = LocationTable.GetItem(_db, int.Parse(data["LocationID"] + "")).LocationTypeID;
                    var approvalworkflow = (from b in _db.ApproveWorkflowTable
                                            where b.FromLocationTypeID == fromID && b.ToLocationTypeID == loclist
                                                && b.ApproveModuleID == (int)ApproveModuleValue.AssetTransfer
                                            select b).FirstOrDefault();
                    if (approvalworkflow != null)
                    {
                        int currentPageID = int.Parse(data["CurrentPageID"] + "");
                        TransactionTable tran = new TransactionTable();
                        tran.CreatedBy = SessionUser.Current.UserID;
                        tran.CreatedFrom = "Web";
                        tran.CreatedDateTime = System.DateTime.Now;
                        tran.TransactionTypeID = (int)TransactionTypeValue.AssetTransfer;
                        tran.TransactionNo = CodeGenerationHelper.GetNextCode("AssetTransfer");
                        tran.Remarks = data["Remarks"];
                        tran.StatusID = (int)StatusValue.Draft;
                        tran.PostingStatusID = (int)PostingStatusValue.WorkInProgress;
                        tran.TransactionDate = System.DateTime.Now;
                        _db.Add(tran);


                        TransferAssetDataModel model = TransferAssetDataModel.GetModel(currentPageID);
                        foreach (var asetIDs in intIDS)
                        {
                            var checkAssets = AssetTable.GetItem(_db, asetIDs);
                            TransactionLineItemTable lineitem = new TransactionLineItemTable();
                            lineitem.AssetID = asetIDs;
                            lineitem.Transaction = tran;
                            lineitem.CreatedBy = SessionUser.Current.UserID;
                            lineitem.CreatedDateTime = System.DateTime.Now;
                            lineitem.FromLocationID = checkAssets.LocationID;
                            lineitem.ToLocationID = int.Parse(data["LocationID"] + "");
                            lineitem.StatusID = (int)StatusValue.WaitingForApproval;
                            _db.Add(lineitem);

                            AssetTable oldasset = AssetTable.GetItem(_db, asetIDs);
                            oldasset.StatusID = (int)StatusValue.WaitingForApproval;

                        }

                        _db.SaveChanges();
                        _db.Entry(tran).Reload();
                        // ApproveWorkflowLineItemTable.ApproveHistoryMaintanance(_db, fromID, tran.TransactionID, (int)ApproveModuleValue.AssetTransfer, SessionUser.Current.UserID, int.Parse(listID), int.Parse(data["LocationID"] + ""), (int)loclist);
                        if (model.Documents.Count > 0)
                        {
                            foreach (var item in model.Documents.Distinct().ToList())
                            {
                                DocumentTable document = new DocumentTable();
                                document.FileName = item.FileName;
                                document.DocumentName = item.DocumentName;
                                document.FilePath = item.FullPath;
                                document.FileExtension = item.FileExtension;
                                document.TransactionType = "AssetTransfer";
                                document.ObjectKeyID = tran.TransactionID;
                                _db.Add(document);
                            }
                            _db.SaveChanges();
                        }
                        TransferAssetDataModel.RemoveModel(currentPageID);

                        base.TraceLog("Asset Transfer Draft Post", $"{SessionUser.Current.Username} -Asset Transfer  details saved to db ");
                        //return RedirectToAction("SuccessAction");
                        return Json(new { Result = "Success", message = " Asset Transfer Draft Saved Successfully" });
                    }
                    else
                    {
                        this.TraceLog("Asset Transfer DraftMode", $"{SessionUser.Current.Username} - DraftMode Error:Selected Location Type not created workflow . please create the workflow");
                        return Json(new { Result = "Error", message = "Selected Location Type not created workflow . please create the workflow" });
                    }


                }
                else
                {
                    this.TraceLog("Asset Transfer DraftMode", $"{SessionUser.Current.Username} - DraftMode Error:Select any asset to transfers");
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
        // VaildateDoc
        [HttpPost]
        public IActionResult Edit(IFormCollection data)
        {
            if (!base.HasRights(RightNames.AssetTransfer, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");
            try
            {
                if (!string.IsNullOrEmpty(data["AssetIDS"]))
                {
                    base.TraceLog("Asset Transfer Edit Post", $"{SessionUser.Current.Username} -Asset Transfer  details will save to db ");
                    string assetID = (string)data["AssetIDS"];
                    string[] arryAssetID = assetID.Split(',');
                    int[] intIDS = Array.ConvertAll(arryAssetID, s => int.Parse(s));
                    int toLocationID = int.Parse(data["LocationID"] + "");
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

                    //if (int.Parse(listID) == int.Parse(data["LocationID"] + ""))
                    //{
                    //    return base.ErrorActionResult("From and To Location are the Same , Please select different Location");
                    //}
                    //var loclist = LocationTable.GetItem(_db, int.Parse(data["LocationID"] + "")).LocationTypeID;
                    //var approvalworkflow = (from b in _db.ApproveWorkflowTable
                    //                        where b.FromLocationTypeID == fromID && b.ToLocationTypeID == loclist
                    //                            && b.ApproveModuleID == (int)ApproveModuleValue.AssetTransfer
                    //                        select b).FirstOrDefault();
                    //int validation = 0;

                    ////string errorMsg = string.Empty;
                    //if (approvalworkflow != null)
                    //{
                    //    //var validations = TransactionTable.GetValidationresult(_db, assetID, int.Parse(data["LocationID"] + ""), fromID, (int)loclist, approvalworkflow.ApproveWorkflowID);
                    //    //validation = validations.Result.result;
                    //    //if (validation == 0)
                    //    //{
                    //    //    return base.ErrorActionResult("Category type of the selected assets are not matched with mapped role ");
                    //    //}
                    //    //else
                    //    //{

                    var allvalid = TransactionTable.GetTransactionResult(_db, assetID, int.Parse(data["LocationID"] + ""), (int)ApproveModuleValue.AssetTransfer, RightNames.AssetTransfer);
                    if (!string.IsNullOrEmpty(allvalid.Result))
                    {
                        return base.ErrorActionResult(allvalid.Result);
                    }
                    //    //}


                    //}

                    //if (approvalworkflow != null)
                    //{
                    int currentPageID = int.Parse(data["CurrentPageID"] + "");
                    int transaction = int.Parse(data["TransactionID"] + "");
                    TransactionTable tran = TransactionTable.GetItem(_db, transaction);
                    tran.StatusID = (int)StatusValue.WaitingForApproval;



                    TransferAssetDataModel model = TransferAssetDataModel.GetModel(currentPageID);
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
                            lineitem.ToLocationID = int.Parse(data["LocationID"] + "");
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

                    Tuple<int?, int?, int?, int?> historyData = TransactionTable.GetApprovalHistoryList(_db, intIDS.ToList(), toLocationID);
                    ApproveWorkflowLineItemTable.ApproveHistoryMaintanance(_db, (int)historyData.Item2, tran.TransactionID, (int)ApproveModuleValue.AssetTransfer, SessionUser.Current.UserID, (int)historyData.Item1, (int)historyData.Item3, (int)historyData.Item4);
                    if (model.Documents.Count > 0)
                    {
                        DocumentTable.deleteDocument(_db, transaction, "AssetTransfer");
                        foreach (var item in model.Documents.Distinct().ToList())
                        {
                            DocumentTable document = new DocumentTable();
                            document.FileName = item.FileName;
                            document.DocumentName = item.DocumentName;
                            document.FilePath = item.FullPath;
                            document.FileExtension = item.FileExtension;
                            document.TransactionType = "AssetTransfer";
                            document.ObjectKeyID = tran.TransactionID;
                            _db.Add(document);
                        }
                        _db.SaveChanges();
                    }
                    TransferAssetDataModel.RemoveModel(currentPageID);

                    base.TraceLog("Asset Transfer Edit Post", $"{SessionUser.Current.Username} -Asset Transfer  details saved to db ");
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
            if (!base.HasRights(RightNames.AssetTransfer, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");
            try
            {
                if (!string.IsNullOrEmpty(data["AssetIDS"]))
                {
                    string assetID = (string)data["AssetIDS"];
                    string[] arryAssetID = assetID.Split(',');
                    int[] intIDS = Array.ConvertAll(arryAssetID, s => int.Parse(s));
                    var list = AssetNewView.GetAllItems(_db).Where(a => intIDS.Contains(a.AssetID)).Select(a => a.LocationType).Distinct().FirstOrDefault();
                    var listID = AssetNewView.GetAllItems(_db).Where(a => intIDS.Contains(a.AssetID)).Select(a => a.LocationL2).Distinct().FirstOrDefault();
                    int fromID = LocationTable.GetLocationType(_db, list);

                    if (int.Parse(listID) == int.Parse(data["LocationID"] + ""))
                    {
                        return base.ErrorActionResult("From and To Location are the Same , Please select different Location");
                    }
                    var loclist = LocationTable.GetItem(_db, int.Parse(data["LocationID"] + "")).LocationTypeID;
                    var approvalworkflow = (from b in _db.ApproveWorkflowTable
                                            where b.FromLocationTypeID == fromID && b.ToLocationTypeID == loclist
                                                && b.ApproveModuleID == (int)ApproveModuleValue.AssetTransfer
                                            select b).FirstOrDefault();
                    if (approvalworkflow != null)
                    {
                        int currentPageID = int.Parse(data["CurrentPageID"] + "");
                        int transaction = int.Parse(data["TransactionID"] + "");
                        int locationID = int.Parse(data["LocationID"] + "");
                        TransactionTable tran = TransactionTable.GetItem(_db, transaction);
                        tran.StatusID = (int)StatusValue.Draft;

                        TransferAssetDataModel model = TransferAssetDataModel.GetModel(currentPageID);

                        foreach (var asetIDs in intIDS)
                        {
                            var oldlineitem = TransactionLineItemTable.LineItems(_db, transaction, asetIDs);
                            if (oldlineitem == null)
                            {
                                var checkAssets = AssetTable.GetItem(_db, asetIDs);
                                TransactionLineItemTable lineitem = new TransactionLineItemTable();
                                lineitem.AssetID = asetIDs;
                                lineitem.TransactionID = tran.TransactionID;
                                lineitem.CreatedBy = SessionUser.Current.UserID;
                                lineitem.CreatedDateTime = System.DateTime.Now;
                                lineitem.FromLocationID = checkAssets.LocationID;
                                lineitem.ToLocationID = int.Parse(data["LocationID"] + "");
                                lineitem.StatusID = (int)StatusValue.WaitingForApproval;
                                _db.Add(lineitem);

                                AssetTable oldasset = AssetTable.GetItem(_db, asetIDs);
                                oldasset.StatusID = (int)StatusValue.WaitingForApproval;
                            }
                        }

                        _db.SaveChanges();
                        _db.Entry(tran).Reload();
                        // ApproveWorkflowLineItemTable.ApproveHistoryMaintanance(_db, fromID, tran.TransactionID, (int)ApproveModuleValue.AssetTransfer, SessionUser.Current.UserID, int.Parse(listID), int.Parse(data["LocationID"] + ""), (int)loclist);
                        if (model.Documents.Count > 0)
                        {
                            DocumentTable.deleteDocument(_db, transaction, "AssetTransfer");
                            foreach (var item in model.Documents.Distinct().ToList())
                            {
                                DocumentTable document = new DocumentTable();
                                document.FileName = item.FileName;
                                document.DocumentName = item.DocumentName;
                                document.FilePath = item.FullPath;
                                document.FileExtension = item.FileExtension;
                                document.TransactionType = "AssetTransfer";
                                document.ObjectKeyID = tran.TransactionID;
                                _db.Add(document);
                            }
                            _db.SaveChanges();
                        }
                        TransferAssetDataModel.RemoveModel(currentPageID);

                        base.TraceLog("Asset Transfer Edit Draft Post", $"{SessionUser.Current.Username} -Asset Transfer  details saved to db ");
                        return Json(new { Result = "Success", message = " Asset Transfer Draft Updated Successfully" });
                    }
                    else
                    {
                        base.TraceLog("Asset Transfer Edit Draft Post", $"{SessionUser.Current.Username} -On Error ");
                        return Json(new { Result = "Error", message = "Selected Location Type not created workflow . please create the workflow" });
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