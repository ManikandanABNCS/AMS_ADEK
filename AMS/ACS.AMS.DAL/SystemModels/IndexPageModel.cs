using ACS.AMS.DAL;

namespace ACS.WebAppPageGenerator.Models.SystemModels
{
    public class IndexPageModel : BasePageModel
    {
        public IndexPageModel()
        {
            
        }

        public string ReadDataHandler { get; set; } = "defaultGridReadMethod";

        public object ReadRouteValues { get; set; } = null;
        
        public string ReadActionName { get; set; } = "_Index";
    }
}
