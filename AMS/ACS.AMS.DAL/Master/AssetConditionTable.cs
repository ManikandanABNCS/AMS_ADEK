using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel
{
    public partial class AssetConditionTable:BaseEntityObject
    {
        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.AssetConditionTable
                                  where b.AssetConditionCode == this.AssetConditionCode

                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();
            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Asset Condition Code already exists ", this.AssetConditionCode);
                args.FieldName = "AssetConditionCode";
                return false;
            }
            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.AssetConditionTable
                                  where (b.AssetConditionCode == this.AssetConditionCode && b.StatusID == (int)StatusValue.Active
                                  && b.AssetConditionID != this.AssetConditionID)

                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Asset Condition Code already exists ", this.AssetConditionCode);
                args.FieldName = "AssetConditionCode";
                return false;
            }
            return true;
        }
        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.AssetTable
                                  where (b.AssetConditionID == this.AssetConditionID)
                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("references found", this.AssetConditionID);
                args.FieldName = "CategoryCode";
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
        //                var checkDuplicate = (from b in args.NewDB.AssetConditionTable
        //                                      where b.AssetConditionCode == this.AssetConditionCode

        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();
        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Asset Condition Code already exists ", this.AssetConditionCode);
        //                    args.FieldName = "AssetConditionCode";
        //                    return false;
        //                }
        //                return true;
        //            }

        //        case EntityObjectState.Edit:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.AssetConditionTable
        //                                      where (b.AssetConditionCode == this.AssetConditionCode && b.StatusID == (int)StatusValue.Active
        //                                      && b.AssetConditionID != this.AssetConditionID)

        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Asset Condition Code already exists ", this.AssetConditionCode);
        //                    args.FieldName = "AssetConditionCode";
        //                    return false;
        //                }

        //                return true;
        //            }

        //        case EntityObjectState.Delete:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.AssetTable
        //                                      where (b.AssetConditionID == this.AssetConditionID)
        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("references found", this.AssetConditionID);
        //                    args.FieldName = "CategoryCode";
        //                    return false;
        //                }
                        
        //                return true;
        //            }
        //    }
        //    return true;
        //}
    }
}
