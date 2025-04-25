using System.Text;
using Kendo.Mvc.Extensions;
using ACS.AMS.DAL;
using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Controllers;
using ACS.AMS.WebApp.Models.Mobile;
using ACS.AMS.WebApp.Models;
using ACS.AMS.WebApp.Classes;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
//using ACS.AMS.WebApp.Models.Mobile.Data;
using System.Globalization;
using Azure;
using Telerik.SvgIcons;

using Microsoft.AspNetCore.Authorization;
using ACS.LMS.WebApp.Models.Mobile;
using ACS.AMS.WebApp.Products.Mobile;
using Org.BouncyCastle.Asn1.X509.SigI;


namespace ACS.AMS.WebApp
{
    public class MobileAPIController : ACSBaseController
    {
        private IConfiguration Configuration;
        private IWebHostEnvironment Environment;
        private static int fileCount = 0;
        private static object threadObject = new object();
        public string syncStatus = string.Empty;

        private const string DecimalFormat = "0.000";

        private DateTime transactionStartDateTime = DateTime.Now.Date.AddDays(-2);

        public MobileAPIController(AMSContext db, IConfiguration _configuration, IWebHostEnvironment _environment)
        {
            _db = db;
            Configuration = _configuration;
            Environment = _environment;
        }

       #region Request & Response Handling

         private WebServiceLogTable serviceLog = new WebServiceLogTable();
        AMSContext serviceLogDB = AMSContext.CreateNewContext();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                StringBuilder pms = new StringBuilder();
                foreach (var itms in filterContext.ActionArguments.Keys)
                {
                    var val1 = filterContext.ActionArguments[itms];

                    switch (itms.ToUpper())
                    {
                        case "APPVERSION":
                            serviceLog.AppVersion = val1 + "";
                            continue;

                        case "DEVICEID":
                            serviceLog.DeviceIMEI = val1 + "";
                            continue;

                        case "USERID":
                            {
                                int uId = -1;
                                int.TryParse(val1 + "", out uId);
                                serviceLog.UserID = uId;
                            }
                            continue;

                        case "WAREHOUSECODE":
                            serviceLog.WarehouseCode = val1 + "";
                            continue;

                        case "FILECONTENT":
                            //dont save the file contents
                            continue;

                        //case "DATA":
                        //    {
                        //        var transData = val1 as ScheduleData;
                        //        if (transData != null)
                        //        {
                        //            pms.Append($"DepartmentID={transData.DepartmentID}&");
                        //            pms.Append($"ScheduleDate={transData.ScheduleDate}&");
                        //            pms.Append($"EmployeeIdsCount={transData.EmployeeIds.Count}&");
                        //        }
                        //        continue;
                        //    }
                        default:
                            {
                                pms.Append(itms);
                                pms.Append("=");
                                pms.Append(val1);
                                pms.Append("&");
                                break;
                            }
                    }
                }

                //+filterContext.ActionDescriptor.GetType()    { Name = "ControllerActionDescriptor" FullName = "Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor"}
                //System.Type { System.RuntimeType}

                var ad = filterContext.ActionDescriptor as ControllerActionDescriptor;
                if (ad != null)
                    serviceLog.MethodName = ad.ActionName;
                else
                {
                    var dName = filterContext.ActionDescriptor.DisplayName;
                    if (!string.IsNullOrWhiteSpace(dName))
                    {
                        if (dName.Length > 99)
                            serviceLog.MethodName = dName.Substring(dName.Length - 90, 90);
                        else
                            serviceLog.MethodName = dName;
                    }
                }

                serviceLog.Parameters = pms.ToString();
                serviceLog.RequestedDateTime = DateTime.Now;

                serviceLogDB.Add(serviceLog);
                serviceLogDB.SaveChanges();
            }
            catch (Exception ex)
            {
                ApplicationErrorLogTable.SaveException(ex);
            }

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                serviceLog.TimeTakenToCompleteService = (int)((DateTime.Now - serviceLog.RequestedDateTime).TotalMilliseconds);

                var result = filterContext.Result as JsonResult;
                if ((result != null) && (result.Value != null))
                {
                    var data = result.Value as BaseResponse;
                    if (data != null)
                    {
                        if (data.IsSuccess)
                            serviceLog.Response = "Success";
                        else
                            serviceLog.Response = "ERROR: " + data.Message;
                    }
                    else
                    {
                        serviceLog.Response = "Invalid Result Data 1";
                    }
                }
                else
                {
                    serviceLog.Response = "Invalid Result Data 2";
                }

                serviceLogDB.SaveChanges();
                serviceLogDB.Dispose();
            }
            catch (Exception ex)
            {
                ApplicationErrorLogTable.SaveException(ex);
            }

            base.OnActionExecuted(filterContext);
        }

        #endregion

        #region Error/Common methods

        protected virtual ActionResult ErrorActionResult<T>(Exception ex, bool dataSaved = true, bool dataPosted = true)
        {
            try
            {
                BaseResponse model = Activator.CreateInstance<T>() as BaseResponse;
                HandleModelException(model, ex, dataSaved, dataPosted);

                return Json(model);
            }
            catch (Exception ex1)
            {
                return Json(ex1.Message);
            }
        }

        protected virtual ActionResult ErrorActionResult<T>(string errorMessage)
        {
            BaseResponse model = Activator.CreateInstance<T>() as BaseResponse;
            model.IsSuccess = false;
            model.Message = errorMessage;

            return Json(model);
        }

        protected virtual ActionResult ErrorActionResult(BaseResponse model, Exception ex)
        {
            HandleModelException(model, ex);

            return Json(model);
        }

        private void HandleModelException(BaseResponse model, Exception ex, bool dataSaved = true, bool dataPosted = true)
        {
            if (ex is ValidationException)
            {
                string errorID = "";

                var dbContext = AMSContext.CreateNewContext();
                errorID = "" + ApplicationErrorLogTable.SaveException(dbContext, ex, base.HttpContext.Request.Path.ToString(), null);
                //return ErrorActionResult(Language.GetErrorMessage("UnknownErrorOccurred") + errorID);

                model.Message = ex.Message + ", Error ID: " + errorID;
            }
            else
            {
                string errorID = "";
                var dbContext = AMSContext.CreateNewContext();
                errorID = "" + ApplicationErrorLogTable.SaveException(dbContext, ex, base.HttpContext.Request.Path.ToString(), null);
                model.Message = "Unknown Error Occurred" + ", Error ID: " + errorID;
            }

            model.IsSuccess = false;
            model.IsDataSaved = dataSaved;
        }

        #endregion

        #region Master/Auth Methods

        #region JE Authentication

        private JwtSecurityToken GetToken(List<Claim> authClaims, bool permanentToken = false)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]));
            DateTime tokenExpiry = DateTime.Now.ToUniversalTime().AddHours(24);

            if (permanentToken)
            {
                tokenExpiry = DateTime.Now.ToUniversalTime().AddYears(10);
            }

            var token = new JwtSecurityToken(
                        issuer: Configuration["JWT:ValidIssuer"],
                        audience: Configuration["JWT:ValidAudience"],
                        expires: tokenExpiry,
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        #endregion
        [HttpGet]
        public ActionResult GenerateJToken(string deviceSerialNo, string userName)
        {
            try
            {
                base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userName}");

                GetAllCompaniesResponse result = new GetAllCompaniesResponse();
                var authClaims = new List<Claim>
                {
                            new Claim(ClaimTypes.Name, userName),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        };

                
                authClaims.Add(new Claim(ClaimTypes.Role, userName));
                var token = GetToken(authClaims, false);// person.Usertypeid == (int)UserTypeValue.KioskDeviceTestUser);

                string tokens = new JwtSecurityTokenHandler().WriteToken(token);
                var results = new BaseResponse()
                {
                    Message = tokens,

                };
                return Json(results, new Newtonsoft.Json.JsonSerializerSettings());
                //return Json(tokens);

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }
        [HttpPost]
        public ActionResult Auth_ValidateUser([FromBody] ValidateUserRequest request)
        {
            try
            {
                if (request is null)
                {
                    throw new ValidationException("Invalid User request data");
                }

                if (string.IsNullOrEmpty(request.Username))
                {
                    throw new ValidationException("Username required for validate user");
                }

                if (string.IsNullOrEmpty(request.Password))
                {
                    throw new ValidationException("Password required for validate user");
                }

                string validate = User_LoginUserTable.ValidateUser(_db, request.Username, request.Password);
                if (string.Compare(validate, "Success") == 0)
                {
                    var user = User_LoginUserTable.GetUser(_db, request.Username);

                    var response = new ValidateUserResponse();
                    response.UserID = user.UserID;

                    var person = PersonTable.GetItem(_db, user.UserID);
                    if (person.StatusID == (int)StatusValue.Deleted)
                    {
                        ErrorActionResult<ValidateUserResponse>($"Enter valid Username and Password");
                    }

                   // if ((person.UserTypeID == (int)UserTypeValue.Mobile) || (person.UserTypeID == (int)UserTypeValue.WebMobile))
                    {
                        //validate locationID
                        var companyMapping = UserCompanyMappingTable.GetCompanyForPersonID(_db, request.UserID);
                        
                        if (companyMapping.Count()== 0)
                            return ErrorActionResult<ValidateUserResponse>("Selected user not mapped with Company");

                        response.Fullname = $"{person.PersonFirstName} {person.PersonLastName}";
                        response.AllowedRights = new List<string>();
                        //response.CompanyID = companyMapping.;
                        //response.LocationName = linenRoom.LinenRoomName;

                        TraceLog($"User validation Ok - {request.Username}", response.UserID+"", request.Username, request.DeviceSerialNo);
                        var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        };

                        //add role as admin
                        authClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                        var token = GetToken(authClaims, false);// person.Usertypeid == (int)UserTypeValue.KioskDeviceTestUser);
                        response.expiration = token.ValidTo;
                        response.token = new JwtSecurityTokenHandler().WriteToken(token);

                        return Json(response);
                    }
                    //else
                    //{
                    //    return ErrorActionResult<ValidateUserResponse>("Mobile login restricted");
                    //}
                }
                else
                {
                    TraceLog($"validation failed for user - {request.Username}", "");
                    return ErrorActionResult<ValidateUserResponse>(validate);
                }
            }
            catch (Exception ex)
            {
                //TraceLog(ex.Message);
                return ErrorActionResult<ValidateUserResponse>(ex);
            }
        }

        [Authorize()]
        [HttpGet]
        public ActionResult GetAllCompanyDetails(string deviceSerialNo)
        {
            try
            {
                base.TraceLog("MobileAPI", $"{deviceSerialNo}");

                GetAllCompaniesResponse result = new GetAllCompaniesResponse();
             
                //load the lines
                result.Companies = (from b in CompanyTable.GetAllItems(_db)
                                    orderby b.CompanyName
                                    select new CompanyData
                                    {
                                        CompanyID = b.CompanyID,
                                        CompanyCode = b.CompanyCode,
                                        CompanyName = b.CompanyName
                                    }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpGet]
        public ActionResult GetAllDepartmentsDetails(string deviceSerialNo)
        {
            try
            {
                base.TraceLog("MobileAPI", $"{deviceSerialNo}");

                GetAllDepartmentResponse result = new GetAllDepartmentResponse();

                //load the lines
                result.Departments = (from b in DepartmentTable.GetAllItems(_db)
                                    orderby b.DepartmentName
                                    select new DepartmentData
                                    {
                                        DepartmentID = b.DepartmentID,
                                        DepartmentCode = b.DepartmentCode,
                                        DepartmentName = b.DepartmentName
                                    }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }
        [Authorize()]
        [HttpPost]
        public ActionResult Save_DepartmentDetails(DapartmentDataProcessData saveData)
        {
            try
            {
                if (saveData.department == null)
                {
                    
                    throw new ValidationException(Language.GetString("DataRequired"), null, "DepartmentData");
                }
                DepartmentTable dep = new DepartmentTable();
                if (string.IsNullOrEmpty(saveData.department.DepartmentCode))
                {
                    throw new ValidationException(Language.GetString("DepartmentCodeRequired"), null, "DepartmentCode");
                }
                if (string.IsNullOrEmpty(saveData.department.DepartmentName))
                {
                    throw new ValidationException(Language.GetString("DepartmentNameRequired"), null, "DepartmentName");
                }

                dep.DepartmentCode = saveData.department.DepartmentCode;
                dep.DepartmentName = saveData.department.DepartmentName;
                dep.StatusID = (int)StatusValue.Active;
                dep.CreatedBy = 1;
                dep.CreatedDateTime = System.DateTime.Now;

                _db.Add(dep);
                _db.SaveChanges();
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.department.DepartmentCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());
             
            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }
        
        [Authorize()]
        [HttpPost]
        public ActionResult Update_DepartmentDetails(DapartmentDataProcessData saveData)
        {
            try
            {
                if (saveData.department == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "DepartmentData");
                }
                DepartmentTable dep = new DepartmentTable();
                if (string.IsNullOrEmpty(saveData.department.DepartmentCode))
                {
                    throw new ValidationException(Language.GetString("DepartmentCodeRequired"), null, "DepartmentCode");
                }
                if (string.IsNullOrEmpty(saveData.department.DepartmentName))
                {
                    throw new ValidationException(Language.GetString("DepartmentNameRequired"), null, "DepartmentName");
                }
                var depart = (from b in _db.DepartmentTable where b.DepartmentCode == saveData.department.DepartmentCode select b).FirstOrDefault();
                if (depart != null)
                {
                    depart.LastModifiedBy = 1;
                    depart.LastModifiedDateTime = System.DateTime.Now;
                    depart.DepartmentName = saveData.department.DepartmentName;

                    _db.SaveChanges();
                }
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.department.DepartmentCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpPost]
        public ActionResult Delete_DepartmentDetails(DapartmentDataProcessData saveData)
        {
            try
            {
                if (saveData.department == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "DepartmentData");
                }
                DepartmentTable dep = new DepartmentTable();
                if (string.IsNullOrEmpty(saveData.department.DepartmentCode))
                {
                    throw new ValidationException(Language.GetString("DepartmentCodeRequired"), null, "DepartmentCode");
                }
                if (string.IsNullOrEmpty(saveData.department.DepartmentName))
                {
                    throw new ValidationException(Language.GetString("DepartmentNameRequired"), null, "DepartmentName");
                }
                var depart = (from b in _db.DepartmentTable where b.DepartmentCode == saveData.department.DepartmentCode select b).FirstOrDefault();
                if (depart != null)
                {
                    depart.LastModifiedBy = 1;
                    depart.LastModifiedDateTime = System.DateTime.Now;
                    depart.StatusID = (int)StatusValue.Deleted;

                    _db.SaveChanges();
                }
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.department.DepartmentCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpGet]
        public ActionResult GetAllSectionDetails(string deviceSerialNo)
        {
            try
            {
                base.TraceLog("MobileAPI", $"{deviceSerialNo}");

                GetAllSectionsResponse result = new GetAllSectionsResponse();

                //load the lines
                result.Sections = (from b in SectionTable.GetAllItems(_db)
                                      orderby b.SectionName
                                      select new SectionData
                                      {
                                          SectionID = b.SectionID,
                                          SectionCode = b.SectionCode,
                                          SectionName = b.SectionName,
                                          DepartmentID=b.DepartmentID
                                      }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpGet]
        public ActionResult Save_SectionDetails(SectionDataProcessData saveData)
        {
            try
            {
                if (saveData.section == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "SectionData");
                }
                SectionTable dep = new SectionTable();
                if (string.IsNullOrEmpty(saveData.section.SectionCode))
                {
                    throw new ValidationException(Language.GetString("SectionCodeRequired"), null, "SectionCode");
                }
                if (string.IsNullOrEmpty(saveData.section.SectionName))
                {
                    throw new ValidationException(Language.GetString("SectionNameRequired"), null, "SectionName");
                }

                dep.SectionCode = saveData.section.SectionCode;
                dep.SectionName = saveData.section.SectionName;
                dep.DepartmentID = saveData.section.DepartmentID;
                dep.StatusID = (int)StatusValue.Active;
                dep.CreatedBy = 1;
                dep.CreatedDateTime = System.DateTime.Now;

                _db.Add(dep);
                _db.SaveChanges();
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.section.SectionCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpPost]
        public ActionResult Update_SectionDetails(SectionDataProcessData saveData)
        {
            try
            {
                if (saveData.section == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "SectionData");
                }
                SectionTable dep = new SectionTable();
                if (string.IsNullOrEmpty(saveData.section.SectionCode))
                {
                    throw new ValidationException(Language.GetString("SectionCodeRequired"), null, "SectionCode");
                }
                if (string.IsNullOrEmpty(saveData.section.SectionName))
                {
                    throw new ValidationException(Language.GetString("SectionNameRequired"), null, "SectionName");
                }
                if (saveData.section.DepartmentID == null)
                {
                    throw new ValidationException(Language.GetString("DepartmentIDRequired"), null, "DepartmentID");
                }
                var depart = (from b in _db.SectionTable where b.SectionCode == saveData.section.SectionCode select b).FirstOrDefault();
                if (depart != null)
                {
                    depart.LastModifiedBy = 1;
                    depart.LastModifiedDateTime = System.DateTime.Now;
                    depart.SectionName = saveData.section.SectionName;
                    depart.DepartmentID = saveData.section.DepartmentID;

                    _db.SaveChanges();
                }
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.section.SectionCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpPost]
        public ActionResult Delete_SectionDetails(SectionDataProcessData saveData)
        {
            try
            {
                if (saveData.section == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "SectionData");
                }
                SectionTable dep = new SectionTable();
                if (string.IsNullOrEmpty(saveData.section.SectionCode))
                {
                    throw new ValidationException(Language.GetString("SectionCodeRequired"), null, "SectionCode");
                }
                if (string.IsNullOrEmpty(saveData.section.SectionName))
                {
                    throw new ValidationException(Language.GetString("SectionNameRequired"), null, "SectionName");
                }
                if (saveData.section.DepartmentID == null)
                {
                    throw new ValidationException(Language.GetString("DepartmentIDRequired"), null, "DepartmentID");
                }
                var depart = (from b in _db.SectionTable where b.SectionCode == saveData.section.SectionCode select b).FirstOrDefault();
                if (depart != null)
                {
                    depart.LastModifiedBy = 1;
                    depart.LastModifiedDateTime = System.DateTime.Now;
                    depart.StatusID = (int)StatusValue.Deleted;
                 
                    _db.SaveChanges();
                }
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.section.SectionCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpGet]
        public ActionResult GetAllSupplierDetails(string deviceSerialNo)
        {
            try
            {
                base.TraceLog("MobileAPI", $"{deviceSerialNo}");

                GetAllSuppliersResponse result = new GetAllSuppliersResponse();

                //load the lines
                result.Suppliers = (from b in PartyTable.GetAllItems(_db).Where(b=>b.PartyTypeID==1)
                                   orderby b.PartyName
                                   select new SupplierData
                                   {
                                       SupplierID = b.PartyID,
                                       SupplierCode = b.PartyCode,
                                       SupplierName = b.PartyName,
                                       
                                   }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpGet]
        public ActionResult Save_SupplierDetails(SupplierDataProcessData saveData)
        {
            try
            {
                if (saveData.supplier == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "SupplierData");
                }
                PartyTable dep = new PartyTable();
                if (string.IsNullOrEmpty(saveData.supplier.SupplierCode))
                {
                    throw new ValidationException(Language.GetString("SupplierCodeRequired"), null, "SupplierCode");
                }
                if (string.IsNullOrEmpty(saveData.supplier.SupplierName))
                {
                    throw new ValidationException(Language.GetString("SupplierNameRequired"), null, "SupplierName");
                }

                dep.PartyCode = saveData.supplier.SupplierCode;
                dep.PartyName = saveData.supplier.SupplierName;
                dep.PartyTypeID = 1;
                dep.StatusID = (int)StatusValue.Active;
                dep.CreatedBy = 1;
                dep.CreatedDateTime = System.DateTime.Now;

                _db.Add(dep);
                _db.SaveChanges();
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.supplier.SupplierCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpPost]
        public ActionResult Update_SupplierDetails(SupplierDataProcessData saveData)
        {
            try
            {
                if (saveData.supplier == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "SupplierData");
                }
                PartyTable dep = new PartyTable();
                if (string.IsNullOrEmpty(saveData.supplier.SupplierCode))
                {
                    throw new ValidationException(Language.GetString("SupplierCodeRequired"), null, "SupplierCode");
                }
                if (string.IsNullOrEmpty(saveData.supplier.SupplierName))
                {
                    throw new ValidationException(Language.GetString("SupplierNameRequired"), null, "SupplierName");
                }
                
                var depart = (from b in _db.PartyTable where b.PartyCode == saveData.supplier.SupplierCode select b).FirstOrDefault();
                if (depart != null)
                {
                    depart.LastModifiedBy = 1;
                    depart.LastModifiedDateTime = System.DateTime.Now;
                    depart.PartyName = saveData.supplier.SupplierName;
                    depart.PartyTypeID =1;

                    _db.SaveChanges();
                }
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.supplier.SupplierCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpPost]
        public ActionResult Delete_SupplierDetails(SupplierDataProcessData saveData)
        {
            try
            {
                if (saveData.supplier == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "SupplierData");
                }
                PartyTable dep = new PartyTable();
                if (string.IsNullOrEmpty(saveData.supplier.SupplierCode))
                {
                    throw new ValidationException(Language.GetString("SupplierCodeRequired"), null, "SupplierCode");
                }
                if (string.IsNullOrEmpty(saveData.supplier.SupplierName))
                {
                    throw new ValidationException(Language.GetString("SupplierNameRequired"), null, "SupplierName");
                }

                var depart = (from b in _db.PartyTable where b.PartyCode == saveData.supplier.SupplierCode select b).FirstOrDefault();
                if (depart != null)
                {
                    depart.LastModifiedBy = 1;
                    depart.LastModifiedDateTime = System.DateTime.Now;
                    depart.StatusID = (int)StatusValue.Deleted;
                    _db.SaveChanges();
                }
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.supplier.SupplierCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpGet]
        public ActionResult GetAllAssetConditionDetails(string deviceSerialNo)
        {
            try
            {
                base.TraceLog("MobileAPI", $"{deviceSerialNo}");

                GetAllAssetConditionsResponse result = new GetAllAssetConditionsResponse();

                //load the lines
                result.AssetConditions = (from b in AssetConditionTable.GetAllItems(_db)
                                    orderby b.AssetConditionName
                                    select new AssetConditionData
                                    {
                                        AssetConditionID = b.AssetConditionID,
                                        AssetConditionCode = b.AssetConditionCode,
                                        AssetConditionName = b.AssetConditionName,

                                    }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpGet]
        public ActionResult Save_AssetConditionDetails(AssetConditionDataProcessData saveData)
        {
            try
            {
                if (saveData.AssetCondition == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "AssetConditionData");
                }
                AssetConditionTable dep = new AssetConditionTable();
                if (string.IsNullOrEmpty(saveData.AssetCondition.AssetConditionCode))
                {
                    throw new ValidationException(Language.GetString("AssetConditionCodeRequired"), null, "AssetConditionCode");
                }
                if (string.IsNullOrEmpty(saveData.AssetCondition.AssetConditionName))
                {
                    throw new ValidationException(Language.GetString("AssetConditionNameRequired"), null, "AssetConditionName");
                }

                dep.AssetConditionCode = saveData.AssetCondition.AssetConditionCode;
                dep.AssetConditionName = saveData.AssetCondition.AssetConditionName;
                dep.StatusID = (int)StatusValue.Active;
                dep.CreatedBy = 1;
                dep.CreatedDateTime = System.DateTime.Now;

                _db.Add(dep);
                _db.SaveChanges();
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.AssetCondition.AssetConditionCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpPost]
        public ActionResult Update_AssetConditionDetails(AssetConditionDataProcessData saveData)
        {
            try
            {
                if (saveData.AssetCondition == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "AssetConditionData");
                }
                PartyTable dep = new PartyTable();
                if (string.IsNullOrEmpty(saveData.AssetCondition.AssetConditionCode))
                {
                    throw new ValidationException(Language.GetString("AssetConditionCodeRequired"), null, "AssetConditionCode");
                }
                if (string.IsNullOrEmpty(saveData.AssetCondition.AssetConditionName))
                {
                    throw new ValidationException(Language.GetString("AssetConditionRequired"), null, "AssetCondition");
                }

                var depart = (from b in _db.AssetConditionTable where b.AssetConditionCode == saveData.AssetCondition.AssetConditionCode select b).FirstOrDefault();
                if (depart != null)
                {
                    depart.LastModifiedBy = 1;
                    depart.LastModifiedDateTime = System.DateTime.Now;
                    depart.AssetConditionName = saveData.AssetCondition.AssetConditionName;
                    _db.SaveChanges();
                }
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.AssetCondition.AssetConditionCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpPost]
        public ActionResult Delete_AssetConditionDetails(AssetConditionDataProcessData saveData)
        {
            try
            {
                if (saveData.AssetCondition == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "AssetConditionData");
                }
                PartyTable dep = new PartyTable();
                if (string.IsNullOrEmpty(saveData.AssetCondition.AssetConditionCode))
                {
                    throw new ValidationException(Language.GetString("AssetConditionCodeRequired"), null, "AssetConditionCode");
                }
                if (string.IsNullOrEmpty(saveData.AssetCondition.AssetConditionName))
                {
                    throw new ValidationException(Language.GetString("AssetConditionRequired"), null, "AssetCondition");
                }

                var depart = (from b in _db.AssetConditionTable where b.AssetConditionCode == saveData.AssetCondition.AssetConditionCode select b).FirstOrDefault();
                if (depart != null)
                {
                    depart.LastModifiedBy = 1;
                    depart.LastModifiedDateTime = System.DateTime.Now;
                    depart.StatusID = (int)StatusValue.Deleted;
                    _db.SaveChanges();
                }
                var message = "DataDeletedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.AssetCondition.AssetConditionCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpGet]
        public ActionResult GetAllCategoryDetails(string deviceSerialNo)
        {
            try
            {
                base.TraceLog("MobileAPI", $"{deviceSerialNo}");
                GetAllCategoriesResponse result = new GetAllCategoriesResponse();
                //load the lines
                result.Categories = (from b in CategoryTable.GetAllItems(_db)
                                          orderby b.CategoryName
                                          select new CategoryData
                                          {
                                              CategoryID = b.CategoryID,
                                              CategoryCode = b.CategoryCode,
                                              CategoryName = b.CategoryName,
                                              parentCategoryID=b.ParentCategoryID
                                          }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }
        [Authorize()]
        [HttpGet]
        public ActionResult Save_CategoryDetails(CategoryDataProcessData saveData)
        {
            try
            {
                if (saveData.Category == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "CategoryData");
                }
                CategoryTable dep = new CategoryTable();
                if (string.IsNullOrEmpty(saveData.Category.CategoryCode))
                {
                    throw new ValidationException(Language.GetString("CategoryCodeRequired"), null, "CategoryCode");
                }
                if (string.IsNullOrEmpty(saveData.Category.CategoryName))
                {
                    throw new ValidationException(Language.GetString("CategoryNameRequired"), null, "CategoryName");
                }

                dep.CategoryCode = saveData.Category.CategoryCode;
                dep.CategoryName = saveData.Category.CategoryName;
                dep.ParentCategoryID = saveData.Category.parentCategoryID;
                dep.StatusID = (int)StatusValue.Active;
                dep.CreatedBy = 1;
                dep.CreatedDateTime = System.DateTime.Now;

                _db.Add(dep);
                _db.SaveChanges();
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.Category.CategoryCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpPost]
        public ActionResult Update_CategoryDetails(CategoryDataProcessData saveData)
        {
            try
            {
                if (saveData.Category == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "CategoryData");
                }
                CategoryTable dep = new CategoryTable();
                if (string.IsNullOrEmpty(saveData.Category.CategoryCode))
                {
                    throw new ValidationException(Language.GetString("CategoryCodeRequired"), null, "CategoryCode");
                }
                if (string.IsNullOrEmpty(saveData.Category.CategoryName))
                {
                    throw new ValidationException(Language.GetString("CategoryNameRequired"), null, "CategoryName");
                }

                var depart = (from b in _db.CategoryTable where b.CategoryCode == saveData.Category.CategoryCode select b).FirstOrDefault();
                if (depart != null)
                {
                    depart.LastModifiedBy = 1;
                    depart.LastModifiedDateTime = System.DateTime.Now;
                    depart.CategoryName = saveData.Category.CategoryName;
                    _db.SaveChanges();
                }
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.Category.CategoryCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpPost]
        public ActionResult Delete_CategoryDetails(CategoryDataProcessData saveData)
        {
            try
            {
                if (saveData.Category == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "CategoryData");
                }
                CategoryTable dep = new CategoryTable();
                if (string.IsNullOrEmpty(saveData.Category.CategoryCode))
                {
                    throw new ValidationException(Language.GetString("CategoryCodeRequired"), null, "CategoryCode");
                }
                if (string.IsNullOrEmpty(saveData.Category.CategoryName))
                {
                    throw new ValidationException(Language.GetString("CategoryNameRequired"), null, "CategoryName");
                }

                var depart = (from b in _db.CategoryTable where b.CategoryCode == saveData.Category.CategoryCode select b).FirstOrDefault();
                if (depart != null)
                {
                    depart.LastModifiedBy = 1;
                    depart.LastModifiedDateTime = System.DateTime.Now;
                    depart.StatusID = (int)StatusValue.Deleted;
                    _db.SaveChanges();
                }
                var message = "DataDeletedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.Category.CategoryCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpGet]
        public ActionResult GetAllLocationDetails(string deviceSerialNo)
        {
            try
            {
                base.TraceLog("MobileAPI", $"{deviceSerialNo}");

                GetAllLocationsResponse result = new GetAllLocationsResponse();

                //load the lines
                result.Locations = (from b in LocationTable.GetAllItems(_db)
                                     orderby b.LocationName
                                     select new LocationData
                                     {
                                         LocationID = b.LocationID,
                                         LocationCode = b.LocationCode,
                                         LocationName = b.LocationName,
                                         parentLocationID = b.ParentLocationID

                                     }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpGet]
        public ActionResult Save_LocationDetails(LocationDataProcessData saveData)
        {
            try
            {
                if (saveData.Location == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "LocationData");
                }
                LocationTable dep = new LocationTable();
                if (string.IsNullOrEmpty(saveData.Location.LocationCode))
                {
                    throw new ValidationException(Language.GetString("LocationCodeRequired"), null, "LocationCode");
                }
                if (string.IsNullOrEmpty(saveData.Location.LocationName))
                {
                    throw new ValidationException(Language.GetString("LocationNameRequired"), null, "LocationName");
                }

                dep.LocationCode = saveData.Location.LocationCode;
                dep.LocationName = saveData.Location.LocationName;
                dep.ParentLocationID = saveData.Location.parentLocationID;
                dep.StatusID = (int)StatusValue.Active;
                dep.CreatedBy = 1;
                dep.CreatedDateTime = System.DateTime.Now;

                _db.Add(dep);
                _db.SaveChanges();
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.Location.LocationCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }


        [Authorize()]
        [HttpPost]
        public ActionResult Update_LocationDetails(LocationDataProcessData saveData)
        {
            try
            {
                if (saveData.Location == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "LocationData");
                }
                LocationTable dep = new LocationTable();
                if (string.IsNullOrEmpty(saveData.Location.LocationCode))
                {
                    throw new ValidationException(Language.GetString("LocationCodeRequired"), null, "LocationCode");
                }
                if (string.IsNullOrEmpty(saveData.Location.LocationName))
                {
                    throw new ValidationException(Language.GetString("LocationNameRequired"), null, "LocationName");
                }

                var depart = (from b in _db.LocationTable where b.LocationCode == saveData.Location.LocationCode select b).FirstOrDefault();
                if (depart != null)
                {
                    depart.LastModifiedBy = 1;
                    depart.LastModifiedDateTime = System.DateTime.Now;
                    depart.LocationName = saveData.Location.LocationName;
                    _db.SaveChanges();
                }
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.Location.LocationCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpPost]
        public ActionResult Delete_LocationDetails(LocationDataProcessData saveData)
        {
            try
            {
                if (saveData.Location == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "LocationData");
                }
                LocationTable dep = new LocationTable();
                if (string.IsNullOrEmpty(saveData.Location.LocationCode))
                {
                    throw new ValidationException(Language.GetString("LocationCodeRequired"), null, "LocationCode");
                }
                if (string.IsNullOrEmpty(saveData.Location.LocationName))
                {
                    throw new ValidationException(Language.GetString("LocationNameRequired"), null, "LocationName");
                }

                var depart = (from b in _db.LocationTable where b.LocationCode == saveData.Location.LocationCode select b).FirstOrDefault();
                if (depart != null)
                {
                    depart.LastModifiedBy = 1;
                    depart.LastModifiedDateTime = System.DateTime.Now;
                    depart.StatusID = (int)StatusValue.Deleted;
                    _db.SaveChanges();
                }
                var message = "DataDeletedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.Location.LocationCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpGet]
        public ActionResult GetAllManufacturerDetails(string deviceSerialNo)
        {
            try
            {
                base.TraceLog("MobileAPI", $"{deviceSerialNo}");

                GetAllManufacturerResponse result = new GetAllManufacturerResponse();

                //load the lines
                result.Manufacturers = (from b in ManufacturerTable.GetAllItems(_db)
                                    orderby b.ManufacturerName
                                    select new ManufacturerData
                                    {
                                        ManufacturerID = b.ManufacturerID,
                                        ManufacturerCode = b.ManufacturerCode,
                                        ManufacturerName = b.ManufacturerName,
                                       

                                    }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpGet]
        public ActionResult Save_ManufacturerDetails(ManufacturerDataProcessData saveData)
        {
            try
            {
                if (saveData.manufacturer == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "manufacturerData");
                }
                ManufacturerTable dep = new ManufacturerTable();
                if (string.IsNullOrEmpty(saveData.manufacturer.ManufacturerCode))
                {
                    throw new ValidationException(Language.GetString("ManufacturerCodeRequired"), null, "ManufacturerCode");
                }
                if (string.IsNullOrEmpty(saveData.manufacturer.ManufacturerName))
                {
                    throw new ValidationException(Language.GetString("ManufacturerNameRequired"), null, "ManufacturerName");
                }

                dep.ManufacturerCode = saveData.manufacturer.ManufacturerCode;
                dep.ManufacturerName = saveData.manufacturer.ManufacturerName;
                dep.StatusID = (int)StatusValue.Active;
                dep.CreatedBy = 1;
                dep.CreatedDateTime = System.DateTime.Now;

                _db.Add(dep);
                _db.SaveChanges();
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.manufacturer.ManufacturerCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpPost]
        public ActionResult Update_ManufacturerDetails(ManufacturerDataProcessData saveData)
        {
            try
            {
                if (saveData.manufacturer == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "manufacturerData");
                }
                ManufacturerTable dep = new ManufacturerTable();
                if (string.IsNullOrEmpty(saveData.manufacturer.ManufacturerCode))
                {
                    throw new ValidationException(Language.GetString("ManufacturerCodeRequired"), null, "ManufacturerCode");
                }
                if (string.IsNullOrEmpty(saveData.manufacturer.ManufacturerName))
                {
                    throw new ValidationException(Language.GetString("ManufacturerNameRequired"), null, "ManufacturerName");
                }

                var depart = (from b in _db.ManufacturerTable where b.ManufacturerCode == saveData.manufacturer.ManufacturerCode select b).FirstOrDefault();
                if (depart != null)
                {
                    depart.LastModifiedBy = 1;
                    depart.LastModifiedDateTime = System.DateTime.Now;
                    depart.ManufacturerName = saveData.manufacturer.ManufacturerName;
                    _db.SaveChanges();
                }
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.manufacturer.ManufacturerCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpPost]
        public ActionResult Delete_ManufacturerDetails(ManufacturerDataProcessData saveData)
        {
            try
            {
                if (saveData.manufacturer == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "manufacturerData");
                }
                ManufacturerTable dep = new ManufacturerTable();
                if (string.IsNullOrEmpty(saveData.manufacturer.ManufacturerCode))
                {
                    throw new ValidationException(Language.GetString("ManufacturerCodeRequired"), null, "ManufacturerCode");
                }
                if (string.IsNullOrEmpty(saveData.manufacturer.ManufacturerName))
                {
                    throw new ValidationException(Language.GetString("ManufacturerNameRequired"), null, "ManufacturerName");
                }

                var depart = (from b in _db.ManufacturerTable where b.ManufacturerCode == saveData.manufacturer.ManufacturerCode select b).FirstOrDefault();
                if (depart != null)
                {
                    depart.LastModifiedBy = 1;
                    depart.LastModifiedDateTime = System.DateTime.Now;
                    depart.StatusID = (int)StatusValue.Deleted;
                    _db.SaveChanges();
                }
                var message = "DataDeletedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.manufacturer.ManufacturerCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpGet]
        public ActionResult GetAllModelDetails(string deviceSerialNo)
        {
            try
            {
                base.TraceLog("MobileAPI", $"{deviceSerialNo}");

                GetAllModelsResponse result = new GetAllModelsResponse();

                //load the lines
                result.Models = (from b in ModelTable.GetAllItems(_db)
                                        orderby b.ModelName
                                        select new ModelData
                                        {
                                            ManufacturerID = b.ManufacturerID,
                                            ModelID = b.ModelID,
                                            ModelCode = b.ModelCode,
                                            ModelName=b.ModelName


                                        }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpGet]
        public ActionResult Save_ModelDetails(ModelDataProcessData saveData)
        {
            try
            {
                if (saveData.model == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "ModelData");
                }
                ModelTable dep = new ModelTable();

                if (string.IsNullOrEmpty(saveData.model.ModelCode))
                {
                    throw new ValidationException(Language.GetString("ModelCodeRequired"), null, "ModelCode");
                }
                if (string.IsNullOrEmpty(saveData.model.ModelName))
                {
                    throw new ValidationException(Language.GetString("ModelNameRequired"), null, "ModelName");
                }

                dep.ModelCode = saveData.model.ModelCode;
                dep.ModelName = saveData.model.ModelName;
                dep.ManufacturerID = saveData.model.ManufacturerID;
                dep.StatusID = (int)StatusValue.Active;
                dep.CreatedBy = 1;
                dep.CreatedDateTime = System.DateTime.Now;

                _db.Add(dep);
                _db.SaveChanges();
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.model.ModelCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }
        [Authorize()]
        [HttpPost]
        public ActionResult Update_ModelDetails(ModelDataProcessData saveData)
        {
            try
            {
                if (saveData.model == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "modelData");
                }
                ModelTable dep = new ModelTable();
                if (string.IsNullOrEmpty(saveData.model.ModelCode))
                {
                    throw new ValidationException(Language.GetString("ModelCodeRequired"), null, "ModelCode");
                }
                if (string.IsNullOrEmpty(saveData.model.ModelName))
                {
                    throw new ValidationException(Language.GetString("ModelNameRequired"), null, "ModelName");
                }

                var depart = (from b in _db.ModelTable where b.ModelCode == saveData.model.ModelCode select b).FirstOrDefault();
                if (depart != null)
                {
                    depart.LastModifiedBy = 1;
                    depart.LastModifiedDateTime = System.DateTime.Now;
                    depart.ModelName = saveData.model.ModelName;
                    _db.SaveChanges();
                }
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.model.ModelCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpPost]
        public ActionResult Delete_ModelDetails(ModelDataProcessData saveData)
        {
            try
            {
                if (saveData.model == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "modelData");
                }
                ModelTable dep = new ModelTable();
                if (string.IsNullOrEmpty(saveData.model.ModelCode))
                {
                    throw new ValidationException(Language.GetString("ModelCodeRequired"), null, "ModelCode");
                }
                if (string.IsNullOrEmpty(saveData.model.ModelName))
                {
                    throw new ValidationException(Language.GetString("ModelNameRequired"), null, "ModelName");
                }

                var depart = (from b in _db.ModelTable where b.ModelCode == saveData.model.ModelCode select b).FirstOrDefault();
                if (depart != null)
                {
                    depart.LastModifiedBy = 1;
                    depart.LastModifiedDateTime = System.DateTime.Now;
                    depart.StatusID = (int)StatusValue.Deleted;
                    _db.SaveChanges();
                }
                var message = "DataDeletedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.model.ModelCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpGet]
        public ActionResult GetAllProductDetails(string deviceSerialNo)
        {
            try
            {
                base.TraceLog("MobileAPI", $"{deviceSerialNo}");

                GetAllProductsResponse result = new GetAllProductsResponse();

                //load the lines
                result.Products = (from b in ProductTable.GetAllItems(_db)
                                 orderby b.ProductName
                                 select new ProductData
                                 {
                                     ProductID = b.ProductID,
                                     CategoryID = b.CategoryID,
                                     ProductCode = b.ProductCode,
                                     ProductName = b.ProductName


                                 }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }
        [Authorize()]
        [HttpGet]
        public ActionResult Save_ProductDetails(ProductDataProcessData saveData)
        {
            try
            {
                if (saveData.Product == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "ProductData");
                }
                ProductTable dep = new ProductTable();

                if (string.IsNullOrEmpty(saveData.Product.ProductCode))
                {
                    throw new ValidationException(Language.GetString("ProductCodeRequired"), null, "ProductCode");
                }
                if (string.IsNullOrEmpty(saveData.Product.ProductName))
                {
                    throw new ValidationException(Language.GetString("ProductNameRequired"), null, "ProductName");
                }

                dep.ProductCode = saveData.Product.ProductCode;
                dep.ProductName = saveData.Product.ProductName;
                dep.CategoryID = saveData.Product.CategoryID;
                dep.StatusID = (int)StatusValue.Active;
                dep.CreatedBy = 1;
                dep.CreatedDateTime = System.DateTime.Now;

                _db.Add(dep);
                _db.SaveChanges();
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.Product.ProductCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }
        [Authorize()]
        [HttpPost]
        public ActionResult Update_ProductDetails(ProductDataProcessData saveData)
        {
            try
            {
                if (saveData.Product == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "ProductData");
                }
                ProductTable dep = new ProductTable();
                if (string.IsNullOrEmpty(saveData.Product.ProductCode))
                {
                    throw new ValidationException(Language.GetString("ProductCodeRequired"), null, "ProductCode");
                }
                if (string.IsNullOrEmpty(saveData.Product.ProductName))
                {
                    throw new ValidationException(Language.GetString("ProductNameRequired"), null, "ProductName");
                }

                var depart = (from b in _db.ProductTable where b.ProductCode == saveData.Product.ProductCode select b).FirstOrDefault();
                if (depart != null)
                {
                    depart.LastModifiedBy = 1;
                    depart.LastModifiedDateTime = System.DateTime.Now;
                    depart.ProductName = saveData.Product.ProductName;
                    _db.SaveChanges();
                }
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.Product.ProductCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpPost]
        public ActionResult Delete_ProductDetails(ProductDataProcessData saveData)
        {
            try
            {
                if (saveData.Product == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "ProductData");
                }
                ProductTable dep = new ProductTable();
                if (string.IsNullOrEmpty(saveData.Product.ProductCode))
                {
                    throw new ValidationException(Language.GetString("ProductCodeRequired"), null, "ProductCode");
                }
                if (string.IsNullOrEmpty(saveData.Product.ProductName))
                {
                    throw new ValidationException(Language.GetString("ProductNameRequired"), null, "ProductName");
                }

                var depart = (from b in _db.ProductTable where b.ProductCode == saveData.Product.ProductCode select b).FirstOrDefault();
                if (depart != null)
                {
                    depart.LastModifiedBy = 1;
                    depart.LastModifiedDateTime = System.DateTime.Now;
                    depart.StatusID = (int)StatusValue.Deleted;
                    _db.SaveChanges();
                }
                var message = "DataDeletedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.Product.ProductCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpGet]
        public ActionResult GetAllCustodianDetails(string deviceSerialNo)
        {
            try
            {
                base.TraceLog("MobileAPI", $"{deviceSerialNo}");

                GetAllCustodiansResponse result = new GetAllCustodiansResponse();

                //load the lines
                result.Custodians = (from b in PersonTable.GetAllItems(_db)
                                   orderby b.PersonFirstName
                                   select new CustodianData
                                   {
                                       CustodianID = b.PersonID,
                                       CustodianCode = b.PersonCode,
                                       CustodianName = b.PersonFirstName+'-'+b.PersonLastName,
                                       Email = b.EMailID,
                                       DOJ=b.DOJ,
                                       DepartmentID=b.DepartmentID,
                                       MobileNo=b.MobileNo,
                                       Gender=b.Gender


                                   }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpGet]
        public ActionResult Save_CustodianDetails(CustodianDataProcessData saveData)
        {
            try
            {
                if (saveData.Custodian == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "CustodianData");
                }
                PersonTable dep = new PersonTable();

                if (string.IsNullOrEmpty(saveData.Custodian.CustodianCode))
                {
                    throw new ValidationException(Language.GetString("CustodianCodeRequired"), null, "CustodianCode");
                }
                if (string.IsNullOrEmpty(saveData.Custodian.CustodianName))
                {
                    throw new ValidationException(Language.GetString("CustodianNameRequired"), null, "CustodianName");
                }

                dep.PersonCode = saveData.Custodian.CustodianCode;
                dep.PersonFirstName = saveData.Custodian.CustodianName;
                dep.PersonLastName = saveData.Custodian.CustodianName;
                dep.DOJ = saveData.Custodian.DOJ;

                dep.MobileNo = saveData.Custodian.MobileNo;
                dep.Gender = saveData.Custodian.Gender;
                dep.EMailID = saveData.Custodian.Email;
                dep.DepartmentID = saveData.Custodian.DepartmentID;
                dep.StatusID = (int)StatusValue.Active;
                dep.CreatedBy = 1;
                dep.CreatedDateTime = System.DateTime.Now;

                _db.Add(dep);
                _db.SaveChanges();
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.Custodian.CustodianCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpPost]
        public ActionResult Update_CustodianDetails(CustodianDataProcessData saveData)
        {
            try
            {
                if (saveData.Custodian == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "CustodianData");
                }
                PersonTable dep = new PersonTable();
                if (string.IsNullOrEmpty(saveData.Custodian.CustodianCode))
                {
                    throw new ValidationException(Language.GetString("CustodianCodeRequired"), null, "CustodianCode");
                }
                if (string.IsNullOrEmpty(saveData.Custodian.CustodianName))
                {
                    throw new ValidationException(Language.GetString("CustodianNameRequired"), null, "CustodianName");
                }

                var depart = (from b in _db.PersonTable where b.PersonCode == saveData.Custodian.CustodianCode select b).FirstOrDefault();
                if (depart != null)
                {
                    depart.LastModifiedBy = 1;
                    depart.LastModifiedDateTime = System.DateTime.Now;
                    depart.PersonFirstName = saveData.Custodian.CustodianName;
                    depart.PersonLastName = saveData.Custodian.CustodianName;
                    depart.MobileNo = saveData.Custodian.MobileNo;
                    depart.EMailID = saveData.Custodian.Email;
                    depart.Gender = saveData.Custodian.Gender;
                    depart.DepartmentID = saveData.Custodian.DepartmentID;
                    _db.SaveChanges();
                }
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.Custodian.CustodianCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }


        [Authorize()]
        [HttpGet]
        public ActionResult GetAllApprovalRoleDetails(string deviceSerialNo)
        {
            try
            {
                base.TraceLog("MobileAPI", $"{deviceSerialNo}");

                GetAllApprovalRoleResponse result = new GetAllApprovalRoleResponse();

                //load the lines
                result.ApprovalRole = (from b in ApprovalRoleTable.GetAllItems(_db)
                                      orderby b.ApprovalRoleName
                                      select new ApprovalRoleData
                                      {
                                          ApprovalRoleID = b.ApprovalRoleID,
                                          ApprovalRoleCode = b.ApprovalRoleCode,
                                          ApprovalRoleName = b.ApprovalRoleName
                                      }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }
        [Authorize()]
        [HttpPost]
        public ActionResult Save_AprpvoalRoleDetails(ApprovalRoleDataProcessData saveData)
        {
            try
            {
                if (saveData.ApprovalRole == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "ApprovalRoleData");
                }
                ApprovalRoleTable dep = new ApprovalRoleTable();
                if (string.IsNullOrEmpty(saveData.ApprovalRole.ApprovalRoleCode))
                {
                    throw new ValidationException(Language.GetString("ApprovalRoleCodeRequired"), null, "ApprovalRoleCode");
                }
                if (string.IsNullOrEmpty(saveData.ApprovalRole.ApprovalRoleName))
                {
                    throw new ValidationException(Language.GetString("ApprovalRoleNameRequired"), null, "ApprovalRoleName");
                }

                dep.ApprovalRoleCode = saveData.ApprovalRole.ApprovalRoleCode;
                dep.ApprovalRoleName = saveData.ApprovalRole.ApprovalRoleName;
                dep.StatusID = (int)StatusValue.Active;
                dep.CreatedBy = 1;
                dep.CreatedDateTime = System.DateTime.Now;

                _db.Add(dep);
                _db.SaveChanges();
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.ApprovalRole.ApprovalRoleCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpPost]
        public ActionResult Update_ApprovalRoleDetails(ApprovalRoleDataProcessData saveData)
        {
            try
            {
                if (saveData.ApprovalRole == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "ApprovalRoleData");
                }
                ApprovalRoleTable dep = new ApprovalRoleTable();
                if (string.IsNullOrEmpty(saveData.ApprovalRole.ApprovalRoleCode))
                {
                    throw new ValidationException(Language.GetString("ApprovalRoleCodeRequired"), null, "ApprovalRoleCode");
                }
                if (string.IsNullOrEmpty(saveData.ApprovalRole.ApprovalRoleName))
                {
                    throw new ValidationException(Language.GetString("ApprovalRoleNameRequired"), null, "ApprovalRoleName");
                }

                var depart = (from b in _db.ApprovalRoleTable where b.ApprovalRoleCode == saveData.ApprovalRole.ApprovalRoleCode select b).FirstOrDefault();
                if (depart != null)
                {
                    depart.LastModifiedBy = 1;
                    depart.LastModifiedDateTime = System.DateTime.Now;
                    depart.ApprovalRoleName = saveData.ApprovalRole.ApprovalRoleName;

                    _db.SaveChanges();
                }
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.ApprovalRole.ApprovalRoleCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }

        [Authorize()]
        [HttpPost]
        public ActionResult Delete_ApprovalRoleDetails(ApprovalRoleDataProcessData saveData)
        {
            try
            {
                if (saveData.ApprovalRole == null)
                {

                    throw new ValidationException(Language.GetString("DataRequired"), null, "ApprovalRoleData");
                }
                ApprovalRoleTable dep = new ApprovalRoleTable();
                if (string.IsNullOrEmpty(saveData.ApprovalRole.ApprovalRoleCode))
                {
                    throw new ValidationException(Language.GetString("ApprovalRoleCodeRequired"), null, "ApprovalRoleCode");
                }
                if (string.IsNullOrEmpty(saveData.ApprovalRole.ApprovalRoleName))
                {
                    throw new ValidationException(Language.GetString("ApprovalRoleNameRequired"), null, "ApprovalRoleName");
                }

                var depart = (from b in _db.ApprovalRoleTable where b.ApprovalRoleCode == saveData.ApprovalRole.ApprovalRoleCode select b).FirstOrDefault();
                if (depart != null)
                {
                    depart.LastModifiedBy = 1;
                    depart.LastModifiedDateTime = System.DateTime.Now;
                    depart.StatusID = (int)StatusValue.Deleted;

                    _db.SaveChanges();
                }
                var message = "DataDeletedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.ApprovalRole.ApprovalRoleCode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }
        [Authorize()]
        [HttpGet]
        public ActionResult GetAllAssetDetails(string deviceSerialNo)
        {
            try
            {
                base.TraceLog("MobileAPI", $"{deviceSerialNo}");

                GetAllAssetResponse result = new GetAllAssetResponse();

                //load the lines
                result.Asset = (from b in AssetTable.GetAllItems(_db)
                                       orderby b.Barcode
                                       select new AssetData
                                       {
                                           AssetID = b.AssetID,
                                           Barcode = b.Barcode.Trim(),
                                           AssetCode = b.AssetCode.Trim(),
                                           CategoryID = b.CategoryID,
                                           LocationID = b.LocationID,
                                           DepartmentID = b.DepartmentID,
                                           CustodianID = b.CustodianID,
                                           SectionID = b.SectionID,
                                           ModelID = b.ModelID,
                                           ManufacturerID = b.ManufacturerID,
                                           AssetConditionID = b.AssetConditionID,
                                           SupplierID = b.SupplierID,
                                           ProductID = b.ProductID,
                                           ReceiptNumber = b.ReceiptNumber,
                                           SerialNo = b.SerialNo,
                                           ReferenceCode = b.ReferenceCode,
                                           StatusID = b.StatusID,
                                           PONumber = b.PONumber,
                                           PurchaseDate = b.PurchaseDate,
                                           ComissionDate = b.ComissionDate,
                                           WarrantyExpiryDate = b.WarrantyExpiryDate,
                                           AssetDescription = b.AssetDescription,
                                           CostOfRemoval = b.CostOfRemoval,
                                           SoldTo = b.SoldTo,
                                           partialDisposalTotalValue = b.partialDisposalTotalValue,
                                           AssetRemarks = b.AssetRemarks,
                                           Capacity = b.Capacity,
                                           Make = b.Make,
                                           CompanyID = b.CompanyID,
                                           Attribute1 = b.Attribute1,
                                           Attribute2 = b.Attribute2,
                                           Attribute3 = b.Attribute3,
                                           Attribute4 = b.Attribute4,
                                           Attribute5 = b.Attribute5,
                                           Attribute6 = b.Attribute6,
                                           Attribute7 = b.Attribute7,
                                           Attribute8 = b.Attribute8,
                                           Attribute9 = b.Attribute9,
                                           Attribute10 = b.Attribute10,
                                           Attribute11 = b.Attribute11,
                                           Attribute12 = b.Attribute12,
                                           Attribute13 = b.Attribute13,
                                           Attribute14 = b.Attribute14,
                                           Attribute15 = b.Attribute15,
                                           Attribute16 = b.Attribute16,
                                           DeliveryNote = b.DeliveryNote,
                                           CurrentCost = b.CurrentCost,
                                           DisposalReferenceNo = b.DisposalReferenceNo,
                                           DisposalTypeID = b.DisposalTypeID,
                                           DisposalValue = b.DisposalValue,
                                           DisposedRemarks = b.DisposedRemarks,
                                           DisposedDateTime = b.DisposedDateTime,
                                           InvoiceDate = b.InvoiceDate,
                                           InvoiceNo = b.InvoiceNo,
                                           NetworkID = b.NetworkID,
                                           Latitude = b.Latitude,
                                           Longitude = b.Longitude,
                                           RFIDTagCode = b.RFIDTagCode,
                                           PurchasePrice = b.PurchasePrice,
                                           VoucherNo = b.VoucherNo
                                       }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }
        [Authorize()]
        [HttpPost]
        public ActionResult Save_AssetDetails(AssetDataProcessData saveData)
        {
            try
            {

                if (saveData.AssetData == null)
                {
                    throw new ValidationException(Language.GetString("DataRequired"), null, "AssetData");
                }
                AssetTable asset = new AssetTable();
                if (string.IsNullOrEmpty(saveData.AssetData.Barcode))
                {
                    throw new ValidationException(Language.GetString("BarcodeRequired"), null, "Barcode");
                }
                if (string.IsNullOrEmpty(saveData.AssetData.AssetCode))
                {
                    throw new ValidationException(Language.GetString("AssetCodeRequired"), null, "AssetCode");
                }
                if (saveData.AssetData.CategoryID == null)
                {
                    throw new ValidationException(Language.GetString("CatgoryRequired"), null, "CategoryID");
                }
                if (string.IsNullOrEmpty(saveData.AssetData.AssetDescription))
                {
                    throw new ValidationException(Language.GetString("AssetDescriptionRequired"), null, "AssetDescription");
                }
                if (saveData.AssetData.StatusID == null)
                {
                    throw new ValidationException(Language.GetString("StatusRequired"), null, "StatusID");
                }
                var standtandValidate = TransactionTable.AssetValidationResult(_db, SessionUser.Current.UserID, "MobileAPIAssetCreation", saveData.AssetData.CategoryID,
                                          null, null, saveData.AssetData.LocationID, saveData.AssetData.DepartmentID, null, saveData.AssetData.SerialNo, null, saveData.AssetData.ManufacturerID);
                if (!string.IsNullOrEmpty(standtandValidate.Result))
                {
                    return base.ErrorActionResult(standtandValidate.Result);
                }

                if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.AssetApprovalBasedOnWorkFlow))
                {
                    var validationError = TransactionTable.GetAssetResult(AMSContext.CreateNewContext(), (int)saveData.AssetData.LocationID, saveData.AssetData.CategoryID);
                    if (!string.IsNullOrEmpty(validationError.Result))
                    {
                        return base.ErrorActionResult(validationError.Result);
                    }
                }
               
                if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.AssetApprovalBasedOnWorkFlow))
                {
                    asset.StatusID = (int)StatusValue.WaitingForApproval;
                    asset.AssetApproval = 0;
                }
                else
                {
                    asset.AssetApproval = 1;
                    asset.StatusID = (int)StatusValue.Active;
                }
                asset.Barcode = saveData.AssetData.Barcode;
                asset.AssetCode = saveData.AssetData.AssetCode;
                asset.CategoryID = saveData.AssetData.CategoryID;
                asset.LocationID = saveData.AssetData.LocationID;
                asset.DepartmentID = saveData.AssetData.DepartmentID;
                asset.CustodianID = saveData.AssetData.CustodianID;
                asset.SectionID = saveData.AssetData.SectionID;
                asset.ModelID = saveData.AssetData.ModelID;
                asset.ManufacturerID = saveData.AssetData.ManufacturerID;
                asset.AssetConditionID = saveData.AssetData.AssetConditionID;
                asset.SupplierID = saveData.AssetData.SupplierID;
                asset.ProductID = saveData.AssetData.ProductID;
                asset.ReceiptNumber = saveData.AssetData.ReceiptNumber;
                asset.SerialNo = saveData.AssetData.SerialNo;
                asset.ReferenceCode = saveData.AssetData.ReferenceCode;
                asset.PONumber = saveData.AssetData.PONumber;
                asset.PurchaseDate = saveData.AssetData.PurchaseDate;
                asset.ComissionDate = saveData.AssetData.ComissionDate;
                asset.WarrantyExpiryDate = saveData.AssetData.WarrantyExpiryDate;
                asset.AssetDescription = saveData.AssetData.AssetDescription;
                asset.CostOfRemoval = saveData.AssetData.CostOfRemoval;
                asset.SoldTo = saveData.AssetData.SoldTo;
                asset.partialDisposalTotalValue = saveData.AssetData.partialDisposalTotalValue;
                asset.AssetRemarks = saveData.AssetData.AssetRemarks;
                asset.Capacity = saveData.AssetData.Capacity;
                asset.Make = saveData.AssetData.Make;
                asset.CompanyID = saveData.AssetData.CompanyID;
                asset.Attribute1 = saveData.AssetData.Attribute1;
                asset.Attribute2 = saveData.AssetData.Attribute2;
                asset.Attribute3 = saveData.AssetData.Attribute3;
                asset.Attribute4 = saveData.AssetData.Attribute4;
                asset.Attribute5 = saveData.AssetData.Attribute5;
                asset.Attribute6 = saveData.AssetData.Attribute6;
                asset.Attribute7 = saveData.AssetData.Attribute7;
                asset.Attribute8 = saveData.AssetData.Attribute8;
                asset.Attribute9 = saveData.AssetData.Attribute9;
                asset.Attribute10 = saveData.AssetData.Attribute10;
                asset.Attribute11 = saveData.AssetData.Attribute11;
                asset.Attribute12 = saveData.AssetData.Attribute12;
                asset.Attribute13 = saveData.AssetData.Attribute13;
                asset.Attribute14 = saveData.AssetData.Attribute14;
                asset.Attribute15 = saveData.AssetData.Attribute15;
                asset.Attribute16 = saveData.AssetData.Attribute16;
                asset.DeliveryNote = saveData.AssetData.DeliveryNote;
                asset.CurrentCost = saveData.AssetData.CurrentCost;
                asset.DisposalReferenceNo = saveData.AssetData.DisposalReferenceNo;
                asset.DisposalTypeID = saveData.AssetData.DisposalTypeID;
                asset.DisposalValue = saveData.AssetData.DisposalValue;
                asset.DisposedRemarks = saveData.AssetData.DisposedRemarks;
                asset.DisposedDateTime = saveData.AssetData.DisposedDateTime;
                asset.InvoiceDate = saveData.AssetData.InvoiceDate;
                asset.InvoiceNo = saveData.AssetData.InvoiceNo;
                asset.NetworkID = saveData.AssetData.NetworkID;
                asset.Latitude = saveData.AssetData.Latitude;
                asset.Longitude = saveData.AssetData.Longitude;
                asset.RFIDTagCode = saveData.AssetData.RFIDTagCode;
                asset.PurchasePrice = saveData.AssetData.PurchasePrice;
                asset.VoucherNo = saveData.AssetData.VoucherNo;
                asset.CreateFromHHT = false;
                asset.DepreciationFlag = false;
                asset.IsScanned = false;
                asset.CreatedBy = 1;
                asset.CreatedDateTime = System.DateTime.Now;
                _db.Add(asset);
                _db.SaveChanges();
                if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.AssetApprovalBasedOnWorkFlow))
                {
                    _db.Entry(asset).Reload();

                    List<string> BarcodeList = new List<string>() { asset.Barcode };
                    TransactionTable.SaveTransactiondata(_db, BarcodeList, SessionUser.Current.UserID, (int)ApproveModuleValue.AssetAddition, CodeGenerationHelper.GetNextCode("AssetAddition"));
                }
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.AssetData.Barcode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }
        [Authorize()]
        [HttpPost]
        public ActionResult update_AssetDetails(AssetDataProcessData saveData)
        {
            try
            {

                if (saveData.AssetData == null)
                {
                    throw new ValidationException(Language.GetString("DataRequired"), null, "AssetData");
                }
                AssetTable asset = new AssetTable();
                if (string.IsNullOrEmpty(saveData.AssetData.Barcode))
                {
                    throw new ValidationException(Language.GetString("BarcodeRequired"), null, "Barcode");
                }
                if (string.IsNullOrEmpty(saveData.AssetData.AssetCode))
                {
                    throw new ValidationException(Language.GetString("AssetCodeRequired"), null, "AssetCode");
                }
                if (saveData.AssetData.CategoryID == null)
                {
                    throw new ValidationException(Language.GetString("CatgoryRequired"), null, "CategoryID");
                }
                if (string.IsNullOrEmpty(saveData.AssetData.AssetDescription))
                {
                    throw new ValidationException(Language.GetString("AssetDescriptionRequired"), null, "AssetDescription");
                }
                if (saveData.AssetData.StatusID == null)
                {
                    throw new ValidationException(Language.GetString("StatusRequired"), null, "StatusID");
                }
                if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.AssetApprovalBasedOnWorkFlow))
                {
                    var standtandValidate = TransactionTable.AssetValidationResult(_db, SessionUser.Current.UserID, "WebAssetCreate", asset.CategoryID,
                                        null, null, asset.LocationID, asset.DepartmentID, null, asset.SerialNo, null, asset.ManufacturerID);
                    if (!string.IsNullOrEmpty(standtandValidate.Result))
                    {
                        return base.ErrorActionResult(standtandValidate.Result);
                    }
                }
                //if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.AssetApproval)

                //           || AppConfigurationManager.GetValue<bool>(AppConfigurationManager.AssetApprovalBasedOnWorkFlow))
                //{
                //    asset.StatusID = (int)StatusValue.WaitingForApproval;
                //    asset.AssetApproval = 0;
                //}
                //else
                //{
                //    asset.AssetApproval = 1;
                //    asset.StatusID = (int)StatusValue.Active;
                //}
                var data = (from b in _db.AssetTable where b.Barcode == saveData.AssetData.Barcode select b).FirstOrDefault();
                asset.Barcode = data.Barcode;
                asset.AssetCode = data.AssetCode;
                asset.CategoryID = data.CategoryID;
                asset.LocationID = data.LocationID;
                asset.DepartmentID = data.DepartmentID;
                asset.CustodianID = data.CustodianID;
                asset.SectionID = data.SectionID;
                asset.ModelID = data.ModelID;
                asset.ManufacturerID = data.ManufacturerID;
                asset.AssetConditionID = data.AssetConditionID;
                asset.SupplierID = data.SupplierID;
                asset.ProductID = data.ProductID;
                asset.ReceiptNumber = data.ReceiptNumber;
                asset.SerialNo = data.SerialNo;
                asset.ReferenceCode = data.ReferenceCode;
                asset.PONumber = data.PONumber;
                asset.PurchaseDate = data.PurchaseDate;
                asset.ComissionDate = data.ComissionDate;
                asset.WarrantyExpiryDate = data.WarrantyExpiryDate;
                asset.AssetDescription = data.AssetDescription;
                asset.CostOfRemoval = data.CostOfRemoval;
                asset.SoldTo = data.SoldTo;
                asset.partialDisposalTotalValue = data.partialDisposalTotalValue;
                asset.AssetRemarks = data.AssetRemarks;
                asset.Capacity = data.Capacity;
                asset.Make = data.Make;
                asset.CompanyID = data.CompanyID;
                asset.Attribute1 = data.Attribute1;
                asset.Attribute2 = data.Attribute2;
                asset.Attribute3 = data.Attribute3;
                asset.Attribute4 = data.Attribute4;
                asset.Attribute5 = data.Attribute5;
                asset.Attribute6 = data.Attribute6;
                asset.Attribute7 = data.Attribute7;
                asset.Attribute8 = data.Attribute8;
                asset.Attribute9 = data.Attribute9;
                asset.Attribute10 = data.Attribute10;
                asset.Attribute11 = data.Attribute11;
                asset.Attribute12 = data.Attribute12;
                asset.Attribute13 = data.Attribute13;
                asset.Attribute14 = data.Attribute14;
                asset.Attribute15 = data.Attribute15;
                asset.Attribute16 = data.Attribute16;
                asset.DeliveryNote = data.DeliveryNote;
                asset.CurrentCost = data.CurrentCost;
                asset.DisposalReferenceNo = data.DisposalReferenceNo;
                asset.DisposalTypeID = data.DisposalTypeID;
                asset.DisposalValue = data.DisposalValue;
                asset.DisposedRemarks = data.DisposedRemarks;
                asset.DisposedDateTime = data.DisposedDateTime;
                asset.InvoiceDate = data.InvoiceDate;
                asset.InvoiceNo = data.InvoiceNo;
                asset.NetworkID = data.NetworkID;
                asset.Latitude = data.Latitude;
                asset.Longitude = data.Longitude;
                asset.RFIDTagCode = data.RFIDTagCode;
                asset.PurchasePrice = data.PurchasePrice;
                asset.VoucherNo = data.VoucherNo;
                asset.CreateFromHHT = false;
                asset.DepreciationFlag = false;
                asset.IsScanned = false;
                asset.LastModifiedBy = 1;
                asset.LastModifiedDateTime = System.DateTime.Now;

                _db.SaveChanges();

                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request '{saveData.AssetData.Barcode}' {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }
        }
        [Authorize()]
        [HttpPost]
        public ActionResult Disposal_AssetDetails(AssetDisposalDataProcessData saveData)
        {
            try
            {
                foreach(var DisposalData in saveData.DisposalData)
                {
                    if (DisposalData == null)
                    {
                        throw new ValidationException(Language.GetString("DataRequired"), null, "DisposalData");
                    }
                  //  AssetTable asset = new AssetTable();
                    if (string.IsNullOrEmpty(DisposalData.Barcode))
                    {
                        throw new ValidationException(Language.GetString("BarcodeRequired"), null, "Barcode");
                    }
                    //if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.BulkDisposeAssetApproval))
                    //{
                    var OldAsset = (from b in _db.AssetTable where b.Barcode == DisposalData.Barcode select b).FirstOrDefault();
                    var standtandValidate = TransactionTable.AssetRetirementValidationResult(_db, SessionUser.Current.UserID, (int)OldAsset.AssetID, "MobileAPIDisposal");
                        if (!string.IsNullOrEmpty(standtandValidate.Result))
                        {
                            return base.ErrorActionResult(standtandValidate.Result);
                        }
                    //    asset.StatusID = (int)StatusValue.WaitingForApproval;
                    //    asset.AssetApproval = 0;
                    //}
                  
                    OldAsset.DisposalReferenceNo = DisposalData.DisposalReferenceNo;
                    OldAsset.DisposalValue = DisposalData.DisposalValue;
                    OldAsset.DisposedDateTime = DisposalData.DisposedDateTime;
                    OldAsset.DisposedRemarks = DisposalData.DisposedRemarks;
                    OldAsset.DisposalTypeID = DisposalData.DisposalTypeID;
                    OldAsset.ProceedofSales = DisposalData.ProceedofSales;
                    OldAsset.SoldTo = DisposalData.SoldTo;
                    OldAsset.CurrentCost = DisposalData.CurrentCost;
                    OldAsset.CostOfRemoval = DisposalData.CostOfRemoval;
                }
                _db.SaveChanges();
                var message = "DataSavedSuccessfully";
                var result = new BaseResponse()
                {
                    Message = $"Request {message}",

                };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                return ErrorActionResult<GetAllCompaniesResponse>(ex);
            }

        }

            //[HttpPost]
            //public ActionResult Auth_ValidateUser([FromBody] ValidateUserRequest request)
            //{
            //    try
            //    {
            //        if (request is null)
            //        {
            //            throw new ValidationException("Invalid User request data");
            //        }

            //        if (string.IsNullOrEmpty(request.Username))
            //        {
            //            throw new ValidationException("Username required for validate user");
            //        }

            //        if (string.IsNullOrEmpty(request.Password))
            //        {
            //            throw new ValidationException("Password required for validate user");
            //        }

            //        string validate = User_LoginUserTable.ValidateUser(_db, request.Username, request.Password);
            //        if (string.Compare(validate, "Success") == 0)
            //        {
            //            var user = User_LoginUserTable.GetUser(_db, request.Username);

            //            var response = new ValidateUserResponse();
            //            response.UserID = user.UserID;

            //            var person = PersonTable.GetItem(_db, user.UserID);
            //            if (person.StatusID == (int)StatusValue.Deleted)
            //            {
            //                ErrorActionResult<ValidateUserResponse>($"Enter valid Username and Password");
            //            }

            //            if ((person.UserTypeID == (int)UserTypeValue.Mobile) || (person.UserTypeID == (int)UserTypeValue.WebMobile))
            //            {
            //                //validate locationID
            //                LinenRoomTable linenRoom = LinenRoomTable.GetItem(_db, request.WarehouseID);
            //                if ((linenRoom == null) || (linenRoom.StatusID > (int)StatusValue.Active))
            //                    return ErrorActionResult<ValidateUserResponse>("Selected linen room not available");

            //                var UserLinenRoomMapping = (from b in UserLinenRoomMappingTable.GetAllItems(_db)
            //                                            where b.StatusID == (int)StatusValue.Active
            //                                                    && b.UserID == user.UserID
            //                                                    && b.LinenRoomID == request.WarehouseID
            //                                            select b);
            //                if (UserLinenRoomMapping.Any() == false)
            //                    return ErrorActionResult<ValidateUserResponse>("Selected linen room not mapped with your account");

            //                response.Fullname = $"{person.PersonFirstName} {person.PersonLastName}";
            //                response.AllowedRights = new List<string>();
            //                response.LocationID = linenRoom.LinenRoomID;
            //                response.LocationName = linenRoom.LinenRoomName;

            //                ////TraceLog($"User validation Ok - {request.Username}", response.UserID, request.WarehouseID, request.DeviceSerialNo);
            //                //var authClaims = new List<Claim>
            //                //{
            //                //    new Claim(ClaimTypes.Name, user.UserName),
            //                //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //                //};

            //                ////add role as admin
            //                //authClaims.Add(new Claim(ClaimTypes.Role, "admin"));
            //                //var token = GetToken(authClaims, false);// person.Usertypeid == (int)UserTypeValue.KioskDeviceTestUser);
            //                //response.expiration = token.ValidTo;
            //                //response.token = new JwtSecurityTokenHandler().WriteToken(token);

            //                return Json(response);
            //            }
            //            else
            //            {
            //                return ErrorActionResult<ValidateUserResponse>("Mobile login restricted");
            //            }
            //        }
            //        else
            //        {
            //            TraceLog($"validation failed for user - {request.Username}", "");
            //            return ErrorActionResult<ValidateUserResponse>(validate);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        //TraceLog(ex.Message);
            //        return ErrorActionResult<ValidateUserResponse>(ex);
            //    }
            //}

            //[HttpPost]
            //public ActionResult Auth_ValidateVendorUser([FromBody] ValidateUserRequest request)
            //{
            //    try
            //    {
            //        if (request is null)
            //        {
            //            throw new ValidationException("Invalid User request data");
            //        }

            //        if (string.IsNullOrEmpty(request.Username))
            //        {
            //            throw new ValidationException("Username required for validate user");
            //        }

            //        if (string.IsNullOrEmpty(request.Password))
            //        {
            //            throw new ValidationException("Password required for validate user");
            //        }

            //        string validate = User_LoginUserTable.ValidateUser(_db, request.Username, request.Password);
            //        if (string.Compare(validate, "Success") == 0)
            //        {
            //            var user = User_LoginUserTable.GetUser(_db, request.Username);

            //            var response = new ValidateUserResponse();
            //            response.UserID = user.UserID;

            //            var person = PersonTable.GetItem(_db, user.UserID);
            //            if (person.StatusID == (int)StatusValue.Deleted)
            //            {
            //                ErrorActionResult<ValidateUserResponse>($"Enter valid Username and Password");
            //            }

            //            if (person.UserTypeID == (int)UserTypeValue.Vendor)
            //            {
            //                //validate locationID
            //                LinenRoomTable linenRoom = LinenRoomTable.GetItem(_db, request.WarehouseID);
            //                if ((linenRoom == null) || (linenRoom.StatusID > (int)StatusValue.Active))
            //                    return ErrorActionResult<ValidateUserResponse>("Selected linene room not available");

            //                response.Fullname = $"{person.PersonFirstName} {person.PersonLastName}";
            //                response.AllowedRights = new List<string>();
            //                response.LaundryVendorID = person.LaundryVendorID.Value;
            //                response.LocationID = linenRoom.LinenRoomID;
            //                response.LocationName = linenRoom.LinenRoomName;

            //                ////TraceLog($"User validation Ok - {request.Username}", response.UserID, request.WarehouseID, request.DeviceSerialNo);
            //                //var authClaims = new List<Claim>
            //                //{
            //                //    new Claim(ClaimTypes.Name, user.UserName),
            //                //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //                //};

            //                ////add role as admin
            //                //authClaims.Add(new Claim(ClaimTypes.Role, "admin"));
            //                //var token = GetToken(authClaims, false);// person.Usertypeid == (int)UserTypeValue.KioskDeviceTestUser);
            //                //response.expiration = token.ValidTo;
            //                //response.token = new JwtSecurityTokenHandler().WriteToken(token);

            //                return Json(response);
            //            }
            //            else
            //            {
            //                return ErrorActionResult<ValidateUserResponse>("Mobile login restricted");
            //            }
            //        }
            //        else
            //        {
            //            TraceLog($"validation failed for user - {request.Username}", "");
            //            return ErrorActionResult<ValidateUserResponse>(validate);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        //TraceLog(ex.Message);
            //        return ErrorActionResult<ValidateUserResponse>(ex);
            //    }
            //}

            //[HttpGet]
            //public ActionResult Master_GetAllLocations(string deviceSerialNo, int userID, int warehouseID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");

            //        GetAllLocationsResponse result = new GetAllLocationsResponse();

            //        //load the lines
            //        result.Locations = (from b in LinenRoomTable.GetAllItems(_db)
            //                            orderby b.LinenRoomName
            //                            select new LocationData
            //                            {
            //                                LocationID = b.LinenRoomID,
            //                                LocationCode = b.LinenRoomCode,
            //                                LocationName = b.LinenRoomName
            //                            }).ToList();

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllLocationsResponse>(ex);
            //    }
            //}

            //[HttpGet]
            //public ActionResult Master_GetAllUniformCategory(string deviceSerialNo, int userID, int warehouseID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");

            //        GetAllUniformCategoryResponse result = new GetAllUniformCategoryResponse();

            //        //load the lines
            //        result.UniformCategory = (from b in UniformCategoryTable.GetAllItems(_db)
            //                                  orderby b.UniformCategoryName
            //                                  select new UniformCategoryData
            //                                  {
            //                                      UniformCategoryID = b.UniformCategoryID,
            //                                      UniformCategoryCode = b.UniformCategoryCode,
            //                                      UniformCategoryName = b.UniformCategoryName,
            //                                      AllowedToWash = b.AllowedToWash,
            //                                      LifeCycle = b.LifeCycle
            //                                  }).ToList();

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllLocationsResponse>(ex);
            //    }
            //}

            //[HttpGet]
            //public ActionResult Master_GetAllWashTypes(string deviceSerialNo, int userID, int warehouseID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");

            //        GetAllWashTypesResponse result = new GetAllWashTypesResponse();

            //        //load the lines
            //        result.WashTypes = (from b in WashTypeTable.GetAllItems(_db)
            //                            orderby b.WashTypeName
            //                            select new WashTypeData
            //                            {
            //                                WashTypeID = b.WashTypeID,
            //                                WashTypeCode = b.WashTypeCode,
            //                                WashTypeName = b.WashTypeName
            //                            }).ToList();

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllDivisionResponse>(ex);
            //    }
            //}

            //[HttpGet]
            //public ActionResult Master_GetAllDivisions(string deviceSerialNo, int userID, int warehouseID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");

            //        GetAllDivisionResponse result = new GetAllDivisionResponse();

            //        //load the lines
            //        result.Divisions = (from b in DivisionTable.GetAllItems(_db)
            //                            orderby b.DivisionName
            //                            select new DivisionData
            //                            {
            //                                DivisionID = b.DivisionID,
            //                                DivisionCode = b.DivisionCode,
            //                                DivisionName = b.DivisionName
            //                            }).ToList();

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllDivisionResponse>(ex);
            //    }
            //}

            //[HttpGet]
            //public ActionResult Master_GetAllDepartments(string deviceSerialNo, int userID, int warehouseID, int divisionID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");

            //        var resultData = DepartmentTable.GetAllItemsByDivision(_db, divisionID);

            //        GetAllDepartmentResponse result = new GetAllDepartmentResponse();
            //        if (resultData != null)
            //        {
            //            //load the lines
            //            result.Departments = (from b in resultData
            //                                  orderby b.DepartmentName
            //                                  select new DepartmentData
            //                                  {
            //                                      DepartmentID = b.DepartmentID,
            //                                      DivisionCode = b.Division.DivisionCode,
            //                                      DivisionName = b.Division.DivisionName,
            //                                      DepartmentCode = b.DepartmentCode,
            //                                      DepartmentName = b.DepartmentName
            //                                  }).ToList();
            //        }
            //        else
            //        {
            //            result.Message = "Department Data Not Found";
            //            result.IsSuccess = false;
            //        }

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllDepartmentResponse>(ex);
            //    }
            //}

            //[HttpGet]
            //public ActionResult Master_GetAllUniformItems(string deviceSerialNo, int userID, int warehouseID, int divisionID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}, divisionID: {divisionID}");

            //        var data = from b in UniformItemTable.GetAllItems(_db)
            //                   orderby b.UniformItemName
            //                   select new UniformItemData()
            //                   {
            //                       ItemCode = b.UniformItemCode,
            //                       ItemName = b.UniformItemName,

            //                       ItemCategory = b.UniformCategory.UniformCategoryName,

            //                       Material = b.Material.MaterialName,
            //                       Color = b.Color.ColorName,
            //                       Logo = b.Logo.LogoName,

            //                       UniformItemID = b.UniformItemID
            //                   };

            //        var result = new GetAllUniformItemResponse()
            //        {
            //            UniformItems = data.ToList()
            //        };

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllUniformItemResponse>(ex);
            //    }
            //}

            //[HttpGet]
            //public ActionResult Master_GetEmployeeUniformSet(string deviceSerialNo, int userID, int warehouseID, int personID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}, employeeID: {personID}");

            //        var person = PersonTable.GetItem(_db, personID);
            //        if (person.UniformSetID == null)
            //        {
            //            //return ErrorActionResult<GetAllUniformSetResponse>($"Uniform set not defined for {person.PersonFirstName}");
            //            return Json(new GetAllUniformSetResponse());
            //        }
            //        else
            //        {
            //            var set = (from b in UniformSetTable.GetAllItems(_db)
            //                       where b.UniformSetID == person.UniformSetID
            //                       select new UniformSetData()
            //                       {
            //                           UniformSetCode = b.UniformSetCode,
            //                           UniformSetName = b.UniformSetName
            //                       }).FirstOrDefault();

            //            set.UniformItems = (from b in UniformSetLineItemTable.GetAllItems(_db)
            //                                where b.UniformSetID == person.UniformSetID
            //                                select new UniformItemData()
            //                                {
            //                                    UniformItemID = b.UniformItemID,
            //                                    ItemCategory = b.UniformItem.UniformCategory.UniformCategoryName,
            //                                    ItemCode = b.UniformItem.UniformItemCode,
            //                                    ItemName = b.UniformItem.UniformItemName,
            //                                    Qty = b.Qty,

            //                                    Color = b.UniformItem.Color.ColorName,
            //                                    Logo = b.UniformItem.Logo.LogoName,
            //                                    Material = b.UniformItem.Material.MaterialName
            //                                }).ToList();

            //            var result = new GetAllUniformSetResponse() { UniformSets = new List<UniformSetData>() { set } };

            //            return Json(result);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllUniformSetResponse>(ex);
            //    }
            //}

            //[HttpGet]
            //public ActionResult Master_GetAllSizes(string deviceSerialNo, int userID, int warehouseID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");

            //        var data = from b in SizeTable.GetAllItems(_db)
            //                   select new SizeData()
            //                   {
            //                       SizeID = b.SizeID,
            //                       SizeCode = b.SizeCode,
            //                       SizeName = b.SizeName,
            //                       IsManualSize = (b.IsManualSize.HasValue) ? b.IsManualSize.Value : false
            //                   };

            //        var result = new GetAllSizeResponse()
            //        {
            //            Sizes = data.ToList()
            //        };
            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllSizeResponse>(ex);
            //    }
            //}

            //[HttpGet]
            //public ActionResult Master_GetAllDisposalReasons(string deviceSerialNo, int userID, int warehouseID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");

            //        var data = from b in DisposalReasonTable.GetAllItems(_db)
            //                   select new ReasonData()
            //                   {
            //                       ReasonID = b.DisposalReasonID,
            //                       ReasonCode = b.DisposalReasonCode,
            //                       ReasonName = b.DisposalReasonName
            //                   };

            //        var result = new GetAllReasonsResponse()
            //        {
            //            Reasons = data.ToList()
            //        };
            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllReasonsResponse>(ex);
            //    }
            //}

            //[HttpGet]
            //public ActionResult Master_GetAllReturnReasons(string deviceSerialNo, int userID, int warehouseID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");

            //        var data = from b in ReturnReasonTable.GetAllItems(_db)
            //                   select new ReasonData()
            //                   {
            //                       ReasonID = b.ReturnReasonID,
            //                       ReasonCode = b.ReturnReasonCode,
            //                       ReasonName = b.ReturnReasonName
            //                   };

            //        var result = new GetAllReasonsResponse()
            //        {
            //            Reasons = data.ToList()
            //        };
            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllReasonsResponse>(ex);
            //    }
            //}

            //[HttpGet]
            //public ActionResult Master_GetAllLaundryVendors(string deviceSerialNo, int userID, int warehouseID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");

            //        var data = from b in LaundryVendorTable.GetAllItems(_db)
            //                   select new LaundryVendorData()
            //                   {
            //                       LaundryVendorID = b.LaundryVendorID,
            //                       LaundryVendorCode = b.LaundryVendorCode,
            //                       LaundryVendorName = b.LaundryVendorName,
            //                   };

            //        var result = new GetAllLaundryVendorsResponse()
            //        {
            //            LaundryVendors = data.ToList()
            //        };
            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllLaundryVendorsResponse>(ex);
            //    }
            //}

            //[HttpGet]
            //public ActionResult Master_GetAllUnprocessedUniformReturnReasons(string deviceSerialNo, int userID, int warehouseID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");

            //        var data = from b in UnprocessedUniformReturnReasonTable.GetAllItems(_db)
            //                   select new UnprocessedUniformReturnReasonData()
            //                   {
            //                       UnprocessedUniformReturnReasonID = b.UnprocessedUniformReturnReasonID,
            //                       UnprocessedUniformReturnReasonCode = b.UnprocessedUniformReturnReasonCode,
            //                       UnprocessedUniformReturnReasonName = b.UnprocessedUniformReturnReasonName,
            //                   };

            //        var result = new GetAllUnprocessedUniformReturnReasonsResponse()
            //        {
            //            UnprocessedUniformReturnReasons = data.ToList()
            //        };
            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllUnprocessedUniformReturnReasonsResponse>(ex);
            //    }
            //}

            //#endregion

            //public ActionResult GetAllEmployeeDetails(string deviceSerialNo, int userID, int warehouseID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");
            //        //_db.EnableInstanceQueryLog = true;
            //        var lineItems = (from b in PersonTable.GetAllItems(_db)
            //                         where b.LaundryVendorID == null && b.DepartmentID !=null
            //                         orderby b.PersonCode
            //                         select new PersonData()
            //                         {
            //                             PersonID = b.PersonID,
            //                             PersonCode = b.PersonCode,
            //                             PersonFirstName = b.PersonFirstName,
            //                             PersonLastName = b.PersonLastName,
            //                             Email = b.EMailID,
            //                             MobileNo = b.MobileNo,
            //                             WhatsAppMobileNo = b.WhatsAppMobileNo,
            //                             Gender = b.Gender,
            //                             DOJ = b.DOJ,

            //                             DepartmentID = b.DepartmentID.Value,
            //                             DivisionID = b.Department.DivisionID,

            //                             DepartmentName = b.Department.DepartmentName,
            //                             DivisionName = b.Department.Division.DivisionName,
            //                         }).ToList();

            //        //_db.EnableInstanceQueryLog un= false;

            //        var result = new GetDepartmentEmployeeDetailResponse()
            //        {
            //            LineItems = lineItems
            //        };

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetDepartmentEmployeeDetailResponse>(ex);
            //    }
            //}

            //public ActionResult GetDepartmentEmployeeDetails(string deviceSerialNo, int userID, int warehouseID, int departmentID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}, {departmentID}");
            //        //_db.EnableInstanceQueryLog = true;
            //        var lineItems = (from b in PersonTable.GetAllItems(_db)
            //                         where b.DepartmentID == departmentID
            //                         orderby b.PersonCode
            //                         select new PersonData()
            //                         {
            //                             PersonID = b.PersonID,
            //                             PersonCode = b.PersonCode,
            //                             PersonFirstName = b.PersonFirstName,
            //                             PersonLastName = b.PersonLastName,
            //                             Email = b.EMailID,
            //                             MobileNo = b.MobileNo,
            //                             WhatsAppMobileNo = b.WhatsAppMobileNo,
            //                             Gender = b.Gender,
            //                             DOJ = b.DOJ
            //                         }).ToList();

            //        //_db.EnableInstanceQueryLog un= false;

            //        var result = new GetDepartmentEmployeeDetailResponse()
            //        {
            //            LineItems = lineItems
            //        };

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetDepartmentEmployeeDetailResponse>(ex);
            //    }
            //}

            //public ActionResult GetPendingUniformRequestEmployeeDetails(string deviceSerialNo, int userID, int warehouseID, int uniformRequestID, int departmentID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}, {uniformRequestID}, {departmentID}");
            //        //_db.EnableInstanceQueryLog = true;
            //        var lineItems = (from b in UniformRequestLineItemTable.GetAllItems(_db)
            //                         where b.UniformRequestID == uniformRequestID
            //                                && b.IssuedUniformQty < b.Qty
            //                         //&& (departmentID == 0 || b.Employee.DepartmentID == departmentID)
            //                         orderby b.Employee.PersonCode
            //                         select new PersonData()
            //                         {
            //                             PersonID = b.Employee.PersonID,
            //                             PersonCode = b.Employee.PersonCode,
            //                             PersonFirstName = b.Employee.PersonFirstName,
            //                             PersonLastName = b.Employee.PersonLastName,
            //                             Email = b.Employee.EMailID,
            //                             MobileNo = b.Employee.MobileNo,
            //                             WhatsAppMobileNo = b.Employee.WhatsAppMobileNo,
            //                             Gender = b.Employee.Gender,
            //                             DOJ = b.Employee.DOJ,

            //                             DepartmentID = b.Employee.DepartmentID.Value
            //                         }).Distinct().ToList();

            //        //_db.EnableInstanceQueryLog un= false;

            //        var result = new GetDepartmentEmployeeDetailResponse()
            //        {
            //            LineItems = lineItems
            //        };

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetDepartmentEmployeeDetailResponse>(ex);
            //    }
            //}

            //public ActionResult GetAllPendingUniformRequests(string deviceSerialNo, int userID, int warehouseID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");
            //        //_db.EnableInstanceQueryLog = true;
            //        var lineItems = (from b in UniformRequestTable.GetAllItems(_db)
            //                         where b.ApprovalStatusID == 2
            //                                && b.IsClosed == false
            //                                && (from c in _db.UniformRequestLineItemTable
            //                                    where c.IssuedUniformQty < c.Qty
            //                                            && c.StatusID != (int)StatusValue.Deleted
            //                                    select c.UniformRequestID).Contains(b.UniformRequestID)
            //                         orderby b.UniformRequestCode
            //                         select new UniformRequestData()
            //                         {
            //                             UniformRequestID = b.UniformRequestID,
            //                             UniformRequestCode = b.UniformRequestCode,
            //                             UniformRequestName = b.UniformRequestCode,
            //                             DivisionID = b.DivisionID,
            //                         }).ToList();

            //        var result = new GetAllUniformRequestResponse()
            //        {
            //            UniformRequests = lineItems
            //        };

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllUniformRequestResponse>(ex);
            //    }
            //}

            //[HttpPost]
            //public ActionResult SaveSchedule([FromBody] ScheduleData scheduleData)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{scheduleData.DeviceSerialNo}, {scheduleData.UserID}, {scheduleData.WarehouseID}, Employee Count: {scheduleData.EmployeeIds.Count}");
            //        //_db.EnableInstanceQueryLog = true;
            //        var result = new BaseResponse();
            //        if (scheduleData != null)
            //        {
            //            ScheduleTable schedule = new ScheduleTable();
            //            schedule.ScheduleDate = scheduleData.ScheduleDate;
            //            schedule.DepartmentID = scheduleData.DepartmentID;
            //            schedule.UniformRequestID = scheduleData.UniformRequestID;
            //            schedule.LinenRoomID = scheduleData.WarehouseID;

            //            schedule.CreatedBy = scheduleData.UserID;
            //            schedule.CreatedDateTime = DateTime.Now;
            //            schedule.StatusID = (int)StatusValue.Active;

            //            _db.Add(schedule);

            //            foreach (var LI in scheduleData.EmployeeIds)
            //            {
            //                ScheduleLineItemTable lineItem = new ScheduleLineItemTable();

            //                lineItem.Schedule = schedule;
            //                lineItem.EmployeeID = LI;
            //                lineItem.CreatedBy = scheduleData.UserID;
            //                lineItem.CreatedDateTime = DateTime.Now;
            //                lineItem.StatusID = (int)StatusValue.Active;

            //                var employee = UniformRequestLineItemTable.GetItem(_db, scheduleData.UniformRequestID.Value, LI);
            //                if (employee != null)
            //                {
            //                    employee.EmployeeRequested = true;
            //                }

            //                _db.Add(lineItem);
            //            }
            //        }

            //        _db.SaveChanges();
            //        result.Message = "Saved Successfully";
            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetDepartmentEmployeeDetailResponse>(ex);
            //    }
            //}

            //public ActionResult BarcodePrinting_GetEmployeeUniformRequest(string deviceSerialNo, int userID, int warehouseID, int employeeID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");
            //        //_db.EnableInstanceQueryLog = true;
            //        var lineItems = (from b in UniformRequestLineItemTable.GetAllItems(_db)
            //                         where b.UniformRequest.ApprovalStatusID == 2
            //                                && b.UniformRequest.IsClosed == false
            //                                && b.StatusID != (int)StatusValue.Deleted
            //                                && b.EmployeeID == employeeID
            //                                && b.IssuedUniformQty < b.Qty
            //                         select new UniformItemData()
            //                         {
            //                             ItemCode = b.UniformItem.UniformItemCode,
            //                             ItemName = b.UniformItem.UniformItemName,

            //                             ItemCategory = b.UniformItem.UniformCategory.UniformCategoryName,

            //                             Material = b.UniformItem.Material.MaterialName,
            //                             Color = b.UniformItem.Color.ColorName,
            //                             Logo = b.UniformItem.Logo.LogoName,

            //                             SizeID = b.SizeID,
            //                             SizeName = b.Size.SizeName,
            //                             ManualSizeValue = b.ManualSizeValue,
            //                             UniformItemID = b.UniformItemID,
            //                             UniformSetID = b.UniformSetID,
            //                             UniformRequestID = b.UniformRequestID,
            //                             Qty = b.Qty - b.IssuedUniformQty,

            //                         }).ToList();

            //        var result = new GetAllUniformItemResponse()
            //        {
            //            UniformItems = lineItems
            //        };

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllUniformItemResponse>(ex);
            //    }
            //}

            //[HttpPost]
            //public ActionResult CreateEmployeeUniforms([FromBody] CreateUniformRequest requestData)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{requestData.DeviceSerialNo}, {requestData.UserID}, {requestData.WarehouseID}, ItemCount: {requestData.Items.Count}");
            //        _db.CurrentUserID = requestData.UserID;

            //        List<EmployeeUniformTable> newItems = new List<EmployeeUniformTable>();

            //        //process employee wise
            //        foreach (var itm in requestData.Items)
            //        {
            //            var person = PersonTable.GetItem(_db, itm.EmployeeID);
            //            var uniformItem = UniformItemTable.GetItem(_db, itm.UniformItemID);
            //            var uniformCategory = UniformCategoryTable.GetItem(_db, uniformItem.UniformCategoryID);

            //            if (person == null) throw new ValidationException("Employee required");
            //            if (uniformItem == null) throw new ValidationException("Uniform Item required");

            //            var barcodePrefix = $"{person.PersonCode}_{uniformCategory.UniformCategoryCode}_";
            //            int startingSeqNo = 0;

            //            var alreadyUsed = newItems.Where(b => b.EmployeeID == itm.EmployeeID && b.UniformBarcode.StartsWith(barcodePrefix));
            //            if (alreadyUsed.Any())
            //            {
            //                startingSeqNo = alreadyUsed.Max(b => b.SequenceNo);
            //            }
            //            else
            //            {
            //                //get the last used no based on category
            //                var lu = (from b in EmployeeUniformTable.GetAllItems(_db)
            //                          where b.EmployeeID == itm.EmployeeID
            //                                && b.UniformBarcode.StartsWith(barcodePrefix)
            //                          select b.SequenceNo);

            //                if (lu.Any())
            //                {
            //                    startingSeqNo = Math.Max(lu.Max(), 0);
            //                }
            //            }

            //            //only allow pending qtys
            //            var tempdate = DateTime.Now.AddYears(1);
            //            for (int i = 0; i < itm.Qty; i++)
            //            {

            //                var uniform = new EmployeeUniformTable()
            //                {
            //                    EmployeeID = itm.EmployeeID,
            //                    UniformItemID = itm.UniformItemID,
            //                    SizeID = itm.SizeID,
            //                    ManualSizeValue = itm.ManualSizeValue,
            //                    Qty = 1,
            //                    UniformRequestID = itm.UniformRequestID,
            //                    UniformSetID = itm.UniformSetID,
            //                    ExpiryDate = new DateTime(tempdate.Year, 4, tempdate.Day),
            //                    SequenceNo = startingSeqNo + i + 1,
            //                    UniformBarcode = $"{barcodePrefix}{startingSeqNo + i + 1:000}",
            //                    CreatedBy = requestData.UserID,
            //                    CreatedDateTime = DateTime.Now,
            //                    IssuedFromLinenRoomID= requestData.WarehouseID
            //                };

            //                if (itm.UniformRequestID <= 0)
            //                    uniform.UniformRequestID = null;

            //                newItems.Add(uniform);
            //                _db.Add(uniform);
            //            }

            //            //Get employee pending items
            //            var uniformRequests = UniformRequestLineItemTable.GetPendingUniformRequests(_db, itm.EmployeeID, itm.UniformItemID);
            //            var currentIssuedQtys = itm.Qty;

            //            foreach (var uniformRequest in uniformRequests)
            //            {
            //                if (currentIssuedQtys > (uniformRequest.Qty - uniformRequest.IssuedUniformQty))
            //                {
            //                    //issued qtys are greater than pending qty, so reduce the issued qty
            //                    currentIssuedQtys = currentIssuedQtys - (uniformRequest.Qty - uniformRequest.IssuedUniformQty);
            //                    //make issued qtys as equal to requested qtys
            //                    uniformRequest.IssuedUniformQty = uniformRequest.Qty;
            //                }
            //                else
            //                {
            //                    //issued qtys are lesser than or equal to pending qty
            //                    uniformRequest.IssuedUniformQty = currentIssuedQtys + uniformRequest.IssuedUniformQty;
            //                    currentIssuedQtys = 0;
            //                    break;
            //                }
            //            }
            //        }

            //        _db.SaveChanges();

            //        var ids = (from b in newItems select b.EmployeeUniformID).ToList();
            //        //_db.EnableInstanceQueryLog = true;
            //        var uniformData = (from b in EmployeeUniformTable.GetAllItems(_db)
            //                           where (from c in ids select c).Contains(b.EmployeeUniformID)
            //                           select new EmployeeUniformData()
            //                           {
            //                               EmployeeUniformID = b.EmployeeUniformID,
            //                               UniformItemID = b.UniformItemID,
            //                               EmployeeID = b.EmployeeID,
            //                               PersonCode = b.Employee.PersonCode,
            //                               PersonFirstName = b.Employee.PersonFirstName,
            //                               PersonLastName = b.Employee.PersonLastName,

            //                               UniformBarcode = b.UniformBarcode,

            //                               ItemCategory = b.UniformItem.UniformCategory.UniformCategoryName,
            //                               ItemCode = b.UniformItem.UniformItemCode,
            //                               ItemName = b.UniformItem.UniformItemName,

            //                               Color = b.UniformItem.Color.ColorName,
            //                               Material = b.UniformItem.Material.MaterialName,
            //                               Logo = b.UniformItem.Logo.LogoName,
            //                               Size = b.Size.SizeName,
            //                               ManualSizeValue = b.ManualSizeValue,
            //                           }).ToList();

            //        var result = new GetEmployeeUniformResponse()
            //        {
            //            Uniforms = uniformData
            //        };

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetEmployeeUniformResponse>(ex);
            //    }
            //}

            //public ActionResult CreateEmployeeUniformBarcode([FromBody] UniformIDRequest requestData)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{requestData.DeviceSerialNo}, {requestData.UserID}, {requestData.WarehouseID}, ItemCount: {requestData.EmployeeUniformIDs.Count}");
            //        _db.CurrentUserID = requestData.UserID;

            //        var format = BarcodeFormatTable.GetAllItems(_db).Take(1).FirstOrDefault();

            //        if (format == null)
            //        {
            //            throw new ValidationException("Barcode format not available");
            //        }

            //        //_db.EnableInstanceQueryLog = true;
            //        var barcodeData = (from b in EmployeeUniformTable.GetAllItems(_db)
            //                           where (from c in requestData.EmployeeUniformIDs select c).Contains(b.EmployeeUniformID)
            //                            && b.UniformItem.UniformCategory.BarcodePrintingRequired == true
            //                            && b.EmployeeUniformStatusID < (int)EmployeeUniformStatusValues.Disposed
            //                           select new BarcodeData()
            //                           {
            //                               EmployeeUniformID = b.EmployeeUniformID,
            //                               Barcode = b.UniformBarcode,
            //                               BarcodePrintContent = format.BarcodeFormat.Replace("##Barcode##", b.UniformBarcode),
            //                           }).ToList();

            //        var result = new GenerateBarcodeResponse()
            //        {
            //            Barcodes = barcodeData
            //        };

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GenerateBarcodeResponse>(ex);
            //    }
            //}

            //public ActionResult DisposeUniforms([FromBody] UniformDisposalRequest requestData)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{requestData.DeviceSerialNo}, {requestData.UserID}, {requestData.WarehouseID}, ItemCount: {requestData.EmployeeUniformIDs.Count}");
            //        _db.CurrentUserID = requestData.UserID;

            //        //_db.EnableInstanceQueryLog = true;
            //        var uniformsList = (from b in EmployeeUniformTable.GetAllItems(_db)
            //                            where (from c in requestData.EmployeeUniformIDs select c).Contains(b.EmployeeUniformID)
            //                            select b).ToList();

            //        foreach (var b in uniformsList)
            //        {
            //            b.EmployeeUniformStatusID = (int)EmployeeUniformStatusValues.Disposed;
            //            //b.Delete();
            //        }
            //        _db.SaveChanges();

            //        return Json(new BaseResponse());
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<BaseResponse>(ex);
            //    }
            //}

            //public ActionResult ReturnUniformsToCompany([FromBody] UniformReturnRequest requestData)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{requestData.DeviceSerialNo}, {requestData.UserID}, {requestData.WarehouseID}, ItemCount: {requestData.EmployeeUniformIDs.Count}");
            //        _db.CurrentUserID = requestData.UserID;

            //        //_db.EnableInstanceQueryLog = true;
            //        var uniformsList = (from b in EmployeeUniformTable.GetAllItems(_db)
            //                            where (from c in requestData.EmployeeUniformIDs select c).Contains(b.EmployeeUniformID)
            //                            select b).ToList();

            //        foreach (var b in uniformsList)
            //        {
            //            b.EmployeeUniformStatusID = (int)EmployeeUniformStatusValues.Returned;
            //            //b.Delete();
            //        }
            //        _db.SaveChanges();

            //        return Json(new BaseResponse());
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<BaseResponse>(ex);
            //    }
            //}

            //public ActionResult GetEmployeeUniformDetails(string deviceSerialNo, int userID, int warehouseID, string uniformBarcode, string? ScreenName, int? workorderID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}, {uniformBarcode}, {ScreenName}");
            //        bool loadUnissuedNonWashableItems = false;

            //        if (!string.IsNullOrEmpty(ScreenName))
            //        {
            //            var uniformDataForValidation = (from b in EmployeeUniformTable.GetAllItems(_db)
            //                                            where b.UniformBarcode == uniformBarcode
            //                                                  && b.EmployeeUniformStatusID < (int)EmployeeUniformStatusValues.Disposed
            //                                            select new
            //                                            {
            //                                                Item = b,
            //                                                b.UniformItem.UniformCategory.UniformCategoryName,
            //                                                b.UniformItem.UniformCategory.AllowedToWash,
            //                                                b.EmployeeUniformStatus.EmployeeUniformStatusCode
            //                                            }).FirstOrDefault();

            //            if (uniformDataForValidation == null)
            //            {
            //                return ErrorActionResult<GetEmployeeUniformResponse>($"Invalid uniform barcode '{uniformBarcode}'");
            //            }

            //            switch (ScreenName.ToUpper())
            //            {
            //                case "ISSUEUNIFORM":
            //                    {
            //                        if (uniformDataForValidation.Item.EmployeeUniformStatusID != (int)EmployeeUniformStatusValues.New)
            //                        {
            //                            return ErrorActionResult<GetEmployeeUniformResponse>($"Uniform already issued");
            //                        }
            //                        loadUnissuedNonWashableItems = true;

            //                        break;
            //                    }

            //                //Collect uniform for laundery
            //                case "COLLECTUNIFORM":
            //                    {
            //                        if (uniformDataForValidation.Item.EmployeeUniformStatusID == (int)EmployeeUniformStatusValues.New)
            //                        {
            //                            return ErrorActionResult<GetEmployeeUniformResponse>($"Uniform not yet issued to employee");
            //                        }
            //                        if (uniformDataForValidation.Item.EmployeeUniformStatusID < (int)EmployeeUniformStatusValues.WithEmployee)
            //                        {
            //                            return ErrorActionResult<GetEmployeeUniformResponse>($"Uniform not yet confirmed by employee");
            //                        }
            //                        if (uniformDataForValidation.AllowedToWash == false)
            //                        {
            //                            return ErrorActionResult<GetEmployeeUniformResponse>($"Item '{uniformDataForValidation.UniformCategoryName}' not allowed to wash");
            //                        }
            //                        if (uniformDataForValidation.Item.EmployeeUniformStatusID != (int)EmployeeUniformStatusValues.WithEmployee)
            //                        {
            //                            return ErrorActionResult<GetEmployeeUniformResponse>($"Invalid uniform status. Current Status: '{uniformDataForValidation.EmployeeUniformStatusCode}'");
            //                        }

            //                        var IsValid = (from c in UniformTransactionTable.GetAllItems(_db)
            //                                       where c.EmployeeUniform.UniformBarcode == uniformBarcode
            //                                            && c.ProcessStatusID != (int)ProcessStatusValue.ReturnToEmployee
            //                                       select c);

            //                        if (IsValid.Any())
            //                        {
            //                            return ErrorActionResult<GetEmployeeUniformResponse>($"Uniform barcode already in progress - '{uniformBarcode}'");
            //                        }

            //                        break;
            //                    }

            //                case "VENDORCOLLECTION":
            //                    {
            //                        if (uniformDataForValidation.Item.EmployeeUniformStatusID != (int)EmployeeUniformStatusValues.InWorkOrder)
            //                        {
            //                            return ErrorActionResult<GetEmployeeUniformResponse>($"Invalid uniform status. Current Status: '{uniformDataForValidation.EmployeeUniformStatusCode}'");
            //                        }

            //                        //item should be in workorder
            //                        var workorderFound = from b in WorkOrderLineItemTable.GetAllItems(_db)
            //                                             where b.WorkOrderID == workorderID
            //                                             select b;

            //                        var IsValid = (from c in UniformTransactionTable.GetAllItems(_db)
            //                                       where c.EmployeeUniform.UniformBarcode == uniformBarcode
            //                                            && c.ProcessStatusID != (int)ProcessStatusValue.WorkOrderCreated
            //                                            && c.ProcessStatusID != (int)ProcessStatusValue.ReturnToEmployee
            //                                            && c.WorkOrderLineItem.WorkOrderID == workorderID
            //                                       select c);

            //                        if (IsValid.Any())
            //                        {
            //                            return ErrorActionResult<GetEmployeeUniformResponse>($"Uniform barcode already in progress - '{uniformBarcode}'");
            //                        }

            //                        break;
            //                    }

            //                case "VENDORRETURN":
            //                    {
            //                        if (uniformDataForValidation.Item.EmployeeUniformStatusID != (int)EmployeeUniformStatusValues.WithLaundryVendor)
            //                        {
            //                            return ErrorActionResult<GetEmployeeUniformResponse>($"Invalid uniform status. Current Status: '{uniformDataForValidation.EmployeeUniformStatusCode}'");
            //                        }

            //                        var IsValid = (from c in UniformTransactionTable.GetAllItems(_db)
            //                                       where c.EmployeeUniform.UniformBarcode == uniformBarcode
            //                                            && c.ProcessStatusID != (int)ProcessStatusValue.CollectedByVendor
            //                                            && c.ProcessStatusID != (int)ProcessStatusValue.ReturnToEmployee
            //                                       select c);

            //                        if (IsValid.Any())
            //                        {
            //                            return ErrorActionResult<GetEmployeeUniformResponse>($"Uniform barcode already in progress - '{uniformBarcode}'");
            //                        }

            //                        break;
            //                    }

            //                //Collect the items from vendor by linen room
            //                case "LINENCOLLECTIONFROMVENDOR":
            //                    {
            //                        if (uniformDataForValidation.Item.EmployeeUniformStatusID != (int)EmployeeUniformStatusValues.WithLaundryVendor)
            //                        {
            //                            return ErrorActionResult<GetEmployeeUniformResponse>($"Invalid uniform status. Current Status: '{uniformDataForValidation.EmployeeUniformStatusCode}'");
            //                        }

            //                        var IsValid = (from c in UniformTransactionTable.GetAllItems(_db)
            //                                       where c.EmployeeUniform.UniformBarcode == uniformBarcode
            //                                            && c.ProcessStatusID != (int)ProcessStatusValue.ProcessedUniformReturnByVendor
            //                                            && c.ProcessStatusID != (int)ProcessStatusValue.UnprocessedUniformReturnByVendor
            //                                            && c.ProcessStatusID != (int)ProcessStatusValue.ReturnToEmployee
            //                                       select c);

            //                        if (IsValid.Any())
            //                        {
            //                            return ErrorActionResult<GetEmployeeUniformResponse>($"Uniform barcode already in progress - '{uniformBarcode}'");
            //                        }

            //                        break;
            //                    }

            //                //Collect the items from vendor by linen room
            //                case "RETURNTOEMPLOYEE":
            //                    {
            //                        if (uniformDataForValidation.Item.EmployeeUniformStatusID != (int)EmployeeUniformStatusValues.InLinenRoomForReturnToEmployee)
            //                        {
            //                            return ErrorActionResult<GetEmployeeUniformResponse>($"Invalid uniform status. Current Status: '{uniformDataForValidation.EmployeeUniformStatusCode}'");
            //                        }

            //                        var IsValid = (from c in UniformTransactionTable.GetAllItems(_db)
            //                                       where c.EmployeeUniform.UniformBarcode == uniformBarcode
            //                                            && c.ProcessStatusID != (int)ProcessStatusValue.ProcessedUniformReturnByVendor
            //                                            && c.ProcessStatusID != (int)ProcessStatusValue.UnprocessedUniformReturnByVendor
            //                                            && c.ProcessStatusID != (int)ProcessStatusValue.ReturnToEmployee
            //                                       select c);

            //                        if (IsValid.Any())
            //                        {
            //                            return ErrorActionResult<GetEmployeeUniformResponse>($"Uniform barcode already in progress - '{uniformBarcode}'");
            //                        }

            //                        break;
            //                    }
            //            }
            //        }

            //        var uniformData = (from b in EmployeeUniformTable.GetAllItems(_db)
            //                           where b.UniformBarcode == uniformBarcode
            //                                && b.EmployeeUniformStatusID < (int)EmployeeUniformStatusValues.Disposed
            //                           select new
            //                           {
            //                               Employee = new PersonData()
            //                               {
            //                                   PersonID = b.Employee.PersonID,
            //                                   PersonCode = b.Employee.PersonCode,
            //                                   PersonFirstName = b.Employee.PersonFirstName,
            //                                   PersonLastName = b.Employee.PersonLastName,
            //                                   Email = b.Employee.EMailID,
            //                                   MobileNo = b.Employee.MobileNo,
            //                                   WhatsAppMobileNo = b.Employee.WhatsAppMobileNo,
            //                                   Gender = b.Employee.Gender,
            //                                   DOJ = b.Employee.DOJ,
            //                                   CustomerName = b.Employee.Customer.CustomerName
            //                               },
            //                               EmployeeUniformData = new EmployeeUniformData()
            //                               {
            //                                   EmployeeUniformID = b.EmployeeUniformID,
            //                                   UniformItemID = b.UniformItemID,
            //                                   EmployeeID = b.EmployeeID,

            //                                   UniformBarcode = b.UniformBarcode,

            //                                   ItemCategory = b.UniformItem.UniformCategory.UniformCategoryName,
            //                                   ItemCode = b.UniformItem.UniformItemCode,
            //                                   ItemName = b.UniformItem.UniformItemName,

            //                                   Color = b.UniformItem.Color.ColorName,
            //                                   Material = b.UniformItem.Material.MaterialName,
            //                                   Logo = b.UniformItem.Logo.LogoName,
            //                                   Size = b.Size.SizeName,
            //                                   ManualSizeValue = b.ManualSizeValue,

            //                                   PersonCode = b.Employee.PersonCode,
            //                                   PersonFirstName = b.Employee.PersonFirstName,
            //                                   PersonLastName = b.Employee.PersonLastName,
            //                               }
            //                           }).FirstOrDefault();

            //        if ((uniformData == null) || (uniformData.EmployeeUniformData == null))
            //        {
            //            return ErrorActionResult<GetEmployeeUniformResponse>($"Uniform barcode not available - '{uniformBarcode}'");
            //        }

            //        var uniformItems = new List<EmployeeUniformData>() { uniformData.EmployeeUniformData };
            //        if (loadUnissuedNonWashableItems)
            //        {
            //            var newNonWashableItems = (from b in EmployeeUniformTable.GetAllItems(_db)
            //                                       where b.EmployeeUniformStatusID == (int)EmployeeUniformStatusValues.New
            //                                            && b.EmployeeID == uniformData.Employee.PersonID
            //                                            && b.UniformItem.UniformCategory.AllowedToWash == false
            //                                       select new EmployeeUniformData()
            //                                       {
            //                                           EmployeeUniformID = b.EmployeeUniformID,
            //                                           UniformItemID = b.UniformItemID,
            //                                           EmployeeID = b.EmployeeID,

            //                                           UniformBarcode = b.UniformBarcode,

            //                                           ItemCategory = b.UniformItem.UniformCategory.UniformCategoryName,
            //                                           ItemCode = b.UniformItem.UniformItemCode,
            //                                           ItemName = b.UniformItem.UniformItemName,

            //                                           Color = b.UniformItem.Color.ColorName,
            //                                           Material = b.UniformItem.Material.MaterialName,
            //                                           Logo = b.UniformItem.Logo.LogoName,
            //                                           Size = b.Size.SizeName,
            //                                           ManualSizeValue = b.ManualSizeValue,

            //                                           PersonCode = b.Employee.PersonCode,
            //                                           PersonFirstName = b.Employee.PersonFirstName,
            //                                           PersonLastName = b.Employee.PersonLastName,
            //                                       });

            //            uniformItems.AddRange(newNonWashableItems);
            //        }

            //        var result = new GetEmployeeUniformResponse()
            //        {
            //            Employee = uniformData.Employee,
            //            Uniforms = uniformItems
            //        };

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetEmployeeUniformResponse>(ex);
            //    }
            //}

            //public ActionResult GetEmployeeAllUniforms(string deviceSerialNo, int userID, int warehouseID, int employeeID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}, {employeeID}");
            //        var employeeData = (from b in PersonTable.GetAllItems(_db)
            //                            where b.PersonID == employeeID
            //                            select new PersonData()
            //                            {
            //                                PersonID = b.PersonID,
            //                                PersonCode = b.PersonCode,
            //                                PersonFirstName = b.PersonFirstName,
            //                                PersonLastName = b.PersonLastName,
            //                                Email = b.EMailID,
            //                                MobileNo = b.MobileNo,
            //                                WhatsAppMobileNo = b.WhatsAppMobileNo,
            //                                Gender = b.Gender,
            //                                DOJ = b.DOJ,
            //                                DepartmentName = b.Department.DepartmentName,
            //                                DivisionName = b.Department.Division.DivisionName,
            //                                CustomerName = b.Customer.CustomerName
            //                            }).FirstOrDefault();

            //        //_db.EnableInstanceQueryLog = true;
            //        var uniformData = (from b in EmployeeUniformTable.GetAllItems(_db)
            //                           where b.EmployeeID == employeeID
            //                           && b.EmployeeUniformStatusID < (int)EmployeeUniformStatusValues.Disposed
            //                           select new EmployeeUniformData()
            //                           {
            //                               EmployeeUniformID = b.EmployeeUniformID,
            //                               UniformItemID = b.UniformItemID,
            //                               EmployeeID = b.EmployeeID,

            //                               UniformBarcode = b.UniformBarcode,

            //                               ItemCategory = b.UniformItem.UniformCategory.UniformCategoryName,
            //                               ItemCode = b.UniformItem.UniformItemCode,
            //                               ItemName = b.UniformItem.UniformItemName,

            //                               Color = b.UniformItem.Color.ColorName,
            //                               Material = b.UniformItem.Material.MaterialName,
            //                               Logo = b.UniformItem.Logo.LogoName,
            //                               Size = b.Size.SizeName,
            //                               ManualSizeValue = b.ManualSizeValue,

            //                               UniformStatusID = b.EmployeeUniformStatusID,
            //                               UniformStatus = b.EmployeeUniformStatus.EmployeeUniformStatusName,

            //                               IssuedDate = b.IssuedDate,
            //                               WashCount = b.WashCount,

            //                               UniformRequestCode = b.UniformRequest.UniformRequestCode,
            //                               UniformSetCode = b.UniformSet.UniformSetCode
            //                           }).ToList();

            //        var result = new GetEmployeeUniformResponse()
            //        {
            //            Employee = employeeData,
            //            Uniforms = uniformData
            //        };
            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetEmployeeUniformResponse>(ex);
            //    }
            //}

            //[HttpPost]
            //public ActionResult IssueEmployeeUniform([FromBody] UniformIDRequest data)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{data.DeviceSerialNo}, {data.UserID}, {data.WarehouseID}, Count: {data.EmployeeUniformIDs.Count}");
            //        var allUniform = (from b in EmployeeUniformTable.GetAllItems(_db)
            //                          where (from c in data.EmployeeUniformIDs
            //                                 select c).Contains(b.EmployeeUniformID)
            //                          select b).ToList();

            //        var receiptNo = CodeGenerationHelper.GetNextCode("UniformIssueReceiptNo");

            //        bool signatureAvailable = false;
            //        //store the signature image
            //        string signatureFilePath = "";
            //        if ((data.SignatureImage != null) && (data.SignatureImage.Length > 0))
            //        {
            //            signatureFilePath = System.IO.Path.Combine(ACSAppContext.UniformIssueSignaturePath, receiptNo + ".jpg");
            //            signatureAvailable = true;

            //            System.IO.File.WriteAllBytes(signatureFilePath, data.SignatureImage);
            //        }
            //        #region InventoryTransaction
            //            InventoryTransactionTable InventoryTransaction = new InventoryTransactionTable();
            //            InventoryTransaction.TransactionNo = CodeGenerationHelper.GetNextCode("InventoryTransaction");
            //            InventoryTransaction.InventoryReferenceNo = InventoryTransaction.TransactionNo;
            //            InventoryTransaction.InventoryTransactionTypeID = (int)InventoryTransactionTypeValue.EmployeeIssue;
            //            _db.Add(InventoryTransaction);
            //        #endregion
            //        foreach (var uniform in allUniform)
            //        {
            //            if (uniform.EmployeeUniformStatusID == (int)EmployeeUniformStatusValues.New)
            //            {
            //                uniform.IssueReceiptNo = receiptNo;
            //                uniform.IssueSignaturePath = signatureFilePath;

            //                if (signatureAvailable)
            //                {
            //                    uniform.ReceiveConfirmedDate = DateTime.Now;
            //                    uniform.EmployeeUniformStatusID = (int)EmployeeUniformStatusValues.WithEmployee;

            //                    InventoryTransactionLineItemTable litem = new InventoryTransactionLineItemTable();
            //                    litem.InventoryTransaction = InventoryTransaction;
            //                    litem.UniformItemID = uniform.UniformItemID;
            //                    litem.SizeID = uniform.SizeID;
            //                    litem.Qty = 1;
            //                    _db.Add(litem);
            //                }
            //                else
            //                {
            //                    uniform.EmployeeUniformStatusID = (int)EmployeeUniformStatusValues.IssuedToEmployee;
            //                }

            //                uniform.IssuedDate = DateTime.Now;
            //            }
            //            else
            //            {
            //                return ErrorActionResult<GetEmployeeUniformResponse>($"Uniform '{uniform.UniformBarcode}' already issued");
            //            }
            //        }
            //        //else
            //        //{
            //        //    return ErrorActionResult<GetEmployeeUniformResponse>($"Invalid uniform barcode '{uniformBarcode}'");
            //        //}

            //        _db.SaveChanges();

            //        return Json(new BaseResult());
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<BaseResult>(ex);
            //    }
            //}

            //#region Desktop Dashboard

            //[HttpGet]
            //public async Task<ActionResult> Dashboard_GetDesktopDashboardData(string deviceSerialNo, int userID, int warehouseID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");
            //        var result = new GetDesktopDashboardDataResponse()
            //        {

            //        };

            //        var proc = new LMSContextProcedures(_db);
            //        var items = await proc.dprc_DataCountAsync();
            //        if ((items != null) && (items.Count > 0))
            //        {
            //            var dataItem = items[0];

            //            result.CountDatas.Add(new DashboardCountData() { Name = "TotalUniforms", Value = dataItem.TotalUniforms + "" });
            //            result.CountDatas.Add(new DashboardCountData() { Name = "UniformsinWashing", Value = dataItem.UniformsInWashing + "" });
            //            result.CountDatas.Add(new DashboardCountData() { Name = "InLinenRoomforWash", Value = dataItem.InLinenRoomForWash + "" });
            //            result.CountDatas.Add(new DashboardCountData() { Name = "InLinenRoomforEmployeeCollection", Value = dataItem.InLinenRoomForEmployeeCollection + "" });
            //            result.CountDatas.Add(new DashboardCountData() { Name = "Noofpendingincidents", Value = dataItem.NoOfPendingIncidents + "" });
            //        }

            //        var chart1 = await proc.dprc_DailyWashedCountAsync(null,null);
            //        DashboardCategoryDataCollection washCount = new DashboardCategoryDataCollection()
            //        {
            //            CollectionName = "DailyWashed",
            //            Data = (from b in chart1
            //                    select new DashboardCategoryData()
            //                    {
            //                        Category = b.CreatedDate,
            //                        Name = b.Color,
            //                        Value = b.count.Value
            //                    }).ToList()
            //        };
            //        result.SeriesDatas.Add(washCount);

            //        var chart2 = await proc.dprc_DivisionWiseCollectedForWashCountAsync(warehouseID);
            //        DashboardCategoryDataCollection divisionWise = new DashboardCategoryDataCollection()
            //        {
            //            CollectionName = "DivisionWiseCollectedForWash",
            //            Data = (from b in chart2
            //                    select new DashboardCategoryData()
            //                    {
            //                        Name = b.DivisionName,
            //                        Value = b.countData.Value,
            //                        ColorCode = b.ColorCode
            //                    }).ToList()
            //        };
            //        result.SeriesDatas.Add(divisionWise);

            //        var chart3 = await proc.dprc_VendorWiseCollectedForWashCountAsync();
            //        DashboardCategoryDataCollection vendorWise = new DashboardCategoryDataCollection()
            //        {
            //            CollectionName = "DailyWashedCount",
            //            Data = (from b in chart3
            //                    select new DashboardCategoryData()
            //                    {
            //                        Name = b.LaundryVendorCode,
            //                        Value = b.CountData.Value
            //                    }).ToList()
            //        };
            //        result.SeriesDatas.Add(vendorWise);

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetDesktopDashboardDataResponse>(ex);
            //    }
            //}

            //#endregion

            //#region Vendor Dashboard

            //[HttpGet]
            //public async Task<ActionResult> Dashboard_GetVendorDashboardData(string deviceSerialNo, int userID, int warehouseID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");
            //        var laundryID = PersonTable.GetPersonLaundryVendorID(_db, userID);
            //        var result = new GetDesktopDashboardDataResponse()
            //        {

            //        };

            //        var proc = new LMSContextProcedures(_db);
            //        var items = await proc.dprc_VendorDataCountAsync(laundryID);
            //        if ((items != null) && (items.Count > 0))
            //        {
            //            foreach (var itm in items)
            //            {
            //                result.CountDatas.Add(new DashboardCountData() { Name = itm.Item, Value = itm.Value + "" });
            //            }
            //        }

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetDesktopDashboardDataResponse>(ex);
            //    }
            //}

            //#endregion

            //#region Collect uniform for laundry

            //[HttpPost]
            //public ActionResult CreateCollectUniformLaundry([FromBody] CollectUniformLaundryListData collectUniformLaundryData)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{collectUniformLaundryData.DeviceSerialNo}, {collectUniformLaundryData.UserID}, {collectUniformLaundryData.WarehouseID},  collect Uniform Laundry Count: {collectUniformLaundryData.CollectUniformLaundry.Count}");
            //        _db.CurrentUserID = collectUniformLaundryData.UserID;
            //        var result = new BaseResponse();

            //        var newCode = CodeGenerationHelper.GetNextCode("UniformCollection");

            //        if (collectUniformLaundryData.CollectUniformLaundry.Count() > 0)
            //        {
            //            foreach (CollectUniformLaundryData data in collectUniformLaundryData.CollectUniformLaundry)
            //            {
            //                UniformTransactionTable uniformTransaction = new UniformTransactionTable();

            //                uniformTransaction.LinenRoomID = collectUniformLaundryData.WarehouseID;
            //                uniformTransaction.EmployeeUniformID = data.EmployeeUniformID;
            //                uniformTransaction.WashTypeID = data.WashTypeID;

            //                uniformTransaction.TransactionTypeID = (int)TransactionTypeValue.CollectedForWash;
            //                uniformTransaction.ProcessStatusID = (int)ProcessStatusValue.CollectedForWashFromEmployee;

            //                uniformTransaction.StatusID = (int)StatusValue.Active;
            //                uniformTransaction.CreatedBy = collectUniformLaundryData.UserID;
            //                uniformTransaction.CreatedDateTime = DateTime.Now;
            //                uniformTransaction.CollectionReceiptNo = newCode;

            //                EmployeeUniformTable.GetItem(_db, data.EmployeeUniformID).EmployeeUniformStatusID = (int)EmployeeUniformStatusValues.InLinenRoomForWash;

            //                _db.Add(uniformTransaction);
            //            }
            //        }
            //        _db.SaveChanges();

            //        result.TransactionNo = newCode;
            //        result.Message = "Saved Successfully";

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<BaseResponse>(ex);
            //    }
            //}

            //#endregion

            //#region Work Order

            //[HttpPost]
            //public ActionResult WorkOrder_GetAllPendingUniforms([FromBody] UniformCategoryExceptListData uniformCategoryExceptListData)
            //{
            //    try
            //    {

            //        base.TraceLog("MobileAPI", $"{uniformCategoryExceptListData.DeviceSerialNo}, {uniformCategoryExceptListData.UserID}, {uniformCategoryExceptListData.WarehouseID}, {uniformCategoryExceptListData.Excepts.Count()}");

            //        var Data = (from b in UniformTransactionTable.GetAllItems(_db)
            //                    join c in EmployeeUniformTable.GetAllItems(_db) on b.EmployeeUniformID equals c.EmployeeUniformID
            //                    join d in UniformItemTable.GetAllItems(_db) on c.UniformItemID equals d.UniformItemID
            //                    join lv in LaundryVendorPricingTable.GetAllItems(_db) on new { d.UniformCategoryID, b.WashTypeID } equals new { lv.UniformCategoryID, lv.WashTypeID }
            //                    where b.TransactionTypeID == (int)TransactionTypeValue.CollectedForWash
            //                          && b.ProcessStatusID == (int)ProcessStatusValue.CollectedForWashFromEmployee
            //                                && lv.LaundryVendorID == uniformCategoryExceptListData.LaundryVendorID
            //                                && !uniformCategoryExceptListData.Excepts.Contains(d.UniformCategoryID)
            //                                && c.EmployeeUniformStatusID < (int)EmployeeUniformStatusValues.Disposed
            //                                && b.LinenRoomID == uniformCategoryExceptListData.WarehouseID
            //                    select new CollectedForWashData
            //                    {
            //                        UniformTransactionID = b.UniformTransactionID,
            //                        EmployeeUniformID = b.EmployeeUniformID,
            //                        WashTypeID = b.WashTypeID,
            //                        WashType = b.WashType.WashTypeName,
            //                        EmployeeID = c.EmployeeID,
            //                        UniformItemID = c.UniformItemID,
            //                        UniformBarcode = c.UniformBarcode,
            //                        ItemCategoryID = c.UniformItem.UniformCategoryID,
            //                        ItemCategory = c.UniformItem.UniformCategory.UniformCategoryName,
            //                        ItemCode = c.UniformItem.UniformItemCode,
            //                        ItemName = c.UniformItem.UniformItemName,
            //                        Color = c.UniformItem.Color.ColorName,
            //                        Material = c.UniformItem.Material.MaterialName,
            //                        Logo = c.UniformItem.Logo.LogoName,
            //                        Size = c.Size.SizeName,

            //                        Price = lv.Price
            //                    }).ToList();

            //        var result = new GetAllUniformCollectedForWashResponse()
            //        {
            //            CollectedForWashData = Data
            //        };
            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllUniformCollectedForWashResponse>(ex);
            //    }
            //}

            //[HttpPost]
            //public ActionResult CreateWorkOrder([FromBody] WorkOrderData Data)
            //{
            //    try
            //    {
            //        _db.CurrentUserID = Data.UserID;

            //        var result = new BaseResponse();
            //        base.TraceLog("MobileAPI", $" {Data.uniformTransactionIDs}, {Data.vendorID}, {Data.UserID}");

            //        string woNo = "";

            //        if (Data.uniformTransactionIDs.Count() > 0)
            //        {
            //            WorkOrderTable workorder = new WorkOrderTable();
            //            workorder.LaundryVendorID = Data.vendorID;
            //            workorder.WorkOrderNo = CodeGenerationHelper.GetNextCode("WorkOrder"); //DateTime.Now.ToString("ddMMyyyyHHmmss") + Data.vendorID.ToString();
            //            workorder.CreatedBy = Data.UserID;
            //            workorder.LinenRoomID = Data.WarehouseID;

            //            _db.Add(workorder);

            //            var vendorPrices = (from b in LaundryVendorPricingTable.GetAllItems(_db)
            //                                where b.LaundryVendorID == Data.vendorID
            //                                select b);

            //            foreach (var id in Data.uniformTransactionIDs)
            //            {
            //                var uniformtransaction = UniformTransactionTable.GetItem(_db, id);
            //                var uniformItemCategory = (from b in EmployeeUniformTable.GetAllItems(_db)
            //                                           where b.EmployeeUniformID == uniformtransaction.EmployeeUniformID
            //                                           select b.UniformItem.UniformCategoryID).FirstOrDefault();

            //                if (uniformtransaction != null)
            //                {
            //                    WorkOrderLineItemTable LI = new WorkOrderLineItemTable();
            //                    LI.WorkOrder = workorder;
            //                    LI.EmployeeUniformID = uniformtransaction.EmployeeUniformID;
            //                    LI.WashTypeID = uniformtransaction.WashTypeID;
            //                    LI.IsForRewash = uniformtransaction.IsForRewash;
            //                    LI.DateOfIssueToVendor = DateTime.Now;
            //                    LI.CreatedBy = Data.UserID;
            //                    LI.Rate = vendorPrices.Where(b => b.WashTypeID == uniformtransaction.WashTypeID && b.UniformCategoryID == uniformItemCategory).FirstOrDefault().Price;

            //                    _db.Add(LI);

            //                    uniformtransaction.ProcessStatusID = (int)ProcessStatusValue.WorkOrderCreated;
            //                    uniformtransaction.WorkOrderLineItem = LI;
            //                }
            //            }

            //            _db.SaveChanges();

            //            woNo = workorder.WorkOrderNo;
            //        }

            //        result.Message = $"Workorder '{woNo}' created successfully.";

            //        return Json(result);

            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<BaseResponse>(ex);
            //    }
            //}

            //public ActionResult WorkOrder_GetAllPendingWorkOrders(string deviceSerialNo, int userID, int warehouseID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");

            //        var data = (from b in UniformTransactionTable.GetAllItems(_db)
            //                    join c in WorkOrderLineItemTable.GetAllItems(_db) on b.WorkOrderLineItemID equals c.WorkOrderLineItemID
            //                    where b.LinenRoomID == warehouseID
            //                        && b.TransactionTypeID == (int)TransactionTypeValue.CollectedForWash
            //                        && b.ProcessStatusID == (int)ProcessStatusValue.WorkOrderCreated
            //                    select new WorkOrderDataForVendor()
            //                    {
            //                        WorkOrderID = c.WorkOrder.WorkOrderID,
            //                        WorkOrderNo = c.WorkOrder.WorkOrderNo,
            //                        VendorName = c.WorkOrder.LaundryVendor.LaundryVendorName,
            //                        WorkOrderDate = c.WorkOrder.CreatedDateTime
            //                    }).Distinct();

            //        var result = new GetAllWorkOrderDetailsForVendorResponse()
            //        {
            //            WorkOrder = data.ToList()
            //        };
            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllWorkOrderDetailsForVendorResponse>(ex);
            //    }
            //}

            //public ActionResult WorkOrder_GetDetails(string deviceSerialNo, int userID, int warehouseID, int workOrderID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}, {workOrderID}");

            //        var uniformItems = (from b in WorkOrderLineItemTable.GetAllItems(_db)
            //                            join c in UniformTransactionTable.GetAllItems(_db) on b.WorkOrderLineItemID equals c.WorkOrderLineItemID
            //                            where b.WorkOrderID == workOrderID
            //                            select new WorkOrderDetailData
            //                            {
            //                                WorkOrderID = b.WorkOrderID,
            //                                WorkOrderNo = b.WorkOrder.WorkOrderNo,
            //                                WorkOrderLineItemID = b.WorkOrderLineItemID,
            //                                VendorID = b.WorkOrder.LaundryVendorID,
            //                                EmployeeUniformID = b.EmployeeUniformID,
            //                                WashTypeID = b.WashTypeID,
            //                                WashType = b.WashType.WashTypeName,
            //                                EmployeeID = b.EmployeeUniform.EmployeeID,
            //                                UniformItemID = b.EmployeeUniform.UniformItemID,
            //                                UniformBarcode = b.EmployeeUniform.UniformBarcode,
            //                                ItemCategoryID = b.EmployeeUniform.UniformItem.UniformCategoryID,
            //                                ItemCategory = b.EmployeeUniform.UniformItem.UniformCategory.UniformCategoryName,
            //                                ItemCode = b.EmployeeUniform.UniformItem.UniformItemCode,
            //                                ItemName = b.EmployeeUniform.UniformItem.UniformItemName,
            //                                Color = b.EmployeeUniform.UniformItem.Color.ColorName,
            //                                Material = b.EmployeeUniform.UniformItem.Material.MaterialName,
            //                                Logo = b.EmployeeUniform.UniformItem.Logo.LogoName,
            //                                Size = b.EmployeeUniform.Size.SizeName,
            //                                Status = c.ProcessStatus.ProcessStatusName,
            //                                Price = b.Rate,
            //                            }).ToList();

            //        var result = new GetAllWorkOrderDetailsResponse()
            //        {
            //            WorkOrderData = uniformItems
            //        };

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllWorkOrderDetailsResponse>(ex);
            //    }
            //}

            //public ActionResult WorkOrder_Cancel(string deviceSerialNo, int userID, int warehouseID, int workOrderID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}, {workOrderID}");

            //        //No item should be collected
            //        var uniformItems = (from b in WorkOrderLineItemTable.GetAllItems(_db)
            //                            join c in UniformTransactionTable.GetAllItems(_db) on b.WorkOrderLineItemID equals c.WorkOrderLineItemID
            //                            where b.WorkOrderID == workOrderID
            //                                    && c.ProcessStatusID != (int)ProcessStatusValue.WorkOrderCreated
            //                                    && b.WorkOrder.StatusID != (int)StatusValue.Deleted
            //                            select b);
            //        if (uniformItems.Any())
            //        {
            //            return ErrorActionResult<BaseResponse>("Some of the uniforms are issued, cancellation is not possible");
            //        }

            //        var wo = WorkOrderTable.GetItem(_db, workOrderID);
            //        wo.Delete();

            //        _db.SaveChanges();

            //        return Json(new BaseResponse());
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<BaseResponse>(ex);
            //    }
            //}

            //#endregion

            //#region Return Uniform To employee

            //public ActionResult GetReturnUniformToEmployee(string deviceSerialNo, int userID, int warehouseID, string uniformBarcode)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}, {uniformBarcode}");

            //        var data = (from b in EmployeeUniformTable.GetAllItems(_db)
            //                    where b.UniformBarcode == uniformBarcode
            //                        && b.EmployeeUniformStatusID < (int)EmployeeUniformStatusValues.Disposed
            //                    select b).FirstOrDefault();
            //        if (data == null)
            //        {
            //            throw new ValidationException("Invalid uniform barcode");
            //        }

            //        var scannedUniformBarcode = (from b in UniformTransactionTable.GetAllItems(_db)
            //                                     join c in EmployeeUniformTable.GetAllItems(_db) on b.EmployeeUniformID equals c.EmployeeUniformID
            //                                     join d in UniformItemTable.GetAllItems(_db) on c.UniformItemID equals d.UniformItemID
            //                                     where (b.ProcessStatusID == (int)ProcessStatusValue.ProcessedUniformReturnByVendor ||
            //                                            b.ProcessStatusID == (int)ProcessStatusValue.UnprocessedUniformReturnByVendor ||
            //                                            b.ProcessStatusID == (int)ProcessStatusValue.CollectedForWashFromEmployee)
            //                                          && b.TransactionTypeID == (int)TransactionTypeValue.CleanedUniform
            //                                          && c.UniformBarcode == uniformBarcode
            //                                     select new EmployeeUniformData
            //                                     {
            //                                         EmployeeUniformID = b.EmployeeUniformID,
            //                                         WashTypeID = b.WashTypeID,
            //                                         WashType = b.WashType.WashTypeName,
            //                                         EmployeeID = c.EmployeeID,
            //                                         PersonCode = c.Employee.PersonCode,
            //                                         PersonFirstName = c.Employee.PersonFirstName,
            //                                         PersonLastName = c.Employee.PersonLastName,
            //                                         UniformItemID = c.UniformItemID,
            //                                         UniformBarcode = c.UniformBarcode,
            //                                         ItemCategory = c.UniformItem.UniformCategory.UniformCategoryName,
            //                                         ItemCode = c.UniformItem.UniformItemCode,
            //                                         ItemName = c.UniformItem.UniformItemName,
            //                                         Color = c.UniformItem.Color.ColorName,
            //                                         Material = c.UniformItem.Material.MaterialName,
            //                                         Logo = c.UniformItem.Logo.LogoName,
            //                                         Size = c.Size.SizeName,
            //                                         ManualSizeValue = c.ManualSizeValue,

            //                                         IssuedDate = b.CreatedDateTime,
            //                                     }).FirstOrDefault();

            //        if (scannedUniformBarcode == null)
            //        {
            //            var currentStatus = (from b in UniformTransactionTable.GetAllItems(_db)
            //                                 join c in EmployeeUniformTable.GetAllItems(_db) on b.EmployeeUniformID equals c.EmployeeUniformID
            //                                 where b.ProcessStatusID != (int)ProcessStatusValue.ReturnToEmployee
            //                                      && c.UniformBarcode == uniformBarcode
            //                                 select b.ProcessStatus.ProcessStatusName).FirstOrDefault();

            //            throw new ValidationException($"Uniform status is incorrect - it's in '{currentStatus}'.");
            //        }

            //        var PendingList = (from b in UniformTransactionTable.GetAllItems(_db)
            //                           join c in EmployeeUniformTable.GetAllItems(_db) on b.EmployeeUniformID equals c.EmployeeUniformID
            //                           join d in UniformItemTable.GetAllItems(_db) on c.UniformItemID equals d.UniformItemID
            //                           where b.ProcessStatusID != (int)ProcessStatusValue.ReturnToEmployee
            //                                && c.EmployeeID == data.EmployeeID
            //                                && b.IsReturnedToEmployee == false
            //                                && c.UniformBarcode != uniformBarcode
            //                           select new EmployeeUniformData
            //                           {
            //                               EmployeeUniformID = b.EmployeeUniformID,
            //                               WashTypeID = b.WashTypeID,
            //                               WashType = b.WashType.WashTypeName,
            //                               EmployeeID = c.EmployeeID,
            //                               PersonCode = c.Employee.PersonCode,
            //                               PersonFirstName = c.Employee.PersonFirstName,
            //                               PersonLastName = c.Employee.PersonLastName,
            //                               UniformItemID = c.UniformItemID,
            //                               UniformBarcode = c.UniformBarcode,
            //                               ItemCategory = c.UniformItem.UniformCategory.UniformCategoryName,
            //                               ItemCode = c.UniformItem.UniformItemCode,
            //                               ItemName = c.UniformItem.UniformItemName,
            //                               Color = c.UniformItem.Color.ColorName,
            //                               Material = c.UniformItem.Material.MaterialName,
            //                               Logo = c.UniformItem.Logo.LogoName,
            //                               Size = c.Size.SizeName,
            //                               ManualSizeValue = c.ManualSizeValue,

            //                               IssuedDate = b.CreatedDateTime,
            //                               UniformStatusID=b.ProcessStatusID.Value,
            //                               UniformStatus = b.ProcessStatus.ProcessStatusName,

            //                               UnprocessedReason = b.UnprocessedUniformReturnReason.UnprocessedUniformReturnReasonName,
            //                               UnprocessedRemarks = b.UnprocessedUniformReturnRemarks
            //                           }).ToList();

            //        var result = new GetAllUniformReturnToEmployeeResponse()
            //        {
            //            ScannedUniformBarodeData = scannedUniformBarcode,
            //            PendingItemtoReturn = PendingList
            //        };

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllUniformReturnToEmployeeResponse>(ex);
            //    }
            //}

            //[HttpPost]
            //public ActionResult CreateUniformReturnToEmployee([FromBody] BarcodeListRequest request)
            //{
            //    try
            //    {
            //        var result = new BaseResponse();
            //        base.TraceLog("MobileAPI", $"{request.DeviceSerialNo}, {request.UserID}, {request.WarehouseID}, {request.UniformBarcodes.Count}");
            //        _db.CurrentUserID = request.UserID;

            //        if (request.UniformBarcodes.Count() > 0)
            //        {
            //            foreach (var id in request.UniformBarcodes)
            //            {
            //                var uniformtransaction = (from b in UniformTransactionTable.GetAllItems(_db)
            //                                          join c in EmployeeUniformTable.GetAllItems(_db) on b.EmployeeUniformID equals c.EmployeeUniformID
            //                                          join d in UniformItemTable.GetAllItems(_db) on c.UniformItemID equals d.UniformItemID
            //                                          where (b.ProcessStatusID == (int)ProcessStatusValue.ProcessedUniformReturnByVendor
            //                                                    || b.ProcessStatusID == (int)ProcessStatusValue.UnprocessedUniformReturnByVendor)
            //                                               && b.TransactionTypeID == (int)TransactionTypeValue.CleanedUniform
            //                                               && c.EmployeeUniformStatusID < (int)EmployeeUniformStatusValues.Disposed
            //                                               && c.UniformBarcode == id
            //                                          select b).FirstOrDefault();

            //                if (uniformtransaction != null)
            //                {
            //                    uniformtransaction.ProcessStatusID = (int)ProcessStatusValue.ReturnToEmployee;
            //                    uniformtransaction.ReturnedToEmployeeDate = DateTime.Now;
            //                    uniformtransaction.ReturnedToEmployeeByUserID = request.UserID;
            //                }

            //                var uniform = (from b in EmployeeUniformTable.GetAllItems(_db)
            //                               where b.UniformBarcode == id
            //                               select b).FirstOrDefault();
            //                if (uniform != null)
            //                {
            //                    uniform.EmployeeUniformStatusID = (int)EmployeeUniformStatusValues.WithEmployee;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            throw new ValidationException("No uniform found");
            //        }

            //        _db.SaveChanges();
            //        result.Message = "Saved Successfully";
            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {

            //        return ErrorActionResult<GetEmployeeUniformResponse>(ex);
            //    }
            //}

            //#endregion

            //#region Collect uniform by Vendor

            //[HttpGet]
            //public ActionResult GetAllPendingWorkOrders(string deviceSerialNo, int userID, int warehouseID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");

            //        var data = (from b in UniformTransactionTable.GetAllItems(_db)
            //                    join c in WorkOrderLineItemTable.GetAllItems(_db) on b.WorkOrderLineItemID equals c.WorkOrderLineItemID
            //                    where b.TransactionTypeID == (int)TransactionTypeValue.CollectedForWash
            //                        && b.ProcessStatusID == (int)ProcessStatusValue.WorkOrderCreated
            //                        && b.LinenRoomID == warehouseID
            //                    select new WorkOrderDataForVendor()
            //                    {
            //                        WorkOrderID = c.WorkOrder.WorkOrderID,
            //                        WorkOrderNo = c.WorkOrder.WorkOrderNo,
            //                        VendorName = c.WorkOrder.LaundryVendor.LaundryVendorName,
            //                        WorkOrderDate = c.WorkOrder.CreatedDateTime
            //                    }).Distinct();

            //        var result = new GetAllWorkOrderDetailsForVendorResponse()
            //        {
            //            WorkOrder = data.ToList()
            //        };
            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllWorkOrderDetailsForVendorResponse>(ex);
            //    }
            //}

            //[HttpGet]
            //public ActionResult WorkOrder_GetAllVendorPendingWorkOrders(string deviceSerialNo, int userID, int warehouseID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");
            //        var vendorID = PersonTable.GetPersonLaundryVendorID(_db, userID);
            //        _db.CurrentUserID = userID;

            //        if (vendorID.HasValue == false)
            //        {
            //            throw new ValidationException("No vendor mapped with you");
            //        }

            //        var data = (from b in UniformTransactionTable.GetAllItems(_db)
            //                    join c in WorkOrderLineItemTable.GetAllItems(_db) on b.WorkOrderLineItemID equals c.WorkOrderLineItemID
            //                    where b.TransactionTypeID == (int)TransactionTypeValue.CollectedForWash
            //                        && b.ProcessStatusID == (int)ProcessStatusValue.WorkOrderCreated
            //                        && c.WorkOrder.LaundryVendorID == vendorID.Value
            //                        && c.WorkOrder.LinenRoomID == warehouseID
            //                    select new WorkOrderDataForVendor()
            //                    {
            //                        WorkOrderID = c.WorkOrder.WorkOrderID,
            //                        WorkOrderNo = c.WorkOrder.WorkOrderNo,
            //                        VendorName = c.WorkOrder.LaundryVendor.LaundryVendorName,
            //                        WorkOrderDate = c.WorkOrder.CreatedDateTime
            //                    }).Distinct();

            //        var result = new GetAllWorkOrderDetailsForVendorResponse()
            //        {
            //            WorkOrder = data.ToList()
            //        };
            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllWorkOrderDetailsForVendorResponse>(ex);
            //    }
            //}

            //public ActionResult WorkOrder_GetWorkOrderUniformDetails(string deviceSerialNo, int userID, int warehouseID, int workOrderID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}, {workOrderID}");

            //        var WorkOrder = (from b in WorkOrderLineItemTable.GetAllItems(_db)
            //                         join c in UniformTransactionTable.GetAllItems(_db) on b.WorkOrderLineItemID equals c.WorkOrderLineItemID
            //                         where b.WorkOrderID == workOrderID
            //                             && c.TransactionTypeID == (int)TransactionTypeValue.CollectedForWash
            //                             && (c.ProcessStatusID == (int)ProcessStatusValue.WorkOrderCreated || c.ProcessStatusID == (int)ProcessStatusValue.CollectedByVendor)
            //                         select new WorkOrderDetailData
            //                         {
            //                             WorkOrderID = b.WorkOrderID,
            //                             WorkOrderNo = b.WorkOrder.WorkOrderNo,
            //                             WorkOrderLineItemID = b.WorkOrderLineItemID,
            //                             VendorID = b.WorkOrder.LaundryVendorID,
            //                             EmployeeUniformID = b.EmployeeUniformID,
            //                             WashTypeID = b.WashTypeID,
            //                             WashType = b.WashType.WashTypeName,
            //                             EmployeeID = b.EmployeeUniform.EmployeeID,
            //                             UniformItemID = b.EmployeeUniform.UniformItemID,
            //                             UniformBarcode = b.EmployeeUniform.UniformBarcode,
            //                             ItemCategoryID = b.EmployeeUniform.UniformItem.UniformCategoryID,
            //                             ItemCategory = b.EmployeeUniform.UniformItem.UniformCategory.UniformCategoryName,
            //                             ItemCode = b.EmployeeUniform.UniformItem.UniformItemCode,
            //                             ItemName = b.EmployeeUniform.UniformItem.UniformItemName,
            //                             Color = b.EmployeeUniform.UniformItem.Color.ColorName,
            //                             Material = b.EmployeeUniform.UniformItem.Material.MaterialName,
            //                             Logo = b.EmployeeUniform.UniformItem.Logo.LogoName,
            //                             Size = b.EmployeeUniform.Size.SizeName,
            //                             Status = (c.ProcessStatusID == (int)ProcessStatusValue.CollectedByVendor) ? "Collected" : ""
            //                         }).ToList();

            //        var result = new GetAllWorkOrderDetailsResponse()
            //        {
            //            WorkOrderData = WorkOrder
            //        };

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        ApplicationErrorLogTable.SaveException(ex);
            //        return ErrorActionResult<GetAllUniformReturnToEmployeeResponse>(ex);
            //    }
            //}

            //public ActionResult VendorCollect_GetWorkOrderUniformDetails(string deviceSerialNo, int userID, int warehouseID, int workOrderID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}, {workOrderID}");

            //        var WorkOrder = (from b in WorkOrderLineItemTable.GetAllItems(_db)
            //                         join c in UniformTransactionTable.GetAllItems(_db) on b.WorkOrderLineItemID equals c.WorkOrderLineItemID
            //                         where b.WorkOrderID == workOrderID
            //                             && c.TransactionTypeID == (int)TransactionTypeValue.CollectedForWash
            //                             && (c.ProcessStatusID == (int)ProcessStatusValue.WorkOrderCreated || c.ProcessStatusID == (int)ProcessStatusValue.CollectedByVendor)
            //                         select new WorkOrderDetailData
            //                         {
            //                             WorkOrderID = b.WorkOrderID,
            //                             WorkOrderNo = b.WorkOrder.WorkOrderNo,
            //                             WorkOrderLineItemID = b.WorkOrderLineItemID,
            //                             VendorID = b.WorkOrder.LaundryVendorID,
            //                             EmployeeUniformID = b.EmployeeUniformID,
            //                             WashTypeID = b.WashTypeID,
            //                             WashType = b.WashType.WashTypeName,
            //                             EmployeeID = b.EmployeeUniform.EmployeeID,
            //                             UniformItemID = b.EmployeeUniform.UniformItemID,
            //                             UniformBarcode = b.EmployeeUniform.UniformBarcode,
            //                             ItemCategoryID = b.EmployeeUniform.UniformItem.UniformCategoryID,
            //                             ItemCategory = b.EmployeeUniform.UniformItem.UniformCategory.UniformCategoryName,
            //                             ItemCode = b.EmployeeUniform.UniformItem.UniformItemCode,
            //                             ItemName = b.EmployeeUniform.UniformItem.UniformItemName,
            //                             Color = b.EmployeeUniform.UniformItem.Color.ColorName,
            //                             Material = b.EmployeeUniform.UniformItem.Material.MaterialName,
            //                             Logo = b.EmployeeUniform.UniformItem.Logo.LogoName,
            //                             Size = b.EmployeeUniform.Size.SizeName,
            //                             Status = (c.ProcessStatusID == (int)ProcessStatusValue.CollectedByVendor) ? "Collected" : "",
            //                             Price = b.Rate,
            //                         }).ToList();

            //        var result = new GetAllWorkOrderDetailsResponse()
            //        {
            //            WorkOrderData = WorkOrder
            //        };

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        ApplicationErrorLogTable.SaveException(ex);
            //        return ErrorActionResult<GetAllUniformReturnToEmployeeResponse>(ex);
            //    }
            //}

            ////[HttpPost]
            //public ActionResult VendorCollect_UniformCollected(string deviceSerialNo, int userID, int warehouseID, int workorderID, string uniformBarcode)
            //{
            //    try
            //    {
            //        var result = new BaseResponse();
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}, {uniformBarcode}");
            //        _db.CurrentUserID = userID;

            //        //if (request.WorkOrderLineItemIDs.Count() > 0)
            //        {
            //            //foreach (var id in request.WorkOrderLineItemIDs)
            //            {
            //                var uniformtransaction = (from b in UniformTransactionTable.GetAllItems(_db)
            //                                          where b.WorkOrderLineItem.WorkOrderID == workorderID
            //                                                && b.EmployeeUniform.UniformBarcode == uniformBarcode
            //                                          select b).FirstOrDefault();
            //                if (uniformtransaction != null)
            //                {
            //                    uniformtransaction.ProcessStatusID = (int)ProcessStatusValue.CollectedByVendor;
            //                }

            //                var uniform = (from b in EmployeeUniformTable.GetAllItems(_db)
            //                               where b.UniformBarcode == uniformBarcode
            //                               select b).FirstOrDefault();
            //                if (uniform != null)
            //                {
            //                    uniform.EmployeeUniformStatusID = (int)EmployeeUniformStatusValues.WithLaundryVendor;
            //                }

            //                _db.SaveChanges();
            //            }
            //        }

            //        result.Message = "Saved Successfully";
            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<BaseResponse>(ex);
            //    }
            //}

            //[HttpGet]
            //public ActionResult LinenRoom_GetAllVendorReceivePendingWorkOrders(string deviceSerialNo, int userID, int warehouseID, int vendorID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}, vendorID: {vendorID}");
            //        _db.CurrentUserID = userID;

            //        var data = (from b in UniformTransactionTable.GetAllItems(_db)
            //                    join c in WorkOrderLineItemTable.GetAllItems(_db) on b.WorkOrderLineItemID equals c.WorkOrderLineItemID
            //                    where c.WorkOrder.LaundryVendorID == vendorID
            //                        && c.WorkOrder.IsClosed == false
            //                    orderby c.WorkOrder.WorkOrderNo
            //                    select new WorkOrderDataForVendor()
            //                    {
            //                        WorkOrderID = c.WorkOrder.WorkOrderID,
            //                        WorkOrderNo = c.WorkOrder.WorkOrderNo,
            //                        VendorName = c.WorkOrder.LaundryVendor.LaundryVendorName,
            //                        WorkOrderDate = c.WorkOrder.CreatedDateTime
            //                    }).Distinct();

            //        var result = new GetAllWorkOrderDetailsForVendorResponse()
            //        {
            //            WorkOrder = data.ToList()
            //        };
            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllWorkOrderDetailsForVendorResponse>(ex);
            //    }
            //}

            //public ActionResult LinenRoom_GetWorkOrderUniformStatus(string deviceSerialNo, int userID, int warehouseID, int workOrderID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}, {workOrderID}");

            //        var receivedItems = (from b in WorkOrderLineItemTable.GetAllItems(_db)
            //                             join c in UniformTransactionTable.GetAllItems(_db) on b.WorkOrderLineItemID equals c.WorkOrderLineItemID
            //                             where b.WorkOrderID == workOrderID
            //                             select new WorkOrderDetailData
            //                             {
            //                                 WorkOrderID = b.WorkOrderID,
            //                                 WorkOrderNo = b.WorkOrder.WorkOrderNo,
            //                                 WorkOrderLineItemID = b.WorkOrderLineItemID,
            //                                 VendorID = b.WorkOrder.LaundryVendorID,
            //                                 EmployeeUniformID = b.EmployeeUniformID,
            //                                 WashTypeID = b.WashTypeID,
            //                                 WashType = b.WashType.WashTypeName,
            //                                 EmployeeID = b.EmployeeUniform.EmployeeID,
            //                                 UniformItemID = b.EmployeeUniform.UniformItemID,
            //                                 UniformBarcode = b.EmployeeUniform.UniformBarcode,
            //                                 ItemCategoryID = b.EmployeeUniform.UniformItem.UniformCategoryID,
            //                                 ItemCategory = b.EmployeeUniform.UniformItem.UniformCategory.UniformCategoryName,
            //                                 ItemCode = b.EmployeeUniform.UniformItem.UniformItemCode,
            //                                 ItemName = b.EmployeeUniform.UniformItem.UniformItemName,
            //                                 Color = b.EmployeeUniform.UniformItem.Color.ColorName,
            //                                 Material = b.EmployeeUniform.UniformItem.Material.MaterialName,
            //                                 Logo = b.EmployeeUniform.UniformItem.Logo.LogoName,
            //                                 Size = b.EmployeeUniform.Size.SizeName,
            //                                 Status = (c.ProcessStatusID == (int)ProcessStatusValue.CollectedByVendor) ? "Missing" : "Received",
            //                                 Price = b.Rate,

            //                                 UniformStatus = c.ProcessStatus.ProcessStatusName,
            //                                 UnprocessedReason = c.UnprocessedUniformReturnReason.UnprocessedUniformReturnReasonName,
            //                                 UnprocessedRemarks = c.UnprocessedUniformReturnRemarks
            //                             }).ToList();

            //        var result = new GetAllWorkOrderDetailsResponse()
            //        {
            //            WorkOrderData = receivedItems
            //        };

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        ApplicationErrorLogTable.SaveException(ex);
            //        return ErrorActionResult<GetAllUniformReturnToEmployeeResponse>(ex);
            //    }
            //}

            //public ActionResult LinenRoom_CloseWorkOrder(string deviceSerialNo, int userID, int warehouseID, int workOrderID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}, {workOrderID}");

            //        var pendingItems = (from b in WorkOrderLineItemTable.GetAllItems(_db)
            //                            join c in UniformTransactionTable.GetAllItems(_db) on b.WorkOrderLineItemID equals c.WorkOrderLineItemID
            //                            where b.WorkOrderID == workOrderID
            //                               && c.ProcessStatusID < (int)ProcessStatusValue.ProcessedUniformReturnByVendor
            //                            select b);

            //        if (pendingItems.Any())
            //        {
            //            return ErrorActionResult<BaseResponse>("Pending items available with vendor, workorder not allowed to close.");
            //        }

            //        var wo = WorkOrderTable.GetItem(_db, workOrderID);
            //        wo.IsClosed = true;

            //        _db.SaveChanges();

            //        return Json(new BaseResponse()
            //        {
            //            Message = "Workorder closed succssfully"
            //        });
            //    }
            //    catch (Exception ex)
            //    {
            //        //ApplicationErrorLogTable.SaveException(ex);
            //        return ErrorActionResult<BaseResponse>(ex);
            //    }
            //}

            //#endregion

            //#region Processed uniform return to Linen room

            //public ActionResult GetAllCollectedByVendorWorkOrderDetails(string deviceSerialNo, int userID, int warehouseID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");
            //        var vendorID = PersonTable.GetPersonLaundryVendorID(_db, userID);

            //        var WorkOrder = (from b in WorkOrderLineItemTable.GetAllItems(_db)
            //                         join c in UniformTransactionTable.GetAllItems(_db) on b.WorkOrderLineItemID equals c.WorkOrderLineItemID
            //                         where b.WorkOrder.LaundryVendorID == vendorID
            //                         && c.TransactionTypeID == (int)TransactionTypeValue.CollectedForWash
            //                         && c.ProcessStatusID == (int)ProcessStatusValue.CollectedByVendor
            //                         select new WorkOrderDetailData
            //                         {
            //                             WorkOrderID = b.WorkOrderID,
            //                             WorkOrderNo = b.WorkOrder.WorkOrderNo,
            //                             WorkOrderLineItemID = b.WorkOrderLineItemID,
            //                             VendorID = b.WorkOrder.LaundryVendorID,
            //                             EmployeeUniformID = b.EmployeeUniformID,
            //                             WashTypeID = b.WashTypeID,
            //                             WashType = b.WashType.WashTypeName,
            //                             EmployeeID = b.EmployeeUniform.EmployeeID,
            //                             UniformItemID = b.EmployeeUniform.UniformItemID,
            //                             UniformBarcode = b.EmployeeUniform.UniformBarcode,
            //                             ItemCategoryID = b.EmployeeUniform.UniformItem.UniformCategoryID,
            //                             ItemCategory = b.EmployeeUniform.UniformItem.UniformCategory.UniformCategoryName,
            //                             ItemCode = b.EmployeeUniform.UniformItem.UniformItemCode,
            //                             ItemName = b.EmployeeUniform.UniformItem.UniformItemName,
            //                             Color = b.EmployeeUniform.UniformItem.Color.ColorName,
            //                             Material = b.EmployeeUniform.UniformItem.Material.MaterialName,
            //                             Logo = b.EmployeeUniform.UniformItem.Logo.LogoName,
            //                             Size = b.EmployeeUniform.Size.SizeName

            //                         }).ToList();


            //        var result = new GetAllWorkOrderDetailsResponse()
            //        {
            //            WorkOrderData = WorkOrder
            //        };
            //        return Json(result);

            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllUniformReturnToEmployeeResponse>(ex);
            //    }
            //}

            ////[HttpPost]
            //public ActionResult Vendor_ProcessedUniformReturn(string deviceSerialNo, int userID, int warehouseID, string uniformBarcode, int? employeeID)//List<int> workOrderLineItemIds)
            //{
            //    try
            //    {
            //        var result = new GetAllWorkOrderDetailsResponse();
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");
            //        var vendorID = PersonTable.GetPersonLaundryVendorID(_db, userID);

            //        _db.CurrentUserID = userID;

            //        //if (workOrderLineItemIds.Count() > 0)
            //        {
            //            //foreach (var id in workOrderLineItemIds)
            //            {
            //                //validate barcode
            //                var uniformItem = (from b in EmployeeUniformTable.GetAllItems(_db)
            //                                   where b.UniformBarcode == uniformBarcode
            //                                   select b).FirstOrDefault();
            //                if (uniformItem == null)
            //                {
            //                    throw new ValidationException($"Invalid barcode - {uniformBarcode}");
            //                }

            //                if ((employeeID.HasValue) && (employeeID.Value > 0))
            //                {
            //                    if(employeeID != uniformItem.EmployeeID)
            //                    {
            //                        throw new ValidationException($"Uniform belongs to another employee"); 
            //                    }
            //                }

            //                var uniformtransaction = (from b in WorkOrderLineItemTable.GetAllItems(_db)
            //                                          join c in UniformTransactionTable.GetAllItems(_db) on b.WorkOrderLineItemID equals c.WorkOrderLineItemID
            //                                          where b.WorkOrder.LaundryVendorID == vendorID
            //                                              && c.TransactionTypeID == (int)TransactionTypeValue.CollectedForWash
            //                                              && c.ProcessStatusID == (int)ProcessStatusValue.CollectedByVendor
            //                                              && c.EmployeeUniform.UniformBarcode == uniformBarcode
            //                                              && c.LinenRoomID == warehouseID
            //                                          select c).FirstOrDefault();

            //                if (uniformtransaction != null)
            //                {
            //                    uniformtransaction.ProcessStatusID = (int)ProcessStatusValue.ProcessedUniformReturnByVendor;
            //                    uniformtransaction.TransactionTypeID = (int)TransactionTypeValue.CleanedUniform;
            //                    uniformtransaction.ReturnedByVendorUserID = userID;
            //                    uniformtransaction.ReturnedFromVendorDate = DateTime.Now;
            //                }
            //                else
            //                {
            //                    throw new ValidationException($"Invalid barcode or not issued to wash - {uniformBarcode}");
            //                }

            //                var uniform = (from b in EmployeeUniformTable.GetAllItems(_db)
            //                               where b.UniformBarcode == uniformBarcode
            //                               select b).FirstOrDefault();
            //                if (uniform != null)
            //                {
            //                    uniform.EmployeeUniformStatusID = (int)EmployeeUniformStatusValues.InLinenRoomForReturnToEmployee;
            //                    uniform.WashCount++;
            //                }

            //                _db.SaveChanges();

            //                var WorkOrder = (from b in WorkOrderLineItemTable.GetAllItems(_db)
            //                                 join c in UniformTransactionTable.GetAllItems(_db) on b.WorkOrderLineItemID equals c.WorkOrderLineItemID
            //                                 where b.WorkOrder.LaundryVendorID == vendorID
            //                                     && c.UniformTransactionID == uniformtransaction.UniformTransactionID
            //                                     && c.EmployeeUniform.UniformBarcode == uniformBarcode
            //                                 select new WorkOrderDetailData
            //                                 {
            //                                     WorkOrderID = b.WorkOrderID,
            //                                     WorkOrderNo = b.WorkOrder.WorkOrderNo,
            //                                     WorkOrderLineItemID = b.WorkOrderLineItemID,
            //                                     VendorID = b.WorkOrder.LaundryVendorID,
            //                                     EmployeeUniformID = b.EmployeeUniformID,
            //                                     WashTypeID = b.WashTypeID,
            //                                     WashType = b.WashType.WashTypeName,
            //                                     EmployeeID = b.EmployeeUniform.EmployeeID,
            //                                     UniformItemID = b.EmployeeUniform.UniformItemID,
            //                                     UniformBarcode = b.EmployeeUniform.UniformBarcode,
            //                                     ItemCategoryID = b.EmployeeUniform.UniformItem.UniformCategoryID,
            //                                     ItemCategory = b.EmployeeUniform.UniformItem.UniformCategory.UniformCategoryName,
            //                                     ItemCode = b.EmployeeUniform.UniformItem.UniformItemCode,
            //                                     ItemName = b.EmployeeUniform.UniformItem.UniformItemName,
            //                                     Color = b.EmployeeUniform.UniformItem.Color.ColorName,
            //                                     Material = b.EmployeeUniform.UniformItem.Material.MaterialName,
            //                                     Logo = b.EmployeeUniform.UniformItem.Logo.LogoName,
            //                                     Size = b.EmployeeUniform.Size.SizeName
            //                                 }).ToList();

            //                result.WorkOrderData = WorkOrder;
            //            }
            //        }

            //        result.Message = "Saved Successfully";
            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllWorkOrderDetailsResponse>(ex);
            //    }
            //}

            //public ActionResult Vendor_MissingUniforms(string deviceSerialNo, int userID, int warehouseID, int? employeeID)
            //{
            //    try
            //    {
            //        var result = new GetAllWorkOrderDetailsResponse();
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");
            //        var vendorID = PersonTable.GetPersonLaundryVendorID(_db, userID);

            //        _db.CurrentUserID = userID;

            //        var query = (from b in WorkOrderLineItemTable.GetAllItems(_db)
            //                     join c in UniformTransactionTable.GetAllItems(_db) on b.WorkOrderLineItemID equals c.WorkOrderLineItemID
            //                     where b.WorkOrder.LaundryVendorID == vendorID
            //                         && b.WorkOrder.StatusID != (int)StatusValue.Deleted
            //                         && c.ProcessStatusID == (int)ProcessStatusValue.CollectedByVendor
            //                         && c.LinenRoomID == warehouseID
            //                     select b);

            //        if ((employeeID.HasValue) && (employeeID.Value > 0))
            //        {
            //            query = (from b in WorkOrderLineItemTable.GetAllItems(_db)
            //                     join c in UniformTransactionTable.GetAllItems(_db) on b.WorkOrderLineItemID equals c.WorkOrderLineItemID
            //                     where b.WorkOrder.LaundryVendorID == vendorID
            //                         && b.WorkOrder.StatusID != (int)StatusValue.Deleted
            //                         && c.ProcessStatusID == (int)ProcessStatusValue.CollectedByVendor
            //                         && c.LinenRoomID == warehouseID
            //                         && c.EmployeeUniform.EmployeeID == employeeID.Value
            //                     select b);
            //        }

            //        var WorkOrder = (from b in query
            //                         select new WorkOrderDetailData
            //                         {
            //                             WorkOrderID = b.WorkOrderID,
            //                             WorkOrderNo = b.WorkOrder.WorkOrderNo,
            //                             WorkOrderLineItemID = b.WorkOrderLineItemID,
            //                             VendorID = b.WorkOrder.LaundryVendorID,
            //                             EmployeeUniformID = b.EmployeeUniformID,
            //                             WashTypeID = b.WashTypeID,
            //                             WashType = b.WashType.WashTypeName,
            //                             EmployeeID = b.EmployeeUniform.EmployeeID,
            //                             UniformItemID = b.EmployeeUniform.UniformItemID,
            //                             UniformBarcode = b.EmployeeUniform.UniformBarcode,
            //                             ItemCategoryID = b.EmployeeUniform.UniformItem.UniformCategoryID,
            //                             ItemCategory = b.EmployeeUniform.UniformItem.UniformCategory.UniformCategoryName,
            //                             ItemCode = b.EmployeeUniform.UniformItem.UniformItemCode,
            //                             ItemName = b.EmployeeUniform.UniformItem.UniformItemName,
            //                             Color = b.EmployeeUniform.UniformItem.Color.ColorName,
            //                             Material = b.EmployeeUniform.UniformItem.Material.MaterialName,
            //                             Logo = b.EmployeeUniform.UniformItem.Logo.LogoName,
            //                             Size = b.EmployeeUniform.Size.SizeName
            //                         }).ToList();

            //        result.WorkOrderData = WorkOrder;

            //        result.Message = "Saved Successfully";
            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllWorkOrderDetailsResponse>(ex);
            //    }
            //}

            //public ActionResult Vendor_UnprocessedUniformReturn(string deviceSerialNo, int userID, int warehouseID, string uniformBarcode, int reasonID, string returnRemarks)//List<int> workOrderLineItemIds)
            //{
            //    try
            //    {
            //        var result = new GetAllWorkOrderDetailsResponse();
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");
            //        var vendorID = PersonTable.GetPersonLaundryVendorID(_db, userID);

            //        _db.CurrentUserID = userID;

            //        //if (workOrderLineItemIds.Count() > 0)
            //        {
            //            //foreach (var id in workOrderLineItemIds)
            //            {
            //                var uniformtransaction = (from b in WorkOrderLineItemTable.GetAllItems(_db)
            //                                          join c in UniformTransactionTable.GetAllItems(_db) on b.WorkOrderLineItemID equals c.WorkOrderLineItemID
            //                                          where b.WorkOrder.LaundryVendorID == vendorID
            //                                              && c.TransactionTypeID == (int)TransactionTypeValue.CollectedForWash
            //                                              && c.ProcessStatusID == (int)ProcessStatusValue.CollectedByVendor
            //                                              && c.EmployeeUniform.UniformBarcode == uniformBarcode
            //                                              && c.LinenRoomID == warehouseID
            //                                          select c).FirstOrDefault();

            //                if (uniformtransaction != null)
            //                {
            //                    uniformtransaction.UnprocessedUniformReturnRemarks = returnRemarks;
            //                    uniformtransaction.UnprocessedUniformReturnReasonID = reasonID;

            //                    uniformtransaction.ProcessStatusID = (int)ProcessStatusValue.UnprocessedUniformReturnByVendor;
            //                    uniformtransaction.TransactionTypeID = (int)TransactionTypeValue.CleanedUniform;

            //                    uniformtransaction.ReturnedByVendorUserID = userID;
            //                    uniformtransaction.ReturnedFromVendorDate = DateTime.Now;
            //                }
            //                else
            //                {
            //                    throw new ValidationException($"Invalid barcode or not issued to wash - {uniformBarcode}");
            //                }

            //                var uniform = (from b in EmployeeUniformTable.GetAllItems(_db)
            //                               where b.UniformBarcode == uniformBarcode
            //                               select b).FirstOrDefault();
            //                if (uniform != null)
            //                {
            //                    uniform.EmployeeUniformStatusID = (int)EmployeeUniformStatusValues.InLinenRoomForReturnToEmployee;
            //                }

            //                _db.SaveChanges();

            //                var WorkOrder = (from b in WorkOrderLineItemTable.GetAllItems(_db)
            //                                 join c in UniformTransactionTable.GetAllItems(_db) on b.WorkOrderLineItemID equals c.WorkOrderLineItemID
            //                                 where b.WorkOrder.LaundryVendorID == vendorID
            //                                     && c.UniformTransactionID == uniformtransaction.UniformTransactionID
            //                                     && c.EmployeeUniform.UniformBarcode == uniformBarcode
            //                                 select new WorkOrderDetailData
            //                                 {
            //                                     WorkOrderID = b.WorkOrderID,
            //                                     WorkOrderNo = b.WorkOrder.WorkOrderNo,
            //                                     WorkOrderLineItemID = b.WorkOrderLineItemID,
            //                                     VendorID = b.WorkOrder.LaundryVendorID,
            //                                     EmployeeUniformID = b.EmployeeUniformID,
            //                                     WashTypeID = b.WashTypeID,
            //                                     WashType = b.WashType.WashTypeName,
            //                                     EmployeeID = b.EmployeeUniform.EmployeeID,
            //                                     UniformItemID = b.EmployeeUniform.UniformItemID,
            //                                     UniformBarcode = b.EmployeeUniform.UniformBarcode,
            //                                     ItemCategoryID = b.EmployeeUniform.UniformItem.UniformCategoryID,
            //                                     ItemCategory = b.EmployeeUniform.UniformItem.UniformCategory.UniformCategoryName,
            //                                     ItemCode = b.EmployeeUniform.UniformItem.UniformItemCode,
            //                                     ItemName = b.EmployeeUniform.UniformItem.UniformItemName,
            //                                     Color = b.EmployeeUniform.UniformItem.Color.ColorName,
            //                                     Material = b.EmployeeUniform.UniformItem.Material.MaterialName,
            //                                     Logo = b.EmployeeUniform.UniformItem.Logo.LogoName,
            //                                     Size = b.EmployeeUniform.Size.SizeName
            //                                 }).ToList();

            //                result.WorkOrderData = WorkOrder;
            //            }
            //        }

            //        result.Message = "Saved Successfully";
            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllWorkOrderDetailsResponse>(ex);
            //    }
            //}

            //#endregion

            //#region Unprocessed Uniform return

            //public ActionResult GetAllUniformTransactionDetails(string deviceSerialNo, int userID, int warehouseID, string uniformBarcode)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}, {uniformBarcode}");

            //        var WorkOrder = (from b in WorkOrderLineItemTable.GetAllItems(_db)
            //                         join c in UniformTransactionTable.GetAllItems(_db) on b.WorkOrderLineItemID equals c.WorkOrderLineItemID
            //                         where c.EmployeeUniform.UniformBarcode == uniformBarcode && c.TransactionTypeID == (int)TransactionTypeValue.CollectedForWash
            //                         && c.ProcessStatusID == (int)ProcessStatusValue.WorkOrderCreated
            //                         select new WorkOrderDetailData
            //                         {
            //                             WorkOrderID = b.WorkOrderID,
            //                             WorkOrderNo = b.WorkOrder.WorkOrderNo,
            //                             WorkOrderLineItemID = b.WorkOrderLineItemID,
            //                             VendorID = b.WorkOrder.LaundryVendorID,
            //                             EmployeeUniformID = b.EmployeeUniformID,
            //                             WashTypeID = b.WashTypeID,
            //                             WashType = b.WashType.WashTypeName,
            //                             EmployeeID = b.EmployeeUniform.EmployeeID,
            //                             UniformItemID = b.EmployeeUniform.UniformItemID,
            //                             UniformBarcode = b.EmployeeUniform.UniformBarcode,
            //                             ItemCategoryID = b.EmployeeUniform.UniformItem.UniformCategoryID,
            //                             ItemCategory = b.EmployeeUniform.UniformItem.UniformCategory.UniformCategoryName,
            //                             ItemCode = b.EmployeeUniform.UniformItem.UniformItemCode,
            //                             ItemName = b.EmployeeUniform.UniformItem.UniformItemName,
            //                             Color = b.EmployeeUniform.UniformItem.Color.ColorName,
            //                             Material = b.EmployeeUniform.UniformItem.Material.MaterialName,
            //                             Logo = b.EmployeeUniform.UniformItem.Logo.LogoName,
            //                             Size = b.EmployeeUniform.Size.SizeName

            //                         }).ToList();


            //        var result = new GetAllWorkOrderDetailsResponse()
            //        {
            //            WorkOrderData = WorkOrder
            //        };
            //        return Json(result);

            //    }
            //    catch (Exception ex)
            //    {

            //        return ErrorActionResult<GetAllUniformReturnToEmployeeResponse>(ex);
            //    }
            //}

            //[HttpPost]
            //public ActionResult SaveUnprocessUniformReturn([FromBody] UnprocessedUniformReturnData unprocessedUniformReturnData)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{unprocessedUniformReturnData.DeviceSerialNo}, {unprocessedUniformReturnData.UserID}, {unprocessedUniformReturnData.WarehouseID}, Employee uniformid: {unprocessedUniformReturnData.EmployeeUniformID}");
            //        //_db.EnableInstanceQueryLog = true;
            //        var result = new BaseResponse();
            //        if (unprocessedUniformReturnData != null)
            //        {
            //            UnProcessedUniformReturnTable unProcessedUniformReturn = new UnProcessedUniformReturnTable();
            //            unProcessedUniformReturn.LaundryVendorID = unprocessedUniformReturnData.VendorID;
            //            unProcessedUniformReturn.LinenRoomID = unprocessedUniformReturnData.WarehouseID;
            //            unProcessedUniformReturn.CreatedBy = unprocessedUniformReturnData.UserID;
            //            unProcessedUniformReturn.CreatedDateTime = DateTime.Now;
            //            unProcessedUniformReturn.StatusID = (int)StatusValue.Active;
            //            _db.Add(unProcessedUniformReturn);

            //            UnProcessedUniformReturnLineItemTable lineItem = new UnProcessedUniformReturnLineItemTable();
            //            lineItem.UnProcessedUniformReturn = unProcessedUniformReturn;
            //            lineItem.EmployeeUniformID = unprocessedUniformReturnData.EmployeeUniformID;
            //            lineItem.UnProcessedUniformReturnReasonID = unprocessedUniformReturnData.UnprocessedUniformReturnReasonID;
            //            lineItem.Remarks = unprocessedUniformReturnData.Remarks;
            //            lineItem.CreatedBy = unprocessedUniformReturnData.UserID;
            //            lineItem.CreatedDateTime = DateTime.Now;
            //            lineItem.StatusID = (int)StatusValue.Active;
            //            _db.Add(lineItem);

            //            var uniformTransaction = (from b in UniformTransactionTable.GetAllItems(_db)
            //                                      where b.WorkOrderLineItemID == unprocessedUniformReturnData.WorkOrderLineItemID
            //                                      && b.ProcessStatusID == (int)ProcessStatusValue.WorkOrderCreated
            //                                      && b.EmployeeUniformID == unprocessedUniformReturnData.EmployeeUniformID
            //                                      select b).FirstOrDefault();

            //            uniformTransaction.ProcessStatusID = (int)ProcessStatusValue.UnprocessedUniformReturnByVendor;

            //        }

            //        _db.SaveChanges();
            //        result.Message = "Saved Successfully";
            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetDepartmentEmployeeDetailResponse>(ex);
            //    }
            //}

            //#endregion

            //#region Desktop - Incident handling

            //public ActionResult GetPendingIncidents(string deviceSerialNo, int userID, int warehouseID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");

            //        var pendingItems = (from b in IncidentsLineItemTable.GetAllItems(_db)
            //                            where b.IsResloved == false
            //                                 && b.Incidents.LinenRoomID == warehouseID && b.StatusID==(int)StatusValue.Active
            //                            select new IncidentsData
            //                            {
            //                                IncidentID = b.IncidentsID,
            //                                IncidentsNo = b.Incidents.IncidentsNo,
            //                                PersonCode = b.Incidents.Employee.PersonCode,
            //                                PersonFirstName = b.Incidents.Employee.PersonFirstName,
            //                                PersonLastName = b.Incidents.Employee.PersonLastName,
            //                                DepartmentName = b.Incidents.Employee.Department.DepartmentName,
            //                                DivisionName = b.Incidents.Employee.Department.Division.DivisionName,
            //                            }).Distinct().ToList();

            //        var result = new GetAllIncidentsResponse()
            //        {
            //            IncidentsData = pendingItems
            //        };

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllIncidentsResponse>(ex);
            //    }
            //}

            //public ActionResult GetAllIncidentDetails(string deviceSerialNo, int userID, int warehouseID, int incidentID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}, {incidentID}");

            //        var incidentsDetailsData = (from b in IncidentsLineItemTable.GetAllItems(_db)
            //                                    join c in EmployeeUniformTable.GetAllItems(_db) on b.EmployeeUniformID equals c.EmployeeUniformID into k
            //                                    from d in k.DefaultIfEmpty()
            //                                    join e in UniformItemTable.GetAllItems(_db) on d.UniformItemID equals e.UniformItemID into l
            //                                    from f in l.DefaultIfEmpty()
            //                                    where b.IncidentsID == incidentID && b.StatusID == (int)StatusValue.Active
            //                                    && b.IsResloved==false
            //                                    select new IncidentsDetailsData
            //                                    {
            //                                        IncidentLineItemID = b.IncidentsLineItemID,
            //                                        IncidentID = b.IncidentsID,
            //                                        IncidentTypeID = b.IncidentTypeID,
            //                                        IncidentType = b.IncidentType.IncidentTypeName,
            //                                        Remarks = b.Remarks,
            //                                        SizeID = d.SizeID,
            //                                        Size = d.Size.SizeName,
            //                                        ManualSizeValue = d.ManualSizeValue,
            //                                        UniformBarcode = d.UniformBarcode,
            //                                        UniformItemID = d.UniformItemID,
            //                                        UniformItemCode = f.UniformItemCode,
            //                                        UniformItemName = f.UniformItemName,
            //                                        UniformCategoryID = f.UniformCategoryID,
            //                                        UniformCategory = f.UniformCategory.UniformCategoryName,
            //                                        LogoID = f.LogoID,
            //                                        LogoName = f.Logo != null ? f.Logo.LogoName : "",
            //                                        ColorID = f.ColorID,
            //                                        ColorName = f.Color != null ? f.Color.ColorName : "",
            //                                        MaterialID = f.MaterialID,
            //                                        MaterialName = f.Material != null ? f.Material.MaterialName : ""
            //                                    }).ToList();


            //        var result = new GetAllIncidentsDetailsResponse()
            //        {
            //            IncidentsDetailsData = incidentsDetailsData
            //        };

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetAllIncidentsDetailsResponse>(ex);
            //    }
            //}

            //[HttpPost]
            //public ActionResult UpdateResolvedIncidents([FromBody] ResolvedIncidentsData Data)
            //{
            //    try
            //    {
            //        var result = new BaseResponse();
            //        base.TraceLog("MobileAPI", $" {Data.IncidentsLineItemID},{Data.ResolveRemarks}");

            //        if (Data.IncidentsLineItemID.Count() > 0)
            //        {
            //            foreach (var id in Data.IncidentsLineItemID)
            //            {
            //                var incidentsLineItemTable = IncidentsLineItemTable.GetItem(_db, id);

            //                if (incidentsLineItemTable != null)
            //                {
            //                    incidentsLineItemTable.IsResloved = true;
            //                    incidentsLineItemTable.ReslovedDateTime = DateTime.Now;
            //                    incidentsLineItemTable.ReslovedBy = Data.UserID;
            //                    incidentsLineItemTable.ResloveRemarks = Data.ResolveRemarks;
            //                }
            //            }

            //            _db.SaveChanges();
            //        }

            //        result.Message = $"Incidents resolved successfully.";

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<BaseResponse>(ex);
            //    }
            //}

            //[HttpPost]
            //public ActionResult ReissueIncidentsItems([FromBody] ResolvedIncidentsData Data)
            //{
            //    try
            //    {
            //        var result = new BaseResponse();
            //        base.TraceLog("MobileAPI", $" {Data.IncidentsLineItemID},{Data.ResolveRemarks}");
            //        var WashTypeID = (from b in _db.WashTypeTable
            //                          where b.StatusID == (int)StatusValue.Active
            //                          orderby b.WashTypeID
            //                          select b.WashTypeID).FirstOrDefault();

            //        if (WashTypeID != null)
            //        {
            //            if (Data.IncidentsLineItemID.Count() > 0)
            //            {
            //                List<LastLaundryVendorListModel> lastLaundryVendorListModel = new List<LastLaundryVendorListModel>();
            //                //Take the last laundry vendor for that employee uniform
            //                foreach (var id in Data.IncidentsLineItemID)
            //                {
            //                    var incidentLineItem = IncidentsLineItemTable.GetItem(_db, id);
            //                    var workorderLineItemDetails = (from b in WorkOrderLineItemTable.GetAllItems(_db)
            //                                                    where b.EmployeeUniformID == incidentLineItem.EmployeeUniformID
            //                                                    select new LastLaundryVendorListModel
            //                                                    {
            //                                                        LaundryvendorID = b.WorkOrder.LaundryVendorID,
            //                                                        EmployeeUniformID = b.EmployeeUniformID,
            //                                                        WorkOrderLineItemID = b.WorkOrderLineItemID
            //                                                    }).OrderByDescending(c => c.WorkOrderLineItemID).FirstOrDefault();

            //                    lastLaundryVendorListModel.Add(workorderLineItemDetails);
            //                    incidentLineItem.IsResloved = true;
            //                    incidentLineItem.ReslovedDateTime = DateTime.Now;
            //                    incidentLineItem.ReslovedBy = Data.UserID;
            //                    incidentLineItem.ResloveRemarks = "Reissue-"+Data.ResolveRemarks;
            //                }

            //                //create work order against the laundry vendor and uifomrm transaction
            //                var UniqueLaundryVendorList = (from k in lastLaundryVendorListModel
            //                                               select k.LaundryvendorID).Distinct().ToList();
            //                foreach (int v in UniqueLaundryVendorList)
            //                {
            //                    WorkOrderTable workorder = new WorkOrderTable();
            //                    workorder.LaundryVendorID = v;
            //                    workorder.WorkOrderNo = CodeGenerationHelper.GetNextCode("WorkOrder");//DateTime.Now.ToString("ddMMyyyyHHmmss") + v.ToString();
            //                    workorder.CreatedBy = Data.UserID;
            //                    workorder.LinenRoomID = Data.WarehouseID;
            //                    _db.Add(workorder);

            //                    var vendorPrices = (from b in LaundryVendorPricingTable.GetAllItems(_db)
            //                                        where b.LaundryVendorID == v
            //                                        select b);
            //                    var workorderLineItems = lastLaundryVendorListModel.Where(c => c.LaundryvendorID == v).ToList();
            //                    var newCode = CodeGenerationHelper.GetNextCode("UniformCollection");

            //                    foreach (var workOrderlI in workorderLineItems)
            //                    {
            //                        UniformTransactionTable uniformTransaction = new UniformTransactionTable();
            //                        uniformTransaction.EmployeeUniformID = workOrderlI.EmployeeUniformID;
            //                        uniformTransaction.LinenRoomID = Data.WarehouseID;
            //                        uniformTransaction.WashTypeID = WashTypeID;
            //                        uniformTransaction.IsForRewash = true;
            //                        uniformTransaction.TransactionTypeID = (int)TransactionTypeValue.CollectedForWash;
            //                        uniformTransaction.ProcessStatusID = (int)ProcessStatusValue.CollectedForWashFromEmployee;
            //                        uniformTransaction.StatusID = (int)StatusValue.Active;
            //                        uniformTransaction.CreatedBy = Data.UserID;
            //                        uniformTransaction.CreatedDateTime = DateTime.Now;
            //                        uniformTransaction.CollectionReceiptNo = newCode;

            //                        EmployeeUniformTable.GetItem(_db, workOrderlI.EmployeeUniformID).EmployeeUniformStatusID = (int)EmployeeUniformStatusValues.InLinenRoomForWash;
            //                        _db.Add(uniformTransaction);

            //                        var uniformItemCategory = (from b in EmployeeUniformTable.GetAllItems(_db)
            //                                                   where b.EmployeeUniformID == workOrderlI.EmployeeUniformID
            //                                                   select b.UniformItem.UniformCategoryID).FirstOrDefault();


            //                        WorkOrderLineItemTable LI = new WorkOrderLineItemTable();
            //                        LI.WorkOrder = workorder;
            //                        LI.EmployeeUniformID = uniformTransaction.EmployeeUniformID;
            //                        LI.WashTypeID = uniformTransaction.WashTypeID;
            //                        LI.IsForRewash = uniformTransaction.IsForRewash;
            //                        LI.DateOfIssueToVendor = DateTime.Now;
            //                        LI.CreatedBy = Data.UserID;
            //                        var PriceList = vendorPrices.Where(b => b.WashTypeID == WashTypeID && b.UniformCategoryID == uniformItemCategory).FirstOrDefault();
            //                        LI.Rate = PriceList != null ? PriceList.Price : 0;

            //                        _db.Add(LI);

            //                        uniformTransaction.ProcessStatusID = (int)ProcessStatusValue.WorkOrderCreated;
            //                        uniformTransaction.WorkOrderLineItem = LI;
            //                    }
            //                }

            //                _db.SaveChanges();
            //            }

            //            result.Message = $"Reissued for wash.";

            //            return Json(result);
            //        }
            //        else
            //        {
            //            result.Message = $"WashType will consider as normal wash which is not in the master, Please create it.";

            //            return Json(result);
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<BaseResponse>(ex);
            //    }
            //}

            //[HttpPost]
            //public ActionResult ReorderIncidentsItems([FromBody] ResolvedIncidentsData Data)
            //{
            //    try
            //    {
            //        var result = new BaseResponse();
            //        base.TraceLog("MobileAPI", $" {Data.IncidentsLineItemID},{Data.ResolveRemarks}");

            //        if (Data.IncidentsLineItemID.Count() > 0)
            //        {
            //            var EmployeeUniformList = (from b in IncidentsLineItemTable.GetAllItems(_db)
            //                                       where Data.IncidentsLineItemID.Contains(b.IncidentsLineItemID)
            //                                       select b.EmployeeUniform).ToList();

            //           UniformRequestTable uniformRequest = new UniformRequestTable();

            //            uniformRequest.ApprovalStatusID = (int)ApprovalStatusValue.WaitingForApproval;
            //            uniformRequest.UniformRequestCode = CodeGenerationHelper.GetNextCode("UniformRequest");
            //            var person = PersonTable.GetItem(_db, EmployeeUniformList.FirstOrDefault().EmployeeID);
            //            var dept = DepartmentTable.GetItem(_db, person.DepartmentID.Value);
            //            uniformRequest.DivisionID = dept.DivisionID;
            //            uniformRequest.IsReorder = true;
            //            _db.Add(uniformRequest);

            //            //add line items
            //            foreach (var entry in EmployeeUniformList)
            //            {
            //                UniformRequestLineItemTable litem = new UniformRequestLineItemTable();
            //                litem.UniformRequest = uniformRequest;
            //                litem.EmployeeID = entry.EmployeeID;
            //                litem.UniformItemID = entry.UniformItemID;
            //                litem.UniformSetID = entry.UniformSetID;
            //                litem.SizeID = entry.SizeID;
            //                litem.ManualSizeValue = entry.ManualSizeValue;
            //                litem.Qty = entry.Qty;
            //                _db.Add(litem);
            //            }
            //            foreach (var entry in Data.IncidentsLineItemID)
            //            {
            //                var incidentLineItem = IncidentsLineItemTable.GetItem(_db, entry);
            //                incidentLineItem.IsResloved = true;
            //                incidentLineItem.ReslovedDateTime = DateTime.Now;
            //                incidentLineItem.ReslovedBy = Data.UserID;
            //                incidentLineItem.ResloveRemarks = "Reorder-" + Data.ResolveRemarks;

            //            }
            //                _db.SaveChanges();
            //        }

            //        result.Message = $"Successfully reordered uniforms.";

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<BaseResponse>(ex);
            //    }
            //}

            //[HttpPost]
            //public ActionResult GenerateCreditNote([FromBody] ResolvedIncidentsData Data)
            //{
            //    try
            //    {
            //        var result = new BaseResponse();
            //        base.TraceLog("MobileAPI", $" {Data.IncidentsLineItemID},{Data.ResolveRemarks}");
            //        string creditNoteNo = "";
            //        if (Data.IncidentsLineItemID.Count() > 0)
            //        {
            //            List<LastLaundryVendorListModel> lastLaundryVendorListModel = new List<LastLaundryVendorListModel>();
            //            //Take the last laundry vendor for that employee uniform
            //            foreach (var id in Data.IncidentsLineItemID)
            //            {
            //                var incidentLineItem = IncidentsLineItemTable.GetItem(_db, id);
            //                incidentLineItem.IsResloved = true;
            //                incidentLineItem.ReslovedDateTime = DateTime.Now;
            //                incidentLineItem.ReslovedBy = Data.UserID;
            //                incidentLineItem.ResloveRemarks = "GeneratedCreditNote-" + Data.ResolveRemarks;
            //                var workorderLineItemDetails = (from b in WorkOrderLineItemTable.GetAllItems(_db)
            //                                                where b.EmployeeUniformID == incidentLineItem.EmployeeUniformID

            //                                                select new LastLaundryVendorListModel
            //                                                {
            //                                                    LaundryvendorID = b.WorkOrder.LaundryVendorID,
            //                                                    EmployeeUniformID = b.EmployeeUniformID,
            //                                                    WorkOrderLineItemID = b.WorkOrderLineItemID
            //                                                }).OrderByDescending(c => c.WorkOrderLineItemID).FirstOrDefault();
            //                lastLaundryVendorListModel.Add(workorderLineItemDetails);
            //            }
            //            var UniqueLaundryVendorList = (from k in lastLaundryVendorListModel
            //                                           select k.LaundryvendorID).Distinct().ToList();
            //            foreach (int v in UniqueLaundryVendorList)
            //            {
            //                CreditNoteTable creditNote = new CreditNoteTable();
            //                creditNote.LaundryVendorID = v;
            //                creditNote.CreditNoteNo = CodeGenerationHelper.GetNextCode("CreditNote");//DateTime.Now.ToString("ddMMyyyyHHmmss") + v.ToString();
            //                _db.Add(creditNote);
            //                var LineItems = lastLaundryVendorListModel.Where(c => c.LaundryvendorID == v).ToList();
            //                foreach (var LI in LineItems)
            //                {

            //                    var EmployeeUniform = EmployeeUniformTable.GetItem(_db, LI.EmployeeUniformID);
            //                    var uniformitem = UniformItemTable.GetItem(_db, EmployeeUniform.UniformItemID);

            //                    CreditNoteLineItemTable lineItemTable = new CreditNoteLineItemTable();
            //                    lineItemTable.CreditNote = creditNote;
            //                    lineItemTable.EmployeeUniformID = LI.EmployeeUniformID;
            //                    lineItemTable.Amount = uniformitem.CreditNotePrice;
            //                    lineItemTable.Remarks = Data.ResolveRemarks;

            //                    _db.Add(lineItemTable);
            //                }
            //                _db.SaveChanges();
            //            }
            //            result.Message = $"CreditNote generated.";

            //            return Json(result);


            //        }
            //        else
            //        {
            //            result.Message = $"Selected Item is empty.";

            //            return Json(result);
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<BaseResponse>(ex);
            //    }
            //}

            //#endregion

            //#region SSA Mobile App Methods

            //[HttpGet]
            //public ActionResult SSA_RegisterMobile(string deviceSerialNo, int userID, int warehouseID, string token)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");

            //        var prevToken = (from b in _db.PersonMobileTokenTable where b.Token == token select b).ToList();
            //        prevToken.ForEach(c =>
            //        {
            //            c.IsActive = false;
            //            c.DeactivatedDateTime = DateTime.Now;
            //        });

            //        var tokenTable = new PersonMobileTokenTable()
            //        {
            //            PersonID = userID,
            //            IsActive = true,
            //            Token = token,
            //            CreatedDateTime = DateTime.Now,
            //            ExpiryDateTime = DateTime.Now.AddYears(1)
            //        };

            //        _db.Add(tokenTable);
            //        _db.SaveChanges();

            //        return Json(new BaseResponse());
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<BaseResponse>(ex);
            //    }
            //}

            //[HttpGet]
            //public ActionResult SSA_DeregisterMobile(string deviceSerialNo, int userID, int warehouseID, string token)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");

            //        var prevToken = (from b in _db.PersonMobileTokenTable where b.Token == token select b).ToList();
            //        prevToken.ForEach(c =>
            //        {
            //            c.IsActive = false;
            //            c.DeactivatedDateTime = DateTime.Now;
            //        });

            //        return Json(new BaseResponse());
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<BaseResponse>(ex);
            //    }
            //}

            //[HttpGet]
            //public ActionResult SSA_GetEmployeeVisitSchedule(string deviceSerialNo, int userID, int warehouseID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");

            //        //check any pending schedules are there
            //        var sch = (from b in ScheduleLineItemTable.GetAllItems(_db)
            //                   where b.EmployeeID == userID
            //                         && b.Schedule.ScheduleDate > DateTime.Now
            //                         && b.Schedule.StatusID != (int)StatusValue.Deleted
            //                   select new GetEmployeeVisitScheduleResponse()
            //                   {
            //                       ScheduleDateTime = b.Schedule.ScheduleDate,
            //                       SchedulFound = true,
            //                       Location = b.Schedule.LinenRoom.LinenRoomName
            //                   }).FirstOrDefault();

            //        if (sch != null)
            //            return Json(sch);
            //        else
            //            return ErrorActionResult<GetEmployeeVisitScheduleResponse>("No schedule found");
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetEmployeeVisitScheduleResponse>(ex);
            //    }
            //}

            //[HttpGet]
            //public ActionResult SSA_GetUniformRequestStatus(string deviceSerialNo, int userID, int warehouseID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");

            //        //check any pending schedules are there
            //        var sch = (from b in UniformRequestLineItemTable.GetAllItems(_db)
            //                   where b.UniformRequest.ApprovalStatusID == 2
            //                        && b.UniformRequest.IsClosed == false
            //                        && b.StatusID != (int)StatusValue.Deleted
            //                        && b.EmployeeID == userID
            //                         //&& b.IssuedUniformQty < b.Qty
            //                         && b.UniformRequest.StatusID != (int)StatusValue.Deleted
            //                   select new UniformRequestStatusData()
            //                   {
            //                       ItemCode = b.UniformItem.UniformItemCode,
            //                       ItemName = b.UniformItem.UniformItemName,
            //                       RequestNo = b.UniformRequest.UniformRequestCode,

            //                       Qty = b.Qty,
            //                       IssuedQty = b.IssuedUniformQty,
            //                       PendingQty = b.Qty - b.IssuedUniformQty
            //                   }).ToList();

            //        return Json(new GetUniformRequestStatusResponse()
            //        {
            //            Requests = sch
            //        });
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetUniformRequestStatusResponse>(ex);
            //    }
            //}

            //public ActionResult SSA_GetUniformsForReceiveConfirmation(string deviceSerialNo, int userID, int warehouseID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");

            //        //_db.EnableInstanceQueryLog = true;
            //        var uniformData = (from b in EmployeeUniformTable.GetAllItems(_db)
            //                           where b.EmployeeID == userID
            //                           && b.EmployeeUniformStatusID == (int)EmployeeUniformStatusValues.IssuedToEmployee
            //                           select new EmployeeUniformData()
            //                           {
            //                               EmployeeUniformID = b.EmployeeUniformID,
            //                               UniformItemID = b.UniformItemID,
            //                               EmployeeID = b.EmployeeID,

            //                               UniformBarcode = b.UniformBarcode,

            //                               ItemCategory = b.UniformItem.UniformCategory.UniformCategoryName,
            //                               ItemCode = b.UniformItem.UniformItemCode,
            //                               ItemName = b.UniformItem.UniformItemName,

            //                               Color = b.UniformItem.Color.ColorName,
            //                               Material = b.UniformItem.Material.MaterialName,
            //                               Logo = b.UniformItem.Logo.LogoName,
            //                               Size = b.Size.SizeName,
            //                               ManualSizeValue = b.ManualSizeValue,

            //                               UniformStatusID = b.EmployeeUniformStatusID,
            //                               UniformStatus = b.EmployeeUniformStatus.EmployeeUniformStatusName,

            //                               IssuedDate = b.IssuedDate,
            //                               WashCount = b.WashCount
            //                           }).ToList();

            //        var result = new GetEmployeeUniformResponse()
            //        {
            //            Uniforms = uniformData
            //        };
            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetEmployeeUniformResponse>(ex);
            //    }
            //}

            //[HttpPost]
            //public ActionResult SSA_ConfirmEmployeeUniformReceived([FromBody] UniformIDRequest data)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{data.DeviceSerialNo}, {data.UserID}, {data.WarehouseID}, Count: {data.EmployeeUniformIDs.Count}");
            //        var allUniform = (from b in EmployeeUniformTable.GetAllItems(_db)
            //                          where (from c in data.EmployeeUniformIDs
            //                                 select c).Contains(b.EmployeeUniformID)
            //                          select b).ToList();
            //        int count = 0;

            //        foreach (var uniform in allUniform)
            //        {
            //            if (uniform.EmployeeUniformStatusID == (int)EmployeeUniformStatusValues.IssuedToEmployee)
            //            {
            //                uniform.EmployeeUniformStatusID = (int)EmployeeUniformStatusValues.WithEmployee;
            //                uniform.ReceiveConfirmedDate = DateTime.Now;
            //            }
            //            else
            //            {
            //                return ErrorActionResult<GetEmployeeUniformResponse>($"Uniform '{uniform.UniformBarcode}' already issued");
            //            }

            //            count++;
            //        }

            //        _db.SaveChanges();

            //        return Json(new BaseResult()
            //        {
            //            Message = $"{count} uniform confirmed."
            //        });
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<BaseResult>(ex);
            //    }
            //}

            //public async Task<ActionResult> SSA_GetDashboardData(string deviceSerialNo, int userID, int warehouseID)
            //{
            //    try
            //    {
            //        //get the uniforms based on status
            //        var uniformStatus = (from b in EmployeeUniformTable.GetAllItems(_db)
            //                             where b.EmployeeID == userID
            //                             group b by new { b.EmployeeUniformStatusID, b.EmployeeUniformStatus.EmployeeUniformStatusCode } into grp
            //                             orderby grp.Key.EmployeeUniformStatusID
            //                             select new DashboardCategoryData()
            //                             {
            //                                 Category = grp.Key.EmployeeUniformStatusCode,
            //                                 Name = grp.Key.EmployeeUniformStatusCode,
            //                                 Value = grp.Count()
            //                             }).ToList();

            //        //Incident Items
            //        //var incidents = from b in IncidentsTable.GetAllItems(_db)
            //        //                where b.EmployeeID == userID
            //        //                //group b by b.EmployeeUniformStatus.EmployeeUniformStatusCode into grp
            //        //                select new
            //        //                {
            //        //                    Status = "",
            //        //                    TotalItems = grp.Count()
            //        //                };

            //        var result = new GetDesktopDashboardDataResponse();

            //        var proc = new LMSContextProcedures(_db);
            //        var items = await proc.dprc_EmployeeDataCountAsync(userID);
            //        if ((items != null) && (items.Count > 0))
            //        {
            //            foreach (var itm in items)
            //            {
            //                result.CountDatas.Add(new DashboardCountData() { Name = itm.Item, Value = itm.Value + "" });
            //            }
            //        }
            //        var dashboardUniformData = new DashboardCategoryDataCollection()
            //        {
            //            CollectionName = "UniformStatus",
            //            Data = uniformStatus
            //        };
            //        result.SeriesDatas.Add(dashboardUniformData);

            //        var dashboardIncidentsData = new DashboardCategoryDataCollection()
            //        {
            //            CollectionName = "IncidentData",
            //            Data = new List<DashboardCategoryData>()
            //            {
            //                new DashboardCategoryData(){ Category = "Total Incidents", Name="Total Incidents", Value=0 },
            //                new DashboardCategoryData(){ Category = "Resolved", Name="Resolved", Value=0 },
            //                new DashboardCategoryData(){ Category = "Pending", Name="Pending", Value=0 },
            //            }
            //        };
            //        result.SeriesDatas.Add(dashboardIncidentsData);

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetDesktopDashboardDataResponse>(ex);
            //    }
            //}

            //#region Forgot Password

            //public ActionResult SSA_FPass_SendOTP(string deviceSerialNo, int userID, int warehouseID, string username)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}, {username}");

            //        //validate user exists or not
            //        var user = User_LoginUserTable.GetUser(_db, username);
            //        if (user == null)
            //        {
            //            ErrorActionResult<BaseResponse>($"Invalid username '{username}'");
            //        }
            //        if (user.IsDisabled)
            //        {
            //            ErrorActionResult<BaseResponse>($"user '{username}' disabled by administrator");
            //        }
            //        if (user.IsLockedOut)
            //        {
            //            ErrorActionResult<BaseResponse>($"user '{username}' is locked, please contact administrator.");
            //        }

            //        var person = PersonTable.GetItem(_db, user.UserID);
            //        if (person == null)
            //        {
            //            ErrorActionResult<BaseResponse>($"Invalid username '{username}'");
            //        }
            //        if ((person.UserTypeID == (int)UserTypeValue.Mobile) || (person.UserTypeID == (int)UserTypeValue.WebMobile))
            //        {
            //        }
            //        else
            //        {
            //            ErrorActionResult<BaseResponse>($"Invalid username '{username}'");
            //        }

            //        if (person.StatusID == (int)StatusValue.Deleted)
            //        {
            //            ErrorActionResult<BaseResponse>($"Invalid username '{username}'");
            //        }

            //        var newOTP = new Random().Next(1000, 9999);
            //        var ft = new UserForgotPasswordTable()
            //        {
            //            EmployeeID = person.PersonID,
            //            OTP = newOTP.ToString(),
            //            Username = username,
            //            OTPCreatedDateTime = DateTime.Now,
            //            OTPExpiryDateTime = DateTime.Now.AddMinutes(5)
            //        };

            //        _db.Add(ft);
            //        _db.SaveChanges();

            //        return Json(new BaseResponse());
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<BaseResponse>(ex);
            //    }
            //}

            //public ActionResult SSA_FPass_ValidateOTP(string deviceSerialNo, int userID, int warehouseID, string username, string otp)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}, {username}");

            //        //validate OTP
            //        var ft = (from b in _db.UserForgotPasswordTable
            //                  where b.Username == username
            //                  orderby b.OTPExpiryDateTime descending
            //                  select b).FirstOrDefault();

            //        if (ft == null)
            //        {
            //            return ErrorActionResult<BaseResponse>("Invalid OTP");
            //        }

            //        if (ft.OTPExpiryDateTime < DateTime.Now)
            //        {
            //            return ErrorActionResult<BaseResponse>("OTP expired, please try again");
            //        }

            //        if (ft.OTP != otp)
            //        {
            //            return ErrorActionResult<BaseResponse>("Invalid OTP");
            //        }

            //        return Json(new BaseResponse());
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<BaseResponse>(ex);
            //    }
            //}

            //public ActionResult SSA_FPass_UpdatePassword(string deviceSerialNo, int userID, int warehouseID, string username, string password)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}, {username}");

            //        var user = User_LoginUserTable.GetUser(_db, username);
            //        user.PasswordSalt = EncryptionManager.EncryptPassword(password);
            //        user.Password = EncryptionManager.EncryptPassword(password + user.PasswordSalt);

            //        _db.SaveChanges();

            //        return Json(new BaseResponse()
            //        {
            //            Message = "Reset password completed"
            //        });
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<BaseResponse>(ex);
            //    }
            //}

            //#endregion

            //#region Feedback Module

            //public ActionResult SSA_FB_GetRecentDates(string deviceSerialNo, int userID, int warehouseID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");

            //        DateTime date = DateTime.Now.AddDays(-10);

            //        //get recent transaction dates
            //        var recentDates = (from b in UniformTransactionTable.GetAllItems(_db)
            //                           where b.EmployeeUniform.EmployeeID == userID
            //                                 && b.ProcessStatusID == (int)ProcessStatusValue.ReturnToEmployee
            //                                 && b.ReturnedToEmployeeDate >= date
            //                                 && b.ReturnedToEmployeeDate <= DateTime.Now
            //                           orderby b.ReturnedToEmployeeDate
            //                           select b.ReturnedToEmployeeDate.Value.Date);

            //        return Json(new GetRecentDatesResponse()
            //        {
            //            RecentDates = recentDates.Distinct().ToList()
            //        });
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetRecentDatesResponse>(ex);
            //    }
            //}

            //public ActionResult SSA_FB_GetUniformsForTheDate(string deviceSerialNo, int userID, int warehouseID, string date)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}, {date}");

            //        DateTime dtValue = DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture);

            //        //get recent transaction dates
            //        var uniformData = (from b in UniformTransactionTable.GetAllItems(_db)
            //                           where b.EmployeeUniform.EmployeeID == userID
            //                                 && b.ProcessStatusID == (int)ProcessStatusValue.ReturnToEmployee
            //                                 && b.ReturnedToEmployeeDate >= dtValue
            //                                 && b.ReturnedToEmployeeDate < dtValue.AddDays(1)
            //                           select new EmployeeUniformData()
            //                           {
            //                               EmployeeUniformID = b.EmployeeUniformID,
            //                               UniformItemID = b.EmployeeUniform.UniformItemID,
            //                               EmployeeID = b.EmployeeUniform.EmployeeID,

            //                               UniformBarcode = b.EmployeeUniform.UniformBarcode,

            //                               ItemCategory = b.EmployeeUniform.UniformItem.UniformCategory.UniformCategoryName,
            //                               ItemCode = b.EmployeeUniform.UniformItem.UniformItemCode,
            //                               ItemName = b.EmployeeUniform.UniformItem.UniformItemName,

            //                               Color = b.EmployeeUniform.UniformItem.Color.ColorName,
            //                               Material = b.EmployeeUniform.UniformItem.Material.MaterialName,
            //                               Logo = b.EmployeeUniform.UniformItem.Logo.LogoName,
            //                               Size = b.EmployeeUniform.Size.SizeName,

            //                               UniformStatusID = b.EmployeeUniform.EmployeeUniformStatusID,
            //                               UniformStatus = b.EmployeeUniform.EmployeeUniformStatus.EmployeeUniformStatusName,

            //                               IssuedDate = b.EmployeeUniform.IssuedDate,
            //                               WashCount = b.EmployeeUniform.WashCount,

            //                               UniformRequestCode = b.EmployeeUniform.UniformRequest.UniformRequestCode,
            //                               UniformSetCode = b.EmployeeUniform.UniformSet.UniformSetCode
            //                           }).ToList();

            //        var result = new GetEmployeeUniformResponse()
            //        {
            //            Uniforms = uniformData
            //        };

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetEmployeeUniformResponse>(ex);
            //    }
            //}

            //public ActionResult SSA_FB_UpdateFeedback(string deviceSerialNo, int userID, int warehouseID, string date, int starRating, string feedbackRemarks)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}, {date}, {starRating}, {feedbackRemarks}");
            //        DateTime dtValue = DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture);

            //        var alreadyExists = from b in _db.EmployeeFeedbackTable
            //                            where b.WashDate == dtValue
            //                                   && b.EmployeeID == userID
            //                            select b;
            //        if (alreadyExists.Any())
            //        {
            //            return ErrorActionResult<BaseResponse>($"Feedback already submitted for the date - {date}");
            //        }

            //        var feedback = new EmployeeFeedbackTable()
            //        {
            //            EmployeeID = userID,
            //            WashDate = dtValue,
            //            StarRating = starRating,
            //            EmployeeFeedback = feedbackRemarks
            //        };

            //        _db.Add(feedback);
            //        _db.SaveChanges();

            //        return Json(new BaseResponse()
            //        {
            //            Message = "Feedback updated successfully"
            //        });
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<BaseResponse>(ex);
            //    }
            //}

            //#endregion

            //#region Incident Registration Module

            //public ActionResult SSA_IM_GetRecentDates(string deviceSerialNo, int userID, int warehouseID)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}");

            //        DateTime date = DateTime.Now.AddDays(-10);

            //        //get recent transaction dates
            //        var recentDates = (from b in UniformTransactionTable.GetAllItems(_db)
            //                           where b.EmployeeUniform.EmployeeID == userID
            //                                 && b.ProcessStatusID == (int)ProcessStatusValue.ReturnToEmployee
            //                                 && b.ReturnedToEmployeeDate >= date
            //                                 && b.ReturnedToEmployeeDate <= DateTime.Now
            //                           orderby b.ReturnedToEmployeeDate
            //                           select b.ReturnedToEmployeeDate.Value.Date);

            //        return Json(new GetRecentDatesResponse()
            //        {
            //            RecentDates = recentDates.ToList()
            //        });
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetRecentDatesResponse>(ex);
            //    }
            //}

            //public ActionResult SSA_IM_GetUniformsForTheDate(string deviceSerialNo, int userID, int warehouseID, string date)
            //{
            //    try
            //    {
            //        base.TraceLog("MobileAPI", $"{deviceSerialNo}, {userID}, {warehouseID}, {date}");

            //        DateTime dtValue = DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture);

            //        //get recent transaction dates
            //        var uniformData = (from b in UniformTransactionTable.GetAllItems(_db)
            //                           where b.EmployeeUniform.EmployeeID == userID
            //                                 && b.ProcessStatusID == (int)ProcessStatusValue.ReturnToEmployee
            //                                 && b.ReturnedToEmployeeDate >= dtValue
            //                                 && b.ReturnedToEmployeeDate < dtValue.AddDays(1)
            //                                 && !(from c in IncidentsLineItemTable.GetAllItems(_db, false)
            //                                      where c.Incidents.WashReturnDate == dtValue
            //                                             && c.Incidents.EmployeeID == userID
            //                                      select c.EmployeeUniformID).Contains(b.EmployeeUniformID)
            //                           select new EmployeeUniformData()
            //                           {
            //                               EmployeeUniformID = b.EmployeeUniformID,
            //                               UniformItemID = b.EmployeeUniform.UniformItemID,
            //                               EmployeeID = b.EmployeeUniform.EmployeeID,

            //                               UniformBarcode = b.EmployeeUniform.UniformBarcode,

            //                               ItemCategory = b.EmployeeUniform.UniformItem.UniformCategory.UniformCategoryName,
            //                               ItemCode = b.EmployeeUniform.UniformItem.UniformItemCode,
            //                               ItemName = b.EmployeeUniform.UniformItem.UniformItemName,

            //                               Color = b.EmployeeUniform.UniformItem.Color.ColorName,
            //                               Material = b.EmployeeUniform.UniformItem.Material.MaterialName,
            //                               Logo = b.EmployeeUniform.UniformItem.Logo.LogoName,
            //                               Size = b.EmployeeUniform.Size.SizeName,

            //                               UniformStatusID = b.EmployeeUniform.EmployeeUniformStatusID,
            //                               UniformStatus = b.EmployeeUniform.EmployeeUniformStatus.EmployeeUniformStatusName,

            //                               IssuedDate = b.EmployeeUniform.IssuedDate,
            //                               WashCount = b.EmployeeUniform.WashCount,

            //                               UniformRequestCode = b.EmployeeUniform.UniformRequest.UniformRequestCode,
            //                               UniformSetCode = b.EmployeeUniform.UniformSet.UniformSetCode
            //                           }).ToList();

            //        var result = new GetEmployeeUniformResponse()
            //        {
            //            Uniforms = uniformData
            //        };

            //        return Json(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<GetEmployeeUniformResponse>(ex);
            //    }
            //}

            //[HttpPost]
            //public ActionResult SSA_IM_UpdateIncidents([FromBody] IncidentRequest request)
            //{
            //    try
            //    {
            //        var Person = PersonTable.GetPerson(_db, request.UserCode);
            //        if (Person == null)
            //            return ErrorActionResult<BaseResponse>($"Invalid userCode '{request.UserCode}'");
            //        base.TraceLog("MobileAPI", $"{request.DeviceSerialNo}, {request.LaundryDate}");

            //        Dictionary<int, IncidentsTable> linenRoomBasedInsidents = new Dictionary<int, IncidentsTable>();
            //        //Use only date without time
            //        request.LaundryDate = request.LaundryDate.Date;

            //        foreach (var id in request.EmployeeUniformIDs)
            //        {
            //            //get the linen room ID of the uniform on the date
            //            var linenRoomID = (from b in _db.UniformTransactionTable
            //                               where b.EmployeeUniformID == id
            //                                   && b.ReturnedToEmployeeDate >= request.LaundryDate
            //                                   && b.ReturnedToEmployeeDate < request.LaundryDate.AddDays(1)
            //                               orderby b.ReturnedToEmployeeDate descending
            //                               select b.LinenRoomID).LastOrDefault();

            //            base.TraceLog("MobileAPI", $"Selected Linen Room ID : {linenRoomID}");

            //            IncidentsTable currentIncident = null;
            //            if (linenRoomBasedInsidents.ContainsKey(linenRoomID))
            //            {
            //                currentIncident = linenRoomBasedInsidents[linenRoomID];
            //            }
            //            else
            //            {
            //                //Create Incident
            //                currentIncident = new IncidentsTable();
            //                currentIncident.EmployeeID = Person.PersonID;
            //                currentIncident.IncidentsNo = CodeGenerationHelper.GetNextCode("Incidents");
            //                currentIncident.WashReturnDate = request.LaundryDate;
            //                currentIncident.LinenRoomID = linenRoomID;

            //                linenRoomBasedInsidents.Add(linenRoomID, currentIncident);
            //                _db.Add(currentIncident);
            //            }

            //            //Create line items
            //            IncidentsLineItemTable litem = new IncidentsLineItemTable();

            //            litem.Incidents = currentIncident;
            //            litem.EmployeeUniformID = id;
            //            litem.IncidentTypeID = request.IncidentTypeID;
            //            litem.Remarks = request.Remarks;

            //            _db.Add(litem);
            //        }

            //        //Add documents

            //        _db.SaveChanges();

            //        return Json(new BaseResponse()
            //        {
            //            Message = "Incident saved successfully"
            //        });
            //    }
            //    catch (Exception ex)
            //    {
            //        return ErrorActionResult<BaseResponse>(ex);
            //    }
            //}

            #endregion

            //#endregion
        }
    }