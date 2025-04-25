using ACS.AMS.DAL.DBModel;
using System.Data;

namespace ACS.AMS.WebApp.Models
{
    public class TransferAssetDataModel
    {
        private TransferAssetDataModel()
        {
            LineItems = new List<TransferAssetData>();
            Documents = new List<DocumentModel>();
        }
        public static TransferAssetDataModel GetModel(int pageID)
        {
            var model = SessionDataContainer.GetSessionObject<TransferAssetDataModel>(pageID);
            if (model == null)
            {
                model = new TransferAssetDataModel();
                SessionDataContainer.SetSessionObject(pageID, model);
            }

            return model;
        }

        public static void RemoveModel(int pageID)
        {
            SessionDataContainer.RemoveSessionObject(pageID);
        }

        public List<TransferAssetData> LineItems { get; set; } = new List<TransferAssetData>();
        public List<DocumentModel> Documents { get; set; } = new List<DocumentModel>();
    }

    public class ImportDataModel
    {
        private ImportDataModel()
        {
            ImportData = new DataTable();
        }
        public static ImportDataModel GetModel(int pageID)
        {
            var model = SessionDataContainer.GetSessionObject<ImportDataModel>(pageID);
            if (model == null)
            {
                model = new ImportDataModel();
                SessionDataContainer.SetSessionObject(pageID, model);
            }

            return model;
        }
        public static void RemoveModel(int pageID)
        {
            SessionDataContainer.RemoveSessionObject(pageID);
        }
        public DataTable ImportData { get; set; } = new DataTable();
    }
    //public class ImportData
    //{
    //    private static int idGenerator = -1;

    //    public ImportData()
    //    {
    //        if (idGenerator < -100000)
    //            idGenerator = -1;

    //        id = idGenerator--;
    //    }

    //    public int id { get; set; }

    
    //    public DataTable Data { get; set; }





    //}

}
