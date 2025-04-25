using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class DisplayControlTypeTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public byte DisplayControlTypeID { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? ControlType { get; set; }

    public bool? String { get; set; }

    public bool? Integer { get; set; }

    public bool? Decimal { get; set; }

    public bool? Boolean { get; set; }

    public bool? Date { get; set; }

    public bool? Time { get; set; }

    public bool? DateTime { get; set; }

    [InverseProperty("DisplayControlNavigation")]
    public virtual ICollection<AttributeDefinitionTable> AttributeDefinitionTable { get; set; } = new List<AttributeDefinitionTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public DisplayControlTypeTable()
    {

    }

	/// <summary>
    /// Default constructor for DisplayControlTypeTable
    /// </summary>
    /// <param name="db"></param>
	public DisplayControlTypeTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "DisplayControlTypeID";
	}

	public override object GetPrimaryKeyValue()
	{
		return DisplayControlTypeID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.ControlType)) this.ControlType = this.ControlType.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static DisplayControlTypeTable GetItem(AMSContext _db, byte id)
    {
        return (from b in _db.DisplayControlTypeTable
                where b.DisplayControlTypeID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<DisplayControlTypeTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.DisplayControlTypeTable select b);
    }

    public static IQueryable<DisplayControlTypeTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return DisplayControlTypeTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.DisplayControlTypeTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return DisplayControlTypeTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return DisplayControlTypeTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
