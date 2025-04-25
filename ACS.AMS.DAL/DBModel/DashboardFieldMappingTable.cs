using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class DashboardFieldMappingTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int DashboardFieldMappingID { get; set; }

    public int DashboardMappingID { get; set; }

    public int DashboardTemplateFieldID { get; set; }

    [StringLength(200)]
    public string FieldName { get; set; } = null!;

    [StringLength(200)]
    public string DisplayTitle { get; set; } = null!;

    [StringLength(200)]
    public string? ColorCode { get; set; }

    [StringLength(100)]
    public string? XAxisField { get; set; }

    [StringLength(100)]
    public string? YAxisField { get; set; }

    public string? IconPath { get; set; }

    [StringLength(100)]
    public string? RedirectPageName { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    public string? CategoriesField { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("DashboardFieldMappingTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("DashboardMappingID")]
    [InverseProperty("DashboardFieldMappingTable")]
    public virtual DashboardMappingTable? DashboardMapping { get; set; } = null!;

    [ForeignKey("DashboardTemplateFieldID")]
    [InverseProperty("DashboardFieldMappingTable")]
    public virtual DashboardTemplateFieldTable? DashboardTemplateField { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("DashboardFieldMappingTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [ForeignKey("StatusID")]
    [InverseProperty("DashboardFieldMappingTable")]
    public virtual StatusTable? Status { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public DashboardFieldMappingTable()
    {

    }

	/// <summary>
    /// Default constructor for DashboardFieldMappingTable
    /// </summary>
    /// <param name="db"></param>
	public DashboardFieldMappingTable(AMSContext _db)
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
		return "DashboardFieldMappingID";
	}

	public override object GetPrimaryKeyValue()
	{
		return DashboardFieldMappingID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.FieldName)) this.FieldName = this.FieldName.Trim();
		if(!string.IsNullOrEmpty(this.DisplayTitle)) this.DisplayTitle = this.DisplayTitle.Trim();
		if(!string.IsNullOrEmpty(this.ColorCode)) this.ColorCode = this.ColorCode.Trim();
		if(!string.IsNullOrEmpty(this.XAxisField)) this.XAxisField = this.XAxisField.Trim();
		if(!string.IsNullOrEmpty(this.YAxisField)) this.YAxisField = this.YAxisField.Trim();
		if(!string.IsNullOrEmpty(this.IconPath)) this.IconPath = this.IconPath.Trim();
		if(!string.IsNullOrEmpty(this.RedirectPageName)) this.RedirectPageName = this.RedirectPageName.Trim();
		if(!string.IsNullOrEmpty(this.CategoriesField)) this.CategoriesField = this.CategoriesField.Trim();

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

    public static DashboardFieldMappingTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.DashboardFieldMappingTable
                where b.DashboardFieldMappingID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<DashboardFieldMappingTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.DashboardFieldMappingTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.DashboardFieldMappingID descending
                    select b);
        }
        else
        {
            return (from b in _db.DashboardFieldMappingTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.DashboardFieldMappingID descending
                    select b);
        }
    }

    public static IQueryable<DashboardFieldMappingTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return DashboardFieldMappingTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.DashboardFieldMappingTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return DashboardFieldMappingTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return DashboardFieldMappingTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
