using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class DashboardTemplateFilterFieldTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int DashboardTemplateFilterFieldID { get; set; }

    public int DashboardTemplateID { get; set; }

    [StringLength(200)]
    public string FieldName { get; set; } = null!;

    [StringLength(200)]
    public string DisplayTitle { get; set; } = null!;

    public int OrderNo { get; set; }

    public bool? IsMandatory { get; set; }

    public bool? IsFixedFilter { get; set; }

    public byte ScreenFilterTypeID { get; set; }

    public int FieldTypeID { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    [StringLength(500)]
    public string QueryField { get; set; } = null!;

    [ForeignKey("CreatedBy")]
    [InverseProperty("DashboardTemplateFilterFieldTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("DashboardTemplateID")]
    [InverseProperty("DashboardTemplateFilterFieldTable")]
    public virtual DashboardTemplateTable? DashboardTemplate { get; set; } = null!;

    [ForeignKey("FieldTypeID")]
    [InverseProperty("DashboardTemplateFilterFieldTable")]
    public virtual AFieldTypeTable? FieldType { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("DashboardTemplateFilterFieldTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [ForeignKey("ScreenFilterTypeID")]
    [InverseProperty("DashboardTemplateFilterFieldTable")]
    public virtual ScreenFilterTypeTable? ScreenFilterType { get; set; } = null!;

    [ForeignKey("StatusID")]
    [InverseProperty("DashboardTemplateFilterFieldTable")]
    public virtual StatusTable? Status { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public DashboardTemplateFilterFieldTable()
    {

    }

	/// <summary>
    /// Default constructor for DashboardTemplateFilterFieldTable
    /// </summary>
    /// <param name="db"></param>
	public DashboardTemplateFilterFieldTable(AMSContext _db)
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
		return "DashboardTemplateFilterFieldID";
	}

	public override object GetPrimaryKeyValue()
	{
		return DashboardTemplateFilterFieldID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.FieldName)) this.FieldName = this.FieldName.Trim();
		if(!string.IsNullOrEmpty(this.DisplayTitle)) this.DisplayTitle = this.DisplayTitle.Trim();
		if(!string.IsNullOrEmpty(this.QueryField)) this.QueryField = this.QueryField.Trim();

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

    public static DashboardTemplateFilterFieldTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.DashboardTemplateFilterFieldTable
                where b.DashboardTemplateFilterFieldID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<DashboardTemplateFilterFieldTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.DashboardTemplateFilterFieldTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.DashboardTemplateFilterFieldID descending
                    select b);
        }
        else
        {
            return (from b in _db.DashboardTemplateFilterFieldTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.DashboardTemplateFilterFieldID descending
                    select b);
        }
    }

    public static IQueryable<DashboardTemplateFilterFieldTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return DashboardTemplateFilterFieldTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.DashboardTemplateFilterFieldTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return DashboardTemplateFilterFieldTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return DashboardTemplateFilterFieldTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
