using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class UserGridNewColumnTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int UserGridColumnID { get; set; }

    public int UserID { get; set; }

    public int MasterGridID { get; set; }

    public int MasterGridLineItemID { get; set; }

    public int OrderID { get; set; }

    [ForeignKey("MasterGridID")]
    [InverseProperty("UserGridNewColumnTable")]
    public virtual MasterGridNewTable? MasterGrid { get; set; } = null!;

    [ForeignKey("MasterGridLineItemID")]
    [InverseProperty("UserGridNewColumnTable")]
    public virtual MasterGridNewLineItemTable? MasterGridLineItem { get; set; } = null!;

    [ForeignKey("UserID")]
    [InverseProperty("UserGridNewColumnTable")]
    public virtual User_LoginUserTable? User { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public UserGridNewColumnTable()
    {

    }

	/// <summary>
    /// Default constructor for UserGridNewColumnTable
    /// </summary>
    /// <param name="db"></param>
	public UserGridNewColumnTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "UserGridColumnID";
	}

	public override object GetPrimaryKeyValue()
	{
		return UserGridColumnID;
	}

	internal override void BeforeSave(AMSContext db)
    {

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static UserGridNewColumnTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.UserGridNewColumnTable
                where b.UserGridColumnID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<UserGridNewColumnTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.UserGridNewColumnTable select b);
    }

    public static IQueryable<UserGridNewColumnTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return UserGridNewColumnTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.UserGridNewColumnTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return UserGridNewColumnTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return UserGridNewColumnTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
