using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ZDOF_PurchaseOrderTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ZDOFPurchaseOrderID { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? APPROVAL_STATUS { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? CLOSED_CODE { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? ITEM_DESCRIPTION { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? ITEM_ID { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? ITEM_NAME { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? LINE_NUM { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? PO_DATE { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? PO_DESC { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? PO_HEADER_ID { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? PO_LINE_DESC { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? PO_LINE_ID { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? PO_LINE_TYPE { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? PO_NUMBER { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? PO_TYPE { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? QUANTITY { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? VENDOR_ID { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? VENDOR_NAME { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? VENDOR_NUMBER { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? VENDOR_SITE_CODE { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? VENDOR_SITE_ID { get; set; }

    public int GeneratedAssetQty { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public bool AssetsCreated { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? AssetsCreatedDateTime { get; set; }

    [InverseProperty("ZDOFPurchaseOrder")]
    public virtual ICollection<ZDOF_UserPODataTable> ZDOF_UserPODataTable { get; set; } = new List<ZDOF_UserPODataTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ZDOF_PurchaseOrderTable()
    {

    }

	/// <summary>
    /// Default constructor for ZDOF_PurchaseOrderTable
    /// </summary>
    /// <param name="db"></param>
	public ZDOF_PurchaseOrderTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "ZDOFPurchaseOrderID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ZDOFPurchaseOrderID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.APPROVAL_STATUS)) this.APPROVAL_STATUS = this.APPROVAL_STATUS.Trim();
		if(!string.IsNullOrEmpty(this.CLOSED_CODE)) this.CLOSED_CODE = this.CLOSED_CODE.Trim();
		if(!string.IsNullOrEmpty(this.ITEM_DESCRIPTION)) this.ITEM_DESCRIPTION = this.ITEM_DESCRIPTION.Trim();
		if(!string.IsNullOrEmpty(this.ITEM_ID)) this.ITEM_ID = this.ITEM_ID.Trim();
		if(!string.IsNullOrEmpty(this.ITEM_NAME)) this.ITEM_NAME = this.ITEM_NAME.Trim();
		if(!string.IsNullOrEmpty(this.LINE_NUM)) this.LINE_NUM = this.LINE_NUM.Trim();
		if(!string.IsNullOrEmpty(this.PO_DESC)) this.PO_DESC = this.PO_DESC.Trim();
		if(!string.IsNullOrEmpty(this.PO_HEADER_ID)) this.PO_HEADER_ID = this.PO_HEADER_ID.Trim();
		if(!string.IsNullOrEmpty(this.PO_LINE_DESC)) this.PO_LINE_DESC = this.PO_LINE_DESC.Trim();
		if(!string.IsNullOrEmpty(this.PO_LINE_ID)) this.PO_LINE_ID = this.PO_LINE_ID.Trim();
		if(!string.IsNullOrEmpty(this.PO_LINE_TYPE)) this.PO_LINE_TYPE = this.PO_LINE_TYPE.Trim();
		if(!string.IsNullOrEmpty(this.PO_NUMBER)) this.PO_NUMBER = this.PO_NUMBER.Trim();
		if(!string.IsNullOrEmpty(this.PO_TYPE)) this.PO_TYPE = this.PO_TYPE.Trim();
		if(!string.IsNullOrEmpty(this.VENDOR_ID)) this.VENDOR_ID = this.VENDOR_ID.Trim();
		if(!string.IsNullOrEmpty(this.VENDOR_NAME)) this.VENDOR_NAME = this.VENDOR_NAME.Trim();
		if(!string.IsNullOrEmpty(this.VENDOR_NUMBER)) this.VENDOR_NUMBER = this.VENDOR_NUMBER.Trim();
		if(!string.IsNullOrEmpty(this.VENDOR_SITE_CODE)) this.VENDOR_SITE_CODE = this.VENDOR_SITE_CODE.Trim();
		if(!string.IsNullOrEmpty(this.VENDOR_SITE_ID)) this.VENDOR_SITE_ID = this.VENDOR_SITE_ID.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static ZDOF_PurchaseOrderTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ZDOF_PurchaseOrderTable
                where b.ZDOFPurchaseOrderID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ZDOF_PurchaseOrderTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.ZDOF_PurchaseOrderTable select b);
    }

    public static IQueryable<ZDOF_PurchaseOrderTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ZDOF_PurchaseOrderTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ZDOF_PurchaseOrderTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ZDOF_PurchaseOrderTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ZDOF_PurchaseOrderTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
