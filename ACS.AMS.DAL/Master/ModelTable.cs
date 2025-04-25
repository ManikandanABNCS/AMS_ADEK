using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using ACS.WebAppPageGenerator.Models.SystemModels;

namespace ACS.AMS.DAL.DBModel
{
    public partial class ModelTable : BaseEntityObject

    {
        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.ModelTable
                                  where b.ModelCode == this.ModelCode

                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();
            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("ModelCode  already exists ", this.ModelCode);
                args.FieldName = "ModelCode";
                return false;
            }
            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.ModelTable
                                  where (b.ModelCode == this.ModelCode && b.StatusID == (int)StatusValue.Active
                                  && b.ModelID != this.ModelID)

                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Model Code already exists ", this.ModelCode);
                args.FieldName = "ModelCode";
                return false;
            }
            return true;
        }
        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.AssetTable
                                  where (b.ModelID == this.ModelID)
                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("references found", this.ModelID);
                args.FieldName = "ModelID";
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
        //                var checkDuplicate = (from b in args.NewDB.ModelTable
        //                                      where b.ModelCode == this.ModelCode

        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();
        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("ModelCode  already exists ", this.ModelCode);
        //                    args.FieldName = "ModelCode";
        //                    return false;
        //                }
        //                return true;
        //            }

        //        case EntityObjectState.Edit:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.ModelTable
        //                                      where (b.ModelCode == this.ModelCode && b.StatusID == (int)StatusValue.Active
        //                                      && b.ModelID != this.ModelID)

        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Model Code already exists ", this.ModelCode);
        //                    args.FieldName = "ModelCode";
        //                    return false;
        //                }

        //                return true;
        //            }

        //        case EntityObjectState.Delete:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.AssetTable
        //                                      where (b.ModelID == this.ModelID)
        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("references found", this.ModelID);
        //                    args.FieldName = "ModelID";
        //                    return false;
        //                }

        //                return true;
        //            }
        //    }
        //    return true;
        //}

        public override PageFieldModelCollection GetCreateScreenControls(string subpageName, int userID)
        {
            var allList = base.GetCreateScreenControls(subpageName, userID);

            PageFieldModelCollection allowedCols = new PageFieldModelCollection()
            {
                new PageFieldModel() { FieldName = "ManufacturerID", IsMandatory = true ,IsMasterCreation=true},
                new PageFieldModel() { FieldName = "ModelCode", IsMandatory = true },
                new PageFieldModel() { FieldName = "ModelName", IsMandatory = true },
            };

            if (AppConfigurationManager.GetValue<bool>("ModelManufacturerCategoryMapping"))
                allowedCols.Add(new PageFieldModel() { FieldName = "CategoryID", IsMandatory = true ,IsMasterCreation=true });
            else
                allowedCols.Add(new PageFieldModel() { FieldName = "CategoryID", IsMandatory = false, IsHidden = true });

            //keep only allowed items, remove remaining
            //allList = allList.Where(b => (from c in allowedCols select c).Contains(b.FieldName) ).ToList();

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