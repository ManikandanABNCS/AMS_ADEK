using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class SupplierQuotationTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int SupplierQuotationID { get; set; }

    public int RequestQuotationID { get; set; }

    [StringLength(200)]
    public string QuotationReferenceNo { get; set; } = null!;

    [Column(TypeName = "smalldatetime")]
    public DateTime QuotationSubmittedDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime QuotationValidity { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("SupplierQuotationTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("SupplierQuotationTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [ForeignKey("RequestQuotationID")]
    [InverseProperty("SupplierQuotationTable")]
    public virtual RequestQuotationTable? RequestQuotation { get; set; } = null!;

    [ForeignKey("StatusID")]
    [InverseProperty("SupplierQuotationTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [InverseProperty("SupplierQuotation")]
    public virtual ICollection<SupplierQuotationLineItemTable> SupplierQuotationLineItemTable { get; set; } = new List<SupplierQuotationLineItemTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public SupplierQuotationTable()
    {

    }

	/// <summary>
    /// Default constructor for SupplierQuotationTable
    /// </summary>
    /// <param name="db"></param>
	public SupplierQuotationTable(AMSContext _db)
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
		return "SupplierQuotationID";
	}

	public override object GetPrimaryKeyValue()
	{
		return SupplierQuotationID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.QuotationReferenceNo)) this.QuotationReferenceNo = this.QuotationReferenceNo.Trim();

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

    public static SupplierQuotationTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.SupplierQuotationTable
                where b.SupplierQuotationID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<SupplierQuotationTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.SupplierQuotationTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.SupplierQuotationID descending
                    select b);
        }
        else
        {
            return (from b in _db.SupplierQuotationTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.SupplierQuotationID descending
                    select b);
        }
    }

    public static IQueryable<SupplierQuotationTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return SupplierQuotationTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.SupplierQuotationTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return SupplierQuotationTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return SupplierQuotationTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
