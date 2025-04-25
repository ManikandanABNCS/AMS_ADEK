namespace ACS.AMS.WebApp.Models.Mobile
{
    public class ManufacturerData
    {
        public ManufacturerData()
        {

        }
        public int ManufacturerID { get; set; }
        public string ManufacturerCode { get; set; }
        public string ManufacturerName { get; set; }
        public override string ToString()
        {
            return $"{ManufacturerCode} ({ManufacturerName})";
        }
    }
}
