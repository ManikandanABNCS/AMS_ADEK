using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Keyless]
public partial class SecondLevelCategoryTable : BaseEntityObject, IACSDBObject
{
    [StringLength(100)]
    public string CategoryCode { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public string ParentLocation { get; set; } = null!;

    public int CategoryID { get; set; }

    public string SecondLevelCategoryName { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public SecondLevelCategoryTable()
    {

    }

	/// <summary>
    /// Default constructor for SecondLevelCategoryTable
    /// </summary>
    /// <param name="db"></param>
	public SecondLevelCategoryTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "CategoryCode";
	}

	public override object GetPrimaryKeyValue()
	{
		return CategoryCode;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.CategoryCode)) this.CategoryCode = this.CategoryCode.Trim();
		if(!string.IsNullOrEmpty(this.CategoryName)) this.CategoryName = this.CategoryName.Trim();
		if(!string.IsNullOrEmpty(this.ParentLocation)) this.ParentLocation = this.ParentLocation.Trim();
		if(!string.IsNullOrEmpty(this.SecondLevelCategoryName)) this.SecondLevelCategoryName = this.SecondLevelCategoryName.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static SecondLevelCategoryTable GetItem(AMSContext _db, string id)
    {
        return (from b in _db.SecondLevelCategoryTable
                where b.CategoryCode == id
                select b).FirstOrDefault();
    }

    public static IQueryable<SecondLevelCategoryTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.SecondLevelCategoryTable select b);
    }

    public static IQueryable<SecondLevelCategoryTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return SecondLevelCategoryTable.GetAllItems(_db, includeInactiveItems);
    }

    public static bool DeleteItem(AMSContext _db, string id)
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
        return (from b in _db.SecondLevelCategoryTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return SecondLevelCategoryTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return SecondLevelCategoryTable.GetAllUserItems(_db, userID, includeInactiveItems);
    }

    BaseEntityObject IACSDBObject.GetItemByID(AMSContext _db, long itemID)
    {
        return GetItem(_db,  itemID+"");
    }

    bool IACSDBObject.DeleteObject()
    {
        this.Delete();

        return true;
    }

    #endregion Interface Methods
}
