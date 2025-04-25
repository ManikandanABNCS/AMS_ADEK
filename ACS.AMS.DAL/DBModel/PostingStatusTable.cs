using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class PostingStatusTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int PostingStatusID { get; set; }

    [StringLength(100)]
    public string PostingStatus { get; set; } = null!;

    [InverseProperty("PostingStatus")]
    public virtual ICollection<TransactionTable> TransactionTable { get; set; } = new List<TransactionTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public PostingStatusTable()
    {

    }

	/// <summary>
    /// Default constructor for PostingStatusTable
    /// </summary>
    /// <param name="db"></param>
	public PostingStatusTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "PostingStatusID";
	}

	public override object GetPrimaryKeyValue()
	{
		return PostingStatusID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.PostingStatus)) this.PostingStatus = this.PostingStatus.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static PostingStatusTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.PostingStatusTable
                where b.PostingStatusID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<PostingStatusTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.PostingStatusTable select b);
    }

    public static IQueryable<PostingStatusTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return PostingStatusTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.PostingStatusTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return PostingStatusTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return PostingStatusTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
