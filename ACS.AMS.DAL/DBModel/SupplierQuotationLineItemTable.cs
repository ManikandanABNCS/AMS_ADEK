using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class SupplierQuotationLineItemTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int SupplierQuotationLineItemID { get; set; }

    public int SupplierQuotationID { get; set; }

    public int ProductID { get; set; }

    public int QuotedQuantity { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? DiscountAmount { get; set; }

    public string? Remarks { get; set; }

    public int? VATID { get; set; }

    [ForeignKey("ProductID")]
    [InverseProperty("SupplierQuotationLineItemTable")]
    public virtual ProductTable? Product { get; set; } = null!;

    [ForeignKey("SupplierQuotationID")]
    [InverseProperty("SupplierQuotationLineItemTable")]
    public virtual SupplierQuotationTable? SupplierQuotation { get; set; } = null!;

    [ForeignKey("VATID")]
    [InverseProperty("SupplierQuotationLineItemTable")]
    public virtual VATTable? VAT { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public SupplierQuotationLineItemTable()
    {

    }

	/// <summary>
    /// Default constructor for SupplierQuotationLineItemTable
    /// </summary>
    /// <param name="db"></param>
	public SupplierQuotationLineItemTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "SupplierQuotationLineItemID";
	}

	public override object GetPrimaryKeyValue()
	{
		return SupplierQuotationLineItemID;
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

    public static SupplierQuotationLineItemTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.SupplierQuotationLineItemTable
                where b.SupplierQuotationLineItemID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<SupplierQuotationLineItemTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.SupplierQuotationLineItemTable select b);
    }

    public static IQueryable<SupplierQuotationLineItemTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return SupplierQuotationLineItemTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.SupplierQuotationLineItemTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return SupplierQuotationLineItemTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return SupplierQuotationLineItemTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
