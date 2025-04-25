using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class NotificationInputTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int NotificationInputID { get; set; }

    public int NotificationTemplateID { get; set; }

    public long SYSDataID1 { get; set; }

    public long? SYSDataID2 { get; set; }

    public long? SYSDataID3 { get; set; }

    public bool IsCompleted { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? CompletedDateTime { get; set; }

    public int? SYSUserID { get; set; }

    [ForeignKey("NotificationTemplateID")]
    [InverseProperty("NotificationInputTable")]
    public virtual NotificationTemplateTable? NotificationTemplate { get; set; } = null!;

    [ForeignKey("SYSUserID")]
    [InverseProperty("NotificationInputTable")]
    public virtual User_LoginUserTable? SYSUser { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public NotificationInputTable()
    {

    }

	/// <summary>
    /// Default constructor for NotificationInputTable
    /// </summary>
    /// <param name="db"></param>
	public NotificationInputTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "NotificationInputID";
	}

	public override object GetPrimaryKeyValue()
	{
		return NotificationInputID;
	}

	internal override void BeforeSave(AMSContext db)
    {

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static NotificationInputTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.NotificationInputTable
                where b.NotificationInputID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<NotificationInputTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.NotificationInputTable select b);
    }

    public static IQueryable<NotificationInputTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return NotificationInputTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.NotificationInputTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return NotificationInputTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return NotificationInputTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
