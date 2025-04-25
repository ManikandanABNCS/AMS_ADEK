using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class User_RightGroupTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int RightGroupID { get; set; }

    public int? ParentRightGroupID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string RightGroupName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? RightGroupDesc { get; set; }

    [InverseProperty("RightGroup")]
    public virtual ICollection<User_RightTable> User_RightTable { get; set; } = new List<User_RightTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public User_RightGroupTable()
    {

    }

	/// <summary>
    /// Default constructor for User_RightGroupTable
    /// </summary>
    /// <param name="db"></param>
	public User_RightGroupTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "RightGroupID";
	}

	public override object GetPrimaryKeyValue()
	{
		return RightGroupID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.RightGroupName)) this.RightGroupName = this.RightGroupName.Trim();
		if(!string.IsNullOrEmpty(this.RightGroupDesc)) this.RightGroupDesc = this.RightGroupDesc.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static User_RightGroupTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.User_RightGroupTable
                where b.RightGroupID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<User_RightGroupTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.User_RightGroupTable select b);
    }

    public static IQueryable<User_RightGroupTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return User_RightGroupTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.User_RightGroupTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return User_RightGroupTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return User_RightGroupTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
