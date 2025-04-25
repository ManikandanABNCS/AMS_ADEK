using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class StatusTable : BaseEntityObject, IACSDBObject
{
    [Key]
    [DisplayName("Status")]
    public int StatusID { get; set; }

    [StringLength(250)]
    public string? Status { get; set; }

    [InverseProperty("Status")]
    public virtual ICollection<ApprovalHistoryTable> ApprovalHistoryTable { get; set; } = new List<ApprovalHistoryTable>();

    [InverseProperty("Status")]
    public virtual ICollection<ApprovalRoleTable> ApprovalRoleTable { get; set; } = new List<ApprovalRoleTable>();

    [InverseProperty("Status")]
    public virtual ICollection<ApprovalTransactionHistoryTable> ApprovalTransactionHistoryTable { get; set; } = new List<ApprovalTransactionHistoryTable>();

    [InverseProperty("Status")]
    public virtual ICollection<ApproveModuleTable> ApproveModuleTable { get; set; } = new List<ApproveModuleTable>();

    [InverseProperty("Status")]
    public virtual ICollection<ApproveWorkflowLineItemTable> ApproveWorkflowLineItemTable { get; set; } = new List<ApproveWorkflowLineItemTable>();

    [InverseProperty("Status")]
    public virtual ICollection<ApproveWorkflowTable> ApproveWorkflowTable { get; set; } = new List<ApproveWorkflowTable>();

    [InverseProperty("Status")]
    public virtual ICollection<AssetConditionTable> AssetConditionTable { get; set; } = new List<AssetConditionTable>();

    [InverseProperty("Status")]
    public virtual ICollection<AssetTable> AssetTable { get; set; } = new List<AssetTable>();

    [InverseProperty("Status")]
    public virtual ICollection<AssetTransactionLineItemTable> AssetTransactionLineItemTable { get; set; } = new List<AssetTransactionLineItemTable>();

    [InverseProperty("Status")]
    public virtual ICollection<AssetTransactionTable> AssetTransactionTable { get; set; } = new List<AssetTransactionTable>();

    [InverseProperty("Status")]
    public virtual ICollection<AssetTransferCategoryHistoryTable> AssetTransferCategoryHistoryTable { get; set; } = new List<AssetTransferCategoryHistoryTable>();

    [InverseProperty("Status")]
    public virtual ICollection<AssetTransferHistoryTable> AssetTransferHistoryTable { get; set; } = new List<AssetTransferHistoryTable>();

    [InverseProperty("Status")]
    public virtual ICollection<AssetTransferTransactionTable> AssetTransferTransactionTable { get; set; } = new List<AssetTransferTransactionTable>();

    [InverseProperty("Status")]
    public virtual ICollection<AssetimportDocumentHistoryTable> AssetimportDocumentHistoryTable { get; set; } = new List<AssetimportDocumentHistoryTable>();

    [InverseProperty("Status")]
    public virtual ICollection<AttributeDefinitionTable> AttributeDefinitionTable { get; set; } = new List<AttributeDefinitionTable>();

    [InverseProperty("Status")]
    public virtual ICollection<BarcodeAutoSequenceTable> BarcodeAutoSequenceTable { get; set; } = new List<BarcodeAutoSequenceTable>();

    [InverseProperty("Status")]
    public virtual ICollection<BarcodeFormatsTable> BarcodeFormatsTable { get; set; } = new List<BarcodeFormatsTable>();

    [InverseProperty("Status")]
    public virtual ICollection<BudgetYearTable> BudgetYearTable { get; set; } = new List<BudgetYearTable>();

    [InverseProperty("Status")]
    public virtual ICollection<CategoryTable> CategoryTable { get; set; } = new List<CategoryTable>();

    [InverseProperty("Status")]
    public virtual ICollection<CategoryTypeTable> CategoryTypeTable { get; set; } = new List<CategoryTypeTable>();

    [InverseProperty("Status")]
    public virtual ICollection<CompanyTable> CompanyTable { get; set; } = new List<CompanyTable>();

    [InverseProperty("Status")]
    public virtual ICollection<CostTypeTable> CostTypeTable { get; set; } = new List<CostTypeTable>();

    [InverseProperty("Status")]
    public virtual ICollection<CountryTable> CountryTable { get; set; } = new List<CountryTable>();

    [InverseProperty("Status")]
    public virtual ICollection<CurrencyConversionTable> CurrencyConversionTable { get; set; } = new List<CurrencyConversionTable>();

    [InverseProperty("Status")]
    public virtual ICollection<CurrencyTable> CurrencyTable { get; set; } = new List<CurrencyTable>();

    [InverseProperty("Status")]
    public virtual ICollection<DashboardFieldMappingTable> DashboardFieldMappingTable { get; set; } = new List<DashboardFieldMappingTable>();

    [InverseProperty("Status")]
    public virtual ICollection<DashboardMappingTable> DashboardMappingTable { get; set; } = new List<DashboardMappingTable>();

    [InverseProperty("Status")]
    public virtual ICollection<DashboardTemplateFieldTable> DashboardTemplateFieldTable { get; set; } = new List<DashboardTemplateFieldTable>();

    [InverseProperty("Status")]
    public virtual ICollection<DashboardTemplateFilterFieldTable> DashboardTemplateFilterFieldTable { get; set; } = new List<DashboardTemplateFilterFieldTable>();

    [InverseProperty("Status")]
    public virtual ICollection<DashboardTemplateTable> DashboardTemplateTable { get; set; } = new List<DashboardTemplateTable>();

    [InverseProperty("Status")]
    public virtual ICollection<DelegateRoleTable> DelegateRoleTable { get; set; } = new List<DelegateRoleTable>();

    [InverseProperty("Status")]
    public virtual ICollection<DepartmentTable> DepartmentTable { get; set; } = new List<DepartmentTable>();

    [InverseProperty("Status")]
    public virtual ICollection<DepreciationClassTable> DepreciationClassTable { get; set; } = new List<DepreciationClassTable>();

    [InverseProperty("Status")]
    public virtual ICollection<DepreciationTable> DepreciationTable { get; set; } = new List<DepreciationTable>();

    [InverseProperty("Status")]
    public virtual ICollection<DesignationTable> DesignationTable { get; set; } = new List<DesignationTable>();

    [InverseProperty("Status")]
    public virtual ICollection<DisposalTypeTable> DisposalTypeTable { get; set; } = new List<DisposalTypeTable>();

    [InverseProperty("Status")]
    public virtual ICollection<DocumentTable> DocumentTable { get; set; } = new List<DocumentTable>();

    [InverseProperty("Status")]
    public virtual ICollection<EmailSignatureTable> EmailSignatureTable { get; set; } = new List<EmailSignatureTable>();

    [InverseProperty("Status")]
    public virtual ICollection<ExchangeRateTable> ExchangeRateTable { get; set; } = new List<ExchangeRateTable>();

    [InverseProperty("Status")]
    public virtual ICollection<GRNTable> GRNTable { get; set; } = new List<GRNTable>();

    [InverseProperty("Status")]
    public virtual ICollection<ImportFormatNewTable> ImportFormatNewTable { get; set; } = new List<ImportFormatNewTable>();

    [InverseProperty("Status")]
    public virtual ICollection<InvoiceTable> InvoiceTable { get; set; } = new List<InvoiceTable>();

    [InverseProperty("Status")]
    public virtual ICollection<ItemDispatchTable> ItemDispatchTable { get; set; } = new List<ItemDispatchTable>();

    [InverseProperty("Status")]
    public virtual ICollection<ItemReqestTable> ItemReqestTable { get; set; } = new List<ItemReqestTable>();

    [InverseProperty("Status")]
    public virtual ICollection<LanguageContentTable> LanguageContentTable { get; set; } = new List<LanguageContentTable>();

    [InverseProperty("Status")]
    public virtual ICollection<LocationTable> LocationTable { get; set; } = new List<LocationTable>();

    [InverseProperty("Status")]
    public virtual ICollection<LocationTypeTable> LocationTypeTable { get; set; } = new List<LocationTypeTable>();

    [InverseProperty("Status")]
    public virtual ICollection<ManufacturerTable> ManufacturerTable { get; set; } = new List<ManufacturerTable>();

    [InverseProperty("Status")]
    public virtual ICollection<ModelTable> ModelTable { get; set; } = new List<ModelTable>();

    [InverseProperty("Status")]
    public virtual ICollection<NotificationFieldTable> NotificationFieldTable { get; set; } = new List<NotificationFieldTable>();

    [InverseProperty("Status")]
    public virtual ICollection<NotificationModuleFieldTable> NotificationModuleFieldTable { get; set; } = new List<NotificationModuleFieldTable>();

    [InverseProperty("Status")]
    public virtual ICollection<NotificationModuleTable> NotificationModuleTable { get; set; } = new List<NotificationModuleTable>();

    [InverseProperty("Status")]
    public virtual ICollection<NotificationTemplateFieldTable> NotificationTemplateFieldTable { get; set; } = new List<NotificationTemplateFieldTable>();

    [InverseProperty("Status")]
    public virtual ICollection<NotificationTemplateNotificationTypeTable> NotificationTemplateNotificationTypeTable { get; set; } = new List<NotificationTemplateNotificationTypeTable>();

    [InverseProperty("Status")]
    public virtual ICollection<NotificationTemplateTable> NotificationTemplateTable { get; set; } = new List<NotificationTemplateTable>();

    [InverseProperty("Status")]
    public virtual ICollection<PartyTable> PartyTable { get; set; } = new List<PartyTable>();

    [InverseProperty("Status")]
    public virtual ICollection<PeriodTable> PeriodTable { get; set; } = new List<PeriodTable>();

    [InverseProperty("Status")]
    public virtual ICollection<PersonTable> PersonTable { get; set; } = new List<PersonTable>();

    [InverseProperty("Status")]
    public virtual ICollection<PriceAnalysisTable> PriceAnalysisTable { get; set; } = new List<PriceAnalysisTable>();

    [InverseProperty("Status")]
    public virtual ICollection<ProductTable> ProductTable { get; set; } = new List<ProductTable>();

    [InverseProperty("Status")]
    public virtual ICollection<ProductUOMMappingTable> ProductUOMMappingTable { get; set; } = new List<ProductUOMMappingTable>();

    [InverseProperty("Status")]
    public virtual ICollection<ProjectTable> ProjectTable { get; set; } = new List<ProjectTable>();

    [InverseProperty("Status")]
    public virtual ICollection<PurchaseOrderTable> PurchaseOrderTable { get; set; } = new List<PurchaseOrderTable>();

    [InverseProperty("Status")]
    public virtual ICollection<ReasonTypeTable> ReasonTypeTable { get; set; } = new List<ReasonTypeTable>();

    [InverseProperty("Status")]
    public virtual ICollection<ReportFieldTable> ReportFieldTable { get; set; } = new List<ReportFieldTable>();

    [InverseProperty("Status")]
    public virtual ICollection<ReportGroupFieldTable> ReportGroupFieldTable { get; set; } = new List<ReportGroupFieldTable>();

    [InverseProperty("Status")]
    public virtual ICollection<ReportTable> ReportTable { get; set; } = new List<ReportTable>();

    [InverseProperty("Status")]
    public virtual ICollection<ReportTemplateCategoryTable> ReportTemplateCategoryTable { get; set; } = new List<ReportTemplateCategoryTable>();

    [InverseProperty("Status")]
    public virtual ICollection<ReportTemplateFieldTable> ReportTemplateFieldTable { get; set; } = new List<ReportTemplateFieldTable>();

    [InverseProperty("Status")]
    public virtual ICollection<ReportTemplateFileTable> ReportTemplateFileTable { get; set; } = new List<ReportTemplateFileTable>();

    [InverseProperty("Status")]
    public virtual ICollection<ReportTemplateTable> ReportTemplateTable { get; set; } = new List<ReportTemplateTable>();

    [InverseProperty("Status")]
    public virtual ICollection<RequestQuotationTable> RequestQuotationTable { get; set; } = new List<RequestQuotationTable>();

    [InverseProperty("Status")]
    public virtual ICollection<RetirementTypeTable> RetirementTypeTable { get; set; } = new List<RetirementTypeTable>();

    [InverseProperty("Status")]
    public virtual ICollection<ScreenFilterLineItemTable> ScreenFilterLineItemTable { get; set; } = new List<ScreenFilterLineItemTable>();

    [InverseProperty("Status")]
    public virtual ICollection<ScreenFilterTable> ScreenFilterTable { get; set; } = new List<ScreenFilterTable>();

    [InverseProperty("Status")]
    public virtual ICollection<SectionTable> SectionTable { get; set; } = new List<SectionTable>();

    [InverseProperty("Status")]
    public virtual ICollection<SupplierQuotationTable> SupplierQuotationTable { get; set; } = new List<SupplierQuotationTable>();

    [InverseProperty("Status")]
    public virtual ICollection<TransactionLineItemTable> TransactionLineItemTable { get; set; } = new List<TransactionLineItemTable>();

    [InverseProperty("Status")]
    public virtual ICollection<TransactionScheduleTable> TransactionScheduleTable { get; set; } = new List<TransactionScheduleTable>();

    [InverseProperty("Status")]
    public virtual ICollection<TransactionTable> TransactionTable { get; set; } = new List<TransactionTable>();

    [InverseProperty("Status")]
    public virtual ICollection<TransferTypeTable> TransferTypeTable { get; set; } = new List<TransferTypeTable>();

    [InverseProperty("Status")]
    public virtual ICollection<UOMTable> UOMTable { get; set; } = new List<UOMTable>();

    [InverseProperty("Status")]
    public virtual ICollection<UserApprovalRoleMappingTable> UserApprovalRoleMappingTable { get; set; } = new List<UserApprovalRoleMappingTable>();

    [InverseProperty("Status")]
    public virtual ICollection<UserCategoryMappingTable> UserCategoryMappingTable { get; set; } = new List<UserCategoryMappingTable>();

    [InverseProperty("Status")]
    public virtual ICollection<UserCompanyMappingTable> UserCompanyMappingTable { get; set; } = new List<UserCompanyMappingTable>();

    [InverseProperty("Status")]
    public virtual ICollection<UserDepartmentMappingTable> UserDepartmentMappingTable { get; set; } = new List<UserDepartmentMappingTable>();

    [InverseProperty("Status")]
    public virtual ICollection<UserLocationMappingTable> UserLocationMappingTable { get; set; } = new List<UserLocationMappingTable>();

    [InverseProperty("Status")]
    public virtual ICollection<UserReportFilterLineItemTable> UserReportFilterLineItemTable { get; set; } = new List<UserReportFilterLineItemTable>();

    [InverseProperty("Status")]
    public virtual ICollection<UserReportFilterTable> UserReportFilterTable { get; set; } = new List<UserReportFilterTable>();

    [InverseProperty("Status")]
    public virtual ICollection<UserTypeTable> UserTypeTable { get; set; } = new List<UserTypeTable>();

    [InverseProperty("Status")]
    public virtual ICollection<VATTable> VATTable { get; set; } = new List<VATTable>();

    [InverseProperty("Status")]
    public virtual ICollection<WarehouseTable> WarehouseTable { get; set; } = new List<WarehouseTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public StatusTable()
    {

    }

	/// <summary>
    /// Default constructor for StatusTable
    /// </summary>
    /// <param name="db"></param>
	public StatusTable(AMSContext _db)
	{
		this.StatusID = (byte) StatusValue.Active;
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "StatusID";
	}

	public override object GetPrimaryKeyValue()
	{
		return StatusID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.Status)) this.Status = this.Status.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		this.StatusID = (int) StatusValue.Deleted;
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static StatusTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.StatusTable
                where b.StatusID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<StatusTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.StatusTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.StatusID descending
                    select b);
        }
        else
        {
            return (from b in _db.StatusTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.StatusID descending
                    select b);
        }
    }

    public static IQueryable<StatusTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return StatusTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.StatusTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return StatusTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return StatusTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
