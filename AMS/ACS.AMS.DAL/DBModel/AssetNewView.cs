using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel;

[Keyless]
public partial class AssetNewView : BaseEntityObject, IACSDBObject
{
    public int AssetID { get; set; }

    [StringLength(50)]
    public string AssetCode { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Barcode { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? RFIDTagCode { get; set; }

    public int? LocationID { get; set; }

    public int? DepartmentID { get; set; }

    public int? SectionID { get; set; }

    public int? CustodianID { get; set; }

    public int? SupplierID { get; set; }

    public int? AssetConditionID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PONumber { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? PurchaseDate { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? PurchasePrice { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ComissionDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? WarrantyExpiryDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? DisposalReferenceNo { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DisposedDateTime { get; set; }

    public string? DisposedRemarks { get; set; }

    public string? AssetRemarks { get; set; }

    public int? DepreciationClassID { get; set; }

    public bool DepreciationFlag { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? SalvageValue { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? VoucherNo { get; set; }

    [DisplayName("Status")]
    public int StatusID { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDateTime { get; set; }

    public int? LastModifiedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LastModifiedDateTime { get; set; }

    public string AssetDescription { get; set; } = null!;

    [StringLength(50)]
    public string? ReferenceCode { get; set; }

    [StringLength(100)]
    public string? SerialNo { get; set; }

    [StringLength(100)]
    public string? NetworkID { get; set; }

    [StringLength(150)]
    public string? InvoiceNo { get; set; }

    [StringLength(150)]
    public string? DeliveryNote { get; set; }

    [StringLength(150)]
    public string? Make { get; set; }

    [StringLength(150)]
    public string? Capacity { get; set; }

    [StringLength(200)]
    public string? MappedAssetID { get; set; }

    public bool CreateFromHHT { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? DisposalValue { get; set; }

    public bool? MailAlert { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? partialDisposalTotalValue { get; set; }

    public int? IsTransfered { get; set; }

    public int CategoryID { get; set; }

    public int? TransferTypeID { get; set; }

    [Unicode(false)]
    public string? UploadedDocumentPath { get; set; }

    [Unicode(false)]
    public string? UploadedImagePath { get; set; }

    public int? AssetApproval { get; set; }

    [StringLength(100)]
    public string? ReceiptNumber { get; set; }

    public bool InsertedToOracle { get; set; }

    [Column(TypeName = "date")]
    public DateTime? InvoiceDate { get; set; }

    [StringLength(50)]
    public string? DistributionID { get; set; }

    public int ProductID { get; set; }

    public int? CompanyID { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? SyncDateTime { get; set; }

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

    public int? ManufacturerID { get; set; }

    public int? ModelID { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? QFAssetCode { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? DOFPO_LINE_NUM { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? DOFMASS_ADDITION_ID { get; set; }

    public byte ERPUpdateType { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? DOFPARENT_MASS_ADDITION_ID { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? DOFFIXED_ASSETS_UNITS { get; set; }

    public bool zDOF_Asset_Updated { get; set; }

    [Column(TypeName = "decimal(15, 10)")]
    public decimal? Latitude { get; set; }

    [Column(TypeName = "decimal(15, 10)")]
    public decimal? Longitude { get; set; }

    public int? DisposalTypeID { get; set; }

    public string? CategoryName { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? CurrentCost { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? ProceedofSales { get; set; }

    [StringLength(400)]
    [Unicode(false)]
    public string? SoldTo { get; set; }

    public bool? AllowTransfer { get; set; }

    [Column(TypeName = "decimal(20, 5)")]
    public decimal? CostOfRemoval { get; set; }

    public string? CategoryHierarchy { get; set; }

    public string? CategoryCode { get; set; }

    public string? LocationCode { get; set; }

    public string? LocationName { get; set; }

    public string? LocationHierarchy { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? DepartmentCode { get; set; }

    public string? DepartmentName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? SectionCode { get; set; }

    public string? SectionDescription { get; set; }

    [StringLength(100)]
    public string? CustodianCode { get; set; }

    [StringLength(201)]
    public string? CustodianName { get; set; }

    public string? Model { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? AssetConditionCode { get; set; }

    public string? AssetCondition { get; set; }

    [StringLength(100)]
    public string? SupplierCode { get; set; }

    public string? suppliername { get; set; }

    public string? ProductCode { get; set; }

    public string? ProductName { get; set; }

    [StringLength(201)]
    public string? CreateBy { get; set; }

    [StringLength(201)]
    public string? ModifedBy { get; set; }

    [StringLength(50)]
    public string? CompanyCode { get; set; }

    public string? CompanyName { get; set; }

    public string? FirstLevelLocationName { get; set; }

    public string? SecondLevelLocationName { get; set; }

    public string? ThirdLevelLocationName { get; set; }

    public string? FourthLevelLocationName { get; set; }

    public string? FifthLevelLocationName { get; set; }

    public string? SixthLevelLocationName { get; set; }

    public string? FirstLevelCategoryName { get; set; }

    public string? SecondLevelCategoryName { get; set; }

    public string? ThirdLevelCategoryName { get; set; }

    public string? FourthLevelCategoryName { get; set; }

    public string? FifthLevelCategoryName { get; set; }

    public string? SixthLevelCategoryName { get; set; }

    [StringLength(100)]
    public string? Status { get; set; }

    public bool IsScanned { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? LocationL1 { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? LocationL2 { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? LocationL3 { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? LocationL4 { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? LocationL5 { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? LocationL6 { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CategoryL1 { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CategoryL2 { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CategoryL3 { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CategoryL4 { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CategoryL5 { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CategoryL6 { get; set; }

    [StringLength(200)]
    public string? CategoryAttribute1 { get; set; }

    [StringLength(200)]
    public string? CategoryAttribute2 { get; set; }

    [StringLength(200)]
    public string? CategoryAttribute3 { get; set; }

    [StringLength(200)]
    public string? CategoryAttribute4 { get; set; }

    [StringLength(200)]
    public string? CategoryAttribute5 { get; set; }

    [StringLength(200)]
    public string? CategoryAttribute6 { get; set; }

    [StringLength(200)]
    public string? CategoryAttribute7 { get; set; }

    [StringLength(200)]
    public string? CategoryAttribute8 { get; set; }

    [StringLength(200)]
    public string? CategoryAttribute9 { get; set; }

    [StringLength(200)]
    public string? CategoryAttribute10 { get; set; }

    [StringLength(200)]
    public string? CategoryAttribute11 { get; set; }

    [StringLength(200)]
    public string? CategoryAttribute12 { get; set; }

    [StringLength(200)]
    public string? CategoryAttribute13 { get; set; }

    [StringLength(200)]
    public string? CategoryAttribute14 { get; set; }

    [StringLength(200)]
    public string? CategoryAttribute15 { get; set; }

    [StringLength(200)]
    public string? CategoryAttribute16 { get; set; }

    [StringLength(200)]
    public string? LocationAttribute1 { get; set; }

    [StringLength(200)]
    public string? LocationAttribute2 { get; set; }

    [StringLength(200)]
    public string? LocationAttribute3 { get; set; }

    [StringLength(200)]
    public string? LocationAttribute4 { get; set; }

    [StringLength(200)]
    public string? LocationAttribute5 { get; set; }

    [StringLength(200)]
    public string? LocationAttribute6 { get; set; }

    [StringLength(200)]
    public string? LocationAttribute7 { get; set; }

    [StringLength(200)]
    public string? LocationAttribute8 { get; set; }

    [StringLength(200)]
    public string? LocationAttribute9 { get; set; }

    [StringLength(200)]
    public string? LocationAttribute10 { get; set; }

    [StringLength(200)]
    public string? LocationAttribute11 { get; set; }

    [StringLength(200)]
    public string? LocationAttribute12 { get; set; }

    [StringLength(200)]
    public string? LocationAttribute13 { get; set; }

    [StringLength(200)]
    public string? LocationAttribute14 { get; set; }

    [StringLength(200)]
    public string? LocationAttribute15 { get; set; }

    [StringLength(200)]
    public string? LocationAttribute16 { get; set; }

    public string? Attribute17 { get; set; }

    public string? Attribute18 { get; set; }

    public string? Attribute19 { get; set; }

    public string? Attribute20 { get; set; }

    public string? Attribute21 { get; set; }

    public string? Attribute22 { get; set; }

    public string? Attribute23 { get; set; }

    public string? Attribute24 { get; set; }

    public string? Attribute25 { get; set; }

    public string? Attribute26 { get; set; }

    public string? Attribute27 { get; set; }

    public string? Attribute28 { get; set; }

    public string? Attribute29 { get; set; }

    public string? Attribute30 { get; set; }

    public string? Attribute31 { get; set; }

    public string? Attribute32 { get; set; }

    public string? Attribute33 { get; set; }

    public string? Attribute34 { get; set; }

    public string? Attribute35 { get; set; }

    public string? Attribute36 { get; set; }

    public string? Attribute37 { get; set; }

    public string? Attribute38 { get; set; }

    public string? Attribute39 { get; set; }

    public string? Attribute40 { get; set; }

    [StringLength(100)]
    public string? LocationType { get; set; }

    [StringLength(100)]
    public string? CategoryType { get; set; }

    public int? MappedLocationID { get; set; }

    public int? MappedCategoryID { get; set; }

    public int? DisposalTransactionID { get; set; }

    public string? VirtualBarcode { get; set; }

    public string? specification { get; set; }

    /*
        #region Constructors

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AssetNewView()
        {

        }

        /// <summary>
        /// Default constructor for AssetNewView
        /// </summary>
        /// <param name="db"></param>
        public AssetNewView(AMSContext _db)
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
		return "AssetID";
	}

	public override object GetPrimaryKeyValue()
	{
		return AssetID;
	}

	internal override void BeforeSave(AMSContext db)
    {
		if(!string.IsNullOrEmpty(this.AssetCode)) this.AssetCode = this.AssetCode.Trim();
		if(!string.IsNullOrEmpty(this.Barcode)) this.Barcode = this.Barcode.Trim();
		if(!string.IsNullOrEmpty(this.RFIDTagCode)) this.RFIDTagCode = this.RFIDTagCode.Trim();
		if(!string.IsNullOrEmpty(this.PONumber)) this.PONumber = this.PONumber.Trim();
		if(!string.IsNullOrEmpty(this.DisposalReferenceNo)) this.DisposalReferenceNo = this.DisposalReferenceNo.Trim();
		if(!string.IsNullOrEmpty(this.DisposedRemarks)) this.DisposedRemarks = this.DisposedRemarks.Trim();
		if(!string.IsNullOrEmpty(this.AssetRemarks)) this.AssetRemarks = this.AssetRemarks.Trim();
		if(!string.IsNullOrEmpty(this.VoucherNo)) this.VoucherNo = this.VoucherNo.Trim();
		if(!string.IsNullOrEmpty(this.AssetDescription)) this.AssetDescription = this.AssetDescription.Trim();
		if(!string.IsNullOrEmpty(this.ReferenceCode)) this.ReferenceCode = this.ReferenceCode.Trim();
		if(!string.IsNullOrEmpty(this.SerialNo)) this.SerialNo = this.SerialNo.Trim();
		if(!string.IsNullOrEmpty(this.NetworkID)) this.NetworkID = this.NetworkID.Trim();
		if(!string.IsNullOrEmpty(this.InvoiceNo)) this.InvoiceNo = this.InvoiceNo.Trim();
		if(!string.IsNullOrEmpty(this.DeliveryNote)) this.DeliveryNote = this.DeliveryNote.Trim();
		if(!string.IsNullOrEmpty(this.Make)) this.Make = this.Make.Trim();
		if(!string.IsNullOrEmpty(this.Capacity)) this.Capacity = this.Capacity.Trim();
		if(!string.IsNullOrEmpty(this.MappedAssetID)) this.MappedAssetID = this.MappedAssetID.Trim();
		if(!string.IsNullOrEmpty(this.UploadedDocumentPath)) this.UploadedDocumentPath = this.UploadedDocumentPath.Trim();
		if(!string.IsNullOrEmpty(this.UploadedImagePath)) this.UploadedImagePath = this.UploadedImagePath.Trim();
		if(!string.IsNullOrEmpty(this.ReceiptNumber)) this.ReceiptNumber = this.ReceiptNumber.Trim();
		if(!string.IsNullOrEmpty(this.DistributionID)) this.DistributionID = this.DistributionID.Trim();
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
		if(!string.IsNullOrEmpty(this.QFAssetCode)) this.QFAssetCode = this.QFAssetCode.Trim();
		if(!string.IsNullOrEmpty(this.DOFPO_LINE_NUM)) this.DOFPO_LINE_NUM = this.DOFPO_LINE_NUM.Trim();
		if(!string.IsNullOrEmpty(this.DOFMASS_ADDITION_ID)) this.DOFMASS_ADDITION_ID = this.DOFMASS_ADDITION_ID.Trim();
		if(!string.IsNullOrEmpty(this.DOFPARENT_MASS_ADDITION_ID)) this.DOFPARENT_MASS_ADDITION_ID = this.DOFPARENT_MASS_ADDITION_ID.Trim();
		if(!string.IsNullOrEmpty(this.CategoryName)) this.CategoryName = this.CategoryName.Trim();
		if(!string.IsNullOrEmpty(this.SoldTo)) this.SoldTo = this.SoldTo.Trim();
		if(!string.IsNullOrEmpty(this.CategoryHierarchy)) this.CategoryHierarchy = this.CategoryHierarchy.Trim();
		if(!string.IsNullOrEmpty(this.CategoryCode)) this.CategoryCode = this.CategoryCode.Trim();
		if(!string.IsNullOrEmpty(this.LocationCode)) this.LocationCode = this.LocationCode.Trim();
		if(!string.IsNullOrEmpty(this.LocationName)) this.LocationName = this.LocationName.Trim();
		if(!string.IsNullOrEmpty(this.LocationHierarchy)) this.LocationHierarchy = this.LocationHierarchy.Trim();
		if(!string.IsNullOrEmpty(this.DepartmentCode)) this.DepartmentCode = this.DepartmentCode.Trim();
		if(!string.IsNullOrEmpty(this.DepartmentName)) this.DepartmentName = this.DepartmentName.Trim();
		if(!string.IsNullOrEmpty(this.SectionCode)) this.SectionCode = this.SectionCode.Trim();
		if(!string.IsNullOrEmpty(this.SectionDescription)) this.SectionDescription = this.SectionDescription.Trim();
		if(!string.IsNullOrEmpty(this.CustodianCode)) this.CustodianCode = this.CustodianCode.Trim();
		if(!string.IsNullOrEmpty(this.CustodianName)) this.CustodianName = this.CustodianName.Trim();
		if(!string.IsNullOrEmpty(this.Model)) this.Model = this.Model.Trim();
		if(!string.IsNullOrEmpty(this.AssetConditionCode)) this.AssetConditionCode = this.AssetConditionCode.Trim();
		if(!string.IsNullOrEmpty(this.AssetCondition)) this.AssetCondition = this.AssetCondition.Trim();
		if(!string.IsNullOrEmpty(this.SupplierCode)) this.SupplierCode = this.SupplierCode.Trim();
		if(!string.IsNullOrEmpty(this.suppliername)) this.suppliername = this.suppliername.Trim();
		if(!string.IsNullOrEmpty(this.ProductCode)) this.ProductCode = this.ProductCode.Trim();
		if(!string.IsNullOrEmpty(this.ProductName)) this.ProductName = this.ProductName.Trim();
		if(!string.IsNullOrEmpty(this.CreateBy)) this.CreateBy = this.CreateBy.Trim();
		if(!string.IsNullOrEmpty(this.ModifedBy)) this.ModifedBy = this.ModifedBy.Trim();
		if(!string.IsNullOrEmpty(this.CompanyCode)) this.CompanyCode = this.CompanyCode.Trim();
		if(!string.IsNullOrEmpty(this.CompanyName)) this.CompanyName = this.CompanyName.Trim();
		if(!string.IsNullOrEmpty(this.FirstLevelLocationName)) this.FirstLevelLocationName = this.FirstLevelLocationName.Trim();
		if(!string.IsNullOrEmpty(this.SecondLevelLocationName)) this.SecondLevelLocationName = this.SecondLevelLocationName.Trim();
		if(!string.IsNullOrEmpty(this.ThirdLevelLocationName)) this.ThirdLevelLocationName = this.ThirdLevelLocationName.Trim();
		if(!string.IsNullOrEmpty(this.FourthLevelLocationName)) this.FourthLevelLocationName = this.FourthLevelLocationName.Trim();
		if(!string.IsNullOrEmpty(this.FifthLevelLocationName)) this.FifthLevelLocationName = this.FifthLevelLocationName.Trim();
		if(!string.IsNullOrEmpty(this.SixthLevelLocationName)) this.SixthLevelLocationName = this.SixthLevelLocationName.Trim();
		if(!string.IsNullOrEmpty(this.FirstLevelCategoryName)) this.FirstLevelCategoryName = this.FirstLevelCategoryName.Trim();
		if(!string.IsNullOrEmpty(this.SecondLevelCategoryName)) this.SecondLevelCategoryName = this.SecondLevelCategoryName.Trim();
		if(!string.IsNullOrEmpty(this.ThirdLevelCategoryName)) this.ThirdLevelCategoryName = this.ThirdLevelCategoryName.Trim();
		if(!string.IsNullOrEmpty(this.FourthLevelCategoryName)) this.FourthLevelCategoryName = this.FourthLevelCategoryName.Trim();
		if(!string.IsNullOrEmpty(this.FifthLevelCategoryName)) this.FifthLevelCategoryName = this.FifthLevelCategoryName.Trim();
		if(!string.IsNullOrEmpty(this.SixthLevelCategoryName)) this.SixthLevelCategoryName = this.SixthLevelCategoryName.Trim();
		if(!string.IsNullOrEmpty(this.Status)) this.Status = this.Status.Trim();
		if(!string.IsNullOrEmpty(this.LocationL1)) this.LocationL1 = this.LocationL1.Trim();
		if(!string.IsNullOrEmpty(this.LocationL2)) this.LocationL2 = this.LocationL2.Trim();
		if(!string.IsNullOrEmpty(this.LocationL3)) this.LocationL3 = this.LocationL3.Trim();
		if(!string.IsNullOrEmpty(this.LocationL4)) this.LocationL4 = this.LocationL4.Trim();
		if(!string.IsNullOrEmpty(this.LocationL5)) this.LocationL5 = this.LocationL5.Trim();
		if(!string.IsNullOrEmpty(this.LocationL6)) this.LocationL6 = this.LocationL6.Trim();
		if(!string.IsNullOrEmpty(this.CategoryL1)) this.CategoryL1 = this.CategoryL1.Trim();
		if(!string.IsNullOrEmpty(this.CategoryL2)) this.CategoryL2 = this.CategoryL2.Trim();
		if(!string.IsNullOrEmpty(this.CategoryL3)) this.CategoryL3 = this.CategoryL3.Trim();
		if(!string.IsNullOrEmpty(this.CategoryL4)) this.CategoryL4 = this.CategoryL4.Trim();
		if(!string.IsNullOrEmpty(this.CategoryL5)) this.CategoryL5 = this.CategoryL5.Trim();
		if(!string.IsNullOrEmpty(this.CategoryL6)) this.CategoryL6 = this.CategoryL6.Trim();
		if(!string.IsNullOrEmpty(this.CategoryAttribute1)) this.CategoryAttribute1 = this.CategoryAttribute1.Trim();
		if(!string.IsNullOrEmpty(this.CategoryAttribute2)) this.CategoryAttribute2 = this.CategoryAttribute2.Trim();
		if(!string.IsNullOrEmpty(this.CategoryAttribute3)) this.CategoryAttribute3 = this.CategoryAttribute3.Trim();
		if(!string.IsNullOrEmpty(this.CategoryAttribute4)) this.CategoryAttribute4 = this.CategoryAttribute4.Trim();
		if(!string.IsNullOrEmpty(this.CategoryAttribute5)) this.CategoryAttribute5 = this.CategoryAttribute5.Trim();
		if(!string.IsNullOrEmpty(this.CategoryAttribute6)) this.CategoryAttribute6 = this.CategoryAttribute6.Trim();
		if(!string.IsNullOrEmpty(this.CategoryAttribute7)) this.CategoryAttribute7 = this.CategoryAttribute7.Trim();
		if(!string.IsNullOrEmpty(this.CategoryAttribute8)) this.CategoryAttribute8 = this.CategoryAttribute8.Trim();
		if(!string.IsNullOrEmpty(this.CategoryAttribute9)) this.CategoryAttribute9 = this.CategoryAttribute9.Trim();
		if(!string.IsNullOrEmpty(this.CategoryAttribute10)) this.CategoryAttribute10 = this.CategoryAttribute10.Trim();
		if(!string.IsNullOrEmpty(this.CategoryAttribute11)) this.CategoryAttribute11 = this.CategoryAttribute11.Trim();
		if(!string.IsNullOrEmpty(this.CategoryAttribute12)) this.CategoryAttribute12 = this.CategoryAttribute12.Trim();
		if(!string.IsNullOrEmpty(this.CategoryAttribute13)) this.CategoryAttribute13 = this.CategoryAttribute13.Trim();
		if(!string.IsNullOrEmpty(this.CategoryAttribute14)) this.CategoryAttribute14 = this.CategoryAttribute14.Trim();
		if(!string.IsNullOrEmpty(this.CategoryAttribute15)) this.CategoryAttribute15 = this.CategoryAttribute15.Trim();
		if(!string.IsNullOrEmpty(this.CategoryAttribute16)) this.CategoryAttribute16 = this.CategoryAttribute16.Trim();
		if(!string.IsNullOrEmpty(this.LocationAttribute1)) this.LocationAttribute1 = this.LocationAttribute1.Trim();
		if(!string.IsNullOrEmpty(this.LocationAttribute2)) this.LocationAttribute2 = this.LocationAttribute2.Trim();
		if(!string.IsNullOrEmpty(this.LocationAttribute3)) this.LocationAttribute3 = this.LocationAttribute3.Trim();
		if(!string.IsNullOrEmpty(this.LocationAttribute4)) this.LocationAttribute4 = this.LocationAttribute4.Trim();
		if(!string.IsNullOrEmpty(this.LocationAttribute5)) this.LocationAttribute5 = this.LocationAttribute5.Trim();
		if(!string.IsNullOrEmpty(this.LocationAttribute6)) this.LocationAttribute6 = this.LocationAttribute6.Trim();
		if(!string.IsNullOrEmpty(this.LocationAttribute7)) this.LocationAttribute7 = this.LocationAttribute7.Trim();
		if(!string.IsNullOrEmpty(this.LocationAttribute8)) this.LocationAttribute8 = this.LocationAttribute8.Trim();
		if(!string.IsNullOrEmpty(this.LocationAttribute9)) this.LocationAttribute9 = this.LocationAttribute9.Trim();
		if(!string.IsNullOrEmpty(this.LocationAttribute10)) this.LocationAttribute10 = this.LocationAttribute10.Trim();
		if(!string.IsNullOrEmpty(this.LocationAttribute11)) this.LocationAttribute11 = this.LocationAttribute11.Trim();
		if(!string.IsNullOrEmpty(this.LocationAttribute12)) this.LocationAttribute12 = this.LocationAttribute12.Trim();
		if(!string.IsNullOrEmpty(this.LocationAttribute13)) this.LocationAttribute13 = this.LocationAttribute13.Trim();
		if(!string.IsNullOrEmpty(this.LocationAttribute14)) this.LocationAttribute14 = this.LocationAttribute14.Trim();
		if(!string.IsNullOrEmpty(this.LocationAttribute15)) this.LocationAttribute15 = this.LocationAttribute15.Trim();
		if(!string.IsNullOrEmpty(this.LocationAttribute16)) this.LocationAttribute16 = this.LocationAttribute16.Trim();
		if(!string.IsNullOrEmpty(this.Attribute17)) this.Attribute17 = this.Attribute17.Trim();
		if(!string.IsNullOrEmpty(this.Attribute18)) this.Attribute18 = this.Attribute18.Trim();
		if(!string.IsNullOrEmpty(this.Attribute19)) this.Attribute19 = this.Attribute19.Trim();
		if(!string.IsNullOrEmpty(this.Attribute20)) this.Attribute20 = this.Attribute20.Trim();
		if(!string.IsNullOrEmpty(this.Attribute21)) this.Attribute21 = this.Attribute21.Trim();
		if(!string.IsNullOrEmpty(this.Attribute22)) this.Attribute22 = this.Attribute22.Trim();
		if(!string.IsNullOrEmpty(this.Attribute23)) this.Attribute23 = this.Attribute23.Trim();
		if(!string.IsNullOrEmpty(this.Attribute24)) this.Attribute24 = this.Attribute24.Trim();
		if(!string.IsNullOrEmpty(this.Attribute25)) this.Attribute25 = this.Attribute25.Trim();
		if(!string.IsNullOrEmpty(this.Attribute26)) this.Attribute26 = this.Attribute26.Trim();
		if(!string.IsNullOrEmpty(this.Attribute27)) this.Attribute27 = this.Attribute27.Trim();
		if(!string.IsNullOrEmpty(this.Attribute28)) this.Attribute28 = this.Attribute28.Trim();
		if(!string.IsNullOrEmpty(this.Attribute29)) this.Attribute29 = this.Attribute29.Trim();
		if(!string.IsNullOrEmpty(this.Attribute30)) this.Attribute30 = this.Attribute30.Trim();
		if(!string.IsNullOrEmpty(this.Attribute31)) this.Attribute31 = this.Attribute31.Trim();
		if(!string.IsNullOrEmpty(this.Attribute32)) this.Attribute32 = this.Attribute32.Trim();
		if(!string.IsNullOrEmpty(this.Attribute33)) this.Attribute33 = this.Attribute33.Trim();
		if(!string.IsNullOrEmpty(this.Attribute34)) this.Attribute34 = this.Attribute34.Trim();
		if(!string.IsNullOrEmpty(this.Attribute35)) this.Attribute35 = this.Attribute35.Trim();
		if(!string.IsNullOrEmpty(this.Attribute36)) this.Attribute36 = this.Attribute36.Trim();
		if(!string.IsNullOrEmpty(this.Attribute37)) this.Attribute37 = this.Attribute37.Trim();
		if(!string.IsNullOrEmpty(this.Attribute38)) this.Attribute38 = this.Attribute38.Trim();
		if(!string.IsNullOrEmpty(this.Attribute39)) this.Attribute39 = this.Attribute39.Trim();
		if(!string.IsNullOrEmpty(this.Attribute40)) this.Attribute40 = this.Attribute40.Trim();
		if(!string.IsNullOrEmpty(this.LocationType)) this.LocationType = this.LocationType.Trim();
		if(!string.IsNullOrEmpty(this.CategoryType)) this.CategoryType = this.CategoryType.Trim();

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

    public static AssetNewView GetItem(AMSContext _db, int id)
    {
        return (from b in _db.AssetNewView
                where b.AssetID == id
                select b).FirstOrDefault();
    }

    public static IQueryable<AssetNewView> GetAllItemsByDepartment(AMSContext _db, int departmentID , bool includeInactiveItems = false )
    {
        return from b in GetAllItems(_db, includeInactiveItems)
                where b.DepartmentID == departmentID
                select b;
    }

    public static IQueryable<AssetNewView> GetAllItems(AMSContext _db , bool includeInactiveItems = false )
    {
        if(includeInactiveItems)
        {
            return (from b in _db.AssetNewView
                    where b.StatusID != (int) StatusValue.Deleted 
                    orderby b.AssetID descending
                    select b);
        }
        else
        {
            return (from b in _db.AssetNewView
                    where b.StatusID != (int) StatusValue.Deleted 
                           && b.StatusID != (int) StatusValue.Inactive
                    orderby b.AssetID descending
                    select b);
        }
    }

    public static IQueryable<AssetNewView> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = false)
    {
        return from b in AssetNewView.GetAllItems(_db, includeInactiveItems)
                
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
        return (from b in _db.AssetNewView select b);
    }

    #region Interface Methods

    IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems = true)
    {
        return AssetNewView.GetAllItems(_db, includeInactiveItems);
    }

    IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true)
    {
        return AssetNewView.GetAllUserItems(_db, userID, includeInactiveItems);
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
