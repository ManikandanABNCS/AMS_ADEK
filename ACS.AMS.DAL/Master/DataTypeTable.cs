using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACS.AMS.DAL.DBModel
{
    public partial class DataTypeTable : BaseEntityObject

    {
        public static IQueryable<DataTypeTable> GetAllDataTypes()
        {
            AMSContext db = AMSContext.CreateNewContext();
            return GetAllDataTypes(db);
        }
        public static IQueryable<DataTypeTable> GetAllDataTypes(AMSContext db, bool includeInactiveItems = false)
        {
            return from b in db.DataTypeTable
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
