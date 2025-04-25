using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class NotificationModuleTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int NotificationModuleID { get; set; }

    [StringLength(200)]
    public string NotificationModule { get; set; } = null!;

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public string? QueryType { get; set; }

    public string? QueryString { get; set; }

    public int? ReportTemplateID { get; set; }

    [InverseProperty("NotificationModule")]
    public virtual ICollection<NotificationFieldTable> NotificationFieldTable { get; set; } = new List<NotificationFieldTable>();

    [InverseProperty("NotificationModule")]
    public virtual ICollection<NotificationModuleFieldTable> NotificationModuleFieldTable { get; set; } = new List<NotificationModuleFieldTable>();

    [InverseProperty("NotificationModule")]
    public virtual ICollection<NotificationTemplateTable> NotificationTemplateTable { get; set; } = new List<NotificationTemplateTable>();

    [ForeignKey("ReportTemplateID")]
    [InverseProperty("NotificationModuleTable")]
    public virtual ReportTemplateTable? ReportTemplate { get; set; }

    [ForeignKey("StatusID")]
    [InverseProperty("NotificationModuleTable")]
    public virtual StatusTable? Status { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public NotificationModuleTable()
    {

    }

	/// <summary>
    /// Default constructor for NotificationModuleTable
    /// </summary>
    /// <param name="db"></param>
	public NotificationModuleTable(AMSContext _db)
	{
		this.StatusID = (byte) StatusValue.Active;
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "NotificationModuleID";
	}

	public override object GetPrimaryKeyValue()
	{
		return NotificationModuleID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.NotificationModule)) this.NotificationModule = this.NotificationModule.Trim();
		if(!string.IsNullOrEmpty(this.QueryType)) this.QueryType = this.QueryType.Trim();
		if(!string.IsNullOrEmpty(this.QueryString)) this.QueryString = this.QueryString.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		this.StatusID = (int) StatusValue.Deleted;
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static NotificationModuleTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.NotificationModuleTable
                where b.NotificationModuleID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<NotificationModuleTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.NotificationModuleTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.NotificationModuleID descending
                    select b);
        }
        else
        {
            return (from b in _db.NotificationModuleTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.NotificationModuleID descending
                    select b);
        }
    }

    public static IQueryable<NotificationModuleTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return NotificationModuleTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.NotificationModuleTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return NotificationModuleTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return NotificationModuleTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
