using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class DepartmentDescriptionTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int DepartmentDescriptionID { get; set; }

    public int DepartmentID { get; set; }

    public string DepartmentDescription { get; set; } = null!;

    public int LanguageID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("DepartmentDescriptionTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("DepartmentID")]
    [InverseProperty("DepartmentDescriptionTable")]
    public virtual DepartmentTable? Department { get; set; } = null!;

    [ForeignKey("LanguageID")]
    [InverseProperty("DepartmentDescriptionTable")]
    public virtual LanguageTable? Language { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("DepartmentDescriptionTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public DepartmentDescriptionTable()
    {

    }

	/// <summary>
    /// Default constructor for DepartmentDescriptionTable
    /// </summary>
    /// <param name="db"></param>
	public DepartmentDescriptionTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "DepartmentDescriptionID";
	}

	public override object GetPrimaryKeyValue()
	{
		return DepartmentDescriptionID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.DepartmentDescription)) this.DepartmentDescription = this.DepartmentDescription.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static DepartmentDescriptionTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.DepartmentDescriptionTable
                where b.DepartmentDescriptionID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<DepartmentDescriptionTable> GetAllItemsByDepartment(AMSContext _db, int departmentID )
    {
        return from b in GetAllItems(_db)
                where b.DepartmentID == departmentID
                select b;
    }

    public static IQueryable<DepartmentDescriptionTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.DepartmentDescriptionTable select b);
    }

    public static IQueryable<DepartmentDescriptionTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return from b in DepartmentDescriptionTable.GetAllItems(_db, includeInactiveItems)
                
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
        return (from b in _db.DepartmentDescriptionTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return DepartmentDescriptionTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return DepartmentDescriptionTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
