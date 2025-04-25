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
using Microsoft.AspNetCore.Authorization;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Net;
using System.Globalization;
using DocumentFormat.OpenXml.Office2010.Excel;
using ACS.AMS.WebApp.Attributes;

namespace ACS.AMS.WebApp.Controllers
{
    public class TransactionApprovalController : ACSBaseController
    {
        private static IWebHostEnvironment WebHostEnvironment;


        public TransactionApprovalController(IWebHostEnvironment _environment)
        {
            WebHostEnvironment = _environment;
        }

        [HttpGet]
        public ActionResult Index(string pageName)
        {
            if (!base.HasRights(pageName, UserRightValue.View))
                return GotoUnauthorizedPage();
            
            ViewBag.rightName = pageName;
            ViewBag.CurrentPageID = SessionUser.Current.GetNextPageID();
            base.TraceLog("Transaction Index", $"{SessionUser.Current.Username} -Transaction Index page requested");
            return PartialView();
        }

        public IActionResult _Index([DataSourceRequest] DataSourceRequest request,int currentPageID, string screenName)
        {
            if (!base.HasRights(screenName, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");

            int approvalModuleID = (int)ApproveModuleValue.AssetTransfer;
            switch (screenName)
            {
                case "AssetTransferApproval":
                    approvalModuleID = (int)ApproveModuleValue.AssetTransfer;
                    break;
                case "AssetRetirementApproval":
                    approvalModuleID=(int)ApproveModuleValue.AssetRetirement;
                    break;
                case "InternalAssetTransferApproval":
                    approvalModuleID = (int)ApproveModuleValue.InternalAssetTransfer;
                    break;
                case "AssetApproval":
                    approvalModuleID = (int)ApproveModuleValue.AssetAddition;
                    break;
                case "AssetMaintenanceRequestApproval":
                    approvalModuleID = (int)ApproveModuleValue.AssetMaintenanceRequest;
                    break;
                case "AMCScheduleApproval":
                    approvalModuleID = (int)ApproveModuleValue.AMCSchedule;
                    break;
            }

            List<int> getUserApprovalList = PersonTable.GetUserRoleMappingDetails(_db, SessionUser.Current.UserID).Select(a => (int)a.ApprovalRoleID).ToList();
            IQueryable<TransactionApprovalView> result = TransactionApprovalView.GetAllItems(_db).Where(a => a.UserID == SessionUser.Current.UserID && a.SerialNo == 1 
            && (a.ApprovalStatusID == (int)StatusValue.WaitingForApproval || a.ApprovalStatusID == (int)StatusValue.ReApproval) && a.ApproveModuleID == approvalModuleID);
            var dsResult = result.ToDataSourceResult(request);


            base.TraceLog("Transaction Index", $"{SessionUser.Current.Username} -Transaction Index page Data Fetch");
            return Json(dsResult);
            //return Json("");
        }

        public ActionResult Details(int id, string pageName)
        {
            if (!base.HasRights(pageName, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("Transaction approval Details", $"{SessionUser.Current.Username} Transaction approval details request");
            int currentPageId = SessionUser.Current.GetNextPageID();
            ViewBag.CurrentPageID = SessionUser.Current.GetNextPageID();
            ViewBag.ApprovalHistoryID = id;
            ViewBag.rightName = pageName;
           
            return PartialView();

        }
        public ActionResult Edit(int id, string pageName)
        {
            if (!base.HasRights(pageName, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("Transaction approval Edit", $"{SessionUser.Current.Username} Transaction approval Edit request");
            IndexPageModel indexPage = new IndexPageModel();
            indexPage.EntityInstance = new BaseEntityObject();
            indexPage.EntityInstance.CurrentPageID = SessionUser.Current.GetNextPageID();
            int currentPageId = indexPage.EntityInstance.CurrentPageID;
            ViewBag.CurrentPageID = indexPage.EntityInstance.CurrentPageID;
            ViewBag.ApprovalHistoryID = id;
            ViewBag.rightName = pageName;
            var finalApproval = TransactionApprovalView.GetItem(_db, id);
            ViewBag.CurrentLevel = finalApproval.OrderNo;

            if(finalApproval != null)
            {
                if((bool)finalApproval.enableUpdate)
                {
                    if (finalApproval.ApproveModuleID == (int)ApproveModuleValue.AssetTransfer)
                    {
                        return PartialView("TransferFinalApproval", indexPage);
                    }
                    else if (finalApproval.ApproveModuleID == (int)ApproveModuleValue.AssetRetirement)
                    {
                        return PartialView("RetirementFinalApproval", indexPage);
                    }
                }
                else
                {
                    return PartialView(indexPage);
                }
            }
            return PartialView(indexPage);

        }
        public IActionResult TransferFinalApproval(int id)
        {
            if (!base.HasRights(RightNames.AssetTransferFinalApproval, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("Transaction approval TransferFinalApproval", $"{SessionUser.Current.Username} Transaction approval TransferFinalApproval request");
            IndexPageModel indexPage = new IndexPageModel();
            indexPage.EntityInstance = new BaseEntityObject();
            indexPage.EntityInstance.CurrentPageID = SessionUser.Current.GetNextPageID();
            ViewBag.historyID = id;
            //ViewBag.id = id;
            TransferAssetDataModel.RemoveModel(indexPage.EntityInstance.CurrentPageID);
            base.TraceLog("Asset Transfer Index", $"{SessionUser.Current.Username} -Asset Transfer Index page requested");
            return PartialView(indexPage);
        }
      
        public ActionResult EmailEdit(int id, int userID)
        {
            //base.TraceLog("Transaction approval EmailEdit", $"{SessionUser.Current.Username} Transaction approval EmailEdit request.userId-{userID}");
            ViewBag.userID = userID;
            ViewBag.ApprovalHistoryID = id;
            ViewBag.currentPageID = SessionUser.GetWithoutSessionNextPageID();
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
           // base.TraceLog("Transaction approval EmailView", $"{SessionUser.Current.Username} Transaction approval EmailView request.id-{id}");
            ViewBag.userID = 1;
            ViewBag.ApprovalHistoryID = id;
          
            ViewBag.currentPageID = SessionUser.GetWithoutSessionNextPageID();
            var getresult = (from b in _db.ApprovalHistoryTable where b.ApprovalHistoryID == id select b).FirstOrDefault();
            ViewBag.moduleID = getresult.ApproveModuleID;
            if (getresult.StatusID == (int)StatusValue.Active || getresult.StatusID == (int)StatusValue.Rejected)
            {
                ModelState.AddModelError("12", "The Approval Process was completed, Asset status check with the application");
                return PartialView("EmailView");
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
       
        public IActionResult _LineItemindex([DataSourceRequest] DataSourceRequest request, int currentPageID,int transactionID)
        {
            try
            {
              
                if (SessionUser.Current != null)
                {
                    base.TraceLog("Transaction approval _LineItemindex", $"{SessionUser.Current.Username} Transaction approval _LineItemindex request.");
                }
                else
                {
                    base.TraceLog("Transaction approval _LineItemindex", "Email Transaction approval _LineItemindex request.");
                }
            
                var transID = ApprovalHistoryTable.GetItem(_db, transactionID);
                var query = TransactionLineItemViewForTransfer.GetAllItems(_db).Where(a => a.TransactionID == transID.TransactionID);
                var dsResult = query.ToDataSourceResult(request);
                return Json(dsResult);
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }
        //_LineItemApproval

        public IActionResult _LineItemApproval([DataSourceRequest] DataSourceRequest request, int currentPageID, int transactionID)
        {
            try
            {
               
                if (SessionUser.Current != null)
                {
                    base.TraceLog("Transaction approval _LineItemindex", $"{SessionUser.Current.Username} Transaction approval _LineItemApproval request.");
                }
                else
                {
                    base.TraceLog("Transaction approval _LineItemindex", "Email Transaction approval _LineItemApproval request.");
                }
                
                var transID = ApprovalHistoryTable.GetItem(_db, transactionID);
                var query = ApprovalHistoryView.GetAllItems(_db, true).Where(a => a.TransactionID == transID.TransactionID && a.ApproveModuleID == transID.ApproveModuleID).OrderBy(a => a.CreatedDateTime).ThenBy(a => a.OrderNo);         
                var dsResult = query.ToDataSourceResult(request);
                return Json(dsResult);
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }

        public async Task<ActionResult> DocumentUpload(IEnumerable<IFormFile> UploadDoc, int currentPageID, int id,int userID)
        {
            try
            {
                if (SessionUser.Current != null)
                {
                    base.TraceLog("Transaction approval DocumentUpload", $"{SessionUser.Current.Username} Transaction approval DocumentUpload request.");
                }
                else
                {
                    base.TraceLog("Transaction approval DocumentUpload", "Email Transaction approval DocumentUpload request.");
                }
                string rootPath = string.Concat(WebHostEnvironment.WebRootPath, "\\");
                string fullPath = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/TransactionDocument");
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                CreateSessionObject(_db, userID);
                //if(SessionUser.Current==null)
                //{
                //    User_LoginUserTable userDetails = User_LoginUserTable.GetItem(_db, userID);
                //    var companymapping = UserCompanyMappingTable.GetCompanyForPersonID(_db, userID).FirstOrDefault();
                //    SessionUser sessionuser = SessionUser.CreateSessionUserObject(userDetails.UserName, userDetails.UserID,
                //          userID, 1, "en-GB", 1, companymapping.CompanyID);
                //}
                TransferAssetDataModel model = TransferAssetDataModel.GetModel(currentPageID);

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
                        string newFileName = Guid.NewGuid()+"" + fileExtension;//fileName + "" + time + "" + fileExtension;
                        fullPath = Path.Combine(fullPath, newFileName).Replace(" ", "");

                        using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        // data."ImagePath" + i = fullPath;
                        //document.CurrentPageID = currentPageID;
                        document.FileName = newFileName.Replace(" ","");
                        document.DocumentName = Path.GetFileName(fileContent.FileName.ToString().Trim('"')).Replace(" ", "");//newFileName;
                        document.FullPath = fullPath.Replace(rootPath,"").Replace(" ", "");
                        document.FileExtension = fileExtension;
                        document.CurrentPageID = currentPageID;
                        model.Documents.Add(document);
                    }
                }
                // Return an empty string to signify success.
                if (SessionUser.Current != null)
                {
                    base.TraceLog("Transaction approval DocumentUpload", $"{SessionUser.Current.Username} Transaction approval DocumentUpload request done.");
                }
                else
                {
                    base.TraceLog("Transaction approval DocumentUpload", "Email Transaction approval DocumentUpload request done.");
                }
              //  base.TraceLog("Transaction approval DocumentUpload", $"{SessionUser.Current.Username} Transaction approval DocumentUpload request done.");
                return Content("");
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }

        }

        public ActionResult DocumentRemove(string[] fileNames, int currentPageID, int id)
        {
            if (SessionUser.Current != null)
            {
                base.TraceLog("Transaction approval DocumentRemove", $"{SessionUser.Current.Username} Transaction approval DocumentRemove request.");
            }
            else
            {
                base.TraceLog("Transaction approval DocumentRemove", "Email Transaction approval DocumentRemove request.");
            }
           // base.TraceLog("Transaction approval DocumentRemove", $"{SessionUser.Current.Username} Transaction approval DocumentRemove request.");
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
                            if (string.Compare(fileName, item.FileName) == 0)
                            {
                                var query = model.Documents.Where(a => a.DocumentName == item.DocumentName).FirstOrDefault(); 
                                var physicalPath = Path.Combine(WebHostEnvironment.WebRootPath, item.FullPath); //Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/TransferAsset", fileName);
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
            base.TraceLog("Transaction approval DocumentRemove", $"{SessionUser.Current.Username} Transaction approval DocumentRemove request done.");
            // Return an empty string to signify success.
            return Content("");
        }
        [HttpPost]
        public IActionResult ApprovalProcess(IFormCollection data, string type, string remarks, int currentPageID,int moduleID,int userID,int enableUpdate,int levels)
        {
            try
            {
               // base.TraceLog("Transaction approval ApprovalProcess", $"{SessionUser.Current.Username} Transaction approval ApprovalProcess request.");
                string historyID = data["id"];
                if(historyID==null)
                {
                    historyID = data["id[]"];
                }
                string[] arrRequestID = historyID.Split(',');
                int[] requestID = Array.ConvertAll(arrRequestID, s => int.Parse(s));
                int requestCount = requestID.Count();
                CreateSessionObject(_db, userID);
                foreach (int ids in requestID)
                {
                    ApprovalHistoryTable wItem = ApprovalHistoryTable.GetWorkflowID(_db, ids, moduleID);
                    
                    TransferAssetDataModel model = TransferAssetDataModel.GetModel(currentPageID);
                    string moduleName = string.Empty;
                    if (moduleID == (int)ApproveModuleValue.AssetTransfer || moduleID == (int)ApproveModuleValue.InternalAssetTransfer)
                    {
                        moduleName = "AssetTransferApproval";
                    }
                    else if(moduleID==(int)ApproveModuleValue.AssetRetirement)
                    {   
                        moduleName = "AssetRetirementApproval";
                    }
                    else if (moduleID == (int)ApproveModuleValue.AssetMaintenanceRequest)
                    {
                        moduleName = "AssetMaintenanceRequestApproval";
                    }
                    else if (moduleID == (int)ApproveModuleValue.AMCSchedule)
                    {
                        moduleName = "AMCScheduleApproval";
                    }
                    else if (moduleID == (int)ApproveModuleValue.AssetAddition)
                    {
                        moduleName = "AssetAdditionApproval";
                    }
                    ApprovalHelper.SaveApproval(_db, type, remarks, model.Documents, moduleName, wItem, ids, userID, moduleID, enableUpdate, model, currentPageID,levels) ;
                }
                _db.SaveChanges();
                TransferAssetDataModel.RemoveModel(currentPageID);

                switch(type)
                {
                    case "Approval":
                        type = "Approved";
                        break;
                    case "Reject":
                        type = "Rejected";
                        break;
                        
                }
                return Json(new { Result = "Success", message =type+ " Successfully" });
               
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }
        public void CreateSessionObject(AMSContext _db,int userID)
        {
            if (SessionUser.Current == null)
            {
                User_LoginUserTable userDetails = User_LoginUserTable.GetItem(_db, userID);
                var companymapping = UserCompanyMappingTable.GetCompanyForPersonID(_db, userID).FirstOrDefault();
                SessionUser sessionuser = SessionUser.CreateSessionUserObject(userDetails.UserName, userDetails.UserID,
                      userID, 1, "en-GB", 1, companymapping.CompanyID);
            }
        }
        public FileResult DownloadFile(string fileName,bool finalLevelDoc=false)
        {
            if (SessionUser.Current != null)
            {
                base.TraceLog("Transaction approval DownloadFile", $"{SessionUser.Current.Username} Transaction approval DownloadFile request.");
            }
            else
            {
                base.TraceLog("Transaction approval DownloadFile", "Email Transaction approval DownloadFile request.");
            }
          //  base.TraceLog("Transaction approval DownloadFile", $"{SessionUser.Current.Username} Transaction approval DownloadFile request.");
            //Build the File Path.
            string path = string.Empty;
            if (!finalLevelDoc)
            {
                path = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/") + fileName;
            }
           else
            {
                path = Path.Combine(WebHostEnvironment.WebRootPath, "ExcelTemplate/") + fileName;
            }

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }

        //string path = Path.Combine(WebHostEnvironment.WebRootPath, "ExcelTemplate/") + fileName;

        public ActionResult DownloadApprovalFile(string fileName, int id)
        {
            if (SessionUser.Current != null)
            {
                base.TraceLog("Transaction approval DownloadApprovalFile", $"{SessionUser.Current.Username} Transaction approval DownloadApprovalFile request.");
            }
            else
            {
                base.TraceLog("Transaction approval DownloadApprovalFile", "Email Transaction approval DownloadApprovalFile request.");
            }
          //  base.TraceLog("Transaction approval DownloadApprovalFile", $"{SessionUser.Current.Username} Transaction approval DownloadApprovalFile request.");
            //Build the File Path.
            var history = ApprovalHistoryTable.GetApproval(_db, id);
            int moduleID = history.ApproveModuleID;
            if (moduleID == (int)ApproveModuleValue.InternalAssetTransfer)
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
            return Json(new { Result = "Success", type = document.TransactionType, fileName = document.FileName });
        }
        public IActionResult _LineItememailindex([DataSourceRequest] DataSourceRequest request, int id)
        {
            if (SessionUser.Current != null)
            {
                base.TraceLog("Transaction approval _LineItememailindex", $"{SessionUser.Current.Username} Transaction approval _LineItememailindex request.");
            }
            else
            {
                base.TraceLog("Transaction approval _LineItememailindex", "Email Transaction approval _LineItememailindex request.");
            }
           // base.TraceLog("Transaction approval _LineItememailindex", $"{SessionUser.Current.Username} Transaction approval _LineItememailindex request.");
            var query = TransactionLineItemViewForTransfer.GetAllItems(_db).Where(a => a.TransactionID == id);
            var dsResult = query.ToDataSourceResult(request);
            return Json(dsResult);
        }

        public IActionResult _LineItemEmailApproval([DataSourceRequest] DataSourceRequest request, int id, int model)
        {
            if (SessionUser.Current != null)
            {
                base.TraceLog("Transaction approval _LineItemEmailApproval", $"{SessionUser.Current.Username} Transaction approval _LineItemEmailApproval request.");
            }
            else
            {
                base.TraceLog("Transaction approval _LineItemEmailApproval", "Email Transaction approval _LineItemEmailApproval request.");
            }
           // base.TraceLog("Transaction approval _LineItemEmailApproval", $"{SessionUser.Current.Username} Transaction approval _LineItemEmailApproval request.");
            var transID = ApprovalHistoryTable.GetItem(_db, id);
            var query = ApprovalHistoryView.GetAllItems(_db, true).Where(a => a.TransactionID == id && a.ApproveModuleID == model).OrderBy(a => a.CreatedDateTime).ThenBy(a => a.OrderNo);

            var dsResult = query.ToDataSourceResult(request);
            return Json(dsResult);
        }

        [HttpPost]
        public async Task<ActionResult> ExcelDocUpload(IFormFile fileNames, int currentPageID, int transactionID,int moduleID)
        {
            try
            {
                base.TraceLog("Transaction approval ExcelDocUpload", $"{SessionUser.Current.Username} Transaction approval ExcelDocUpload request.");
                if (fileNames != null)
                {

                var filePath = await ReadDocument(fileNames);
                string filePaths = filePath.Item1;
                DataTable dt = filePath.Item2;
                //   List<string> prop = new List<string>();
                int appPageID = 0;
                if (moduleID == (int)ApproveModuleValue.AssetTransfer)
                {
                    appPageID = (int)EntityValues.AssetTransfer;
                }
                if (moduleID == (int)ApproveModuleValue.AssetRetirement)
                {
                    appPageID = (int)EntityValues.AssetRetirement;
                }
                var tableField = ImportFormatLineItemNewTable.GetImportFinalFormat(_db, appPageID);

                var excelLineItem = tableField.OrderBy(a => a.DispalyOrderID).Select(a => a.ImportField).ToList();
                var mandtoryFields = tableField.Where(a => a.IsMandatory == true).Select(a => a.ImportField).ToList();
                var UniqueFields = tableField.Where(a => a.IsUnique == true);

                string validationColumnMisMatch = ApprovalHelper.ValidateExcelSheet(dt, excelLineItem);
                if (string.IsNullOrEmpty(validationColumnMisMatch))
                {
                    string validationMandatory = ApprovalHelper.ValidateMandatory(dt, mandtoryFields);
                    string validationUnique = ApprovalHelper.ValidateUnique(dt, UniqueFields);
                    if (moduleID == (int)ApproveModuleValue.AssetTransfer)
                    {
                        if (string.IsNullOrEmpty(validationMandatory) && string.IsNullOrEmpty(validationUnique))
                        {
                            TransferAssetDataModel model = TransferAssetDataModel.GetModel(currentPageID);
                            foreach (DataRow row in dt.Rows)
                            {
                                TransferAssetData item = new TransferAssetData();
                                AssetTable asset = AssetTable.GetAssetBarcode(_db, row["Barcode"] + "");
                                if (asset != null)
                                {

                                    item.Asset = asset;
                                    item.Asset.Barcode = asset.Barcode;
                                    item.Asset.AssetID = asset.AssetID;
                                    if (!string.IsNullOrEmpty(row["RoomCode"] + ""))
                                    {
                                        bool validateType = LocationTable.ValidateLocationType(_db, row["RoomCode"] + "", transactionID);

                                        if (validateType)
                                        {
                                            bool validate = LocationTable.ValidateThirdLocationCode(_db, row["RoomCode"] + "", transactionID);
                                            if (validate)
                                            {
                                                var location = LocationTable.GetThirdLocationCode(_db, row["RoomCode"] + "");
                                                item.Asset.LocationID = location.LocationID;
                                                item.RoomCode = row["RoomCode"] + "";
                                            }
                                            else
                                            {
                                                TransferAssetDataModel.RemoveModel(currentPageID);
                                                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                                string err = "given Location code not room related code please fill it correctly";
                                                return base.ErrorActionResult(err);
                                                // return base.ErrorActionResult("");
                                            }
                                        }
                                        else
                                        {
                                            TransferAssetDataModel.RemoveModel(currentPageID);
                                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                            string err = "given Location code not matched with transfer Location/Location Type please check it";
                                            return base.ErrorActionResult(err);
                                            //return base.ErrorActionResult(" ");
                                        }

                                    }
                                    else
                                    {
                                        TransferAssetDataModel.RemoveModel(currentPageID);
                                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                        string err = "Please fille Room code";
                                        return base.ErrorActionResult(err);

                                    }
                                    if (!string.IsNullOrEmpty(row["DepartmentCode"] + ""))
                                    {
                                        var department = DepartmentTable.GetDepartmentCode(_db, row["DepartmentCode"] + "");
                                        if (department != null)
                                        {
                                            item.Asset.DepartmentID = department.DepartmentID;
                                        }
                                        else
                                        {
                                            TransferAssetDataModel.RemoveModel(currentPageID);
                                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                            string err = row["DepartmentCode"] + "" + " - given Department not available in a db ";
                                            return base.ErrorActionResult(err);

                                        }
                                    }
                                    if (!string.IsNullOrEmpty(row["CustodianCode"] + ""))
                                    {
                                        var custodian = PersonTable.GetPerson(_db, row["CustodianCode"] + "");
                                        if (custodian != null)
                                        {
                                            item.Asset.CustodianID = custodian.PersonID;
                                        }
                                        else
                                        {
                                            TransferAssetDataModel.RemoveModel(currentPageID);
                                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                            string err = row["CustodianCode"] + "" + " - given Custodian not available in a db ";
                                            return base.ErrorActionResult(err);
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(row["SectionCode"] + ""))
                                    {
                                        var section = SectionTable.GetSectionCode(_db, row["SectionCode"] + "");
                                        if (section != null)
                                        {
                                            item.Asset.SectionID = section.SectionID;
                                        }
                                        else
                                        {
                                            TransferAssetDataModel.RemoveModel(currentPageID);
                                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                            string err = row["CustodianCode"] + "" + " - given Custodian not available in a db ";
                                            return base.ErrorActionResult(err);
                                        }
                                    }
                                }
                                model.LineItems.Add(item);
                            }

                            List<int> ids = model.LineItems.Select(a => a.Asset.AssetID).ToList();
                            bool check = TransactionLineItemTable.ValidateDisposalExcel(_db, transactionID, ids);
                            if (!check)
                            {
                                base.TraceLog("Transaction approval excelupload", $"{SessionUser.Current.Username} Transaction approval excelupload request done.");
                                return Json(new { Result = "success", Type = "Upload", error = "" });
                            }
                            else
                            {
                                TransferAssetDataModel.RemoveModel(currentPageID);
                                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                string err = "Selected References No against some barcodes missed to give the Transfer details please fill all the details";
                                return base.ErrorActionResult(err);
                                // return base.ErrorActionResult();
                            }
                        }
                        else
                        {
                            string err = string.Empty;
                            if (!string.IsNullOrEmpty(validationUnique))
                            {
                                err = validationUnique + "These Values Have been Duplicated";
                            }
                            if (!string.IsNullOrEmpty(validationMandatory))
                            {
                                err += ',' + validationMandatory + "Has some mandatory Field Missing";
                            }
                            TransferAssetDataModel.RemoveModel(currentPageID);
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            return base.ErrorActionResult(err);
                        }
                    }
                    if (moduleID == (int)ApproveModuleValue.AssetRetirement)
                    {
                        if (string.IsNullOrEmpty(validationMandatory) && string.IsNullOrEmpty(validationUnique))
                        {
                            // RetirementDataModel model = RetirementDataModel.GetModel(currentPageID);
                            TransferAssetDataModel model = TransferAssetDataModel.GetModel(currentPageID);
                            foreach (DataRow row in dt.Rows)
                            {

                                // RetirementData item = new RetirementData();
                                TransferAssetData item = new TransferAssetData();
                                AssetTable asset = AssetTable.GetAssetBarcode(_db, row["Barcode"] + "");
                                if (asset != null)
                                {
                                    if (!string.IsNullOrEmpty(row["RetirementTypeCode"] + ""))
                                    {
                                        RetirementTypeTable oldretirement = RetirementTypeTable.GetRetirementTypeCode(_db, row["RetirementTypeCode"] + "");
                                        if (oldretirement == null)
                                        {
                                            RetirementTypeTable retirement = new RetirementTypeTable();
                                            retirement.RetirementTypeCode = row["RetirementTypeCode"] + "";
                                            retirement.RetirementTypeName = row["RetirementTypeCode"] + "";
                                            retirement.StatusID = (int)StatusValue.Active;
                                            retirement.CreatedBy = SessionUser.Current.UserID;
                                            retirement.CreatedDateTime = DateTime.Now.Date;
                                            _db.Add(retirement);
                                            _db.SaveChanges();
                                        }
                                    }

                                    RetirementTypeTable retirementGet = RetirementTypeTable.GetRetirementTypeCode(_db, row["RetirementTypeCode"] + "");
                                    item.Asset = asset;
                                    item.Asset.Barcode = asset.Barcode;
                                    item.Asset.AssetID = asset.AssetID;
                                    item.Asset.DisposalReferenceNo = row["RetirementReferencesNo"] + "";
                                    item.Asset.DisposedDateTime = string.IsNullOrEmpty(row["RetirementDate"] + "") ? System.DateTime.Now : DateTime.ParseExact(row["RetirementDate"] + "", CultureHelper.ConfigureDateFormat, CultureInfo.InvariantCulture); //DateTime.Parse(row["RetirementDate"] + "");//System.DateTime.Now;
                                    item.Asset.DisposedRemarks = row["RetirementRemarks"] + "";
                                    item.Asset.DisposalValue = string.IsNullOrEmpty(row["RetirementValue"] + "") ? 0 : decimal.Parse(row["RetirementValue"] + "");
                                    item.Asset.Attribute40 = "Import Asset Retirement";
                                    item.Asset.ProceedofSales = string.IsNullOrEmpty(row["ProceedOfSales"] + "") ? 0 : decimal.Parse(row["ProceedOfSales"] + "");
                                    item.Asset.CostOfRemoval = string.IsNullOrEmpty(row["CostOfRemoval"] + "") ? 0 : decimal.Parse(row["CostOfRemoval"] + "");
                                    if (retirementGet != null)
                                    {
                                        item.Asset.RetirementTypeID = retirementGet.RetirementTypeID;
                                    }
                                    model.LineItems.Add(item);

                                }

                            }
                            List<int> ids = model.LineItems.Select(a => a.Asset.AssetID).ToList();
                            bool check = TransactionLineItemTable.ValidateDisposalExcel(_db, transactionID, ids);
                            if (!check)
                            {
                                return Json(new { Result = "success", Type = "Upload", FilePath = filePaths, error = "" });
                            }
                            else
                            {
                                //RetirementDataModel.RemoveModel(currentPageID);
                                TransferAssetDataModel.RemoveModel(currentPageID);
                                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                string err = "Selected References No against some barcodes missed to give the disposal details please fill all the details";
                                return ErrorActionResult(err);
                                //return base.ErrorActionResult("Selected References No against some barcodes missed to give the disposal details please fill all the details");
                            }


                        }
                        else
                        {
                            //RetirementDataModel.RemoveModel(currentPageID);
                            TransferAssetDataModel.RemoveModel(currentPageID);
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            string err = string.Empty;
                            if (!string.IsNullOrEmpty(validationUnique))
                            {
                                err = validationUnique + "These Values Have been Duplicated";
                            }
                            if (!string.IsNullOrEmpty(validationMandatory))
                            {
                                err += ',' + validationMandatory + "Has some mandatory Field Missing";
                            }
                            return ErrorActionResult(err);

                        }
                    }
                }
                else
                {
                    string err = string.Empty;
                    if (!string.IsNullOrEmpty(validationColumnMisMatch))
                    {
                        err = validationColumnMisMatch;
                    }
                    TransferAssetDataModel.RemoveModel(currentPageID);
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    // string err = "Excel there is no data please fill the data and import";
                    return base.ErrorActionResult(err);
                }
            }
            else
            {
                    TransferAssetDataModel.RemoveModel(currentPageID);
                    return Json(new { Result = "failed", Type = "Upload", FilePath = "", error = "" });
                }
                return Json(new { Result = "failed", Type = "Upload", FilePath = "", error = "" });

            }
            catch (FormatException ex)
            {
                return ErrorActionResult(ex.Message);
            }
            catch (Exception ex)
            {
                //AppErrorLogTable.SaveException(WasNowContext.CreateNewContext(), ex);
                ErrorActionResult(ex);
                TransferAssetDataModel.RemoveModel(currentPageID);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                string err = ex.Message;
                return ErrorActionResult(err);
                //return Json(new { Result = "false", Type = "Upload", FilePath = "" });

            }
        }
        public async Task<Tuple<string, DataTable>> ReadDocument(IFormFile files)
        {
            base.TraceLog("Transaction approval ReadDocument", $"{SessionUser.Current.Username} Transaction approval ReadDocument request.");
            string path = System.IO.Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/ImportExcel");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var newFileName = Guid.NewGuid().ToString() + files.FileName;
            string fileName = System.IO.Path.GetFileName(files.FileName);
            string NameTrimmed = String.Concat(newFileName.Where(c => !Char.IsWhiteSpace(c)));
            string filePath = System.IO.Path.Combine(path, NameTrimmed);
            DataTable excelTable = new DataTable();

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                files.CopyTo(stream);
                stream.Position = 0;
                string FileName = System.IO.Path.GetExtension(filePath);

                if (FileName == ".xls")
                {
                    HSSFWorkbook workbook = new HSSFWorkbook(stream);
                    excelTable = ImportExcel.Import(excelTable, workbook, _db);
                   
                }
                else
                {
                    XSSFWorkbook workbooks = new XSSFWorkbook(stream);
                    excelTable = ImportExcel.Import(excelTable, workbooks, _db);
                  
                }
            }

            Tuple<string, DataTable> obj = new Tuple<string, DataTable>(filePath, excelTable);
            base.TraceLog("Transaction approval ReadDocument", $"{SessionUser.Current.Username} Transaction approval ReadDocument request done.");
            return obj;

        }
        public ActionResult ExcelDocRemove(string[] fileNames, int currentPageID)
        {
            base.TraceLog("Transaction approval ExcelDocRemove", $"{SessionUser.Current.Username} Transaction approval ExcelDocRemove request.");
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
            base.TraceLog("Transaction approval ExcelDocRemove", $"{SessionUser.Current.Username} Transaction approval ExcelDocRemove request done.");
            // Return an empty string to signify success.
            return Json(new { Result = "success", Type = "Remove", FilePath = "" });
        }
        public IActionResult _LineItemFinalindex([DataSourceRequest] DataSourceRequest request, int id, int transactionID)
        {
            base.TraceLog("Transaction approval _LineItemFinalindex", $"{SessionUser.Current.Username} Transaction approval _LineItemFinalindex request.");
            // var query = uniformRequestDetailsDataModel;
            TransferAssetDataModel model = TransferAssetDataModel.GetModel(id);
            if (model != null)
            {
                var query = model.LineItems.Select(a => a.Asset);
                var dsResult = query.ToDataSourceResult(request);
                base.TraceLog("UserMaster Index", $"{SessionUser.Current.Username} -UserMaster Index page Data Fetch");
                return Json(dsResult);
            }
            this.TraceLog("Index", $"{SessionUser.Current.Username} - Index page Data Fetch");

            return Json("");
        }
    }

}
