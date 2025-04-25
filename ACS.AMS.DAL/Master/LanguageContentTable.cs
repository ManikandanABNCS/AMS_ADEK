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
   public partial class LanguageContentTable
    {
        public static IQueryable<LanguageContentTable> GetAllLanguageContentTable(AMSContext db)
        {

            return db.LanguageContentTable.Include("LanguageContentLineItemTable").AsQueryable();
        }
        public static LanguageContentTable GetLanguageContent(AMSContext db, int languageContentID)
        {
            return db.LanguageContentTable.Where(c => c.LanguageContentID == languageContentID).FirstOrDefault();
              
        }
    }
}
