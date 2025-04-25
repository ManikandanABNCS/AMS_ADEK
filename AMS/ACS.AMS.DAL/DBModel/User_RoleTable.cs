using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class User_RoleTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int RoleID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string RoleName { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public int ApplicationID { get; set; }

    [StringLength(250)]
    public string? Description { get; set; }

    public bool DisplayRole { get; set; }

    public bool IsActive { get; set; }

    [ForeignKey("ApplicationID")]
    [InverseProperty("User_RoleTable")]
    public virtual App_Applications? Application { get; set; } = null!;

    [InverseProperty("Role")]
    public virtual ICollection<User_RoleRightTable> User_RoleRightTable { get; set; } = new List<User_RoleRightTable>();

    [InverseProperty("Role")]
    public virtual ICollection<User_UserRoleTable> User_UserRoleTable { get; set; } = new List<User_UserRoleTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public User_RoleTable()
    {

    }

	/// <summary>
    /// Default constructor for User_RoleTable
    /// </summary>
    /// <param name="db"></param>
	public User_RoleTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "RoleID";
	}

	public override object GetPrimaryKeyValue()
	{
		return RoleID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.RoleName)) this.RoleName = this.RoleName.Trim();
		if(!string.IsNullOrEmpty(this.Description)) this.Description = this.Description.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static User_RoleTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.User_RoleTable
                where b.RoleID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<User_RoleTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.User_RoleTable select b);
    }

    public static IQueryable<User_RoleTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return User_RoleTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.User_RoleTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return User_RoleTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return User_RoleTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
