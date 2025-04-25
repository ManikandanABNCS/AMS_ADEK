using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class VATTypeTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int VATTypeID { get; set; }

    public string VATType { get; set; } = null!;

    [InverseProperty("VATType")]
    public virtual ICollection<VATTable> VATTable { get; set; } = new List<VATTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public VATTypeTable()
    {

    }

	/// <summary>
    /// Default constructor for VATTypeTable
    /// </summary>
    /// <param name="db"></param>
	public VATTypeTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "VATTypeID";
	}

	public override object GetPrimaryKeyValue()
	{
		return VATTypeID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.VATType)) this.VATType = this.VATType.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static VATTypeTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.VATTypeTable
                where b.VATTypeID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<VATTypeTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.VATTypeTable select b);
    }

    public static IQueryable<VATTypeTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return VATTypeTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.VATTypeTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return VATTypeTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return VATTypeTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
