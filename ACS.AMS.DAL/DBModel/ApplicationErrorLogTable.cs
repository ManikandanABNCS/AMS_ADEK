using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ApplicationErrorLogTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ApplicationErrorLogID { get; set; }

    [StringLength(4000)]
    public string? ErrorMessage { get; set; }

    [StringLength(4000)]
    public string? StackTrace { get; set; }

    [StringLength(4000)]
    public string? RequestData { get; set; }

    [StringLength(500)]
    public string? URI { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime OccuredDateTime { get; set; }

    [StringLength(1000)]
    public string? HostName { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ApplicationErrorLogTable()
    {

    }

	/// <summary>
    /// Default constructor for ApplicationErrorLogTable
    /// </summary>
    /// <param name="db"></param>
	public ApplicationErrorLogTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "ApplicationErrorLogID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ApplicationErrorLogID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.ErrorMessage)) this.ErrorMessage = this.ErrorMessage.Trim();
		if(!string.IsNullOrEmpty(this.StackTrace)) this.StackTrace = this.StackTrace.Trim();
		if(!string.IsNullOrEmpty(this.RequestData)) this.RequestData = this.RequestData.Trim();
		if(!string.IsNullOrEmpty(this.URI)) this.URI = this.URI.Trim();
		if(!string.IsNullOrEmpty(this.HostName)) this.HostName = this.HostName.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static ApplicationErrorLogTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ApplicationErrorLogTable
                where b.ApplicationErrorLogID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ApplicationErrorLogTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.ApplicationErrorLogTable select b);
    }

    public static IQueryable<ApplicationErrorLogTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ApplicationErrorLogTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ApplicationErrorLogTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ApplicationErrorLogTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ApplicationErrorLogTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
