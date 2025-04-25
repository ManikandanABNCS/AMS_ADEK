using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class App_Applications : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ApplicationID { get; set; }

    [StringLength(150)]
    public string ApplicationName { get; set; } = null!;

    [StringLength(250)]
    public string? Description { get; set; }

    [InverseProperty("Application")]
    public virtual ICollection<User_LoginUserTable> User_LoginUserTable { get; set; } = new List<User_LoginUserTable>();

    [InverseProperty("Application")]
    public virtual ICollection<User_RoleTable> User_RoleTable { get; set; } = new List<User_RoleTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public App_Applications()
    {

    }

	/// <summary>
    /// Default constructor for App_Applications
    /// </summary>
    /// <param name="db"></param>
	public App_Applications(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "ApplicationID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ApplicationID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.ApplicationName)) this.ApplicationName = this.ApplicationName.Trim();
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

    public static App_Applications GetItem(AMSContext _db, int id)
    {
        return (from b in _db.App_Applications
                where b.ApplicationID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<App_Applications> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.App_Applications select b);
    }

    public static IQueryable<App_Applications> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return App_Applications.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.App_Applications select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return App_Applications.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return App_Applications.GetAllUserItems(_db, userID, includeInactiveItems);
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
