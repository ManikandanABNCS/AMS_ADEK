using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class LanguageContentLineItemTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int LanguageContentLineItemID { get; set; }

    public int LanguageContentID { get; set; }

    public int LanguageID { get; set; }

    public string LanguageContent { get; set; } = null!;

    [ForeignKey("LanguageID")]
    [InverseProperty("LanguageContentLineItemTable")]
    public virtual LanguageTable? Language { get; set; } = null!;

    [ForeignKey("LanguageContentID")]
    [InverseProperty("LanguageContentLineItemTable")]
    public virtual LanguageContentTable? LanguageContentNavigation { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public LanguageContentLineItemTable()
    {

    }

	/// <summary>
    /// Default constructor for LanguageContentLineItemTable
    /// </summary>
    /// <param name="db"></param>
	public LanguageContentLineItemTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "LanguageContentLineItemID";
	}

	public override object GetPrimaryKeyValue()
	{
		return LanguageContentLineItemID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.LanguageContent)) this.LanguageContent = this.LanguageContent.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static LanguageContentLineItemTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.LanguageContentLineItemTable
                where b.LanguageContentLineItemID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<LanguageContentLineItemTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.LanguageContentLineItemTable select b);
    }

    public static IQueryable<LanguageContentLineItemTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return LanguageContentLineItemTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.LanguageContentLineItemTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return LanguageContentLineItemTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return LanguageContentLineItemTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
