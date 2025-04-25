using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class InvoiceTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int InvoiceID { get; set; }

    [StringLength(100)]
    public string InvoiceNo { get; set; } = null!;

    [Column(TypeName = "smalldatetime")]
    public DateTime InvoiceDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime ImvoiceDueDate { get; set; }

    public int PurchaseOrderID { get; set; }

    [StringLength(100)]
    public string SupplierInvoiceNo { get; set; } = null!;

    public string? PurposeofPayment { get; set; }

    public string? PaymentInstructions { get; set; }

    public int PaymentTypeID { get; set; }

    public int SupplierAccountDetailID { get; set; }

    public int CompanyID { get; set; }

    public int CurrencyID { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    [ForeignKey("CompanyID")]
    [InverseProperty("InvoiceTable")]
    public virtual CompanyTable? Company { get; set; } = null!;

    [ForeignKey("CreatedBy")]
    [InverseProperty("InvoiceTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("CurrencyID")]
    [InverseProperty("InvoiceTable")]
    public virtual CurrencyTable? Currency { get; set; } = null!;

    [InverseProperty("Invoice")]
    public virtual ICollection<InvoiceLineItemTable> InvoiceLineItemTable { get; set; } = new List<InvoiceLineItemTable>();

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("InvoiceTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [ForeignKey("PaymentTypeID")]
    [InverseProperty("InvoiceTable")]
    public virtual PaymentTypeTable? PaymentType { get; set; } = null!;

    [ForeignKey("PurchaseOrderID")]
    [InverseProperty("InvoiceTable")]
    public virtual PurchaseOrderTable? PurchaseOrder { get; set; } = null!;

    [ForeignKey("StatusID")]
    [InverseProperty("InvoiceTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [ForeignKey("SupplierAccountDetailID")]
    [InverseProperty("InvoiceTable")]
    public virtual SupplierAccountDetailTable? SupplierAccountDetail { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public InvoiceTable()
    {

    }

	/// <summary>
    /// Default constructor for InvoiceTable
    /// </summary>
    /// <param name="db"></param>
	public InvoiceTable(AMSContext _db)
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
		return "InvoiceID";
	}

	public override object GetPrimaryKeyValue()
	{
		return InvoiceID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.InvoiceNo)) this.InvoiceNo = this.InvoiceNo.Trim();
		if(!string.IsNullOrEmpty(this.SupplierInvoiceNo)) this.SupplierInvoiceNo = this.SupplierInvoiceNo.Trim();
		if(!string.IsNullOrEmpty(this.PurposeofPayment)) this.PurposeofPayment = this.PurposeofPayment.Trim();
		if(!string.IsNullOrEmpty(this.PaymentInstructions)) this.PaymentInstructions = this.PaymentInstructions.Trim();

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

    public static InvoiceTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.InvoiceTable
                where b.InvoiceID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<InvoiceTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.InvoiceTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.InvoiceID descending
                    select b);
        }
        else
        {
            return (from b in _db.InvoiceTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.InvoiceID descending
                    select b);
        }
    }

    public static IQueryable<InvoiceTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return InvoiceTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.InvoiceTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return InvoiceTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return InvoiceTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
