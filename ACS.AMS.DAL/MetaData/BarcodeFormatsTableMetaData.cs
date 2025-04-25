using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace ACS.AMS.DAL.DBModel
{
    public class BarcodeFormatsMeatDataTable
    {
        [StringLength(30)]
        [Required(ErrorMessage = "Format Name Field is required")]
        [DisplayName("Format Name")]
        public string FormatName { get; set; }

        [StringLength(100)]
        [DisplayName("Company Name")]
        //[RegularExpression("([/^]{1}[a-zA-Z0-9]{3}[/,]{1}[0-9]{1,4}[/,]{1}[0-9]{1,4}[/^]{1}[a-zA-Z]{2}[0-9]{1,4}[/,]{1}[0-9]{1,4}[/^]{1}[a-zA-Z0-9]{3}[/^]{1}[a-zA-Z]{2})", ErrorMessage = "Enter Company Name ZPL Format")]
        [RegularExpression("([/^]{1}[a-zA-Z0-9]{3}(.*)[/^]{1}[a-zA-Z]{2})", ErrorMessage = "Enter Company Name ZPL Format")]
        public string CompanyName { get; set; }

        [StringLength(200)]
        [DisplayName("Barcode Header")]
        //[RegularExpression("([/^]{1}[a-zA-Z0-9]{3}[/,]{1}[0-9]{1,4}[/,]{1}[0-9]{1,4}[/^]{1}[a-zA-Z]{2}[0-9]{1,4}[/,]{1}[0-9]{1,4}[/^]{1}[a-zA-Z0-9]{3}[/^]{1}[a-zA-Z]{2})", ErrorMessage = "Enter Company Name ZPL Format")]
        [RegularExpression("([/^]{1}[a-zA-Z0-9]{3}(.*)[/^]{1}[a-zA-Z]{2})", ErrorMessage = "Enter Barcode Header ZPL Format")]
        public string BarcodeHeader { get; set; }
        [StringLength(200)]
        [DisplayName("Barcode Footer")]
        //[RegularExpression("([/^]{1}[a-zA-Z0-9]{3}[/,]{1}[0-9]{1,4}[/,]{1}[0-9]{1,4}[/^]{1}[a-zA-Z]{2}[0-9]{1,4}[/,]{1}[0-9]{1,4}[/^]{1}[a-zA-Z0-9]{3}[/^]{1}[a-zA-Z]{2})", ErrorMessage = "Enter Company Name ZPL Format")]
        [RegularExpression("([/^]{1}[a-zA-Z0-9]{3}(.*)[/^]{1}[a-zA-Z]{2})", ErrorMessage = "Enter Barcode Footer ZPL Format")]
        public string BarcodeFooterr { get; set; }


        [StringLength(100)]
        //[AllowHtml]
        //[RegularExpression("([/^]{1}[a-zA-Z0-9]{3}[/^]{1}[a-zA-Z]{2}[0-9]{1,4}[/,]{1}[0-9]{1,4}[/^]{1}[a-zA-Z]{3}[/,]{1}[0-9]{1,4}[/,]{1}[a-zA-Z]{1}[/,]{1}[a-zA-Z]{1}[/,]{1}[a-zA-Z]{1}[/^]{1}[a-zA-Z]{2})", ErrorMessage = "Enter Barcode ZPL Format")]
        [RegularExpression("([/^]{1}[a-zA-Z0-9]{3}(.*)[/^]{1}[a-zA-Z]{2})", ErrorMessage = "Enter Barcode ZPL Format")]
        [DisplayName("Barcode")]
        public string Barcode { get; set; }

        [StringLength(100)]
        [DisplayName("Human Readable")]
        //[RegularExpression("([/^]{1}[a-zA-Z0-9]{3}[/,]{1}[0-9]{1,4}[/,]{1}[0-9]{1,4}[/^]{1}[a-zA-Z]{2}[0-9]{1,4}[/,]{1}[0-9]{1,4}[/^]{1}[a-zA-Z0-9]{3}[/^]{1}[a-zA-Z]{2})", ErrorMessage = "Enter Human Readable ZPL Format")]
        [RegularExpression("([/^]{1}[a-zA-Z0-9]{3}(.*)[/^]{1}[a-zA-Z]{2})", ErrorMessage = "Enter Human Readable ZPL Format")]
      //  [AllowHtml]
        public string HumanReadable { get; set; }

        [StringLength(100)]
        [DisplayName("Asset Description")]
        //[RegularExpression("([/^]{1}[a-zA-Z0-9]{3}[/,]{1}[0-9]{1,4}[/,]{1}[0-9]{1,4}[/^]{1}[a-zA-Z]{2}[0-9]{1,4}[/,]{1}[0-9]{1,4}[/^]{1}[a-zA-Z0-9]{3}[/^]{1}[a-zA-Z]{2})", ErrorMessage = "Enter Asset Desc ZPL Format")]
        [RegularExpression("([/^]{1}[a-zA-Z0-9]{3}(.*)[/^]{1}[a-zA-Z]{2})", ErrorMessage = "Enter Asset Desc ZPL Format")]
      //  [AllowHtml]
        public string AssetDescription { get; set; }

        [StringLength(100)]
        [DisplayName("Category")]
        //[RegularExpression("([/^]{1}[a-zA-Z0-9]{3}[/,]{1}[0-9]{1,4}[/,]{1}[0-9]{1,4}[/^]{1}[a-zA-Z]{2}[0-9]{1,4}[/,]{1}[0-9]{1,4}[/^]{1}[a-zA-Z0-9]{3}[/^]{1}[a-zA-Z]{2})", ErrorMessage = "Enter Category ZPL Format")]
        [RegularExpression("([/^]{1}[a-zA-Z0-9]{3}(.*)[/^]{1}[a-zA-Z]{2})", ErrorMessage = "Enter Category ZPL Format")]
      //  [AllowHtml]
        public string Category { get; set; }

        [StringLength(100)]
        [DisplayName("Department")]
        //[RegularExpression("([/^]{1}[a-zA-Z0-9]{3}[/,]{1}[0-9]{1,4}[/,]{1}[0-9]{1,4}[/^]{1}[a-zA-Z]{2}[0-9]{1,4}[/,]{1}[0-9]{1,4}[/^]{1}[a-zA-Z0-9]{3}[/^]{1}[a-zA-Z]{2})", ErrorMessage = "Enter Department ZPL Format")]
        [RegularExpression("([/^]{1}[a-zA-Z0-9]{3}(.*)[/^]{1}[a-zA-Z]{2})", ErrorMessage = "Enter Department ZPL Format")]
    //    [AllowHtml]
        public string Department { get; set; }

        [StringLength(100)]
        [DisplayName("Location")]
        //[RegularExpression("([/^]{1}[a-zA-Z0-9]{3}[/,]{1}[0-9]{1,4}[/,]{1}[0-9]{1,4}[/^]{1}[a-zA-Z]{2}[0-9]{1,4}[/,]{1}[0-9]{1,4}[/^]{1}[a-zA-Z0-9]{3}[/^]{1}[a-zA-Z]{2})", ErrorMessage = "Enter Location ZPL Format")]
        [RegularExpression("([/^]{1}[a-zA-Z0-9]{3}(.*)[/^]{1}[a-zA-Z]{2})", ErrorMessage = "Enter Location ZPL Format")]
    //    [AllowHtml]
        public string L1Location { get; set; }

        [StringLength(100)]
        [DisplayName("PurchaseDate")]
        [RegularExpression("([/^]{1}[a-zA-Z0-9]{3}(.*)[/^]{1}[a-zA-Z]{2})", ErrorMessage = "Enter PurchaseDate ZPL Format")]
      //  [AllowHtml]
        public string PurchaseDate { get; set; }

        [StringLength(1000)]
        [DisplayName("Logo Format")]
        [RegularExpression("([/^]{1}[a-zA-Z0-9]{3}(.*)[/^]{1})", ErrorMessage = "Enter Logo ZPL Format")]
      //  [AllowHtml]
        public string LogoFormat { get; set; }


        [StringLength(6000)]
        [DisplayName("Convert Logo to ZPL Format")]
       // [AllowHtml]
        public string ConvertLogoToZPL { get; set; }

    }
}
