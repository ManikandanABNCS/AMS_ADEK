using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class CategoryTypeDescriptionTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int CategoryTypeDescriptionID { get; set; }

    public int CategoryTypeID { get; set; }

    public string CategoryTypeDescription { get; set; } = null!;

    public int LanguageID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    [ForeignKey("CategoryTypeID")]
    [InverseProperty("CategoryTypeDescriptionTable")]
    public virtual CategoryTypeTable? CategoryType { get; set; } = null!;

    [ForeignKey("CreatedBy")]
    [InverseProperty("CategoryTypeDescriptionTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("LanguageID")]
    [InverseProperty("CategoryTypeDescriptionTable")]
    public virtual LanguageTable? Language { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("CategoryTypeDescriptionTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public CategoryTypeDescriptionTable()
    {

    }

	/// <summary>
    /// Default constructor for CategoryTypeDescriptionTable
    /// </summary>
    /// <param name="db"></param>
	public CategoryTypeDescriptionTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "CategoryTypeDescriptionID";
	}

	public override object GetPrimaryKeyValue()
	{
		return CategoryTypeDescriptionID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.CategoryTypeDescription)) this.CategoryTypeDescription = this.CategoryTypeDescription.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static CategoryTypeDescriptionTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.CategoryTypeDescriptionTable
                where b.CategoryTypeDescriptionID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<CategoryTypeDescriptionTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.CategoryTypeDescriptionTable select b);
    }

    public static IQueryable<CategoryTypeDescriptionTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return CategoryTypeDescriptionTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.CategoryTypeDescriptionTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return CategoryTypeDescriptionTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return CategoryTypeDescriptionTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
