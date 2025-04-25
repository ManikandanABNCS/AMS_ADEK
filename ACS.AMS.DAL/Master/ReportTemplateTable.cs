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
    public partial class ReportTemplateTable :BaseEntityObject
    {
        [NotMapped]
        public int ObjectID { get; set; }

        #region Static Methods
        public static ReportTable GetReport(AMSContext _db, int id)
        {
            return (from b in _db.ReportTable.Include("ReportTemplate").Include("ReportPaperSize")
                    where b.ReportID == id
                    select b).FirstOrDefault();
        }


        //public static IQueryable<ReportTemplateTable> GetAllReportTemplates(AMSContext db)
        //{
        //    return db.ReportTemplateTable.Where(b=>b.StatusID != (int)StatusValue.Deleted).Include("Status").Include("ReportTemplateCategory").AsQueryable();
        //}
        public static bool IsQueryTextAlreadyExists(AMSContext db, string queryString, string queryType)
        {
            var result = (from b in db.ReportTemplateTable
                          where b.QueryString == queryString.Trim()
                                && b.QueryType == queryType.Trim()
                          select b);

            return result.Any();
        }

        public static string GetReportName(AMSContext db, int reportID)
        {
            var result = (from b in db.ReportTemplateTable
                          where b.ReportTemplateID == reportID
                          select b.ReportTemplateName).FirstOrDefault();

            return result;
        }

        public static int GetReportID(AMSContext db, string reportName)
        {
            int result = (from b in db.ReportTemplateTable
                          where b.ReportTemplateName == reportName
                          select b.ReportTemplateID).FirstOrDefault();

            return result;
        }

        public static IQueryable<ReportTemplateTable> GetAllReportTemplatesForCategory(AMSContext db, int reportTemplateCategoryID)
        {
            return from b in GetAllItems(db)
                   where b.ReportTemplateCategoryID == reportTemplateCategoryID
                   select b;
        }

        #endregion
    }
}