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
    public partial class ApprovalRoleTable:BaseEntityObject
    {
        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.ApprovalRoleTable
                                  where b.ApprovalRoleCode == this.ApprovalRoleCode

                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();
            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("ApprovalRole Code already exists ", this.ApprovalRoleCode);
                args.FieldName = "ApprovalRoleCode";
                return false;
            }
            if (this.ApprovalLocationTypeID == null)
            {
                args.ErrorMessage = string.Format("Approval Location Type is mandatory ", this.ApprovalRoleCode);
                args.FieldName = "ApprovalLocationTypeID";
                return false;
            }
            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.ApprovalRoleTable
                                  where (b.ApprovalRoleCode == this.ApprovalRoleCode && b.StatusID == (int)StatusValue.Active
                                  && b.ApprovalRoleID != this.ApprovalRoleID)

                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("ApprovalRole Code already exists ", this.ApprovalRoleCode);
                args.FieldName = "ApprovalRoleCode";
                return false;
            }
            if(this.ApprovalLocationTypeID==null)
            {
                args.ErrorMessage = string.Format("Approval Location Type is mandatory ", this.ApprovalRoleCode);
                args.FieldName = "ApprovalLocationTypeID";
                return false;
            }
            
            return true;
        }
        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.ApproveWorkflowLineItemTable
                                  where (b.ApprovalRoleID == this.ApprovalRoleID)
                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("references found", this.ApprovalRoleID);
                args.FieldName = "ApprovalRoleCode";
                return false;
            }
            var checksectionDuplicate = (from b in args.NewDB.UserApprovalRoleMappingTable
                                         where (b.ApprovalRoleID == this.ApprovalRoleID)
                                         && b.StatusID == (int)StatusValue.Active
                                         select b).Count();

            if (checksectionDuplicate > 0)
            {
                args.ErrorMessage = string.Format("references found", this.ApprovalRoleID);
                args.FieldName = "ApprovalRoleCode";
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
        //                var checkDuplicate = (from b in args.NewDB.ApprovalRoleTable
        //                                      where b.ApprovalRoleCode == this.ApprovalRoleCode

        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();
        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("ApprovalRole Code already exists ", this.ApprovalRoleCode);
        //                    args.FieldName = "ApprovalRoleCode";
        //                    return false;
        //                }
        //                return true;
        //            }

        //        case EntityObjectState.Edit:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.ApprovalRoleTable
        //                                      where (b.ApprovalRoleCode == this.ApprovalRoleCode && b.StatusID == (int)StatusValue.Active
        //                                      && b.ApprovalRoleID != this.ApprovalRoleID)

        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("ApprovalRole Code already exists ", this.ApprovalRoleCode);
        //                    args.FieldName = "ApprovalRoleCode";
        //                    return false;
        //                }

        //                return true;
        //            }

        //        case EntityObjectState.Delete:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.ApproveWorkflowLineItemTable
        //                                      where (b.ApprovalRoleID == this.ApprovalRoleID)
        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("references found", this.ApprovalRoleID);
        //                    args.FieldName = "ApprovalRoleCode";
        //                    return false;
        //                }
        //                var checksectionDuplicate = (from b in args.NewDB.UserApprovalRoleMappingTable
        //                                             where (b.ApprovalRoleID == this.ApprovalRoleID)
        //                                             && b.StatusID == (int)StatusValue.Active
        //                                             select b).Count();

        //                if (checksectionDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("references found", this.ApprovalRoleID);
        //                    args.FieldName = "ApprovalRoleCode";
        //                    return false;
        //                }

        //                return true;
        //            }
        //    }
        //    return true;
        //}
    }
}
