namespace ACS.AMS.WebApp.Models
{
    public class UpdateAssetCostDataModel
    {
        private UpdateAssetCostDataModel()
        {
            
        }

        public static UpdateAssetCostDataModel GetModel(int pageID)
        {
            var model = SessionDataContainer.GetSessionObject<UpdateAssetCostDataModel>(pageID);
            if(model == null)
            {
                model = new UpdateAssetCostDataModel();
                SessionDataContainer.SetSessionObject(pageID, model);
            }

            return model;
        }

        public static void RemoveModel(int pageID)
        {
            SessionDataContainer.RemoveSessionObject(pageID);
        }

        public List<UpdateAssetCostModel> LineItems { get; set; } = new List<UpdateAssetCostModel>();
    }
}
