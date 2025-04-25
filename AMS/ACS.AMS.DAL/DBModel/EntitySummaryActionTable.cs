using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class EntitySummaryActionTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int EntitySummaryActionID { get; set; }

    [StringLength(100)]
    public string TemplateName { get; set; } = null!;

    [StringLength(100)]
    public string? ActionName { get; set; }

    public string? ActionQuery { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public EntitySummaryActionTable()
    {

    }

	/// <summary>
    /// Default constructor for EntitySummaryActionTable
    /// </summary>
    /// <param name="db"></param>
	public EntitySummaryActionTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "EntitySummaryActionID";
	}

	public override object GetPrimaryKeyValue()
	{
		return EntitySummaryActionID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.TemplateName)) this.TemplateName = this.TemplateName.Trim();
		if(!string.IsNullOrEmpty(this.ActionName)) this.ActionName = this.ActionName.Trim();
		if(!string.IsNullOrEmpty(this.ActionQuery)) this.ActionQuery = this.ActionQuery.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static EntitySummaryActionTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.EntitySummaryActionTable
                where b.EntitySummaryActionID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<EntitySummaryActionTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.EntitySummaryActionTable select b);
    }

    public static IQueryable<EntitySummaryActionTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return EntitySummaryActionTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.EntitySummaryActionTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return EntitySummaryActionTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return EntitySummaryActionTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
