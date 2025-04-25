using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections;
using System.Threading;
using ACS.AMS.DAL;
using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System.ComponentModel;
using System.Threading.Tasks;

namespace ACS.AMS.DAL
{

    public class AppConfigurationManager
    {
        public static IDistributedCache _cache;

       private static AMSContext _db;
        private const string CacheContentName = "__ConfigurationContent__";
        public const string DepartmentName = "DepartmentName";
        public const string BarcodeEqualAssetCode = "BarcodeEqualAssetCode";
        public const string BarcodeAutoGenerateEnable = "BarcodeAutoGenerateEnable";
        public const string ActiveDirectoryEnabled = "ActiveDirectoryEnable";
        public const string UserDepartmentMapping = "UserDepartmentMapping";
        public const string DepartmentCustodianMapping = "DepartmentCustodianMapping";
        public const string PeriodStartDate = "PeriodStartDate";
        public const string LocationName = "LocationName";
        public const string SectionName = "SectionName";
        public const string CustodianName = "CustodianName";
        public const string UomName = "UomName";
        public const string DepreciationClass = "DepreciationClass";
        public const string SupplierName = "SupplierName";
        public const string AssetCondition = "AssetCondition";
        public const string CategoryCodeLength = "CategoryCodeLength";
        public const string LocationCodeLength = "LocationCodeLength";
        public const string EnableRFID = "EnableRFID";
        public const string BarcodePrefix = "BarcodePrefix";
        public const string BarcodeSeperator = "BarcodeSeperator";
        public const string CustomPrefixLength = "CustomPrefixLength";
        public const string BarcodeCustomPrefix = "BarcodeCustomPrefix";
        public const string BarcodeNumericLength = "BarcodeNumericLength";
        public const string UserLocationMapping = "UserLocationMapping";
        public const string AssetReceiptEnable = "AssetReceiptEnable";
        public const string NoOfRecordsInGrid = "NoOfRecordsInGrid";
        public const string AssetApproval = "AssetApproval";
        public const string DepreciationMethod = "DepreciationMethod";
        public const string DepreciationApproval = "DepreciationApproval";
        public const string AssetAttachDetachApproval = "AssetAttachDetachApproval";
        public const string IsMultiLanguageRequired = "IsMultiLanguageRequired";
        public const string CompanyName = "CompanyName";
        public const string AutoDepreciation = "AutoDepreciation";
        public const string LocationMandatory = "LocationMandatory";
        public const string TransferAssetApproval = "TransferAssetApproval";

        public const string DashboardGrid = "DashboardGrid";
        public const string CategoryWiseDetails = "CategoryWiseDetails";
        public const string LocationWiseDetails = "LocationWiseDetails";
        public const string DepartmentWiseDetails = "DepartmentWiseDetails";
        public const string DepartmentWiseApprovalDetails = "DepartmentWiseApprovalDetails";
        public const string WarrantyNotification = "WarrantyNotification";
        public const string TransferAssetCount = "TransferAssetCount";
        public const string CategorybyAssetsDispalyin = "CategorybyAssetsDispalyin";
        public const string LocationbyAssetsDispalyin = "LocationbyAssetsDispalyin";

        public const string UserCategoryMapping = "UserCategoryMapping";
        public const string ADDomainName = "ADDomainName";
        public const string ADGroupName = "ADGroupName";
        public const string HHTAssetModuleApproval = "HHTAssetModuleApproval";
        public const string HHTParentChildAssetApproval = "HHTParentChildAssetApproval";
        public const string HHTAssetAuditApproval = "HHTAssetAuditApproval";
        public const string HHTAssetTransferApproval = "HHTAssetTransferApproval";
        public const string ImportCustodiansFromADUser = "ImportCustodiansFromADUser";
        public const string DomainUsernamePrefix = "DomainUsernamePrefix";
        public const string BulkAssetDeletion = "BulkAssetDeletion";

        public const string BulkDisposeAsset = "BulkDisposeAsset";
        public const string NoOfDecimalDigits = "NoOfDecimalDigits";
        public const string AssignAssetToParentDetails = "AssignAssetToParentDetails";
        public const string BulkDisposeAssetApproval = "BulkDisposeAssetApproval";
        public const string BulkDeleteAssetApproval = "BulkDeleteAssetApproval";
        public const string CaptureDocumentsForAsset = "CaptureDocumentsForAsset";
        public const string GridDisplayOrder = "GridDisplayOrder";
        public const string DepartmentSectionMapping = "DepartmentSectionMapping";
        public const string CategoryLocationPrefixLength = "CategoryPrefixLength";
        public const string CustodianLocationMapping = "CustodianLocationMapping";
        public const string LicenseExpireNotification = "LicenseExpireNotification";
        public const string IsReportHeaderRequiredForExcelReport = "IsReportHeaderRequiredForExcelReport";
        public const string BarcodePrintLogoName = "BarcodePrintLogoName";

        public const string IsEnableBarcodePrintinAssetScreen = "IsEnableBarcodePrintinAssetScreen";
        public const string IsWarrantyAlertNotificationEnable = "IsWarrantyAlertNotificationEnable";
        public const string WarrantyAlertNotificationDays = "WarrantyAlertNotificationDays";
        public const string IsAssetReceiptSendEmail = "IsAssetReceiptSendEmail";

        public const string MasterGridDisplayOrder = "MasterGridDisplayOrder";
        public const string EnableDirectPrinting = "EnableDirectPrinting";
        public const string MasterCodeAutoGenerate = "MasterCodeAutoGenerate";
        public const string EnableAuditLog = "EnableAuditLog";
        public const string TransactionPendingDetails = "TransactionPendingDetails";
        public const string IsDepreciationSummaryCountReportInBetweenTwoDate = "IsDepreciationSummaryCountReportInBetweenTwoDate";
        public const string MonthlyDepreciationDurationBasedOn = "MonthlyDepreciationDurationBasedOn";
        public const string MonthlyDepreciationBasedOn = "MonthlyDepreciationBasedOn";
        public const string IsMandatorySerialNumberinAssetScreen = "IsMandatorySerialNumberinAssetScreen";
        public const string MonthlyDepreciationbasedonyearlypercentage = "MonthlyDepreciationbasedonyearlypercentage";
        public const string IsBarcodeSettingApplyImportAsset = "IsBarcodeSettingApplyImportAsset";
        public const string LocationPrefixLength = "LocationPrefixLength";
        public const string CompanyPrefixLength = "CompanyPrefixLength";

        public const string PageFooterContent = "PageFooterContent";
        public const string LoginPageFooterContent = "LoginPageFooterContent";

        // MAster Name Settings //
        public const string DepartmentList = "DepartmentList";
        public const string SectionList = "SectionList";
        public const string CustodianList = "CustodianList";
        public const string AssetTransferCustodianList = "AssetTransferCustodianList";
        public const string CategoryList = "CategoryList";
        public const string LocationList = "LocationList";
        public const string SupplierList = "SupplierList";
        public const string AssetConditionList = "AssetConditionList";
        public const string ProductList = "ProductList";
        public const string CompanyList = "CompanyList";
        public const string TransferTypeList = "TransferTypeList";
        public const string UomList = "UomList";

        public const string ManufacturerList = "ManufacturerList";
        public const string ModelList = "ModelList";
        public const string ModelName = "ModelName";
        public const string ManufacturerName = "ManufacturerName";

        public const string ADUsername = "ADUsername";
        public const string ADPassword = "ADPassword";

        public const string EnableSenderRecevierSignatureInAssetReceipt = "EnableSenderRecevierSignatureInAssetReceipt";
        public const string ManufacturerModelMapping = "ManufacturerModelMapping";
        public const string TransferVoucherNoEnabled = "TransferVoucherNoEnabled";

        public const string EmailLinkExiredInDays = "EmailLinkExiredInDays";
        public const string SMSLinkExpiredInDays = "SMSLinkExpiredInDays";
        public const string VoucherNoAutoGenerate = "VoucherNoAutoGenerate";
        public const string ApproverUser = "ApproverUser";
        public const string WebAuditEnabled = "WebAuditEnabled";
        public const string AssetWebAuditApproval = "AssetWebAuditApproval";
        public const string TermsAndConditionLink = "TermsAndConditionLink";
        public const string IsEscalationMailForTransferAsset = "IsEscalationMailForTransferAsset";
        public const string AssetIssuanceApproval = "AssetIssuanceApproval";
        public const string AssetRequestApproval = "AssetRequestApproval";
        public const string AssetReturnApproval = "AssetReturnApproval";
        public const string RetirementApproval = "RetirementApproval";
        //public const string ChangeDefaultApplicationLogo = "ChangeDefaultApplicationLogo";
        public const string Logo = "Logo";
        // private const string CacheContentName = "__ConfigurationContent__";
        private const string CacheContentID = "__ConfigurationContentID__";

        public const string EmailServiceRunTime = "EmailServiceRunTime";
        public const string AllowMultipleDocumentUpload = "AllowMultipleDocumentUpload";
        public const string LocationEnable = "LocationEnable";
        public const string ManufacturerCategoryMapping = "ManufacturerCategoryMapping";
        public const string ModelManufacturerCategoryMapping = "ModelManufacturerCategoryMapping";
        public const string EnableCustomExcelImportForModification = "EnableCustomExcelImportForModification";
        public const string AssetApprovalBasedOnWorkFlow = "AssetApprovalBasedOnWorkFlow";
        public const string TransferAssetApprovalBasedOnWorkFlow = "TransferAssetApprovalBasedOnWorkFlow";

        public const string ImportExcelNotAllowCreateReferenceFieldNewEntry = "ImportExcelNotAllowCreateReferenceFieldNewEntry";

        public const string UserCustodianMapping = "UserCustodianMapping";
        public const string InventoryType = "InventoryType";
        public const string CategoryType = "CategoryType";
        public const string TempAssetRequestApproval = "TempAssetRequestApproval";
        public const string EnableInventoryModule = "EnableInventoryModule";

        public const string TempAssetIssueApproval = "TempAssetIssueApproval";
        public const string TempAssetReturnApproval = "TempAssetReturnApproval";
        public const string IsEmailnotificationrequired = "IsEmailnotificationrequired";
        public const string PurchaseOrderApproval = "PurchaseOrderApproval";
        public const string GRNApproval = "GRNApproval";
        public const string PurchaseReturnApproval = "PurchaseReturnApproval";
        public const string AssetReturnlist = "AssetReturnlist";
        public const string IsAssetReturnRemainderRequired = "IsAssetReturnRemainderRequired";
        public const string AssetReturnReminderWithInDays = "AssetReturnReminderWithInDays";

        public const string EnableLineItemWiseTempAssetRequestApproval = "EnableLineItemWiseTempAssetRequestApproval";
        public const string EnableLineItemWiseTempAssetIssueApproval = "EnableLineItemWiseTempAssetIssueApproval";
        public const string EnableLineItemWiseTempAssetReturnApproval = "EnableLineItemWiseTempAssetReturnApproval";

        public const string DisposalType = "DisposalType";
        public const string DisposalTypeList = "DisposalTypeList";

        public const string MultiLanguage = "MultiLanguage";
        public const string AllowMoreQtyThenPOQty = "AllowMoreQtyThenPOQty";
        public const string EnableCategoryMapping = "EnableCategoryMapping";
        public const string TempLoanReturn = "TempLoanReturn";
        public const string MinimuPeriodToDisposeAsset = "MinimuPeriodToDisposeAsset";
        public const string AssetDeletionSuffix = "AssetDeletionSuffix ";
        //Kannan - 08/06/2020 - Refer UDC Change Request recd on 04/06/2020
        public const string EnableDepartmentCustodianMappingApplyToAssetScreen = "EnableDepartmentCustodianMappingApplyToAssetScreen";
        public const string EnableTransferHistoryTab = "EnableTransferHistoryTab";
        public const string EnableMainScreenSearch = "EnableMainScreenSearch";
        public const string EnableAllMappedCompaniesinLoginScreen = "EnableAllMappedCompaniesinLoginScreen";
        public const string UserSubLocationMapping = "UserSubLocationMapping";
        public const string MutipleApprovalBasedOnCompanyWiseUser = "MutipleApprovalBasedOnCompanyWiseUser";
        public const string ApprovalBasedOnSubLocationMappedUser = "ApprovalBasedOnSubLocationMappedUser";
        public const string IsMainUserApproveFinalLevelProcess = "IsMainUserApproveFinalLevelProcess";

        public const string HHTAssetModuleApprovalBasedOnWorkFlow = "HHTAssetModuleApprovalBasedOnWorkFlow";
        public const string HHTAssetTransferApprovalBasedOnWorkFlow = "HHTAssetTransferApprovalBasedOnWorkFlow";
        public const string HHTAssetAuditApprovalBasedOnWorkFlow = "HHTAssetAuditApprovalBasedOnWorkFlow";
        public const string DisposalReferenceNoAutoGenerate = "DisposalReferenceNoAutoGenerate";
        public const string BulkDisposalBasedOn = "BulkDisposalBasedOn";
        public const string EmailUrl = "EmailUrl";
        public const string ViewAllLocationinTransferAssetScreen = "ViewAllLocationinTransferAssetScreen";

        public const string SelectedCategoriesareMandatorySerialNumberInAssetScreen = "SelectedCategoriesareMandatorySerialNumberInAssetScreen";
        public const string NextAvailableBarcodeDisplayInAssetScreen = "NextAvailableBarcodeDisplayInAssetScreen";
        public const string IsEnableCategoryBasedAttribute = "IsEnableCategoryBasedAttribute";
        public const string CustodianDepartmentMapping = "CustodianDepartmentMapping";
        public const string IsMandatoryLocationType = "IsMandatoryLocationType";
        public const string IsMandatoryCategoryType = "IsMandatoryCategoryType";
        public const string PreferredLevelCategoryMapping = "PreferredLevelCategoryMapping";
        public const string PreferredLevelLocationMapping = "PreferredLevelLocationMapping";

        public const string SystemDateFormat = "SystemDateFormat";
        public AppConfigurationManager(AMSContext db, IDistributedCache cache)
        {
            _db = db;
            _cache = cache;
        }
        public static string GetValue(string configName)
        { 
            return GetValue<string>(configName);
        }

        public static T GetValue<T>(string configName)
        {
            try
            {
                //var itmTask = _cache.GetRecordAsync<Dictionary<string, string>>(CacheContentName);
                //itmTask.Wait(1000);

                Dictionary<string, string> itm= new Dictionary<string, string>(); //itmTask.Result;
                //if (itm == null || itm.Count == 0)
                //{
                //    itm = new Dictionary<string, string>();
                //    //Thread.Sleep(10000);
                    var qry = ConfigurationTable.GetAllConfiguration(AMSContext.CreateNewContext());
                    foreach (ConfigurationTable dbItm in qry)
                    {
                        if (!itm.ContainsKey(dbItm.ConfiguarationName))
                        {
                            itm.Add(dbItm.ConfiguarationName, dbItm.ConfiguarationValue);
                        }
                    }

                //    _cache.SetRecordAsync(CacheContentName, itm);
                //}

                string val = itm[configName];
                Type newType = typeof(T);
                var converter = TypeDescriptor.GetConverter(newType);
                return (T)converter.ConvertFromString(val);
            }
            catch (Exception ex)
            {
                ApplicationErrorLogTable.SaveException(ex);
                throw ex;
            }
        }
    }
}