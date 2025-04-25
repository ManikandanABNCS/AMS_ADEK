using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL;
using ACS.AMS.DAL.Models;
using System.Linq;
using ACS.AMS.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using ACS.AMS.DAL.Master.HiFi;
using Microsoft.EntityFrameworkCore;
using ACS.AMS.DAL.DBContext;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ACS.WebAppPageGenerator.Models.SystemModels;
using Microsoft.AspNetCore.Mvc.Razor;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.IdentityModel.Tokens;

namespace ACS.AMS.WebApp
{
    public class DisplayHelper
    {
        public dynamic view;
        private string primaryKeyName;

        public string ControllerName { get; set; }

        public string ColumnIndexName { get; set; }

        public static string IndexPageWidth_Small { get { return "825px"; } }

        public static string IndexPageWidth_Medium { get { return "750px"; } }

        public static string IndexPageWidth_Large { get { return "1070px"; } }

        public static string IndexPageWidth_Full { get { return "1200px"; } }

        public static string GridWidth_Full { get { return "1150px"; } }

        public DisplayHelper(dynamic v, string controller, string pkName)
        {
            view = v;
            ControllerName = controller;
            primaryKeyName = pkName;

            ColumnIndexName = ControllerName;
        }

        public static AjaxOptions GetAjaxOptions(BasePageModel pageModel, InsertionMode insertionMode = InsertionMode.Replace,
             string onBegin = "beginPageLoad", string onSuccess = "successPageLoad", string onFailure = "failurePageLoad",
             string onComplete = "pageLoadCompleted")
        {
            string updateTargetId = "workingArea";
            if (!string.IsNullOrEmpty(pageModel.SubPageName))
                updateTargetId = "childWorkingArea";

            return GetAjaxOptions(updateTargetId, insertionMode, onBegin, onSuccess, onFailure, onComplete);
        }

        public static AjaxOptions GetAjaxOptions(string updateTargetId = "workingArea", InsertionMode insertionMode = InsertionMode.Replace,
             string onBegin = "beginPageLoad", string onSuccess = "successPageLoad", string onFailure = "failurePageLoad",
             string onComplete = "pageLoadCompleted")
        {
            AjaxOptions ajaxOptions = new AjaxOptions();

            ajaxOptions.LoadingElementId = "loadingmask";
            ajaxOptions.UpdateTargetId = updateTargetId;
            ajaxOptions.InsertionMode = insertionMode;
            ajaxOptions.OnBegin = onBegin;
            ajaxOptions.OnSuccess = onSuccess;
            ajaxOptions.OnFailure = onFailure;
            ajaxOptions.OnComplete = onComplete;           
           
            return ajaxOptions;
        }
        
        public void ConfigureGrid<T>(GridBuilder<T> grid, int pageSize = 10, bool pageSizesRequired = true,
                               bool filterable = true, bool groupable = true, string readDataHandler = "defaultGridReadMethod",
                               object readRouteValues = null, string readActionName = "_Index") where T : class
        {
            grid.Events(clientEvents => clientEvents.DataBound("restoreNoRecordsText"));

            //grid.Sortable(sorting => sorting.Enabled(true))//.ClientEvents(events => events.OnDataBound("onGridDataBound"))    
            //        .Resizable(resize => resize.Columns(true))
            //        .Pageable(paging => paging.Refresh(true).PageSizes(pageSizesRequired))
            //      //  .Filterable(filtering => filtering.Enabled(filterable))
            //        .Groupable(grouping => grouping.Enabled(false));

            if (pageSizesRequired)
            {
                grid.Pageable(paging => paging.PageSizes(new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50 }));
            }

            grid.DataSource(dataSource => dataSource
                    .Ajax().PageSize(pageSize)
                     .Events(events => events.Error("onGridError"))
                    .Model(model => model.Id(primaryKeyName))
                    .Read(read => read.Action(readActionName, ControllerName, readRouteValues).Data(readDataHandler))
                    .Destroy(destroy => destroy.Action("_Delete" + ControllerName, ControllerName))
                );
        }

        public void AddGridActionColumns<T>(GridBuilder<T> grid, IUrlHelper url, string indexName, string rightName, bool addDetailsColumn = true,
         bool addEditColumn = true, bool addDeleteColumn = true, bool gridPrimaryID = false, bool addPrintColumns = false, bool addDocumentColumns = false, bool addCheckbox = true,
         bool enableFontColor = false, string fontColorField = "", string rowAlternateTextField = "",
         string editScriptFunctionName = "editRecord", string deleteScriptFunctionName = "deleteRecord",
         string gridName = "DetailsGrid",
         string detailsActionName = "Details", string editActionName = "Edit", string deleteActionName = "Delete",
         string detailsAdditionalData = null, string editAdditionalData = null, string deleteAdditionalData = null, string editableFieldName = "", bool adddownloadColumns = false, bool approvalCheckbox = false,bool transactionRpt=false) where T : class
        {
            //ApplicationErrorLogTable.SaveException(new Exception(indexName));
            List<UserGridNewColumnTable> result = UserGridNewColumnTable.GetUserColumns(indexName);
            if(result.Count == 0)
            {
                ApplicationErrorLogTable.SaveException(new Exception($"Data missing for MasterGrid index => {indexName}"));
            }

            AddGridActionColumns(grid, url, result, rightName, addDetailsColumn, addEditColumn, addDeleteColumn, gridPrimaryID, addPrintColumns, addDocumentColumns, addCheckbox, enableFontColor,
                fontColorField, rowAlternateTextField, editScriptFunctionName, deleteScriptFunctionName, gridName, detailsActionName, editActionName, deleteActionName,
                detailsAdditionalData, editAdditionalData, deleteAdditionalData, editableFieldName, adddownloadColumns,approvalCheckbox,transactionRpt);
        }

        public void AddGridActionColumns<T>(GridBuilder<T> grid, IUrlHelper url, DataTable table, string rightName, bool addDetailsColumn = true,
           bool addEditColumn = true, bool addDeleteColumn = true, bool gridPrimaryID = false, bool addPrintColumns = false, bool addDocumentColumns = false, bool addCheckbox = true,
           bool enableFontColor = false, string fontColorField = "", string rowAlternateTextField = "",
           string editScriptFunctionName = "editRecord", string deleteScriptFunctionName = "deleteRecord",
           string gridName = "DetailsGrid",
           string detailsActionName = "Details", string editActionName = "Edit", string deleteActionName = "Delete",
           string detailsAdditionalData = null, string editAdditionalData = null, string deleteAdditionalData = null, long? dashboardConfigurationID = null) where T : class
        {
            List<UserGridNewColumnTable> result = new List<UserGridNewColumnTable>();
            int colIndex = 1;

            int totalCols = 0;
            foreach (System.Data.DataColumn col in table.Columns)
            {
                if (col.ColumnName.StartsWith("__")) continue;

                totalCols++;
            }

            int columnWidth = 100 / totalCols;

            foreach (System.Data.DataColumn col in table.Columns)
            {
                if (col.ColumnName.StartsWith("__")) continue;

                string format = "";
                if (col.DataType == typeof(DateTime))
                    format = CultureHelper.DateTimeFormat;

                MasterGridNewLineItemTable li = new MasterGridNewLineItemTable()
                {
                    FieldName = col.ColumnName,
                    DisplayName = col.ColumnName,
                    OrderID = colIndex,
                    IsDefault = true,
                    Width = columnWidth,
                    Format = format
                };

                result.Add(new UserGridNewColumnTable()
                {
                    MasterGridLineItem = li,
                    MasterGridLineItemID = colIndex
                });

                colIndex++;
            }

            AddGridActionColumns(grid, url, result, rightName, false, addEditColumn, addDeleteColumn, gridPrimaryID, addPrintColumns, addDocumentColumns, addCheckbox, enableFontColor,
            fontColorField, rowAlternateTextField, editScriptFunctionName, deleteScriptFunctionName, gridName, detailsActionName, editActionName, deleteActionName,
            detailsAdditionalData, editAdditionalData, deleteAdditionalData);
        }


        public void AddGridActionColumns<T>(GridBuilder<T> grid, IUrlHelper url, List<UserGridNewColumnTable> columnList, string rightName, bool addDetailsColumn = true,
         bool addEditColumn = true, bool addDeleteColumn = true, bool gridPrimaryID = false, bool addPrintColumns = false, bool addDocumentColumns = false, bool addCheckbox = true,
         bool enableFontColor = false, string fontColorField = "", string rowAlternateTextField = "",
         string editScriptFunctionName = "editRecord", string deleteScriptFunctionName = "deleteRecord",
         string gridName = "DetailsGrid",
         string detailsActionName = "Details", string editActionName = "Edit", string deleteActionName = "Delete",
         string detailsAdditionalData = null, string editAdditionalData = null, string deleteAdditionalData = null, string editableFieldName = "", bool adddownloadColumns = false,
         bool approvalCheckbox= false,bool transactionRpt= false) where T : class
        {
            grid.Columns(col =>
            {
                var result = columnList;
                string colorString = "";
                if (enableFontColor)
                    colorString = "color: #=" + fontColorField + "#;";

                //Adjust the width of all columns to fit within 90% of the size
                int maxSize = 90;
                var totW = result.Sum(a => a.MasterGridLineItem.Width);
                if (result.Count > 0)
                {
                    if (totW < maxSize)
                    {
                        int adjustW = (maxSize - totW) / result.Count;
                        if (adjustW < 0)
                            adjustW = 1;

                        foreach (var d in result)
                        {
                            d.MasterGridLineItem.Width += adjustW;

                            totW = result.Sum(a => a.MasterGridLineItem.Width);
                            if (totW < maxSize) continue;
                        }

                        totW = result.Sum(a => a.MasterGridLineItem.Width);
                        if (totW < maxSize)
                        {
                            result.FirstOrDefault().MasterGridLineItem.Width += (maxSize - totW);
                        }
                    }
                    else
                    {
                        if (totW > maxSize)
                        {
                            int adjustW = (totW - maxSize) / result.Count;
                            if (adjustW < 0)
                                adjustW = 1;

                            foreach (var d in result)
                            {
                                d.MasterGridLineItem.Width -= adjustW;

                                totW = result.Sum(a => a.MasterGridLineItem.Width);
                                if (totW > maxSize) continue;
                            }
                        }
                    }
                }

                if (approvalCheckbox)
                {
                    col.Bound(primaryKeyName).Width(20).Filterable(false)
                    .ClientTemplate(
                        "#if (enableUpdate=='0') {#" +
                        "<input type='checkbox' name='" + primaryKeyName + "' id='" + primaryKeyName + "' value='#=" + primaryKeyName + " #' onclick='enableMasterGridRow(this)' class='masterGridClass' />" +
                        "#} else {#" +
                         " #}#"
                            )
                        .ClientHeaderTemplate("<input id='selectAllMasterGridID' name='selectAllMasterGridID' type='checkbox' onclick='selectAllMasterGridClicked(this)'  />")
                        .HeaderHtmlAttributes(new { @style = "font-weight:bold; text-align: lieft" })
                        .Sortable(false).Filterable(false);
                }
                if (addCheckbox)
                {
                    col.Bound(primaryKeyName).Width(20).Filterable(false)
                    .ClientTemplate("<input type='checkbox' name='" + primaryKeyName + "' id='" + primaryKeyName + "' value='#=" + primaryKeyName + " #' onclick='enableMasterGridRow(this)' class='masterGridClass' />")
                        .ClientHeaderTemplate("<input id='selectAllMasterGridID' name='selectAllMasterGridID' type='checkbox' onclick='selectAllMasterGridClicked(this)'  />")
                        .HeaderHtmlAttributes(new { @style = "font-weight:bold; text-align: lieft" })
                        .Sortable(false).Filterable(false);
                }
                foreach (var d in result)
                {
                    string displayFieldName = d.MasterGridLineItem.FieldName.Replace('.', '_');

                    var typeStr = "System.String";
                    if (!string.IsNullOrEmpty(d.MasterGridLineItem.ColumnType))
                        typeStr = d.MasterGridLineItem.ColumnType;

                    var type = Type.GetType(typeStr, false);
                    if (type == null)
                        type = typeof(string);
                    bool fieldIsEditable = false;

                    if (string.IsNullOrEmpty(d.MasterGridLineItem.Format))
                    {
                        //string[] referenceTables = d.MasterGridNewLineItemTable.FieldName.ToString().Split('.');

                        string altText = displayFieldName;
                        if (!string.IsNullOrEmpty(rowAlternateTextField))
                            altText = rowAlternateTextField;

                        if (string.Compare(editableFieldName, d.MasterGridLineItem.FieldName, true) == 0)
                        {
                            col.Bound(d.MasterGridLineItem.FieldName).Title(Language.GetString(d.MasterGridLineItem.DisplayName))
                                .Width(d.MasterGridLineItem.Width)
                                .HeaderHtmlAttributes(new { @style = "font-weight:bold; width: " + d.MasterGridLineItem.Width + "%;" }).Encoded(false)
                                .ClientTemplate("#if(CustomerContractNo != null && CustomerContractNo != '' && IsAlgeriaContract == 1){# <input type='text' value='#=" + editableFieldName + "#' name='" + editableFieldName + "_#=" + primaryKeyName + "#' style='color: black;' /> #} else {# N/A #} #");
                        }
                        else
                        {
                            if (string.Compare("DocumentName", d.MasterGridLineItem.FieldName, true) == 0)
                            {
                                col.Bound(d.MasterGridLineItem.FieldName).Title(Language.GetString(d.MasterGridLineItem.DisplayName))
                               .Width(d.MasterGridLineItem.Width)
                               .HeaderHtmlAttributes(new { @style = "font-weight:bold; width: " + d.MasterGridLineItem.Width + "%;" }).Encoded(false)
                                  .ClientTemplate("#= tagsTemplate(DocumentName,ApprovalHistoryID) #");
                            }
                            else
                            {
                                col.Bound(d.MasterGridLineItem.FieldName).Title(Language.GetString(d.MasterGridLineItem.DisplayName))
                                .Width(d.MasterGridLineItem.Width)
                                .HeaderHtmlAttributes(new { @style = "font-weight:bold; width: " + d.MasterGridLineItem.Width + "%;" }).Encoded(false)
                                   .ClientTemplate("<div style='" + colorString + "' class='text-overflow-dynamic-container'><span  style='width: " + d.MasterGridLineItem.Width + "px;' class='text-overflow-dynamic-ellipsis' title='#=" +
                                    displayFieldName + " #'>#if (" + displayFieldName + " != null) { ##:getTheSubstring(" + displayFieldName + ",40)##} else {# &nbsp; #}#</div>");
                            }
                            //.ClientTemplate("<div style='" + colorString + "' class = 'tableContent' title='#=" +
                            //altText + " #'>#if (" + displayFieldName + " != null) { ##=" +
                            //displayFieldName + "##} else {# &nbsp; #}#</div>");
                        }
                    }
                    else
                    {
                        col.Bound(d.MasterGridLineItem.FieldName).Title(Language.GetString(d.MasterGridLineItem.DisplayName))
                            .Width(d.MasterGridLineItem.Width).Encoded(false)
                           .HeaderHtmlAttributes(new { @style = "font-weight:bold; width: " + d.MasterGridLineItem.Width + "%;" })
                           .Format("{0:" + d.MasterGridLineItem.Format + "}");
                    }


                    grid.DataSource(dataSource => dataSource.Ajax().Model(m =>
                    {
                        //Attach the types to all columns
                        m.Field(d.MasterGridLineItem.FieldName, type).Editable(fieldIsEditable);
                    }));

                    if (type == typeof(string))
                    {
                        grid.Search(s =>
                        {
                            s.Field(displayFieldName, "contains");
                        });
                    }
                    if (type == typeof(int))
                    {
                        grid.Search(s =>
                        {
                            s.Field(displayFieldName, "eq");
                        });
                    }

                }


                if ((addDetailsColumn) && (SessionUser.HasRights(rightName, UserRightValue.Details)))
                {
                    string toolTip = Language.GetToolTipText("Details");
                    col.Template("<a class='' style='min-width: 25px;' title='" + toolTip + "' href='javascript:editRecord(\"" + ControllerName +
                                                "\", \"" + gridName + "\", #=" + primaryKeyName + " #, \"" +
                               url.Action(detailsActionName)
                               + "\", \"" + detailsAdditionalData + "\")'>"
                               + "<img src='/css/images/view.png' alt ='icon' style='Width:20px;height:20px'/></a>").Title(Language.GetString("View")).
                               HeaderHtmlAttributes(new { @style = "max-width: 20px;" })
                             .Width(20).Exportable(false);
                }

                if ((addEditColumn) && (SessionUser.HasRights(rightName, UserRightValue.Edit)))
                {
                    //if(rightName.EndsWith("Approval"))
                    //{
                    //    col.Template("<a class='' style='min-width: 25px;' title='" + Language.GetToolTipText("EditItemButton") + "' href='javascript:"
                    //        + editScriptFunctionName + "(\"" + ControllerName +
                    //                        "\", \"" + gridName + "\", #=" + primaryKeyName + " #, \"" +
                    //       url.Action(editActionName)
                    //        + "\", \"" + editAdditionalData + "\")'>"
                    //        // + "\", #=enableUpdate#)'>"
                    //       + "<img src='/css/images/Edit-icon.png' alt ='icon' style='Width:15px;height:18px'/></a>").Title(Language.GetString("Edit"))
                    //       .HeaderHtmlAttributes(new { @style = "max-width: 20px;" }).HtmlAttributes(new { @style = "text-align: center" })
                    //    .Width(20).Exportable(false);
                    //}
                    //else
                    //{
                        col.Template("<a class='' style='min-width: 25px;' title='" + Language.GetToolTipText("EditItemButton") + "' href='javascript:"
                                + editScriptFunctionName + "(\"" + ControllerName +
                                                "\", \"" + gridName + "\", #=" + primaryKeyName + " #, \"" +
                               url.Action(editActionName)
                               + "\", \"" + editAdditionalData + "\")'>"
                               + "<img src='/css/images/Edit-icon.png' alt ='icon' style='Width:15px;height:18px'/></a>").Title(Language.GetString("Edit"))
                               .HeaderHtmlAttributes(new { @style = "max-width: 20px;" }).HtmlAttributes(new { @style = "text-align: center" })
                            .Width(20).Exportable(false);
                    //}
                    

                }

                if ((addDeleteColumn) && (SessionUser.HasRights(rightName, UserRightValue.Delete)))
                {
                    col.Template("<a class='' style='min-width: 25px;' title='" + Language.GetToolTipText("EditItemButton") + "' href='javascript:" +
                                deleteScriptFunctionName + "(\"" + ControllerName +
                                              "\", \"" + gridName + "\", \"#=" + primaryKeyName + " #\", \"" +
                             url.Action(deleteActionName)
                             + "\", \"" + deleteAdditionalData + "\")'>"
                             + "<img src='/css/images/delete-icon.png' alt ='icon' style='Width:15px;height:20px'/></a>").Title(Language.GetString("Del")).HeaderHtmlAttributes(new { @style = "max-width: 20px;" })
                          .Width(20).Exportable(false);
                }

                if (addPrintColumns)
                {
                    col.Template("<a class='' style='min-width: 25px;' title='" + Language.GetToolTipText("PrintItemButton") + "' href='javascript:printRecord(\"" + ControllerName +
                                                "\", \"" + gridName + "\", #=" + primaryKeyName + " #, \"" +
                                                url.Action("Print")
                                                + "\")'><span  title='" + Language.GetToolTipText("Print")
                                                + "'><i class='glyphicon glyphicon-print'></i></span></a>").Title(Language.GetString("Print")).HeaderHtmlAttributes(new { @style = "max-width: 20px;" })
                            .Width(20).Exportable(false);
                }
                if(transactionRpt)
                {
                    string toolTip = Language.GetToolTipText("Report");
                    col.Template("<a class='' style='min-width: 25px;' title='" + toolTip + "'  href='javascript:ReportFunction(\"#=" + primaryKeyName + " #\")'>"
                               + "<img src='/css/images/Report.png' alt ='icon' style='Width:20px;height:20px'/></a>").Title(Language.GetString("Report")).
                               HeaderHtmlAttributes(new { @style = "max-width: 20px;" })
                             .Width(20).Exportable(false);
                }
               
                if ((adddownloadColumns))
                {
                    string toolTip = Language.GetToolTipText("Download");
                    col.Template("<a class='' style='min-width: 25px;' title='" + toolTip + "'  href='javascript:DownloadFunction(\"#=" + primaryKeyName + " #\")'>"
                               + "<img src='/css/images/download.png' alt ='icon' style='Width:20px;height:20px'/></a>").Title(Language.GetString("Download")).
                               HeaderHtmlAttributes(new { @style = "max-width: 20px;" })
                             .Width(20).Exportable(false);


                }
                if (addDocumentColumns)
                {
                    col.Template("<a class='' style='min-width: 25px;' title='" + Language.GetToolTipText("DocumentItemButton") + "' href='javascript:"
                                + editScriptFunctionName + "(\"" + ControllerName +
                                                "\", \"" + gridName + "\", #=" + primaryKeyName + " #, \"" +
                               url.Action("Document")
                               + "\", \"" + editAdditionalData + "\")'><span  title='" + Language.GetToolTipText("Edit")
                               + "'><i class='glyphicon glyphicon-open'></i></span></a>").Title(Language.GetString("Document")).HeaderHtmlAttributes(new { @style = "" })
                            .Width(20).Exportable(false);
                }


            });
        }


        public void AddGridHeaders<T>(GridBuilder<T> grid, IUrlHelper Url, IHtmlHelper<IndexPageModel> htmlHelper, string rightName, bool addBinGenerateBarcode = false, bool exportToCSVButtonRequired = true,
           bool addNewButtonRequired = true, string addRequestString = "",
           bool exportToPDFButtonRequired = true, bool exportToExcelButtonRequired = true, bool ChangeColumnRequired = true, bool addDeleteButtonRequired = true,
           string addRecordText = "") where T : class
        {
            List<GridButton> buttons = new List<GridButton>();
            //if (addNewButtonRequired)
            //    buttons.Add(GridButton.CreateNewRecordButton(ControllerName, caption: addRecordText, addRequestString: addRequestString));

            AddGridHeaders(grid, Url, htmlHelper, rightName, buttons, addBinGenerateBarcode, exportToCSVButtonRequired, addNewButtonRequired, exportToPDFButtonRequired,
                exportToExcelButtonRequired, ChangeColumnRequired, addDeleteButtonRequired);
        }

        public void AddGridHeaders<T>(GridBuilder<T> grid, IUrlHelper Url, IHtmlHelper<IndexPageModel> htmlHelper, string rightName, List<GridButton> buttons, bool addBinGenerateBarcode = false,
        bool exportToCSVButtonRequired = true, bool addNewButtonRequired = true, bool exportToPDFButtonRequired = true,
        bool exportToExcelButtonRequired = true, bool ChangeColumnRequired = true, bool addDeleteButtonRequired = true) where T : class
        {
            var controllerName = this.ControllerName;

            var defaultToolBar = htmlHelper.Kendo().ToolBar()
                .Name("ToolBar")
                .HtmlAttributes(new { style = "width: 100%;" })
                .Items(items =>
                {

                    if (SessionUser.HasRights(rightName, UserRightValue.Create))
                    {
                        if (addNewButtonRequired)
                        {
                            items.Add().Type(Kendo.Mvc.UI.CommandType.Button).Text(Language.GetButtonText("AddNewRecord")).
                            //Click($"gridButtonClicked").
                            Id(Guid.NewGuid().ToString())
                            .Template("<img src='/css/images/AddNewRecordIcon.png' alt ='icon' style='Width:30px;height:30px'/>")
                            .HtmlAttributes(new { onclick = "gridButtonClicked(\"" + controllerName + "\",\"Create\")" });//.HtmlAttributes(new { @class = "k-button-solid-primary" });
                        }
                    }


                    if (SessionUser.HasRights(rightName, UserRightValue.Delete))//addNewButtonRequired)
                    {
                        if (addDeleteButtonRequired)
                        {
                            items.Add().Type(Kendo.Mvc.UI.CommandType.Button).Text(Language.GetButtonText("Delete")).Id(Guid.NewGuid().ToString())
                              // .Template("<img src='/css/images/delete-icon.png' alt ='icon' style='Width:20px;height:25px'/>")
                              .Template("<a class='' style='min-width: 25px;' title='" + Language.GetToolTipText("DeleteItemButton") + "' href='javascript:DeleteMultipleGridRecord"
                              + "(\"" + ControllerName +
                                                 "\", \"" + "DetailsGrid" + "\", \"" + "" + "\", \"" +
                                Url.Action("DeleteAll")
                                + "\")'>"
                                + "<img src='/css/images/delete-icon.png' alt ='icon' style='Width:20px;height:25px'/></a>")
                              .HtmlAttributes(new { @controllerName = controllerName, @actionName = "Delete" });//.HtmlAttributes(new { @class = "k-button-solid-primary" });
                        }
                    }
                    items.Add().Type(Kendo.Mvc.UI.CommandType.Spacer);
                    items.Add().Template("<span class=\"k-searchbox k-input k-input-md k-rounded-md k-input-solid k-grid-search\" style='width: 300px;'><span class=\"k-icon k-i-search k-input-icon\"></span><input autocomplete=\"off\" placeholder=\"Search...\" title=\"Search...\" aria-label=\"Search...\" class=\"k-input-inner\"></span>");
                  //  items.Add().Type(Kendo.Mvc.UI.CommandType.Spacer);
                    string exportMethod = "";

                    if (exportToCSVButtonRequired)
                    {
                        if (SessionUser.HasRights(rightName, UserRightValue.ExportToCSV))
                        {
                            items.Add().Type(Kendo.Mvc.UI.CommandType.SplitButton).Icon("Download").Text(Language.GetButtonText("Export"))

                            .MenuButtons(menuButtons =>
                            {
                                if (exportToPDFButtonRequired)
                                    menuButtons.Add().SpriteCssClass("k-grid-pdf")
                                                    .Text(Language.GetButtonText("PDF")).Icon("pdf")
                                                    .Url($"javascript:exportToPDF{exportMethod}('" + ControllerName + "')");

                                menuButtons.Add().SpriteCssClass("k-grid-excel")
                                            .Text(Language.GetButtonText("Excel")).Icon("excel")
                                            .Url($"javascript:exportToExcel{exportMethod}('" + ControllerName + "')");
                            });

                        }
                    }
                    if (ChangeColumnRequired)
                    {
                        items.Add().Type(Kendo.Mvc.UI.CommandType.Button).Text(Language.GetButtonText("ChangeColumn"))
                         .Template("<img src='/css/images/ChangeColumnIcon.png' alt ='icon' style='Width:20px;height:20px'/>")
                                .Id(Guid.NewGuid().ToString())
                                .HtmlAttributes(new { onclick = "StaticChangeColumnGrid('" + ColumnIndexName + "')" });
                    }
                });

            grid.ToolBar(toolbar =>
            {
                toolbar.ClientTemplate(defaultToolBar.ToClientTemplate().ToString());
            })

            //grid.ToolBar(toolbar =>
            //{
            //    if (addNewButtonRequired)
            //    {
            //        toolbar.Custom().Text("Add New Records").HtmlAttributes(new { onclick = "loadContentPage('" + "/" + ControllerName + "/Create" + "')" });
            //    }

            //    if ((exportToCSVButtonRequired) && (exportToExcelButtonRequired) && (exportToPDFButtonRequired))
            //    {
            //        toolbar.Excel();
            //        toolbar.Pdf();
            //    }
            //    if ((ChangeColumnRequired))
            //    {
            //        toolbar.Custom().Text(Language.GetButtonText("ChangeColumn")).HtmlAttributes(new { onclick = "StaticChangeColumnGrid('" + ColumnIndexName + "')" });
            //    }
            //})

                .Excel(excel => excel.AllPages().FileName(ControllerName + ".xlsx").Filterable(true).ProxyURL(Url.Action("Export", ControllerName)))
            .Pdf(pdf => pdf.AllPages().AvoidLinks().PaperSize("A4").Margin("1cm", "1cm", "1cm", "1cm").Landscape()
                            .RepeatHeaders().FileName(ControllerName + ".pdf").ProxyURL(Url.Action("Export", ControllerName)))
            .Events(e => e.ExcelExport("gridExportToExcelHandler"))
            ;


        }

        public void AddPopUpWindow(IHtmlHelper htmlHelper, string ControllerName)
        {
            dynamic response = view;
            var ajaxOption = GetAjaxOptions();

            response.WriteAsync("<div id='custom-menu'>");
            response.WriteAsync("<ul style='list-style-type: none;padding: 10px 10px 1px 5px;'>");
            response.WriteAsync(string.Format("<li><a href='javascript:openChildWindowItem(\"{0}\",\"{1}\",\"{2}\",\"{3}\")'>ChangeColumn</a> </li>",
                "#upload" + ControllerName + "Window",
                        Language.GetString("CustomizeGridColumn"), ControllerName, "/DynamicGrid/Index/"));
            response.WriteAsync("</ul>");
            response.WriteAsync("</div>");
            response.WriteAsync("<div id='upload" + ControllerName + "Window'>");
        }

        #region Grid Column related activities

        public static List<TextValuePair<string, string>> GetObjectProperties(string typeName)
        {
            List<TextValuePair<string, string>> list = new List<TextValuePair<string, string>>();

            Type type = DisplayHelper.FindType(typeName);
            GetObjectProperties(list, "", type, 0);

            return list;
        }

        private static void GetObjectProperties(List<TextValuePair<string, string>> list, string propertyAccessName, Type objectType, int level = 0)
        {
            var prop = objectType.GetProperties();

            //for some of the objects only add required columns
            if (objectType == typeof(StatusTable))
            {
                list.Add(new TextValuePair<string, string>() { Text = propertyAccessName + "StatusName", Value = propertyAccessName + "StatusName" });
                return;
            }
            if (objectType == typeof(User_LoginUserTable))
            {
                list.Add(new TextValuePair<string, string>() { Text = propertyAccessName + "UserName", Value = propertyAccessName + "UserName" });
                return;
            }

            foreach (var p in prop)
            {
                if (p.Name == "CurrentPageID") continue;

                if ((p.PropertyType != typeof(string)) && (p.PropertyType.IsClass))
                {
                    if (level < 2)
                        GetObjectProperties(list, p.Name + ".", p.PropertyType, level++);
                }
                else
                {
                    list.Add(new TextValuePair<string, string>() { Text = propertyAccessName + p.Name, Value = propertyAccessName + p.Name });
                }
            }
        }

        public static Type FindType(string qualifiedTypeName)
        {
            Type t = Type.GetType(qualifiedTypeName);

            if (t != null)
            {
                return t;
            }
            else
            {
                foreach (System.Reflection.Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    t = asm.GetType(qualifiedTypeName);
                    if (t != null)
                        return t;
                }
                return null;
            }
        }

        #endregion

        #region Multi Column Combobox

        public static void AddMultiColumnComboBoxColumns(MultiColumnComboBoxColumnFactory factory, string indexName)
        {
            List<UserGridNewColumnTable> result = UserGridNewColumnTable.GetUserColumns(indexName);
            foreach (var itm in result)
            {
                factory.Add().Field(itm.MasterGridLineItem.FieldName).Title(itm.MasterGridLineItem.DisplayName).Width(itm.MasterGridLineItem.Width + "px").ToString();
            }
        }

        public static MultiColumnComboBoxBuilder CreateMultiColumnComboBox(IHtmlHelper<ScreenControlViewModel> Html, ScreenControlViewModel m)
        {
            var ctrl = Html.Kendo()
                .MultiColumnComboBox()
                .Name(m.ControlName)
                .Value(m.Value + "")
                //.Text(m.Text)
                .Placeholder(m.PlaceholderText)
                .DataTextField(m.SelectedItemDisplayField)
                .DataValueField(m.ValueFieldName)
                .SyncValueAndText(false)
                .AutoBind(!string.IsNullOrEmpty(m.Value + "") ? true : false)
                .Filter(FilterType.Contains)
                .HtmlAttributes(new { style = "width: 100%;" })
                .DataSource(source =>
                {
                    source.Read(read =>
                    {
                        read.Action(m.DataReadActionName, m.DataReadControllerName, m.RouteValues).Data(m.DataReadScriptFunctionName);
                    })
                    .ServerFiltering(true);
                });

            //if(m.IsMandatory)
            //    ctrl.HtmlAttributes(GetMandatoryFieldAttributes(m.ControlName));

            ctrl.Columns(columns =>
            {
                foreach (string col in m.DisplayFields)
                {
                    columns.Add().Field(col).Title(col);
                }
            });

            if (!string.IsNullOrWhiteSpace(m.ChangeScriptFunctionName))
                ctrl.Events(e => e.Change(m.ChangeScriptFunctionName));

            if (!string.IsNullOrWhiteSpace(m.SelectScriptFunctionName))
                ctrl.Events(e => e.Select(m.SelectScriptFunctionName));

            return ctrl;
        }

        //public static MultiColumnComboBoxBuilder CreateMultiColumnComboBox<T>(IHtmlHelper<T> Html, ScreenControlViewModel m)
        //{
        //    var ctrl = Html.Kendo()
        //        .MultiColumnComboBox()
        //        .Name(m.ControlName)
        //        .Value(m.Value + "")
        //        .Text(m.Text)
        //        .Placeholder(m.PlaceholderText)
        //        .DataTextField(m.SelectedItemDisplayField)
        //        .DataValueField(m.ValueFieldName)
        //        .SyncValueAndText(false)
        //        .AutoBind(!string.IsNullOrEmpty(m.Value + "") ? true : false)
        //        .Filter(FilterType.Contains)
        //        .HtmlAttributes(new { style = "width: 100%;" })
        //        .DataSource(source =>
        //        {
        //            source.Read(read =>
        //            {
        //                read.Action(m.DataReadActionName, m.DataReadControllerName, m.RouteValues).Data(m.DataReadScriptFunctionName);
        //            })
        //            .ServerFiltering(true);
        //        });

        //    //if(m.IsMandatory)
        //    //    ctrl.HtmlAttributes(GetMandatoryFieldAttributes(m.ControlName));

        //    ctrl.Columns(columns =>
        //    {
        //        foreach (string col in m.DisplayFields)
        //        {
        //            columns.Add().Field(col).Title(col);
        //        }
        //    });

        //    if (!string.IsNullOrWhiteSpace(m.ChangeScriptFunctionName))
        //        ctrl.Events(e => e.Change(m.ChangeScriptFunctionName));

        //    if (!string.IsNullOrWhiteSpace(m.SelectScriptFunctionName))
        //        ctrl.Events(e => e.Select(m.SelectScriptFunctionName));

        //    return ctrl;
        //}

        #endregion

        #region Menu Items

        private void AddMenuItems(MenuItemFactory menu, List<Hi5UserMenu> items, int parentIndex)
        {
            foreach (var menuItem in items)
            {
                string targetObject = menuItem.TargetObject;
                if (string.Compare(targetObject, "HandlerNull()", true) == 0)
                    targetObject = "";

                //avoid the empty parent items
                if ((string.IsNullOrEmpty(targetObject))
                    && (menuItem.ChildItems.Count == 0))
                    continue;

                var newItem = menu.Add();
                newItem.Text(Language.GetString(menuItem.MenuName));
                //newItem.SpriteCssClasses(menuItem.Image);

                if (!string.IsNullOrEmpty(menuItem.Image))//if (parentIndex == 0) 
                    newItem.ImageUrl("~/" + menuItem.Image);

                string className = "";
                if (parentIndex == 0) className = "childMenuItem";

                if (string.IsNullOrEmpty(menuItem.TargetObject))
                    newItem.HtmlAttributes(new { @Title = Language.GetString(menuItem.MenuName), @id = className });
                else
                    newItem.HtmlAttributes(new
                    {
                        @NavigateURL = menuItem.TargetObject,
                        @ParentIndex = parentIndex,
                        @Title = menuItem.MenuName,
                        @id = className
                    });

                if (menuItem.ChildItems.Count > 0)
                {
                    newItem.Items(item =>
                    {
                        AddMenuItems(item, menuItem.ChildItems, parentIndex + 1);
                    });
                }

                if (string.IsNullOrEmpty(targetObject))
                {

                }
            }
        }

        public void AddMenuItems(MenuItemFactory menu)
        {
            //dont load the menu items if change password is required
            //if (SessionUser.Current.ForceChangePassword)
            //    return;

            //var menuItems = SessionUser.Current.GetUserMenuItems();
            var menuItems = ACS.AMS.DAL.DBModel.User_MenuTable.GetUserMenus(AMSContext.CreateNewContext(), SessionUser.Current.Username);
            AddMenuItems(menu, menuItems, 0);
        }

        #endregion Menu Items

        #region Treeview Items

        public void AddTreeViewItems(TreeViewItemFactory menu, string masters = "", int? model = null, bool inReportView = false)
        {
            try
            {
                int parentIndex = 0;
                var menuItems = CategoryModel.GetCategoryItems(masters);

                var locationItem = menu.Add();

                locationItem.Id("0");
                locationItem.Text("Categories");
                locationItem.SpriteCssClasses("");
                locationItem.Expanded(true);

                locationItem.Items(item =>
                {
                    AddTreeViewItems(item, menuItems, 0, true, model);
                });

                parentIndex++;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogTable.SaveException(ex);
            }
        }

        private void AddTreeViewItems(TreeViewItemFactory menu, IList<CategoryModel> items, int parentIndex, bool increaseParentIndex = false, int? model = null)
        {
            foreach (var menuItem in items.ToList()) //.OrderBy(a => a.Name).
            {
                if (string.IsNullOrEmpty(menuItem.Name))
                    continue;
                var newItem = menu.Add();

                newItem.Id(menuItem.id + "");

                newItem.Text(menuItem.Name);
                newItem.SpriteCssClasses(menuItem.spriteCssClass);
                var desc = menuItem.Description;
                if (!model.HasValue)
                {
                    if (menuItem.ChildItems.Count > 0)
                    {
                        IList<CategoryModel> childs = menuItem.ChildItems.Select(t => new CategoryModel
                        {
                            id = desc.Where(a => a.CategoryID == t.CategoryID).Select(a => a.CategoryID).FirstOrDefault(),//t.LocationID,
                            Name = desc.Where(a => a.CategoryID == t.CategoryID).Select(a => a.CategoryName).FirstOrDefault(),
                            hasChildren = t.InverseParentCategory.Any(x => x.StatusID != (int)StatusValue.Deleted && x.StatusID != (int)StatusValue.DeletedOLD),
                            spriteCssClass = t.InverseParentCategory.Count(x => x.StatusID != (int)StatusValue.Deleted && x.StatusID != (int)StatusValue.DeletedOLD) > 0 ? "rootfolder" : "html",
                            ChildItems = t.InverseParentCategory.Where(x => x.StatusID != (int)StatusValue.Deleted && x.StatusID != (int)StatusValue.DeletedOLD).ToList(),//.OrderBy(x => x.CategoryName).ToList()
                            Description = desc
                        }).OrderBy(x => x.Name).ToList();
                        newItem.Items(item =>
                        {
                            AddTreeViewItems(item, childs, parentIndex);
                        });
                    }
                }

                if (increaseParentIndex) parentIndex++;
            }
        }

        private void AddTreeViewLocationItems(TreeViewItemFactory menu, IList<LocationModel> items, int parentIndex, bool increaseParentIndex = false, int? model = null)
        {
            foreach (var menuItem in items) //.OrderBy(a => a.Name)
            {
                var newItem = menu.Add();

                newItem.Id(menuItem.id + "");
                newItem.Text(menuItem.Name);
                newItem.SpriteCssClasses(menuItem.spriteCssClass);

                var locationDesc = menuItem.Description;

                if (!model.HasValue)
                {
                    if (menuItem.ChildItems.Count > 0)
                    {
                        IList<LocationModel> childs = menuItem.ChildItems.Select(t => new LocationModel
                        {
                            id = t.LocationID,//dd.Where(a => a.locationID == t.LocationID).Select(a => a.locationID).FirstOrDefault(),//t.LocationID,
                            Name = locationDesc.Where(a => a.LocationID == t.LocationID).Select(a => a.LocationName).FirstOrDefault(),
                            spriteCssClass = t.InverseParentLocation.Count(x => x.StatusID != (int)StatusValue.Deleted && x.StatusID != (int)StatusValue.DeletedOLD) > 0 ? "rootfolder" : "html",
                            ChildItems = t.InverseParentLocation.Where(x => x.StatusID != (int)StatusValue.Deleted && x.StatusID != (int)StatusValue.DeletedOLD).ToList(),
                            Description = locationDesc
                        }).OrderBy(x => x.Name).ToList();

                        newItem.Items(item =>
                        {
                            AddTreeViewLocationItems(item, childs, parentIndex);
                        });
                    }
                }

                if (increaseParentIndex) parentIndex++;
            }
        }

        public void AddTreeViewLocationItems(TreeViewItemFactory menu, string masters = "", int? model = null)
        {
            int parentIndex = 0;

            var menuItems = LocationModel.GetLocationItems(masters);

            var locationItem = menu.Add();

            locationItem.Id("0");
            locationItem.Text("Locations");
            locationItem.SpriteCssClasses("");
            locationItem.Expanded(true);

            locationItem.Items(item =>
            {
                AddTreeViewLocationItems(item, menuItems, 0, true, model);
            });

            parentIndex++;
        }

        #endregion Treeview Items

        //public  static void RenderDynamicFields(IHtmlHelper htmlHelper, dynamic view, int reportTemplateID, string screenName = "", int Id = 0, List<ACS.AMS.DAL.DynamicReportFieldModel> dynamicfield = null)
        //{
        //    using (var _db = AMSContext.CreateNewContext())
        //    {
        //        var filterLineQry = from b in _db.ScreenFilterLineItemTable.Include("FieldTypeTable").Include("FieldTypeTable.ScreenControlQueryTable")
        //                            where b.ScreenFilter.ParentID == reportTemplateID
        //                                    && b.ScreenFilter.ParentType == "ReportTemplate"
        //                                    && b.StatusID != (byte)StatusValue.Deleted
        //                            orderby b.OrderNo
        //                            select b;


        //        var lstdynamicfieldandValueMapping = new List<ACS.AMS.DAL.DynamicReportFieldModel>();

        //        if (!string.IsNullOrEmpty(screenName) && (Id > 0 || reportTemplateID > 0))
        //        {
        //            switch (screenName.ToUpper())
        //            {


        //                case "REPORT":
        //                    lstdynamicfieldandValueMapping = LoadReportFields(_db, reportTemplateID);
        //                    break;

        //                default:
        //                    break;
        //            }
        //        }

        //        var res = view.Response;

        //        foreach (ScreenFilterLineItemTable itm in filterLineQry)
        //        {
        //            bool isRangeControl = itm.ScreenFilterTypeID == 2;

        //            string ctrlName1 = itm.QueryField;
        //            string ctrlName2 = "";

        //            if (isRangeControl)
        //            {
        //                ctrlName1 = itm.QueryField + "_Start";
        //                ctrlName2 = itm.QueryField + "_End";
        //            }

        //            var fieldValue = string.Empty;
        //            var fieldValue2 = string.Empty;
        //            if (!string.IsNullOrEmpty(ctrlName1))
        //            {
        //                fieldValue = lstdynamicfieldandValueMapping.Where(x => x.FieldName == ctrlName1).Select(x => x.FieldValue).FirstOrDefault();
        //                if (!string.IsNullOrEmpty(fieldValue))
        //                {
        //                    itm.FieldValue = fieldValue;
        //                }
        //            }

        //            if (!string.IsNullOrEmpty(ctrlName2))
        //            {
        //                fieldValue2 = lstdynamicfieldandValueMapping.Where(x => x.FieldName == ctrlName2).Select(x => x.FieldValue).FirstOrDefault();
        //            }

        //            ACS.AMS.WebApp.Models.BaseControlViewModel model = null;
        //            string viewFile = null;
        //            //string valueFieldName = null;

        //            //Load the control from existing view page
        //            if ((itm.FieldType != null) && (!string.IsNullOrEmpty(itm.FieldType.ViewFileLocation)))
        //            {
        //                viewFile = itm.FieldType.ViewFileLocation.ToString() + ".ascx";

        //                if (itm.FieldType.ScreenControlQueryID.HasValue)
        //                {
        //                    model = new ACS.AMS.WebApp.Models.ScreenControlViewModel(itm.FieldType.ScreenControlQuery,
        //                        ctrlName1, itm.FieldType.ValueFieldName)
        //                    {
        //                        //IsMandatory = itm.IsMandatory
        //                        //Value = itm.FieldValue
        //                    };

        //                    viewFile = "/Views/Shared/ReportFields/DynamicFilterControl.ascx";
        //                }
        //                else
        //                {
        //                    model = new ACS.AMS.WebApp.Models.ReportControlViewModel(ctrlName1, null)
        //                    {
        //                        //IsMandatory = itm.IsMandatory,
        //                        ValueFieldName = itm.FieldType.ValueFieldName,
        //                        //Value = itm.FieldValue
        //                    };
        //                }
        //            }

        //            res.Write("<div class='col-12 col-sm-4'>");

        //            res.Write(htmlHelper.FieldLabel(itm.DisplayName, itm.IsMandatory));
        //            if (isRangeControl)
        //            {
        //                IDictionary<string, object> attributes1 = null, attributes2 = null;
        //                if (itm.IsMandatory)
        //                {
        //                    attributes1 = DisplayHelper.GetMandatoryFieldAttributes(ctrlName1);
        //                    attributes2 = DisplayHelper.GetMandatoryFieldAttributes(ctrlName2);
        //                }

        //                res.Write("<table style='padding: 0px; font-size: 13px;'><tr style='padding: 0px;'><td style='width: 10%; padding: 0px; text-align:right; font-size: 14px;'>");
        //                res.Write("From : </td><td style='width: 35%; padding: 0px;'>");
        //                if (string.IsNullOrEmpty(viewFile))
        //                {
        //                    res.Write(htmlHelper.TextBox(ctrlName1, itm.FieldValue, attributes1));
        //                }
        //                else
        //                {
        //                    view.ViewBag.HtmlAttributes = attributes1;

        //                    model.ControlName = ctrlName1;
        //                    htmlHelper.RenderPartial(viewFile, model);
        //                }
        //                res.Write($"<span class='field-validation-error' data-vaAMSg-for='{ctrlName1}' data-vaAMSg-replace='false'> </span>");
        //                res.Write("</td><td style='width: 10%; padding: 0px; text-align:right; font-size: 14px;'>To : </td>");
        //                res.Write("<td style='width: 35%; padding: 0px;'>");
        //                if (string.IsNullOrEmpty(viewFile))
        //                {
        //                    res.Write(htmlHelper.TextBox(ctrlName2, fieldValue2, attributes2));
        //                }
        //                else
        //                {
        //                    view.ViewBag.HtmlAttributes = attributes2;

        //                    model.ControlName = ctrlName2;
        //                    model.Value = fieldValue2;
        //                    htmlHelper.RenderPartial(viewFile, model);
        //                }
        //                res.Write($"<span class='field-validation-error' data-vaAMSg-for='{ctrlName2}' data-vaAMSg-replace='false'> </span>");
        //                res.Write("</td></tr></table>");

        //                if (itm.IsMandatory)
        //                {
        //                    res.Write(string.Format("<script>setRequiredAttr('{0}')</script>", ctrlName1));
        //                    res.Write(string.Format("<script>setRequiredAttr('{0}')</script>", ctrlName2));
        //                }
        //            }
        //            else
        //            {
        //                IDictionary<string, object> attributes = null;
        //                if (itm.IsMandatory)
        //                {
        //                    attributes = DisplayHelper.GetMandatoryFieldAttributes(itm.DisplayName);
        //                }

        //                if (string.IsNullOrEmpty(viewFile))
        //                {
        //                    res.Write(htmlHelper.TextBox(ctrlName1, itm.FieldValue, attributes));
        //                }
        //                else
        //                {
        //                    view.ViewBag.HtmlAttributes = attributes;
        //                    htmlHelper.RenderPartial(viewFile, model);
        //                }

        //                res.Write($"<span class='field-validation-error' data-vaAMSg-for='{ctrlName1}' data-vaAMSg-replace='false'> </span>");

        //                if (itm.IsMandatory)
        //                {
        //                    res.Write(string.Format("<script>setRequiredAttr('{0}')</script>", ctrlName1));
        //                }
        //            }

        //            res.Write("</div>");
        //        }
        //    }
        //}

        //public static List<DynamicReportFieldModel> LoadReportFields(AMSContext _db, int Id)
        //{
        //    var lstparam = (from b in _db.UserReportFilterTable
        //                    join b1 in _db.UserReportFilterLineItemTable on b.UserReportFilterID equals b1.UserReportFilterID
        //                    where b.ReportTemplateID == Id
        //                    && b.StatusID != (byte)StatusValue.Deleted
        //                                      && b1.StatusID != (byte)StatusValue.Deleted
        //                    orderby b1.UserReportFilterLineItemID
        //                    select new DynamicReportFieldModel { FieldValue = b1.SelectedValue, FieldName = b1.SelectedName }).ToList();

        //    return lstparam;

        //}
        //public static IDictionary<string, object> GetMandatoryFieldAttributes(string displayfieldName)
        //{
        //    Dictionary<string, object> attributes = new Dictionary<string, object>();

        //    attributes.Add("required", "required");
        //    attributes.Add("data-val", "true");
        //    attributes.Add("data-val-required", $"The {displayfieldName} field is required.");

        //    return attributes;
        //}
        public static void RenderDynamicFields(IHtmlHelper htmlHelper, RazorPage page, int reportTemplateID, string screenName = "", int Id = 0, List<ACS.AMS.DAL.DynamicReportFieldModel> dynamicfield = null)
        {
            using (var _db = AMSContext.CreateNewContext())
            {
                var filterLineQry = from b in _db.ScreenFilterLineItemTable.Include("FieldType").Include("FieldType.SelectionControlQuery")
                                    where b.ScreenFilter.ParentID == reportTemplateID
                                            && b.ScreenFilter.ParentType == "ReportTemplate"
                                            && b.FieldType.FieldTypeDesc!="DefaultParameter"
                                            && b.StatusID != (int)StatusValue.Deleted && b.StatusID != (int)StatusValue.DeletedOLD

                                    orderby b.OrderNo
                                    select b;


                var lstdynamicfieldandValueMapping = new List<ACS.AMS.DAL.DynamicReportFieldModel>();

                if (!string.IsNullOrEmpty(screenName) && (Id > 0 || reportTemplateID > 0))
                {
                    switch (screenName.ToUpper())
                    {


                        case "REPORT":
                            lstdynamicfieldandValueMapping = LoadReportFields(_db, reportTemplateID);
                            break;

                        default:
                            break;
                    }
                }

                //var res = view.Response;

                foreach (ScreenFilterLineItemTable itm in filterLineQry)
                {
                    bool isRangeControl = itm.ScreenFilterTypeID == 2;

                    string ctrlName1 = itm.QueryField;
                    string ctrlName2 = "";

                    if (isRangeControl)
                    {
                        ctrlName1 = itm.QueryField + "_Start";
                        ctrlName2 = itm.QueryField + "_End";
                    }

                    var fieldValue = string.Empty;
                    var fieldValue2 = string.Empty;
                    if (!string.IsNullOrEmpty(ctrlName1))
                    {
                        fieldValue = lstdynamicfieldandValueMapping.Where(x => x.FieldName == ctrlName1).Select(x => x.FieldValue).FirstOrDefault();
                        if (!string.IsNullOrEmpty(fieldValue))
                        {
                            itm.FieldValue = fieldValue;
                        }
                    }

                    if (!string.IsNullOrEmpty(ctrlName2))
                    {
                        fieldValue2 = lstdynamicfieldandValueMapping.Where(x => x.FieldName == ctrlName2).Select(x => x.FieldValue).FirstOrDefault();
                    }

                    ACS.AMS.WebApp.Models.BaseControlViewModel model = null;
                    string viewFile = null;
                    //string valueFieldName = null;

                    //Load the control from existing view page
                    if ((itm.FieldType != null) && (!string.IsNullOrEmpty(itm.FieldType.ViewFileLocation)))
                    {
                        viewFile = itm.FieldType.ViewFileLocation.ToString() + ".cshtml";

                        if (itm.FieldType.SelectionControlQueryID.HasValue)
                        {
                            model = new ACS.AMS.WebApp.Models.ScreenControlViewModel(itm.FieldType.SelectionControlQuery,
                                ctrlName1, itm.FieldType.ValueFieldName)
                            {
                                //IsMandatory = itm.IsMandatory
                                //Value = itm.FieldValue
                            };

                            viewFile = "/Views/Shared/ReportFields/DynamicFilterControl.cshtml";
                        }
                        else
                        {
                            model = new ACS.AMS.WebApp.Models.ReportControlViewModel(ctrlName1, null)
                            {
                                //IsMandatory = itm.IsMandatory,
                                ValueFieldName = itm.FieldType.ValueFieldName,
                                //Value = itm.FieldValue
                            };
                        }
                    }

                    page.WriteLiteral("<div class='col-12 col-sm-4'>");

                    page.WriteLiteral(htmlHelper.FieldLabel(itm.DisplayName, itm.IsMandatory));
                    if (isRangeControl)
                    {
                        IDictionary<string, object> attributes1 = null, attributes2 = null;
                        if (itm.IsMandatory)
                        {
                            attributes1 = DisplayHelper.GetMandatoryFieldAttributes(ctrlName1);
                            attributes2 = DisplayHelper.GetMandatoryFieldAttributes(ctrlName2);
                        }

                        page.WriteLiteral("<table style='padding: 0px; font-size: 13px;'><tr style='padding: 0px;'><td style='width: 10%; padding: 0px; text-align:right; font-size: 14px;'>");
                        page.WriteLiteral("From : </td><td style='width: 35%; padding: 0px;'>");
                        if (string.IsNullOrEmpty(viewFile))
                        {
                            page.Write(htmlHelper.TextBox(ctrlName1, itm.FieldValue, attributes1));
                        }
                        else
                        {
                            page.ViewBag.HtmlAttributes = attributes1;

                            model.ControlName = ctrlName1;
                            HtmlHelperPartialExtensions.RenderPartial(htmlHelper, viewFile, model);
                        }
                        page.WriteLiteral($"<span class='field-validation-error' data-valmsg-for='{ctrlName1}' data-valmsg-replace='false'> </span>");
                        page.WriteLiteral("</td><td style='width: 10%; padding: 0px; text-align:right; font-size: 14px;'>To : </td>");
                        page.WriteLiteral("<td style='width: 35%; padding: 0px;'>");
                        if (string.IsNullOrEmpty(viewFile))
                        {
                            page.WriteLiteral(htmlHelper.TextBox(ctrlName2, fieldValue2, attributes2));
                        }
                        else
                        {
                            page.ViewBag.HtmlAttributes = attributes2;

                            model.ControlName = ctrlName2;
                            model.Value = fieldValue2;
                            HtmlHelperPartialExtensions.RenderPartial(htmlHelper, viewFile, model);
                        }
                        page.WriteLiteral($"<span class='field-validation-error' data-valmsg-for='{ctrlName2}' data-valmsg-replace='false'> </span>");
                        page.WriteLiteral("</td></tr></table>");

                        if (itm.IsMandatory)
                        {
                            page.WriteLiteral(string.Format("<script>setRequiredAttr('{0}')</script>", ctrlName1));
                            page.WriteLiteral(string.Format("<script>setRequiredAttr('{0}')</script>", ctrlName2));
                        }
                    }
                    else
                    {
                        IDictionary<string, object> attributes = null;
                        if (itm.IsMandatory)
                        {
                            attributes = DisplayHelper.GetMandatoryFieldAttributes(itm.DisplayName);
                        }

                        if (string.IsNullOrEmpty(viewFile))
                        {


                            page.Write(htmlHelper.TextBox(ctrlName1, itm.FieldValue, attributes));
                        }
                        else
                        {
                            page.ViewBag.HtmlAttributes = attributes;
                            HtmlHelperPartialExtensions.RenderPartial(htmlHelper, viewFile, model);
                        }

                        page.WriteLiteral($"<span class='field-validation-error' data-valmsg-for='{ctrlName1}' data-valmsg-replace='false'> </span>");

                        if (itm.IsMandatory)
                        {
                            page.WriteLiteral(string.Format("<script>setRequiredAttr('{0}')</script>", ctrlName1));
                        }
                    }

                    page.WriteLiteral("</div>");
                }
            }
        }
        public static List<DynamicReportFieldModel> LoadReportFields(AMSContext _db, int Id)
        {
            var lstparam = (from b in _db.UserReportFilterTable
                            join b1 in _db.UserReportFilterLineItemTable on b.UserReportFilterID equals b1.UserReportFilterID
                            where b.ReportTemplateID == Id
                            && b.StatusID != (int)StatusValue.Deleted && b.StatusID != (int)StatusValue.DeletedOLD
                                              && b1.StatusID != (int)StatusValue.Deleted
                            orderby b1.UserReportFilterLineItemID
                            select new DynamicReportFieldModel { FieldValue = b1.SelectedValue, FieldName = b1.SelectedName }).ToList();

            return lstparam;

        }
        public static IDictionary<string, object> GetMandatoryFieldAttributes(string displayfieldName)
        {
            Dictionary<string, object> attributes = new Dictionary<string, object>();

            attributes.Add("required", "required");
            attributes.Add("data-val", "true");
            attributes.Add("data-val-required", $"The {displayfieldName} field is required.");

            return attributes;
        }

        public static FileResult BarcodeExportToFile(IQueryable items, string index, string fileFormat, string file, IQueryable headerName, bool displayValue = true)
        {
            switch (fileFormat.ToUpper())
            {
                case "EXCEL":
                    var excelResult = new FileContentResult(BarcodeExportToExcel(items, index, headerName, displayValue).ToArray(), "application/vnd.ms-excel");
                    excelResult.FileDownloadName = file + ".xlsx";
                    return excelResult;
            }

            return null;
        }
        
        private static MemoryStream BarcodeExportToExcel(IQueryable items, string indexName, IQueryable headerName, bool displayValues = true)
        {
            var workbook = new XSSFWorkbook();

            var sheet = workbook.CreateSheet();
            var style = workbook.CreateCellStyle();
            //Text Wrap 
            style.WrapText = true;
            //Set the columns as Text Format 
            style.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
            indexName = ComboBoxHelper.RemoveSpecialCharacters(indexName);
            workbook.SetSheetName(0, indexName);
            for (int i = 0; i < headerName.Count(); i++)
            {
                sheet.SetColumnWidth(i, 10 * 256);
                sheet.SetDefaultColumnStyle(i, style);
            }

            var headerRow = sheet.CreateRow(0);
            int index = 0;

            foreach (var d in headerName)
            {
                sheet.SetColumnWidth(index, 10 * 256);
                headerRow.CreateCell(index).SetCellValue(Language.GetString(d + ""));
                index++;
            }

            if (displayValues)
            {
                int rowNumber = 1;
                if (items.Count() > 0)
                {

                    foreach (var itm in items)
                    {
                        var row = sheet.CreateRow(rowNumber++);
                        int cell = 0;

                        row.CreateCell(cell).SetCellValue(itm + "");


                    }
                }
            }

            MemoryStream output = new MemoryStream();
            workbook.Write(output);
            return output;
        }
    }
}