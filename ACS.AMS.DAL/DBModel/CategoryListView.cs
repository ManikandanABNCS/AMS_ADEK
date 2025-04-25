using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Keyless]
public partial class CategoryListView : BaseEntityObject, IACSDBObject
{
    public int CategoryID { get; set; }

    [StringLength(100)]
    public string CategoryCode { get; set; } = null!;

    public int? ParentCategoryID { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    public int? DepreciationClassID { get; set; }

    public string CategoryName { get; set; } = null!;

    public int? CategoryTypeID { get; set; }

    public string? CatalogueImage { get; set; }

    public string? ParentCategory { get; set; }

    [StringLength(250)]
    public string? Status { get; set; }

    public string? ClassName { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public CategoryListView()
    {

    }

	/// <summary>
    /// Default constructor for CategoryListView
    /// </summary>
    /// <param name="db"></param>
	public CategoryListView(AMSContext _db)
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
		return "CategoryID";
	}

	public override object GetPrimaryKeyValue()
	{
		return CategoryID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.CategoryCode)) this.CategoryCode = this.CategoryCode.Trim();
		if(!string.IsNullOrEmpty(this.CategoryName)) this.CategoryName = this.CategoryName.Trim();
		if(!string.IsNullOrEmpty(this.CatalogueImage)) this.CatalogueImage = this.CatalogueImage.Trim();
		if(!string.IsNullOrEmpty(this.ParentCategory)) this.ParentCategory = this.ParentCategory.Trim();
		if(!string.IsNullOrEmpty(this.Status)) this.Status = this.Status.Trim();
		if(!string.IsNullOrEmpty(this.ClassName)) this.ClassName = this.ClassName.Trim();

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

    public static CategoryListView GetItem(AMSContext _db, int id)
    {
        return (from b in _db.CategoryListView
                where b.CategoryID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<CategoryListView> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.CategoryListView
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.CategoryID descending
                    select b);
        }
        else
        {
            return (from b in _db.CategoryListView
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.CategoryID descending
                    select b);
        }
    }

    public static IQueryable<CategoryListView> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return CategoryListView.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.CategoryListView select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return CategoryListView.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return CategoryListView.GetAllUserItems(_db, userID, includeInactiveItems);
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
