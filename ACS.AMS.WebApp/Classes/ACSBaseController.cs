using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using ACS.AMS.DAL;
using ACS.AMS.DAL.DBContext;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using ACS.AMS.WebApp.Attributes;
using ACS.AMS.DAL.DBModel;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ACS.AMS.WebApp
{
    //[License]
    [IsAuthenticated]
    [RequestFormLimits(ValueCountLimit = 5000)]
    public class ACSBaseController : Controller
    {
        //protected AMSContext _db;
        protected AMSContext _db = AMSContext.CreateNewContext();
        //public const string SessionUserID = "_'userID";
        //public const string SessionUserName = "_'userName";

        public int loggedUserID { get; set; }

        protected string _RightName = "";
        protected string _TransactionColumnIndexName = "";
        protected string _TransactionLinesColumnIndexName = "";

        public ACSBaseController()
        {

        }

        #region Log Data

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //set the culture info
            Thread.CurrentThread.CurrentUICulture =
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-GB");

            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //set the culture info
           
            base.OnActionExecuted(context);
            
        }

        #endregion

        protected JsonResult ErrorJsonResult(string errorMessage)
        {
            //Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //Response.StatusCode = 500;
            this.TraceLog("Error", $"{SessionUser.Current.Username} - {errorMessage}");
            var errorResult = new { ErrorMessage = errorMessage };
            return Json(errorResult);
            //return new JavaScriptResult("alert('" + errorMessage + "');");
        }

        protected JsonResult ErrorJsonResult(Exception ex)
        {
            string errorID = "";
            if (!(ex is ValidationException))
                errorID = "-" + ApplicationErrorLogTable.SaveException(ex, null);

            if (ex is ValidationException)
                return ErrorJsonResult(ex.Message);

            if (ex is DbUpdateException)
            {
                //return the DB error, if it is custom one
                var inner = ex.InnerException;
                while (inner != null)
                {
                    if (inner is System.Data.SqlClient.SqlException)
                    {
                        var errors = ((System.Data.SqlClient.SqlException)inner).Errors;
                        if (errors.Count > 0)
                        {
                            if (errors[0].Class == 11)
                            {
                                errorID = "-" + ApplicationErrorLogTable.SaveException(ex, null);
                                return ErrorJsonResult("ErrorID" + errorID + " " + errors[0].Message);
                            }
                        }
                        break;
                    }

                    inner = inner.InnerException;
                }
            }

            //return ErrorActionResult(ex.Message);
            return ErrorJsonResult("UnknownErrorOccurred" + errorID);
        }

        protected ActionResult ErrorActionResult(string errorMessage)
        {
            this.TraceLog("InternalServerError", $"{SessionUser.Current.Username} - {errorMessage}");
            //Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //Response.StatusCode = 500;
            return Content(errorMessage);
            //return new JavaScriptResult("alert('" + errorMessage + "');");
        }

        protected ActionResult GotoUnauthorizedPage()
        {
            return RedirectToAction("UnauthorizedPage");
        }

        protected ActionResult ErrorActionResult(Exception ex)
        {
            string errorID = "";
            if (!(ex is ValidationException))
                errorID = "-" + ApplicationErrorLogTable.SaveException(ex, null);

            if (ex is ValidationException)
                return ErrorActionResult(ex.Message);


            if (ex is DbUpdateException)
            {
                //return the DB error, if it is custom one
                var inner = ex.InnerException;
                while (inner != null)
                {
                    if (inner is System.Data.SqlClient.SqlException)
                    {
                        var errors = ((System.Data.SqlClient.SqlException)inner).Errors;
                        if (errors.Count > 0)
                        {
                            if (errors[0].Class == 11)
                            {
                                errorID = "-" + ApplicationErrorLogTable.SaveException(ex, null);
                                return ErrorJsonResult("Error" + errorID+" "+ errors[0].Message);
                            }
                        }
                        break;
                    }

                    inner = inner.InnerException;
                }
            }

            //return ErrorActionResult(ex.Message);
            return ErrorActionResult("UnknownErrorOccurred" + errorID);
        }

        //protected JavaScriptResult ValidationMessage(string errorMessage, string url = null, string details = null)
        //{
        //    if (string.IsNullOrEmpty(url))
        //    {
        //        return new JavaScriptResult { Script = "KendoErrorMsg('" + Language.GetErrorMessage(errorMessage) + "');" };
        //    }
        //    else if (string.IsNullOrEmpty(details))
        //    {
        //        return new JavaScriptResult { Script = "showSuccessMessage('" + Language.GetString(errorMessage) + "');loadDefaultPage('" + Url.Action(url) + "');" };
        //        //return new JavaScriptResult { Script = "showSuccessMessage('" + Language.GetString(errorMessage) + "');" };
        //    }
        //    else
        //    {
        //        return new JavaScriptResult { Script = "showSuccessMessage('" + Language.GetString(errorMessage) + "');loadDefaultPage('" + Url.Action(url, new { chkID = details }) + "');" };
        //    }
        //}

        public IActionResult SuccessAction(string pageName,string actionName="",string controlName="",string controlValue="",string functionName = "",string param="")
        {
            if (TempData["Message"] != null)
                ViewData["Message"] = TempData["Message"];

            if (TempData["URL"] != null)
                ViewData["URL"] = TempData["URL"];
            
            if (TempData["URLControl"] != null)
                ViewData["URLControl"] = TempData["URLControl"];
            
            if (TempData["MessageSuffix"] != null)
                ViewData["MessageSuffix"] = TempData["MessageSuffix"];
            
            if (TempData["ExternalReportPageURL"] != null)
                ViewData["ExternalReportPageURL"] = TempData["ExternalReportPageURL"];

            if(!string.IsNullOrEmpty(pageName))
                ViewData["PageName"] = pageName;
            if(!string.IsNullOrEmpty(actionName))
            {
                ViewData["PopupwindowClose"] = actionName;
            }
            if (!string.IsNullOrEmpty(controlName))
            {
                ViewData["controlName"] = controlName;
            }
            if (!string.IsNullOrEmpty(controlValue))
            {
                ViewData["controlValue"] = controlValue;
            }
            if (!string.IsNullOrEmpty(functionName))
            {
                ViewData["functionName"] = functionName;
                ViewData["param"] = param;
            }

            return View();
        }

        public IActionResult ErrorAction()
        {
            ViewData["Message"] = TempData["Message"];
            return View();

        }
        protected bool HasRights(string rightName, UserRightValue right)
        {
            return SessionUser.HasRights(rightName, right);
        }
        public ActionResult UnauthorizedPage()
        {
            return PartialView();
        }

        #region Debug/Trace Logs

        protected virtual void SaveExceptionAndTraceLog(string errorMessage, [CallerMemberName] string method = null,
            [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = -1)
        {
            ApplicationErrorLogTable.SaveException(new Exception(errorMessage));
            TraceLog("", errorMessage, method, filePath, lineNumber);
        }

        protected virtual void TraceLog(string functionality, string message, [CallerMemberName] string method = null,
            [CallerFilePath] string filePath =  null, [CallerLineNumber] int lineNumber = -1)
        {
            var sessionID = "<NO_SESSION>";
            if (base.HttpContext.Session != null)
                sessionID = base.HttpContext.Session.Id;

            message = $"SID: {sessionID}, {message}";
            LogHelper.AddTraceLog(functionality, message, method, filePath, lineNumber);
        }

        #endregion

        #region Page Actions

        protected virtual Type GetEntityObjectType(string entityName,bool indexScreen=false)
        {
            return EntityHelper.GetEntityObjectType(entityName, indexScreen);
        }

        #endregion

        [HttpPost]
        public IActionResult Export(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        public IActionResult ExceptionAction()
        {
            if (TempData["CancelGridPopUp"] != null)
            {
                ViewData["CancelGridPopUp"] = TempData["CancelGridPopUp"];
                if (TempData["Url"] != null)
                    ViewData["Url"] = TempData["Url"];
            }
            if (TempData["Exception"] != null)
            {
                ViewData["Exception"] = TempData["Exception"];

                if (TempData["Message"] != null)
                    ViewData["Message"] = TempData["Message"];

                if (TempData["URL"] != null)
                    ViewData["URL"] = TempData["URL"];
            }
        
            return View();
        }
      
        public IActionResult ValidationMessage(string errorMessage, string url = null, string details = null)
        {
            TempData["Exception"] = true;
            TempData["Message"] = errorMessage;
            return RedirectToAction("ExceptionAction");
        }
    }
    public class JavaScriptResult : ContentResult
    {
        public JavaScriptResult(string script)
        {
            this.Content = script;
            this.ContentType = "application/javascript";
        }
    }
}