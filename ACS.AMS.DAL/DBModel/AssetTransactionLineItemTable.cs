using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class AssetTransactionLineItemTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int AssetTransactionLineItemID { get; set; }

    public int AssetTransactionID { get; set; }

    public int AssetID { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string Remarks { get; set; } = null!;

    public int AdjustmentValue { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    public bool IsAdjustmentNetBook { get; set; }

    [ForeignKey("AssetID")]
    [InverseProperty("AssetTransactionLineItemTable")]
    public virtual AssetTable? Asset { get; set; } = null!;

    [ForeignKey("AssetTransactionID")]
    [InverseProperty("AssetTransactionLineItemTable")]
    public virtual AssetTransactionTable? AssetTransaction { get; set; } = null!;

    [ForeignKey("CreatedBy")]
    [InverseProperty("AssetTransactionLineItemTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("AssetTransactionLineItemTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [ForeignKey("StatusID")]
    [InverseProperty("AssetTransactionLineItemTable")]
    public virtual StatusTable? Status { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public AssetTransactionLineItemTable()
    {

    }

	/// <summary>
    /// Default constructor for AssetTransactionLineItemTable
    /// </summary>
    /// <param name="db"></param>
	public AssetTransactionLineItemTable(AMSContext _db)
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
		return "AssetTransactionLineItemID";
	}

	public override object GetPrimaryKeyValue()
	{
		return AssetTransactionLineItemID;
	}

	internal override void BeforeSave(AMSContext db)
    {
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

    public static AssetTransactionLineItemTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.AssetTransactionLineItemTable
                where b.AssetTransactionLineItemID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<AssetTransactionLineItemTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.AssetTransactionLineItemTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.AssetTransactionLineItemID descending
                    select b);
        }
        else
        {
            return (from b in _db.AssetTransactionLineItemTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.AssetTransactionLineItemID descending
                    select b);
        }
    }

    public static IQueryable<AssetTransactionLineItemTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return AssetTransactionLineItemTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.AssetTransactionLineItemTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return AssetTransactionLineItemTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return AssetTransactionLineItemTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
