using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class PriceAnalysisLineItemTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int PriceAnalysisLineItemID { get; set; }

    public int PriceAnalysisID { get; set; }

    public int ProductID { get; set; }

    public int SupplierID { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal Price { get; set; }

    [ForeignKey("PriceAnalysisID")]
    [InverseProperty("PriceAnalysisLineItemTable")]
    public virtual PriceAnalysisTable? PriceAnalysis { get; set; } = null!;

    [ForeignKey("ProductID")]
    [InverseProperty("PriceAnalysisLineItemTable")]
    public virtual ProductTable? Product { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public PriceAnalysisLineItemTable()
    {

    }

	/// <summary>
    /// Default constructor for PriceAnalysisLineItemTable
    /// </summary>
    /// <param name="db"></param>
	public PriceAnalysisLineItemTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "PriceAnalysisLineItemID";
	}

	public override object GetPrimaryKeyValue()
	{
		return PriceAnalysisLineItemID;
	}

	internal override void BeforeSave(AMSContext db)
    {

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static PriceAnalysisLineItemTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.PriceAnalysisLineItemTable
                where b.PriceAnalysisLineItemID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<PriceAnalysisLineItemTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.PriceAnalysisLineItemTable select b);
    }

    public static IQueryable<PriceAnalysisLineItemTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return PriceAnalysisLineItemTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.PriceAnalysisLineItemTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return PriceAnalysisLineItemTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return PriceAnalysisLineItemTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
