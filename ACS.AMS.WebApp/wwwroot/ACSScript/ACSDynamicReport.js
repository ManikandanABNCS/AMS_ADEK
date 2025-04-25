//Dynamic Report 
function ChangeReportName(e) {
    var availableList = $('#lstAvailableGroupFields');
    var availablefieldList = $('#lstAvailableFields');
    $.get("/DataService/GetAvaliblityReportcolumns?reportTemplateID=" + $('#ReportTemplateID').data("kendoMultiColumnComboBox").value(), function (response) {
        var result = response;//$.parseJSON(response);

        var listItems = [];
        var listedSelectedItem = [];
        var available = result.availableFields;
        var avaliableDisplayField = result.availableReportFields;
        if (available.length > 0) {
            for (var key in available) {
                listItems.push('<option value="' +
                    available[key].Value + '">' + available[key].Text
                    + '</option>');
                listedSelectedItem.push('<option value="' +
                    avaliableDisplayField[key].Value + '">' + avaliableDisplayField[key].Text
                    + '</option>');
            }
        }

        //remove all items from listbox
        $('#lstAvailableGroupFields option').each(function (i, option) { $(option).remove(); });
        //groupComboBox.remove();
        availableList.append(listItems.join(''));
        $('#lstSelectedGroupFields option').each(function (i, option) { $(option).remove(); });

        //remove all items from listbox
        $('#lstAvailableFields option').each(function (i, option) { $(option).remove(); });
        //groupComboBox.remove();
        availablefieldList.append(listedSelectedItem.join(''));
        $('#lstSelectedFields option').each(function (i, option) { $(option).remove(); });

    });

}


function ChangeReportFieldName(e) {
    var availableList = $('#lstAvailableGroupFields');
    var availablefieldList = $('#lstAvailableFields');
    $.get("/DataService/GetAvaliblityReportFieldcolumns?reportTemplateID=" + $('#ReportTemplateID').val(), function (response) {
        var result = $.parseJSON(response);

        var listItems = [];
        var listedSelectedItem = [];
        var available = result.availableFields;
        var avaliableDisplayField = result.availableFields;
        if (available.length > 0) {
            for (var key in available) {
                listItems.push('<option value="' +
                    available[key].Value + '">' + available[key].Text
                    + '</option>');
                listedSelectedItem.push('<option value="' +
                    avaliableDisplayField[key].Value + '">' + avaliableDisplayField[key].Text
                    + '</option>');
            }
        }

        //remove all items from listbox
        //$('#lstAvailableGroupFields option').each(function (i, option) { $(option).remove(); });
        ////groupComboBox.remove();
        //availableList.append(listItems.join(''));
        //$('#lstSelectedGroupFields option').each(function (i, option) { $(option).remove(); });

        //remove all items from listbox
        $('#lstAvailableFields option').each(function (i, option) { $(option).remove(); });
        //groupComboBox.remove();
        availablefieldList.append(listedSelectedItem.join(''));
        $('#lstSelectedFields option').each(function (i, option) { $(option).remove(); });
    });
}

function addGroupTemplate() {
    debugger
    var ctrl = document.getElementById("lstAvailableGroupFields");

    for (i = 0; i < ctrl.length; i++) {
        if (ctrl[i].selected) {
            document.getElementById("lstSelectedGroupFields").appendChild(document.getElementById("lstAvailableGroupFields").options.item(i));
            i--;
        }
    }
}

//type=> asc/desc
function addOrderbyTemplate(orderby) {

    var ctrl = $("#lstAvailableOrderbyFields").find(":selected");

    $.each(ctrl, function (i, data) {
        //var option = document.getElementById("lstAvailableOrderbyFields").options.item(i);
        var option = data;
        var selVal = option.value;
        var SelText = option.text + "-" + orderby;
        //var optionChild = "<option value=\"" + selVal + "\" Orderby=\"" + orderby + "\" > " + SelTest + "</option>";
        var optionChild = new Option(SelText, selVal);
        document.getElementById("lstSelectedOrderbyFields").appendChild(optionChild);
        //i++;

        //Remove from AvailableList
        document.getElementById("lstAvailableOrderbyFields").removeChild(option);

    });
}

function delOrderbyTemplate() {

    var ctrl = $("#lstSelectedOrderbyFields").find(":selected");

    $.each(ctrl, function (i, data) {

        //var option = document.getElementById("lstSelectedOrderbyFields").options.item(i);
        var option = data;
        var selVal = option.value;
        var SelText = option.text;
        if (option.text.indexOf("-") != -1) {
            SelText = option.text.split("-")[0]
        }
        //var optionChild = "<option value=\"" + selVal + "\" Orderby=\"" + orderby + "\" > " + SelTest + "</option>";
        var optionChild = new Option(SelText, selVal);
        document.getElementById("lstAvailableOrderbyFields").appendChild(optionChild);

        //document.getElementById("lstAvailableGroupFields").appendChild(document.getElementById("lstSelectedGroupFields").options.item(i));
        // i--;        
        //Remove from AvailableList
        document.getElementById("lstSelectedOrderbyFields").removeChild(option);
    });
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

$('#lstSelectedFields').on('change', function () {
    
    var dat = [];
    var ctrl = document.getElementById("lstSelectedFields");
    for (i = 0; i < ctrl.length; i++) {
        if (ctrl[i].selected) {
            dat = (ctrl.options[i].value).toString().split("@");
            if (dat[1] == "System.String" || dat[1] == "System.DateTime") {
                $('#hide_Sum').hide();
                break;
            }
            else {
                $('#hide_Sum').show();
                if (dat.length > 4) {
                    if (dat[5]) {
                        $('#isSum').attr("checked", "checked");
                    }
                    else {
                        $('#isSum').removeAttr("checked", "checked");
                    }
                }
            }
        }
    }

    return false;
});

var objarray = [];
$('#width_Textbox').on('blur', function () {
    
    var ctrl = document.getElementById("lstSelectedFields");
    var dat = [];
    for (i = 0; i < ctrl.length; i++) {
        if (ctrl[i].selected) {
            //ids.push(ctrl.options[i].value);
            dat = (ctrl.options[i].value).toString().split("@");
            var reportModel = {};
            reportModel.ColumnID = dat[0];
            reportModel.Type = dat[1];
            reportModel.width = $('#width_Textbox').val() == "" ? null : $('#width_Textbox').val();
            reportModel.isSumField = $('#isSum').is(':checked') ? true : false;
            //reportModel.displayOrder = dat[2];
            reportModel.ColumName = dat[3];
            reportModel.keyVal = dat[0] + "@" + dat[1] + "@" + dat[2] + "@" + dat[3];
            checkObjArray(dat[0] + "@" + dat[1] + "@" + dat[2] + "@" + dat[3], reportModel);
        }
    }
});

$('#isSum').on('change', function () {
    var ctrl = document.getElementById("lstSelectedFields");
    var dat = [];
    for (i = 0; i < ctrl.length; i++) {
        if (ctrl[i].selected) {
            //ids.push(ctrl.options[i].value);
            dat = (ctrl.options[i].value).toString().split("@");
            var reportModel = {};
            reportModel.ColumnID = dat[0];
            reportModel.Type = dat[1];
            reportModel.width = $('#width_Textbox').val() == "" ? null : $('#width_Textbox').val();
            reportModel.isSumField = $('#isSum').is(':checked') ? true : false;
            //reportModel.displayOrder = dat[2];
            reportModel.ColumName = dat[3];
            reportModel.keyVal = dat[0] + "@" + dat[1] + "@" + dat[2] + "@" + dat[3];
            checkObjArray(dat[0] + "@" + dat[1] + "@" + dat[2] + "@" + dat[3], reportModel);
        }
    }
});

function onkeyPressWidthTextBox() {
    var str = $('#width_Textbox').val();
    str = str.replace(/[^0-9\.]/g, '');
    $('#width_Textbox').val(str);
    var len = str.split('.');
    if (len.length > 2) {
        str = str.replace(/\.+$/, "");
        $('#width_Textbox').val(str);
        return false;
    }
    if (len[1].length > 2) {
        str = str.substring(0, str.length - 1);
        $('#width_Textbox').val(str);
        return false;
    }

    var ctrl = document.getElementById("lstSelectedFields");
    if (ctrl.length > 0) {

    }
    else {
        alert("Add Fields to Selected Fields,Select and Enter Width");
        return false;
    }
    var count = $("#lstSelectedFields :selected").length;
    if (count == 0 || count == -1) {
        alert(" Select Fields and Enter Width");
        return false;
    }
}

function checkObjArray(value, desc) {
    for (var i in objarray) {
        if (objarray[i].keyVal == desc.keyVal) {
            objarray[i] = desc;
            break; //Stop this loop, we found it!
        }
    }
}

function spliceObjArray(value) {
    
    for (var i in objarray) {
        if (objarray[i].keyVal == value) {
            objarray.splice(i, 1);
            break; //Stop this loop, we found it!
        }
    }
}


function getArrayData(value) {
   
    for (var i in objarray) {
        if (objarray[i].keyVal == value) {
            return objarray[i];
            //break; //Stop this loop, we found it!
        }
    }
}


function addReportFieldsTemplate(lstAvalibleField, lstSelectedField) {

    
    var ctrl = document.getElementById(lstAvalibleField);
    var ids = [];
    for (i = 0; i < ctrl.length; i++) {
        if (ctrl[i].selected) {
            dat = (ctrl.options[i].value).toString().split("@");
            var reportModel = {};
            reportModel.ColumnID = dat[0];
            reportModel.Type = dat[1];
            reportModel.width = $('#width_Textbox').val() == "" ? null : $('#width_Textbox').val();
            reportModel.isSumField = $('#isSum').is(':checked') ? true : false;
            //reportModel.displayOrder = dat[2];
            reportModel.ColumName = dat[3];
            reportModel.keyVal = ctrl.options[i].value;
            objarray.push(reportModel);
            document.getElementById(lstSelectedField).appendChild(document.getElementById(lstAvalibleField).options.item(i));
            ids.push(reportModel.keyVal);
            document.getElementById('hdReportConcatanatedVals').value = ctrl.options[i].value + ",";
            i--;
        }

    }

}
function delReportFieldTemplate(lstAvalibleField, lstSelectedField) {
    var ctrl = document.getElementById(lstSelectedField);
    for (i = 0; i < ctrl.length; i++) {
        if (ctrl[i].selected) {
            dat = (ctrl.options[i].value).toString().split("@");
            spliceObjArray(dat[0] + "@" + dat[1] + "@" + dat[2] + "@" + dat[3]);
            document.getElementById(lstAvalibleField).appendChild(document.getElementById(lstSelectedField).options.item(i));
            var delVal = document.getElementById('hdReportConcatanatedVals').value;
            delVal.replace(ctrl.options[i].value + ",", "");
            i--;
        }
    }
}

function addToReportColumnModel() {
    debugger
    var objarrayEdit = [];
    var ctrl = document.getElementById("lstSelectedFields");
    var ids = [];
    var dat = [];
    if (ctrl) {
        for (i = 0; i < ctrl.length; i++) {
            dat = (ctrl.options[i].value).toString().split("@");
            var reportModelEdit = {};
            var dats = getArrayData(dat[0] + "@" + dat[1] + "@" + dat[2] + "@" + dat[3]);
            reportModelEdit.ColumnID = dats.ColumnID;
            reportModelEdit.Type = dats.Type;
            reportModelEdit.width = dats.width;
            reportModelEdit.isSumField = dats.isSumField;
            reportModelEdit.ColumName = dats.ColumName;
            reportModelEdit.keyVal = dat[0] + "@" + dat[1] + "@" + dat[2] + "@" + dat[3];
            objarrayEdit.push(reportModelEdit);
        }
    }
    $.ajax({
        contentType: 'application/json',
        type: 'POST',
        async: false,
        url: '/ReportsTemplate/AddDataToReportColumnModel',
        data: JSON.stringify(objarrayEdit),
        success: function (data) {
            objarray.length = 0;
            return true;
        },
        error: function () {
            return false;
        }
    });
}


$(document).ready(function () {
    objarray.length = 0;
    var ctrl = document.getElementById("lstSelectedFields");
    var ids = [];
    var dat = [];
    if (ctrl) {
        for (i = 0; i < ctrl.length; i++) {
            dat = (ctrl.options[i].value).toString().split("@");
            var reportModel = {};
            reportModel.ColumnID = dat[0];
            reportModel.Type = dat[1];
            reportModel.width = dat[4];
            reportModel.isSumField = dat[5]
            reportModel.displayOrder = dat[2];
            reportModel.ColumName = dat[3];
            reportModel.keyVal = dat[0] + "@" + dat[1] + "@" + dat[2] + "@" + dat[3];
            objarray.push(reportModel);
        }
    }
});


function selectedItems() {

    var ctrl = document.getElementById("lstSelectedField");
    var ids = [];
    for (i = 0; i < ctrl.length; i++) {
        if (ctrl[i].selected) {
            ids.push(ctrl[i].selected);
        }
    }
    alert(ids);
}
function delGroupTemplate() {
    debugger
    var ctrl = document.getElementById("lstSelectedGroupFields");

    for (i = 0; i < ctrl.length; i++) {
        if (ctrl[i].selected) {
            document.getElementById("lstAvailableGroupFields").appendChild(document.getElementById("lstSelectedGroupFields").options.item(i));
            i--;
        }
    }
}
function delFieldTemplate(lstAvalibleField, lstSelectedField) {
    var ctrl = document.getElementById(lstSelectedField);

    for (i = 0; i < ctrl.length; i++) {
        if (ctrl[i].selected) {
            document.getElementById(lstAvalibleField).appendChild(document.getElementById(lstSelectedField).options.item(i));
            i--;
        }
    }
}
function OnGridReportColumnChange(e) {

    if (e.action == "itemchange") {

        var grid = $("#Grid").data("kendoGrid");
        var data = grid.dataSource.data();
        $.each(data, function (i, row) {

            if (e.field == "ColumName") {
                if (row.ColumName != null) {
                    $.ajax({
                        url: "/ReportsTemplate/GetColumnDetails/",
                        dataType: "json",
                        cache: false,
                        async: false,
                        type: 'GET',
                        data: { reportTemplateID: $("#ReportTemplateID").val(), columnName: row.ColumName },
                        success: function (result) {
                            if (result.Success) {

                                grid.dataSource.data()[i]["Width"] = result.width;
                                grid.dataSource.data()[i]["ColumnID"] = result.columnID;
                                grid.dataSource.data()[i]["Type"] = result.dataType;

                                if ($('#Width').val() == "") {
                                    $('#Width').val(result.width);
                                    $('#Width').click();
                                    $('#ColumnID').val(result.columnID);
                                    $('#Type').val(result.dataType);
                                    if (result.dataType != "System.Int32") {
                                        grid.dataSource.data()[i]["isSumField"] = false;
                                        $('#isSumField').attr("checked", false);
                                        $('#isSumField').attr('disabled', true);
                                    }
                                    else if (result.dataType == "System.Int32") {
                                        grid.dataSource.data()[i]["isSumField"] = false;
                                        $('#isSumField').attr("checked", false);
                                        $('#isSumField').attr('disabled', false);
                                    }
                                }
                                e.preventDefault();
                                return false;

                            }
                            else {
                                alert(result.FailureMessage);

                            }
                        }
                    });
                }
            }
        });
    }
}

$("#Width").click(function () {
    alert("Handler for .change() called.");
});
function onReportError(e, status) {

    if (e.errors) {
        var message = "Error:\n";

        var grid = $('#Grid').data('kendoGrid');
        var gridElement = grid.editable.element;

        var validationMessageTemplate = kendo.template(
            "<div id='#=field#_validationMessage' " +
            "class='k-widget k-tooltip k-tooltip-validation " +
            "k-invalid-msg field-validation-error' " +
            "style='margin: 0.5em;' data-for='#=field#' " +
            "data-val-msg-for='#=field#' role='alert'>" +
            "<span class='k-icon k-warning'></span>" +
            "#=message#" +
            "<div class='k-callout k-callout-n'></div>" +
            "</div>");

        $.each(e.errors, function (key, value) {
            if (value.errors) {
                gridElement.find("[data-vaAMSg-for=" + key + "],[data-val-msg-for=" + key + "]")
                    .replaceWith(validationMessageTemplate({ field: key, message: value.errors[0] }));
                gridElement.find("input[name=" + key + "]").focus();
            }
        });
        grid.one("dataBinding", function (e) {
            e.preventDefault();   // cancel grid rebind
        });
    }
}

function Listbox_Move(ListID, direction) {
    var listbox = document.getElementById(ListID);
    var selIndex = listbox.selectedIndex;
    if (-1 == selIndex) {
        $.when(kendo.ui.ExtAlertDialog.show({
            message: "Please Select any options to move ",
            icon: "k-ext-warning"
        })
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
