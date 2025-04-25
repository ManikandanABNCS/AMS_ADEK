//using ACS.DHL.KioskApp.ServiceHelper;
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
using System.Net.Mail;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Hosting;

namespace ACS.AMS.WebApp.Controllers
{
    public class LoginController : ACSBaseController
    {
        protected static IHttpContextAccessor _httpContextAccessor;
        public LoginController(IHttpContextAccessor httpContextAccessor)
        {            
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

		public async Task<ActionResult> LoadLoginPage()
		{
			return PartialView();
		}

		public async Task<ActionResult> Login()
        {
            ViewBag.Context = _db;
            CultureHelper.setHttpContextAccessor(_httpContextAccessor);
            Language.setHttpContextAccessor(_httpContextAccessor);

            return View(new LoginModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(IFormCollection data)
        {
            try
            {
                string UserName = data["UserName"];
				string password = data["Password"];
				
                base.TraceLog("New Login-Post", $"Login requested. username:{UserName}, password:*****");
                if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(password))
                {
                    //_db.EnableInstanceQueryLog = true;

                    string validate = User_LoginUserTable.ValidateUser(_db, UserName, password);
                    
                    base.TraceLog("New Login-Post", $"Login validation, result: {validate}");

                    if (string.Compare(validate, "Success") == 0)
                    {
                        var userDetails = User_LoginUserTable.GetUser(_db, UserName);
                        var person = PersonTable.GetPersonBasedOnID(_db, userDetails.UserID);
                        var company = PersonTable.GetAllCompanyDetails(_db, userDetails.UserID);

                        int languageID = LanguageTable.GetAllLanguageTable(_db).Where(a => a.CultureSymbol == person.Culture).Select(a => a.LanguageID).FirstOrDefault();
                        if (company.Count() > 0)
                        {
                           // var companyList = company.Select(a => a.CompanyID).Distinct().ToList();
                            //if (companyList.Count() == 1)
                            //{
                                int companyID = company.Select(a => a.CompanyID).First();
                            //}
                            SessionUser sessionuser = SessionUser.CreateSessionUserObject(userDetails.UserName, userDetails.UserID,
                          userDetails.UserID, 1, person.Culture, languageID, companyID);

                            base.TraceLog("New Login-Post", $"Session object created");
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, userDetails.UserName),
                                new Claim("FullName", userDetails.UserName),
                                new Claim(ClaimTypes.Role, "Administrator"),
                            };

                            var claimsIdentity = new ClaimsIdentity(
                                claims, CookieAuthenticationDefaults.AuthenticationScheme);

                            await HttpContext.SignInAsync(
                                CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(claimsIdentity),
                                new AuthenticationProperties
                                {
                                    IsPersistent = false,
                                    ExpiresUtc = DateTime.UtcNow.AddMinutes(2)
                                });

                            return RedirectToAction("Index", "Dashboard");
                        }
                        else
                        {
                            base.TraceLog("New Login-Post", $"Empty username or password");
                            ViewData["ErrorMesg"] = "User Not Mapped with Company Details";

                            return View();
                        }
                              
                    }
                    else
                    {
                        ViewData["ErrorMesg"] = validate;
                        return View();
                    }
                }
                else
                {
                    base.TraceLog("New Login-Post", $"Empty username or password");
                    ViewData["ErrorMesg"] = "Enter valid Username and Password";

                    return View();
                }
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }

        public async Task<IActionResult> LogOut(string returnUrl = null)
        {
            base.TraceLog("LogOut", $"LogOut called");
            // Clear the existing external cookie
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            string url = "/Login/Login";
            // return RedirectToAction("Login", "Login");
             return Redirect(url);
           // return RedirectToAction("LoadLoginPage");
        }
        public ActionResult ResetPassword()
        {
            base.TraceLog("ResetPassword", $"ResetPassword called");
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(ForgetPasswordModel forgetDetails, IFormCollection data)
        {
            base.TraceLog("ResetPassword-post", $"ResetPassword called");
            string emailID = data["Email"].ToString();
            var userName1 = PersonTable.GetPersonByEmail(_db, emailID);
            if (userName1 != null)
            {

                var username = User_LoginUserTable.GetExistingUser(_db, userName1.PersonID);
                if (username != null)
                {
                    int passwordLength = 8;
                    //var user = Hi5Soft.Security.Hi5MembershipHandler.Provider.GetUser(username.UserName, false);

                    string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
                    char[] chars = new char[passwordLength];
                    Random rd = new Random();

                    for (int i = 0; i < passwordLength; i++)
                    {
                        chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
                    }

                    string pwd = new string(chars);
                    if (!string.IsNullOrEmpty(pwd))
                    {
                        username.PasswordSalt = EncryptionManager.EncryptPassword(pwd);
                        username.Password = EncryptionManager.EncryptPassword(pwd + username.PasswordSalt);
                    }

                    _db.SaveChanges();
                    //Hi5MembershipHandler.Provider.ResetPassword(username.UserName, pwd);
                   // SendChangeEmail(username.UserName, pwd, userName1.EMailID);

                  

                    var userObject = User_LoginUserTable.GetUser(_db,username.UserName);
                    int userID = (int)userObject.UserID;
                    var loginUser = (from b in _db.User_LoginUserTable
                                     where b.UserID == userID
                                     select b).FirstOrDefault();
                    loginUser.IsDisabled = false;
                    loginUser.LastLoggedInDate = null;
                    _db.SaveChanges();
                    ViewBag.NewPassword = pwd;
                    return PartialView("PasswordResetFinal");
                }
                else
                {
                    ViewData["ErrorMesg"] = "The Entered Email Address is not Registered with us,Please enter valid email address";
                   
                }
            }
            else
            {
                ViewData["ErrorMesg"] = "The Entered Email Address is not Registered with us,Please enter valid email address";
            }
            return View(forgetDetails);
        }

        public ActionResult PasswordResetFinal(string userName, string answer)
        {
            base.TraceLog("PasswordResetFinal", $"PasswordResetFinal called");
            return PartialView();
        }

        

    }
}