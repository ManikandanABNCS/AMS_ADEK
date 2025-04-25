using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class EntityTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int EntityID { get; set; }

    [StringLength(100)]
    public string EntityName { get; set; } = null!;

    [StringLength(100)]
    public string? QueryString { get; set; }

    [InverseProperty("Entity")]
    public virtual ICollection<AttributeDefinitionTable> AttributeDefinitionTable { get; set; } = new List<AttributeDefinitionTable>();

    [InverseProperty("Entity")]
    public virtual ICollection<ImportFormatNewTable> ImportFormatNewTable { get; set; } = new List<ImportFormatNewTable>();

    [InverseProperty("Entity")]
    public virtual ICollection<ImportTemplateNewTable> ImportTemplateNewTable { get; set; } = new List<ImportTemplateNewTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public EntityTable()
    {

    }

	/// <summary>
    /// Default constructor for EntityTable
    /// </summary>
    /// <param name="db"></param>
	public EntityTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "EntityID";
	}

	public override object GetPrimaryKeyValue()
	{
		return EntityID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.EntityName)) this.EntityName = this.EntityName.Trim();
		if(!string.IsNullOrEmpty(this.QueryString)) this.QueryString = this.QueryString.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static EntityTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.EntityTable
                where b.EntityID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<EntityTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.EntityTable select b);
    }

    public static IQueryable<EntityTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return EntityTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.EntityTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return EntityTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return EntityTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
