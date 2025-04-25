using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ACS.AMS.DAL.DBModel
{
   
    public partial class ScreenFilterLineItemTable
    {
        //partial void AddSingleEntityIncludeTableObjects(AMSContext db)
        //{
        //    SingleEntityQuery = SingleEntityQuery;
        //}
        [NotMapped]
        public bool EnableFilter { get; set; }
        [NotMapped]
        public string FilterTypeDesc { get; set; }
        [NotMapped]
        public string FieldDataType { get; set; }


     
        //[NotMapped]
        //public string FieldName { get; set; }

        [NotMapped]
        public string FieldValue { get; set; }

      
        #region Static Methods

        //public static IQueryable<ScreenFilterLineItemTable> GetAllScreenFilterLineItems(AMSContext db, int screenFilterID)
        //{
        //    return from b in db.ScreenFilterLineItemTable
        //           where b.ScreenFilterID == screenFilterID
        //           select b;
        //}

        public static ScreenFilterLineItemTable GetFilterByQueryFieldName(AMSContext db, int screenFilterID, string queryFieldName)
        {
            var qry = (from b in db.ScreenFilterLineItemTable
                       where b.QueryField == queryFieldName && b.ScreenFilterID == screenFilterID
                       select b).FirstOrDefault();

            return qry;
        }

        public static ScreenFilterLineItemTable GetFilterByQueryFieldName(AMSContext db, string fieldName)
        {
            var qry = (from b in db.ScreenFilterLineItemTable
                       where b.QueryField == fieldName
                       select b).FirstOrDefault();

            return qry;
        }

        public static IQueryable<ScreenFilterLineItemTable> GetFiltersList(AMSContext db, string screenFilterName)
        {
            var qry = from b in db.ScreenFilterLineItemTable.Include("AFieldTypeTable").Include("AFieldTypeTable.ASelectionControlQueryTable")
                      where b.ScreenFilter.ScreenFilterName == screenFilterName
                            && b.ScreenFilter.StatusID == (int)StatusValue.Active
                            && b.StatusID == (int)StatusValue.Active
                      select b;
            return qry;
        }

        public static int GetScreenFilterID(AMSContext db, int screenFilterLineItemID)
        {
            int result = (from b in db.ScreenFilterLineItemTable
                          where b.ScreenFilterLineItemID == screenFilterLineItemID
                          select b.ScreenFilterID).FirstOrDefault();
            return result;
        }

        #endregion

       
    }
}
