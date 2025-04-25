using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ACS.AMS.DAL;
namespace ACS.AMS.DAL.DBModel
{
   public partial class LanguageTable
    {
        public static IQueryable<LanguageTable> GetAllLanguageTable(AMSContext db)
        {
            return db.LanguageTable.AsQueryable();
        }
    }
}
