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
    public class BarcodePrintingController : ACSBaseController
    {

        public ActionResult Index()
        {
            if (!base.HasRights(RightNames.BarcodePrinting, UserRightValue.View))
                return GotoUnauthorizedPage();
            IndexPageModel indexPage = new IndexPageModel();
            indexPage.EntityInstance = new BaseEntityObject();
            indexPage.EntityInstance.CurrentPageID = SessionUser.Current.GetNextPageID();
            base.TraceLog("Barcode Priniting Index", $"{SessionUser.Current.Username} -Barcode Printing Index page requested");
            return PartialView(indexPage);
        }

        public IActionResult _Index([DataSourceRequest] DataSourceRequest request, int currentPageID)
        {
            if (!base.HasRights(RightNames.BarcodePrinting, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");

            IQueryable<AssetNewView> query = AssetNewView.GetAllItems(_db).Where(a => /*a.LocationType != null &&*/ a.StatusID == (int)StatusValue.Active);
            var dsResult = query.ToDataSourceResult(request);
            base.TraceLog("asset Priniting Index", $"{SessionUser.Current.Username} -asset Priniting Index page Data Fetch");
            return Json(dsResult);
        }

        [HttpPost]
        public IActionResult Index(IFormCollection data)
        {
            if (!base.HasRights(RightNames.BarcodePrinting, UserRightValue.Create))
                return GotoUnauthorizedPage();
            try
            {
                base.TraceLog("Barcode printing Post", $"{SessionUser.Current.Username} -Barcode printing details call");
                var sampleCode = "";
                var dataToPrint = "";
                List<int> barcodeList = new List<int>();
                var check = data["ids[]"];
                var formatID = data["formatID"];

                sampleCode = BarcodeFormatsTable.ReadBarcodeString(_db, int.Parse(formatID + ""));
                string checkdItems = data["ids[]"];

                if ((string.IsNullOrEmpty(checkdItems) != true))
                {
                    string[] arrindate = checkdItems.Split(',');
                    arrindate = arrindate.Where(e => !string.IsNullOrEmpty(e)).Select(e => e).ToArray();
                    int[] itemID = Array.ConvertAll(arrindate, s => int.Parse(s));
                    if (itemID.Count() != 0)
                    {
                        for (int i = 0; i < itemID.Count(); i++)
                        {
                            int assetID = itemID[i];
                            var bar = (from b in _db.AssetTable where b.AssetID == assetID select b.AssetID).FirstOrDefault();
                            var barcode = bar;
                            barcodeList.Add(barcode);
                        }

                        var result = AssetTable.GetCheckBarcodeDetails(_db, barcodeList);
                        //var result = AssetTable.GetAllAssets().Where(a => barcodeList.Contains(a.AssetID));

                        List<AssetTable> res = result.ToList();
                        if (res.Count > 0)
                        {
                            for (int i = 0; i < res.Count; i++)
                            {
                                string sampleCodeCopy = sampleCode;
                                sampleCodeCopy = sampleCodeCopy.Replace("LOGOFORMAT", AppConfigurationManager.GetValue<string>(AppConfigurationManager.BarcodePrintLogoName)).Trim();
                                sampleCodeCopy = sampleCodeCopy.Replace("COMPANYNAME", SessionUser.Current.CompanyName);//AppConfigurationManager.GetValue<string>(AppConfigurationManager.CompanyName)
                                sampleCodeCopy = sampleCodeCopy.Replace("COMPANYCODE", CompanyTable.GetItem(_db, res[i].CompanyID.Value).CompanyCode);
                                int categoryID = res[i].CategoryID;
                                var details = CategoryTable.GetItem(_db, categoryID);

                                sampleCodeCopy = sampleCodeCopy.Replace("CATEGORYNAME", details.CategoryName);

                                var productDescription = ProductTable.GetItem(_db, res[i].ProductID);

                                sampleCodeCopy = sampleCodeCopy.Replace("ASSETDESCRIPTION", productDescription.ProductName);


                                if (!string.IsNullOrEmpty(res[i].ReferenceCode + ""))
                                    sampleCodeCopy = sampleCodeCopy.Replace("REFERENCECODE", res[i].ReferenceCode.ToString());
                                else
                                    sampleCodeCopy = sampleCodeCopy.Replace("REFERENCECODE", "");
                                if (!string.IsNullOrEmpty(res[i].SerialNo + ""))
                                    sampleCodeCopy = sampleCodeCopy.Replace("SERIALNO", res[i].SerialNo.ToString());
                                else
                                    sampleCodeCopy = sampleCodeCopy.Replace("SERIALNO", "");
                                string location = string.Empty;
                                if (res[i].LocationID.HasValue)
                                {
                                    int locationID = (int)res[i].LocationID;
                                    location = LocationTable.GetItem(_db, locationID).LocationName;
                                }
                                sampleCodeCopy = sampleCodeCopy.Replace("FULLL1LOCCODE", location);
                                sampleCodeCopy = sampleCodeCopy.Replace("BARCODEHUMAN", res[i].Barcode.ToString());
                                sampleCodeCopy = sampleCodeCopy.Replace("BARCODEDETAILS", res[i].Barcode.ToString());
                                if (res[i].PurchaseDate.HasValue)
                                {
                                    sampleCodeCopy = sampleCodeCopy.Replace("PURCHASEDATE", String.Format("{0:" + CultureHelper.ConfigureDateFormat + "}", res[i].PurchaseDate));
                                }

                                dataToPrint += sampleCodeCopy;
                            }

                            //  BarCodeFormatFile.WriteCartonLabelDataToFileAndPrint(_db, dataToPrint);
                            base.TraceLog("Barcode printing Post", $"{SessionUser.Current.Username} -Barcode printing details call done");
                            return Json(new { Result = "Success", rawData = dataToPrint, directprint = "0" });

                        }
                    }
                }

                else
                {
                    return base.ErrorActionResult(Language.GetString("SelectAnyBarcode"));
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