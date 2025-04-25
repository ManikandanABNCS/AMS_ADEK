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
   public partial class DisplayControlTypeTable :BaseEntityObject
    {
        public static IQueryable<DisplayControlTypeTable> GetControlType(string dataType)
        {
            AMSContext db = AMSContext.CreateNewContext();

            var result = DisplayControlTypeTable.GetAllDisplayControlTypes();

            if (dataType == "Integer")
            {
                result = from d in result
                         where d.Integer == true
                         select d;
            }
            else if (dataType == "String")
            {
                result = from d in result
                         where d.String == true
                         select d;
            }
            else if (dataType == "Decimal")
            {
                result = from d in result
                         where d.Decimal == true
                         select d;
            }
            else if (dataType == "Boolean")
            {
                result = from d in result
                         where d.Boolean == true
                         select d;
            }
            else if (dataType == "Date")
            {
                result = from d in result
                         where d.Date == true
                         select d;
            }
            else if (dataType == "Time")
            {
                result = from d in result
                         where d.Time == true
                         select d;
            }
            else if (dataType == "DateTime")
            {
                result = from d in result
                         where d.DateTime == true
                         select d;
            }

            return result;
        }

        public static IQueryable<DisplayControlTypeTable> GetAllDisplayControlTypes()
        {
            AMSContext db = AMSContext.CreateNewContext();
            return GetAllDisplayControlTypes(db);
        }
        public static IQueryable<DisplayControlTypeTable> GetAllDisplayControlTypes(AMSContext db, bool includeInactiveItems = false)
        {
            return from b in db.DisplayControlTypeTable
                   select b;
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
        //internal override bool ValidateObject(ValidateEventArgs<AMSContext> args)
        //{
        //    return true;
        //}
    }
}
