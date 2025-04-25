using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Keyless]
public partial class ApprovalHistoryView : BaseEntityObject, IACSDBObject
{
    [StringLength(500)]
    public string ApprovalRoleName { get; set; } = null!;

    [StringLength(250)]
    public string? ApprovalStatus { get; set; }

    [StringLength(201)]
    public string? ApprovedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ApprovedDatetime { get; set; }

    public int ApprovalHistoryID { get; set; }

    public int ApproveWorkFlowID { get; set; }

    public int ApproveWorkFlowLineItemID { get; set; }

    public int ApproveModuleID { get; set; }

    public int ApprovalRoleID { get; set; }

    public int TransactionID { get; set; }

    public int OrderNo { get; set; }

    public string? Remarks { get; set; }

    public int FromLocationID { get; set; }

    public int? ToLocationID { get; set; }

    public int FromLocationTypeID { get; set; }

    public int? ToLocationTypeID { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    public int? ObjectKeyID1 { get; set; }

    public string? EmailsecrectKey { get; set; }

    public string? DocumentName { get; set; }

    public string? UserName { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ApprovalHistoryView()
    {

    }

	/// <summary>
    /// Default constructor for ApprovalHistoryView
    /// </summary>
    /// <param name="db"></param>
	public ApprovalHistoryView(AMSContext _db)
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
		return "ApprovalRoleName";
	}

	public override object GetPrimaryKeyValue()
	{
		return ApprovalRoleName;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.ApprovalRoleName)) this.ApprovalRoleName = this.ApprovalRoleName.Trim();
		if(!string.IsNullOrEmpty(this.ApprovalStatus)) this.ApprovalStatus = this.ApprovalStatus.Trim();
		if(!string.IsNullOrEmpty(this.ApprovedBy)) this.ApprovedBy = this.ApprovedBy.Trim();
		if(!string.IsNullOrEmpty(this.Remarks)) this.Remarks = this.Remarks.Trim();
		if(!string.IsNullOrEmpty(this.EmailsecrectKey)) this.EmailsecrectKey = this.EmailsecrectKey.Trim();
		if(!string.IsNullOrEmpty(this.DocumentName)) this.DocumentName = this.DocumentName.Trim();
		if(!string.IsNullOrEmpty(this.UserName)) this.UserName = this.UserName.Trim();

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

    public static ApprovalHistoryView GetItem(AMSContext _db, string id)
    {
        return (from b in _db.ApprovalHistoryView
                where b.ApprovalRoleName == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ApprovalHistoryView> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.ApprovalHistoryView
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.ApprovalRoleName descending
                    select b);
        }
        else
        {
            return (from b in _db.ApprovalHistoryView
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.ApprovalRoleName descending
                    select b);
        }
    }

    public static IQueryable<ApprovalHistoryView> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ApprovalHistoryView.GetAllItems(_db, includeInactiveItems);
    }

    public static bool DeleteItem(AMSContext _db, string id)
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
        return (from b in _db.ApprovalHistoryView select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ApprovalHistoryView.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ApprovalHistoryView.GetAllUserItems(_db, userID, includeInactiveItems);
    }

    BaseEntityObject IACSDBObject.GetItemByID(AMSContext _db, long itemID)
    {
        return GetItem(_db,  itemID+"");
    }

    bool IACSDBObject.DeleteObject()
    {
        this.Delete();

        return true;
    }

    #endregion Interface Methods
}
