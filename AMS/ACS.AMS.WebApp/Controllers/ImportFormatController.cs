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
using DocumentFormat.OpenXml.Vml;

namespace ACS.AMS.WebApp.Controllers
{
    public class ImportFormatController : ACSBaseController
    {
        public ActionResult Index()
        {
            if (!base.HasRights(RightNames.ImportFormat, UserRightValue.View))
                return GotoUnauthorizedPage();

            base.TraceLog("Import Format Index", $"{SessionUser.Current.Username} -Import Format Index page requested");
            return PartialView();
        }

        public IActionResult _Index([DataSourceRequest] DataSourceRequest request, int currentPageID)
        {
            if (!base.HasRights(RightNames.ImportFormat, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");

            IQueryable<ImportFormatNewTable> result = ImportFormatNewTable.GetListOfFormatsForEntity(_db, null, null, SessionUser.Current.UserID);
            //var details = TransactionTable.GetUserImportTemplate(_db, SessionUser.Current.UserID);
            ////if (details.Result.Count() > 0)
            ////{
            //var ids = details.Result.AsQueryable();
            //var id = (from b in ids select b.ImportFormatID).ToList();
            //result = result.Where(a => id.Contains(a.ImportFormatID));
            //}
            // result=result.Where(a=>details.cont)
            var dsResult = request.ToDataSourceResult(result, "ImportFormat", "ImportFormatID");


            base.TraceLog("Import Format Index", $"{SessionUser.Current.Username} -Import Format Index page Data Fetch");
            return Json(dsResult);
        }
        
        public IActionResult Create()
        {
            if (!base.HasRights(RightNames.ImportFormat, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("Import Format Create", $"{SessionUser.Current.Username} -Import Format Create page requested");

            return PartialView();
        }
        
        [HttpPost]
        public IActionResult Create(IFormCollection data, ImportFormatNewTable item)
        {
            if (!base.HasRights(RightNames.ImportFormat, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");
            item.FormatExtension = ".xls";
            try
            {
                base.TraceLog("Import Format Create-post", $"{SessionUser.Current.Username} -Import Format Create-post page requested");
                if (ModelState.IsValid)
                {
                    item.CreatedBy = SessionUser.Current.UserID;
                    _db.Add(item);
                    string items = data["hdSelectedItemsIDS"];
                    string[] ColumnID = items.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string Columns = data["hdSelectedItems"];
                    string[] columnName = Columns.Split(new char[] { ',', '*' }, StringSplitOptions.RemoveEmptyEntries); int cnt = 1;
                    foreach (string str in ColumnID)
                    {
                        string dynamicColumn = "Dyn_";
                        ImportFormatLineItemNewTable excelLineItem = new ImportFormatLineItemNewTable();
                       
                        excelLineItem.ImportTemplateID = int.Parse(str);
                        //excelLineItem.IsDynamicColumn = false;
                        excelLineItem.DisplayOrderID = cnt;
                        excelLineItem.ImportFormat = item;
                        _db.Add(excelLineItem);
                       

                        cnt = cnt + 1;
                    }
                    _db.SaveChanges();
                    return PartialView("SuccessAction");
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
            return PartialView(item);
        }
        
        public IActionResult Edit(int id)
        {
            if (!base.HasRights(RightNames.ImportFormat, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("Import Format Edit", $"{SessionUser.Current.Username} -Import Format Edit page requested.id-{id}");

            ImportFormatNewTable flow = ImportFormatNewTable.GetImportFormat(_db, id);
            return PartialView(flow);
        }
        
        [HttpPost]
        public IActionResult Edit(IFormCollection data, ImportFormatNewTable item)
        {
            if (!base.HasRights(RightNames.ImportFormat, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");
            try
            {
                base.TraceLog("Import Format Edit-post", $"{SessionUser.Current.Username} -Import Format Edit Post requested");
                if (ModelState.IsValid)
                {
                    ImportFormatNewTable oldItem = ImportFormatNewTable.GetImportFormat(_db, item.ImportFormatID);
                    string columnsID = data["hdSelectedItemsIDS"];
                    string[] ColumnID = columnsID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string Columns = data["hdSelectedItems"];
                    string[] columnName = Columns.Split(new char[] { ',', '*' }, StringSplitOptions.RemoveEmptyEntries);
                    var datatoDelete = ImportFormatLineItemNewTable.GetImportFormatItems(_db, item.ImportFormatID);
                    
                    if (datatoDelete.Count() > 0)
                    {
                        ImportFormatLineItemNewTable.DeleteExistingFieldlineItem(_db, datatoDelete);
                    }
                       // _db.Remove(datatoDelete);

                    int cnt = 1;

                    foreach (string str in ColumnID)
                    {
                        string dynamicColumn = "Dyn_";
                        ImportFormatLineItemNewTable excelLineItem = new ImportFormatLineItemNewTable();
                        
                            excelLineItem.ImportTemplateID = int.Parse(str);
                            //excelLineItem.IsDynamicColumn = false;
                            excelLineItem.DisplayOrderID = cnt;
                            excelLineItem.ImportFormatID = item.ImportFormatID;
                            _db.Add(excelLineItem);
                        
                        cnt = cnt + 1;
                    }

                    DataUtilities.CopyObject(item, oldItem);
                    _db.SaveChanges();
                    base.TraceLog("Import Format Edit-post", $"{SessionUser.Current.Username} -Import Format Edit Post saved");
                    return PartialView("SuccessAction");
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
            return PartialView(item);
        }

        public IActionResult Details(int id)
        {
            if (!base.HasRights(RightNames.ImportFormat, UserRightValue.Details))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("Import Format Details", $"{SessionUser.Current.Username} -Import Format Details page requested.id-{id}");

            ImportFormatNewTable flow = ImportFormatNewTable.GetImportFormat(_db, id);
            return PartialView(flow);
        }

        public IActionResult Delete(int id)
        {
            if (!base.HasRights(RightNames.ImportFormat, UserRightValue.Delete))
                return RedirectToAction("UnauthorizedPage");

            var pageName = "ImportFormat";
            base.TraceLog("Delete", $"{SessionUser.Current.Username} - {pageName} Delete action requested.id-{id}");

            ImportFormatNewTable flow = ImportFormatNewTable.GetItem(_db, id);

            if (flow != null)
                flow.Delete();

            _db.SaveChanges();

            base.TraceLog("Delete", $"{pageName} details page deleted successfully {flow.GetPrimaryKeyFieldName()} {id}");
            return PartialView("SuccessAction", new { pageName = pageName });
        }

        public IActionResult GetdownloadDetails(int ImportFormatID)
        {
            var result = ImportFormatNewTable.GetImportFormat(_db, ImportFormatID);
            var columns = ImportFormatLineItemNewTable.GetColumns(_db, ImportFormatID);
            var list = ImportFormatLineItemNewTable.MergeColumnName(columns);
            IQueryable listOppLineData = Enumerable.Empty<string>().AsQueryable();

            return DisplayHelper.BarcodeExportToFile(listOppLineData, result.TamplateName, "EXCEL", result.TamplateName, list.AsQueryable(), false);
        }
    }
}
