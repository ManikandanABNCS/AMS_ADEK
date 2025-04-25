namespace ACS.AMS.WebApp.Models
{

    public class DashboardSeriesModel
    {
        public string Name { get; set; }
        public string axis { get; set; }
        public IEnumerable<int> Data { get; set; }

    }

}
