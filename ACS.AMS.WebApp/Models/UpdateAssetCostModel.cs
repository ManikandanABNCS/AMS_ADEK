using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACS.AMS.DAL.DBContext;
using ACS.AMS.DAL.DBModel;


namespace ACS.AMS.WebApp.Models
{

    public class UpdateAssetCostModel
    {
        private static int idGenerator = -1;

        public UpdateAssetCostModel()
        {
            if (idGenerator < -100000)
                idGenerator = -1;

            ID = idGenerator--;
        }
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
