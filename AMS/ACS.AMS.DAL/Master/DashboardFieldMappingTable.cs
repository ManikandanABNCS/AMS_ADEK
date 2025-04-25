using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.AMS.DAL.DBModel
{
    public partial class DashboardFieldMappingTable : BaseEntityObject
    {
     

        public static IQueryable<DashboardFieldMappingTable> GetMappingDetails(AMSContext db, int dashboardMappingID)
        {
            var result = (from b in db.DashboardFieldMappingTable.Include("DashboardMapping").Include("DashboardTemplateField")
                          where b.DashboardMappingID == dashboardMappingID
                                && b.StatusID == (int)StatusValue.Active
                          select b);
            return result;
        }
        public static void DeleteExistingMappingItem(AMSContext db, IQueryable<DashboardFieldMappingTable> lineItem)
        {
            foreach (DashboardFieldMappingTable item in lineItem)
            {
                item.StatusID = (int)StatusValue.Deleted;
              //  db.Remove(item);
                //db.SaveChanges();
            }

        }
        public static IQueryable<DashboardFieldMappingTable> GetColumnLineItemFromFieldTable(AMSContext db, int dashboardMappingID)
        {
            var result = from b in db.DashboardFieldMappingTable
                         where b.DashboardMappingID == dashboardMappingID
                         && b.StatusID == (int)StatusValue.Active
                         select b;
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

        public static IQueryable<DashboardFieldMappingTable> GetAllFieldMapping(AMSContext db, int dashboardMappingID)
        {
            var result = (from b in db.DashboardFieldMappingTable.Include("DashboardMapping").Include("DashboardTemplateField")
                          where b.DashboardMappingID == dashboardMappingID && b.StatusID == (int)StatusValue.Active select b);
            return result;
        }
    }
}