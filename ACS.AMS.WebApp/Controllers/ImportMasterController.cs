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
using Microsoft.EntityFrameworkCore;
namespace ACS.AMS.WebApp.Controllers
{
    public class ImportMasterController : ACSBaseController
    {
        private static IWebHostEnvironment WebHostEnvironment;
        public IActionResult Import()
        {
            if (!base.HasRights(RightNames.ImportMaser, UserRightValue.Create))
                return GotoUnauthorizedPage();
            base.TraceLog("Import Master Index", $"{SessionUser.Current.Username} -Import Master Index page requested");
            return PartialView();
        }
       
        //public IActionResult DisplayGrid(string appID, int excelID, string UploadPath)
        //{
        //    try
        //    {
               
        //        var tableField = ImportFormatLineItemTable.GetImportMasterForImportFormat(_db, excelID, appID);              
        //        var excelLineItem = tableField.OrderBy(a => a.DispalyOrderID).Select(a => a.ImportField).ToList();
              
               
              
        //    }
        //    catch (Exception ex)
        //    {
                
        //        return base.ValidationMessage("FileNotMappedWithTemplate,PleaseSelectCorrectFile");
        //    }
        //}

        public async Task<Tuple<string, DataTable>> ReadDocument(IFormFile files)
        {
            string path = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/ImportMaster");
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
                }
                else
                {
                    XSSFWorkbook workbooks = new XSSFWorkbook(stream);
                    excelTable = ImportExcel.Import(excelTable, workbooks, _db);
                }
            }

            Tuple<string, DataTable> obj = new Tuple<string, DataTable>(filePath, excelTable);
            return obj;

        }
    }
}
