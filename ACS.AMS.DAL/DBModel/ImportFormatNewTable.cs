using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ImportFormatNewTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ImportFormatID { get; set; }

    [StringLength(500)]
    public string TamplateName { get; set; } = null!;

    public string? FormatPath { get; set; }

    [StringLength(50)]
    public string FormatExtension { get; set; } = null!;

    public int ImportTemplateTypeID { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    public int EntityID { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ImportFormatNewTableCreatedByNavigation")]
    public virtual PersonTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("EntityID")]
    [InverseProperty("ImportFormatNewTable")]
    public virtual EntityTable? Entity { get; set; } = null!;

    [InverseProperty("ImportFormat")]
    public virtual ICollection<ImportFormatLineItemNewTable> ImportFormatLineItemNewTable { get; set; } = new List<ImportFormatLineItemNewTable>();

    [ForeignKey("ImportTemplateTypeID")]
    [InverseProperty("ImportFormatNewTable")]
    public virtual ImportTemplateTypeTable? ImportTemplateType { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("ImportFormatNewTableLastModifiedByNavigation")]
    public virtual PersonTable? LastModifiedByNavigation { get; set; }

    [ForeignKey("StatusID")]
    [InverseProperty("ImportFormatNewTable")]
    public virtual StatusTable? Status { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ImportFormatNewTable()
    {

    }

	/// <summary>
    /// Default constructor for ImportFormatNewTable
    /// </summary>
    /// <param name="db"></param>
	public ImportFormatNewTable(AMSContext _db)
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
		return "ImportFormatID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ImportFormatID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.TamplateName)) this.TamplateName = this.TamplateName.Trim();
		if(!string.IsNullOrEmpty(this.FormatPath)) this.FormatPath = this.FormatPath.Trim();
		if(!string.IsNullOrEmpty(this.FormatExtension)) this.FormatExtension = this.FormatExtension.Trim();

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

    public static ImportFormatNewTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ImportFormatNewTable
                where b.ImportFormatID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ImportFormatNewTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.ImportFormatNewTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.ImportFormatID descending
                    select b);
        }
        else
        {
            return (from b in _db.ImportFormatNewTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.ImportFormatID descending
                    select b);
        }
    }

    public static IQueryable<ImportFormatNewTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ImportFormatNewTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ImportFormatNewTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ImportFormatNewTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ImportFormatNewTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
