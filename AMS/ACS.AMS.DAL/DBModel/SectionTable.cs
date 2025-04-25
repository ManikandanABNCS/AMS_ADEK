using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Index("SectionCode", Name = "UC_SectionCode", IsUnique = true)]
public partial class SectionTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int SectionID { get; set; }

    [StringLength(100)]
    public string SectionCode { get; set; } = null!;

    public int DepartmentID { get; set; }

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

    public string SectionName { get; set; } = null!;

    [InverseProperty("Section")]
    public virtual ICollection<AssetTable> AssetTable { get; set; } = new List<AssetTable>();

    [InverseProperty("NewSection")]
    public virtual ICollection<AssetTransferHistoryTable> AssetTransferHistoryTableNewSection { get; set; } = new List<AssetTransferHistoryTable>();

    [InverseProperty("OldSection")]
    public virtual ICollection<AssetTransferHistoryTable> AssetTransferHistoryTableOldSection { get; set; } = new List<AssetTransferHistoryTable>();

    [InverseProperty("Section")]
    public virtual ICollection<BarcodeConstructTable> BarcodeConstructTable { get; set; } = new List<BarcodeConstructTable>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("SectionTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("DepartmentID")]
    [InverseProperty("SectionTable")]
    public virtual DepartmentTable? Department { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("SectionTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [InverseProperty("Section")]
    public virtual ICollection<SectionDescriptionTable> SectionDescriptionTable { get; set; } = new List<SectionDescriptionTable>();

    [ForeignKey("StatusID")]
    [InverseProperty("SectionTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [InverseProperty("FromSection")]
    public virtual ICollection<TransactionLineItemTable> TransactionLineItemTableFromSection { get; set; } = new List<TransactionLineItemTable>();

    [InverseProperty("Section")]
    public virtual ICollection<TransactionLineItemTable> TransactionLineItemTableSection { get; set; } = new List<TransactionLineItemTable>();

    [InverseProperty("ToSection")]
    public virtual ICollection<TransactionLineItemTable> TransactionLineItemTableToSection { get; set; } = new List<TransactionLineItemTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public SectionTable()
    {

    }

	/// <summary>
    /// Default constructor for SectionTable
    /// </summary>
    /// <param name="db"></param>
	public SectionTable(AMSContext _db)
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
		return "SectionID";
	}

	public override object GetPrimaryKeyValue()
	{
		return SectionID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.SectionCode)) this.SectionCode = this.SectionCode.Trim();
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
		if(!string.IsNullOrEmpty(this.SectionName)) this.SectionName = this.SectionName.Trim();

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

    public static SectionTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.SectionTable
                where b.SectionID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<SectionTable> GetAllItemsByDepartment(AMSContext _db, int departmentID , bool includeInactiveItems = false )
    {
        return from b in GetAllItems(_db, includeInactiveItems)
                where b.DepartmentID == departmentID
                select b;
    }

    public static IQueryable<SectionTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.SectionTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.SectionID descending
                    select b);
        }
        else
        {
            return (from b in _db.SectionTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.SectionID descending
                    select b);
        }
    }

    public static IQueryable<SectionTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return from b in SectionTable.GetAllItems(_db, includeInactiveItems)
                
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
        return (from b in _db.SectionTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return SectionTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return SectionTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
