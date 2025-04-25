using ACS.AMS.DAL.DBModel;
using ACS.AMS.WebApp.Models;

namespace ACS.AMS.WebApp.Models.ReportTemplate
{
    public class FilterFieldDataModel
    {
        #region Static Members

        public static FilterFieldDataModel GetCurrentModel(int pageID)
        {
            FilterFieldDataModel model = SessionDataContainer.GetSessionObject<FilterFieldDataModel>(pageID);
            if (model == null)
            {
                model = new FilterFieldDataModel();
                SessionDataContainer.SetSessionObject(pageID, model);
            }
            return model;
        }

        #endregion Static Members

        public FilterFieldDataModel()
        {

        }

        /// <summary>
        /// All columns from View or Procedure
        /// </summary>
        public List<ReportTemplateFieldTable> Fields { get; set; } = new List<ReportTemplateFieldTable>();

        /// <summary>
        /// All Parameters from Procedure
        /// All columns from View
        /// </summary>
        public List<ScreenFilterLineItemTable> FilterFields { get; set; } = new List<ScreenFilterLineItemTable>();
    }


    public class FilterFieldDashBoardDataModel
    {
        #region Static Members

        public static FilterFieldDashBoardDataModel GetCurrentModel(int pageID)
        {
            FilterFieldDashBoardDataModel model = SessionDataContainer.GetSessionObject<FilterFieldDashBoardDataModel>(pageID);
            if (model == null)
            {
                model = new FilterFieldDashBoardDataModel();
                SessionDataContainer.SetSessionObject(pageID, model);
            }
            return model;
        }

        #endregion Static Members

        public FilterFieldDashBoardDataModel()
        {

        }

        /// <summary>
        /// All columns from View or Procedure
        /// </summary>
        public List<DashboardTemplateFieldTable> Fields { get; set; } = new List<DashboardTemplateFieldTable>();

        /// <summary>
        /// All Parameters from Procedure
        /// All columns from View
        /// </summary>
        public List<DashboardTemplateFilterFieldTable> FilterFields { get; set; } = new List<DashboardTemplateFilterFieldTable>();
    }
}