using ACS.AMS.DAL.DBModel;

namespace ACS.AMS.WebApp.Models.NotificationModule
{
    public class NotificationFieldDataModel
    {
        #region Static Members

        public static NotificationFieldDataModel GetCurrentModel(int pageID)
        {
            NotificationFieldDataModel model = SessionDataContainer.GetSessionObject<NotificationFieldDataModel>(pageID);
            if (model == null)
            {
                model = new NotificationFieldDataModel();
                SessionDataContainer.SetSessionObject(pageID, model);
            }
            return model;
        }
        public static void RemoveModel(int pageID)
        {
            SessionDataContainer.RemoveSessionObject(pageID);
        }
        #endregion Static Members

        public NotificationFieldDataModel()
        {

        }

        /// <summary>
        /// All columns from View or Procedure
        /// </summary>
        public List<NotificationModuleFieldTable> Fields { get; set; } = new List<NotificationModuleFieldTable>();

 
    }
}