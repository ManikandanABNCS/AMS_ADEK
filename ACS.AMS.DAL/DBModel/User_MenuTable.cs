using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class User_MenuTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int MenuID { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string MenuName { get; set; } = null!;

    public int? RightID { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? TargetObject { get; set; }

    public int? ParentMenuID { get; set; }

    public short OrderNo { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? Image { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ShortCutKeys { get; set; }

    public int? ModuleID { get; set; }

    [StringLength(400)]
    public string? Attribute1 { get; set; }

    public int? ParentTransactionID { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? ParentTransactionType { get; set; }

    [InverseProperty("ParentMenu")]
    public virtual ICollection<User_MenuTable> InverseParentMenu { get; set; } = new List<User_MenuTable>();

    [ForeignKey("ParentMenuID")]
    [InverseProperty("InverseParentMenu")]
    public virtual User_MenuTable? ParentMenu { get; set; }

    [ForeignKey("RightID")]
    [InverseProperty("User_MenuTable")]
    public virtual User_RightTable? Right { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public User_MenuTable()
    {

    }

	/// <summary>
    /// Default constructor for User_MenuTable
    /// </summary>
    /// <param name="db"></param>
	public User_MenuTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "MenuID";
	}

	public override object GetPrimaryKeyValue()
	{
		return MenuID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.MenuName)) this.MenuName = this.MenuName.Trim();
		if(!string.IsNullOrEmpty(this.TargetObject)) this.TargetObject = this.TargetObject.Trim();
		if(!string.IsNullOrEmpty(this.Image)) this.Image = this.Image.Trim();
		if(!string.IsNullOrEmpty(this.ShortCutKeys)) this.ShortCutKeys = this.ShortCutKeys.Trim();
		if(!string.IsNullOrEmpty(this.Attribute1)) this.Attribute1 = this.Attribute1.Trim();
		if(!string.IsNullOrEmpty(this.ParentTransactionType)) this.ParentTransactionType = this.ParentTransactionType.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static User_MenuTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.User_MenuTable
                where b.MenuID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<User_MenuTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.User_MenuTable select b);
    }

    public static IQueryable<User_MenuTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return User_MenuTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.User_MenuTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return User_MenuTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return User_MenuTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
