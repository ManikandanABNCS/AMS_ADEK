using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ScreenFilterTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ScreenFilterID { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string ScreenFilterName { get; set; } = null!;

    public bool ShowDynamicFields { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? ParentID { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? ParentType { get; set; }

    public int ReportTemplateID { get; set; }

    [ForeignKey("ReportTemplateID")]
    [InverseProperty("ScreenFilterTable")]
    public virtual ReportTemplateTable? ReportTemplate { get; set; } = null!;

    [InverseProperty("ScreenFilter")]
    public virtual ICollection<ScreenFilterLineItemTable> ScreenFilterLineItemTable { get; set; } = new List<ScreenFilterLineItemTable>();

    [ForeignKey("StatusID")]
    [InverseProperty("ScreenFilterTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [InverseProperty("ScreenFilter")]
    public virtual ICollection<UserReportFilterLineItemTable> UserReportFilterLineItemTable { get; set; } = new List<UserReportFilterLineItemTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ScreenFilterTable()
    {

    }

	/// <summary>
    /// Default constructor for ScreenFilterTable
    /// </summary>
    /// <param name="db"></param>
	public ScreenFilterTable(AMSContext _db)
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
		return "ScreenFilterID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ScreenFilterID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.ScreenFilterName)) this.ScreenFilterName = this.ScreenFilterName.Trim();
		if(!string.IsNullOrEmpty(this.ParentType)) this.ParentType = this.ParentType.Trim();

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

    public static ScreenFilterTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ScreenFilterTable
                where b.ScreenFilterID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ScreenFilterTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.ScreenFilterTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.ScreenFilterID descending
                    select b);
        }
        else
        {
            return (from b in _db.ScreenFilterTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.ScreenFilterID descending
                    select b);
        }
    }

    public static IQueryable<ScreenFilterTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ScreenFilterTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ScreenFilterTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ScreenFilterTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ScreenFilterTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
