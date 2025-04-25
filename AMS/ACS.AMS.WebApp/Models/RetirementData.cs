using ACS.AMS.DAL.DBModel;
using Telerik.SvgIcons;

namespace ACS.AMS.WebApp.Models
{
    public class RetirementData
    {
        private static int idGenerator = -1;

        public RetirementData()
        {
            if (idGenerator < -100000)
                idGenerator = -1;

            id = idGenerator--;
        }

        public int id { get; set; }
        
        public string RetirementCode { get; set; }
        public AssetTable Asset { get; set; }

      



    }
}
