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

  
    public partial class AFieldTypeTable
    {
        #region Static Methods

        public static IQueryable<AFieldTypeTable> GetAllFieldTypes(AMSContext db, bool includeInactiveItems = false)
        {
            return from b in db.AFieldTypeTable
                   select b;
        }

        public static AFieldTypeTable GetAllFieldType(AMSContext db, string fieldTypeName)
        {

            var result = (from b in db.AFieldTypeTable
                          where b.FieldTypeDesc  == fieldTypeName                        
                          select b).FirstOrDefault();
            //copy to new empty list
            return result;
        }
      
        public static string GetFieldTypeDesc(AMSContext db, int FieldTypeID)
        {
            if (db == null)
            {
                db = AMSContext.CreateNewContext();
            }

            return (from b in db.AFieldTypeTable
                    where b.FieldTypeID == FieldTypeID
                    select b.FieldTypeDesc).FirstOrDefault();
        }

        #endregion


    }
}


