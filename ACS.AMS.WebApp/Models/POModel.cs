using ACS.AMS.DAL.DBModel;
using DocumentFormat.OpenXml.Office2010.Excel;
using Telerik.SvgIcons;

namespace ACS.AMS.WebApp.Models
{
    public class POModel
    {
        private static int idCount = 1;

        public POModel()
        {
            TransactionLineItemID = -1;
            id = idCount++;

            if (idCount > int.MaxValue / 2)
                idCount = 1;
        }

        public int? id { get; set; }
        public int TransactionLineItemID { get; set; }
        public int? PurchaseOrderID { get; set; }
        public int? CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string ItemName { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int POQuantity { get; set; }
        public int Quantity { get; set; }
        public string DepartmentName { get; set; }
        public int? DepartmentID { get; set; }
        public decimal? UnitCost { get; set; }
        public string Attribute1 { get; set; }
        public string PO_HEADER_ID { get; set; }
        public string PO_LINE_ID { get; set; }
        public string PoNumber { get; set; }
        public string POLineNumber { get; set; }

        public int? LocationID { get; set; }

        public string LocationName { get; set; }

    }

    public class GenerateAssetFromPOModel
    {
        private GenerateAssetFromPOModel()
        {
            LineItems = new List<POModel>();
           
        }
        public static GenerateAssetFromPOModel GetModel(int pageID)
        {
            var model = SessionDataContainer.GetSessionObject<GenerateAssetFromPOModel>(pageID);
            if (model == null)
            {
                model = new GenerateAssetFromPOModel();
                SessionDataContainer.SetSessionObject(pageID, model);
            }

            return model;
        }

        public static void RemoveModel(int pageID)
        {
            SessionDataContainer.RemoveSessionObject(pageID);
        }
        public List<POModel> LineItems { get; set; } = new List<POModel>();
    }
}
