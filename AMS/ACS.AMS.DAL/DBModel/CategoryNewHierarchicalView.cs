using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Keyless]
public partial class CategoryNewHierarchicalView : BaseEntityObject, IACSDBObject
{
    public int? CategoryID { get; set; }

    [StringLength(100)]
    public string? CategoryCode { get; set; }

    public string? CategoryCodeHierarchy { get; set; }

    public string? CategoryName { get; set; }

    public string? CategoryNameHierarchy { get; set; }

    public int? ParentCategoryID { get; set; }

    public int? Level { get; set; }

    [DisplayName("Status")]
    public int? StatusID { get; set; }

    public string? AllLevelIDs { get; set; }

    public int? Level1ID { get; set; }

    public int? Level2ID { get; set; }

    public int? Level3ID { get; set; }

    public int? Level4ID { get; set; }

    public int? Level5ID { get; set; }

    public int? Level6ID { get; set; }

    public string? L1CategoryName { get; set; }

    public string? L2CategoryName { get; set; }

    public string? L3CategoryName { get; set; }

    public string? L4CategoryName { get; set; }

    public string? L5CategoryName { get; set; }

    public string? L6CategoryName { get; set; }

    [StringLength(200)]
    public string? Attribute1 { get; set; }

    [StringLength(200)]
    public string? Attribute2 { get; set; }

    [StringLength(200)]
    public string? Attribute3 { get; set; }

    [StringLength(200)]
    public string? Attribute4 { get; set; }

    [StringLength(200)]
    public string? Attribute5 { get; set; }

    [StringLength(200)]
    public string? Attribute6 { get; set; }

    [StringLength(200)]
    public string? Attribute7 { get; set; }

    [StringLength(200)]
    public string? Attribute8 { get; set; }

    [StringLength(200)]
    public string? Attribute9 { get; set; }

    [StringLength(200)]
    public string? Attribute10 { get; set; }

    [StringLength(200)]
    public string? Attribute11 { get; set; }

    [StringLength(200)]
    public string? Attribute12 { get; set; }

    [StringLength(200)]
    public string? Attribute13 { get; set; }

    [StringLength(200)]
    public string? Attribute14 { get; set; }

    [StringLength(200)]
    public string? Attribute15 { get; set; }

    [StringLength(200)]
    public string? Attribute16 { get; set; }

    [StringLength(100)]
    public string? CategoryType { get; set; }

    public int? CategoryTypeID { get; set; }

    public int? MappedCategoryID { get; set; }

    [StringLength(100)]
    public string? MappedCategoryCode { get; set; }

    public string? MappedCategoryName { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public CategoryNewHierarchicalView()
    {

    }

	/// <summary>
    /// Default constructor for CategoryNewHierarchicalView
    /// </summary>
    /// <param name="db"></param>
	public CategoryNewHierarchicalView(AMSContext _db)
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
		if(!string.IsNullOrEmpty(this.CategoryCodeHierarchy)) this.CategoryCodeHierarchy = this.CategoryCodeHierarchy.Trim();
		if(!string.IsNullOrEmpty(this.CategoryName)) this.CategoryName = this.CategoryName.Trim();
		if(!string.IsNullOrEmpty(this.CategoryNameHierarchy)) this.CategoryNameHierarchy = this.CategoryNameHierarchy.Trim();
		if(!string.IsNullOrEmpty(this.AllLevelIDs)) this.AllLevelIDs = this.AllLevelIDs.Trim();
		if(!string.IsNullOrEmpty(this.L1CategoryName)) this.L1CategoryName = this.L1CategoryName.Trim();
		if(!string.IsNullOrEmpty(this.L2CategoryName)) this.L2CategoryName = this.L2CategoryName.Trim();
		if(!string.IsNullOrEmpty(this.L3CategoryName)) this.L3CategoryName = this.L3CategoryName.Trim();
		if(!string.IsNullOrEmpty(this.L4CategoryName)) this.L4CategoryName = this.L4CategoryName.Trim();
		if(!string.IsNullOrEmpty(this.L5CategoryName)) this.L5CategoryName = this.L5CategoryName.Trim();
		if(!string.IsNullOrEmpty(this.L6CategoryName)) this.L6CategoryName = this.L6CategoryName.Trim();
		if(!string.IsNullOrEmpty(this.Attribute1)) this.Attribute1 = this.Attribute1.Trim();
		if(!string.IsNullOrEmpty(this.Attribute2)) this.Attribute2 = this.Attribute2.Trim();
		if(!string.IsNullOrEmpty(this.Attribute3)) this.Attribute3 = this.Attribute3.Trim();
		if(!string.IsNullOrEmpty(this.Attribute4)) this.Attribute4 = this.Attribute4.Trim();
		if(!string.IsNullOrEmpty(this.Attribute5)) this.Attribute5 = this.Attribute5.Trim();
		if(!string.IsNullOrEmpty(this.Attribute6)) this.Attribute6 = this.Attribute6.Trim();
		if(!string.IsNullOrEmpty(this.Attribute7)) this.Attribute7 = this.Attribute7.Trim();
		if(!string.IsNullOrEmpty(this.Attribute8)) this.Attribute8 = this.Attribute8.Trim();
		if(!string.IsNullOrEmpty(this.Attribute9)) this.Attribute9 = this.Attribute9.Trim();
		if(!string.IsNullOrEmpty(this.Attribute10)) this.Attribute10 = this.Attribute10.Trim();
		if(!string.IsNullOrEmpty(this.Attribute11)) this.Attribute11 = this.Attribute11.Trim();
		if(!string.IsNullOrEmpty(this.Attribute12)) this.Attribute12 = this.Attribute12.Trim();
		if(!string.IsNullOrEmpty(this.Attribute13)) this.Attribute13 = this.Attribute13.Trim();
		if(!string.IsNullOrEmpty(this.Attribute14)) this.Attribute14 = this.Attribute14.Trim();
		if(!string.IsNullOrEmpty(this.Attribute15)) this.Attribute15 = this.Attribute15.Trim();
		if(!string.IsNullOrEmpty(this.Attribute16)) this.Attribute16 = this.Attribute16.Trim();
		if(!string.IsNullOrEmpty(this.CategoryType)) this.CategoryType = this.CategoryType.Trim();
		if(!string.IsNullOrEmpty(this.MappedCategoryCode)) this.MappedCategoryCode = this.MappedCategoryCode.Trim();
		if(!string.IsNullOrEmpty(this.MappedCategoryName)) this.MappedCategoryName = this.MappedCategoryName.Trim();

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

    public static CategoryNewHierarchicalView GetItem(AMSContext _db, int? id)
    {
        return (from b in _db.CategoryNewHierarchicalView
                where b.CategoryID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<CategoryNewHierarchicalView> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.CategoryNewHierarchicalView
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.CategoryID descending
                    select b);
        }
        else
        {
            return (from b in _db.CategoryNewHierarchicalView
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.CategoryID descending
                    select b);
        }
    }

    public static IQueryable<CategoryNewHierarchicalView> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return CategoryNewHierarchicalView.GetAllItems(_db, includeInactiveItems);
    }

    public static bool DeleteItem(AMSContext _db, int? id)
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
        return (from b in _db.CategoryNewHierarchicalView select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return CategoryNewHierarchicalView.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return CategoryNewHierarchicalView.GetAllUserItems(_db, userID, includeInactiveItems);
    }

    BaseEntityObject IACSDBObject.GetItemByID(AMSContext _db, long itemID)
    {
        return GetItem(_db, (int?) itemID);
    }

    bool IACSDBObject.DeleteObject()
    {
        this.Delete();

        return true;
    }

    #endregion Interface Methods
}
