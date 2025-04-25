namespace ACS.AMS.WebApp.Models.Mobile
{
    public class DisposalAssetData
    {
        public DisposalAssetData()
        {

        }
        public int? AssetID { get; set; }
        public String Barcode { get;set; }    
        public string DisposalReferenceNo { get; set; }
        public DateTime? DisposedDateTime { get; set; }
        public string DisposedRemarks { get; set; }
        public decimal? DisposalValue { get; set; }
        public int? CompanyID { get; set; }
        public int? DisposalTypeID { get; set; }
        public decimal? CurrentCost { get; set; }
        public decimal? ProceedofSales { get; set; }
        public string SoldTo { get; set; }
        public decimal? CostOfRemoval { get; set; }
        
    }
}
