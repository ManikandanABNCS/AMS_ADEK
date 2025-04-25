using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class RequestQuotationTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int RequestQuotationID { get; set; }

    [StringLength(200)]
    public string MeterialRequisitionNo { get; set; } = null!;

    [StringLength(200)]
    public string PreOrderNo { get; set; } = null!;

    [StringLength(200)]
    public string RequestForQuotationNo { get; set; } = null!;

    public int CurrencyID { get; set; }

    public int SupplierID { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime RequestForQuotationDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime RequestForQuotationClosingDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ExpectingDeliveryDate { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("RequestQuotationTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("CurrencyID")]
    [InverseProperty("RequestQuotationTable")]
    public virtual CurrencyTable? Currency { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("RequestQuotationTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [InverseProperty("RequestQuotation")]
    public virtual ICollection<RequestQuotationLineItemTable> RequestQuotationLineItemTable { get; set; } = new List<RequestQuotationLineItemTable>();

    [ForeignKey("StatusID")]
    [InverseProperty("RequestQuotationTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [ForeignKey("SupplierID")]
    [InverseProperty("RequestQuotationTable")]
    public virtual PartyTable? Supplier { get; set; } = null!;

    [InverseProperty("RequestQuotation")]
    public virtual ICollection<SupplierQuotationTable> SupplierQuotationTable { get; set; } = new List<SupplierQuotationTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public RequestQuotationTable()
    {

    }

	/// <summary>
    /// Default constructor for RequestQuotationTable
    /// </summary>
    /// <param name="db"></param>
	public RequestQuotationTable(AMSContext _db)
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
		return "RequestQuotationID";
	}

	public override object GetPrimaryKeyValue()
	{
		return RequestQuotationID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.MeterialRequisitionNo)) this.MeterialRequisitionNo = this.MeterialRequisitionNo.Trim();
		if(!string.IsNullOrEmpty(this.PreOrderNo)) this.PreOrderNo = this.PreOrderNo.Trim();
		if(!string.IsNullOrEmpty(this.RequestForQuotationNo)) this.RequestForQuotationNo = this.RequestForQuotationNo.Trim();

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

    public static RequestQuotationTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.RequestQuotationTable
                where b.RequestQuotationID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<RequestQuotationTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.RequestQuotationTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.RequestQuotationID descending
                    select b);
        }
        else
        {
            return (from b in _db.RequestQuotationTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.RequestQuotationID descending
                    select b);
        }
    }

    public static IQueryable<RequestQuotationTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return RequestQuotationTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.RequestQuotationTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return RequestQuotationTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return RequestQuotationTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
