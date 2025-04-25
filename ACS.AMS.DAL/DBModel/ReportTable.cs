using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ReportTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ReportID { get; set; }

    public int ReportTemplateID { get; set; }

    public int ReportPaperSizeID { get; set; }

    [StringLength(150)]
    public string ReportName { get; set; } = null!;

    [StringLength(150)]
    public string ReportTitle { get; set; } = null!;

    [Column(TypeName = "decimal(10, 6)")]
    public decimal ReportPageHeight { get; set; }

    [Column(TypeName = "decimal(10, 6)")]
    public decimal ReportPageWidth { get; set; }

    [Column(TypeName = "decimal(10, 6)")]
    public decimal ReportLeftMargin { get; set; }

    [Column(TypeName = "decimal(10, 6)")]
    public decimal ReportRightMargin { get; set; }

    [Column(TypeName = "decimal(10, 6)")]
    public decimal ReportTopMargin { get; set; }

    [Column(TypeName = "decimal(10, 6)")]
    public decimal ReportBottomMargin { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? RDLFileName { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ReportTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("ReportTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [InverseProperty("Report")]
    public virtual ICollection<NotificationReportAttachmentTable> NotificationReportAttachmentTable { get; set; } = new List<NotificationReportAttachmentTable>();

    [InverseProperty("Report")]
    public virtual ICollection<ReportFieldTable> ReportFieldTable { get; set; } = new List<ReportFieldTable>();

    [InverseProperty("Report")]
    public virtual ICollection<ReportGroupFieldTable> ReportGroupFieldTable { get; set; } = new List<ReportGroupFieldTable>();

    [ForeignKey("ReportPaperSizeID")]
    [InverseProperty("ReportTable")]
    public virtual ReportPaperSizeTable? ReportPaperSize { get; set; } = null!;

    [ForeignKey("ReportTemplateID")]
    [InverseProperty("ReportTable")]
    public virtual ReportTemplateTable? ReportTemplate { get; set; } = null!;

    [ForeignKey("StatusID")]
    [InverseProperty("ReportTable")]
    public virtual StatusTable? Status { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ReportTable()
    {

    }

	/// <summary>
    /// Default constructor for ReportTable
    /// </summary>
    /// <param name="db"></param>
	public ReportTable(AMSContext _db)
	{
		this.StatusID = (byte) StatusValue.Active;
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "ReportID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ReportID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.ReportName)) this.ReportName = this.ReportName.Trim();
		if(!string.IsNullOrEmpty(this.ReportTitle)) this.ReportTitle = this.ReportTitle.Trim();
		if(!string.IsNullOrEmpty(this.RDLFileName)) this.RDLFileName = this.RDLFileName.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		this.StatusID = (int) StatusValue.Deleted;
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static ReportTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ReportTable
                where b.ReportID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ReportTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.ReportTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.ReportID descending
                    select b);
        }
        else
        {
            return (from b in _db.ReportTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.ReportID descending
                    select b);
        }
    }

    public static IQueryable<ReportTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ReportTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ReportTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ReportTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ReportTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
