
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
using Microsoft.AspNetCore.Hosting;
using static NPOI.POIFS.Crypt.Dsig.SignatureInfo;
using System.Net.Http.Headers;


namespace ACS.AMS.WebApp.Controllers
{
    public class ConfigurationController : ACSBaseController
    {
        private static IWebHostEnvironment WebHostEnvironment;
        public ConfigurationController(IWebHostEnvironment _environment)
        {
            WebHostEnvironment = _environment;
        }
        public IActionResult Edit()
        {
            if (!base.HasRights(RightNames.Configuration, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");
            List<ConfigurationCategoryNameModel> categoryList = ConfigurationTable.GetCategoryName(_db);
            IQueryable<ConfigurationTable> AllConfiguration = ConfigurationTable.GetAllConfiguration(_db);
            var truple = new Tuple<List<ConfigurationCategoryNameModel>, IQueryable<ConfigurationTable>>(categoryList, AllConfiguration);
            base.TraceLog("Configuration Edit", $"{SessionUser.Current.Username} -Configuration Edit data fetch ");
            return PartialView(truple);
        }
        [HttpPost]
        public IActionResult Edit(IFormCollection data)
        {
            if (!base.HasRights(RightNames.Configuration, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");
            try
            {
                if (ModelState.IsValid)
                {
                    base.TraceLog("Configuration Edit-post", $"{SessionUser.Current.Username} -Configuration Edit will save to db");
                    var configurations = ConfigurationTable.GetAllConfiguration(_db);
                    foreach (ConfigurationTable config in configurations)
                    {
                        string val = data["Config_" + config.ConfigurationID];


                        if (string.IsNullOrEmpty(val) && (string.Compare(config.DataType.ToUpper(), "STRING") == 0 ||
                            string.Compare(config.DataType.ToUpper(), "PASSWORD") == 0))
                        {
                            val = string.Empty;
                        }
                        else if (string.Compare(config.DataType.ToUpper(), "MULTISELECT") == 0)
                        {
                            if (string.IsNullOrEmpty(val))
                                //val = string.Empty;
                                val = "1";
                        }
                        else if (string.Compare(config.DataType.ToUpper(), "PICTURE") == 0)
                        {
                            if (string.IsNullOrEmpty(val))
                                val = string.Empty;
                        }
                        else if (string.IsNullOrEmpty(val) &&
                                (string.Compare(config.DataType.ToUpper(), "STRING") != 0 ||
                                string.Compare(config.DataType.ToUpper(), "PASSWORD") != 0
                                ))
                            continue;
                        if (val != null)
                            config.ConfiguarationValue = val;



                        switch (config.DataType.ToUpper())
                        {
                            //case "PICTURE":
                            //    var Logo = (HttpPostedFileBase)Session["ConfigurationImage"];
                            //    if (Logo != null)
                            //    {
                            //        try
                            //        {
                            //            if (config.ConfiguarationName.Trim() == "Logo")
                            //            {
                            //                var fileName = Path.GetFileName(Logo.FileName);
                            //                BinaryReader binary = new BinaryReader(Logo.InputStream);
                            //                int length = Convert.ToInt32(Logo.InputStream.Length);
                            //                byte[] logoByte = binary.ReadBytes(length);
                            //                config.Images = logoByte;
                            //                var physicalPath = Path.Combine(Server.MapPath("~/Content/CompanyLogo"), "Fasoftlogo.jpg");
                            //                if (System.IO.File.Exists(physicalPath))
                            //                {
                            //                    System.IO.File.Delete(physicalPath);
                            //                }

                            //                Logo.SaveAs(physicalPath);
                            //                config.ConfiguarationValue = "/Content/CompanyLogo/Fasoftlogo.jpg";
                            //            }

                            //        }
                            //        catch (Exception ex)
                            //        {
                            //            AppErrorLogTable.SaveException(_db, ex, base.HttpContext.Request.Url.ToString());
                            //        }
                            //    }
                            //    break;
                            case "PASSWORD":
                                //if (data["Config_" + config.ConfigurationID] != data["Config_" + config.ConfigurationID + "_Confirm"])
                                //{
                                //    ViewData["Confirmpassword"] = data["Config_" + config.ConfigurationID + "_Confirm"];

                                //    ModelState.AddModelError("123", "Invalid password and confirm password");
                                //    return PartialView(configurations);
                                //}
                                //else
                                    config.ConfiguarationValue = val;
                                break;
                            case "BOOLEAN":
                                if (val.Contains(','))
                                    val = val.Substring(0, val.IndexOf(','));

                                config.ConfiguarationValue = val;
                                break;

                        }
                    }
                    _db.SaveChanges();
                    base.TraceLog("Configuration Edit-post", $"{SessionUser.Current.Username} -Configuration Edit saved to db");
                    return PartialView("ConfigurationControls/ConfigurationLogoutAction");
                }
            }
            catch (Exception ex)
            {
                return base.ErrorActionResult(ex);
            }
            return PartialView(data);
        }

        [HttpPost]
        public async Task<ActionResult> SaveConfigurationImages(IFormFile attachments)
        {
            base.TraceLog("Configuration SaveConfigurationImages", $"{SessionUser.Current.Username} -SaveConfigurationImages Edit will save to db");
            string fullPath = Path.Combine(WebHostEnvironment.WebRootPath, "FileStoragePath/CompanyLogo");
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            if (attachments != null)
            {
                var fileContent = ContentDispositionHeaderValue.Parse(attachments.ContentDisposition);
                string fileExtension = System.IO.Path.GetExtension(Path.GetFileName(fileContent.FileName.ToString().Trim('"')));
                string time = ComboBoxHelper.RemoveSpecialCharacters(String.Format("{0:" + CultureHelper.DateTimeFormatForGrid + "}", DateTime.Now));
                string fileName = System.IO.Path.GetFileNameWithoutExtension(fileContent.FileName.ToString().Trim('"'));
                string newFileName = fileName + "" + time + "" + fileExtension;
                fullPath = Path.Combine(fullPath, newFileName);

                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await attachments.CopyToAsync(fileStream);
                }
                TempData["UploadedFile"] = fullPath;
            }
            //    Session["ConfigurationImage"] = attachments;
            //var fileName = Path.GetFileName(attachments.FileName);
            //var physicalPath = Path.Combine(Server.MapPath("~/Content/CompanyLogo"), fileName);
            //if (System.IO.File.Exists(physicalPath))
            //{
            //    System.IO.File.Delete(physicalPath);
            //}

            //attachments.SaveAs(physicalPath);
            //return Json(new { ImageName = "/Content/CompanyLogo/" + fileName }, "text/html", JsonRequestBehavior.AllowGet);
            base.TraceLog("Configuration SaveConfigurationImages", $"{SessionUser.Current.Username} -SaveConfigurationImages saved to db");
            return Json(Content(""));
        }

        //[HttpPost]
        //public IActionResult Remove(string[] fileNames)
        //{
        //    foreach (var fullName in fileNames)
        //    {
        //        Session["ConfigurationImage"] = null;
        //        var fileName = Path.GetFileName(fullName);
        //        var physicalPath = Path.Combine(Server.MapPath("~/Content/CompanyLogo"), fileName);
        //        if (System.IO.File.Exists(physicalPath))
        //        {
        //            System.IO.File.Delete(physicalPath);
        //        }
        //    }
        //    return Content("");
        //}
    }
}
