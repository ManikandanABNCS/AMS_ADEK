using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class TransactionTypeTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int TransactionTypeID { get; set; }

    [StringLength(500)]
    public string TransactionTypeName { get; set; } = null!;

    public bool IsSourceTransactionType { get; set; }

    public int? SourceTransactionTypeID { get; set; }

    public int? SourceTransactionTypeID2 { get; set; }

    public int? SourceTransactionTypeID3 { get; set; }

    public int? SourceTransactionTypeID4 { get; set; }

    public int? SourceTransactionTypeID5 { get; set; }

    public string TransactionTypeDesc { get; set; } = null!;

    [InverseProperty("SourceTransactionType")]
    public virtual ICollection<TransactionTypeTable> InverseSourceTransactionType { get; set; } = new List<TransactionTypeTable>();

    [InverseProperty("SourceTransactionTypeID2Navigation")]
    public virtual ICollection<TransactionTypeTable> InverseSourceTransactionTypeID2Navigation { get; set; } = new List<TransactionTypeTable>();

    [InverseProperty("SourceTransactionTypeID3Navigation")]
    public virtual ICollection<TransactionTypeTable> InverseSourceTransactionTypeID3Navigation { get; set; } = new List<TransactionTypeTable>();

    [InverseProperty("SourceTransactionTypeID4Navigation")]
    public virtual ICollection<TransactionTypeTable> InverseSourceTransactionTypeID4Navigation { get; set; } = new List<TransactionTypeTable>();

    [InverseProperty("SourceTransactionTypeID5Navigation")]
    public virtual ICollection<TransactionTypeTable> InverseSourceTransactionTypeID5Navigation { get; set; } = new List<TransactionTypeTable>();

    [ForeignKey("SourceTransactionTypeID")]
    [InverseProperty("InverseSourceTransactionType")]
    public virtual TransactionTypeTable? SourceTransactionType { get; set; }

    [ForeignKey("SourceTransactionTypeID2")]
    [InverseProperty("InverseSourceTransactionTypeID2Navigation")]
    public virtual TransactionTypeTable? SourceTransactionTypeID2Navigation { get; set; }

    [ForeignKey("SourceTransactionTypeID3")]
    [InverseProperty("InverseSourceTransactionTypeID3Navigation")]
    public virtual TransactionTypeTable? SourceTransactionTypeID3Navigation { get; set; }

    [ForeignKey("SourceTransactionTypeID4")]
    [InverseProperty("InverseSourceTransactionTypeID4Navigation")]
    public virtual TransactionTypeTable? SourceTransactionTypeID4Navigation { get; set; }

    [ForeignKey("SourceTransactionTypeID5")]
    [InverseProperty("InverseSourceTransactionTypeID5Navigation")]
    public virtual TransactionTypeTable? SourceTransactionTypeID5Navigation { get; set; }

    [InverseProperty("TransactionType")]
    public virtual ICollection<TransactionTable> TransactionTable { get; set; } = new List<TransactionTable>();

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public TransactionTypeTable()
    {

    }

	/// <summary>
    /// Default constructor for TransactionTypeTable
    /// </summary>
    /// <param name="db"></param>
	public TransactionTypeTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "TransactionTypeID";
	}

	public override object GetPrimaryKeyValue()
	{
		return TransactionTypeID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.TransactionTypeName)) this.TransactionTypeName = this.TransactionTypeName.Trim();
		if(!string.IsNullOrEmpty(this.TransactionTypeDesc)) this.TransactionTypeDesc = this.TransactionTypeDesc.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static TransactionTypeTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.TransactionTypeTable
                where b.TransactionTypeID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<TransactionTypeTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.TransactionTypeTable select b);
    }

    public static IQueryable<TransactionTypeTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return TransactionTypeTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.TransactionTypeTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return TransactionTypeTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return TransactionTypeTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
