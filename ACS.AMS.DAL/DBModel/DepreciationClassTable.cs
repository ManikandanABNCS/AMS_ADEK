using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class DepreciationClassTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int DepreciationClassID { get; set; }

    public string ClassName { get; set; } = null!;

    public int DepreciationMethodID { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    public string DepreciationClassName { get; set; } = null!;

    [InverseProperty("DepreciationClass")]
    public virtual ICollection<AssetTable> AssetTable { get; set; } = new List<AssetTable>();

    [InverseProperty("DepreciationClass")]
    public virtual ICollection<CategoryTable> CategoryTable { get; set; } = new List<CategoryTable>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("DepreciationClassTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [InverseProperty("DepreciationClass")]
    public virtual ICollection<DepreciationClassLineItemTable> DepreciationClassLineItemTable { get; set; } = new List<DepreciationClassLineItemTable>();

    [InverseProperty("DepreciationClass")]
    public virtual ICollection<DepreciationLineItemTable> DepreciationLineItemTable { get; set; } = new List<DepreciationLineItemTable>();

    [ForeignKey("DepreciationMethodID")]
    [InverseProperty("DepreciationClassTable")]
    public virtual DepreciationMethodTable? DepreciationMethod { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("DepreciationClassTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [ForeignKey("StatusID")]
    [InverseProperty("DepreciationClassTable")]
    public virtual StatusTable? Status { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public DepreciationClassTable()
    {

    }

	/// <summary>
    /// Default constructor for DepreciationClassTable
    /// </summary>
    /// <param name="db"></param>
	public DepreciationClassTable(AMSContext _db)
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
		return "DepreciationClassID";
	}

	public override object GetPrimaryKeyValue()
	{
		return DepreciationClassID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.ClassName)) this.ClassName = this.ClassName.Trim();
		if(!string.IsNullOrEmpty(this.DepreciationClassName)) this.DepreciationClassName = this.DepreciationClassName.Trim();

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

    public static DepreciationClassTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.DepreciationClassTable
                where b.DepreciationClassID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<DepreciationClassTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.DepreciationClassTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.DepreciationClassID descending
                    select b);
        }
        else
        {
            return (from b in _db.DepreciationClassTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.DepreciationClassID descending
                    select b);
        }
    }

    public static IQueryable<DepreciationClassTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return DepreciationClassTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.DepreciationClassTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return DepreciationClassTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return DepreciationClassTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
