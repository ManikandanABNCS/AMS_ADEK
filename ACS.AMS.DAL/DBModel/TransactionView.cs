using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Keyless]
public partial class TransactionView : BaseEntityObject, IACSDBObject
{
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

    [StringLength(500)]
    public string TransactionTypeName { get; set; } = null!;

    [StringLength(250)]
    public string? Status { get; set; }

    [StringLength(201)]
    public string CreatedUSer { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public TransactionView()
    {

    }

	/// <summary>
    /// Default constructor for TransactionView
    /// </summary>
    /// <param name="db"></param>
	public TransactionView(AMSContext _db)
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
		return "TransactionID";
	}

	public override object GetPrimaryKeyValue()
	{
		return TransactionID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.TransactionNo)) this.TransactionNo = this.TransactionNo.Trim();
		if(!string.IsNullOrEmpty(this.TransactionSubTypeName)) this.TransactionSubTypeName = this.TransactionSubTypeName.Trim();
		if(!string.IsNullOrEmpty(this.ReferenceNo)) this.ReferenceNo = this.ReferenceNo.Trim();
		if(!string.IsNullOrEmpty(this.CreatedFrom)) this.CreatedFrom = this.CreatedFrom.Trim();
		if(!string.IsNullOrEmpty(this.SourceDocumentNo)) this.SourceDocumentNo = this.SourceDocumentNo.Trim();
		if(!string.IsNullOrEmpty(this.Remarks)) this.Remarks = this.Remarks.Trim();
		if(!string.IsNullOrEmpty(this.TransactionTypeName)) this.TransactionTypeName = this.TransactionTypeName.Trim();
		if(!string.IsNullOrEmpty(this.Status)) this.Status = this.Status.Trim();
		if(!string.IsNullOrEmpty(this.CreatedUSer)) this.CreatedUSer = this.CreatedUSer.Trim();

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

    public static TransactionView GetItem(AMSContext _db, int id)
    {
        return (from b in _db.TransactionView
                where b.TransactionID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<TransactionView> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.TransactionView
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.TransactionID descending
                    select b);
        }
        else
        {
            return (from b in _db.TransactionView
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.TransactionID descending
                    select b);
        }
    }

    public static IQueryable<TransactionView> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return TransactionView.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.TransactionView select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return TransactionView.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return TransactionView.GetAllUserItems(_db, userID, includeInactiveItems);
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
