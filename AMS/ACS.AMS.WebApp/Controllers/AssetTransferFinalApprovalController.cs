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
using System.Runtime.CompilerServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using DocumentFormat.OpenXml.Vml;
using System.IO;
using System.Net;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ACS.AMS.WebApp.Controllers
{
    public class AssetTransferFinalApprovalController : ACSBaseController
    {
        private static IWebHostEnvironment WebHostEnvironment;
        public AssetTransferFinalApprovalController(IWebHostEnvironment _environment)
        {
            WebHostEnvironment = _environment;
        }
        public IActionResult Index(int id)
        {
            if (!base.HasRights(RightNames.AssetTransferFinalApproval, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");
            IndexPageModel indexPage = new IndexPageModel();
            indexPage.EntityInstanceName = new BaseEntityObject();
            indexPage.EntityInstanceName.CurrentPageID = SessionUser.Current.GetNextPageID();
            ViewBag.historyID = id;
            //ViewBag.id = id;
            TransferAssetDataModel.RemoveModel(indexPage.EntityInstanceName.CurrentPageID);
            base.TraceLog("Asset Transfer Index", $"{SessionUser.Current.Username} -Asset Transfer Index page requested");
            return PartialView(indexPage);
        }
        public IActionResult _LineItemindex([DataSourceRequest] DataSourceRequest request, int id,int transactionID)
        {
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
        public FileResult DownloadFile(string fileName)
        {
            //Build the File Path.
            string path = System.IO.Path.Combine(WebHostEnvironment.WebRootPath, "ExcelTemplate/") + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }
        [HttpPost]
        public async Task<ActionResult> DisposalDocUpload(IFormFile fileNames, int currentPageID,int transactionID)
        {
            try
            {
                var filePath = await ReadDocument(fileNames);
                string filePaths = filePath.Item1;
                DataTable dt = filePath.Item2;
                List<string> prop = GetDBColumnName();
                string validation = ValidateExcelSheet(dt, prop);
                string valueValidation = ValidateValues(dt);
                if (string.IsNullOrEmpty(validation) && string.IsNullOrEmpty(valueValidation))
                {
                    TransferAssetDataModel model = TransferAssetDataModel.GetModel(currentPageID);
                    foreach (DataRow row in dt.Rows)
                    {
                        //if (dt.Rows.IndexOf(row) != 0)
                        //{

                            TransferAssetData item = new TransferAssetData();
                            AssetTable asset = AssetTable.GetAssetBarcode(_db, row["Barcode"] + "");
                            if (asset != null)
                            {
                                
                                 item.Asset = asset;
                                item.Asset.Barcode = asset.Barcode;
                                item.Asset.AssetID = asset.AssetID;
                                if (!string.IsNullOrEmpty(row["RoomCode"] + "") && !string.IsNullOrEmpty(row["OracleLocationID"] + ""))
                                {
                                    //ErrorActionResult(new Exception(transactionID + ""));
                                    bool validateType = LocationTable.ValidateLocationType(_db, row["RoomCode"] + "", transactionID, row["OracleLocationID"]+"");
                                    
                                    if (validateType)
                                    {
                                        bool validate = LocationTable.ValidateThirdLocationCode(_db, row["RoomCode"] + "", transactionID, row["OracleLocationID"] + "");
                                        if (validate)
                                        {
                                            var location = LocationTable.GetThirdLocationCode(_db, row["RoomCode"] + "", row["OracleLocationID"] + "");
                                            item.Asset.LocationID = location.LocationID;
                                            item.RoomCode= row["RoomCode"] + "";
                                        }
                                        else
                                        {
                                            TransferAssetDataModel.RemoveModel(currentPageID);
                                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                            string err = "given Location code not room related code please fill it correctly";
                                            return Content(err);
                                           // return base.ErrorActionResult("");
                                        }
                                    }
                                    else
                                    {
                                        TransferAssetDataModel.RemoveModel(currentPageID);
                                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                        string err = "given Location code not matched with transfer Location/Location Type please check it";
                                        return Content(err);
                                        //return base.ErrorActionResult(" ");
                                    }
                                   
                                }
                                else
                                {
                                    TransferAssetDataModel.RemoveModel(currentPageID);
                                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                    string err = "Please fille Room code";
                                    return Content(err);
                                    //return base.ErrorActionResult("Please fille Room code");
                                }
                            }
                               // item.RoomCode = row["RoomCode"] + "";
                                
                                model.LineItems.Add(item);

                           // }
                        }
                    
                    List<int> ids = model.LineItems.Select(a => a.Asset.AssetID).ToList();
                    bool check = TransactionLineItemTable.ValidateDisposalExcel(_db, transactionID, ids);
                    if(!check)
                    {
                       
                        return Json(new { Result = "success", Type = "Upload",  error = "" });
                    }
                    else
                    {
                        TransferAssetDataModel.RemoveModel(currentPageID);
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        string err = "Selected References No against some barcodes missed to give the Transfer details please fill all the details";
                        return Content(err);
                       // return base.ErrorActionResult();
                    }

                   
                }
                else
                {
                    TransferAssetDataModel.RemoveModel(currentPageID);
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    string err = "";
                    if (!string.IsNullOrEmpty(validation))
                    {
                         err = "Upload excel not matched sample format . please upload sample format";
                    }
                    if (!string.IsNullOrEmpty(valueValidation))
                    {
                        err = valueValidation;
                    }
                    return Content(err);
                    
                    //return Json(new { Result = "false", Type = "Upload", FilePath = validation });
                }
                //string validation = "";
                //if (validation == "")
                //    return Json(new { Result = "success", Type = "Upload", FilePath = filePaths });
                //else
                //{

                //    return Json(new { Result = "false", Type = "Upload", FilePath = validation });
                //}
            }
            catch (Exception ex)
            {
                //AppErrorLogTable.SaveException(WasNowContext.CreateNewContext(), ex);
                ErrorActionResult(ex);
                TransferAssetDataModel.RemoveModel(currentPageID);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                string err = ex.Message;
                return Content(err);
                //return Json(new { Result = "false", Type = "Upload", FilePath = "" });

            }
        }
        public async Task<Tuple<string, DataTable>> ReadDocument(IFormFile files)
        {
            string path = System.IO.Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/AssetTransferImport");
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
            return obj;

        }
        public ActionResult DisposalDocRemove(string[] fileNames, int currentPageID)
        {
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

            // Return an empty string to signify success.
            return Json(new { Result = "success", Type = "Remove", FilePath = "" });
        }
        public static string ValidateExcelSheet(DataTable table, List<string> dbColumns)
        {

            string retrunString = string.Empty;
            int outerCount = 0;
            int tablecnt = table.Columns.Count;
            if (table.Columns.Count == dbColumns.Count())
            {
                foreach (var excelAvailabilityField in dbColumns)
                {
                    //var data = res.Data.Rows[0][outerCount].ToString();
                    string columnName = table.Columns[outerCount].ColumnName.ToString();
                    if (columnName.Replace("\n", "").Replace(" ", "").Trim().ToUpper() == excelAvailabilityField.Replace(" ", "").Trim().ToUpper())
                    {
                        outerCount++;
                        continue;
                    }
                    else
                    {
                        retrunString = columnName + " Column not available in Datatable, please Check it";
                        return retrunString;
                    }
                }
            }
            else
            {
                retrunString = " Column mismatched . Please download the sample format and fill the data upload it";
                return retrunString;
            }

            return retrunString;

        }
        public static List<string> GetDBColumnName()
        {

            List<string> prop = new List<string>()
                            {
                                "Barcode",
                                "RoomCode",
                                "OracleLocationID"
                            };

            
            return prop;

        }
        public static string ValidateValues(DataTable table)
        {
            string retrunString = string.Empty;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (string.IsNullOrEmpty(table.Rows[i][j] + ""))
                    {
                        retrunString = "Some fields are have empty please check its";
                        return retrunString;
                    }
                }
            }
            return retrunString;
        }
        [HttpPost]
        public IActionResult SingleApproval(IFormCollection data, string type, int currentPageID, string remarks)
        {
            try
            {
                string transID = data["id"];
                string historyID = data["historyID"];

                ApprovalHistoryTable wItem = ApprovalHistoryTable.GetWorkflowID(_db, int.Parse(historyID), (int)ApproveModuleValue.AssetTransfer);
                //ApprovalHistoryTable wItem = ApprovalHistoryTable.LastLevelData(_db,(int)ApproveModuleValue.AssetTransfer,int.Parse(transID));
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
                        document.TransactionType = "AssetApproval";
                        document.ObjectKeyID = wItem.ApprovalHistoryID;
                        _db.Add(document);
                    }

                }

                if (string.Compare(type, "Approval") == 0)
                {
                    var maxworkflowID = ApprovalHistoryTable.GetWorkflowDetailsworkflowMax(_db, wItem.ApprovalHistoryID);
                    var max = ApprovalHistoryTable.GetItem(_db, maxworkflowID);
                    //List<int> getUserApprovalList = PersonTable.GetUserRoleMappingDetails(_db, SessionUser.Current.UserID).Select(a => (int)a.ApprovalRoleID).ToList();
                    //if (getUserApprovalList.Contains(max.ApprovalRoleID))
                    //{
                    var results = AssetTransferApprovalView.GetAllItems(_db).Where(a => a.UserID == SessionUser.Current.UserID && a.ApprovalHistoryID == int.Parse(historyID)).FirstOrDefault();
                    if (results.ApprovalRoleID == max.ApprovalRoleID)
                    {
                        wItem.StatusID = (int)StatusValue.Active;
                        wItem.LastModifiedDateTime = System.DateTime.Now;
                        wItem.LastModifiedBy = SessionUser.Current.UserID;
                        // wItem.Remarks = remarks;
                        TransactionTable tramsType = TransactionTable.GetItem(_db, wItem.TransactionID);
                        tramsType.StatusID = (int)StatusValue.Active;
                        tramsType.PostingStatusID = (int)PostingStatusValue.CompletedByEndUser;
                        tramsType.VerifiedBy = SessionUser.Current.UserID;
                        tramsType.VerifiedDateTime = System.DateTime.Now;
                        //tramsType.Remarks = remarks;

                        var lineitem = TransactionLineItemTable.GetTransactionLineItems(_db, wItem.TransactionID);
                        foreach (var line in lineitem.ToList())
                        {
                            TransactionLineItemTable oldline = TransactionLineItemTable.GetItem(_db, line.TransactionLineItemID);
                            oldline.StatusID = (int)StatusValue.Active;

                        }
                        var lineitemAsset = TransferAssetDataModel.GetModel(currentPageID);
                        if (lineitemAsset.LineItems.Count() > 0)
                        {
                            foreach (var line in lineitemAsset.LineItems.Select(a => a.Asset).ToList())
                            {
                                AssetTable OldAsset = AssetTable.GetItem(_db, line.AssetID);
                                OldAsset.StatusID = (int)StatusValue.Active;
                                OldAsset.LocationID = line.LocationID;

                                var tranLine = lineitem.Where(a => a.AssetID == line.AssetID).FirstOrDefault();
                                tranLine.RoomID = line.LocationID;
                            }
                        }
                        else
                        {
                            foreach (var line in lineitem)
                            {
                                AssetTable OldAsset = AssetTable.GetItem(_db, line.AssetID);
                                OldAsset.StatusID = (int)StatusValue.Active;
                                OldAsset.LocationID = line.RoomID;
                            }
                        }

                    }
                    else
                    {
                        wItem.StatusID = (int)StatusValue.Active;
                        wItem.LastModifiedDateTime = System.DateTime.Now;
                        wItem.LastModifiedBy = SessionUser.Current.UserID;

                        var lineitem = TransactionLineItemTable.GetTransactionLineItems(_db, wItem.TransactionID);
                        var lineitemAsset = TransferAssetDataModel.GetModel(currentPageID);
                        var getmodeldate = lineitemAsset.LineItems.Select(a => a.Asset).ToList();
                        //lineitemAsset.LineItems.Select(a => a.Asset).ToList()
                        foreach (var line in lineitem)
                        {
                            var locationid = (from b in getmodeldate where b.AssetID == line.AssetID select b).FirstOrDefault();
                            if (locationid != null)
                            {

                                line.RoomID = (int)locationid.LocationID;
                            }

                        }

                    }

                }
                else if (string.Compare(type, "Reject") == 0)
                {
                    wItem.StatusID = (int)StatusValue.Rejected;
                    wItem.LastModifiedDateTime = System.DateTime.Now;
                    wItem.LastModifiedBy = SessionUser.Current.UserID;
                    wItem.Remarks = remarks;
                    var remainingUsersameLevel = ApprovalHistoryTable.SameLevelUserList(_db, wItem.ApprovalHistoryID, (int)ApproveModuleValue.AssetTransfer, wItem.TransactionID);
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

                    return Json(new { Result = "Success", message = " Rejected Successfully" });
                }

                _db.SaveChanges();
                TransferAssetDataModel.RemoveModel(currentPageID);

                return Json(new { Result = "Success", message = " Approved Successfully" });
            }
            catch (Exception ex)
            {
                ApplicationErrorLogTable.SaveException(ex);
                return ErrorActionResult(ex);
            }
        }

        public async Task<ActionResult> DocumentUpload(IEnumerable<IFormFile> UploadDoc, int currentPageID)
        {
            try
            {
                // string absolutePath = DateTime.Now.ToString("yyyy_MM_dd");
                string rootPath = string.Concat(WebHostEnvironment.WebRootPath, "\\");
                string fullPath = System.IO.Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/TransferTransferApproval");
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
                        //var fileName = Path.GetFileName(fileContent.FileName.ToString().Trim('"'));
                        //string fileExtension = System.IO.Path.GetExtension(fileName);
                        //var newFileName = Guid.NewGuid().ToString() + fileExtension;
                        //fullPath = Path.Combine(fullPath, newFileName);
                        string fileExtension = System.IO.Path.GetExtension(System.IO.Path.GetFileName(fileContent.FileName.ToString().Trim('"')));
                        string time = ComboBoxHelper.RemoveSpecialCharacters(String.Format("{0:" + CultureHelper.DateTimeFormatForGrid + "}", DateTime.Now));
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(fileContent.FileName.ToString().Trim('"'));
                        //  var fileName = Path.GetFileName(fileContent.FileName.ToString().Trim('"'));


                        //var newFileName = fileName + fileExtension; //Guid.NewGuid().ToString() + fileExtension;
                        string newFileName = fileName + "" + time + "" + fileExtension;
                        fullPath = System.IO.Path.Combine(fullPath, newFileName).Replace(" ", "");
                        using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        // data."ImagePath" + i = fullPath;
                        //document.CurrentPageID = currentPageID;
                        document.FileName = newFileName.Replace(" ", "");
                        document.DocumentName = System.IO.Path.GetFileName(fileContent.FileName.ToString().Trim('"')).Replace(" ", "");//newFileName;
                        document.FullPath = fullPath.Replace(rootPath, "").Replace(" ", "");
                        document.FileExtension = fileExtension;
                        document.CurrentPageID = currentPageID;
                        model.Documents.Add(document);
                    }
                }
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
            TransferAssetDataModel model = TransferAssetDataModel.GetModel(currentPageID);
            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    if (model.Documents.Count > 0)
                    {
                        foreach (var item in model.Documents.Distinct().ToList())
                        {
                            var fileName = System.IO.Path.GetFileName(fullName);
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

            // Return an empty string to signify success.
            return Content("");
        }
    }
}
