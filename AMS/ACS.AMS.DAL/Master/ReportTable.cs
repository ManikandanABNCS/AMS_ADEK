using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.AMS.DAL.DBModel
{
    public partial class ReportTable : BaseEntityObject
    {
        public static IQueryable<ReportTable> GetAllReports(AMSContext db, int reportTemplateID)
        {
            IQueryable<ReportTable> result = (from b in db.ReportTable
                                              where b.StatusID == (int)StatusValue.Active
                                                    && b.ReportTemplateID == reportTemplateID
                                              orderby b.ReportName
                                              select b);

            //copy to new empty list
            return result;
        }
    }
}