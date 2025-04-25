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
    public partial class DashboardTemplateTable :BaseEntityObject
    {
        [NotMapped]
        public int ObjectID { get; set; }

        public static bool IsQueryTextAlreadyExists(AMSContext db, string queryString)
        {
            var result = (from b in db.DashboardTemplateTable
                          where b.QueryString == queryString.Trim()
                                && b.StatusID==(int)StatusValue.Active
                          select b);

            return result.Any();
        }
        public static string GetQueryType(AMSContext db,string queryString)
        {
            string queryTyoe = "Procedure";
                if(queryString.Contains("dprc"))
                    queryTyoe = "Procedure";
                else
                    queryTyoe = "View";
            return queryTyoe;
        }

        public static IQueryable<DashboardTemplateFieldTable> GetAllDashboardTemplateFields(AMSContext db, int dashboardTemplateID,string ? text=null)
        {
            IQueryable<DashboardTemplateFieldTable> result = (from b in db.DashboardTemplateFieldTable
                          where b.StatusID == (int)StatusValue.Active
                                && b.DashboardTemplateID == dashboardTemplateID
                          select b);
            if (!string.IsNullOrEmpty(text))
                result = result.Where(a => a.FieldName.ToUpper().Contains(text.ToUpper()));

            return result; // new SelectList(result, "Value", "Text");
        }
        public static IQueryable<DashboardTemplateFieldTable> AllDashboardFields(AMSContext db,int dashboardTemplateID)
        {
            var result = (from b in db.DashboardTemplateFieldTable.Include("DashboardTemplate").Include("Status") where b.DashboardTemplateID == dashboardTemplateID && b.StatusID == (int)StatusValue.Active select b);
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
    }
}