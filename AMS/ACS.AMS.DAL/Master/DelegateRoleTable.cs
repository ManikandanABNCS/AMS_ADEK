using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACS.AMS.DAL.DBContext;
using Castle.Components.DictionaryAdapter.Xml;

namespace ACS.AMS.DAL.DBModel
{
    public partial class DelegateRoleTable
    {
        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            if (this.EmployeeID == this.DelegatedEmployeeID)
            {
                args.ErrorMessage = string.Format("Employee and delegate employee could not be the same.");
                args.FieldName = "EmployeeID";
                return false;
            }
            var existingItem = (from b in args.NewDB.DelegateRoleTable
                                where b.StatusID != (int)StatusValue.Deleted &&
                                  (b.EmployeeID == this.EmployeeID)
                                select new { EmployeeID = b.EmployeeID }).FirstOrDefault();
            if (existingItem != null)
            {
                args.FieldName = "EmployeeID";
                args.ErrorMessage = string.Format(Language.GetErrorMessage("Delegate role Already Exist"), "EmployeeID");
                return false;
            }

            var checkDuplicate = (from b in args.NewDB.DelegateRoleTable
                                  where b.EmployeeID == this.EmployeeID
                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();
            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Delegate Role already exists to the employee", this.EmployeeID);
                args.FieldName = "EmployeeID";
                return false;
            }
            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            if (this.EmployeeID == this.DelegatedEmployeeID)
            {
                args.ErrorMessage = string.Format("Employee and delegate employee could not be the same.");
                args.FieldName = "EmployeeID";
                return false;
            }
            var checkDuplicate = (from b in args.NewDB.DelegateRoleTable
                                  where b.EmployeeID == this.EmployeeID && b.DelegateRoleID != this.DelegateRoleID
                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();
            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Delegate Role already exists to the employee", this.EmployeeID);
                args.FieldName = "EmployeeID";
                return false;
            }
            return true;
        }
        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            return true;
        }
        //internal override bool ValidateObject(ValidateEventArgs<AMSContext> args)
        //{
        //    switch (args.State)
        //    {
        //        case EntityObjectState.New:
        //            {
        //                if (this.EmployeeID == this.DelegatedEmployeeID)
        //                {
        //                    args.ErrorMessage = string.Format("Employee and delegate employee could not be the same.");
        //                    args.FieldName = "EmployeeID";
        //                    return false;
        //                }
        //                var existingItem = (from b in args.NewDB.DelegateRoleTable
        //                                    where b.StatusID != (int)StatusValue.Deleted  &&
        //                                      (b.EmployeeID == this.EmployeeID)
        //                                    select new { EmployeeID = b.EmployeeID }).FirstOrDefault();
        //                if (existingItem != null)
        //                {
        //                    args.FieldName = "EmployeeID";
        //                    args.ErrorMessage = string.Format(Language.GetErrorMessage("Delegate role Already Exist"), "EmployeeID");
        //                    return false;
        //                }

        //                var checkDuplicate = (from b in args.NewDB.DelegateRoleTable
        //                                      where b.EmployeeID == this.EmployeeID 
        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();
        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Delegate Role already exists to the employee", this.EmployeeID);
        //                    args.FieldName = "EmployeeID";
        //                    return false;
        //                }

        //                return true;
        //            }

        //        case EntityObjectState.Edit:
        //            {
        //                if (this.EmployeeID == this.DelegatedEmployeeID)
        //                {
        //                    args.ErrorMessage = string.Format("Employee and delegate employee could not be the same.");
        //                    args.FieldName = "EmployeeID";
        //                    return false;
        //                }
        //                var checkDuplicate = (from b in args.NewDB.DelegateRoleTable
        //                                      where b.EmployeeID == this.EmployeeID &&  b.DelegateRoleID != this.DelegateRoleID
        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();
        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Delegate Role already exists to the employee", this.EmployeeID);
        //                    args.FieldName = "EmployeeID";
        //                    return false;
        //                }
        //                return true;
        //            }

        //    }
        //    return true;
        //}
    }
}
