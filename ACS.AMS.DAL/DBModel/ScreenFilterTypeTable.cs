using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ScreenFilterTypeTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public byte ScreenFilterTypeID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ScreenFilterTypeCode { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string ScreenFilterTypeName { get; set; } = null!;

    [InverseProperty("ScreenFilterType")]
    public virtual ICollection<DashboardTemplateFilterFieldTable> DashboardTemplateFilterFieldTable { get; set; } = new List<DashboardTemplateFilterFieldTable>();

    [InverseProperty("ScreenFilterType")]
    public virtual ICollection<ScreenFilterLineItemTable> ScreenFilterLineItemTable { get; set; } = new List<ScreenFilterLineItemTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ScreenFilterTypeTable()
    {

    }

	/// <summary>
    /// Default constructor for ScreenFilterTypeTable
    /// </summary>
    /// <param name="db"></param>
	public ScreenFilterTypeTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "ScreenFilterTypeID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ScreenFilterTypeID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.ScreenFilterTypeCode)) this.ScreenFilterTypeCode = this.ScreenFilterTypeCode.Trim();
		if(!string.IsNullOrEmpty(this.ScreenFilterTypeName)) this.ScreenFilterTypeName = this.ScreenFilterTypeName.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static ScreenFilterTypeTable GetItem(AMSContext _db, byte id)
    {
        return (from b in _db.ScreenFilterTypeTable
                where b.ScreenFilterTypeID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ScreenFilterTypeTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.ScreenFilterTypeTable select b);
    }

    public static IQueryable<ScreenFilterTypeTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ScreenFilterTypeTable.GetAllItems(_db, includeInactiveItems);
    }

    public static bool DeleteItem(AMSContext _db, byte id)
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
        return (from b in _db.ScreenFilterTypeTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ScreenFilterTypeTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ScreenFilterTypeTable.GetAllUserItems(_db, userID, includeInactiveItems);
    }

    BaseEntityObject IACSDBObject.GetItemByID(AMSContext _db, long itemID)
    {
        return GetItem(_db, (byte) itemID);
    }

    bool IACSDBObject.DeleteObject()
    {
        this.Delete();

        return true;
    }

    #endregion Interface Methods
}
