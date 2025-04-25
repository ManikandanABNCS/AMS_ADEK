using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Keyless]
public partial class LocationForUserMappingView : BaseEntityObject, IACSDBObject
{
    [StringLength(100)]
    public string LocationCode { get; set; } = null!;

    public string LocationName { get; set; } = null!;

    public string ParentLocation { get; set; } = null!;

    public int LocationID { get; set; }

    public string SecondLevelLocationName { get; set; } = null!;

    [StringLength(100)]
    public string? LocationType { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public LocationForUserMappingView()
    {

    }

	/// <summary>
    /// Default constructor for LocationForUserMappingView
    /// </summary>
    /// <param name="db"></param>
	public LocationForUserMappingView(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "LocationCode";
	}

	public override object GetPrimaryKeyValue()
	{
		return LocationCode;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.LocationCode)) this.LocationCode = this.LocationCode.Trim();
		if(!string.IsNullOrEmpty(this.LocationName)) this.LocationName = this.LocationName.Trim();
		if(!string.IsNullOrEmpty(this.ParentLocation)) this.ParentLocation = this.ParentLocation.Trim();
		if(!string.IsNullOrEmpty(this.SecondLevelLocationName)) this.SecondLevelLocationName = this.SecondLevelLocationName.Trim();
		if(!string.IsNullOrEmpty(this.LocationType)) this.LocationType = this.LocationType.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static LocationForUserMappingView GetItem(AMSContext _db, string id)
    {
        return (from b in _db.LocationForUserMappingView
                where b.LocationCode == id
                select b).FirstOrDefault();
    }

    public static IQueryable<LocationForUserMappingView> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.LocationForUserMappingView select b);
    }

    public static IQueryable<LocationForUserMappingView> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return LocationForUserMappingView.GetAllItems(_db, includeInactiveItems);
    }

    public static bool DeleteItem(AMSContext _db, string id)
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
        return (from b in _db.LocationForUserMappingView select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return LocationForUserMappingView.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return LocationForUserMappingView.GetAllUserItems(_db, userID, includeInactiveItems);
    }

    BaseEntityObject IACSDBObject.GetItemByID(AMSContext _db, long itemID)
    {
        return GetItem(_db, itemID+"");
    }

    bool IACSDBObject.DeleteObject()
    {
        this.Delete();

        return true;
    }

    #endregion Interface Methods
}
