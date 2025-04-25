using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class AFieldTypeTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int FieldTypeID { get; set; }

    public int FieldTypeCode { get; set; }

    [StringLength(50)]
    public string FieldTypeDesc { get; set; } = null!;

    [StringLength(250)]
    [Unicode(false)]
    public string? ViewFileLocation { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? ValueFieldName { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? AllowedDataTypes { get; set; }

    public int? SelectionControlQueryID { get; set; }

    [InverseProperty("FieldType")]
    public virtual ICollection<DashboardTemplateFilterFieldTable> DashboardTemplateFilterFieldTable { get; set; } = new List<DashboardTemplateFilterFieldTable>();

    [InverseProperty("FieldType")]
    public virtual ICollection<ScreenFilterLineItemTable> ScreenFilterLineItemTable { get; set; } = new List<ScreenFilterLineItemTable>();

    [ForeignKey("SelectionControlQueryID")]
    [InverseProperty("AFieldTypeTable")]
    public virtual ASelectionControlQueryTable? SelectionControlQuery { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public AFieldTypeTable()
    {

    }

	/// <summary>
    /// Default constructor for AFieldTypeTable
    /// </summary>
    /// <param name="db"></param>
	public AFieldTypeTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "FieldTypeID";
	}

	public override object GetPrimaryKeyValue()
	{
		return FieldTypeID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.FieldTypeDesc)) this.FieldTypeDesc = this.FieldTypeDesc.Trim();
		if(!string.IsNullOrEmpty(this.ViewFileLocation)) this.ViewFileLocation = this.ViewFileLocation.Trim();
		if(!string.IsNullOrEmpty(this.ValueFieldName)) this.ValueFieldName = this.ValueFieldName.Trim();
		if(!string.IsNullOrEmpty(this.AllowedDataTypes)) this.AllowedDataTypes = this.AllowedDataTypes.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static AFieldTypeTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.AFieldTypeTable
                where b.FieldTypeID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<AFieldTypeTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.AFieldTypeTable select b);
    }

    public static IQueryable<AFieldTypeTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return AFieldTypeTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.AFieldTypeTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return AFieldTypeTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return AFieldTypeTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
