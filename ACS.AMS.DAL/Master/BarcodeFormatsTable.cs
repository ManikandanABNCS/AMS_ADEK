using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;


namespace ACS.AMS.DAL.DBModel
{
    [MetadataType(typeof(BarcodeFormatsMeatDataTable))]
    public partial class BarcodeFormatsTable : BaseEntityObject

    {
        
        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            return true;
        }
        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            return true;
        }
        public static string ReadBarcodeString(AMSContext db, int formatID)
        {
            string sampleCode = "";
            var formatItem = BarcodeFormatsTable.GetItem(db, int.Parse(formatID + ""));
            if (!string.IsNullOrEmpty(formatItem.LabelStart))
            {
                sampleCode = formatItem.LabelStart.ToString() + "</br> \r\n";
            }
            else
            {
                sampleCode = "^XA</br>\r\n";
            }
            if (!string.IsNullOrEmpty(formatItem.BarcodeHeader))
            {
                sampleCode += formatItem.BarcodeHeader + "</br>\r\n";
            }
            if (!string.IsNullOrEmpty(formatItem.LogoFormat))
            {
                sampleCode += formatItem.LogoFormat + "LOGOFORMAT^FS</br>\r\n";
            }
            if (!string.IsNullOrEmpty(formatItem.CompanyName))
            {
                sampleCode += formatItem.CompanyName + "\r\n COMPANYNAME^FS</br> \r\n";
            }

            if (!string.IsNullOrEmpty(formatItem.AssetDescription))
            {
                sampleCode += formatItem.AssetDescription + "\r\n ASSETDESCRIPTION^FS</br> \r\n";
            }

            if (!string.IsNullOrEmpty(formatItem.L1Location))
            {
                sampleCode += formatItem.L1Location + "\r\n FULLL1LOCCODE^FS</br> \r\n";
            }
            if (!string.IsNullOrEmpty(formatItem.Category))
            {
                sampleCode += formatItem.Category + "\r\n CATEGORYNAME^FS</br> \r\n";
            }
            if (!string.IsNullOrEmpty(formatItem.ReferenceCode))
            {
                sampleCode += formatItem.ReferenceCode + "\r\n REFERENCECODE^FS</br> \r\n";
            }
            if (!string.IsNullOrEmpty(formatItem.Barcode))
            {
                sampleCode += formatItem.Barcode + "\r\nBARCODEDETAILS^FS</br> \r\n";
            }
            if (!string.IsNullOrEmpty(formatItem.Department))
            {
                sampleCode += formatItem.Barcode + "\r\n DEPARTMENTNAME^FS</br> \r\n";
            }
            if (!string.IsNullOrEmpty(formatItem.HumanReadable))
            {
                sampleCode += formatItem.HumanReadable + "\r\nBARCODEHUMAN^FS</br> \r\n";
            }
            if (!string.IsNullOrEmpty(formatItem.PurchaseDate))
            {
                sampleCode += formatItem.PurchaseDate + "\r\n PURCHASEDATE^FS</br> \r\n";
            }
            if (!string.IsNullOrEmpty(formatItem.SerialNo))
            {
                sampleCode += formatItem.SerialNo + "\r\n SERIALNO^FS</br> \r\n";
            }
            if (!string.IsNullOrEmpty(formatItem.BarcodeFooter))
            {
                sampleCode += formatItem.BarcodeFooter + "</br>\r\n";
            }
            if (!string.IsNullOrEmpty(formatItem.LabelEnd))
            {
                sampleCode += "^PQ1</br>\r\n";
                sampleCode += formatItem.LabelEnd.ToString() + "</br> \r\n";
            }
            else
            {
                sampleCode += "^PQ1</br>\r\n";
                sampleCode += "^XZ</br>  \r\n";
            }

            return sampleCode;
        }
        //public static void WriteCartonLabelDataToFileAndPrint(AMSContext db, string dataToWrite)
        //{
        //    System.IO.StreamWriter sw;
        //    System.IO.FileStream fs;
        //    HttpContext context = HttpContext.Current;
        //    if (!Directory.Exists(context.Server.MapPath("~\\GeneratedLabels")))
        //    {
        //        Directory.CreateDirectory(context.Server.MapPath("~\\GeneratedLabels"));
        //    }
        //    if (File.Exists(context.Server.MapPath("~\\GeneratedLabels\\BarcodeLabel.txt")))
        //    {
        //        File.Delete(context.Server.MapPath("~\\GeneratedLabels\\BarcodeLabel.txt"));
        //    }
        //    fs = new System.IO.FileStream(context.Server.MapPath("~\\GeneratedLabels\\BarcodeLabel.txt"), System.IO.FileMode.Append);
        //    sw = new System.IO.StreamWriter(fs, System.Text.Encoding.ASCII);
        //    sw.WriteLine(dataToWrite);
        //    sw.Flush();
        //    sw.Close();
        //    fs.Close();
        //    fs = new System.IO.FileStream(context.Server.MapPath("~\\GeneratedLabels\\BarcodeLabel.txt"), System.IO.FileMode.Open, FileAccess.Read);
        //    StreamReader d = new StreamReader(fs);
        //    string strData = d.ReadToEnd();
        //    fs.Close();
        //}
    }
}
