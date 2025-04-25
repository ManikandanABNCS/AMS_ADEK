using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class CurrencyConversionTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int CurrencyConversionID { get; set; }

    public int FromCurrencyID { get; set; }

    public int ToCurrencyID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CurrencyStartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CurrencyEndDate { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal ConversionValue { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("CurrencyConversionTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("FromCurrencyID")]
    [InverseProperty("CurrencyConversionTableFromCurrency")]
    public virtual CurrencyTable? FromCurrency { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("CurrencyConversionTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [ForeignKey("StatusID")]
    [InverseProperty("CurrencyConversionTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [ForeignKey("ToCurrencyID")]
    [InverseProperty("CurrencyConversionTableToCurrency")]
    public virtual CurrencyTable? ToCurrency { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public CurrencyConversionTable()
    {

    }

	/// <summary>
    /// Default constructor for CurrencyConversionTable
    /// </summary>
    /// <param name="db"></param>
	public CurrencyConversionTable(AMSContext _db)
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
		return "CurrencyConversionID";
	}

	public override object GetPrimaryKeyValue()
	{
		return CurrencyConversionID;
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

    public static CurrencyConversionTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.CurrencyConversionTable
                where b.CurrencyConversionID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<CurrencyConversionTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.CurrencyConversionTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.CurrencyConversionID descending
                    select b);
        }
        else
        {
            return (from b in _db.CurrencyConversionTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.CurrencyConversionID descending
                    select b);
        }
    }

    public static IQueryable<CurrencyConversionTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return CurrencyConversionTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.CurrencyConversionTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return CurrencyConversionTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return CurrencyConversionTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
