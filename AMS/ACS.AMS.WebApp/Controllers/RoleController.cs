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

namespace ACS.AMS.WebApp.Controllers
{
    public class RoleController : ACSBaseController
    {
        public IActionResult _Index([DataSourceRequest] DataSourceRequest request)
        {
            if (!base.HasRights(RightNames.RoleMaster, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");
            var query = User_RoleTable.GetAllRole(_db).Where(c => c.IsActive ==true && c.IsDeleted==false);

            var rl = from b in query
                     select new RolesModel
                     {
                         RoleID = b.RoleID,
                         RoleName = b.RoleName,
                         DisplayRole = (bool)b.DisplayRole,
                         Description = b.Description,
                         IsActive = b.IsActive
                     };

            var dsResult = rl.ToDataSourceResult(request);
            base.TraceLog("RoleMaster Index", $"{SessionUser.Current.Username} -RoleMaster Index page Data Fetch");
            return Json(dsResult);
        }

        public IActionResult Index()
        {
            if (!base.HasRights(RightNames.RoleMaster, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("RoleMaster Index", $"{SessionUser.Current.Username} -RoleMaster Index page requested");
            return PartialView();
        }

        public IActionResult Create()
        {
            if (!base.HasRights(RightNames.RoleMaster, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("RoleMaster Create", $"{SessionUser.Current.Username} -RoleMaster Create page requested");
            User_RoleTable Role = new User_RoleTable();
            Role.Description = "none";
            
            return PartialView(Role);

        }
        [HttpPost]
        public IActionResult Create(User_RoleTable item, IFormCollection data)
        {
            if (!base.HasRights(RightNames.RoleMaster, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");
            try
            {
                base.TraceLog("RoleMaster Create Post", $"{SessionUser.Current.Username} -RoleMaster details will add to db ");
                if (ModelState.IsValid)
                {
                    item.Description = item.RoleName;
                    item.IsActive = true;
                    item.IsDeleted = false;
                    item.ApplicationID = 1;
                    item.DisplayRole = true;
                    _db.Add(item);
                    _db.SaveChanges();
                    base.TraceLog("RoleMaster Create Post", $"{SessionUser.Current.Username} -RoleMaster details saved to db ");
                    return PartialView("SuccessAction");
                }
            }
            catch(Exception ex)
            {
                return ErrorActionResult(ex);
            }
            return PartialView(item);
        }
        public IActionResult Edit(int id)
        {
            if (!base.HasRights(RightNames.RoleMaster, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("RoleMaster Edit", $"{SessionUser.Current.Username} -RoleMaster Edit page requested ");
            var role = User_RoleTable.GetRoleDetails(_db, id);
            base.TraceLog("RoleMaster Edit", $"RoleMaster Edit page requested RoleMasterID {id}");
            return PartialView(role);
        }

        [HttpPost]
        public IActionResult Edit(User_RoleTable item, IFormCollection data)
        {
            if (!base.HasRights(RightNames.RoleMaster, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");
            try
            {

                if (ModelState.IsValid)
                {
                    base.TraceLog("RoleMaster Edit Post", $"RoleMaster details will modify to db : RoleMaster id- {item.RoleID}");
                    var OldRole = User_RoleTable.GetRoleDetails(_db, item.RoleID);
                    OldRole.RoleName = item.RoleName;
                    OldRole.Description = item.Description;
                    OldRole.IsActive = item.IsActive;
                    OldRole.IsDeleted = item.IsDeleted;
                    _db.SaveChanges();
                    base.TraceLog("RoleMaster Edit Post", $"RoleMaster details modified to db : RoleMaster id- {item.RoleID}");
                    return PartialView("SuccessAction");
                }
                }
            catch(Exception ex)
            {
                return ErrorActionResult(ex);
            }
            return PartialView(item);
        }

        public IActionResult Details(int id)
        {
            if (!base.HasRights(RightNames.RoleMaster, UserRightValue.Details))
                return RedirectToAction("UnauthorizedPage");
              base.TraceLog("RoleMaster Details", $"{SessionUser.Current.Username} -RoleMaster Detail Page requested");
            var role = User_RoleTable.GetRoleDetails(_db, id);
            return PartialView(role);
        }
		public IActionResult Delete(int id)
		{
			

			if(!base.HasRights(RightNames.RoleMaster, UserRightValue.Delete))
				return RedirectToAction("UnauthorizedPage");
			base.TraceLog("RoleMaster Delete", $"{SessionUser.Current.Username} -RoleMaster Detail Page requested");

			var role = User_RoleTable.GetRoleDetails(_db, id);
            role.IsDeleted = true;
			_db.SaveChanges();

			
			return PartialView("SuccessAction");
		}
        public IActionResult DeleteAll(string toBeDeleteIds)
        {
            var pageName = "Role";

            base.TraceLog("DeleteAll", $"{SessionUser.Current.Username} - {pageName} Delete action requested");
            if (!base.HasRights(RightNames.UserMaster, UserRightValue.Delete))
                return RedirectToAction("UnauthorizedPage");
            
            string checkdItems = toBeDeleteIds;
            string[] arrRequestID = checkdItems.Split(',');
            int[] requestID = Array.ConvertAll(arrRequestID, s => int.Parse(s));
            int requestCount = requestID.Count();
            if (requestCount > 0)
            {
                foreach (var id in requestID)
                {
                    var role = User_RoleTable.GetRoleDetails(_db, id);
                    role.IsDeleted = true;
                    _db.SaveChanges();
                }
            }
            base.TraceLog("Delete", $"{pageName} details page deleted successfully");
            return RedirectToAction("SuccessAction", new { pageName = pageName });
        }
    }
}
