using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class LocationTypeTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int LocationTypeID { get; set; }

    [StringLength(100)]
    public string LocationTypeName { get; set; } = null!;

    [StringLength(100)]
    public string LocationTypeCode { get; set; } = null!;

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    [InverseProperty("FromLocationType")]
    public virtual ICollection<ApprovalHistoryTable> ApprovalHistoryTableFromLocationType { get; set; } = new List<ApprovalHistoryTable>();

    [InverseProperty("ToLocationType")]
    public virtual ICollection<ApprovalHistoryTable> ApprovalHistoryTableToLocationType { get; set; } = new List<ApprovalHistoryTable>();

    [InverseProperty("FromLocationType")]
    public virtual ICollection<ApproveWorkflowTable> ApproveWorkflowTableFromLocationType { get; set; } = new List<ApproveWorkflowTable>();

    [InverseProperty("ToLocationType")]
    public virtual ICollection<ApproveWorkflowTable> ApproveWorkflowTableToLocationType { get; set; } = new List<ApproveWorkflowTable>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("LocationTypeTable")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [InverseProperty("LocationType")]
    public virtual ICollection<LocationTable> LocationTable { get; set; } = new List<LocationTable>();

    [ForeignKey("StatusID")]
    [InverseProperty("LocationTypeTable")]
    public virtual StatusTable? Status { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public LocationTypeTable()
    {

    }

	/// <summary>
    /// Default constructor for LocationTypeTable
    /// </summary>
    /// <param name="db"></param>
	public LocationTypeTable(AMSContext _db)
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
		return "LocationTypeID";
	}

	public override object GetPrimaryKeyValue()
	{
		return LocationTypeID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.LocationTypeName)) this.LocationTypeName = this.LocationTypeName.Trim();
		if(!string.IsNullOrEmpty(this.LocationTypeCode)) this.LocationTypeCode = this.LocationTypeCode.Trim();

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

    public static LocationTypeTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.LocationTypeTable
                where b.LocationTypeID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<LocationTypeTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.LocationTypeTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.LocationTypeID descending
                    select b);
        }
        else
        {
            return (from b in _db.LocationTypeTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.LocationTypeID descending
                    select b);
        }
    }

    public static IQueryable<LocationTypeTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return LocationTypeTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.LocationTypeTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return LocationTypeTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return LocationTypeTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
