using ACS.AMS.DAL;

namespace ACS.WebAppPageGenerator.Models.SystemModels
{
    public class DashboardPageModel : BasePageModel
    {
        public DashboardPageModel()
        {
            PageTitle = "Dashboard Page";
        }
        public int CurrentPageID { get; set; }

        //public BaseEntityObject EntityInstance { get; set; }

        //public IFormCollection FormCollection { get; set; }

        //public List<PageFieldModel> TransactionFields { get; set; } = new List<PageFieldModel>();
    }
}
