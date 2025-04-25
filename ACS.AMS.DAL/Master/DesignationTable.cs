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
    public partial class DesignationTable : BaseEntityObject
    {
        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.DesignationTable
                                  where b.DesignationCode == this.DesignationCode

                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();
            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Designation Code already exists ", this.DesignationCode);
                args.FieldName = "DesignationCode";
                return false;
            }
            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.DesignationTable
                                  where (b.DesignationCode == this.DesignationCode && b.StatusID == (int)StatusValue.Active
                                  && b.DesignationID != this.DesignationID)

                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Designation Code already exists ", this.DesignationCode);
                args.FieldName = "DesignationCode";
                return false;
            }
            return true;
        }
        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.PersonTable
                                  where (b.DesignationID == this.DesignationID)
                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("references found", this.DesignationID);
                args.FieldName = "DesignationCode";
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
        //                var checkDuplicate = (from b in args.NewDB.DesignationTable
        //                                      where b.DesignationCode == this.DesignationCode

        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();
        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Designation Code already exists ", this.DesignationCode);
        //                    args.FieldName = "DesignationCode";
        //                    return false;
        //                }
        //                return true;
        //            }

        //        case EntityObjectState.Edit:
        //            {

        //                var checkDuplicate = (from b in args.NewDB.DesignationTable
        //                                      where (b.DesignationCode == this.DesignationCode && b.StatusID == (int)StatusValue.Active
        //                                      && b.DesignationID != this.DesignationID)

        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Designation Code already exists ", this.DesignationCode);
        //                    args.FieldName = "DesignationCode";
        //                    return false;
        //                }

        //                return true;
        //            }

        //        case EntityObjectState.Delete:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.PersonTable
        //                                      where (b.DesignationID == this.DesignationID)
        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("references found", this.DesignationID);
        //                    args.FieldName = "DesignationCode";
        //                    return false;
        //                }

        //                return true;
        //            }
        //    }
        //    return true;
        //}
    }
}

