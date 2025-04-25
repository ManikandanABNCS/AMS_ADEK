using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class AssetTransferTransactionLineItemTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int AssetTransferTransactionLineItemID { get; set; }

    public int AssetTransferTransactionID { get; set; }

    public int AssetID { get; set; }

    [ForeignKey("AssetID")]
    [InverseProperty("AssetTransferTransactionLineItemTable")]
    public virtual AssetTable? Asset { get; set; } = null!;

    [ForeignKey("AssetTransferTransactionID")]
    [InverseProperty("AssetTransferTransactionLineItemTable")]
    public virtual AssetTransferTransactionTable? AssetTransferTransaction { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public AssetTransferTransactionLineItemTable()
    {

    }

	/// <summary>
    /// Default constructor for AssetTransferTransactionLineItemTable
    /// </summary>
    /// <param name="db"></param>
	public AssetTransferTransactionLineItemTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "AssetTransferTransactionLineItemID";
	}

	public override object GetPrimaryKeyValue()
	{
		return AssetTransferTransactionLineItemID;
	}

	internal override void BeforeSave(AMSContext db)
    {

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static AssetTransferTransactionLineItemTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.AssetTransferTransactionLineItemTable
                where b.AssetTransferTransactionLineItemID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<AssetTransferTransactionLineItemTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.AssetTransferTransactionLineItemTable select b);
    }

    public static IQueryable<AssetTransferTransactionLineItemTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return AssetTransferTransactionLineItemTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.AssetTransferTransactionLineItemTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return AssetTransferTransactionLineItemTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return AssetTransferTransactionLineItemTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
