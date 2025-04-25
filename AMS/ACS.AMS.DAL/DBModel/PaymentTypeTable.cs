using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class PaymentTypeTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int PaymentTypeID { get; set; }

    public string PaymentType { get; set; } = null!;

    [InverseProperty("PaymentType")]
    public virtual ICollection<InvoiceTable> InvoiceTable { get; set; } = new List<InvoiceTable>();

    [InverseProperty("PaymentType")]
    public virtual ICollection<SupplierAccountDetailTable> SupplierAccountDetailTable { get; set; } = new List<SupplierAccountDetailTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public PaymentTypeTable()
    {

    }

	/// <summary>
    /// Default constructor for PaymentTypeTable
    /// </summary>
    /// <param name="db"></param>
	public PaymentTypeTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "PaymentTypeID";
	}

	public override object GetPrimaryKeyValue()
	{
		return PaymentTypeID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.PaymentType)) this.PaymentType = this.PaymentType.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static PaymentTypeTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.PaymentTypeTable
                where b.PaymentTypeID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<PaymentTypeTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.PaymentTypeTable select b);
    }

    public static IQueryable<PaymentTypeTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return PaymentTypeTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.PaymentTypeTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return PaymentTypeTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return PaymentTypeTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
