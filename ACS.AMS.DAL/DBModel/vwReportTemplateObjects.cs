using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Keyless]
public partial class vwReportTemplateObjects : BaseEntityObject, IACSDBObject
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
    public vwReportTemplateObjects()
    {

    }

	/// <summary>
    /// Default constructor for vwReportTemplateObjects
    /// </summary>
    /// <param name="db"></param>
	public vwReportTemplateObjects(AMSContext _db)
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

    public static vwReportTemplateObjects GetItem(AMSContext _db, int id)
    {
        return (from b in _db.vwReportTemplateObjects
                where b.ObjectID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<vwReportTemplateObjects> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.vwReportTemplateObjects select b);
    }

    public static IQueryable<vwReportTemplateObjects> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return vwReportTemplateObjects.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.vwReportTemplateObjects select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return vwReportTemplateObjects.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return vwReportTemplateObjects.GetAllUserItems(_db, userID, includeInactiveItems);
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
