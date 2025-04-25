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
    public partial class CategoryDescriptionTable:BaseEntityObject
    {
        public static IQueryable<CategoryDescriptionTable> GetAllCategoryDescriptions(AMSContext db, bool includeInactiveItems = false)
        {
            return from b in db.CategoryDescriptionTable.Include("CategoryTable")
                   where b.LanguageID == 1 && b.Category.StatusID != (int)StatusValue.Deleted && (includeInactiveItems == false ? b.Category.StatusID != (int)StatusValue.Inactive : b.Category.StatusID != (int)StatusValue.Deleted)
                   select b;
            
        }
        public static List<CategoryDescriptionModel> GetItemsForViewing(IQueryable<CategoryTable> query,
                                                           string text = null)
        {
            var query2 = from b in query
                         select new
                         {
                             EntityID = b.CategoryID,
                             EntityCode = b.CategoryCode,
                             EntityDescription = b.CategoryName
                         };

            var itms = (from b in query2.ToList()
                        select new CategoryDescriptionModel
                        {
                            CategoryID = b.EntityID,
                            CategoryName = b.EntityDescription + " (" + b.EntityCode + ")"//string.Format( b.EntityCode, b.EntityDescription)
                        }).OrderBy(b => b.CategoryName);

            return itms.ToList();
        }
    }
}
