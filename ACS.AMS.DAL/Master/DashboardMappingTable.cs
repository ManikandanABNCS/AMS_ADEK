using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.AMS.DAL.DBModel
{
    public partial class DashboardMappingTable :BaseEntityObject
    {
     

        public static DashboardMappingTable GetMapping(AMSContext db, int dashboardMappingID)
        {
            var result = (from b in db.DashboardMappingTable.Include("DashboardType").Include("DashboardTemplate")
                          where b.DashboardMappingID == dashboardMappingID
                                && b.StatusID==(int)StatusValue.Active
                          select b).FirstOrDefault();

            return result;
        }
        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            return true;
        }
        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            return true;
        }

        public static IQueryable<DashboardMappingTable> GetAllActivItem(AMSContext db)
        {
            var result = (from b in db.DashboardMappingTable.Include("DashboardTemplate").Include("DashboardType").Include("Status")
                          where b.StatusID == (int)StatusValue.Active  select b );
            return result.OrderBy(a => a.DashboardTypeID);
        }
    }
}