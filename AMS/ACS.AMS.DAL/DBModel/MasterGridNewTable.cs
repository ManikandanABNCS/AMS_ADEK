using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Index("MasterGridName", Name = "IX_MasterGridNameNew_Unique", IsUnique = true)]
public partial class MasterGridNewTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int MasterGridID { get; set; }

    [StringLength(250)]
    public string MasterGridName { get; set; } = null!;

    [StringLength(50)]
    public string EntityName { get; set; } = null!;

    [InverseProperty("MasterGrid")]
    public virtual ICollection<MasterGridNewLineItemTable> MasterGridNewLineItemTable { get; set; } = new List<MasterGridNewLineItemTable>();

    [InverseProperty("MasterGrid")]
    public virtual ICollection<UserGridNewColumnTable> UserGridNewColumnTable { get; set; } = new List<UserGridNewColumnTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public MasterGridNewTable()
    {

    }

	/// <summary>
    /// Default constructor for MasterGridNewTable
    /// </summary>
    /// <param name="db"></param>
	public MasterGridNewTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "MasterGridID";
	}

	public override object GetPrimaryKeyValue()
	{
		return MasterGridID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.MasterGridName)) this.MasterGridName = this.MasterGridName.Trim();
		if(!string.IsNullOrEmpty(this.EntityName)) this.EntityName = this.EntityName.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static MasterGridNewTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.MasterGridNewTable
                where b.MasterGridID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<MasterGridNewTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.MasterGridNewTable select b);
    }

    public static IQueryable<MasterGridNewTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return MasterGridNewTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.MasterGridNewTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return MasterGridNewTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return MasterGridNewTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
