using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ACS.AMS.DAL.DBModel
{    
    public partial class ScreenFilterTypeTable : BaseEntityObject
    {
        //public static IQueryable<ScreenFilterTypeTable> GetAllScreenFilterTypes(AMSContext db, bool includeInactiveItems = false)
        //{
        //    return from b in db.ScreenFilterTypeTable
        //           select b;
        //}
        public static string GetScreenFilterTypeName(AMSContext db, byte ScreenFilterTypeID)
        {
            if (db == null)
            {
                db = AMSContext.CreateNewContext();
            }

            return (from b in db.ScreenFilterTypeTable
                    where b.ScreenFilterTypeID == ScreenFilterTypeID
                    select b.ScreenFilterTypeName).FirstOrDefault();
        }
    }
}

