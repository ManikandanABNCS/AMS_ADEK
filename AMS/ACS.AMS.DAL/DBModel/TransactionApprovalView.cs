using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Keyless]
public partial class TransactionApprovalView : BaseEntityObject, IACSDBObject
{
    public int ApprovalHistoryID { get; set; }

    public int ApproveWorkFlowID { get; set; }

    public int ApproveWorkFlowLineItemID { get; set; }

    public int ApproveModuleID { get; set; }

    public int ApprovalRoleID { get; set; }

    public int ApprovalTransactionID { get; set; }

    public int OrderNo { get; set; }

    public string? ApprovalRemarks { get; set; }

    public int FromLocationID { get; set; }

    public int? ToLocationID { get; set; }

    public int FromLocationTypeID { get; set; }

    public int? ToLocationTypeID { get; set; }

    public int ApprovalStatusID { get; set; }

    public int ApprovalCreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime ApprovalCreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    public int? ObjectKeyID1 { get; set; }

    public string? EmailsecrectKey { get; set; }

    public int TransactionID { get; set; }

    [StringLength(100)]
    public string TransactionNo { get; set; } = null!;

    public int TransactionTypeID { get; set; }

    [StringLength(500)]
    public string? TransactionSubTypeName { get; set; }

    [StringLength(4000)]
    public string? ReferenceNo { get; set; }

    [StringLength(100)]
    public string? CreatedFrom { get; set; }

    public int? SourceTransactionID { get; set; }

    [StringLength(200)]
    public string? SourceDocumentNo { get; set; }

    public string? Remarks { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? TransactionDate { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? TransactionValue { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int PostingStatusID { get; set; }

    public int? VerifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? VerifiedDateTime { get; set; }

    public int? PostedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? PostedDateTime { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public long? SerialNo { get; set; }

    [StringLength(201)]
    public string CreatedUSer { get; set; } = null!;

    [StringLength(250)]
    public string? ApprovalStatus { get; set; }

    public bool? enableUpdate { get; set; }

    public int? UserID { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public TransactionApprovalView()
    {

    }

	/// <summary>
    /// Default constructor for TransactionApprovalView
    /// </summary>
    /// <param name="db"></param>
	public TransactionApprovalView(AMSContext _db)
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
		return "ApprovalHistoryID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ApprovalHistoryID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.ApprovalRemarks)) this.ApprovalRemarks = this.ApprovalRemarks.Trim();
		if(!string.IsNullOrEmpty(this.EmailsecrectKey)) this.EmailsecrectKey = this.EmailsecrectKey.Trim();
		if(!string.IsNullOrEmpty(this.TransactionNo)) this.TransactionNo = this.TransactionNo.Trim();
		if(!string.IsNullOrEmpty(this.TransactionSubTypeName)) this.TransactionSubTypeName = this.TransactionSubTypeName.Trim();
		if(!string.IsNullOrEmpty(this.ReferenceNo)) this.ReferenceNo = this.ReferenceNo.Trim();
		if(!string.IsNullOrEmpty(this.CreatedFrom)) this.CreatedFrom = this.CreatedFrom.Trim();
		if(!string.IsNullOrEmpty(this.SourceDocumentNo)) this.SourceDocumentNo = this.SourceDocumentNo.Trim();
		if(!string.IsNullOrEmpty(this.Remarks)) this.Remarks = this.Remarks.Trim();
		if(!string.IsNullOrEmpty(this.CreatedUSer)) this.CreatedUSer = this.CreatedUSer.Trim();
		if(!string.IsNullOrEmpty(this.ApprovalStatus)) this.ApprovalStatus = this.ApprovalStatus.Trim();

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

    public static TransactionApprovalView GetItem(AMSContext _db, int id)
    {
        return (from b in _db.TransactionApprovalView
                where b.ApprovalHistoryID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<TransactionApprovalView> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.TransactionApprovalView
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.ApprovalHistoryID descending
                    select b);
        }
        else
        {
            return (from b in _db.TransactionApprovalView
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.ApprovalHistoryID descending
                    select b);
        }
    }

    public static IQueryable<TransactionApprovalView> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return TransactionApprovalView.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.TransactionApprovalView select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return TransactionApprovalView.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return TransactionApprovalView.GetAllUserItems(_db, userID, includeInactiveItems);
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
