using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class RetirementTypeTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int RetirementTypeID { get; set; }

    [StringLength(100)]
    public string RetirementTypeCode { get; set; } = null!;

    [StringLength(100)]
    public string RetirementTypeName { get; set; } = null!;

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

    [InverseProperty("RetirementType")]
    public virtual ICollection<AssetTable> AssetTable { get; set; } = new List<AssetTable>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("RetirementTypeTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("RetirementTypeTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [InverseProperty("RetirementType")]
    public virtual ICollection<RetirementTypeDescriptionTable> RetirementTypeDescriptionTable { get; set; } = new List<RetirementTypeDescriptionTable>();

    [ForeignKey("StatusID")]
    [InverseProperty("RetirementTypeTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [InverseProperty("RetirementType")]
    public virtual ICollection<TransactionLineItemTable> TransactionLineItemTable { get; set; } = new List<TransactionLineItemTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public RetirementTypeTable()
    {

    }

	/// <summary>
    /// Default constructor for RetirementTypeTable
    /// </summary>
    /// <param name="db"></param>
	public RetirementTypeTable(AMSContext _db)
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
		return "RetirementTypeID";
	}

	public override object GetPrimaryKeyValue()
	{
		return RetirementTypeID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.RetirementTypeCode)) this.RetirementTypeCode = this.RetirementTypeCode.Trim();
		if(!string.IsNullOrEmpty(this.RetirementTypeName)) this.RetirementTypeName = this.RetirementTypeName.Trim();
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

    public static RetirementTypeTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.RetirementTypeTable
                where b.RetirementTypeID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<RetirementTypeTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.RetirementTypeTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.RetirementTypeID descending
                    select b);
        }
        else
        {
            return (from b in _db.RetirementTypeTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.RetirementTypeID descending
                    select b);
        }
    }

    public static IQueryable<RetirementTypeTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return RetirementTypeTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.RetirementTypeTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return RetirementTypeTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return RetirementTypeTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
