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
    public partial class DepartmentTable : BaseEntityObject
    {
        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.DepartmentTable
                                  where b.DepartmentCode == this.DepartmentCode

                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();
            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Department Code already exists ", this.DepartmentCode);
                args.FieldName = "DepartmentCode";
                return false;
            }
            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.DepartmentTable
                                  where (b.DepartmentCode == this.DepartmentCode && b.StatusID == (int)StatusValue.Active
                                  && b.DepartmentID != this.DepartmentID)

                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Department Code already exists ", this.DepartmentCode);
                args.FieldName = "DepartmentCode";
                return false;
            }

            return true;
        }
        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.AssetTable
                                  where (b.DepartmentID == this.DepartmentID)
                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("references found", this.DepartmentID);
                args.FieldName = "DepartmentCode";
                return false;
            }
            var checksectionDuplicate = (from b in args.NewDB.SectionTable
                                         where (b.DepartmentID == this.DepartmentID)
                                         && b.StatusID == (int)StatusValue.Active
                                         select b).Count();

            if (checksectionDuplicate > 0)
            {
                args.ErrorMessage = string.Format("references found", this.DepartmentID);
                args.FieldName = "DepartmentCode";
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
        //                var checkDuplicate = (from b in args.NewDB.DepartmentTable
        //                                      where b.DepartmentCode == this.DepartmentCode

        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();
        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Department Code already exists ", this.DepartmentCode);
        //                    args.FieldName = "DepartmentCode";
        //                    return false;
        //                }
        //                return true;
        //            }

        //        case EntityObjectState.Edit:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.DepartmentTable
        //                                      where (b.DepartmentCode == this.DepartmentCode && b.StatusID == (int)StatusValue.Active
        //                                      && b.DepartmentID != this.DepartmentID)

        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Department Code already exists ", this.DepartmentCode);
        //                    args.FieldName = "DepartmentCode";
        //                    return false;
        //                }

        //                return true;
        //            }

        //        case EntityObjectState.Delete:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.AssetTable
        //                                      where (b.DepartmentID == this.DepartmentID)
        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("references found", this.DepartmentID);
        //                    args.FieldName = "DepartmentCode";
        //                    return false;
        //                }
        //                var checksectionDuplicate = (from b in args.NewDB.SectionTable
        //                                      where (b.DepartmentID == this.DepartmentID)
        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();

        //                if (checksectionDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("references found", this.DepartmentID);
        //                    args.FieldName = "DepartmentCode";
        //                    return false;
        //                }

        //                return true;
        //            }
        //    }
        //    return true;
        //}

        public static DepartmentTable GetDepartmentDetails(AMSContext _db,string departmentName)
        {
            var res = (from b in _db.DepartmentTable where b.DepartmentName == departmentName && b.StatusID != (int)StatusValue.Deleted && b.StatusID != (int)StatusValue.DeletedOLD select b).FirstOrDefault();
            return res;
        }
        public static DepartmentTable GetDepartmentCode(AMSContext _db,string departmentCode)
        {
            var res = (from b in _db.DepartmentTable where b.DepartmentCode == departmentCode && b.StatusID == (int)StatusValue.Active select b).FirstOrDefault();
            return res;
        }
    }

   
}

