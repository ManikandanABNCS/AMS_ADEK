using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class UserReportFilterLineItemTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int UserReportFilterLineItemID { get; set; }

    public int UserReportFilterID { get; set; }

    public int ScreenFilterID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? LogicalOperator { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? RelationalOperator { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? SelectedValue { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public string SelectedName { get; set; } = null!;

    [ForeignKey("CreatedBy")]
    [InverseProperty("UserReportFilterLineItemTable")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("ScreenFilterID")]
    [InverseProperty("UserReportFilterLineItemTable")]
    public virtual ScreenFilterTable? ScreenFilter { get; set; } = null!;

    [ForeignKey("StatusID")]
    [InverseProperty("UserReportFilterLineItemTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [ForeignKey("UserReportFilterID")]
    [InverseProperty("UserReportFilterLineItemTable")]
    public virtual UserReportFilterTable? UserReportFilter { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public UserReportFilterLineItemTable()
    {

    }

	/// <summary>
    /// Default constructor for UserReportFilterLineItemTable
    /// </summary>
    /// <param name="db"></param>
	public UserReportFilterLineItemTable(AMSContext _db)
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
		return "UserReportFilterLineItemID";
	}

	public override object GetPrimaryKeyValue()
	{
		return UserReportFilterLineItemID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.LogicalOperator)) this.LogicalOperator = this.LogicalOperator.Trim();
		if(!string.IsNullOrEmpty(this.RelationalOperator)) this.RelationalOperator = this.RelationalOperator.Trim();
		if(!string.IsNullOrEmpty(this.SelectedValue)) this.SelectedValue = this.SelectedValue.Trim();
		if(!string.IsNullOrEmpty(this.SelectedName)) this.SelectedName = this.SelectedName.Trim();

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

    public static UserReportFilterLineItemTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.UserReportFilterLineItemTable
                where b.UserReportFilterLineItemID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<UserReportFilterLineItemTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.UserReportFilterLineItemTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.UserReportFilterLineItemID descending
                    select b);
        }
        else
        {
            return (from b in _db.UserReportFilterLineItemTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.UserReportFilterLineItemID descending
                    select b);
        }
    }

    public static IQueryable<UserReportFilterLineItemTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return UserReportFilterLineItemTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.UserReportFilterLineItemTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return UserReportFilterLineItemTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return UserReportFilterLineItemTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
