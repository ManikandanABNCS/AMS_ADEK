using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class AuditLogTransactionTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int AuditLogTransactionID { get; set; }

    public DateTime AuditLogTransactionDateTime { get; set; }

    public int UserID { get; set; }

    public string? URL { get; set; }

    public string? SessionID { get; set; }

    [InverseProperty("AuditLogTransaction")]
    public virtual ICollection<AuditLogTable> AuditLogTable { get; set; } = new List<AuditLogTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public AuditLogTransactionTable()
    {

    }

	/// <summary>
    /// Default constructor for AuditLogTransactionTable
    /// </summary>
    /// <param name="db"></param>
	public AuditLogTransactionTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "AuditLogTransactionID";
	}

	public override object GetPrimaryKeyValue()
	{
		return AuditLogTransactionID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.URL)) this.URL = this.URL.Trim();
		if(!string.IsNullOrEmpty(this.SessionID)) this.SessionID = this.SessionID.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static AuditLogTransactionTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.AuditLogTransactionTable
                where b.AuditLogTransactionID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<AuditLogTransactionTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.AuditLogTransactionTable select b);
    }

    public static IQueryable<AuditLogTransactionTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return AuditLogTransactionTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.AuditLogTransactionTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return AuditLogTransactionTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return AuditLogTransactionTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
