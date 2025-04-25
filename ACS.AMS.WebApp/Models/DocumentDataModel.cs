namespace ACS.AMS.WebApp.Models
{
    public class DocumentDataModel
    {
        private DocumentDataModel()
        {
            LineItems = new List<DocumentModel>();
        }
       
        public static DocumentDataModel GetModel(int pageID)
        {
            var model = SessionDataContainer.GetSessionObject<DocumentDataModel>(pageID);
            if (model == null)
            {
                model = new DocumentDataModel();
                SessionDataContainer.SetSessionObject(pageID, model);
            }

            return model;
        }

        public static void RemoveModel(int pageID)
        {
            SessionDataContainer.RemoveSessionObject(pageID);
        }

        public List<DocumentModel> LineItems { get; set; } = new List<DocumentModel>();
    }
}
