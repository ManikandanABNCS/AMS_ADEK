using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ReportTemplateCategoryTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ReportTemplateCategoryID { get; set; }

    [StringLength(150)]
    public string ReportTemplateCategoryName { get; set; } = null!;

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ReportTemplateCategoryTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("ReportTemplateCategoryTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [InverseProperty("ReportTemplateCategory")]
    public virtual ICollection<ReportTemplateTable> ReportTemplateTable { get; set; } = new List<ReportTemplateTable>();

    [ForeignKey("StatusID")]
    [InverseProperty("ReportTemplateCategoryTable")]
    public virtual StatusTable? Status { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ReportTemplateCategoryTable()
    {

    }

	/// <summary>
    /// Default constructor for ReportTemplateCategoryTable
    /// </summary>
    /// <param name="db"></param>
	public ReportTemplateCategoryTable(AMSContext _db)
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
		return "ReportTemplateCategoryID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ReportTemplateCategoryID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.ReportTemplateCategoryName)) this.ReportTemplateCategoryName = this.ReportTemplateCategoryName.Trim();

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

    public static ReportTemplateCategoryTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ReportTemplateCategoryTable
                where b.ReportTemplateCategoryID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ReportTemplateCategoryTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.ReportTemplateCategoryTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.ReportTemplateCategoryID descending
                    select b);
        }
        else
        {
            return (from b in _db.ReportTemplateCategoryTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.ReportTemplateCategoryID descending
                    select b);
        }
    }

    public static IQueryable<ReportTemplateCategoryTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ReportTemplateCategoryTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ReportTemplateCategoryTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ReportTemplateCategoryTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ReportTemplateCategoryTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
