using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class DynamicColumnRequiredEntityTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int DynamicColumnRequiredEntityID { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string DynamicColumnRequiredName { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string EntityName { get; set; } = null!;

    public bool? isEnableExcel { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? ImportProcedure { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public DynamicColumnRequiredEntityTable()
    {

    }

	/// <summary>
    /// Default constructor for DynamicColumnRequiredEntityTable
    /// </summary>
    /// <param name="db"></param>
	public DynamicColumnRequiredEntityTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "DynamicColumnRequiredEntityID";
	}

	public override object GetPrimaryKeyValue()
	{
		return DynamicColumnRequiredEntityID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.DynamicColumnRequiredName)) this.DynamicColumnRequiredName = this.DynamicColumnRequiredName.Trim();
		if(!string.IsNullOrEmpty(this.EntityName)) this.EntityName = this.EntityName.Trim();
		if(!string.IsNullOrEmpty(this.ImportProcedure)) this.ImportProcedure = this.ImportProcedure.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static DynamicColumnRequiredEntityTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.DynamicColumnRequiredEntityTable
                where b.DynamicColumnRequiredEntityID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<DynamicColumnRequiredEntityTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.DynamicColumnRequiredEntityTable select b);
    }

    public static IQueryable<DynamicColumnRequiredEntityTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return DynamicColumnRequiredEntityTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.DynamicColumnRequiredEntityTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return DynamicColumnRequiredEntityTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return DynamicColumnRequiredEntityTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
