using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ReportGroupFieldTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ReportGroupFieldID { get; set; }

    public int ReportID { get; set; }

    public int ReportTemplateFieldID { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string DisplayTitle { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string? FieldFormat { get; set; }

    [Column(TypeName = "decimal(10, 6)")]
    public decimal FieldWidth { get; set; }

    public int DisplayOrderID { get; set; }

    public byte GroupLevel { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ReportGroupFieldTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("ReportGroupFieldTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [ForeignKey("ReportID")]
    [InverseProperty("ReportGroupFieldTable")]
    public virtual ReportTable? Report { get; set; } = null!;

    [ForeignKey("ReportTemplateFieldID")]
    [InverseProperty("ReportGroupFieldTable")]
    public virtual ReportTemplateFieldTable? ReportTemplateField { get; set; } = null!;

    [ForeignKey("StatusID")]
    [InverseProperty("ReportGroupFieldTable")]
    public virtual StatusTable? Status { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ReportGroupFieldTable()
    {

    }

	/// <summary>
    /// Default constructor for ReportGroupFieldTable
    /// </summary>
    /// <param name="db"></param>
	public ReportGroupFieldTable(AMSContext _db)
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
		return "ReportGroupFieldID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ReportGroupFieldID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.DisplayTitle)) this.DisplayTitle = this.DisplayTitle.Trim();
		if(!string.IsNullOrEmpty(this.FieldFormat)) this.FieldFormat = this.FieldFormat.Trim();

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

    public static ReportGroupFieldTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ReportGroupFieldTable
                where b.ReportGroupFieldID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ReportGroupFieldTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.ReportGroupFieldTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.ReportGroupFieldID descending
                    select b);
        }
        else
        {
            return (from b in _db.ReportGroupFieldTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.ReportGroupFieldID descending
                    select b);
        }
    }

    public static IQueryable<ReportGroupFieldTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ReportGroupFieldTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ReportGroupFieldTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ReportGroupFieldTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ReportGroupFieldTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
