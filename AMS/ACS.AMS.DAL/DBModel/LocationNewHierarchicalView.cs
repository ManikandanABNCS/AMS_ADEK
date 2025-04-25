using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Keyless]
public partial class LocationNewHierarchicalView : BaseEntityObject, IACSDBObject
{
    public int? LocationID { get; set; }

    [StringLength(100)]
    public string? LocationCode { get; set; }

    public string? LocationCodeHierarchy { get; set; }

    public string? LocationName { get; set; }

    public string? LocationNameHierarchy { get; set; }

    public int? ParentLocationID { get; set; }

    public int? Level { get; set; }

    [DisplayName("Status")]
    public int? StatusID { get; set; }

    public string? AllLevelIDs { get; set; }

    public int? Level1ID { get; set; }

    public int? Level2ID { get; set; }

    public int? Level3ID { get; set; }

    public int? Level4ID { get; set; }

    public int? Level5ID { get; set; }

    public int? Level6ID { get; set; }

    public string? L1LocationName { get; set; }

    public string? L2LocationName { get; set; }

    public string? L3LocationName { get; set; }

    public string? L4LocationName { get; set; }

    public string? L5LocationName { get; set; }

    public string? L6LocationName { get; set; }

    public int? CompanyID { get; set; }

    [StringLength(200)]
    public string? Attribute1 { get; set; }

    [StringLength(200)]
    public string? Attribute2 { get; set; }

    [StringLength(200)]
    public string? Attribute3 { get; set; }

    [StringLength(200)]
    public string? Attribute4 { get; set; }

    [StringLength(200)]
    public string? Attribute5 { get; set; }

    [StringLength(200)]
    public string? Attribute6 { get; set; }

    [StringLength(200)]
    public string? Attribute7 { get; set; }

    [StringLength(200)]
    public string? Attribute8 { get; set; }

    [StringLength(200)]
    public string? Attribute9 { get; set; }

    [StringLength(200)]
    public string? Attribute10 { get; set; }

    [StringLength(200)]
    public string? Attribute11 { get; set; }

    [StringLength(200)]
    public string? Attribute12 { get; set; }

    [StringLength(200)]
    public string? Attribute13 { get; set; }

    [StringLength(200)]
    public string? Attribute14 { get; set; }

    [StringLength(200)]
    public string? Attribute15 { get; set; }

    [StringLength(200)]
    public string? Attribute16 { get; set; }

    [StringLength(100)]
    public string? LocationType { get; set; }

    public int? LocationTypeID { get; set; }

    public int? MappedLocationID { get; set; }

    [StringLength(100)]
    public string? MappedLocationCode { get; set; }

    public string? MappedLocationName { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public LocationNewHierarchicalView()
    {

    }

	/// <summary>
    /// Default constructor for LocationNewHierarchicalView
    /// </summary>
    /// <param name="db"></param>
	public LocationNewHierarchicalView(AMSContext _db)
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
		return "LocationID";
	}

	public override object GetPrimaryKeyValue()
	{
		return LocationID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.LocationCode)) this.LocationCode = this.LocationCode.Trim();
		if(!string.IsNullOrEmpty(this.LocationCodeHierarchy)) this.LocationCodeHierarchy = this.LocationCodeHierarchy.Trim();
		if(!string.IsNullOrEmpty(this.LocationName)) this.LocationName = this.LocationName.Trim();
		if(!string.IsNullOrEmpty(this.LocationNameHierarchy)) this.LocationNameHierarchy = this.LocationNameHierarchy.Trim();
		if(!string.IsNullOrEmpty(this.AllLevelIDs)) this.AllLevelIDs = this.AllLevelIDs.Trim();
		if(!string.IsNullOrEmpty(this.L1LocationName)) this.L1LocationName = this.L1LocationName.Trim();
		if(!string.IsNullOrEmpty(this.L2LocationName)) this.L2LocationName = this.L2LocationName.Trim();
		if(!string.IsNullOrEmpty(this.L3LocationName)) this.L3LocationName = this.L3LocationName.Trim();
		if(!string.IsNullOrEmpty(this.L4LocationName)) this.L4LocationName = this.L4LocationName.Trim();
		if(!string.IsNullOrEmpty(this.L5LocationName)) this.L5LocationName = this.L5LocationName.Trim();
		if(!string.IsNullOrEmpty(this.L6LocationName)) this.L6LocationName = this.L6LocationName.Trim();
		if(!string.IsNullOrEmpty(this.Attribute1)) this.Attribute1 = this.Attribute1.Trim();
		if(!string.IsNullOrEmpty(this.Attribute2)) this.Attribute2 = this.Attribute2.Trim();
		if(!string.IsNullOrEmpty(this.Attribute3)) this.Attribute3 = this.Attribute3.Trim();
		if(!string.IsNullOrEmpty(this.Attribute4)) this.Attribute4 = this.Attribute4.Trim();
		if(!string.IsNullOrEmpty(this.Attribute5)) this.Attribute5 = this.Attribute5.Trim();
		if(!string.IsNullOrEmpty(this.Attribute6)) this.Attribute6 = this.Attribute6.Trim();
		if(!string.IsNullOrEmpty(this.Attribute7)) this.Attribute7 = this.Attribute7.Trim();
		if(!string.IsNullOrEmpty(this.Attribute8)) this.Attribute8 = this.Attribute8.Trim();
		if(!string.IsNullOrEmpty(this.Attribute9)) this.Attribute9 = this.Attribute9.Trim();
		if(!string.IsNullOrEmpty(this.Attribute10)) this.Attribute10 = this.Attribute10.Trim();
		if(!string.IsNullOrEmpty(this.Attribute11)) this.Attribute11 = this.Attribute11.Trim();
		if(!string.IsNullOrEmpty(this.Attribute12)) this.Attribute12 = this.Attribute12.Trim();
		if(!string.IsNullOrEmpty(this.Attribute13)) this.Attribute13 = this.Attribute13.Trim();
		if(!string.IsNullOrEmpty(this.Attribute14)) this.Attribute14 = this.Attribute14.Trim();
		if(!string.IsNullOrEmpty(this.Attribute15)) this.Attribute15 = this.Attribute15.Trim();
		if(!string.IsNullOrEmpty(this.Attribute16)) this.Attribute16 = this.Attribute16.Trim();
		if(!string.IsNullOrEmpty(this.LocationType)) this.LocationType = this.LocationType.Trim();
		if(!string.IsNullOrEmpty(this.MappedLocationCode)) this.MappedLocationCode = this.MappedLocationCode.Trim();
		if(!string.IsNullOrEmpty(this.MappedLocationName)) this.MappedLocationName = this.MappedLocationName.Trim();

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

    public static LocationNewHierarchicalView GetItem(AMSContext _db, int? id)
    {
        return (from b in _db.LocationNewHierarchicalView
                where b.LocationID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<LocationNewHierarchicalView> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.LocationNewHierarchicalView
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.LocationID descending
                    select b);
        }
        else
        {
            return (from b in _db.LocationNewHierarchicalView
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.LocationID descending
                    select b);
        }
    }

    public static IQueryable<LocationNewHierarchicalView> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return LocationNewHierarchicalView.GetAllItems(_db, includeInactiveItems);
    }

    public static bool DeleteItem(AMSContext _db, int? id)
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
        return (from b in _db.LocationNewHierarchicalView select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return LocationNewHierarchicalView.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return LocationNewHierarchicalView.GetAllUserItems(_db, userID, includeInactiveItems);
    }

    BaseEntityObject IACSDBObject.GetItemByID(AMSContext _db, long itemID)
    {
        return GetItem(_db, (int?) itemID);
    }

    bool IACSDBObject.DeleteObject()
    {
        this.Delete();

        return true;
    }

    #endregion Interface Methods
}
