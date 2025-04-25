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
    public class AssetRetirementFinalApprovalController : ACSBaseController
    {
        private static IWebHostEnvironment WebHostEnvironment;
        public AssetRetirementFinalApprovalController(IWebHostEnvironment _environment)
        {
            WebHostEnvironment = _environment;
        }
        public IActionResult Index(int id)
        {
            if (!base.HasRights(RightNames.AssetRetirementFinalApproval, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");
            IndexPageModel indexPage = new IndexPageModel();
            indexPage.EntityInstanceName = new BaseEntityObject();
            indexPage.EntityInstanceName.CurrentPageID = SessionUser.Current.GetNextPageID();
            ViewBag.historyID = id;
            RetirementDataModel.RemoveModel(indexPage.EntityInstanceName.CurrentPageID);
            base.TraceLog("Asset Retirement Final Approval Index", $"{SessionUser.Current.Username} -Asset Retirement Final Approval Index page requested");
            return PartialView(indexPage);
        }
        public IActionResult _LineItemindex([DataSourceRequest] DataSourceRequest request, int id,int transactionID)
        {
            // var query = uniformRequestDetailsDataModel;
            RetirementDataModel model = RetirementDataModel.GetModel(id);
            if (model != null)
            {

                //List<int> ids  = model.LineItems.Select(a => a.Asset.AssetID).ToList();
                // var query = TransactionLineItemViewForTransfer.GetAllItems(_db).Where(a => a.TransactionID == transactionID);
                // query = query.Where(a => ids.Contains(a.AssetID));
                var query = model.LineItems.Select(a => a.Asset);
                var dsResult = query.ToDataSourceResult(request);
                base.TraceLog("Asset Retirement Final Approval Index Index", $"{SessionUser.Current.Username} -Asset Retirement Final Approval  Index page Data Fetch");
                return Json(dsResult);
            }
            this.TraceLog("Index", $"{SessionUser.Current.Username} - Index page Data Fetch");

            return Json("");
        }
        public FileResult DownloadFile(string fileName)
        {
            this.TraceLog("Asset Retirement Final Approval DownloadFile", $"{SessionUser.Current.Username} - DownloadFile call");
            //Build the File Path.
            string path = System.IO.Path.Combine(WebHostEnvironment.WebRootPath, "ExcelTemplate/") + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            this.TraceLog("Asset Retirement Final Approval DownloadFile", $"{SessionUser.Current.Username} - DownloadFile call Done");
            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }
        [HttpPost]
        public async Task<ActionResult> DisposalDocUpload(IFormFile fileNames, int currentPageID,int transactionID)
        {
            try
            {
                this.TraceLog("Asset Retirement Final Approval DisposalDocUpload", $"{SessionUser.Current.Username} - DisposalDocUpload call");
                var filePath = await ReadDocument(fileNames);
                string filePaths = filePath.Item1;
                DataTable dt = filePath.Item2;
                List<string> prop = GetDBColumnName();
                string validation = ValidateExcelSheet(dt, prop);
                string valueValidation = ValidateValues(dt);
                if (string.IsNullOrEmpty(validation) && string.IsNullOrEmpty(valueValidation))
                {
                    RetirementDataModel model = RetirementDataModel.GetModel(currentPageID);
                    foreach (DataRow row in dt.Rows)
                    {
                        //if (dt.Rows.IndexOf(row) != 0)
                        //{

                            RetirementData item = new RetirementData();
                            AssetTable asset = AssetTable.GetAssetBarcode(_db, row["Barcode"] + "");
                            if (asset != null)
                            {
                                if(!string.IsNullOrEmpty(row["RetirementTypeCode"] + ""))
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
                                item.Asset.DisposedDateTime = string.IsNullOrEmpty(row["RetirementDate"] + "") ? System.DateTime.Now : DateTime.ParseExact(row["RetirementDate"] + "", "dd/MM/yyyy", null); //DateTime.Parse(row["RetirementDate"] + "");//System.DateTime.Now;
                                item.Asset.DisposedRemarks = row["RetirementRemarks"] + "";
                                item.Asset.DisposalValue = string.IsNullOrEmpty(row["RetirementValue"] + "") ? 0 : decimal.Parse(row["RetirementValue"] + "");
                                item.Asset.Attribute40 = "Import Asset Retirement";
                                item.Asset.ProceedofSales= string.IsNullOrEmpty(row["ProceedOfSales"] + "") ? 0 : decimal.Parse(row["ProceedOfSales"] + "");
                                item.Asset.CostOfRemoval = string.IsNullOrEmpty(row["CostOfRemoval"] + "") ? 0 : decimal.Parse(row["CostOfRemoval"] + "");
                                if (retirementGet != null)
                                {
                                    item.Asset.RetirementTypeID = retirementGet.RetirementTypeID;
                                }
                                model.LineItems.Add(item);

                            }
                        //}
                    }
                    List<int> ids = model.LineItems.Select(a => a.Asset.AssetID).ToList();
                    bool check = TransactionLineItemTable.ValidateDisposalExcel(_db, transactionID, ids);
                    if(!check)
                    {
                        this.TraceLog("Asset Retirement Final Approval DisposalDocUpload", $"{SessionUser.Current.Username} - DisposalDocUpload call done");
                        return Json(new { Result = "success", Type = "Upload", FilePath = filePaths, error = "" });
                    }
                    else
                    {
                        RetirementDataModel.RemoveModel(currentPageID);
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        string err = "Selected References No against some barcodes missed to give the disposal details please fill all the details";
                        this.TraceLog("Asset Retirement Final Approval DisposalDocUpload", $"{SessionUser.Current.Username} - DisposalDocUpload call Error");
                        return ErrorActionResult(err);
                        //return base.ErrorActionResult("Selected References No against some barcodes missed to give the disposal details please fill all the details");
                    }

                   
                }
                else
                {
                    RetirementDataModel.RemoveModel(currentPageID);
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
                    this.TraceLog("Asset Retirement Final Approval DisposalDocUpload", $"{SessionUser.Current.Username} - DisposalDocUpload call Error");
                    return ErrorActionResult(err);
                    // return base.ErrorActionResult("Upload excel not matched sample format . please upload sample format");
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
                TransferAssetDataModel.RemoveModel(currentPageID);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return base.ErrorActionResult(ex);


            }
        }
        public async Task<Tuple<string, DataTable>> ReadDocument(IFormFile files)
        {
            this.TraceLog("Asset Retirement Final Approval ReadDocument", $"{SessionUser.Current.Username} - ReadDocument call");
            string path = System.IO.Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/AssetRetirementImport");
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
            this.TraceLog("Asset Retirement Final Approval ReadDocument", $"{SessionUser.Current.Username} - ReadDocument call Done");
            return obj;

        }
        public ActionResult DisposalDocRemove(string[] fileNames, int currentPageID)
        {
            this.TraceLog("Asset Retirement Final Approval DisposalDocRemove", $"{SessionUser.Current.Username} - DisposalDocRemove call");
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


            this.TraceLog("Asset Retirement Final Approval DisposalDocRemove", $"{SessionUser.Current.Username} - DisposalDocRemove call Done");
            // Return an empty string to signify success.
            return Json(new { Result = "success", Type = "Remove", FilePath = "" });
        }
        public static string ValidateExcelSheet(DataTable table, List<string> dbColumns)
        {
           
            string retrunString = string.Empty;
            int outerCount = 0;

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

            return retrunString;

        }
        
        public static string ValidateValues(DataTable table)
        {
            string retrunString = string.Empty;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (string.IsNullOrEmpty(table.Rows[i][j]+"")) {
                        retrunString = "Some fields are have empty please check its";
                        return retrunString;
                    }
                }
            }
            return retrunString;
        }

        public static List<string> GetDBColumnName()
        {

            List<string> prop = new List<string>()
                            {
                                "Barcode",
                                "RetirementValue",
                                "RetirementReferencesNo",
                                "RetirementRemarks",
                                "ProceedOfSales",
                                "CostOfRemoval",
                                "RetirementTypeCode",
                                "RetirementDate"

                            };

            
            return prop;

        }

        [HttpPost]
        public IActionResult SingleApproval(IFormCollection data, string type, int currentPageID,string remarks)
        {
            try
            {

                string transID = data["id"];
                string historyID = data["historyID"];
                ApprovalHistoryTable wItem = ApprovalHistoryTable.GetWorkflowID(_db, int.Parse(historyID), (int)ApproveModuleValue.AssetRetirement);
                // ApprovalHistoryTable wItem = ApprovalHistoryTable.LastLevelData(_db,  (int)ApproveModuleValue.AssetRetirement,int.Parse(transID));
                RetirementDataModel model = RetirementDataModel.GetModel(currentPageID);
                if (model.Documents.Count > 0)
                {
                    foreach (var item in model.Documents)
                    {
                        DocumentTable document = new DocumentTable();
                        document.FileName = item.FileName;
                        document.DocumentName = item.DocumentName;
                        document.FilePath = item.FullPath;
                        document.FileExtension = item.FileExtension;
                        document.TransactionType = "AssetRetirementApproval";
                        document.ObjectKeyID = wItem.ApprovalHistoryID;
                        _db.Add(document);
                    }

                }

                if (string.Compare(type, "Approval") == 0)
                {
                    this.TraceLog("Asset Retirement Final Approval SingleApproval", $"{SessionUser.Current.Username} - Approve call");
                    var maxworkflowID = ApprovalHistoryTable.GetWorkflowDetailsworkflowMax(_db, wItem.ApprovalHistoryID);
                    var max = ApprovalHistoryTable.GetItem(_db, maxworkflowID);

                    //List<int> getUserApprovalList = PersonTable.GetUserRoleMappingDetails(_db, SessionUser.Current.UserID).Select(a => (int)a.ApprovalRoleID).ToList();
                    //if (getUserApprovalList.Contains(max.ApprovalRoleID))
                    //{
                    var result = AssetRetirementApprovalView.GetAllItems(_db).Where(a => a.UserID == SessionUser.Current.UserID && a.ApprovalHistoryID == wItem.ApprovalHistoryID).FirstOrDefault();
                    if (result.ApprovalRoleID == max.ApprovalRoleID)
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
                        var lineitemAsset = RetirementDataModel.GetModel(currentPageID);
                        if (lineitemAsset.LineItems.Count() > 0)
                        {
                            foreach (var line in lineitemAsset.LineItems.Select(a => a.Asset).ToList())
                            {

                                AssetTable OldAsset = AssetTable.GetItem(_db, line.AssetID);

                                OldAsset.StatusID = (int)StatusValue.Disposed;
                                OldAsset.DisposalReferenceNo = line.DisposalReferenceNo;
                                OldAsset.DisposalValue = line.DisposalValue;
                                OldAsset.DisposedRemarks = line.DisposedRemarks;
                                OldAsset.DisposedDateTime = line.DisposedDateTime;
                                OldAsset.ProceedofSales = line.ProceedofSales;
                                OldAsset.CostOfRemoval = line.CostOfRemoval;
                                OldAsset.DisposedDateTime = line.DisposedDateTime;
                                OldAsset.RetirementTypeID = line.RetirementTypeID;

                                var tranLine = lineitem.Where(a => a.AssetID == line.AssetID).FirstOrDefault();
                                tranLine.CostOfRemoval = line.CostOfRemoval;
                                tranLine.DisposalReferencesNo = line.DisposalReferenceNo;
                                tranLine.DisposalValue = line.DisposalValue;
                                tranLine.DisposalRemarks = line.DisposedRemarks;
                                tranLine.ProceedOfSales = line.ProceedofSales;
                                tranLine.DisposalDate = line.DisposedDateTime;
                                tranLine.RetirementTypeID = line.RetirementTypeID;


                            }
                        }
                        else
                        {
                            foreach (var line in lineitem)
                            {
                                AssetTable OldAsset = AssetTable.GetItem(_db, line.AssetID);

                                OldAsset.StatusID = (int)StatusValue.Disposed;
                                OldAsset.DisposalReferenceNo = line.DisposalReferencesNo;
                                OldAsset.DisposalValue = line.DisposalValue;
                                OldAsset.DisposedRemarks = line.DisposalRemarks;
                                OldAsset.DisposedDateTime = line.DisposalDate;
                                OldAsset.ProceedofSales = line.ProceedOfSales;
                                OldAsset.CostOfRemoval = line.CostOfRemoval;
                                //  OldAsset.DisposedDateTime = line.DisposedDateTime;
                                OldAsset.RetirementTypeID = line.RetirementTypeID;
                            }
                        }

                    }
                    else
                    {
                        wItem.StatusID = (int)StatusValue.Active;
                        wItem.LastModifiedDateTime = System.DateTime.Now;
                        wItem.LastModifiedBy = SessionUser.Current.UserID;
                        ///  wItem.Remarks = remarks;

                        var lineitem = TransactionLineItemTable.GetTransactionLineItems(_db, wItem.TransactionID);
                        var lineitemAsset = RetirementDataModel.GetModel(currentPageID);
                        var getmodeldate = lineitemAsset.LineItems.Select(a => a.Asset).ToList();
                        foreach (var line in lineitem)
                        {
                            var datas = (from b in getmodeldate where b.AssetID == line.AssetID select b).FirstOrDefault();
                            if (datas != null)
                            {
                                line.ProceedOfSales = datas.ProceedofSales;
                                line.CostOfRemoval = datas.CostOfRemoval;
                                line.DisposalDate = datas.DisposedDateTime;
                                line.DisposalReferencesNo= datas.DisposalReferenceNo;
                                line.DisposalRemarks = datas.DisposedRemarks;
                                line.DisposalValue = datas.DisposalValue;
                                line.RetirementTypeID= datas.RetirementTypeID;

                            }
                        }
                    }


                }
                else if (string.Compare(type, "Reject") == 0)
                {
                    this.TraceLog("Asset Retirement Final Approval SingleApproval", $"{SessionUser.Current.Username} - Reject call");
                    wItem.StatusID = (int)StatusValue.Rejected;
                    wItem.LastModifiedDateTime = System.DateTime.Now;
                    wItem.LastModifiedBy = SessionUser.Current.UserID;
                    wItem.Remarks = remarks;

                    var remainingUsersameLevel = ApprovalHistoryTable.SameLevelUserList(_db, wItem.ApprovalHistoryID, (int)ApproveModuleValue.AssetRetirement, wItem.TransactionID);
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
                    RetirementDataModel.RemoveModel(currentPageID);
                    this.TraceLog("Asset Retirement Final Approval SingleApproval", $"{SessionUser.Current.Username} - Reject call Done");
                    return Json(new { Result = "Success", message = " Rejected Successfully" });
                }
                _db.SaveChanges();
                RetirementDataModel.RemoveModel(currentPageID);
                this.TraceLog("Asset Retirement Final Approval SingleApproval", $"{SessionUser.Current.Username} - Approve call Done");
                return Json(new { Result = "Success", message = " Approved Successfully" });
            }
            catch (Exception ex)
            {
                return base.ErrorActionResult(ex);
            }
        }

        public async Task<ActionResult> DocumentUpload(IEnumerable<IFormFile> UploadDoc, int currentPageID)
        {
            try
            {
                this.TraceLog("Asset Retirement Final Approval DocumentUpload", $"{SessionUser.Current.Username} - call");
                // string absolutePath = DateTime.Now.ToString("yyyy_MM_dd");
                string rootPath = string.Concat(WebHostEnvironment.WebRootPath, "\\");
                string fullPath = System.IO.Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/TransferRetirementApproval");
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                //DocumentDataModel.RemoveModel(currentPageID);
                //DocumentDataModel detailsDocuments = DocumentDataModel.GetModel(currentPageID);
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
                        document.FullPath = fullPath.Replace(rootPath,"").Replace(" ", "");
                        document.FileExtension = fileExtension;
                        document.CurrentPageID = currentPageID;
                        model.Documents.Add(document);
                    }
                }
                this.TraceLog("Asset Retirement Final Approval DocumentUpload", $"{SessionUser.Current.Username} - call Done");
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
            this.TraceLog("Asset Retirement Final Approval DocumentRemove", $"{SessionUser.Current.Username} - call");
            RetirementDataModel model = RetirementDataModel.GetModel(currentPageID);
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
            this.TraceLog("Asset Retirement Final Approval DocumentRemove", $"{SessionUser.Current.Username} - call Done");
            // Return an empty string to signify success.
            return Content("");
        }
    }
}
