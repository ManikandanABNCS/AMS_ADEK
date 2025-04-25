using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ACS.AMS.DAL;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel
{
   public partial class LanguageContentLineItemTable
    {
        public static IQueryable<LanguageContentLineItemTable> GetAllLanguageContentLineItemTable(AMSContext db)
        {

            return db.LanguageContentLineItemTable.AsQueryable();
        }
       
        public static IQueryable<LanguageContentLineItemTable> GetLanguageContentDetails(AMSContext db, int languageContentID, int? languageID = null)
        {
            var result = from b in GetAllLanguageContentLineItemTable(db)
                         where b.LanguageContentID == languageContentID
                         select b;

            if (languageID.HasValue)
                result = result.Where(x => x.LanguageID == languageID.Value);
            return result;

        }

        public static LanguageContentLineItemTable GetLanguageContentDetail(AMSContext db, int languageContentID, int? languageID = null)
        {
            var result = from b in GetAllLanguageContentLineItemTable(db)
                         where b.LanguageContentID == languageContentID
                         select b;

            if (languageID.HasValue)
                result = result.Where(x => x.LanguageID == languageID.Value);
            return result.FirstOrDefault();

        }
        public static LanguageContentLineItemTable GetLanguageContentLineItem(AMSContext db, int languageContentID, int? languageID = null)
        {
            var result= db.LanguageContentLineItemTable.Where(b => b.LanguageContentID == languageContentID).AsQueryable();
            if (languageID.HasValue)
                result = result.Where(x => x.LanguageID == languageID.Value);
            return result.FirstOrDefault();

        }
    }
}
