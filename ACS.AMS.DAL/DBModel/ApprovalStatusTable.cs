using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Index("ApprovalStatus", Name = "UQ__Approval__CB1E562885DC9CD1", IsUnique = true)]
public partial class ApprovalStatusTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ApprovalStatusID { get; set; }

    [StringLength(250)]
    public string? ApprovalStatus { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ApprovalStatusTable()
    {

    }

	/// <summary>
    /// Default constructor for ApprovalStatusTable
    /// </summary>
    /// <param name="db"></param>
	public ApprovalStatusTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "ApprovalStatusID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ApprovalStatusID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.ApprovalStatus)) this.ApprovalStatus = this.ApprovalStatus.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static ApprovalStatusTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ApprovalStatusTable
                where b.ApprovalStatusID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ApprovalStatusTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.ApprovalStatusTable select b);
    }

    public static IQueryable<ApprovalStatusTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ApprovalStatusTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ApprovalStatusTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ApprovalStatusTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ApprovalStatusTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
