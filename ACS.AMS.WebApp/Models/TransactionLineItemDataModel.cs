namespace ACS.AMS.WebApp.Models
{
    public class TransactionLineItemDataModel
    {
        private TransactionLineItemDataModel()
        {
            
        }

        public static TransactionLineItemDataModel GetModel(int pageID)
        {
            var model = SessionDataContainer.GetSessionObject<TransactionLineItemDataModel>(pageID);
            if(model == null)
            {
                model = new TransactionLineItemDataModel();
                SessionDataContainer.SetSessionObject(pageID, model);
            }

            return model;
        }

        public static void RemoveModel(int pageID)
        {
            SessionDataContainer.RemoveSessionObject(pageID);
        }

        public List<TransactionLineItemData> LineItems { get; set; } = new List<TransactionLineItemData>();
    }
}
