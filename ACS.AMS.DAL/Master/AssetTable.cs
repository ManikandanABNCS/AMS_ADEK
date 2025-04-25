using ACS.AMS.DAL.DBContext;
using ACS.WebAppPageGenerator.Models.SystemModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ACS.AMS.DAL.DBModel
{
    public partial class AssetTable:BaseEntityObject
    {
        public static IQueryable<AssetTable> GetAllAssets(AMSContext db, bool includeInactiveItems = false)
        {
            if (includeInactiveItems)
            {
                return (from b in db.AssetTable.Include("AssetCondition")
                                    .Include("Category").Include("Location").Include("Company").Include("DepreciationClass")
                                    .Include("DisposalType").Include("Model").Include("Product").Include("Section").Include("Status")
                                    .Include("CreatedByNavigation")
                        where b.StatusID != (int)StatusValue.Deleted && b.StatusID != (int)StatusValue.DeletedOLD
                        orderby b.AssetID descending
                        select b);
            }
            else
            {
                return (from b in db.AssetTable.Include("AssetCondition")
                                    .Include("Category").Include("Location").Include("Company").Include("DepreciationClass")
                                    .Include("DisposalType").Include("Model").Include("Product").Include("Section").Include("Status")
                                    .Include("CreatedByNavigation")
                        where b.StatusID != (int)StatusValue.Deleted && b.StatusID != (int)StatusValue.DeletedOLD
                        && b.StatusID != (int)StatusValue.Inactive
                        orderby b.AssetID descending
                        select b);
            }
        }

        public static AssetTable GetAssetBarcode(AMSContext db, string barcode)
        {
            var result = (from b in db.AssetTable where b.Barcode == barcode && b.StatusID != (int)StatusValue.Deleted && b.StatusID != (int)StatusValue.DeletedOLD select b).FirstOrDefault();
            
            return result;
        }

        public static bool GetValidationType(AMSContext db, List<int> assetids)
        {
            var result = (from b in db.AssetNewView join c in assetids on b.AssetID equals c select b.LocationType).Distinct();
                          //where assetids.Contains(b.AssetID) select b.LocationType).Distinct();
            if (result.Count() > 1)
            {
                return true;
            }
            else 
            { 
                return false; 
            }
        }

        public override PageFieldModelCollection GetCreateScreenControls(string subpageName, int userID)
        {
            var allList = base.GetCreateScreenControls(subpageName, userID);

            List<PageFieldModel> allowedCols = new List<PageFieldModel>();

            switch (subpageName)
            {
                case "Basic":
                    allowedCols = new List<PageFieldModel>()
                    {
                        //new PageFieldModel() { FieldName = "Barcode", IsMandatory = true },
                        //new PageFieldModel() { FieldName = "AssetCode", IsMandatory = true },

                        new PageFieldModel() { FieldName = "CategoryID", IsMandatory = true,DisplayLabel="Category" },
                       //  new PageFieldModel() { FieldName = "Category.CatalogueImage", IsMandatory = false,DisplayLabel="CategoryCatalogueImage" },
                        new PageFieldModel() { FieldName = "ProductID", IsMandatory = true,DisplayLabel="AssetDescription" },
                        new PageFieldModel() { FieldName = "AssetDescription", IsHidden=true },
                        new PageFieldModel() { FieldName = "LocationID", IsMandatory = true ,DisplayLabel="Location",IsMasterCreation=true},
                        new PageFieldModel() { FieldName = "DepartmentID", IsMandatory = false ,DisplayLabel="Department",IsMasterCreation=true },
                        new PageFieldModel() { FieldName = "SectionID", IsMandatory = false ,DisplayLabel="Section",IsMasterCreation=true },
                        new PageFieldModel() { FieldName = "CustodianID", IsMandatory = false,DisplayLabel="Custodian",IsMasterCreation=true },
                        new PageFieldModel() { FieldName = "SupplierID", IsMandatory = false,DisplayLabel="Supplier",IsMasterCreation=true },

                        new PageFieldModel() { FieldName = "ManufacturerID", IsMandatory = false,DisplayLabel="Manufacturer",IsMasterCreation=true },
                        new PageFieldModel() { FieldName = "ModelID", IsMandatory = false,DisplayLabel="Model",IsMasterCreation=true },
                        new PageFieldModel() { FieldName = "AssetConditionID", IsMandatory = false,DisplayLabel="AssetCondition",IsMasterCreation=true },
                        new PageFieldModel() { FieldName = "SerialNo", IsMandatory = false },
                        new PageFieldModel() { FieldName = "ReferenceCode", IsMandatory = false },

                        new PageFieldModel() { FieldName = "AssetRemarks", IsMandatory = false },
                    };

                    if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.EnableRFID) == true)
                    {
                        allowedCols.Insert(0, new PageFieldModel() { FieldName = "RFIDTagCode", IsMandatory = true });
                    }

                    if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.BarcodeAutoGenerateEnable) == true)
                    {
                        allowedCols.Insert(0, new PageFieldModel() { FieldName = "Barcode", IsMandatory = true });
                    }

                    if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.BarcodeEqualAssetCode) == true)
                    {
                        allowedCols.Insert(0, new PageFieldModel() { FieldName = "AssetCode", IsMandatory = true });
                    }
                    break;
                case "Catalogue":
                    {
                        PageFieldModelCollection newList1 =
                       [
                           new CataloguePageFieldModel() { FieldName = "Catalogue", IsMandatory = false },
                       
                       ];

                        return newList1;
                    }
                    break;

                case "Finance":
                    allowedCols = new List<PageFieldModel>()
                    {
                        new PageFieldModel() { FieldName = "PONumber", IsMandatory = false },
                        new PageFieldModel() { FieldName = "PurchaseDate", IsMandatory = false },
                        new PageFieldModel() { FieldName = "PurchasePrice", IsMandatory = false },
                        new PageFieldModel() { FieldName = "ComissionDate", IsMandatory = false },
                        new PageFieldModel() { FieldName = "WarrantyExpiryDate", IsMandatory = false },

                        new PageFieldModel() { FieldName = "InvoiceNo", IsMandatory = false },
                        new PageFieldModel() { FieldName = "ReceiptNumber", IsMandatory = false },
                        new PageFieldModel() { FieldName = "DeliveryNote", IsMandatory = false },

                        new PageFieldModel() { FieldName = "DepreciationFlag", IsMandatory = false },
                        new PageFieldModel() { FieldName = "SalvageValue", IsMandatory = false },
                        new PageFieldModel() { FieldName = "DepreciationClassID", IsMandatory = false },
                    };
                    break;

                case "Documents":
                    {
                        allowedCols = new List<PageFieldModel>()
                    {
                        new PageFieldModel() { FieldName = "UploadedDocumentPath", IsMandatory = false,ControlType=PageControlTypes.DocUpload },
                        new PageFieldModel() { FieldName = "UploadedImagePath", IsMandatory = false ,ControlType=PageControlTypes.ImageUpload },
                        };
                    }
                    break;

                case "Others":
                    {
                        
                            
                        //Show the attributes
                    }
                    break;

                case "Activity":
                    {
                        //Show the Audit log & Transaction of the asset
                    }
                    break;
                case "ApprovalHistory":
                    {
                        PageFieldModelCollection newList1 =
                        [
                            new QueryPageFieldModel() { FieldName = "ApprovalHistory", IsMandatory = false, ProcedureName = "prc_ApprovalHistoryDetails", Param1Name = "AssetID" }
                        ];

                        return newList1;
                    }
                case "Integration":
                    {
                        PageFieldModelCollection newList1 =
                        [
                            new QueryPageFieldModel() { FieldName = "Integration", IsMandatory = false, ProcedureName = "zDOF_AssetIntegrationDetails", Param1Name = "AssetID" }
                        ];

                        return newList1;
                    }
            }

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
                    itm.IsMasterCreation = c.IsMasterCreation;
                    newList.Add(itm);
                }
            }

            //add remaining fields as hidden
            var removedList = allList.Where(b => !(from c in newList select c.FieldName).Contains(b.FieldName));
            foreach (var c in removedList)
            {
                c.IsHidden = true;
                newList.Add(c);
            }

            return newList;
        }

        private void ValidateAssetProduct(AMSContext db)
        {
            if(this.ProductID > 0)
            {
                var oldProductExists = (from b in db.ProductTable
                                 where b.CategoryID == this.CategoryID 
                                        && b.StatusID != (int)StatusValue.Deleted
                                        && b.ProductName == this.AssetDescription
                                        && b.ProductID == this.ProductID
                                 select b).Any();

                if (oldProductExists) return;
            }

            //Add the product if its not exists
            var oldProduct = (from b in db.ProductTable
                                 where b.CategoryID == this.CategoryID && b.StatusID != (int)StatusValue.Deleted
                                        && b.ProductName == this.AssetDescription
                                 select b).FirstOrDefault();
            if(oldProduct == null)
            {
                this.Product = ProductTable.CreateProduct(db, this.AssetDescription, this.CategoryID);
            }
            else
            {
                this.ProductID = oldProduct.ProductID;
            }
        }

        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.AssetTable
                                  where b.Barcode == this.Barcode
                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();
            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Barcode already exists ", this.Barcode);
                args.FieldName = "Barcode";
                return false;
            }

            var checkAssetDuplicate = (from b in args.NewDB.AssetTable
                                       where b.AssetCode == this.AssetCode
                                       && b.StatusID == (int)StatusValue.Active
                                       select b).Count();
            if (checkAssetDuplicate > 0)
            {
                args.ErrorMessage = string.Format("AssetCode already exists ", this.AssetCode);
                args.FieldName = "AssetCode";
                return false;
            }

            //validate product for the assets
            ValidateAssetProduct(args.CurrentDB);

            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.AssetTable
                                  where (b.Barcode == this.Barcode 
                                        && b.StatusID == (int)StatusValue.Active
                                        && b.AssetID != this.AssetID)
                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Barcode already exists ", this.Barcode);
                args.FieldName = "Barcode";
                return false;
            }

            var checkassetDuplicate = (from b in args.NewDB.AssetTable
                                       where (b.AssetCode == this.AssetCode 
                                            && b.StatusID == (int)StatusValue.Active
                                            && b.AssetID != this.AssetID)
                                       select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Asset Code already exists ", this.AssetCode);
                args.FieldName = "AssetCode";
                return false;
            }

            //validate product for the assets
            ValidateAssetProduct(args.CurrentDB);

            return true;
        }

        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.TransactionLineItemTable
                                  where (b.AssetID == this.AssetID)
                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("references found", this.AssetID);
                args.FieldName = "AssetID";
                return false;
            }
            return true;
        }
        
        public static IQueryable<AssetTable> GetCheckBarcodeDetails(AMSContext db, List<int> barcode)
        {
            IQueryable<AssetTable> result = (from b in db.AssetTable
                                             select b);
            if (barcode.Count() > 0)
            {
                result = (from b in result join c in barcode on b.AssetID equals c select b);//result.Where(a => barcode.Contains(a.AssetID));
            }

            return result;
        }
    }
}
