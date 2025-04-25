using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class UserReportFilterTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int UserReportFilterID { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? SearchTemplateName { get; set; }

    public int UserID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDateTime { get; set; }

    public bool? LastSelected { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int ReportTemplateID { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("UserReportFilterTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("ReportTemplateID")]
    [InverseProperty("UserReportFilterTable")]
    public virtual ReportTemplateTable? ReportTemplate { get; set; } = null!;

    [ForeignKey("StatusID")]
    [InverseProperty("UserReportFilterTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [ForeignKey("UserID")]
    [InverseProperty("UserReportFilterTableUser")]
    public virtual User_LoginUserTable? User { get; set; } = null!;

    [InverseProperty("UserReportFilter")]
    public virtual ICollection<UserReportFilterLineItemTable> UserReportFilterLineItemTable { get; set; } = new List<UserReportFilterLineItemTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public UserReportFilterTable()
    {

    }

	/// <summary>
    /// Default constructor for UserReportFilterTable
    /// </summary>
    /// <param name="db"></param>
	public UserReportFilterTable(AMSContext _db)
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
		return "UserReportFilterID";
	}

	public override object GetPrimaryKeyValue()
	{
		return UserReportFilterID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.SearchTemplateName)) this.SearchTemplateName = this.SearchTemplateName.Trim();

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

    public static UserReportFilterTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.UserReportFilterTable
                where b.UserReportFilterID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<UserReportFilterTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.UserReportFilterTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.UserReportFilterID descending
                    select b);
        }
        else
        {
            return (from b in _db.UserReportFilterTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.UserReportFilterID descending
                    select b);
        }
    }

    public static IQueryable<UserReportFilterTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return UserReportFilterTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.UserReportFilterTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return UserReportFilterTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return UserReportFilterTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
