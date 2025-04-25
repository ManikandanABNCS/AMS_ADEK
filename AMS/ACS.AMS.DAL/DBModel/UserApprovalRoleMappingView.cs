using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Keyless]
public partial class UserApprovalRoleMappingView : BaseEntityObject, IACSDBObject
{
    public int UserID { get; set; }

    public int ApprovalRoleID { get; set; }

    [StringLength(201)]
    public string personName { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public UserApprovalRoleMappingView()
    {

    }

	/// <summary>
    /// Default constructor for UserApprovalRoleMappingView
    /// </summary>
    /// <param name="db"></param>
	public UserApprovalRoleMappingView(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "UserID";
	}

	public override object GetPrimaryKeyValue()
	{
		return UserID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.personName)) this.personName = this.personName.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static UserApprovalRoleMappingView GetItem(AMSContext _db, int id)
    {
        return (from b in _db.UserApprovalRoleMappingView
                where b.UserID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<UserApprovalRoleMappingView> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.UserApprovalRoleMappingView select b);
    }

    public static IQueryable<UserApprovalRoleMappingView> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return UserApprovalRoleMappingView.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.UserApprovalRoleMappingView select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return UserApprovalRoleMappingView.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return UserApprovalRoleMappingView.GetAllUserItems(_db, userID, includeInactiveItems);
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
