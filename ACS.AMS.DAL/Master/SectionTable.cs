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
using ACS.WebAppPageGenerator.Models.SystemModels;

namespace ACS.AMS.DAL.DBModel
{
    public partial class SectionTable : BaseEntityObject
    {
        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.SectionTable
                                  where b.SectionCode == this.SectionCode

                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();
            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Section Code already exists ", this.SectionCode);
                args.FieldName = "SectionCode";
                return false;
            }
            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.SectionTable
                                  where (b.SectionCode == this.SectionCode && b.StatusID == (int)StatusValue.Active
                                  && b.SectionID != this.SectionID)

                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Section Code already exists ", this.SectionCode);
                args.FieldName = "SectionCode";
                return false;
            }

            return true;
        }
        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.AssetTable
                                  where (b.SectionID == this.SectionID)
                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("references found", this.SectionID);
                args.FieldName = "SectionCode";
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
        //                var checkDuplicate = (from b in args.NewDB.SectionTable
        //                                      where b.SectionCode == this.SectionCode

        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();
        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Section Code already exists ", this.SectionCode);
        //                    args.FieldName = "SectionCode";
        //                    return false;
        //                }
        //                return true;
        //            }

        //        case EntityObjectState.Edit:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.SectionTable
        //                                      where (b.SectionCode == this.SectionCode && b.StatusID == (int)StatusValue.Active
        //                                      && b.SectionID != this.SectionID)

        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Section Code already exists ", this.SectionCode);
        //                    args.FieldName = "SectionCode";
        //                    return false;
        //                }

        //                return true;
        //            }

        //        case EntityObjectState.Delete:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.AssetTable
        //                                      where (b.SectionID == this.SectionID)
        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("references found", this.SectionID);
        //                    args.FieldName = "SectionCode";
        //                    return false;
        //                }

        //                return true;
        //            }

        //    }
        //    return true;
        //}
        public static SectionTable GetSectionCode(AMSContext _db, string sectionCode)
        {
            var res = (from b in _db.SectionTable where b.SectionCode == sectionCode && b.StatusID == (int)StatusValue.Active select b).FirstOrDefault();
            return res;
        }
        public override PageFieldModelCollection GetCreateScreenControls(string subpageName, int userID)
        {
            var allList = base.GetCreateScreenControls(subpageName, userID);

            PageFieldModelCollection allowedCols = new PageFieldModelCollection()
            {
                new PageFieldModel() { FieldName = "SectionCode", IsMandatory = true,StringMaxLength=100 },
                new PageFieldModel() { FieldName = "DepartmentID", IsMandatory = true,IsMasterCreation=true },
                new PageFieldModel() { FieldName = "SectionName", IsMandatory = true ,StringMaxLength=100}
            };


         
            PageFieldModelCollection newList = new PageFieldModelCollection();
            foreach (var c in allowedCols)
            {
                var itm = allList.Where(b => string.Compare(b.FieldName, c.FieldName, true) == 0).FirstOrDefault();
                if (itm != null)
                {
                    itm.IsMandatory = c.IsMandatory;
                    itm.IsHidden = c.IsHidden;
                    itm.DefaultValue = c.DefaultValue;
                    itm.ControlType = c.ControlType;
                    itm.ControlName = c.ControlName;
                    itm.DisplayLabel = string.IsNullOrEmpty(c.DisplayLabel) ? c.FieldName : c.DisplayLabel;
                    itm.DataReadScriptFunctionName = c.DataReadScriptFunctionName;
                    itm.StringMaxLength = c.StringMaxLength;
                    itm.IsMasterCreation = c.IsMasterCreation;
                    newList.Add(itm);
                }
            }

            return newList;
        }

    }
}

