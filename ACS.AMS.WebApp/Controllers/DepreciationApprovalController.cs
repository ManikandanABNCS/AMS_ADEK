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
using Azure.Identity;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using static SQLite.SQLite3;
using Telerik.SvgIcons;

namespace ACS.AMS.WebApp.Controllers
{
    public class DepreciationApprovalController : ACSBaseController
    {
       
        public ActionResult Index()
        {
            if (!base.HasRights(RightNames.DepreciationApproval, UserRightValue.View))
                return GotoUnauthorizedPage();

            base.TraceLog("Depreciation Approval Index", $"{SessionUser.Current.Username} -Depreciation Approval Index page requested");
            return PartialView();
        }
        public IActionResult _IndexDetails([DataSourceRequest] DataSourceRequest request, int currentPageID)
        {
            if (!base.HasRights(RightNames.Depreciation, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");

            var res = DepreciationTable.GetAllDepreciationApproval(_db, SessionUser.Current.CompanyID, SessionUser.Current.UserID);
            if (res.Count() > 0)
            {
                var dsResult = request.ToDataSourceResult(res);
                base.TraceLog("TransactionList Index", $"{SessionUser.Current.Username} -TransactionList Index page Data Fetch");
                return Json(dsResult);
            }
            else
            {
                return Json(new { });
            }

        }
        public IActionResult _lineItems([DataSourceRequest] DataSourceRequest request, int currentPageID, int transactionID)
        {
            if (!base.HasRights(RightNames.DepreciationApproval, UserRightValue.Details))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("Depreciation Approval _lineItems", $"{SessionUser.Current.Username} -Depreciation Approval _lineItems requested");
            var query = DepreciationLineItemTable.GetAllItems(_db).Where(a => a.Depreciation.DepreciationID == transactionID);

            var dsResult = request.ToDataSourceResult(query, "Depreciation", "DepreciationLineItemID");
            return Json(dsResult);

        }
        public IActionResult Details(int id,int depreciationID)
        {
            if (!base.HasRights(RightNames.DepreciationApproval, UserRightValue.Details))
                return RedirectToAction("UnauthorizedPage");
            IndexPageModel indexPage = new IndexPageModel();
            indexPage.EntityInstance = new BaseEntityObject();
            indexPage.EntityInstance.CurrentPageID = SessionUser.Current.GetNextPageID();
            indexPage.PageTitle = id + "";

            ViewBag.PeriodID = depreciationID;

            base.TraceLog("Depreciation Approval Details", $"{SessionUser.Current.Username} -Depreciation Approval Details page requested.id- {id}");
            return PartialView(indexPage);
        }
        [HttpPost]
        public IActionResult Approved(int id, int depreciationID, string status)
        {
            if (!base.HasRights(RightNames.DepreciationApproval, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");
            try
            {
                base.TraceLog("Depreciation Approval Approved", $"{SessionUser.Current.Username} -Depreciation Approval Approved page requested.id- {id},{depreciationID}");
                DepreciationTable item = DepreciationTable.GetDeprecation(_db, id, depreciationID,SessionUser.Current.CompanyID);
                if (status == "Depreciaton Undo")
                {
                    item.IsDeletedApproval = true;
                    item.DeletedApprovedBy = SessionUser.Current.UserID;
                    item.DeletedApprovedDate = System.DateTime.Now;
                    item.StatusID = (int)StatusValue.Deleted;
                }
                else
                {
                    item.DepreciationApprovedBy = SessionUser.Current.UserID;
                    item.DepreciationApprovedDate = DateTime.Now;
                    item.IsDepreciationApproval = true;
                    item.StatusID = (int)StatusValue.Active;
                }
                _db.SaveChanges();
                base.TraceLog("Depreciation Approval Approved", $"{SessionUser.Current.Username} -Depreciation Approval Approved page request done");
                ViewData["MessageSuffix"] = Language.GetString("RecordApprovedSuccessfully");
                return Json(new { Result = "Success", message = "Depreciation Approval Approved" });
                //return PartialView("SuccessAction");
                //return base.ErrorActionResult("Record Approved Successfully");
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
            return PartialView();
        }
        [HttpPost]
        public IActionResult Reject(int id, int depreciationID, string status)
        {
            if (!base.HasRights(RightNames.DepreciationApproval, UserRightValue.Delete))
                return RedirectToAction("UnauthorizedPage");
            try
            {
                base.TraceLog("Depreciation Approval Reject", $"{SessionUser.Current.Username} -Depreciation Approval Reject page requested.id- {id},{depreciationID}");
                DepreciationTable item = DepreciationTable.GetDeprecation(_db, id, depreciationID);
                DepreciationTable newItem = new DepreciationTable();
                if (item != null)
                {
                    if (status == "Depreciaton Undo")
                    {
                        newItem = item;
                        newItem.StatusID = (int)StatusValue.Active;
                        item.DepreciationApprovedBy = SessionUser.Current.UserID;
                        item.DepreciationApprovedDate = DateTime.Now;
                        item.IsDepreciationApproval = true;
                        newItem.IsDeletedApproval = false;
                        newItem.DeletedApprovedBy = null;
                        newItem.DeletedApprovedDate = null;
                        newItem.DeletedDoneBy = null;
                        newItem.DeletedDoneDate = null;
                        //  item.DeletedDoneBy = null;
                        DataUtilities.CopyObject(newItem, item);
                    }
                    else
                    {
                        newItem = item;
                        newItem.DeletedDoneBy = SessionUser.Current.UserID;
                        newItem.DeletedDoneDate = System.DateTime.Now;
                        newItem.StatusID = (int)StatusValue.Deleted;
                        newItem.IsDeletedApproval = true;
                        newItem.DeletedApprovedBy = SessionUser.Current.UserID;
                        newItem.DeletedApprovedDate = System.DateTime.Now;
                        DataUtilities.CopyObject(newItem, item);
                    }

                    _db.SaveChanges();
                    base.TraceLog("Depreciation Approval Reject", $"{SessionUser.Current.Username} -Depreciation Approval Reject page requested done");
                    ViewData["MessageSuffix"] = Language.GetString("RecordApprovedSuccessfully");
                    return Json(new { Result = "Success", message = "Depreciation Approval Reject" });
                    return PartialView("SuccessAction");
                    //return base.ErrorActionResult("Record Deleted Successfully");
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
            return PartialView();
        }
    }
}
