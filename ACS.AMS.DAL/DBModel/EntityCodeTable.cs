using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class EntityCodeTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int EntityCodeID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string EntityCodeName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? CodePrefix { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CodeSuffix { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CodeFormatString { get; set; }

    public int LastUsedNo { get; set; }

    public bool UseDateTime { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public EntityCodeTable()
    {

    }

	/// <summary>
    /// Default constructor for EntityCodeTable
    /// </summary>
    /// <param name="db"></param>
	public EntityCodeTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "EntityCodeID";
	}

	public override object GetPrimaryKeyValue()
	{
		return EntityCodeID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.EntityCodeName)) this.EntityCodeName = this.EntityCodeName.Trim();
		if(!string.IsNullOrEmpty(this.CodePrefix)) this.CodePrefix = this.CodePrefix.Trim();
		if(!string.IsNullOrEmpty(this.CodeSuffix)) this.CodeSuffix = this.CodeSuffix.Trim();
		if(!string.IsNullOrEmpty(this.CodeFormatString)) this.CodeFormatString = this.CodeFormatString.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static EntityCodeTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.EntityCodeTable
                where b.EntityCodeID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<EntityCodeTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.EntityCodeTable select b);
    }

    public static IQueryable<EntityCodeTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return EntityCodeTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.EntityCodeTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return EntityCodeTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return EntityCodeTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
