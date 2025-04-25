using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class InvoiceLineItemTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int InvoiceLineItemID { get; set; }

    public int InvoiceID { get; set; }

    public int ProductID { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal Price { get; set; }

    public int? VATID { get; set; }

    public string? Remarks { get; set; }

    [ForeignKey("InvoiceID")]
    [InverseProperty("InvoiceLineItemTable")]
    public virtual InvoiceTable? Invoice { get; set; } = null!;

    [ForeignKey("ProductID")]
    [InverseProperty("InvoiceLineItemTable")]
    public virtual ProductTable? Product { get; set; } = null!;

    [ForeignKey("VATID")]
    [InverseProperty("InvoiceLineItemTable")]
    public virtual VATTable? VAT { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public InvoiceLineItemTable()
    {

    }

	/// <summary>
    /// Default constructor for InvoiceLineItemTable
    /// </summary>
    /// <param name="db"></param>
	public InvoiceLineItemTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "InvoiceLineItemID";
	}

	public override object GetPrimaryKeyValue()
	{
		return InvoiceLineItemID;
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

    public static InvoiceLineItemTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.InvoiceLineItemTable
                where b.InvoiceLineItemID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<InvoiceLineItemTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.InvoiceLineItemTable select b);
    }

    public static IQueryable<InvoiceLineItemTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return InvoiceLineItemTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.InvoiceLineItemTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return InvoiceLineItemTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return InvoiceLineItemTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
