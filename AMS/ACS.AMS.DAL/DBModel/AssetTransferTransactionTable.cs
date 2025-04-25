using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class AssetTransferTransactionTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int AssetTransferTransactionID { get; set; }

    [StringLength(100)]
    public string TransferReferenceNo { get; set; } = null!;

    public int LocationID { get; set; }

    public string? Remarks { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    [InverseProperty("AssetTransferTransaction")]
    public virtual ICollection<AssetTransferTransactionLineItemTable> AssetTransferTransactionLineItemTable { get; set; } = new List<AssetTransferTransactionLineItemTable>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("AssetTransferTransactionTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("AssetTransferTransactionTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [ForeignKey("LocationID")]
    [InverseProperty("AssetTransferTransactionTable")]
    public virtual LocationTable? Location { get; set; } = null!;

    [ForeignKey("StatusID")]
    [InverseProperty("AssetTransferTransactionTable")]
    public virtual StatusTable? Status { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public AssetTransferTransactionTable()
    {

    }

	/// <summary>
    /// Default constructor for AssetTransferTransactionTable
    /// </summary>
    /// <param name="db"></param>
	public AssetTransferTransactionTable(AMSContext _db)
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
		return "AssetTransferTransactionID";
	}

	public override object GetPrimaryKeyValue()
	{
		return AssetTransferTransactionID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.TransferReferenceNo)) this.TransferReferenceNo = this.TransferReferenceNo.Trim();
		if(!string.IsNullOrEmpty(this.Remarks)) this.Remarks = this.Remarks.Trim();

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

    public static AssetTransferTransactionTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.AssetTransferTransactionTable
                where b.AssetTransferTransactionID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<AssetTransferTransactionTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.AssetTransferTransactionTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.AssetTransferTransactionID descending
                    select b);
        }
        else
        {
            return (from b in _db.AssetTransferTransactionTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.AssetTransferTransactionID descending
                    select b);
        }
    }

    public static IQueryable<AssetTransferTransactionTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return AssetTransferTransactionTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.AssetTransferTransactionTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return AssetTransferTransactionTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return AssetTransferTransactionTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
