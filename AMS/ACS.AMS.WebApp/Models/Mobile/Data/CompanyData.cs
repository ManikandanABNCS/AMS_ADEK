namespace ACS.AMS.WebApp.Models.Mobile
{
    public class CompanyData
    {
        public CompanyData()
        {

        }
        public int CompanyID { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }

        public override string ToString()
        {
            return $"{CompanyCode} ({CompanyName})";
        }
    }
}
