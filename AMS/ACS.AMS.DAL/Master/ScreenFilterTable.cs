using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.AMS.DAL.DBModel
{
    public partial class ScreenFilterTable
    {
        #region Static Methods

        public static int CreateScreenFilterTable(AMSContext db, string screenFilterName, int? parentID, string parentType)
        {
            if (db == null)
                db = AMSContext.CreateNewContext();

            var ser = new ScreenFilterTable();
            ser.ScreenFilterName = screenFilterName;
            ser.ParentID = parentID;
            ser.ParentType = parentType;
            ser.CreatedDateTime = DateTime.Now;
            ser.ReportTemplateID = (int)parentID;
            db.Add(ser);
            db.SaveChanges();

            return ser.ScreenFilterID;
        }

        public static IQueryable<ScreenFilterLineItemTable> GetFiltersList(AMSContext db, string screenFilterName)
        {
            var qry = from b in db.ScreenFilterLineItemTable.Include(c => c.FieldType).Include(c => c.ScreenFilter)
                      where b.ScreenFilter.ScreenFilterName == screenFilterName && b.StatusID == (int)StatusValue.Active
                      select b;

            return qry;
        }

        #endregion
    }
}
