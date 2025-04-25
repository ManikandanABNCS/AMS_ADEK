using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class AssetTransferCategoryHistoryTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int AssetTransferCategoryHistoryID { get; set; }

    public int AssetID { get; set; }

    public int OldCategoryID { get; set; }

    public int OldProductID { get; set; }

    public string OldAssetDescription { get; set; } = null!;

    public int NewCategoryID { get; set; }

    public int NewProductID { get; set; }

    public string NewAssetDescription { get; set; } = null!;

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int Createdby { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    [ForeignKey("AssetID")]
    [InverseProperty("AssetTransferCategoryHistoryTable")]
    public virtual AssetTable? Asset { get; set; } = null!;

    [ForeignKey("Createdby")]
    [InverseProperty("AssetTransferCategoryHistoryTable")]
    public virtual User_LoginUserTable? CreatedbyNavigation { get; set; } = null!;

    [ForeignKey("NewCategoryID")]
    [InverseProperty("AssetTransferCategoryHistoryTableNewCategory")]
    public virtual CategoryTable? NewCategory { get; set; } = null!;

    [ForeignKey("NewProductID")]
    [InverseProperty("AssetTransferCategoryHistoryTableNewProduct")]
    public virtual ProductTable? NewProduct { get; set; } = null!;

    [ForeignKey("OldCategoryID")]
    [InverseProperty("AssetTransferCategoryHistoryTableOldCategory")]
    public virtual CategoryTable? OldCategory { get; set; } = null!;

    [ForeignKey("OldProductID")]
    [InverseProperty("AssetTransferCategoryHistoryTableOldProduct")]
    public virtual ProductTable? OldProduct { get; set; } = null!;

    [ForeignKey("StatusID")]
    [InverseProperty("AssetTransferCategoryHistoryTable")]
    public virtual StatusTable? Status { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public AssetTransferCategoryHistoryTable()
    {

    }

	/// <summary>
    /// Default constructor for AssetTransferCategoryHistoryTable
    /// </summary>
    /// <param name="db"></param>
	public AssetTransferCategoryHistoryTable(AMSContext _db)
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
		return "AssetTransferCategoryHistoryID";
	}

	public override object GetPrimaryKeyValue()
	{
		return AssetTransferCategoryHistoryID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.OldAssetDescription)) this.OldAssetDescription = this.OldAssetDescription.Trim();
		if(!string.IsNullOrEmpty(this.NewAssetDescription)) this.NewAssetDescription = this.NewAssetDescription.Trim();

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

    public static AssetTransferCategoryHistoryTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.AssetTransferCategoryHistoryTable
                where b.AssetTransferCategoryHistoryID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<AssetTransferCategoryHistoryTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.AssetTransferCategoryHistoryTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.AssetTransferCategoryHistoryID descending
                    select b);
        }
        else
        {
            return (from b in _db.AssetTransferCategoryHistoryTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.AssetTransferCategoryHistoryID descending
                    select b);
        }
    }

    public static IQueryable<AssetTransferCategoryHistoryTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return AssetTransferCategoryHistoryTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.AssetTransferCategoryHistoryTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return AssetTransferCategoryHistoryTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return AssetTransferCategoryHistoryTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
