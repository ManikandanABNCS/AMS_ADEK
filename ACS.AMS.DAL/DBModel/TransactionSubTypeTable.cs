using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class TransactionSubTypeTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int TransactionSubTypeID { get; set; }

    [StringLength(100)]
    public string TransactionSubTypeCode { get; set; } = null!;

    [StringLength(500)]
    public string TransactionSubTypeName { get; set; } = null!;

    [InverseProperty("TransactionSubType")]
    public virtual ICollection<TransactionTable> TransactionTable { get; set; } = new List<TransactionTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public TransactionSubTypeTable()
    {

    }

	/// <summary>
    /// Default constructor for TransactionSubTypeTable
    /// </summary>
    /// <param name="db"></param>
	public TransactionSubTypeTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "TransactionSubTypeID";
	}

	public override object GetPrimaryKeyValue()
	{
		return TransactionSubTypeID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.TransactionSubTypeCode)) this.TransactionSubTypeCode = this.TransactionSubTypeCode.Trim();
		if(!string.IsNullOrEmpty(this.TransactionSubTypeName)) this.TransactionSubTypeName = this.TransactionSubTypeName.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static TransactionSubTypeTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.TransactionSubTypeTable
                where b.TransactionSubTypeID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<TransactionSubTypeTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.TransactionSubTypeTable select b);
    }

    public static IQueryable<TransactionSubTypeTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return TransactionSubTypeTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.TransactionSubTypeTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return TransactionSubTypeTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return TransactionSubTypeTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
