using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ItemReqestLineItemTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ItemReqestLineItemID { get; set; }

    public int ItemRequestID { get; set; }

    public int ProductID { get; set; }

    public int UOMID { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal UnitPrice { get; set; }

    public int? VATID { get; set; }

    [ForeignKey("ItemRequestID")]
    [InverseProperty("ItemReqestLineItemTable")]
    public virtual ItemReqestTable? ItemRequest { get; set; } = null!;

    [ForeignKey("ProductID")]
    [InverseProperty("ItemReqestLineItemTable")]
    public virtual ProductTable? Product { get; set; } = null!;

    [ForeignKey("UOMID")]
    [InverseProperty("ItemReqestLineItemTable")]
    public virtual UOMTable? UOM { get; set; } = null!;

    [ForeignKey("VATID")]
    [InverseProperty("ItemReqestLineItemTable")]
    public virtual VATTable? VAT { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ItemReqestLineItemTable()
    {

    }

	/// <summary>
    /// Default constructor for ItemReqestLineItemTable
    /// </summary>
    /// <param name="db"></param>
	public ItemReqestLineItemTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "ItemReqestLineItemID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ItemReqestLineItemID;
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

    public static ItemReqestLineItemTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ItemReqestLineItemTable
                where b.ItemReqestLineItemID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ItemReqestLineItemTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.ItemReqestLineItemTable select b);
    }

    public static IQueryable<ItemReqestLineItemTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ItemReqestLineItemTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ItemReqestLineItemTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ItemReqestLineItemTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ItemReqestLineItemTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
