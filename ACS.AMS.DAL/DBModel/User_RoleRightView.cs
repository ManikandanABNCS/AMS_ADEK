using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Keyless]
public partial class User_RoleRightView : BaseEntityObject, IACSDBObject
{
    public int? RoleRightID { get; set; }

    public int? RoleID { get; set; }

    public int? RightID { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? RightValue { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string RightName { get; set; } = null!;

    public int? RightGroupID { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public User_RoleRightView()
    {

    }

	/// <summary>
    /// Default constructor for User_RoleRightView
    /// </summary>
    /// <param name="db"></param>
	public User_RoleRightView(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "RoleRightID";
	}

	public override object GetPrimaryKeyValue()
	{
		return RoleRightID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.RightValue)) this.RightValue = this.RightValue.Trim();
		if(!string.IsNullOrEmpty(this.RightName)) this.RightName = this.RightName.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static User_RoleRightView GetItem(AMSContext _db, int? id)
    {
        return (from b in _db.User_RoleRightView
                where b.RoleRightID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<User_RoleRightView> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.User_RoleRightView select b);
    }

    public static IQueryable<User_RoleRightView> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return User_RoleRightView.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.User_RoleRightView select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return User_RoleRightView.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return User_RoleRightView.GetAllUserItems(_db, userID, includeInactiveItems);
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
