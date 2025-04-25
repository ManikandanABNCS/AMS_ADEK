using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ItemDispatchTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ItemDispatchID { get; set; }

    [StringLength(100)]
    public string PurchaseOrderID { get; set; } = null!;

    [Column(TypeName = "smalldatetime")]
    public DateTime ReceivedOn { get; set; }

    public int WarehouseID { get; set; }

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
    [InverseProperty("ItemDispatchTable")]
    public virtual CompanyTable? Company { get; set; } = null!;

    [ForeignKey("CreatedBy")]
    [InverseProperty("ItemDispatchTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("CurrencyID")]
    [InverseProperty("ItemDispatchTable")]
    public virtual CurrencyTable? Currency { get; set; } = null!;

    [InverseProperty("ItemDispatch")]
    public virtual ICollection<ItemDispatchLineItemTable> ItemDispatchLineItemTable { get; set; } = new List<ItemDispatchLineItemTable>();

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("ItemDispatchTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [ForeignKey("StatusID")]
    [InverseProperty("ItemDispatchTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [ForeignKey("WarehouseID")]
    [InverseProperty("ItemDispatchTable")]
    public virtual WarehouseTable? Warehouse { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ItemDispatchTable()
    {

    }

	/// <summary>
    /// Default constructor for ItemDispatchTable
    /// </summary>
    /// <param name="db"></param>
	public ItemDispatchTable(AMSContext _db)
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
		return "ItemDispatchID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ItemDispatchID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.PurchaseOrderID)) this.PurchaseOrderID = this.PurchaseOrderID.Trim();

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

    public static ItemDispatchTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ItemDispatchTable
                where b.ItemDispatchID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ItemDispatchTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.ItemDispatchTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.ItemDispatchID descending
                    select b);
        }
        else
        {
            return (from b in _db.ItemDispatchTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.ItemDispatchID descending
                    select b);
        }
    }

    public static IQueryable<ItemDispatchTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ItemDispatchTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ItemDispatchTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ItemDispatchTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ItemDispatchTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
