using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class DashboardMappingTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int DashboardMappingID { get; set; }

    public int DashboardTemplateID { get; set; }

    [StringLength(100)]
    public string DasboardMappingName { get; set; } = null!;

    public string DashboardBackGrounndColors { get; set; } = null!;

    [StringLength(100)]
    public string DashboardHeight { get; set; } = null!;

    [StringLength(100)]
    public string DashboardWeight { get; set; } = null!;

    public int DashboardTypeID { get; set; }

    [StringLength(100)]
    public string? XAxisTitle { get; set; }

    [StringLength(100)]
    public string? YAxisTitle { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("DashboardMappingTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [InverseProperty("DashboardMapping")]
    public virtual ICollection<DashboardFieldMappingTable> DashboardFieldMappingTable { get; set; } = new List<DashboardFieldMappingTable>();

    [ForeignKey("DashboardTemplateID")]
    [InverseProperty("DashboardMappingTable")]
    public virtual DashboardTemplateTable? DashboardTemplate { get; set; } = null!;

    [ForeignKey("DashboardTypeID")]
    [InverseProperty("DashboardMappingTable")]
    public virtual DashboardTypeTable? DashboardType { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("DashboardMappingTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [ForeignKey("StatusID")]
    [InverseProperty("DashboardMappingTable")]
    public virtual StatusTable? Status { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public DashboardMappingTable()
    {

    }

	/// <summary>
    /// Default constructor for DashboardMappingTable
    /// </summary>
    /// <param name="db"></param>
	public DashboardMappingTable(AMSContext _db)
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
		return "DashboardMappingID";
	}

	public override object GetPrimaryKeyValue()
	{
		return DashboardMappingID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.DasboardMappingName)) this.DasboardMappingName = this.DasboardMappingName.Trim();
		if(!string.IsNullOrEmpty(this.DashboardBackGrounndColors)) this.DashboardBackGrounndColors = this.DashboardBackGrounndColors.Trim();
		if(!string.IsNullOrEmpty(this.DashboardHeight)) this.DashboardHeight = this.DashboardHeight.Trim();
		if(!string.IsNullOrEmpty(this.DashboardWeight)) this.DashboardWeight = this.DashboardWeight.Trim();
		if(!string.IsNullOrEmpty(this.XAxisTitle)) this.XAxisTitle = this.XAxisTitle.Trim();
		if(!string.IsNullOrEmpty(this.YAxisTitle)) this.YAxisTitle = this.YAxisTitle.Trim();

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

    public static DashboardMappingTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.DashboardMappingTable
                where b.DashboardMappingID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<DashboardMappingTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.DashboardMappingTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.DashboardMappingID descending
                    select b);
        }
        else
        {
            return (from b in _db.DashboardMappingTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.DashboardMappingID descending
                    select b);
        }
    }

    public static IQueryable<DashboardMappingTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return DashboardMappingTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.DashboardMappingTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return DashboardMappingTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return DashboardMappingTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
