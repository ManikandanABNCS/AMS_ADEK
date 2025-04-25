using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Index("PartyCode", Name = "UC_PartyCode", IsUnique = true)]
public partial class PartyTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int PartyID { get; set; }

    [StringLength(100)]
    public string PartyCode { get; set; } = null!;

    public int PartyTypeID { get; set; }

    public string? TradeName { get; set; }

    public int? CountryID { get; set; }

    public string? ContactPerson { get; set; }

    public string? ContactPersonMobile { get; set; }

    public string? Email { get; set; }

    public string? ContactPersonEmail { get; set; }

    public string? Telephone1 { get; set; }

    public string? Telephone2 { get; set; }

    public string? NotificationEmail { get; set; }

    public string? Mobile { get; set; }

    public string? Fax { get; set; }

    public string? TRNNo { get; set; }

    public string? Remark { get; set; }

    public string? TaxFileNo { get; set; }

    public string? CorporateRegistartionNo { get; set; }

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

    public string PartyName { get; set; } = null!;

    [InverseProperty("Supplier")]
    public virtual ICollection<AssetTable> AssetTable { get; set; } = new List<AssetTable>();

    [InverseProperty("Vendor")]
    public virtual ICollection<AssetTransactionTable> AssetTransactionTable { get; set; } = new List<AssetTransactionTable>();

    [ForeignKey("CountryID")]
    [InverseProperty("PartyTable")]
    public virtual CountryTable? Country { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("PartyTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [InverseProperty("Supplier")]
    public virtual ICollection<ItemReqestTable> ItemReqestTable { get; set; } = new List<ItemReqestTable>();

    [InverseProperty("Party")]
    public virtual ICollection<ItemSupplierMappingTable> ItemSupplierMappingTable { get; set; } = new List<ItemSupplierMappingTable>();

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("PartyTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [InverseProperty("Party")]
    public virtual ICollection<PartyDescriptionTable> PartyDescriptionTable { get; set; } = new List<PartyDescriptionTable>();

    [ForeignKey("PartyTypeID")]
    [InverseProperty("PartyTable")]
    public virtual PartyTypeTable? PartyType { get; set; } = null!;

    [InverseProperty("Supplier")]
    public virtual ICollection<RequestQuotationTable> RequestQuotationTable { get; set; } = new List<RequestQuotationTable>();

    [ForeignKey("StatusID")]
    [InverseProperty("PartyTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [InverseProperty("Party")]
    public virtual ICollection<SupplierAccountDetailTable> SupplierAccountDetailTable { get; set; } = new List<SupplierAccountDetailTable>();

    [InverseProperty("Vendor")]
    public virtual ICollection<TransactionTable> TransactionTable { get; set; } = new List<TransactionTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public PartyTable()
    {

    }

	/// <summary>
    /// Default constructor for PartyTable
    /// </summary>
    /// <param name="db"></param>
	public PartyTable(AMSContext _db)
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
		return "PartyID";
	}

	public override object GetPrimaryKeyValue()
	{
		return PartyID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.PartyCode)) this.PartyCode = this.PartyCode.Trim();
		if(!string.IsNullOrEmpty(this.TradeName)) this.TradeName = this.TradeName.Trim();
		if(!string.IsNullOrEmpty(this.ContactPerson)) this.ContactPerson = this.ContactPerson.Trim();
		if(!string.IsNullOrEmpty(this.ContactPersonMobile)) this.ContactPersonMobile = this.ContactPersonMobile.Trim();
		if(!string.IsNullOrEmpty(this.Email)) this.Email = this.Email.Trim();
		if(!string.IsNullOrEmpty(this.ContactPersonEmail)) this.ContactPersonEmail = this.ContactPersonEmail.Trim();
		if(!string.IsNullOrEmpty(this.Telephone1)) this.Telephone1 = this.Telephone1.Trim();
		if(!string.IsNullOrEmpty(this.Telephone2)) this.Telephone2 = this.Telephone2.Trim();
		if(!string.IsNullOrEmpty(this.NotificationEmail)) this.NotificationEmail = this.NotificationEmail.Trim();
		if(!string.IsNullOrEmpty(this.Mobile)) this.Mobile = this.Mobile.Trim();
		if(!string.IsNullOrEmpty(this.Fax)) this.Fax = this.Fax.Trim();
		if(!string.IsNullOrEmpty(this.TRNNo)) this.TRNNo = this.TRNNo.Trim();
		if(!string.IsNullOrEmpty(this.Remark)) this.Remark = this.Remark.Trim();
		if(!string.IsNullOrEmpty(this.TaxFileNo)) this.TaxFileNo = this.TaxFileNo.Trim();
		if(!string.IsNullOrEmpty(this.CorporateRegistartionNo)) this.CorporateRegistartionNo = this.CorporateRegistartionNo.Trim();
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
		if(!string.IsNullOrEmpty(this.PartyName)) this.PartyName = this.PartyName.Trim();

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

    public static PartyTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.PartyTable
                where b.PartyID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<PartyTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.PartyTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.PartyID descending
                    select b);
        }
        else
        {
            return (from b in _db.PartyTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.PartyID descending
                    select b);
        }
    }

    public static IQueryable<PartyTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return PartyTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.PartyTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return PartyTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return PartyTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
