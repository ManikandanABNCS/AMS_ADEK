using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class BarcodeConstructSequenceTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int BarcodeSeqID { get; set; }

    [StringLength(2000)]
    public string? CategoryCode { get; set; }

    [StringLength(2000)]
    public string? Locationcode { get; set; }

    public int? LastSeqNo { get; set; }

    [StringLength(100)]
    public string? DepartmentCode { get; set; }

    [StringLength(100)]
    public string? SectionCode { get; set; }

    [StringLength(100)]
    public string? CustomCode { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public BarcodeConstructSequenceTable()
    {

    }

	/// <summary>
    /// Default constructor for BarcodeConstructSequenceTable
    /// </summary>
    /// <param name="db"></param>
	public BarcodeConstructSequenceTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "BarcodeSeqID";
	}

	public override object GetPrimaryKeyValue()
	{
		return BarcodeSeqID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.CategoryCode)) this.CategoryCode = this.CategoryCode.Trim();
		if(!string.IsNullOrEmpty(this.Locationcode)) this.Locationcode = this.Locationcode.Trim();
		if(!string.IsNullOrEmpty(this.DepartmentCode)) this.DepartmentCode = this.DepartmentCode.Trim();
		if(!string.IsNullOrEmpty(this.SectionCode)) this.SectionCode = this.SectionCode.Trim();
		if(!string.IsNullOrEmpty(this.CustomCode)) this.CustomCode = this.CustomCode.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static BarcodeConstructSequenceTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.BarcodeConstructSequenceTable
                where b.BarcodeSeqID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<BarcodeConstructSequenceTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.BarcodeConstructSequenceTable select b);
    }

    public static IQueryable<BarcodeConstructSequenceTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return BarcodeConstructSequenceTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.BarcodeConstructSequenceTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return BarcodeConstructSequenceTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return BarcodeConstructSequenceTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
