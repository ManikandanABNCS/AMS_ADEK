namespace ACS.AMS.WebApp.Models.Mobile
{
    public class SupplierData
    {
        public SupplierData()
        {

        }
        public int SupplierID { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public override string ToString()
        {
            return $"{SupplierCode} ({SupplierName})";
        }
    }
}
