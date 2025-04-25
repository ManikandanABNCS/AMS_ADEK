var reportPageCurrentRowCount = 0;
function initReportPage() {
    reportPageCurrentRowCount = 0;
}

function AdjustLogicalOperatorsCombo() {
    var rowIndex = $('#dynamicSearchFieldTable').children('tbody').children('tr:first').attr("rowIndex");
    if ($("#LogicalOperator_" + rowIndex) != null)
        $("#LogicalOperator_" + rowIndex).remove();
}

function RemoveFilterRow(rowIndex) {
    $("#filterRow_" + rowIndex).remove();

    AdjustLogicalOperatorsCombo();
}

function addDynamicFieldClicked() {
    //add the selected field into search list
    var selectedFieldID = $("#dynamicFieldList").val();
    if (isNaN(parseInt(selectedFieldID))) {
        showErrorMessage("Please select a field");
        setInitialFocus("dynamicFieldList");

        return;
    }

    addFilterRow(selectedFieldID);
}

function addFilterRowContent(data) {
    reportPageCurrentRowCount++;

    var c = '/';
    data = data.replace("< AA script>", "<" + c + "script>");
    $('#dynamicSearchFieldTable tbody').append('<tr rowIndex="' + reportPageCurrentRowCount + '" id="filterRow_' + reportPageCurrentRowCount + '">' + data + '</tr>');
    AdjustLogicalOperatorsCombo();
}

function addFilterRow(selectedFieldID) {
    reportPageCurrentRowCount++;

    var prm = { dynamicFieldID: selectedFieldID, rowCount: reportPageCurrentRowCount };
    var url = "/ShowReport/DynamicFieldControls?" + $.param(prm);
    //var url = "";

    var jqxhr = $.ajax(url)
        .done(function (data) {
            $('#dynamicSearchFieldTable tbody').append('<tr rowIndex="' + reportPageCurrentRowCount + '" id="filterRow_' + reportPageCurrentRowCount + '">' + data + '</tr>');
            AdjustLogicalOperatorsCombo();
        })
        .fail(function () {
            alert("Unknown error occurred, please try again later.");
        })
        .always(function () {
            //alert("complete");
        });
}

function fnRelationalOperator(obj) {
    var id = obj.id.substring(obj.id.indexOf("_") + 1, obj.id.length);
    if (obj.value == "Isempty") {
        $("#DynamicFieldValue_" + id).val("");
        $("#DynamicFieldValue_" + id).prop("disabled", "disabled");
    }
    else {
        $("#DynamicFieldValue_" + id).removeAttr("disabled");
    }
}

/***
 * Report Template screen related scripts
 * **/
/**
 * @param {any} screen
 * @param {any} queryObjectName
 * @param {any} queryObjectType
 * @param {any} currentPageID
 * @param {any} successAction
 */
function ReportTemplate_QueryObjectChanged(screen, queryObjectName, queryObjectType, currentPageID, successAction) {
    var prm = { screen: screen, queryName: queryObjectName, queryType: queryObjectType, currentPageID: currentPageID };
    var url = "/ReportTemplate/LoadQueryObjectFields?" + $.param(prm);

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
function ReportTemplate_loadLineItemsPage(pageNumber, saveData) {
    currentPageID = GetCurrentPageID();
    var params = {
        screen: "SHOWDATA", //data already loaded only show the details

        currentPageID: currentPageID,
        pageNumber: pageNumber
    };

    var url = '/ReportTemplate/ReportTemplateFields?' + jQuery.param(params);
    //var url = '/ASN/POLineItems?poNumber=' + poNumber + '&currentPageID=' + currentPageID + '&pageNumber=' + pageNumber;
    loadContentPageToControl(url, "ReportFieldGrid", false);
}

function ReportTemplate_loadFilterLineItemsPage(pageNumber, saveData) {
    currentPageID = GetCurrentPageID();
    var params = {
        screen: "SHOWDATA", //data already loaded only show the details

        currentPageID: currentPageID,
        pageNumber: pageNumber
    };

    var url = '/ReportTemplate/ReportTemplateFilterFields?' + jQuery.param(params);
    //var url = '/ASN/POLineItems?poNumber=' + poNumber + '&currentPageID=' + currentPageID + '&pageNumber=' + pageNumber;
    loadContentPageToControl(url, "ReportFilterFieldGrid", false);
}