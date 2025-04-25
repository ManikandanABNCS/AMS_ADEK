namespace ACS.AMS.WebApp.Models.Mobile
{
    public class ModelData
    {
        public ModelData()
        {

        }
        public int ModelID { get; set; }
        public string ModelCode { get; set; }
        public string ModelName { get; set; }
        public int ManufacturerID { get; set; }
        public override string ToString()
        {
            return $"{ModelCode} ({ModelName})";
        }
    }
}
