using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class LanguageTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int LanguageID { get; set; }

    [StringLength(200)]
    public string? LanguageName { get; set; }

    [StringLength(50)]
    public string? CultureSymbol { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    public bool IsDefault { get; set; }

    [InverseProperty("Language")]
    public virtual ICollection<AssetConditionDescriptionTable> AssetConditionDescriptionTable { get; set; } = new List<AssetConditionDescriptionTable>();

    [InverseProperty("Language")]
    public virtual ICollection<CategoryDescriptionTable> CategoryDescriptionTable { get; set; } = new List<CategoryDescriptionTable>();

    [InverseProperty("Language")]
    public virtual ICollection<CategoryTypeDescriptionTable> CategoryTypeDescriptionTable { get; set; } = new List<CategoryTypeDescriptionTable>();

    [InverseProperty("Language")]
    public virtual ICollection<CompanyDescriptionTable> CompanyDescriptionTable { get; set; } = new List<CompanyDescriptionTable>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("LanguageTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [InverseProperty("Language")]
    public virtual ICollection<DepartmentDescriptionTable> DepartmentDescriptionTable { get; set; } = new List<DepartmentDescriptionTable>();

    [InverseProperty("Language")]
    public virtual ICollection<DesignationDescriptionTable> DesignationDescriptionTable { get; set; } = new List<DesignationDescriptionTable>();

    [InverseProperty("Language")]
    public virtual ICollection<DisposalTypeDescriptionTable> DisposalTypeDescriptionTable { get; set; } = new List<DisposalTypeDescriptionTable>();

    [InverseProperty("Language")]
    public virtual ICollection<LanguageContentLineItemTable> LanguageContentLineItemTable { get; set; } = new List<LanguageContentLineItemTable>();

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("LanguageTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [InverseProperty("Language")]
    public virtual ICollection<LocationDescriptionTable> LocationDescriptionTable { get; set; } = new List<LocationDescriptionTable>();

    [InverseProperty("Language")]
    public virtual ICollection<ManufacturerDescriptionTable> ManufacturerDescriptionTable { get; set; } = new List<ManufacturerDescriptionTable>();

    [InverseProperty("Language")]
    public virtual ICollection<ModelDescriptionTable> ModelDescriptionTable { get; set; } = new List<ModelDescriptionTable>();

    [InverseProperty("Language")]
    public virtual ICollection<PartyDescriptionTable> PartyDescriptionTable { get; set; } = new List<PartyDescriptionTable>();

    [InverseProperty("Language")]
    public virtual ICollection<ProductDescriptionTable> ProductDescriptionTable { get; set; } = new List<ProductDescriptionTable>();

    [InverseProperty("Language")]
    public virtual ICollection<ProjectDescriptionTable> ProjectDescriptionTable { get; set; } = new List<ProjectDescriptionTable>();

    [InverseProperty("Language")]
    public virtual ICollection<RetirementTypeDescriptionTable> RetirementTypeDescriptionTable { get; set; } = new List<RetirementTypeDescriptionTable>();

    [InverseProperty("Language")]
    public virtual ICollection<SectionDescriptionTable> SectionDescriptionTable { get; set; } = new List<SectionDescriptionTable>();

    [InverseProperty("Language")]
    public virtual ICollection<TransferTypeDescriptionTable> TransferTypeDescriptionTable { get; set; } = new List<TransferTypeDescriptionTable>();

    [InverseProperty("Language")]
    public virtual ICollection<UOMDescriptionTable> UOMDescriptionTable { get; set; } = new List<UOMDescriptionTable>();

    [InverseProperty("Language")]
    public virtual ICollection<VATDescriptionTable> VATDescriptionTable { get; set; } = new List<VATDescriptionTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public LanguageTable()
    {

    }

	/// <summary>
    /// Default constructor for LanguageTable
    /// </summary>
    /// <param name="db"></param>
	public LanguageTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "LanguageID";
	}

	public override object GetPrimaryKeyValue()
	{
		return LanguageID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.LanguageName)) this.LanguageName = this.LanguageName.Trim();
		if(!string.IsNullOrEmpty(this.CultureSymbol)) this.CultureSymbol = this.CultureSymbol.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static LanguageTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.LanguageTable
                where b.LanguageID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<LanguageTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.LanguageTable select b);
    }

    public static IQueryable<LanguageTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return LanguageTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.LanguageTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return LanguageTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return LanguageTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
