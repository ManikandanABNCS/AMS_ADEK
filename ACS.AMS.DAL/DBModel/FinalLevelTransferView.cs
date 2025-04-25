using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Keyless]
public partial class FinalLevelTransferView : BaseEntityObject, IACSDBObject
{
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

    [StringLength(100)]
    public string TransactionNo { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public FinalLevelTransferView()
    {

    }

	/// <summary>
    /// Default constructor for FinalLevelTransferView
    /// </summary>
    /// <param name="db"></param>
	public FinalLevelTransferView(AMSContext _db)
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
		if(!string.IsNullOrEmpty(this.TransactionNo)) this.TransactionNo = this.TransactionNo.Trim();

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

    public static FinalLevelTransferView GetItem(AMSContext _db, int id)
    {
        return (from b in _db.FinalLevelTransferView
                where b.ApprovalHistoryID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<FinalLevelTransferView> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.FinalLevelTransferView
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.ApprovalHistoryID descending
                    select b);
        }
        else
        {
            return (from b in _db.FinalLevelTransferView
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.ApprovalHistoryID descending
                    select b);
        }
    }

    public static IQueryable<FinalLevelTransferView> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return FinalLevelTransferView.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.FinalLevelTransferView select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return FinalLevelTransferView.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return FinalLevelTransferView.GetAllUserItems(_db, userID, includeInactiveItems);
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
