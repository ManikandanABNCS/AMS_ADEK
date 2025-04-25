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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using System.Web;
using System.Net.Mail;

namespace ACS.AMS.WebApp.Controllers
{
    public class DepreciationClassController : MasterPageController
    {
        public DepreciationClassController(IWebHostEnvironment _environment)
          : base(_environment)
        {
        }
        protected override ActionResult DoCreatePage(string pageName, bool isPopupCreation = false)
        {
            var rightName = pageName;

            if (!base.HasRights(rightName, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("Create", $"{SessionUser.Current.Username} - {pageName} Create page requested");
            var obj = Activator.CreateInstance(GetEntityObjectType(pageName)) as BaseEntityObject;
            int currentPageID = SessionUser.Current.GetNextPageID();

            obj.CurrentPageID = currentPageID;
            DepreciationClassLineItemModel.RemoveModel(currentPageID);
            DepreciationClassLineItemModel.GetModel(currentPageID);
            //var pageModel = GetTransactionEntryPageModel(pageName);
            //pageModel.EntityInstance = obj;

            //obj.CurrentPageID = SessionUser.Current.GetNextPageID();
            //pageModel.EntityLineItemInstance.CurrentPageID = obj.CurrentPageID;

            return PartialView("CreatePage",
                new EntryPageModel()
                {
                    PageTitle = pageName,
                    PageName = pageName,
                    EntityInstance = obj
                });
        }
        public IActionResult _LineItemindex([DataSourceRequest] DataSourceRequest request, int currentPageID)
        {

            var query = DepreciationClassLineItemModel.GetModel(currentPageID).LineItems;
            var dsResult = query.ToDataSourceResult(request);
            // var dsResult = request.ToDataSourceResult(query.AsQueryable(), "AssetTransactionUniformItemEntry", "AssetTransactionLineItemID");
            this.TraceLog("Index", $"{SessionUser.Current.Username} - Index page Data Fetch");

            return Json(dsResult);
        } 
        [HttpPost]
        public async Task<ActionResult> AddLineItem(IFormCollection collection, int? id, int currentPageID, string pageName, string StartValue, string EndValue, int Duration, bool UnEndValue,string screen)
        {
            try
            {
                base.TraceLog("Depreciation class AddLineItem", $"{SessionUser.Current.Username} -AddLineItem details will add to model");
                var AssetTransactionDetailsDataModel = DepreciationClassLineItemModel.GetModel(currentPageID);
                int chkValue = AssetTransactionDetailsDataModel.LineItems
                                   .Where(o => o.EndValue.Equals("*")).Count();
                if (string.Compare(screen, "Create") == 0)
                {
                    if (AssetTransactionDetailsDataModel.LineItems.Count() > 0)
                    {

                        if (chkValue == 0)
                        {
                            foreach (var item in (AssetTransactionDetailsDataModel.LineItems))
                            {
                                if (int.Parse(StartValue) <= int.Parse(item.EndValue) &&
                                              int.Parse(StartValue) >= int.Parse(item.StartValue))
                                {
                                    return base.ErrorActionResult("Start Value is Overlapping with other data");
                                }
                            }
                        }
                        else
                        {
                            var lastStartValue = AssetTransactionDetailsDataModel.LineItems.Where(o => o.EndValue == "*").Select(x => x.StartValue).FirstOrDefault();
                            var lastEndValue = AssetTransactionDetailsDataModel.LineItems
                                            .Where(o => o.EndValue != "*").Select(x => x.EndValue)
                                                .Max();
                            if (int.Parse(StartValue) <= int.Parse(lastEndValue) || int.Parse(EndValue) >= int.Parse(lastStartValue))
                            {
                                return base.ErrorActionResult("Class line item overlap, please enter the correct limit.");
                            }
                        }
                    }
                }
                else
                {
                    if (chkValue == 0)
                    {
                        var existingloop = AssetTransactionDetailsDataModel.LineItems
                                      .Where(o => int.Parse(o.EndValue) >= int.Parse(StartValue) && o.LineItemID != id)
                                          .FirstOrDefault();
                        if (existingloop != null)
                        {
                            return base.ErrorActionResult("Start Value is Overlap");
                        }
                    }
                    else
                    {
                        foreach (var item in AssetTransactionDetailsDataModel.LineItems)
                        {
                            if (item.LineItemID != id)
                            {
                                if (item.EndValue == "*")
                                {
                                    if (UnEndValue)
                                    {
                                        return base.ErrorActionResult("End Value is Overlap");
                                    }

                                }
                                else
                                {
                                    if (!(int.Parse(EndValue) < int.Parse(item.StartValue)) &&
                                                    int.Parse(StartValue) < (int.Parse(item.StartValue)))
                                    {
                                        return Content("AssetEndValue", "End Value is Overlap.");
                                    }
                                }

                            }
                            else
                            {
                                if (EndValue == "*")
                                {
                                    if ((int.Parse(StartValue) < int.Parse(item.StartValue)) &&
                                                (int.Parse(StartValue) > int.Parse(item.StartValue)))
                                    {
                                        return base.ErrorActionResult("Start Value is Overlap.");
                                    }
                                }
                                else
                                {
                                    if (item.EndValue != "*")
                                    {
                                        if ((int.Parse(EndValue) <= int.Parse(item.EndValue)) &&
                                                    (int.Parse(EndValue) >= int.Parse(item.StartValue)))
                                        {
                                            return base.ErrorActionResult("End Value is Overlap.");
                                        }
                                        else
                                        {
                                            if ((int.Parse(StartValue) <= int.Parse(item.EndValue)) &&
                                                    (int.Parse(StartValue) >= int.Parse(item.StartValue)))
                                            {
                                                return base.ErrorActionResult("Start Value is Overlap.");
                                            }
                                        }
                                    }
                                }
                            }

                        }

                    }
                }
                
                if (!string.IsNullOrEmpty(StartValue) && !string.IsNullOrEmpty(EndValue))
                {
                    if (StartValue == EndValue)
                    {
                        return base.ErrorActionResult("Start and End Value are Same.");
                    }
                }



                if (id != null && id != 0)
                {
                    if (AssetTransactionDetailsDataModel.LineItems.Where(c => c.StartValue == StartValue && c.id != id).Any())
                    {
                        return base.ErrorActionResult("Start Value already exists");
                    }
                    var query = AssetTransactionDetailsDataModel.LineItems.Where(b => b.id == id).FirstOrDefault();
                    if (query != null)
                    {

                        query.StartValue = StartValue;
                        query.EndValue = UnEndValue ? "*" : EndValue + "";
                        query.Duration = Duration;
                    }
                }
                else
                {
                    if (AssetTransactionDetailsDataModel.LineItems.Where(c => c.StartValue == StartValue).Any())
                    {
                        return base.ErrorActionResult("Start Value already exists");
                    }

                    LineItemModel data = new LineItemModel();
                    data.StartValue = StartValue;
                    data.EndValue = UnEndValue ? "*" : EndValue + "";
                    data.Duration = Duration;
                    AssetTransactionDetailsDataModel.LineItems.Add(data);
                }

                base.TraceLog("DepreciationClass AddLineItem", $"{SessionUser.Current.Username} -AddLineItem details added to model");

                return Content("");
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }
        }
        public ActionResult LoadEditData(int currentPageID, int id)
        {
            try
            {
                base.TraceLog("Depreciationclass LoadEditData", $"{SessionUser.Current.Username} -LoadEditData details fetch from model.id- {id}");
                var AssetTransactionDetailsDataModel = DepreciationClassLineItemModel.GetModel(currentPageID);

                var target = AssetTransactionDetailsDataModel.LineItems.Where(p => p.id == id);
                return Json(new { Success = true, result = target });
            }
            catch (Exception ex)
            {
                return ErrorActionResult(ex);
            }

        }
        public IActionResult DeleteAllLineItem(string toBeDeleteIds, string pageName, int currentPageID)
        {
            var rightName = pageName;
            if (!base.HasRights(rightName, UserRightValue.Delete))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("DeleteAll", $"{SessionUser.Current.Username} - {pageName} Delete action requested");

            var obj = Activator.CreateInstance(GetEntityObjectType(pageName)) as IACSDBObject;


            string checkdItems = toBeDeleteIds;
            string[] arrRequestID = checkdItems.Split(',');
            int[] requestID = Array.ConvertAll(arrRequestID, s => int.Parse(s));
            var requestID1 = requestID.Distinct().ToList();
            int requestCount = requestID1.Count();
            var AssetTransactionDetailsDataModel = DepreciationClassLineItemModel.GetModel(currentPageID);
            if (requestCount > 0)
            {
                foreach (var id in requestID1)
                {

                    var query = AssetTransactionDetailsDataModel.LineItems.Where(b => b.id == id).FirstOrDefault();
                    if (query != null)
                        AssetTransactionDetailsDataModel.LineItems.Remove(query);

                }
            }
            base.TraceLog("DeleteAll", $"{pageName}  page deleted successfully");

            return Json("Success");
        }
        public IActionResult _DeleteLineItem(int id, int currentPageID)
        {
            base.TraceLog("DepreciationClass _DeleteLineItem", $" page deleted request.id-{id}");
            var AssetTransactionDetailsDataModel = DepreciationClassLineItemModel.GetModel(currentPageID);
            var query = AssetTransactionDetailsDataModel.LineItems.Where(b => b.id == id).FirstOrDefault();
            if (query != null)
                AssetTransactionDetailsDataModel.LineItems.Remove(query);
            base.TraceLog("DepreciationClass _DeleteLineItem", $" page deleted request done.id-{id}");
            return Content("");
        }
        [HttpPost]
        public async Task<ActionResult> DepreciationClassCreate(IFormCollection collection, string pageName, int CurrentPageID, string? FilePath, bool IsBulkInsert = false)
        {
            return await ProcessDataCreation(collection, pageName, CurrentPageID, FilePath, IsBulkInsert);
        }

        protected async Task<ActionResult> ProcessDataCreation(IFormCollection collection, string pageName, int currentPageID, string? FilePath, bool IsBulkInsert)
        {

            var rightName = pageName;

            if (!base.HasRights(rightName, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");
            var modelType = GetEntityObjectType(pageName);
            var item = Activator.CreateInstance(modelType) as BaseEntityObject;

            try
            {
                base.TraceLog("Create Post", $"{SessionUser.Current.Username} - {pageName} details will add to db ");
                string className = collection["ClassName"];
                string method = AppConfigurationManager.GetValue<string>(AppConfigurationManager.DepreciationMethod);// ConfigurationTable.GetConfigurationValue(_db, "DepreciationMethod");
                var methodID= DepreciationMethodTable.GetAllItems(_db).Where(a=>a.MethodName==method).FirstOrDefault();

             
                item.SetFieldValue("ClassName", className);
                item.SetFieldValue("DepreciationClassName", className);
                if(methodID!=null)
                {
                    item.SetFieldValue("DepreciationMethodID", methodID.DepreciationMethodID);
                }

                if (ModelState.IsValid)
                {
                    var AssetTransactionDetailsDataModel = DepreciationClassLineItemModel.GetModel(currentPageID).LineItems;
                    if (AssetTransactionDetailsDataModel.Count == 0)
                        return base.ErrorActionResult("Atleast One LineItem Required");
                   
                    item.ValidateUniqueKey(_db);
                    _db.Add(item);
                    //add line items
                    foreach (var entry in AssetTransactionDetailsDataModel)
                    {
                        DepreciationClassLineItemTable litem = new DepreciationClassLineItemTable();
                        litem.DepreciationClass = item as DepreciationClassTable;
                        litem.AssetStartValue = entry.StartValue;
                        litem.AssetEndValue = entry.UnEndValue == true ? "*" : entry.EndValue;
                        litem.Duration = entry.Duration+"";
                        _db.Add(litem);
                    }

                    _db.SaveChanges();
                    ViewData["pageName"] = pageName;
                    DepreciationClassLineItemModel.RemoveModel(currentPageID);
                    base.TraceLog("Create Post", $"{SessionUser.Current.Username} - {pageName} details saved to db ");
                    return PartialView("SuccessAction", new { pageName = pageName });
                }
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

            item.CurrentPageID = currentPageID;
            return PartialView("CreatePage",
                new EntryPageModel()
                {
                    PageTitle = pageName,
                    PageName = pageName,
                    EntityInstance = item,
                    FormCollection = collection
                });
        }
        protected override ActionResult DoEditPage(int id, string pageName)
        {
            var rightName = pageName;

            if (!base.HasRights(rightName, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("Edit", $"{SessionUser.Current.Username} - {pageName} Edit page requested.id- {id}");
            var obj = Activator.CreateInstance(GetEntityObjectType(pageName)) as BaseEntityObject;
            var objInstance = obj as IACSDBObject;
            BaseEntityObject EntityInstance = objInstance.GetItemByID(_db, id);
            DepreciationClassTable itm = EntityInstance as DepreciationClassTable;

            int currentPageID = SessionUser.Current.GetNextPageID();
            ViewBag.CurrentPageID = currentPageID;
            DepreciationClassLineItemModel.RemoveModel(currentPageID);
            var AssetTransactionDetailsDataModel = DepreciationClassLineItemModel.GetModel(currentPageID);

            AssetTransactionDetailsDataModel.LineItems = (from b in DepreciationClassLineItemTable.GetAllItems(_db)
                                                          where b.DepreciationClassID == id
                                                          select new LineItemModel()
                                                          {
                                                              LineItemID = b.DepreciationClassLineItemID,
                                                             
                                                              StartValue=b.AssetStartValue,
                                                              EndValue=b.AssetEndValue=="*"?"":b.AssetEndValue,
                                                              Duration=int.Parse(b.Duration),
                                                              UnEndValue= b.AssetEndValue == "*" ? true :false,

                                                          }).ToList();

            return PartialView("EditPage",
                new EntryPageModel()
                {
                    PageTitle = pageName,
                    PageName = pageName,
                    EntityInstance = objInstance.GetItemByID(_db, id)
                });

        }
        [HttpPost]
        public async Task<ActionResult> DepreciationClassEdit(IFormCollection data, long primaryKeyID, string pageName, int CurrentPageID)
        {
            return await ProcessDataUpdation(data, primaryKeyID, pageName, CurrentPageID);
        }
        protected async Task<ActionResult> ProcessDataUpdation(IFormCollection collection, long primaryKeyID, string pageName, int CurrentPageID)
        {
            var rightName = pageName;
            ViewBag.CurrentPageID = CurrentPageID;
            if (!base.HasRights(rightName, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");

            var modelType = GetEntityObjectType(pageName);
            var item = Activator.CreateInstance(modelType) as BaseEntityObject;
            try
            {
                base.TraceLog("Edit Post", $"{pageName} details will modify to db : Entity id- {primaryKeyID}");
                var res = await base.TryUpdateModelAsync(item, modelType, "");

                var objInstance = item as IACSDBObject;
                var oldItem = objInstance.GetItemByID(_db, (int)primaryKeyID) as BaseEntityObject;
                //var olditemdata = oldItem as AssetTransactionTable;
                //item.SetFieldValue("TransactionNo", olditemdata.TransactionNo);

                //if (ModelState.IsValid)
                {
                    var AssetTransactionDetailsDataModel = DepreciationClassLineItemModel.GetModel(CurrentPageID).LineItems;
                    if (AssetTransactionDetailsDataModel.Count == 0)
                        return base.ErrorActionResult("Atleast one lineItem required");

                    DataUtilities.CopyObject(item, oldItem);

                    foreach (var entry in AssetTransactionDetailsDataModel)
                    {
                        if (DepreciationClassLineItemTable.CheckExistingLineProcess(_db, entry.LineItemID, (int)primaryKeyID))
                        {
                            DepreciationClassLineItemTable line = new DepreciationClassLineItemTable();
                            DepreciationClassLineItemTable oldLineitem = DepreciationClassLineItemTable.GetItem(_db, entry.LineItemID);
                            line = oldLineitem;
                            line.DepreciationClassID = oldLineitem.DepreciationClassID;
                            line.AssetStartValue = entry.StartValue;
                            line.AssetEndValue = entry.UnEndValue == true ? "*" : entry.EndValue;
                            line.Duration = entry.Duration+"";
                            //decimal num1;
                            //bool res = decimal.TryParse(lst.DepreciationPercentage, out num1);
                            //if (res)
                            //{
                            //    line.DepreciationPercentage = decimal.Parse(lst.DepreciationPercentage + "");
                            //}
                            DataUtilities.CopyObject(line, oldLineitem);
                        }
                        else
                        {
                            DepreciationClassLineItemTable line = new DepreciationClassLineItemTable();
                            line.DepreciationClassID =(int)primaryKeyID;
                            line.AssetStartValue = entry.StartValue;
                            line.AssetEndValue = entry.UnEndValue == true ? "*" : entry.EndValue;
                            line.Duration = entry.Duration + "";
                            _db.Add(line);
                        }
                    }
                    _db.SaveChanges();

                    base.TraceLog("Edit Post", $"{pageName} details modified to db : Entity id- {primaryKeyID}");
                    ViewData["pageName"] = pageName;
                    return PartialView("SuccessAction", new { pageName = pageName });
                }
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

            return PartialView("EditPage",
                new EntryPageModel()
                {
                    PageTitle = pageName,
                    PageName = pageName,
                    EntityInstance = item
                });
        }

        protected override ActionResult DoDetailsPage(int id, string pageName)
        {
            var rightName = pageName;

            if (!base.HasRights(rightName, UserRightValue.Details))
                return RedirectToAction("UnauthorizedPage");

            base.TraceLog("Details", $"{SessionUser.Current.Username} - {pageName} Details page requested.id- {id}");
            var obj = Activator.CreateInstance(GetEntityObjectType(pageName)) as BaseEntityObject;
            var objInstance = obj as IACSDBObject;
            BaseEntityObject EntityInstance = objInstance.GetItemByID(_db, id);
            AssetTransactionTable itm = EntityInstance as AssetTransactionTable;

            int currentPageID = SessionUser.Current.GetNextPageID();
            ViewBag.CurrentPageID = currentPageID;
            DepreciationClassLineItemModel.RemoveModel(currentPageID);
            var AssetTransactionDetailsDataModel = DepreciationClassLineItemModel.GetModel(currentPageID);

            AssetTransactionDetailsDataModel.LineItems = (from b in DepreciationClassLineItemTable.GetAllItems(_db)
                                                          where b.DepreciationClassID == id
                                                          select new LineItemModel()
                                                          {
                                                              LineItemID = b.DepreciationClassLineItemID,

                                                              StartValue = b.AssetStartValue,
                                                              EndValue = b.AssetEndValue == "*" ? "" : b.AssetEndValue,
                                                              Duration = int.Parse(b.Duration),
                                                              UnEndValue = b.AssetEndValue == "*" ? true : false,

                                                          }).ToList();

            //AssetTransactionDetailsDataModel.AddRange(lines.ToArray());

            return PartialView("DetailsPage",
                new EntryPageModel()
                {
                    PageTitle = pageName,
                    PageName = pageName,
                    EntityInstance = objInstance.GetItemByID(_db, id) as BaseEntityObject
                });
        }
    }
}
