using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel
{
    public partial class PartyTable:BaseEntityObject

    {
        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.PartyTable
                                  where b.PartyCode == this.PartyCode

                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();    
            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("PartyCode  already exists ", this.PartyCode);
                args.FieldName = "PartyCode";
                return false;
            }
            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.PartyTable
                                  where (b.PartyCode == this.PartyCode && b.StatusID == (int)StatusValue.Active
                                  && b.PartyID != this.PartyID)

                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Party Code already exists ", this.PartyCode);
                args.FieldName = "PartyCode";
                return false;
            }
            return true;
        }
        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.AssetTable
                                  where (b.SupplierID == this.PartyID)
                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("references found", this.PartyID);
                args.FieldName = "PartyID";
                return false;
            }
            var checkManufDuplicate = (from b in args.NewDB.AssetTable
                                       where (b.ManufacturerID == this.PartyID)
                                       && b.StatusID == (int)StatusValue.Active
                                       select b).Count();

            if (checkManufDuplicate > 0)
            {
                args.ErrorMessage = string.Format("references found", this.PartyID);
                args.FieldName = "PartyID";
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
        //                var checkDuplicate = (from b in args.NewDB.PartyTable
        //                                      where b.PartyCode == this.PartyCode

        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();
        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("PartyCode  already exists ", this.PartyCode);
        //                    args.FieldName = "PartyCode";
        //                    return false;
        //                }
        //                return true;
        //            }

        //        case EntityObjectState.Edit:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.PartyTable
        //                                      where (b.PartyCode == this.PartyCode && b.StatusID == (int)StatusValue.Active
        //                                      && b.PartyID != this.PartyID)

        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Party Code already exists ", this.PartyCode);
        //                    args.FieldName = "PartyCode";
        //                    return false;
        //                }

        //                return true;
        //            }

        //        case EntityObjectState.Delete:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.AssetTable
        //                                      where (b.SupplierID == this.PartyID)
        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("references found", this.PartyID);
        //                    args.FieldName = "PartyID";
        //                    return false;
        //                }
        //                var checkManufDuplicate = (from b in args.NewDB.AssetTable
        //                                      where (b.ManufacturerID == this.PartyID)
        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();

        //                if (checkManufDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("references found", this.PartyID);
        //                    args.FieldName = "PartyID";
        //                    return false;
        //                }
                        
        //                return true;
        //            }
        //    }
        //    return true;
        //}

       

    }
}
