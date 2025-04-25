using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class UserDepartmentMappingTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int UserDepartmentMappingID { get; set; }

    public int PersonID { get; set; }

    public int DepartmentID { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    [ForeignKey("DepartmentID")]
    [InverseProperty("UserDepartmentMappingTable")]
    public virtual DepartmentTable? Department { get; set; } = null!;

    [ForeignKey("StatusID")]
    [InverseProperty("UserDepartmentMappingTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [ForeignKey("PersonID")]
    [InverseProperty("UserDepartmentMappingTable")]
    public virtual User_LoginUserTable? User { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public UserDepartmentMappingTable()
    {

    }

	/// <summary>
    /// Default constructor for UserDepartmentMappingTable
    /// </summary>
    /// <param name="db"></param>
	public UserDepartmentMappingTable(AMSContext _db)
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
		return "UserDepartmentMappingID";
	}

	public override object GetPrimaryKeyValue()
	{
		return UserDepartmentMappingID;
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

    public static UserDepartmentMappingTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.UserDepartmentMappingTable
                where b.UserDepartmentMappingID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<UserDepartmentMappingTable> GetAllItemsByDepartment(AMSContext _db, int departmentID , bool includeInactiveItems = false )
    {
        return from b in GetAllItems(_db, includeInactiveItems)
                where b.DepartmentID == departmentID
                select b;
    }

    public static IQueryable<UserDepartmentMappingTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.UserDepartmentMappingTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.UserDepartmentMappingID descending
                    select b);
        }
        else
        {
            return (from b in _db.UserDepartmentMappingTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.UserDepartmentMappingID descending
                    select b);
        }
    }

    public static IQueryable<UserDepartmentMappingTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return from b in UserDepartmentMappingTable.GetAllItems(_db, includeInactiveItems)
                
                select b;
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
        return (from b in _db.UserDepartmentMappingTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return UserDepartmentMappingTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return UserDepartmentMappingTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
