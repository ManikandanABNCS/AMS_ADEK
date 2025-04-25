/// <reference path="Common.js" />

function gotoLoginPage() {
    window.location = "/Login/Login";
}

function gotoHomePage() {
    window.location = "/Dashboard/Index";
}

function gotoHelpPage() {
    window.open("/Help/Index", "_WMSHelp");
}

function gotoChangePasswordPage() {
    loadContentPage("/Account/ChangePassword");
}

function gotoMyProfilePage() {
    loadContentPage("/Account/MyProfile");
}

function setPageHeading(heading) {
    $("#pageMainHeadingPortion").text(heading);
    setWindowTitle(heading);
}

function setPageHeadingSub(heading) {
    $("#pageMainHeadingPortionSub").text(heading);
    setWindowTitle(heading);
}

function setWindowTitle(tit) {
    document.title = "AMS - " + tit;

}
function loadDefaultPage(url) {
    loadContentPage(url);
}
function showLoadingMask() {

    //Get the screen height and width
    var maskHeight = $(window).height() - 75;
    var maskWidth = $(window).width() - 75;

    //Set height and width to mask to fill up the whole screen
    //  $('#loadingmask').css({ 'width': maskWidth, 'height': 900 });
    // $('#loadingmask').css({ 'width': maskWidth, 'height': 900 });

    //transition effect
    $('#loadingmaskApproval').show();
    //$('#loadingmask').fadeTo("slow", 0.1);
    //$('#mask').fadeIn(1000);
    return true;
}

function hideLoadingMask() {
    $('#loadingmaskApproval').hide();
}

function loadContentPage(url) {
    loadContentPageToControl(url, "workingArea", true);
}
function loadContentPageToControl(url, controlID, storePageURL) {
    if (url != null) {
        showLoadingMask();
        $("#" + controlID).load(url, function (response, status, xhr) {
            if (status == "error") {
                failurePageLoad(response, null);
            }
            else {
                gridCommands = null;
                if (storePageURL == true) currentPageURL = url;
                if (storePageURL) {
                    storePageHistory(url);
                }

                ParseLoadedForm();
                hideLoadingMask();
            }
        });
    }
}

function successPageLoad(e, e1) {
    ParseLoadedForm();
}

function beginPageLoad(args, e1) {
    showLoadingMask();
    return true;
    //alert(args.get_request().get_url());
}

function failurePageLoad(cnt, cnt1) {
    hideLoadingMask();
    if ((cnt != null) && (cnt.responseText == null))
        showErrorMessage(cnt);
    else if ((cnt == null) || (cnt.responseText == null) || (cnt.responseText == ""))
        showErrorMessage("Error while retriving data, Please try again later.");
    else
        showErrorMessage(cnt.responseText);
}

function pageLoadCompleted(ajaxContext) {
    hideLoadingMask();
    //alert(ajaxContext);
    //alert(ajaxContext.get_request());
    //var url = ajaxContext.get_request().get_url();
    //alert(url);
}

//function storePageHistory(url) {
//    try {
//        isPageHistoryAssigningInProcess = true;
//        Sys.Application.addHistoryPoint({ "url": url });
//    } catch (err) { }

//    isPageHistoryAssigningInProcess = false;
//}
function storePageHistory(url) {
    try {
        isPageHistoryAssigningInProcess = true;
        //

        url = "/Dashboard/Index?PageURL=" + url;
        //Sys.Application.addHistoryPoint({ "url": url });

        var obj = { Page: "New Page", Url: url };
        window.history.replaceState(obj, obj.Page, obj.Url);
        //alert("Updating 4 => " + url);
    } catch (err) { }

    isPageHistoryAssigningInProcess = false;
}

var initialFocusCtrlID = null;
function setInitialFocusInternal_ToDropDownList() {
    $(document).ready(function () {
        $('#' + initialFocusCtrlID).closest(".k-widget").focus();
    });
}
function setInitialFocusInternal_ToInputCtrls() {
    $(document).ready(function () {
        $('#' + initialFocusCtrlID).parent().find("input:first").focus()
    });
}
function setInitialFocusInternal() {
    $(document).ready(function () {
        ctrl = $('#' + initialFocusCtrlID);
        if (ctrl != null) ctrl.focus();
    });
}   
function setInitialFocus(ctrlID) {
    initialFocusCtrlID = ctrlID;

    if ($('#' + initialFocusCtrlID).data("kendoDropDownList") != null)
        setTimeout(setInitialFocusInternal_ToDropDownList, 250);
    else
        if ($('#' + initialFocusCtrlID).data("kendoMultiColumnComboBox") != null)
            setTimeout(setInitialFocusInternal_ToDropDownList, 250);
        else
            if (($('#' + initialFocusCtrlID).data("kendoComboBox") != null) ||
                ($('#' + initialFocusCtrlID).data("kendoNumericTextBox") != null) ||
                ($('#' + initialFocusCtrlID).data("kendoMultiSelect") != null))
                setTimeout(setInitialFocusInternal_ToInputCtrls, 250);
            else
                setTimeout(setInitialFocusInternal, 250);
}

function loadMasterIndexPage(pageName) {
    showLoadingMask();
    loadContentPage("/MasterPage/Index?pageName=" + pageName);
}

function loadDynamicIndexPage(controllerName, pageName) {
     showLoadingMask();
    loadContentPage("/" + controllerName + "/Index?pageName=" + pageName);
}
function loadIndexPage(controllerName) {
     showLoadingMask();
    loadContentPage("/" + controllerName + "/Index");
}
function ParseLoadedForm() {
    //find the available textboxes with maxlength
    $('[data-val-length-max]').each(function (index) {
        var itmID = $(this).attr('id');
        if (itmID != null) {
            var ctrl = document.getElementById(itmID);
            if (ctrl != null) ctrl.maxLength = $(this).attr('data-val-length-max');
        }
    });

    //For decimal values
    $('[data-val-range-max]').each(function (index) {
        var itmID = $(this).attr('id');
        if (itmID != null) {
            var ctrl = document.getElementById(itmID);
            if (ctrl != null) ctrl.maxLength = $(this).attr('data-val-range-max').toString().length + 3; //+3 is for decimal digits
        }
    });

    var ele1 = $.find(".userWorkingArea");
    var ele2 = $(".userWorkingArea");

    if (ele1.length > 0) {
        var frm = $("form");
        var attr = frm.attr("data-ajax-update");
        if ((attr != null) && (attr != undefined)) {
            frm.attr("data-ajax-update", "#" + ele1[ele1.length - 1].id)

            var newAttr = frm.attr("data-ajax-update");
        }
    }

    //var ffms = $('form');
    //if (ffms.length > 0) {
    //    var currentForm = ffms[0];

    //    $.validator.unobtrusive.parse(currentForm);
    //}

    //var frms = document.forms;
    //$.validator.unobtrusive.parse(frms[0]);

    //parseFunction($("#workingArea"));
    //CenterChildWindow();
}

function editRecord(pageName, gridName, recordID, url, additionalData) {
    loadContentPage(url + '/' + recordID + "?" + additionalData);
}
function showConfirmMessage(message) {
    return confirm(message);
}
function deleteRecord(pageName, gridName, recordID, url, additionalData) {
    debugger;
    if (!showConfirmMessage("Are you sure, you want to delete this record?"))
        return;

    url = url + "?id=" + recordID + "&" + additionalData;
    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",

        success: function (response) {
            if (response.actionRequired === true) {
                loadContentPage(response.actionURL);
            }
            else {
                $("#" + gridName).data('kendoGrid').dataSource.read();
                //showSuccessMessage("Record Deleted Successsfully");
            }
        },
        failure: function (response) {
            showErrorMessage(response.responseText);
        },
        error: function (response) {
            showErrorMessage(response.responseText);
        }
    });
}

function clickoneback(e) {

    e.preventDefault();

    var tabStrip = $("#TabStrip").data("kendoTabStrip");
    var indexValue = tabStrip.select().index();
    tabStrip.select(indexValue - 1);
}

function clickoneNext(e) {

    //e.preventDefault();
    //$("#form0").removeData("validator").removeData("unobtrusiveValidation");//remove the form validation
    //$.validator.unobtrusive.parse($("#form0"));//add the form validation
    //if ($("#form0").valid()) {
    var tabstrip = $("#TabStrip").data("kendoTabStrip");
    var nextItem = tabstrip.select().next();
    if (nextItem.length)
        tabstrip.select(nextItem);
    else
        tabstrip.select(tabstrip.tabGroup.children().first());
    //}
}

function showSuccessMessage(message1) {
    alert(message1);
    //var popupNotification = $("#popupNotification").data("kendoNotification");
  
    //    popupNotification.show({
    //        message: message1
    //    }, "success");
 
    //popupNotification.show(message, "success");
}

function showErrorMessage(msg) {
    window.alert(msg);
    //var popupNotification = $("#popupNotification").data("kendoNotification");
    //popupNotification.show(msg, "error");
}

function onGridError(e) {
    //`this` is the DOM element of the grid
    this.cancelChanges();
    var value = e.xhr.responseText;
    var grid = $(this).data('kendoGrid');

    //the current XMLHttpRequest object
    var xhr = e.XMLHttpRequest;
    //the text status of the error - 'timeout', 'error' etc.
    var status = e.status;

    if (e.status == 'error') {
        if (e.xhr.status == "404") {
            alert("requested url not found")
            return;
        }
        if (e.xhr.status == "500") {
            alert(e.xhr.responseText);
        }
    }

    if (e.status == "modelstateerror" && e.modelState) {
        var message = "Error: \n";
        $.each(e.modelState, function (key, value) {
            if ('errors' in value) {
                $.each(value.errors, function () {
                    message += this + "\n";
                });
            }
        });

        alert(message);
    }
    e.preventDefault();

    //e.preventDefault();
    //failurePageLoad();
}
function openChildWindowItem(name, title, index, url) {

    // console.log("Index:" + index);
    openChildWindow(name, title, index, url, 900, 550);
}
//var AddItemwindow;
var currentChildWindow;
function openChildWindow(name, title, index, url, width, height) {


    var name = document.getElementById("CustomizeGridColumnWindow");
    //$(name).kendoWindow({
    //    title: title,
    //    visible: false,
    //    width: 700,
    //    height: 500,
    //    content: {
    //        url: url

    //    },

    //    draggable: false,
    //    modal: true,
    //    resizable: false
    //});
    //var window = $(name).data("kendoWindow");
    //sessionStorage.setItem("checkWindow", true);
    //AddItemwindow = window;
    //AddItemwindow.center();
    //AddItemwindow.open();

    //var container = $("#custom-menu");

    //$("#custom-menu").css({ display: "none" });
    $(name).kendoWindow({
        title: title,
        visible: false,
        width: width,
        height: height,
        content: {
            url: url,
            data: { indexName: index }
        },
        actions: [
            "Maximize",
            "Close"
        ],
        draggable: false,
        modal: true,
        resizable: true
    });

    var window = $(name).data("kendoWindow");
    currentChildWindow = window;
    currentChildWindow.center();
    currentChildWindow.open();
    currentChildWindow.refresh();
}
function StaticChangeColumnGrid(controllerName) {

    openChildWindowItem("#upload" + controllerName + "Window", "CustomizeGridColumn", controllerName, "/DynamicGrid/Index?controllerName=" + controllerName + "&pageName=" + controllerName + "&Isdynamic=" + false);
}
//function changeColumnGrid(e) {
//    
//    var controllerName = $("#" + e.id).attr("controllerName");
//    var gridIndexName = $("#" + e.id).attr("valueID");
//    var pageName = $("#" + e.id).attr("pageName");
//    openChildWindowItem("#upload" + controllerName + "Window", "CustomizeGridColumn", gridIndexName, "/DynamicGrid/Index?controllerName=" + controllerName + "&pageName=" + pageName + "&Isdynamic=" + true);
//}
function changeColumnGrid(controllerName, gridIndexName, pageName) {

    var controllerName = controllerName;
    var gridIndexName = gridIndexName;
    var pageName = pageName;
    openChildWindowItem("#upload" + controllerName + "Window", "CustomizeGridColumn", gridIndexName, "/DynamicGrid/Index?controllerName=" + controllerName + "&pageName=" + pageName + "&Isdynamic=" + true);
}
function CancelGridPopUp(url) {

    parent.parent.CloseGridFromPopUpWindow(url);


}
function CloseGridFromPopUpWindow(url) {

    // alert("" + window.parent);
    currentChildWindow.close();
    //alert(showAdvanceSearch);
    showAdvanceSearch = true;
    loadDefaultPage(url);
    //var container = parent.window.doc.getElementById("#custom-menu");
    //container.hide();

    //$('#DetailsGrid').data('kendoGrid').dataSource.read();
    //window.location = window.location;
}

//function gridButtonClicked(e) {
//    //

//    var cName = $("#" + e.id).attr("controllerName");
//    var aName = $("#" + e.id).attr("actionName");
//    var vID = $("#" + e.id).attr("valueID");
//    var pageName = $("#" + e.id).attr("pageName");
//    if (vID == undefined)
//        loadContentPage("/" + cName + "/" + aName + "?pageName=" + pageName);
//    else
//        loadContentPage("/" + cName + "/" + aName + "/" + vID + "?pageName=" + pageName);
//}
function gridButtonClicked(controllerName, actionName, pageName,valueID ) {
    //

    if (valueID == undefined)
        loadContentPage("/" + controllerName + "/" + actionName + "?pageName=" + pageName);
    else
        loadContentPage("/" + controllerName + "/" + actionName + "/" + valueID + "?pageName=" + pageName);
}

/*
    GRID METHODS
*/

function restoreNoRecordsText() {
    if ($('#ExportType') && kendoHeaderDropDown != null) {
        $("#ExportType").kendoDropDownList({
            optionLabel: {
                Text: "Select Export....",
                Value: ""
            },
            dataTextField: "Text",
            dataValueField: "Value",
            dataSource: kendoHeaderDropDown
        });
    }
    $("tr.t-no-data td").html("No records to display");
}

function onGridError(e) {
    //`this` is the DOM element of the grid
    this.cancelChanges();
    var value = e.xhr.responseText;
    var grid = $(this).data('kendoGrid');

    //the current XMLHttpRequest object
    var xhr = e.XMLHttpRequest;
    //the text status of the error - 'timeout', 'error' etc.
    var status = e.status;

    if (e.status == 'error') {
        if (e.xhr.status == "404") {
            alert("requested url not found")
            return;
        }
        if (e.xhr.status == "500") {
            alert(e.xhr.responseText);
        }
    }

    if (e.status == "modelstateerror" && e.modelState) {
        var message = "Error: \n";
        $.each(e.modelState, function (key, value) {
            if ('errors' in value) {
                $.each(value.errors, function () {
                    message += this + "\n";
                });
            }
        });

        alert(message);
    }
    e.preventDefault();

    //e.preventDefault();
    //failurePageLoad();
}

var kendoHeaderDropDown;
function defaultGridReadMethod(e1) {
    var state = {
        page: e1.page,
        pageSize: e1.pageSize,
        sort: e1.sort,
        filter: e1.filter
    };

    return {
        gridState: JSON.stringify(state)
    };
}
function defaultGridReadWithCurrenPageMethod(e1) {
    var state = {
        page: e1.page,
        pageSize: e1.pageSize,
        sort: e1.sort,
        filter: e1.filter
    };

    return {
        gridState: JSON.stringify(state),
        currentPageID:$("#CurrentPageID").val()
    };
}

function defaultGridReadWithParamMethod(e1) {
    var state = {
        page: e1.page,
        pageSize: e1.pageSize,
        sort: e1.sort,
        filter: e1.filter
    };

    return {
        gridState: JSON.stringify(state),
        currentPageID: $("#CurrentPageID").val(),
        transactionID: $("#TransactionID").val()
    };
}
function defaultGridReadWithAdditionPrarm(e1) {
    var state = {
        page: e1.page,
        pageSize: e1.pageSize,
        sort: e1.sort,
        filter: e1.filter
    };

    return {
        gridState: JSON.stringify(state),
        currentPageID: $("#CurrentPageID").val(),
        screenName: $("#ScreenName").val()
    };
}
/**
 * 
 * Export methods
 */

function exportToExcel(url) {

    var grid = $("#DetailsGrid").data("kendoGrid");
    grid.saveAsExcel();
}

function exportToPDF(url) {
    var grid = $("#DetailsGrid").data("kendoGrid");
    grid.saveAsPDF();

    //exportCurrentGridData(url + '\\ExportToPDF');
}

function exportToExcelCustom(url) {
    exportCurrentGridData(url + '\\ExportToExcel');
}

function exportToPDFCustom(url) {
    exportCurrentGridData(url + '\\ExportToPDF');
}

function exportCurrentGridData(url) {
    var data1 = null;
    if (typeof getFiltersForExport === "function")
        data1 = getFiltersForExport();

    var grid1 = $("#DetailsGrid").data("kendoGrid");
    var data = grid1.dataSource.filter;

    var qry = jQuery.param(data1);
    window.open('\\' + url + '?' + qry);
}

//Function for handling export excel column width (By default columns are coming with tiny width)
function gridExportToExcelHandler(e) {
   

    var sheet = e.workbook.sheets[0];
    sheet.frozenRows = 1;
    sheet.name = "Orders";

    var columns = sheet.columns;

    //var data = e.sender.dataSource.view();
    //for (var i = 0; i < data.length; i++) {
    //    for (var col = 0; col < columns.length; col++) {
    //        var template = kendo.template(e.sender.columns[col].template)
    //        sheet.rows[i + 1].cells[col].value = template(data[col]);
    //    }
    //}

    columns.forEach(function (column) {
        delete column.width;
        column.autoWidth = true;
    });
}

function Sever_UpdateData(url, data, successCallBack, failureCallBack) {
    $.ajax({
        url: url,
        type: "GET",
        data: data,
        success: function (data) {
            if (successCallBack != null)
                successCallBack(data);
            else {
                if ((data != null) && (data != ''))
                    alert(data);
            }
        },
        failure: function (data) {
            if (failureCallBack != null)
                failureCallBack(data);
            else {
                if (data != null)
                    alert(data.responseText);
                else
                    alert("Unknown Error Occurred");
            }
        },
        error: function (data) {
            if (failureCallBack != null)
                failureCallBack(data);
            else {
                if (data != null)
                    alert(data.responseText);
                else
                    alert("Unknown Error Occurred");
            }
        }
    });
}

function Sever_PostData(url, data, successCallBack, failureCallBack) {
    $.ajax({
        type: "POST",
        url: url,
        data: data,

        success: function (responce) {
            //debugger
            //alert(response);
            if (successCallBack != null) successCallBack(responce);
        },
        failure: function (responce) {
            //
            //alert(response.responseText);
            if (failureCallBack != null)
                failureCallBack(responce);
            else
                alert(response.responseText);
        },
        error: function (responce) {
            //
            if (failureCallBack != null)
                failureCallBack(responce);
            else
                alert(response.responseText);
        }
    });
}


/*
    MESSAGE METHODS
*/



/************************** */

function printBarcode(url, data) {
    $.ajax({
        url: url,
        type: "POST",
        //dataType: "json",
        data: data,
        success: function (response) {
            CallPrint(response);
        },
        failure: function (response) {
            showErrorMessage(response.responseText);
        },
        error: function (response) {
            showErrorMessage(response.responseText);
        }
    });
}

function printRawBarcode(url, data) {
    $.ajax({
        url: url,
        type: "POST",
        //dataType: "json",
        data: data,
        success: function (response) {
            CallPrint(response);
        },
        failure: function (response) {
            showErrorMessage(response.responseText);
        },
        error: function (response) {
            showErrorMessage(response.responseText);
        }
    });
}

function viewBarcode(url, data) {
    $.ajax({
        url: url,
        type: "POST",
        //dataType: "json",
        data: data,
        success: function (response) {
            LoadBarcodeDataFromExternal(response);
        },
        failure: function (response) {
            showErrorMessage(response.responseText);
        },
        error: function (response) {
            showErrorMessage(response.responseText);
        }
    });
}

function CallPrint(data) {
    var strOldOne = window.location + "?" + Math.floor(Math.random());
    var WinPrint = window.open("", 'Print', 'left=0,top=0,width=500,height=500,toolbar=0,scrollbars=1,status=0');
    if (WinPrint == null) {
        alert("Unable to open print window");
    }

    WinPrint.document.write('<FONT>' + data + '</FONT>');
    WinPrint.document.close();
    WinPrint.focus();
    WinPrint.print();
    WinPrint.close();

    // WinPrint.opener.document.forms[0].reset();
    window.location = strOldOne;
}

function LoadBarcodeDataFromExternal(zpl) {
    
    zpl = zpl.replace("<BR>", "\n");

    var baseUrl = "https://api.labelary.com/v1/printers/8dpmm/labels/4x6/";

    var formData = new FormData();
    formData.append('file', zpl);

    var xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () { dataReadFromClient(this); };
    xhr.open('POST', baseUrl);
    xhr.setRequestHeader('Accept', 'application/pdf');
    xhr.responseType = 'blob';
    xhr.send(formData);
}

function dataReadFromClient(xhr) {
 
    if (xhr.readyState == 4) {

        var totalCountHeader = xhr.getResponseHeader('X-Total-Count'); // received with all 200s and with some 404s
        if (xhr.status == 200) {
            var wurl = window.URL || window.webkitURL;
            var url = wurl.createObjectURL(xhr.response);
            var filename = 'labels.pdf';
            triggerDownload(url, filename);

            wurl.revokeObjectURL(url);
        } else if (xhr.status >= 400 && xhr.status <= 599) {
            var reader = new FileReader();
            reader.onload = function (e) {
                //labelDone(totalCountHeader, reader.result);
            };

            alert(xhr.statusText + "\r\nMaximum 50 labels only allowed");
        } else if (xhr.status == 0) {
        }
    }
}

function triggerDownload(url, filename) {
    var a = document.createElement('a');
    a.href = url;
    a.download = filename;
    a.style = 'display: none';
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
}

/**
* Master data Popup window
**/
function openItemPopupWindow(itemCode, itemID) {
    var params = {
        itemCode: itemCode,
        itemID: itemID
    };

    var url = '/Item/ItemDetails?' + jQuery.param(params);
    openChildWindow('#popupWindowControl', 'Item Details', -1, url, 1500, 750);
}

function onMenuSelected(e) {

}
function getTheSubstring(value, length) {
    if (value.length > length)
        return kendo.toString(value.substring(0, length)) + "...";
    else return kendo.toString(value);
}

/*dynamic grid checkbox selection */
var checkedIds = {};
var ItemIds = [];
function selectAllMasterGridClicked(checkBox) {
    
    ItemIds = [];
    var grid = $("#DetailsGrid").data("kendoGrid");
    var sel = $("input:checkbox", grid.tbody).closest("tr");

    if (checkBox.checked == true) {

        $.each(sel, function (idx, row) {
            var item = grid.dataItem(row);

            $(this).closest('tr').find('[type=checkbox]').prop('checked', true);
            ItemIds.push(item.id);
            checkedIds[item.id] = true;

        });

    }
    else {
        $('.masterGridClass').removeAttr('checked');
        $.each(sel, function (idx, row) {
            var item = grid.dataItem(row);
            ItemIds = jQuery.grep(ItemIds, function (value) {
                return value != item.id;
            });
            checkedIds[item.id] = false;
        });
    }
    $("#MasterTobeDeleteIDs").val(ItemIds);
}


function enableMasterGridRow(e) {
     //ItemIds = [];
    //var grid = $(e).closest("tr");
    var grid = $("#DetailsGrid").data("kendoGrid");
    grid.table.on("click", ".masterGridClass", selectMasterGridRow);
    //on click of the checkbox:
    function selectMasterGridRow() {
        
        var checked = this.checked,
            row = $(this).closest("tr"),
            grid = $("#DetailsGrid").data("kendoGrid"),
            dataItem = grid.dataItem(row);
        checkedIds[dataItem.id] = checked;


        if (checked) {
            //-select the row

            row.addClass("k-state-selected");
            ItemIds.push(dataItem.id);


        }
        else {
            //-remove selections
            //checkedIds[dataItem.id] = checked;
            row.removeClass("k-state-selected");
            ItemIds = jQuery.grep(ItemIds, function (value) {
                return value != dataItem.id;
            });

        }
        $("#MasterTobeDeleteIDs").val(ItemIds);
    }
}


function DeleteMultipleGridRecord(pageName, gridName, recordID, url, additionalData) {
    debugger;
    if (ItemIds.length > 0) {
        if (!showConfirmMessage("Are you sure, you want to delete this record?"))
            return;
  
        url = url + "?toBeDeleteIds=" + ItemIds + "&" + additionalData;
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response.actionRequired === true) {
                    ItemIds.length = 0;
                    loadContentPage(response.actionURL);
                }
                else {
                    ItemIds.length = 0;
                    $("#" + gridName).data('kendoGrid').dataSource.read();
                    //showSuccessMessage("Record Deleted Successsfully");
                }
            },
            failure: function (response) {
                showErrorMessage(response.responseText);
            },
            error: function (response) {
                showErrorMessage(response.responseText);
            }
        });
       
    }
    else {
        showErrorMessage("Select Atleast One Record to delete.");
        return false;
    }
}
/*End dynamic grid checkbox selection */
function Listbox_Move(ListID, direction) {

    var listbox = document.getElementById(ListID);
    var selIndex = listbox.selectedIndex;
    if (-1 == selIndex) {
        $.when(KendoErrorMsg("Please Select any options to move ")
        ).done(function () {

        });

    }
    var increment = -1;
    if (direction == 'up') {
        increment = -1;
    }
    else {
        increment = 1;
    }
    if ((selIndex + increment) < 0 || (selIndex + increment) > (listbox.options.length - 1)) {
        return;
    }
    var selValue = listbox.options[selIndex].value;
    var selText = listbox.options[selIndex].text;
    listbox.options[selIndex].value = listbox.options[selIndex + increment].value;
    listbox.options[selIndex].text = listbox.options[selIndex + increment].text;

    listbox.options[selIndex + increment].value = selValue;
    listbox.options[selIndex + increment].text = selText;

    listbox.selectedIndex = selIndex + increment;
}
 function addFieldsTemplate(lstAvalibleField, lstSelectedField) {
        var ctrl = document.getElementById(lstAvalibleField);
        for (i = 0; i < ctrl.length; i++) {
            if (ctrl[i].selected) {
                document.getElementById(lstSelectedField).appendChild(document.getElementById(lstAvalibleField).options.item(i));
                i--;
            }
        }


    }
function showErrorMessage(msg) {
    window.alert(msg);
}

function EntityChange(e) {
    if ($("#ImportTemplateTypeID").val() == "") {
        KendoErrorMsg("Please select Excel Type");
        return;
    }
    var availableList = $('#lstAvailableFields');
    var SelectedList = $('#lstSelectedFields');

    $.ajax({
        url: '/DataService/CheckRights',
                  type: "Get",
                  async: false,
        data: { entityName: $('#EntityID').val(), type: $("#ImportTemplateTypeID").val() },
                  success: function (data) {
                      if (data.Result == "Success") {
                          if (data.rights == 1) {
                              $.get("/DataService/GetAvailableFieldsForGenerateTemplate?entityName=" + $('#EntityID').val() + "&type=" + $("#ImportTemplateTypeID").val(), function (response) {
                                  var result = response;//$.parseJSON(response);

                                  var listItems = [];
                                  var listedSelectedItem = [];
                                  var listDynamic = [];
                                  var listDynamicSekected = [];
                                  var available = result.availableFields;
                                  var selected = result.selectedFields;

                                  if (available.length > 0) {
                                      for (var key in available) {
                                          listItems.push('<option value="' +
                                              available[key].Value + '">' + available[key].Text
                                              + '</option>');
                                      }
                                  }

                                  if (selected.length > 0) {
                                      for (var key in selected) {
                                          listedSelectedItem.push('<option value="' +
                                              selected[key].Value + '">' + selected[key].Text + "<span color='red'>*</span>"
                                              + '</option>');
                                      }
                                  }

                                  //remove all items from listbox
                                  $('#lstAvailableFields option').each(function (i, option) { $(option).remove(); });
                                  //groupComboBox.remove();
                                  availableList.append(listItems.join(''));

                                  //remove all items from listbox
                                  $('#lstSelectedFields option').each(function (i, option) { $(option).remove(); });
                                  SelectedList.append(listedSelectedItem.join(''));
                              });

                              var ctrl = document.getElementById("lstSelectedFields");
                              for (i = 0; i < ctrl.length; i++) {
                                  var str = ctrl.options.item(i).innerHTML;
                                  ctrl.options.item(i).innerHTML = str + "<span class='newStar' >*</span>";
                              }

                          }
                          else {
                              alert("You have no access rights for select template");
                          }
                    
                      }
                     

                  }
              });

}

function delImport() {
    var ctrl = document.getElementById("lstSelectedFields");
    for (i = 0; i < ctrl.length; i++) {
        if (ctrl[i].selected) {
            document.getElementById("lstAvailableFields").appendChild(document.getElementById("lstSelectedFields").options.item(i));
            i--;
        }
    }
}

function addImport() {
    var ctrl = document.getElementById("lstAvailableFields");
    for (i = 0; i < ctrl.length; i++) {
        if (ctrl[i].selected) {
            document.getElementById("lstSelectedFields").appendChild(document.getElementById("lstAvailableFields").options.item(i));
            i--;
        }
    }
}


function Listbox_Move(ListID, direction) {

    var listbox = document.getElementById(ListID);
    var selIndex = listbox.selectedIndex;
    if (-1 == selIndex) {
        $.when(KendoErrorMsg("Please Select any options to move ")
        ).done(function () {

        });

    }
    var increment = -1;
    if (direction == 'up') {
        increment = -1;
    }
    else {
        increment = 1;
    }
    if ((selIndex + increment) < 0 || (selIndex + increment) > (listbox.options.length - 1)) {
        return;
    }
    var selValue = listbox.options[selIndex].value;
    var selText = listbox.options[selIndex].text;
    listbox.options[selIndex].value = listbox.options[selIndex + increment].value;
    listbox.options[selIndex].text = listbox.options[selIndex + increment].text;

    listbox.options[selIndex + increment].value = selValue;
    listbox.options[selIndex + increment].text = selText;

    listbox.selectedIndex = selIndex + increment;
}

function ImportTemplateChange(e) {
    $('#lstAvailableFields option').each(function (i, option) { $(option).remove(); });
    $('#lstSelectedFields option').each(function (i, option) { $(option).remove(); });

    var ddl = $('#EntityID').data("kendoDropDownList");
    ddl.select(0);
}
function Notification_QueryObjectChanged(screen, queryObjectName, queryObjectType, currentPageID, successAction) {
    var prm = { screen: screen, queryName: queryObjectName, queryType: queryObjectType, currentPageID: currentPageID };
    var url = "/NotificationModule/LoadQueryObjectFields?" + $.param(prm);

    var jqxhr = $.ajax(url)
        .done(function (data) {
            successAction();
        })
        .fail(function () {
            alert("Unknown error occurred, please try again later.");
        })
        .always(function () {
            //alert("complete");
        });
}

function NotificationModule_loadLineItemsPage(pageNo, IsDetailPage, saveData) {
    
    currentPageID = GetCurrentPageID();
    var params = {
        screen: "SHOWDATA", //data already loaded only show the details

        currentPageID: currentPageID,
        pageNumber: Math.trunc(pageNo),
        detailsPage: IsDetailPage
    };

    var url = '/NotificationModule/NotificationModuleFields?' + jQuery.param(params);
    //var url = '/ASN/POLineItems?poNumber=' + poNumber + '&currentPageID=' + currentPageID + '&pageNumber=' + pageNumber;
    loadContentPageToControl(url, "NotificationFieldGrid", false);
}
function LoadIndexPage(controllerName, pageName) {
    // showLoadingMask();
    loadContentPage("/" + controllerName + "/Index?pageName=" + pageName);
}
//function GetAdditionReportParam() {

//    var result = GetAdditionParam1();
//    alert("TEST");

//    return result;
//}



var checkedTransactionLineItemIds = {};
var TransactionLineItemIds = [];
function enableGridLineItemRow(e) {
    debugger;
    var grid = $(e).closest("tr");
    var grid = $("#GridLineItemDetailGrid").data("kendoGrid");
    grid.table.on("click", ".GridLineItemGridClass", selectTransactionLineItemGridRow);

}

function selectTransactionLineItemGridRow() {
    debugger;
    var checked = this.checked,
        row = $(this).closest("tr"),
        grid = $("#GridLineItemDetailGrid").data("kendoGrid"),
        dataItem = grid.dataItem(row);
    checkedTransactionLineItemIds[dataItem.id] = checked;

    if (checked) {
        //-select the row
        row.addClass("k-state-selected");
        TransactionLineItemIds.push(dataItem.id);
    } else {
        //-remove selections
        row.removeClass("k-state-selected");

        TransactionLineItemIds = jQuery.grep(TransactionLineItemIds, function (value) {
            return value != dataItem.id;
        });

    }
}
function selectAllGridLineItemClicked(checkBox) {
    debugger;
    var grid = $("#GridLineItemDetailGrid").data("kendoGrid");
    var sel = $("input:checkbox", grid.tbody).closest("tr");

    if (checkBox.checked == true) {
        $('.GridLineItemGridClass').prop('checked', 'checked');
        $.each(sel, function (idx, row) {
            var item = grid.dataItem(row);

            //$(this).closest('tr').find('[type=checkbox]').prop('checked', true);
            TransactionLineItemIds.push(item.id);
            checkedTransactionLineItemIds[item.id] = true;

        });
    }
    else {
        $('.GridLineItemGridClass').removeAttr('checked');
        $.each(sel, function (idx, row) {
            var item = grid.dataItem(row);
            TransactionLineItemIds = jQuery.grep(TransactionLineItemIds, function (value) {
                return value != item.id;
            });
            checkedTransactionLineItemIds[item.id] = false;
        });

    }
}

/*LineItem delete all*/
function DeleteMultipleGridLineItemDetailRecord(pageName, gridName, recordID, url, additionalData) {
    debugger;
    if (TransactionLineItemIds.length > 0) {
        if (!showConfirmMessage("Are you sure, you want to delete this record?"))
            return;
        debugger;
        url = url + "?toBeDeleteIds=" + TransactionLineItemIds + "&" + additionalData;
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                debugger;
                if (response == "Success") {
                    $("#GridLineItemDetailGrid").data('kendoGrid').dataSource.read();
                }

            },
            failure: function (response) {
                showErrorMessage(response.responseText);
            },
            error: function (response) {
                showErrorMessage(response.responseText);
            }
        });
    }
    else {
        showErrorMessage("Select Atleast One Record to delete.");
        return false;
    }
}
function DownloadFunction(id) {
   
    window.location = "/ImportFormat/GetdownloadDetails/?ImportFormatID=" + id;
}
function ValidateKey(event) {
    if (event.keyCode == 46 || event.keyCode == 8) {
    }
    else {
        event.preventDefault();
    }
}
function GetPopupNames(id, Name, objectName) {
    if (objectName == "Location") {
        var strarray = id.split('-');
        if (strarray.length == 2) {
            var loc = strarray[0].slice(3);

            getHierarchDetails(loc, "Location", function (data) {
                document.getElementById("RoomLocation").value = data;
                //document.getElementById("RoomLocation").value = Name;        
                document.getElementById("LocID").value = id;
            });
        }
        else {
            getHierarchDetails(id, "Location", function (data) {
                document.getElementById("RoomLocation").value = data;
                //document.getElementById("RoomLocation").value = Name;        
                document.getElementById("LocID").value = id;
            });
        }

        //window.parent.document.getElementById("LocationID").value = id;
    }

    if (objectName == "Category") {
        getHierarchDetails(id, "Category", function (data) {
            document.getElementById("Category").value = data;
            document.getElementById("CatID").value = id;
            //window.parent.document.getElementById("CategoryID").value = id;
            //window.parent.document.getElementById("AssetDescription").value = "";
        });
    }
    if (objectName == "ReportCategory") {
        getHierarchDetails(id, "Category", function (data) {
            document.getElementById("CatID").value = data;
            document.getElementById("Category").value = id;
            if (window.parent.document.getElementById("CategoryID") != null) {
                window.parent.document.getElementById("CategoryID").value = id;
            }
        });
    }
}
function getHierarchDetails(id, type, fn) {
    var data;
    $.ajax({
        url: '/DataService/GetHierarchDetails',
        dataType: "json",
        cache: false,
        async: false,
        type: 'GET',
        data: { id: id, type: type },
        success: function (result) {

            data = result.hierarchy;
            fn(data);
        }
    });
}
function hiddenvalues(id, name) {
    $("#" + name).val(id);
}
function ongroupBoxchangeevent(textbox, ID) {
    if (!textbox) {
        hiddenvalues(textbox, ID);
    }
}
function ValidateKey(event) {

    if (event.keyCode == 46 || event.keyCode == 8) {
    }
    else {
        event.preventDefault();
    }
}
function onPopupOpenWindow(id, title, row) {
    row = row || 0;
    url = "/Popup/Index?id=" + id + "&row=" + row;
    popupWindow(url, title);
}
function popupWindow(url, titleCon) {
    var wnd = $("#window").data("kendoWindow");
    if (wnd == null)
        wnd = parent.window.$("#window").data("kendoWindow");
    wnd.refresh({
        url: url
    });

    wnd.center();
    wnd.open();
    wnd.title(titleCon);
}
function onPopupProductOpenWindow(id, title) {
   
    url = "/PopupProduct/Create?pageName=Product";
    popupWindow(url, title);
}
function OnPopupClose() {
    var w = window.parent.$("#window").data("kendoWindow");
    if (w == null)
        w = window.parent.$("#Popupchild").data("kendoWindow");
    w.close();

}

function UpdateDesc() {
    debugger;
    if ($("#CategoryID").val() == 0) {
        alert("Please select category");
    }
    else {
        url = "/PopupProduct/Edit?id=" + $("#ProductID").val() + "&pageName=Product";
        var title = "Load Description";
        popupWindow(url, title);
    }
}
function checkCategory() {
    if (!parseInt(document.getElementById("CatID").value)) {
        $.when(alert("Please Select any one Category ")
        ).done(function () {


        });
    }
    else {
        popupProductWindow("/Popup/QuickCreate?id=" + parseInt(document.getElementById("CatID").value), "ProductDescription");
    }

}
//ProductWindow
function popupProductWindow(url, titleCon) {
    var wnd = $("#ProductWindow").data("kendoWindow");
    if (wnd == null)
        wnd = parent.window.$("#ProductWindow").data("kendoWindow");
    wnd.refresh({
        url: url
    });

    wnd.center();
    wnd.open();
    wnd.title(titleCon);
}
function popupProductClose(categoryID) {
    debugger;
    var grid = window.parent.$("#ListView").data("kendoListView");
    if (categoryID != '') {
        grid.dataSource.read();
    }
    var w = window.parent.$("#ProductWindow").data("kendoWindow");
    w.close();
}

function ConfirmCategoryDelete(pageName) {

    if ($("#SelectedID").val() == 0) {
        alert("Please Select" + pageName + " from TreeView");
    }
    else {
        if (!showConfirmMessage("Are you sure, you want to delete this record?"))
            return;

        url = "/TreeView/DeletePopup?pageName=" + pageName + "&id=" + $("#SelectedID").val();
        $.ajax({

            type: "POST",
            url: url,
            async: false,
            contentType: "application/json; charset=utf-8",


        });
    }
}
function onEndDatePicker(e) {
      
        var startDate  = document.getElementById("StartDate").value;
        var endDate = document.getElementById("EndDate").value;        
        var regExp = /(\d{1,2})\/(\d{1,2})\/(\d{2,4})/;
        if (parseInt(endDate.replace(regExp, "$3$2$1")) < parseInt(startDate.replace(regExp, "$3$2$1"))) {

            $.when(showErrorMessage("Select End Date Greater Than Start Date")
    ).done(function () {
        document.getElementById("EndDate").value = "";
    });
        }
    }


  function CallPrint(data) {
    var strOldOne = window.location;
    var WinPrint = window.open("/BarcodePrinting/Create", 'Print', 'left=0,top=0,width=500,height=500,toolbar=0,scrollbars=0,status=0');
    WinPrint.document.write('<FONT COLOR="WHITE">' + data + '</FONT>');
    WinPrint.document.close();
    WinPrint.focus();
    WinPrint.print();
    WinPrint.close();
    WinPrint.opener.document.forms[0].reset();
    window.location = strOldOne;
}
function pad(number, length) {
    var str = '' + number;
    while (str.length < length) {
        str = '0' + str;
    }
    return str;
}
function onlyNos(e, t) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    }
    catch (err) {
        alert(err.Description);
    }
}
function removeSpecialCharacter(e) {
    var regex = new RegExp("[a-zA-Z0-9]");
    var key = e.keyCode || e.which;
    key = String.fromCharCode(key);

    if (!regex.test(key) && key.charCodeAt(0) >= 32) {
        e.returnValue = false;
        if (e.preventDefault) {
            e.preventDefault();
        }
    }
}
function removeSpace(textbox, ID) {

    var space = textbox.replace(/\s/g, '');
    var special = textbox.replace("£©→–€é℃℉®©",'');
    document.getElementById(ID).value = special;
}
function DashboardTemplate_QueryObjectChanged(screen, queryObjectName, queryObjectType, currentPageID, successAction) {
    var prm = { screen: screen, queryName: queryObjectName, queryType: queryObjectType, currentPageID: currentPageID };
    var url = "/DashboardTemplate/LoadQueryObjectFields?" + $.param(prm);

    var jqxhr = $.ajax(url)
        .done(function (data) {
            debugger;
            successAction();
        })
        .fail(function () {
            alert("Unknown error occurred, please try again later.");
        })
        .always(function () {
            //alert("complete");
        });
}

function DashboardTemplate_loadLineItemsPage(pageNumber, saveData) {
    currentPageID = GetCurrentPageID();
    var params = {
        screen: "SHOWDATA", //data already loaded only show the details

        currentPageID: currentPageID,
        pageNumber: pageNumber
    };

    var url = '/DashboardTemplate/DashboardTemplateFields?' + jQuery.param(params);
    //var url = '/ASN/POLineItems?poNumber=' + poNumber + '&currentPageID=' + currentPageID + '&pageNumber=' + pageNumber;
    loadContentPageToControl(url, "DashboardFieldGrid", false);
}

function DashboardTemplate_loadFilterLineItemsPage(pageNumber, saveData) {
    currentPageID = GetCurrentPageID();
    var params = {
        screen: "SHOWDATA", //data already loaded only show the details

        currentPageID: currentPageID,
        pageNumber: pageNumber
    };

    var url = '/DashboardTemplate/DashboardTemplateFilterFields?' + jQuery.param(params);
    //var url = '/ASN/POLineItems?poNumber=' + poNumber + '&currentPageID=' + currentPageID + '&pageNumber=' + pageNumber;
    loadContentPageToControl(url, "DashboardFilterFieldGrid", false);
}

function ChangeDashboardName(e) {
    var availablefieldList = $('#lstAvailableFields');
    $.get("/DataService/GetAvaliblityDashboardcolumns?dashboardTemplateID=" + $('#DashboardTemplateID').data("kendoMultiColumnComboBox").value(), function (response) {
        var result = response;//$.parseJSON(response);

        var listItems = [];
        var available = result.availableFields;
        if (available.length > 0) {
            for (var key in available) {
                listItems.push('<option value="' +
                    available[key].Value + '">' + available[key].Text
                    + '</option>');
            }
        }

        //remove all items from listbox
        $('#lstAvailableFields option').each(function (i, option) { $(option).remove(); });
        //groupComboBox.remove();
        availablefieldList.append(listItems.join(''));
        $('#lstSelectedFields option').each(function (i, option) { $(option).remove(); });

    });
}

function addDashboardTemplate(lstAvailableFields, lstSelectedFields) {
    var ctrl = document.getElementById(lstAvailableFields);
    for (i = 0; i < ctrl.length; i++) {
        if (ctrl[i].selected) {
            document.getElementById(lstSelectedFields).appendChild(document.getElementById(lstAvailableFields).options.item(i));
            i--;
        }
    }
}

function delDashboardTemplateFields(lstAvailableFields, lstSelectedFields) {
    var ctrl = document.getElementById(lstSelectedFields);
    for (i = 0; i < ctrl.length; i++) {
        if (ctrl[i].selected) {
            document.getElementById(lstAvailableFields).appendChild(document.getElementById(lstSelectedFields).options.item(i));
            i--;
        }
    }
}
function ChangeDashboardType() {
    var type = $("#DashboardTypeID").data("kendoDropDownList").text();
    if (type == "Count") {
        document.getElementById("XaxisLabel").style.display = "none";
        document.getElementById("XaxisValue").style.display = "none";
        document.getElementById("YaxisLabel").style.display = "none";
        document.getElementById("YaxisValue").style.display = "none";
        document.getElementById("IconLabel").style.display = "";
        document.getElementById("IconValue").style.display = "";
        document.getElementById("PageLabel").style.display = "";
        document.getElementById("PageValue").style.display = "";
        document.getElementById("CategoryaxisLabel").style.display = "none";
        document.getElementById("CategoryaxisValue").style.display = "none";
    }
    if (type != "Count") {
        document.getElementById("IconLabel").style.display = "none";
        document.getElementById("IconValue").style.display = "none";
        document.getElementById("PageLabel").style.display = "none";
        document.getElementById("PageValue").style.display = "none";
        document.getElementById("XaxisLabel").style.display = "";
        document.getElementById("XaxisValue").style.display = "";
        document.getElementById("YaxisLabel").style.display = "";
        document.getElementById("YaxisValue").style.display = "";
        if ((type == "BarChart-Withseries") || (type == "LineChart")) {
            document.getElementById("CategoryaxisLabel").style.display = "";
            document.getElementById("CategoryaxisValue").style.display = "";
        }
        else {
            document.getElementById("CategoryaxisLabel").style.display = "none";
            document.getElementById("CategoryaxisValue").style.display = "none";
        }


    }
}
function DefaultDashboardPage()
{
    loadDefaultPage('/Dashboard/Dashboard');
}

function ActivityCascadeFromAMCSchedule() {
    return {
        sourceTransactionID: $("#SourceTransactionID").val()
    };
}
function onActivityStartDatePicker(e) {
    document.getElementById("ActivityEndDate").value = "";
}

function onActivityEndDatePicker(e) {

    var startDate = document.getElementById("ActivityStartDate").value;
    var endDate = document.getElementById("ActivityEndDate").value;
    var regExp = /(\d{1,2})\/(\d{1,2})\/(\d{2,4})/;
    if (parseInt(endDate.replace(regExp, "$3$2$1")) < parseInt(startDate.replace(regExp, "$3$2$1"))) {

        $.when(showErrorMessage("Select End Date Greater Than Start Date")
        ).done(function () {
            document.getElementById("ActivityEndDate").value = "";
        });
    }
}

function OnAMCScheduleChange() {
    debugger;
    $("#SourceTransactionScheduleID").data("kendoMultiColumnComboBox").value("");
    $("#AssetID").data("kendoMultiColumnComboBox").value("");
    $("#AssetID").data("kendoMultiColumnComboBox").dataSource.read();
    $("#SourceTransactionScheduleID").data("kendoMultiColumnComboBox").dataSource.read();
    UploadFileClear($("#CurrentPageID").val());
}
function AssetFilter() {
    return {
        sourceTransactionID: $("#SourceTransactionID").val()
    };
}


function UploadFileClear(cPageID) {
    debugger;
    var flag = true;
   
    if (flag) {
        var data = {
            currentPageID: cPageID,
        }
        Sever_PostData("/AssetMaintenance/ImportRemove", data, UploadFileClear_Success, UploadFileClear_Failure);
    }
    else {
        return flag;
    }

}
function UploadFileClear_Success(data) {
    $("#GridLineItemDetailGrid").data("kendoGrid").dataSource.read();
    $("#loadingmaskApproval").css("display", "none");
    $(".k-upload-files.k-reset").remove();
    e.preventDefault();
    return;
}
function UploadFileClear_Failure(data) {
    showErrorMessage(data.responseText);
}

