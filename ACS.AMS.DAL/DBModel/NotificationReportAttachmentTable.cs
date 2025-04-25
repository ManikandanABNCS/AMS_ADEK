using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class NotificationReportAttachmentTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int NotificationReportAttachmentID { get; set; }

    public int NotificationTemplateID { get; set; }

    public int AttachmentFormatID { get; set; }

    public int ReportID { get; set; }

    [StringLength(50)]
    public string SourceField1 { get; set; } = null!;

    [StringLength(50)]
    public string SourceField2 { get; set; } = null!;

    [StringLength(50)]
    public string SourceField3 { get; set; } = null!;

    [StringLength(50)]
    public string DestinationField1 { get; set; } = null!;

    [StringLength(50)]
    public string DestinationField2 { get; set; } = null!;

    [StringLength(50)]
    public string DestinationField3 { get; set; } = null!;

    [ForeignKey("AttachmentFormatID")]
    [InverseProperty("NotificationReportAttachmentTable")]
    public virtual AttachmentFormatTable? AttachmentFormat { get; set; } = null!;

    [ForeignKey("NotificationTemplateID")]
    [InverseProperty("NotificationReportAttachmentTable")]
    public virtual NotificationTemplateTable? NotificationTemplate { get; set; } = null!;

    [ForeignKey("ReportID")]
    [InverseProperty("NotificationReportAttachmentTable")]
    public virtual ReportTable? Report { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public NotificationReportAttachmentTable()
    {

    }

	/// <summary>
    /// Default constructor for NotificationReportAttachmentTable
    /// </summary>
    /// <param name="db"></param>
	public NotificationReportAttachmentTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "NotificationReportAttachmentID";
	}

	public override object GetPrimaryKeyValue()
	{
		return NotificationReportAttachmentID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.SourceField1)) this.SourceField1 = this.SourceField1.Trim();
		if(!string.IsNullOrEmpty(this.SourceField2)) this.SourceField2 = this.SourceField2.Trim();
		if(!string.IsNullOrEmpty(this.SourceField3)) this.SourceField3 = this.SourceField3.Trim();
		if(!string.IsNullOrEmpty(this.DestinationField1)) this.DestinationField1 = this.DestinationField1.Trim();
		if(!string.IsNullOrEmpty(this.DestinationField2)) this.DestinationField2 = this.DestinationField2.Trim();
		if(!string.IsNullOrEmpty(this.DestinationField3)) this.DestinationField3 = this.DestinationField3.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static NotificationReportAttachmentTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.NotificationReportAttachmentTable
                where b.NotificationReportAttachmentID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<NotificationReportAttachmentTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.NotificationReportAttachmentTable select b);
    }

    public static IQueryable<NotificationReportAttachmentTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return NotificationReportAttachmentTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.NotificationReportAttachmentTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return NotificationReportAttachmentTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return NotificationReportAttachmentTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
