using ACS.AMS.DAL;
using ACS.WebAppPageGenerator.Models.SystemModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ACS.WebAppPageGenerator.Models.SystemModels
{
    public static class PageGenerationHelper
    {
        #region Static Fields

        public static List<string> commonFields = new List<string>()
        {
            "CreatedBy",
            "CreatedDateTime",
            "LastModifyBy",
            "LastModifiedBy",
            "LastModifiedDate",
            "LastModifiedDateTime",
            "CurrentPageID",
            "StatusID"
        };

        public static List<string> noNeedToDisplayFields = new List<string>()
        {
            "CurrencyStartDate",
            "CurrencyEndDate",
            "TransactionSubType",
            "CreatedFrom",
             "SourceDocumentNo"
        };

        public static List<string> AssetFields = new List<string>()
        {
            "CreateFromHHT",
            "ProjectID",
            "WIPAssetID",
            "MappedAssetID",
            "MailAlert",
            "IsTransfered",
            "TransferTypeID",
            "AssetApproval",
            "InsertedToOracle",
            "DistributionID",
            "ClassificationID",
            "CompanyID",
            "SyncDateTime",
            "QFAssetCode",
            "DOFPO_LINE_NUM",
            "DOFMASS_ADDITION_ID",
            "ERPUpdateType",
            "DOFPARENT_MASS_ADDITION_ID",
            "DOFFIXED_ASSETS_UNITS",
            "zDOF_Asset_Updated",
            "DOF_MASS_SPLIT_EXECUTED",
            "AllowTransfer",
            "ConsultantID",
            "LastScannedLocationID",
            "IsScanned",
            "NetbookValue",
            "zDOFAssetNumber",
            "zDOF_UpdateErrorOccurred",
            "DisposalReferenceNo",
            "DisposedDateTime",
            "DisposedRemarks",
            "DisposalValue",
            "DisposalTypeID",
            "CurrentCost",
            "ProceedofSales",
            "SoldTo",
            "RetirementTypeID",
            "CostOfRemoval"
        };

        #endregion

        public static PageFieldModelCollection CreateDefaultPage(Type type, string subpageName, int userID, BaseEntityObject obj = null)
        {
            var fieldLists = type.GetProperties();
            var entityInstance = Activator.CreateInstance(type);
            var baseEntityInstance = entityInstance as BaseEntityObject;

            if (baseEntityInstance == null)
                throw new InvalidDataException($"{type} is not derived from BaseEntityObject");

            string primaryKeyFieldName = baseEntityInstance.GetPrimaryKeyFieldName();

            PageFieldModelCollection allowedFields = new PageFieldModelCollection();
            foreach (var field in fieldLists)
            {
                var propertyType = field.PropertyType;
                string fieldLabelTitle = field.Name;

                //its a non null field
                if (Nullable.GetUnderlyingType(field.PropertyType) != null)
                {
                    propertyType = Nullable.GetUnderlyingType(field.PropertyType);
                }

                if ((propertyType != typeof(string)) && (propertyType.IsClass))
                    continue;

                if (propertyType.IsGenericType) continue;
                if (field.Name == primaryKeyFieldName) continue;
                if (commonFields.Contains(field.Name)) continue;
                if (noNeedToDisplayFields.Contains(field.Name)) continue;
                if (AssetFields.Contains(field.Name)) continue;
                if (field.Name.StartsWith("Attribute")) continue;

                //if (string.Compare(PageName, "Manufacturer") == 0 && !AppConfigurationManager.GetValue<bool>(AppConfigurationManager.ManufacturerCategoryMapping))
                //{
                //    if (field.Name.Contains("CategoryID")) continue;
                //}
                //if (string.Compare(PageName, "Model") == 0 && !AppConfigurationManager.GetValue<bool>(AppConfigurationManager.ModelManufacturerCategoryMapping))
                //{
                //    if (field.Name.Contains("CategoryID")) continue;
                //}

                //if (string.Compare(PageName, "Location", true) == 0)
                //{
                //    if (LocationFields.Contains(field.Name) == false) continue;
                //}
                //if (string.Compare(PageName, "Category", true) == 0)
                //{
                //    if (CategoryFields.Contains(field.Name) == false) continue;
                //}

                //if (transactionFields != null)
                //{
                //    if (transactionFields.Where(b => string.Compare(field.Name, b.FieldName, true) == 0).Any())
                //        allowedFields.Add(field);
                //}
                //else

                var customAtts = field.CustomAttributes;
                int stringMaxLength = 1000;
                bool IsMandatory = false;

                foreach (var customAttribute in customAtts)
                {
                    if (customAttribute.AttributeType == typeof(StringLengthAttribute))
                    {
                        stringMaxLength = (int)customAttribute.ConstructorArguments[0].Value;
                    }

                    if (customAttribute.AttributeType == typeof(RequiredAttribute))
                    {
                        IsMandatory = true;
                    }

                    if (customAttribute.AttributeType == typeof(DisplayNameAttribute))
                    {
                        fieldLabelTitle = customAttribute.ConstructorArguments[0].Value + "";
                    }

                    if (customAttribute.AttributeType.Name == "NullableAttribute")
                    {
                        IsMandatory = false;
                    }
                }

                allowedFields.Add(
                        new PageFieldModel()
                        {
                            FieldName = field.Name,
                            ControlName = field.Name,
                            DisplayLabel = fieldLabelTitle,
                            IsMandatory = IsMandatory,
                            PropertyType = propertyType,

                            StringMaxLength = stringMaxLength
                        });

            }

            return allowedFields;
        }

        public static void CopyFieldsProperties(PageFieldModelCollection source, PageFieldModelCollection destList)
        {
            foreach (var dest in destList)
            {
                var srcField = source.FirstOrDefault(x => x.FieldName.Equals(dest.FieldName, StringComparison.OrdinalIgnoreCase));

                if (srcField != null)
                {
                    dest.PropertyType = srcField.PropertyType;
                    dest.IsMandatory = srcField.IsMandatory;
                    dest.StringMaxLength = srcField.StringMaxLength;
                }
            }
        }
    }
}
