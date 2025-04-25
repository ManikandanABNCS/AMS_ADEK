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
using Microsoft.CodeAnalysis;
using static SQLite.SQLite3;

namespace ACS.AMS.WebApp.Controllers
{
    public class TransactionListController : ACSBaseController
    {
        [HttpGet]
        public virtual IActionResult  Index()
        {
            if (!base.HasRights(RightNames.TransactionList, UserRightValue.View))
                return GotoUnauthorizedPage();
            string pageName = "TransactionList";
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
        public IActionResult _Index([DataSourceRequest] DataSourceRequest request)
        {
            if (!base.HasRights(RightNames.TransactionList, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");

          //  List<int> userBasedLocation = PersonTable.GetUserBasedLocationList(_db, SessionUser.Current.UserID).Select(a => a.LocationID).ToList();

            IQueryable<TransactionView> result = TransactionView.GetAllItems(_db);
            //if(userBasedLocation.Count()>0)
            //{
            //    var line = TransactionLineItemTable.GetAllItems(_db).Where(a => userBasedLocation.Contains((int)a.FromLocationID) || userBasedLocation.Contains((int)a.ToLocationID)).Select(a => a.TransactionID).ToList();
            var line = TransactionLineItemTable.GetUserBasedTransactionID(_db, SessionUser.Current.UserID, null);
            if (line.Count() > 0)
            {
                result = result.Where(a => line.Contains(a.TransactionID));
                // var dsResult = result.ToDataSourceResult(request);
                var dsResult = request.ToDataSourceResult(result, "TransactionList", "TransactionID");
                base.TraceLog("TransactionList Index", $"{SessionUser.Current.Username} -TransactionList Index page Data Fetch");
                return Json(dsResult);
            }
           else
            {
                return Json(new { });
            }


           
         
            //return Json("");
        }
        public ActionResult Details(int id, string pageName)
        {
            if (!base.HasRights(RightNames.TransactionList, UserRightValue.Details))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("TransactionList Details", $"{SessionUser.Current.Username} -TransactionList Details page request");
            int currentPageId = SessionUser.Current.GetNextPageID();
            ViewBag.CurrentPageID = SessionUser.Current.GetNextPageID();
            ViewBag.ApprovalHistoryID = id;
            return PartialView();

        }
        public IActionResult _LineItemindex([DataSourceRequest] DataSourceRequest request, int id)
        {
            // var query = uniformRequestDetailsDataModel;
            //var transID = ApprovalHistoryTable.GetItem(_db, id);
            var query = TransactionLineItemViewForTransfer.GetAllItems(_db).Where(a => a.TransactionID == id);

            var dsResult = query.ToDataSourceResult(request);

            // var dsResult = request.ToDataSourceResult(query.AsQueryable(), "UniformRequestUniformItemEntry", "UniformRequestLineItemID");
            this.TraceLog("TransactionList _LineItemindex", $"{SessionUser.Current.Username} - _LineItemindex page Data Fetch");

            return Json(dsResult);
        }
        public IActionResult _LineItemApproval([DataSourceRequest] DataSourceRequest request, int id)
        {
            // var query = uniformRequestDetailsDataModel;
            var transID = TransactionTable.GetItem(_db, id);
            var history = ApprovalHistoryTable.GetTransaction(_db, id, (int)transID.TransactionTypeID);
            var query = ApprovalHistoryView.GetAllItems(_db, true).Where(a => a.TransactionID == transID.TransactionID && a.ApproveModuleID == history.ApproveModuleID).OrderBy(a => a.CreatedDateTime).ThenBy(a => a.OrderNo);

            var dsResult = query.ToDataSourceResult(request);

            // var dsResult = request.ToDataSourceResult(query.AsQueryable(), "UniformRequestUniformItemEntry", "UniformRequestLineItemID");
            this.TraceLog("TransactionList _LineItemApproval", $"{SessionUser.Current.Username} - _LineItemApproval page Data Fetch");

            return Json(dsResult);
        }
        public ActionResult DownloadFile(string fileName,int id)
        {
            this.TraceLog("TransactionList DownloadFile", $"{SessionUser.Current.Username} - DownloadFile request");
            //Build the File Path.
            var history=ApprovalHistoryTable.GetApproval(_db, id);
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
            return Json(new { Result = "Success", type=document.TransactionType,fileName=document.FileName });
        }
    }
}
