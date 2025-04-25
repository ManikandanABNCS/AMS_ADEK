using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ApprovalHistoryTable : BaseEntityObject, IACSDBObject
{
    [Key]
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

    public int? CategoryTypeID { get; set; }

    [ForeignKey("ApprovalRoleID")]
    [InverseProperty("ApprovalHistoryTable")]
    public virtual ApprovalRoleTable? ApprovalRole { get; set; } = null!;

    [InverseProperty("ApprovalHistory")]
    public virtual ICollection<ApprovalTransactionHistoryTable> ApprovalTransactionHistoryTable { get; set; } = new List<ApprovalTransactionHistoryTable>();

    [ForeignKey("ApproveModuleID")]
    [InverseProperty("ApprovalHistoryTable")]
    public virtual ApproveModuleTable? ApproveModule { get; set; } = null!;

    [ForeignKey("ApproveWorkFlowID")]
    [InverseProperty("ApprovalHistoryTable")]
    public virtual ApproveWorkflowTable? ApproveWorkFlow { get; set; } = null!;

    [ForeignKey("ApproveWorkFlowLineItemID")]
    [InverseProperty("ApprovalHistoryTable")]
    public virtual ApproveWorkflowLineItemTable? ApproveWorkFlowLineItem { get; set; } = null!;

    [ForeignKey("CategoryTypeID")]
    [InverseProperty("ApprovalHistoryTable")]
    public virtual CategoryTypeTable? CategoryType { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ApprovalHistoryTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("FromLocationID")]
    [InverseProperty("ApprovalHistoryTableFromLocation")]
    public virtual LocationTable? FromLocation { get; set; } = null!;

    [ForeignKey("FromLocationTypeID")]
    [InverseProperty("ApprovalHistoryTableFromLocationType")]
    public virtual LocationTypeTable? FromLocationType { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("ApprovalHistoryTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [ForeignKey("StatusID")]
    [InverseProperty("ApprovalHistoryTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [ForeignKey("ToLocationID")]
    [InverseProperty("ApprovalHistoryTableToLocation")]
    public virtual LocationTable? ToLocation { get; set; }

    [ForeignKey("ToLocationTypeID")]
    [InverseProperty("ApprovalHistoryTableToLocationType")]
    public virtual LocationTypeTable? ToLocationType { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ApprovalHistoryTable()
    {

    }

	/// <summary>
    /// Default constructor for ApprovalHistoryTable
    /// </summary>
    /// <param name="db"></param>
	public ApprovalHistoryTable(AMSContext _db)
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
		if(!string.IsNullOrEmpty(this.Remarks)) this.Remarks = this.Remarks.Trim();
		if(!string.IsNullOrEmpty(this.EmailsecrectKey)) this.EmailsecrectKey = this.EmailsecrectKey.Trim();

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

    public static ApprovalHistoryTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ApprovalHistoryTable
                where b.ApprovalHistoryID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ApprovalHistoryTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.ApprovalHistoryTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.ApprovalHistoryID descending
                    select b);
        }
        else
        {
            return (from b in _db.ApprovalHistoryTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.ApprovalHistoryID descending
                    select b);
        }
    }

    public static IQueryable<ApprovalHistoryTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ApprovalHistoryTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ApprovalHistoryTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ApprovalHistoryTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ApprovalHistoryTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
