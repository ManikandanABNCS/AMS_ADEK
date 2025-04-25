using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class PurchaseOrderLineItemTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int PurchaseOrderLineItemID { get; set; }

    public int PurchaseOrderID { get; set; }

    public int ProductID { get; set; }

    public int SupplierID { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal Price { get; set; }

    public int? VATID { get; set; }

    [ForeignKey("ProductID")]
    [InverseProperty("PurchaseOrderLineItemTable")]
    public virtual ProductTable? Product { get; set; } = null!;

    [ForeignKey("PurchaseOrderID")]
    [InverseProperty("PurchaseOrderLineItemTable")]
    public virtual PurchaseOrderTable? PurchaseOrder { get; set; } = null!;

    [ForeignKey("VATID")]
    [InverseProperty("PurchaseOrderLineItemTable")]
    public virtual VATTable? VAT { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public PurchaseOrderLineItemTable()
    {

    }

	/// <summary>
    /// Default constructor for PurchaseOrderLineItemTable
    /// </summary>
    /// <param name="db"></param>
	public PurchaseOrderLineItemTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "PurchaseOrderLineItemID";
	}

	public override object GetPrimaryKeyValue()
	{
		return PurchaseOrderLineItemID;
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

    public static PurchaseOrderLineItemTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.PurchaseOrderLineItemTable
                where b.PurchaseOrderLineItemID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<PurchaseOrderLineItemTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.PurchaseOrderLineItemTable select b);
    }

    public static IQueryable<PurchaseOrderLineItemTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return PurchaseOrderLineItemTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.PurchaseOrderLineItemTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return PurchaseOrderLineItemTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return PurchaseOrderLineItemTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
