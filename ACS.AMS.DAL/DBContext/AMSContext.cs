using System;
using System.Collections.Generic;
using ACS.AMS.DAL.DBModel;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBContext;

public partial class AMSContext : DbContext
{
    public AMSContext(DbContextOptions<AMSContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AFieldTypeTable> AFieldTypeTable { get; set; }

    public virtual DbSet<ASelectionControlQueryTable> ASelectionControlQueryTable { get; set; }

    public virtual DbSet<App_Applications> App_Applications { get; set; }

    public virtual DbSet<ApplicationErrorLogTable> ApplicationErrorLogTable { get; set; }

    public virtual DbSet<ApprovalHistoryTable> ApprovalHistoryTable { get; set; }

    public virtual DbSet<ApprovalHistoryView> ApprovalHistoryView { get; set; }

    public virtual DbSet<ApprovalLocationTypeTable> ApprovalLocationTypeTable { get; set; }

    public virtual DbSet<ApprovalRoleDescriptionTable> ApprovalRoleDescriptionTable { get; set; }

    public virtual DbSet<ApprovalRoleTable> ApprovalRoleTable { get; set; }

    public virtual DbSet<ApprovalStatusTable> ApprovalStatusTable { get; set; }

    public virtual DbSet<ApprovalTransactionHistoryTable> ApprovalTransactionHistoryTable { get; set; }

    public virtual DbSet<ApproveModuleTable> ApproveModuleTable { get; set; }

    public virtual DbSet<ApproveWorkflowLineItemTable> ApproveWorkflowLineItemTable { get; set; }

    public virtual DbSet<ApproveWorkflowTable> ApproveWorkflowTable { get; set; }

    public virtual DbSet<AssetConditionDescriptionTable> AssetConditionDescriptionTable { get; set; }

    public virtual DbSet<AssetConditionTable> AssetConditionTable { get; set; }

    public virtual DbSet<AssetNewView> AssetNewView { get; set; }

    public virtual DbSet<AssetRetirementApprovalView> AssetRetirementApprovalView { get; set; }

    public virtual DbSet<AssetTable> AssetTable { get; set; }

    public virtual DbSet<AssetTransactionLineItemTable> AssetTransactionLineItemTable { get; set; }

    public virtual DbSet<AssetTransactionTable> AssetTransactionTable { get; set; }

    public virtual DbSet<AssetTransferApprovalView> AssetTransferApprovalView { get; set; }

    public virtual DbSet<AssetTransferCategoryHistoryTable> AssetTransferCategoryHistoryTable { get; set; }

    public virtual DbSet<AssetTransferHistoryTable> AssetTransferHistoryTable { get; set; }

    public virtual DbSet<AssetTransferTransactionLineItemTable> AssetTransferTransactionLineItemTable { get; set; }

    public virtual DbSet<AssetTransferTransactionTable> AssetTransferTransactionTable { get; set; }

    public virtual DbSet<AssetimportDocumentHistoryTable> AssetimportDocumentHistoryTable { get; set; }

    public virtual DbSet<AttachmentFormatTable> AttachmentFormatTable { get; set; }

    public virtual DbSet<AttributeDefinitionTable> AttributeDefinitionTable { get; set; }

    public virtual DbSet<AuditLogLineItemTable> AuditLogLineItemTable { get; set; }

    public virtual DbSet<AuditLogTable> AuditLogTable { get; set; }

    public virtual DbSet<AuditLogTransactionTable> AuditLogTransactionTable { get; set; }

    public virtual DbSet<BarcodeAutoSequenceTable> BarcodeAutoSequenceTable { get; set; }

    public virtual DbSet<BarcodeConstructSequenceTable> BarcodeConstructSequenceTable { get; set; }

    public virtual DbSet<BarcodeConstructTable> BarcodeConstructTable { get; set; }

    public virtual DbSet<BarcodeFormatsTable> BarcodeFormatsTable { get; set; }

    public virtual DbSet<BudgetYearTable> BudgetYearTable { get; set; }

    public virtual DbSet<CategoryDescriptionTable> CategoryDescriptionTable { get; set; }

    public virtual DbSet<CategoryListView> CategoryListView { get; set; }

    public virtual DbSet<CategoryNewHierarchicalView> CategoryNewHierarchicalView { get; set; }

    public virtual DbSet<CategoryNewView> CategoryNewView { get; set; }

    public virtual DbSet<CategoryTable> CategoryTable { get; set; }

    public virtual DbSet<CategoryTypeDescriptionTable> CategoryTypeDescriptionTable { get; set; }

    public virtual DbSet<CategoryTypeTable> CategoryTypeTable { get; set; }

    public virtual DbSet<CompanyDescriptionTable> CompanyDescriptionTable { get; set; }

    public virtual DbSet<CompanyTable> CompanyTable { get; set; }

    public virtual DbSet<ConfigurationTable> ConfigurationTable { get; set; }

    public virtual DbSet<CostTypeDescriptionTable> CostTypeDescriptionTable { get; set; }

    public virtual DbSet<CostTypeTable> CostTypeTable { get; set; }

    public virtual DbSet<CountryDescriptionTable> CountryDescriptionTable { get; set; }

    public virtual DbSet<CountryTable> CountryTable { get; set; }

    public virtual DbSet<CurrencyConversionTable> CurrencyConversionTable { get; set; }

    public virtual DbSet<CurrencyDescriptionTable> CurrencyDescriptionTable { get; set; }

    public virtual DbSet<CurrencyTable> CurrencyTable { get; set; }

    public virtual DbSet<DashboardFieldMappingTable> DashboardFieldMappingTable { get; set; }

    public virtual DbSet<DashboardMappingTable> DashboardMappingTable { get; set; }

    public virtual DbSet<DashboardTemplateFieldTable> DashboardTemplateFieldTable { get; set; }

    public virtual DbSet<DashboardTemplateFilterFieldTable> DashboardTemplateFilterFieldTable { get; set; }

    public virtual DbSet<DashboardTemplateTable> DashboardTemplateTable { get; set; }

    public virtual DbSet<DashboardTypeTable> DashboardTypeTable { get; set; }

    public virtual DbSet<DataTypeTable> DataTypeTable { get; set; }

    public virtual DbSet<DelegateRoleTable> DelegateRoleTable { get; set; }

    public virtual DbSet<DepartmentDescriptionTable> DepartmentDescriptionTable { get; set; }

    public virtual DbSet<DepartmentTable> DepartmentTable { get; set; }

    public virtual DbSet<DepreciationClassLineItemTable> DepreciationClassLineItemTable { get; set; }

    public virtual DbSet<DepreciationClassTable> DepreciationClassTable { get; set; }

    public virtual DbSet<DepreciationLineItemTable> DepreciationLineItemTable { get; set; }

    public virtual DbSet<DepreciationMethodTable> DepreciationMethodTable { get; set; }

    public virtual DbSet<DepreciationTable> DepreciationTable { get; set; }

    public virtual DbSet<DesignationDescriptionTable> DesignationDescriptionTable { get; set; }

    public virtual DbSet<DesignationTable> DesignationTable { get; set; }

    public virtual DbSet<DisplayControlTypeTable> DisplayControlTypeTable { get; set; }

    public virtual DbSet<DisposalTypeDescriptionTable> DisposalTypeDescriptionTable { get; set; }

    public virtual DbSet<DisposalTypeTable> DisposalTypeTable { get; set; }

    public virtual DbSet<DocumentTable> DocumentTable { get; set; }

    public virtual DbSet<DynamicColumnRequiredEntityTable> DynamicColumnRequiredEntityTable { get; set; }

    public virtual DbSet<EmailSignatureTable> EmailSignatureTable { get; set; }

    public virtual DbSet<EntityActionTable> EntityActionTable { get; set; }

    public virtual DbSet<EntityCodeTable> EntityCodeTable { get; set; }

    public virtual DbSet<EntitySummaryActionTable> EntitySummaryActionTable { get; set; }

    public virtual DbSet<EntityTable> EntityTable { get; set; }

    public virtual DbSet<ExchangeRateTable> ExchangeRateTable { get; set; }

    public virtual DbSet<ExpenseTypeTable> ExpenseTypeTable { get; set; }

    public virtual DbSet<FinalLevelRetirementView> FinalLevelRetirementView { get; set; }

    public virtual DbSet<FinalLevelTransferView> FinalLevelTransferView { get; set; }

    public virtual DbSet<GRNLineItemTable> GRNLineItemTable { get; set; }

    public virtual DbSet<GRNTable> GRNTable { get; set; }

    public virtual DbSet<ImportFormatLineItemNewTable> ImportFormatLineItemNewTable { get; set; }

    public virtual DbSet<ImportFormatNewTable> ImportFormatNewTable { get; set; }

    public virtual DbSet<ImportTemplateNewTable> ImportTemplateNewTable { get; set; }

    public virtual DbSet<ImportTemplateTypeTable> ImportTemplateTypeTable { get; set; }

    public virtual DbSet<InvoiceLineItemTable> InvoiceLineItemTable { get; set; }

    public virtual DbSet<InvoiceTable> InvoiceTable { get; set; }

    public virtual DbSet<ItemDispatchLineItemTable> ItemDispatchLineItemTable { get; set; }

    public virtual DbSet<ItemDispatchTable> ItemDispatchTable { get; set; }

    public virtual DbSet<ItemReqestLineItemTable> ItemReqestLineItemTable { get; set; }

    public virtual DbSet<ItemReqestTable> ItemReqestTable { get; set; }

    public virtual DbSet<ItemSupplierMappingTable> ItemSupplierMappingTable { get; set; }

    public virtual DbSet<ItemTypeTable> ItemTypeTable { get; set; }

    public virtual DbSet<LanguageContentLineItemTable> LanguageContentLineItemTable { get; set; }

    public virtual DbSet<LanguageContentTable> LanguageContentTable { get; set; }

    public virtual DbSet<LanguageTable> LanguageTable { get; set; }

    public virtual DbSet<LocationDescriptionTable> LocationDescriptionTable { get; set; }

    public virtual DbSet<LocationForUserMappingView> LocationForUserMappingView { get; set; }

    public virtual DbSet<LocationNewHierarchicalView> LocationNewHierarchicalView { get; set; }

    public virtual DbSet<LocationNewView> LocationNewView { get; set; }

    public virtual DbSet<LocationTable> LocationTable { get; set; }

    public virtual DbSet<LocationTypeTable> LocationTypeTable { get; set; }

    public virtual DbSet<ManufacturerDescriptionTable> ManufacturerDescriptionTable { get; set; }

    public virtual DbSet<ManufacturerTable> ManufacturerTable { get; set; }

    public virtual DbSet<MasterGridNewLineItemTable> MasterGridNewLineItemTable { get; set; }

    public virtual DbSet<MasterGridNewTable> MasterGridNewTable { get; set; }

    public virtual DbSet<ModelDescriptionTable> ModelDescriptionTable { get; set; }

    public virtual DbSet<ModelTable> ModelTable { get; set; }

    public virtual DbSet<NotificationFieldTable> NotificationFieldTable { get; set; }

    public virtual DbSet<NotificationInputTable> NotificationInputTable { get; set; }

    public virtual DbSet<NotificationModuleFieldTable> NotificationModuleFieldTable { get; set; }

    public virtual DbSet<NotificationModuleTable> NotificationModuleTable { get; set; }

    public virtual DbSet<NotificationReportAttachmentTable> NotificationReportAttachmentTable { get; set; }

    public virtual DbSet<NotificationTemplateFieldTable> NotificationTemplateFieldTable { get; set; }

    public virtual DbSet<NotificationTemplateNotificationTypeTable> NotificationTemplateNotificationTypeTable { get; set; }

    public virtual DbSet<NotificationTemplateTable> NotificationTemplateTable { get; set; }

    public virtual DbSet<NotificationTypeTable> NotificationTypeTable { get; set; }

    public virtual DbSet<PartyDescriptionTable> PartyDescriptionTable { get; set; }

    public virtual DbSet<PartyTable> PartyTable { get; set; }

    public virtual DbSet<PartyTypeTable> PartyTypeTable { get; set; }

    public virtual DbSet<PaymentTypeTable> PaymentTypeTable { get; set; }

    public virtual DbSet<PeriodTable> PeriodTable { get; set; }

    public virtual DbSet<PersonTable> PersonTable { get; set; }

    public virtual DbSet<PersonView> PersonView { get; set; }

    public virtual DbSet<PostingStatusTable> PostingStatusTable { get; set; }

    public virtual DbSet<PriceAnalysisLineItemTable> PriceAnalysisLineItemTable { get; set; }

    public virtual DbSet<PriceAnalysisTable> PriceAnalysisTable { get; set; }

    public virtual DbSet<ProductDescriptionTable> ProductDescriptionTable { get; set; }

    public virtual DbSet<ProductTable> ProductTable { get; set; }

    public virtual DbSet<ProductUOMMappingTable> ProductUOMMappingTable { get; set; }

    public virtual DbSet<ProjectDescriptionTable> ProjectDescriptionTable { get; set; }

    public virtual DbSet<ProjectTable> ProjectTable { get; set; }

    public virtual DbSet<PurchaseOrderLineItemTable> PurchaseOrderLineItemTable { get; set; }

    public virtual DbSet<PurchaseOrderTable> PurchaseOrderTable { get; set; }

    public virtual DbSet<PurchaseTypeTable> PurchaseTypeTable { get; set; }

    public virtual DbSet<ReasonTypeTable> ReasonTypeTable { get; set; }

    public virtual DbSet<ReportFieldTable> ReportFieldTable { get; set; }

    public virtual DbSet<ReportGroupFieldTable> ReportGroupFieldTable { get; set; }

    public virtual DbSet<ReportPaperSizeTable> ReportPaperSizeTable { get; set; }

    public virtual DbSet<ReportTable> ReportTable { get; set; }

    public virtual DbSet<ReportTemplateCategoryTable> ReportTemplateCategoryTable { get; set; }

    public virtual DbSet<ReportTemplateFieldTable> ReportTemplateFieldTable { get; set; }

    public virtual DbSet<ReportTemplateFileTable> ReportTemplateFileTable { get; set; }

    public virtual DbSet<ReportTemplateTable> ReportTemplateTable { get; set; }

    public virtual DbSet<RequestQuotationLineItemTable> RequestQuotationLineItemTable { get; set; }

    public virtual DbSet<RequestQuotationTable> RequestQuotationTable { get; set; }

    public virtual DbSet<RetirementTypeDescriptionTable> RetirementTypeDescriptionTable { get; set; }

    public virtual DbSet<RetirementTypeTable> RetirementTypeTable { get; set; }

    public virtual DbSet<ScreenFilterLineItemTable> ScreenFilterLineItemTable { get; set; }

    public virtual DbSet<ScreenFilterTable> ScreenFilterTable { get; set; }

    public virtual DbSet<ScreenFilterTypeTable> ScreenFilterTypeTable { get; set; }

    public virtual DbSet<SecondLevelCategoryTable> SecondLevelCategoryTable { get; set; }

    public virtual DbSet<SectionDescriptionTable> SectionDescriptionTable { get; set; }

    public virtual DbSet<SectionTable> SectionTable { get; set; }

    public virtual DbSet<StatusTable> StatusTable { get; set; }

    public virtual DbSet<SupplierAccountDetailTable> SupplierAccountDetailTable { get; set; }

    public virtual DbSet<SupplierQuotationLineItemTable> SupplierQuotationLineItemTable { get; set; }

    public virtual DbSet<SupplierQuotationTable> SupplierQuotationTable { get; set; }

    public virtual DbSet<TransactionApprovalView> TransactionApprovalView { get; set; }

    public virtual DbSet<TransactionLineItemTable> TransactionLineItemTable { get; set; }

    public virtual DbSet<TransactionLineItemViewForTransfer> TransactionLineItemViewForTransfer { get; set; }

    public virtual DbSet<TransactionScheduleTable> TransactionScheduleTable { get; set; }

    public virtual DbSet<TransactionSubTypeTable> TransactionSubTypeTable { get; set; }

    public virtual DbSet<TransactionTable> TransactionTable { get; set; }

    public virtual DbSet<TransactionTypeTable> TransactionTypeTable { get; set; }

    public virtual DbSet<TransactionView> TransactionView { get; set; }

    public virtual DbSet<TransferTypeDescriptionTable> TransferTypeDescriptionTable { get; set; }

    public virtual DbSet<TransferTypeTable> TransferTypeTable { get; set; }

    public virtual DbSet<UOMDescriptionTable> UOMDescriptionTable { get; set; }

    public virtual DbSet<UOMTable> UOMTable { get; set; }

    public virtual DbSet<UserApprovalRoleMappingTable> UserApprovalRoleMappingTable { get; set; }

    public virtual DbSet<UserApprovalRoleMappingView> UserApprovalRoleMappingView { get; set; }

    public virtual DbSet<UserCategoryMappingTable> UserCategoryMappingTable { get; set; }

    public virtual DbSet<UserCompanyMappingTable> UserCompanyMappingTable { get; set; }

    public virtual DbSet<UserDepartmentMappingTable> UserDepartmentMappingTable { get; set; }

    public virtual DbSet<UserGridNewColumnTable> UserGridNewColumnTable { get; set; }

    public virtual DbSet<UserLocationMappingTable> UserLocationMappingTable { get; set; }

    public virtual DbSet<UserReportFilterLineItemTable> UserReportFilterLineItemTable { get; set; }

    public virtual DbSet<UserReportFilterTable> UserReportFilterTable { get; set; }

    public virtual DbSet<UserRightView> UserRightView { get; set; }

    public virtual DbSet<UserTypeTable> UserTypeTable { get; set; }

    public virtual DbSet<User_LoginUserTable> User_LoginUserTable { get; set; }

    public virtual DbSet<User_MenuTable> User_MenuTable { get; set; }

    public virtual DbSet<User_RightGroupTable> User_RightGroupTable { get; set; }

    public virtual DbSet<User_RightTable> User_RightTable { get; set; }

    public virtual DbSet<User_RoleRightTable> User_RoleRightTable { get; set; }

    public virtual DbSet<User_RoleRightView> User_RoleRightView { get; set; }

    public virtual DbSet<User_RoleTable> User_RoleTable { get; set; }

    public virtual DbSet<User_UserRightTable> User_UserRightTable { get; set; }

    public virtual DbSet<User_UserRightView> User_UserRightView { get; set; }

    public virtual DbSet<User_UserRoleTable> User_UserRoleTable { get; set; }

    public virtual DbSet<VATDescriptionTable> VATDescriptionTable { get; set; }

    public virtual DbSet<VATTable> VATTable { get; set; }

    public virtual DbSet<VATTypeTable> VATTypeTable { get; set; }

    public virtual DbSet<WarehouseDescriptionTable> WarehouseDescriptionTable { get; set; }

    public virtual DbSet<WarehouseTable> WarehouseTable { get; set; }

    public virtual DbSet<WarehouseTypeTable> WarehouseTypeTable { get; set; }

    public virtual DbSet<WebServiceLogTable> WebServiceLogTable { get; set; }

    public virtual DbSet<ZDOF_PurchaseOrderTable> ZDOF_PurchaseOrderTable { get; set; }

    public virtual DbSet<ZDOF_UserPODataTable> ZDOF_UserPODataTable { get; set; }

    public virtual DbSet<vwDashboardTemplateObjects> vwDashboardTemplateObjects { get; set; }

    public virtual DbSet<vwNotificationTemplateObjects> vwNotificationTemplateObjects { get; set; }

    public virtual DbSet<vwReportTemplateObjects> vwReportTemplateObjects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AFieldTypeTable>(entity =>
        {
            entity.HasKey(e => e.FieldTypeID).HasName("PK__AFieldTy__74418A82D5E5B0F0");

            entity.HasOne(d => d.SelectionControlQuery).WithMany(p => p.AFieldTypeTable).HasConstraintName("FK__AFieldTyp__Selec__6E0C4425");
        });

        modelBuilder.Entity<ASelectionControlQueryTable>(entity =>
        {
            entity.HasKey(e => e.SelectionControlQueryID).HasName("PK__ASelecti__ED4477805FF7B0AE");
        });

        modelBuilder.Entity<App_Applications>(entity =>
        {
            entity.HasKey(e => e.ApplicationID).HasName("PK__App_Appl__C93A4F79D7D62E89");
        });

        modelBuilder.Entity<ApplicationErrorLogTable>(entity =>
        {
            entity.HasKey(e => e.ApplicationErrorLogID).HasName("PK__Applicat__7790566ECFF334A8");

            entity.Property(e => e.HostName).HasDefaultValueSql("(host_name())");
            entity.Property(e => e.OccuredDateTime).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<ApprovalHistoryTable>(entity =>
        {
            entity.HasKey(e => e.ApprovalHistoryID).HasName("PK__Approval__46B53267F77AE57F");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("trg_AlertDataInputTable_Ins");
                    tb.HasTrigger("trg_AlertDataInputTable_Upt");
                });

            entity.HasOne(d => d.ApprovalRole).WithMany(p => p.ApprovalHistoryTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ApprovalH__Appro__33AA9866");

            entity.HasOne(d => d.ApproveModule).WithMany(p => p.ApprovalHistoryTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ApprovalH__Appro__32B6742D");

            entity.HasOne(d => d.ApproveWorkFlow).WithMany(p => p.ApprovalHistoryTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ApprovalH__Appro__30CE2BBB");

            entity.HasOne(d => d.ApproveWorkFlowLineItem).WithMany(p => p.ApprovalHistoryTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ApprovalH__Appro__31C24FF4");

            entity.HasOne(d => d.CategoryType).WithMany(p => p.ApprovalHistoryTable).HasConstraintName("FK__ApprovalH__Categ__41AE9EFA");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ApprovalHistoryTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ApprovalH__Creat__396371BC");

            entity.HasOne(d => d.FromLocation).WithMany(p => p.ApprovalHistoryTableFromLocation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ApprovalH__FromL__349EBC9F");

            entity.HasOne(d => d.FromLocationType).WithMany(p => p.ApprovalHistoryTableFromLocationType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ApprovalH__FromL__36870511");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.ApprovalHistoryTableLastModifiedByNavigation).HasConstraintName("FK__ApprovalH__LastM__3A5795F5");

            entity.HasOne(d => d.Status).WithMany(p => p.ApprovalHistoryTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ApprovalH__Statu__386F4D83");

            entity.HasOne(d => d.ToLocation).WithMany(p => p.ApprovalHistoryTableToLocation).HasConstraintName("FK__ApprovalH__ToLoc__3592E0D8");

            entity.HasOne(d => d.ToLocationType).WithMany(p => p.ApprovalHistoryTableToLocationType).HasConstraintName("FK__ApprovalH__ToLoc__377B294A");
        });

        modelBuilder.Entity<ApprovalHistoryView>(entity =>
        {
            entity.ToView("ApprovalHistoryView");
        });

        modelBuilder.Entity<ApprovalLocationTypeTable>(entity =>
        {
            entity.HasKey(e => e.ApprovalLocationTypeID).HasName("PK__Approval__5878911ABCBF110B");

            entity.Property(e => e.ApprovalLocationTypeID).ValueGeneratedNever();
        });

        modelBuilder.Entity<ApprovalRoleDescriptionTable>(entity =>
        {
            entity.HasKey(e => e.ApprovalRoleDescriptionID).HasName("PK__Approval__D8923E1418DF33F9");

            entity.HasOne(d => d.ApprovalRole).WithMany(p => p.ApprovalRoleDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ApprovalR__Appro__0C90CB45");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ApprovalRoleDescriptionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ApprovalR__Creat__0D84EF7E");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.ApprovalRoleDescriptionTableLastModifiedByNavigation).HasConstraintName("FK__ApprovalR__LastM__0E7913B7");
        });

        modelBuilder.Entity<ApprovalRoleTable>(entity =>
        {
            entity.HasKey(e => e.ApprovalRoleID).HasName("PK__Approval__E01C7522CBD9A5B3");

            entity.Property(e => e.UpdateDestinationLocationsForTransfer).HasDefaultValue(false);
            entity.Property(e => e.UpdateRetirementDetailsForEachAssets).HasDefaultValue(false);

            entity.HasOne(d => d.ApprovalLocationType).WithMany(p => p.ApprovalRoleTable).HasConstraintName("FK__ApprovalR__Appro__7FF5EA36");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ApprovalRoleTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ApprovalR__Creat__08C03A61");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.ApprovalRoleTableLastModifiedByNavigation).HasConstraintName("FK__ApprovalR__LastM__09B45E9A");

            entity.HasOne(d => d.Status).WithMany(p => p.ApprovalRoleTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ApprovalR__Statu__07CC1628");
        });

        modelBuilder.Entity<ApprovalStatusTable>(entity =>
        {
            entity.HasKey(e => e.ApprovalStatusID).HasName("PK__Approval__08E526185D6614EB");

            entity.Property(e => e.ApprovalStatusID).ValueGeneratedNever();
        });

        modelBuilder.Entity<ApprovalTransactionHistoryTable>(entity =>
        {
            entity.HasKey(e => e.ApprovalTransactionHistoryID).HasName("PK__Approval__8BF36029F434667B");

            entity.HasOne(d => d.ApprovalHistory).WithMany(p => p.ApprovalTransactionHistoryTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ApprovalT__Appro__39D87308");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ApprovalTransactionHistoryTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ApprovalT__Creat__3BC0BB7A");

            entity.HasOne(d => d.Status).WithMany(p => p.ApprovalTransactionHistoryTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ApprovalT__Statu__3ACC9741");
        });

        modelBuilder.Entity<ApproveModuleTable>(entity =>
        {
            entity.HasKey(e => e.ApproveModuleID).HasName("PK__ApproveM__A3D11DEC29C53AE1");

            entity.Property(e => e.ApproveModuleID).ValueGeneratedNever();

            entity.HasOne(d => d.Status).WithMany(p => p.ApproveModuleTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ApproveMo__Statu__218BE82B");
        });

        modelBuilder.Entity<ApproveWorkflowLineItemTable>(entity =>
        {
            entity.HasKey(e => e.ApproveWorkFlowLineItemID).HasName("PK__ApproveW__B76F4F169BC07A44");

            entity.HasOne(d => d.ApprovalRole).WithMany(p => p.ApproveWorkflowLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ApproveWo__Appro__2CFD9AD7");

            entity.HasOne(d => d.ApproveWorkFlow).WithMany(p => p.ApproveWorkflowLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ApproveWo__Appro__2C09769E");

            entity.HasOne(d => d.Status).WithMany(p => p.ApproveWorkflowLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ApproveWo__Statu__2DF1BF10");
        });

        modelBuilder.Entity<ApproveWorkflowTable>(entity =>
        {
            entity.HasKey(e => e.ApproveWorkflowID).HasName("PK__ApproveW__1F18A41474DC9071");

            entity.HasOne(d => d.ApproveModule).WithMany(p => p.ApproveWorkflowTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ApproveWo__Appro__246854D6");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ApproveWorkflowTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ApproveWo__Creat__2838E5BA");

            entity.HasOne(d => d.FromLocationType).WithMany(p => p.ApproveWorkflowTableFromLocationType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ApproveWo__FromL__255C790F");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.ApproveWorkflowTableLastModifiedByNavigation).HasConstraintName("FK__ApproveWo__LastM__292D09F3");

            entity.HasOne(d => d.Status).WithMany(p => p.ApproveWorkflowTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ApproveWo__Statu__2744C181");

            entity.HasOne(d => d.ToLocationType).WithMany(p => p.ApproveWorkflowTableToLocationType).HasConstraintName("FK__ApproveWo__ToLoc__26509D48");
        });

        modelBuilder.Entity<AssetConditionDescriptionTable>(entity =>
        {
            entity.HasKey(e => e.AssetConditionDescriptionID).HasName("PK__AssetCon__CB5A9A94E94B8A1F");

            entity.HasOne(d => d.AssetCondition).WithMany(p => p.AssetConditionDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetCond__Asset__0E04126B");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AssetConditionDescriptionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetCond__Creat__1590259A");

            entity.HasOne(d => d.Language).WithMany(p => p.AssetConditionDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetCond__Langu__0EF836A4");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.AssetConditionDescriptionTableLastModifiedByNavigation).HasConstraintName("FK__AssetCond__LastM__17786E0C");
        });

        modelBuilder.Entity<AssetConditionTable>(entity =>
        {
            entity.HasKey(e => e.AssetConditionID).HasName("PK__AssetCon__06285944A8F4615A");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AssetConditionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetCond__Creat__149C0161");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.AssetConditionTableLastModifiedByNavigation).HasConstraintName("FK__AssetCond__LastM__168449D3");

            entity.HasOne(d => d.Status).WithMany(p => p.AssetConditionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetCond__Statu__093F5D4E");
        });

        modelBuilder.Entity<AssetNewView>(entity =>
        {
            entity.ToView("AssetNewView");
        });

        modelBuilder.Entity<AssetRetirementApprovalView>(entity =>
        {
            entity.ToView("AssetRetirementApprovalView");
        });

        modelBuilder.Entity<AssetTable>(entity =>
        {
            entity.Property(e => e.AllowTransfer).HasDefaultValue(false);
            entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DOF_MASS_SPLIT_EXECUTED).HasDefaultValue(false);
            entity.Property(e => e.DisposalValue).HasDefaultValue(0m);
            entity.Property(e => e.InsertedToOracle).HasDefaultValue(true);
            entity.Property(e => e.PurchasePrice).HasDefaultValue(0m);

            entity.HasOne(d => d.AssetCondition).WithMany(p => p.AssetTable).HasConstraintName("FK_AssetTable_AssetConditionTable");

            entity.HasOne(d => d.Category).WithMany(p => p.AssetTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AssetTable_CategoryTable");

            entity.HasOne(d => d.Company).WithMany(p => p.AssetTable).HasConstraintName("FK__AssetTabl__Compa__10AB74EC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AssetTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetTabl__Creat__711DBAFA");

            entity.HasOne(d => d.Custodian).WithMany(p => p.AssetTable).HasConstraintName("FK_AssetTable_PersonTable2");

            entity.HasOne(d => d.CustodianNavigation).WithMany(p => p.AssetTableCustodianNavigation).HasConstraintName("FK_AssetTable_CustodianID");

            entity.HasOne(d => d.Department).WithMany(p => p.AssetTable).HasConstraintName("FK_AssetTable_DepartmentTable");

            entity.HasOne(d => d.DepreciationClass).WithMany(p => p.AssetTable).HasConstraintName("FK_AssetTable_DepreciationClassTable");

            entity.HasOne(d => d.DisposalType).WithMany(p => p.AssetTable).HasConstraintName("FK_AssetTable_DisposalTypeID");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.AssetTableLastModifiedByNavigation).HasConstraintName("FK__AssetTabl__LastM__7211DF33");

            entity.HasOne(d => d.Location).WithMany(p => p.AssetTable).HasConstraintName("FK_AssetTable_LocationTable");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.AssetTable).HasConstraintName("FK_AssetTable_ManufacturerTable");

            entity.HasOne(d => d.Model).WithMany(p => p.AssetTable).HasConstraintName("FK_AssetTable_ModelTable");

            entity.HasOne(d => d.Product).WithMany(p => p.AssetTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AssetTable_ProductTable");

            entity.HasOne(d => d.RetirementType).WithMany(p => p.AssetTable).HasConstraintName("FK__AssetTabl__Retir__163A3110");

            entity.HasOne(d => d.Section).WithMany(p => p.AssetTable).HasConstraintName("FK_AssetTable_SectionTable");

            entity.HasOne(d => d.Status).WithMany(p => p.AssetTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AssetTable_StatusTable");

            entity.HasOne(d => d.Supplier).WithMany(p => p.AssetTable).HasConstraintName("FK_AssetTable_SupplierTable");
        });

        modelBuilder.Entity<AssetTransactionLineItemTable>(entity =>
        {
            entity.HasKey(e => e.AssetTransactionLineItemID).HasName("PK__AssetTra__FC193AD8895D42A6");

            entity.HasOne(d => d.Asset).WithMany(p => p.AssetTransactionLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetTran__Asset__32D66FBE");

            entity.HasOne(d => d.AssetTransaction).WithMany(p => p.AssetTransactionLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetTran__Asset__31E24B85");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AssetTransactionLineItemTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetTran__Creat__34BEB830");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.AssetTransactionLineItemTableLastModifiedByNavigation).HasConstraintName("FK__AssetTran__LastM__35B2DC69");

            entity.HasOne(d => d.Status).WithMany(p => p.AssetTransactionLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetTran__Statu__33CA93F7");
        });

        modelBuilder.Entity<AssetTransactionTable>(entity =>
        {
            entity.HasKey(e => e.AssetTransactionID).HasName("PK__AssetTra__B48ABEE71DBDDAE0");

            entity.Property(e => e.TransactionDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AssetTransactionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetTran__Creat__2E11BAA1");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.AssetTransactionTableLastModifiedByNavigation).HasConstraintName("FK__AssetTran__LastM__2F05DEDA");

            entity.HasOne(d => d.Status).WithMany(p => p.AssetTransactionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetTran__Statu__2D1D9668");

            entity.HasOne(d => d.Vendor).WithMany(p => p.AssetTransactionTable).HasConstraintName("FK__AssetTran__Vendo__31AD415B");
        });

        modelBuilder.Entity<AssetTransferApprovalView>(entity =>
        {
            entity.ToView("AssetTransferApprovalView");
        });

        modelBuilder.Entity<AssetTransferCategoryHistoryTable>(entity =>
        {
            entity.HasKey(e => e.AssetTransferCategoryHistoryID).HasName("PK__AssetTra__54C0C2364404DC4F");

            entity.HasOne(d => d.Asset).WithMany(p => p.AssetTransferCategoryHistoryTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetTran__Asset__379037E3");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.AssetTransferCategoryHistoryTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetTran__Creat__186C9245");

            entity.HasOne(d => d.NewCategory).WithMany(p => p.AssetTransferCategoryHistoryTableNewCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetTran__NewCa__39788055");

            entity.HasOne(d => d.NewProduct).WithMany(p => p.AssetTransferCategoryHistoryTableNewProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetTran__NewPr__3A6CA48E");

            entity.HasOne(d => d.OldCategory).WithMany(p => p.AssetTransferCategoryHistoryTableOldCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetTran__OldCa__3B60C8C7");

            entity.HasOne(d => d.OldProduct).WithMany(p => p.AssetTransferCategoryHistoryTableOldProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetTran__OldPr__3C54ED00");

            entity.HasOne(d => d.Status).WithMany(p => p.AssetTransferCategoryHistoryTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetTran__Statu__3D491139");
        });

        modelBuilder.Entity<AssetTransferHistoryTable>(entity =>
        {
            entity.HasKey(e => e.AssetHistoryID).HasName("PK__AssetTra__5681DDED182C5F62");

            entity.Property(e => e.TransferDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.AssetCondition).WithMany(p => p.AssetTransferHistoryTable).HasConstraintName("FK_AssetTransfer_AssetConditionTable");

            entity.HasOne(d => d.Asset).WithMany(p => p.AssetTransferHistoryTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetTran__Asset__20E1DCB5");

            entity.HasOne(d => d.NewCustodian).WithMany(p => p.AssetTransferHistoryTableNewCustodian).HasConstraintName("FK_AssetTransferHistoryTable_NewCustodianID");

            entity.HasOne(d => d.NewDepartment).WithMany(p => p.AssetTransferHistoryTableNewDepartment).HasConstraintName("FK__AssetTran__NewDe__2A363CC5");

            entity.HasOne(d => d.NewLocation).WithMany(p => p.AssetTransferHistoryTableNewLocation).HasConstraintName("FK__AssetTran__NewLo__2B2A60FE");

            entity.HasOne(d => d.NewSection).WithMany(p => p.AssetTransferHistoryTableNewSection).HasConstraintName("FK__AssetTran__NewSe__2C1E8537");

            entity.HasOne(d => d.OldCustodian).WithMany(p => p.AssetTransferHistoryTableOldCustodian).HasConstraintName("FK_AssetTransferHistoryTable_OldCustodianID");

            entity.HasOne(d => d.OldDepartment).WithMany(p => p.AssetTransferHistoryTableOldDepartment).HasConstraintName("FK__AssetTran__OldDe__2D12A970");

            entity.HasOne(d => d.OldLocation).WithMany(p => p.AssetTransferHistoryTableOldLocation).HasConstraintName("FK__AssetTran__OldLo__2E06CDA9");

            entity.HasOne(d => d.OldSection).WithMany(p => p.AssetTransferHistoryTableOldSection).HasConstraintName("FK__AssetTran__OldSe__2EFAF1E2");

            entity.HasOne(d => d.Status).WithMany(p => p.AssetTransferHistoryTable).HasConstraintName("FK__AssetTran__Statu__2FEF161B");

            entity.HasOne(d => d.TransferByNavigation).WithMany(p => p.AssetTransferHistoryTableTransferByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetTran__Trans__30E33A54");

            entity.HasOne(d => d.TransferType).WithMany(p => p.AssetTransferHistoryTable).HasConstraintName("FK__AssetTran__Trans__31D75E8D");
        });

        modelBuilder.Entity<AssetTransferTransactionLineItemTable>(entity =>
        {
            entity.HasKey(e => e.AssetTransferTransactionLineItemID).HasName("PK__AssetTra__B874BB17151E300E");

            entity.HasOne(d => d.Asset).WithMany(p => p.AssetTransferTransactionLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetTran__Asset__6F00685E");

            entity.HasOne(d => d.AssetTransferTransaction).WithMany(p => p.AssetTransferTransactionLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetTran__Asset__6E0C4425");
        });

        modelBuilder.Entity<AssetTransferTransactionTable>(entity =>
        {
            entity.HasKey(e => e.AssetTransferTransactionID).HasName("PK__AssetTra__EA972BCAA668E7F2");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AssetTransferTransactionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetTran__Creat__6A3BB341");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.AssetTransferTransactionTableLastModifiedByNavigation).HasConstraintName("FK__AssetTran__LastM__6B2FD77A");

            entity.HasOne(d => d.Location).WithMany(p => p.AssetTransferTransactionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetTran__Locat__68536ACF");

            entity.HasOne(d => d.Status).WithMany(p => p.AssetTransferTransactionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetTran__Statu__69478F08");
        });

        modelBuilder.Entity<AssetimportDocumentHistoryTable>(entity =>
        {
            entity.HasKey(e => e.AssetimportDocumentHistoryID).HasName("PK__Assetimp__61389E4CA28C7936");

            entity.HasOne(d => d.Status).WithMany(p => p.AssetimportDocumentHistoryTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Assetimpo__Statu__4826925F");

            entity.HasOne(d => d.UploadedByNavigation).WithMany(p => p.AssetimportDocumentHistoryTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Assetimpo__Uploa__491AB698");
        });

        modelBuilder.Entity<AttachmentFormatTable>(entity =>
        {
            entity.HasKey(e => e.AttachmentFormatID).HasName("PK__Attachme__EC9A8D16ADAD0410");

            entity.Property(e => e.AttachmentFormatID).ValueGeneratedNever();
        });

        modelBuilder.Entity<AttributeDefinitionTable>(entity =>
        {
            entity.HasKey(e => e.AttributeDefinitionID).HasName("PK__Attribut__DDC68C77209FE943");

            entity.Property(e => e.StringSize).HasDefaultValue(50);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AttributeDefinitionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Attribute__Creat__7128A7F2");

            entity.HasOne(d => d.DataTypeNavigation).WithMany(p => p.AttributeDefinitionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Attribute__DataT__66AB197F");

            entity.HasOne(d => d.DisplayControlNavigation).WithMany(p => p.AttributeDefinitionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Attribute__Displ__679F3DB8");

            entity.HasOne(d => d.Entity).WithMany(p => p.AttributeDefinitionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Attribute__Entit__65B6F546");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.AttributeDefinitionTableModifiedByNavigation).HasConstraintName("FK__Attribute__Modif__721CCC2B");

            entity.HasOne(d => d.RangeValidateFromNavigation).WithMany(p => p.InverseRangeValidateFromNavigation).HasConstraintName("FK__Attribute__Range__6E4C3B47");

            entity.HasOne(d => d.Status).WithMany(p => p.AttributeDefinitionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Attribute__Statu__703483B9");
        });

        modelBuilder.Entity<AuditLogLineItemTable>(entity =>
        {
            entity.HasKey(e => e.AuditLogLineItemID).HasName("PK__AuditLog__11B0EE050CE31F01");

            entity.HasOne(d => d.AuditLog).WithMany(p => p.AuditLogLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AuditLogLineItemTable_AuditLogID");
        });

        modelBuilder.Entity<AuditLogTable>(entity =>
        {
            entity.HasKey(e => e.AuditLogID).HasName("PK__AuditLog__EB5F6CDD3195C73B");

            entity.HasOne(d => d.AuditLogTransaction).WithMany(p => p.AuditLogTable).HasConstraintName("FK_AuditLogTable_AuditLogTransactionID");
        });

        modelBuilder.Entity<AuditLogTransactionTable>(entity =>
        {
            entity.HasKey(e => e.AuditLogTransactionID).HasName("PK__AuditLog__A53D6FD302DE1AA9");
        });

        modelBuilder.Entity<BarcodeAutoSequenceTable>(entity =>
        {
            entity.HasKey(e => e.BarcodeAutoID).HasName("PK__BarcodeA__365F5CA222A65DE6");

            entity.HasOne(d => d.Status).WithMany(p => p.BarcodeAutoSequenceTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BarcodeAu__Statu__2942188C");
        });

        modelBuilder.Entity<BarcodeConstructSequenceTable>(entity =>
        {
            entity.HasKey(e => e.BarcodeSeqID).HasName("PK__BarcodeC__43490C2C6452AE87");
        });

        modelBuilder.Entity<BarcodeConstructTable>(entity =>
        {
            entity.HasKey(e => e.BarcodeConstructID).HasName("PK__BarcodeC__89A60A95E5894DD2");

            entity.HasOne(d => d.Category).WithMany(p => p.BarcodeConstructTable).HasConstraintName("FK__BarcodeCo__Categ__54F67D98");

            entity.HasOne(d => d.Department).WithMany(p => p.BarcodeConstructTable).HasConstraintName("FK__BarcodeCo__Depar__56DEC60A");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.BarcodeConstructTable).HasConstraintName("FK__BarcodeCo__LastM__5402595F");

            entity.HasOne(d => d.Location).WithMany(p => p.BarcodeConstructTable).HasConstraintName("FK__BarcodeCo__Locat__55EAA1D1");

            entity.HasOne(d => d.Section).WithMany(p => p.BarcodeConstructTable).HasConstraintName("FK__BarcodeCo__Secti__57D2EA43");
        });

        modelBuilder.Entity<BarcodeFormatsTable>(entity =>
        {
            entity.HasKey(e => e.FormatID).HasName("PK__BarcodeF__5D3DCB7931D182C6");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BarcodeFormatsTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BarcodeFo__Creat__5031C87B");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.BarcodeFormatsTableLastModifiedByNavigation).HasConstraintName("FK__BarcodeFo__LastM__5125ECB4");

            entity.HasOne(d => d.Status).WithMany(p => p.BarcodeFormatsTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BarcodeFo__Statu__4F3DA442");
        });

        modelBuilder.Entity<BudgetYearTable>(entity =>
        {
            entity.HasKey(e => e.BudgetYearID).HasName("PK__BudgetYe__375278CB5AB8C1BB");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BudgetYearTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BudgetYea__Creat__1960B67E");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.BudgetYearTableLastModifiedByNavigation).HasConstraintName("FK__BudgetYea__LastM__1A54DAB7");

            entity.HasOne(d => d.Status).WithMany(p => p.BudgetYearTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BudgetYea__Statu__5D60DB10");
        });

        modelBuilder.Entity<CategoryDescriptionTable>(entity =>
        {
            entity.HasKey(e => e.CategoryDescriptionID).HasName("PK__Category__2E2930CCEB91997D");

            entity.HasOne(d => d.Category).WithMany(p => p.CategoryDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CategoryD__Categ__753864A1");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CategoryDescriptionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CategoryD__Creat__1B48FEF0");

            entity.HasOne(d => d.Language).WithMany(p => p.CategoryDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CategoryD__Langu__762C88DA");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.CategoryDescriptionTableLastModifiedByNavigation).HasConstraintName("FK__CategoryD__LastM__1C3D2329");
        });

        modelBuilder.Entity<CategoryListView>(entity =>
        {
            entity.ToView("CategoryListView");
        });

        modelBuilder.Entity<CategoryNewHierarchicalView>(entity =>
        {
            entity.ToView("CategoryNewHierarchicalView");
        });

        modelBuilder.Entity<CategoryNewView>(entity =>
        {
            entity.ToView("CategoryNewView");
        });

        modelBuilder.Entity<CategoryTable>(entity =>
        {
            entity.HasKey(e => e.CategoryID).HasName("PK__Category__19093A2BA391B67F");

            entity.Property(e => e.IsInventory).HasDefaultValue(false);

            entity.HasOne(d => d.BudgetYear).WithMany(p => p.CategoryTable).HasConstraintName("FK__CategoryT__Budge__7073AF84");

            entity.HasOne(d => d.CategoryType).WithMany(p => p.CategoryTable).HasConstraintName("FK__CategoryT__Categ__3ED2324F");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CategoryTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CategoryT__Creat__1D314762");

            entity.HasOne(d => d.DepreciationClass).WithMany(p => p.CategoryTable).HasConstraintName("FK__CategoryT__Depre__725BF7F6");

            entity.HasOne(d => d.ExpenseType).WithMany(p => p.CategoryTable).HasConstraintName("FK__CategoryT__Expen__66161CA2");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.CategoryTableLastModifiedByNavigation).HasConstraintName("FK__CategoryT__LastM__1E256B9B");

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory).HasConstraintName("FK__CategoryT__Paren__6CA31EA0");

            entity.HasOne(d => d.Status).WithMany(p => p.CategoryTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CategoryT__Statu__6D9742D9");
        });

        modelBuilder.Entity<CategoryTypeDescriptionTable>(entity =>
        {
            entity.HasKey(e => e.CategoryTypeDescriptionID).HasName("PK__Category__CC29BA4D8A4A330F");

            entity.HasOne(d => d.CategoryType).WithMany(p => p.CategoryTypeDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CategoryT__Categ__3B01A16B");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CategoryTypeDescriptionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CategoryT__Creat__3CE9E9DD");

            entity.HasOne(d => d.Language).WithMany(p => p.CategoryTypeDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CategoryT__Langu__3BF5C5A4");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.CategoryTypeDescriptionTableLastModifiedByNavigation).HasConstraintName("FK__CategoryT__LastM__3DDE0E16");
        });

        modelBuilder.Entity<CategoryTypeTable>(entity =>
        {
            entity.HasKey(e => e.CategoryTypeID).HasName("PK__Category__7B30E78365E638E9");

            entity.Property(e => e.IsAllCategoryType).HasDefaultValue(false);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CategoryTypeTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CategoryT__Creat__37311087");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.CategoryTypeTableLastModifiedByNavigation).HasConstraintName("FK__CategoryT__LastM__382534C0");

            entity.HasOne(d => d.Status).WithMany(p => p.CategoryTypeTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CategoryT__Statu__363CEC4E");
        });

        modelBuilder.Entity<CompanyDescriptionTable>(entity =>
        {
            entity.HasKey(e => e.CompanyDescriptionID).HasName("PK__CompanyD__3BF73F893A277EC4");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CompanyDe__Compa__27F8EE98");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CompanyDescriptionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CompanyDe__Creat__1F198FD4");

            entity.HasOne(d => d.Language).WithMany(p => p.CompanyDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CompanyDe__Langu__28ED12D1");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.CompanyDescriptionTableLastModifiedByNavigation).HasConstraintName("FK__CompanyDe__LastM__200DB40D");
        });

        modelBuilder.Entity<CompanyTable>(entity =>
        {
            entity.HasKey(e => e.CompanyID).HasName("PK__CompanyT__2D971C4CEB12E9ED");

            entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.StatusID).HasDefaultValue(5);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CompanyTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CompanyTa__Creat__7306036C");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.CompanyTableLastModifiedByNavigation).HasConstraintName("FK__CompanyTa__LastM__73FA27A5");

            entity.HasOne(d => d.Status).WithMany(p => p.CompanyTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CompanyTable_StatusID");
        });

        modelBuilder.Entity<ConfigurationTable>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("trg_Upt_RoleRightsUpdate"));
        });

        modelBuilder.Entity<CostTypeDescriptionTable>(entity =>
        {
            entity.HasKey(e => e.CostTypeDescriptionID).HasName("PK__CostType__F20109212544361D");

            entity.HasOne(d => d.CostType).WithMany(p => p.CostTypeDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CostTypeD__CostT__5006DFF2");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CostTypeDescriptionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CostTypeD__Creat__2101D846");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.CostTypeDescriptionTableLastModifiedByNavigation).HasConstraintName("FK__CostTypeD__LastM__21F5FC7F");
        });

        modelBuilder.Entity<CostTypeTable>(entity =>
        {
            entity.HasKey(e => e.CostTypeID).HasName("PK__CostType__B3160951614D9E2F");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CostTypeTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CostTypeT__Creat__22EA20B8");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.CostTypeTableLastModifiedByNavigation).HasConstraintName("FK__CostTypeT__LastM__23DE44F1");

            entity.HasOne(d => d.Status).WithMany(p => p.CostTypeTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CostTypeT__Statu__4B422AD5");
        });

        modelBuilder.Entity<CountryDescriptionTable>(entity =>
        {
            entity.HasKey(e => e.CountryDescriptionID).HasName("PK__CountryD__38A8CCDE557EC4DB");

            entity.HasOne(d => d.Country).WithMany(p => p.CountryDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CountryDe__Count__3BFFE745");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CountryDescriptionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CountryDe__Creat__24D2692A");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.CountryDescriptionTableLastModifiedByNavigation).HasConstraintName("FK__CountryDe__LastM__25C68D63");
        });

        modelBuilder.Entity<CountryTable>(entity =>
        {
            entity.HasKey(e => e.CountryID).HasName("PK__CountryT__10D160BF19F9BEBA");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CountryTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CountryTa__Creat__26BAB19C");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.CountryTableLastModifiedByNavigation).HasConstraintName("FK__CountryTa__LastM__27AED5D5");

            entity.HasOne(d => d.Status).WithMany(p => p.CountryTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CountryTa__Statu__373B3228");
        });

        modelBuilder.Entity<CurrencyConversionTable>(entity =>
        {
            entity.HasKey(e => e.CurrencyConversionID).HasName("PK__Currency__EE3624C405BAE42F");

            entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CurrencyStartDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.StatusID).HasDefaultValue(50);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CurrencyConversionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CurrencyC__Creat__3B36AB95");

            entity.HasOne(d => d.FromCurrency).WithMany(p => p.CurrencyConversionTableFromCurrency)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CurrencyC__FromC__357DD23F");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.CurrencyConversionTableLastModifiedByNavigation).HasConstraintName("FK__CurrencyC__LastM__3D1EF407");

            entity.HasOne(d => d.Status).WithMany(p => p.CurrencyConversionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CurrencyC__Statu__394E6323");

            entity.HasOne(d => d.ToCurrency).WithMany(p => p.CurrencyConversionTableToCurrency)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CurrencyC__ToCur__3671F678");
        });

        modelBuilder.Entity<CurrencyDescriptionTable>(entity =>
        {
            entity.HasKey(e => e.CurrencyDescriptionID).HasName("PK__Currency__A5C234820803207F");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CurrencyDescriptionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CurrencyD__Creat__28A2FA0E");

            entity.HasOne(d => d.Currency).WithMany(p => p.CurrencyDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CurrencyD__Curre__467D75B8");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.CurrencyDescriptionTableLastModifiedByNavigation).HasConstraintName("FK__CurrencyD__LastM__29971E47");
        });

        modelBuilder.Entity<CurrencyTable>(entity =>
        {
            entity.HasKey(e => e.CurrencyID).HasName("PK__Currency__14470B10DF06CD47");

            entity.HasOne(d => d.Country).WithMany(p => p.CurrencyTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CurrencyT__Count__40C49C62");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CurrencyTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CurrencyT__Creat__2A8B4280");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.CurrencyTableLastModifiedByNavigation).HasConstraintName("FK__CurrencyT__LastM__2B7F66B9");

            entity.HasOne(d => d.Status).WithMany(p => p.CurrencyTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CurrencyT__Statu__41B8C09B");
        });

        modelBuilder.Entity<DashboardFieldMappingTable>(entity =>
        {
            entity.HasKey(e => e.DashboardFieldMappingID).HasName("PK__Dashboar__9E58CC4BD73A14D1");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DashboardFieldMappingTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dashboard__Creat__2E9BCA86");

            entity.HasOne(d => d.DashboardMapping).WithMany(p => p.DashboardFieldMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dashboard__Dashb__2BBF5DDB");

            entity.HasOne(d => d.DashboardTemplateField).WithMany(p => p.DashboardFieldMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dashboard__Dashb__2CB38214");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.DashboardFieldMappingTableLastModifiedByNavigation).HasConstraintName("FK__Dashboard__LastM__2F8FEEBF");

            entity.HasOne(d => d.Status).WithMany(p => p.DashboardFieldMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dashboard__Statu__2DA7A64D");
        });

        modelBuilder.Entity<DashboardMappingTable>(entity =>
        {
            entity.HasKey(e => e.DashboardMappingID).HasName("PK__Dashboar__0BDEAEBE8CB00DE8");

            entity.Property(e => e.DashboardHeight).HasDefaultValue("300px");
            entity.Property(e => e.DashboardWeight).HasDefaultValue("300px");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DashboardMappingTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dashboard__Creat__27EECCF7");

            entity.HasOne(d => d.DashboardTemplate).WithMany(p => p.DashboardMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dashboard__Dashb__232A17DA");

            entity.HasOne(d => d.DashboardType).WithMany(p => p.DashboardMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dashboard__Dashb__26068485");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.DashboardMappingTableLastModifiedByNavigation).HasConstraintName("FK__Dashboard__LastM__28E2F130");

            entity.HasOne(d => d.Status).WithMany(p => p.DashboardMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dashboard__Statu__26FAA8BE");
        });

        modelBuilder.Entity<DashboardTemplateFieldTable>(entity =>
        {
            entity.HasKey(e => e.DashboardTemplateFieldID).HasName("PK__Dashboar__587ABB09B109E048");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DashboardTemplateFieldTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dashboard__Creat__7C104AB9");

            entity.HasOne(d => d.DashboardTemplate).WithMany(p => p.DashboardTemplateFieldTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dashboard__Dashb__7A280247");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.DashboardTemplateFieldTableLastModifiedByNavigation).HasConstraintName("FK__Dashboard__LastM__7D046EF2");

            entity.HasOne(d => d.Status).WithMany(p => p.DashboardTemplateFieldTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dashboard__Statu__7B1C2680");
        });

        modelBuilder.Entity<DashboardTemplateFilterFieldTable>(entity =>
        {
            entity.HasKey(e => e.DashboardTemplateFilterFieldID).HasName("PK__Dashboar__7D0F9B642A1CAB7F");

            entity.Property(e => e.IsFixedFilter).HasDefaultValue(false);
            entity.Property(e => e.IsMandatory).HasDefaultValue(false);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DashboardTemplateFilterFieldTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dashboard__Creat__0599B4F3");

            entity.HasOne(d => d.DashboardTemplate).WithMany(p => p.DashboardTemplateFilterFieldTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dashboard__Dashb__7FE0DB9D");

            entity.HasOne(d => d.FieldType).WithMany(p => p.DashboardTemplateFilterFieldTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dashboard__Field__03B16C81");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.DashboardTemplateFilterFieldTableLastModifiedByNavigation).HasConstraintName("FK__Dashboard__LastM__068DD92C");

            entity.HasOne(d => d.ScreenFilterType).WithMany(p => p.DashboardTemplateFilterFieldTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dashboard__Scree__02BD4848");

            entity.HasOne(d => d.Status).WithMany(p => p.DashboardTemplateFilterFieldTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dashboard__Statu__04A590BA");
        });

        modelBuilder.Entity<DashboardTemplateTable>(entity =>
        {
            entity.HasKey(e => e.DashboardTemplateID).HasName("PK__Dashboar__E9E7402F79DDD122");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DashboardTemplateTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dashboard__Creat__76577163");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.DashboardTemplateTableLastModifiedByNavigation).HasConstraintName("FK__Dashboard__LastM__774B959C");

            entity.HasOne(d => d.Status).WithMany(p => p.DashboardTemplateTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dashboard__Statu__75634D2A");
        });

        modelBuilder.Entity<DashboardTypeTable>(entity =>
        {
            entity.HasKey(e => e.DashboardTypeID).HasName("PK__Dashboar__798283FB50DAE2FE");
        });

        modelBuilder.Entity<DataTypeTable>(entity =>
        {
            entity.HasKey(e => e.DataTypeID).HasName("PK__DataType__4382083F93886091");

            entity.Property(e => e.DataTypeID).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<DelegateRoleTable>(entity =>
        {
            entity.HasKey(e => e.DelegateRoleID).HasName("PK__Delegate__6843D02AB677EA89");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DelegateRoleTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DelegateR__Creat__1ECF7711");

            entity.HasOne(d => d.DelegatedEmployee).WithMany(p => p.DelegateRoleTableDelegatedEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DelegateR__Deleg__1DDB52D8");

            entity.HasOne(d => d.Employee).WithMany(p => p.DelegateRoleTableEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DelegateR__Emplo__1CE72E9F");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.DelegateRoleTableLastModifiedByNavigation).HasConstraintName("FK__DelegateR__LastM__1FC39B4A");

            entity.HasOne(d => d.ReasonType).WithMany(p => p.DelegateRoleTable).HasConstraintName("FK__DelegateR__Reaso__2858E14B");

            entity.HasOne(d => d.Status).WithMany(p => p.DelegateRoleTable).HasConstraintName("FK__DelegateR__Statu__2764BD12");
        });

        modelBuilder.Entity<DepartmentDescriptionTable>(entity =>
        {
            entity.HasKey(e => e.DepartmentDescriptionID).HasName("PK__Departme__92F0CADC9C4F6F41");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DepartmentDescriptionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Departmen__Creat__2D67AF2B");

            entity.HasOne(d => d.Department).WithMany(p => p.DepartmentDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Departmen__Depar__3E723F9C");

            entity.HasOne(d => d.Language).WithMany(p => p.DepartmentDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Departmen__Langu__3F6663D5");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.DepartmentDescriptionTableLastModifiedByNavigation).HasConstraintName("FK__Departmen__LastM__2F4FF79D");
        });

        modelBuilder.Entity<DepartmentTable>(entity =>
        {
            entity.HasKey(e => e.DepartmentID).HasName("PK__Departme__B2079BCD4A53BCCC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DepartmentTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Departmen__Creat__2C738AF2");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.DepartmentTableLastModifiedByNavigation).HasConstraintName("FK__Departmen__LastM__2E5BD364");

            entity.HasOne(d => d.Status).WithMany(p => p.DepartmentTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Departmen__Statu__39AD8A7F");
        });

        modelBuilder.Entity<DepreciationClassLineItemTable>(entity =>
        {
            entity.HasKey(e => e.DepreciationClassLineItemID).HasName("PK__Deprecia__6BF5A2427AEB22D1");

            entity.Property(e => e.AssetEndValue).HasDefaultValue("");
            entity.Property(e => e.AssetStartValue).HasDefaultValue("");
            entity.Property(e => e.DepreciationPercentage).HasDefaultValue(0.00m);
            entity.Property(e => e.Duration).HasDefaultValue("");

            entity.HasOne(d => d.DepreciationClass).WithMany(p => p.DepreciationClassLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Depreciat__Depre__69C6B1F5");
        });

        modelBuilder.Entity<DepreciationClassTable>(entity =>
        {
            entity.HasKey(e => e.DepreciationClassID).HasName("PK__Deprecia__ACFC13A42F6C96AB");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DepreciationClassTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Depreciat__Creat__30441BD6");

            entity.HasOne(d => d.DepreciationMethod).WithMany(p => p.DepreciationClassTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Depreciat__Depre__640DD89F");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.DepreciationClassTableLastModifiedByNavigation).HasConstraintName("FK__Depreciat__LastM__3138400F");

            entity.HasOne(d => d.Status).WithMany(p => p.DepreciationClassTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Depreciat__Statu__6501FCD8");
        });

        modelBuilder.Entity<DepreciationLineItemTable>(entity =>
        {
            entity.HasKey(e => e.DepreciationLineItemID).HasName("PK__Deprecia__5B0C2217123BA16C");

            entity.HasOne(d => d.Asset).WithMany(p => p.DepreciationLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Depreciat__Asset__5F492382");

            entity.HasOne(d => d.Category).WithMany(p => p.DepreciationLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Depreciat__Categ__1F83A428");

            entity.HasOne(d => d.DepreciationClass).WithMany(p => p.DepreciationLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Depreciat__Depre__2077C861");

            entity.HasOne(d => d.DepreciationClassLineItem).WithMany(p => p.DepreciationLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Depreciat__Depre__226010D3");

            entity.HasOne(d => d.Depreciation).WithMany(p => p.DepreciationLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Depreciat__Depre__216BEC9A");
        });

        modelBuilder.Entity<DepreciationMethodTable>(entity =>
        {
            entity.HasKey(e => e.DepreciationMethodID).HasName("PK__Deprecia__08386A5BE838F4D7");
        });

        modelBuilder.Entity<DepreciationTable>(entity =>
        {
            entity.HasKey(e => e.DepreciationID).HasName("PK__Deprecia__DEEBFE29DC41C23D");

            entity.Property(e => e.IsReclassification).HasDefaultValue(false);
            entity.Property(e => e.IsUpdateEquation).HasDefaultValue(false);
            entity.Property(e => e.IsUpdateIMALL).HasDefaultValue(false);

            entity.HasOne(d => d.Company).WithMany(p => p.DepreciationTable).HasConstraintName("FK__Depreciat__Compa__2354350C");

            entity.HasOne(d => d.DeletedApprovedByNavigation).WithMany(p => p.DepreciationTableDeletedApprovedByNavigation).HasConstraintName("FK__Depreciat__Delet__253C7D7E");

            entity.HasOne(d => d.DeletedDoneByNavigation).WithMany(p => p.DepreciationTableDeletedDoneByNavigation).HasConstraintName("FK__Depreciat__Delet__24485945");

            entity.HasOne(d => d.DepreciationApprovedByNavigation).WithMany(p => p.DepreciationTableDepreciationApprovedByNavigation).HasConstraintName("FK__Depreciat__Depre__2724C5F0");

            entity.HasOne(d => d.DepreciationDoneByNavigation).WithMany(p => p.DepreciationTableDepreciationDoneByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Depreciat__Depre__2630A1B7");

            entity.HasOne(d => d.Period).WithMany(p => p.DepreciationTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Depreciat__Perio__2818EA29");

            entity.HasOne(d => d.Status).WithMany(p => p.DepreciationTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Depreciat__Statu__290D0E62");
        });

        modelBuilder.Entity<DesignationDescriptionTable>(entity =>
        {
            entity.HasKey(e => e.DesignationDescriptionID).HasName("PK__Designat__882D5A16382DB294");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DesignationDescriptionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Designati__Creat__5728DECD");

            entity.HasOne(d => d.Designation).WithMany(p => p.DesignationDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Designati__Desig__5540965B");

            entity.HasOne(d => d.Language).WithMany(p => p.DesignationDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Designati__Langu__5634BA94");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.DesignationDescriptionTableLastModifiedByNavigation).HasConstraintName("FK__Designati__LastM__581D0306");
        });

        modelBuilder.Entity<DesignationTable>(entity =>
        {
            entity.HasKey(e => e.DesignationID).HasName("PK__Designat__BABD603E86C5DFCE");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DesignationTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Designati__Creat__322C6448");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.DesignationTableLastModifiedByNavigation).HasConstraintName("FK__Designati__LastM__3414ACBA");

            entity.HasOne(d => d.Status).WithMany(p => p.DesignationTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Designati__Statu__04CFADEC");
        });

        modelBuilder.Entity<DisplayControlTypeTable>(entity =>
        {
            entity.HasKey(e => e.DisplayControlTypeID).HasName("PK__DisplayC__23D3CC12156D89C7");

            entity.Property(e => e.DisplayControlTypeID).ValueGeneratedOnAdd();
            entity.Property(e => e.Boolean).HasDefaultValue(false);
            entity.Property(e => e.Date).HasDefaultValue(false);
            entity.Property(e => e.DateTime).HasDefaultValue(false);
            entity.Property(e => e.Decimal).HasDefaultValue(false);
            entity.Property(e => e.Integer).HasDefaultValue(false);
            entity.Property(e => e.String).HasDefaultValue(false);
            entity.Property(e => e.Time).HasDefaultValue(false);
        });

        modelBuilder.Entity<DisposalTypeDescriptionTable>(entity =>
        {
            entity.HasKey(e => e.DisposalTypeDescriptionID).HasName("PK__Disposal__13DE2B4078929699");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DisposalTypeDescriptionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DisposalT__Creat__36F11965");

            entity.HasOne(d => d.DisposalType).WithMany(p => p.DisposalTypeDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DisposalT__Dispo__567ED357");

            entity.HasOne(d => d.Language).WithMany(p => p.DisposalTypeDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DisposalT__Langu__5772F790");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.DisposalTypeDescriptionTableLastModifiedByNavigation).HasConstraintName("FK__DisposalT__LastM__38D961D7");
        });

        modelBuilder.Entity<DisposalTypeTable>(entity =>
        {
            entity.HasKey(e => e.DisposalTypeID).HasName("PK__Disposal__3BFE52D32A5B391E");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DisposalTypeTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DisposalT__Creat__35FCF52C");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.DisposalTypeTableLastModifiedByNavigation).HasConstraintName("FK__DisposalT__LastM__37E53D9E");

            entity.HasOne(d => d.Status).WithMany(p => p.DisposalTypeTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DisposalT__Statu__51BA1E3A");
        });

        modelBuilder.Entity<DocumentTable>(entity =>
        {
            entity.HasKey(e => e.DocumentID).HasName("PK__Document__1ABEEF6FAC530789");

            entity.HasOne(d => d.Status).WithMany(p => p.DocumentTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DocumentT__Statu__71DCD509");
        });

        modelBuilder.Entity<DynamicColumnRequiredEntityTable>(entity =>
        {
            entity.HasKey(e => e.DynamicColumnRequiredEntityID).HasName("PK__DynamicC__5F1FBAD5F6AA87D3");
        });

        modelBuilder.Entity<EmailSignatureTable>(entity =>
        {
            entity.HasKey(e => e.EmailSignatureID).HasName("PK__EmailSig__6844473F93567EAC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.EmailSignatureTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EmailSign__Creat__5F1F0650");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.EmailSignatureTableLastModifiedByNavigation).HasConstraintName("FK__EmailSign__LastM__60132A89");

            entity.HasOne(d => d.Status).WithMany(p => p.EmailSignatureTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EmailSign__Statu__5E2AE217");
        });

        modelBuilder.Entity<EntityActionTable>(entity =>
        {
            entity.HasKey(e => e.EntityActionID).HasName("PK__EntityAc__792321F6C7E9E423");
        });

        modelBuilder.Entity<EntityCodeTable>(entity =>
        {
            entity.HasKey(e => e.EntityCodeID).HasName("PK__EntityCo__FC357D140CDD05D3");
        });

        modelBuilder.Entity<EntityTable>(entity =>
        {
            entity.HasKey(e => e.EntityID).HasName("PK__EntityTa__9C892FFDDCC2A321");
        });

        modelBuilder.Entity<ExchangeRateTable>(entity =>
        {
            entity.HasKey(e => e.ExchangeRateID).HasName("PK__Exchange__B05604A9CF2815BE");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ExchangeRateTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ExchangeR__Creat__39CD8610");

            entity.HasOne(d => d.Currency).WithMany(p => p.ExchangeRateTableCurrency)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ExchangeR__Curre__56B3DD81");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.ExchangeRateTableLastModifiedByNavigation).HasConstraintName("FK__ExchangeR__LastM__3AC1AA49");

            entity.HasOne(d => d.Status).WithMany(p => p.ExchangeRateTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ExchangeR__Statu__589C25F3");

            entity.HasOne(d => d.ToCurrency).WithMany(p => p.ExchangeRateTableToCurrency)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ExchangeR__ToCur__57A801BA");
        });

        modelBuilder.Entity<ExpenseTypeTable>(entity =>
        {
            entity.HasKey(e => e.ExpenseTypeID).HasName("PK__ExpenseT__E082A36F790FF4D2");
        });

        modelBuilder.Entity<FinalLevelRetirementView>(entity =>
        {
            entity.ToView("FinalLevelRetirementView");
        });

        modelBuilder.Entity<FinalLevelTransferView>(entity =>
        {
            entity.ToView("FinalLevelTransferView");
        });

        modelBuilder.Entity<GRNLineItemTable>(entity =>
        {
            entity.HasKey(e => e.GRNLineItemID).HasName("PK__GRNLineI__42A301CBC76CCA22");

            entity.HasOne(d => d.GRN).WithMany(p => p.GRNLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GRNLineIt__GRNID__4E0988E7");

            entity.HasOne(d => d.Product).WithMany(p => p.GRNLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GRNLineIt__Produ__4EFDAD20");

            entity.HasOne(d => d.VAT).WithMany(p => p.GRNLineItemTable).HasConstraintName("FK__GRNLineIt__VATID__4FF1D159");
        });

        modelBuilder.Entity<GRNTable>(entity =>
        {
            entity.HasKey(e => e.GRNID).HasName("PK__GRNTable__BC0E8C6255FB5F7B");

            entity.HasOne(d => d.Company).WithMany(p => p.GRNTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GRNTable__Compan__475C8B58");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.GRNTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GRNTable__Create__3BB5CE82");

            entity.HasOne(d => d.Currency).WithMany(p => p.GRNTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GRNTable__Curren__4850AF91");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.GRNTableLastModifiedByNavigation).HasConstraintName("FK__GRNTable__LastMo__3CA9F2BB");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.GRNTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GRNTable__Purcha__457442E6");

            entity.HasOne(d => d.Status).WithMany(p => p.GRNTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GRNTable__Status__4944D3CA");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.GRNTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GRNTable__Wareho__4668671F");
        });

        modelBuilder.Entity<ImportFormatLineItemNewTable>(entity =>
        {
            entity.HasKey(e => e.ImportFormatLineItemID).HasName("PK__ImportFo__495B6D4F597E858D");

            entity.HasOne(d => d.ImportFormat).WithMany(p => p.ImportFormatLineItemNewTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ImportFor__Impor__6932806F");

            entity.HasOne(d => d.ImportTemplate).WithMany(p => p.ImportFormatLineItemNewTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ImportFor__Impor__6A26A4A8");
        });

        modelBuilder.Entity<ImportFormatNewTable>(entity =>
        {
            entity.HasKey(e => e.ImportFormatID).HasName("PK__ImportFo__D88E14F3133DAEED");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ImportFormatNewTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ImportFor__Creat__6B1AC8E1");

            entity.HasOne(d => d.Entity).WithMany(p => p.ImportFormatNewTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ImportFor__Entit__6C0EED1A");

            entity.HasOne(d => d.ImportTemplateType).WithMany(p => p.ImportFormatNewTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ImportFor__Impor__6D031153");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.ImportFormatNewTableLastModifiedByNavigation).HasConstraintName("FK__ImportFor__LastM__6DF7358C");

            entity.HasOne(d => d.Status).WithMany(p => p.ImportFormatNewTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ImportFor__Statu__6EEB59C5");
        });

        modelBuilder.Entity<ImportTemplateNewTable>(entity =>
        {
            entity.HasKey(e => e.ImportTemplateID).HasName("PK__ImportTe__AEC13CCC84A21742");

            entity.Property(e => e.IsDisplay).HasDefaultValue(true);

            entity.HasOne(d => d.Entity).WithMany(p => p.ImportTemplateNewTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ImportTem__Entit__6379A719");

            entity.HasOne(d => d.ImportTemplateType).WithMany(p => p.ImportTemplateNewTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ImportTem__Impor__646DCB52");
        });

        modelBuilder.Entity<ImportTemplateTypeTable>(entity =>
        {
            entity.HasKey(e => e.ImportTemplateTypeID).HasName("PK__ImportTe__B94083E705EE045E");
        });

        modelBuilder.Entity<InvoiceLineItemTable>(entity =>
        {
            entity.HasKey(e => e.InvoiceLineItemID).HasName("PK__InvoiceL__70466D405BBD473D");

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__InvoiceLi__Invoi__79E80B25");

            entity.HasOne(d => d.Product).WithMany(p => p.InvoiceLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__InvoiceLi__Produ__7ADC2F5E");

            entity.HasOne(d => d.VAT).WithMany(p => p.InvoiceLineItemTable).HasConstraintName("FK__InvoiceLi__VATID__7BD05397");
        });

        modelBuilder.Entity<InvoiceTable>(entity =>
        {
            entity.HasKey(e => e.InvoiceID).HasName("PK__InvoiceT__D796AAD5E517B8E5");

            entity.HasOne(d => d.Company).WithMany(p => p.InvoiceTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__InvoiceTa__Compa__733B0D96");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InvoiceTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__InvoiceTa__Creat__3D9E16F4");

            entity.HasOne(d => d.Currency).WithMany(p => p.InvoiceTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__InvoiceTa__Curre__742F31CF");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.InvoiceTableLastModifiedByNavigation).HasConstraintName("FK__InvoiceTa__LastM__3E923B2D");

            entity.HasOne(d => d.PaymentType).WithMany(p => p.InvoiceTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__InvoiceTa__Payme__7152C524");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.InvoiceTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__InvoiceTa__Purch__705EA0EB");

            entity.HasOne(d => d.Status).WithMany(p => p.InvoiceTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__InvoiceTa__Statu__75235608");

            entity.HasOne(d => d.SupplierAccountDetail).WithMany(p => p.InvoiceTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__InvoiceTa__Suppl__7246E95D");
        });

        modelBuilder.Entity<ItemDispatchLineItemTable>(entity =>
        {
            entity.HasKey(e => e.ItemDispatchLineItemID).HasName("PK__ItemDisp__41A6C1824FE7471E");

            entity.HasOne(d => d.ItemDispatch).WithMany(p => p.ItemDispatchLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemDispa__ItemD__5A6F5FCC");

            entity.HasOne(d => d.Product).WithMany(p => p.ItemDispatchLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemDispa__Produ__5B638405");

            entity.HasOne(d => d.VAT).WithMany(p => p.ItemDispatchLineItemTable).HasConstraintName("FK__ItemDispa__VATID__5C57A83E");
        });

        modelBuilder.Entity<ItemDispatchTable>(entity =>
        {
            entity.HasKey(e => e.ItemDispatchID).HasName("PK__ItemDisp__FD419B2367657255");

            entity.HasOne(d => d.Company).WithMany(p => p.ItemDispatchTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemDispa__Compa__53C2623D");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ItemDispatchTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemDispa__Creat__3F865F66");

            entity.HasOne(d => d.Currency).WithMany(p => p.ItemDispatchTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemDispa__Curre__54B68676");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.ItemDispatchTableLastModifiedByNavigation).HasConstraintName("FK__ItemDispa__LastM__407A839F");

            entity.HasOne(d => d.Status).WithMany(p => p.ItemDispatchTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemDispa__Statu__55AAAAAF");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.ItemDispatchTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemDispa__Wareh__52CE3E04");
        });

        modelBuilder.Entity<ItemReqestLineItemTable>(entity =>
        {
            entity.HasKey(e => e.ItemReqestLineItemID).HasName("PK__ItemReqe__2D474830DA3ECE9F");

            entity.HasOne(d => d.ItemRequest).WithMany(p => p.ItemReqestLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemReqes__ItemR__764C846B");

            entity.HasOne(d => d.Product).WithMany(p => p.ItemReqestLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemReqes__Produ__7740A8A4");

            entity.HasOne(d => d.UOM).WithMany(p => p.ItemReqestLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemReqes__UOMID__7834CCDD");

            entity.HasOne(d => d.VAT).WithMany(p => p.ItemReqestLineItemTable).HasConstraintName("FK__ItemReqes__VATID__7928F116");
        });

        modelBuilder.Entity<ItemReqestTable>(entity =>
        {
            entity.HasKey(e => e.ItemRequestID).HasName("PK__ItemReqe__0300A15D579C35BF");

            entity.Property(e => e.IsProject).HasDefaultValue(false);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ItemReqestTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemReqes__Creat__416EA7D8");

            entity.HasOne(d => d.Currency).WithMany(p => p.ItemReqestTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemReqes__Curre__6ADAD1BF");

            entity.HasOne(d => d.Department).WithMany(p => p.ItemReqestTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemReqes__Depar__69E6AD86");

            entity.HasOne(d => d.ItemType).WithMany(p => p.ItemReqestTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemReqes__ItemT__68F2894D");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.ItemReqestTableLastModifiedByNavigation).HasConstraintName("FK__ItemReqes__LastM__4262CC11");

            entity.HasOne(d => d.Project).WithMany(p => p.ItemReqestTable).HasConstraintName("FK__ItemReqes__Proje__6DB73E6A");

            entity.HasOne(d => d.Status).WithMany(p => p.ItemReqestTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemReqes__Statu__6EAB62A3");

            entity.HasOne(d => d.Supplier).WithMany(p => p.ItemReqestTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemReqes__Suppl__6BCEF5F8");
        });

        modelBuilder.Entity<ItemSupplierMappingTable>(entity =>
        {
            entity.HasKey(e => e.ItemSupplierMappingID).HasName("PK__ItemSupp__895E6B1CC6688BB8");

            entity.HasOne(d => d.ItemType).WithMany(p => p.ItemSupplierMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemSuppl__ItemT__4F32B74A");

            entity.HasOne(d => d.Party).WithMany(p => p.ItemSupplierMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemSuppl__Party__5026DB83");

            entity.HasOne(d => d.Product).WithMany(p => p.ItemSupplierMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemSuppl__Produ__4E3E9311");
        });

        modelBuilder.Entity<ItemTypeTable>(entity =>
        {
            entity.HasKey(e => e.ItemTypeID).HasName("PK__ItemType__F51540DB60C1C09D");
        });

        modelBuilder.Entity<LanguageContentLineItemTable>(entity =>
        {
            entity.HasKey(e => e.LanguageContentLineItemID).HasName("PK__Language__124475A12B8E5F93");

            entity.HasOne(d => d.LanguageContentNavigation).WithMany(p => p.LanguageContentLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LanguageContentLineItemTable_LanguageContentTable");

            entity.HasOne(d => d.Language).WithMany(p => p.LanguageContentLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LanguageContentLineItemTable_LanguageTable");
        });

        modelBuilder.Entity<LanguageContentTable>(entity =>
        {
            entity.HasKey(e => e.LanguageContentID).HasName("PK__Language__31210D9064F8FA2E");

            entity.Property(e => e.StatusID).HasDefaultValue(5);

            entity.HasOne(d => d.Status).WithMany(p => p.LanguageContentTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LanguageContentTable_StatusTable");
        });

        modelBuilder.Entity<LanguageTable>(entity =>
        {
            entity.HasKey(e => e.LanguageID).HasName("PK__Language__B938558BB03361DE");

            entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LanguageTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LanguageT__Creat__74EE4BDE");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.LanguageTableLastModifiedByNavigation).HasConstraintName("FK__LanguageT__LastM__75E27017");
        });

        modelBuilder.Entity<LocationDescriptionTable>(entity =>
        {
            entity.HasKey(e => e.LocationDescriptionID).HasName("PK__Location__FC5F4641C7B873F4");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LocationDescriptionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LocationD__Creat__4356F04A");

            entity.HasOne(d => d.Language).WithMany(p => p.LocationDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LocationD__Langu__047AA831");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.LocationDescriptionTableLastModifiedByNavigation).HasConstraintName("FK__LocationD__LastM__444B1483");

            entity.HasOne(d => d.Location).WithMany(p => p.LocationDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LocationD__Locat__038683F8");
        });

        modelBuilder.Entity<LocationForUserMappingView>(entity =>
        {
            entity.ToView("LocationForUserMappingView");
        });

        modelBuilder.Entity<LocationNewHierarchicalView>(entity =>
        {
            entity.ToView("LocationNewHierarchicalView");
        });

        modelBuilder.Entity<LocationNewView>(entity =>
        {
            entity.ToView("LocationNewView");
        });

        modelBuilder.Entity<LocationTable>(entity =>
        {
            entity.HasKey(e => e.LocationID).HasName("PK__Location__E7FEA4779FCF224D");

            entity.Property(e => e.IsWIPLocation).HasDefaultValue(false);

            entity.HasOne(d => d.BudgetYear).WithMany(p => p.LocationTable).HasConstraintName("FK__LocationT__Budge__7EC1CEDB");

            entity.HasOne(d => d.Company).WithMany(p => p.LocationTable).HasConstraintName("FK__LocationT__Compa__00AA174D");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LocationTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LocationT__Creat__453F38BC");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.LocationTableLastModifiedByNavigation).HasConstraintName("FK__LocationT__LastM__46335CF5");

            entity.HasOne(d => d.LocationType).WithMany(p => p.LocationTable).HasConstraintName("FK__LocationT__Locat__170E59B8");

            entity.HasOne(d => d.ParentLocation).WithMany(p => p.InverseParentLocation).HasConstraintName("FK__LocationT__Paren__7AF13DF7");

            entity.HasOne(d => d.Status).WithMany(p => p.LocationTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LocationT__Statu__7BE56230");
        });

        modelBuilder.Entity<LocationTypeTable>(entity =>
        {
            entity.HasKey(e => e.LocationTypeID).HasName("PK__Location__737D32D990E6F152");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LocationTypeTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LocationTypeTable_CreatedBy");

            entity.HasOne(d => d.Status).WithMany(p => p.LocationTypeTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LocationTypeTable_StatusID");
        });

        modelBuilder.Entity<ManufacturerDescriptionTable>(entity =>
        {
            entity.HasKey(e => e.ManufacturerDescriptionID).HasName("PK__Manufact__A097BC962DB0D2BB");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ManufacturerDescriptionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Manufactu__Creat__0B879873");

            entity.HasOne(d => d.Language).WithMany(p => p.ManufacturerDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Manufactu__Langu__0A93743A");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.ManufacturerDescriptionTableLastModifiedByNavigation).HasConstraintName("FK__Manufactu__LastM__0C7BBCAC");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.ManufacturerDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Manufactu__Manuf__099F5001");
        });

        modelBuilder.Entity<ManufacturerTable>(entity =>
        {
            entity.HasKey(e => e.ManufacturerID).HasName("PK__Manufact__357E5CA1544BB743");

            entity.HasOne(d => d.Category).WithMany(p => p.ManufacturerTable).HasConstraintName("FK__Manufactu__Categ__03E676AB");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ManufacturerTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ManufacturerTable_CreatedBy");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.ManufacturerTableLastModifiedByNavigation).HasConstraintName("FK_ManufacturerTable_LastModifiedBy");

            entity.HasOne(d => d.Status).WithMany(p => p.ManufacturerTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Manufactu__Statu__04DA9AE4");
        });

        modelBuilder.Entity<MasterGridNewLineItemTable>(entity =>
        {
            entity.HasKey(e => e.MasterGridLineItemID).HasName("PK__MasterGr__53B282313DFC8665");

            entity.Property(e => e.IsDefault).HasDefaultValue(true);

            entity.HasOne(d => d.MasterGrid).WithMany(p => p.MasterGridNewLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MasterGridNewLineItemTable_MasterGridNewTable");
        });

        modelBuilder.Entity<MasterGridNewTable>(entity =>
        {
            entity.HasKey(e => e.MasterGridID).HasName("PK__MasterGr__A73B267A2CA8E9D6");
        });

        modelBuilder.Entity<ModelDescriptionTable>(entity =>
        {
            entity.HasKey(e => e.ModelDescriptionID).HasName("PK__ModelDes__BEC76454931A45A5");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ModelDescriptionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ModelDesc__Creat__4727812E");

            entity.HasOne(d => d.Language).WithMany(p => p.ModelDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ModelDesc__Langu__4277DAAA");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.ModelDescriptionTableLastModifiedByNavigation).HasConstraintName("FK__ModelDesc__LastM__481BA567");

            entity.HasOne(d => d.Model).WithMany(p => p.ModelDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ModelDesc__Model__4183B671");
        });

        modelBuilder.Entity<ModelTable>(entity =>
        {
            entity.HasKey(e => e.ModelID).HasName("PK__ModelTab__E8D7A1CC29F20188");

            entity.HasOne(d => d.Category).WithMany(p => p.ModelTable).HasConstraintName("FK__ModelTabl__Categ__3BCADD1B");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ModelTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ModelTabl__Creat__490FC9A0");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.ModelTableLastModifiedByNavigation).HasConstraintName("FK__ModelTabl__LastM__4A03EDD9");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.ModelTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ModelTabl__Manuf__6ADAD1BF");

            entity.HasOne(d => d.Status).WithMany(p => p.ModelTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ModelTabl__Statu__3CBF0154");
        });

        modelBuilder.Entity<NotificationFieldTable>(entity =>
        {
            entity.HasKey(e => e.NotificationFieldID).HasName("PK__Notifica__FC3477F8955AC3C3");

            entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.StatusID).HasDefaultValue(5);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.NotificationFieldTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__Creat__76D69450");

            entity.HasOne(d => d.NotificationModule).WithMany(p => p.NotificationFieldTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotificationFieldTable_NotificationModuleID");

            entity.HasOne(d => d.Status).WithMany(p => p.NotificationFieldTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotificationFieldTable_StatusTable");
        });

        modelBuilder.Entity<NotificationInputTable>(entity =>
        {
            entity.HasKey(e => e.NotificationInputID).HasName("PK__Notifica__6C6CC2895A7C1895");

            entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.NotificationTemplate).WithMany(p => p.NotificationInputTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__Notif__321755AF");

            entity.HasOne(d => d.SYSUser).WithMany(p => p.NotificationInputTable).HasConstraintName("FK__Notificat__SYSUs__47677850");
        });

        modelBuilder.Entity<NotificationModuleFieldTable>(entity =>
        {
            entity.HasKey(e => e.NotificationFieldID).HasName("PK__Notifica__FC3477F8D643D164");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.NotificationModuleFieldTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__Creat__76F68FE1");

            entity.HasOne(d => d.NotificationModule).WithMany(p => p.NotificationModuleFieldTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__Notif__750E476F");

            entity.HasOne(d => d.Status).WithMany(p => p.NotificationModuleFieldTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__Statu__76026BA8");
        });

        modelBuilder.Entity<NotificationModuleTable>(entity =>
        {
            entity.HasKey(e => e.NotificationModuleID).HasName("PK__Notifica__CD0C21759AFB9857");

            entity.Property(e => e.StatusID).HasDefaultValue(5);

            entity.HasOne(d => d.ReportTemplate).WithMany(p => p.NotificationModuleTable).HasConstraintName("FK__Notificat__Repor__78DED853");

            entity.HasOne(d => d.Status).WithMany(p => p.NotificationModuleTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotificationModuleTable_StatusTable");
        });

        modelBuilder.Entity<NotificationReportAttachmentTable>(entity =>
        {
            entity.HasKey(e => e.NotificationReportAttachmentID).HasName("PK__Notifica__C9FEC663BFF70D14");

            entity.HasOne(d => d.AttachmentFormat).WithMany(p => p.NotificationReportAttachmentTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__Attac__6B84DD35");

            entity.HasOne(d => d.NotificationTemplate).WithMany(p => p.NotificationReportAttachmentTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__Notif__6A90B8FC");

            entity.HasOne(d => d.Report).WithMany(p => p.NotificationReportAttachmentTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__Repor__6C79016E");
        });

        modelBuilder.Entity<NotificationTemplateFieldTable>(entity =>
        {
            entity.HasKey(e => e.NotificationTemplateFieldID).HasName("PK__Notifica__229CF50E6466F90E");

            entity.HasOne(d => d.NotificationField).WithMany(p => p.NotificationTemplateFieldTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__Notif__38C4533E");

            entity.HasOne(d => d.NotificationTemplate).WithMany(p => p.NotificationTemplateFieldTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__Notif__37D02F05");

            entity.HasOne(d => d.Status).WithMany(p => p.NotificationTemplateFieldTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__Statu__39B87777");
        });

        modelBuilder.Entity<NotificationTemplateNotificationTypeTable>(entity =>
        {
            entity.HasKey(e => e.NotificationTemplateNotificationTypeID).HasName("PK__Notifica__5B055679E68F0C6D");

            entity.HasOne(d => d.NotificationTemplate).WithMany(p => p.NotificationTemplateNotificationTypeTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__Notif__6F556E19");

            entity.HasOne(d => d.NotificationType).WithMany(p => p.NotificationTemplateNotificationTypeTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__Notif__70499252");

            entity.HasOne(d => d.Status).WithMany(p => p.NotificationTemplateNotificationTypeTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__Statu__713DB68B");
        });

        modelBuilder.Entity<NotificationTemplateTable>(entity =>
        {
            entity.HasKey(e => e.NotificationTemplateID).HasName("PK__Notifica__9C914C2BE6481ED4");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.NotificationTemplateTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__Creat__66C02818");

            entity.HasOne(d => d.EmailSignature).WithMany(p => p.NotificationTemplateTable).HasConstraintName("FK__Notificat__Email__64D7DFA6");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.NotificationTemplateTableLastModifiedByNavigation).HasConstraintName("FK__Notificat__LastM__67B44C51");

            entity.HasOne(d => d.NotificationModule).WithMany(p => p.NotificationTemplateTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotificationTemplateTable_NotificationModuleID");

            entity.HasOne(d => d.Status).WithMany(p => p.NotificationTemplateTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__Statu__65CC03DF");
        });

        modelBuilder.Entity<NotificationTypeTable>(entity =>
        {
            entity.HasKey(e => e.NotificationTypeID).HasName("PK__Notifica__299002A124E2F71A");

            entity.Property(e => e.NotificationTypeID).ValueGeneratedNever();
            entity.Property(e => e.AllowHTMLContent).HasDefaultValue(false);
            entity.Property(e => e.IsAttachmentRequired).HasDefaultValue(false);
        });

        modelBuilder.Entity<PartyDescriptionTable>(entity =>
        {
            entity.HasKey(e => e.PartyDescriptionID).HasName("PK__PartyDes__66A62E4CB7220BEF");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PartyDescriptionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PartyDesc__Creat__4AF81212");

            entity.HasOne(d => d.Language).WithMany(p => p.PartyDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PartyDesc__Langu__1D4655FB");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.PartyDescriptionTableLastModifiedByNavigation).HasConstraintName("FK__PartyDesc__LastM__4BEC364B");

            entity.HasOne(d => d.Party).WithMany(p => p.PartyDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PartyDesc__Party__1C5231C2");
        });

        modelBuilder.Entity<PartyTable>(entity =>
        {
            entity.HasKey(e => e.PartyID).HasName("PK__PartyTab__1640CD1340FC2167");

            entity.HasOne(d => d.Country).WithMany(p => p.PartyTable).HasConstraintName("FK__PartyTabl__Count__1699586C");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PartyTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PartyTabl__Creat__4CE05A84");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.PartyTableLastModifiedByNavigation).HasConstraintName("FK__PartyTabl__LastM__4DD47EBD");

            entity.HasOne(d => d.PartyType).WithMany(p => p.PartyTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PartyTabl__Party__15A53433");

            entity.HasOne(d => d.Status).WithMany(p => p.PartyTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PartyTabl__Statu__178D7CA5");
        });

        modelBuilder.Entity<PartyTypeTable>(entity =>
        {
            entity.HasKey(e => e.PartyTypeID).HasName("PK__PartyTyp__02C10250A8CBE2AF");

            entity.Property(e => e.PartyTypeID).ValueGeneratedNever();
        });

        modelBuilder.Entity<PaymentTypeTable>(entity =>
        {
            entity.HasKey(e => e.PaymentTypeID).HasName("PK__PaymentT__BA430B15FAF28F55");
        });

        modelBuilder.Entity<PeriodTable>(entity =>
        {
            entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.StatusID).HasDefaultValue(1);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PeriodTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PeriodTable_CreatedBy");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.PeriodTableLastModifiedByNavigation).HasConstraintName("FK_PeriodTable_LastModifiedBy");

            entity.HasOne(d => d.Status).WithMany(p => p.PeriodTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PeriodTable_StatusID");
        });

        modelBuilder.Entity<PersonTable>(entity =>
        {
            entity.HasKey(e => e.PersonID).HasName("PK__PersonTa__AA2FFB8595E38645");

            entity.Property(e => e.PersonID).ValueGeneratedNever();
            entity.Property(e => e.Gender)
                .HasDefaultValue("M")
                .IsFixedLength();
            entity.Property(e => e.StatusID).HasDefaultValue(5);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PersonTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PersonTab__Creat__31783731");

            entity.HasOne(d => d.Department).WithMany(p => p.PersonTable).HasConstraintName("FK__PersonTab__Depar__424DBD78");

            entity.HasOne(d => d.Designation).WithMany(p => p.PersonTable).HasConstraintName("FK__PersonTab__Desig__1E105D02");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.PersonTableLastModifiedByNavigation).HasConstraintName("FK__PersonTab__LastM__326C5B6A");

            entity.HasOne(d => d.Status).WithMany(p => p.PersonTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonTable_StatusID");

            entity.HasOne(d => d.UserType).WithMany(p => p.PersonTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonTable_UserTypeID");
        });

        modelBuilder.Entity<PersonView>(entity =>
        {
            entity.ToView("PersonView");

            entity.Property(e => e.Gender).IsFixedLength();
        });

        modelBuilder.Entity<PostingStatusTable>(entity =>
        {
            entity.HasKey(e => e.PostingStatusID).HasName("PK__PostingS__51FF37A856C83145");

            entity.Property(e => e.PostingStatusID).ValueGeneratedNever();
        });

        modelBuilder.Entity<PriceAnalysisLineItemTable>(entity =>
        {
            entity.HasKey(e => e.PriceAnalysisLineItemID).HasName("PK__PriceAna__5A754F332485FA5B");

            entity.HasOne(d => d.PriceAnalysis).WithMany(p => p.PriceAnalysisLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PriceAnal__Price__222B06A9");

            entity.HasOne(d => d.Product).WithMany(p => p.PriceAnalysisLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PriceAnal__Produ__231F2AE2");
        });

        modelBuilder.Entity<PriceAnalysisTable>(entity =>
        {
            entity.HasKey(e => e.PriceAnalysisID).HasName("PK__PriceAna__A83CBD7000C94F84");

            entity.Property(e => e.isPurchaseContact).HasDefaultValue(false);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PriceAnalysisTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PriceAnal__Creat__4EC8A2F6");

            entity.HasOne(d => d.DeliveredWarehouse).WithMany(p => p.PriceAnalysisTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PriceAnal__Deliv__17AD7836");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.PriceAnalysisTableLastModifiedByNavigation).HasConstraintName("FK__PriceAnal__LastM__4FBCC72F");

            entity.HasOne(d => d.Status).WithMany(p => p.PriceAnalysisTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PriceAnal__Statu__18A19C6F");
        });

        modelBuilder.Entity<ProductDescriptionTable>(entity =>
        {
            entity.HasKey(e => e.ProductDescriptionID).HasName("PK__ProductD__C0134939FFA09823");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProductDescriptionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductDe__Creat__50B0EB68");

            entity.HasOne(d => d.Language).WithMany(p => p.ProductDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductDe__Langu__361203C5");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.ProductDescriptionTableLastModifiedByNavigation).HasConstraintName("FK__ProductDe__LastM__51A50FA1");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductDe__Produ__351DDF8C");
        });

        modelBuilder.Entity<ProductTable>(entity =>
        {
            entity.HasKey(e => e.ProductID).HasName("PK__ProductT__B40CC6EDB5248CBC");

            entity.Property(e => e.IsInventoryItem).HasDefaultValue(false);
            entity.Property(e => e.IsSerialNumberRequired).HasDefaultValue(false);

            entity.HasOne(d => d.Category).WithMany(p => p.ProductTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductTa__Categ__2C88998B");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProductTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductTa__Creat__529933DA");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.ProductTableLastModifiedByNavigation).HasConstraintName("FK__ProductTa__LastM__538D5813");

            entity.HasOne(d => d.Status).WithMany(p => p.ProductTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductTa__Statu__30592A6F");

            entity.HasOne(d => d.UOM).WithMany(p => p.ProductTable).HasConstraintName("FK__ProductTa__UOMID__2F650636");
        });

        modelBuilder.Entity<ProductUOMMappingTable>(entity =>
        {
            entity.HasKey(e => e.ProductUOMMappingID).HasName("PK__ProductU__BEE2C89E52CB81E6");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProductUOMMappingTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductUO__Creat__79B300FB");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.ProductUOMMappingTableLastModifiedByNavigation).HasConstraintName("FK__ProductUO__LastM__7AA72534");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductUOMMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductUOM_ProductID");

            entity.HasOne(d => d.Status).WithMany(p => p.ProductUOMMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductUOM_StatusID");

            entity.HasOne(d => d.UOM).WithMany(p => p.ProductUOMMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductUOM_UOMID");
        });

        modelBuilder.Entity<ProjectDescriptionTable>(entity =>
        {
            entity.HasKey(e => e.ProjectDescriptionID).HasName("PK__ProjectD__3059C0E3B8376FDE");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectDescriptionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProjectDe__Creat__54817C4C");

            entity.HasOne(d => d.Language).WithMany(p => p.ProjectDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProjectDe__Langu__375B2DB9");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.ProjectDescriptionTableLastModifiedByNavigation).HasConstraintName("FK__ProjectDe__LastM__5575A085");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProjectDe__Proje__36670980");
        });

        modelBuilder.Entity<ProjectTable>(entity =>
        {
            entity.HasKey(e => e.ProjectID).HasName("PK__ProjectT__761ABED01A99F3C3");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProjectTa__Creat__5669C4BE");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.ProjectTableLastModifiedByNavigation).HasConstraintName("FK__ProjectTa__LastM__575DE8F7");

            entity.HasOne(d => d.Status).WithMany(p => p.ProjectTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProjectTa__Statu__31A25463");
        });

        modelBuilder.Entity<PurchaseOrderLineItemTable>(entity =>
        {
            entity.HasKey(e => e.PurchaseOrderLineItemID).HasName("PK__Purchase__CA1B2E277A2A1156");

            entity.HasOne(d => d.Product).WithMany(p => p.PurchaseOrderLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PurchaseO__Produ__40AF8DC9");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.PurchaseOrderLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PurchaseO__Purch__3FBB6990");

            entity.HasOne(d => d.VAT).WithMany(p => p.PurchaseOrderLineItemTable).HasConstraintName("FK__PurchaseO__VATID__41A3B202");
        });

        modelBuilder.Entity<PurchaseOrderTable>(entity =>
        {
            entity.HasKey(e => e.PurchaseOrderID).HasName("PK__Purchase__036BAC446607C9F1");

            entity.HasOne(d => d.Company).WithMany(p => p.PurchaseOrderTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PurchaseO__Compa__3A02903A");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PurchaseOrderTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PurchaseO__Creat__58520D30");

            entity.HasOne(d => d.Department).WithMany(p => p.PurchaseOrderTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PurchaseO__Depar__4297D63B");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.PurchaseOrderTableLastModifiedByNavigation).HasConstraintName("FK__PurchaseO__LastM__59463169");

            entity.HasOne(d => d.PurchaseType).WithMany(p => p.PurchaseOrderTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PurchaseO__Purch__390E6C01");

            entity.HasOne(d => d.Status).WithMany(p => p.PurchaseOrderTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PurchaseO__Statu__3AF6B473");
        });

        modelBuilder.Entity<PurchaseTypeTable>(entity =>
        {
            entity.HasKey(e => e.PurchaseTypeID).HasName("PK__Purchase__774F588D4F820227");
        });

        modelBuilder.Entity<ReasonTypeTable>(entity =>
        {
            entity.HasKey(e => e.ReasonTypeID).HasName("PK__ReasonTy__B87FDA26E6816EB5");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ReasonTypeTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReasonTyp__Creat__257C74A0");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.ReasonTypeTableLastModifiedByNavigation).HasConstraintName("FK__ReasonTyp__LastM__267098D9");

            entity.HasOne(d => d.Status).WithMany(p => p.ReasonTypeTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReasonTyp__Statu__24885067");
        });

        modelBuilder.Entity<ReportFieldTable>(entity =>
        {
            entity.HasKey(e => e.ReportFieldID).HasName("PK__ReportFi__3045082FCF4F28C7");

            entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.FieldWidth).HasDefaultValue(1.0m);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ReportFieldTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReportFie__Creat__7B9B496D");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.ReportFieldTableLastModifiedByNavigation).HasConstraintName("FK__ReportFie__LastM__7C8F6DA6");

            entity.HasOne(d => d.Report).WithMany(p => p.ReportFieldTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReportFie__Repor__6EC0713C");

            entity.HasOne(d => d.ReportTemplateField).WithMany(p => p.ReportFieldTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportFieldTable_ReportTemplateFieldID");

            entity.HasOne(d => d.Status).WithMany(p => p.ReportFieldTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportFieldTable_StatusID");
        });

        modelBuilder.Entity<ReportGroupFieldTable>(entity =>
        {
            entity.HasKey(e => e.ReportGroupFieldID).HasName("PK__ReportGr__84E7D31530DA179C");

            entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.FieldWidth).HasDefaultValue(1.0m);
            entity.Property(e => e.GroupLevel).HasDefaultValue((byte)1);
            entity.Property(e => e.StatusID).HasDefaultValue(5);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ReportGroupFieldTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReportGro__Creat__7D8391DF");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.ReportGroupFieldTableLastModifiedByNavigation).HasConstraintName("FK__ReportGro__LastM__7E77B618");

            entity.HasOne(d => d.Report).WithMany(p => p.ReportGroupFieldTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReportGro__Repor__73852659");

            entity.HasOne(d => d.ReportTemplateField).WithMany(p => p.ReportGroupFieldTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportGroupFieldTable_ReportTemplateFieldID");

            entity.HasOne(d => d.Status).WithMany(p => p.ReportGroupFieldTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportGroupFieldTable_StatusID");
        });

        modelBuilder.Entity<ReportPaperSizeTable>(entity =>
        {
            entity.HasKey(e => e.ReportPaperSizeID).HasName("PK__ReportPa__D006E9655DC4AA2A");
        });

        modelBuilder.Entity<ReportTable>(entity =>
        {
            entity.HasKey(e => e.ReportID).HasName("PK__ReportTa__D5BD48E5A0731216");

            entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ReportBottomMargin).HasDefaultValue(0.5m);
            entity.Property(e => e.ReportLeftMargin).HasDefaultValue(0.5m);
            entity.Property(e => e.ReportPageHeight).HasDefaultValue(11.69m);
            entity.Property(e => e.ReportPageWidth).HasDefaultValue(8.27m);
            entity.Property(e => e.ReportRightMargin).HasDefaultValue(0.5m);
            entity.Property(e => e.ReportTopMargin).HasDefaultValue(0.5m);
            entity.Property(e => e.StatusID).HasDefaultValue(5);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ReportTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReportTab__Creat__7F6BDA51");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.ReportTableLastModifiedByNavigation).HasConstraintName("FK__ReportTab__LastM__005FFE8A");

            entity.HasOne(d => d.ReportPaperSize).WithMany(p => p.ReportTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportTable_ReportPaperSizeID");

            entity.HasOne(d => d.ReportTemplate).WithMany(p => p.ReportTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportTable_ReportTemplateID");

            entity.HasOne(d => d.Status).WithMany(p => p.ReportTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportTable_StatusID");
        });

        modelBuilder.Entity<ReportTemplateCategoryTable>(entity =>
        {
            entity.HasKey(e => e.ReportTemplateCategoryID).HasName("PK__ReportTe__1B7181155FD54D11");

            entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.StatusID).HasDefaultValue(5);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ReportTemplateCategoryTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReportTem__Creat__015422C3");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.ReportTemplateCategoryTableLastModifiedByNavigation).HasConstraintName("FK__ReportTem__LastM__024846FC");

            entity.HasOne(d => d.Status).WithMany(p => p.ReportTemplateCategoryTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportTemplateCategoryTable_StatusID");
        });

        modelBuilder.Entity<ReportTemplateFieldTable>(entity =>
        {
            entity.HasKey(e => e.ReportTemplateFieldID).HasName("PK__ReportTe__9F8A59AC134B4F0B");

            entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DefaultWidth).HasDefaultValue(1.0m);
            entity.Property(e => e.FieldDataType).HasDefaultValueSql("((3))");
            entity.Property(e => e.StatusID).HasDefaultValue(5);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ReportTemplateFieldTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReportTem__Creat__033C6B35");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.ReportTemplateFieldTableLastModifiedByNavigation).HasConstraintName("FK__ReportTem__LastM__04308F6E");

            entity.HasOne(d => d.ReportTemplate).WithMany(p => p.ReportTemplateFieldTable).HasConstraintName("FK_ReportTemplateFieldTable_ReportTemplateID");

            entity.HasOne(d => d.Status).WithMany(p => p.ReportTemplateFieldTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportTemplateFieldTable_StatusID");
        });

        modelBuilder.Entity<ReportTemplateFileTable>(entity =>
        {
            entity.HasKey(e => e.ReportTemplateFileID).HasName("PK__ReportTe__D4511CDCD5C5D80E");

            entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.StatusID).HasDefaultValue(5);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ReportTemplateFileTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReportTem__Creat__0524B3A7");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.ReportTemplateFileTableLastModifiedByNavigation).HasConstraintName("FK__ReportTem__LastM__0618D7E0");

            entity.HasOne(d => d.Status).WithMany(p => p.ReportTemplateFileTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportTemplateFileTable_StatusID");
        });

        modelBuilder.Entity<ReportTemplateTable>(entity =>
        {
            entity.HasKey(e => e.ReportTemplateID).HasName("PK__ReportTe__C7EA2866C35103F2");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("trg_Ins_ReportTemplateTable");
                    tb.HasTrigger("trg_Upd_ReportTemplateTable");
                });

            entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.QueryType).HasDefaultValue("View");
            entity.Property(e => e.StatusID).HasDefaultValue(5);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ReportTemplateTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReportTem__Creat__070CFC19");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.ReportTemplateTableLastModifiedByNavigation).HasConstraintName("FK__ReportTem__LastM__08012052");

            entity.HasOne(d => d.ReportTemplateCategory).WithMany(p => p.ReportTemplateTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportTemplateTable_ReportTemplateCategoryID");

            entity.HasOne(d => d.Right).WithMany(p => p.ReportTemplateTable).HasConstraintName("FK__ReportTem__Right__0697FACD");

            entity.HasOne(d => d.Status).WithMany(p => p.ReportTemplateTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportTemplateTable_StatusID");
        });

        modelBuilder.Entity<RequestQuotationLineItemTable>(entity =>
        {
            entity.HasKey(e => e.RequestQuotationLineItemID).HasName("PK__RequestQ__AEC45ACE681B1F6D");

            entity.HasOne(d => d.Product).WithMany(p => p.RequestQuotationLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RequestQu__Produ__03A67F89");

            entity.HasOne(d => d.RequestQuotation).WithMany(p => p.RequestQuotationLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RequestQu__Reque__02B25B50");
        });

        modelBuilder.Entity<RequestQuotationTable>(entity =>
        {
            entity.HasKey(e => e.RequestQuotationID).HasName("PK__RequestQ__EB5824EAE8271560");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.RequestQuotationTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RequestQu__Creat__5A3A55A2");

            entity.HasOne(d => d.Currency).WithMany(p => p.RequestQuotationTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RequestQu__Curre__7C055DC1");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.RequestQuotationTableLastModifiedByNavigation).HasConstraintName("FK__RequestQu__LastM__5B2E79DB");

            entity.HasOne(d => d.Status).WithMany(p => p.RequestQuotationTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RequestQu__Statu__7DEDA633");

            entity.HasOne(d => d.Supplier).WithMany(p => p.RequestQuotationTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RequestQu__Suppl__7CF981FA");
        });

        modelBuilder.Entity<RetirementTypeDescriptionTable>(entity =>
        {
            entity.HasKey(e => e.RetirementTypeDescriptionID).HasName("PK__Retireme__FB6A66F8B77AD378");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.RetirementTypeDescriptionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Retiremen__Creat__1451E89E");

            entity.HasOne(d => d.Language).WithMany(p => p.RetirementTypeDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Retiremen__Langu__135DC465");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.RetirementTypeDescriptionTableLastModifiedByNavigation).HasConstraintName("FK__Retiremen__LastM__15460CD7");

            entity.HasOne(d => d.RetirementType).WithMany(p => p.RetirementTypeDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Retiremen__Retir__1269A02C");
        });

        modelBuilder.Entity<RetirementTypeTable>(entity =>
        {
            entity.HasKey(e => e.RetirementTypeID).HasName("PK__Retireme__1ABAAB9EA4E614CF");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.RetirementTypeTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Retiremen__Creat__0E990F48");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.RetirementTypeTableLastModifiedByNavigation).HasConstraintName("FK__Retiremen__LastM__0F8D3381");

            entity.HasOne(d => d.Status).WithMany(p => p.RetirementTypeTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Retiremen__Statu__0DA4EB0F");
        });

        modelBuilder.Entity<ScreenFilterLineItemTable>(entity =>
        {
            entity.HasKey(e => e.ScreenFilterLineItemID).HasName("PK__ScreenFi__4FAED6A51AC85A50");

            entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.QueryField).HasDefaultValue("A");
            entity.Property(e => e.ScreenFilterTypeID).HasDefaultValue((byte)1);
            entity.Property(e => e.StatusID).HasDefaultValue(5);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ScreenFilterLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScreenFilterLineItemTable_CreatedBy");

            entity.HasOne(d => d.FieldType).WithMany(p => p.ScreenFilterLineItemTable).HasConstraintName("FK_ScreenFilterLineItemTable_FieldTypeID");

            entity.HasOne(d => d.ScreenFilter).WithMany(p => p.ScreenFilterLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScreenFilterLineItemTable_ScreenFilterID");

            entity.HasOne(d => d.ScreenFilterType).WithMany(p => p.ScreenFilterLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScreenFilterLineItemTable_ScreenFilterTypeID");

            entity.HasOne(d => d.Status).WithMany(p => p.ScreenFilterLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScreenFilterLineItemTable_StatusID");
        });

        modelBuilder.Entity<ScreenFilterTable>(entity =>
        {
            entity.HasKey(e => e.ScreenFilterID).HasName("PK__ScreenFi__47183AC7F2AF0847");

            entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.StatusID).HasDefaultValue(5);

            entity.HasOne(d => d.ReportTemplate).WithMany(p => p.ScreenFilterTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ScreenFil__Repor__79D2FC8C");

            entity.HasOne(d => d.Status).WithMany(p => p.ScreenFilterTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScreenFilterTable_StatusID");
        });

        modelBuilder.Entity<ScreenFilterTypeTable>(entity =>
        {
            entity.HasKey(e => e.ScreenFilterTypeID).HasName("PK__ScreenFi__C2ED2B0AB859ED8C");
        });

        modelBuilder.Entity<SecondLevelCategoryTable>(entity =>
        {
            entity.ToView("SecondLevelCategoryTable");
        });

        modelBuilder.Entity<SectionDescriptionTable>(entity =>
        {
            entity.HasKey(e => e.SectionDescriptionID).HasName("PK__SectionD__8773AA74A125FFBB");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SectionDescriptionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SectionDe__Creat__5C229E14");

            entity.HasOne(d => d.Language).WithMany(p => p.SectionDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SectionDe__Langu__05F8DC4F");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.SectionDescriptionTableLastModifiedByNavigation).HasConstraintName("FK__SectionDe__LastM__5D16C24D");

            entity.HasOne(d => d.Section).WithMany(p => p.SectionDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SectionDe__Secti__0504B816");
        });

        modelBuilder.Entity<SectionTable>(entity =>
        {
            entity.HasKey(e => e.SectionID).HasName("PK__SectionT__80EF0892ABB47F5B");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SectionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SectionTa__Creat__5E0AE686");

            entity.HasOne(d => d.Department).WithMany(p => p.SectionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SectionTa__Depar__7F4BDEC0");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.SectionTableLastModifiedByNavigation).HasConstraintName("FK__SectionTa__LastM__5EFF0ABF");

            entity.HasOne(d => d.Status).WithMany(p => p.SectionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SectionTa__Statu__004002F9");
        });

        modelBuilder.Entity<StatusTable>(entity =>
        {
            entity.HasKey(e => e.StatusID).HasName("PK__StatusTa__C8EE204387A06241");

            entity.Property(e => e.StatusID).ValueGeneratedNever();
        });

        modelBuilder.Entity<SupplierAccountDetailTable>(entity =>
        {
            entity.HasKey(e => e.SupplierAccountDetailID).HasName("PK__Supplier__98312D03270B557E");

            entity.Property(e => e.IsDefaultAccount).HasDefaultValue(false);

            entity.HasOne(d => d.AccountCurrency).WithMany(p => p.SupplierAccountDetailTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SupplierA__Accou__47919582");

            entity.HasOne(d => d.Country).WithMany(p => p.SupplierAccountDetailTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SupplierA__Count__4979DDF4");

            entity.HasOne(d => d.Party).WithMany(p => p.SupplierAccountDetailTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SupplierA__Party__45A94D10");

            entity.HasOne(d => d.PaymentType).WithMany(p => p.SupplierAccountDetailTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SupplierA__Payme__469D7149");
        });

        modelBuilder.Entity<SupplierQuotationLineItemTable>(entity =>
        {
            entity.HasKey(e => e.SupplierQuotationLineItemID).HasName("PK__Supplier__5AB4422C5843F129");

            entity.HasOne(d => d.Product).WithMany(p => p.SupplierQuotationLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SupplierQ__Produ__1E5A75C5");

            entity.HasOne(d => d.SupplierQuotation).WithMany(p => p.SupplierQuotationLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SupplierQ__Suppl__1D66518C");

            entity.HasOne(d => d.VAT).WithMany(p => p.SupplierQuotationLineItemTable).HasConstraintName("FK__SupplierQ__VATID__1F4E99FE");
        });

        modelBuilder.Entity<SupplierQuotationTable>(entity =>
        {
            entity.HasKey(e => e.SupplierQuotationID).HasName("PK__Supplier__C948D75BE7EE4D0A");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SupplierQuotationTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SupplierQ__Creat__5FF32EF8");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.SupplierQuotationTableLastModifiedByNavigation).HasConstraintName("FK__SupplierQ__LastM__60E75331");

            entity.HasOne(d => d.RequestQuotation).WithMany(p => p.SupplierQuotationTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SupplierQ__Reque__11007AA7");

            entity.HasOne(d => d.Status).WithMany(p => p.SupplierQuotationTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SupplierQ__Statu__11F49EE0");
        });

        modelBuilder.Entity<TransactionApprovalView>(entity =>
        {
            entity.ToView("TransactionApprovalView");
        });

        modelBuilder.Entity<TransactionLineItemTable>(entity =>
        {
            entity.HasKey(e => e.TransactionLineItemID).HasName("PK__Transact__BFEC1E138374379E");

            entity.Property(e => e.AdjustmentValue).HasDefaultValue(0);
            entity.Property(e => e.Remarks).HasDefaultValue("");

            entity.HasOne(d => d.Asset).WithMany(p => p.TransactionLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Asset__6FBF826D");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TransactionLineItemTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Creat__7D197D8B");

            entity.HasOne(d => d.Custodian).WithMany(p => p.TransactionLineItemTableCustodian).HasConstraintName("FK__Transacti__Custo__7251D655");

            entity.HasOne(d => d.Department).WithMany(p => p.TransactionLineItemTableDepartment).HasConstraintName("FK__Transacti__Depar__7345FA8E");

            entity.HasOne(d => d.FromAssetCondition).WithMany(p => p.TransactionLineItemTableFromAssetCondition).HasConstraintName("FK__Transacti__FromA__752E4300");

            entity.HasOne(d => d.FromCategory).WithMany(p => p.TransactionLineItemTableFromCategory).HasConstraintName("FK__Transacti__FromC__766C7FFC");

            entity.HasOne(d => d.FromCustodian).WithMany(p => p.TransactionLineItemTableFromCustodian).HasConstraintName("FK__Transacti__FromC__7484378A");

            entity.HasOne(d => d.FromDepartment).WithMany(p => p.TransactionLineItemTableFromDepartment).HasConstraintName("FK__Transacti__FromD__729BEF18");

            entity.HasOne(d => d.FromLocation).WithMany(p => p.TransactionLineItemTableFromLocation).HasConstraintName("FK__Transacti__FromL__70B3A6A6");

            entity.HasOne(d => d.FromProduct).WithMany(p => p.TransactionLineItemTableFromProduct).HasConstraintName("FK__Transacti__FromP__7854C86E");

            entity.HasOne(d => d.FromSection).WithMany(p => p.TransactionLineItemTableFromSection).HasConstraintName("FK__Transacti__FromS__7A3D10E0");

            entity.HasOne(d => d.RetirementType).WithMany(p => p.TransactionLineItemTable).HasConstraintName("FK__Transacti__Retir__1A0AC1F4");

            entity.HasOne(d => d.Room).WithMany(p => p.TransactionLineItemTableRoom).HasConstraintName("FK__Transacti__RoomI__19169DBB");

            entity.HasOne(d => d.Section).WithMany(p => p.TransactionLineItemTableSection).HasConstraintName("FK__Transacti__Secti__743A1EC7");

            entity.HasOne(d => d.Status).WithMany(p => p.TransactionLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Statu__7C255952");

            entity.HasOne(d => d.ToAssetCondition).WithMany(p => p.TransactionLineItemTableToAssetCondition).HasConstraintName("FK__Transacti__ToAss__76226739");

            entity.HasOne(d => d.ToCategory).WithMany(p => p.TransactionLineItemTableToCategory).HasConstraintName("FK__Transacti__ToCat__7760A435");

            entity.HasOne(d => d.ToCustodian).WithMany(p => p.TransactionLineItemTableToCustodian).HasConstraintName("FK__Transacti__ToCus__75785BC3");

            entity.HasOne(d => d.ToDepartment).WithMany(p => p.TransactionLineItemTableToDepartment).HasConstraintName("FK__Transacti__ToDep__73901351");

            entity.HasOne(d => d.ToLocation).WithMany(p => p.TransactionLineItemTableToLocation).HasConstraintName("FK__Transacti__ToLoc__71A7CADF");

            entity.HasOne(d => d.ToProduct).WithMany(p => p.TransactionLineItemTableToProduct).HasConstraintName("FK__Transacti__ToPro__7948ECA7");

            entity.HasOne(d => d.ToSection).WithMany(p => p.TransactionLineItemTableToSection).HasConstraintName("FK__Transacti__ToSec__7B313519");

            entity.HasOne(d => d.Transaction).WithMany(p => p.TransactionLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Trans__6ECB5E34");

            entity.HasOne(d => d.TransferType).WithMany(p => p.TransactionLineItemTable).HasConstraintName("FK__Transacti__Trans__1D1C38C9");
        });

        modelBuilder.Entity<TransactionLineItemViewForTransfer>(entity =>
        {
            entity.ToView("TransactionLineItemViewForTransfer");
        });

        modelBuilder.Entity<TransactionScheduleTable>(entity =>
        {
            entity.HasKey(e => e.TransactionScheduleID).HasName("PK__Transact__C1303FE960E6FAE3");

            entity.Property(e => e.ActivityEndDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ActivityStartDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TransactionScheduleTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Creat__6C98FCFF");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.TransactionScheduleTableLastModifiedByNavigation).HasConstraintName("FK__Transacti__LastM__6D8D2138");

            entity.HasOne(d => d.Status).WithMany(p => p.TransactionScheduleTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Statu__6BA4D8C6");

            entity.HasOne(d => d.Transaction).WithMany(p => p.TransactionScheduleTable).HasConstraintName("FK__Transacti__Trans__715DB21C");
        });

        modelBuilder.Entity<TransactionSubTypeTable>(entity =>
        {
            entity.HasKey(e => e.TransactionSubTypeID).HasName("PK__Transact__C7C5CC6122E14EA3");

            entity.Property(e => e.TransactionSubTypeID).ValueGeneratedNever();
        });

        modelBuilder.Entity<TransactionTable>(entity =>
        {
            entity.HasKey(e => e.TransactionID).HasName("PK__Transact__55433A4B98706F0D");

            entity.ToTable(tb => tb.HasTrigger("trg_TransactionTable_Upt"));

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TransactionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Creat__6BEEF189");

            entity.HasOne(d => d.PostedByNavigation).WithMany(p => p.TransactionTablePostedByNavigation).HasConstraintName("FK__Transacti__Poste__6AFACD50");

            entity.HasOne(d => d.PostingStatus).WithMany(p => p.TransactionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Posti__691284DE");

            entity.HasOne(d => d.SourceTransaction).WithMany(p => p.InverseSourceTransaction).HasConstraintName("FK__Transacti__Sourc__672A3C6C");

            entity.HasOne(d => d.SourceTransactionSchedule).WithMany(p => p.TransactionTable).HasConstraintName("FK__Transacti__Sourc__12BEA5E7");

            entity.HasOne(d => d.Status).WithMany(p => p.TransactionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Statu__681E60A5");

            entity.HasOne(d => d.TransactionSubType).WithMany(p => p.TransactionTable).HasConstraintName("FK_Transactiontable_SubTypeID");

            entity.HasOne(d => d.TransactionType).WithMany(p => p.TransactionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Trans__66361833");

            entity.HasOne(d => d.Vendor).WithMany(p => p.TransactionTable).HasConstraintName("FK__Transacti__Vendo__51E506C3");

            entity.HasOne(d => d.VerifiedByNavigation).WithMany(p => p.TransactionTableVerifiedByNavigation).HasConstraintName("FK__Transacti__Verif__6A06A917");
        });

        modelBuilder.Entity<TransactionTypeTable>(entity =>
        {
            entity.HasKey(e => e.TransactionTypeID).HasName("PK__Transact__20266CEBB4745C2A");

            entity.Property(e => e.TransactionTypeID).ValueGeneratedNever();
            entity.Property(e => e.IsSourceTransactionType).HasDefaultValue(true);

            entity.HasOne(d => d.SourceTransactionType).WithMany(p => p.InverseSourceTransactionType).HasConstraintName("FK__Transacti__Sourc__5DA0D232");

            entity.HasOne(d => d.SourceTransactionTypeID2Navigation).WithMany(p => p.InverseSourceTransactionTypeID2Navigation).HasConstraintName("FK__Transacti__Sourc__5E94F66B");

            entity.HasOne(d => d.SourceTransactionTypeID3Navigation).WithMany(p => p.InverseSourceTransactionTypeID3Navigation).HasConstraintName("FK__Transacti__Sourc__5F891AA4");

            entity.HasOne(d => d.SourceTransactionTypeID4Navigation).WithMany(p => p.InverseSourceTransactionTypeID4Navigation).HasConstraintName("FK__Transacti__Sourc__607D3EDD");

            entity.HasOne(d => d.SourceTransactionTypeID5Navigation).WithMany(p => p.InverseSourceTransactionTypeID5Navigation).HasConstraintName("FK__Transacti__Sourc__61716316");
        });

        modelBuilder.Entity<TransactionView>(entity =>
        {
            entity.ToView("TransactionView");
        });

        modelBuilder.Entity<TransferTypeDescriptionTable>(entity =>
        {
            entity.HasKey(e => e.TransferTypeDescriptionID).HasName("PK__Transfer__1CDF6416D69B76B2");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TransferTypeDescriptionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TransferT__Creat__62CF9BA3");

            entity.HasOne(d => d.Language).WithMany(p => p.TransferTypeDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TransferT__Langu__4CF5691D");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.TransferTypeDescriptionTableLastModifiedByNavigation).HasConstraintName("FK__TransferT__LastM__64B7E415");

            entity.HasOne(d => d.TransferType).WithMany(p => p.TransferTypeDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TransferT__Trans__4C0144E4");
        });

        modelBuilder.Entity<TransferTypeTable>(entity =>
        {
            entity.HasKey(e => e.TransferTypeID).HasName("PK__Transfer__FF328DCFBDE9B91F");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TransferTypeTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TransferT__Creat__61DB776A");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.TransferTypeTableLastModifiedByNavigation).HasConstraintName("FK__TransferT__LastM__63C3BFDC");

            entity.HasOne(d => d.Status).WithMany(p => p.TransferTypeTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TransferT__Statu__473C8FC7");
        });

        modelBuilder.Entity<UOMDescriptionTable>(entity =>
        {
            entity.HasKey(e => e.UOMDescriptionID).HasName("PK__UOMDescr__139FC1982B65C042");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.UOMDescriptionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UOMDescri__Creat__65AC084E");

            entity.HasOne(d => d.Language).WithMany(p => p.UOMDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UOMDescri__Langu__27C3E46E");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.UOMDescriptionTableLastModifiedByNavigation).HasConstraintName("FK__UOMDescri__LastM__66A02C87");

            entity.HasOne(d => d.UOM).WithMany(p => p.UOMDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UOMDescri__UOMID__26CFC035");
        });

        modelBuilder.Entity<UOMTable>(entity =>
        {
            entity.HasKey(e => e.UOMID).HasName("PK__UOMTable__9825D9FB8EBA4BAB");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.UOMTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UOMTable__Create__679450C0");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.UOMTableLastModifiedByNavigation).HasConstraintName("FK__UOMTable__LastMo__688874F9");

            entity.HasOne(d => d.Status).WithMany(p => p.UOMTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UOMTable__Status__220B0B18");
        });

        modelBuilder.Entity<UserApprovalRoleMappingTable>(entity =>
        {
            entity.HasKey(e => e.UserApprovalRoleMappingID).HasName("PK__UserAppr__1DC14E1EEF7C9CF4");

            entity.HasOne(d => d.ApprovalRole).WithMany(p => p.UserApprovalRoleMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserAppro__Appro__133DC8D4");

            entity.HasOne(d => d.CategoryType).WithMany(p => p.UserApprovalRoleMappingTable).HasConstraintName("FK__UserAppro__Categ__3FC65688");

            entity.HasOne(d => d.Location).WithMany(p => p.UserApprovalRoleMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserAppro__Locat__1249A49B");

            entity.HasOne(d => d.Status).WithMany(p => p.UserApprovalRoleMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserAppro__Statu__1431ED0D");

            entity.HasOne(d => d.User).WithMany(p => p.UserApprovalRoleMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserAppro__UserI__11558062");
        });

        modelBuilder.Entity<UserApprovalRoleMappingView>(entity =>
        {
            entity.ToView("UserApprovalRoleMappingView");
        });

        modelBuilder.Entity<UserCategoryMappingTable>(entity =>
        {
            entity.HasKey(e => e.UserCategoryMappingID).HasName("PK__UserCate__9BD9D08513B7FB54");

            entity.HasOne(d => d.Category).WithMany(p => p.UserCategoryMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserCateg__Categ__513AFB4D");

            entity.HasOne(d => d.Status).WithMany(p => p.UserCategoryMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserCateg__Statu__522F1F86");

            entity.HasOne(d => d.User).WithMany(p => p.UserCategoryMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserCateg__UserI__5046D714");
        });

        modelBuilder.Entity<UserCompanyMappingTable>(entity =>
        {
            entity.HasKey(e => e.UserCompanyMappingID).HasName("PK__UserComp__E62A8C47C21DB0F5");

            entity.HasOne(d => d.Company).WithMany(p => p.UserCompanyMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserCompa__Compa__2BF46805");

            entity.HasOne(d => d.Status).WithMany(p => p.UserCompanyMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserCompa__Statu__78F3E6EC");

            entity.HasOne(d => d.User).WithMany(p => p.UserCompanyMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserCompa__UserI__2DDCB077");
        });

        modelBuilder.Entity<UserDepartmentMappingTable>(entity =>
        {
            entity.HasKey(e => e.UserDepartmentMappingID).HasName("PK__UserDepa__8D15F3026B90F7FB");

            entity.HasOne(d => d.Department).WithMany(p => p.UserDepartmentMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserDepar__Depar__55FFB06A");

            entity.HasOne(d => d.Status).WithMany(p => p.UserDepartmentMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserDepar__Statu__56F3D4A3");

            entity.HasOne(d => d.User).WithMany(p => p.UserDepartmentMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserDepar__UserI__550B8C31");
        });

        modelBuilder.Entity<UserGridNewColumnTable>(entity =>
        {
            entity.HasKey(e => e.UserGridColumnID).HasName("PK__UserGrid__0332B57B52351DB0");

            entity.HasOne(d => d.MasterGrid).WithMany(p => p.UserGridNewColumnTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserGridC1__Maste__42ACE4D4");

            entity.HasOne(d => d.MasterGridLineItem).WithMany(p => p.UserGridNewColumnTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserGridN__Maste__58FC18A6");

            entity.HasOne(d => d.User).WithMany(p => p.UserGridNewColumnTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserGridN__UserI__59F03CDF");
        });

        modelBuilder.Entity<UserLocationMappingTable>(entity =>
        {
            entity.HasKey(e => e.UserLocationMappingID).HasName("PK__UserLoca__DA68405D2D9A2C6A");

            entity.HasOne(d => d.Location).WithMany(p => p.UserLocationMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserLocat__Locat__4C764630");

            entity.HasOne(d => d.Status).WithMany(p => p.UserLocationMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserLocat__Statu__4D6A6A69");

            entity.HasOne(d => d.User).WithMany(p => p.UserLocationMappingTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserLocat__UserI__4B8221F7");
        });

        modelBuilder.Entity<UserReportFilterLineItemTable>(entity =>
        {
            entity.HasKey(e => e.UserReportFilterLineItemID).HasName("PK__UserRepo__A31AD969CA86D456");

            entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.StatusID).HasDefaultValue(5);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.UserReportFilterLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRepor__Creat__08F5448B");

            entity.HasOne(d => d.ScreenFilter).WithMany(p => p.UserReportFilterLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserReportFilterLineItemTable_ScreenFilterID");

            entity.HasOne(d => d.Status).WithMany(p => p.UserReportFilterLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserReportFilterLineItemTable_StatusID");

            entity.HasOne(d => d.UserReportFilter).WithMany(p => p.UserReportFilterLineItemTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserReportFilterLineItemTable_UserReportFilterID");
        });

        modelBuilder.Entity<UserReportFilterTable>(entity =>
        {
            entity.HasKey(e => e.UserReportFilterID).HasName("PK__UserRepo__7268237500384542");

            entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.LastSelected).HasDefaultValue(false);
            entity.Property(e => e.StatusID).HasDefaultValue(5);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.UserReportFilterTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRepor__Creat__09E968C4");

            entity.HasOne(d => d.ReportTemplate).WithMany(p => p.UserReportFilterTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserReportFilterTable_ReportTemplateID");

            entity.HasOne(d => d.Status).WithMany(p => p.UserReportFilterTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserReportFilterTable_StatusID");

            entity.HasOne(d => d.User).WithMany(p => p.UserReportFilterTableUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserReportFilterTable_UserID");
        });

        modelBuilder.Entity<UserRightView>(entity =>
        {
            entity.ToView("UserRightView");
        });

        modelBuilder.Entity<UserTypeTable>(entity =>
        {
            entity.HasKey(e => e.UserTypeID).HasName("PK__UserType__40D2D8F610C74526");

            entity.Property(e => e.UserTypeID).ValueGeneratedNever();

            entity.HasOne(d => d.Status).WithMany(p => p.UserTypeTable).HasConstraintName("FK_UserTypeTable_StatusID");
        });

        modelBuilder.Entity<User_LoginUserTable>(entity =>
        {
            entity.HasKey(e => e.UserID).HasName("PK__User_Log__1788CCAC89946C4E");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.FailedPasswordAttemptCount).HasDefaultValue(3);

            entity.HasOne(d => d.Application).WithMany(p => p.User_LoginUserTable).HasConstraintName("FK_User_LoginUserTable_App_Applications");
        });

        modelBuilder.Entity<User_MenuTable>(entity =>
        {
            entity.HasKey(e => e.MenuID).HasName("PK__User_Men__C99ED250D938EFD5");

            entity.HasOne(d => d.ParentMenu).WithMany(p => p.InverseParentMenu).HasConstraintName("USER_MENU_PARENT_FK");

            entity.HasOne(d => d.Right).WithMany(p => p.User_MenuTable).HasConstraintName("USER_MENU_RIGHT_FK");
        });

        modelBuilder.Entity<User_RightGroupTable>(entity =>
        {
            entity.HasKey(e => e.RightGroupID).HasName("PK__User_Rig__D836E627628DE532");
        });

        modelBuilder.Entity<User_RightTable>(entity =>
        {
            entity.HasKey(e => e.RightID).HasName("PK__User_Rig__465E8876E328C84C");

            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.RightGroup).WithMany(p => p.User_RightTable).HasConstraintName("FK_User_RightTable_User_RightGroupTable");
        });

        modelBuilder.Entity<User_RoleRightTable>(entity =>
        {
            entity.HasKey(e => e.RoleRightID).HasName("PK__User_Rol__FEFE520C0A557D7C");

            entity.HasOne(d => d.Right).WithMany(p => p.User_RoleRightTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("USER_ROLERIGHTTABLE_FK2");

            entity.HasOne(d => d.Role).WithMany(p => p.User_RoleRightTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("USER_ROLERIGHTTABLE_FK");
        });

        modelBuilder.Entity<User_RoleRightView>(entity =>
        {
            entity.ToView("User_RoleRightView");
        });

        modelBuilder.Entity<User_RoleTable>(entity =>
        {
            entity.HasKey(e => e.RoleID).HasName("PK__User_Rol__8AFACE3AA83E96D5");

            entity.Property(e => e.DisplayRole).HasDefaultValue(true);
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Application).WithMany(p => p.User_RoleTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_RoleTable_App_Applications");
        });

        modelBuilder.Entity<User_UserRightTable>(entity =>
        {
            entity.HasKey(e => e.UserRightID).HasName("PK__User_Use__956096421D4EFD7B");

            entity.HasOne(d => d.Right).WithMany(p => p.User_UserRightTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("USER_USERRIGHTTABLE_FK");

            entity.HasOne(d => d.User).WithMany(p => p.User_UserRightTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_UserRightTable_User_LoginUserTable");
        });

        modelBuilder.Entity<User_UserRightView>(entity =>
        {
            entity.ToView("User_UserRightView");
        });

        modelBuilder.Entity<User_UserRoleTable>(entity =>
        {
            entity.HasKey(e => e.UserRoleID).HasName("PK__User_Use__3D978A55790C3B2B");

            entity.HasOne(d => d.Role).WithMany(p => p.User_UserRoleTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("USER_USERROLETABLE_FK3");

            entity.HasOne(d => d.User).WithMany(p => p.User_UserRoleTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_UserRoleTable_User_LoginUserTable");
        });

        modelBuilder.Entity<VATDescriptionTable>(entity =>
        {
            entity.HasKey(e => e.VATDescriptionID).HasName("PK__VATDescr__3EA4D74761D403E7");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.VATDescriptionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VATDescri__Creat__697C9932");

            entity.HasOne(d => d.Language).WithMany(p => p.VATDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VATDescri__Langu__62458BBE");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.VATDescriptionTableLastModifiedByNavigation).HasConstraintName("FK__VATDescri__LastM__6A70BD6B");

            entity.HasOne(d => d.VAT).WithMany(p => p.VATDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VATDescri__VATID__61516785");
        });

        modelBuilder.Entity<VATTable>(entity =>
        {
            entity.HasKey(e => e.VATID).HasName("PK__VATTable__4A9628CE69BD9428");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.VATTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VATTable__Create__6B64E1A4");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.VATTableLastModifiedByNavigation).HasConstraintName("FK__VATTable__LastMo__6C5905DD");

            entity.HasOne(d => d.Status).WithMany(p => p.VATTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VATTable__Status__5C8CB268");

            entity.HasOne(d => d.VATType).WithMany(p => p.VATTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VATTable__VATTyp__5AA469F6");
        });

        modelBuilder.Entity<VATTypeTable>(entity =>
        {
            entity.HasKey(e => e.VATTypeID).HasName("PK__VATTypeT__8C0EDEA9F1406F7D");
        });

        modelBuilder.Entity<WarehouseDescriptionTable>(entity =>
        {
            entity.HasKey(e => e.WarehouseDescriptionID).HasName("PK__Warehous__48E37B440863B342");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.WarehouseDescriptionTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Warehouse__Creat__6E414E4F");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.WarehouseDescriptionTableLastModifiedByNavigation).HasConstraintName("FK__Warehouse__LastM__702996C1");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.WarehouseDescriptionTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Warehouse__Wareh__32767D0B");
        });

        modelBuilder.Entity<WarehouseTable>(entity =>
        {
            entity.HasKey(e => e.WarehouseID).HasName("PK__Warehous__2608AFD98BCB3404");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.WarehouseTableCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Warehouse__Creat__6D4D2A16");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.WarehouseTableLastModifiedByNavigation).HasConstraintName("FK__Warehouse__LastM__6F357288");

            entity.HasOne(d => d.Location).WithMany(p => p.WarehouseTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Warehouse__Locat__3A379A64");

            entity.HasOne(d => d.Status).WithMany(p => p.WarehouseTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Warehouse__Statu__2DB1C7EE");

            entity.HasOne(d => d.WarehouseType).WithMany(p => p.WarehouseTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Warehouse__WAreh__3D14070F");
        });

        modelBuilder.Entity<WarehouseTypeTable>(entity =>
        {
            entity.HasKey(e => e.WarehouseTypeID).HasName("PK__Warehous__7E008AEE02C1415A");
        });

        modelBuilder.Entity<WebServiceLogTable>(entity =>
        {
            entity.HasKey(e => e.WebServiceLogID).HasName("PK__WebServi__129E5D89E18CD2A7");

            entity.Property(e => e.RequestedDateTime).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<ZDOF_PurchaseOrderTable>(entity =>
        {
            entity.HasKey(e => e.ZDOFPurchaseOrderID).HasName("PK__INT_DOFP__036BAC4401B1C089");

            entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<ZDOF_UserPODataTable>(entity =>
        {
            entity.HasKey(e => e.ZDOFUserPODataID).HasName("PK__ZDOF_Use__A22C66F83D066D56");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ZDOF_UserPODataTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ZDOF_User__Creat__6D6D25A7");

            entity.HasOne(d => d.Department).WithMany(p => p.ZDOF_UserPODataTable).HasConstraintName("FK__ZDOF_User__Depar__17ED6F58");

            entity.HasOne(d => d.Location).WithMany(p => p.ZDOF_UserPODataTable).HasConstraintName("FK__ZDOF_User__Locat__630F92C5");

            entity.HasOne(d => d.Product).WithMany(p => p.ZDOF_UserPODataTable).HasConstraintName("FK__ZDOF_User__Produ__4A0EDAD1");

            entity.HasOne(d => d.ZDOFPurchaseOrder).WithMany(p => p.ZDOF_UserPODataTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ZDOF_User__ZDOFP__6C79016E");
        });

        modelBuilder.Entity<vwDashboardTemplateObjects>(entity =>
        {
            entity.ToView("vwDashboardTemplateObjects");
        });

        modelBuilder.Entity<vwNotificationTemplateObjects>(entity =>
        {
            entity.ToView("vwNotificationTemplateObjects");
        });

        modelBuilder.Entity<vwReportTemplateObjects>(entity =>
        {
            entity.ToView("vwReportTemplateObjects");
        });

        OnModelCreatingGeneratedProcedures(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
