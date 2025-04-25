using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class PurchaseTypeTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int PurchaseTypeID { get; set; }

    public string PurchaseType { get; set; } = null!;

    [InverseProperty("PurchaseType")]
    public virtual ICollection<PurchaseOrderTable> PurchaseOrderTable { get; set; } = new List<PurchaseOrderTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public PurchaseTypeTable()
    {

    }

	/// <summary>
    /// Default constructor for PurchaseTypeTable
    /// </summary>
    /// <param name="db"></param>
	public PurchaseTypeTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "PurchaseTypeID";
	}

	public override object GetPrimaryKeyValue()
	{
		return PurchaseTypeID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.PurchaseType)) this.PurchaseType = this.PurchaseType.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static PurchaseTypeTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.PurchaseTypeTable
                where b.PurchaseTypeID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<PurchaseTypeTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.PurchaseTypeTable select b);
    }

    public static IQueryable<PurchaseTypeTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return PurchaseTypeTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.PurchaseTypeTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return PurchaseTypeTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return PurchaseTypeTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
