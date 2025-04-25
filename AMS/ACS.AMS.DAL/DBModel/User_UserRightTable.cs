using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class User_UserRightTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int UserRightID { get; set; }

    public int UserID { get; set; }

    public int RightID { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string RightValue { get; set; } = null!;

    [ForeignKey("RightID")]
    [InverseProperty("User_UserRightTable")]
    public virtual User_RightTable? Right { get; set; } = null!;

    [ForeignKey("UserID")]
    [InverseProperty("User_UserRightTable")]
    public virtual User_LoginUserTable? User { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public User_UserRightTable()
    {

    }

	/// <summary>
    /// Default constructor for User_UserRightTable
    /// </summary>
    /// <param name="db"></param>
	public User_UserRightTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "UserRightID";
	}

	public override object GetPrimaryKeyValue()
	{
		return UserRightID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.RightValue)) this.RightValue = this.RightValue.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static User_UserRightTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.User_UserRightTable
                where b.UserRightID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<User_UserRightTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.User_UserRightTable select b);
    }

    public static IQueryable<User_UserRightTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return User_UserRightTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.User_UserRightTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return User_UserRightTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return User_UserRightTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
