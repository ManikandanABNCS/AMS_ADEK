using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACS.AMS.WebApp.Models
{
    public static class MultiColumnViewModels
    {
        #region Supplier Model

        public static MultiColumnViewModel GetSupplierModel(string fieldName = "SupplierCode", object val = null, string placeholderText = "<--Select Supplier-->")
        {
            return new MultiColumnViewModel(fieldName, val)
            {
                ValueFieldName = "SupplierCode",
                PlaceholderText = placeholderText,
                DataReadActionName = "GetSuppliersForMCombobox",
                DataReadControllerName = "DataService",
                DataReadScriptFunctionName = null,
                ParentFieldName = null,
                RouteValues = null,

                ChangeScriptFunctionName = "view_Supplier_Changed",
                SelectScriptFunctionName = "view_Supplier_Selected"
            };
        }

        public static MultiColumnViewModel GetSupplierModel_Report(string fieldName = "SupplierCode", object val = null, string placeholderText = "<--ALl Suppliers-->")
        {
            return GetSupplierModel(fieldName, val, placeholderText);
        }

        #endregion

        #region Stock Take Model        

        public static MultiColumnViewModel GetStockCodeSelectionModel(string fieldName = "StockTakeID", object val = null, string placeholderText = "<--Select Stocktake-->")
        {
            return new MultiColumnViewModel(fieldName, val)
            {
                ValueFieldName = "StockTakeID",
                PlaceholderText = placeholderText,
                DataReadActionName = "GetStockTakeCodesForMCombobox",
                DataReadControllerName = "DataService",
                DataReadScriptFunctionName = null,
                ParentFieldName = null,
                RouteValues = null,

                ChangeScriptFunctionName = "view_StockTake_Changed",
                SelectScriptFunctionName = "view_StockTake_Selected"
            };
        }

        public static MultiColumnViewModel GetStockCodeSelectionModel_Report(string fieldName = "StockTakeID", object val = null, 
            string placeholderText = "<--ALl Stocktake-->")
        {
            return GetSupplierModel(fieldName, val, placeholderText);
        }

        #endregion

        #region Product Model

        public static MultiColumnViewModel GetProductModel(string fieldName = "ProductID", object val = null, string placeholderText = "<--Select Product-->")
        {
            return new MultiColumnViewModel(fieldName, val)
            {
                ValueFieldName = "ProductID",
                PlaceholderText = placeholderText,
                DataReadActionName = "GetProductsForMCombobox",
                DataReadControllerName = "DataService",
                DataReadScriptFunctionName = null,
                ParentFieldName = null,
                RouteValues = null,

                ChangeScriptFunctionName = "view_Product_Changed",
                SelectScriptFunctionName = "view_Product_Selected"
            };
        }

        public static MultiColumnViewModel GetProductModel_Report(string fieldName = "ProductID", object val = null, string placeholderText = "<--All Product-->")
        {
            return GetProductModel(fieldName, val, placeholderText);
        }

        #endregion

        #region Uniform Item Model

        public static MultiColumnViewModel GetUniformItemModel(string fieldName = "UniformItemID", object val = null, string placeholderText = "Select Uniform Item")
        {
            return new MultiColumnViewModel(fieldName, val)
            {
                ValueFieldName = "UniformItemID",
                PlaceholderText = placeholderText,
                DataReadActionName = "GetUniformItemsForMCombobox",
                DataReadControllerName = "DataService",
                DataReadScriptFunctionName = null,
                ParentFieldName = null,
                RouteValues = null,

                ChangeScriptFunctionName = "view_UniformItem_Changed",
                SelectScriptFunctionName = "view_UniformItem_Selected"
            };
        }

        public static MultiColumnViewModel GetUniformItemModel_Report(string fieldName = "UniformItemID", object val = null, string placeholderText = "All Uniform Items")
        {
            return GetUniformItemModel(fieldName, val, placeholderText);
        }

        #endregion


        #region Uniform Item Model

        public static MultiColumnViewModel GetMiscellaneousUniformItemModel(string fieldName = "UniformItemID", object val = null, string placeholderText = "Select Uniform Item")
        {
            return new MultiColumnViewModel(fieldName, val)
            {
                ValueFieldName = "UniformItemID",
                PlaceholderText = placeholderText,
                DataReadActionName = "GetMiscellaneousUniformItemsForMCombobox",
                DataReadControllerName = "DataService",
                DataReadScriptFunctionName = null,
                ParentFieldName = null,
                RouteValues = null,

                ChangeScriptFunctionName = "view_UniformItem_Changed",
                SelectScriptFunctionName = "view_UniformItem_Selected"
            };
        }

        public static MultiColumnViewModel GetMiscellaneousUniformItemModel_Report(string fieldName = "UniformItemID", object val = null, string placeholderText = "All Uniform Items")
        {
            return GetUniformItemModel(fieldName, val, placeholderText);
        }

        #endregion

        #region Uniform Set Model

        public static MultiColumnViewModel GetUniformSetModel(string fieldName = "UniformSetID", object val = null, string placeholderText = "Select Uniform Set")
        {
            return new MultiColumnViewModel(fieldName, val)
            {
                ValueFieldName = "UniformSetID",
                PlaceholderText = placeholderText,
                DataReadActionName = "GetUniformSetsForMCombobox",
                DataReadControllerName = "DataService",
                DataReadScriptFunctionName = null,
                ParentFieldName = null,
                RouteValues = null,

                ChangeScriptFunctionName = "view_UniformSet_Changed",
                SelectScriptFunctionName = "view_UniformSet_Selected"
            };
        }

        public static MultiColumnViewModel GetUniformSetModel_Report(string fieldName = "UniformSetID", object val = null, string placeholderText = "All Uniform Sets")
        {
            return GetUniformSetModel(fieldName, val, placeholderText);
        }

        #endregion

        #region Employee Model

        public static MultiColumnViewModel GetEmployeeModel(string fieldName = "PersonID", object val = null, string placeholderText = "Select Employee")
        {
            return new MultiColumnViewModel(fieldName, val)
            {
                ValueFieldName = "PersonID",
                PlaceholderText = placeholderText,
                DataReadActionName = "GetEmployeeForMCombobox",
                DataReadControllerName = "DataService",
                DataReadScriptFunctionName = null,
                ParentFieldName = null,
                RouteValues = null,

                ChangeScriptFunctionName = "view_Employee_Changed",
                SelectScriptFunctionName = "view_Employee_Selected"
            };
        }

        public static MultiColumnViewModel GetEmployeeModel_Report(string fieldName = "PersonID", object val = null, string placeholderText = "All Employee")
        {
            return GetEmployeeModel(fieldName, val, placeholderText);
        }

        #endregion

        #region UOM Model

        public static MultiColumnViewModel GetUOMModel(string fieldName = "UomID", object val = null, string placeholderText = "<--Select UOM-->")
        {
            return new MultiColumnViewModel(fieldName, val)
            {
                ValueFieldName = "UomID",
                PlaceholderText = placeholderText,
                DataReadActionName = "GetUomsForMCombobox",
                DataReadControllerName = "DataService",
                DataReadScriptFunctionName = null,
                ParentFieldName = null,
                RouteValues = null,

                ChangeScriptFunctionName = "view_Uom_Changed",
                SelectScriptFunctionName = "view_Uom_Selected"
            };
        }

        public static MultiColumnViewModel GetUomModel_Report(string fieldName = "UomID", object val = null, string placeholderText = "<--All UOM-->")
        {
            return GetUOMModel(fieldName, val, placeholderText);
        }

        #endregion

        #region Bins Model

        public static MultiColumnViewModel GetBinModel(string fieldName = "BinID", object val = null, string placeholderText = "<--Select Bin-->", string parentFieldName = "WarehouseCode")
        {
            return new MultiColumnViewModel(fieldName, val)
            {
                ValueFieldName = "BinID",
                PlaceholderText = placeholderText,
                DataReadActionName = "GetBinsForMCombobox",
                DataReadControllerName = "DataService",
                DataReadScriptFunctionName = "binAdditionalData",
                ParentFieldName = parentFieldName, //"WarehouseCode",

                ChangeScriptFunctionName = "view_Bin_Changed",
                SelectScriptFunctionName = "view_Bin_Selected",

                RouteValues = null
            };
        }

        public static MultiColumnViewModel GetBinModel_Report(string fieldName = "BinID", object val = null, string placeholderText = "<--All Bins-->", string parentFieldName = "WarehouseCode")
        {
            return GetBinModel(fieldName, val, placeholderText, parentFieldName);
        }

        #endregion

        #region Item Request Model

        public static MultiColumnViewModel GetItemRequestModel(string fieldName = "ItemRequestID", object val = null, string placeholderText = "<--Select Request-->", string parentFieldName = "")
        {
            return new MultiColumnViewModel(fieldName, val)
            {
                ValueFieldName = "ItemRequestID",
                PlaceholderText = placeholderText,
                DataReadActionName = "GetItemRequestForMultiComboBox",
                DataReadControllerName = "DataService",
                DataReadScriptFunctionName = "",
                ParentFieldName = parentFieldName, //"WarehouseCode",

                ChangeScriptFunctionName = "view_Bin_Changed",
                SelectScriptFunctionName = "view_Bin_Selected",

                RouteValues = null
            };
        }

        public static MultiColumnViewModel GetItemRequestModel_Report(string fieldName = "ItemRequestID", object val = null, string placeholderText = "<--All Requests-->", string parentFieldName = "")
        {
            var model= GetItemRequestModel(fieldName, val, placeholderText, parentFieldName);
            model.ChangeScriptFunctionName = "view_Bin_Changed";
            model.SelectScriptFunctionName = "view_Bin_Selected";

            return model;
        }

        #endregion

        #region PickLsit Models

        public static MultiColumnViewModel GetPickListIDModel(string fieldName = "PickListID", object val = null, string placeholderText = "<--Select PickList-->", 
            string parentFieldName = "")
        {
            return new MultiColumnViewModel(fieldName, val)
            {
                ValueFieldName = "PickListID",
                PlaceholderText = placeholderText,
                DataReadActionName = "GetAllPickListNosForMCombobox",
                DataReadControllerName = "DataService",
                DataReadScriptFunctionName = "",
                ParentFieldName = parentFieldName, //"WarehouseCode",

                ChangeScriptFunctionName = "view_PickList_Changed",
                SelectScriptFunctionName = "view_PickList_Selected",

                RouteValues = null
            };
        }

        public static MultiColumnViewModel GetPickListIDModel_Report(string fieldName = "PickListID", object val = null, string placeholderText = "<--All PickLists-->", 
            string parentFieldName = "")
        {
            var m = GetPickListIDModel(fieldName, val, placeholderText, parentFieldName);

            m.ChangeScriptFunctionName = "";
            m.SelectScriptFunctionName = "";

            return m;
        }

        #endregion

        #region Warehouse Model

        public static MultiColumnViewModel GetWarehouseModel(string fieldName = "WarehouseCode", object val = null, string placeholderText = "<--Select Warehouse-->")
        {
            return new MultiColumnViewModel(fieldName, val)
            {
                ValueFieldName = "WarehouseCode",
                PlaceholderText = placeholderText,
                DataReadActionName = "GetWarehousesForMCombobox",
                DataReadControllerName = "DataService",
                DataReadScriptFunctionName = null,
                ParentFieldName = null,

                ChangeScriptFunctionName = "view_Warehouse_Changed",
                SelectScriptFunctionName = "view_Warehouse_Selected",

                RouteValues = null
            };
        }

        public static MultiColumnViewModel GetWarehouseModel_Report(string fieldName = "WarehouseCode", object val = null, string placeholderText = "<--All Warehouses-->")
        {
            return GetWarehouseModel(fieldName, val, placeholderText);
        }

        #endregion

        #region Warehouse Stock Model

        public static MultiColumnViewModel GetWarehouseStockModel(string fieldName = "WarehouseCode", object val = null, string placeholderText = "<--Select Warehouse-->")
        {
            return new MultiColumnViewModel(fieldName, val)
            {
                ValueFieldName = "WarehouseCode",
                PlaceholderText = placeholderText,
                DataReadActionName = "GetWarehousesWithItemStockForMCombobox",
                DataReadControllerName = "DataService",
                ParentFieldName = null,
                RouteValues = null,
                DataReadScriptFunctionName = "warehouseStockAdditionalData",

                ChangeScriptFunctionName = "view_Warehouse_Changed",
                SelectScriptFunctionName = "view_Warehouse_Selected",
            };
        }

        public static MultiColumnViewModel GetWarehouseStockModel_Report(string fieldName = "WarehouseCode", object val = null, string placeholderText = "<--All Warehouses-->")
        {
            return GetWarehouseStockModel(fieldName, val, placeholderText);
        }

        #endregion

        #region Employee Model

        public static MultiColumnViewModel GetWMSEmployeeModel(string fieldName = "EmployeeCode", object val = null, string placeholderText = "<--Select Employee-->")
        {
            return new MultiColumnViewModel(fieldName, val)
            {
                ValueFieldName = "EmployeeCode",
                PlaceholderText = placeholderText,
                DataReadActionName = "GetWMSEmployeesForMCombobox",
                DataReadControllerName = "DataService",
                DataReadScriptFunctionName = null,
                ParentFieldName = null,
                RouteValues = null,

                ChangeScriptFunctionName = "view_Employee_Changed",
                SelectScriptFunctionName = "view_Employee_Selected",
            };
        }

        public static MultiColumnViewModel GetWMSEmployeeModel_Report(string fieldName = "EmployeeCode", object val = null, string placeholderText = "<--All Employees-->")
        {
            return GetWMSEmployeeModel(fieldName, val, placeholderText);
        }

        #endregion

        #region Asset Model

        public static MultiColumnViewModel GetAssetModel(string fieldName = "AssetCode", object val = null, string placeholderText = "<--Select Asset-->")
        {
            return new MultiColumnViewModel(fieldName, val)
            {
                ValueFieldName = "AssetID",
                PlaceholderText = placeholderText,
                DataReadActionName = "GetAssetsForMCombobox",
                DataReadControllerName = "DataService",
                DataReadScriptFunctionName = null,
                ParentFieldName = null,
                RouteValues = null,

                ChangeScriptFunctionName = "view_Asset_Changed",
                SelectScriptFunctionName = "view_Asset_Selected"
            };
        }

        public static MultiColumnViewModel GetAssetModel_Report(string fieldName = "AssetCode", object val = null, string placeholderText = "<--All Assets-->")
        {
            return GetAssetModel(fieldName, val, placeholderText);
        }

        #endregion

        #region Asset Model

        public static MultiColumnViewModel GetAssetApprovalModel(string fieldName = "AssetCode", object val = null, string placeholderText = "<--Select Asset-->")
        {
            return new MultiColumnViewModel(fieldName, val)
            {
                ValueFieldName = "AssetApprovalID",
                PlaceholderText = placeholderText,
                DataReadActionName = "GetAssetApprovalsForMCombobox",
                DataReadControllerName = "DataService",
                DataReadScriptFunctionName = null,
                ParentFieldName = null,
                RouteValues = null,

                ChangeScriptFunctionName = "view_AssetApproval_Changed",
                SelectScriptFunctionName = "view_AssetApproval_Selected"
            };
        }

        public static MultiColumnViewModel GetAssetApprovalModel_Report(string fieldName = "AssetCode", object val = null, string placeholderText = "<--All Assets-->")
        {
            return GetAssetApprovalModel(fieldName, val, placeholderText);
        }

        #endregion

        #region Purchase Model

        public static MultiColumnViewModel GetPurchaseOrderModel(string fieldName = "PONumber", object val = null, string placeholderText = "<--Select PO-->")
        {
            return new MultiColumnViewModel(fieldName, val)
            {
                ValueFieldName = "PONumber",
                PlaceholderText = placeholderText,
                DataReadActionName = "GetPoNumberForMultiComboBox",
                DataReadControllerName = "DataService",
                DataReadScriptFunctionName = null,
                ParentFieldName = null,
                RouteValues = null,

                ChangeScriptFunctionName = "view_PO_Changed",
                SelectScriptFunctionName = "view_PO_Selected"
            };
        }

        public static MultiColumnViewModel GetPurchaseOrderModel_Report(string fieldName = "PONumber", object val = null, string placeholderText = "<--ALL POs-->")
        {
            return GetPurchaseOrderModel(fieldName, val, placeholderText);
        }

        #endregion

        #region User Model

        public static MultiColumnViewModel GetUserModel(string fieldName = "UserID", object val = null, string placeholderText = "<--Select User-->")
        {
            return new MultiColumnViewModel(fieldName, val)
            {
                ValueFieldName = "UserID",
                PlaceholderText = placeholderText,
                DataReadActionName = "GetUsersForMCombobox",
                DataReadControllerName = "DataService",
                DataReadScriptFunctionName = null,
                ParentFieldName = null,
                RouteValues = null,

                ChangeScriptFunctionName = "view_User_Changed",
                SelectScriptFunctionName = "view_User_Selected"
            };
        }

        public static MultiColumnViewModel GetUserModel_Report(string fieldName = "UserID", object val = null, string placeholderText = "<--ALL Users-->")
        {
            return GetSupplierModel(fieldName, val, placeholderText);
        }

        #endregion

        public static MultiColumnViewModel GetReturnPickListModel(string fieldName = "PickListAllocationID", object val = null, string placeholderText = "<--Select Picklist-->")
        {
            return new MultiColumnViewModel(fieldName, val)
            {
                ValueFieldName = "PickListAllocationID",
                PlaceholderText = placeholderText,
                DataReadActionName = "GetPickListsForReturnForMCombobox",
                DataReadControllerName = "DataService",
                DataReadScriptFunctionName = null,
                ParentFieldName = null,
                RouteValues = null,

                ChangeScriptFunctionName = "view_ReturnPickList_Changed",
                SelectScriptFunctionName = "view_ReturnPickList_Selected",
            };
        }

        public static MultiColumnViewModel GetReturnPickListModel_Report(string fieldName = "EmployeeCode", object val = null, string placeholderText = "<--All Employees-->")
        {
            return GetReturnPickListModel(fieldName, val, placeholderText);
        }

        //public static MultiColumnViewModel ReturnPickListModel
        //{
        //    get
        //    {
        //        return new MultiColumnViewModel("PickListAllocationID", "");
        //    }
        //}

        #region GRN Selection

        public static MultiColumnViewModel GetReturnGRNModel(string fieldName = "GRNID", object val = null, string placeholderText = "<--Select GRN-->")
        {
            return new MultiColumnViewModel(fieldName, val)
            {
                ValueFieldName = "GRNID",
                PlaceholderText = placeholderText,
                DataReadActionName = "GetGRNsForReturnForMCombobox",
                DataReadControllerName = "DataService",
                DataReadScriptFunctionName = null,
                ParentFieldName = null,
                RouteValues = null,

                ChangeScriptFunctionName = "view_ReturnGRN_Changed",
                SelectScriptFunctionName = "view_ReturnGRN_Selected"
            };
        }

        public static MultiColumnViewModel GetReturnGRNModel_Report(string fieldName = "GRNID", object val = null, string placeholderText = "<--ALL GRNs-->")
        {
            return GetSupplierModel(fieldName, val, placeholderText);
        }

        #endregion

        public static MultiColumnViewModel WMSPickListDocumentNoModel
        {
            get
            {
                return new MultiColumnViewModel("DocumentNo", "") { PlaceholderText = "Select Document", ValueFieldName = "DocumentNo" };
            }
        }

        public static MultiColumnViewModel WMSPickListMODocumentNoModel
        {
            get
            {
                return new MultiColumnViewModel("MONumber", "") { PlaceholderText = "Select MO Number", ValueFieldName = "MONumber" };
            }
        }

        public static MultiColumnViewModel WMSGPPickListNoModel
        {
            get
            {
                return new MultiColumnViewModel("GPPickListNo", "") { PlaceholderText = "Select GP Picklist No", ValueFieldName = "GPPickListNo" };
            }
        }
        
        /// <summary>
        /// Model for handling return screens warehouse control
        /// </summary>
        public static MultiColumnViewModel ReturnWarehouseModel
        {
            get
            {
                return new MultiColumnViewModel("WarehouseCode", "") { PlaceholderText = "Select Warehouse", ValueFieldName = "WarehouseCode" };
            }
        }

        public static MultiColumnViewModel StockCodeSelectionModel
        {
            get
            {
                return GetStockCodeSelectionModel();
            }
        }

        public static MultiColumnViewModel TransactionNoSelectionModel
        {
            get
            {
                return new MultiColumnViewModel("TransactionID", "") { PlaceholderText = "Select Transaction", ValueFieldName = "TransactionID" };
            }
        }

        #region Generate Report Model from a specific object

        private static Dictionary<string, string> viewMethodMapping = new Dictionary<string, string>()
        {
            { "DefaultAsset", "GetAssetModel" },
            { "DefaultAssetApproval", "" },
            { "DefaultBin", "" },
            { "DefaultBinType", "" },
            { "DefaultEmployee", "" },
            { "DefaultItem", "" },
            { "DefaultItemType", "" },
            { "DefaultPickListNo", "PickListNoModel" },
            { "DefaultPurchaseOrder", "GetPurchaseOrderModel" },
            { "DefaultWarehouse", "GetWarehouseModel" },
        };

        public static MultiColumnViewModel GetMultiColumnViewModel(string viewName, BaseControlViewModel baseModel)
        {
            var m = baseModel as ACS.AMS.WebApp.Models.MultiColumnViewModel;
            if (m != null)
                return m;

            if (baseModel is ACS.AMS.WebApp.Models.ReportControlViewModel)
            {
                m = new ACS.AMS.WebApp.Models.MultiColumnViewModel(baseModel.ControlName, baseModel.Value);
            }
            else
            {
                m = new ACS.AMS.WebApp.Models.MultiColumnViewModel("PostingStatusID", "") { PlaceholderText = "Select Posting Status" };
            }

            return m;
        }

        #endregion
    }
}