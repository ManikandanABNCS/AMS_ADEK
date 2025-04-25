using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class UserCategoryMappingTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int UserCategoryMappingID { get; set; }

    public int PersonID { get; set; }

    public int CategoryID { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    [ForeignKey("CategoryID")]
    [InverseProperty("UserCategoryMappingTable")]
    public virtual CategoryTable? Category { get; set; } = null!;

    [ForeignKey("StatusID")]
    [InverseProperty("UserCategoryMappingTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [ForeignKey("PersonID")]
    [InverseProperty("UserCategoryMappingTable")]
    public virtual User_LoginUserTable? User { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public UserCategoryMappingTable()
    {

    }

	/// <summary>
    /// Default constructor for UserCategoryMappingTable
    /// </summary>
    /// <param name="db"></param>
	public UserCategoryMappingTable(AMSContext _db)
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
		return "UserCategoryMappingID";
	}

	public override object GetPrimaryKeyValue()
	{
		return UserCategoryMappingID;
	}

	internal override void BeforeSave(AMSContext db)
    {

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

    public static UserCategoryMappingTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.UserCategoryMappingTable
                where b.UserCategoryMappingID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<UserCategoryMappingTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.UserCategoryMappingTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.UserCategoryMappingID descending
                    select b);
        }
        else
        {
            return (from b in _db.UserCategoryMappingTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.UserCategoryMappingID descending
                    select b);
        }
    }

    public static IQueryable<UserCategoryMappingTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return UserCategoryMappingTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.UserCategoryMappingTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return UserCategoryMappingTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return UserCategoryMappingTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
