using ACS.AMS.DAL.DBModel;
using ACS.AMS.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACS.AMS.DAL;
using ACS.AMS.DAL.DBContext;
using Microsoft.AspNetCore.Http;

namespace ACS.AMS.WebApp.Controllers
{
    public class AccountController : ACSBaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserRole()
        {
            if (!base.HasRights(RightNames.UserRole, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("UserRole", "User Role page requested");
            return PartialView();
        }

        public IActionResult UserRolesSelectionEntry(int userID)
        {
            if (!base.HasRights(RightNames.UserRole, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("User Role", $"User Role selected Entry page requested.userID-{userID}");
            return PartialView("UserRolesSelectionEntry", userID);
        }

        [HttpPost]
        public IActionResult UserRole(IFormCollection data)
        {
            if (!base.HasRights(RightNames.UserRole, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");

            try
            {
                base.TraceLog("user Role-post", $"username:{data["UserID"]}");
                string user = data["UserID"].ToString();
                string selectedList = data["hdSelectedItems"];
                string[] roles = selectedList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (roles.Length > 0)
                {
                    User_UserRoleTable.deleteUserRole(_db, int.Parse(user));
                    foreach (string role in roles)
                    {
                        if (role != string.Empty)
                        {
                            User_UserRoleTable userRole = new User_UserRoleTable();
                            userRole.RoleID = int.Parse(role);
                            userRole.UserID = int.Parse(user);
                            _db.Add(userRole);
                        }
                    }
                    _db.SaveChanges();
                    base.TraceLog("User Role", $"Roles are created");
                    string url = "/Dashboard/Dashboard";
                    ViewData["URL"] = url;
                    return PartialView("SuccessAction");
                }
                else
                {
                    return base.ErrorActionResult("Atleast one role need to selected");

                }
            }
            catch (Exception ex)
            {
                return base.ErrorActionResult(ex);
            }

            return PartialView(data);
        }

        public IActionResult ChangePassword()
        {
            //if (!base.HasRights(RightNames.ChangePassword, UserRightValue.View))
            //    return RedirectToAction("UnauthorizedPage");
            base.TraceLog("Change Password", "Change password page requested");
            return PartialView();
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordModel password)
        {
            //if (!base.HasRights(RightNames.ChangePassword, UserRightValue.View))
            //    return RedirectToAction("UnauthorizedPage");
            try
            {
                if (ModelState.IsValid)
                {
                    //var user = HttpContext.Session.GetInt32(SessionUserID).ToString();
                    //int loginUserID = int.Parse(user);
                    User_LoginUserTable oldUser = User_LoginUserTable.GetExistingUser(_db, SessionUser.Current.UserID);
                    base.TraceLog("Change password-Post", $"Change password requested. username:{oldUser.UserName}, password:*****");
                    if (oldUser != null)
                    {
                        string checkOldPwd = User_LoginUserTable.CheckOldPwd(_db, oldUser.UserID, password.OldPassword);
                        base.TraceLog("Change password-Post", $"Password details. newpwd:{password.NewPassword}, oldpwd:{password.OldPassword}");
                        if (string.Compare(checkOldPwd, "Success") == 0)
                        {
                            oldUser.PasswordSalt = EncryptionManager.EncryptPassword(password.NewPassword);
                            oldUser.Password = EncryptionManager.EncryptPassword(password.NewPassword + oldUser.PasswordSalt);
                            _db.SaveChanges();
                        }
                        else
                        {
                            base.TraceLog("Change Password-Post", $"Please Enter Valid Old Password");
                            ViewData["ErrorMesg"] = "Please Enter Valid Old Password";

                            return View();
                        }
                    }
                    return PartialView("ChangePasswordLogoutAction");

                    //string url = "/Dashboard/Dashboard";
                    //TempData["URL"] = url;
                    //TempData["Message"] = "Password Successfully Changed";
                    // return RedirectToAction("SuccessAction");
                }
            }
            catch (Exception ex)
            {
                return base.ErrorActionResult(ex);
            }

            return PartialView(password);
        }

        public ActionResult UserRights()
        {
            if (!base.HasRights(RightNames.UserRights, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("User Rights", $"{SessionUser.Current.Username} :User Right page requested");
            return PartialView();
        }

        public ActionResult UserRightSelectionEntry(int userID)
        {
            if (!base.HasRights(RightNames.UserRights, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");
            ViewBag.Context = _db;
            base.TraceLog("User Rights seclected", $"{SessionUser.Current.Username} :User Right selected page requested.userID-{userID}");
            return PartialView("UserRightSelectionEntry", userID);
        }

        [HttpPost]
        public ActionResult UserRights(IFormCollection data)
        {
            if (!base.HasRights(RightNames.UserRights, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(data["UserID"]))
                    {
                        return base.ErrorActionResult("Please select Username");
                    }
                    else
                    {
                        base.TraceLog("user Rights-Post", $"User Rights added for. userid:{data["UserID"].ToString()}");
                        string user = data["UserID"].ToString();
                        int userID = int.Parse(user);
                        var list = UserRightView.GetAllRights(_db).ToList();
                        Dictionary<object, object> rightIDValues = new Dictionary<object, object>();
                        foreach (var itm in list)
                        {
                            UserRightValue rValue = UserRightValue.None;
                            string trimGroupNID = itm.RightGroupID + "";
                            string view = data["View_" + trimGroupNID + "_" + itm.RightID.ToString()];
                            string create = data["Create_" + trimGroupNID + "_" + itm.RightID.ToString()];
                            string edit = data["Edit_" + trimGroupNID + "_" + itm.RightID.ToString()];
                            string delete = data["Delete_" + trimGroupNID + "_" + itm.RightID.ToString()];
                            string export = data["Export_" + trimGroupNID + "_" + itm.RightID.ToString()];
                            string details = data["Details_" + trimGroupNID + "_" + itm.RightID.ToString()];

                            if ((!string.IsNullOrEmpty(view)) && (view.StartsWith("true")))

                                rValue = rValue | UserRightValue.View;

                            if ((!string.IsNullOrEmpty(create)) && (create.StartsWith("true")))

                                rValue = rValue | UserRightValue.Create;

                            if ((!string.IsNullOrEmpty(edit)) && (edit.StartsWith("true")))

                                rValue = rValue | UserRightValue.Edit;

                            if ((!string.IsNullOrEmpty(delete)) && (delete.StartsWith("true")))
                                rValue = rValue | UserRightValue.Delete;

                            if ((!string.IsNullOrEmpty(export)) && (export.StartsWith("true")))
                                rValue = rValue | UserRightValue.ExportToCSV;

                            if ((!string.IsNullOrEmpty(details)) && (details.StartsWith("true")))
                                rValue = rValue | UserRightValue.Details;

                            int rVal = (int)rValue;
                            rightIDValues.Add(itm.RightID, rVal);
                            //Hi5Soft.Security.Hi5RightHandler.SetRightValueOfUser(user, itm.RightsName, (int)rValue);
                        }

                        var allUserRights = (from b in _db.User_UserRightTable
                                             where b.UserID == userID
                                             select b).ToList();

                        foreach (var rightID in rightIDValues.Keys)
                        {
                            int rID = int.Parse(rightID + "");
                            //get the user right object
                            var userRight = (from u in allUserRights
                                             where u.RightID == rID
                                             select u).FirstOrDefault();
                            if (userRight == null)
                            {
                                userRight = new User_UserRightTable();
                                userRight.RightID = rID;
                                userRight.UserID = userID;

                                _db.Add(userRight);
                            }
                            userRight.RightValue = rightIDValues[rightID] + "";
                        }

                        _db.SaveChanges();
                        base.TraceLog("user Rights-Post", $"User Rights added successfully for. userid:{data["UserID"].ToString()}");
                        string url = "/Dashboard/Dashboard";
                        ViewData["URL"] = url;
                        ViewData["Message"] = "User Rights Successfully Changed";
                        return PartialView("SuccessAction");
                    }
                }
            }
            catch (Exception ex)
            {
                return base.ErrorActionResult(ex);

            }
            return PartialView(data);
        }

        public ActionResult RoleRights()
        {
            if (!base.HasRights(RightNames.RoleRights, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("Role Rights", $"{SessionUser.Current.Username} :Role Right page requested");
            return PartialView();
        }

        public ActionResult RoleRightSelectionEntry(int roleID)
        {
            if (!base.HasRights(RightNames.RoleRights, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("Role Rights", $"{SessionUser.Current.Username} :Role Right selected page requested.roleID-{roleID}");
            return PartialView("RoleRightSelectionEntry", roleID);
        }

        [HttpPost]
        public ActionResult RoleRights(IFormCollection data)
        {
            if (!base.HasRights(RightNames.RoleRights, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");
            try
            {
                if (string.IsNullOrEmpty(data["RoleID"]))
                {
                    return base.ErrorActionResult("Please select Role");
                }
                else
                {

                    string role = data["RoleID"].ToString();
                    base.TraceLog("Role Rights-Post", $"Role Rights added for. RoleID:{role}");
                    int roleID = int.Parse(role);
                    var list = UserRightView.GetAllRights(_db).ToList();
                    Dictionary<object, object> rightIDValues = new Dictionary<object, object>();
                    foreach (var itm in list)
                    {
                        UserRightValue rValue = UserRightValue.None;

                        string trimGroupNID = itm.RightGroupID + "";

                        string view = data["View_" + trimGroupNID + "_" + itm.RightID.ToString()];
                        string create = data["Create_" + trimGroupNID + "_" + itm.RightID.ToString()];
                        string edit = data["Edit_" + trimGroupNID + "_" + itm.RightID.ToString()];
                        string delete = data["Delete_" + trimGroupNID + "_" + itm.RightID.ToString()];
                        string export = data["Export_" + trimGroupNID + "_" + itm.RightID.ToString()];
                        string details = data["Details_" + trimGroupNID + "_" + itm.RightID.ToString()];

                        if ((!string.IsNullOrEmpty(view)) && (view.StartsWith("true")))
                            rValue = rValue | UserRightValue.View;

                        if ((!string.IsNullOrEmpty(create)) && (create.StartsWith("true")))
                            rValue = rValue | UserRightValue.Create;

                        if ((!string.IsNullOrEmpty(edit)) && (edit.StartsWith("true")))
                            rValue = rValue | UserRightValue.Edit;

                        if ((!string.IsNullOrEmpty(delete)) && (delete.StartsWith("true")))
                            rValue = rValue | UserRightValue.Delete;

                        if ((!string.IsNullOrEmpty(export)) && (export.StartsWith("true")))
                            rValue = rValue | UserRightValue.ExportToCSV;

                        if ((!string.IsNullOrEmpty(details)) && (details.StartsWith("true")))
                            rValue = rValue | UserRightValue.Details;

                        int rVal = (int)rValue;
                        rightIDValues.Add(itm.RightID, rVal);
                    }

                    var allUserRights = (from b in _db.User_RoleRightTable
                                         where b.RoleID == roleID
                                         select b).ToList();

                    foreach (var rightID in rightIDValues.Keys)
                    {
                        int rID = int.Parse(rightID + "");
                        //get the user right object
                        var userRight = (from u in allUserRights
                                         where u.RightID == rID
                                         select u).FirstOrDefault();
                        if (userRight == null)
                        {
                            userRight = new User_RoleRightTable();
                            userRight.RightID = rID;
                            userRight.RoleID = roleID;


                            _db.Add(userRight);
                        }
                        userRight.RightValue = rightIDValues[rightID] + "";
                    }

                    _db.SaveChanges();

                    base.TraceLog("Role Rights-Post", $"Role Rights added successfully for. RoleID:{role}");
                    string url = "/Dashboard/Dashboard";
                    ViewData["URL"] = url;
                    ViewData["Message"] = "Roles and Rights Successfully Changed";

                    return PartialView("SuccessAction");
                }
            }
            catch (Exception ex)
            {
                return base.ErrorActionResult(ex);

            }

            return PartialView(data);
        }
    }
}