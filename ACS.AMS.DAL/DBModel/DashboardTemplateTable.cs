using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class DashboardTemplateTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int DashboardTemplateID { get; set; }

    [StringLength(100)]
    public string DashboardTemplateName { get; set; } = null!;

    [StringLength(100)]
    public string QueryString { get; set; } = null!;

    public string? Remarks { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("DashboardTemplateTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [InverseProperty("DashboardTemplate")]
    public virtual ICollection<DashboardMappingTable> DashboardMappingTable { get; set; } = new List<DashboardMappingTable>();

    [InverseProperty("DashboardTemplate")]
    public virtual ICollection<DashboardTemplateFieldTable> DashboardTemplateFieldTable { get; set; } = new List<DashboardTemplateFieldTable>();

    [InverseProperty("DashboardTemplate")]
    public virtual ICollection<DashboardTemplateFilterFieldTable> DashboardTemplateFilterFieldTable { get; set; } = new List<DashboardTemplateFilterFieldTable>();

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("DashboardTemplateTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [ForeignKey("StatusID")]
    [InverseProperty("DashboardTemplateTable")]
    public virtual StatusTable? Status { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public DashboardTemplateTable()
    {

    }

	/// <summary>
    /// Default constructor for DashboardTemplateTable
    /// </summary>
    /// <param name="db"></param>
	public DashboardTemplateTable(AMSContext _db)
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
		return "DashboardTemplateID";
	}

	public override object GetPrimaryKeyValue()
	{
		return DashboardTemplateID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.DashboardTemplateName)) this.DashboardTemplateName = this.DashboardTemplateName.Trim();
		if(!string.IsNullOrEmpty(this.QueryString)) this.QueryString = this.QueryString.Trim();
		if(!string.IsNullOrEmpty(this.Remarks)) this.Remarks = this.Remarks.Trim();

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

    public static DashboardTemplateTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.DashboardTemplateTable
                where b.DashboardTemplateID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<DashboardTemplateTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.DashboardTemplateTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.DashboardTemplateID descending
                    select b);
        }
        else
        {
            return (from b in _db.DashboardTemplateTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.DashboardTemplateID descending
                    select b);
        }
    }

    public static IQueryable<DashboardTemplateTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return DashboardTemplateTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.DashboardTemplateTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return DashboardTemplateTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return DashboardTemplateTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
