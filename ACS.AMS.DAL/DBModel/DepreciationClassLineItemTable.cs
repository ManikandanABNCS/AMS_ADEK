using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Index("DepreciationClassID", Name = "FK_DepreciationClassID")]
public partial class DepreciationClassLineItemTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int DepreciationClassLineItemID { get; set; }

    public int DepreciationClassID { get; set; }

    [StringLength(100)]
    public string AssetStartValue { get; set; } = null!;

    [StringLength(100)]
    public string AssetEndValue { get; set; } = null!;

    [StringLength(100)]
    public string? Duration { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? DepreciationPercentage { get; set; }

    [ForeignKey("DepreciationClassID")]
    [InverseProperty("DepreciationClassLineItemTable")]
    public virtual DepreciationClassTable? DepreciationClass { get; set; } = null!;

    [InverseProperty("DepreciationClassLineItem")]
    public virtual ICollection<DepreciationLineItemTable> DepreciationLineItemTable { get; set; } = new List<DepreciationLineItemTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public DepreciationClassLineItemTable()
    {

    }

	/// <summary>
    /// Default constructor for DepreciationClassLineItemTable
    /// </summary>
    /// <param name="db"></param>
	public DepreciationClassLineItemTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "DepreciationClassLineItemID";
	}

	public override object GetPrimaryKeyValue()
	{
		return DepreciationClassLineItemID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.AssetStartValue)) this.AssetStartValue = this.AssetStartValue.Trim();
		if(!string.IsNullOrEmpty(this.AssetEndValue)) this.AssetEndValue = this.AssetEndValue.Trim();
		if(!string.IsNullOrEmpty(this.Duration)) this.Duration = this.Duration.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static DepreciationClassLineItemTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.DepreciationClassLineItemTable
                where b.DepreciationClassLineItemID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<DepreciationClassLineItemTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.DepreciationClassLineItemTable select b);
    }

    public static IQueryable<DepreciationClassLineItemTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return DepreciationClassLineItemTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.DepreciationClassLineItemTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return DepreciationClassLineItemTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return DepreciationClassLineItemTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
