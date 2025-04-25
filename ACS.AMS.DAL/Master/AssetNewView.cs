using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using ACS.AMS.DAL.Models;

namespace ACS.AMS.DAL.DBModel
{
    public partial class AssetNewView
    {
        public static IQueryable<AssetNewView> GetAllUserItem(AMSContext _db, int userID, bool includeInactiveItems = false)
        {
            var userCompanyIDs = UserCompanyMappingTable.GetCompanyForPersonID(_db, SessionUserHelper.UserID);
            IQueryable<AssetNewView> result = from b in _db.AssetNewView
                                              join d in userCompanyIDs on b.CompanyID equals d.CompanyID
                                              where b.StatusID != (int)StatusValue.Rejected && (b.StatusID!=(int)StatusValue.Deleted && b.StatusID!=(int)StatusValue.DeletedOLD)
                                             
                                              select b;

            //if user department mapping option enable 
            if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.UserDepartmentMapping))
            {
                var currentDepartID = UserCompanyMappingTable.GetDepartmentForPersonID(_db, SessionUserHelper.UserID);
                if (currentDepartID.Count() > 0)
                {
                    //List<int> departmentID = currentDepartID.Select(a => a.DepartmentID).ToList();
                    //result = result.Where(b => b.DepartmentID.HasValue == true 
                    //&& departmentID.Contains((int)b.DepartmentID));
                    result = (from b in result
                              join d in currentDepartID on b.DepartmentID equals d.DepartmentID
                              where b.DepartmentID.HasValue == true
                              select b
                            );
                }
            }

            // if user parent Category Mapping enable in config,get the all child category and load the grid 
            if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.UserCategoryMapping))
            {
                var currentUserCategoryIDs = UserCompanyMappingTable.GetCategoryForPersonID(_db, SessionUserHelper.UserID);
                if (currentUserCategoryIDs.Count() > 0)
                {
                    //result = result.Where(b => (from c in currentUserCategoryIDs select c.CategoryID).Contains(b.MappedCategoryID));
                    result = from b in result
                             join d in currentUserCategoryIDs on b.MappedCategoryID equals d.CategoryID
                             select b;
                }
            }

            // if user parent Location Mapping enable in config,get the all child Location and load the grid 
            if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.UserLocationMapping))
            {
                var currentLocationIDs = UserCompanyMappingTable.GetLocationForPersonID(_db, SessionUserHelper.UserID);
                if (currentLocationIDs.Count() > 0)
                {
                    //List<string> mappedLocationID = currentLocationIDs.Select(a => a.LocationID + "").ToList();
                    //result = result.Where(b => (from c in currentLocationIDs select c.LocationID).Contains(b.MappedLocationID) );
                    result = from b in result
                             join d in currentLocationIDs on b.MappedLocationID equals d.LocationID
                             select b;
                }
            }

            return result.OrderByDescending(x => x.AssetID);
        }
        public static IQueryable<AssetNewView> GetAssetListAgainstPO(AMSContext db, string ponumber, string invoiceno, string polineno)
        {
            var query = (from b in db.AssetNewView
                         where b.PONumber == ponumber &&
                           b.InvoiceNo == invoiceno && b.DOFPO_LINE_NUM == polineno
                           && b.PONumber != null && b.InvoiceNo != null && b.DOFPO_LINE_NUM != null
                         select b);
            return query;

                       
        }
    }
}