using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class DocumentTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int DocumentID { get; set; }

    [StringLength(100)]
    public string DocumentName { get; set; } = null!;

    [StringLength(100)]
    public string TransactionType { get; set; } = null!;

    [StringLength(100)]
    public string FileName { get; set; } = null!;

    public int ObjectKeyID { get; set; }

    public string FilePath { get; set; } = null!;

    [StringLength(100)]
    public string FileExtension { get; set; } = null!;

    [DisplayName("Status")]
    public int StatusID { get; set; }

    [ForeignKey("StatusID")]
    [InverseProperty("DocumentTable")]
    public virtual StatusTable? Status { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public DocumentTable()
    {

    }

	/// <summary>
    /// Default constructor for DocumentTable
    /// </summary>
    /// <param name="db"></param>
	public DocumentTable(AMSContext _db)
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
		return "DocumentID";
	}

	public override object GetPrimaryKeyValue()
	{
		return DocumentID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.DocumentName)) this.DocumentName = this.DocumentName.Trim();
		if(!string.IsNullOrEmpty(this.TransactionType)) this.TransactionType = this.TransactionType.Trim();
		if(!string.IsNullOrEmpty(this.FileName)) this.FileName = this.FileName.Trim();
		if(!string.IsNullOrEmpty(this.FilePath)) this.FilePath = this.FilePath.Trim();
		if(!string.IsNullOrEmpty(this.FileExtension)) this.FileExtension = this.FileExtension.Trim();

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

    public static DocumentTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.DocumentTable
                where b.DocumentID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<DocumentTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.DocumentTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.DocumentID descending
                    select b);
        }
        else
        {
            return (from b in _db.DocumentTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.DocumentID descending
                    select b);
        }
    }

    public static IQueryable<DocumentTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return DocumentTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.DocumentTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return DocumentTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return DocumentTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
