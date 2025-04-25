using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class AssetimportDocumentHistoryTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int AssetimportDocumentHistoryID { get; set; }

    public string UploadDocPath { get; set; } = null!;

    [StringLength(10)]
    public string ImportType { get; set; } = null!;

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int UploadedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime UploadedDateTime { get; set; }

    [ForeignKey("StatusID")]
    [InverseProperty("AssetimportDocumentHistoryTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [ForeignKey("UploadedBy")]
    [InverseProperty("AssetimportDocumentHistoryTable")]
    public virtual User_LoginUserTable? UploadedByNavigation { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public AssetimportDocumentHistoryTable()
    {

    }

	/// <summary>
    /// Default constructor for AssetimportDocumentHistoryTable
    /// </summary>
    /// <param name="db"></param>
	public AssetimportDocumentHistoryTable(AMSContext _db)
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
		return "AssetimportDocumentHistoryID";
	}

	public override object GetPrimaryKeyValue()
	{
		return AssetimportDocumentHistoryID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.UploadDocPath)) this.UploadDocPath = this.UploadDocPath.Trim();
		if(!string.IsNullOrEmpty(this.ImportType)) this.ImportType = this.ImportType.Trim();

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

    public static AssetimportDocumentHistoryTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.AssetimportDocumentHistoryTable
                where b.AssetimportDocumentHistoryID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<AssetimportDocumentHistoryTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.AssetimportDocumentHistoryTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.AssetimportDocumentHistoryID descending
                    select b);
        }
        else
        {
            return (from b in _db.AssetimportDocumentHistoryTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.AssetimportDocumentHistoryID descending
                    select b);
        }
    }

    public static IQueryable<AssetimportDocumentHistoryTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return AssetimportDocumentHistoryTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.AssetimportDocumentHistoryTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return AssetimportDocumentHistoryTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return AssetimportDocumentHistoryTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
