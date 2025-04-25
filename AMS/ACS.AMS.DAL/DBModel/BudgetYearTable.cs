using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Index("BudgetYearCode", Name = "UC_BudgetYearCode", IsUnique = true)]
public partial class BudgetYearTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int BudgetYearID { get; set; }

    [StringLength(100)]
    public string BudgetYearCode { get; set; } = null!;

    [StringLength(100)]
    public string BudgetYear { get; set; } = null!;

    [Column(TypeName = "smalldatetime")]
    public DateTime BudgetStartDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime BudgetEndDate { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    [InverseProperty("BudgetYear")]
    public virtual ICollection<CategoryTable> CategoryTable { get; set; } = new List<CategoryTable>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("BudgetYearTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("BudgetYearTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [InverseProperty("BudgetYear")]
    public virtual ICollection<LocationTable> LocationTable { get; set; } = new List<LocationTable>();

    [ForeignKey("StatusID")]
    [InverseProperty("BudgetYearTable")]
    public virtual StatusTable? Status { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public BudgetYearTable()
    {

    }

	/// <summary>
    /// Default constructor for BudgetYearTable
    /// </summary>
    /// <param name="db"></param>
	public BudgetYearTable(AMSContext _db)
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
		return "BudgetYearID";
	}

	public override object GetPrimaryKeyValue()
	{
		return BudgetYearID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.BudgetYearCode)) this.BudgetYearCode = this.BudgetYearCode.Trim();
		if(!string.IsNullOrEmpty(this.BudgetYear)) this.BudgetYear = this.BudgetYear.Trim();

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

    public static BudgetYearTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.BudgetYearTable
                where b.BudgetYearID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<BudgetYearTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.BudgetYearTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.BudgetYearID descending
                    select b);
        }
        else
        {
            return (from b in _db.BudgetYearTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.BudgetYearID descending
                    select b);
        }
    }

    public static IQueryable<BudgetYearTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return BudgetYearTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.BudgetYearTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return BudgetYearTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return BudgetYearTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
