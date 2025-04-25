using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ProductUOMMappingTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ProductUOMMappingID { get; set; }

    public int ProductID { get; set; }

    public int UOMID { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? ConversionQuantity { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProductUOMMappingTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("ProductUOMMappingTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [ForeignKey("ProductID")]
    [InverseProperty("ProductUOMMappingTable")]
    public virtual ProductTable? Product { get; set; } = null!;

    [ForeignKey("StatusID")]
    [InverseProperty("ProductUOMMappingTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [ForeignKey("UOMID")]
    [InverseProperty("ProductUOMMappingTable")]
    public virtual UOMTable? UOM { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ProductUOMMappingTable()
    {

    }

	/// <summary>
    /// Default constructor for ProductUOMMappingTable
    /// </summary>
    /// <param name="db"></param>
	public ProductUOMMappingTable(AMSContext _db)
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
		return "ProductUOMMappingID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ProductUOMMappingID;
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

    public static ProductUOMMappingTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ProductUOMMappingTable
                where b.ProductUOMMappingID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ProductUOMMappingTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.ProductUOMMappingTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.ProductUOMMappingID descending
                    select b);
        }
        else
        {
            return (from b in _db.ProductUOMMappingTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.ProductUOMMappingID descending
                    select b);
        }
    }

    public static IQueryable<ProductUOMMappingTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ProductUOMMappingTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ProductUOMMappingTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ProductUOMMappingTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ProductUOMMappingTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
