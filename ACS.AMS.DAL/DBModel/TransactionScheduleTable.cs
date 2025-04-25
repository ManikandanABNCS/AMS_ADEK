using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class TransactionScheduleTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int TransactionScheduleID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ActivityStartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ActivityEndDate { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string? Activity { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    public int? TransactionID { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("TransactionScheduleTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("TransactionScheduleTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [ForeignKey("StatusID")]
    [InverseProperty("TransactionScheduleTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [ForeignKey("TransactionID")]
    [InverseProperty("TransactionScheduleTable")]
    public virtual TransactionTable? Transaction { get; set; }

    [InverseProperty("SourceTransactionSchedule")]
    public virtual ICollection<TransactionTable> TransactionTable { get; set; } = new List<TransactionTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public TransactionScheduleTable()
    {

    }

	/// <summary>
    /// Default constructor for TransactionScheduleTable
    /// </summary>
    /// <param name="db"></param>
	public TransactionScheduleTable(AMSContext _db)
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
		return "TransactionScheduleID";
	}

	public override object GetPrimaryKeyValue()
	{
		return TransactionScheduleID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.Activity)) this.Activity = this.Activity.Trim();

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

    public static TransactionScheduleTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.TransactionScheduleTable
                where b.TransactionScheduleID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<TransactionScheduleTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.TransactionScheduleTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.TransactionScheduleID descending
                    select b);
        }
        else
        {
            return (from b in _db.TransactionScheduleTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.TransactionScheduleID descending
                    select b);
        }
    }

    public static IQueryable<TransactionScheduleTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return TransactionScheduleTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.TransactionScheduleTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return TransactionScheduleTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return TransactionScheduleTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
