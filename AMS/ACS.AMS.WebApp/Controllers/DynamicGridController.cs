using ACS.AMS.WebApp.Models;
using ACS.AMS.DAL;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using ACS.AMS.DAL.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using ACS.WebAppPageGenerator.Models.SystemModels;

namespace ACS.AMS.WebApp.Controllers
{
    public class DynamicGridController : ACSBaseController
    {
        

        // GET: /DynamicGrid/
        public IActionResult Index(string controllerName,string pageName, bool Isdynamic)
        {
            base.TraceLog("DynamicGrid Index", $"{SessionUser.Current.Username} -Index requested.{controllerName},{pageName},{Isdynamic}");
            return PartialView("Index", new IndexPageModel()
            {
                PageTitle = pageName,
                PageName = pageName,
                ControllerName = controllerName,
                Isdynamic= Isdynamic

            });
        }

        public IActionResult ItemList(string controllerName, string pageName, bool Isdynamic)
        {
            base.TraceLog("DynamicGrid ItemList", $"{SessionUser.Current.Username} -ItemList requested.{controllerName},{pageName},{Isdynamic}");
            return PartialView("ItemList", new IndexPageModel()
            {
                PageTitle = pageName,
                PageName = pageName,
                ControllerName = controllerName,
                Isdynamic= Isdynamic
            });
        }

        [HttpPost]
        public IActionResult ItemList(IFormCollection data)
        {
            //string id = data["Columns"];
            try
            {
                base.TraceLog("DynamicGrid ItemList Post", $"{SessionUser.Current.Username} -ItemList requested.");

                string[] targetIndexName = data["Controller"].ToString().Split('/');//.Replace("Description", "");
                string controllerName = data["Controller"].ToString().Replace("Description", "");
                string pageName = data["PageNameIndex"].ToString();
                string[] Column = data["hdGridSelectedFieldItems"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] ColumnID = data["hdGridSelectedFieldItemsIDS"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                int[] gridColumns = ComboBoxHelper.StringToListDynamicGrid(data["hdGridSelectedFieldItemsIDS"], ',');
                int targetIndexID = MasterGridNewTable.GetTragetIndexID(_db, pageName);
                bool Isdynamic = bool.Parse(data["Isdynamic"]);
                UserGridNewColumnTable.DeleteExistingColumnForUser(_db, targetIndexID);
                int i = 1;
                gridColumns = gridColumns.Where(x => x.ToString() != "0").ToArray(); 
                foreach (int str in gridColumns)
                {
                    if (str == 0)
                    {
                        continue;
                    }
                    //if (UserGridTable.CheckExistingColumnForUser(_db, targetIndexID, str))
                    //{
                    UserGridNewColumnTable userGrid = new UserGridNewColumnTable();
                    userGrid.MasterGridLineItemID = str;
                    userGrid.MasterGridID = targetIndexID;
                    userGrid.UserID = _db.CurrentUserID;
                    userGrid.OrderID = i;
                    i++;
                    _db.Add(userGrid);
                    //}
                }

                _db.SaveChanges();
                DefaultCacheProvider settings = new DefaultCacheProvider();
                settings.Invalidate("AdvanceGridUserCache");
                string url1;
                if (!Isdynamic)
                    url1 = "/" + controllerName + "/Index";
                else
                    url1 = "/" + controllerName + "/Index?pageName=" + pageName;
                ViewData["CancelGridPopUp"] = true;
                ViewData["Url"] = url1;
                //TempData["IndexName"] = controllerName;
                return PartialView("ExceptionAction");
               // return new JavaScriptResult("CancelGridPopUp('" + controllerName + "')");

            }
            catch (ValidationException ex)
            {
                ViewData["FocusControl"] = ex.FieldName;
                ModelState.AddModelError("12", ex.Message);
                ViewData["Exception"] = true;
                ViewData["Message"] = ex.Message.ToString();
                return PartialView("ExceptionAction");
               
            }
            catch (Exception ex)
            {
                ApplicationErrorLogTable.SaveException(ex);
                ModelState.AddModelError("123", ex.Message);
                ViewData["Exception"] = true;
                ViewData["Message"] = ex.Message.ToString();
                return PartialView("ExceptionAction");
                
            }
        }

       
    }
}