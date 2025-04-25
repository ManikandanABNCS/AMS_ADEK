using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class PurchaseOrderTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int PurchaseOrderID { get; set; }

    [StringLength(100)]
    public string? PurchaseOrderReferenceNo { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime PurchaseOrderDate { get; set; }

    public int PurchaseTypeID { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ExpectedDeliveryDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ShippingDate { get; set; }

    public string? ShippingTerms { get; set; }

    public string? PaymentTerms { get; set; }

    [StringLength(100)]
    public string? PaymentMethods { get; set; }

    public int CompanyID { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    public int DepartmentID { get; set; }

    [ForeignKey("CompanyID")]
    [InverseProperty("PurchaseOrderTable")]
    public virtual CompanyTable? Company { get; set; } = null!;

    [ForeignKey("CreatedBy")]
    [InverseProperty("PurchaseOrderTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("DepartmentID")]
    [InverseProperty("PurchaseOrderTable")]
    public virtual DepartmentTable? Department { get; set; } = null!;

    [InverseProperty("PurchaseOrder")]
    public virtual ICollection<GRNTable> GRNTable { get; set; } = new List<GRNTable>();

    [InverseProperty("PurchaseOrder")]
    public virtual ICollection<InvoiceTable> InvoiceTable { get; set; } = new List<InvoiceTable>();

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("PurchaseOrderTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [InverseProperty("PurchaseOrder")]
    public virtual ICollection<PurchaseOrderLineItemTable> PurchaseOrderLineItemTable { get; set; } = new List<PurchaseOrderLineItemTable>();

    [ForeignKey("PurchaseTypeID")]
    [InverseProperty("PurchaseOrderTable")]
    public virtual PurchaseTypeTable? PurchaseType { get; set; } = null!;

    [ForeignKey("StatusID")]
    [InverseProperty("PurchaseOrderTable")]
    public virtual StatusTable? Status { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public PurchaseOrderTable()
    {

    }

	/// <summary>
    /// Default constructor for PurchaseOrderTable
    /// </summary>
    /// <param name="db"></param>
	public PurchaseOrderTable(AMSContext _db)
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
		return "PurchaseOrderID";
	}

	public override object GetPrimaryKeyValue()
	{
		return PurchaseOrderID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.PurchaseOrderReferenceNo)) this.PurchaseOrderReferenceNo = this.PurchaseOrderReferenceNo.Trim();
		if(!string.IsNullOrEmpty(this.ShippingTerms)) this.ShippingTerms = this.ShippingTerms.Trim();
		if(!string.IsNullOrEmpty(this.PaymentTerms)) this.PaymentTerms = this.PaymentTerms.Trim();
		if(!string.IsNullOrEmpty(this.PaymentMethods)) this.PaymentMethods = this.PaymentMethods.Trim();

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

    public static PurchaseOrderTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.PurchaseOrderTable
                where b.PurchaseOrderID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<PurchaseOrderTable> GetAllItemsByDepartment(AMSContext _db, int departmentID , bool includeInactiveItems = false )
    {
        return from b in GetAllItems(_db, includeInactiveItems)
                where b.DepartmentID == departmentID
                select b;
    }

    public static IQueryable<PurchaseOrderTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.PurchaseOrderTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.PurchaseOrderID descending
                    select b);
        }
        else
        {
            return (from b in _db.PurchaseOrderTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.PurchaseOrderID descending
                    select b);
        }
    }

    public static IQueryable<PurchaseOrderTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return from b in PurchaseOrderTable.GetAllItems(_db, includeInactiveItems)
                
                select b;
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
        return (from b in _db.PurchaseOrderTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return PurchaseOrderTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return PurchaseOrderTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
