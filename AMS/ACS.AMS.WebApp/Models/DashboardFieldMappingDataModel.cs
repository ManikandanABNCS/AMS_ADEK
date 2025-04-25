using ACS.AMS.DAL.DBModel;

namespace ACS.AMS.WebApp.Models
{
    public class DashboardFieldMappingDataModel
    {
        private DashboardFieldMappingDataModel()
        {
            LineItems = new List<DashboardFieldMappingModel>();
        }
       
        public static DashboardFieldMappingDataModel GetModel(int pageID)
        {
            var model = SessionDataContainer.GetSessionObject<DashboardFieldMappingDataModel>(pageID);
            if (model == null)
            {
                model = new DashboardFieldMappingDataModel();
                SessionDataContainer.SetSessionObject(pageID, model);
            }

            return model;
        }   

        public static void RemoveModel(int pageID)
        {
            SessionDataContainer.RemoveSessionObject(pageID);
        }

        public List<DashboardFieldMappingModel> LineItems { get; set; } = new List<DashboardFieldMappingModel>();
    }

    public class DashboardFieldMappingModel
    {
        private static int idCount = 1;
        public DashboardFieldMappingModel()
        {
            DashboardFieldMappingID = -1;
            ID = idCount++;

            if (idCount > int.MaxValue / 2)
                idCount = 1;
        }
        public int? ID { get; set; }
        public int? DashboardMappingID { get; set; }
        public int DashboardFieldMappingID { get; set; }
        public int DashboardTemplateID { get; set; }
        public int DashboardTypeID { get; set; }
        public int DashboardTemplateFieldID { get; set; }
        public string FieldName { get; set; }
        public string DisplayTitle { get; set; }
        public string ColorCode { get; set; }
        public string XAxisField { get; set; }
        public string CategoriesField { get; set; }
        public string YAxisField { get; set; }
        public string IconPath { get; set; }
        public string RedirectPageName { get; set; }
    }
}
