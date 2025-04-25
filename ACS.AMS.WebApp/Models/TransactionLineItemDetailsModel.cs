using ACS.AMS.DAL.DBModel;

namespace ACS.AMS.WebApp.Models
{
    public class TransactionLineItemDetailsModel
    {
        #region Static Members

        public static void ClearCurrentModel(int pageID)

        {
            SessionDataContainer.RemoveSessionObject(pageID);
        }

        public static TransactionLineItemDetailsModel GetCurrentModel(int pageID)
        {
            TransactionLineItemDetailsModel model = SessionDataContainer.GetSessionObject<TransactionLineItemDetailsModel>(pageID);
            if (model == null)
            {
                model = new TransactionLineItemDetailsModel();
                SessionDataContainer.SetSessionObject(pageID, model);
            }
            return model;
        }

        #endregion Static Members

        public TransactionLineItemDetailsModel()
        {
            LineItems = new List<TransactionLineItemModel>();
        }

        public List<TransactionLineItemModel> LineItems { get; set; }

    }

    public class TransactionLineItemModel
    {
        private static int idCount = 1;
        public TransactionLineItemModel()
        {
            TransactionLineItemID = -1;
            ID = idCount++;

            if (idCount > int.MaxValue / 2)
                idCount = 1;
        }
        public int? ID { get; set; }
        public int TransactionLineItemID { get; set; }

        public int TransactionID { get; set; }

        public AssetTable Asset { get; set; }
        public int AssetID { get; set; }

        public int? FromLocationID { get; set; }
        public string AssetCode { get; set; }

        public string Barcode { get; set; }
        public string LocationType { get; set; }

        public string MaintenanceRemarks { get; set; }

        public int AdjustmentValue { get; set; }

        public bool IsNetBookAdjustment { get; set; }


        public string CategoryHierarchy { get; set; }
        public string AssetDescription { get; set; }
        public string LocationHierarchy { get; set; }

    }

}
