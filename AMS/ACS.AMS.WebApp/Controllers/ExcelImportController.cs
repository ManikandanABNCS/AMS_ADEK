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
using MathNet.Numerics;
using Telerik.SvgIcons;
using System.Net;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data.SqlClient;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Fixed.Model.Objects;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.RegularExpressions;
//using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ACS.AMS.WebApp.Controllers
{
    public class ExcelImportController : ACSBaseController
    {
        private static IWebHostEnvironment WebHostEnvironment;
        public ExcelImportController(IWebHostEnvironment _environment)
        {
            WebHostEnvironment = _environment;
        }
        public IActionResult Read([DataSourceRequest] DataSourceRequest request, int currentPageID)
        {
            base.TraceLog("ExcelImport Read", $"{SessionUser.Current.Username} -Read requested.");
            ImportDataModel model = ImportDataModel.GetModel(currentPageID);
            DataTable dt = model.ImportData;

            return Json(dt.ToDataSourceResult(request));
        }

        public IActionResult Import()
        {
            if (!base.HasRights(RightNames.ExcelImport, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("ExcelImport Import", $"{SessionUser.Current.Username} -Import requested.");
            IndexPageModel indexPage = new IndexPageModel();
            indexPage.EntityInstance = new BaseEntityObject();
            indexPage.EntityInstance.CurrentPageID = SessionUser.Current.GetNextPageID();
            ImportDataModel.RemoveModel(indexPage.EntityInstance.CurrentPageID);

            base.TraceLog("Import data Index", $"{SessionUser.Current.Username} -Import page requested");
            return PartialView(indexPage);
        }

        public IActionResult DisplayGrid(int appID, int excelID, int currentPageID)
        {
            DataTable dt = new DataTable();
            try
            {
                base.TraceLog("ExcelImport DisplayGrid", $"{SessionUser.Current.Username} -DisplayGrid requested.");
                ImportDataModel model = ImportDataModel.GetModel(currentPageID);
                dt = model.ImportData;
                ViewBag.CurrentPageID = currentPageID;
                return PartialView(dt);
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
            return PartialView(dt);
        }

        public async Task<ActionResult> MasterUpload(IFormFile attachments, int appPageID, int templateID, int currentPageID)
        {
            try
            {
                base.TraceLog("ExcelImport MasterUpload", $"{SessionUser.Current.Username} -MasterUpload requested.");
                var filePath = await ReadDocument(attachments,appPageID);
                string filePaths = filePath.Item1;
                DataTable dt = filePath.Item2;
                var tableField = ImportFormatLineItemNewTable.GetImportMasterForImportFormat(_db, templateID, appPageID);

                var excelLineItem = tableField.OrderBy(a => a.DispalyOrderID).Select(a => a.ImportField).ToList();
                var mandtoryFields = tableField.Where(a => a.IsMandatory == true).Select(a => a.ImportField).ToList();
                var UniqueFields = tableField.Where(a => a.IsUnique == true);

                string validationColumnMisMatch = ValidateExcelSheet(dt, excelLineItem);
                if (string.IsNullOrEmpty(validationColumnMisMatch))
                {
                    string validationMandatory = ValidateMandatory(dt, mandtoryFields);
                    string validationUnique = ValidateUnique(dt, UniqueFields);

                    if (string.IsNullOrEmpty(validationMandatory) && string.IsNullOrEmpty(validationUnique))
                    {
                        ImportDataModel data = ImportDataModel.GetModel(currentPageID);

                        if (dt.Rows.Count == 0)
                        {
                            ImportDataModel.RemoveModel(currentPageID);
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            
                            string err = "Excel there is no data please fill the data and import";
                            return Content(err);
                        }
                        else
                        {
                            if (appPageID == (int)EntityValues.AssetTable)
                            {
                                if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.AssetApprovalBasedOnWorkFlow))
                                {
                                    bool validation = ImportFormatLineItemNewTable.ValidateAsset(_db, templateID, appPageID);

                                    if (validation)
                                    {
                                        List<string> Categorylist = dt.Rows.OfType<DataRow>().Select(dr => (string)dr["CategoryCode"]).Distinct().ToList();
                                        List<string> Locationlist = dt.Rows.OfType<DataRow>().Select(dr => (string)dr["LocationCode"]).Distinct().ToList();
                                        var categoryResult = System.String.Join(",", Categorylist);
                                        var locationResult = System.String.Join(",", Locationlist);

                                        var validationError = TransactionTable.GetBulkAssetResult(AMSContext.CreateNewContext(), locationResult, categoryResult);//, errorID, errorMsg);
                                                                                                                                                                 // string errorMsg = validationError.Result.ErrorMsg;
                                        if (!string.IsNullOrEmpty(validationError.Result))
                                        {
                                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                            return base.ErrorActionResult(validationError.Result); //base.ErrorActionResult(errorMsg);
                                        }

                                    }
                                }
                            }

                            data.ImportData = dt;
                            base.TraceLog("ExcelImport MasterUpload", $"{SessionUser.Current.Username} -MasterUpload requested done");
                            return Json(new { Result = "success", Type = "Upload", FilePath = filePaths, error = "" });
                        }
                    }
                    else
                    {
                        string err = string.Empty;
                        if (!string.IsNullOrEmpty(validationUnique))
                        {
                            err = validationUnique + " These Values Have been Duplicated in excel file";
                        }
                        if (!string.IsNullOrEmpty(validationMandatory))
                        {
                            err += ',' + validationMandatory + " Has some mandatory Field Missing in excel file";
                        }
                        base.TraceLog("ExcelImport MasterUpload", $"{SessionUser.Current.Username} -MasterUpload requested Error");
                        ImportDataModel.RemoveModel(currentPageID);
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        // err = "Excel there is no data please fill the data and import";
                        return Content(err);
                    }
                }
                else
                {
                    string err = string.Empty;
                    if (!string.IsNullOrEmpty(validationColumnMisMatch))
                    {
                        err = validationColumnMisMatch;
                    }
                    ImportDataModel.RemoveModel(currentPageID);
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    base.TraceLog("ExcelImport MasterUpload", $"{SessionUser.Current.Username} -MasterUpload requested Error");
                    // string err = "Excel there is no data please fill the data and import";
                    return Content(err);
                }
            }
            catch (Exception ex)
            {
                //AppErrorLogTable.SaveException(WasNowContext.CreateNewContext(), ex);
                ErrorActionResult(ex);
                string err = string.Empty;

                ImportDataModel.RemoveModel(currentPageID);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                err = ex.ToString();
                return Content(err);
                //return Json(new { Result = "false", Type = "Upload", FilePath = "", error = ex.Message });
            }
        }

        public async Task<Tuple<string, DataTable>> ReadDocument(IFormFile files,int appPageID)
        {
            base.TraceLog("ExcelImport ReadDocument", $"{SessionUser.Current.Username} -ReadDocument requested");
            string path = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/ExcelImport");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var newFileName = Guid.NewGuid().ToString() + files.FileName;
            string fileName = Path.GetFileName(files.FileName);
            string NameTrimmed = System.String.Concat(newFileName.Where(c => !Char.IsWhiteSpace(c)));
            string filePath = Path.Combine(path, NameTrimmed);
           
            DataTable excelTable = new DataTable();

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                files.CopyTo(stream);
                stream.Position = 0;
                string FileName = Path.GetExtension(filePath);
                string rootFolderName = WebHostEnvironment.WebRootPath;
                string sign = string.Empty;
                if (appPageID == (int)EntityValues.custodianTable)
                {
                    sign = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/SignaturePath");
                }
                if (appPageID == (int)EntityValues.CategoryTable)
                {
                    sign = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/UploadImage/Category");
                }
                if (appPageID == (int)EntityValues.ProductTable)
                {
                    sign = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/UploadImage/Product");
                }
                if (!string.IsNullOrEmpty(sign))
                {
                    if (!Directory.Exists(sign))
                    {
                        Directory.CreateDirectory(sign);
                    }
                }

                excelTable =  ImportExcelTelerik.Import(excelTable, stream, _db, sign, rootFolderName,appPageID);

                //if (FileName == ".xls")
                //{
                //    HSSFWorkbook workbook = new HSSFWorkbook(stream);
                //    excelTable = ImportExcel.Import(excelTable, workbook, _db);
                //}
                //else
                //{
                //    XSSFWorkbook workbooks = new XSSFWorkbook(stream);
                //    excelTable = ImportExcel.Import(excelTable, workbooks, _db);
                //}
            }
            excelTable.Rows[0].Delete();
            excelTable.AcceptChanges();
            Tuple<string, DataTable> obj = new Tuple<string, DataTable>(filePath, excelTable);
                 base.TraceLog("ExcelImport ReadDocument", $"{SessionUser.Current.Username} -ReadDocument request done");
            return obj;

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
                    retrunString = columnName + " Column mismatch, please Check it";
                    return retrunString;
                }
            }

            return retrunString;

        }
        public static string ValidateMandatory(DataTable table, List<string> mandatoryfields)
        {
            string retrunString = string.Empty;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    string columnName = table.Columns[j].ColumnName.ToString();
                    var valid = mandatoryfields.Where(a => a.Equals(columnName)).FirstOrDefault();
                    if (valid != null)
                    {
                        if (string.IsNullOrEmpty(table.Rows[i][j] + ""))
                        {
                            retrunString += columnName + ",";
                        }
                    }
                }
            }
            return retrunString;
        }
        
        public static string ValidateUnique(DataTable table, IQueryable<ImportTemplateModel> uniquefields)
        {
            string dupValue = string.Empty;

            foreach (var availField in uniquefields)
            {
                var duplicateValues = (from row in table.AsEnumerable()
                                       let ID = row.Field<string>(availField.ImportField)
                                       group row by new { ID } into grp
                                       where grp.Count() > 1
                                       select new
                                       {
                                           DupID = grp.Key.ID,
                                           count = grp.Count(),
                                           field = availField.ImportField
                                       }).ToList();

                foreach (var dup in duplicateValues)
                {
                    dupValue += string.IsNullOrEmpty(dupValue) ? dup.DupID : (dupValue.LastIndexOf("\n") == dupValue.Length - 1) ? Environment.NewLine + dup.DupID : "," + dup.DupID;
                }

                if (duplicateValues.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dupValue))
                    {
                        dupValue += " In " + duplicateValues.FirstOrDefault().field + Environment.NewLine;
                    }
                }
            }

            return dupValue;
        }

        public ActionResult MasterRemove(string[] attachments, int currentPageID)
        {
            base.TraceLog("ExcelImport MasterRemove", $"{SessionUser.Current.Username} -MasterRemove request");
            ImportDataModel model = ImportDataModel.GetModel(currentPageID);
            if (attachments != null)
            {
                DataTable dt = new DataTable();
                if (model.ImportData.Columns.Count > 0)
                {
                    TransferAssetDataModel.RemoveModel(currentPageID);
                }
            }
            return Json(new { Result = "success", Type = "Remove", FilePath = "" });
        }

        [HttpPost]
        public IActionResult Import(IFormCollection data)
        {
            try
            {
                base.TraceLog("ExcelImport Import", $"{SessionUser.Current.Username} -Import request");
                string fileID = data["ImportFormatID"];
                string master = data["ImportMaster"];
                string currentPageID = data["CurrentPageID"];

                string filePath = data["UploadedFilePath"];
                string UploadDocPath = data["DocUploadPath"];
                string rootPath = WebHostEnvironment.WebRootPath;
                string result = SaveAllImportDetails(_db, (string)filePath, int.Parse(master), int.Parse(fileID), int.Parse(currentPageID), false, rootPath, UploadDocPath);
                
                if (!string.IsNullOrEmpty(result))
                {
                    return ErrorActionResult(Language.GetString(result.ToString()));
                }
                else
                {
                    string url = "/ExcelImport/Import";
                    ViewData["URL"] = url;
                    return PartialView("SuccessAction");
                }
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }

        public string SaveAllImportDetails(AMSContext _db, string filePath, int appID, int excelFromatID, int currentPageID, bool fileVaildation,string rootpath,string UploadDocPath="")
        {
            base.TraceLog("ExcelImport SaveAllImportDetails", $"{SessionUser.Current.Username} -SaveAllImportDetails request");
            string parentTable = string.Empty;
            string returnResult = string.Empty;
            string validateMSg = string.Empty;
            string validate = string.Empty;
            ImportDataModel model = ImportDataModel.GetModel(currentPageID);
            DataTable dt = model.ImportData;
            var formatDetails = ImportFormatNewTable.GetImportFormat(_db, excelFromatID);

            if (formatDetails != null)
            {
                parentTable = formatDetails.Entity.QueryString;
                if (!string.IsNullOrEmpty(parentTable))
                {
                    if (appID == (int)EntityValues.ImportAssetTransfer)
                    {
                        List<string> Locationlist = dt.Rows.OfType<DataRow>().Select(dr => (string)dr["ToLocationCode"]).Distinct().ToList();
                       
                        foreach (var loc in Locationlist)
                        {
                            var selectedRows = dt.AsEnumerable()
                                     .Where(row => row.Field<string>("ToLocationCode") == loc);
                            DataTable selectedRowsTable = selectedRows.CopyToDataTable();
                            List<string> Barcodelist = selectedRowsTable.Rows.OfType<DataRow>().Select(dr => (string)dr["Barcode"]).Distinct().ToList();

                            var BarcodeResult = System.String.Join(",", Barcodelist);
                            DataTable clonedTable = selectedRowsTable.Clone();
                            DataRow row = clonedTable.NewRow();
                            if (clonedTable.Columns.Contains("Barcode"))
                            {
                                row["Barcode"] = BarcodeResult;
                            }
                            if (clonedTable.Columns.Contains("ToLocationCode"))
                            {
                                row["ToLocationCode"] = loc;
                            }
                            if (clonedTable.Columns.Contains("ToCustodianCode"))
                            {
                                string custodian = selectedRowsTable.Rows.OfType<DataRow>().Select(dr => (string)dr["ToCustodianCode"]).FirstOrDefault();
                                row["ToCustodianCode"] = custodian;
                            }
                            if (clonedTable.Columns.Contains("ToDepartmentCode"))
                            {
                                string department = selectedRowsTable.Rows.OfType<DataRow>().Select(dr => (string)dr["ToDepartmentCode"]).FirstOrDefault();
                                row["ToDepartmentCode"] = department;
                            }
                            if (clonedTable.Columns.Contains("ToSectionCode"))
                            {
                                string section = selectedRowsTable.Rows.OfType<DataRow>().Select(dr => (string)dr["ToSectionCode"]).FirstOrDefault();
                                row["ToSectionCode"] = section;
                            }
                            if (clonedTable.Columns.Contains("TransferRemarks"))
                            {
                                string remarks = selectedRowsTable.Rows.OfType<DataRow>().Select(dr => (string)dr["TransferRemarks"]).FirstOrDefault();
                                row["TransferRemarks"] = remarks;
                            }
                            if (clonedTable.Columns.Contains("ToAssetConditionCode"))
                            {
                                string toAssetCondition = selectedRowsTable.Rows.OfType<DataRow>().Select(dr => (string)dr["ToAssetConditionCode"]).FirstOrDefault();
                                row["ToAssetConditionCode"] = toAssetCondition;
                            }
                            if (clonedTable.Columns.Contains("TransferTypeCode"))
                            {
                                string typecode = selectedRowsTable.Rows.OfType<DataRow>().Select(dr => (string)dr["TransferTypeCode"]).FirstOrDefault();
                                row["TransferTypeCode"] = typecode;
                            }
                            if (clonedTable.Columns.Contains("TransferDate"))
                            {
                                string transferDate = selectedRowsTable.Rows.OfType<DataRow>().Select(dr => (string)dr["TransferDate"]).FirstOrDefault();
                                row["TransferDate"] = transferDate;
                            }
                            if (clonedTable.Columns.Contains("L1LocationCode"))
                            {
                                string L1LocationCode = selectedRowsTable.Rows.OfType<DataRow>().Select(dr => (string)dr["L1LocationCode"]).FirstOrDefault();
                                row["L1LocationCode"] = L1LocationCode;
                            }
                            if (clonedTable.Columns.Contains("L2LocationCode"))
                            {
                                string L2LocationCode = selectedRowsTable.Rows.OfType<DataRow>().Select(dr => (string)dr["L2LocationCode"]).FirstOrDefault();
                                row["L2LocationCode"] = L2LocationCode;
                            }
                            clonedTable.Rows.Add(row);
                            validateMSg = SaveAssetImportMasters(_db, formatDetails.ImportTemplateTypeID, clonedTable, appID, parentTable, 
                                (int)formatDetails.ImportTemplateTypeID, excelFromatID, rootpath);
                            if (!string.IsNullOrEmpty(validateMSg))
                            {
                                validate += validateMSg + ',';
                            }
                        }

                        returnResult = RemoveLastComma(validate);
                    }
                    else
                    {
                        returnResult = SaveAssetImportMasters(_db, formatDetails.ImportTemplateTypeID, dt, appID, parentTable, (int)formatDetails.ImportTemplateTypeID, excelFromatID, rootpath, UploadDocPath);
                        if (string.IsNullOrEmpty(returnResult))
                        {
                            if (appID == (int)EntityValues.AssetTable)
                            {
                                if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.AssetApprovalBasedOnWorkFlow))
                                {
                                    if (formatDetails.ImportTemplateTypeID == 1)
                                    {
                                        List<string> BarcodeList = dt.Rows.OfType<DataRow>().Select(dr => (string)dr["Barcode"]).ToList();
                                        if (BarcodeList.Count() > 0)
                                        {
                                            TransactionTable.SaveTransactiondata(_db, BarcodeList, SessionUser.Current.UserID, (int)ApproveModuleValue.AssetAddition, CodeGenerationHelper.GetNextCode("AssetAddition"));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return returnResult;
        }

        public static string SaveAssetImportMasters(AMSContext _db, int importTypeID, DataTable importDataTable, int appID, string tableName, int importExcelFormatTypeID, int templateID,string rootPath,string UploadDocPath="",string uploadedFileName="")
        {           
            int checkCnt = 0;
            string resultStatus = string.Empty;
            string duplicateValues = string.Empty;

            //data import
            using (SqlConnection cn = new SqlConnection(AMSContext.ConnectionString))
            {
                DataSet dataSet = new DataSet();
                DataTable dataTable = new DataTable();
                cn.Open();
                //var trans = cn.BeginTransaction();
                bool transactionSuccess = true;

                try
                {
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = tableName;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                  //  cmd.Transaction = trans;
                    DataTable dt = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    var importTempalteDetails = ImportTemplateNewTable.GetImportTemplateFields(_db, appID, importExcelFormatTypeID);
                    bool signature = false;
                    string signaturePath = string.Empty;
                    string personCode = string.Empty;
                    if(appID==(int)EntityValues.custodianTable)
                    {
                        signature = true;
                    }

                    foreach (DataRow dr in importDataTable.Rows)
                    {
                        cmd.Parameters.Clear();

                        foreach (var column in importTempalteDetails)
                        {
                            string importField = column.ImportField;
                            string columnvalue = null;
                            DataColumnCollection excelColumns = importDataTable.Columns;
                            if (excelColumns.Contains(column.ImportField))
                            {
                                columnvalue = dr[column.ImportField] + "".Trim();
                              
                            }

                            cmd.Parameters.AddWithValue("@" + importField, columnvalue);
                        }

                        cmd.Parameters.AddWithValue("@ImportTypeID", importTypeID);
                        cmd.Parameters.AddWithValue("@UserID", SessionUser.Current.UserID);
                       
                        if ((int)EntityValues.AssetTable == appID )
                        {
                            cmd.Parameters.AddWithValue("@LanguageID", SessionUser.Current.LanguageID);
                            cmd.Parameters.AddWithValue("@CompanyID", 1003);
                            cmd.Parameters.AddWithValue("@UploadedDocumentPath", UploadDocPath);
                            cmd.Parameters.AddWithValue("@ImportExcelFileName", uploadedFileName);
                        }

                        try
                        {
                            dt.Clear();
                            adapter.Fill(dt);
                        }
                        catch (Exception ex)
                        {
                            var errorID = ApplicationErrorLogTable.SaveException(ex, "SqlBulkCopy AssetApprovalModel Upload-Web, At record: " + checkCnt);
                            transactionSuccess = false;
                            //    trans.Rollback();
                            resultStatus = resultStatus + $"Error ID: {errorID}" + Environment.NewLine;
                            //return ex.Message;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow newDataRow in dt.Rows)
                            {
                                var err = newDataRow[0] + "";
                                if (!string.IsNullOrWhiteSpace(err))
                                {
                                    resultStatus = resultStatus + err + Environment.NewLine;

                                    transactionSuccess = false;
                                    // trans.Rollback();
                                    break;
                                }
                                else
                                {
                                }
                            }
                        }

                        //if (transactionSuccess == false) break;
                    }

                    if (transactionSuccess)
                    {
                      //  trans.Commit();
                       // trans.Dispose();
                        if (signature)
                        {                            
                            foreach (DataRow dr in importDataTable.Rows)
                            {
                                signaturePath = dr["SignatureImage"] + "".Trim();
                                personCode= dr["PersonCode"] + "".Trim();
                                if(!string.IsNullOrEmpty(signaturePath))
                                {
                                    var persons = PersonTable.GetPerson(_db, personCode);
                                    if (persons != null)
                                    {
                                        var newpath = RenameFileWithFileInfoClass(signaturePath, string.Concat(persons.PersonCode, '_', persons.PersonID.ToString()), rootPath, appID);
                                        persons.SignaturePath = newpath;
                                        _db.SaveChanges();
                                    }
                                }
                            }

                        }
                        if (appID == (int)EntityValues.CategoryTable || appID == (int)EntityValues.ProductTable)
                        {
                            bool columnExists = importDataTable.Columns
                             .Cast<DataColumn>()
                             .Any(col => string.Equals(col.ColumnName, "CatalogueImage", StringComparison.OrdinalIgnoreCase));
                            if (columnExists)
                            {
                                string columnName = "CategoryCode";
                                if(appID == (int)EntityValues.ProductTable)
                                {
                                    columnName = "ProductCode";
                                }
                                foreach (DataRow dr in importDataTable.Rows)
                                {
                                    signaturePath = dr["CatalogueImage"] + "".Trim();
                                    personCode = dr[columnName] + "".Trim();
                                    if (!string.IsNullOrEmpty(signaturePath))
                                    {
                                        if (appID == (int)EntityValues.CategoryTable)
                                        {
                                            var persons = CategoryTable.GetCateory(_db, personCode);
                                            if (persons != null)
                                            {
                                                var newpath = RenameFileWithFileInfoClass(signaturePath, string.Concat(persons.CategoryCode, '_', persons.CategoryID.ToString()), rootPath,appID);
                                                persons.CatalogueImage = newpath;
                                                _db.SaveChanges();
                                            }
                                        }
                                        if (appID == (int)EntityValues.ProductTable)
                                        {
                                            var persons = ProductTable.GetProduct(_db, personCode);
                                            if (persons != null)
                                            {
                                                var newpath = RenameFileWithFileInfoClass(signaturePath, string.Concat(persons.ProductCode, '_', persons.ProductID.ToString()), rootPath, appID);
                                                persons.CatalogueImage = newpath;
                                                _db.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (appID==(int)EntityValues.AssetTable)
                        {
                            if(!string.IsNullOrEmpty(UploadDocPath))
                            {
                                AssetimportDocumentHistoryTable document = new AssetimportDocumentHistoryTable();
                                document.UploadDocPath = UploadDocPath;
                                document.ImportType = importTypeID+"";
                                document.StatusID = (int)StatusValue.Active;
                                document.UploadedBy = SessionUser.Current.UserID;
                                document.UploadedDateTime = System.DateTime.Now;
                                _db.Add(document);
                                _db.SaveChanges();
                            }
                            

                        }
                    }
                }
                catch (Exception ex)
                {
                    //sqlTrans.Rollback();
                    ApplicationErrorLogTable.SaveException(ex, "SqlBulkCopy AssetApprovalModel Upload" + "Web");
                    return ex.Message;
                }
                finally
                {
                    cn.Dispose();
                    cn.Close();
                }
                if (!string.IsNullOrEmpty(duplicateValues))
                {
                    resultStatus = duplicateValues + "Have been Already Exist";
                    return resultStatus;
                }

                return resultStatus;
            }
        }

        public static string RenameFileWithFileInfoClass(string oldFile, string newFile,string rootPath,int appID)
        {
            var oldPath = Path.Combine(rootPath, oldFile);
            var oldfiles = Path.GetFileName(oldPath);
            var extension = Path.GetExtension(oldPath);
            var folderName = string.Empty;
            if (appID == (int)EntityValues.custodianTable)
            {
                folderName = "FileStoragePath/SignaturePath";
            }
            if (appID == (int)EntityValues.CategoryTable)
            {
                folderName = "FileStoragePath/UploadImage/Category";
                
            }
            if (appID == (int)EntityValues.ProductTable)
            {
                folderName = "FileStoragePath/UploadImage/Product";
               
            }
            string result = Regex.Replace(newFile, @"[^a-zA-Z0-9\s]", "");
            var newphysicalpth = System.IO.Path.Combine(folderName, string.Concat(result.Replace(" ", ""), extension));
       //     var newphysicalpth = Path.Combine(folderName, string.Concat(newFile.Replace(" ",""), extension));
            var newpath = Path.Combine(WebHostEnvironment.WebRootPath, newphysicalpth);
            var file = new FileInfo(oldPath);
            file.MoveTo(newpath);
            return newphysicalpth;
        }
        public static string RemoveLastComma(string input)
        {
            int lastIndex = input.LastIndexOf(',');
            if (lastIndex >= 0)
            {
                input = input.Remove(lastIndex, 1);
            }
            return input;
        }

        public async Task<ActionResult> DocumentUpload(IFormFile UploadDoc, int currentPageID)
        {
            try
            {
                this.TraceLog("Excel DocumentUpload", $"{SessionUser.Current.Username} - DocumentUpload call");
                // string absolutePath = DateTime.Now.ToString("yyyy_MM_dd");
                string rootPath = string.Concat(WebHostEnvironment.WebRootPath, "\\");
                string fullPath = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/ImportExcelAssetData");
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                //DocumentDataModel.RemoveModel(currentPageID);
                //DocumentDataModel detailsDocuments = DocumentDataModel.GetModel(currentPageID);
               // TransferAssetDataModel model = TransferAssetDataModel.GetModel(currentPageID);

                // The Name of the Upload component is "files".
                if (UploadDoc != null)
                {
                        var fileContent = ContentDispositionHeaderValue.Parse(UploadDoc.ContentDisposition);

                       
                        string fileExtension = System.IO.Path.GetExtension(Path.GetFileName(fileContent.FileName.ToString().Trim('"').Replace(" ", "")));
                        string time = ComboBoxHelper.RemoveSpecialCharacters(String.Format("{0:" + CultureHelper.DateTimeFormatForGrid + "}", DateTime.Now));
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(fileContent.FileName.ToString().Trim('"'));
                       

                       
                        string newFileName = fileName + "" + time + "" + fileExtension; //Guid.NewGuid() + "" + fileExtension;
                    fullPath = Path.Combine(fullPath, newFileName).Replace(" ", "");

                        using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        {
                            await UploadDoc.CopyToAsync(fileStream);
                        }
                    TempData["UploadedFile"] = fullPath;
                    var physicalPath = fullPath.Replace(rootPath, "");
                    base.TraceLog("Excel", $"{SessionUser.Current.Username} -  Document request done");
                    return Json(new { DocumentPath = physicalPath, fileName = newFileName });

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
    }
}