namespace ACS.AMS.WebApp.Models.Mobile
{
    public class CustodianData
    {
        public CustodianData()
        {

        }
        public int CustodianID { get; set; }
        public string CustodianCode { get; set; }
        public string CustodianName { get; set; }
        public string Email { get; set; }
        public int? DepartmentID { get; set; }

        public string MobileNo { get; set; }
        public DateTime? DOJ { get; set; }
        public string Gender { get; set; }    
        public override string ToString()
        {
            return $"{CustodianCode} ({CustodianName})";
        }
    }
}
