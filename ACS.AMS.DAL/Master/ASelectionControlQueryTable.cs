using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACS.AMS.DAL.DBContext;
namespace ACS.AMS.DAL.DBModel
{

    public partial class ASelectionControlQueryTable :BaseEntityObject
    {
        #region Static Methods

        public static ASelectionControlQueryTable GetScreenControlQuery(AMSContext db, string controlName)
        {
            if (db == null)
                db = AMSContext.CreateNewContext();

            return (from b in db.ASelectionControlQueryTable
                    where b.ControlName == controlName
                    select b).FirstOrDefault();
        }

        #endregion
        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.ASelectionControlQueryTable
                                  where b.ControlName == this.ControlName
                                  select b).Count();
            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Control Name already exists", this.ControlName);
                args.FieldName = "ControlName";
                return false;
            }
            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.ASelectionControlQueryTable
                                  where (b.ControlName == this.ControlName)
                                        && b.SelectionControlQueryID != this.SelectionControlQueryID
                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Control Name already exists", this.ControlName);
                args.FieldName = "ControlName";
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
        //                var checkDuplicate = (from b in args.NewDB.ASelectionControlQueryTable
        //                                      where b.ControlName == this.ControlName
        //                                      select b).Count();
        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Control Name already exists", this.ControlName);
        //                    args.FieldName = "ControlName";
        //                    return false;
        //                }
        //                return true;
        //            }

        //        case EntityObjectState.Edit:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.ASelectionControlQueryTable
        //                                      where (b.ControlName == this.ControlName)
        //                                            && b.SelectionControlQueryID != this.SelectionControlQueryID
        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Control Name already exists", this.ControlName);
        //                    args.FieldName = "ControlName";
        //                    return false;
        //                }

        //                return true;
        //            }

        //        case EntityObjectState.Delete:
        //            {
        //                return true;
        //            }
        //    }
        //    return true;
        //}

    }
}