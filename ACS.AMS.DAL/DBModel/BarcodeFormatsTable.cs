using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class BarcodeFormatsTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int FormatID { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string FormatName { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? LabelStart { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? HumanReadable { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Barcode { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? CompanyName { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? AssetDescription { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Category { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Department { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? L1Location { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? LabelEnd { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    public string? LogoFormat { get; set; }

    public string? ConvertLogoToZPL { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? PurchaseDate { get; set; }

    public string? ReferenceCode { get; set; }

    public string? SerialNo { get; set; }

    [StringLength(200)]
    public string? BarcodeHeader { get; set; }

    [StringLength(200)]
    public string? BarcodeFooter { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("BarcodeFormatsTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("BarcodeFormatsTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [ForeignKey("StatusID")]
    [InverseProperty("BarcodeFormatsTable")]
    public virtual StatusTable? Status { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public BarcodeFormatsTable()
    {

    }

	/// <summary>
    /// Default constructor for BarcodeFormatsTable
    /// </summary>
    /// <param name="db"></param>
	public BarcodeFormatsTable(AMSContext _db)
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
		return "FormatID";
	}

	public override object GetPrimaryKeyValue()
	{
		return FormatID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.FormatName)) this.FormatName = this.FormatName.Trim();
		if(!string.IsNullOrEmpty(this.LabelStart)) this.LabelStart = this.LabelStart.Trim();
		if(!string.IsNullOrEmpty(this.HumanReadable)) this.HumanReadable = this.HumanReadable.Trim();
		if(!string.IsNullOrEmpty(this.Barcode)) this.Barcode = this.Barcode.Trim();
		if(!string.IsNullOrEmpty(this.CompanyName)) this.CompanyName = this.CompanyName.Trim();
		if(!string.IsNullOrEmpty(this.AssetDescription)) this.AssetDescription = this.AssetDescription.Trim();
		if(!string.IsNullOrEmpty(this.Category)) this.Category = this.Category.Trim();
		if(!string.IsNullOrEmpty(this.Department)) this.Department = this.Department.Trim();
		if(!string.IsNullOrEmpty(this.L1Location)) this.L1Location = this.L1Location.Trim();
		if(!string.IsNullOrEmpty(this.LabelEnd)) this.LabelEnd = this.LabelEnd.Trim();
		if(!string.IsNullOrEmpty(this.LogoFormat)) this.LogoFormat = this.LogoFormat.Trim();
		if(!string.IsNullOrEmpty(this.ConvertLogoToZPL)) this.ConvertLogoToZPL = this.ConvertLogoToZPL.Trim();
		if(!string.IsNullOrEmpty(this.PurchaseDate)) this.PurchaseDate = this.PurchaseDate.Trim();
		if(!string.IsNullOrEmpty(this.ReferenceCode)) this.ReferenceCode = this.ReferenceCode.Trim();
		if(!string.IsNullOrEmpty(this.SerialNo)) this.SerialNo = this.SerialNo.Trim();
		if(!string.IsNullOrEmpty(this.BarcodeHeader)) this.BarcodeHeader = this.BarcodeHeader.Trim();
		if(!string.IsNullOrEmpty(this.BarcodeFooter)) this.BarcodeFooter = this.BarcodeFooter.Trim();

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

    public static BarcodeFormatsTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.BarcodeFormatsTable
                where b.FormatID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<BarcodeFormatsTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.BarcodeFormatsTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.FormatID descending
                    select b);
        }
        else
        {
            return (from b in _db.BarcodeFormatsTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.FormatID descending
                    select b);
        }
    }

    public static IQueryable<BarcodeFormatsTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return BarcodeFormatsTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.BarcodeFormatsTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return BarcodeFormatsTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return BarcodeFormatsTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
