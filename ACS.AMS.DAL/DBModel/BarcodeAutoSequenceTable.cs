using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class BarcodeAutoSequenceTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int BarcodeAutoID { get; set; }

    public int BarcodeLastValue { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public string? L1CategoryCode { get; set; }

    public string? L2CategoeyCode { get; set; }

    public string? L1LocationCode { get; set; }

    public string? L2LocationCode { get; set; }

    [StringLength(100)]
    public string? BarcodePrefix { get; set; }

    public int? CompanyID { get; set; }

    public string? CompanyCode { get; set; }

    [ForeignKey("StatusID")]
    [InverseProperty("BarcodeAutoSequenceTable")]
    public virtual StatusTable? Status { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public BarcodeAutoSequenceTable()
    {

    }

	/// <summary>
    /// Default constructor for BarcodeAutoSequenceTable
    /// </summary>
    /// <param name="db"></param>
	public BarcodeAutoSequenceTable(AMSContext _db)
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
		return "BarcodeAutoID";
	}

	public override object GetPrimaryKeyValue()
	{
		return BarcodeAutoID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.L1CategoryCode)) this.L1CategoryCode = this.L1CategoryCode.Trim();
		if(!string.IsNullOrEmpty(this.L2CategoeyCode)) this.L2CategoeyCode = this.L2CategoeyCode.Trim();
		if(!string.IsNullOrEmpty(this.L1LocationCode)) this.L1LocationCode = this.L1LocationCode.Trim();
		if(!string.IsNullOrEmpty(this.L2LocationCode)) this.L2LocationCode = this.L2LocationCode.Trim();
		if(!string.IsNullOrEmpty(this.BarcodePrefix)) this.BarcodePrefix = this.BarcodePrefix.Trim();
		if(!string.IsNullOrEmpty(this.CompanyCode)) this.CompanyCode = this.CompanyCode.Trim();

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

    public static BarcodeAutoSequenceTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.BarcodeAutoSequenceTable
                where b.BarcodeAutoID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<BarcodeAutoSequenceTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.BarcodeAutoSequenceTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.BarcodeAutoID descending
                    select b);
        }
        else
        {
            return (from b in _db.BarcodeAutoSequenceTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.BarcodeAutoID descending
                    select b);
        }
    }

    public static IQueryable<BarcodeAutoSequenceTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return BarcodeAutoSequenceTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.BarcodeAutoSequenceTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return BarcodeAutoSequenceTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return BarcodeAutoSequenceTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
