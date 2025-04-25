using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Index("CurrencyCode", Name = "UC_CurrencyCode", IsUnique = true)]
public partial class CurrencyTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int CurrencyID { get; set; }

    public bool IsDefaultCurrency { get; set; }

    [StringLength(100)]
    public string CurrencyCode { get; set; } = null!;

    public int CountryID { get; set; }

    public int NoOfDecimalDigits { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    [ForeignKey("CountryID")]
    [InverseProperty("CurrencyTable")]
    public virtual CountryTable? Country { get; set; } = null!;

    [ForeignKey("CreatedBy")]
    [InverseProperty("CurrencyTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [InverseProperty("FromCurrency")]
    public virtual ICollection<CurrencyConversionTable> CurrencyConversionTableFromCurrency { get; set; } = new List<CurrencyConversionTable>();

    [InverseProperty("ToCurrency")]
    public virtual ICollection<CurrencyConversionTable> CurrencyConversionTableToCurrency { get; set; } = new List<CurrencyConversionTable>();

    [InverseProperty("Currency")]
    public virtual ICollection<CurrencyDescriptionTable> CurrencyDescriptionTable { get; set; } = new List<CurrencyDescriptionTable>();

    [InverseProperty("Currency")]
    public virtual ICollection<ExchangeRateTable> ExchangeRateTableCurrency { get; set; } = new List<ExchangeRateTable>();

    [InverseProperty("ToCurrency")]
    public virtual ICollection<ExchangeRateTable> ExchangeRateTableToCurrency { get; set; } = new List<ExchangeRateTable>();

    [InverseProperty("Currency")]
    public virtual ICollection<GRNTable> GRNTable { get; set; } = new List<GRNTable>();

    [InverseProperty("Currency")]
    public virtual ICollection<InvoiceTable> InvoiceTable { get; set; } = new List<InvoiceTable>();

    [InverseProperty("Currency")]
    public virtual ICollection<ItemDispatchTable> ItemDispatchTable { get; set; } = new List<ItemDispatchTable>();

    [InverseProperty("Currency")]
    public virtual ICollection<ItemReqestTable> ItemReqestTable { get; set; } = new List<ItemReqestTable>();

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("CurrencyTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [InverseProperty("Currency")]
    public virtual ICollection<RequestQuotationTable> RequestQuotationTable { get; set; } = new List<RequestQuotationTable>();

    [ForeignKey("StatusID")]
    [InverseProperty("CurrencyTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [InverseProperty("AccountCurrency")]
    public virtual ICollection<SupplierAccountDetailTable> SupplierAccountDetailTable { get; set; } = new List<SupplierAccountDetailTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public CurrencyTable()
    {

    }

	/// <summary>
    /// Default constructor for CurrencyTable
    /// </summary>
    /// <param name="db"></param>
	public CurrencyTable(AMSContext _db)
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
		return "CurrencyID";
	}

	public override object GetPrimaryKeyValue()
	{
		return CurrencyID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.CurrencyCode)) this.CurrencyCode = this.CurrencyCode.Trim();

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

    public static CurrencyTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.CurrencyTable
                where b.CurrencyID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<CurrencyTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.CurrencyTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.CurrencyID descending
                    select b);
        }
        else
        {
            return (from b in _db.CurrencyTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.CurrencyID descending
                    select b);
        }
    }

    public static IQueryable<CurrencyTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return CurrencyTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.CurrencyTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return CurrencyTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return CurrencyTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
