using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Index("ExpenseTypeCode", Name = "UC_ExpenseTypeCode", IsUnique = true)]
public partial class ExpenseTypeTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ExpenseTypeID { get; set; }

    [StringLength(100)]
    public string ExpenseTypeCode { get; set; } = null!;

    public string ExpenseDescription { get; set; } = null!;

    [InverseProperty("ExpenseType")]
    public virtual ICollection<CategoryTable> CategoryTable { get; set; } = new List<CategoryTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ExpenseTypeTable()
    {

    }

	/// <summary>
    /// Default constructor for ExpenseTypeTable
    /// </summary>
    /// <param name="db"></param>
	public ExpenseTypeTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "ExpenseTypeID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ExpenseTypeID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.ExpenseTypeCode)) this.ExpenseTypeCode = this.ExpenseTypeCode.Trim();
		if(!string.IsNullOrEmpty(this.ExpenseDescription)) this.ExpenseDescription = this.ExpenseDescription.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static ExpenseTypeTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ExpenseTypeTable
                where b.ExpenseTypeID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ExpenseTypeTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.ExpenseTypeTable select b);
    }

    public static IQueryable<ExpenseTypeTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ExpenseTypeTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ExpenseTypeTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ExpenseTypeTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ExpenseTypeTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
