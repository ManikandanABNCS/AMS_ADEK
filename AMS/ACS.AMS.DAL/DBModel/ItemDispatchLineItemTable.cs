using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ItemDispatchLineItemTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ItemDispatchLineItemID { get; set; }

    public int ItemDispatchID { get; set; }

    public int ProductID { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal Price { get; set; }

    public int? VATID { get; set; }

    public string? Remarks { get; set; }

    [ForeignKey("ItemDispatchID")]
    [InverseProperty("ItemDispatchLineItemTable")]
    public virtual ItemDispatchTable? ItemDispatch { get; set; } = null!;

    [ForeignKey("ProductID")]
    [InverseProperty("ItemDispatchLineItemTable")]
    public virtual ProductTable? Product { get; set; } = null!;

    [ForeignKey("VATID")]
    [InverseProperty("ItemDispatchLineItemTable")]
    public virtual VATTable? VAT { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ItemDispatchLineItemTable()
    {

    }

	/// <summary>
    /// Default constructor for ItemDispatchLineItemTable
    /// </summary>
    /// <param name="db"></param>
	public ItemDispatchLineItemTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "ItemDispatchLineItemID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ItemDispatchLineItemID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.Remarks)) this.Remarks = this.Remarks.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static ItemDispatchLineItemTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ItemDispatchLineItemTable
                where b.ItemDispatchLineItemID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ItemDispatchLineItemTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.ItemDispatchLineItemTable select b);
    }

    public static IQueryable<ItemDispatchLineItemTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ItemDispatchLineItemTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ItemDispatchLineItemTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ItemDispatchLineItemTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ItemDispatchLineItemTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
