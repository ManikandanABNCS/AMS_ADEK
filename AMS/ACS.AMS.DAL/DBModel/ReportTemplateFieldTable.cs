using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ReportTemplateFieldTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ReportTemplateFieldID { get; set; }

    public int? ReportTemplateID { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string FieldName { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string QueryFieldName { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string DisplayTitle { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string FieldDataType { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string? DefaultFormat { get; set; }

    [Column(TypeName = "decimal(10, 6)")]
    public decimal DefaultWidth { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? Expression { get; set; }

    public bool FooterSumRequired { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    public bool GroupSumRequired { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ReportTemplateFieldTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("ReportTemplateFieldTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [InverseProperty("ReportTemplateField")]
    public virtual ICollection<ReportFieldTable> ReportFieldTable { get; set; } = new List<ReportFieldTable>();

    [InverseProperty("ReportTemplateField")]
    public virtual ICollection<ReportGroupFieldTable> ReportGroupFieldTable { get; set; } = new List<ReportGroupFieldTable>();

    [ForeignKey("ReportTemplateID")]
    [InverseProperty("ReportTemplateFieldTable")]
    public virtual ReportTemplateTable? ReportTemplate { get; set; }

    [ForeignKey("StatusID")]
    [InverseProperty("ReportTemplateFieldTable")]
    public virtual StatusTable? Status { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ReportTemplateFieldTable()
    {

    }

	/// <summary>
    /// Default constructor for ReportTemplateFieldTable
    /// </summary>
    /// <param name="db"></param>
	public ReportTemplateFieldTable(AMSContext _db)
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
		return "ReportTemplateFieldID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ReportTemplateFieldID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.FieldName)) this.FieldName = this.FieldName.Trim();
		if(!string.IsNullOrEmpty(this.QueryFieldName)) this.QueryFieldName = this.QueryFieldName.Trim();
		if(!string.IsNullOrEmpty(this.DisplayTitle)) this.DisplayTitle = this.DisplayTitle.Trim();
		if(!string.IsNullOrEmpty(this.FieldDataType)) this.FieldDataType = this.FieldDataType.Trim();
		if(!string.IsNullOrEmpty(this.DefaultFormat)) this.DefaultFormat = this.DefaultFormat.Trim();
		if(!string.IsNullOrEmpty(this.Expression)) this.Expression = this.Expression.Trim();

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

    public static ReportTemplateFieldTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ReportTemplateFieldTable
                where b.ReportTemplateFieldID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ReportTemplateFieldTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.ReportTemplateFieldTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.ReportTemplateFieldID descending
                    select b);
        }
        else
        {
            return (from b in _db.ReportTemplateFieldTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.ReportTemplateFieldID descending
                    select b);
        }
    }

    public static IQueryable<ReportTemplateFieldTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ReportTemplateFieldTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ReportTemplateFieldTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ReportTemplateFieldTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ReportTemplateFieldTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
