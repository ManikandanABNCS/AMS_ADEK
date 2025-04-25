using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Index("CategoryID", Name = "FK_CategoryID")]
[Index("DepartmentID", Name = "FK_DepartmentID")]
[Index("DepreciationClassID", Name = "FK_DepreciationClassID")]
[Index("LocationID", Name = "FK_LocationID")]
[Index("SectionID", Name = "FK_SectionID")]
[Index("StatusID", Name = "FK_StatusID")]
[Index("SupplierID", Name = "FK_SupplierID")]
[Index("TransferTypeID", Name = "FK_TransferTypeID")]
[Index("CompanyID", Name = "IX_AssetTable_CompanyID")]
[Index("CustodianID", Name = "IX_AssetTable_CustodianID")]
[Index("ManufacturerID", Name = "IX_AssetTable_ManufacturerID")]
[Index("ModelID", Name = "IX_AssetTable_ModelID")]
[Index("AssetID", Name = "IX_AssetTable_MutipleColumnsIndex")]
[Index("Barcode", Name = "IX_Barcode")]
[Index("StatusID", Name = "IX_MultiColum_Epired")]
[Index("ProductID", Name = "IX_ProductID")]
[Index("AssetCode", Name = "UC_AssetCode", IsUnique = true)]
[Index("Barcode", Name = "UC_Barcode", IsUnique = true)]
[Index("CompanyID", "WarrantyExpiryDate", "StatusID", Name = "inx_AssetTable_001")]
[Index("CompanyID", "StatusID", Name = "inx_AssetTable_002")]
[Index("CompanyID", "AssetCode", Name = "uc_CompanyAssetcode", IsUnique = true)]
[Index("CompanyID", "Barcode", Name = "uc_CompanyBarcode", IsUnique = true)]
public partial class AssetTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int AssetID { get; set; }

    [StringLength(50)]
    public string AssetCode { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Barcode { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? RFIDTagCode { get; set; }

    public int? LocationID { get; set; }

    public int? DepartmentID { get; set; }

    public int? SectionID { get; set; }

    public int? CustodianID { get; set; }

    public int? SupplierID { get; set; }

    public int? AssetConditionID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PONumber { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? PurchaseDate { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? PurchasePrice { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ComissionDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? WarrantyExpiryDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? DisposalReferenceNo { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DisposedDateTime { get; set; }

    public string? DisposedRemarks { get; set; }

    public string? AssetRemarks { get; set; }

    public int? DepreciationClassID { get; set; }

    public bool DepreciationFlag { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? SalvageValue { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? VoucherNo { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    public string AssetDescription { get; set; } = null!;

    [StringLength(50)]
    public string? ReferenceCode { get; set; }

    [StringLength(100)]
    public string? SerialNo { get; set; }

    [StringLength(100)]
    public string? NetworkID { get; set; }

    [StringLength(150)]
    public string? InvoiceNo { get; set; }

    [StringLength(150)]
    public string? DeliveryNote { get; set; }

    [StringLength(150)]
    public string? Make { get; set; }

    [StringLength(150)]
    public string? Capacity { get; set; }

    [StringLength(200)]
    public string? MappedAssetID { get; set; }

    public bool CreateFromHHT { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? DisposalValue { get; set; }

    public bool? MailAlert { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? partialDisposalTotalValue { get; set; }

    public int? IsTransfered { get; set; }

    public int CategoryID { get; set; }

    public int? TransferTypeID { get; set; }

    [Unicode(false)]
    public string? UploadedDocumentPath { get; set; }

    [Unicode(false)]
    public string? UploadedImagePath { get; set; }

    public int? AssetApproval { get; set; }

    [StringLength(100)]
    public string? ReceiptNumber { get; set; }

    public bool InsertedToOracle { get; set; }

    [Column(TypeName = "date")]
    public DateTime? InvoiceDate { get; set; }

    [StringLength(50)]
    public string? DistributionID { get; set; }

    public int ProductID { get; set; }

    public int? CompanyID { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? SyncDateTime { get; set; }

    [StringLength(400)]
    public string? Attribute1 { get; set; }

    [StringLength(400)]
    public string? Attribute2 { get; set; }

    [StringLength(400)]
    public string? Attribute3 { get; set; }

    [StringLength(400)]
    public string? Attribute4 { get; set; }

    [StringLength(400)]
    public string? Attribute5 { get; set; }

    [StringLength(400)]
    public string? Attribute6 { get; set; }

    [StringLength(400)]
    public string? Attribute7 { get; set; }

    [StringLength(400)]
    public string? Attribute8 { get; set; }

    [StringLength(400)]
    public string? Attribute9 { get; set; }

    [StringLength(400)]
    public string? Attribute10 { get; set; }

    [StringLength(400)]
    public string? Attribute11 { get; set; }

    [StringLength(400)]
    public string? Attribute12 { get; set; }

    [StringLength(400)]
    public string? Attribute13 { get; set; }

    [StringLength(400)]
    public string? Attribute14 { get; set; }

    [StringLength(400)]
    public string? Attribute15 { get; set; }

    [StringLength(400)]
    public string? Attribute16 { get; set; }

    public int? ManufacturerID { get; set; }

    public int? ModelID { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? QFAssetCode { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? DOFPO_LINE_NUM { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? DOFMASS_ADDITION_ID { get; set; }

    public byte ERPUpdateType { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? DOFPARENT_MASS_ADDITION_ID { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? DOFFIXED_ASSETS_UNITS { get; set; }

    public bool? DOF_MASS_SPLIT_EXECUTED { get; set; }

    public bool zDOF_Asset_Updated { get; set; }

    [Column(TypeName = "decimal(15, 10)")]
    public decimal? Latitude { get; set; }

    [Column(TypeName = "decimal(15, 10)")]
    public decimal? Longitude { get; set; }

    public int? DisposalTypeID { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? CurrentCost { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? ProceedofSales { get; set; }

    [StringLength(400)]
    [Unicode(false)]
    public string? SoldTo { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? CostOfRemoval { get; set; }

    public bool? AllowTransfer { get; set; }

    public bool IsScanned { get; set; }

    [StringLength(800)]
    public string? Attribute17 { get; set; }

    [StringLength(800)]
    public string? Attribute18 { get; set; }

    [StringLength(800)]
    public string? Attribute19 { get; set; }

    [StringLength(800)]
    public string? Attribute20 { get; set; }

    [StringLength(800)]
    public string? Attribute21 { get; set; }

    [StringLength(800)]
    public string? Attribute22 { get; set; }

    [StringLength(800)]
    public string? Attribute23 { get; set; }

    [StringLength(800)]
    public string? Attribute24 { get; set; }

    [StringLength(800)]
    public string? Attribute25 { get; set; }

    [StringLength(800)]
    public string? Attribute26 { get; set; }

    [StringLength(800)]
    public string? Attribute27 { get; set; }

    [StringLength(800)]
    public string? Attribute28 { get; set; }

    [StringLength(800)]
    public string? Attribute29 { get; set; }

    [StringLength(800)]
    public string? Attribute30 { get; set; }

    [StringLength(800)]
    public string? Attribute31 { get; set; }

    [StringLength(800)]
    public string? Attribute32 { get; set; }

    [StringLength(800)]
    public string? Attribute33 { get; set; }

    [StringLength(800)]
    public string? Attribute34 { get; set; }

    [StringLength(800)]
    public string? Attribute35 { get; set; }

    [StringLength(800)]
    public string? Attribute36 { get; set; }

    [StringLength(800)]
    public string? Attribute37 { get; set; }

    [StringLength(800)]
    public string? Attribute38 { get; set; }

    [StringLength(800)]
    public string? Attribute39 { get; set; }

    [StringLength(800)]
    public string? Attribute40 { get; set; }

    public int? RetirementTypeID { get; set; }

    public int? DisposalTransactionID { get; set; }

    public string? ImportExcelFileName { get; set; }

    [ForeignKey("AssetConditionID")]
    [InverseProperty("AssetTable")]
    public virtual AssetConditionTable? AssetCondition { get; set; }

    [InverseProperty("Asset")]
    public virtual ICollection<AssetTransactionLineItemTable> AssetTransactionLineItemTable { get; set; } = new List<AssetTransactionLineItemTable>();

    [InverseProperty("Asset")]
    public virtual ICollection<AssetTransferCategoryHistoryTable> AssetTransferCategoryHistoryTable { get; set; } = new List<AssetTransferCategoryHistoryTable>();

    [InverseProperty("Asset")]
    public virtual ICollection<AssetTransferHistoryTable> AssetTransferHistoryTable { get; set; } = new List<AssetTransferHistoryTable>();

    [InverseProperty("Asset")]
    public virtual ICollection<AssetTransferTransactionLineItemTable> AssetTransferTransactionLineItemTable { get; set; } = new List<AssetTransferTransactionLineItemTable>();

    [ForeignKey("CategoryID")]
    [InverseProperty("AssetTable")]
    public virtual CategoryTable? Category { get; set; } = null!;

    [ForeignKey("CompanyID")]
    [InverseProperty("AssetTable")]
    public virtual CompanyTable? Company { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("AssetTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("CustodianID")]
    [InverseProperty("AssetTable")]
    public virtual PersonTable? Custodian { get; set; }

    [ForeignKey("CustodianID")]
    [InverseProperty("AssetTableCustodianNavigation")]
    public virtual User_LoginUserTable? CustodianNavigation { get; set; }

    [ForeignKey("DepartmentID")]
    [InverseProperty("AssetTable")]
    public virtual DepartmentTable? Department { get; set; }

    [ForeignKey("DepreciationClassID")]
    [InverseProperty("AssetTable")]
    public virtual DepreciationClassTable? DepreciationClass { get; set; }

    [InverseProperty("Asset")]
    public virtual ICollection<DepreciationLineItemTable> DepreciationLineItemTable { get; set; } = new List<DepreciationLineItemTable>();

    [ForeignKey("DisposalTypeID")]
    [InverseProperty("AssetTable")]
    public virtual DisposalTypeTable? DisposalType { get; set; }

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("AssetTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [ForeignKey("LocationID")]
    [InverseProperty("AssetTable")]
    public virtual LocationTable? Location { get; set; }

    [ForeignKey("ManufacturerID")]
    [InverseProperty("AssetTable")]
    public virtual ManufacturerTable? Manufacturer { get; set; }

    [ForeignKey("ModelID")]
    [InverseProperty("AssetTable")]
    public virtual ModelTable? Model { get; set; }

    [ForeignKey("ProductID")]
    [InverseProperty("AssetTable")]
    public virtual ProductTable? Product { get; set; } = null!;

    [ForeignKey("RetirementTypeID")]
    [InverseProperty("AssetTable")]
    public virtual RetirementTypeTable? RetirementType { get; set; }

    [ForeignKey("SectionID")]
    [InverseProperty("AssetTable")]
    public virtual SectionTable? Section { get; set; }

    [ForeignKey("StatusID")]
    [InverseProperty("AssetTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [ForeignKey("SupplierID")]
    [InverseProperty("AssetTable")]
    public virtual PartyTable? Supplier { get; set; }

    [InverseProperty("Asset")]
    public virtual ICollection<TransactionLineItemTable> TransactionLineItemTable { get; set; } = new List<TransactionLineItemTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public AssetTable()
    {

    }

	/// <summary>
    /// Default constructor for AssetTable
    /// </summary>
    /// <param name="db"></param>
	public AssetTable(AMSContext _db)
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
		return "AssetID";
	}

	public override object GetPrimaryKeyValue()
	{
		return AssetID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.AssetCode)) this.AssetCode = this.AssetCode.Trim();
		if(!string.IsNullOrEmpty(this.Barcode)) this.Barcode = this.Barcode.Trim();
		if(!string.IsNullOrEmpty(this.RFIDTagCode)) this.RFIDTagCode = this.RFIDTagCode.Trim();
		if(!string.IsNullOrEmpty(this.PONumber)) this.PONumber = this.PONumber.Trim();
		if(!string.IsNullOrEmpty(this.DisposalReferenceNo)) this.DisposalReferenceNo = this.DisposalReferenceNo.Trim();
		if(!string.IsNullOrEmpty(this.DisposedRemarks)) this.DisposedRemarks = this.DisposedRemarks.Trim();
		if(!string.IsNullOrEmpty(this.AssetRemarks)) this.AssetRemarks = this.AssetRemarks.Trim();
		if(!string.IsNullOrEmpty(this.VoucherNo)) this.VoucherNo = this.VoucherNo.Trim();
		if(!string.IsNullOrEmpty(this.AssetDescription)) this.AssetDescription = this.AssetDescription.Trim();
		if(!string.IsNullOrEmpty(this.ReferenceCode)) this.ReferenceCode = this.ReferenceCode.Trim();
		if(!string.IsNullOrEmpty(this.SerialNo)) this.SerialNo = this.SerialNo.Trim();
		if(!string.IsNullOrEmpty(this.NetworkID)) this.NetworkID = this.NetworkID.Trim();
		if(!string.IsNullOrEmpty(this.InvoiceNo)) this.InvoiceNo = this.InvoiceNo.Trim();
		if(!string.IsNullOrEmpty(this.DeliveryNote)) this.DeliveryNote = this.DeliveryNote.Trim();
		if(!string.IsNullOrEmpty(this.Make)) this.Make = this.Make.Trim();
		if(!string.IsNullOrEmpty(this.Capacity)) this.Capacity = this.Capacity.Trim();
		if(!string.IsNullOrEmpty(this.MappedAssetID)) this.MappedAssetID = this.MappedAssetID.Trim();
		if(!string.IsNullOrEmpty(this.UploadedDocumentPath)) this.UploadedDocumentPath = this.UploadedDocumentPath.Trim();
		if(!string.IsNullOrEmpty(this.UploadedImagePath)) this.UploadedImagePath = this.UploadedImagePath.Trim();
		if(!string.IsNullOrEmpty(this.ReceiptNumber)) this.ReceiptNumber = this.ReceiptNumber.Trim();
		if(!string.IsNullOrEmpty(this.DistributionID)) this.DistributionID = this.DistributionID.Trim();
		if(!string.IsNullOrEmpty(this.Attribute1)) this.Attribute1 = this.Attribute1.Trim();
		if(!string.IsNullOrEmpty(this.Attribute2)) this.Attribute2 = this.Attribute2.Trim();
		if(!string.IsNullOrEmpty(this.Attribute3)) this.Attribute3 = this.Attribute3.Trim();
		if(!string.IsNullOrEmpty(this.Attribute4)) this.Attribute4 = this.Attribute4.Trim();
		if(!string.IsNullOrEmpty(this.Attribute5)) this.Attribute5 = this.Attribute5.Trim();
		if(!string.IsNullOrEmpty(this.Attribute6)) this.Attribute6 = this.Attribute6.Trim();
		if(!string.IsNullOrEmpty(this.Attribute7)) this.Attribute7 = this.Attribute7.Trim();
		if(!string.IsNullOrEmpty(this.Attribute8)) this.Attribute8 = this.Attribute8.Trim();
		if(!string.IsNullOrEmpty(this.Attribute9)) this.Attribute9 = this.Attribute9.Trim();
		if(!string.IsNullOrEmpty(this.Attribute10)) this.Attribute10 = this.Attribute10.Trim();
		if(!string.IsNullOrEmpty(this.Attribute11)) this.Attribute11 = this.Attribute11.Trim();
		if(!string.IsNullOrEmpty(this.Attribute12)) this.Attribute12 = this.Attribute12.Trim();
		if(!string.IsNullOrEmpty(this.Attribute13)) this.Attribute13 = this.Attribute13.Trim();
		if(!string.IsNullOrEmpty(this.Attribute14)) this.Attribute14 = this.Attribute14.Trim();
		if(!string.IsNullOrEmpty(this.Attribute15)) this.Attribute15 = this.Attribute15.Trim();
		if(!string.IsNullOrEmpty(this.Attribute16)) this.Attribute16 = this.Attribute16.Trim();
		if(!string.IsNullOrEmpty(this.QFAssetCode)) this.QFAssetCode = this.QFAssetCode.Trim();
		if(!string.IsNullOrEmpty(this.DOFPO_LINE_NUM)) this.DOFPO_LINE_NUM = this.DOFPO_LINE_NUM.Trim();
		if(!string.IsNullOrEmpty(this.DOFMASS_ADDITION_ID)) this.DOFMASS_ADDITION_ID = this.DOFMASS_ADDITION_ID.Trim();
		if(!string.IsNullOrEmpty(this.DOFPARENT_MASS_ADDITION_ID)) this.DOFPARENT_MASS_ADDITION_ID = this.DOFPARENT_MASS_ADDITION_ID.Trim();
		if(!string.IsNullOrEmpty(this.SoldTo)) this.SoldTo = this.SoldTo.Trim();
		if(!string.IsNullOrEmpty(this.Attribute17)) this.Attribute17 = this.Attribute17.Trim();
		if(!string.IsNullOrEmpty(this.Attribute18)) this.Attribute18 = this.Attribute18.Trim();
		if(!string.IsNullOrEmpty(this.Attribute19)) this.Attribute19 = this.Attribute19.Trim();
		if(!string.IsNullOrEmpty(this.Attribute20)) this.Attribute20 = this.Attribute20.Trim();
		if(!string.IsNullOrEmpty(this.Attribute21)) this.Attribute21 = this.Attribute21.Trim();
		if(!string.IsNullOrEmpty(this.Attribute22)) this.Attribute22 = this.Attribute22.Trim();
		if(!string.IsNullOrEmpty(this.Attribute23)) this.Attribute23 = this.Attribute23.Trim();
		if(!string.IsNullOrEmpty(this.Attribute24)) this.Attribute24 = this.Attribute24.Trim();
		if(!string.IsNullOrEmpty(this.Attribute25)) this.Attribute25 = this.Attribute25.Trim();
		if(!string.IsNullOrEmpty(this.Attribute26)) this.Attribute26 = this.Attribute26.Trim();
		if(!string.IsNullOrEmpty(this.Attribute27)) this.Attribute27 = this.Attribute27.Trim();
		if(!string.IsNullOrEmpty(this.Attribute28)) this.Attribute28 = this.Attribute28.Trim();
		if(!string.IsNullOrEmpty(this.Attribute29)) this.Attribute29 = this.Attribute29.Trim();
		if(!string.IsNullOrEmpty(this.Attribute30)) this.Attribute30 = this.Attribute30.Trim();
		if(!string.IsNullOrEmpty(this.Attribute31)) this.Attribute31 = this.Attribute31.Trim();
		if(!string.IsNullOrEmpty(this.Attribute32)) this.Attribute32 = this.Attribute32.Trim();
		if(!string.IsNullOrEmpty(this.Attribute33)) this.Attribute33 = this.Attribute33.Trim();
		if(!string.IsNullOrEmpty(this.Attribute34)) this.Attribute34 = this.Attribute34.Trim();
		if(!string.IsNullOrEmpty(this.Attribute35)) this.Attribute35 = this.Attribute35.Trim();
		if(!string.IsNullOrEmpty(this.Attribute36)) this.Attribute36 = this.Attribute36.Trim();
		if(!string.IsNullOrEmpty(this.Attribute37)) this.Attribute37 = this.Attribute37.Trim();
		if(!string.IsNullOrEmpty(this.Attribute38)) this.Attribute38 = this.Attribute38.Trim();
		if(!string.IsNullOrEmpty(this.Attribute39)) this.Attribute39 = this.Attribute39.Trim();
		if(!string.IsNullOrEmpty(this.Attribute40)) this.Attribute40 = this.Attribute40.Trim();
		if(!string.IsNullOrEmpty(this.ImportExcelFileName)) this.ImportExcelFileName = this.ImportExcelFileName.Trim();

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

    public static AssetTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.AssetTable
                where b.AssetID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<AssetTable> GetAllItemsByDepartment(AMSContext _db, int departmentID , bool includeInactiveItems = false )
    {
        return from b in GetAllItems(_db, includeInactiveItems)
                where b.DepartmentID == departmentID
                select b;
    }

    public static IQueryable<AssetTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.AssetTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.AssetID descending
                    select b);
        }
        else
        {
            return (from b in _db.AssetTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.AssetID descending
                    select b);
        }
    }

    public static IQueryable<AssetTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return from b in AssetTable.GetAllItems(_db, includeInactiveItems)
                
                select b;
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
        return (from b in _db.AssetTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return AssetTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return AssetTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
