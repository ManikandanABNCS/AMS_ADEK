using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ManufacturerTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ManufacturerID { get; set; }

    [StringLength(100)]
    public string ManufacturerCode { get; set; } = null!;

    public string ManufacturerName { get; set; } = null!;

    public int? CategoryID { get; set; }

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

    [InverseProperty("Manufacturer")]
    public virtual ICollection<AssetTable> AssetTable { get; set; } = new List<AssetTable>();

    [ForeignKey("CategoryID")]
    [InverseProperty("ManufacturerTable")]
    public virtual CategoryTable? Category { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ManufacturerTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("ManufacturerTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [InverseProperty("Manufacturer")]
    public virtual ICollection<ManufacturerDescriptionTable> ManufacturerDescriptionTable { get; set; } = new List<ManufacturerDescriptionTable>();

    [InverseProperty("Manufacturer")]
    public virtual ICollection<ModelTable> ModelTable { get; set; } = new List<ModelTable>();

    [ForeignKey("StatusID")]
    [InverseProperty("ManufacturerTable")]
    public virtual StatusTable? Status { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ManufacturerTable()
    {

    }

	/// <summary>
    /// Default constructor for ManufacturerTable
    /// </summary>
    /// <param name="db"></param>
	public ManufacturerTable(AMSContext _db)
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
		return "ManufacturerID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ManufacturerID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.ManufacturerCode)) this.ManufacturerCode = this.ManufacturerCode.Trim();
		if(!string.IsNullOrEmpty(this.ManufacturerName)) this.ManufacturerName = this.ManufacturerName.Trim();
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

    public static ManufacturerTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ManufacturerTable
                where b.ManufacturerID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ManufacturerTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.ManufacturerTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.ManufacturerID descending
                    select b);
        }
        else
        {
            return (from b in _db.ManufacturerTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.ManufacturerID descending
                    select b);
        }
    }

    public static IQueryable<ManufacturerTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ManufacturerTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ManufacturerTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ManufacturerTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ManufacturerTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
