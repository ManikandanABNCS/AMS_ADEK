namespace ACS.AMS.WebApp.Models
{
    public class ApprovalRoleModel
    {
        private static int idCount = 1;
        public ApprovalRoleModel()
        {
        ApprovalRoleID = -1;
            ID = idCount++;

            if (idCount > int.MaxValue / 2)
                idCount = 1;
        }
        public int? ID { get; set; }
        
        public int LocationID { get; set; }
        public int ApprovalRoleID { get; set; }
        public string ApprovalRoleName { get; set; }
        public int CategoryTypeID { get; set; }
        public string CategoryType { get; set; }
        public string LocationName { get; set; }
    }
    public class ApprovalRoleDataModel
    {
        private ApprovalRoleDataModel()
        {
            LineItems = new List<ApprovalRoleModel>();
            
        }
        public static ApprovalRoleDataModel GetModel(int pageID)
        {
            var model = SessionDataContainer.GetSessionObject<ApprovalRoleDataModel>(pageID);
            if (model == null)
            {
                model = new ApprovalRoleDataModel();
                SessionDataContainer.SetSessionObject(pageID, model);
            }

            return model;
        }

        public static void RemoveModel(int pageID)
        {
            SessionDataContainer.RemoveSessionObject(pageID);
        }

        public List<ApprovalRoleModel> LineItems { get; set; } = new List<ApprovalRoleModel>();
       
    }
}
