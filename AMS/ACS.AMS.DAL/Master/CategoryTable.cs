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
    public partial class CategoryTable : BaseEntityObject
    {
        public static CategoryTable GetCateory(AMSContext db, string code)
        {
            return db.CategoryTable.Where(b => b.CategoryCode == code).FirstOrDefault();
        }

        public static IQueryable<CategoryTable> GetAllCategorys(AMSContext db, bool includeInactiveItems = false)
        {
            var result = from b in db.CategoryTable
                         where b.StatusID != (int)StatusValue.Deleted && b.StatusID != (int)StatusValue.DeletedOLD
                         && (includeInactiveItems == false ? b.StatusID != (int)StatusValue.Inactive : (b.StatusID != (int)StatusValue.Deleted) && b.StatusID != (int)StatusValue.DeletedOLD)
                         orderby b.CategoryCode
                         select b;
            return result;
        }

        public static CategoryTable GetCategory(AMSContext db, int categoryID)
        {
            var result = (from b in db.CategoryTable.Include("ParentCategory")
                          where b.StatusID != (int)StatusValue.Deleted && b.StatusID != (int)StatusValue.DeletedOLD && b.CategoryID == categoryID
                          orderby b.CategoryCode
                          select b).FirstOrDefault();
            return result;
        }

        public override PageFieldModelCollection GetCreateScreenControls(string subpageName, int userID)
        {
            var allList = base.GetCreateScreenControls(subpageName, userID);

            PageFieldModelCollection allowedCols = new PageFieldModelCollection()
            {
                new PageFieldModel() { FieldName = "ParentCategoryID", IsMandatory = false },
                new PageFieldModel() { FieldName = "CategoryCode", IsMandatory = true,StringMaxLength=100 },
                new PageFieldModel() { FieldName = "CategoryName", IsMandatory = true ,StringMaxLength=100},
            };

            if (AppConfigurationManager.GetValue<bool>("IsMandatoryCategoryType"))
            {
                allowedCols.Add(new PageFieldModel() { FieldName = "CategoryTypeID", IsMandatory = true ,IsMasterCreation=false});
            }
            else
            {
                var data = CategoryTypeTable.GetCategoryTypeDetails(AMSContext.CreateNewContext(), "All");
                allowedCols.Add(new PageFieldModel() { FieldName = "CategoryTypeID", IsHidden = true, DefaultValue = (int)data.CategoryTypeID, IsMasterCreation = false });
            }

            allowedCols.Add(new PageFieldModel() { FieldName = "CatalogueImage", IsMandatory = false,ControlType=PageControlTypes.ImageUpload });

            //if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.DepreciationClass))
            //    allowedCols.Add(new PageFieldModel() { FieldName = "CategoryTypeID", IsMandatory = true });
            // if (!base.HasRights("Depreciation", UserRightValue.Index))

            allowedCols.Add(new PageFieldModel() { FieldName = "DepreciationClassID", IsMandatory = false, IsMasterCreation = false });

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

        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.CategoryTable
                                  where b.CategoryCode == this.CategoryCode

                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();
            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Category Code already exists ", this.CategoryCode);
                args.FieldName = "CategoryCode";
                return false;
            }
            if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.IsMandatoryCategoryType))
            {
                int level = AppConfigurationManager.GetValue<int>(AppConfigurationManager.PreferredLevelCategoryMapping);

                if (this.ParentCategoryID == null && this.CategoryTypeID != null && level > 1)
                {
                    args.ErrorMessage = string.Format("Category Type can be changed only at the Preferred Level ", this.CategoryCode);
                    args.FieldName = "CategoryCode";
                    return false;
                }

                if (this.ParentCategoryID != null)
                {
                    var loc = (from b in args.NewDB.CategoryNewView where b.CategoryID == this.ParentCategoryID select b).FirstOrDefault();
                    if (loc != null)
                    {
                        if (loc.ParentCategoryID != null && this.CategoryTypeID != null)
                        {
                            if ((level - 1) != loc.Level)
                            {
                                args.ErrorMessage = string.Format("Category Type can be changed only at the Preferred Level ", this.CategoryCode);
                                args.FieldName = "CategoryCode";
                                return false;
                            }
                        }
                        else if (loc.ParentCategoryID == null && this.CategoryTypeID == null)
                        {
                            if ((level - 1) == loc.Level)
                            {
                                args.ErrorMessage = string.Format("Category Type mandatory for Preferred Level Category  ", this.CategoryCode);
                                args.FieldName = "CategoryCode";
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.CategoryTable
                                  where (b.CategoryCode == this.CategoryCode && b.StatusID == (int)StatusValue.Active
                                  && b.CategoryID != this.CategoryID)

                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Category Code already exists ", this.CategoryCode);
                args.FieldName = "CategoryCode";
                return false;
            }

            if (this.IsInventory == null)
                this.IsInventory = false;

            return true;
        }

        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.AssetTable
                                  where (b.CategoryID == this.CategoryID)
                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("references found", this.CategoryID);
                args.FieldName = "CategoryCode";
                return false;
            }
            var checkTemplateDuplicate = (from b in args.NewDB.ProductTable
                                          where (b.CategoryID == this.CategoryID)
                                          && b.StatusID == (int)StatusValue.Active
                                          select b).Count();

            if (checkTemplateDuplicate > 0)
            {
                args.ErrorMessage = string.Format("references found", this.CategoryID);
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
        //                var checkDuplicate = (from b in args.NewDB.CategoryTable
        //                                      where b.CategoryCode == this.CategoryCode

        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();
        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Category Code already exists ", this.CategoryCode);
        //                    args.FieldName = "CategoryCode";
        //                    return false;
        //                }
        //                return true;
        //            }

        //        case EntityObjectState.Edit:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.CategoryTable
        //                                      where (b.CategoryCode == this.CategoryCode && b.StatusID == (int)StatusValue.Active
        //                                      && b.CategoryID != this.CategoryID)

        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Category Code already exists ", this.CategoryCode);
        //                    args.FieldName = "CategoryCode";
        //                    return false;
        //                }

        //                return true;
        //            }

        //        case EntityObjectState.Delete:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.AssetTable
        //                                      where (b.CategoryID == this.CategoryID)
        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("references found", this.CategoryID);
        //                    args.FieldName = "CategoryCode";
        //                    return false;
        //                }
        //                var checkTemplateDuplicate = (from b in args.NewDB.ProductTable
        //                                              where (b.CategoryID == this.CategoryID)
        //                                              && b.StatusID == (int)StatusValue.Active
        //                                              select b).Count();

        //                if (checkTemplateDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("references found", this.CategoryID);
        //                    args.FieldName = "CategoryCode";
        //                    return false;
        //                }

        //                return true;
        //            }
        //    }
        //    return true;
        //}

        public static string GetName(AMSContext _db, int id, string controlID)
        {
            string name = string.Empty;
            if (id > 0)
            {
                if (string.Compare(controlID, "CategoryID") == 0 || string.Compare(controlID, "ParentCategoryID") == 0)
                {
                    name = CategoryTable.GetItem(_db, id).CategoryName;
                }
                else
                {
                    name = LocationTable.GetItem(_db, id).LocationName;
                }
            }
            return name;
        }

    }
}