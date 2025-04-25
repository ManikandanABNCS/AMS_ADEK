using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACS.AMS.DAL.DBModel
{
    public partial class DepreciationClassLineItemTable : BaseEntityObject

    {
       
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
        public static bool CheckExistingLineProcess(AMSContext db, int itemLineID, int itemID)
        {
            var result = (from b in db.DepreciationClassLineItemTable
                          where b.DepreciationClassLineItemID == itemLineID && b.DepreciationClassID == itemID
                          select b);
            if (result.Count() > 0)
                return true;
            return false;
        }

    }
}
