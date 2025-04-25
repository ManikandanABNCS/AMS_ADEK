namespace ACS.AMS.WebApp.Models.Mobile
{
    public class LocationData
    {
        public LocationData()
        {

        }

        public int LocationID { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public int? parentLocationID { get; set; }

        public override string ToString()
        {
            return $"{LocationCode} ({LocationName})";
        }
    }
}
