using System;
using System.Data;
using ACS.AMS.DAL.DBContext;
using ACS.AMS.DAL.DBModel;
using System.Collections;
using ACS.AMS.DAL;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings;
using System.Reflection.Metadata.Ecma335;

namespace ACS.AMS.WebApp.Classes
{
    public class ImportExcelTelerik
    {
        public static DataTable Import(DataTable dt, Stream input, AMSContext db,string path,string rootFolderName,int appPageID)
        {
            Workbook workbook;
            IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();
          
            //using (Stream input = new FileStream(fileName, FileMode.Open))
            {
                workbook =  formatProvider.Import(input);
            }

            Worksheet worksheet = workbook.Worksheets[0];
            int rCount = worksheet.UsedCellRange.RowCount;
            int cols = worksheet.UsedCellRange.ColumnCount;

            var imgs = worksheet.Images;
            int PersonCodeIndex = 0;
           
            for (int i = 0; i < cols; i++)
            {
                CellSelection selection = worksheet.Cells[0, i];
                var columnName = selection.GetValue().Value.RawValue.ToString();
                if (appPageID == (int)EntityValues.custodianTable)
                {
                    if (string.Compare(columnName.Replace(" ", ""), "PersonCode") == 0)
                    {
                        PersonCodeIndex = i;
                    }
                }
                if (appPageID == (int)EntityValues.CategoryTable)
                {
                    if (string.Compare(columnName.Replace(" ", ""), "CategoryCode") == 0)
                    {
                        PersonCodeIndex = i;
                    }
                }
                if (appPageID == (int)EntityValues.ProductTable)
                {
                    if (string.Compare(columnName.Replace(" ", ""), "ProductCode") == 0)
                    {
                        PersonCodeIndex = i;
                    }
                }
                dt.Columns.Add(columnName.Replace(" ",""));
            }
            
            for (int i = 0; i < rCount; i++)
            {
                string imgName = string.Empty;
                var values = new object[cols];//new object[cols.Length + 1];
                for (int j = 0; j < cols; j++)
                {

                    CellSelection selection = worksheet.Cells[i, j];
                    var columnName = selection.GetValue().Value.RawValue.ToString();
                   
                        ICellValue value = selection.GetValue().Value;
                    CellValueFormat format = selection.GetFormat().Value;
                    CellValueFormatResult formatResult = format.GetFormatResult(value);
                    string result = formatResult.InfosText;

                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        result = result.Replace((char)0, ' ');
                        result = result.Trim();
                    }

                    if (string.IsNullOrWhiteSpace(result))
                    {
                        values[j] = DBNull.Value;
                    }
                    else
                    {
                        values[j] = result;
                        if (j == PersonCodeIndex)
                        {
                            imgName = result;
                        }
                    }
                   
                    var img= (from b in imgs where b.CellIndex.ColumnIndex == j && b.CellIndex.RowIndex == i select b).FirstOrDefault();
                    if (img != null)
                    {
                        string fullPath = Path.Combine(path, string.Concat(imgName, '.', img.ImageSource.Extension));
                        //string filePath = "~/ProfilePic/" + obj.image.ToString();
                        File.WriteAllBytes(fullPath, img.ImageSource.Data);
                        values[j] = fullPath.Replace(string.Concat(rootFolderName,"\\"),"");
                        //values[j] = img.ImageSource.Data;
                    }
                }

                dt.Rows.Add(values);
            }


            return dt;
        }
    }
}