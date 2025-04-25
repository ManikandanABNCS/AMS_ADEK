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
using System.Diagnostics;
using System.Numerics;

namespace ACS.AMS.WebApp.Controllers
{
    public class ApproveWorkflowController : ACSBaseController
    {
        public IActionResult Index()
        {
            if (!base.HasRights(RightNames.ApproveWorkflow, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");
            this.TraceLog("Index", $"{SessionUser.Current.Username} - ApproveWorkflow Index page requested");
            return PartialView();
        }

        public IActionResult _Index([DataSourceRequest] DataSourceRequest request)
        {
            try
            {


                if (!base.HasRights(RightNames.ApproveWorkflow, UserRightValue.View))
                    return RedirectToAction("UnauthorizedPage");
                var query = ApproveWorkflowTable.GetAllApprovalWorkflow(_db).OrderByDescending(a => a.ApproveWorkflowID);

                var dsResult = request.ToDataSourceResult(query);//query.ToDataSourceResult(request);
                this.TraceLog("Index", $"{SessionUser.Current.Username} - ApproveWorkflow Index page Data Fetch");
                return Json(dsResult);
            }
            catch (Exception ex)
            {
                return base.ErrorActionResult(ex);
            }
        }

        public IActionResult Create()
        {
            if (!base.HasRights(RightNames.ApproveWorkflow, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("Create", $"{SessionUser.Current.Username} -approval Workflow Create page requested");

            if (!AppConfigurationManager.GetValue<bool>(AppConfigurationManager.IsMandatoryLocationType))
            {
                var locationType = LocationTypeTable.GetAllLocationType(_db, "All");
                ApproveWorkflowTable create = new ApproveWorkflowTable();
                if (locationType != null)
                {
                    create.FromLocationTypeID = locationType.LocationTypeID;
                    create.ToLocationTypeID = locationType.LocationTypeID;
                }
                else
                {
                    var type = LocationTypeTable.insertAllType(_db);
                    create.FromLocationTypeID = type.LocationTypeID;
                    create.ToLocationTypeID = type.LocationTypeID;

                }


                return PartialView(create);
            }
            return PartialView();
        }

        [HttpPost]
        public IActionResult Create(IFormCollection data, ApproveWorkflowTable roleTable)
        {
            if (!base.HasRights(RightNames.ApproveWorkflow, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");

            try
            {
                base.TraceLog("Create Post", $"{SessionUser.Current.Username} - ApproveWorkflow details will add to db");
                if (ModelState.IsValid)
                {
                    bool validate = true;

                    if (roleTable.ApproveModuleID == (int)ApproveModuleValue.AssetTransfer)
                    {
                        validate = ApproveWorkflowTable.ValidateWorkFlow(_db, roleTable.FromLocationTypeID, roleTable.ApproveModuleID, (int)roleTable.ToLocationTypeID);
                    }
                    else if (roleTable.ApproveModuleID == (int)ApproveModuleValue.AssetRetirement)
                    {


                        validate = ApproveWorkflowTable.ValidateWorkFlow(_db, roleTable.FromLocationTypeID, roleTable.ApproveModuleID);
                    }
                    else if (roleTable.ApproveModuleID == (int)ApproveModuleValue.InternalAssetTransfer)
                    {
                        validate = ApproveWorkflowTable.ValidateWorkFlow(_db, roleTable.FromLocationTypeID, roleTable.ApproveModuleID, (int)roleTable.ToLocationTypeID);
                    }
                    else if (roleTable.ApproveModuleID == (int)ApproveModuleValue.AssetAddition)
                    {

                        validate = ApproveWorkflowTable.ValidateWorkFlow(_db, roleTable.FromLocationTypeID, roleTable.ApproveModuleID);
                    }
                    else if (roleTable.ApproveModuleID == (int)ApproveModuleValue.AssetMaintenanceRequest)
                    {
                        ;
                        validate = ApproveWorkflowTable.ValidateWorkFlow(_db, roleTable.FromLocationTypeID, roleTable.ApproveModuleID);
                    }
                    else if (roleTable.ApproveModuleID == (int)ApproveModuleValue.AMCSchedule)
                    {

                        validate = ApproveWorkflowTable.ValidateWorkFlow(_db, roleTable.FromLocationTypeID, roleTable.ApproveModuleID);
                    }
                    if (!validate)
                    {
                        if (!AppConfigurationManager.GetValue<bool>(AppConfigurationManager.IsMandatoryLocationType))
                        {
                            if ((roleTable.ApproveModuleID == (int)ApproveModuleValue.AssetTransfer)
                                    || (roleTable.ApproveModuleID == (int)ApproveModuleValue.InternalAssetTransfer))
                            {
                            }
                            else
                            {
                                roleTable.ToLocationTypeID = null;
                            }
                        }
                        _db.Add(roleTable);

                        int p = 0;
                        string selectedList = data["hdSelectedItems"];
                        string[] roles = selectedList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (roles.Length > 0)
                        {
                            int cntValidation = 0;
                            if (roleTable.ApproveModuleID == (int)ApproveModuleValue.AssetTransfer || roleTable.ApproveModuleID == (int)ApproveModuleValue.AssetRetirement)

                            {
                                cntValidation = ComboBoxHelper.ValidateApprovalRole(roles, roleTable.ApproveModuleID, _db);
                            }
                            else
                            {
                                cntValidation = 1;
                            }
                            if (cntValidation == 1)
                            {
                                foreach (string role in roles)
                                {
                                    if (role != string.Empty)
                                    {
                                        ApproveWorkflowLineItemTable userRole = new ApproveWorkflowLineItemTable();
                                        userRole.ApproveWorkFlow = roleTable;
                                        p = p + 1;
                                        userRole.OrderNo = p;
                                        userRole.ApprovalRoleID = int.Parse(role);
                                        userRole.StatusID = (int)StatusValue.Active;
                                        _db.Add(userRole);
                                    }
                                }
                                _db.SaveChanges();
                                base.TraceLog("Create Post", $"{SessionUser.Current.Username} - ApproveWorkflow details saved to db");
                                return PartialView("SuccessAction");
                            }
                            else if (cntValidation == 0)
                            {
                                return base.ErrorActionResult("Selected Role there is no enable update option.");
                            }
                            else if (cntValidation > 1)
                            {
                                return base.ErrorActionResult("Selected Role there have more then one update option enable. its allow only one update option");
                            }
                        }
                        else
                        {

                            return base.ErrorActionResult("Please select anyone approval Role Details");
                        }
                    }
                    else
                    {

                        return base.ErrorActionResult("Selected From Location Type and To Location Type with category type already created");
                    }
                }
            }
            catch (ValidationException ex)
            {
                ViewData["FocusControl"] = ex.FieldName;
                ModelState.AddModelError("12", ex.Message);
            }
            catch (Exception ex)
            {
                return base.ErrorActionResult(ex);
            }

            return base.ErrorActionResult("Please fill the mandatory fields");
            //return View(roleTable);
        }

        public IActionResult Edit(int id)
        {
            if (!base.HasRights(RightNames.ApproveWorkflow, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("Edit", $"{SessionUser.Current.Username} -approval Workflow Edit page requested");

            ApproveWorkflowTable flow = ApproveWorkflowTable.GetApprovalWorkflow(_db, id);
            bool validate = ApprovalHistoryTable.GethistoryValidation(_db, id);
            if (validate)
            {
                return PartialView("Details", flow);
            }
            else
            {
                return PartialView(flow);
            }

        }
        [HttpPost]
        public IActionResult Edit(IFormCollection data, ApproveWorkflowTable roleTable)
        {
            if (!base.HasRights(RightNames.ApproveWorkflow, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");

            try
            {
                base.TraceLog("Edit-post", $"{SessionUser.Current.Username} -approval Workflow details will be modify to db.id-{roleTable.ApproveWorkflowID}");
                if (ModelState.IsValid)
                {
                    bool validate = true;
                    //if (roleTable.ApproveModuleID == (int)ApproveModuleValue.AssetTransfer)
                    //{
                    //    validate = ApproveWorkflowTable.ValidateWorkFlow(_db, roleTable.FromLocationTypeID, roleTable.ApproveModuleID, (int)roleTable.ToLocationTypeID, roleTable.CategoryTypeID,roleTable.ApproveWorkflowID);
                    //}
                    //else if (roleTable.ApproveModuleID == (int)ApproveModuleValue.AssetRetirement)
                    //{
                    //    validate = ApproveWorkflowTable.ValidateWorkFlow(_db, roleTable.FromLocationTypeID, roleTable.ApproveModuleID, roleTable.CategoryTypeID, roleTable.ApproveWorkflowID);
                    //}
                    //else if (roleTable.ApproveModuleID == (int)ApproveModuleValue.InternalAssetTransfer)
                    //{
                    //    validate = ApproveWorkflowTable.ValidateWorkFlow(_db, roleTable.FromLocationTypeID, roleTable.ApproveModuleID, (int)roleTable.ToLocationTypeID, roleTable.CategoryTypeID, roleTable.ApproveWorkflowID);
                    //}
                    //if (!validate)
                    //{
                    int p = 0;
                    string selectedList = data["hdSelectedItems"];
                    string[] roles = selectedList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    int cntValidation = 0;
                    if (roleTable.ApproveModuleID == (int)ApproveModuleValue.AssetTransfer || roleTable.ApproveModuleID == (int)ApproveModuleValue.AssetRetirement)
                    {
                        cntValidation = ComboBoxHelper.ValidateApprovalRole(roles, roleTable.ApproveModuleID, _db);
                    }
                    else
                    {
                        cntValidation = 1;
                    }
                    if (cntValidation == 1)
                    {
                        if (roles.Length > 0)
                        {
                            ApproveWorkflowTable oldItm = ApproveWorkflowTable.GetItem(_db, roleTable.ApproveWorkflowID);
                            DataUtilities.CopyObject(roleTable, oldItm);

                            var oldlineitem = ApproveWorkflowLineItemTable.GetAllApproveWorkflowLineItems(_db, roleTable.ApproveWorkflowID);
                            List<int> addedRoleIDs = new List<int>();
                            List<int> deletedRoleIDs = new List<int>();
                            List<int> AllRoleIDs = new List<int>();
                            List<int> matchedRoleIDs = new List<int>();
                            AllRoleIDs = oldlineitem.Select(a => a.ApprovalRoleID).ToList();
                            int orderNos = 0;

                            foreach (string role in roles)
                            {
                                if (role != string.Empty)
                                {
                                    int roleid = 0;
                                    orderNos = orderNos + 1;
                                    //int.TryParse(desig, out designationID);
                                    int.TryParse(role, out roleid);
                                    var existingItem = (from x in oldlineitem
                                                        where x.ApprovalRoleID == int.Parse(role)
                                                        select x).FirstOrDefault();
                                    if (existingItem != null)
                                    {
                                        ApproveWorkflowLineItemTable oldmatch = ApproveWorkflowLineItemTable.GetApproveWorkflowLineItem(_db, roleTable.ApproveWorkflowID, roleid);
                                        oldmatch.OrderNo = orderNos;
                                        oldmatch.ApprovalRoleID = roleid;
                                        oldmatch.StatusID = (int)StatusValue.Active;

                                        matchedRoleIDs.Add(roleid);
                                    }
                                    else if (existingItem == null)
                                    {
                                        ApproveWorkflowLineItemTable userRole = new ApproveWorkflowLineItemTable();
                                        userRole.ApproveWorkFlowID = roleTable.ApproveWorkflowID;

                                        userRole.OrderNo = orderNos;
                                        userRole.ApprovalRoleID = int.Parse(role);
                                        userRole.StatusID = (int)StatusValue.Active;
                                        _db.Add(userRole);
                                        addedRoleIDs.Add(roleid);
                                    }


                                }
                            }
                            deletedRoleIDs = AllRoleIDs.Except(matchedRoleIDs).ToList();
                            foreach (int roleids in deletedRoleIDs)
                            {
                                ApproveWorkflowLineItemTable olddeletematch = ApproveWorkflowLineItemTable.GetApproveWorkflowLineItem(_db, roleTable.ApproveWorkflowID, roleids);
                                olddeletematch.StatusID = (int)StatusValue.Deleted;
                            }
                            _db.SaveChanges();
                            base.TraceLog("Edit-post", $"{SessionUser.Current.Username} -Approval Workflow Edit page saved to db ");
                            return PartialView("SuccessAction");
                        }
                        else
                        {
                            return base.ErrorActionResult("Please select anyone approval Role DEtails");
                        }
                    }
                    else if (cntValidation == 0)
                    {
                        return base.ErrorActionResult("Selected Role there is no enable update option.");
                    }
                    else if (cntValidation > 1)
                    {
                        return base.ErrorActionResult("Selected Role there have more then one update option enable. its allow only one update option");
                    }
                    //}
                    //else
                    //{

                    //    return base.ErrorActionResult("Selected From Location Type and To Location Type with category type already created");
                    //}

                }
            }
            catch (ValidationException ex)
            {
                ViewData["FocusControl"] = ex.FieldName;
                ModelState.AddModelError("12", ex.Message);
            }
            catch (Exception ex)
            {
                return base.ErrorActionResult(ex);
            }
            return base.ErrorActionResult("Please fill the mandatory fields");
            //return View(roleTable);
        }

        public IActionResult Details(int id)
        {
            if (!base.HasRights(RightNames.ApproveWorkflow, UserRightValue.Details))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("Details", $"{SessionUser.Current.Username} -apporval Workflow Details page requested");

            ApproveWorkflowTable flow = ApproveWorkflowTable.GetApprovalWorkflow(_db, id);
            return PartialView(flow);
        }

        public IActionResult Delete(int id)
        {
            try
            {
                if (!base.HasRights(RightNames.ApproveWorkflow, UserRightValue.Delete))
                    return RedirectToAction("UnauthorizedPage");

                var pageName = "ApproveWorkflow";
                base.TraceLog("Delete", $"{SessionUser.Current.Username} - {pageName} Delete action requested");

                ApproveWorkflowTable flow = ApproveWorkflowTable.GetItem(_db, id);

                if (flow != null)
                    flow.Delete();

                _db.SaveChanges();

                base.TraceLog("Delete", $"{pageName} details page deleted successfully {flow.GetPrimaryKeyFieldName()} {id}");
                return PartialView("SuccessAction", new { pageName = pageName });
            }
            catch (Exception ex)
            {
                return base.ErrorActionResult(ex);
            }
        }
    }
}