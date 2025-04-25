using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class TransactionTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int TransactionID { get; set; }

    [StringLength(100)]
    public string TransactionNo { get; set; } = null!;

    public int TransactionTypeID { get; set; }

    public int? TransactionSubTypeID { get; set; }

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

    public int? VendorID { get; set; }

    [StringLength(200)]
    public string? ServiceDoneBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? TransactionStartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? TransactionEndDate { get; set; }

    public int? SourceTransactionScheduleID { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("TransactionTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [InverseProperty("SourceTransaction")]
    public virtual ICollection<TransactionTable> InverseSourceTransaction { get; set; } = new List<TransactionTable>();

    [ForeignKey("PostedBy")]
    [InverseProperty("TransactionTablePostedByNavigation")]
    public virtual User_LoginUserTable? PostedByNavigation { get; set; }

    [ForeignKey("PostingStatusID")]
    [InverseProperty("TransactionTable")]
    public virtual PostingStatusTable? PostingStatus { get; set; } = null!;

    [ForeignKey("SourceTransactionID")]
    [InverseProperty("InverseSourceTransaction")]
    public virtual TransactionTable? SourceTransaction { get; set; }

    [ForeignKey("SourceTransactionScheduleID")]
    [InverseProperty("TransactionTable")]
    public virtual TransactionScheduleTable? SourceTransactionSchedule { get; set; }

    [ForeignKey("StatusID")]
    [InverseProperty("TransactionTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [InverseProperty("Transaction")]
    public virtual ICollection<TransactionLineItemTable> TransactionLineItemTable { get; set; } = new List<TransactionLineItemTable>();

    [InverseProperty("Transaction")]
    public virtual ICollection<TransactionScheduleTable> TransactionScheduleTable { get; set; } = new List<TransactionScheduleTable>();

    [ForeignKey("TransactionSubTypeID")]
    [InverseProperty("TransactionTable")]
    public virtual TransactionSubTypeTable? TransactionSubType { get; set; }

    [ForeignKey("TransactionTypeID")]
    [InverseProperty("TransactionTable")]
    public virtual TransactionTypeTable? TransactionType { get; set; } = null!;

    [ForeignKey("VendorID")]
    [InverseProperty("TransactionTable")]
    public virtual PartyTable? Vendor { get; set; }

    [ForeignKey("VerifiedBy")]
    [InverseProperty("TransactionTableVerifiedByNavigation")]
    public virtual User_LoginUserTable? VerifiedByNavigation { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public TransactionTable()
    {

    }

	/// <summary>
    /// Default constructor for TransactionTable
    /// </summary>
    /// <param name="db"></param>
	public TransactionTable(AMSContext _db)
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
		if(!string.IsNullOrEmpty(this.ReferenceNo)) this.ReferenceNo = this.ReferenceNo.Trim();
		if(!string.IsNullOrEmpty(this.CreatedFrom)) this.CreatedFrom = this.CreatedFrom.Trim();
		if(!string.IsNullOrEmpty(this.SourceDocumentNo)) this.SourceDocumentNo = this.SourceDocumentNo.Trim();
		if(!string.IsNullOrEmpty(this.Remarks)) this.Remarks = this.Remarks.Trim();
		if(!string.IsNullOrEmpty(this.ServiceDoneBy)) this.ServiceDoneBy = this.ServiceDoneBy.Trim();

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

    public static TransactionTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.TransactionTable
                where b.TransactionID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<TransactionTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.TransactionTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.TransactionID descending
                    select b);
        }
        else
        {
            return (from b in _db.TransactionTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.TransactionID descending
                    select b);
        }
    }

    public static IQueryable<TransactionTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return TransactionTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.TransactionTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return TransactionTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return TransactionTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
