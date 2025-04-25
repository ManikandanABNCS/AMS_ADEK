using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.WebAppPageGenerator.Models.SystemModels
{
    public class QueryPageFieldModel : PageFieldModel
    {
        public QueryPageFieldModel()
        {

        }

        public string ProcedureName { get; set; }

        public string Param1Name { get; set; }
    }
    public class CataloguePageFieldModel : PageFieldModel
    {
    }
}
