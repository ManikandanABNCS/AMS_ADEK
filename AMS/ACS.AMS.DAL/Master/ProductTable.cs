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
    public partial class ProductTable : BaseEntityObject
    {
        public static ProductTable GetProduct(AMSContext db, string code)
        {
            return db.ProductTable.Include("Category").Where(b => b.ProductCode == code).FirstOrDefault();
        }
        public static ProductTable GetProductDetails(AMSContext db, int id)
        {
            return db.ProductTable.Include("Category").Where(b => b.ProductID ==id).FirstOrDefault();
        }
        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.ProductTable
                                  where b.ProductCode == this.ProductCode

                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();
            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Product Code already exists ", this.ProductCode);
                args.FieldName = "ProductCode";
                return false;
            }
            if (!string.IsNullOrEmpty(this.VirtualBarcode))
            {
                var checkVirtualBarcodeDuplicate = (from b in args.NewDB.ProductTable
                                                    where b.VirtualBarcode == this.VirtualBarcode

                                                    && b.StatusID == (int)StatusValue.Active
                                                    select b).Count();
                if (checkVirtualBarcodeDuplicate > 0)
                {
                    args.ErrorMessage = string.Format("Virtual Barcode already exists ", this.VirtualBarcode);
                    args.FieldName = "VirtualBarcode";
                    return false;
                }
            }
            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.ProductTable
                                  where (b.ProductCode == this.ProductCode && b.StatusID == (int)StatusValue.Active
                                  && b.ProductID != this.ProductID)

                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Product Code already exists ", this.ProductCode);
                args.FieldName = "ProductCode";
                return false;
            }
            if (!string.IsNullOrEmpty(this.VirtualBarcode))
            {
                var checkVirtualBarcodeDuplicate = (from b in args.NewDB.ProductTable
                                                    where (b.VirtualBarcode == this.VirtualBarcode && b.StatusID == (int)StatusValue.Active
                                                    && b.ProductID != this.ProductID)

                                                    select b).Count();

                if (checkVirtualBarcodeDuplicate > 0)
                {
                    args.ErrorMessage = string.Format("Virtual Barcode already exists ", this.VirtualBarcode);
                    args.FieldName = "VirtualBarcode";
                    return false;
                }
            }
            return true;
        }

        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.AssetTable
                                  where (b.ProductID == this.ProductID)
                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("references found", this.ProductID);
                args.FieldName = "ProductCode";
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
        //                var checkDuplicate = (from b in args.NewDB.ProductTable
        //                                      where b.ProductCode == this.ProductCode

        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();
        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Product Code already exists ", this.ProductCode);
        //                    args.FieldName = "ProductCode";
        //                    return false;
        //                }
        //                return true;
        //            }

        //        case EntityObjectState.Edit:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.ProductTable
        //                                      where (b.ProductCode == this.ProductCode && b.StatusID == (int)StatusValue.Active
        //                                      && b.ProductID != this.ProductID)

        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Product Code already exists ", this.ProductCode);
        //                    args.FieldName = "ProductCode";
        //                    return false;
        //                }

        //                return true;
        //            }

        //        case EntityObjectState.Delete:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.AssetTable
        //                                      where (b.ProductID == this.ProductID)
        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("references found", this.ProductID);
        //                    args.FieldName = "ProductCode";
        //                    return false;
        //                }
                       

        //                return true;
        //            }
        //    }
        //    return true;
        //}
        
        public static IQueryable<ProductTable> GetCategoryAgainProduct(AMSContext _db,int categoryID)
        {
            var res = (from b in _db.ProductTable 
                       where b.CategoryID == categoryID 
                            && b.StatusID != (int)StatusValue.Deleted 
                            && b.StatusID != (int)StatusValue.DeletedOLD 
                       select b);

            return res;
        }

        public static ProductTable CreateProduct(AMSContext db, string productName, int categoryID)
        {
            ProductTable productTable = new ProductTable()
            {
                ProductName = productName,
                CategoryID = categoryID,
                ProductCode = DateTime.UtcNow.ToString("yyddMMHHmmss"),

                CreatedBy = db.CurrentUserID,
                CreatedDateTime = DateTime.Now,
                StatusID = (int)StatusValue.Active,
            };

            db.Add(productTable);

            //Pull the language table data and push the desc table

            return productTable;
        }
        public override PageFieldModelCollection GetCreateScreenControls(string subpageName, int userID)
        {
            var allList = base.GetCreateScreenControls(subpageName, userID);

            PageFieldModelCollection allowedCols = new PageFieldModelCollection()
            {
                new PageFieldModel() { FieldName = "CategoryID", IsMandatory = true,IsMasterCreation=true },
                new PageFieldModel() { FieldName = "ProductCode", IsMandatory = true,StringMaxLength=20 },
                new PageFieldModel() { FieldName = "ProductName", IsMandatory = true,StringMaxLength=200 },
                new PageFieldModel() { FieldName = "CatalogueImage", IsMandatory = false,ControlType=PageControlTypes.ImageUpload },
                new PageFieldModel() { FieldName = "VirtualBarcode", IsMandatory = false,StringMaxLength=200 },
                new PageFieldModel() { FieldName = "Specification", IsMandatory = false,StringMaxLength=4000 ,ControlType=PageControlTypes.TextArea },
                new PageFieldModel() { FieldName = "Remarks", IsMandatory = false,StringMaxLength=4000 ,ControlType=PageControlTypes.TextArea },
                 new PageFieldModel() { FieldName = "IsInventoryItem", IsMandatory = false,IsHidden=true,DefaultValue=false },
                  new PageFieldModel() { FieldName = "IsSerialNumberRequired", IsMandatory = false,IsHidden=true,DefaultValue=false },
                   new PageFieldModel() { FieldName = "UOMID", IsMandatory = false,IsHidden=true },
                    new PageFieldModel() { FieldName = "ReorderLevelQuantity", IsMandatory = false,IsHidden=true },
                     new PageFieldModel() { FieldName = "Price", IsMandatory = false,IsHidden=true },

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
