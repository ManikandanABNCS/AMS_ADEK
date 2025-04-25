namespace ACS.AMS.WebApp.Models.Mobile
{
    public class AssetConditionData
    {
        public AssetConditionData()
        {

        }
        public int AssetConditionID { get; set; }
        public string AssetConditionCode { get; set; }
        public string AssetConditionName { get; set; }
        public override string ToString()
        {
            return $"{AssetConditionCode} ({AssetConditionName})";
        }
    }
}
