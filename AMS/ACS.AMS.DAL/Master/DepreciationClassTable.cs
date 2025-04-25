using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACS.AMS.DAL.DBModel
{
    public partial class DepreciationClassTable : BaseEntityObject

    {
      
        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            var existingItem = (from b in args.NewDB.DepreciationClassTable
                                where b.ClassName.ToUpper() == this.ClassName.ToUpper() && b.StatusID != (int)StatusValue.Deleted
                                select new { ClassName = b.ClassName }).FirstOrDefault();
            if (existingItem != null)
            {
                if (string.Compare(existingItem.ClassName, this.ClassName.Trim(), true) == 0)
                {
                    args.FieldName = "ClassName"; //this is an parent table - so add ItemName_ infront of each fields
                    args.ErrorMessage = string.Format(Language.GetErrorMessage("ClassNameAlreadyExists"), Language.GetFieldName("ClassName", CultureHelper.EnglishCultureSymbol), ClassName);
                    return false;
                }

            }
            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            var existingItem = (from b in args.NewDB.DepreciationClassTable
                                where b.ClassName.ToUpper() == this.ClassName.ToUpper() && b.DepreciationClassID != this.DepreciationClassID && b.StatusID != (int)StatusValue.Deleted
                                select new { ClassName = b.ClassName }).FirstOrDefault();
            if (existingItem != null)
            {
                if (string.Compare(existingItem.ClassName, this.ClassName.Trim(), true) == 0)
                {
                    args.FieldName = "ClassName"; //this is an parent table - so add ItemName_ infront of each fields
                    args.ErrorMessage = string.Format(Language.GetErrorMessage("ClassNameAlreadyExists"), Language.GetFieldName("ClassName", CultureHelper.EnglishCultureSymbol), ClassName);
                    return false;
                }

            }
            return true;
        }
        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            var depreciationclass = (from b in args.NewDB.AssetTable
                                     where b.DepreciationClassID == this.DepreciationClassID && b.StatusID != (int)StatusValue.Deleted
                                     select b).Count();

            if (depreciationclass > 0)
            {

                args.ErrorMessage = string.Format(Language.GetErrorMessage("ReferenceFound"), Language.GetEntityName("DepreciationClassTable"));
                return false;
            }
            var depreciationCategory = (from b in args.NewDB.CategoryTable
                                        where b.DepreciationClassID == this.DepreciationClassID && b.StatusID != (int)StatusValue.Deleted
                                        select b).Count();

            if (depreciationCategory > 0)
            {
                args.ErrorMessage = string.Format(Language.GetErrorMessage("ReferenceFound"), Language.GetEntityName("DepreciationClassTable"));
                return false;
            }
            return true;
        }
        //internal override bool ValidateObject(ValidateEventArgs<AMSContext> args)
        //{
        //    return true;
        //}
    }
}
