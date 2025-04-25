using ACS.AMS.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using ACS.AMS.DAL;
using System;
using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using Telerik.Reporting.Processing;

namespace ACS.AMS.WebApp.Attributes
{
    public class IsAuthenticatedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Controller is LoginController)
                return;
            if (context.Controller is TransactionApprovalController)
            {
                string actionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;
                if (string.Compare(actionName, "EmailEdit") == 0 || string.Compare(actionName, "EmailView") == 0 || string.Compare(actionName, "DownloadApprovalFile") == 0
                    || string.Compare(actionName, "EmailDetails") == 0|| string.Compare(actionName, "_LineItemindex") == 0 || string.Compare(actionName, "ApprovalProcess") == 0
                    || string.Compare(actionName, "_LineItemApproval") == 0 || string.Compare(actionName, "DocumentUpload") == 0 ||  string.Compare(actionName, "DocumentRemove") == 0
                    || string.Compare(actionName, "DownloadFile") == 0 || string.Compare(actionName, "_LineItememailindex") == 0 || string.Compare(actionName, "_LineItemEmailApproval") == 0
                    )
                {
                    return;
                }

            }
            //if (context.Controller is MobileAPIController)
            //    return;

            if (SessionUser.IsAuthenticated == false)
            {
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    //create default account
                    var person = PersonTable.GetPerson(AMSContext.CreateNewContext(), "Admin");
                    var company = PersonTable.GetAllCompanyDetails(AMSContext.CreateNewContext(), person.PersonID);
                    if (company.Count() > 0)
                    {
                        int companyID = company.Select(a => a.CompanyID).First();
                        SessionUser sessionuser = SessionUser.CreateSessionUserObject("Admin", person.PersonID, 1, 1, "en-US", 1, companyID);
                    }
                    else
                    {
                       
                        context.Result = new RedirectToActionResult(nameof(LoginController.LoadLoginPage), "Login", null);
                    }
                       
                    LogHelper.AddTraceLog("DefaultLogin", $"Session object created");

                    //context.HttpContext.Session.SetInt32(ACSBaseController.SessionUserID, 1);
                    //context.HttpContext.Session.SetString(ACSBaseController.SessionUserName, "admin");
                }
                else
                {
                    context.Result = new RedirectToActionResult(nameof(LoginController.LoadLoginPage), "Login", null);
                }
            }

            //if (!context.HttpContext.User.Identity.IsAuthenticated)
            //    context.Result = new RedirectToActionResult(nameof(AccountController.Login), "Account", null);

            //var wh = context.HttpContext.Session[].ToString();
        }
    }
}
