using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ReportPaperSizeTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ReportPaperSizeID { get; set; }

    [StringLength(100)]
    public string PaperSizeOrientation { get; set; } = null!;

    [Column(TypeName = "decimal(10, 6)")]
    public decimal PageWidth { get; set; }

    [Column(TypeName = "decimal(10, 6)")]
    public decimal PageHeight { get; set; }

    [InverseProperty("ReportPaperSize")]
    public virtual ICollection<ReportTable> ReportTable { get; set; } = new List<ReportTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ReportPaperSizeTable()
    {

    }

	/// <summary>
    /// Default constructor for ReportPaperSizeTable
    /// </summary>
    /// <param name="db"></param>
	public ReportPaperSizeTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "ReportPaperSizeID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ReportPaperSizeID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.PaperSizeOrientation)) this.PaperSizeOrientation = this.PaperSizeOrientation.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static ReportPaperSizeTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ReportPaperSizeTable
                where b.ReportPaperSizeID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ReportPaperSizeTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.ReportPaperSizeTable select b);
    }

    public static IQueryable<ReportPaperSizeTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ReportPaperSizeTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ReportPaperSizeTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ReportPaperSizeTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ReportPaperSizeTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
