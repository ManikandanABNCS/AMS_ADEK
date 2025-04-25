using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACS.AMS.DAL;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using ACS.WebAppPageGenerator.Models.SystemModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ACS.AMS.WebApp.Controllers
{
    public class DashboardController : ACSBaseController
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        //public async Task<IActionResult> Dashboard()
        //{
        //    return PartialView();
        //}
        public IActionResult Index(string pageURL)
        {
            if (string.IsNullOrEmpty(pageURL) == false)
                ViewBag.PageURL = pageURL;
            base.TraceLog("Dashboard Index", $"{SessionUser.Current.Username} - call");

            return View();
        }

        public async Task<IActionResult> Dashboard(int? divID, int? locID, string pageURL) 
		{
            var rightName = "Dashboard";
            base.TraceLog("Dashboard Dashboard", $"{SessionUser.Current.Username} - call");

            DashboardPageModel indexPage = new DashboardPageModel();
            indexPage.PageName = rightName;
            indexPage.PageTitle = rightName;
            indexPage.CurrentPageID = SessionUser.Current.GetNextPageID();
            //indexPage.EntityInstance = new BaseEntityObject()
            //{
            //    CurrentPageID = SessionUser.Current.GetNextPageID(),
            //};

            if (string.IsNullOrEmpty(pageURL))
            {
                return PartialView(indexPage);
            }
            else
            {
                ViewBag.PageURL = pageURL;

                return PartialView(indexPage);
            }
		}
    }
}
