using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class MasterGridNewLineItemTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int MasterGridLineItemID { get; set; }

    public int MasterGridID { get; set; }

    [StringLength(250)]
    public string FieldName { get; set; } = null!;

    [StringLength(50)]
    public string DisplayName { get; set; } = null!;

    public int Width { get; set; }

    [StringLength(100)]
    public string? Format { get; set; }

    public bool IsDefault { get; set; }

    public int OrderID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ColumnType { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? ActionScript { get; set; }

    [ForeignKey("MasterGridID")]
    [InverseProperty("MasterGridNewLineItemTable")]
    public virtual MasterGridNewTable? MasterGrid { get; set; } = null!;

    [InverseProperty("MasterGridLineItem")]
    public virtual ICollection<UserGridNewColumnTable> UserGridNewColumnTable { get; set; } = new List<UserGridNewColumnTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public MasterGridNewLineItemTable()
    {

    }

	/// <summary>
    /// Default constructor for MasterGridNewLineItemTable
    /// </summary>
    /// <param name="db"></param>
	public MasterGridNewLineItemTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "MasterGridLineItemID";
	}

	public override object GetPrimaryKeyValue()
	{
		return MasterGridLineItemID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.FieldName)) this.FieldName = this.FieldName.Trim();
		if(!string.IsNullOrEmpty(this.DisplayName)) this.DisplayName = this.DisplayName.Trim();
		if(!string.IsNullOrEmpty(this.Format)) this.Format = this.Format.Trim();
		if(!string.IsNullOrEmpty(this.ColumnType)) this.ColumnType = this.ColumnType.Trim();
		if(!string.IsNullOrEmpty(this.ActionScript)) this.ActionScript = this.ActionScript.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static MasterGridNewLineItemTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.MasterGridNewLineItemTable
                where b.MasterGridLineItemID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<MasterGridNewLineItemTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.MasterGridNewLineItemTable select b);
    }

    public static IQueryable<MasterGridNewLineItemTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return MasterGridNewLineItemTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.MasterGridNewLineItemTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return MasterGridNewLineItemTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return MasterGridNewLineItemTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
