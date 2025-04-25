using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ImportTemplateNewTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ImportTemplateID { get; set; }

    [StringLength(500)]
    public string ImportField { get; set; } = null!;

    public int DispalyOrderID { get; set; }

    public bool IsDisplay { get; set; }

    public bool IsMandatory { get; set; }

    public int DataSize { get; set; }

    [StringLength(50)]
    public string? DataFormat { get; set; }

    public bool IsForeignKey { get; set; }

    public int Width { get; set; }

    public int ImportTemplateTypeID { get; set; }

    public bool IsUnique { get; set; }

    public int? ReferenceEntityID { get; set; }

    public int EntityID { get; set; }

    [ForeignKey("EntityID")]
    [InverseProperty("ImportTemplateNewTable")]
    public virtual EntityTable? Entity { get; set; } = null!;

    [InverseProperty("ImportTemplate")]
    public virtual ICollection<ImportFormatLineItemNewTable> ImportFormatLineItemNewTable { get; set; } = new List<ImportFormatLineItemNewTable>();

    [ForeignKey("ImportTemplateTypeID")]
    [InverseProperty("ImportTemplateNewTable")]
    public virtual ImportTemplateTypeTable? ImportTemplateType { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ImportTemplateNewTable()
    {

    }

	/// <summary>
    /// Default constructor for ImportTemplateNewTable
    /// </summary>
    /// <param name="db"></param>
	public ImportTemplateNewTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "ImportTemplateID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ImportTemplateID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.ImportField)) this.ImportField = this.ImportField.Trim();
		if(!string.IsNullOrEmpty(this.DataFormat)) this.DataFormat = this.DataFormat.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static ImportTemplateNewTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ImportTemplateNewTable
                where b.ImportTemplateID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ImportTemplateNewTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.ImportTemplateNewTable select b);
    }

    public static IQueryable<ImportTemplateNewTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ImportTemplateNewTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ImportTemplateNewTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ImportTemplateNewTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ImportTemplateNewTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
