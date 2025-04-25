using ACS.AMS.DAL;
using ACS.AMS.DAL.DBContext;
using ACS.AMS.DAL.DBModel;
using ACS.WebAppPageGenerator.Models.SystemModels;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Security.Policy;

namespace ACS.AMS.WebApp
{
    public class IndexPageHelper<T>
    {
        public IndexPageHelper(IndexPageModel model) 
        {
            Model = model;
            GridName = "DetailsGrid";
            ControllerName = model.ControllerName;
        }

        public IndexPageModel Model { get; set; }

        public string GridName { get; set; }

        public IUrlHelper Url { get; set; } 

        private string PageName { get => Model.PageName; }

        private string PrimaryKeyFieldName { get => Model.EntityInstance.GetPrimaryKeyFieldName(); }

        public string ControllerName { get; set; }

        public void CreateGridControl(IHtmlHelper<T> htmlHelper)
        {
            //TODO: Load below data from Database
            string _rightName = PageName;
            var gridColumnIndexName = PageName;// "Color"; 

            //based on the DB configuration create the control
            var gridCtrl = htmlHelper.Kendo().Grid<dynamic>().Name(GridName);

            //Configure Grid columns            
            AddGridActionColumns(gridCtrl, Url, gridColumnIndexName, _rightName);

            //Configure Grid data source
            ConfigureGrid(gridCtrl, readDataHandler: Model.ReadDataHandler, readRouteValues: Model.ReadRouteValues, readActionName: Model.ReadActionName);

            //Configure Grid Header
            AddGridHeaders(htmlHelper, gridColumnIndexName, gridCtrl, _rightName);

            gridCtrl.Render();
        }

        private void AddGridHeaders(IHtmlHelper<T> htmlHelper, string gridColumnIndexName, GridBuilder<dynamic> grid, string rightName)
        {
            var controllerName = this.ControllerName;
            string deleteAdditionalData=null;
            deleteAdditionalData = (deleteAdditionalData + "&" ?? string.Empty) + "pageName=" + PageName;
            var defaultToolBar = htmlHelper.Kendo().ToolBar()
                .Name("ToolBar")
                .HtmlAttributes(new { style = "width: 100%;" })
                .Items(items =>
                {
                    //if (true)//pageTitleRequired)
                    //    items.Add().Template("<label class='pageMainHeadingPortion' id='pageMainHeadingPortion'>PageTitle</label>");
                    if (SessionUser.HasRights(rightName, UserRightValue.Create))//addNewButtonRequired)
                    {
                        items.Add().Type(CommandType.Button).Text(Language.GetButtonText("AddNewRecord"))
                        //.Click($"gridButtonClicked")
                        .Id(Guid.NewGuid().ToString())
                          .Template("<img src='/css/images/AddNewRecordIcon.png' alt ='icon' style='Width:30px;height:30px'/>")
                        .HtmlAttributes(new { onclick = "gridButtonClicked(\"" + controllerName + "\",\"Create\",\""+ PageName + "\")" });
                        //.HtmlAttributes(new { @class = "k-button-solid-primary" });
                    }
                    if (SessionUser.HasRights(rightName, UserRightValue.Delete))//addNewButtonRequired)
                    {
                        items.Add().Type(Kendo.Mvc.UI.CommandType.Button).Text(Language.GetButtonText("Delete")).Id(Guid.NewGuid().ToString())
                            //.Template("<img src='/css/images/delete-icon.png' alt ='icon' style='Width:20px;height:25px'/>")
                            .Template("<a class='' style='min-width: 25px;' title='" + Language.GetToolTipText("DeleteItemButton") + "' href='javascript:DeleteMultipleGridRecord"
                          + "(\"" + ControllerName +
                          "\", \"" + "DetailsGrid" + "\", \"" + "" + "\", \"" +
                            Url.Action("DeleteAll")
                            + "\")'>"
                            + "<img src='/css/images/delete-icon.png' alt ='icon' style='Width:20px;height:25px'/></a>")
                              .Template("<a class='' style='min-width: 25px;' title='" + Language.GetToolTipText("DeleteItemButton") + "' href='javascript:DeleteMultipleGridRecord" 
                              + "(\"" + controllerName +
                                              "\", \"" + "DetailsGrid" + "\", \"" + "" + "\", \"" +
                             Url.Action("DeleteAll")
                             + "\", \"" + deleteAdditionalData + "\")'>"
                             + "<img src='/css/images/delete-icon.png' alt ='icon' style='Width:15px;height:20px'/></a>")
                           .HtmlAttributes(new { @controllerName = controllerName, @actionName = "Delete" });//.HtmlAttributes(new { @class = "k-button-solid-primary" });
                    }
                    items.Add().Type(CommandType.Spacer);

                    items.Add().Template("<span class=\"k-searchbox k-input k-input-md k-rounded-md k-input-solid k-grid-search\" style='width: 300px;'><span class=\"k-icon k-i-search k-input-icon\"></span><input autocomplete=\"off\" placeholder=\"Search...\" title=\"Search...\" aria-label=\"Search...\" class=\"k-input-inner\"></span>");

                    //Always allow export buttons
                    //if (exportToPDFButtonRequired || exportToExcelButtonRequired)
                    {
                        string exportMethod = "";

                        //if (false)//AllowCustomExport)
                        //    exportMethod = "Custom";

                        items.Add().Type(CommandType.SplitButton).Icon("Download").Text(Language.GetButtonText("Export"))
                         
                        .MenuButtons(menuButtons =>
                        {
                            //if(exportToPDFButtonRequired)
                            menuButtons.Add().SpriteCssClass("k-grid-pdf")
                                            .Text(Language.GetButtonText("PDF")).Icon("pdf")
                                            .Url($"javascript:exportToPDF{exportMethod}('" + ControllerName + "')");
                            //if(exportToCSVButtonRequired)
                            menuButtons.Add().SpriteCssClass("k-grid-excel")
                                            .Text(Language.GetButtonText("Excel")).Icon("excel")
                                            .Url($"javascript:exportToExcel{exportMethod}('" + ControllerName + "')");
                        });
                    }

                    items.Add().Type(CommandType.Button).Text(Language.GetButtonText("ChangeColumn"))
                     .Template("<img src='/css/images/ChangeColumnIcon.png' alt ='icon' style='Width:20px;height:20px'/>")
                            //.Click($"changeColumnGrid")
                            .Id(Guid.NewGuid().ToString())
                            .HtmlAttributes(new { onclick = "changeColumnGrid(\""+ ControllerName + "\",\"ChangeColumn\",\""+ PageName + "\")" });
                });

            grid.ToolBar(toolbar =>
            {
                toolbar.ClientTemplate(defaultToolBar.ToClientTemplate().ToString());
            })
            .Excel(excel => excel.FileName(PageName + ".xlsx").AllPages(true).Filterable(true))
            .Pdf(pdf => pdf.AllPages().AvoidLinks().PaperSize("A4").Margin("1cm", "1cm", "1cm", "1cm").Landscape()
                            .RepeatHeaders().FileName(PageName + ".pdf").ProxyURL(Url.Action("Export", ControllerName)))
            .Events(e => e.ExcelExport("gridExportToExcelHandler"))
            ;
        }

        public void ConfigureGrid(GridBuilder<dynamic> grid, int pageSize = 10, bool pageSizesRequired = true,
                               string readDataHandler = "defaultGridReadMethod",
                               object readRouteValues = null, string readActionName = "_Index")
        {
            var controllerName = this.ControllerName;
            var primaryKeyName = PrimaryKeyFieldName;

            if(readRouteValues == null)
                readRouteValues = new { pageName = PageName, currentPageID = Model.EntityInstance.CurrentPageID };

            grid.Events(clientEvents => clientEvents.DataBound("restoreNoRecordsText"));

            grid.Sortable(sorting => sorting.Enabled(true))//.ClientEvents(events => events.OnDataBound("onGridDataBound"))    
                    .Resizable(resize => resize.Columns(true))
                    .Pageable(paging => paging.Refresh(true).PageSizes(pageSizesRequired))
                    .Filterable(filtering => filtering.Enabled(true))
                    .Groupable(grouping => grouping.Enabled(false));

            if (pageSizesRequired)
            {
                grid.Pageable(paging => paging.PageSizes(new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50 }));
            }

            grid.DataSource(dataSource => dataSource
                    .Ajax().PageSize(pageSize)
                    .Events(events => events.Error("onGridError"))
                    .Model(model =>
                    {
                        model.Field("CreatedDateTime", typeof(DateTime));
                        model.Id(primaryKeyName);
                    })
                    .Read(read => read.Action(readActionName, controllerName, readRouteValues).Data(readDataHandler))
                    .Destroy(destroy => destroy.Action("_Delete", controllerName))
                );
        }

        public void AddGridActionColumns<T>(GridBuilder<T> grid, IUrlHelper url, string indexName, string rightName, 
         string editScriptFunctionName = "editRecord", string deleteScriptFunctionName = "deleteRecord",
         string gridName = "DetailsGrid",
         string detailsActionName = "Details", string editActionName = "Edit", string deleteActionName = "Delete",
         string detailsAdditionalData = null, string editAdditionalData = null, string deleteAdditionalData = null, string editableFieldName = "") where T : class
        {
            //ApplicationErrorLogTable.SaveException(new Exception(indexName));
            List<UserGridNewColumnTable> result = UserGridNewColumnTable.GetUserColumns(indexName);

            AddGridActionColumns(grid, url, result, rightName, editScriptFunctionName, deleteScriptFunctionName, gridName, detailsActionName, editActionName, 
                deleteActionName, detailsAdditionalData, editAdditionalData, deleteAdditionalData, editableFieldName);
        }

        public void AddGridActionColumns<T>(GridBuilder<T> grid, IUrlHelper url, List<UserGridNewColumnTable> columnList, string rightName, 
         string editScriptFunctionName = "editRecord", string deleteScriptFunctionName = "deleteRecord",
         string gridName = "DetailsGrid",
         string detailsActionName = "Details", string editActionName = "Edit", string deleteActionName = "Delete",
         string detailsAdditionalData = null, string editAdditionalData = null, string deleteAdditionalData = null, 
         string editableFieldName = "") where T : class
        {
            var controllerName = this.ControllerName;
            var primaryKeyName = PrimaryKeyFieldName;

            editAdditionalData = (editAdditionalData + "&" ?? string.Empty) + "pageName=" + PageName;
            detailsAdditionalData = (detailsAdditionalData + "&" ?? string.Empty) + "pageName=" + PageName;
            deleteAdditionalData = (deleteAdditionalData + "&" ?? string.Empty) + "pageName=" + PageName;

            grid.Columns(col =>
            {
                var result = columnList;
                string colorString = "";
                //if (enableFontColor)
                //    colorString = "color: #=" + fontColorField + "#;";

                //Adjust the width of all columns to fit within 90% of the size
                int maxSize = 90;
                var totW = result.Sum(a => a.MasterGridLineItem.Width);
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

                {
                    col.Bound(primaryKeyName).Width(20).Filterable(false)
                    .ClientTemplate("<input type='checkbox' name='" + primaryKeyName + "' id='" + primaryKeyName + "' value='#=" + primaryKeyName + " #' onclick='enableMasterGridRow(this)' class='masterGridClass' />")
                        .ClientHeaderTemplate("<input id='selectAllMasterGridID' name='selectAllMasterGridID' type='checkbox' onclick='selectAllMasterGridClicked(this)'  />")
                        .HeaderHtmlAttributes(new { @style = "font-weight:bold; text-align: lieft" })
                        .Sortable(false).Filterable(false);
                }

                if (/*(addDetailsColumn) && */(SessionUser.HasRights(rightName, UserRightValue.Details)))
                {
                    string toolTip = Language.GetToolTipText("Details");
                    col.Template("<a class='' style='min-width: 25px;' title='" + toolTip + "' href='javascript:editRecord(\"" + ControllerName +
                                                "\", \"" + gridName + "\", #=" + primaryKeyName + " #, \"" +
                               url.Action(detailsActionName)
                               + "\", \"" + detailsAdditionalData + "\")'>"
                               + "<img src='/css/images/view.png' alt ='icon' style='Width:20px;height:20px'/></a>").Title(Language.GetString("View")).
                               HeaderHtmlAttributes(new { @style = "max-width: 20px;" })
                             .Width(20);
                }

                if (/*(addEditColumn) &&*/ (SessionUser.HasRights(rightName, UserRightValue.Edit)))
                {
                    col.Template("<a class='' style='min-width: 25px;' title='" + Language.GetToolTipText("EditItemButton") + "' href='javascript:"
                                + editScriptFunctionName + "(\"" + controllerName +
                                                "\", \"" + gridName + "\", #=" + primaryKeyName + " #, \"" +
                               url.Action(editActionName)
                               + "\", \"" + editAdditionalData + "\")'>"
                               + "<img src='/css/images/Edit-icon.png' alt ='icon' style='Width:15px;height:18px'/></a>").Title(Language.GetString("Edit"))
                               .HeaderHtmlAttributes(new { @style = "max-width: 20px;" }).HtmlAttributes(new { @style = "text-align: center" })
                            .Width(20).Exportable(false);
                }

                if (/*(addDeleteColumn) &&*/ (SessionUser.HasRights(rightName, UserRightValue.Delete)))
                {
                    col.Template("<a class='' style='min-width: 25px;' title='" + Language.GetToolTipText("EditItemButton") + "' href='javascript:" +
                                deleteScriptFunctionName + "(\"" + controllerName +
                                              "\", \"" + gridName + "\", \"#=" + primaryKeyName + " #\", \"" +
                             url.Action(deleteActionName)
                             + "\", \"" + deleteAdditionalData + "\")'>"
                             + "<img src='/css/images/delete-icon.png' alt ='icon' style='Width:15px;height:20px'/></a>").Title(Language.GetString("Del")).HeaderHtmlAttributes(new { @style = "max-width: 20px;" })
                          .Width(20).Exportable(false);
                }

                foreach (var d in result)
                {
                    string displayFieldName = d.MasterGridLineItem.FieldName.Replace('.', '_');
                    string fieldName = d.MasterGridLineItem.FieldName.Replace('.', '_');
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
                        //if (!string.IsNullOrEmpty(rowAlternateTextField))
                        //    altText = rowAlternateTextField;

                        if (string.Compare(editableFieldName, d.MasterGridLineItem.FieldName, true) == 0)
                        {
                            col.Bound(fieldName).Title(Language.GetString(d.MasterGridLineItem.DisplayName))
                                .Width(d.MasterGridLineItem.Width)
                                .HeaderHtmlAttributes(new { @style = "font-weight:bold; width: " + d.MasterGridLineItem.Width + "%;" }).Encoded(false)
                                .ClientTemplate("#if(CustomerContractNo != null && CustomerContractNo != '' && IsAlgeriaContract == 1){# <input type='text' value='#=" + editableFieldName + "#' name='" + editableFieldName + "_#=" + primaryKeyName + "#' style='color: black;' /> #} else {# N/A #} #");
                        }
                        else
                        {
                            if (string.Compare("CatalogueImage", d.MasterGridLineItem.FieldName, true) == 0)
                            {
                                col.Bound(fieldName).HeaderHtmlAttributes(new { @style = "font-weight:bold; width: " + d.MasterGridLineItem.Width + "%;" }).Encoded(false)
                                .ClientTemplate(
                                                  "# if (CatalogueImage) { #" +
                                                  "<img src='/#= kendo.htmlEncode(CatalogueImage) #' style='width:100px;height:100px; background-size: cover;background-position: center;' alt='Image' />" +
                                                  "# } #").Title(Language.GetString("CatalogueImage")).Width(100);

                            }
                            else
                            {
                                col.Bound(fieldName).Title(Language.GetString(d.MasterGridLineItem.DisplayName))
                                .Width(d.MasterGridLineItem.Width)
                                .HeaderHtmlAttributes(new { @style = "font-weight:bold; width: " + d.MasterGridLineItem.Width + "%;" }).Encoded(false)
                                .ClientTemplate("<div style='" + colorString + "' class = 'tableContent' title='#=" +
                                altText + " #'>#if (" + displayFieldName + " != null) { ##=" +
                                displayFieldName + "##} else {# &nbsp; #}#</div>");
                            }
                        }
                    }
                    else
                    {
                        if (string.Compare(d.MasterGridLineItem.ColumnType, "System.DateTime") == 0)
                        {
                            col.Bound(fieldName).Title(Language.GetString(d.MasterGridLineItem.DisplayName))
                          .Width(d.MasterGridLineItem.Width).Encoded(false)
                         .HeaderHtmlAttributes(new { @style = "font-weight:bold; width: " + d.MasterGridLineItem.Width + "%;" })
                         .Format("{0:" + CultureHelper.ConfigureDateFormat + "}");
                        }
                        else
                        {
                            col.Bound(fieldName).Title(Language.GetString(d.MasterGridLineItem.DisplayName))
                                .Width(d.MasterGridLineItem.Width).Encoded(false)
                               .HeaderHtmlAttributes(new { @style = "font-weight:bold; width: " + d.MasterGridLineItem.Width + "%;" })
                               .Format("{0:" + d.MasterGridLineItem.Format + "}");
                        }
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

            });
        }
    }
}
