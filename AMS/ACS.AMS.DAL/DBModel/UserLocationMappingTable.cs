using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class UserLocationMappingTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int UserLocationMappingID { get; set; }

    public int PersonID { get; set; }

    public int LocationID { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    [ForeignKey("LocationID")]
    [InverseProperty("UserLocationMappingTable")]
    public virtual LocationTable? Location { get; set; } = null!;

    [ForeignKey("StatusID")]
    [InverseProperty("UserLocationMappingTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [ForeignKey("PersonID")]
    [InverseProperty("UserLocationMappingTable")]
    public virtual User_LoginUserTable? User { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public UserLocationMappingTable()
    {

    }

	/// <summary>
    /// Default constructor for UserLocationMappingTable
    /// </summary>
    /// <param name="db"></param>
	public UserLocationMappingTable(AMSContext _db)
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
		return "UserLocationMappingID";
	}

	public override object GetPrimaryKeyValue()
	{
		return UserLocationMappingID;
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

    public static UserLocationMappingTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.UserLocationMappingTable
                where b.UserLocationMappingID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<UserLocationMappingTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.UserLocationMappingTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.UserLocationMappingID descending
                    select b);
        }
        else
        {
            return (from b in _db.UserLocationMappingTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.UserLocationMappingID descending
                    select b);
        }
    }

    public static IQueryable<UserLocationMappingTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return UserLocationMappingTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.UserLocationMappingTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return UserLocationMappingTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return UserLocationMappingTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
