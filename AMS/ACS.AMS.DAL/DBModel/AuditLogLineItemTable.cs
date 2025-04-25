using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class AuditLogLineItemTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int AuditLogLineItemID { get; set; }

    public int AuditLogID { get; set; }

    [StringLength(50)]
    public string FieldName { get; set; } = null!;

    [StringLength(4000)]
    public string? OldValue { get; set; }

    [StringLength(4000)]
    public string? NewValue { get; set; }

    [ForeignKey("AuditLogID")]
    [InverseProperty("AuditLogLineItemTable")]
    public virtual AuditLogTable? AuditLog { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public AuditLogLineItemTable()
    {

    }

	/// <summary>
    /// Default constructor for AuditLogLineItemTable
    /// </summary>
    /// <param name="db"></param>
	public AuditLogLineItemTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "AuditLogLineItemID";
	}

	public override object GetPrimaryKeyValue()
	{
		return AuditLogLineItemID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.FieldName)) this.FieldName = this.FieldName.Trim();
		if(!string.IsNullOrEmpty(this.OldValue)) this.OldValue = this.OldValue.Trim();
		if(!string.IsNullOrEmpty(this.NewValue)) this.NewValue = this.NewValue.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static AuditLogLineItemTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.AuditLogLineItemTable
                where b.AuditLogLineItemID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<AuditLogLineItemTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.AuditLogLineItemTable select b);
    }

    public static IQueryable<AuditLogLineItemTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return AuditLogLineItemTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.AuditLogLineItemTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return AuditLogLineItemTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return AuditLogLineItemTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
