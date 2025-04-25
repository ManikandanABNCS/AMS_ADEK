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
    public partial class ReportTemplateFileTable
    {
        //public static IQueryable<ReportTemplateFileTable> GetAllReportTemplateFiles(AMSContext db)
        //{

        //    return db.ReportTemplateFileTable.Include("Status").AsQueryable();
        //}
        //public static ReportTemplateFileTable GetReportTemplateFile(AMSContext db, int reportTemplateFileID)
        //{
        //    return db.ReportTemplateFileTable.Include("Status").Where(c => c.ReportTemplateFileID == reportTemplateFileID).FirstOrDefault();

        //}
    }
    
}
