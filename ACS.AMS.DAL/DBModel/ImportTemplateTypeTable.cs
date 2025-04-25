using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ImportTemplateTypeTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ImportTemplateTypeID { get; set; }

    [StringLength(50)]
    public string ImportTemplateType { get; set; } = null!;

    [InverseProperty("ImportTemplateType")]
    public virtual ICollection<ImportFormatNewTable> ImportFormatNewTable { get; set; } = new List<ImportFormatNewTable>();

    [InverseProperty("ImportTemplateType")]
    public virtual ICollection<ImportTemplateNewTable> ImportTemplateNewTable { get; set; } = new List<ImportTemplateNewTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ImportTemplateTypeTable()
    {

    }

	/// <summary>
    /// Default constructor for ImportTemplateTypeTable
    /// </summary>
    /// <param name="db"></param>
	public ImportTemplateTypeTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "ImportTemplateTypeID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ImportTemplateTypeID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.ImportTemplateType)) this.ImportTemplateType = this.ImportTemplateType.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static ImportTemplateTypeTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ImportTemplateTypeTable
                where b.ImportTemplateTypeID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ImportTemplateTypeTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.ImportTemplateTypeTable select b);
    }

    public static IQueryable<ImportTemplateTypeTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ImportTemplateTypeTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ImportTemplateTypeTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ImportTemplateTypeTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ImportTemplateTypeTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
