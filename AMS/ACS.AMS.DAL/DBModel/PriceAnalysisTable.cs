using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class PriceAnalysisTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int PriceAnalysisID { get; set; }

    [StringLength(200)]
    public string PreOrderNo { get; set; } = null!;

    [Column(TypeName = "smalldatetime")]
    public DateTime PreOrderDate { get; set; }

    public bool? isPurchaseContact { get; set; }

    public int DeliveredWarehouseID { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DeliveryDate { get; set; }

    public string? Remarks { get; set; }

    public string? SpecialInstructions { get; set; }

    public string? PaymentTerms { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("PriceAnalysisTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("DeliveredWarehouseID")]
    [InverseProperty("PriceAnalysisTable")]
    public virtual WarehouseTable? DeliveredWarehouse { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("PriceAnalysisTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [InverseProperty("PriceAnalysis")]
    public virtual ICollection<PriceAnalysisLineItemTable> PriceAnalysisLineItemTable { get; set; } = new List<PriceAnalysisLineItemTable>();

    [ForeignKey("StatusID")]
    [InverseProperty("PriceAnalysisTable")]
    public virtual StatusTable? Status { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public PriceAnalysisTable()
    {

    }

	/// <summary>
    /// Default constructor for PriceAnalysisTable
    /// </summary>
    /// <param name="db"></param>
	public PriceAnalysisTable(AMSContext _db)
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
		return "PriceAnalysisID";
	}

	public override object GetPrimaryKeyValue()
	{
		return PriceAnalysisID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.PreOrderNo)) this.PreOrderNo = this.PreOrderNo.Trim();
		if(!string.IsNullOrEmpty(this.Remarks)) this.Remarks = this.Remarks.Trim();
		if(!string.IsNullOrEmpty(this.SpecialInstructions)) this.SpecialInstructions = this.SpecialInstructions.Trim();
		if(!string.IsNullOrEmpty(this.PaymentTerms)) this.PaymentTerms = this.PaymentTerms.Trim();

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

    public static PriceAnalysisTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.PriceAnalysisTable
                where b.PriceAnalysisID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<PriceAnalysisTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.PriceAnalysisTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.PriceAnalysisID descending
                    select b);
        }
        else
        {
            return (from b in _db.PriceAnalysisTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.PriceAnalysisID descending
                    select b);
        }
    }

    public static IQueryable<PriceAnalysisTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return PriceAnalysisTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.PriceAnalysisTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return PriceAnalysisTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return PriceAnalysisTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
