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
    public class SearchByPhotoController : ACSBaseController
    {
        private static IWebHostEnvironment WebHostEnvironment;
        public SearchByPhotoController(IWebHostEnvironment _environment)
        {
            WebHostEnvironment = _environment;
            Thread.CurrentThread.CurrentUICulture =
           Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-GB");
        }
        public IActionResult Index()
        {
            if (!base.HasRights(RightNames.SearchByPhoto, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");

            IndexPageModel indexPageModel = new IndexPageModel();
            base.TraceLog("SearchByPhoto Index", $"{SessionUser.Current.Username} -SearchByPhoto Index page requested");
            return PartialView(indexPageModel);
        }

        public JsonResult _TreeIndex([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                base.TraceLog("_TreeIndex", $"{SessionUser.Current.Username} - _TreeIndex  requested");

                var data = CategoryListView.GetAllItems(_db);
                var result = data.ToList();
                var list = result.ToTreeDataSourceResult(request, e => e.CategoryID, e => e.ParentCategoryID, e => e);

                return Json(list);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ex);
            }
        }


       
    }
}