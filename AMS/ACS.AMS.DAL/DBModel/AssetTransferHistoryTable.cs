using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Index("AssetID", Name = "FK_AssetID")]
[Index("NewDepartmentID", Name = "FK_NewDepartmentID")]
[Index("NewLocationID", Name = "FK_NewLocationID")]
[Index("NewSectionID", Name = "FK_NewSectionID")]
[Index("OldDepartmentID", Name = "FK_OldDepartmentID")]
[Index("OldLocationID", Name = "FK_OldLocationID")]
[Index("OldSectionID", Name = "FK_OldSectionID")]
[Index("TransferBy", Name = "FK_TransferBy")]
[Index("NewCustodianID", Name = "IX_AssetTransferHistoryTable_NewCustodianID")]
[Index("OldCustodianID", Name = "IX_AssetTransferHistoryTable_OldCustodianID")]
[Index("TransferTypeID", Name = "IX_TransferTypeID")]
public partial class AssetTransferHistoryTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int AssetHistoryID { get; set; }

    public int AssetID { get; set; }

    public int? OldLocationID { get; set; }

    public int? NewLocationID { get; set; }

    public int? OldDepartmentID { get; set; }

    public int? NewDepartmentID { get; set; }

    public int? OldSectionID { get; set; }

    public int? NewSectionID { get; set; }

    public int? OldCustodianID { get; set; }

    public int? NewCustodianID { get; set; }

    [StringLength(50)]
    public string? TransferFrom { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime TransferDate { get; set; }

    public int TransferBy { get; set; }

    public string? TransferRemarks { get; set; }

    public int? TransferTypeID { get; set; }

    [StringLength(100)]
    public string? OldReferencesNo { get; set; }

    [StringLength(100)]
    public string? NewReferencesNo { get; set; }

    public int? AssetConditionID { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DueDate { get; set; }

    [DisplayName("Status")]
    public int? StatusID { get; set; }

    public bool? IsReturnAsset { get; set; }

    [StringLength(100)]
    public string? OldCustodianType { get; set; }

    [StringLength(100)]
    public string? NewCustodianType { get; set; }

    [StringLength(400)]
    public string? Attribute1 { get; set; }

    [StringLength(400)]
    public string? Attribute2 { get; set; }

    [StringLength(400)]
    public string? Attribute3 { get; set; }

    [StringLength(400)]
    public string? Attribute4 { get; set; }

    [StringLength(400)]
    public string? Attribute5 { get; set; }

    [StringLength(400)]
    public string? Attribute6 { get; set; }

    [StringLength(400)]
    public string? Attribute7 { get; set; }

    [StringLength(400)]
    public string? Attribute8 { get; set; }

    [StringLength(400)]
    public string? Attribute9 { get; set; }

    [StringLength(400)]
    public string? Attribute10 { get; set; }

    [StringLength(400)]
    public string? Attribute11 { get; set; }

    [StringLength(400)]
    public string? Attribute12 { get; set; }

    [StringLength(400)]
    public string? Attribute13 { get; set; }

    [StringLength(400)]
    public string? Attribute14 { get; set; }

    [StringLength(400)]
    public string? Attribute15 { get; set; }

    [StringLength(400)]
    public string? Attribute16 { get; set; }

    [StringLength(400)]
    public string? OldAttribute1 { get; set; }

    [StringLength(400)]
    public string? OldAttribute2 { get; set; }

    [StringLength(400)]
    public string? OldAttribute3 { get; set; }

    [StringLength(400)]
    public string? OldAttribute4 { get; set; }

    [StringLength(400)]
    public string? OldAttribute5 { get; set; }

    [StringLength(400)]
    public string? OldAttribute6 { get; set; }

    [StringLength(400)]
    public string? OldAttribute7 { get; set; }

    [StringLength(400)]
    public string? OldAttribute8 { get; set; }

    [StringLength(400)]
    public string? OldAttribute9 { get; set; }

    [StringLength(400)]
    public string? OldAttribute10 { get; set; }

    [StringLength(400)]
    public string? OldAttribute11 { get; set; }

    [StringLength(400)]
    public string? OldAttribute12 { get; set; }

    [StringLength(400)]
    public string? OldAttribute13 { get; set; }

    [StringLength(400)]
    public string? OldAttribute14 { get; set; }

    [StringLength(400)]
    public string? OldAttribute15 { get; set; }

    [StringLength(400)]
    public string? OldAttribute16 { get; set; }

    [ForeignKey("AssetID")]
    [InverseProperty("AssetTransferHistoryTable")]
    public virtual AssetTable? Asset { get; set; } = null!;

    [ForeignKey("AssetConditionID")]
    [InverseProperty("AssetTransferHistoryTable")]
    public virtual AssetConditionTable? AssetCondition { get; set; }

    [ForeignKey("NewCustodianID")]
    [InverseProperty("AssetTransferHistoryTableNewCustodian")]
    public virtual User_LoginUserTable? NewCustodian { get; set; }

    [ForeignKey("NewDepartmentID")]
    [InverseProperty("AssetTransferHistoryTableNewDepartment")]
    public virtual DepartmentTable? NewDepartment { get; set; }

    [ForeignKey("NewLocationID")]
    [InverseProperty("AssetTransferHistoryTableNewLocation")]
    public virtual LocationTable? NewLocation { get; set; }

    [ForeignKey("NewSectionID")]
    [InverseProperty("AssetTransferHistoryTableNewSection")]
    public virtual SectionTable? NewSection { get; set; }

    [ForeignKey("OldCustodianID")]
    [InverseProperty("AssetTransferHistoryTableOldCustodian")]
    public virtual User_LoginUserTable? OldCustodian { get; set; }

    [ForeignKey("OldDepartmentID")]
    [InverseProperty("AssetTransferHistoryTableOldDepartment")]
    public virtual DepartmentTable? OldDepartment { get; set; }

    [ForeignKey("OldLocationID")]
    [InverseProperty("AssetTransferHistoryTableOldLocation")]
    public virtual LocationTable? OldLocation { get; set; }

    [ForeignKey("OldSectionID")]
    [InverseProperty("AssetTransferHistoryTableOldSection")]
    public virtual SectionTable? OldSection { get; set; }

    [ForeignKey("StatusID")]
    [InverseProperty("AssetTransferHistoryTable")]
    public virtual StatusTable? Status { get; set; }

    [ForeignKey("TransferBy")]
    [InverseProperty("AssetTransferHistoryTableTransferByNavigation")]
    public virtual User_LoginUserTable? TransferByNavigation { get; set; } = null!;

    [ForeignKey("TransferTypeID")]
    [InverseProperty("AssetTransferHistoryTable")]
    public virtual TransferTypeTable? TransferType { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public AssetTransferHistoryTable()
    {

    }

	/// <summary>
    /// Default constructor for AssetTransferHistoryTable
    /// </summary>
    /// <param name="db"></param>
	public AssetTransferHistoryTable(AMSContext _db)
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
		return "AssetHistoryID";
	}

	public override object GetPrimaryKeyValue()
	{
		return AssetHistoryID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.TransferFrom)) this.TransferFrom = this.TransferFrom.Trim();
		if(!string.IsNullOrEmpty(this.TransferRemarks)) this.TransferRemarks = this.TransferRemarks.Trim();
		if(!string.IsNullOrEmpty(this.OldReferencesNo)) this.OldReferencesNo = this.OldReferencesNo.Trim();
		if(!string.IsNullOrEmpty(this.NewReferencesNo)) this.NewReferencesNo = this.NewReferencesNo.Trim();
		if(!string.IsNullOrEmpty(this.OldCustodianType)) this.OldCustodianType = this.OldCustodianType.Trim();
		if(!string.IsNullOrEmpty(this.NewCustodianType)) this.NewCustodianType = this.NewCustodianType.Trim();
		if(!string.IsNullOrEmpty(this.Attribute1)) this.Attribute1 = this.Attribute1.Trim();
		if(!string.IsNullOrEmpty(this.Attribute2)) this.Attribute2 = this.Attribute2.Trim();
		if(!string.IsNullOrEmpty(this.Attribute3)) this.Attribute3 = this.Attribute3.Trim();
		if(!string.IsNullOrEmpty(this.Attribute4)) this.Attribute4 = this.Attribute4.Trim();
		if(!string.IsNullOrEmpty(this.Attribute5)) this.Attribute5 = this.Attribute5.Trim();
		if(!string.IsNullOrEmpty(this.Attribute6)) this.Attribute6 = this.Attribute6.Trim();
		if(!string.IsNullOrEmpty(this.Attribute7)) this.Attribute7 = this.Attribute7.Trim();
		if(!string.IsNullOrEmpty(this.Attribute8)) this.Attribute8 = this.Attribute8.Trim();
		if(!string.IsNullOrEmpty(this.Attribute9)) this.Attribute9 = this.Attribute9.Trim();
		if(!string.IsNullOrEmpty(this.Attribute10)) this.Attribute10 = this.Attribute10.Trim();
		if(!string.IsNullOrEmpty(this.Attribute11)) this.Attribute11 = this.Attribute11.Trim();
		if(!string.IsNullOrEmpty(this.Attribute12)) this.Attribute12 = this.Attribute12.Trim();
		if(!string.IsNullOrEmpty(this.Attribute13)) this.Attribute13 = this.Attribute13.Trim();
		if(!string.IsNullOrEmpty(this.Attribute14)) this.Attribute14 = this.Attribute14.Trim();
		if(!string.IsNullOrEmpty(this.Attribute15)) this.Attribute15 = this.Attribute15.Trim();
		if(!string.IsNullOrEmpty(this.Attribute16)) this.Attribute16 = this.Attribute16.Trim();
		if(!string.IsNullOrEmpty(this.OldAttribute1)) this.OldAttribute1 = this.OldAttribute1.Trim();
		if(!string.IsNullOrEmpty(this.OldAttribute2)) this.OldAttribute2 = this.OldAttribute2.Trim();
		if(!string.IsNullOrEmpty(this.OldAttribute3)) this.OldAttribute3 = this.OldAttribute3.Trim();
		if(!string.IsNullOrEmpty(this.OldAttribute4)) this.OldAttribute4 = this.OldAttribute4.Trim();
		if(!string.IsNullOrEmpty(this.OldAttribute5)) this.OldAttribute5 = this.OldAttribute5.Trim();
		if(!string.IsNullOrEmpty(this.OldAttribute6)) this.OldAttribute6 = this.OldAttribute6.Trim();
		if(!string.IsNullOrEmpty(this.OldAttribute7)) this.OldAttribute7 = this.OldAttribute7.Trim();
		if(!string.IsNullOrEmpty(this.OldAttribute8)) this.OldAttribute8 = this.OldAttribute8.Trim();
		if(!string.IsNullOrEmpty(this.OldAttribute9)) this.OldAttribute9 = this.OldAttribute9.Trim();
		if(!string.IsNullOrEmpty(this.OldAttribute10)) this.OldAttribute10 = this.OldAttribute10.Trim();
		if(!string.IsNullOrEmpty(this.OldAttribute11)) this.OldAttribute11 = this.OldAttribute11.Trim();
		if(!string.IsNullOrEmpty(this.OldAttribute12)) this.OldAttribute12 = this.OldAttribute12.Trim();
		if(!string.IsNullOrEmpty(this.OldAttribute13)) this.OldAttribute13 = this.OldAttribute13.Trim();
		if(!string.IsNullOrEmpty(this.OldAttribute14)) this.OldAttribute14 = this.OldAttribute14.Trim();
		if(!string.IsNullOrEmpty(this.OldAttribute15)) this.OldAttribute15 = this.OldAttribute15.Trim();
		if(!string.IsNullOrEmpty(this.OldAttribute16)) this.OldAttribute16 = this.OldAttribute16.Trim();

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

    public static AssetTransferHistoryTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.AssetTransferHistoryTable
                where b.AssetHistoryID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<AssetTransferHistoryTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.AssetTransferHistoryTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.AssetHistoryID descending
                    select b);
        }
        else
        {
            return (from b in _db.AssetTransferHistoryTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.AssetHistoryID descending
                    select b);
        }
    }

    public static IQueryable<AssetTransferHistoryTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return AssetTransferHistoryTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.AssetTransferHistoryTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return AssetTransferHistoryTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return AssetTransferHistoryTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
