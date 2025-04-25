using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Keyless]
public partial class UserRightView : BaseEntityObject, IACSDBObject
{
    public int RightID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string RightName { get; set; } = null!;

    [StringLength(250)]
    public string? RightDescription { get; set; }

    public int ValueType { get; set; }

    public bool DisplayRight { get; set; }

    public int? RightGroupID { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string RightGroupName { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public UserRightView()
    {

    }

	/// <summary>
    /// Default constructor for UserRightView
    /// </summary>
    /// <param name="db"></param>
	public UserRightView(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "RightID";
	}

	public override object GetPrimaryKeyValue()
	{
		return RightID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.RightName)) this.RightName = this.RightName.Trim();
		if(!string.IsNullOrEmpty(this.RightDescription)) this.RightDescription = this.RightDescription.Trim();
		if(!string.IsNullOrEmpty(this.RightGroupName)) this.RightGroupName = this.RightGroupName.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static UserRightView GetItem(AMSContext _db, int id)
    {
        return (from b in _db.UserRightView
                where b.RightID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<UserRightView> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.UserRightView select b);
    }

    public static IQueryable<UserRightView> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return UserRightView.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.UserRightView select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return UserRightView.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return UserRightView.GetAllUserItems(_db, userID, includeInactiveItems);
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
