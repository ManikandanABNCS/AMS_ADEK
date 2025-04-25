using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ItemSupplierMappingTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ItemSupplierMappingID { get; set; }

    public int ProductID { get; set; }

    public int ItemTypeID { get; set; }

    public int PartyID { get; set; }

    [ForeignKey("ItemTypeID")]
    [InverseProperty("ItemSupplierMappingTable")]
    public virtual ItemTypeTable? ItemType { get; set; } = null!;

    [ForeignKey("PartyID")]
    [InverseProperty("ItemSupplierMappingTable")]
    public virtual PartyTable? Party { get; set; } = null!;

    [ForeignKey("ProductID")]
    [InverseProperty("ItemSupplierMappingTable")]
    public virtual ProductTable? Product { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ItemSupplierMappingTable()
    {

    }

	/// <summary>
    /// Default constructor for ItemSupplierMappingTable
    /// </summary>
    /// <param name="db"></param>
	public ItemSupplierMappingTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "ItemSupplierMappingID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ItemSupplierMappingID;
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

    public static ItemSupplierMappingTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ItemSupplierMappingTable
                where b.ItemSupplierMappingID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ItemSupplierMappingTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.ItemSupplierMappingTable select b);
    }

    public static IQueryable<ItemSupplierMappingTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ItemSupplierMappingTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ItemSupplierMappingTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ItemSupplierMappingTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ItemSupplierMappingTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
