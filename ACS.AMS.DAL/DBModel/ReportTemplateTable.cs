using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ReportTemplateTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ReportTemplateID { get; set; }

    [StringLength(150)]
    public string ReportTemplateName { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string? ReportTemplateFile { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? QueryString { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    public int ReportTemplateCategoryID { get; set; }

    [StringLength(250)]
    public string? TemplateDescription { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? QueryType { get; set; }

    public int? RightID { get; set; }

    [StringLength(300)]
    public string? ProcedureName { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ReportTemplateTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("ReportTemplateTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [InverseProperty("ReportTemplate")]
    public virtual ICollection<NotificationModuleTable> NotificationModuleTable { get; set; } = new List<NotificationModuleTable>();

    [InverseProperty("ReportTemplate")]
    public virtual ICollection<ReportTable> ReportTable { get; set; } = new List<ReportTable>();

    [ForeignKey("ReportTemplateCategoryID")]
    [InverseProperty("ReportTemplateTable")]
    public virtual ReportTemplateCategoryTable? ReportTemplateCategory { get; set; } = null!;

    [InverseProperty("ReportTemplate")]
    public virtual ICollection<ReportTemplateFieldTable> ReportTemplateFieldTable { get; set; } = new List<ReportTemplateFieldTable>();

    [ForeignKey("RightID")]
    [InverseProperty("ReportTemplateTable")]
    public virtual User_RightTable? Right { get; set; }

    [InverseProperty("ReportTemplate")]
    public virtual ICollection<ScreenFilterTable> ScreenFilterTable { get; set; } = new List<ScreenFilterTable>();

    [ForeignKey("StatusID")]
    [InverseProperty("ReportTemplateTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [InverseProperty("ReportTemplate")]
    public virtual ICollection<UserReportFilterTable> UserReportFilterTable { get; set; } = new List<UserReportFilterTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ReportTemplateTable()
    {

    }

	/// <summary>
    /// Default constructor for ReportTemplateTable
    /// </summary>
    /// <param name="db"></param>
	public ReportTemplateTable(AMSContext _db)
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
		return "ReportTemplateID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ReportTemplateID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.ReportTemplateName)) this.ReportTemplateName = this.ReportTemplateName.Trim();
		if(!string.IsNullOrEmpty(this.ReportTemplateFile)) this.ReportTemplateFile = this.ReportTemplateFile.Trim();
		if(!string.IsNullOrEmpty(this.QueryString)) this.QueryString = this.QueryString.Trim();
		if(!string.IsNullOrEmpty(this.TemplateDescription)) this.TemplateDescription = this.TemplateDescription.Trim();
		if(!string.IsNullOrEmpty(this.QueryType)) this.QueryType = this.QueryType.Trim();
		if(!string.IsNullOrEmpty(this.ProcedureName)) this.ProcedureName = this.ProcedureName.Trim();

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

    public static ReportTemplateTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ReportTemplateTable
                where b.ReportTemplateID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ReportTemplateTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.ReportTemplateTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.ReportTemplateID descending
                    select b);
        }
        else
        {
            return (from b in _db.ReportTemplateTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.ReportTemplateID descending
                    select b);
        }
    }

    public static IQueryable<ReportTemplateTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ReportTemplateTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ReportTemplateTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ReportTemplateTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ReportTemplateTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
