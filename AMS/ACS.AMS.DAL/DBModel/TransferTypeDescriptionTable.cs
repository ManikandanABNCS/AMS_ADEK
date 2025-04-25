using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

public partial class TransferTypeDescriptionTable : BaseEntityObject, IACSDBObject
{
    [Key]
    public int TransferTypeDescriptionID { get; set; }

    public int TransferTypeID { get; set; }

    public string TransferTypeDescription { get; set; } = null!;

    public int LanguageID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("TransferTypeDescriptionTableCreatedByNavigation")]
    public virtual User_LoginUserTable? CreatedByNavigation { get; set; } = null!;

    [ForeignKey("LanguageID")]
    [InverseProperty("TransferTypeDescriptionTable")]
    public virtual LanguageTable? Language { get; set; } = null!;

    [ForeignKey("LastModifiedBy")]
    [InverseProperty("TransferTypeDescriptionTableLastModifiedByNavigation")]
    public virtual User_LoginUserTable? LastModifiedByNavigation { get; set; }

    [ForeignKey("TransferTypeID")]
    [InverseProperty("TransferTypeDescriptionTable")]
    public virtual TransferTypeTable? TransferType { get; set; } = null!;

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public TransferTypeDescriptionTable()
    {

    }

	/// <summary>
    /// Default constructor for TransferTypeDescriptionTable
    /// </summary>
    /// <param name="db"></param>
	public TransferTypeDescriptionTable(AMSContext _db)
	{
		
		InitializeProperties();

		_db.Add(this);
	}

	#endregion Constructors
*/

    #region Instance methods

	public override string GetPrimaryKeyFieldName()
	{
		return "TransferTypeDescriptionID";
	}

	public override object GetPrimaryKeyValue()
	{
		return TransferTypeDescriptionID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.TransferTypeDescription)) this.TransferTypeDescription = this.TransferTypeDescription.Trim();

		OnBeforeSave(db);

        base.BeforeSave(db);
    }

	public void Delete()
	{
		OnDelete();
	}

	#endregion Instance methods

    #region Static Methods

    public static TransferTypeDescriptionTable GetItem(AMSContext _db, int id)
    {
        return (from b in _db.TransferTypeDescriptionTable
                where b.TransferTypeDescriptionID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<TransferTypeDescriptionTable> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        return (from b in _db.TransferTypeDescriptionTable select b);
    }

    public static IQueryable<TransferTypeDescriptionTable> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return TransferTypeDescriptionTable.GetAllItems(_db, includeInactiveItems);
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
        return (from b in _db.TransferTypeDescriptionTable select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return TransferTypeDescriptionTable.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return TransferTypeDescriptionTable.GetAllUserItems(_db, userID, includeInactiveItems);
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
