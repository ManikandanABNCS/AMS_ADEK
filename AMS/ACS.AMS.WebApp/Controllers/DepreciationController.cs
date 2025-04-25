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

namespace ACS.AMS.WebApp.Controllers
{
    public class DepreciationController : ACSBaseController
    {
       
        public ActionResult Index()
        {
            if (!base.HasRights(RightNames.Depreciation, UserRightValue.Create))
                return GotoUnauthorizedPage();

            base.TraceLog("Depreciation Index", $"{SessionUser.Current.Username} -Depreciation Index page requested");
            return PartialView();
        }
        public IActionResult _Index([DataSourceRequest] DataSourceRequest request, int currentPageID)
        {
            if (!base.HasRights(RightNames.Depreciation, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");

            var res = DepreciationTable.GetAllPeriodWithData(_db, SessionUser.Current.CompanyID);
            if (res.Count() > 0)
            {
                var dsResult = request.ToDataSourceResult(res);
                base.TraceLog("Depreciation Index", $"{SessionUser.Current.Username} -Depreciation Index page Data Fetch");
                return Json(dsResult);
            }
            else
            {
                return Json(new { });
            }

        }
        public IActionResult _lineItems([DataSourceRequest] DataSourceRequest request,int currentPageID, int transactionID)
        {
            if (!base.HasRights(RightNames.Depreciation, UserRightValue.Details))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("Depreciation _lineItems", $"{SessionUser.Current.Username} -_lineItems request.transactionid-{transactionID}");
            var period = DepreciationTable.GetDeprecation(_db, transactionID, null, SessionUser.Current.CompanyID);

            var query = DepreciationLineItemTable.GetAllItems(_db).Where(a => a.DepreciationID == period.DepreciationID
             );

            var dsResult = request.ToDataSourceResult(query, "Depreciation", "DepreciationLineItemID");
            return Json(dsResult);

        }
        [HttpPost]
        public IActionResult Index(IFormCollection data,PeriodModel item)
        {
            if (!base.HasRights(RightNames.Depreciation, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");

            try
            {
                base.TraceLog("Depreciation Index-post", $"{SessionUser.Current.Username} -Index post  request");
                if (!string.IsNullOrEmpty(data["PeriodID"]))
                {
                    string checkdItems = data["PeriodID"];
                    string[] arrindate = checkdItems.Split(',');
                    int[] PeriodID = Array.ConvertAll(arrindate, s => int.Parse(s));
                    if (PeriodID.Count() > 1)
                    {
                        return base.ValidationMessage("Select Any one Period");
                    }
                    foreach (var ID in PeriodID)
                    {
                        var res = DepreciationTable.SaveDepreciation(_db, ID, SessionUser.Current.CompanyID, SessionUser.Current.UserID);

                    }
                    _db.SaveChanges();
                    if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.DepreciationApproval))
                    {
                        ViewData["MessageSuffix"] = Language.GetString("DepreciationIsSentForApproval");
                    }
                    base.TraceLog("Depreciation Index Post", $"{SessionUser.Current.Username} - request done");
                    return PartialView("SuccessAction");
                }
                else
                {
                    base.TraceLog("Depreciation Index Post", $"{SessionUser.Current.Username} - request Error:Selected Location Type not created workflow . please create the workflow");
                    return base.ErrorActionResult("Please select the any one period to do deprecation");
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
            return PartialView(data);
        }
        public IActionResult UndoAll()
        {
            if (!base.HasRights(RightNames.Depreciation, UserRightValue.Delete))
                return RedirectToAction("UnauthorizedPage");
            try
            {
                base.TraceLog("Depreciation UndoAll", $"{SessionUser.Current.Username} - UndoAll request");
                var allItems = DepreciationTable.GetAllItems(_db).ToList();
                if (DepreciationTable.validateApprovedProcess(_db, null, SessionUser.Current.CompanyID))
                {
                    foreach (var itm in allItems)
                    {
                        var item = DepreciationTable.GetItem(_db, itm.PeriodID);
                        if (item != null)
                        {
                            item.Delete();
                            if (!AppConfigurationManager.GetValue<bool>(AppConfigurationManager.DepreciationApproval))
                            {
                                item.IsDeletedApproval = true;
                                item.DeletedApprovedDate = System.DateTime.Now;
                                item.DeletedApprovedBy = SessionUser.Current.UserID;
                            }
                            else
                            {
                                item.DeletedDoneBy = SessionUser.Current.UserID;
                                item.DeletedDoneDate = System.DateTime.Now;
                                item.IsDeletedApproval = false;
                            }

                            this._db.SaveChanges();
                            base.TraceLog("Depreciation UndoAll", $"{SessionUser.Current.Username} - UndoAll request done");
                        }
                        else
                        {
                            base.TraceLog("Depreciation UndoAll", $"{SessionUser.Current.Username} - UndoAll request error:YouMustDepreciate");
                            return Json(new { Result = "Error", message = Language.GetString("YouMustDepreciate") });
                        }
                            
                    }
                }
               
                else
                {
                    base.TraceLog("Depreciation UndoAll", $"{SessionUser.Current.Username} - UndoAll request error:SomeDepreciationWasWaitingForApprovalProcess");
                    return Json(new { Result = "Success", message = Language.GetString("SomeDepreciationWasWaitingForApprovalProcess,CantAbleToBulkUndo") });
                }

                base.TraceLog("Depreciation UndoAll", $"{SessionUser.Current.Username} - UndoAll request done");
                if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.DepreciationApproval))
                    return Json(new { Result = "Success", message = Language.GetString("DepreciationUndoMovedToApprvalProcess") });
                else
                    return Json(new { Result = "Success", message = Language.GetString("DepreciationUndoSuccessfully") });
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
        public IActionResult Undo(int id)
        {
            if (!base.HasRights(RightNames.Depreciation, UserRightValue.Delete))
                return RedirectToAction("UnauthorizedPage");
            try
            {
                base.TraceLog("Depreciation Undo", $"{SessionUser.Current.Username} - Undo request.id-{id}");
                var item = DepreciationTable.GetItem(_db, id);
                if (item != null)
                {
                    item.Delete();
                    if (!AppConfigurationManager.GetValue<bool>(AppConfigurationManager.DepreciationApproval))
                    {
                        item.IsDeletedApproval = true;
                        item.DeletedApprovedDate = System.DateTime.Now;
                        item.DeletedApprovedBy = SessionUser.Current.UserID;
                    }
                    else
                    {
                        item.DeletedDoneBy = SessionUser.Current.UserID;
                        item.DeletedDoneDate = System.DateTime.Now;
                        item.IsDeletedApproval = false;
                    }

                    this._db.SaveChanges();
                }
                else
                {
                    base.TraceLog("Depreciation Undo", $"{SessionUser.Current.Username} - Undo request.id-{id} Error:YouMustDepreciate");
                      return base.ErrorActionResult("YouMustDepreciate");
                }
                base.TraceLog("Depreciation Undo", $"{SessionUser.Current.Username} - Undo request.id-{id} Done");
                if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.DepreciationApproval))
                    return base.ErrorActionResult("DepreciationUndoMovedToApprvalProcess");
                else
                    return base.ErrorActionResult("DepreciationUndoSuccessfully");
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
            return PartialView(id);
        }

        public IActionResult Details(int id)
        {
            if (!base.HasRights(RightNames.Depreciation, UserRightValue.Details))
                return RedirectToAction("UnauthorizedPage");
            IndexPageModel indexPage = new IndexPageModel();
            indexPage.EntityInstance = new BaseEntityObject();
            indexPage.EntityInstance.CurrentPageID = SessionUser.Current.GetNextPageID();
            indexPage.PageTitle = id + "";

            ViewBag.PeriodID = id;
            base.TraceLog("Depreciation Details", $"{SessionUser.Current.Username} - Details request.id-{id}");
            
            return PartialView(indexPage);

        }
        public IActionResult SelectedUndo(IFormCollection data)
        {
            if (!base.HasRights(RightNames.Depreciation, UserRightValue.Delete))
                return GotoUnauthorizedPage();
            try
            {
                base.TraceLog("Depreciation SelectedUndo", $"{SessionUser.Current.Username} - SelectedUndo request.");
                string checkdItems = data["ids[]"];
                if ((string.IsNullOrEmpty(checkdItems) != true))
                {
                    string[] arrindate = checkdItems.Split(',');

                    arrindate = arrindate.Where(e => !string.IsNullOrEmpty(e)).Select(e => e).OrderByDescending(e => e).ToArray();
                    int[] itemID = Array.ConvertAll(arrindate, s => int.Parse(s));
                    if (itemID.Count() != 0)
                    {
                        for (int i = 0; i < itemID.Count(); i++)
                        {

                            if (DepreciationTable.ValidationPeriodUndo(_db, itemID[i], SessionUser.Current.CompanyID))
                            {
                                //check depreciationapproved or not if enable approval process
                                if (DepreciationTable.validateApprovedProcess(_db, itemID[i], SessionUser.Current.CompanyID))
                                {
                                    var item = DepreciationTable.GetDeprecation(_db, itemID[i], null, SessionUser.Current.CompanyID);
                                    if (item != null)
                                    {
                                        item.Delete();
                                        if (!AppConfigurationManager.GetValue<bool>(AppConfigurationManager.DepreciationApproval))
                                        {
                                            item.IsDeletedApproval = true;
                                            item.DeletedApprovedDate = System.DateTime.Now;
                                            item.DeletedApprovedBy = SessionUser.Current.UserID;
                                        }
                                        else
                                        {
                                            item.DeletedDoneBy = SessionUser.Current.UserID;
                                            item.DeletedDoneDate = System.DateTime.Now;
                                            item.IsDeletedApproval = false;
                                        }
                                        _db.SaveChanges();
                                    }
                                    else
                                    {
                                        base.TraceLog("Depreciation SelectedUndo", $"{SessionUser.Current.Username} - SelectedUndo request Error:YouMustDepreciate");
                                        return Json(new { Result = "Error", message = Language.GetString("YouMustDepreciate") });
                                    }

                                }
                                else
                                {
                                    base.TraceLog("Depreciation SelectedUndo", $"{SessionUser.Current.Username} - SelectedUndo request Error:waitingForApprovalProcess,CantAbleToUndo");
                                    return Json(new { Result = "Error", message = Language.GetString("waitingForApprovalProcess,CantAbleToUndo") });
                                }
                            }
                            else
                            {
                                base.TraceLog("Depreciation SelectedUndo", $"{SessionUser.Current.Username} - SelectedUndo request Error:LastMonthDepreciationStillNotUndo,PleaseSelectPeriodDescendingOrder");
                                return Json(new { Result = "Error", message = Language.GetString("LastMonthDepreciationStillNotUndo,PleaseSelectPeriodDescendingOrder") });
                            }
                        }
                    }
                    base.TraceLog("Depreciation SelectedUndo", $"{SessionUser.Current.Username} - SelectedUndo request Done.");
                    if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.DepreciationApproval))
                        return Json(new { Result = "Success", message = Language.GetString("Depreciation Undo Moved To Apprval Process") });
                    else
                        return Json(new { Result = "Success", message = Language.GetString("Depreciation Undo Successfully") });
                }
                else
                {
                    base.TraceLog("Depreciation SelectedUndo", $"{SessionUser.Current.Username} - SelectedUndo request Error.:SelectAnyonePeriodForUndo");
                    return Json(new { Result = "Error", message = Language.GetString("Select Any One Period For Undo") });
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
            return PartialView(data);
        }


    }
}
