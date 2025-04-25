using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class RequestQuotationLineItemTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int RequestQuotationLineItemID { get; set; }

    public int RequestQuotationID { get; set; }

    public int ProductID { get; set; }

    public int Quantity { get; set; }

    [ForeignKey("ProductID")]
    [InverseProperty("RequestQuotationLineItemTable")]
    public virtual ProductTable? Product { get; set; } = null!;

    [ForeignKey("RequestQuotationID")]
    [InverseProperty("RequestQuotationLineItemTable")]
    public virtual RequestQuotationTable? RequestQuotation { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public RequestQuotationLineItemTable()
    {

    }

	/// <summary>
    /// Default constructor for RequestQuotationLineItemTable
    /// </summary>
    /// <param name="db"></param>
	public RequestQuotationLineItemTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "RequestQuotationLineItemID";
	}

	public override object GetPrimaryKeyValue()
	{
		return RequestQuotationLineItemID;
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

    public static RequestQuotationLineItemTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.RequestQuotationLineItemTable
                where b.RequestQuotationLineItemID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<RequestQuotationLineItemTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.RequestQuotationLineItemTable select b);
    }

    public static IQueryable<RequestQuotationLineItemTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return RequestQuotationLineItemTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.RequestQuotationLineItemTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return RequestQuotationLineItemTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return RequestQuotationLineItemTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
