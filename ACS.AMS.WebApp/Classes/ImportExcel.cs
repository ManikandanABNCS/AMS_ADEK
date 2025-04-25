using System;
using NPOI.HSSF.UserModel;
using System.Data;
using ACS.AMS.DAL.DBContext;
using ACS.AMS.DAL.DBModel;
using System.Collections;
using NPOI.XSSF.UserModel;
using ACS.AMS.DAL;
using DocumentFormat.OpenXml.Spreadsheet;
using NPOI.SS.UserModel;

namespace ACS.AMS.WebApp.Classes
{
    public class ImportExcel
    {
        public static DataTable Import(DataTable dt, HSSFWorkbook workbook, AMSContext db)
        {
            NPOI.SS.UserModel.ISheet sheet = workbook.GetSheetAt(0);
           // DataTable dt1 = sheet.Cells.ExportDataTable(0, 0, sheet.Cells.Rows.Count, sheet.Cells.Columns.Count + 1, true);
            var lst = workbook.GetAllPictures();
            for (int i = 0; i < lst.Count; i++)
            {
                var pic = (HSSFPictureData)lst[i];
                byte[] data = pic.Data;

                /*Save Image From Byte[]*/
            }
            IEnumerator rows = sheet.GetRowEnumerator();
            for (int j = 0; j < (sheet.GetRow(0).LastCellNum); j++)
            {
                dt.Columns.Add(sheet.GetRow(0).Cells[j].ToString().Replace(" ",""));
            }
          
            while (rows.MoveNext())
            {
                
                HSSFRow row = (HSSFRow)rows.Current;
                DataRow dr = dt.NewRow();
                for (int i = 0; i < row.LastCellNum; i++)
                {
                    NPOI.SS.UserModel.ICell cell = row.GetCell(i);
                    if (cell == null)
                    {
                        dr[i] = null;
                    }
                    else
                    {
                        dr[i] = GetValue(cell.CellType, cell);
                    }
                }
                dt.Rows.Add(dr);
            }
            dt.Rows.RemoveAt(0);
            return dt;
        }
        public static DataTable ExcelHeaderName(DataTable dt, HSSFWorkbook workbook, AMSContext db)
        {
            try
            {
                NPOI.SS.UserModel.ISheet sheet = workbook.GetSheetAt(0);
                IEnumerator rows = sheet.GetRowEnumerator();
                for (int j = 0; j < (sheet.GetRow(0).LastCellNum); j++)
                {
                    dt.Columns.Add(sheet.GetRow(0).Cells[j].ToString().Replace(" ", ""));
                }
                return dt;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogTable.SaveException(ex);
                throw ex;
            }
        }

        public static DataTable ExcelHeaderName(DataTable dt, XSSFWorkbook workbook, AMSContext db)
        {
            try
            {
                NPOI.SS.UserModel.ISheet sheet = workbook.GetSheetAt(0);
                IEnumerator rows = sheet.GetRowEnumerator();
                for (int j = 0; j < (sheet.GetRow(0).LastCellNum); j++)
                {
                    dt.Columns.Add(sheet.GetRow(0).Cells[j].ToString().Replace(" ", ""));
                }
                return dt;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogTable.SaveException(ex);
                throw ex;
            }
        }

        public static DataTable Import(DataTable dt, XSSFWorkbook workbook, AMSContext db)
        {
            try
            {
                NPOI.SS.UserModel.ISheet sheet = workbook.GetSheetAt(0);
                IEnumerator rows = sheet.GetRowEnumerator();
                for (int j = 0; j < (sheet.GetRow(0).LastCellNum); j++)
                {
                    dt.Columns.Add(sheet.GetRow(0).Cells[j].ToString().Replace(" ", ""));
                }
                int k = 0;
                while (rows.MoveNext())
                {
                    //AppErrorLogTable.SaveException(WasNowContext.CreateNewContext(),new Exception("table row for " + k + ", result: " ));
                    XSSFRow row = (XSSFRow)rows.Current;
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < row.LastCellNum; i++)
                    {
                        NPOI.SS.UserModel.ICell cell = row.GetCell(i);
                        if (cell == null)
                        {
                            dr[i] = null;
                        }
                        else
                        {
                            dr[i] = GetValue(cell.CellType, cell);
                        }
                    }
                    dt.Rows.Add(dr);
                    k = k++;

                }
                dt.Rows.RemoveAt(0);
                //if (dt != null && dt.Rows.Count != 0)
                //{
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        book b = new book();
                //        b.dld = dt.Rows[i]["dld"].ToString();
                //        b.BO = dt.Rows[i]["BO"].ToString();
                //        b.paymentstatus = dt.Rows[i]["paymentstatus"].ToString();
                //        var flag = db.books.Where(x => x.BO == b.BO).FirstOrDefault();
                //        if (flag != null && flag.paymentstatus != b.paymentstatus)
                //        {
                //            flag.paymentstatus = b.paymentstatus;
                //            db.books.AddOrUpdate(flag);
                //        }
                //        if (flag != null)
                //        {
                //            db.books.AddOrUpdate(flag);
                //        }
                //        else
                //        {
                //            db.books.AddOrUpdate(b);
                //        }
                //    }
                //}
                //db.SaveChanges();
                return dt;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogTable.SaveException(ex);
                throw ex;
            }
        }

        private static object GetValue(NPOI.SS.UserModel.CellType cellType, NPOI.SS.UserModel.ICell cell)
        {
            switch (cellType)
            {
                case NPOI.SS.UserModel.CellType.String:
                    return cell.StringCellValue;

                case NPOI.SS.UserModel.CellType.Numeric:
                    return cell.NumericCellValue;

                case NPOI.SS.UserModel.CellType.Blank:
                    return null;

                case (NPOI.SS.UserModel.CellType.Boolean):
                    return cell.BooleanCellValue + "";

                case (NPOI.SS.UserModel.CellType.Formula):
                    return GetValue(cell.CachedFormulaResultType, cell);

                case NPOI.SS.UserModel.CellType.Unknown:
                    return cell.ToString();


                default:
                    return cell.ToString();

            }

        }
    }
}
