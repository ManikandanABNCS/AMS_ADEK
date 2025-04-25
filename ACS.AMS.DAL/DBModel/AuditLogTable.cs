using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class AuditLogTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int AuditLogID { get; set; }

    public int? ActionType { get; set; }

    [StringLength(50)]
    public string? EntityName { get; set; }

    public string? AuditedObjectKeyValue1 { get; set; }

    public string? AuditedObjectKeyValue2 { get; set; }

    public int? AuditLogTransactionID { get; set; }

    [InverseProperty("AuditLog")]
    public virtual ICollection<AuditLogLineItemTable> AuditLogLineItemTable { get; set; } = new List<AuditLogLineItemTable>();

    [ForeignKey("AuditLogTransactionID")]
    [InverseProperty("AuditLogTable")]
    public virtual AuditLogTransactionTable? AuditLogTransaction { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public AuditLogTable()
    {

    }

	/// <summary>
    /// Default constructor for AuditLogTable
    /// </summary>
    /// <param name="db"></param>
	public AuditLogTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "AuditLogID";
	}

	public override object GetPrimaryKeyValue()
	{
		return AuditLogID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.EntityName)) this.EntityName = this.EntityName.Trim();
		if(!string.IsNullOrEmpty(this.AuditedObjectKeyValue1)) this.AuditedObjectKeyValue1 = this.AuditedObjectKeyValue1.Trim();
		if(!string.IsNullOrEmpty(this.AuditedObjectKeyValue2)) this.AuditedObjectKeyValue2 = this.AuditedObjectKeyValue2.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static AuditLogTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.AuditLogTable
                where b.AuditLogID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<AuditLogTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.AuditLogTable select b);
    }

    public static IQueryable<AuditLogTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return AuditLogTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.AuditLogTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return AuditLogTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return AuditLogTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
