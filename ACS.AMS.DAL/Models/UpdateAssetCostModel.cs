using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACS.AMS.DAL.DBContext;
using ACS.AMS.DAL.DBModel;


namespace ACS.AMS.DAL.Models
{
 
        public class UpdateAssetCostModel
        {
            public int? ID { get; set; }

            public string Barcode { get; set; }
            public string AssetDesc { get; set; }
            public string CategoryName { get; set; }

            public decimal? AdditionalCost { get; set; }
            public decimal? CurrentCost { get; set; }

            public int AssetID { get; set; }
            public string PONumber { get; set; }
            public string InvoiceNo { get; set; }
            public string DOFPO_LINE_NUM { get; set; }
        }
}
