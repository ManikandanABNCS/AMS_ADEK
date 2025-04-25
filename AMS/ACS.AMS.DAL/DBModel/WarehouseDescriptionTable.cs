using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class WarehouseDescriptionTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int WarehouseDescriptionID { get; set; }

    public string WarehouseDescription { get; set; } = null!;

    public int WarehouseID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("WarehouseDescriptionTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("WarehouseDescriptionTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [ForeignKey("WarehouseID")]
    [InverseProperty("WarehouseDescriptionTable")]
    public virtual WarehouseTable? Warehouse { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public WarehouseDescriptionTable()
    {

    }

	/// <summary>
    /// Default constructor for WarehouseDescriptionTable
    /// </summary>
    /// <param name="db"></param>
	public WarehouseDescriptionTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "WarehouseDescriptionID";
	}

	public override object GetPrimaryKeyValue()
	{
		return WarehouseDescriptionID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.WarehouseDescription)) this.WarehouseDescription = this.WarehouseDescription.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static WarehouseDescriptionTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.WarehouseDescriptionTable
                where b.WarehouseDescriptionID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<WarehouseDescriptionTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.WarehouseDescriptionTable select b);
    }

    public static IQueryable<WarehouseDescriptionTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return WarehouseDescriptionTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.WarehouseDescriptionTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return WarehouseDescriptionTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return WarehouseDescriptionTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
