using DocumentFormat.OpenXml.Spreadsheet;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Telerik.Reporting;
using Telerik.Reporting.Drawing;
using Telerik.Reporting.Processing;

namespace ReportLibrary
{
    partial class GenericReport
    {
        private DataSet dataSet;
        private static Telerik.Reporting.Report report ;

        private static void InitializeComponent(DataTable dataTable)
        {
            report= new Telerik.Reporting.Report();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();

            Telerik.Reporting.PageHeaderSection pageHeaderSection1 = new Telerik.Reporting.PageHeaderSection();
            Telerik.Reporting.DetailSection detail = new Telerik.Reporting.DetailSection();
            Telerik.Reporting.PageFooterSection pageFooterSection1 = new Telerik.Reporting.PageFooterSection();

            ((System.ComponentModel.ISupportInitialize)(report)).BeginInit();
            //
            // pageHeaderSection1
            //
            pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1D);
            pageHeaderSection1.Name = "pageHeaderSection1";
            //
            // detail
            //
            detail.Height = Telerik.Reporting.Drawing.Unit.Inch(2D);
            detail.Name = "detail";
            //
            // pageFooterSection1
            //
            pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1D);
            pageFooterSection1.Name = "pageFooterSection1";
            //
            // GenericReport
            //

    
            Telerik.Reporting.Table table = null;
            table = GenerateTable(dataTable);
            detail.Items.Add(table);
            

            report.Items.AddRange(new Telerik.Reporting.ReportItemBase[] { pageHeaderSection1, detail, pageFooterSection1 });
            report.Name = "GenericReport";
            report.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0), Telerik.Reporting.Drawing.Unit.Inch(0), Telerik.Reporting.Drawing.Unit.Inch(0), Telerik.Reporting.Drawing.Unit.Inch(0));
            report.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            report.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1});
            report.Width = Telerik.Reporting.Drawing.Unit.Inch(6.125D);
            ((System.ComponentModel.ISupportInitialize)(report)).EndInit();
        }

        private static Telerik.Reporting.Table GenerateTable(DataTable dataTable)
        {
            Telerik.Reporting.Table table = new Telerik.Reporting.Table();
            table.Name = dataTable.TableName+Guid.NewGuid();

            //Create columns
            int counterColumn = 0;
            foreach (System.Data.DataColumn column in dataTable.Columns)
            {
                //Add column
                table.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1D)));

                //Create group to include column
                Telerik.Reporting.TableGroup tmpTableGroup = new Telerik.Reporting.TableGroup();
                tmpTableGroup.Name = "tableGroup" + counterColumn;

                //Column textBox
                Telerik.Reporting.TextBox tmpTextBox = new Telerik.Reporting.TextBox();
                tmpTextBox.Name = "textBox" + counterColumn;
                tmpTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.94166666269302368D), Telerik.Reporting.Drawing.Unit.Inch(0.19166666269302368D));
                tmpTextBox.Value = column.ColumnName;
                tmpTableGroup.ReportItem = tmpTextBox;
                table.ColumnGroups.Add(tmpTableGroup);

                table.Items.Add(tmpTextBox);

                counterColumn++;
            }

            //Create rows

            Telerik.Reporting.TableGroup tmpTableGroupRowForChild = new TableGroup();
            tmpTableGroupRowForChild.Name = "detailTableGroup";

            int counterRow = 0;
            foreach (System.Data.DataRow row in dataTable.Rows)
            {
                table.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Unit.Pixel(30)));

                Telerik.Reporting.TableGroup tmpTableGroup = new TableGroup();
                tmpTableGroup.Name = "group" + counterRow;

                tmpTableGroupRowForChild.ChildGroups.Add(tmpTableGroup);

                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    Telerik.Reporting.TextBox tmpTextBox = new Telerik.Reporting.TextBox();
                    tmpTextBox.Name = "dd";
                    tmpTextBox.Value = row.ItemArray[i].ToString();
                    tmpTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(0.5D));

                    table.Body.SetCellContent(counterRow, i, tmpTextBox);
                    table.Items.Add(tmpTextBox);
                }

                counterRow++;
            }

            tmpTableGroupRowForChild.Groupings.Add(new Telerik.Reporting.Grouping(null));
            table.RowGroups.Add(tmpTableGroupRowForChild);
            //table.RowGroups.Add(tmpTableGroupRowForChild);
            table.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8250000476837158D), Telerik.Reporting.Drawing.Unit.Inch(0.98333334922790527D));
            table.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896D));

            return table;
        }

        public static Telerik.Reporting.Report GenericDynamicReport(DataTable dataSet)
        {
            InitializeComponent(dataSet);

            return report;
        }
        //public static Telerik.Reporting.Report GenericDynamicReport(IWebHostEnvironment _env, DataTable dataSet)
        //{
        //    InitializeComponent(_env, dataSet);

        //    return report;
        //}
    }
}