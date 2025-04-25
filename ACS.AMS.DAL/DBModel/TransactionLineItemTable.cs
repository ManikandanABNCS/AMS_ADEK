using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class TransactionLineItemTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int TransactionLineItemID { get; set; }

    public int TransactionID { get; set; }

    [StringLength(200)]
    public string? TransactionLineNo { get; set; }

    public int AssetID { get; set; }

    public int? FromLocationID { get; set; }

    public int? ToLocationID { get; set; }

    public int? FromDepartmentID { get; set; }

    public int? ToDepartmentID { get; set; }

    public int? FromCustodianID { get; set; }

    public int? ToCustodianID { get; set; }

    public int? FromCategoryID { get; set; }

    public int? ToCategoryID { get; set; }

    public int? FromProductID { get; set; }

    public int? ToProductID { get; set; }

    public int? FromSectionID { get; set; }

    public int? ToSectionID { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? TransferTypeID { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? DisposalValue { get; set; }

    [StringLength(200)]
    public string? DisposalReferencesNo { get; set; }

    public string? DisposalRemarks { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ProceedOfSales { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? CostOfRemoval { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DisposalDate { get; set; }

    public int? RoomID { get; set; }

    public int? RetirementTypeID { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    public int? AdjustmentValue { get; set; }

    public bool IsAdjustmentNetBook { get; set; }

    public int? CustodianID { get; set; }

    public int? DepartmentID { get; set; }

    public int? SectionID { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? TransferDate { get; set; }

    public int? FromAssetConditionID { get; set; }

    public int? ToAssetConditionID { get; set; }

    [ForeignKey("AssetID")]
    [InverseProperty("TransactionLineItemTable")]
    public virtual AssetTable? Asset { get; set; } = null!;

    [ForeignKey("CreatedBy")]
    [InverseProperty("TransactionLineItemTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("CustodianID")]
    [InverseProperty("TransactionLineItemTableCustodian")]
    public virtual User_LoginUserTable? Custodian { get; set; }

    [ForeignKey("DepartmentID")]
    [InverseProperty("TransactionLineItemTableDepartment")]
    public virtual DepartmentTable? Department { get; set; }

    [ForeignKey("FromAssetConditionID")]
    [InverseProperty("TransactionLineItemTableFromAssetCondition")]
    public virtual AssetConditionTable? FromAssetCondition { get; set; }

    [ForeignKey("FromCategoryID")]
    [InverseProperty("TransactionLineItemTableFromCategory")]
    public virtual CategoryTable? FromCategory { get; set; }

    [ForeignKey("FromCustodianID")]
    [InverseProperty("TransactionLineItemTableFromCustodian")]
    public virtual PersonTable? FromCustodian { get; set; }

    [ForeignKey("FromDepartmentID")]
    [InverseProperty("TransactionLineItemTableFromDepartment")]
    public virtual DepartmentTable? FromDepartment { get; set; }

    [ForeignKey("FromLocationID")]
    [InverseProperty("TransactionLineItemTableFromLocation")]
    public virtual LocationTable? FromLocation { get; set; }

    [ForeignKey("FromProductID")]
    [InverseProperty("TransactionLineItemTableFromProduct")]
    public virtual ProductTable? FromProduct { get; set; }

    [ForeignKey("FromSectionID")]
    [InverseProperty("TransactionLineItemTableFromSection")]
    public virtual SectionTable? FromSection { get; set; }

    [ForeignKey("RetirementTypeID")]
    [InverseProperty("TransactionLineItemTable")]
    public virtual RetirementTypeTable? RetirementType { get; set; }

    [ForeignKey("RoomID")]
    [InverseProperty("TransactionLineItemTableRoom")]
    public virtual LocationTable? Room { get; set; }

    [ForeignKey("SectionID")]
    [InverseProperty("TransactionLineItemTableSection")]
    public virtual SectionTable? Section { get; set; }

    [ForeignKey("StatusID")]
    [InverseProperty("TransactionLineItemTable")]
    public virtual StatusTable? Status { get; set; } = null!;

    [ForeignKey("ToAssetConditionID")]
    [InverseProperty("TransactionLineItemTableToAssetCondition")]
    public virtual AssetConditionTable? ToAssetCondition { get; set; }

    [ForeignKey("ToCategoryID")]
    [InverseProperty("TransactionLineItemTableToCategory")]
    public virtual CategoryTable? ToCategory { get; set; }

    [ForeignKey("ToCustodianID")]
    [InverseProperty("TransactionLineItemTableToCustodian")]
    public virtual PersonTable? ToCustodian { get; set; }

    [ForeignKey("ToDepartmentID")]
    [InverseProperty("TransactionLineItemTableToDepartment")]
    public virtual DepartmentTable? ToDepartment { get; set; }

    [ForeignKey("ToLocationID")]
    [InverseProperty("TransactionLineItemTableToLocation")]
    public virtual LocationTable? ToLocation { get; set; }

    [ForeignKey("ToProductID")]
    [InverseProperty("TransactionLineItemTableToProduct")]
    public virtual ProductTable? ToProduct { get; set; }

    [ForeignKey("ToSectionID")]
    [InverseProperty("TransactionLineItemTableToSection")]
    public virtual SectionTable? ToSection { get; set; }

    [ForeignKey("TransactionID")]
    [InverseProperty("TransactionLineItemTable")]
    public virtual TransactionTable? Transaction { get; set; } = null!;

    [ForeignKey("TransferTypeID")]
    [InverseProperty("TransactionLineItemTable")]
    public virtual TransferTypeTable? TransferType { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public TransactionLineItemTable()
    {

    }

	/// <summary>
    /// Default constructor for TransactionLineItemTable
    /// </summary>
    /// <param name="db"></param>
	public TransactionLineItemTable(AMSContext _db)
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
		return "TransactionLineItemID";
	}

	public override object GetPrimaryKeyValue()
	{
		return TransactionLineItemID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.TransactionLineNo)) this.TransactionLineNo = this.TransactionLineNo.Trim();
		if(!string.IsNullOrEmpty(this.DisposalReferencesNo)) this.DisposalReferencesNo = this.DisposalReferencesNo.Trim();
		if(!string.IsNullOrEmpty(this.DisposalRemarks)) this.DisposalRemarks = this.DisposalRemarks.Trim();
		if(!string.IsNullOrEmpty(this.Remarks)) this.Remarks = this.Remarks.Trim();

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

    public static TransactionLineItemTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.TransactionLineItemTable
                where b.TransactionLineItemID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<TransactionLineItemTable> GetAllItemsByDepartment(AMSContext _db, int departmentID , bool includeInactiveItems = false )
    {
        return from b in GetAllItems(_db, includeInactiveItems)
                where b.DepartmentID == departmentID
                select b;
    }

    public static IQueryable<TransactionLineItemTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.TransactionLineItemTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.TransactionLineItemID descending
                    select b);
        }
        else
        {
            return (from b in _db.TransactionLineItemTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.TransactionLineItemID descending
                    select b);
        }
    }

    public static IQueryable<TransactionLineItemTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return from b in TransactionLineItemTable.GetAllItems(_db, includeInactiveItems)
                
                select b;
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
        return (from b in _db.TransactionLineItemTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return TransactionLineItemTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return TransactionLineItemTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
