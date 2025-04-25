using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ZDOF_UserPODataTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ZDOFUserPODataID { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? PO_HEADER_ID { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? PO_LINE_ID { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? QUANTITY { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? UnitCost { get; set; }

    public int? CategoryID { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? PO_LINE_DESC { get; set; }

    public int ZDOFPurchaseOrderID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? DepartmentID { get; set; }

    public int? MergeLineNumber { get; set; }

    public int? LocationID { get; set; }

    public int? ProductID { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ZDOF_UserPODataTable")]
    public virtual PersonTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("DepartmentID")]
    [InverseProperty("ZDOF_UserPODataTable")]
    public virtual DepartmentTable? Department { get; set; }

    [ForeignKey("LocationID")]
    [InverseProperty("ZDOF_UserPODataTable")]
    public virtual LocationTable? Location { get; set; }

    [ForeignKey("ProductID")]
    [InverseProperty("ZDOF_UserPODataTable")]
    public virtual ProductTable? Product { get; set; }

    [ForeignKey("ZDOFPurchaseOrderID")]
    [InverseProperty("ZDOF_UserPODataTable")]
    public virtual ZDOF_PurchaseOrderTable? ZDOFPurchaseOrder { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ZDOF_UserPODataTable()
    {

    }

	/// <summary>
    /// Default constructor for ZDOF_UserPODataTable
    /// </summary>
    /// <param name="db"></param>
	public ZDOF_UserPODataTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "ZDOFUserPODataID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ZDOFUserPODataID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.PO_HEADER_ID)) this.PO_HEADER_ID = this.PO_HEADER_ID.Trim();
		if(!string.IsNullOrEmpty(this.PO_LINE_ID)) this.PO_LINE_ID = this.PO_LINE_ID.Trim();
		if(!string.IsNullOrEmpty(this.PO_LINE_DESC)) this.PO_LINE_DESC = this.PO_LINE_DESC.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static ZDOF_UserPODataTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ZDOF_UserPODataTable
                where b.ZDOFUserPODataID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ZDOF_UserPODataTable> GetAllItemsByDepartment(AMSContext _db, int departmentID )
    {
        return from b in GetAllItems(_db)
                where b.DepartmentID == departmentID
                select b;
    }

    public static IQueryable<ZDOF_UserPODataTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.ZDOF_UserPODataTable select b);
    }

    public static IQueryable<ZDOF_UserPODataTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return from b in ZDOF_UserPODataTable.GetAllItems(_db, includeInactiveItems)
                
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
        return (from b in _db.ZDOF_UserPODataTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ZDOF_UserPODataTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ZDOF_UserPODataTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
