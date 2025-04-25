using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Index("PeriodID", Name = "FK_PeriodID")]
[Index("StatusID", Name = "FK_StatusID")]
[Index("PeriodID", Name = "IX_DepreciationTable_PeriodID")]
public partial class DepreciationTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int DepreciationID { get; set; }

    public int PeriodID { get; set; }

    public int DepreciationDoneBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime DepreciationDoneDate { get; set; }

    public bool? IsDepreciationApproval { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DepreciationApprovedDate { get; set; }

    public int? DepreciationApprovedBy { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public bool? IsDeletedApproval { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedApprovedDate { get; set; }

    public int? DeletedApprovedBy { get; set; }

    public int? DeletedDoneBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDoneDate { get; set; }

    public bool? IsReclassification { get; set; }

    public int? CompanyID { get; set; }

    public bool? IsUpdateIMALL { get; set; }

    public bool? IsUpdateEquation { get; set; }

    [ForeignKey("CompanyID")]
    [InverseProperty("DepreciationTable")]
    public virtual CompanyTable? Company { get; set; }

    [ForeignKey("DeletedApprovedBy")]
    [InverseProperty("DepreciationTableDeletedApprovedByNavigation")]
    public virtual User_LoginUserTable? DeletedApprovedByNavigation { get; set; }

    [ForeignKey("DeletedDoneBy")]
    [InverseProperty("DepreciationTableDeletedDoneByNavigation")]
    public virtual User_LoginUserTable? DeletedDoneByNavigation { get; set; }

    [ForeignKey("DepreciationApprovedBy")]
    [InverseProperty("DepreciationTableDepreciationApprovedByNavigation")]
    public virtual User_LoginUserTable? DepreciationApprovedByNavigation { get; set; }

    [ForeignKey("DepreciationDoneBy")]
    [InverseProperty("DepreciationTableDepreciationDoneByNavigation")]
    public virtual User_LoginUserTable? DepreciationDoneByNavigation { get; set; } = null!;

    [InverseProperty("Depreciation")]
    public virtual ICollection<DepreciationLineItemTable> DepreciationLineItemTable { get; set; } = new List<DepreciationLineItemTable>();

    [ForeignKey("PeriodID")]
    [InverseProperty("DepreciationTable")]
    public virtual PeriodTable? Period { get; set; } = null!;

    [ForeignKey("StatusID")]
    [InverseProperty("DepreciationTable")]
    public virtual StatusTable? Status { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public DepreciationTable()
    {

    }

	/// <summary>
    /// Default constructor for DepreciationTable
    /// </summary>
    /// <param name="db"></param>
	public DepreciationTable(AMSContext _db)
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
		return "DepreciationID";
	}

	public override object GetPrimaryKeyValue()
	{
		return DepreciationID;
	}

	internal override void BeforeSave(AMSContext db)
    {

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

    public static DepreciationTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.DepreciationTable
                where b.DepreciationID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<DepreciationTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.DepreciationTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.DepreciationID descending
                    select b);
        }
        else
        {
            return (from b in _db.DepreciationTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.DepreciationID descending
                    select b);
        }
    }

    public static IQueryable<DepreciationTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return DepreciationTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.DepreciationTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return DepreciationTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return DepreciationTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
