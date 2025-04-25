using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class User_LoginUserTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int UserID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string UserName { get; set; } = null!;

    [StringLength(250)]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    [StringLength(250)]
    [Unicode(false)]
    public string PasswordSalt { get; set; } = null!;

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastLoggedInDate { get; set; }

    public bool? ChangePasswordAtNextLogin { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? Email { get; set; }

    [StringLength(250)]
    public string? PasswordQuestion { get; set; }

    [StringLength(250)]
    public string? PasswordAnswer { get; set; }

    public bool IsLockedOut { get; set; }

    public bool IsDisabled { get; set; }

    public bool IsApproved { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastActivityDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastLoginDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastPasswordChangedDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastLockOutDate { get; set; }

    public int FailedPasswordAttemptCount { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? PasswordAttemptWindowStart { get; set; }

    public int? PasswordAnswerAttemptCount { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? PasswordAnswerAttemptWindowStart { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? LastLoginIPAddress { get; set; }

    public int? ApplicationID { get; set; }

    [StringLength(1000)]
    public string? UserComment { get; set; }

    [ForeignKey("ApplicationID")]
    [InverseProperty("User_LoginUserTable")]
    public virtual App_Applications? Application { get; set; }

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ApprovalHistoryTable> ApprovalHistoryTableCreatedByNavigation { get; set; } = new List<ApprovalHistoryTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<ApprovalHistoryTable> ApprovalHistoryTableLastModifiedByNavigation { get; set; } = new List<ApprovalHistoryTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ApprovalRoleDescriptionTable> ApprovalRoleDescriptionTableCreatedByNavigation { get; set; } = new List<ApprovalRoleDescriptionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<ApprovalRoleDescriptionTable> ApprovalRoleDescriptionTableLastModifiedByNavigation { get; set; } = new List<ApprovalRoleDescriptionTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ApprovalRoleTable> ApprovalRoleTableCreatedByNavigation { get; set; } = new List<ApprovalRoleTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<ApprovalRoleTable> ApprovalRoleTableLastModifiedByNavigation { get; set; } = new List<ApprovalRoleTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ApprovalTransactionHistoryTable> ApprovalTransactionHistoryTable { get; set; } = new List<ApprovalTransactionHistoryTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ApproveWorkflowTable> ApproveWorkflowTableCreatedByNavigation { get; set; } = new List<ApproveWorkflowTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<ApproveWorkflowTable> ApproveWorkflowTableLastModifiedByNavigation { get; set; } = new List<ApproveWorkflowTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<AssetConditionDescriptionTable> AssetConditionDescriptionTableCreatedByNavigation { get; set; } = new List<AssetConditionDescriptionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<AssetConditionDescriptionTable> AssetConditionDescriptionTableLastModifiedByNavigation { get; set; } = new List<AssetConditionDescriptionTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<AssetConditionTable> AssetConditionTableCreatedByNavigation { get; set; } = new List<AssetConditionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<AssetConditionTable> AssetConditionTableLastModifiedByNavigation { get; set; } = new List<AssetConditionTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<AssetTable> AssetTableCreatedByNavigation { get; set; } = new List<AssetTable>();

    [InverseProperty("CustodianNavigation")]
    public virtual ICollection<AssetTable> AssetTableCustodianNavigation { get; set; } = new List<AssetTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<AssetTable> AssetTableLastModifiedByNavigation { get; set; } = new List<AssetTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<AssetTransactionLineItemTable> AssetTransactionLineItemTableCreatedByNavigation { get; set; } = new List<AssetTransactionLineItemTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<AssetTransactionLineItemTable> AssetTransactionLineItemTableLastModifiedByNavigation { get; set; } = new List<AssetTransactionLineItemTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<AssetTransactionTable> AssetTransactionTableCreatedByNavigation { get; set; } = new List<AssetTransactionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<AssetTransactionTable> AssetTransactionTableLastModifiedByNavigation { get; set; } = new List<AssetTransactionTable>();

    [InverseProperty("CreatedbyNavigation")]
    public virtual ICollection<AssetTransferCategoryHistoryTable> AssetTransferCategoryHistoryTable { get; set; } = new List<AssetTransferCategoryHistoryTable>();

    [InverseProperty("NewCustodian")]
    public virtual ICollection<AssetTransferHistoryTable> AssetTransferHistoryTableNewCustodian { get; set; } = new List<AssetTransferHistoryTable>();

    [InverseProperty("OldCustodian")]
    public virtual ICollection<AssetTransferHistoryTable> AssetTransferHistoryTableOldCustodian { get; set; } = new List<AssetTransferHistoryTable>();

    [InverseProperty("TransferByNavigation")]
    public virtual ICollection<AssetTransferHistoryTable> AssetTransferHistoryTableTransferByNavigation { get; set; } = new List<AssetTransferHistoryTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<AssetTransferTransactionTable> AssetTransferTransactionTableCreatedByNavigation { get; set; } = new List<AssetTransferTransactionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<AssetTransferTransactionTable> AssetTransferTransactionTableLastModifiedByNavigation { get; set; } = new List<AssetTransferTransactionTable>();

    [InverseProperty("UploadedByNavigation")]
    public virtual ICollection<AssetimportDocumentHistoryTable> AssetimportDocumentHistoryTable { get; set; } = new List<AssetimportDocumentHistoryTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<AttributeDefinitionTable> AttributeDefinitionTableCreatedByNavigation { get; set; } = new List<AttributeDefinitionTable>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<AttributeDefinitionTable> AttributeDefinitionTableModifiedByNavigation { get; set; } = new List<AttributeDefinitionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<BarcodeConstructTable> BarcodeConstructTable { get; set; } = new List<BarcodeConstructTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<BarcodeFormatsTable> BarcodeFormatsTableCreatedByNavigation { get; set; } = new List<BarcodeFormatsTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<BarcodeFormatsTable> BarcodeFormatsTableLastModifiedByNavigation { get; set; } = new List<BarcodeFormatsTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<BudgetYearTable> BudgetYearTableCreatedByNavigation { get; set; } = new List<BudgetYearTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<BudgetYearTable> BudgetYearTableLastModifiedByNavigation { get; set; } = new List<BudgetYearTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<CategoryDescriptionTable> CategoryDescriptionTableCreatedByNavigation { get; set; } = new List<CategoryDescriptionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<CategoryDescriptionTable> CategoryDescriptionTableLastModifiedByNavigation { get; set; } = new List<CategoryDescriptionTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<CategoryTable> CategoryTableCreatedByNavigation { get; set; } = new List<CategoryTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<CategoryTable> CategoryTableLastModifiedByNavigation { get; set; } = new List<CategoryTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<CategoryTypeDescriptionTable> CategoryTypeDescriptionTableCreatedByNavigation { get; set; } = new List<CategoryTypeDescriptionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<CategoryTypeDescriptionTable> CategoryTypeDescriptionTableLastModifiedByNavigation { get; set; } = new List<CategoryTypeDescriptionTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<CategoryTypeTable> CategoryTypeTableCreatedByNavigation { get; set; } = new List<CategoryTypeTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<CategoryTypeTable> CategoryTypeTableLastModifiedByNavigation { get; set; } = new List<CategoryTypeTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<CompanyDescriptionTable> CompanyDescriptionTableCreatedByNavigation { get; set; } = new List<CompanyDescriptionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<CompanyDescriptionTable> CompanyDescriptionTableLastModifiedByNavigation { get; set; } = new List<CompanyDescriptionTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<CompanyTable> CompanyTableCreatedByNavigation { get; set; } = new List<CompanyTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<CompanyTable> CompanyTableLastModifiedByNavigation { get; set; } = new List<CompanyTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<CostTypeDescriptionTable> CostTypeDescriptionTableCreatedByNavigation { get; set; } = new List<CostTypeDescriptionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<CostTypeDescriptionTable> CostTypeDescriptionTableLastModifiedByNavigation { get; set; } = new List<CostTypeDescriptionTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<CostTypeTable> CostTypeTableCreatedByNavigation { get; set; } = new List<CostTypeTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<CostTypeTable> CostTypeTableLastModifiedByNavigation { get; set; } = new List<CostTypeTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<CountryDescriptionTable> CountryDescriptionTableCreatedByNavigation { get; set; } = new List<CountryDescriptionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<CountryDescriptionTable> CountryDescriptionTableLastModifiedByNavigation { get; set; } = new List<CountryDescriptionTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<CountryTable> CountryTableCreatedByNavigation { get; set; } = new List<CountryTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<CountryTable> CountryTableLastModifiedByNavigation { get; set; } = new List<CountryTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<CurrencyConversionTable> CurrencyConversionTableCreatedByNavigation { get; set; } = new List<CurrencyConversionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<CurrencyConversionTable> CurrencyConversionTableLastModifiedByNavigation { get; set; } = new List<CurrencyConversionTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<CurrencyDescriptionTable> CurrencyDescriptionTableCreatedByNavigation { get; set; } = new List<CurrencyDescriptionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<CurrencyDescriptionTable> CurrencyDescriptionTableLastModifiedByNavigation { get; set; } = new List<CurrencyDescriptionTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<CurrencyTable> CurrencyTableCreatedByNavigation { get; set; } = new List<CurrencyTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<CurrencyTable> CurrencyTableLastModifiedByNavigation { get; set; } = new List<CurrencyTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DashboardFieldMappingTable> DashboardFieldMappingTableCreatedByNavigation { get; set; } = new List<DashboardFieldMappingTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<DashboardFieldMappingTable> DashboardFieldMappingTableLastModifiedByNavigation { get; set; } = new List<DashboardFieldMappingTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DashboardMappingTable> DashboardMappingTableCreatedByNavigation { get; set; } = new List<DashboardMappingTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<DashboardMappingTable> DashboardMappingTableLastModifiedByNavigation { get; set; } = new List<DashboardMappingTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DashboardTemplateFieldTable> DashboardTemplateFieldTableCreatedByNavigation { get; set; } = new List<DashboardTemplateFieldTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<DashboardTemplateFieldTable> DashboardTemplateFieldTableLastModifiedByNavigation { get; set; } = new List<DashboardTemplateFieldTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DashboardTemplateFilterFieldTable> DashboardTemplateFilterFieldTableCreatedByNavigation { get; set; } = new List<DashboardTemplateFilterFieldTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<DashboardTemplateFilterFieldTable> DashboardTemplateFilterFieldTableLastModifiedByNavigation { get; set; } = new List<DashboardTemplateFilterFieldTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DashboardTemplateTable> DashboardTemplateTableCreatedByNavigation { get; set; } = new List<DashboardTemplateTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<DashboardTemplateTable> DashboardTemplateTableLastModifiedByNavigation { get; set; } = new List<DashboardTemplateTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DelegateRoleTable> DelegateRoleTableCreatedByNavigation { get; set; } = new List<DelegateRoleTable>();

    [InverseProperty("DelegatedEmployee")]
    public virtual ICollection<DelegateRoleTable> DelegateRoleTableDelegatedEmployee { get; set; } = new List<DelegateRoleTable>();

    [InverseProperty("Employee")]
    public virtual ICollection<DelegateRoleTable> DelegateRoleTableEmployee { get; set; } = new List<DelegateRoleTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<DelegateRoleTable> DelegateRoleTableLastModifiedByNavigation { get; set; } = new List<DelegateRoleTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DepartmentDescriptionTable> DepartmentDescriptionTableCreatedByNavigation { get; set; } = new List<DepartmentDescriptionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<DepartmentDescriptionTable> DepartmentDescriptionTableLastModifiedByNavigation { get; set; } = new List<DepartmentDescriptionTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DepartmentTable> DepartmentTableCreatedByNavigation { get; set; } = new List<DepartmentTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<DepartmentTable> DepartmentTableLastModifiedByNavigation { get; set; } = new List<DepartmentTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DepreciationClassTable> DepreciationClassTableCreatedByNavigation { get; set; } = new List<DepreciationClassTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<DepreciationClassTable> DepreciationClassTableLastModifiedByNavigation { get; set; } = new List<DepreciationClassTable>();

    [InverseProperty("DeletedApprovedByNavigation")]
    public virtual ICollection<DepreciationTable> DepreciationTableDeletedApprovedByNavigation { get; set; } = new List<DepreciationTable>();

    [InverseProperty("DeletedDoneByNavigation")]
    public virtual ICollection<DepreciationTable> DepreciationTableDeletedDoneByNavigation { get; set; } = new List<DepreciationTable>();

    [InverseProperty("DepreciationApprovedByNavigation")]
    public virtual ICollection<DepreciationTable> DepreciationTableDepreciationApprovedByNavigation { get; set; } = new List<DepreciationTable>();

    [InverseProperty("DepreciationDoneByNavigation")]
    public virtual ICollection<DepreciationTable> DepreciationTableDepreciationDoneByNavigation { get; set; } = new List<DepreciationTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DesignationTable> DesignationTableCreatedByNavigation { get; set; } = new List<DesignationTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<DesignationTable> DesignationTableLastModifiedByNavigation { get; set; } = new List<DesignationTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DisposalTypeDescriptionTable> DisposalTypeDescriptionTableCreatedByNavigation { get; set; } = new List<DisposalTypeDescriptionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<DisposalTypeDescriptionTable> DisposalTypeDescriptionTableLastModifiedByNavigation { get; set; } = new List<DisposalTypeDescriptionTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DisposalTypeTable> DisposalTypeTableCreatedByNavigation { get; set; } = new List<DisposalTypeTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<DisposalTypeTable> DisposalTypeTableLastModifiedByNavigation { get; set; } = new List<DisposalTypeTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<EmailSignatureTable> EmailSignatureTableCreatedByNavigation { get; set; } = new List<EmailSignatureTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<EmailSignatureTable> EmailSignatureTableLastModifiedByNavigation { get; set; } = new List<EmailSignatureTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ExchangeRateTable> ExchangeRateTableCreatedByNavigation { get; set; } = new List<ExchangeRateTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<ExchangeRateTable> ExchangeRateTableLastModifiedByNavigation { get; set; } = new List<ExchangeRateTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<GRNTable> GRNTableCreatedByNavigation { get; set; } = new List<GRNTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<GRNTable> GRNTableLastModifiedByNavigation { get; set; } = new List<GRNTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<InvoiceTable> InvoiceTableCreatedByNavigation { get; set; } = new List<InvoiceTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<InvoiceTable> InvoiceTableLastModifiedByNavigation { get; set; } = new List<InvoiceTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ItemDispatchTable> ItemDispatchTableCreatedByNavigation { get; set; } = new List<ItemDispatchTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<ItemDispatchTable> ItemDispatchTableLastModifiedByNavigation { get; set; } = new List<ItemDispatchTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ItemReqestTable> ItemReqestTableCreatedByNavigation { get; set; } = new List<ItemReqestTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<ItemReqestTable> ItemReqestTableLastModifiedByNavigation { get; set; } = new List<ItemReqestTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<LanguageTable> LanguageTableCreatedByNavigation { get; set; } = new List<LanguageTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<LanguageTable> LanguageTableLastModifiedByNavigation { get; set; } = new List<LanguageTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<LocationDescriptionTable> LocationDescriptionTableCreatedByNavigation { get; set; } = new List<LocationDescriptionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<LocationDescriptionTable> LocationDescriptionTableLastModifiedByNavigation { get; set; } = new List<LocationDescriptionTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<LocationTable> LocationTableCreatedByNavigation { get; set; } = new List<LocationTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<LocationTable> LocationTableLastModifiedByNavigation { get; set; } = new List<LocationTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<LocationTypeTable> LocationTypeTable { get; set; } = new List<LocationTypeTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ManufacturerTable> ManufacturerTableCreatedByNavigation { get; set; } = new List<ManufacturerTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<ManufacturerTable> ManufacturerTableLastModifiedByNavigation { get; set; } = new List<ManufacturerTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ModelDescriptionTable> ModelDescriptionTableCreatedByNavigation { get; set; } = new List<ModelDescriptionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<ModelDescriptionTable> ModelDescriptionTableLastModifiedByNavigation { get; set; } = new List<ModelDescriptionTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ModelTable> ModelTableCreatedByNavigation { get; set; } = new List<ModelTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<ModelTable> ModelTableLastModifiedByNavigation { get; set; } = new List<ModelTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<NotificationFieldTable> NotificationFieldTable { get; set; } = new List<NotificationFieldTable>();

    [InverseProperty("SYSUser")]
    public virtual ICollection<NotificationInputTable> NotificationInputTable { get; set; } = new List<NotificationInputTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<NotificationModuleFieldTable> NotificationModuleFieldTable { get; set; } = new List<NotificationModuleFieldTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<NotificationTemplateTable> NotificationTemplateTableCreatedByNavigation { get; set; } = new List<NotificationTemplateTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<NotificationTemplateTable> NotificationTemplateTableLastModifiedByNavigation { get; set; } = new List<NotificationTemplateTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PartyDescriptionTable> PartyDescriptionTableCreatedByNavigation { get; set; } = new List<PartyDescriptionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<PartyDescriptionTable> PartyDescriptionTableLastModifiedByNavigation { get; set; } = new List<PartyDescriptionTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PartyTable> PartyTableCreatedByNavigation { get; set; } = new List<PartyTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<PartyTable> PartyTableLastModifiedByNavigation { get; set; } = new List<PartyTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PeriodTable> PeriodTableCreatedByNavigation { get; set; } = new List<PeriodTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<PeriodTable> PeriodTableLastModifiedByNavigation { get; set; } = new List<PeriodTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PersonTable> PersonTableCreatedByNavigation { get; set; } = new List<PersonTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<PersonTable> PersonTableLastModifiedByNavigation { get; set; } = new List<PersonTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PriceAnalysisTable> PriceAnalysisTableCreatedByNavigation { get; set; } = new List<PriceAnalysisTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<PriceAnalysisTable> PriceAnalysisTableLastModifiedByNavigation { get; set; } = new List<PriceAnalysisTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProductDescriptionTable> ProductDescriptionTableCreatedByNavigation { get; set; } = new List<ProductDescriptionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<ProductDescriptionTable> ProductDescriptionTableLastModifiedByNavigation { get; set; } = new List<ProductDescriptionTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProductTable> ProductTableCreatedByNavigation { get; set; } = new List<ProductTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<ProductTable> ProductTableLastModifiedByNavigation { get; set; } = new List<ProductTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProductUOMMappingTable> ProductUOMMappingTableCreatedByNavigation { get; set; } = new List<ProductUOMMappingTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<ProductUOMMappingTable> ProductUOMMappingTableLastModifiedByNavigation { get; set; } = new List<ProductUOMMappingTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectDescriptionTable> ProjectDescriptionTableCreatedByNavigation { get; set; } = new List<ProjectDescriptionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<ProjectDescriptionTable> ProjectDescriptionTableLastModifiedByNavigation { get; set; } = new List<ProjectDescriptionTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectTable> ProjectTableCreatedByNavigation { get; set; } = new List<ProjectTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<ProjectTable> ProjectTableLastModifiedByNavigation { get; set; } = new List<ProjectTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PurchaseOrderTable> PurchaseOrderTableCreatedByNavigation { get; set; } = new List<PurchaseOrderTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<PurchaseOrderTable> PurchaseOrderTableLastModifiedByNavigation { get; set; } = new List<PurchaseOrderTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ReasonTypeTable> ReasonTypeTableCreatedByNavigation { get; set; } = new List<ReasonTypeTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<ReasonTypeTable> ReasonTypeTableLastModifiedByNavigation { get; set; } = new List<ReasonTypeTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ReportFieldTable> ReportFieldTableCreatedByNavigation { get; set; } = new List<ReportFieldTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<ReportFieldTable> ReportFieldTableLastModifiedByNavigation { get; set; } = new List<ReportFieldTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ReportGroupFieldTable> ReportGroupFieldTableCreatedByNavigation { get; set; } = new List<ReportGroupFieldTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<ReportGroupFieldTable> ReportGroupFieldTableLastModifiedByNavigation { get; set; } = new List<ReportGroupFieldTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ReportTable> ReportTableCreatedByNavigation { get; set; } = new List<ReportTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<ReportTable> ReportTableLastModifiedByNavigation { get; set; } = new List<ReportTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ReportTemplateCategoryTable> ReportTemplateCategoryTableCreatedByNavigation { get; set; } = new List<ReportTemplateCategoryTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<ReportTemplateCategoryTable> ReportTemplateCategoryTableLastModifiedByNavigation { get; set; } = new List<ReportTemplateCategoryTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ReportTemplateFieldTable> ReportTemplateFieldTableCreatedByNavigation { get; set; } = new List<ReportTemplateFieldTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<ReportTemplateFieldTable> ReportTemplateFieldTableLastModifiedByNavigation { get; set; } = new List<ReportTemplateFieldTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ReportTemplateFileTable> ReportTemplateFileTableCreatedByNavigation { get; set; } = new List<ReportTemplateFileTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<ReportTemplateFileTable> ReportTemplateFileTableLastModifiedByNavigation { get; set; } = new List<ReportTemplateFileTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ReportTemplateTable> ReportTemplateTableCreatedByNavigation { get; set; } = new List<ReportTemplateTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<ReportTemplateTable> ReportTemplateTableLastModifiedByNavigation { get; set; } = new List<ReportTemplateTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<RequestQuotationTable> RequestQuotationTableCreatedByNavigation { get; set; } = new List<RequestQuotationTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<RequestQuotationTable> RequestQuotationTableLastModifiedByNavigation { get; set; } = new List<RequestQuotationTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<RetirementTypeDescriptionTable> RetirementTypeDescriptionTableCreatedByNavigation { get; set; } = new List<RetirementTypeDescriptionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<RetirementTypeDescriptionTable> RetirementTypeDescriptionTableLastModifiedByNavigation { get; set; } = new List<RetirementTypeDescriptionTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<RetirementTypeTable> RetirementTypeTableCreatedByNavigation { get; set; } = new List<RetirementTypeTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<RetirementTypeTable> RetirementTypeTableLastModifiedByNavigation { get; set; } = new List<RetirementTypeTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ScreenFilterLineItemTable> ScreenFilterLineItemTable { get; set; } = new List<ScreenFilterLineItemTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SectionDescriptionTable> SectionDescriptionTableCreatedByNavigation { get; set; } = new List<SectionDescriptionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<SectionDescriptionTable> SectionDescriptionTableLastModifiedByNavigation { get; set; } = new List<SectionDescriptionTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SectionTable> SectionTableCreatedByNavigation { get; set; } = new List<SectionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<SectionTable> SectionTableLastModifiedByNavigation { get; set; } = new List<SectionTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SupplierQuotationTable> SupplierQuotationTableCreatedByNavigation { get; set; } = new List<SupplierQuotationTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<SupplierQuotationTable> SupplierQuotationTableLastModifiedByNavigation { get; set; } = new List<SupplierQuotationTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TransactionLineItemTable> TransactionLineItemTableCreatedByNavigation { get; set; } = new List<TransactionLineItemTable>();

    [InverseProperty("Custodian")]
    public virtual ICollection<TransactionLineItemTable> TransactionLineItemTableCustodian { get; set; } = new List<TransactionLineItemTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TransactionScheduleTable> TransactionScheduleTableCreatedByNavigation { get; set; } = new List<TransactionScheduleTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<TransactionScheduleTable> TransactionScheduleTableLastModifiedByNavigation { get; set; } = new List<TransactionScheduleTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TransactionTable> TransactionTableCreatedByNavigation { get; set; } = new List<TransactionTable>();

    [InverseProperty("PostedByNavigation")]
    public virtual ICollection<TransactionTable> TransactionTablePostedByNavigation { get; set; } = new List<TransactionTable>();

    [InverseProperty("VerifiedByNavigation")]
    public virtual ICollection<TransactionTable> TransactionTableVerifiedByNavigation { get; set; } = new List<TransactionTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TransferTypeDescriptionTable> TransferTypeDescriptionTableCreatedByNavigation { get; set; } = new List<TransferTypeDescriptionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<TransferTypeDescriptionTable> TransferTypeDescriptionTableLastModifiedByNavigation { get; set; } = new List<TransferTypeDescriptionTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TransferTypeTable> TransferTypeTableCreatedByNavigation { get; set; } = new List<TransferTypeTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<TransferTypeTable> TransferTypeTableLastModifiedByNavigation { get; set; } = new List<TransferTypeTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<UOMDescriptionTable> UOMDescriptionTableCreatedByNavigation { get; set; } = new List<UOMDescriptionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<UOMDescriptionTable> UOMDescriptionTableLastModifiedByNavigation { get; set; } = new List<UOMDescriptionTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<UOMTable> UOMTableCreatedByNavigation { get; set; } = new List<UOMTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<UOMTable> UOMTableLastModifiedByNavigation { get; set; } = new List<UOMTable>();

    [InverseProperty("User")]
    public virtual ICollection<UserApprovalRoleMappingTable> UserApprovalRoleMappingTable { get; set; } = new List<UserApprovalRoleMappingTable>();

    [InverseProperty("User")]
    public virtual ICollection<UserCategoryMappingTable> UserCategoryMappingTable { get; set; } = new List<UserCategoryMappingTable>();

    [InverseProperty("User")]
    public virtual ICollection<UserCompanyMappingTable> UserCompanyMappingTable { get; set; } = new List<UserCompanyMappingTable>();

    [InverseProperty("User")]
    public virtual ICollection<UserDepartmentMappingTable> UserDepartmentMappingTable { get; set; } = new List<UserDepartmentMappingTable>();

    [InverseProperty("User")]
    public virtual ICollection<UserGridNewColumnTable> UserGridNewColumnTable { get; set; } = new List<UserGridNewColumnTable>();

    [InverseProperty("User")]
    public virtual ICollection<UserLocationMappingTable> UserLocationMappingTable { get; set; } = new List<UserLocationMappingTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<UserReportFilterLineItemTable> UserReportFilterLineItemTable { get; set; } = new List<UserReportFilterLineItemTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<UserReportFilterTable> UserReportFilterTableCreatedByNavigation { get; set; } = new List<UserReportFilterTable>();

    [InverseProperty("User")]
    public virtual ICollection<UserReportFilterTable> UserReportFilterTableUser { get; set; } = new List<UserReportFilterTable>();

    [InverseProperty("User")]
    public virtual ICollection<User_UserRightTable> User_UserRightTable { get; set; } = new List<User_UserRightTable>();

    [InverseProperty("User")]
    public virtual ICollection<User_UserRoleTable> User_UserRoleTable { get; set; } = new List<User_UserRoleTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<VATDescriptionTable> VATDescriptionTableCreatedByNavigation { get; set; } = new List<VATDescriptionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<VATDescriptionTable> VATDescriptionTableLastModifiedByNavigation { get; set; } = new List<VATDescriptionTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<VATTable> VATTableCreatedByNavigation { get; set; } = new List<VATTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<VATTable> VATTableLastModifiedByNavigation { get; set; } = new List<VATTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<WarehouseDescriptionTable> WarehouseDescriptionTableCreatedByNavigation { get; set; } = new List<WarehouseDescriptionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<WarehouseDescriptionTable> WarehouseDescriptionTableLastModifiedByNavigation { get; set; } = new List<WarehouseDescriptionTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<WarehouseTable> WarehouseTableCreatedByNavigation { get; set; } = new List<WarehouseTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<WarehouseTable> WarehouseTableLastModifiedByNavigation { get; set; } = new List<WarehouseTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public User_LoginUserTable()
    {

    }

	/// <summary>
    /// Default constructor for User_LoginUserTable
    /// </summary>
    /// <param name="db"></param>
	public User_LoginUserTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "UserID";
	}

	public override object GetPrimaryKeyValue()
	{
		return UserID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.UserName)) this.UserName = this.UserName.Trim();
		if(!string.IsNullOrEmpty(this.Password)) this.Password = this.Password.Trim();
		if(!string.IsNullOrEmpty(this.PasswordSalt)) this.PasswordSalt = this.PasswordSalt.Trim();
		if(!string.IsNullOrEmpty(this.Email)) this.Email = this.Email.Trim();
		if(!string.IsNullOrEmpty(this.PasswordQuestion)) this.PasswordQuestion = this.PasswordQuestion.Trim();
		if(!string.IsNullOrEmpty(this.PasswordAnswer)) this.PasswordAnswer = this.PasswordAnswer.Trim();
		if(!string.IsNullOrEmpty(this.LastLoginIPAddress)) this.LastLoginIPAddress = this.LastLoginIPAddress.Trim();
		if(!string.IsNullOrEmpty(this.UserComment)) this.UserComment = this.UserComment.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static User_LoginUserTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.User_LoginUserTable
                where b.UserID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<User_LoginUserTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.User_LoginUserTable select b);
    }

    public static IQueryable<User_LoginUserTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return User_LoginUserTable.GetAllItems(_db, includeInactiveItems);
    }

    public static bool DeleteItem(AMSContext _db, int id)
    {
        var item = GetItem(_db, id);
        if (item != null)
        {
            item.Delete();
    		return true;
        }
    
    	return false;
    }
    
    #endregion Static Methods

    #region Partial Methods
	
	partial void InitializeProperties();
	partial void OnBeforeSave(AMSContext db);
	partial void OnDelete();

	#endregion Partial Methods

    protected override IQueryable GetAllItemsQuery(AMSContext _db)
    {
        return (from b in _db.User_LoginUserTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return User_LoginUserTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return User_LoginUserTable.GetAllUserItems(_db, userID, includeInactiveItems);
    }

    BaseEntityObject IACSDBObject.GetItemByID(AMSContext _db, long itemID)
    {
        return GetItem(_db, (int) itemID);
    }

    bool IACSDBObject.DeleteObject()
    {
        this.Delete();

        return true;
    }

    #endregion Interface Methods
}
