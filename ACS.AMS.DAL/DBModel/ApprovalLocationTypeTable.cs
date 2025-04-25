using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ApprovalLocationTypeTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ApprovalLocationTypeID { get; set; }

    [StringLength(100)]
    public string ApprovalLocationTypeCode { get; set; } = null!;

    [StringLength(100)]
    public string ApprovalLocationTypeName { get; set; } = null!;

    [InverseProperty("ApprovalLocationType")]
    public virtual ICollection<ApprovalRoleTable> ApprovalRoleTable { get; set; } = new List<ApprovalRoleTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ApprovalLocationTypeTable()
    {

    }

	/// <summary>
    /// Default constructor for ApprovalLocationTypeTable
    /// </summary>
    /// <param name="db"></param>
	public ApprovalLocationTypeTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "ApprovalLocationTypeID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ApprovalLocationTypeID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.ApprovalLocationTypeCode)) this.ApprovalLocationTypeCode = this.ApprovalLocationTypeCode.Trim();
		if(!string.IsNullOrEmpty(this.ApprovalLocationTypeName)) this.ApprovalLocationTypeName = this.ApprovalLocationTypeName.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static ApprovalLocationTypeTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ApprovalLocationTypeTable
                where b.ApprovalLocationTypeID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ApprovalLocationTypeTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.ApprovalLocationTypeTable select b);
    }

    public static IQueryable<ApprovalLocationTypeTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ApprovalLocationTypeTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ApprovalLocationTypeTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ApprovalLocationTypeTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ApprovalLocationTypeTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
