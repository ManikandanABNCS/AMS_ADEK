using ACS.AMS.DAL;
using ACS.AMS.DAL.DBModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;


namespace ACS.AMS.DAL
{


    
    public partial class GridEntityModel
    {
        public long EntityPrimaryKeyID { get; set; }


    }
    public partial class IndexModel
    {
        public long EntityPrimaryKeyID { get; set; }


        public string Code { get; set; }
        public string Description { get; set; }

        public string Status { get; set; }

        public string Country { get; set; }
    }
    public partial class MultiColumnModel
    {
        public long EntityPrimaryKeyID { get; set; }


        public string Code { get; set; }
        public string Name { get; set; }

        public string DisplayValue { get; set; }

   
    }

}
