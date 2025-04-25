function OnPOLineItemChange(e) {
    $.ajax({
        contentType: 'application/json',
        type: 'GET',
        async: false,
        url: '/DataService/GetAssetPOType',
        data: {
            poLineItemID: $("#PO_LINE_ID").val()
        },
        success: function (data) {
            if (data.Success == true) {
                debugger;
                $("#assetTypeHidden").val(data.assetType);

                var iNum = parseInt(data.qty);// - parseInt(data.generateQty)

                $("#Quantity").data('kendoNumericTextBox').value(iNum);
                $("#POQuantity").val(iNum);
                $("#POQty").val(iNum);
                $("#ProductName").val(data.prodName);
                $("#PurchaseOrderID").val(data.purchaseOrderID);
                //$("#DetailsGrid").data('kendoGrid').dataSource.read();
            }
        },
        error: function () {
            alert("Asset Type is Empty");
        }
    });
}
function filterProducts(e) {
    return {
        poNumber: $("#PO_HEADER_ID").val(),
        text: $("#PO_LINE_ID").data("kendoMultiColumnComboBox").input.val()
    };
}
function OnPOChange(e) {
    $("#CategoryID").val("");
    $("#CategoryName").val("");
    $("#ProductName").val('');
    $("#POQuantity").val('');
    $("#POQty").val('');
    $("#Quantity").data('kendoNumericTextBox').value('');
    $("#UnitCost").data('kendoNumericTextBox').value('');
    $("#ID").val('');
    $("#PurchaseOrderID").val('');

}
function OnPOFromAssetCategoryChanges(text) {
    var flag = true;

    if (!$("#PO_LINE_ID").val()) {
        flag = false;
        if ($("#validatePOLine").val() != "") {
            $("#errpoLine").after('<span style="color:red;font-weight:normal;" id="validatePOLine">Select PO Line Number is Required</span>');
        }
    }
    else {
        $("#validatePOLine").remove();
    }

    if (flag) {
        opAssetGenerateFromPOPopupOpenWindow(text);
    }
}
function opAssetGenerateFromPOPopupOpenWindow(title, row) {
    var id = $("#assetTypeHidden").val();

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
function hiddenvalues(id, name) {
    $("#" + name).val(id);
}
function ongroupBoxchangeevent(textbox, ID) {
    if (!textbox) {
        hiddenvalues(textbox, ID);
    }
}
function LoadPOGenerateDelete(id, ponumber, polineno, productName) {
    var obj = {
        categoryID: id,
        currentPageID: $("#CurrentPageID").val(),
        PoNumber: ponumber,
        POLineNumber: polineno,
        ProductName: productName
    }
    Sever_PostData("/GenerateAssetFromPO/DeleteModelData", obj, ItemRequest_SubmitItemDetails_Success, IssueDetails_SubmitItemDetails_Failure);
    // $.ajax({
    //     contentType: 'application/json',
    //     type: 'GET',
    //     async: false,
    //     url: '/GenerateAssetFromPO/DeleteModelData',
    //     data: {
    //         id: id,
    //         currentPageID: $("#CurrentPageID").val()
    //     },
    //     success: function (data) {
    //         if (data.Success == true) {
    //             $("#POQuantity").val(data.poQty);
    //             $("#POQty").val(data.poQty);

    //             $("#DetailsGrid").data('kendoGrid').dataSource.read();
    //         }
    //     },
    //     error: function () {
    //         KendoErrorMsg("Category Not Found");
    //     }

    // });
}

function onItemCreateform() {

    $("#PoNumber").val($("#PO_HEADER_ID").data("kendoMultiColumnComboBox").text());
    $("#POLineNumber").val($("#PO_LINE_ID").data("kendoMultiColumnComboBox").text());
    var fag = true;
    if (!$("#CategoryID").val()) {
        fag = false;
        if ($("#validateCategory").val() != "") {
            $("#errCategory").after('<span style="color:red;font-weight:normal;" id="validateCategory">CategoryName is Required</span>');
        }
    }
    else {
        $("#validateCategory").remove();
    }
    var qty = $("#Quantity").data("kendoNumericTextBox").value();
    if (qty == null || qty <= 0) {
        fag = false;
        if ($("#validateQuantity").val() != "") {
            $("#errQuantity").after('<span style="color:red;font-weight:normal;" id="validateQuantity">Quantity is Required</span>');
        }
    }
    else {
        $("#validateQuantity").remove();
    }
    var qty = $("#UnitCost").data("kendoNumericTextBox").value();
    if (qty == null || qty <= 0) {
        fag = false;
        if ($("#validateunity").val() != "") {
            $("#errunitCost").after('<span style="color:red;font-weight:normal;" id="validateunity">UnitCost is Required</span>');
        }
    }
    else {
        $("#validateunity").remove();
    }

    if ('True' == 'False') {
        if (parseInt($("#Quantity").data('kendoNumericTextBox').value()) > parseInt(document.getElementById("POQuantity").value)) {
            fag = false;
            if ($("#validateQuantity").val() != "") {
                $("#errQuantity").after('<span style="color:red;font-weight:normal;" id="validateQuantity">Quantity should not be greater than PO Qunatity</span>');
            }
        }
        else {
            $("#validateQuantity").remove();
        }


    }
    if (fag) {
        //var obj = $("#form0").serialize();
        var obj = {
            id: $("#LineItemIdentityID").val(),
            CategoryID: $("#CategoryID").val(),
            CategoryName: $("#CategoryName").val(),
            ProductName: $("#ProductName").val(),
            POQuantity: parseInt(document.getElementById("POQuantity").value) - parseInt($("#Quantity").data('kendoNumericTextBox').value()),
            Quantity: parseInt($("#Quantity").data('kendoNumericTextBox').value()),
            DepartmentID: $("#DepartmentID").data("kendoMultiColumnComboBox").input.val(),
            DepartmentName: $("#DepartmentID").data("kendoMultiColumnComboBox").text(),
                UnitCost: $("#UnitCost").data("kendoNumericTextBox").value(),
            PoNumber: $("#PoNumber").val(),
            POLineNumber: $("#POLineNumber").val(),
            PO_HEADER_ID: $("#PO_HEADER_ID").val(),
            PO_LINE_ID: $("#PO_LINE_ID").val(),
            currentPageID: $("#CurrentPageID").val(),
            PurchaseOrderID: $("#PurchaseOrderID").val()

        }

        Sever_PostData("/GenerateAssetFromPO/AddLineItems", obj, ItemRequest_SubmitItemDetails_Success, IssueDetails_SubmitItemDetails_Failure);
    }

}
function Sever_PostData(url, data, successCallBack, failureCallBack) {
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        success: function (responce) {
            if (successCallBack != null) successCallBack(responce);
        },
        failure: function (responce) {
            if (failureCallBack != null)
                failureCallBack(responce);
            else
                alert(response.responseText);
        },
        error: function (responce) {
            if (failureCallBack != null)
                failureCallBack(responce);
            else
                alert(response.responseText);
        }
    });
}
function ItemRequest_SubmitItemDetails_Success(data) {
    if (data != '') {
    }
    else {
        $("#CategoryID").val("");
        $("#CategoryName").val("");
        $("#UnitCost").data('kendoNumericTextBox').value('');
        var currentqty = parseInt(document.getElementById("POQuantity").value) - parseInt($("#Quantity").data('kendoNumericTextBox').value());
        $("#POQuantity").val(currentqty);
        $("#POQty").val(currentqty);
        $("#ID").val('');
    }
    setInitialFocus("CategoryName");
    $("#DetailsGrid").data('kendoGrid').dataSource.read();
}
function IssueDetails_SubmitItemDetails_Failure(data) {

    setInitialFocus("CategoryName");
    $('#availableStock').html('');
}
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
