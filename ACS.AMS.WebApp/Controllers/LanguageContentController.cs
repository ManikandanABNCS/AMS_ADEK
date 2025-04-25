
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
using ACS.AMS.WebApp.Classes;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ACS.AMS.WebApp.Controllers
{
    public class LanguageContentController : ACSBaseController
    {
        public IActionResult Index()
        {
            if (!base.HasRights(RightNames.LanguageContent, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");

            SystemDatabaseHelper.GenerateMasterGridColumns(typeof(LanguageContentTable), "LanguageContent");
            base.TraceLog("LanguageContent Index", $"{SessionUser.Current.Username} -LanguageContent Index page requested");
            return PartialView();
        }

        public IActionResult _Index([DataSourceRequest] DataSourceRequest request)
        {
            if (!base.HasRights(RightNames.LanguageContent, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");
            var query = LanguageContentTable.GetAllLanguageContentTable(_db);

            var dsResult = request.ToDataSourceResult(query, "LanguageContent");
            base.TraceLog("LanguageContent Index", $"{SessionUser.Current.Username} -LanguageContent Index page Data Fetch");
            return Json(dsResult);
        }

        public ActionResult Create()
        {
            if (!base.HasRights(RightNames.LanguageContent, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("LanguageContent Create", $"{SessionUser.Current.Username} -LanguageContent Create page requested");
            return PartialView(new LanguageContentTable());
        }

        [HttpPost]
        public ActionResult Create(IFormCollection collection, LanguageContentTable item)
        {
            if (!base.HasRights(RightNames.LanguageContent, UserRightValue.Create))
                return RedirectToAction("UnauthorizedPage");

            try
            {
                base.TraceLog("LanguageContent Create Post", $"{SessionUser.Current.Username} -LanguageContent details will add to db ");
                if (ModelState.IsValid)
                {
                    item.StatusID = (int)StatusValue.Active;
                    _db.Add(item);
                    var languageTable = LanguageTable.GetAllLanguageTable(_db);
                    var defaultLanguageID = languageTable.Where(c => c.IsDefault == true).FirstOrDefault().LanguageID;
                    string defaultDescription = string.Empty;
                    int cnt = 0;
                    foreach (LanguageTable language in languageTable)
                    {
                       string description = collection["LanguageContent_" + language.LanguageID];
                      
                        int languageID = language.LanguageID;
                        if (languageID == defaultLanguageID)
                            defaultDescription = description;

                        if (string.IsNullOrEmpty(description))
                        {
                            if (languageID == defaultLanguageID)
                                return ErrorActionResult("LanguageContent Description Should be Entered");
                            else
                                description = defaultDescription;

                            if (cnt == 0)
                                ModelState.AddModelError("LanguageContent_"+ language.LanguageID, Language.GetString("Atleast One LanguageContent Description Should be Entered"));

                            cnt++;
                            continue;
                        }
                            LanguageContentLineItemTable newLineItem = new LanguageContentLineItemTable();
                            newLineItem.LanguageContent = description;
                            newLineItem.LanguageID = language.LanguageID;
                            newLineItem.LanguageContentNavigation = item;
                        
                    }
                    ModelState.Clear();
                    _db.SaveChanges();
                    base.TraceLog("LanguageContent Create Post", $"{SessionUser.Current.Username} -LanguageContent details saved to db ");
                    Language.ClearContents();
                    return PartialView("SuccessAction");
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
            
            return PartialView(item);
        }
        public ActionResult Edit(int id)
        {
            if (!base.HasRights(RightNames.LanguageContent, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("LanguageContent Edit", $"{SessionUser.Current.Username} -LanguageContent Edit page requested ");
            LanguageContentTable content = LanguageContentTable.GetLanguageContent(_db, id);
            base.TraceLog("LanguageContent Edit", $"LanguageContent Edit page requested LanguageContentID {id}");
            return PartialView(content);
        }

        [HttpPost]
        public ActionResult Edit(LanguageContentTable item, IFormCollection data)
        {
            if (!base.HasRights(RightNames.LanguageContent, UserRightValue.Edit))
                return RedirectToAction("UnauthorizedPage");
            try
            {
                base.TraceLog("LanguageContent Edit Post", $"LanguageContent details will modify to db : LanguageContent id- {item.LanguageContentID}");
                if (ModelState.IsValid)
                {
                    LanguageContentTable oldItm = LanguageContentTable.GetLanguageContent(_db, item.LanguageContentID);

                    DataUtilities.CopyObject(item, oldItm);

                    //List<string> allKeys = data.Where(x => x.StartsWith("LanguageContent_")).ToList();
                    var languageTable = LanguageTable.GetAllLanguageTable(_db);
                    var defaultLanguageID = languageTable.Where(c => c.IsDefault == true).FirstOrDefault().LanguageID;
                    var oldLineItems = LanguageContentLineItemTable.GetLanguageContentDetails(_db, item.LanguageContentID).ToList();
                    string defaultDescription = oldLineItems.Where(x => x.LanguageID== defaultLanguageID).FirstOrDefault().LanguageContent;
                    foreach (LanguageTable language in languageTable)
                    {
                        string description = data["LanguageContent_"+ language.LanguageID];
                        if (string.IsNullOrEmpty(description))
                        {
                            description = defaultDescription;
                        }
                        
                        int languageID = language.LanguageID;
                        var languageDescription = oldLineItems.Where(x => x.LanguageID == languageID).FirstOrDefault();
                        if (languageDescription != null)
                        {
                            languageDescription.LanguageContent = description;
                            languageDescription.LanguageID = language.LanguageID;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(description))
                            {
                                LanguageContentLineItemTable newLanguageContent = new LanguageContentLineItemTable();
                                newLanguageContent.LanguageContentID = oldItm.LanguageContentID;
                                newLanguageContent.LanguageID = language.LanguageID;
                                newLanguageContent.LanguageContent = description;

                                _db.Add(newLanguageContent);
                            }
                        }
                    }


                    _db.SaveChanges();
                    base.TraceLog("LanguageContent Edit Post", $"LanguageContent details modified to db : LanguageContent id- {item.LanguageContentID}");
                    //Language.ClearContents();

                    return PartialView("SuccessAction");
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
          
            return PartialView(item);
        }

        public ActionResult Details(int id)
        {
            if (!base.HasRights(RightNames.LanguageContent, UserRightValue.Details))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("LanguageContent Details", $"{SessionUser.Current.Username} -LanguageContent Detail Page requested.id-{id}");
            LanguageContentTable content = LanguageContentTable.GetLanguageContent(_db, id);
            return PartialView(content);
        }

        public IActionResult Delete(int id)
        {
            if (!base.HasRights(RightNames.LanguageContent, UserRightValue.Delete))
                return RedirectToAction("UnauthorizedPage");
            base.TraceLog("LanguageContent delete", $"LanguageContent details page requested LanguageContentID {id}");
            var item = LanguageContentTable.GetLanguageContent(_db, id);
            item.StatusID = (int)StatusValue.Deleted;
            _db.SaveChanges();
            base.TraceLog("LanguageContent delete", $"LanguageContent details page deleted successfully LanguageContentID {id}");
            return PartialView("SuccessAction");
        }


      

    }
}