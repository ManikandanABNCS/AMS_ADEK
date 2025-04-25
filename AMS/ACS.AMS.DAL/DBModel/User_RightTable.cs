using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class User_RightTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int RightID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string RightName { get; set; } = null!;

    [StringLength(250)]
    public string? RightDescription { get; set; }

    public int ValueType { get; set; }

    public bool DisplayRight { get; set; }

    public int? RightGroupID { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    [InverseProperty("Right")]
    public virtual ICollection<ReportTemplateTable> ReportTemplateTable { get; set; } = new List<ReportTemplateTable>();

    [ForeignKey("RightGroupID")]
    [InverseProperty("User_RightTable")]
    public virtual User_RightGroupTable? RightGroup { get; set; }

    [InverseProperty("Right")]
    public virtual ICollection<User_MenuTable> User_MenuTable { get; set; } = new List<User_MenuTable>();

    [InverseProperty("Right")]
    public virtual ICollection<User_RoleRightTable> User_RoleRightTable { get; set; } = new List<User_RoleRightTable>();

    [InverseProperty("Right")]
    public virtual ICollection<User_UserRightTable> User_UserRightTable { get; set; } = new List<User_UserRightTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public User_RightTable()
    {

    }

	/// <summary>
    /// Default constructor for User_RightTable
    /// </summary>
    /// <param name="db"></param>
	public User_RightTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "RightID";
	}

	public override object GetPrimaryKeyValue()
	{
		return RightID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.RightName)) this.RightName = this.RightName.Trim();
		if(!string.IsNullOrEmpty(this.RightDescription)) this.RightDescription = this.RightDescription.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static User_RightTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.User_RightTable
                where b.RightID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<User_RightTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.User_RightTable select b);
    }

    public static IQueryable<User_RightTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return User_RightTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.User_RightTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return User_RightTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return User_RightTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
