using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class UserTypeTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int UserTypeID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string UserType { get; set; } = null!;

    [DisplayName("Status")]
    public int? StatusID { get; set; }

    [InverseProperty("UserType")]
    public virtual ICollection<PersonTable> PersonTable { get; set; } = new List<PersonTable>();

    [ForeignKey("StatusID")]
    [InverseProperty("UserTypeTable")]
    public virtual StatusTable? Status { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public UserTypeTable()
    {

    }

	/// <summary>
    /// Default constructor for UserTypeTable
    /// </summary>
    /// <param name="db"></param>
	public UserTypeTable(AMSContext _db)
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
		return "UserTypeID";
	}

	public override object GetPrimaryKeyValue()
	{
		return UserTypeID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.UserType)) this.UserType = this.UserType.Trim();

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

    public static UserTypeTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.UserTypeTable
                where b.UserTypeID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<UserTypeTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.UserTypeTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.UserTypeID descending
                    select b);
        }
        else
        {
            return (from b in _db.UserTypeTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.UserTypeID descending
                    select b);
        }
    }

    public static IQueryable<UserTypeTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return UserTypeTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.UserTypeTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return UserTypeTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return UserTypeTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
