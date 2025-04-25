using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Index("CategoryCode", Name = "UC_CategoryCode", IsUnique = true)]
public partial class CategoryTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int CategoryID { get; set; }

    [StringLength(100)]
    public string CategoryCode { get; set; } = null!;

    public int? ParentCategoryID { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    public int? BudgetYearID { get; set; }

    public bool? IsInventory { get; set; }

    public int? ERPCategoryID { get; set; }

    public int? DepreciationClassID { get; set; }

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

    public bool ISBudgetCategory { get; set; }

    public int? ExpenseTypeID { get; set; }

    public string CategoryName { get; set; } = null!;

    public int? CategoryTypeID { get; set; }

    public string? CatalogueImage { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<AssetTable> AssetTable { get; set; } = new List<AssetTable>();

    [InverseProperty("NewCategory")]
    public virtual ICollection<AssetTransferCategoryHistoryTable> AssetTransferCategoryHistoryTableNewCategory { get; set; } = new List<AssetTransferCategoryHistoryTable>();

    [InverseProperty("OldCategory")]
    public virtual ICollection<AssetTransferCategoryHistoryTable> AssetTransferCategoryHistoryTableOldCategory { get; set; } = new List<AssetTransferCategoryHistoryTable>();

    [InverseProperty("Category")]
    public virtual ICollection<BarcodeConstructTable> BarcodeConstructTable { get; set; } = new List<BarcodeConstructTable>();

    [ForeignKey("BudgetYearID")]
    [InverseProperty("CategoryTable")]
    public virtual BudgetYearTable? BudgetYear { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<CategoryDescriptionTable> CategoryDescriptionTable { get; set; } = new List<CategoryDescriptionTable>();

    [ForeignKey("CategoryTypeID")]
    [InverseProperty("CategoryTable")]
    public virtual CategoryTypeTable? CategoryType { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("CategoryTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("DepreciationClassID")]
    [InverseProperty("CategoryTable")]
    public virtual DepreciationClassTable? DepreciationClass { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<DepreciationLineItemTable> DepreciationLineItemTable { get; set; } = new List<DepreciationLineItemTable>();

    [ForeignKey("ExpenseTypeID")]
    [InverseProperty("CategoryTable")]
    public virtual ExpenseTypeTable? ExpenseType { get; set; }

    [InverseProperty("ParentCategory")]
    public virtual ICollection<CategoryTable> InverseParentCategory { get; set; } = new List<CategoryTable>();

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("CategoryTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<ManufacturerTable> ManufacturerTable { get; set; } = new List<ManufacturerTable>();

    [InverseProperty("Category")]
    public virtual ICollection<ModelTable> ModelTable { get; set; } = new List<ModelTable>();

    [ForeignKey("ParentCategoryID")]
    [InverseProperty("InverseParentCategory")]
    public virtual CategoryTable? ParentCategory { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<ProductTable> ProductTable { get; set; } = new List<ProductTable>();

    [ForeignKey("StatusID")]
    [InverseProperty("CategoryTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [InverseProperty("FromCategory")]
    public virtual ICollection<TransactionLineItemTable> TransactionLineItemTableFromCategory { get; set; } = new List<TransactionLineItemTable>();

    [InverseProperty("ToCategory")]
    public virtual ICollection<TransactionLineItemTable> TransactionLineItemTableToCategory { get; set; } = new List<TransactionLineItemTable>();

    [InverseProperty("Category")]
    public virtual ICollection<UserCategoryMappingTable> UserCategoryMappingTable { get; set; } = new List<UserCategoryMappingTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public CategoryTable()
    {

    }

	/// <summary>
    /// Default constructor for CategoryTable
    /// </summary>
    /// <param name="db"></param>
	public CategoryTable(AMSContext _db)
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
		return "CategoryID";
	}

	public override object GetPrimaryKeyValue()
	{
		return CategoryID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.CategoryCode)) this.CategoryCode = this.CategoryCode.Trim();
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
		if(!string.IsNullOrEmpty(this.CategoryName)) this.CategoryName = this.CategoryName.Trim();
		if(!string.IsNullOrEmpty(this.CatalogueImage)) this.CatalogueImage = this.CatalogueImage.Trim();

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

    public static CategoryTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.CategoryTable
                where b.CategoryID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<CategoryTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.CategoryTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.CategoryID descending
                    select b);
        }
        else
        {
            return (from b in _db.CategoryTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.CategoryID descending
                    select b);
        }
    }

    public static IQueryable<CategoryTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return CategoryTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.CategoryTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return CategoryTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return CategoryTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
