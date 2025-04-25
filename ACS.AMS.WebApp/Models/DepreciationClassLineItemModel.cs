using ACS.AMS.DAL.DBModel;
using DocumentFormat.OpenXml.Office2010.Excel;
using Telerik.SvgIcons;

namespace ACS.AMS.WebApp.Models
{
    public class LineItemModel
    {
        private static int idCount = 1;

        public LineItemModel()
        {
            LineItemID = -1;
            id = idCount++;

            if (idCount > int.MaxValue / 2)
                idCount = 1;
        }

        public int? id { get; set; }
        public int LineItemID { get; set; }
        public string StartValue { get; set; }
        public string EndValue { get; set; }
        public bool UnEndValue { get; set; }
             
        public int Duration { get; set; }


    }

    public class DepreciationClassLineItemModel
    {
        private DepreciationClassLineItemModel()
        {
            LineItems = new List<LineItemModel>();
           
        }
        public static DepreciationClassLineItemModel GetModel(int pageID)
        {
            var model = SessionDataContainer.GetSessionObject<DepreciationClassLineItemModel>(pageID);
            if (model == null)
            {
                model = new DepreciationClassLineItemModel();
                SessionDataContainer.SetSessionObject(pageID, model);
            }

            return model;
        }

        public static void RemoveModel(int pageID)
        {
            SessionDataContainer.RemoveSessionObject(pageID);
        }
        public List<LineItemModel> LineItems { get; set; } = new List<LineItemModel>();
    }
}
