using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Index("ProductCode", Name = "UC_ProductCode", IsUnique = true)]
public partial class ProductTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ProductID { get; set; }

    [StringLength(100)]
    public string ProductCode { get; set; } = null!;

    public int CategoryID { get; set; }

    public bool? IsInventoryItem { get; set; }

    public bool? IsSerialNumberRequired { get; set; }

    public int? UOMID { get; set; }

    public int? ReorderLevelQuantity { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? Price { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    [StringLength(200)]
    public string? Attribute1 { get; set; }

    [StringLength(200)]
    public string? Attribute2 { get; set; }

    [StringLength(200)]
    public string? Attribute3 { get; set; }

    [StringLength(200)]
    public string? Attribute4 { get; set; }

    [StringLength(200)]
    public string? Attribute5 { get; set; }

    [StringLength(200)]
    public string? Attribute6 { get; set; }

    [StringLength(200)]
    public string? Attribute7 { get; set; }

    [StringLength(200)]
    public string? Attribute8 { get; set; }

    [StringLength(200)]
    public string? Attribute9 { get; set; }

    [StringLength(200)]
    public string? Attribute10 { get; set; }

    [StringLength(200)]
    public string? Attribute11 { get; set; }

    [StringLength(200)]
    public string? Attribute12 { get; set; }

    [StringLength(200)]
    public string? Attribute13 { get; set; }

    [StringLength(200)]
    public string? Attribute14 { get; set; }

    [StringLength(200)]
    public string? Attribute15 { get; set; }

    [StringLength(200)]
    public string? Attribute16 { get; set; }

    public string ProductName { get; set; } = null!;

    public string? CatalogueImage { get; set; }

    [StringLength(100)]
    public string? VirtualBarcode { get; set; }

    public string? Specification { get; set; }

    public string? Remarks { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<AssetTable> AssetTable { get; set; } = new List<AssetTable>();

    [InverseProperty("NewProduct")]
    public virtual ICollection<AssetTransferCategoryHistoryTable> AssetTransferCategoryHistoryTableNewProduct { get; set; } = new List<AssetTransferCategoryHistoryTable>();

    [InverseProperty("OldProduct")]
    public virtual ICollection<AssetTransferCategoryHistoryTable> AssetTransferCategoryHistoryTableOldProduct { get; set; } = new List<AssetTransferCategoryHistoryTable>();

    [ForeignKey("CategoryID")]
    [InverseProperty("ProductTable")]
    public virtual CategoryTable? Category { get; set; } = null!;

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProductTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<GRNLineItemTable> GRNLineItemTable { get; set; } = new List<GRNLineItemTable>();

    [InverseProperty("Product")]
    public virtual ICollection<InvoiceLineItemTable> InvoiceLineItemTable { get; set; } = new List<InvoiceLineItemTable>();

    [InverseProperty("Product")]
    public virtual ICollection<ItemDispatchLineItemTable> ItemDispatchLineItemTable { get; set; } = new List<ItemDispatchLineItemTable>();

    [InverseProperty("Product")]
    public virtual ICollection<ItemReqestLineItemTable> ItemReqestLineItemTable { get; set; } = new List<ItemReqestLineItemTable>();

    [InverseProperty("Product")]
    public virtual ICollection<ItemSupplierMappingTable> ItemSupplierMappingTable { get; set; } = new List<ItemSupplierMappingTable>();

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("ProductTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<PriceAnalysisLineItemTable> PriceAnalysisLineItemTable { get; set; } = new List<PriceAnalysisLineItemTable>();

    [InverseProperty("Product")]
    public virtual ICollection<ProductDescriptionTable> ProductDescriptionTable { get; set; } = new List<ProductDescriptionTable>();

    [InverseProperty("Product")]
    public virtual ICollection<ProductUOMMappingTable> ProductUOMMappingTable { get; set; } = new List<ProductUOMMappingTable>();

    [InverseProperty("Product")]
    public virtual ICollection<PurchaseOrderLineItemTable> PurchaseOrderLineItemTable { get; set; } = new List<PurchaseOrderLineItemTable>();

    [InverseProperty("Product")]
    public virtual ICollection<RequestQuotationLineItemTable> RequestQuotationLineItemTable { get; set; } = new List<RequestQuotationLineItemTable>();

    [ForeignKey("StatusID")]
    [InverseProperty("ProductTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<SupplierQuotationLineItemTable> SupplierQuotationLineItemTable { get; set; } = new List<SupplierQuotationLineItemTable>();

    [InverseProperty("FromProduct")]
    public virtual ICollection<TransactionLineItemTable> TransactionLineItemTableFromProduct { get; set; } = new List<TransactionLineItemTable>();

    [InverseProperty("ToProduct")]
    public virtual ICollection<TransactionLineItemTable> TransactionLineItemTableToProduct { get; set; } = new List<TransactionLineItemTable>();

    [ForeignKey("UOMID")]
    [InverseProperty("ProductTable")]
    public virtual UOMTable? UOM { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<ZDOF_UserPODataTable> ZDOF_UserPODataTable { get; set; } = new List<ZDOF_UserPODataTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ProductTable()
    {

    }

	/// <summary>
    /// Default constructor for ProductTable
    /// </summary>
    /// <param name="db"></param>
	public ProductTable(AMSContext _db)
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
		return "ProductID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ProductID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.ProductCode)) this.ProductCode = this.ProductCode.Trim();
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
		if(!string.IsNullOrEmpty(this.ProductName)) this.ProductName = this.ProductName.Trim();
		if(!string.IsNullOrEmpty(this.CatalogueImage)) this.CatalogueImage = this.CatalogueImage.Trim();
		if(!string.IsNullOrEmpty(this.VirtualBarcode)) this.VirtualBarcode = this.VirtualBarcode.Trim();
		if(!string.IsNullOrEmpty(this.Specification)) this.Specification = this.Specification.Trim();
		if(!string.IsNullOrEmpty(this.Remarks)) this.Remarks = this.Remarks.Trim();

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

    public static ProductTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ProductTable
                where b.ProductID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ProductTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.ProductTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.ProductID descending
                    select b);
        }
        else
        {
            return (from b in _db.ProductTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.ProductID descending
                    select b);
        }
    }

    public static IQueryable<ProductTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ProductTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ProductTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ProductTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ProductTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
