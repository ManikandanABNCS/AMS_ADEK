using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ItemReqestTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ItemRequestID { get; set; }

    public int ItemTypeID { get; set; }

    public int DepartmentID { get; set; }

    public int CurrencyID { get; set; }

    public int SupplierID { get; set; }

    public bool? IsProject { get; set; }

    public int? ProjectID { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ItemReqestTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("CurrencyID")]
    [InverseProperty("ItemReqestTable")]
    public virtual CurrencyTable? Currency { get; set; } = null!;

    [ForeignKey("DepartmentID")]
    [InverseProperty("ItemReqestTable")]
    public virtual DepartmentTable? Department { get; set; } = null!;

    [InverseProperty("ItemRequest")]
    public virtual ICollection<ItemReqestLineItemTable> ItemReqestLineItemTable { get; set; } = new List<ItemReqestLineItemTable>();

    [ForeignKey("ItemTypeID")]
    [InverseProperty("ItemReqestTable")]
    public virtual ItemTypeTable? ItemType { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("ItemReqestTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [ForeignKey("ProjectID")]
    [InverseProperty("ItemReqestTable")]
    public virtual ProjectTable? Project { get; set; }

    [ForeignKey("StatusID")]
    [InverseProperty("ItemReqestTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [ForeignKey("SupplierID")]
    [InverseProperty("ItemReqestTable")]
    public virtual PartyTable? Supplier { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ItemReqestTable()
    {

    }

	/// <summary>
    /// Default constructor for ItemReqestTable
    /// </summary>
    /// <param name="db"></param>
	public ItemReqestTable(AMSContext _db)
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
		return "ItemRequestID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ItemRequestID;
	}

	internal override void BeforeSave(AMSContext db)
    {

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

    public static ItemReqestTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ItemReqestTable
                where b.ItemRequestID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ItemReqestTable> GetAllItemsByDepartment(AMSContext _db, int departmentID , bool includeInactiveItems = false )
    {
        return from b in GetAllItems(_db, includeInactiveItems)
                where b.DepartmentID == departmentID
                select b;
    }

    public static IQueryable<ItemReqestTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.ItemReqestTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.ItemRequestID descending
                    select b);
        }
        else
        {
            return (from b in _db.ItemReqestTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.ItemRequestID descending
                    select b);
        }
    }

    public static IQueryable<ItemReqestTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return from b in ItemReqestTable.GetAllItems(_db, includeInactiveItems)
                
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
        return (from b in _db.ItemReqestTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ItemReqestTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ItemReqestTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
