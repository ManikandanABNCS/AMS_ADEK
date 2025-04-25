using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Kendo.Mvc;
using System.Xml.Serialization;
using ACS.AMS.DAL;
using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using Microsoft.AspNetCore.Mvc;
using ACS.AMS.WebApp.Models.SystemModels;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using ACS.AMS.WebApp.Classes;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Components.Web;
using ACS.AMS.WebApp.Models;
using Microsoft.Identity.Client.Extensibility;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ACS.AMS.WebApp.Controllers
{
    public class TransactionController : MasterPageController
    {
        private PageFieldModel GetItemFieldModel()
        {
            return new PageFieldModel() 
            { 
                FieldName = "ItemID", 
                DisplayLabel = "Item",
                ControlType = PageControlTypes.MultiColumnComboBox,
                
                ControlName = "ItemSelection",
            };
        }

        private PageFieldModel GetSupplierFieldModel()
        {
            return new PageFieldModel()
            {
                FieldName = "PartyID",
                DisplayLabel = "Supplier",
                ControlType = PageControlTypes.MultiColumnComboBox,

                ControlName = "SupplierSelection",
            };
        }

        private PageFieldModel GetCustomerFieldModel()
        {
            return new PageFieldModel()
            {
                FieldName = "PartyID",
                DisplayLabel = "Customer",
                ControlType = PageControlTypes.MultiColumnComboBox,

                ControlName = "CustomerSelection",
            };
        }

        private TransactionEntryPageModel GetTransactionEntryPageModel(string pageName)
        {
            TransactionEntryPageModel newModel = new TransactionEntryPageModel()
            {
                EntityLineItemInstance = Activator.CreateInstance<TransactionLineItemTable>(),
                LineItemPageName = pageName + "LineItem",
                PageTitle = pageName,
                PageName = pageName,
                ControllerName = this.GetType().Name.Substring(0, this.GetType().Name.Length - "Controller".Length)
            };

            SystemDatabaseHelper.GenerateMasterGridColumns(typeof(TransactionLineItemData), newModel.LineItemPageName);

            switch (pageName)
            {
                case "PurchaseOrder":
                    {
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "TransactionNo", DisplayLabel = "PurchaseOrderNo" });
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "ReferenceNo", DisplayLabel = "ReferenceNo" });
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "TransactionDateTime", DisplayLabel = "PurchaseOrderDate" });
                        newModel.TransactionFields.Add(GetSupplierFieldModel());
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "CurrencyID", DisplayLabel = "Currency" });

                        newModel.TransactionLineFields.Add(GetItemFieldModel());
                        newModel.TransactionLineFields.Add(new PageFieldModel() { FieldName = "Qty", DisplayLabel = "Qty" });
                        newModel.TransactionLineFields.Add(new PageFieldModel() { FieldName = "UOMID", DisplayLabel = "UOM" });
                        newModel.TransactionLineFields.Add(new PageFieldModel() { FieldName = "UnitPrice", DisplayLabel = "UnitPrice" });
                    }
                    break;

                case "GRNWithPO":
                    {
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "TransactionNo", DisplayLabel = "GRNNo" });
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "TransactionDateTime", DisplayLabel = "GRNDate" });
                        newModel.TransactionFields.Add(new PageFieldModel() 
                        { 
                            FieldName = "SourceTransactionID", 
                            DisplayLabel = "PurchaseOrder",
                            ControlType = PageControlTypes.MultiColumnComboBox,
                            ControlName = "POSelectionForGRN"
                        });
                    }
                    break;

                case "GRNWithoutPO":
                    {
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "TransactionNo", DisplayLabel = "GRNNo" });
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "ReferenceNo", DisplayLabel = "ReferenceNo" });
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "TransactionDateTime", DisplayLabel = "GRNDate" });
                        newModel.TransactionFields.Add(GetSupplierFieldModel());
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "CurrencyID", DisplayLabel = "Currency" });

                        newModel.TransactionLineFields.Add(GetItemFieldModel());
                        newModel.TransactionLineFields.Add(new PageFieldModel() { FieldName = "Qty", DisplayLabel = "Qty" });
                        newModel.TransactionLineFields.Add(new PageFieldModel() { FieldName = "UOMID", DisplayLabel = "UOM" });
                        newModel.TransactionLineFields.Add(new PageFieldModel() { FieldName = "UnitPrice", DisplayLabel = "UnitPrice" });
                    }
                    break;

                case "SalesOrder":
                    {
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "TransactionNo", DisplayLabel = "SalesOrderNo" });
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "ReferenceNo", DisplayLabel = "ReferenceNo" });
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "TransactionDateTime", DisplayLabel = "SalesOrderDate" });
                        newModel.TransactionFields.Add(GetCustomerFieldModel());
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "CurrencyID", DisplayLabel = "Currency" });

                        newModel.TransactionLineFields.Add(GetItemFieldModel());
                        newModel.TransactionLineFields.Add(new PageFieldModel() { FieldName = "Qty", DisplayLabel = "Qty" });
                        newModel.TransactionLineFields.Add(new PageFieldModel() { FieldName = "UOMID", DisplayLabel = "UOM" });
                        newModel.TransactionLineFields.Add(new PageFieldModel() { FieldName = "UnitPrice", DisplayLabel = "UnitPrice" });
                    }
                    break;

                case "ItemDispatch":
                    {
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "TransactionNo", DisplayLabel = "DispatchNo" });
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "TransactionDateTime", DisplayLabel = "DispatchDateTime" });
                        newModel.TransactionFields.Add(new PageFieldModel()
                        {
                            FieldName = "SourceTransactionID",
                            DisplayLabel = "SalesOrder",
                            ControlType = PageControlTypes.MultiColumnComboBox,
                            ControlName = "SOSelectionForGRN"
                        });
                    }
                    break;

                case "ItemIssue":
                    {
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "TransactionNo", DisplayLabel = "IssueNo" });
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "TransactionDateTime", DisplayLabel = "IssueDate" });
                        newModel.TransactionFields.Add(GetCustomerFieldModel());
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "CurrencyID", DisplayLabel = "Currency" });

                        newModel.TransactionLineFields.Add(GetItemFieldModel());
                        newModel.TransactionLineFields.Add(new PageFieldModel() { FieldName = "Qty", DisplayLabel = "Qty" });
                        newModel.TransactionLineFields.Add(new PageFieldModel() { FieldName = "UOMID", DisplayLabel = "UOM" });
                        newModel.TransactionLineFields.Add(new PageFieldModel() { FieldName = "UnitPrice", DisplayLabel = "UnitPrice" });
                    }
                    break;

                case "TransferOut":
                    {
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "TransactionNo", DisplayLabel = "TransferOutNo" });
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "TransactionDateTime", DisplayLabel = "TransferDate" });
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "ToWarehouseID", DisplayLabel = "ToWarehouse" });

                        newModel.TransactionLineFields.Add(new PageFieldModel() { FieldName = "FromBinID", DisplayLabel = "FromBin" });
                        newModel.TransactionLineFields.Add(GetItemFieldModel());
                        newModel.TransactionLineFields.Add(new PageFieldModel() { FieldName = "Qty", DisplayLabel = "Qty" });
                        newModel.TransactionLineFields.Add(new PageFieldModel() { FieldName = "UOMID", DisplayLabel = "UOM" });
                    }
                    break;

                case "TransferIn":
                    {
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "TransactionNo", DisplayLabel = "TransferInNo" });
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "TransactionDateTime", DisplayLabel = "TransferDate" });

                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "SourceTransactionID", DisplayLabel = "TransferOutNo" });
                    }
                    break;

                case "Putaway":
                    {
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "TransactionNo", DisplayLabel = "PutawayNo" });
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "TransactionDateTime", DisplayLabel = "PutawayDateTime" });
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "FromWarehouseID", DisplayLabel = "FromWarehouse" });
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "ToWarehouseID", DisplayLabel = "ToWarehouse" });

                        newModel.TransactionLineFields.Add(GetItemFieldModel());
                        newModel.TransactionLineFields.Add(new PageFieldModel() { FieldName = "Qty", DisplayLabel = "Qty" });
                        newModel.TransactionLineFields.Add(new PageFieldModel() { FieldName = "ToBinID", DisplayLabel = "ToBin" });
                    }
                    break;

                case "BinToBin":
                    {
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "TransactionNo", DisplayLabel = "BinToBinNo" });

                        newModel.TransactionLineFields.Add(new PageFieldModel() { FieldName = "FromBinID", DisplayLabel = "FromBin" });
                        newModel.TransactionLineFields.Add(GetItemFieldModel());
                        newModel.TransactionLineFields.Add(new PageFieldModel() { FieldName = "Qty", DisplayLabel = "Qty" });
                        newModel.TransactionLineFields.Add(new PageFieldModel() { FieldName = "ToBinID", DisplayLabel = "ToBin" });
                    }
                    break;

                case "CreditNote":
                    {
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "TransactionNo", DisplayLabel = "CreditNoteNo" });
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "TransactionDateTime", DisplayLabel = "CreditNoteDate" });
                        newModel.TransactionFields.Add(GetCustomerFieldModel());
                    }
                    break;

                case "DebitNote":
                    {
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "TransactionNo", DisplayLabel = "DebitNoteNo" });
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "TransactionDateTime", DisplayLabel = "DebitNoteDate" });
                        newModel.TransactionFields.Add(GetSupplierFieldModel());
                    }
                    break;

                default:
                    {
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "TransactionNo", DisplayLabel = "DocumentNo" });
                        newModel.TransactionFields.Add(new PageFieldModel() { FieldName = "TransactionDateTime", DisplayLabel = "Date" });
                    }
                    break;
            }

            return newModel;
        }

        private int GetTransactionTypeID(string pageName)
        {
            var type = TransactionTypeTable.GetAllItems(_db).Where(b => b.TransactionTypeName == pageName).FirstOrDefault();
            if (type != null)
                return type.TransactionTypeID;

            return -1;
        }

        public override IActionResult _Index([DataSourceRequest] DataSourceRequest request, string pageName)
        {
            var rightName = pageName;
            if (!this.HasRights(rightName, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");

            //fetch the records from language table
            var query = TransactionTable.GetAllUserItems(_db, SessionUser.Current.UserID).Where(t => t.TransactionTypeID == GetTransactionTypeID(pageName));

            var dsResult = request.ToDataSourceResult(query, pageName, "TransactionID");
            this.TraceLog("Index", $"{SessionUser.Current.Username} - {pageName} Index page Data Fetch");

            return Json(dsResult);
        }

        /// <summary>
        /// Override this method, so error in create method will show transaction page
        /// </summary>
        /// <param name="pageName"></param>
        /// <param name="obj"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        protected override EntryPageModel GetCreateEntryPageModel(string pageName, BaseEntityObject obj, IFormCollection collection = null)
        {
            var pageModel = GetTransactionEntryPageModel(pageName);

            pageModel.EntityInstance = obj;
            pageModel.FormCollection = collection;
            pageModel.ControllerName = this.GetType().Name.Substring(0, this.GetType().Name.Length - "Controller".Length);

            return pageModel;
        }

        protected override ActionResult DoCreatePage(string pageName)
        {
            var rightName = pageName;

            if (!base.HasRights(rightName, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("Create", $"{SessionUser.Current.Username} - {pageName} Create page requested");
            var obj = Activator.CreateInstance(GetEntityObjectType(pageName)) as BaseEntityObject;

            var model = GetTransactionEntryPageModel(pageName);
            model.EntityInstance = obj;

            obj.CurrentPageID = SessionUser.Current.GetNextPageID();
            model.EntityLineItemInstance.CurrentPageID = obj.CurrentPageID;

            return PartialView("BaseViews/CreatePage", model);
        }

        protected override ActionResult DoEditPage(int id, string pageName)
        {
            var rightName = pageName;

            if (!base.HasRights(rightName, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("Create", $"{SessionUser.Current.Username} - {pageName} Create page requested");
            var obj = Activator.CreateInstance(GetEntityObjectType(pageName)) as BaseEntityObject;
            var objInstance = obj as IACSDBObject;

            var model = GetTransactionEntryPageModel(pageName);
            model.PageTitle = pageName;
            model.PageName = pageName;
            model.EntityInstance = objInstance.GetItemByID(_db, id) as BaseEntityObject;

            obj.CurrentPageID = SessionUser.Current.GetNextPageID();
            model.EntityLineItemInstance.CurrentPageID = obj.CurrentPageID;

            TransactionLineItemDataModel lineModel = TransactionLineItemDataModel.GetModel(obj.CurrentPageID);
            var lines = from b in TransactionLineItemTable.GetAllItems(_db)
                        where b.TransactionID == id
                        select new TransactionLineItemData()
                        {
                            TransactionLineItemID = b.TransactionLineItemID,
                            ItemID = b.ItemID,
                            ItemCode = b.Item.ItemCode,
                            ItemName = b.Item.ItemName,
                            Qty = b.Qty,
                            UnitPrice = b.UnitPrice,
                            UOMID = b.UOMID,
                            UOMCode = b.UOM.UOMCode,

                            TotalQty = b.Qty,
                            PendingQty = b.Qty,
                            FromBinID = b.FromBinID,
                            FromBinCode = b.FromBin.BinCode,

                            ToBinID = b.ToBinID,
                            ToBinCode = b.ToBin.BinCode
                        };
            lineModel.LineItems.AddRange(lines.ToList());

            return PartialView("BaseViews/EditPage", model);
        }

        protected override ActionResult DoDetailsPage(int id, string pageName)
        {
            var rightName = pageName;

            if (!base.HasRights(rightName, UserRightValue.Details))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("Details", $"{SessionUser.Current.Username} - {pageName} Details page requested");
            var obj = Activator.CreateInstance(GetEntityObjectType(pageName)) as BaseEntityObject;
            var objInstance = obj as IACSDBObject;

            var model = GetTransactionEntryPageModel(pageName);
            model.PageTitle = pageName;
            model.PageName = pageName;
            model.EntityInstance = objInstance.GetItemByID(_db, id) as BaseEntityObject;

            obj.CurrentPageID = SessionUser.Current.GetNextPageID();
            model.EntityLineItemInstance.CurrentPageID = obj.CurrentPageID;

            TransactionLineItemDataModel lineModel = TransactionLineItemDataModel.GetModel(obj.CurrentPageID);
            var lines = from b in TransactionLineItemTable.GetAllItems(_db)
                        where b.TransactionID == id
                        select new TransactionLineItemData()
                        {
                            TransactionLineItemID = b.TransactionLineItemID,
                            ItemID = b.ItemID,
                            ItemCode = b.Item.ItemCode,
                            ItemName = b.Item.ItemName,
                            Qty = b.Qty,
                            UnitPrice = b.UnitPrice,
                            UOMID = b.UOMID,
                            UOMCode = b.UOM.UOMCode,

                            TotalQty = b.Qty,
                            PendingQty = b.Qty,
                            FromBinID = b.FromBinID,
                            FromBinCode = b.FromBin.BinCode,

                            ToBinID = b.ToBinID,
                            ToBinCode = b.ToBin.BinCode
                        };
            lineModel.LineItems.AddRange(lines.ToList());

            return PartialView("BaseViews/DetailsPage", model);
        }

        protected override void UpdateNewEntityObject(BaseEntityObject entity, IFormCollection collection, string pageName)
        {
            entity.SetFieldValue("TransactionTypeID", GetTransactionTypeID(pageName));

            //add the line items
            TransactionLineItemDataModel model = TransactionLineItemDataModel.GetModel(entity.CurrentPageID);
            if((model.LineItems.Count == 0) || (model.LineItems.Where(b => b.Qty > 0).Any() == false ))
            {
                throw new ValidationException($"Line items required");
            }

            foreach (var item in model.LineItems )
            {
                var lineItem = new TransactionLineItemTable()
                {
                    ItemID = item.ItemID,
                    UOMID = item.UOMID,
                    Qty = item.Qty,
                    UnitPrice = item.UnitPrice,

                    StatusID = (int) StatusValue.Active,
                    Transaction = (TransactionTable)entity
                };

                _db.Add(lineItem);
            }

            base.UpdateNewEntityObject(entity, collection, pageName);
        }

        #region Line Item Methods

        [HttpPost]
        public ActionResult AddLineItem(IFormCollection collection, int currentPageID, TransactionLineItemData item)
        {
            try
            {
                TransactionLineItemDataModel model = TransactionLineItemDataModel.GetModel(currentPageID);

                var itemObject = ItemTable.GetItem(_db, item.ItemID);
                if(itemObject != null)
                {
                    item.ItemCode = itemObject.ItemCode;
                    item.ItemName = itemObject.ItemName;
                }
                var uomObject = UOMTable.GetItem(_db, item.UOMID);
                if (uomObject != null)
                {
                    item.UOMCode = uomObject.UOMCode;
                }

                model.LineItems.Add(item);

                return Content("");
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }

        public IActionResult _LineItemindex([DataSourceRequest] DataSourceRequest request, int currentPageID)
        {
            TransactionLineItemDataModel model = TransactionLineItemDataModel.GetModel(currentPageID);

            var dsResult = request.ToDataSourceResult(model.LineItems.AsQueryable());
            this.TraceLog("Index", $"{SessionUser.Current.Username} - Index page Data Fetch");

            return Json(dsResult);
        }

        public IActionResult _DeleteLineItem(int id, int currentPageID)
        {
            TransactionLineItemDataModel model = TransactionLineItemDataModel.GetModel(currentPageID);

            var query = model.LineItems.Where(b => b.TransactionLineItemID == id).FirstOrDefault();
            if (query != null)
                model.LineItems.Remove(query);

            return Content("");
        }
        #endregion

    }
}
