using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ApprovalTransactionHistoryTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ApprovalTransactionHistoryID { get; set; }

    public int ApprovalHistoryID { get; set; }

    public string? Remarks { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    [ForeignKey("ApprovalHistoryID")]
    [InverseProperty("ApprovalTransactionHistoryTable")]
    public virtual ApprovalHistoryTable? ApprovalHistory { get; set; } = null!;

    [ForeignKey("CreatedBy")]
    [InverseProperty("ApprovalTransactionHistoryTable")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("StatusID")]
    [InverseProperty("ApprovalTransactionHistoryTable")]
    public virtual StatusTable? Status { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ApprovalTransactionHistoryTable()
    {

    }

	/// <summary>
    /// Default constructor for ApprovalTransactionHistoryTable
    /// </summary>
    /// <param name="db"></param>
	public ApprovalTransactionHistoryTable(AMSContext _db)
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
		return "ApprovalTransactionHistoryID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ApprovalTransactionHistoryID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.Remarks)) this.Remarks = this.Remarks.Trim();

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

    public static ApprovalTransactionHistoryTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ApprovalTransactionHistoryTable
                where b.ApprovalTransactionHistoryID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ApprovalTransactionHistoryTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.ApprovalTransactionHistoryTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.ApprovalTransactionHistoryID descending
                    select b);
        }
        else
        {
            return (from b in _db.ApprovalTransactionHistoryTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.ApprovalTransactionHistoryID descending
                    select b);
        }
    }

    public static IQueryable<ApprovalTransactionHistoryTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ApprovalTransactionHistoryTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ApprovalTransactionHistoryTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ApprovalTransactionHistoryTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ApprovalTransactionHistoryTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
