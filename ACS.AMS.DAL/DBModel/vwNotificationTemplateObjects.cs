using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Keyless]
public partial class vwNotificationTemplateObjects : BaseEntityObject, IACSDBObject
{
    public int ObjectID { get; set; }

    [StringLength(128)]
    public string ObjectName { get; set; } = null!;

    [StringLength(9)]
    [Unicode(false)]
    public string ObjectType { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public vwNotificationTemplateObjects()
    {

    }

	/// <summary>
    /// Default constructor for vwNotificationTemplateObjects
    /// </summary>
    /// <param name="db"></param>
	public vwNotificationTemplateObjects(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "ObjectID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ObjectID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.ObjectName)) this.ObjectName = this.ObjectName.Trim();
		if(!string.IsNullOrEmpty(this.ObjectType)) this.ObjectType = this.ObjectType.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static vwNotificationTemplateObjects GetItem(AMSContext _db, int id)
    {
        return (from b in _db.vwNotificationTemplateObjects
                where b.ObjectID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<vwNotificationTemplateObjects> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.vwNotificationTemplateObjects select b);
    }

    public static IQueryable<vwNotificationTemplateObjects> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return vwNotificationTemplateObjects.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.vwNotificationTemplateObjects select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return vwNotificationTemplateObjects.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return vwNotificationTemplateObjects.GetAllUserItems(_db, userID, includeInactiveItems);
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
