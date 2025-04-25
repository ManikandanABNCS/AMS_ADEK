using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Index("PersonCode", Name = "UC_PersonCode", IsUnique = true)]
public partial class PersonTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int PersonID { get; set; }

    [StringLength(100)]
    public string PersonCode { get; set; } = null!;

    [StringLength(100)]
    public string PersonFirstName { get; set; } = null!;

    [StringLength(100)]
    public string PersonLastName { get; set; } = null!;

    [StringLength(200)]
    public string? PersonFirstNameLanguageTwo { get; set; }

    [StringLength(200)]
    public string? PersonLastNameLanguageTwo { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? EMailID { get; set; }

    public int? DepartmentID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DOJ { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string Gender { get; set; } = null!;

    [StringLength(50)]
    public string Culture { get; set; } = null!;

    public int UserTypeID { get; set; }

    [StringLength(100)]
    public string? MobileNo { get; set; }

    [StringLength(100)]
    public string? WhatsAppMobileNo { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

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

    public int? DesignationID { get; set; }

    public string? SignaturePath { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    [StringLength(50)]
    public string? CreatedFrom { get; set; }

    [InverseProperty("Custodian")]
    public virtual ICollection<AssetTable> AssetTable { get; set; } = new List<AssetTable>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("PersonTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("DepartmentID")]
    [InverseProperty("PersonTable")]
    public virtual DepartmentTable? Department { get; set; }

    [ForeignKey("DesignationID")]
    [InverseProperty("PersonTable")]
    public virtual DesignationTable? Designation { get; set; }

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DesignationDescriptionTable> DesignationDescriptionTableCreatedByNavigation { get; set; } = new List<DesignationDescriptionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<DesignationDescriptionTable> DesignationDescriptionTableLastModifiedByNavigation { get; set; } = new List<DesignationDescriptionTable>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ImportFormatNewTable> ImportFormatNewTableCreatedByNavigation { get; set; } = new List<ImportFormatNewTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<ImportFormatNewTable> ImportFormatNewTableLastModifiedByNavigation { get; set; } = new List<ImportFormatNewTable>();

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("PersonTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ManufacturerDescriptionTable> ManufacturerDescriptionTableCreatedByNavigation { get; set; } = new List<ManufacturerDescriptionTable>();

    [InverseProperty("LastModifiedByNavigation")]
    public virtual ICollection<ManufacturerDescriptionTable> ManufacturerDescriptionTableLastModifiedByNavigation { get; set; } = new List<ManufacturerDescriptionTable>();

    [ForeignKey("StatusID")]
    [InverseProperty("PersonTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [InverseProperty("FromCustodian")]
    public virtual ICollection<TransactionLineItemTable> TransactionLineItemTableFromCustodian { get; set; } = new List<TransactionLineItemTable>();

    [InverseProperty("ToCustodian")]
    public virtual ICollection<TransactionLineItemTable> TransactionLineItemTableToCustodian { get; set; } = new List<TransactionLineItemTable>();

    [ForeignKey("UserTypeID")]
    [InverseProperty("PersonTable")]
    public virtual UserTypeTable? UserType { get; set; } = null!;

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ZDOF_UserPODataTable> ZDOF_UserPODataTable { get; set; } = new List<ZDOF_UserPODataTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public PersonTable()
    {

    }

	/// <summary>
    /// Default constructor for PersonTable
    /// </summary>
    /// <param name="db"></param>
	public PersonTable(AMSContext _db)
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
		return "PersonID";
	}

	public override object GetPrimaryKeyValue()
	{
		return PersonID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.PersonCode)) this.PersonCode = this.PersonCode.Trim();
		if(!string.IsNullOrEmpty(this.PersonFirstName)) this.PersonFirstName = this.PersonFirstName.Trim();
		if(!string.IsNullOrEmpty(this.PersonLastName)) this.PersonLastName = this.PersonLastName.Trim();
		if(!string.IsNullOrEmpty(this.PersonFirstNameLanguageTwo)) this.PersonFirstNameLanguageTwo = this.PersonFirstNameLanguageTwo.Trim();
		if(!string.IsNullOrEmpty(this.PersonLastNameLanguageTwo)) this.PersonLastNameLanguageTwo = this.PersonLastNameLanguageTwo.Trim();
		if(!string.IsNullOrEmpty(this.EMailID)) this.EMailID = this.EMailID.Trim();
		if(!string.IsNullOrEmpty(this.Gender)) this.Gender = this.Gender.Trim();
		if(!string.IsNullOrEmpty(this.Culture)) this.Culture = this.Culture.Trim();
		if(!string.IsNullOrEmpty(this.MobileNo)) this.MobileNo = this.MobileNo.Trim();
		if(!string.IsNullOrEmpty(this.WhatsAppMobileNo)) this.WhatsAppMobileNo = this.WhatsAppMobileNo.Trim();
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
		if(!string.IsNullOrEmpty(this.SignaturePath)) this.SignaturePath = this.SignaturePath.Trim();
		if(!string.IsNullOrEmpty(this.CreatedFrom)) this.CreatedFrom = this.CreatedFrom.Trim();

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

    public static PersonTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.PersonTable
                where b.PersonID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<PersonTable> GetAllItemsByDepartment(AMSContext _db, int departmentID , bool includeInactiveItems = false )
    {
        return from b in GetAllItems(_db, includeInactiveItems)
                where b.DepartmentID == departmentID
                select b;
    }

    public static IQueryable<PersonTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.PersonTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.PersonID descending
                    select b);
        }
        else
        {
            return (from b in _db.PersonTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.PersonID descending
                    select b);
        }
    }

    public static IQueryable<PersonTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return from b in PersonTable.GetAllItems(_db, includeInactiveItems)
                
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
        return (from b in _db.PersonTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return PersonTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return PersonTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
