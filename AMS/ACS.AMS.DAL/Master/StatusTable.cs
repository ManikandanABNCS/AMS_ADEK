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
   public partial class StatusTable
    {
        public static IQueryable<StatusTable> GetAllStatuss(AMSContext db, bool includeInactiveItems = false)
        {
            return from b in db.StatusTable
                   where b.StatusID != (int)StatusValue.Deleted && b.StatusID != (int)StatusValue.DeletedOLD  && (includeInactiveItems || b.StatusID != (int)StatusValue.Inactive)
                   orderby b.Status descending
                   select b;
        }

        public static IQueryable<StatusTable> GetAllStatusesIncludingDeleted(AMSContext db)
        {
            return from b in db.StatusTable
                   orderby b.StatusID
                   select b;
        }
    }
}
