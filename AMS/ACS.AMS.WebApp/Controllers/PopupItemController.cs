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
using ACS.AMS.DAL.Models;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Crypto;
using ACS.WebAppPageGenerator.Models.SystemModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace ACS.AMS.WebApp.Controllers
{
    public class PopupItemController : ACSBaseController
    {
        public IActionResult Index(int currentPageID, string sceenName)
        {
            if (!base.HasRights(RightNames.Asset, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("PopupItem Index", $"{SessionUser.Current.Username} -PopupItem Index page requested");
            IndexPageModel indexPage = new IndexPageModel();
            indexPage.EntityInstance = new BaseEntityObject();
            indexPage.EntityInstance.CurrentPageID = currentPageID;
            ViewBag.ScreenName = sceenName;
            ////indexPage.EntityInstance.CurrentPageID = currentPageID;
            //IndexPageHelper<IndexPageModel> indexPage = new IndexPageHelper<IndexPageModel>(Model)
            //{
            //    Url = this.Url
            //};

            return PartialView(indexPage);
            //var obj = Activator.CreateInstance(GetEntityObjectType("Asset")) as BaseEntityObject;

            //return PartialView("Index",obj);
        }

        public IActionResult _Index([DataSourceRequest] DataSourceRequest request, int currentPageID, string screenName)
        {
            if (!base.HasRights(RightNames.Asset, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");

            bool locationtype = AppConfigurationManager.GetValue<bool>(AppConfigurationManager.IsMandatoryLocationType);

            IQueryable<AssetNewView> query = AssetNewView.GetAllUserItem(_db, SessionUser.Current.UserID)
                                    .Where(a => a.StatusID == (int)StatusValue.Active /*&& a.LocationType != null*/);
            if(locationtype)
            {
                query = query.Where(a => a.LocationType != null);
            }
            //IQueryable<AssetNewView> query = AssetNewView.GetAllItems(_db).Where(a => a.LocationType != null && a.StatusID == (int)StatusValue.Active) ;
            //var userBasedLocation = PersonTable.GetUserBasedLocationList(_db, SessionUser.Current.UserID).Select(a=>a.LocationID+"").ToList();

            //if (userBasedLocation.Count() > 0)
            //{
            // query = query.Where(a => userBasedLocation.Contains(a.LocationL2));
            //query=query.Where()
            if (string.Compare("AssetTransfer", screenName) == 0 || string.Compare("InternalAssetTransfer", screenName) == 0)
            {
                TransferAssetDataModel model = TransferAssetDataModel.GetModel(currentPageID);
                if (model != null)
                {
                    if (model.LineItems.Count() > 0)
                    {
                        List<int> ids = model.LineItems.Select(a => a.Asset.AssetID).ToList();
                      
                        query = query.Where(a => !ids.Contains(a.AssetID));
                        if (locationtype)
                        {
                            var list = AssetNewView.GetAllUserItem(_db, SessionUser.Current.UserID).Where(a => ids.Contains(a.AssetID)).Select(a => a.LocationType).Distinct().ToList();
                            query = query.Where(a => list.Contains(a.LocationType));
                        }
                    }

                    var dsResult = request.ToDataSourceResult(query, "PopupItem", "AssetID"); //query.ToDataSourceResult(request);
                    base.TraceLog("asset popup Index", $"{SessionUser.Current.Username} -asset popup Index page Data Fetch");
                    return Json(dsResult);
                }
                else
                {
                    //var dsResult = query.ToDataSourceResult(request);
                    //base.TraceLog("asset popup Index", $"{SessionUser.Current.Username} -asset popup Index page Data Fetch");
                    //return Json(dsResult);
                    var dsResult = request.ToDataSourceResult(query, "PopupItem", "AssetID"); //query.ToDataSourceResult(request);
                    base.TraceLog("asset popup Index", $"{SessionUser.Current.Username} -asset popup Index page Data Fetch");
                    return Json(dsResult);
                }
            }
            else if (string.Compare("AssetRetirement", screenName) == 0)
            {
                RetirementDataModel model = RetirementDataModel.GetModel(currentPageID);
                if (model != null)
                {
                    if (model.LineItems.Count() > 0)
                    {
                        List<int> ids = model.LineItems.Select(a => a.Asset.AssetID).ToList();
                       
                        query = query.Where(a => !ids.Contains(a.AssetID));
                        if (locationtype)
                        {
                            var list = AssetNewView.GetAllUserItem(_db, SessionUser.Current.UserID).Where(a => ids.Contains(a.AssetID)).Select(a => a.LocationType).Distinct().ToList();
                            query = query.Where(a => list.Contains(a.LocationType));
                        }
                    }
                    var dsResult = request.ToDataSourceResult(query, "PopupItem", "AssetID"); //query.ToDataSourceResult(request);
                    base.TraceLog("asset popup Index", $"{SessionUser.Current.Username} -asset popup Index page Data Fetch");
                    return Json(dsResult);
                    //var dsResult = query.ToDataSourceResult(request);
                    //base.TraceLog("asset popup Index", $"{SessionUser.Current.Username} -asset popup Index page Data Fetch");
                    //return Json(dsResult);
                }
                else
                {
                    var dsResult = request.ToDataSourceResult(query, "PopupItem", "AssetID"); //query.ToDataSourceResult(request);
                    base.TraceLog("asset popup Index", $"{SessionUser.Current.Username} -asset popup Index page Data Fetch");
                    return Json(dsResult);
                    //var dsResult = query.ToDataSourceResult(request);
                    //base.TraceLog("asset popup Index", $"{SessionUser.Current.Username} -asset popup Index page Data Fetch");
                    //return Json(dsResult);
                }
            }
            else if (string.Compare("AMCSchedule", screenName) == 0)
            {
                TransactionLineItemDetailsModel model = TransactionLineItemDetailsModel.GetCurrentModel(currentPageID);

                if (model != null)
                {
                    if (model.LineItems.Count() > 0)
                    {
                        List<int> ids = model.LineItems.Select(a => a.AssetID).ToList();
                    

                        query = query.Where(a => !ids.Contains(a.AssetID));
                        if (locationtype)
                        {
                            var list = AssetNewView.GetAllUserItem(_db, SessionUser.Current.UserID)
                                    .Where(a => ids.Contains(a.AssetID)).Select(a => a.LocationType).Distinct().ToList();
                            query = query.Where(a => list.Contains(a.LocationType));
                        }
                    }

                    var dsResult = request.ToDataSourceResult(query, "PopupItem", "AssetID"); //query.ToDataSourceResult(request);
                    base.TraceLog("asset popup Index", $"{SessionUser.Current.Username} -asset popup Index page Data Fetch");
                    return Json(dsResult);
                    //var dsResult = query.ToDataSourceResult(request);
                    //base.TraceLog("asset popup Index", $"{SessionUser.Current.Username} -asset popup Index page Data Fetch");
                    //return Json(dsResult);
                }
                else
                {
                    var dsResult = request.ToDataSourceResult(query, "PopupItem", "AssetID"); //query.ToDataSourceResult(request);
                    base.TraceLog("asset popup Index", $"{SessionUser.Current.Username} -asset popup Index page Data Fetch");
                    return Json(dsResult);
                    //var dsResult = query.ToDataSourceResult(request);
                    //base.TraceLog("asset popup Index", $"{SessionUser.Current.Username} -asset popup Index page Data Fetch");
                    //return Json(dsResult);
                }
            }
            //}
            //else
            //{
            //    return Json("");
            //}

            return Json("");
        }

        [HttpPost]
        public ActionResult ValidateAssets(IFormCollection collection)
        {
            try
            {
                base.TraceLog("Popup index", $"{SessionUser.Current.Username} -ValidateAssets");
                if (!string.IsNullOrEmpty(collection["ids"]))
                {
                    string screenName = collection["screenName"];
                    int currentPageID = int.Parse(collection["currentPageID"] + "");
                    string assetTransferID = collection["ids"];
                    string[] arrindate = assetTransferID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    int[] intIDS = Array.ConvertAll(arrindate, s => int.Parse(s));
                    List<int> assetid = intIDS.ToList();
                    List<int> locAssetID = new List<int>();
                    if (string.Compare(screenName, "AssetTransfer") == 0)
                    {
                        TransferAssetDataModel model = TransferAssetDataModel.GetModel(currentPageID);
                        if (model != null)
                        {
                            if (model.LineItems.Count() > 0)
                            {
                                locAssetID = model.LineItems.Select(a => a.Asset.AssetID).ToList();
                            }
                        }
                    }
                    if (string.Compare(screenName, "AssetRetirement") == 0)
                    {
                        RetirementDataModel model = RetirementDataModel.GetModel(currentPageID);
                        if (model != null)
                        {
                            if (model.LineItems.Count() > 0)
                            {
                                locAssetID = model.LineItems.Select(a => a.Asset.AssetID).ToList();
                            }
                        }
                    }
                    var selectedAssetID = assetid.Concat(locAssetID);

                    if (string.Compare(screenName, "InternalAssetTransfer") == 0)
                    {
                        var loc = AssetNewView.GetAllItems(_db).Where(a => assetid.Contains(a.AssetID)).Select(a => a.MappedLocationID).ToList();
                        List<int> lastnode = new List<int>();
                        foreach (int id in loc)
                        {
                            var details = LocationTable.GetItem(_db, id);
                            lastnode.Add(details.ParentLocationID.HasValue ? (int)details.ParentLocationID : 0);
                        }
                        if (lastnode.Distinct().Count() > 1)
                        {
                            return Json(new { Result = "Error", Errormsg = "Location Mismatch: Same  Location  only allowed here\r\n\r\n\r\n\r\n" }, new Newtonsoft.Json.JsonSerializerSettings());

                        }
                        else
                        {
                            var locid = (from b in loc select b).FirstOrDefault();
                            return Json(new { Result = "Success" }, new Newtonsoft.Json.JsonSerializerSettings()); ;
                        }
                    }
                    else
                    {

                        var loc = AssetNewView.GetAllItems(_db).Where(a => selectedAssetID.Contains(a.AssetID)).Select(a => a.LocationL2).Distinct().ToList();
                        if (loc.Count() > 1)
                        {
                            return Json(new { Result = "Error", Errormsg = "Location Mismatch: Same Second level Location  only allowed here\r\n\r\n\r\n\r\n" }, new Newtonsoft.Json.JsonSerializerSettings());

                        }
                        else
                        {
                            var locid = (from b in loc select b).FirstOrDefault();
                            return Json(new { Result = "Success" }, new Newtonsoft.Json.JsonSerializerSettings()); ;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
            return Json(new { }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult AddLineItem(IFormCollection collection, int currentPageID, string screenName)
        {
            try
            {
                base.TraceLog("PopupItem", $"{SessionUser.Current.Username} -{screenName} AddLineItem request");
                if (string.Compare("AssetTransfer", screenName) == 0 || string.Compare("InternalAssetTransfer", screenName) == 0)
                {

                    TransferAssetDataModel model = TransferAssetDataModel.GetModel(currentPageID);

                    if (!string.IsNullOrEmpty(collection["ids"]))
                    {
                        string assetTransferID = collection["ids"];
                        string[] arrindate = assetTransferID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        int[] intIDS = Array.ConvertAll(arrindate, s => int.Parse(s));
                        List<int> assetid = intIDS.ToList();

                        foreach (var id in assetid)
                        {
                            TransferAssetData item = new TransferAssetData();
                            AssetTable asset = AssetTable.GetItem(_db, id);
                            item.Asset = asset;
                            item.Asset.Attribute40 = "Search Asset Transfer";
                            model.LineItems.Add(item);
                        }
                    }
                }
                else if (string.Compare("AssetRetirement", screenName) == 0)
                {
                    RetirementDataModel model = RetirementDataModel.GetModel(currentPageID);

                    if (!string.IsNullOrEmpty(collection["ids"]))
                    {
                        string assetTransferID = collection["ids"];
                        string[] arrindate = assetTransferID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        int[] intIDS = Array.ConvertAll(arrindate, s => int.Parse(s));
                        List<int> assetid = intIDS.ToList();

                        foreach (var id in assetid)
                        {
                            RetirementData item = new RetirementData();
                            AssetTable asset = AssetTable.GetItem(_db, id);
                            item.Asset = asset;
                            item.Asset.Attribute40 = "Search Asset Retirement";
                            model.LineItems.Add(item);
                        }
                    }
                }
                else if (string.Compare("AMCSchedule", screenName) == 0)
                {
                    TransactionLineItemDetailsModel model = TransactionLineItemDetailsModel.GetCurrentModel(currentPageID);

                    if (!string.IsNullOrEmpty(collection["ids"]))
                    {
                        string assetTransferID = collection["ids"];
                        string[] arrindate = assetTransferID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        int[] intIDS = Array.ConvertAll(arrindate, s => int.Parse(s));
                        List<int> assetid = intIDS.ToList().Distinct().ToList();

                        foreach (var id in assetid)
                        {
                            TransactionLineItemModel item = new TransactionLineItemModel();
                            AssetNewView asset = AssetNewView.GetItem(_db, id);
                            item.AssetID = id;
                            item.AssetCode = asset.AssetCode;
                            item.Barcode = asset.Barcode;
                            item.CategoryHierarchy = asset.CategoryHierarchy;
                            item.LocationHierarchy = asset.LocationHierarchy;
                            item.AssetDescription = asset.AssetDescription;
                            item.LocationType = asset.LocationType;
                            item.FromLocationID = asset.MappedLocationID;
                            model.LineItems.Add(item);
                        }
                    }
                }
                base.TraceLog("PopupItem", $"{SessionUser.Current.Username} -{screenName} AddLineItem request done");
                return Json(new { Result = "Success" }, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
            return Json(new { }, new Newtonsoft.Json.JsonSerializerSettings());
        }
    }
}