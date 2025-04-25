using ACS.AMS.DAL;
using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACS.AMS.WebApp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using NPOI.POIFS.FileSystem;
using ACS.WebAppPageGenerator.Models.SystemModels;
using System.Net.Http.Headers;
using ACS.AMS.WebApp.Classes;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using Microsoft.EntityFrameworkCore;
using MathNet.Numerics;
using Telerik.SvgIcons;
using System.Net;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data.SqlClient;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Org.BouncyCastle.Utilities;

namespace ACS.AMS.WebApp.Controllers
{
    public class GenerateAssetFromPOController : ACSBaseController
    {
        public IActionResult Index()
        {
            if (!base.HasRights(RightNames.GenerateAssetFromPO, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");
            IndexPageModel indexPage = new IndexPageModel();
            indexPage.EntityInstance = new BaseEntityObject();
            indexPage.EntityInstance.CurrentPageID = SessionUser.Current.GetNextPageID();
            TransferAssetDataModel.RemoveModel(indexPage.EntityInstance.CurrentPageID);

            base.TraceLog("GenerateAssetFromPO Index", $"{SessionUser.Current.Username} -GenerateAssetFromPO Index page requested");
            return PartialView(indexPage);
            //return PartialView();

        }
        protected virtual IndexPageModel GetEntryPageModel( int currentPageID)
        {
            IndexPageModel indexPage = new IndexPageModel();
            indexPage.EntityInstance = new BaseEntityObject();
            indexPage.EntityInstance.CurrentPageID = currentPageID;
            
            return indexPage;
        }
        public IActionResult GetItemList([DataSourceRequest] DataSourceRequest request, int currentPageID)
        {
            GenerateAssetFromPOModel model = GenerateAssetFromPOModel.GetModel(currentPageID);

            var dsResult = request.ToDataSourceResult(model.LineItems.AsQueryable());
            this.TraceLog("GenerateAssetFromPO Index", $"{SessionUser.Current.Username} - GenerateAssetFromPO Index page Data Fetch");

            return Json(dsResult);
        }

        [HttpPost]
        public ActionResult AddLineItems(IFormCollection collection, POModel item, int currentPageID)
        {
            try
            {
                base.TraceLog("GenerateAssetFromPO AddLineItems", $"{SessionUser.Current.Username} -AddLineItems request ");
                GenerateAssetFromPOModel model = GenerateAssetFromPOModel.GetModel(currentPageID);
                if (item.DepartmentID.HasValue)
                {
                    var itemObject = DepartmentTable.GetItem(_db, (int)item.DepartmentID);
                    if (itemObject != null)
                    {
                        item.DepartmentName = itemObject.DepartmentName;
                    }
                    else
                    {
                        return ErrorActionResult(new Exception("Department required"));
                    }
                }

                if(item.CategoryID.HasValue)
                {
                    item.CategoryName = CategoryTable.GetItem(_db, item.CategoryID.Value).CategoryName;
                }
                if (item.LocationID.HasValue)
                {
                    item.LocationName = LocationTable.GetItem(_db, item.LocationID.Value).LocationName;
                }
               
                    item.ProductName = ProductTable.GetItem(_db, item.ProductID).ProductName;
                
                if (!string.IsNullOrEmpty(item.DepartmentName))
                {
                    var itemObject = DepartmentTable.GetDepartmentDetails(_db, item.DepartmentName);
                    if (itemObject != null)
                    {
                        item.DepartmentID = itemObject.DepartmentID;
                    }
                }

                if (model.LineItems.Where(c => c.LocationID == item.LocationID && c.PoNumber == item.PoNumber
                            && c.POLineNumber == item.POLineNumber && c.ProductID == item.ProductID && c.DepartmentID==item.DepartmentID).Any())
                {
                    return ErrorActionResult(new Exception($"Asset ({item.CategoryName} - {item.ProductID}) already added"));
                }
                base.TraceLog("GenerateAssetFromPO AddLineItems", $"{SessionUser.Current.Username} -AddLineItems request done");
                model.LineItems.Add(item);
                return Content("");
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }

        public IActionResult DeleteModelData(int categoryID, int currentPageID,string PoNumber,string POLineNumber,string ProductName)
        {
            base.TraceLog("GenerateAssetFromPO DeleteModelData", $"{SessionUser.Current.Username} -DeleteModelData request ");
            GenerateAssetFromPOModel model = GenerateAssetFromPOModel.GetModel(currentPageID);

            var query = model.LineItems.Where(b => b.CategoryID == categoryID && b.PoNumber == PoNumber && b.POLineNumber == POLineNumber && b.ProductName == ProductName).FirstOrDefault();
            if (query != null)
            {
               
                    model.LineItems.Remove(query);

            }

            return Content("");
        }
        [HttpPost]
        public IActionResult Index(IFormCollection data)
        {
            if (!base.HasRights(RightNames.GenerateAssetFromPO, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");
            int currentPageID = int.Parse(data["CurrentPageID"] + "");
            try
            {
                base.TraceLog("GenerateAssetFromPO Index-post", $"{SessionUser.Current.Username} -Index-Post request ");
                
                GenerateAssetFromPOModel model = GenerateAssetFromPOModel.GetModel(currentPageID);
                if (model.LineItems.Count > 0)
                {
                    foreach (var lst in model.LineItems)
                    {
                        var standtandValidate = TransactionTable.AssetValidationResult(_db, SessionUser.Current.UserID, "WebGenerateAssetFromPO-AssetCreate", (int)lst.CategoryID,
                                            null, null, lst.LocationID, lst.DepartmentID, null, null, null, null);
                        if (!string.IsNullOrEmpty(standtandValidate.Result))
                        {
                            return base.ErrorActionResult(standtandValidate.Result);
                        }
                        ZDOF_UserPODataTable lineitem = new ZDOF_UserPODataTable();
                        lineitem.PO_HEADER_ID = lst.PO_HEADER_ID;
                        lineitem.PO_LINE_ID = lst.PO_LINE_ID;
                        lineitem.QUANTITY = lst.Quantity;
                        lineitem.UnitCost = lst.UnitCost;
                        lineitem.CategoryID = lst.CategoryID;
                        lineitem.LocationID = lst.LocationID;
                        lineitem.PO_LINE_DESC = lst.ProductName;
                        lineitem.ProductID = lst.ProductID;
                        lineitem.ZDOFPurchaseOrderID = (int)lst.PurchaseOrderID;
                        lineitem.DepartmentID = lst.DepartmentID;
                        _db.Add(lineitem);
                    }
                    _db.SaveChanges();
                    GenerateAssetFromPOModel.RemoveModel(currentPageID);
                }
                return PartialView("SuccessAction");

            }
            catch (ValidationException ex)
            {
                ViewData["FocusControl"] = ex.FieldName;
                ModelState.AddModelError("12", ex.Message);
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
            return PartialView("index", GetEntryPageModel(currentPageID));

        }
    }
}