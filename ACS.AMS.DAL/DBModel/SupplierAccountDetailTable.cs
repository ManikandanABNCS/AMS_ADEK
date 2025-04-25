using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Index("AccountNumber", Name = "uc_AccountNo", IsUnique = true)]
[Index("IBAN", Name = "uc_IBAN", IsUnique = true)]
[Index("SwiftCode", Name = "uc_SwiftCode", IsUnique = true)]
[Index("WireNumber", Name = "uc_WireNumber", IsUnique = true)]
public partial class SupplierAccountDetailTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int SupplierAccountDetailID { get; set; }

    public int PartyID { get; set; }

    public int PaymentTypeID { get; set; }

    public int AccountCurrencyID { get; set; }

    public bool? IsDefaultAccount { get; set; }

    [StringLength(200)]
    public string BeneficiaryName { get; set; } = null!;

    [StringLength(200)]
    public string AccountNumber { get; set; } = null!;

    [StringLength(200)]
    public string SwiftCode { get; set; } = null!;

    [StringLength(200)]
    public string IBAN { get; set; } = null!;

    [StringLength(200)]
    public string WireNumber { get; set; } = null!;

    [StringLength(200)]
    public string SortCode { get; set; } = null!;

    [StringLength(500)]
    public string BankName { get; set; } = null!;

    public int CountryID { get; set; }

    public string Address1 { get; set; } = null!;

    public string Address2 { get; set; } = null!;

    [StringLength(200)]
    public string City { get; set; } = null!;

    [StringLength(200)]
    public string PostCode { get; set; } = null!;

    [ForeignKey("AccountCurrencyID")]
    [InverseProperty("SupplierAccountDetailTable")]
    public virtual CurrencyTable? AccountCurrency { get; set; } = null!;

    [ForeignKey("CountryID")]
    [InverseProperty("SupplierAccountDetailTable")]
    public virtual CountryTable? Country { get; set; } = null!;

    [InverseProperty("SupplierAccountDetail")]
    public virtual ICollection<InvoiceTable> InvoiceTable { get; set; } = new List<InvoiceTable>();

    [ForeignKey("PartyID")]
    [InverseProperty("SupplierAccountDetailTable")]
    public virtual PartyTable? Party { get; set; } = null!;

    [ForeignKey("PaymentTypeID")]
    [InverseProperty("SupplierAccountDetailTable")]
    public virtual PaymentTypeTable? PaymentType { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public SupplierAccountDetailTable()
    {

    }

	/// <summary>
    /// Default constructor for SupplierAccountDetailTable
    /// </summary>
    /// <param name="db"></param>
	public SupplierAccountDetailTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "SupplierAccountDetailID";
	}

	public override object GetPrimaryKeyValue()
	{
		return SupplierAccountDetailID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.BeneficiaryName)) this.BeneficiaryName = this.BeneficiaryName.Trim();
		if(!string.IsNullOrEmpty(this.AccountNumber)) this.AccountNumber = this.AccountNumber.Trim();
		if(!string.IsNullOrEmpty(this.SwiftCode)) this.SwiftCode = this.SwiftCode.Trim();
		if(!string.IsNullOrEmpty(this.IBAN)) this.IBAN = this.IBAN.Trim();
		if(!string.IsNullOrEmpty(this.WireNumber)) this.WireNumber = this.WireNumber.Trim();
		if(!string.IsNullOrEmpty(this.SortCode)) this.SortCode = this.SortCode.Trim();
		if(!string.IsNullOrEmpty(this.BankName)) this.BankName = this.BankName.Trim();
		if(!string.IsNullOrEmpty(this.Address1)) this.Address1 = this.Address1.Trim();
		if(!string.IsNullOrEmpty(this.Address2)) this.Address2 = this.Address2.Trim();
		if(!string.IsNullOrEmpty(this.City)) this.City = this.City.Trim();
		if(!string.IsNullOrEmpty(this.PostCode)) this.PostCode = this.PostCode.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static SupplierAccountDetailTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.SupplierAccountDetailTable
                where b.SupplierAccountDetailID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<SupplierAccountDetailTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.SupplierAccountDetailTable select b);
    }

    public static IQueryable<SupplierAccountDetailTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return SupplierAccountDetailTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.SupplierAccountDetailTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return SupplierAccountDetailTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return SupplierAccountDetailTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
