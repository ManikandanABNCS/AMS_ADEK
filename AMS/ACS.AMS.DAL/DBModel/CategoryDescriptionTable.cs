using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class CategoryDescriptionTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int CategoryDescriptionID { get; set; }

    public int CategoryID { get; set; }

    public string CategoryDescription { get; set; } = null!;

    public int LanguageID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    [ForeignKey("CategoryID")]
    [InverseProperty("CategoryDescriptionTable")]
    public virtual CategoryTable? Category { get; set; } = null!;

    [ForeignKey("CreatedBy")]
    [InverseProperty("CategoryDescriptionTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("LanguageID")]
    [InverseProperty("CategoryDescriptionTable")]
    public virtual LanguageTable? Language { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("CategoryDescriptionTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public CategoryDescriptionTable()
    {

    }

	/// <summary>
    /// Default constructor for CategoryDescriptionTable
    /// </summary>
    /// <param name="db"></param>
	public CategoryDescriptionTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "CategoryDescriptionID";
	}

	public override object GetPrimaryKeyValue()
	{
		return CategoryDescriptionID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.CategoryDescription)) this.CategoryDescription = this.CategoryDescription.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static CategoryDescriptionTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.CategoryDescriptionTable
                where b.CategoryDescriptionID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<CategoryDescriptionTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.CategoryDescriptionTable select b);
    }

    public static IQueryable<CategoryDescriptionTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return CategoryDescriptionTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.CategoryDescriptionTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return CategoryDescriptionTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return CategoryDescriptionTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
