using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Index("LocationCode", Name = "UC_LocationCode", IsUnique = true)]
public partial class LocationTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int LocationID { get; set; }

    [StringLength(100)]
    public string LocationCode { get; set; } = null!;

    public int? ParentLocationID { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    public int? BudgetYearID { get; set; }

    public bool? IsWIPLocation { get; set; }

    [StringLength(100)]
    public string? OracleLocationID { get; set; }

    public int? CompanyID { get; set; }

    public bool? IsStoreLocation { get; set; }

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

    public string LocationName { get; set; } = null!;

    public int? LocationTypeID { get; set; }

    public string? StampPath { get; set; }

    [InverseProperty("FromLocation")]
    public virtual ICollection<ApprovalHistoryTable> ApprovalHistoryTableFromLocation { get; set; } = new List<ApprovalHistoryTable>();

    [InverseProperty("ToLocation")]
    public virtual ICollection<ApprovalHistoryTable> ApprovalHistoryTableToLocation { get; set; } = new List<ApprovalHistoryTable>();

    [InverseProperty("Location")]
    public virtual ICollection<AssetTable> AssetTable { get; set; } = new List<AssetTable>();

    [InverseProperty("NewLocation")]
    public virtual ICollection<AssetTransferHistoryTable> AssetTransferHistoryTableNewLocation { get; set; } = new List<AssetTransferHistoryTable>();

    [InverseProperty("OldLocation")]
    public virtual ICollection<AssetTransferHistoryTable> AssetTransferHistoryTableOldLocation { get; set; } = new List<AssetTransferHistoryTable>();

    [InverseProperty("Location")]
    public virtual ICollection<AssetTransferTransactionTable> AssetTransferTransactionTable { get; set; } = new List<AssetTransferTransactionTable>();

    [InverseProperty("Location")]
    public virtual ICollection<BarcodeConstructTable> BarcodeConstructTable { get; set; } = new List<BarcodeConstructTable>();

    [ForeignKey("BudgetYearID")]
    [InverseProperty("LocationTable")]
    public virtual BudgetYearTable? BudgetYear { get; set; }

    [ForeignKey("CompanyID")]
    [InverseProperty("LocationTable")]
    public virtual CompanyTable? Company { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("LocationTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [InverseProperty("ParentLocation")]
    public virtual ICollection<LocationTable> InverseParentLocation { get; set; } = new List<LocationTable>();

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("LocationTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [InverseProperty("Location")]
    public virtual ICollection<LocationDescriptionTable> LocationDescriptionTable { get; set; } = new List<LocationDescriptionTable>();

    [ForeignKey("LocationTypeID")]
    [InverseProperty("LocationTable")]
    public virtual LocationTypeTable? LocationType { get; set; }

    [ForeignKey("ParentLocationID")]
    [InverseProperty("InverseParentLocation")]
    public virtual LocationTable? ParentLocation { get; set; }

    [ForeignKey("StatusID")]
    [InverseProperty("LocationTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [InverseProperty("FromLocation")]
    public virtual ICollection<TransactionLineItemTable> TransactionLineItemTableFromLocation { get; set; } = new List<TransactionLineItemTable>();

    [InverseProperty("Room")]
    public virtual ICollection<TransactionLineItemTable> TransactionLineItemTableRoom { get; set; } = new List<TransactionLineItemTable>();

    [InverseProperty("ToLocation")]
    public virtual ICollection<TransactionLineItemTable> TransactionLineItemTableToLocation { get; set; } = new List<TransactionLineItemTable>();

    [InverseProperty("Location")]
    public virtual ICollection<UserApprovalRoleMappingTable> UserApprovalRoleMappingTable { get; set; } = new List<UserApprovalRoleMappingTable>();

    [InverseProperty("Location")]
    public virtual ICollection<UserLocationMappingTable> UserLocationMappingTable { get; set; } = new List<UserLocationMappingTable>();

    [InverseProperty("Location")]
    public virtual ICollection<WarehouseTable> WarehouseTable { get; set; } = new List<WarehouseTable>();

    [InverseProperty("Location")]
    public virtual ICollection<ZDOF_UserPODataTable> ZDOF_UserPODataTable { get; set; } = new List<ZDOF_UserPODataTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public LocationTable()
    {

    }

	/// <summary>
    /// Default constructor for LocationTable
    /// </summary>
    /// <param name="db"></param>
	public LocationTable(AMSContext _db)
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
		return "LocationID";
	}

	public override object GetPrimaryKeyValue()
	{
		return LocationID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.LocationCode)) this.LocationCode = this.LocationCode.Trim();
		if(!string.IsNullOrEmpty(this.OracleLocationID)) this.OracleLocationID = this.OracleLocationID.Trim();
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
		if(!string.IsNullOrEmpty(this.LocationName)) this.LocationName = this.LocationName.Trim();
		if(!string.IsNullOrEmpty(this.StampPath)) this.StampPath = this.StampPath.Trim();

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

    public static LocationTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.LocationTable
                where b.LocationID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<LocationTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.LocationTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.LocationID descending
                    select b);
        }
        else
        {
            return (from b in _db.LocationTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.LocationID descending
                    select b);
        }
    }

    public static IQueryable<LocationTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return LocationTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.LocationTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return LocationTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return LocationTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
