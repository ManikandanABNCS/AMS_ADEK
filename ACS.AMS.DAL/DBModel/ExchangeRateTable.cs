using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ExchangeRateTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ExchangeRateID { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime EffectiveDate { get; set; }

    public int CurrencyID { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal ExchangeRate { get; set; }

    public int ToCurrencyID { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ExchangeRateTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("CurrencyID")]
    [InverseProperty("ExchangeRateTableCurrency")]
    public virtual CurrencyTable? Currency { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("ExchangeRateTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [ForeignKey("StatusID")]
    [InverseProperty("ExchangeRateTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [ForeignKey("ToCurrencyID")]
    [InverseProperty("ExchangeRateTableToCurrency")]
    public virtual CurrencyTable? ToCurrency { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ExchangeRateTable()
    {

    }

	/// <summary>
    /// Default constructor for ExchangeRateTable
    /// </summary>
    /// <param name="db"></param>
	public ExchangeRateTable(AMSContext _db)
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
		return "ExchangeRateID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ExchangeRateID;
	}

	internal override void BeforeSave(AMSContext db)
    {

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

    public static ExchangeRateTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ExchangeRateTable
                where b.ExchangeRateID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ExchangeRateTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.ExchangeRateTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.ExchangeRateID descending
                    select b);
        }
        else
        {
            return (from b in _db.ExchangeRateTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.ExchangeRateID descending
                    select b);
        }
    }

    public static IQueryable<ExchangeRateTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ExchangeRateTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ExchangeRateTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ExchangeRateTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ExchangeRateTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
