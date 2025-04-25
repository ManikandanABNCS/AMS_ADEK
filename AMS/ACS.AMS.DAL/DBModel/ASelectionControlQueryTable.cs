using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ASelectionControlQueryTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int SelectionControlQueryID { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string ControlName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string ControlType { get; set; } = null!;

    [StringLength(1000)]
    [Unicode(false)]
    public string? Query { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string DisplayFields { get; set; } = null!;

    [StringLength(250)]
    [Unicode(false)]
    public string? SearchFields { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? OrderByQuery { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string SelectedItemDisplayField { get; set; } = null!;

    [StringLength(250)]
    [Unicode(false)]
    public string ValueFieldName { get; set; } = null!;

    [InverseProperty("SelectionControlQuery")]
    public virtual ICollection<AFieldTypeTable> AFieldTypeTable { get; set; } = new List<AFieldTypeTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ASelectionControlQueryTable()
    {

    }

	/// <summary>
    /// Default constructor for ASelectionControlQueryTable
    /// </summary>
    /// <param name="db"></param>
	public ASelectionControlQueryTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "SelectionControlQueryID";
	}

	public override object GetPrimaryKeyValue()
	{
		return SelectionControlQueryID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.ControlName)) this.ControlName = this.ControlName.Trim();
		if(!string.IsNullOrEmpty(this.ControlType)) this.ControlType = this.ControlType.Trim();
		if(!string.IsNullOrEmpty(this.Query)) this.Query = this.Query.Trim();
		if(!string.IsNullOrEmpty(this.DisplayFields)) this.DisplayFields = this.DisplayFields.Trim();
		if(!string.IsNullOrEmpty(this.SearchFields)) this.SearchFields = this.SearchFields.Trim();
		if(!string.IsNullOrEmpty(this.OrderByQuery)) this.OrderByQuery = this.OrderByQuery.Trim();
		if(!string.IsNullOrEmpty(this.SelectedItemDisplayField)) this.SelectedItemDisplayField = this.SelectedItemDisplayField.Trim();
		if(!string.IsNullOrEmpty(this.ValueFieldName)) this.ValueFieldName = this.ValueFieldName.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static ASelectionControlQueryTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ASelectionControlQueryTable
                where b.SelectionControlQueryID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ASelectionControlQueryTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.ASelectionControlQueryTable select b);
    }

    public static IQueryable<ASelectionControlQueryTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ASelectionControlQueryTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ASelectionControlQueryTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ASelectionControlQueryTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ASelectionControlQueryTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
