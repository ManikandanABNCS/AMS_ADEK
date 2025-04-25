using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class BarcodeConstructTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int BarcodeConstructID { get; set; }

    [StringLength(2000)]
    public string? CategoryCode { get; set; }

    [StringLength(2000)]
    public string? LocationCode { get; set; }

    [StringLength(100)]
    public string? DepartmentCode { get; set; }

    [StringLength(100)]
    public string? SectionCode { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    public int? CategoryID { get; set; }

    public int? LocationID { get; set; }

    public int? DepartmentID { get; set; }

    public int? SectionID { get; set; }

    public int? CustomPrefixLength { get; set; }

    [StringLength(100)]
    public string? CustomPrefix { get; set; }

    [ForeignKey("CategoryID")]
    [InverseProperty("BarcodeConstructTable")]
    public virtual CategoryTable? Category { get; set; }

    [ForeignKey("DepartmentID")]
    [InverseProperty("BarcodeConstructTable")]
    public virtual DepartmentTable? Department { get; set; }

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("BarcodeConstructTable")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [ForeignKey("LocationID")]
    [InverseProperty("BarcodeConstructTable")]
    public virtual LocationTable? Location { get; set; }

    [ForeignKey("SectionID")]
    [InverseProperty("BarcodeConstructTable")]
    public virtual SectionTable? Section { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public BarcodeConstructTable()
    {

    }

	/// <summary>
    /// Default constructor for BarcodeConstructTable
    /// </summary>
    /// <param name="db"></param>
	public BarcodeConstructTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "BarcodeConstructID";
	}

	public override object GetPrimaryKeyValue()
	{
		return BarcodeConstructID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.CategoryCode)) this.CategoryCode = this.CategoryCode.Trim();
		if(!string.IsNullOrEmpty(this.LocationCode)) this.LocationCode = this.LocationCode.Trim();
		if(!string.IsNullOrEmpty(this.DepartmentCode)) this.DepartmentCode = this.DepartmentCode.Trim();
		if(!string.IsNullOrEmpty(this.SectionCode)) this.SectionCode = this.SectionCode.Trim();
		if(!string.IsNullOrEmpty(this.CustomPrefix)) this.CustomPrefix = this.CustomPrefix.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static BarcodeConstructTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.BarcodeConstructTable
                where b.BarcodeConstructID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<BarcodeConstructTable> GetAllItemsByDepartment(AMSContext _db, int departmentID )
    {
        return from b in GetAllItems(_db)
                where b.DepartmentID == departmentID
                select b;
    }

    public static IQueryable<BarcodeConstructTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.BarcodeConstructTable select b);
    }

    public static IQueryable<BarcodeConstructTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return from b in BarcodeConstructTable.GetAllItems(_db, includeInactiveItems)
                
                select b;
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
        return (from b in _db.BarcodeConstructTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return BarcodeConstructTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return BarcodeConstructTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
