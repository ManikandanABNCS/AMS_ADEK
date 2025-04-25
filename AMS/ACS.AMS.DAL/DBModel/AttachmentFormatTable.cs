using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class AttachmentFormatTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int AttachmentFormatID { get; set; }

    [StringLength(100)]
    public string AttachmentFormatCode { get; set; } = null!;

    [StringLength(100)]
    public string AttachmentFormat { get; set; } = null!;

    [InverseProperty("AttachmentFormat")]
    public virtual ICollection<NotificationReportAttachmentTable> NotificationReportAttachmentTable { get; set; } = new List<NotificationReportAttachmentTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public AttachmentFormatTable()
    {

    }

	/// <summary>
    /// Default constructor for AttachmentFormatTable
    /// </summary>
    /// <param name="db"></param>
	public AttachmentFormatTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "AttachmentFormatID";
	}

	public override object GetPrimaryKeyValue()
	{
		return AttachmentFormatID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.AttachmentFormatCode)) this.AttachmentFormatCode = this.AttachmentFormatCode.Trim();
		if(!string.IsNullOrEmpty(this.AttachmentFormat)) this.AttachmentFormat = this.AttachmentFormat.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static AttachmentFormatTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.AttachmentFormatTable
                where b.AttachmentFormatID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<AttachmentFormatTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.AttachmentFormatTable select b);
    }

    public static IQueryable<AttachmentFormatTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return AttachmentFormatTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.AttachmentFormatTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return AttachmentFormatTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return AttachmentFormatTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
