using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class WebServiceLogTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public long WebServiceLogID { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? MethodName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RequestedDateTime { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? DeviceIMEI { get; set; }

    public int? UserID { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? AppVersion { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? WarehouseCode { get; set; }

    [Unicode(false)]
    public string? Parameters { get; set; }

    public int? TimeTakenToCompleteService { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? Response { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public WebServiceLogTable()
    {

    }

	/// <summary>
    /// Default constructor for WebServiceLogTable
    /// </summary>
    /// <param name="db"></param>
	public WebServiceLogTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "WebServiceLogID";
	}

	public override object GetPrimaryKeyValue()
	{
		return WebServiceLogID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.MethodName)) this.MethodName = this.MethodName.Trim();
		if(!string.IsNullOrEmpty(this.DeviceIMEI)) this.DeviceIMEI = this.DeviceIMEI.Trim();
		if(!string.IsNullOrEmpty(this.AppVersion)) this.AppVersion = this.AppVersion.Trim();
		if(!string.IsNullOrEmpty(this.WarehouseCode)) this.WarehouseCode = this.WarehouseCode.Trim();
		if(!string.IsNullOrEmpty(this.Parameters)) this.Parameters = this.Parameters.Trim();
		if(!string.IsNullOrEmpty(this.Response)) this.Response = this.Response.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static WebServiceLogTable GetItem(AMSContext _db, long id)
    {
        return (from b in _db.WebServiceLogTable
                where b.WebServiceLogID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<WebServiceLogTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.WebServiceLogTable select b);
    }

    public static IQueryable<WebServiceLogTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return WebServiceLogTable.GetAllItems(_db, includeInactiveItems);
    }

    public static bool DeleteItem(AMSContext _db, long id)
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
        return (from b in _db.WebServiceLogTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return WebServiceLogTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return WebServiceLogTable.GetAllUserItems(_db, userID, includeInactiveItems);
    }

    BaseEntityObject IACSDBObject.GetItemByID(AMSContext _db, long itemID)
    {
        return GetItem(_db, (long) itemID);
    }

    bool IACSDBObject.DeleteObject()
    {
        this.Delete();

        return true;
    }

    #endregion Interface Methods
}
