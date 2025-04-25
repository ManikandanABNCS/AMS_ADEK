using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ACS.AMS.DAL.DBModel
{
    public partial class RetirementTypeTable
    {
        public static RetirementTypeTable GetRetirementTypeCode(AMSContext db, string RetirementTypeCode)
        {
            return db.RetirementTypeTable.Where(b => b.RetirementTypeCode == RetirementTypeCode).FirstOrDefault();
        }
        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.RetirementTypeTable
                                  where b.RetirementTypeCode == this.RetirementTypeCode

                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();
            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Retirement Type already exists ", this.RetirementTypeCode);
                args.FieldName = "RetirementTypeCode";
                return false;
            }
            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.RetirementTypeTable
                                  where (b.RetirementTypeCode == this.RetirementTypeCode && b.StatusID == (int)StatusValue.Active
                                  && b.RetirementTypeID != this.RetirementTypeID)

                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Retirement Type already exists", this.RetirementTypeCode);
                args.FieldName = "RetirementTypeCode";
                return false;
            }
            return true;
        }
        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.AssetTable
                                  where (b.RetirementTypeID == this.RetirementTypeID)
                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Retirement Type references found", this.RetirementTypeID);
                args.FieldName = "RetirementTypeID";
                return false;
            }
            return true;
        }

        //internal override bool ValidateObject(ValidateEventArgs<AMSContext> args)
        //{
        //    switch (args.State)
        //    {
        //        case EntityObjectState.New:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.RetirementTypeTable
        //                                      where b.RetirementTypeCode == this.RetirementTypeCode

        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();
        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Retirement Type already exists ", this.RetirementTypeCode);
        //                    args.FieldName = "RetirementTypeCode";
        //                    return false;
        //                }
        //                return true;
        //            }

        //        case EntityObjectState.Edit:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.RetirementTypeTable
        //                                      where (b.RetirementTypeCode == this.RetirementTypeCode && b.StatusID == (int)StatusValue.Active
        //                                      && b.RetirementTypeID != this.RetirementTypeID)

        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Retirement Type already exists", this.RetirementTypeCode);
        //                    args.FieldName = "RetirementTypeCode";
        //                    return false;
        //                }

        //                return true;
        //            }

        //        case EntityObjectState.Delete:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.AssetTable
        //                                      where (b.RetirementTypeID == this.RetirementTypeID)
        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Retirement Type references found", this.RetirementTypeID);
        //                    args.FieldName = "RetirementTypeID";
        //                    return false;
        //                }
                        
        //                return true;
        //            }
        //    }
        //    return true;
        //}
    }
}
