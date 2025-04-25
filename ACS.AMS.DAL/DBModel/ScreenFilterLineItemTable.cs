using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ScreenFilterLineItemTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ScreenFilterLineItemID { get; set; }

    public int ScreenFilterID { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string DisplayName { get; set; } = null!;

    public int? FieldTypeID { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string QueryField { get; set; } = null!;

    [DisplayName("Status")]
    public int StatusID { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public bool IsFixedFilter { get; set; }

    public byte ScreenFilterTypeID { get; set; }

    public bool IsMandatory { get; set; }

    public int OrderNo { get; set; }

    [StringLength(500)]
    public string? FieldName { get; set; }

    public int CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ScreenFilterLineItemTable")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("FieldTypeID")]
    [InverseProperty("ScreenFilterLineItemTable")]
    public virtual AFieldTypeTable? FieldType { get; set; }

    [ForeignKey("ScreenFilterID")]
    [InverseProperty("ScreenFilterLineItemTable")]
    public virtual ScreenFilterTable? ScreenFilter { get; set; } = null!;

    [ForeignKey("ScreenFilterTypeID")]
    [InverseProperty("ScreenFilterLineItemTable")]
    public virtual ScreenFilterTypeTable? ScreenFilterType { get; set; } = null!;

    [ForeignKey("StatusID")]
    [InverseProperty("ScreenFilterLineItemTable")]
    public virtual StatusTable? Status { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ScreenFilterLineItemTable()
    {

    }

	/// <summary>
    /// Default constructor for ScreenFilterLineItemTable
    /// </summary>
    /// <param name="db"></param>
	public ScreenFilterLineItemTable(AMSContext _db)
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
		return "ScreenFilterLineItemID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ScreenFilterLineItemID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.DisplayName)) this.DisplayName = this.DisplayName.Trim();
		if(!string.IsNullOrEmpty(this.QueryField)) this.QueryField = this.QueryField.Trim();
		if(!string.IsNullOrEmpty(this.FieldName)) this.FieldName = this.FieldName.Trim();

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

    public static ScreenFilterLineItemTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ScreenFilterLineItemTable
                where b.ScreenFilterLineItemID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ScreenFilterLineItemTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.ScreenFilterLineItemTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.ScreenFilterLineItemID descending
                    select b);
        }
        else
        {
            return (from b in _db.ScreenFilterLineItemTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.ScreenFilterLineItemID descending
                    select b);
        }
    }

    public static IQueryable<ScreenFilterLineItemTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ScreenFilterLineItemTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ScreenFilterLineItemTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ScreenFilterLineItemTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ScreenFilterLineItemTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
