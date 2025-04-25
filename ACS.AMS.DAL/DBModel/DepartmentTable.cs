using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Index("DepartmentCode", Name = "UC_DepartmentCode", IsUnique = true)]
public partial class DepartmentTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int DepartmentID { get; set; }

    [StringLength(100)]
    public string DepartmentCode { get; set; } = null!;

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

    public string DepartmentName { get; set; } = null!;

    [InverseProperty("Department")]
    public virtual ICollection<AssetTable> AssetTable { get; set; } = new List<AssetTable>();

    [InverseProperty("NewDepartment")]
    public virtual ICollection<AssetTransferHistoryTable> AssetTransferHistoryTableNewDepartment { get; set; } = new List<AssetTransferHistoryTable>();

    [InverseProperty("OldDepartment")]
    public virtual ICollection<AssetTransferHistoryTable> AssetTransferHistoryTableOldDepartment { get; set; } = new List<AssetTransferHistoryTable>();

    [InverseProperty("Department")]
    public virtual ICollection<BarcodeConstructTable> BarcodeConstructTable { get; set; } = new List<BarcodeConstructTable>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("DepartmentTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [InverseProperty("Department")]
    public virtual ICollection<DepartmentDescriptionTable> DepartmentDescriptionTable { get; set; } = new List<DepartmentDescriptionTable>();

    [InverseProperty("Department")]
    public virtual ICollection<ItemReqestTable> ItemReqestTable { get; set; } = new List<ItemReqestTable>();

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("DepartmentTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [InverseProperty("Department")]
    public virtual ICollection<PersonTable> PersonTable { get; set; } = new List<PersonTable>();

    [InverseProperty("Project")]
    public virtual ICollection<ProjectDescriptionTable> ProjectDescriptionTable { get; set; } = new List<ProjectDescriptionTable>();

    [InverseProperty("Department")]
    public virtual ICollection<PurchaseOrderTable> PurchaseOrderTable { get; set; } = new List<PurchaseOrderTable>();

    [InverseProperty("Department")]
    public virtual ICollection<SectionTable> SectionTable { get; set; } = new List<SectionTable>();

    [ForeignKey("StatusID")]
    [InverseProperty("DepartmentTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [InverseProperty("Department")]
    public virtual ICollection<TransactionLineItemTable> TransactionLineItemTableDepartment { get; set; } = new List<TransactionLineItemTable>();

    [InverseProperty("FromDepartment")]
    public virtual ICollection<TransactionLineItemTable> TransactionLineItemTableFromDepartment { get; set; } = new List<TransactionLineItemTable>();

    [InverseProperty("ToDepartment")]
    public virtual ICollection<TransactionLineItemTable> TransactionLineItemTableToDepartment { get; set; } = new List<TransactionLineItemTable>();

    [InverseProperty("Department")]
    public virtual ICollection<UserDepartmentMappingTable> UserDepartmentMappingTable { get; set; } = new List<UserDepartmentMappingTable>();

    [InverseProperty("Department")]
    public virtual ICollection<ZDOF_UserPODataTable> ZDOF_UserPODataTable { get; set; } = new List<ZDOF_UserPODataTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public DepartmentTable()
    {

    }

	/// <summary>
    /// Default constructor for DepartmentTable
    /// </summary>
    /// <param name="db"></param>
	public DepartmentTable(AMSContext _db)
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
		return "DepartmentID";
	}

	public override object GetPrimaryKeyValue()
	{
		return DepartmentID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.DepartmentCode)) this.DepartmentCode = this.DepartmentCode.Trim();
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
		if(!string.IsNullOrEmpty(this.DepartmentName)) this.DepartmentName = this.DepartmentName.Trim();

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

    public static DepartmentTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.DepartmentTable
                where b.DepartmentID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<DepartmentTable> GetAllItemsByDepartment(AMSContext _db, int departmentID , bool includeInactiveItems = false )
    {
        return from b in GetAllItems(_db, includeInactiveItems)
                where b.DepartmentID == departmentID
                select b;
    }

    public static IQueryable<DepartmentTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.DepartmentTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.DepartmentID descending
                    select b);
        }
        else
        {
            return (from b in _db.DepartmentTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.DepartmentID descending
                    select b);
        }
    }

    public static IQueryable<DepartmentTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return from b in DepartmentTable.GetAllItems(_db, includeInactiveItems)
                
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
        return (from b in _db.DepartmentTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return DepartmentTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return DepartmentTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
