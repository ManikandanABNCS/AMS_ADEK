using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class GRNTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int GRNID { get; set; }

    [StringLength(100)]
    public string GRNNO { get; set; } = null!;

    [Column(TypeName = "smalldatetime")]
    public DateTime ReceivedOn { get; set; }

    public int PurchaseOrderID { get; set; }

    public int WarehouseID { get; set; }

    public int CompanyID { get; set; }

    public int CurrencyID { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? LaborCost { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? TransportedCost { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? ShippingCost { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? OtherCost { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    [ForeignKey("CompanyID")]
    [InverseProperty("GRNTable")]
    public virtual CompanyTable? Company { get; set; } = null!;

    [ForeignKey("CreatedBy")]
    [InverseProperty("GRNTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("CurrencyID")]
    [InverseProperty("GRNTable")]
    public virtual CurrencyTable? Currency { get; set; } = null!;

    [InverseProperty("GRN")]
    public virtual ICollection<GRNLineItemTable> GRNLineItemTable { get; set; } = new List<GRNLineItemTable>();

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("GRNTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [ForeignKey("PurchaseOrderID")]
    [InverseProperty("GRNTable")]
    public virtual PurchaseOrderTable? PurchaseOrder { get; set; } = null!;

    [ForeignKey("StatusID")]
    [InverseProperty("GRNTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [ForeignKey("WarehouseID")]
    [InverseProperty("GRNTable")]
    public virtual WarehouseTable? Warehouse { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public GRNTable()
    {

    }

	/// <summary>
    /// Default constructor for GRNTable
    /// </summary>
    /// <param name="db"></param>
	public GRNTable(AMSContext _db)
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
		return "GRNID";
	}

	public override object GetPrimaryKeyValue()
	{
		return GRNID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.GRNNO)) this.GRNNO = this.GRNNO.Trim();

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

    public static GRNTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.GRNTable
                where b.GRNID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<GRNTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.GRNTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.GRNID descending
                    select b);
        }
        else
        {
            return (from b in _db.GRNTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.GRNID descending
                    select b);
        }
    }

    public static IQueryable<GRNTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return GRNTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.GRNTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return GRNTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return GRNTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
