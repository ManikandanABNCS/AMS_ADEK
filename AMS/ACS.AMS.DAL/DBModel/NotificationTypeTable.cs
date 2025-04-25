using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class NotificationTypeTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int NotificationTypeID { get; set; }

    [StringLength(50)]
    public string NotificationType { get; set; } = null!;

    public bool? AllowHTMLContent { get; set; }

    public bool? IsAttachmentRequired { get; set; }

    [InverseProperty("NotificationType")]
    public virtual ICollection<NotificationTemplateNotificationTypeTable> NotificationTemplateNotificationTypeTable { get; set; } = new List<NotificationTemplateNotificationTypeTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public NotificationTypeTable()
    {

    }

	/// <summary>
    /// Default constructor for NotificationTypeTable
    /// </summary>
    /// <param name="db"></param>
	public NotificationTypeTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "NotificationTypeID";
	}

	public override object GetPrimaryKeyValue()
	{
		return NotificationTypeID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.NotificationType)) this.NotificationType = this.NotificationType.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static NotificationTypeTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.NotificationTypeTable
                where b.NotificationTypeID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<NotificationTypeTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.NotificationTypeTable select b);
    }

    public static IQueryable<NotificationTypeTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return NotificationTypeTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.NotificationTypeTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return NotificationTypeTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return NotificationTypeTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
