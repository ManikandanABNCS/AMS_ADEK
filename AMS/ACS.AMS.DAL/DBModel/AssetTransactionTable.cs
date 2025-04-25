using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class AssetTransactionTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int AssetTransactionID { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string TransactionNo { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime TransactionDate { get; set; }

    [StringLength(100)]
    public string? ReferenceNo { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    public int? VendorID { get; set; }

    [StringLength(200)]
    public string? ServiceDoneBy { get; set; }

    [InverseProperty("AssetTransaction")]
    public virtual ICollection<AssetTransactionLineItemTable> AssetTransactionLineItemTable { get; set; } = new List<AssetTransactionLineItemTable>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("AssetTransactionTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("AssetTransactionTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [ForeignKey("StatusID")]
    [InverseProperty("AssetTransactionTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [ForeignKey("VendorID")]
    [InverseProperty("AssetTransactionTable")]
    public virtual PartyTable? Vendor { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public AssetTransactionTable()
    {

    }

	/// <summary>
    /// Default constructor for AssetTransactionTable
    /// </summary>
    /// <param name="db"></param>
	public AssetTransactionTable(AMSContext _db)
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
		return "AssetTransactionID";
	}

	public override object GetPrimaryKeyValue()
	{
		return AssetTransactionID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.TransactionNo)) this.TransactionNo = this.TransactionNo.Trim();
		if(!string.IsNullOrEmpty(this.ReferenceNo)) this.ReferenceNo = this.ReferenceNo.Trim();
		if(!string.IsNullOrEmpty(this.Remarks)) this.Remarks = this.Remarks.Trim();
		if(!string.IsNullOrEmpty(this.ServiceDoneBy)) this.ServiceDoneBy = this.ServiceDoneBy.Trim();

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

    public static AssetTransactionTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.AssetTransactionTable
                where b.AssetTransactionID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<AssetTransactionTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.AssetTransactionTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.AssetTransactionID descending
                    select b);
        }
        else
        {
            return (from b in _db.AssetTransactionTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.AssetTransactionID descending
                    select b);
        }
    }

    public static IQueryable<AssetTransactionTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return AssetTransactionTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.AssetTransactionTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return AssetTransactionTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return AssetTransactionTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
