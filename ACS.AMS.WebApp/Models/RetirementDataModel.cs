namespace ACS.AMS.WebApp.Models
{
    public class RetirementDataModel
    {
        private RetirementDataModel()
        {
            LineItems = new List<RetirementData>();
            Documents = new List<DocumentModel>();
        }
        public static RetirementDataModel GetModel(int pageID)
        {
            var model = SessionDataContainer.GetSessionObject<RetirementDataModel>(pageID);
            if (model == null)
            {
                model = new RetirementDataModel();
                SessionDataContainer.SetSessionObject(pageID, model);
            }

            return model;
        }

        public static void RemoveModel(int pageID)
        {
            SessionDataContainer.RemoveSessionObject(pageID);
        }

        public List<RetirementData> LineItems { get; set; } = new List<RetirementData>();
        public List<DocumentModel> Documents { get; set; } = new List<DocumentModel>();
    }
    
}
