using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class UserCompanyMappingTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int UserCompanyMappingID { get; set; }

    public int UserID { get; set; }

    public int CompanyID { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    [ForeignKey("CompanyID")]
    [InverseProperty("UserCompanyMappingTable")]
    public virtual CompanyTable? Company { get; set; } = null!;

    [ForeignKey("StatusID")]
    [InverseProperty("UserCompanyMappingTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [ForeignKey("UserID")]
    [InverseProperty("UserCompanyMappingTable")]
    public virtual User_LoginUserTable? User { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public UserCompanyMappingTable()
    {

    }

	/// <summary>
    /// Default constructor for UserCompanyMappingTable
    /// </summary>
    /// <param name="db"></param>
	public UserCompanyMappingTable(AMSContext _db)
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
		return "UserCompanyMappingID";
	}

	public override object GetPrimaryKeyValue()
	{
		return UserCompanyMappingID;
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

    public static UserCompanyMappingTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.UserCompanyMappingTable
                where b.UserCompanyMappingID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<UserCompanyMappingTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.UserCompanyMappingTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.UserCompanyMappingID descending
                    select b);
        }
        else
        {
            return (from b in _db.UserCompanyMappingTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.UserCompanyMappingID descending
                    select b);
        }
    }

    public static IQueryable<UserCompanyMappingTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return UserCompanyMappingTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.UserCompanyMappingTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return UserCompanyMappingTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return UserCompanyMappingTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
