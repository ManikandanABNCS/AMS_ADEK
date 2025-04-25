namespace ACS.AMS.WebApp.Models
{
    public class TransactionScheduleListDataModel
    {
        #region Static Members

        public static void ClearCurrentModel(int pageID)

        {
            SessionDataContainer.RemoveSessionObjectWithName("Activity",pageID);
        }

        public static TransactionScheduleListDataModel GetCurrentModel(int pageID)
        {
            TransactionScheduleListDataModel model = SessionDataContainer.GetSessionObjectWithName<TransactionScheduleListDataModel>("Activity",pageID);
            if (model == null)
            {
                model = new TransactionScheduleListDataModel();
                SessionDataContainer.SetSessionObjectWithName("Activity",pageID, model);
            }
            return model;
        }

        #endregion Static Members

        public TransactionScheduleListDataModel()
        {
            LineItems = new List<TransactionScheduleModel>();
        }

        public List<TransactionScheduleModel> LineItems { get; set; }

    }

    public class TransactionScheduleModel
    {
        private static int idCount = 1;
        public TransactionScheduleModel()
        {
            TransactionScheduleID = -1;
            ID = idCount++;

            if (idCount > int.MaxValue / 2)
                idCount = 1;
        }
        public int? ID { get; set; }
        public int TransactionScheduleID { get; set; }

        public int TransactionID { get; set; }
     
       
        public string Activity { get; set; }

        public DateTime ActivityStartDate { get; set; }
        public DateTime ActivityEndDate { get; set; }


    }

}
