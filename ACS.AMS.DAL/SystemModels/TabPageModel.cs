using ACS.WebAppPageGenerator.Models.SystemModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.WebAppPageGenerator.Models.SystemModels
{
    public class TabPageModel : BasePageModel
    {
        public Dictionary<string, BasePageModel> TabPages { get; set; }

        public TabPageModel()
        {
            TabPages = new Dictionary<string, BasePageModel>();
        }

        public void GeneratePage()
        {

        }
    }
}
