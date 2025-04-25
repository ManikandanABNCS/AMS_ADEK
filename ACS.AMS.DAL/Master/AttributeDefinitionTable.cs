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
using System.ComponentModel;
using System.Reflection;

namespace ACS.AMS.DAL.DBModel
{
    public partial class AttributeDefinitionTable : BaseEntityObject
    {
        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            if (string.IsNullOrEmpty(this.AttributeName.Trim()))
            {
                args.FieldName = "AttributeName"; //this is an parent table - so add ItemName_ infront of each fields
                args.ErrorMessage = string.Format(Language.GetErrorMessage("AttributeNameNotAllowedBlankSpace"), Language.GetFieldName("AttributeName", CultureHelper.EnglishCultureSymbol), AttributeName);
                return false;
            }
            var existingItem = (from b in args.NewDB.AttributeDefinitionTable
                                where b.StatusID != (int)StatusValue.Deleted && (b.AttributeName.ToUpper() == this.AttributeName.ToUpper()) &&
                                b.EntityID == this.EntityID
                                select new { AttributeName = b.AttributeName }).FirstOrDefault();
            if (existingItem != null)
            {

                if (string.Compare(existingItem.AttributeName, this.AttributeName.Trim(), true) == 0)
                {
                    args.FieldName = "AttributeName"; //this is an parent table - so add ItemName_ infront of each fields
                    args.ErrorMessage = string.Format(Language.GetErrorMessage("AttributeNameAlreadyExists"), Language.GetFieldName("AttributeName", CultureHelper.EnglishCultureSymbol), AttributeName);
                    return false;
                }

            }
            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            var existingItem = (from b in args.NewDB.AttributeDefinitionTable
                                where b.AttributeDefinitionID != this.AttributeDefinitionID && b.StatusID != (int)StatusValue.Deleted
                                && (b.AttributeName.ToUpper() == this.AttributeName.ToUpper()) && b.EntityID == this.EntityID
                                select new { AttributeName = b.AttributeName }).FirstOrDefault();
            if (existingItem != null)
            {
                if (string.Compare(existingItem.AttributeName, this.AttributeName.Trim(), true) == 0)
                {
                    args.FieldName = "AttributeName"; //this is an parent table - so add ItemName_ infront of each fields
                    args.ErrorMessage = string.Format(Language.GetErrorMessage("TableNameAlreadyExists"), Language.GetFieldName("AttributeName", CultureHelper.EnglishCultureSymbol), AttributeName);
                    return false;
                }
            }

            return true;
        }
        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            if (this.EntityID == (int)EntityValues.CategoryTable)
            {
                var category = (from b in args.NewDB.CategoryTable where this.AttributeName != null select b);
                int cnt = category.GroupBy(a => a.CategoryID).Count();
                if (cnt > 0)
                {
                    args.FieldName = "AttributeName"; //this is an parent table - so add ItemName_ infront of each fields
                    args.ErrorMessage = string.Format(Language.GetErrorMessage("ReferencesFound"), Language.GetFieldName("AttributeName", CultureHelper.EnglishCultureSymbol), AttributeName);
                    return false;
                }
            }
            if (this.EntityID == (int)EntityValues.LocationTable)
            {
                var location = (from b in args.NewDB.LocationTable where this.AttributeName != null select b);
                int cnt = location.GroupBy(a => a.LocationID).Count();
                if (cnt > 0)
                {
                    args.FieldName = "AttributeName"; //this is an parent table - so add ItemName_ infront of each fields
                    args.ErrorMessage = string.Format(Language.GetErrorMessage("ReferencesFound"), Language.GetFieldName("AttributeName", CultureHelper.EnglishCultureSymbol), AttributeName);
                    return false;
                }
            }
            if (this.EntityID == (int)EntityValues.AssetTable)
            {
                var asset = (from b in args.NewDB.AssetTable where this.AttributeName != null select b);
                int cnt = asset.GroupBy(a => a.AssetID).Count();
                if (cnt > 0)
                {
                    args.FieldName = "AttributeName"; //this is an parent table - so add ItemName_ infront of each fields
                    args.ErrorMessage = string.Format(Language.GetErrorMessage("ReferencesFound"), Language.GetFieldName("AttributeName", CultureHelper.EnglishCultureSymbol), AttributeName);
                    return false;
                }
            }
            if (this.EntityID == (int)EntityValues.SectionTable)
            {
                var section = (from b in args.NewDB.SectionTable where this.AttributeName != null select b);
                int cnt = section.GroupBy(a => a.SectionID).Count();
                if (cnt > 0)
                {
                    args.FieldName = "AttributeName"; //this is an parent table - so add ItemName_ infront of each fields
                    args.ErrorMessage = string.Format(Language.GetErrorMessage("ReferencesFound"), Language.GetFieldName("AttributeName", CultureHelper.EnglishCultureSymbol), AttributeName);
                    return false;
                }
            }
            if (this.EntityID == (int)EntityValues.DepartmentTable)
            {
                var dept = (from b in args.NewDB.DepartmentTable where this.AttributeName != null select b);
                int cnt = dept.GroupBy(a => a.DepartmentID).Count();
                if (cnt > 0)
                {
                    args.FieldName = "AttributeName"; //this is an parent table - so add ItemName_ infront of each fields
                    args.ErrorMessage = string.Format(Language.GetErrorMessage("ReferencesFound"), Language.GetFieldName("AttributeName", CultureHelper.EnglishCultureSymbol), AttributeName);
                    return false;
                }
            }
            if (this.EntityID == (int)EntityValues.TranferTypeTable)
            {
                var transfer = (from b in args.NewDB.TransferTypeTable where this.AttributeName != null select b);
                int cnt = transfer.GroupBy(a => a.TransferTypeID).Count();
                if (cnt > 0)
                {
                    args.FieldName = "AttributeName"; //this is an parent table - so add ItemName_ infront of each fields
                    args.ErrorMessage = string.Format(Language.GetErrorMessage("ReferencesFound"), Language.GetFieldName("AttributeName", CultureHelper.EnglishCultureSymbol), AttributeName);
                    return false;
                }
            }
            if (this.EntityID == (int)EntityValues.SupplierTable)
            {
                var supplier = (from b in args.NewDB.PartyTable where this.AttributeName != null select b);
                int cnt = supplier.GroupBy(a => a.PartyID).Count();
                if (cnt > 0)
                {
                    args.FieldName = "AttributeName"; //this is an parent table - so add ItemName_ infront of each fields
                    args.ErrorMessage = string.Format(Language.GetErrorMessage("ReferencesFound"), Language.GetFieldName("AttributeName", CultureHelper.EnglishCultureSymbol), AttributeName);
                    return false;
                }
            }
            if (this.EntityID == (int)EntityValues.ManufacturerTable)
            {
                var manu = (from b in args.NewDB.ManufacturerTable where this.AttributeName != null select b);
                int cnt = manu.GroupBy(a => a.ManufacturerID).Count();
                if (cnt > 0)
                {
                    args.FieldName = "AttributeName"; //this is an parent table - so add ItemName_ infront of each fields
                    args.ErrorMessage = string.Format(Language.GetErrorMessage("ReferencesFound"), Language.GetFieldName("AttributeName", CultureHelper.EnglishCultureSymbol), AttributeName);
                    return false;
                }
            }
            if (this.EntityID == (int)EntityValues.ModelTable)
            {
                var model = (from b in args.NewDB.ModelTable where this.AttributeName != null select b);
                int cnt = model.GroupBy(a => a.ModelID).Count();
                if (cnt > 0)
                {
                    args.FieldName = "AttributeName"; //this is an parent table - so add ItemName_ infront of each fields
                    args.ErrorMessage = string.Format(Language.GetErrorMessage("ReferencesFound"), Language.GetFieldName("AttributeName", CultureHelper.EnglishCultureSymbol), AttributeName);
                    return false;
                }
            }
            if (this.EntityID == (int)EntityValues.CompanyTable)
            {
                var comp = (from b in args.NewDB.CompanyTable where this.AttributeName != null select b);
                int cnt = comp.GroupBy(a => a.CompanyID).Count();
                if (cnt > 0)
                {
                    args.FieldName = "AttributeName"; //this is an parent table - so add ItemName_ infront of each fields
                    args.ErrorMessage = string.Format(Language.GetErrorMessage("ReferencesFound"), Language.GetFieldName("AttributeName", CultureHelper.EnglishCultureSymbol), AttributeName);
                    return false;
                }
            }
            if (this.EntityID == (int)EntityValues.UomTable)
            {
                var uom = (from b in args.NewDB.UOMTable where this.AttributeName != null select b);
                int cnt = uom.GroupBy(a => a.UOMID).Count();
                if (cnt > 0)
                {
                    args.FieldName = "AttributeName"; //this is an parent table - so add ItemName_ infront of each fields
                    args.ErrorMessage = string.Format(Language.GetErrorMessage("ReferencesFound"), Language.GetFieldName("AttributeName", CultureHelper.EnglishCultureSymbol), AttributeName);
                    return false;
                }
            }
            if (this.EntityID == (int)EntityValues.ProductTable)
            {
                var product = (from b in args.NewDB.ProductTable where this.AttributeName != null select b);
                int cnt = product.GroupBy(a => a.ProductID).Count();
                if (cnt > 0)
                {
                    args.FieldName = "AttributeName"; //this is an parent table - so add ItemName_ infront of each fields
                    args.ErrorMessage = string.Format(Language.GetErrorMessage("ReferencesFound"), Language.GetFieldName("AttributeName", CultureHelper.EnglishCultureSymbol), AttributeName);
                    return false;
                }
            }
            if (this.EntityID == (int)EntityValues.PersonTable)
            {
                var person = (from b in args.NewDB.PersonTable where this.AttributeName != null select b);
                int cnt = person.GroupBy(a => a.PersonID).Count();
                if (cnt > 0)
                {
                    args.FieldName = "AttributeName"; //this is an parent table - so add ItemName_ infront of each fields
                    args.ErrorMessage = string.Format(Language.GetErrorMessage("ReferencesFound"), Language.GetFieldName("AttributeName", CultureHelper.EnglishCultureSymbol), AttributeName);
                    return false;
                }
            }
            if (this.EntityID == (int)EntityValues.DisposalTypeTable)
            {
                var disposal = (from b in args.NewDB.DisposalTypeTable where this.AttributeName != null select b);
                int cnt = disposal.GroupBy(a => a.DisposalTypeID).Count();
                if (cnt > 0)
                {
                    args.FieldName = "AttributeName"; //this is an parent table - so add ItemName_ infront of each fields
                    args.ErrorMessage = string.Format(Language.GetErrorMessage("ReferencesFound"), Language.GetFieldName("AttributeName", CultureHelper.EnglishCultureSymbol), AttributeName);
                    return false;
                }
            }
            if (this.EntityID == (int)EntityValues.CountryTable)
            {
                var country = (from b in args.NewDB.CountryTable where this.AttributeName != null select b);
                int cnt = country.GroupBy(a => a.CountryID).Count();
                if (cnt > 0)
                {
                    args.FieldName = "AttributeName"; //this is an parent table - so add ItemName_ infront of each fields
                    args.ErrorMessage = string.Format(Language.GetErrorMessage("ReferencesFound"), Language.GetFieldName("AttributeName", CultureHelper.EnglishCultureSymbol), AttributeName);
                    return false;
                }
            }
            return true;
        }
        public override PageFieldModelCollection GetCreateScreenControls(string subpageName, int userID)
        {
            var allList = base.GetCreateScreenControls(subpageName, userID);

            PageFieldModelCollection allowedCols = new PageFieldModelCollection()
            {
                new PageFieldModel() { FieldName = "EntityID", IsMandatory = true },
                new PageFieldModel() { FieldName = "AttributeName", IsMandatory = true,ControlName="AttributeSelection",ControlType=PageControlTypes.MultiColumnComboBox,DataReadScriptFunctionName ="AttributeData"},
                new PageFieldModel() { FieldName = "DisplayTitle", IsMandatory = true },
                new PageFieldModel() { FieldName = "ToolTipName", IsMandatory = true },
                new PageFieldModel() { FieldName = "ToolTipName", IsMandatory = true },
                new PageFieldModel() { FieldName = "DataType", IsMandatory = true },
                new PageFieldModel() { FieldName = "DisplayControl", IsMandatory = true },
                  new PageFieldModel() { FieldName = "IsMandatory", IsMandatory = true },
                       new PageFieldModel() { FieldName = "StringSize", IsMandatory = true },
                       new PageFieldModel() { FieldName = "MinValue", IsMandatory = false ,ControlType=PageControlTypes.DatePicker},
                        new PageFieldModel() { FieldName = "MaxValue", IsMandatory = false ,ControlType=PageControlTypes.DatePicker},
                          new PageFieldModel() { FieldName = "StepIncrement", IsMandatory = false },
                           new PageFieldModel() { FieldName = "DisplayOrderID", IsMandatory = false,IsHidden=true},


            };

          

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
                    itm.DataReadScriptFunctionName = c.DataReadScriptFunctionName;
                    newList.Add(itm);

                }
            }

            return newList;
        }
        public static List<string> GetTableWiseAttribute(AMSContext db, string tableName, string dataType, bool withFilter = false)
        {
            string attribute = "Attribute";
            var nameSpace = Assembly.GetExecutingAssembly().GetName().Name;
            string tName = nameSpace + "." + tableName;
            TypeConverter p = TypeDescriptor.GetConverter(tName);
            object propValue = p.ConvertFromString(tName);
            Type elementType = Type.GetType(propValue + "");

            int applicationTableListID = 0;
            var applicationList = DynamicColumnRequiredEntityTable.GetDynamicColumnsForEntity(db, tableName);
            if (applicationList != null)
                applicationTableListID = applicationList.DynamicColumnRequiredEntityID;

            var attributelist = (from b in db.AttributeDefinitionTable
                                 where b.StatusID == (int)StatusValue.Active && b.EntityID == applicationTableListID && b.AttributeName.StartsWith(attribute)
                                 orderby b.AttributeDefinitionID
                                 select b.AttributeName).ToList();
            if (elementType == null)
            {
                return null;
            }
            var qry = elementType.GetProperties().Where(t => t.Name.StartsWith(attribute)).OrderBy(t => t.Name).Select(t => t.Name);

            if (withFilter)
            {
                var query = (from c in qry
                             where !attributelist.Contains(c)
                             //where !(from o in attributelist
                             //        select o.AttributeName)
                             //       .Contains(c)
                             select c);
                return query.ToList();
            }
            return qry.ToList();
        }

    }
}

