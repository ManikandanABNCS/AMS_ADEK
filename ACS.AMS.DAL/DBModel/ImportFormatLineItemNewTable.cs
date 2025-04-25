using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ImportFormatLineItemNewTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ImportFormatLineItemID { get; set; }

    public int ImportFormatID { get; set; }

    public int ImportTemplateID { get; set; }

    public int DisplayOrderID { get; set; }

    [ForeignKey("ImportFormatID")]
    [InverseProperty("ImportFormatLineItemNewTable")]
    public virtual ImportFormatNewTable? ImportFormat { get; set; } = null!;

    [ForeignKey("ImportTemplateID")]
    [InverseProperty("ImportFormatLineItemNewTable")]
    public virtual ImportTemplateNewTable? ImportTemplate { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ImportFormatLineItemNewTable()
    {

    }

	/// <summary>
    /// Default constructor for ImportFormatLineItemNewTable
    /// </summary>
    /// <param name="db"></param>
	public ImportFormatLineItemNewTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "ImportFormatLineItemID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ImportFormatLineItemID;
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

    public static ImportFormatLineItemNewTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ImportFormatLineItemNewTable
                where b.ImportFormatLineItemID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ImportFormatLineItemNewTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.ImportFormatLineItemNewTable select b);
    }

    public static IQueryable<ImportFormatLineItemNewTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ImportFormatLineItemNewTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ImportFormatLineItemNewTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ImportFormatLineItemNewTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ImportFormatLineItemNewTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
