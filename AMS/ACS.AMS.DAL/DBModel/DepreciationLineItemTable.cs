using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Index("AssetID", Name = "FK_AssetID")]
[Index("CategoryID", Name = "FK_CategoryID")]
[Index("DepreciationClassID", Name = "FK_DepreciationClassID")]
[Index("DepreciationID", Name = "FK_DepreciationID")]
[Index("AssetID", Name = "IX_DepreciationLineItemTable_AssetID")]
[Index("DepreciationID", Name = "IX_DepreciationLineItemTable_DepreciationID")]
public partial class DepreciationLineItemTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int DepreciationLineItemID { get; set; }

    public int DepreciationID { get; set; }

    public int AssetID { get; set; }

    public int DepreciationClassLineItemID { get; set; }

    [Column(TypeName = "decimal(22, 10)")]
    public decimal? DepreciationValue { get; set; }

    [Column(TypeName = "decimal(22, 10)")]
    public decimal? AssetValueAfterDepreciation { get; set; }

    [Column(TypeName = "decimal(22, 10)")]
    public decimal? AssetValueBeforeDepreciation { get; set; }

    public int CategoryID { get; set; }

    public int DepreciationClassID { get; set; }

    public bool? IsReclassification { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ReclassificationValue { get; set; }

    public int? CategoryL2 { get; set; }

    [ForeignKey("AssetID")]
    [InverseProperty("DepreciationLineItemTable")]
    public virtual AssetTable? Asset { get; set; } = null!;

    [ForeignKey("CategoryID")]
    [InverseProperty("DepreciationLineItemTable")]
    public virtual CategoryTable? Category { get; set; } = null!;

    [ForeignKey("DepreciationID")]
    [InverseProperty("DepreciationLineItemTable")]
    public virtual DepreciationTable? Depreciation { get; set; } = null!;

    [ForeignKey("DepreciationClassID")]
    [InverseProperty("DepreciationLineItemTable")]
    public virtual DepreciationClassTable? DepreciationClass { get; set; } = null!;

    [ForeignKey("DepreciationClassLineItemID")]
    [InverseProperty("DepreciationLineItemTable")]
    public virtual DepreciationClassLineItemTable? DepreciationClassLineItem { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public DepreciationLineItemTable()
    {

    }

	/// <summary>
    /// Default constructor for DepreciationLineItemTable
    /// </summary>
    /// <param name="db"></param>
	public DepreciationLineItemTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "DepreciationLineItemID";
	}

	public override object GetPrimaryKeyValue()
	{
		return DepreciationLineItemID;
	}

	internal override void BeforeSave(AMSContext db)
    {

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static DepreciationLineItemTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.DepreciationLineItemTable
                where b.DepreciationLineItemID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<DepreciationLineItemTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.DepreciationLineItemTable select b);
    }

    public static IQueryable<DepreciationLineItemTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return DepreciationLineItemTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.DepreciationLineItemTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return DepreciationLineItemTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return DepreciationLineItemTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
