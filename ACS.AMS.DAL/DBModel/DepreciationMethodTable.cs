using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class DepreciationMethodTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int DepreciationMethodID { get; set; }

    [StringLength(200)]
    public string MethodName { get; set; } = null!;

    public string Description { get; set; } = null!;

    [InverseProperty("DepreciationMethod")]
    public virtual ICollection<DepreciationClassTable> DepreciationClassTable { get; set; } = new List<DepreciationClassTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public DepreciationMethodTable()
    {

    }

	/// <summary>
    /// Default constructor for DepreciationMethodTable
    /// </summary>
    /// <param name="db"></param>
	public DepreciationMethodTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "DepreciationMethodID";
	}

	public override object GetPrimaryKeyValue()
	{
		return DepreciationMethodID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.MethodName)) this.MethodName = this.MethodName.Trim();
		if(!string.IsNullOrEmpty(this.Description)) this.Description = this.Description.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static DepreciationMethodTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.DepreciationMethodTable
                where b.DepreciationMethodID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<DepreciationMethodTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.DepreciationMethodTable select b);
    }

    public static IQueryable<DepreciationMethodTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return DepreciationMethodTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.DepreciationMethodTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return DepreciationMethodTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return DepreciationMethodTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
