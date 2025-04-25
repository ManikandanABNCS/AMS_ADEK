using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class ConfigurationTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int ConfigurationID { get; set; }

    [StringLength(100)]
    public string ConfiguarationName { get; set; } = null!;

    [StringLength(1000)]
    public string ConfiguarationValue { get; set; } = null!;

    [StringLength(50)]
    public string? ToolTipName { get; set; }

    [StringLength(50)]
    public string? DataType { get; set; }

    [StringLength(2000)]
    public string? DropDownValue { get; set; }

    public int? MinValue { get; set; }

    public int? MaxValue { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string ConfiguarationType { get; set; } = null!;

    public int DisplayOrderID { get; set; }

    [StringLength(1000)]
    public string? DefaultValue { get; set; }

    public bool DisplayConfiguration { get; set; }

    [StringLength(100)]
    public string? SuffixText { get; set; }

    [StringLength(50)]
    public string CategoryName { get; set; } = null!;

    [Column(TypeName = "image")]
    public byte[]? Images { get; set; }

    [StringLength(400)]
    public string? RequiredLicenseName { get; set; }

    [StringLength(400)]
    public string? CompanyName { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ConfigurationTable()
    {

    }

	/// <summary>
    /// Default constructor for ConfigurationTable
    /// </summary>
    /// <param name="db"></param>
	public ConfigurationTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "ConfigurationID";
	}

	public override object GetPrimaryKeyValue()
	{
		return ConfigurationID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.ConfiguarationName)) this.ConfiguarationName = this.ConfiguarationName.Trim();
		if(!string.IsNullOrEmpty(this.ConfiguarationValue)) this.ConfiguarationValue = this.ConfiguarationValue.Trim();
		if(!string.IsNullOrEmpty(this.ToolTipName)) this.ToolTipName = this.ToolTipName.Trim();
		if(!string.IsNullOrEmpty(this.DataType)) this.DataType = this.DataType.Trim();
		if(!string.IsNullOrEmpty(this.DropDownValue)) this.DropDownValue = this.DropDownValue.Trim();
		if(!string.IsNullOrEmpty(this.ConfiguarationType)) this.ConfiguarationType = this.ConfiguarationType.Trim();
		if(!string.IsNullOrEmpty(this.DefaultValue)) this.DefaultValue = this.DefaultValue.Trim();
		if(!string.IsNullOrEmpty(this.SuffixText)) this.SuffixText = this.SuffixText.Trim();
		if(!string.IsNullOrEmpty(this.CategoryName)) this.CategoryName = this.CategoryName.Trim();
		if(!string.IsNullOrEmpty(this.RequiredLicenseName)) this.RequiredLicenseName = this.RequiredLicenseName.Trim();
		if(!string.IsNullOrEmpty(this.CompanyName)) this.CompanyName = this.CompanyName.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static ConfigurationTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.ConfigurationTable
                where b.ConfigurationID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<ConfigurationTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.ConfigurationTable select b);
    }

    public static IQueryable<ConfigurationTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return ConfigurationTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.ConfigurationTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return ConfigurationTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return ConfigurationTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
