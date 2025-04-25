namespace ACS.AMS.WebApp.Models.Mobile
{
    public class AssetData
    {
        public AssetData()
        {

        }
        public int? AssetID { get; set; }
        public string AssetCode { get; set; }
        public string Barcode { get; set; }
        public string RFIDTagCode { get; set; }
        public int? LocationID { get; set; }
        public int? DepartmentID { get; set; }
        public int? SectionID { get; set; }
        public int? CustodianID { get; set; }
        public int? SupplierID { get; set; }
        public int? AssetConditionID { get; set; }
        public string PONumber { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public decimal? PurchasePrice { get; set; }
        public DateTime? ComissionDate { get; set; }
        public DateTime? WarrantyExpiryDate { get; set; }
        public string DisposalReferenceNo { get; set; }
        public DateTime? DisposedDateTime { get; set; }
        public string DisposedRemarks { get; set; }
        public string AssetRemarks { get; set; }

        public string VoucherNo { get; set; }

        public int StatusID { get; set; }

        public string AssetDescription { get; set; }
        public string ReferenceCode { get; set; }
        public string SerialNo { get; set; }
        public string NetworkID { get; set; }
        public string InvoiceNo { get; set; }
        public string DeliveryNote { get; set; }
        public string Make { get; set; }
        public string Capacity { get; set; }
        public string MappedAssetID { get; set; }

        public decimal? DisposalValue { get; set; }

        public decimal? partialDisposalTotalValue { get; set; }

        public int CategoryID { get; set; }
        public int? TransferTypeID { get; set; }


        public string ReceiptNumber { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public int ProductID { get; set; }
        public int? CompanyID { get; set; }

        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }
        public string Attribute4 { get; set; }
        public string Attribute5 { get; set; }
        public string Attribute6 { get; set; }
        public string Attribute7 { get; set; }
        public string Attribute8 { get; set; }
        public string Attribute9 { get; set; }
        public string Attribute10 { get; set; }
        public string Attribute11 { get; set; }
        public string Attribute12 { get; set; }
        public string Attribute13 { get; set; }
        public string Attribute14 { get; set; }
        public string Attribute15 { get; set; }
        public string Attribute16 { get; set; }
        public int? ManufacturerID { get; set; }
        public int? ModelID { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int? DisposalTypeID { get; set; }
        public decimal? CurrentCost { get; set; }
        public decimal? ProceedofSales { get; set; }
        public string SoldTo { get; set; }

        public decimal? CostOfRemoval { get; set; }

        public int? DisposalTransactionID { get; set; }

        public int AssetApproval { get; set; }

        public override string ToString()
        {
            return $"{Barcode} ({AssetDescription})";
        }
    }
}
