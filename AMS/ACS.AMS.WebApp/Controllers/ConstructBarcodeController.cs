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
    public class ConstructBarcodeController : ACSBaseController
    {
       
        public ActionResult Index()
        {
            if (!base.HasRights(RightNames.ConstructBarcode, UserRightValue.Create))
                return GotoUnauthorizedPage();

            base.TraceLog("ConstructBarcode Index", $"{SessionUser.Current.Username} -Index page called");
            BarcodeConstructTable item = BarcodeConstructTable.GetAllItems(_db).SingleOrDefault();
            if (item != null)
            {
                return PartialView(item);
            }
            else
            {
                BarcodeConstructTable barcode = new BarcodeConstructTable();
                return PartialView(barcode);
            }
        }
       
        [HttpPost]
        public IActionResult Index(BarcodeConstructTable item, IFormCollection data)
        {
            if (!base.HasRights(RightNames.ConstructBarcode, UserRightValue.Create))
                return GotoUnauthorizedPage();
            try
            {
                base.TraceLog("ConstructBarcode Index-post", $"{SessionUser.Current.Username} -barcode construct details will save to db");
                if (ModelState.IsValid)
                {
                    var result = BarcodeConstructTable.GetAllItems(_db);
                    if (result.Count() > 0)
                    {
                        foreach (var res in result.ToList())
                        {
                            _db.BarcodeConstructTable.Remove(res);

                            _db.SaveChanges();
                        }
                    }

                    _db.Add(item);
                    _db.SaveChanges();
                    base.TraceLog("ConstructBarcode Index-post", $"{SessionUser.Current.Username} -barcode construct details saved to db");
                }
                return PartialView("SuccessAction");
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
            return PartialView(item);
        }

        [HttpPost]
        public IActionResult BarcodePrint(IFormCollection data)
        {
            try
            {
                base.TraceLog("ConstructBarcode BarcodePrint", $"{SessionUser.Current.Username} -BarcodePrint call");
                var sampleCode = "";
                var dataToPrint = "";
                int barcodeCheck = 0;
                List<string> barcodeList = new List<string>();
                var check = data["fromAsset"];
                var noofAsset = data["toAsset"];
                var formatID = data["formatID"];
                var categoryCode = data["Categorycode"];
                var location = data["locationCode"];
                var deptCode = data["deptCode"];
                var sectcode = data["sectCode"];
                var barcodeSep = AppConfigurationManager.GetValue<string>(AppConfigurationManager.BarcodeSeperator);
                int barcodeLen = AppConfigurationManager.GetValue<int>(AppConfigurationManager.BarcodeNumericLength);
                sampleCode = BarcodeFormatsTable.ReadBarcodeString(_db, int.Parse(formatID + ""));

                BarcodeConstructTable item = BarcodeConstructTable.GetAllItems(_db).SingleOrDefault();
                bool deptCustMapping = AppConfigurationManager.GetValue<bool>(AppConfigurationManager.DepartmentCustodianMapping);
                var prefix = string.Empty;
                if (item != null)
                {
                    if (!string.IsNullOrEmpty(item.CategoryCode))
                    {
                        prefix = item.CategoryCode + barcodeSep;
                    }
                    if (!string.IsNullOrEmpty(item.LocationCode))
                    {
                        if (!string.IsNullOrEmpty(prefix))
                        {
                            prefix = prefix + item.CategoryCode + barcodeSep;
                        }
                        else
                        {
                            prefix = item.CategoryCode + barcodeSep;
                        }
                    }
                    if (!string.IsNullOrEmpty(item.DepartmentCode))
                    {
                        if (!string.IsNullOrEmpty(prefix))
                        {
                            prefix = prefix + item.DepartmentCode + barcodeSep;
                        }
                        else
                        {
                            prefix = item.DepartmentCode + barcodeSep;
                        }
                    }
                    if (!string.IsNullOrEmpty(item.SectionCode))
                    {
                        if (!string.IsNullOrEmpty(prefix))
                        {
                            prefix = prefix + item.SectionCode + barcodeSep;
                        }
                        else
                        {
                            prefix = item.SectionCode + barcodeSep;
                        }
                    }
                    if (!string.IsNullOrEmpty(item.CustomPrefix))
                    {
                        if (!string.IsNullOrEmpty(prefix))
                        {
                            prefix = prefix + item.CustomPrefix + barcodeSep;
                        }
                        else
                        {
                            prefix = item.CustomPrefix + barcodeSep;
                        }
                    }
                }


                //  prefix =  + item.LocationCode + barcodeSep + item.DepartmentCode + barcodeSep + item.SectionCode + barcodeSep;
                if (!string.IsNullOrEmpty(noofAsset))
                {
                    for (int i = 0; i < int.Parse(noofAsset); i++)
                    {
                        barcodeCheck = int.Parse(check) + i;
                        string sampleCodeCopy = sampleCode;
                        sampleCodeCopy = sampleCodeCopy.Replace("COMPANYNAME", SessionUser.Current.CompanyName);//AppConfigurationManager.GetValue<string>(AppConfigurationManager.CompanyName)
                        sampleCodeCopy = sampleCodeCopy.Replace("ASSETDESCRIPTION", string.Empty);
                        sampleCodeCopy = sampleCodeCopy.Replace("FULLL1LOCCODE", string.Empty);
                        sampleCodeCopy = sampleCodeCopy.Replace("BARCODEHUMAN", prefix + barcodeCheck.ToString().PadLeft(barcodeLen, '0'));
                        sampleCodeCopy = sampleCodeCopy.Replace("BARCODEDETAILS", prefix + barcodeCheck.ToString().PadLeft(barcodeLen, '0'));
                        dataToPrint += sampleCodeCopy;
                    }

                    BarcodeConstructSequenceTable sequences = new BarcodeConstructSequenceTable();
                    if (!string.IsNullOrEmpty(dataToPrint))
                    {
                        var checkCode = BarcodeConstructTable.CompareData(_db, item == null ? null : string.IsNullOrEmpty(item.CategoryCode) ? null : item.CategoryCode,
                                            item == null ? null : string.IsNullOrEmpty(item.LocationCode) ? null : item.LocationCode, item == null ? null : string.IsNullOrEmpty(item.DepartmentCode) ? null : item.DepartmentCode,
                                            item == null ? null : string.IsNullOrEmpty(item.SectionCode) ? null : item.SectionCode, item == null ? null : string.IsNullOrEmpty(item.CustomPrefix) ? null : item.CustomPrefix);
                        if (checkCode != null)
                        {
                            if (item != null)
                            {
                                BarcodeConstructSequenceTable oldItem = BarcodeConstructTable.CompareData(_db, item.CategoryCode, item.LocationCode, item.DepartmentCode, item.SectionCode, item.CustomPrefix);
                                oldItem.LastSeqNo += int.Parse(noofAsset);
                                // _db.Add(oldItem);
                            }
                            else
                            {
                                BarcodeConstructSequenceTable oldItem = BarcodeConstructTable.CompareData(_db, null, null, null, null, null);
                                oldItem.LastSeqNo += int.Parse(noofAsset);
                                _db.Add(oldItem);
                            }
                        }
                        else
                        {
                            sequences.CategoryCode = item == null ? string.Empty : string.IsNullOrEmpty(item.CategoryCode) ? string.Empty : item.CategoryCode;
                            sequences.Locationcode = item == null ? string.Empty : string.IsNullOrEmpty(item.LocationCode) ? string.Empty : item.LocationCode;
                            sequences.DepartmentCode = item == null ? string.Empty : string.IsNullOrEmpty(item.DepartmentCode) ? string.Empty : item.DepartmentCode;
                            sequences.SectionCode = item == null ? string.Empty : string.IsNullOrEmpty(item.SectionCode) ? string.Empty : item.SectionCode;
                            sequences.CustomCode = item == null ? string.Empty : string.IsNullOrEmpty(item.CustomPrefix) ? string.Empty : item.CustomPrefix;
                            sequences.LastSeqNo = int.Parse(noofAsset);
                            _db.Add(sequences);
                        }
                        _db.SaveChanges();
                    }
                    base.TraceLog("ConstructBarcode BarcodePrint", $"{SessionUser.Current.Username} -BarcodePrint call done");
                    return Json(new { Result = "Success", rawData = dataToPrint, directprint = "0" }, new Newtonsoft.Json.JsonSerializerSettings());

                    //return Json(dataToPrint, JsonRequestBehavior.AllowGet);
                }

                return PartialView();
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
            return PartialView(data);
        }
        }
}
