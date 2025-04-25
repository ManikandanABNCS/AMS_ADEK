using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using ACS.AMS.DAL.Models;

namespace ACS.AMS.DAL.DBModel
{
    public partial class LocationDescriptionTable:BaseEntityObject
    {
        public static IQueryable<LocationDescriptionTable> GetAllLocationDescriptions(AMSContext db, bool includeInactiveItems = false)
        {
            return from b in db.LocationDescriptionTable.Include("LocationTable")
                   where b.LanguageID == 1 && b.Location.StatusID != (int)StatusValue.Deleted && b.Location.StatusID != (int)StatusValue.DeletedOLD && (includeInactiveItems == false ? b.Location.StatusID != (int)StatusValue.Inactive : (b.Location.StatusID != (int)StatusValue.Deleted &&  b.Location.StatusID != (int)StatusValue.DeletedOLD))
                   select b;
            
        }
        public static List<LocationDescriptionModel> GetItemsForViewing(IQueryable<LocationTable> query,
                                                           string text = null)
        {
            var query2 = from b in query
                         select new
                         {
                             EntityID = b.LocationID,
                             EntityCode = b.LocationCode,
                             EntityDescription = b.LocationName
                         };

            var itms = (from b in query2.ToList()
                        select new LocationDescriptionModel
                        {
                            LocationID = b.EntityID,
                            LocationName = b.EntityDescription + " (" + b.EntityCode + ")"//string.Format( b.EntityCode, b.EntityDescription)
                        }).OrderBy(b => b.LocationName);

            return itms.ToList();
        }
    }
}
