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
    public class SearchByVirtualBarcodeController : ACSBaseController
    {
        private static IWebHostEnvironment WebHostEnvironment;
        public SearchByVirtualBarcodeController(IWebHostEnvironment _environment)
        {
            WebHostEnvironment = _environment;
            Thread.CurrentThread.CurrentUICulture =
           Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-GB");
        }
        public IActionResult Index()
        {
            if (!base.HasRights(RightNames.SearchByVirtualBarcode, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");

            Type entityType = GetEntityObjectType("SearchByVirtualBarcode", true);
            var obj = Activator.CreateInstance(entityType) as BaseEntityObject;
            SystemDatabaseHelper.GenerateMasterGridColumns(entityType, "SearchByVirtualBarcode");
            return PartialView("Index",
                  new IndexPageModel()
                  {
                      PageTitle = "SearchByVirtualBarcode",
                      PageName = "SearchByVirtualBarcode",
                      EntityInstance = obj,
                      ControllerName = this.GetType().Name.Replace("Controller", ""),
                  });
            base.TraceLog("SearchByVirtualBarcoder Index", $"{SessionUser.Current.Username} -SearchByVirtualBarcode Index page requested");
        
        }

        public virtual IActionResult _Index([DataSourceRequest] DataSourceRequest request, string pageName)
        {
            try
            {
                pageName = "SearchByVirtualBarcode";
                var rightName = pageName;
                if (!this.HasRights(rightName, UserRightValue.View))
                    return RedirectToAction("UnauthorizedPage");

                //get the 
                Type languageEntityType = null;// EntityHelper.GetLanguageTable(pageName);
                Type entityType = GetEntityObjectType(pageName, true);

                IACSDBObject languageObj = null;
                if (languageEntityType != null)
                    languageObj = Activator.CreateInstance(languageEntityType) as IACSDBObject;
                var obj = Activator.CreateInstance(entityType) as IACSDBObject;

                if (languageObj == null) languageObj = obj;

                var baseEntityInstance = obj as BaseEntityObject;

                //fetch the records from language table

                if (string.Compare(pageName, "Asset") == 0)
                {
                    //_db.EnableInstanceQueryLog = true;

                    var query = AssetNewView.GetAllUserItem(_db, SessionUser.Current.UserID);
                    var dsResult = request.ToDataSourceResultForAsset(query, pageName, baseEntityInstance.GetPrimaryKeyFieldName());

                    //var totalRecs = query.Take(request.PageSize).ToList();
                    //_db.EnableInstanceQueryLog = false;

                    this.TraceLog("Index", $"{SessionUser.Current.Username} - {pageName} Index page Data Fetch");

                    return Json(dsResult);
                }
                else
                {
                    var query = languageObj.GetAllUserItems(_db, SessionUser.Current.UserID);
                    var dsResult = request.ToDataSourceResult(query, pageName, baseEntityInstance.GetPrimaryKeyFieldName());
                    this.TraceLog("Index", $"{SessionUser.Current.Username} - {pageName} Index page Data Fetch");

                    return Json(dsResult);
                }
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }

        public IActionResult Details(int id)
        {
            if (!base.HasRights(RightNames.SearchByVirtualBarcode, UserRightValue.Details))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("SearchByVirtualBarcode Details", $"{SessionUser.Current.Username} -SearchByVirtualBarcode Detail Page requested");
            try
            {
                ProductTable product = ProductTable.GetProductDetails(_db, id);
                Type entityType = GetEntityObjectType("VirtualBarcodeMappedAsset", true);
                var obj = Activator.CreateInstance(entityType) as BaseEntityObject;
                SystemDatabaseHelper.GenerateMasterGridColumns(entityType, "VirtualBarcodeMappedAsset");
                return PartialView(product);
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }
        public IActionResult _LineItemMappedAsset([DataSourceRequest] DataSourceRequest request, int productID)
        {
            try
            {
                base.TraceLog("_LineItemMappedAsset", $"{SessionUser.Current.Username} - _LineItemMappedAsset  requested");

                var data = AssetNewView.GetAllItems(_db).Where(a=>a.ProductID==productID);
                var dsResult = request.ToDataSourceResult(data, "VirtualBarcodeMappedAsset", "AssetID");
              

                return Json(dsResult);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ex);
            }
        }
    }
}