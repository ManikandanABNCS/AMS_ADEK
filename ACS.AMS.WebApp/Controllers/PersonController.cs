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
using System.Text.RegularExpressions;

namespace ACS.AMS.WebApp.Controllers
{
    public class PersonController : ACSBaseController
    {
        private static IWebHostEnvironment WebHostEnvironment;
        public PersonController(IWebHostEnvironment _environment)
        {
            WebHostEnvironment = _environment;
            Thread.CurrentThread.CurrentUICulture =
           Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-GB");
        }
        public IActionResult Index()
        {
            if (!base.HasRights(RightNames.UserMaster, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("UserMaster Index", $"{SessionUser.Current.Username} -UserMaster Index page requested");
            return PartialView();
        }

        public IActionResult _Index([DataSourceRequest] DataSourceRequest request)
        {
            if (!base.HasRights(RightNames.UserMaster, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");

            var query = PersonView.GetAllItems(_db);
            var dsResult = query.ToDataSourceResult(request);

            base.TraceLog("UserMaster Index", $"{SessionUser.Current.Username} -UserMaster Index page Data Fetch");
            return Json(dsResult);
        }


        public IActionResult Create()
        {
            if (!base.HasRights(RightNames.UserMaster, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("UserMaster Create", $"{SessionUser.Current.Username} -UserMaster Create page requested");
            RegisterModel data = new RegisterModel();
            data.CurrentPageID = SessionUser.Current.GetNextPageID();
            return PartialView(data);

        }

        [HttpPost]
        public IActionResult Create(RegisterModel item, IFormCollection data)
        {
            if (!base.HasRights(RightNames.UserMaster, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");
            try
            {
                base.TraceLog("UserMaster Create Post", $"{SessionUser.Current.Username} -UserMaster details will add to db ");
                if (item.Person.PersonCode == null)
                {
                    ModelState.AddModelError("Person.PersonCode", "Person Code field is required ");
                }

                if (item.Person.DOJ == null)
                {
                    ModelState.AddModelError("Person.DOJ", "DOJ field is required ");
                }
                //if (item.Person.MobileNo == null)
                //{
                //    ModelState.AddModelError("Person.MobileNo", "MobileNo field is required ");
                //}
                //if (item.Person.WhatsAppMobileNo == null)
                //{
                //    ModelState.AddModelError("Person.WhatsAppMobileNo", "WhatsAppMobileNo field is required ");
                //}
                if (item.Person.EMailID == null)
                {
                    ModelState.AddModelError("Person.EMailID", "EMailID field is required ");
                }

                if (ModelState.IsValid)
                {
                    var validateUser = User_LoginUserTable.GetUser(_db, item.UserName);

                    if (validateUser == null)
                    {
                        item.Person.ValidateUniqueKey(_db);
                        bool diableUser = item.Person.UserTypeID == 2 ? true : false;
                        User_LoginUserTable userDetails = new User_LoginUserTable();
                        userDetails.UserName = item.UserName;
                        userDetails.PasswordSalt = EncryptionManager.EncryptPassword(item.Password);//"Mod6/JMHjHeKXDkUK/zd7PfLlJg=";
                        userDetails.Password = EncryptionManager.EncryptPassword(item.Password + userDetails.PasswordSalt);//"BZxI8E2lroNt28VMhZsyyaNZha8=";
                        userDetails.LastActivityDate = System.DateTime.Now;
                        userDetails.LastLoginDate = System.DateTime.Now;
                        userDetails.LastLoggedInDate = System.DateTime.Now;
                        userDetails.ChangePasswordAtNextLogin = true;
                        userDetails.IsLockedOut = false;
                        userDetails.IsDisabled = diableUser;
                        userDetails.IsApproved = true;

                        //userDetails.UserDataSource = "A";
                        _db.Add(userDetails);
                        _db.SaveChanges();
                        _db.Entry(userDetails).Reload();

                        item.Person.PersonID = userDetails.UserID;
                        item.Person.StatusID = (int)StatusValue.Active;
                        item.Person.UserTypeID = item.Person.UserTypeID;
                        item.Person.CreatedBy = SessionUser.Current.UserID;
                        item.Person.CreatedDateTime = System.DateTime.Now;
                        item.Person.CreatedFrom = "App";
                        if (!string.IsNullOrEmpty(item.Person.SignaturePath))
                        {
                            var newpath = RenameFileWithFileInfoClass(item.Person.SignaturePath, string.Concat(item.Person.PersonCode, '_', item.Person.PersonID.ToString()));
                            item.Person.SignaturePath = newpath;
                        }

                        //if (TempData["UploadedFile"] != null)
                        //{
                        //    item.Person.SignaturePath = TempData["UploadedFile"].ToString();
                        //}
                        //if (TempData["UploadedStampFile"] != null)
                        //{
                        //    item.Person.StampPath = TempData["UploadedStampFile"].ToString();
                        //}
                        //item.Person.AllowLogin = true;

                        _db.Add(item.Person);
                        if (!string.IsNullOrEmpty(data["CurrentPageID"]))
                        {
                            int currentpageID = int.Parse(data["CurrentPageID"]);

                            var obj = ApprovalRoleDataModel.GetModel(currentpageID);
                            if (obj.LineItems.Count() > 0)
                            {
                                foreach (var line in obj.LineItems)
                                {
                                    UserApprovalRoleMappingTable userApprovalRoleMappingTable = new UserApprovalRoleMappingTable();
                                    userApprovalRoleMappingTable.LocationID = line.LocationID;
                                    userApprovalRoleMappingTable.ApprovalRoleID = line.ApprovalRoleID;
                                    userApprovalRoleMappingTable.UserID = userDetails.UserID;
                                    userApprovalRoleMappingTable.StatusID = (int)StatusValue.Active;
                                    userApprovalRoleMappingTable.CategoryTypeID = line.CategoryTypeID;
                                    _db.Add(userApprovalRoleMappingTable);

                                }
                            }

                        }
                        string company = (string)data["hdCompanySelectedItemsIDS"];
                        string[] companyColumnID = company.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var str in companyColumnID.Distinct())
                        {
                            UserCompanyMappingTable line = new UserCompanyMappingTable();
                            line.CompanyID = int.Parse(str);
                            line.UserID = userDetails.UserID; ;
                            _db.Add(line);
                        }

                        string location = (string)data["hdLocationSelectedItemsIDS"];
                        string[] locColumnID = location.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var str in locColumnID.Distinct())
                        {
                            UserLocationMappingTable line = new UserLocationMappingTable();
                            line.LocationID = int.Parse(str);
                            line.PersonID = userDetails.UserID; ;
                            _db.Add(line);
                        }
                        string Category = (string)data["hdCategorySelectedItemsIDS"];
                        //userCategory Mapping 
                        string[] cateColumnID = Category.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var str in cateColumnID.Distinct())
                        {
                            UserCategoryMappingTable cate = new UserCategoryMappingTable();
                            cate.CategoryID = int.Parse(str);
                            cate.PersonID = userDetails.UserID;
                            _db.Add(cate);
                        }
                        string department = (string)data["hdDepartmentSelectedItemsIDS"];
                        //userCategory Mapping 
                        string[] depatColumnID = department.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var str in depatColumnID.Distinct())
                        {
                            UserDepartmentMappingTable dept = new UserDepartmentMappingTable();
                            dept.DepartmentID = int.Parse(str);
                            dept.PersonID = userDetails.UserID;
                            _db.Add(dept);
                        }
                        _db.SaveChanges();

                        ApprovalRoleDataModel.RemoveModel(int.Parse(data["CurrentPageID"]));

                        base.TraceLog("UserMaster Create Post", $"{SessionUser.Current.Username} -UserMaster details saved to db ");
                        return PartialView("SuccessAction");
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "UserName is Already exist ");
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
                return ErrorActionResult(ex);
            }
            return PartialView(item);
        }
        public static string RenameFileWithFileInfoClass(string oldFile, string newFile)
        {


            var oldPath = Path.Combine(WebHostEnvironment.WebRootPath, oldFile);
            var oldfiles = Path.GetFileName(oldPath);
            var extension = Path.GetExtension(oldPath);
            string result = Regex.Replace(newFile, @"[^a-zA-Z0-9\s]", "");
           // var newphysicalpth = System.IO.Path.Combine(folderName, string.Concat(result.Replace(" ", ""), extension));
            var newphysicalpth = Path.Combine("FileStoragePath/SignaturePath", string.Concat(result.Replace(" ", ""), extension));
            var newpath = Path.Combine(WebHostEnvironment.WebRootPath, newphysicalpth);
            if (string.Compare(oldPath, newpath) != 0)
            {
                var file = new FileInfo(oldPath);
                if (System.IO.File.Exists(newpath))
                {
                    System.IO.File.Delete(newpath);
                }
                file.MoveTo(newpath);
            }
            return newphysicalpth;

        }
        public IActionResult Edit(int id)
        {
            if (!base.HasRights(RightNames.UserMaster, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("UserMaster Edit", $"{SessionUser.Current.Username} -UserMaster Edit page requested ");
            PersonTable person = PersonTable.GetPersonBasedOnID(_db, id);
            var user = User_LoginUserTable.GetExistingUser(_db, id);
            EditRegisterModel ed = new EditRegisterModel();
            ed.Person = person;
            ed.UserName = user.UserName;
            ed.CurrentPageID = SessionUser.Current.GetNextPageID();
            var list = PersonTable.GetList(_db, id);
            var obj = ApprovalRoleDataModel.GetModel(ed.CurrentPageID);
            bool validation = false;

            foreach (var item in list.ToList())
            {
                ApprovalRoleModel ap = new ApprovalRoleModel();
                ap.LocationID = item.LocationID;
                ap.ApprovalRoleID = item.ApprovalRoleID;
                ap.LocationName = LocationTable.GetItem(_db, item.LocationID).LocationName;
                ap.ApprovalRoleName = ApprovalRoleTable.GetItem(_db, item.ApprovalRoleID).ApprovalRoleName;
                if (item.CategoryTypeID.HasValue)
                {
                    ap.CategoryTypeID = (int)item.CategoryTypeID;
                    ap.CategoryType = CategoryTypeTable.GetItem(_db, (int)item.CategoryTypeID).CategoryTypeName;
                }
                validation = ApprovalHistoryTable.GetMappingValidation(_db, item.LocationID, item.ApprovalRoleID);
                obj.LineItems.Add(ap);
            }
            var categorycount = obj.LineItems.Where(a => a.CategoryTypeID == 0).Count();
            if (categorycount > 0)
            {
                ModelState.AddModelError("12", "User Approval Role Category Type is not mapped ");
            }
            base.TraceLog("UserMaster Edit", $"UserMaster Edit page requested UserMasterID {id}");
            if (validation)
            {
                return PartialView("Details", ed);
            }
            else
            {
                return PartialView(ed);
            }
        }

        [HttpPost]
        public IActionResult Edit(EditRegisterModel model, IFormCollection data)
        {
            if (!base.HasRights(RightNames.UserMaster, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");
            try
            {
                base.TraceLog("UserMaster Edit Post", $"UserMaster details will modify to db : UserMaster id- {model.Person.PersonID}");
                if (model.Person.DOJ == null)
                {
                    ModelState.AddModelError("Person.DOJ", "DOJ field is required ");
                }
                //if (model.Person.MobileNo == null)
                //{
                //    ModelState.AddModelError("Person.MobileNo", "MobileNo field is required ");
                //}
                //if (model.Person.WhatsAppMobileNo == null)
                //{
                //    ModelState.AddModelError("Person.WhatsAppMobileNo", "WhatsAppMobileNo field is required ");
                //}
                if (model.Person.EMailID == null)
                {
                    ModelState.AddModelError("Person.EMailID", "EMailID field is required ");
                }

                if (ModelState.IsValid)
                {
                    PersonTable oldItm = PersonTable.GetPersonBasedOnID(_db, model.Person.PersonID);
                    oldItm.PersonCode = model.Person.PersonCode;
                    oldItm.PersonFirstName = model.Person.PersonFirstName;
                    oldItm.PersonLastName = model.Person.PersonLastName;
                    oldItm.EMailID = model.Person.EMailID;
                    oldItm.MobileNo = model.Person.MobileNo;
                    oldItm.StatusID = model.Person.StatusID;
                    oldItm.DepartmentID = model.Person.DepartmentID;
                    oldItm.DOJ = model.Person.DOJ;
                    oldItm.WhatsAppMobileNo = model.Person.WhatsAppMobileNo;
                    oldItm.UserTypeID = model.Person.UserTypeID;
                    oldItm.Gender = model.Person.Gender;
                    oldItm.LastModifiedBy = SessionUser.Current.UserID;
                    oldItm.LastModifiedDateTime = System.DateTime.Now;
                    oldItm.CreatedFrom = model.Person.CreatedFrom;

                    // oldItm.CreatedFrom = "App";
                    if (!string.IsNullOrEmpty(model.Person.SignaturePath))
                    {
                        var newpath = RenameFileWithFileInfoClass(model.Person.SignaturePath, string.Concat(model.Person.PersonCode, '_', model.Person.PersonID.ToString()));
                        oldItm.SignaturePath = newpath;
                    }
                    else
                    {
                        oldItm.SignaturePath = model.Person.SignaturePath;
                    }

                    //if (TempData["UploadedFile"] != null)
                    //{
                    //    oldItm.SignaturePath = TempData["UploadedFile"].ToString();
                    //}
                    //if (TempData["UploadedStampFile"] != null)
                    //{
                    //    oldItm.StampPath = TempData["UploadedStampFile"].ToString();
                    //}
                    oldItm.ValidateUniqueKey(_db);
                    User_LoginUserTable oldUser = User_LoginUserTable.GetExistingUser(_db, model.Person.PersonID);
                    //if (!string.IsNullOrEmpty(model.Password))
                    //{
                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        oldUser.PasswordSalt = EncryptionManager.EncryptPassword(model.Password);
                        oldUser.Password = EncryptionManager.EncryptPassword(model.Password + oldUser.PasswordSalt);
                    }
                    if (!string.IsNullOrEmpty(data["CurrentPageID"]))
                    {
                        int currentpageID = int.Parse(data["CurrentPageID"]);

                        var obj = ApprovalRoleDataModel.GetModel(currentpageID);
                        if (obj.LineItems.Count() > 0)
                        {
                            var categorycount = obj.LineItems.Where(a => a.CategoryTypeID == 0).Count();
                            if (categorycount > 0)
                            {
                                return base.ErrorActionResult("User Approval Role Category Type is not mapped");
                            }
                            PersonTable.deleteApprovalRole(_db, model.Person.PersonID);
                            foreach (var line in obj.LineItems)
                            {
                                var validate = PersonTable.validateRole(_db, model.Person.PersonID, line.LocationID, line.ApprovalRoleID);
                                if (validate == null)
                                {
                                    UserApprovalRoleMappingTable userApprovalRoleMappingTable = new UserApprovalRoleMappingTable();
                                    userApprovalRoleMappingTable.LocationID = line.LocationID;
                                    userApprovalRoleMappingTable.ApprovalRoleID = line.ApprovalRoleID;
                                    userApprovalRoleMappingTable.UserID = model.Person.PersonID;
                                    userApprovalRoleMappingTable.StatusID = (int)StatusValue.Active;
                                    userApprovalRoleMappingTable.CategoryTypeID = line.CategoryTypeID;
                                    _db.Add(userApprovalRoleMappingTable);
                                }


                            }
                        }

                    }
                    string company = (string)data["hdCompanySelectedItemsIDS"];
                    string[] compColumnID = company.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    PersonTable.deletecompanyLinteItem(_db, model.Person.PersonID);
                    foreach (var str in compColumnID.Distinct())
                    {
                        UserCompanyMappingTable line = new UserCompanyMappingTable();
                        line.CompanyID = int.Parse(str);
                        line.UserID = model.Person.PersonID; ;
                        _db.Add(line);
                    }

                    string location = (string)data["hdLocationSelectedItemsIDS"];
                    string[] locColumnID = location.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    PersonTable.deleteLocationLinteItem(_db, model.Person.PersonID);
                    foreach (var str in locColumnID.Distinct())
                    {
                        UserLocationMappingTable line = new UserLocationMappingTable();
                        line.LocationID = int.Parse(str);
                        line.PersonID = model.Person.PersonID; ;
                        _db.Add(line);
                    }
                    string Category = (string)data["hdCategorySelectedItemsIDS"];
                    //userCategory Mapping 
                    string[] cateColumnID = Category.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    PersonTable.deleteCategoryLinteItem(_db, model.Person.PersonID);
                    foreach (var str in cateColumnID.Distinct())
                    {
                        UserCategoryMappingTable cate = new UserCategoryMappingTable();
                        cate.CategoryID = int.Parse(str);
                        cate.PersonID = model.Person.PersonID;
                        _db.Add(cate);
                    }
                    string department = (string)data["hdDepartmentSelectedItemsIDS"];
                    //userCategory Mapping 
                    string[] depatColumnID = department.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    PersonTable.deleteDepartmentLinteItem(_db, model.Person.PersonID);
                    foreach (var str in depatColumnID.Distinct())
                    {
                        UserDepartmentMappingTable dept = new UserDepartmentMappingTable();
                        dept.DepartmentID = int.Parse(str);
                        dept.PersonID = model.Person.PersonID;
                        _db.Add(dept);
                    }
                    _db.SaveChanges();
                    ApprovalRoleDataModel.RemoveModel(int.Parse(data["CurrentPageID"]));
                    base.TraceLog("UserMaster Edit Post", $"UserMaster details modified to db : UserMaster id- {model.Person.PersonID}");
                    return PartialView("SuccessAction");
                    //return PartialView("SuccessAction");
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
                return ErrorActionResult(ex);
            }
            return PartialView(model);
        }

        public IActionResult Details(int id)
        {
            if (!base.HasRights(RightNames.UserMaster, UserRightValue.Details))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("UserMaster Details", $"{SessionUser.Current.Username} -UserMaster Detail Page requested");
            try
            {
                PersonTable person = PersonTable.GetPersonBasedOnID(_db, id);
                var user = User_LoginUserTable.GetExistingUser(_db, id);
                EditRegisterModel ed = new EditRegisterModel();
                ed.Person = person;
                ed.UserName = user.UserName;
                ed.CurrentPageID = SessionUser.Current.GetNextPageID();
                var list = PersonTable.GetList(_db, id);
                var obj = ApprovalRoleDataModel.GetModel(ed.CurrentPageID);
                foreach (var item in list.ToList())
                {
                    ApprovalRoleModel ap = new ApprovalRoleModel();
                    ap.LocationID = item.LocationID;
                    ap.ApprovalRoleID = item.ApprovalRoleID;
                    ap.LocationName = LocationTable.GetItem(_db, item.LocationID).LocationName;
                    ap.ApprovalRoleName = ApprovalRoleTable.GetItem(_db, item.ApprovalRoleID).ApprovalRoleName;
                    if (item.CategoryTypeID.HasValue)
                    {
                        ap.CategoryTypeID = (int)item.CategoryTypeID;
                        ap.CategoryType = CategoryTypeTable.GetItem(_db, (int)item.CategoryTypeID).CategoryTypeName;
                    }
                    obj.LineItems.Add(ap);
                }

                return PartialView(ed);
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }

        public IActionResult Delete(int id)
        {
            if (!base.HasRights(RightNames.UserMaster, UserRightValue.Delete))
                return RedirectToAction("UnauthorizedPage");

            var pageName = "Person";
            base.TraceLog("Delete", $"{SessionUser.Current.Username} - {pageName} Delete action requested");
            PersonTable person = PersonTable.GetPersonBasedOnID(_db, id);
            var user = User_LoginUserTable.GetExistingUser(_db, id);

            if (person != null)
                person.Delete();
            if (user != null)
            {
                user.IsLockedOut = user.IsDisabled = true;
                user.UserName = string.Concat(user.UserName, '_', user.UserID + "");
            }
            person.UpdateUniqueKey(_db);
            _db.SaveChanges();

            base.TraceLog("Delete", $"{pageName} details page deleted successfully {person.GetPrimaryKeyFieldName()} {id}");
            return RedirectToAction("SuccessAction", new { pageName = pageName });
        }

        public IActionResult DeleteAll(string toBeDeleteIds)
        {
            var pageName = "Person";

            base.TraceLog("DeleteAll", $"{SessionUser.Current.Username} - {pageName} Delete action requested");
            if (!base.HasRights(RightNames.UserMaster, UserRightValue.Delete))
                return RedirectToAction("UnauthorizedPage");

            string checkdItems = toBeDeleteIds;
            string[] arrRequestID = checkdItems.Split(',');
            int[] requestID = Array.ConvertAll(arrRequestID, s => int.Parse(s));
            int requestCount = requestID.Count();
            if (requestCount > 0)
            {
                foreach (var id in requestID)
                {
                    PersonTable person = PersonTable.GetPersonBasedOnID(_db, id);
                    var user = User_LoginUserTable.GetExistingUser(_db, id);

                    if (person != null)
                        person.Delete();
                    if (user != null)
                    {
                        user.IsLockedOut = user.IsDisabled = true;
                        user.UserName = string.Concat(user.UserName, '_', user.UserID + "");
                    }
                    person.UpdateUniqueKey(_db);
                    _db.SaveChanges();
                }
            }
            base.TraceLog("Delete", $"{pageName} details page deleted successfully");
            return RedirectToAction("SuccessAction", new { pageName = pageName });
        }
        [HttpPost]
        public async Task<ActionResult> AddLineItem(IFormCollection collection, int currentPageID, int locationID, int approvalroleID, int categorytypeID)
        {
            try
            {
                base.TraceLog("UserMaster", $"{SessionUser.Current.Username} -  AddLineItem requested");
                var obj = ApprovalRoleDataModel.GetModel(currentPageID);
                if (obj.LineItems.Count() > 0)
                {
                    var validation = obj.LineItems.Where(a => a.LocationID == locationID && a.ApprovalRoleID == approvalroleID && a.CategoryTypeID == categorytypeID).FirstOrDefault();
                    if (validation == null)
                    {
                        ApprovalRoleModel ap = new ApprovalRoleModel();
                        ap.LocationID = locationID;
                        ap.ApprovalRoleID = approvalroleID;
                        ap.CategoryTypeID = categorytypeID;
                        ap.CategoryType = CategoryTypeTable.GetItem(_db, ap.CategoryTypeID).CategoryTypeName;
                        ap.LocationName = LocationTable.GetItem(_db, ap.LocationID).LocationName;
                        ap.ApprovalRoleName = ApprovalRoleTable.GetItem(_db, ap.ApprovalRoleID).ApprovalRoleName;
                        obj.LineItems.Add(ap);
                    }
                    else
                    {
                        return base.ErrorActionResult("Already added Location and Approval Role, Please select other data");
                    }
                }
                else

                {
                    ApprovalRoleModel ap = new ApprovalRoleModel();
                    ap.LocationID = locationID;
                    ap.ApprovalRoleID = approvalroleID;
                    ap.CategoryTypeID = categorytypeID;
                    ap.CategoryType = CategoryTypeTable.GetItem(_db, ap.CategoryTypeID).CategoryTypeName;
                    ap.LocationName = LocationTable.GetItem(_db, ap.LocationID).LocationName;
                    ap.ApprovalRoleName = ApprovalRoleTable.GetItem(_db, ap.ApprovalRoleID).ApprovalRoleName;
                    obj.LineItems.Add(ap);

                }
                base.TraceLog("UserMaster", $"{SessionUser.Current.Username} -  AddLineItem requested done");
                return Content("");
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }
        public IActionResult Approvalindex([DataSourceRequest] DataSourceRequest request, int currentPageID)
        {
            base.TraceLog("UserMaster", $"{SessionUser.Current.Username} -  Approvalindex requested");
            var query = ApprovalRoleDataModel.GetModel(currentPageID).LineItems;
            var dsResult = query.ToDataSourceResult(request);
            // var dsResult = request.ToDataSourceResult(query.AsQueryable(), "UniformRequestUniformItemEntry", "UniformRequestLineItemID");
            this.TraceLog("Index", $"{SessionUser.Current.Username} - Index page Data Fetch");

            return Json(dsResult);
        }
        public IActionResult _DeleteLineItem(int id, int currentPageID)
        {
            base.TraceLog("UserMaster", $"{SessionUser.Current.Username} -  _DeleteLineItem requested");
            var dataModel = ApprovalRoleDataModel.GetModel(currentPageID);
            var query = dataModel.LineItems.Where(b => b.ID == id).FirstOrDefault();
            if (query != null)
                dataModel.LineItems.Remove(query);

            return Content("");
        }
        public async Task<ActionResult> SignatureUpload(IFormFile Signatureimage, int currentPageID)
        {
            try
            {
                base.TraceLog("UserMaster", $"{SessionUser.Current.Username} -  SignatureUpload requested");
                //string absolutePath = DateTime.Now.ToString("yyyy_MM_dd");
                var rootPath = WebHostEnvironment.WebRootPath;
                string fullPath = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/SignaturePath");
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }


                // The Name of the Upload component is "files".
                if (Signatureimage != null)
                {
                    var fileContent = ContentDispositionHeaderValue.Parse(Signatureimage.ContentDisposition);

                    // Some browsers send file names with full path.
                    // The sample is interested only in the file name.
                    string fileExtension = System.IO.Path.GetExtension(Path.GetFileName(fileContent.FileName.ToString().Trim('"')));
                    string time = ComboBoxHelper.RemoveSpecialCharacters(String.Format("{0:" + CultureHelper.DateTimeFormatForGrid + "}", DateTime.Now));
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(fileContent.FileName.ToString().Trim('"').Replace(" ", ""));

                    string newFileName = fileName + "" + time + "" + fileExtension;
                    fullPath = Path.Combine(fullPath, newFileName);

                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await Signatureimage.CopyToAsync(fileStream);
                    }

                    //var fileName = Path.GetFileName(fileContent.FileName.ToString().Trim('"'));
                    //    string fileExtension = System.IO.Path.GetExtension(fileName);
                    //    var newFileName = Guid.NewGuid().ToString() + fileExtension;
                    //    fullPath = Path.Combine(fullPath, newFileName);

                    //    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    //    {
                    //        await SignaturePath.CopyToAsync(fileStream);
                    //    }

                    TempData["UploadedFile"] = fullPath;
                    var physicalPath = fullPath.Replace(string.Concat(rootPath, "\\"), "");
                    base.TraceLog("UserMaster", $"{SessionUser.Current.Username} -  SignatureUpload request done");
                    return Json(new { ImageName = physicalPath, fileName = newFileName });

                }
                // Return an empty string to signify success.
                return Content("");

            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }

        }

        public ActionResult SignatureRemove(string fileNames)
        {
            // The parameter of the Remove action must be called "fileNames".
            base.TraceLog("UserMaster", $"{SessionUser.Current.Username} -  SignatureRemove requested");
            if (fileNames != null)
            {

                var fileName = Path.GetFileName(fileNames);
                var physicalPath = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/SignaturePath", fileName);

                // TODO: Verify user permissions.

                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }

            }
            base.TraceLog("UserMaster", $"{SessionUser.Current.Username} -  SignatureRemove request done");
            // Return an empty string to signify success.
            return Content("");
        }

        //public async Task<ActionResult> StampUpload(IFormFile StampPath, int currentPageID)
        //{
        //    try
        //    {
        //        string absolutePath = DateTime.Now.ToString("yyyy_MM_dd");
        //        string fullPath = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/StampPath", absolutePath);
        //        if (!Directory.Exists(fullPath))
        //        {
        //            Directory.CreateDirectory(fullPath);
        //        }


        //        // The Name of the Upload component is "files".
        //        if (StampPath != null)
        //        {

        //            var fileContent = ContentDispositionHeaderValue.Parse(StampPath.ContentDisposition);

        //            // Some browsers send file names with full path.
        //            // The sample is interested only in the file name.
        //            var fileName = Path.GetFileName(fileContent.FileName.ToString().Trim('"'));
        //            string fileExtension = System.IO.Path.GetExtension(fileName);
        //            var newFileName = Guid.NewGuid().ToString() + fileExtension;
        //            fullPath = Path.Combine(fullPath, newFileName);

        //            using (var fileStream = new FileStream(fullPath, FileMode.Create))
        //            {
        //                await StampPath.CopyToAsync(fileStream);
        //            }

        //            TempData["UploadedStampFile"] = fullPath;
        //        }
        //        // Return an empty string to signify success.
        //        return Content("");

        //    }
        //    catch (Exception ex)
        //    {
        //        return ErrorActionResult(ex);
        //    }

        //}

        //public ActionResult StampRemove(string fileNames)
        //{
        //    // The parameter of the Remove action must be called "fileNames".

        //    if (fileNames != null)
        //    {

        //        var fileName = Path.GetFileName(fileNames);
        //        var physicalPath = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/StampPath", fileName);

        //        // TODO: Verify user permissions.

        //        if (System.IO.File.Exists(physicalPath))
        //        {
        //            System.IO.File.Delete(physicalPath);
        //        }

        //    }

        //    // Return an empty string to signify success.
        //    return Content("");
        //}

        public async Task<ActionResult> StampPathUpload(IFormFile StampPath1, int currentPageID)
        {
            try
            {
                base.TraceLog("UserMaster", $"{SessionUser.Current.Username} -  StampPathUpload requested");
                var rootPath = WebHostEnvironment.WebRootPath;
                //string absolutePath = DateTime.Now.ToString("yyyy_MM_dd");
                string fullPath = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/StampPath");
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }


                // The Name of the Upload component is "files".
                if (StampPath1 != null)
                {
                    var fileContent = ContentDispositionHeaderValue.Parse(StampPath1.ContentDisposition);

                    // Some browsers send file names with full path.
                    // The sample is interested only in the file name.
                    string fileExtension = System.IO.Path.GetExtension(Path.GetFileName(fileContent.FileName.ToString().Trim('"')));
                    string time = ComboBoxHelper.RemoveSpecialCharacters(String.Format("{0:" + CultureHelper.DateTimeFormatForGrid + "}", DateTime.Now).Replace(" ", ""));
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(fileContent.FileName.ToString().Trim('"'));

                    string newFileName = fileName + "" + time + "" + fileExtension;
                    fullPath = Path.Combine(fullPath, newFileName);

                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await StampPath1.CopyToAsync(fileStream);
                    }



                    TempData["StampUploadedFile"] = fullPath;
                    //return Content("");
                    var physicalPath = fullPath.Replace(string.Concat(rootPath, "\\"), "");
                    base.TraceLog("UserMaster", $"{SessionUser.Current.Username} -  StampPathUpload request done");
                    return Json(new { ImageName = physicalPath, fileName = newFileName, rootpath = string.Concat(rootPath, "\\") });
                }

                // Return an empty string to signify success.
                return Content("");
                //return Json(new { ImageName = newFileName }, "text/plain");
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }

        public ActionResult StampRemove(string fileNames)
        {
            // The parameter of the Remove action must be called "fileNames".
            base.TraceLog("UserMaster", $"{SessionUser.Current.Username} -  StampRemove requested");
            if (fileNames != null)
            {
                var fileName = Path.GetFileName(fileNames);
                var physicalPath = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/StampPath", fileName);

                // TODO: Verify user permissions.

                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }
            }

            base.TraceLog("UserMaster", $"{SessionUser.Current.Username} -  StampRemove request done");
            // Return an empty string to signify success.
            return Content("");
        }
    }
}