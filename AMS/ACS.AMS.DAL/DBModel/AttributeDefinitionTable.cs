using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class AttributeDefinitionTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int AttributeDefinitionID { get; set; }

    public int EntityID { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string AttributeName { get; set; } = null!;

    [StringLength(200)]
    public string DisplayTitle { get; set; } = null!;

    [StringLength(100)]
    public string? ToolTipName { get; set; }

    public byte DataType { get; set; }

    public byte DisplayControl { get; set; }

    public bool IsMandatory { get; set; }

    public int StringSize { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? MinValue { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? MaxValue { get; set; }

    public double? StepIncrement { get; set; }

    public int DisplayOrderID { get; set; }

    [StringLength(2000)]
    public string? DefaultValue { get; set; }

    [StringLength(4000)]
    public string? SelectionFieldValues { get; set; }

    [StringLength(2000)]
    public string? SelectQuery { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? ValueField { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? DisplayField { get; set; }

    public bool RequiredOnHHT { get; set; }

    public bool MaintainHHTDataOnSeparateTable { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? MinDateValue { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? MaxDateValue { get; set; }

    public bool VisibleToTransferAssetScreen { get; set; }

    public bool RangeValidateRequired { get; set; }

    public int? RangeValidateFrom { get; set; }

    [StringLength(20)]
    public string? ComparisonOperator { get; set; }

    public bool CasCadingFieldRequired { get; set; }

    [StringLength(20)]
    public string? CasCadingFieldName { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? ModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDateTime { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("AttributeDefinitionTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("DataType")]
    [InverseProperty("AttributeDefinitionTable")]
    public virtual DataTypeTable? DataTypeNavigation { get; set; } = null!;

    [ForeignKey("DisplayControl")]
    [InverseProperty("AttributeDefinitionTable")]
    public virtual DisplayControlTypeTable? DisplayControlNavigation { get; set; } = null!;

    [ForeignKey("EntityID")]
    [InverseProperty("AttributeDefinitionTable")]
    public virtual EntityTable? Entity { get; set; } = null!;

    [InverseProperty("RangeValidateFromNavigation")]
    public virtual ICollection<AttributeDefinitionTable> InverseRangeValidateFromNavigation { get; set; } = new List<AttributeDefinitionTable>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("AttributeDefinitionTableModifiedByNavigation")]
    public virtual User_LoginUserTable? ModifiedByNavigation { get; set; }

    [ForeignKey("RangeValidateFrom")]
    [InverseProperty("InverseRangeValidateFromNavigation")]
    public virtual AttributeDefinitionTable? RangeValidateFromNavigation { get; set; }

    [ForeignKey("StatusID")]
    [InverseProperty("AttributeDefinitionTable")]
    public virtual StatusTable? Status { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public AttributeDefinitionTable()
    {

    }

	/// <summary>
    /// Default constructor for AttributeDefinitionTable
    /// </summary>
    /// <param name="db"></param>
	public AttributeDefinitionTable(AMSContext _db)
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
		return "AttributeDefinitionID";
	}

	public override object GetPrimaryKeyValue()
	{
		return AttributeDefinitionID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.AttributeName)) this.AttributeName = this.AttributeName.Trim();
		if(!string.IsNullOrEmpty(this.DisplayTitle)) this.DisplayTitle = this.DisplayTitle.Trim();
		if(!string.IsNullOrEmpty(this.ToolTipName)) this.ToolTipName = this.ToolTipName.Trim();
		if(!string.IsNullOrEmpty(this.MinValue)) this.MinValue = this.MinValue.Trim();
		if(!string.IsNullOrEmpty(this.MaxValue)) this.MaxValue = this.MaxValue.Trim();
		if(!string.IsNullOrEmpty(this.DefaultValue)) this.DefaultValue = this.DefaultValue.Trim();
		if(!string.IsNullOrEmpty(this.SelectionFieldValues)) this.SelectionFieldValues = this.SelectionFieldValues.Trim();
		if(!string.IsNullOrEmpty(this.SelectQuery)) this.SelectQuery = this.SelectQuery.Trim();
		if(!string.IsNullOrEmpty(this.ValueField)) this.ValueField = this.ValueField.Trim();
		if(!string.IsNullOrEmpty(this.DisplayField)) this.DisplayField = this.DisplayField.Trim();
		if(!string.IsNullOrEmpty(this.ComparisonOperator)) this.ComparisonOperator = this.ComparisonOperator.Trim();
		if(!string.IsNullOrEmpty(this.CasCadingFieldName)) this.CasCadingFieldName = this.CasCadingFieldName.Trim();

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

    public static AttributeDefinitionTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.AttributeDefinitionTable
                where b.AttributeDefinitionID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<AttributeDefinitionTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.AttributeDefinitionTable
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.AttributeDefinitionID descending
                    select b);
        }
        else
        {
            return (from b in _db.AttributeDefinitionTable
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.AttributeDefinitionID descending
                    select b);
        }
    }

    public static IQueryable<AttributeDefinitionTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return AttributeDefinitionTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.AttributeDefinitionTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return AttributeDefinitionTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return AttributeDefinitionTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
