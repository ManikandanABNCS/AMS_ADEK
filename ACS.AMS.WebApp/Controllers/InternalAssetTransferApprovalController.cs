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
using ACS.AMS.WebApp.Models.SystemModels;
using System.Net.Http.Headers;
using ACS.AMS.WebApp.Classes;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using Microsoft.AspNetCore.Authorization;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace ACS.AMS.WebApp.Controllers
{
    public class InternalAssetTransferApprovalController : ACSBaseController
    {
        private static IWebHostEnvironment WebHostEnvironment;


        public InternalAssetTransferApprovalController(IWebHostEnvironment _environment)
        {
            WebHostEnvironment = _environment;
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (!base.HasRights(RightNames.InternalAssetTransferApproval, UserRightValue.Create))
                return GotoUnauthorizedPage();

            base.TraceLog("InternalAssetTransferApproval Index", $"{SessionUser.Current.Username} -InternalAssetTransferApproval Index page requested");
            return PartialView();
        }

        public IActionResult _Index([DataSourceRequest] DataSourceRequest request)
        {
            if (!base.HasRights(RightNames.InternalAssetTransferApproval, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");

            List<int> getUserApprovalList = PersonTable.GetUserRoleMappingDetails(_db, SessionUser.Current.UserID).Select(a => (int)a.ApprovalRoleID).ToList();
            IQueryable<AssetTransferApprovalView> result = AssetTransferApprovalView.GetAllItems(_db).Where(a => a.UserID == SessionUser.Current.UserID && a.SerialNo == 1 
                                        && a.ApprovalStatusID == (int)StatusValue.WaitingForApproval && a.ApproveModuleID==(int)ApproveModuleValue.InternalAssetTransfer);
            var dsResult = result.ToDataSourceResult(request);
            base.TraceLog("InternalAssetTransferApproval Index", $"{SessionUser.Current.Username} -InternalAssetTransferApproval Index page Data Fetch");
            return Json(dsResult);
          
        }
        public ActionResult Details(int id, string pageName)
        {
            if (!base.HasRights(RightNames.InternalAssetTransferApproval, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("InternalAssetTransferApproval Details", $"{SessionUser.Current.Username} -InternalAssetTransferApproval Detail requested.id-{id}");
            int currentPageId = SessionUser.Current.GetNextPageID();
            ViewBag.CurrentPageID = SessionUser.Current.GetNextPageID();
            ViewBag.ApprovalHistoryID = id;
            return PartialView();

        }
        public ActionResult Edit(int id, string pageName)
        {
            if (!base.HasRights(RightNames.InternalAssetTransferApproval, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("InternalAssetTransferApproval Edit", $"{SessionUser.Current.Username} -InternalAssetTransferApproval Edit requested.id-{id}");
            IndexPageModel indexPage = new IndexPageModel();
            indexPage.EntityInstanceName = new BaseEntityObject();
            indexPage.EntityInstanceName.CurrentPageID = SessionUser.Current.GetNextPageID();
            int currentPageId = indexPage.EntityInstanceName.CurrentPageID;
            ViewBag.CurrentPageID = indexPage.EntityInstanceName.CurrentPageID;
            ViewBag.ApprovalHistoryID = id;
            return PartialView(indexPage);

        }
        
       
        public IActionResult _LineItemindex([DataSourceRequest] DataSourceRequest request, int id)
        {
            base.TraceLog("InternalAssetTransferApproval _LineItemindex", $"{SessionUser.Current.Username}  _LineItemindex requested.id-{id}");
            // var query = uniformRequestDetailsDataModel;
            var transID = ApprovalHistoryTable.GetItem(_db, id);
            var query = TransactionLineItemViewForTransfer.GetAllItems(_db).Where(a => a.TransactionID == transID.TransactionID);

            var dsResult = query.ToDataSourceResult(request);

            // var dsResult = request.ToDataSourceResult(query.AsQueryable(), "UniformRequestUniformItemEntry", "UniformRequestLineItemID");
           // this.TraceLog("Index", $"{SessionUser.Current.Username} - Index page Data Fetch");

            return Json(dsResult);
        }
        //_LineItemApproval

        public IActionResult _LineItemApproval([DataSourceRequest] DataSourceRequest request, int id)
        {
            base.TraceLog("InternalAssetTransferApproval _LineItemApproval", $"{SessionUser.Current.Username} -_LineItemApproval _LineItemindex requested.id-{id}");
            // var query = uniformRequestDetailsDataModel;
            var transID = ApprovalHistoryTable.GetItem(_db, id);
            var query = ApprovalHistoryView.GetAllItems(_db,true).Where(a => a.TransactionID == transID.TransactionID && a.ApproveModuleID == transID.ApproveModuleID).OrderBy(a => a.CreatedDateTime).ThenBy(a => a.OrderNo);

            var dsResult = query.ToDataSourceResult(request);

            // var dsResult = request.ToDataSourceResult(query.AsQueryable(), "UniformRequestUniformItemEntry", "UniformRequestLineItemID");
           // this.TraceLog("Index", $"{SessionUser.Current.Username} - Index page Data Fetch");

            return Json(dsResult);
        }

        public async Task<ActionResult> DocumentUpload(IEnumerable<IFormFile> UploadDoc, int currentPageID, int id)
        {
            try
            {
                base.TraceLog("InternalAssetTransferApproval DocumentUpload", $"{SessionUser.Current.Username} -DocumentUpload  requested.id-{id}");
                //string absolutePath = DateTime.Now.ToString("yyyy_MM_dd");
                string rootPath = string.Concat(WebHostEnvironment.WebRootPath, "\\");

                string fullPath = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/TransferAssetApproval");
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                //DocumentDataModel.RemoveModel(currentPageID);
                //DocumentDataModel detailsDocuments = DocumentDataModel.GetModel(currentPageID);
                TransferAssetDataModel model = TransferAssetDataModel.GetModel(currentPageID);

                // The Name of the Upload component is "files".
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
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(fileContent.FileName.ToString().Trim('"'));
                        //  var fileName = Path.GetFileName(fileContent.FileName.ToString().Trim('"'));


                        //var newFileName = fileName + fileExtension; //Guid.NewGuid().ToString() + fileExtension;
                        string newFileName = fileName + "" + time + "" + fileExtension;
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
                base.TraceLog("InternalAssetTransferApproval DocumentUpload", $"{SessionUser.Current.Username} -DocumentUpload  requested done.id-{id}");
                // Return an empty string to signify success.
                return Content("");
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }

        }

        public ActionResult DocumentRemove(string[] fileNames, int currentPageID, int id)
        {
            base.TraceLog("InternalAssetTransferApproval DocumentRemove", $"{SessionUser.Current.Username} -DocumentRemove  requested.id-{id}");
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
            base.TraceLog("InternalAssetTransferApproval DocumentRemove", $"{SessionUser.Current.Username} -DocumentRemove  requested done.id-{id}");
            // Return an empty string to signify success.
            return Content("");
        }
        [HttpPost]
        public IActionResult SingleApproval(IFormCollection data, string type, string remarks, int currentPageID)
        {
            try
            {
  
                string historyID = data["id"];

                ApprovalHistoryTable wItem = ApprovalHistoryTable.GetWorkflowID(_db, int.Parse(historyID), (int)ApproveModuleValue.InternalAssetTransfer);
                TransferAssetDataModel model = TransferAssetDataModel.GetModel(currentPageID);
                if (model.Documents.Count > 0)
                {
                    foreach (var item in model.Documents)
                    {
                        DocumentTable document = new DocumentTable();
                        document.FileName = item.FileName;
                        document.DocumentName = item.DocumentName;
                        document.FilePath = item.FullPath;
                        document.FileExtension = item.FileExtension;
                        document.TransactionType = "AssetTransferApproval";
                        document.ObjectKeyID = wItem.ApprovalHistoryID;
                        _db.Add(document);
                    }

                }

                if (string.Compare(type, "Approval") == 0)
                {
                    base.TraceLog("InternalAssetTransferApproval SingleApproval", $"{SessionUser.Current.Username} -SingleApproval approve request");
                    var maxworkflowID = ApprovalHistoryTable.GetWorkflowDetailsworkflowMax(_db, int.Parse(historyID));
                    var max = ApprovalHistoryTable.GetItem(_db, maxworkflowID);
                    var previousID = ApprovalHistoryTable.GetWorkflowDetailsworkflowPrevious(_db, int.Parse(historyID));
                    //List<int> getUserApprovalList = PersonTable.GetUserRoleMappingDetails(_db, SessionUser.Current.UserID).Select(a => (int)a.ApprovalRoleID).ToList();
                    var results = AssetTransferApprovalView.GetAllItems(_db).Where(a => a.UserID == SessionUser.Current.UserID && a.ApprovalHistoryID== int.Parse(historyID)).FirstOrDefault();
                    if (results.ApprovalRoleID==max.ApprovalRoleID)
                    {
                        wItem.StatusID = (int)StatusValue.Active;
                        wItem.LastModifiedDateTime = System.DateTime.Now;
                        wItem.LastModifiedBy = SessionUser.Current.UserID;
                        wItem.Remarks = remarks;
                        TransactionTable tramsType = TransactionTable.GetItem(_db, wItem.TransactionID);
                        tramsType.StatusID = (int)StatusValue.Active;
                        tramsType.PostingStatusID = (int)PostingStatusValue.CompletedByEndUser;
                        tramsType.VerifiedBy = SessionUser.Current.UserID;
                        tramsType.VerifiedDateTime = System.DateTime.Now;
                        tramsType.Remarks = remarks;

                        var lineitem = TransactionLineItemTable.GetTransactionLineItems(_db, wItem.TransactionID);
                        foreach(var line in lineitem.ToList())
                        {
                            TransactionLineItemTable oldline = TransactionLineItemTable.GetItem(_db, line.TransactionLineItemID);
                            oldline.StatusID = (int)StatusValue.Active;

                            AssetTable OldAsset = AssetTable.GetItem(_db, oldline.AssetID);
                            OldAsset.LocationID = oldline.RoomID;
                            OldAsset.StatusID = (int)StatusValue.Active;

                        }
                    }
                    else
                    {
                        if (previousID.ApprovalHistoryID == wItem.ApprovalHistoryID)
                        {
                            TransactionTable tramsType = TransactionTable.GetItem(_db, wItem.TransactionID);
                            tramsType.PostingStatusID = (int)PostingStatusValue.WaitingForFinalApproval;
                            tramsType.PostedBy = SessionUser.Current.UserID;
                            tramsType.PostedDateTime = System.DateTime.Now;
                        }
                        wItem.StatusID = (int)StatusValue.Active;
                        wItem.LastModifiedDateTime = System.DateTime.Now;
                        wItem.LastModifiedBy = SessionUser.Current.UserID;
                        wItem.Remarks = remarks;
                    }

                }
                else if (string.Compare(type, "Reject") == 0)
                {
                    base.TraceLog("InternalAssetTransferApproval SingleApproval", $"{SessionUser.Current.Username} -SingleApproval approve request");
                    wItem.StatusID = (int)StatusValue.Rejected;
                    wItem.LastModifiedDateTime = System.DateTime.Now;
                    wItem.LastModifiedBy = SessionUser.Current.UserID;
                    wItem.Remarks = remarks;

                    var remainingUsersameLevel = ApprovalHistoryTable.SameLevelUserList(_db, wItem.ApprovalHistoryID, (int)ApproveModuleValue.InternalAssetTransfer, wItem.TransactionID);
                    foreach (var remaining in remainingUsersameLevel.ToList())
                    {
                        ApprovalHistoryTable OldHistory = ApprovalHistoryTable.GetItem(_db, remaining.ApprovalHistoryID);
                        OldHistory.StatusID = (int)StatusValue.Inactive;
                        OldHistory.LastModifiedBy = SessionUser.Current.UserID;
                        OldHistory.LastModifiedDateTime = System.DateTime.Now;

                    }
                    TransactionTable tramsType = TransactionTable.GetItem(_db, wItem.TransactionID);
                    tramsType.StatusID = (int)StatusValue.Rejected;
                    tramsType.PostingStatusID = (int)PostingStatusValue.CompletedByEndUser;
                    tramsType.VerifiedBy = SessionUser.Current.UserID;
                    tramsType.VerifiedDateTime = System.DateTime.Now;
                    tramsType.Remarks = remarks;

                    var lineitem = TransactionLineItemTable.GetTransactionLineItems(_db, wItem.TransactionID);
                    foreach (var line in lineitem.ToList())
                    {
                        TransactionLineItemTable oldline = TransactionLineItemTable.GetItem(_db, line.TransactionLineItemID);
                        oldline.StatusID = (int)StatusValue.Rejected;

                        AssetTable OldAsset = AssetTable.GetItem(_db, oldline.AssetID);
                        OldAsset.StatusID = (int)StatusValue.Active;

                    }
                    _db.SaveChanges();
                    TransferAssetDataModel.RemoveModel(currentPageID);
                    base.TraceLog("InternalAssetTransferApproval SingleApproval", $"{SessionUser.Current.Username} -SingleApproval reject request done");
                    return Json(new { Result = "Success", message = " Rejected Successfully" });
                }
                _db.SaveChanges();
                TransferAssetDataModel.RemoveModel(currentPageID);
                base.TraceLog("InternalAssetTransferApproval SingleApproval", $"{SessionUser.Current.Username} -SingleApproval approve request done");
                return Json(new { Result = "Success", message = " Approved Successfully" });
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }

        [HttpPost]
        public IActionResult MultipleApproval(IFormCollection data, string type, string remarks)
        {
            try
            {
                string checkdItems = data["docID[]"];
                string[] arrRequestID = checkdItems.Split(',');
                int[] requestID = Array.ConvertAll(arrRequestID, s => int.Parse(s));
                int requestCount = requestID.Count();

                foreach (int historyID in requestID)
                {
                    ApprovalHistoryTable wItem = ApprovalHistoryTable.GetWorkflowID(_db, historyID, (int)ApproveModuleValue.InternalAssetTransfer);
                    if (string.Compare(type, "Approval") == 0)
                    {
                        base.TraceLog("InternalAssetTransferApproval MultipleApproval", $"{SessionUser.Current.Username} -SingleApproval approve request");
                        var maxworkflowID = ApprovalHistoryTable.GetWorkflowDetailsworkflowMax(_db, historyID);
                        var max = ApprovalHistoryTable.GetItem(_db, maxworkflowID);
                        var previousID = ApprovalHistoryTable.GetWorkflowDetailsworkflowPrevious(_db, historyID);
                        //List<int> getUserApprovalList = PersonTable.GetUserRoleMappingDetails(_db, SessionUser.Current.UserID).Select(a => (int)a.ApprovalRoleID).ToList();
                        //if (getUserApprovalList.Contains(max.ApprovalRoleID))
                        //{
                        var results = AssetTransferApprovalView.GetAllItems(_db).Where(a => a.UserID == SessionUser.Current.UserID && a.ApprovalHistoryID == historyID).FirstOrDefault();
                        if (results.ApprovalRoleID == max.ApprovalRoleID)
                        {
                            wItem.StatusID = (int)StatusValue.Active;
                            wItem.LastModifiedDateTime = System.DateTime.Now;
                            wItem.LastModifiedBy = SessionUser.Current.UserID;
                            wItem.Remarks = remarks;
                            TransactionTable tramsType = TransactionTable.GetItem(_db, wItem.TransactionID);
                            tramsType.StatusID = (int)StatusValue.Active;
                            tramsType.PostingStatusID = (int)PostingStatusValue.CompletedByEndUser;
                            tramsType.VerifiedBy = SessionUser.Current.UserID;
                            tramsType.VerifiedDateTime = System.DateTime.Now;
                            tramsType.Remarks = remarks;
                            var lineitem = TransactionLineItemTable.GetTransactionLineItems(_db, wItem.TransactionID);
                            foreach (var line in lineitem.ToList())
                            {
                                TransactionLineItemTable oldline = TransactionLineItemTable.GetItem(_db, line.TransactionLineItemID);
                                oldline.StatusID = (int)StatusValue.Active;

                                AssetTable OldAsset = AssetTable.GetItem(_db, oldline.AssetID);
                                OldAsset.LocationID = oldline.ToLocationID;
                                OldAsset.StatusID = (int)StatusValue.Active;

                            }
                        }
                        else
                        {
                            if (previousID.ApprovalHistoryID == wItem.ApprovalHistoryID)
                            {
                                TransactionTable tramsType = TransactionTable.GetItem(_db, wItem.TransactionID);
                                tramsType.PostingStatusID = (int)PostingStatusValue.WaitingForFinalApproval;
                                tramsType.PostedBy = SessionUser.Current.UserID;
                                tramsType.PostedDateTime = System.DateTime.Now;
                            }
                            wItem.StatusID = (int)StatusValue.Active;
                            wItem.LastModifiedDateTime = System.DateTime.Now;
                            wItem.LastModifiedBy = SessionUser.Current.UserID;
                            wItem.Remarks = remarks;
                        }


                    }
                    else if (string.Compare(type, "Reject") == 0)
                    {
                        base.TraceLog("InternalAssetTransferApproval MultipleApproval", $"{SessionUser.Current.Username} -SingleApproval reject request");
                        wItem.StatusID = (int)StatusValue.Rejected;
                        wItem.LastModifiedDateTime = System.DateTime.Now;
                        wItem.LastModifiedBy = SessionUser.Current.UserID;
                        wItem.Remarks = remarks;

                        var remainingUsersameLevel = ApprovalHistoryTable.SameLevelUserList(_db, wItem.ApprovalHistoryID, (int)ApproveModuleValue.InternalAssetTransfer, wItem.TransactionID);
                        foreach (var remaining in remainingUsersameLevel.ToList())
                        {
                            ApprovalHistoryTable OldHistory = ApprovalHistoryTable.GetItem(_db, remaining.ApprovalHistoryID);
                            OldHistory.StatusID = (int)StatusValue.Inactive;
                            OldHistory.LastModifiedBy = SessionUser.Current.UserID;
                            OldHistory.LastModifiedDateTime = System.DateTime.Now;

                        }
                        TransactionTable tramsType = TransactionTable.GetItem(_db, wItem.TransactionID);
                        tramsType.StatusID = (int)StatusValue.Rejected;
                        tramsType.PostingStatusID = (int)PostingStatusValue.CompletedByEndUser;
                        tramsType.VerifiedBy = SessionUser.Current.UserID;
                        tramsType.VerifiedDateTime = System.DateTime.Now;
                        tramsType.Remarks = remarks;

                        var lineitem = TransactionLineItemTable.GetTransactionLineItems(_db, wItem.TransactionID);
                        foreach (var line in lineitem.ToList())
                        {
                            TransactionLineItemTable oldline = TransactionLineItemTable.GetItem(_db, line.TransactionLineItemID);
                            oldline.StatusID = (int)StatusValue.Rejected;

                            AssetTable OldAsset = AssetTable.GetItem(_db, oldline.AssetID);
                            OldAsset.StatusID = (int)StatusValue.Active;

                        }
                        _db.SaveChanges();
                        base.TraceLog("InternalAssetTransferApproval MultipleApproval", $"{SessionUser.Current.Username} -SingleApproval reject request done");
                        return Json(new { Result = "Success", message = " Rejected Successfully" });
                    }

                }
                _db.SaveChanges();
                base.TraceLog("InternalAssetTransferApproval MultipleApproval", $"{SessionUser.Current.Username} -SingleApproval approve request done");
                return Json(new { Result = "Success", message = " Approved Successfully" });

            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }
        public FileResult DownloadFile(string fileName)
        {
            base.TraceLog("InternalAssetTransferApproval DownloadFile", $"{SessionUser.Current.Username} -DownloadFile request");
            //Build the File Path.
            string path = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/") + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }

        public ActionResult EmailEdit(int id, int userID)
        {
            base.TraceLog("InternalAssetTransferApproval EmailEdit", $"{SessionUser.Current.Username} -EmailEdit request. id-{id},{userID}");
            ViewBag.userID = userID;
            ViewBag.ApprovalHistoryID = id;
            ViewBag.currentPageID = SessionUser.Current.GetNextPageID();
            var getresult = (from b in _db.ApprovalHistoryTable where b.ApprovalHistoryID == id select b).FirstOrDefault();
            if (getresult.StatusID == (int)StatusValue.Active || getresult.StatusID == (int)StatusValue.Rejected)
            {
                ModelState.AddModelError("12", "The Approval Process was completed, Asset status check with the application");
                return PartialView("EmailDetails");
                //return base.ErrorActionResult("The Approval Process was completed, Asset status check with the application");
                //return Content(@"<body>
                //       <script type='text/javascript'>
                //            window.parent.alert('The Approval Process was completed, Asset status check with the application');

                //         window.close();
                //       </script>
                //     </body> ");
            }
            return PartialView();
        }
        public ActionResult EmailView(int id)
        {
            base.TraceLog("InternalAssetTransferApproval EmailView", $"{SessionUser.Current.Username} -EmailView request. id-{id}");
            ViewBag.userID = 1;
            ViewBag.ApprovalHistoryID = id;
            ViewBag.moduleID = (int)ApproveModuleValue.AssetRetirement;
            ViewBag.currentPageID = SessionUser.Current.GetNextPageID();
            var getresult = (from b in _db.ApprovalHistoryTable where b.ApprovalHistoryID == id select b).FirstOrDefault();
            if (getresult.StatusID == (int)StatusValue.Active || getresult.StatusID == (int)StatusValue.Rejected)
            {
                ModelState.AddModelError("12", "The Approval Process was completed, Asset status check with the application");
                return PartialView("EmailDetails");
                //return base.ErrorActionResult("The Approval Process was completed, Asset status check with the application");
                //return Content(@"<body>
                //       <script type='text/javascript'>
                //            window.parent.alert('The Approval Process was completed, Asset status check with the application');

                //         window.close();
                //       </script>
                //     </body> ");
            }
            return PartialView();
        }
        [HttpPost]
        public IActionResult EmailSingleApproval(IFormCollection data, string type, string remarks, int currentPageID, int userID)
        {
            try
            {
                string historyID = data["id"];

                ApprovalHistoryTable wItem = ApprovalHistoryTable.GetWorkflowID(_db, int.Parse(historyID), (int)ApproveModuleValue.InternalAssetTransfer);
                TransferAssetDataModel model = TransferAssetDataModel.GetModel(currentPageID);
                if (model.Documents.Count > 0)
                {
                    foreach (var item in model.Documents)
                    {
                        DocumentTable document = new DocumentTable();
                        document.FileName = item.FileName;
                        document.DocumentName = item.DocumentName;
                        document.FilePath = item.FullPath;
                        document.FileExtension = item.FileExtension;
                        document.TransactionType = "AssetTransferApproval";
                        document.ObjectKeyID = wItem.ApprovalHistoryID;
                        _db.Add(document);
                    }

                }

                if (string.Compare(type, "Approval") == 0)
                {
                    base.TraceLog("InternalAssetTransferApproval EmailSingleApproval", $"{SessionUser.Current.Username} -EmailView request approval. userID-{userID}");
                    var maxworkflowID = ApprovalHistoryTable.GetWorkflowDetailsworkflowMax(_db, int.Parse(historyID));
                    var max = ApprovalHistoryTable.GetItem(_db, maxworkflowID);
                    var previousID = ApprovalHistoryTable.GetWorkflowDetailsworkflowPrevious(_db, int.Parse(historyID));
                    //List<int> getUserApprovalList = PersonTable.GetUserRoleMappingDetails(_db, SessionUser.Current.UserID).Select(a => (int)a.ApprovalRoleID).ToList();
                    var results = AssetTransferApprovalView.GetAllItems(_db).Where(a => a.UserID == userID && a.ApprovalHistoryID == int.Parse(historyID)).FirstOrDefault();
                    if (results.ApprovalRoleID == max.ApprovalRoleID)
                    {
                        wItem.StatusID = (int)StatusValue.Active;
                        wItem.LastModifiedDateTime = System.DateTime.Now;
                        wItem.LastModifiedBy = userID;
                        wItem.Remarks = remarks;
                        TransactionTable tramsType = TransactionTable.GetItem(_db, wItem.TransactionID);
                        tramsType.StatusID = (int)StatusValue.Active;
                        tramsType.PostingStatusID = (int)PostingStatusValue.CompletedByEndUser;
                        tramsType.VerifiedBy = userID;
                        tramsType.VerifiedDateTime = System.DateTime.Now;
                        tramsType.Remarks = remarks;

                        var lineitem = TransactionLineItemTable.GetTransactionLineItems(_db, wItem.TransactionID);
                        foreach (var line in lineitem.ToList())
                        {
                            TransactionLineItemTable oldline = TransactionLineItemTable.GetItem(_db, line.TransactionLineItemID);
                            oldline.StatusID = (int)StatusValue.Active;

                            AssetTable OldAsset = AssetTable.GetItem(_db, oldline.AssetID);
                            OldAsset.LocationID = oldline.RoomID;
                            OldAsset.StatusID = (int)StatusValue.Active;

                        }
                    }
                    else
                    {
                        if (previousID.ApprovalHistoryID == wItem.ApprovalHistoryID)
                        {
                            TransactionTable tramsType = TransactionTable.GetItem(_db, wItem.TransactionID);
                            tramsType.PostingStatusID = (int)PostingStatusValue.WaitingForFinalApproval;
                            tramsType.PostedBy = userID;
                            tramsType.PostedDateTime = System.DateTime.Now;
                        }
                        wItem.StatusID = (int)StatusValue.Active;
                        wItem.LastModifiedDateTime = System.DateTime.Now;
                        wItem.LastModifiedBy = userID;
                        wItem.Remarks = remarks;
                    }

                }
                else if (string.Compare(type, "Reject") == 0)
                {
                    base.TraceLog("InternalAssetTransferApproval EmailSingleApproval", $"{SessionUser.Current.Username} -EmailView request reject. userID-{userID}");
                    wItem.StatusID = (int)StatusValue.Rejected;
                    wItem.LastModifiedDateTime = System.DateTime.Now;
                    wItem.LastModifiedBy = userID;
                    wItem.Remarks = remarks;

                    var remainingUsersameLevel = ApprovalHistoryTable.SameLevelUserList(_db, wItem.ApprovalHistoryID, (int)ApproveModuleValue.InternalAssetTransfer, wItem.TransactionID);
                    foreach (var remaining in remainingUsersameLevel.ToList())
                    {
                        ApprovalHistoryTable OldHistory = ApprovalHistoryTable.GetItem(_db, remaining.ApprovalHistoryID);
                        OldHistory.StatusID = (int)StatusValue.Inactive;
                        OldHistory.LastModifiedBy = userID;
                        OldHistory.LastModifiedDateTime = System.DateTime.Now;

                    }
                    TransactionTable tramsType = TransactionTable.GetItem(_db, wItem.TransactionID);
                    tramsType.StatusID = (int)StatusValue.Rejected;
                    tramsType.PostingStatusID = (int)PostingStatusValue.CompletedByEndUser;
                    tramsType.VerifiedBy = userID;
                    tramsType.VerifiedDateTime = System.DateTime.Now;
                    tramsType.Remarks = remarks;

                    var lineitem = TransactionLineItemTable.GetTransactionLineItems(_db, wItem.TransactionID);
                    foreach (var line in lineitem.ToList())
                    {
                        TransactionLineItemTable oldline = TransactionLineItemTable.GetItem(_db, line.TransactionLineItemID);
                        oldline.StatusID = (int)StatusValue.Rejected;

                        AssetTable OldAsset = AssetTable.GetItem(_db, oldline.AssetID);
                        OldAsset.StatusID = (int)StatusValue.Active;

                    }
                    _db.SaveChanges();
                    TransferAssetDataModel.RemoveModel(currentPageID);
                    base.TraceLog("InternalAssetTransferApproval EmailSingleApproval", $"{SessionUser.Current.Username} -EmailView request reject done. userID-{userID}");
                    return Json(new { Result = "Success", message = " Rejected Successfully" });
                }
                _db.SaveChanges();
                TransferAssetDataModel.RemoveModel(currentPageID);
                base.TraceLog("InternalAssetTransferApproval EmailSingleApproval", $"{SessionUser.Current.Username} -EmailView request approval done. userID-{userID}");
                return Json(new { Result = "Success", message = " Approved Successfully" });
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }
        public IActionResult _LineItememailindex([DataSourceRequest] DataSourceRequest request, int id)
        {
            base.TraceLog("InternalAssetTransferApproval _LineItememailindex", $"{SessionUser.Current.Username} -_LineItememailindex request");
            var query = TransactionLineItemViewForTransfer.GetAllItems(_db).Where(a => a.TransactionID == id);
            var dsResult = query.ToDataSourceResult(request);

            return Json(dsResult);
        }

        public IActionResult _LineItemEmailApproval([DataSourceRequest] DataSourceRequest request, int id, int model)
        {
            base.TraceLog("InternalAssetTransferApproval _LineItemEmailApproval", $"{SessionUser.Current.Username} -_LineItemEmailApproval request");
            // var query = uniformRequestDetailsDataModel;
            var transID = ApprovalHistoryTable.GetItem(_db, id);
            var query = ApprovalHistoryView.GetAllItems(_db, true).Where(a => a.TransactionID == id && a.ApproveModuleID == model).OrderBy(a => a.CreatedDateTime).ThenBy(a => a.OrderNo);

            var dsResult = query.ToDataSourceResult(request);

            // var dsResult = request.ToDataSourceResult(query.AsQueryable(), "UniformRequestUniformItemEntry", "UniformRequestLineItemID");
            // this.TraceLog("Index", $"{SessionUser.Current.Username} - Index page Data Fetch");

            return Json(dsResult);
        }
        public async Task<ActionResult> EmailDocumentUpload(IEnumerable<IFormFile> UploadDoc, int currentPageID, int id)
        {
            try
            {
                base.TraceLog("InternalAssetTransferApproval EmailDocumentUpload", $"{SessionUser.Current.Username} -EmailDocumentUpload request");
                //string absolutePath = DateTime.Now.ToString("yyyy_MM_dd");
                string rootPath = string.Concat(WebHostEnvironment.WebRootPath, "\\");

                string fullPath = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/TransferAssetApproval");
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                //DocumentDataModel.RemoveModel(currentPageID);
                //DocumentDataModel detailsDocuments = DocumentDataModel.GetModel(currentPageID);
                TransferAssetDataModel model = TransferAssetDataModel.GetModel(currentPageID);

                // The Name of the Upload component is "files".
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
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(fileContent.FileName.ToString().Trim('"'));
                        //  var fileName = Path.GetFileName(fileContent.FileName.ToString().Trim('"'));


                        //var newFileName = fileName + fileExtension; //Guid.NewGuid().ToString() + fileExtension;
                        string newFileName = fileName + "" + time + "" + fileExtension;
                        fullPath = Path.Combine(fullPath, newFileName).Replace(" ", "");

                        using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        // data."ImagePath" + i = fullPath;
                        //document.CurrentPageID = currentPageID;
                        document.FileName = newFileName.Replace(" ", "");
                        document.DocumentName = Path.GetFileName(fileContent.FileName.ToString().Trim('"')).Replace(" ", "");//newFileName;
                        document.FullPath = fullPath.Replace(rootPath,"").Replace(" ", "");
                        document.FileExtension = fileExtension;
                        document.CurrentPageID = currentPageID;
                        model.Documents.Add(document);
                    }
                }
                base.TraceLog("InternalAssetTransferApproval EmailDocumentUpload", $"{SessionUser.Current.Username} -EmailDocumentUpload request done");
                // Return an empty string to signify success.
                return Content("");
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }

        }

        public ActionResult EmailDocumentRemove(string[] fileNames, int currentPageID, int id)
        {
            base.TraceLog("InternalAssetTransferApproval EmailDocumentRemove", $"{SessionUser.Current.Username} -EmailDocumentRemove request");
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
            base.TraceLog("InternalAssetTransferApproval EmailDocumentRemove", $"{SessionUser.Current.Username} -EmailDocumentRemove request done");
            // Return an empty string to signify success.
            return Content("");
        }
        public ActionResult DownloadApprovalFile(string fileName, int id)
        {
            base.TraceLog("InternalAssetTransferApproval DownloadApprovalFile", $"{SessionUser.Current.Username} -DownloadApprovalFile request");
            //Build the File Path.
            var history = ApprovalHistoryTable.GetApproval(_db, id);
            int moduleID = history.ApproveModuleID;
            if(moduleID==(int)ApproveModuleValue.InternalAssetTransfer)
            {
                moduleID = (int)ApproveModuleValue.AssetTransfer;
            }
            var module = ApproveModuleTable.GetItem(_db, moduleID).ModuleName;
            string moduleName = string.Concat(module, "Approval");
            var document = (from b in _db.DocumentTable
                            where b.ObjectKeyID == id
                          && b.TransactionType == moduleName && b.DocumentName == fileName.Trim()
                            select b).FirstOrDefault();

            string path = document.FilePath;

            //////Read the File data into Byte Array.
            //byte[] bytes = System.IO.File.ReadAllBytes(path);

            ////Send the File to Download.
            //return File(bytes, "application/octet-stream", document.FileName);
            base.TraceLog("InternalAssetTransferApproval DownloadApprovalFile", $"{SessionUser.Current.Username} -DownloadApprovalFile request done");
            return Json(new { Result = "Success", type = document.TransactionType, fileName = document.FileName });
        }
    }

}
