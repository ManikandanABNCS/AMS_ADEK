using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Keyless]
public partial class PersonView : BaseEntityObject, IACSDBObject
{
    public int PersonID { get; set; }

    [StringLength(100)]
    public string PersonCode { get; set; } = null!;

    [StringLength(100)]
    public string PersonFirstName { get; set; } = null!;

    [StringLength(100)]
    public string PersonLastName { get; set; } = null!;

    [StringLength(200)]
    public string? PersonFirstNameLanguageTwo { get; set; }

    [StringLength(200)]
    public string? PersonLastNameLanguageTwo { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? EMailID { get; set; }

    public int? DepartmentID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DOJ { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string Gender { get; set; } = null!;

    [StringLength(50)]
    public string Culture { get; set; } = null!;

    public int UserTypeID { get; set; }

    [StringLength(100)]
    public string? MobileNo { get; set; }

    [StringLength(100)]
    public string? WhatsAppMobileNo { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [StringLength(250)]
    public string? Status { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? UserType { get; set; }

/*
	#region Constructors

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public PersonView()
    {

    }

	/// <summary>
    /// Default constructor for PersonView
    /// </summary>
    /// <param name="db"></param>
	public PersonView(AMSContext _db)
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
		return "PersonID";
	}

	public override object GetPrimaryKeyValue()
	{
		return PersonID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.PersonCode)) this.PersonCode = this.PersonCode.Trim();
		if(!string.IsNullOrEmpty(this.PersonFirstName)) this.PersonFirstName = this.PersonFirstName.Trim();
		if(!string.IsNullOrEmpty(this.PersonLastName)) this.PersonLastName = this.PersonLastName.Trim();
		if(!string.IsNullOrEmpty(this.PersonFirstNameLanguageTwo)) this.PersonFirstNameLanguageTwo = this.PersonFirstNameLanguageTwo.Trim();
		if(!string.IsNullOrEmpty(this.PersonLastNameLanguageTwo)) this.PersonLastNameLanguageTwo = this.PersonLastNameLanguageTwo.Trim();
		if(!string.IsNullOrEmpty(this.EMailID)) this.EMailID = this.EMailID.Trim();
		if(!string.IsNullOrEmpty(this.Gender)) this.Gender = this.Gender.Trim();
		if(!string.IsNullOrEmpty(this.Culture)) this.Culture = this.Culture.Trim();
		if(!string.IsNullOrEmpty(this.MobileNo)) this.MobileNo = this.MobileNo.Trim();
		if(!string.IsNullOrEmpty(this.WhatsAppMobileNo)) this.WhatsAppMobileNo = this.WhatsAppMobileNo.Trim();
		if(!string.IsNullOrEmpty(this.Username)) this.Username = this.Username.Trim();
		if(!string.IsNullOrEmpty(this.Status)) this.Status = this.Status.Trim();
		if(!string.IsNullOrEmpty(this.UserType)) this.UserType = this.UserType.Trim();

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

    public static PersonView GetItem(AMSContext _db, int id)
    {
        return (from b in _db.PersonView
                where b.PersonID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<PersonView> GetAllItemsByDepartment(AMSContext _db, int departmentID , bool includeInactiveItems = false )
    {
        return from b in GetAllItems(_db, includeInactiveItems)
                where b.DepartmentID == departmentID
                select b;
    }

    public static IQueryable<PersonView> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.PersonView
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.PersonID descending
                    select b);
        }
        else
        {
            return (from b in _db.PersonView
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.PersonID descending
                    select b);
        }
    }

    public static IQueryable<PersonView> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return from b in PersonView.GetAllItems(_db, includeInactiveItems)
                
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
        return (from b in _db.PersonView select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return PersonView.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return PersonView.GetAllUserItems(_db, userID, includeInactiveItems);
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
