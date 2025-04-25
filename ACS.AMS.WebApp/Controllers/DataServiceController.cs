using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACS.AMS.DAL;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.Collections;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using Org.BouncyCastle.Utilities;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Reflection;
using static SQLite.SQLite3;
using Telerik.SvgIcons;

namespace ACS.AMS.WebApp.Controllers
{
    public class DataServiceController : ACSBaseController
    {
        private static int maxRecordsPerRequest = 200;

        public JsonResult GetAllLoginUserList(string text, string userID, bool custodianOnly = false)
        {
            base.TraceLog("DataService GetAllLoginUserList", $"{SessionUser.Current.Username} -GetAllLoginUserList requested.text-{text},UserID-{userID}");
            if (!string.IsNullOrEmpty(text))
                text = text.ToUpper();
            if (!string.IsNullOrEmpty(userID))
                text = userID.ToUpper();

            var personQry = PersonView.GetAllPerson(_db);
            if (!string.IsNullOrEmpty(text))
            {
                personQry = personQry.Where(c => c.PersonCode.ToUpper().Contains(text.ToUpper()) ||
                                                c.PersonFirstName.ToUpper().Contains(text.ToUpper()) ||
                                                c.PersonLastName.ToUpper().Contains(text.ToUpper()) ||
                                                c.EMailID.ToUpper().Contains(text.ToUpper()) ||
                                                c.Username.ToUpper().Contains(text.ToUpper()));
            }

            var qry = from b in personQry
                      where !custodianOnly ? (b.UserTypeID == 1 || b.UserTypeID == 3) : b.UserTypeID == 2
                      orderby b.PersonCode, b.PersonFirstName, b.PersonLastName
                      select new
                      {
                          PersonCode = b.PersonCode,
                          PersonName = b.PersonFirstName + '-' + b.PersonLastName,
                          UserID = b.PersonID,
                          EMailID = b.EMailID,
                          UserName = b.Username
                      };

            var list = qry.Take(maxRecordsPerRequest);
            return Json(list, new Newtonsoft.Json.JsonSerializerSettings());
        }

        public JsonResult GetAllApprovalRoleList(string text)
        {
            base.TraceLog("DataService GetAllApprovalRoleList", $"{SessionUser.Current.Username} -GetAllApprovalRoleList requested.text-{text}");
            if (!string.IsNullOrEmpty(text))
                text = text.ToUpper();


            var qry = ApprovalRoleTable.GetAllItems(_db).ToList()
                .Select(b => new
                {
                    ApprovalRoleCode = b.ApprovalRoleCode,
                    ApprovalRoleName = b.ApprovalRoleName,
                    ApprovalRoleID = b.ApprovalRoleID,

                }).Distinct();
            if (!string.IsNullOrEmpty(text) && qry.Count() != 0)
            {
                qry = qry.Where(c => c.ApprovalRoleCode.ToUpper().Contains(text.ToUpper()) || c.ApprovalRoleName.ToUpper().Contains(text.ToUpper())

                    );
            }
            var list = qry;
            return Json(qry, new Newtonsoft.Json.JsonSerializerSettings());
        }

        public JsonResult GetAllSecondLevelLocationList(string text)
        {
            base.TraceLog("DataService GetAllSecondLevelLocationList", $"{SessionUser.Current.Username} -GetAllSecondLevelLocationList requested.text-{text}");

            IQueryable<LocationForUserMappingView> dbQry = from b in LocationForUserMappingView.GetAllItems(_db)
                                                               // where b.LocationType != null
                                                           select b;
            if(AppConfigurationManager.GetValue<bool>(AppConfigurationManager.IsMandatoryLocationType))
            {
                dbQry = dbQry.Where(a => a.LocationType != null);
            }

            if (!string.IsNullOrEmpty(text))
            {
                text = text.ToUpper();

                dbQry = dbQry.Where(c =>
                            c.LocationCode.ToUpper().Contains(text.ToUpper())
                            || c.LocationName.ToUpper().Contains(text.ToUpper())
                            || c.ParentLocation.ToUpper().Contains(text.ToUpper())
                            || c.LocationType.ToUpper().Contains(text.ToUpper()));
            }

            var qry = (from b in dbQry
                       select new
                       {
                           LocationCode = b.LocationCode,
                           LocationName = b.LocationName,
                           ParentLocationName = b.ParentLocation,
                           LocationID = b.LocationID,
                           LoctionType = b.LocationType
                       }).Distinct().ToList();

            var list = qry.Take(maxRecordsPerRequest);
            return Json(list, new Newtonsoft.Json.JsonSerializerSettings());
        }

        public ActionResult GetDynamicControlDataForMCombobox(int screenControlQueryID, string text, int? sourceTransactionID)
        {
            try
            {
                base.TraceLog("DataService GetDynamicControlDataForMCombobox", $"{SessionUser.Current.Username} -GetDynamicControlDataForMCombobox requested.text-{text},screenControlQueryID-{screenControlQueryID}");
                //using (var db = AMS.CreateNewEntityObject())
                {
                    if (sourceTransactionID == null)
                        sourceTransactionID = 0;

                    ASelectionControlQueryTable tbl = ASelectionControlQueryTable.GetItem(_db, screenControlQueryID);

                    //add the where condition for the query
                    string whereCondition = "";
                    string[] filterFields = tbl.SearchFields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    if ((!string.IsNullOrEmpty(text)) && (filterFields.Length > 0))
                    {
                        foreach (var f in filterFields)
                        {
                            var field = f.Trim();
                            if (whereCondition.Length > 0)
                                whereCondition += " OR ";

                            whereCondition += $"{field} like @{field}";
                            parameters.Add($"@{field}", $"%{text}%");
                        }
                    }

                    if (whereCondition.Length > 0)
                        whereCondition = " WHERE " + whereCondition;
                    string finalQuery = $"SELECT TOP {maxRecordsPerRequest} * FROM ({tbl.Query}) A {whereCondition} {tbl.OrderByQuery}";
                    //ApplicationErrorLogTable.SaveException(new Exception(finalQuery));
                    if (finalQuery.Contains("@SourceTransactionID") && sourceTransactionID.HasValue)
                    {
                        parameters.Add("@SourceTransactionID", sourceTransactionID);
                    }
                    DataTable dt = AMSContext.GetDataTable(finalQuery, parameters, false);

                    return Json(dt);
                }
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ex);
            }
        }

        public JsonResult GetAvaliblityApprovalRoleList(int locationTypeID)
        {
            base.TraceLog("DataService GetAvaliblityApprovalRoleList", $"{SessionUser.Current.Username} -GetAvaliblityApprovalRoleList requested.locationTypeID-{locationTypeID}");

            var location = (from b in _db.LocationTable where (int)b.LocationTypeID == locationTypeID select b.LocationID).ToList();
            var userMapping = (from b in _db.UserApprovalRoleMappingTable where location.Contains(b.LocationID) && b.StatusID == (int)StatusValue.Active select b.ApprovalRoleID).ToList();
            if (userMapping.Count() > 0)
            {
                IQueryable<ApprovalRoleTable> qry = ApprovalRoleTable.GetAllItems(_db);
                qry = qry.Where(a => userMapping.Contains(a.ApprovalRoleID));
                List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();

                list = (from b in qry
                        select new TextValuePair<string, int>
                        {
                            Text = b.ApprovalRoleName,
                            Value = b.ApprovalRoleID
                        }).ToList();

                var _list1 = new SelectList(list, "Value", "Text", null);
                return Json(new { availableFields = _list1 });
            }
            else
            {
                return null;
            }

        }

        public JsonResult GetImportTemplateFields(int type)
        {
            base.TraceLog("DataService GetImportTemplateFields", $"{SessionUser.Current.Username} -GetImportTemplateFields requested.type-{type}");
            AMSContext db = AMSContext.CreateNewContext();
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();

            var qry = EntityTable.GetAllItems(db, true).Where(a => a.QueryString != null);

            list = (from b in qry
                    orderby b.EntityName
                    select new TextValuePair<string, int>
                    {
                        Text = b.EntityName,
                        Value = b.EntityID
                    }).ToList();

            return Json(new SelectList(list, "Value", "Text"), new Newtonsoft.Json.JsonSerializerSettings());
        }

        public JsonResult GetAvailableFieldsForGenerateTemplate(int entityName, int type)
        {
            base.TraceLog("DataService GetAvailableFieldsForGenerateTemplate", $"{SessionUser.Current.Username} -GetAvailableFieldsForGenerateTemplate requested.entityName-{entityName},type-{type}");
            var result = ImportTemplateNewTable.GetAvailableFields(_db, entityName, type);
            var resultMandatory = ImportTemplateNewTable.GetMandatoryFields(_db, entityName, type);

            //var dynamicColumn = ImportTemplateTable.GetDynamicAvailableFields(_db, appPageID);
            //var dynamicColumnMandatory = ImportTemplateTable.GetMandatoryDynamicFields(_db, appPageID);

            var _list1 = new SelectList(result.OrderBy(a => a.Text), "Value", "Text", null);
            var _list2 = new SelectList(resultMandatory, "Value", "Text", null);

            //var _list3 = new SelectList(dynamicColumn, "Value", "Text", null);
            //var _list4 = new SelectList(dynamicColumnMandatory, "Value", "Text", null);

            // return new JavaScriptSerializer().Serialize(new { availableFields = _list1.Concat(_list3), selectedFields = _list2.Concat(_list4) });
            return Json(new { availableFields = _list1, selectedFields = _list2 });
        }
        public ActionResult GetMandatoryCount(int entityName, int type)
        {
            base.TraceLog("DataService GetMandatoryCount", $"{SessionUser.Current.Username} -GetMandatoryCount requested.{entityName},{type}");
            var resultMandatory = ImportTemplateNewTable.GetMandatoryFields(_db, entityName, type);
            //  var dynamicColumnMandatory = ImportTemplateTable.GetMandatoryDynamicFields(_db, appPageID);
            return Json(new { Success = true, mandatoryCnt = resultMandatory.Count(), dynamicMandatoryCnt = 0 }, new Newtonsoft.Json.JsonSerializerSettings());
        }
        public ActionResult GetCategoryCode(int categoryID)
        {
            base.TraceLog("DataService GetCategoryCode", $"{SessionUser.Current.Username} -GetCategoryCode requested.{categoryID}");
            var resultMandatory = CategoryNewView.GetItem(_db, categoryID);
            //  var dynamicColumnMandatory = ImportTemplateTable.GetMandatoryDynamicFields(_db, appPageID);
            return Json(new { Success = true, code = resultMandatory.CategoryCode }, new Newtonsoft.Json.JsonSerializerSettings());
        }
        public ActionResult GetLocationCode(int LocationID)
        {
            base.TraceLog("DataService GetLocationCode", $"{SessionUser.Current.Username} -GetLocationCode requested {LocationID}");
            var resultMandatory = LocationNewView.GetItem(_db, LocationID);
            //  var dynamicColumnMandatory = ImportTemplateTable.GetMandatoryDynamicFields(_db, appPageID);
            return Json(new { Success = true, code = resultMandatory.LocationCode }, new Newtonsoft.Json.JsonSerializerSettings());
        }
        public ActionResult GetDepartmentCode(int deptID)
        {
            base.TraceLog("DataService GetDepartmentCode", $"{SessionUser.Current.Username} -GetDepartmentCode requested.{deptID}");
            var resultMandatory = DepartmentTable.GetItem(_db, deptID);
            //  var dynamicColumnMandatory = ImportTemplateTable.GetMandatoryDynamicFields(_db, appPageID);
            return Json(new { Success = true, code = resultMandatory.DepartmentCode }, new Newtonsoft.Json.JsonSerializerSettings());
        }
        public ActionResult GetSectionCode(int id)
        {
            base.TraceLog("DataService GetSectionCode", $"{SessionUser.Current.Username} -GetSectionCode requested.{id}");
            var resultMandatory = SectionTable.GetItem(_db, id);
            //  var dynamicColumnMandatory = ImportTemplateTable.GetMandatoryDynamicFields(_db, appPageID);
            return Json(new { Success = true, code = resultMandatory.SectionCode }, new Newtonsoft.Json.JsonSerializerSettings());
        }
        public ActionResult GetSelectMandatoryCount(string IDs, string type, int entityName)
        {
            base.TraceLog("DataService GetSelectMandatoryCount", $"{SessionUser.Current.Username} -GetSelectMandatoryCount requested.{type},{entityName}");
            var result = ImportTemplateNewTable.GetMandatoryCount(_db, IDs, type, entityName);
            return Json(new { Success = true, cnt = result }, new Newtonsoft.Json.JsonSerializerSettings());
        }
        public JsonResult GetTemplateDetails(int? appID, string text = null)
        {
            base.TraceLog("DataService GetTemplateDetails", $"{SessionUser.Current.Username} -GetTemplateDetails requested");
            AMSContext db = AMSContext.CreateNewContext();
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();
            if (appID.HasValue)
            {
                var qry = ImportFormatNewTable.GetListOfFormatsForEntity(db, appID, text, SessionUser.Current.UserID);
                list = (from b in qry
                            //orderby b.TamplateName
                        select new TextValuePair<string, int>
                        {
                            Text = b.TamplateName,
                            Value = b.ImportFormatID,
                        }).ToList();
            }
            return Json(new SelectList(list, "Value", "Text"), new Newtonsoft.Json.JsonSerializerSettings());
        }

        public JsonResult GetReports(int? notificationModuleID = null)
        {
            base.TraceLog("DataService GetReports", $"{SessionUser.Current.Username} -GetReports requested");
            AMSContext db = AMSContext.CreateNewContext();
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();
            if (notificationModuleID.HasValue)
            {
                var templateID = NotificationModuleTable.GetItem(db, (int)(notificationModuleID));
                if (templateID.ReportTemplateID.HasValue)
                {
                    var reports = (from b in db.ReportTable where b.ReportTemplateID == (int)templateID.ReportTemplateID && b.StatusID == (int)StatusValue.Active select b);
                    list = (from b in reports
                            select new TextValuePair<string, int>
                            {
                                Text = b.ReportName,
                                Value = b.ReportID
                            }).ToList();
                }
                else
                {
                    return Json(list, new Newtonsoft.Json.JsonSerializerSettings());
                }
                return Json(new SelectList(list, "Value", "Text"), new Newtonsoft.Json.JsonSerializerSettings());
            }

            else
            {
                return Json(list, new Newtonsoft.Json.JsonSerializerSettings());
            }

        }
        public JsonResult GetReportsFilter(int? notificationModuleID = null)
        {
            base.TraceLog("DataService GetReportsFilter", $"{SessionUser.Current.Username} -GetReportsFilter requested");
            AMSContext db = AMSContext.CreateNewContext();
            List<TextValuePair<string, string>> list = new List<TextValuePair<string, string>>();
            if (notificationModuleID.HasValue)
            {
                var templateID = NotificationModuleTable.GetItem(db, (int)notificationModuleID);

                if (templateID.ReportTemplateID.HasValue)
                {
                    var userReport = (from b in db.ScreenFilterTable where b.ReportTemplateID == (int)templateID.ReportTemplateID select b).FirstOrDefault();
                    if (userReport != null)
                    {
                        var reports = (from b in db.ScreenFilterLineItemTable where b.ScreenFilterID == userReport.ScreenFilterID && b.StatusID == (int)StatusValue.Active select b);
                        list = (from b in reports
                                select new TextValuePair<string, string>
                                {
                                    Text = b.DisplayName,
                                    Value = b.DisplayName
                                }).ToList();
                    }
                    else
                    {
                        return Json(list, new Newtonsoft.Json.JsonSerializerSettings());
                    }
                }
                else
                {
                    return Json(list, new Newtonsoft.Json.JsonSerializerSettings());
                }
                return Json(new SelectList(list, "Value", "Text"), new Newtonsoft.Json.JsonSerializerSettings());
            }

            else
            {
                return Json(list, new Newtonsoft.Json.JsonSerializerSettings());
            }

        }
        public ActionResult GetReportDBObjectsForMCombobox()
        {
            try
            {
                base.TraceLog("DataService GetReportDBObjectsForMCombobox", $"{SessionUser.Current.Username} -GetReportDBObjectsForMCombobox requested");
                using (var db = AMSContext.CreateNewContext())
                {
                    var result = vwReportTemplateObjects.GetAllItems(db);
                    var list = from b in result
                               orderby b.ObjectName
                               select new
                               {
                                   ObjectID = b.ObjectID,
                                   ObjectType = b.ObjectType.Trim(),
                                   ObjectName = b.ObjectName.Trim(),

                                   DisplayValue = b.ObjectName.Trim() + " (" + b.ObjectType.Trim() + ")"
                               };

                    return Json(list.ToList());
                }
            }
            catch (Exception ex)
            {
                return base.ErrorJsonResult(ex);
            }

            return Json("Error");
        }

        public ActionResult GetNotificationDBObjectsForMCombobox()
        {
            try
            {
                base.TraceLog("DataService GetNotificationDBObjectsForMCombobox", $"{SessionUser.Current.Username} -GetNotificationDBObjectsForMCombobox requested");
                using (var db = AMSContext.CreateNewContext())
                {
                    var result = vwNotificationTemplateObjects.GetAllItems(db);
                    var list = from b in result
                               orderby b.ObjectName
                               select new
                               {
                                   ObjectID = b.ObjectID,
                                   ObjectType = b.ObjectType.Trim(),
                                   ObjectName = b.ObjectName.Trim(),

                                   DisplayValue = b.ObjectName.Trim() + " (" + b.ObjectType.Trim() + ")"
                               };

                    return Json(list.ToList());
                }
            }
            catch (Exception ex)
            {
                return base.ErrorJsonResult(ex);
            }

            return Json("Error");
        }
        public object GetNotificationFields(int moduleID)
        {
            base.TraceLog("DataService GetNotificationFields", $"{SessionUser.Current.Username} -GetNotificationFields requested.{moduleID}");
            var result1 = NotificationModuleFieldTable.GetAllItems(_db).Where(c => c.NotificationModuleID == moduleID);
            List<TextValuePair<string, string>> list = new List<TextValuePair<string, string>>();

            list = (from b in result1
                    select new TextValuePair<string, string>
                    {
                        Text = b.FieldName,
                        Value = b.FieldName,
                    }).ToList();

            TextValuePair<string, string> list1 = new TextValuePair<string, string>();
            list1.Text = "<--SELECT-->";
            list1.Value = "<--SELECT-->";
            list.Insert(0, list1);


            return Json(new { data = list });

        }
        public ActionResult GetAllReportTemplate(string text)
        {
            try
            {
                base.TraceLog("DataService GetAllReportTemplate", $"{SessionUser.Current.Username} -GetAllReportTemplate requested.{text}");
                text = !string.IsNullOrEmpty(text) ? text.Trim() : text;
                var dt = ReportTemplateTable.GetAllItems(_db).Where(a => a.QueryString != null);
                if (!string.IsNullOrEmpty(text))
                    dt = dt.Where(b => b.ReportTemplateName.Contains(text) || b.QueryString.Contains(text));

                var list = from b in dt
                           orderby b.ReportTemplateName
                           select new
                           {
                               ReportTemplateName = b.ReportTemplateName.Trim(),
                               ProcedureName = b.QueryString.Trim(),
                               ReportTemplateFile = b.ReportTemplateFile.Trim(),
                               ReportTemplateID = b.ReportTemplateID
                               //DisplayValue = b.WarehouseCode.Trim() + " - " + b.WarehouseName.Trim()
                           };

                return Json(list.Take(maxRecordsPerRequest));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ex);
            }

            return Json("Error");
        }
        public JsonResult GetAvaliblityReportcolumns(int reportTemplateID)
        {
            base.TraceLog("DataService GetAvaliblityReportcolumns", $"{SessionUser.Current.Username} -GetAvaliblityReportcolumns requested.{reportTemplateID}");
            var result = (from b in ReportTemplateFieldTable.GetAllReportTemplateFields(_db, reportTemplateID)
                          orderby b.DisplayTitle
                          select new TextValuePair<string, int>
                          {
                              Text = b.DisplayTitle,
                              Value = b.ReportTemplateFieldID
                          }).ToList();

            var _list1 = new SelectList(result, "Value", "Text", null);


            var _list2 = new SelectList(result, "Value", "Text", null);

            return Json(new { availableFields = _list1, availableReportFields = _list2 });
        }
        public JsonResult GetAllDepartmentList(string text)
        {
            base.TraceLog("DataService GetAllDepartmentList", $"{SessionUser.Current.Username} -GetAllDepartmentList requested.{text}");
            if (!string.IsNullOrEmpty(text))
                text = text.ToUpper();

            var qry = DepartmentTable.GetAllItems(_db);
            if (!string.IsNullOrEmpty(text))
            {
                qry = qry.Where(c => c.DepartmentCode.ToUpper().Contains(text.ToUpper()) || c.DepartmentName.ToUpper().Contains(text.ToUpper()));
            }
            var lists = (from b in qry
                         orderby b.DepartmentCode, b.DepartmentName
                         select new
                         {
                             DepartmentCode = b.DepartmentCode,
                             DepartmentName = b.DepartmentName,
                             DepartmentID = b.DepartmentID
                         }
                      );


            var list = lists.Take(maxRecordsPerRequest);
            return Json(list, new Newtonsoft.Json.JsonSerializerSettings());

        }
        public JsonResult GetAllSectionList(string text)
        {
            base.TraceLog("DataService GetAllSectionList", $"{SessionUser.Current.Username} -GetAllSectionList requested.{text}");
            if (!string.IsNullOrEmpty(text))
                text = text.ToUpper();

            var qry = SectionTable.GetAllItems(_db);
            if (!string.IsNullOrEmpty(text))
            {
                qry = qry.Where(c => c.SectionCode.ToUpper().Contains(text.ToUpper()) || c.SectionName.ToUpper().Contains(text.ToUpper()));
            }
            var lists = (from b in qry
                         orderby b.SectionCode, b.SectionName
                         select new
                         {
                             SectionCode = b.SectionCode,
                             SectionName = b.SectionName,
                             SectionID = b.SectionID
                         }
                      );


            var list = lists.Take(maxRecordsPerRequest);
            return Json(list, new Newtonsoft.Json.JsonSerializerSettings());

        }
        public JsonResult GetAllDesignationList(string text)
        {
            base.TraceLog("DataService GetAllDesignationList", $"{SessionUser.Current.Username} -GetAllDesignationList requested.{text}");
            if (!string.IsNullOrEmpty(text))
                text = text.ToUpper();
            var qry = DesignationTable.GetAllItems(_db);
            if (!string.IsNullOrEmpty(text))
            {
                qry = qry.Where(c => c.DesignationCode.ToUpper().Contains(text.ToUpper()) || c.DesignationName.ToUpper().Contains(text.ToUpper()));
            }
            var lists = (from b in qry
                         orderby b.DesignationCode, b.DesignationName
                         select new
                         {
                             DesignationCode = b.DesignationCode,
                             DesignationName = b.DesignationName,
                             DesignationID = b.DesignationID
                         }
                   );


            var list = lists.Take(maxRecordsPerRequest);
            return Json(list, new Newtonsoft.Json.JsonSerializerSettings());

        }
        [HttpGet]
        public ActionResult GetNotificationTypeDetails(int? notificationTypeID)
        {
            base.TraceLog("DataService GetNotificationTypeDetails", $"{SessionUser.Current.Username} -GetNotificationTypeDetails requested.");
            var result = new
            {
                IsHtmlContent = false,
                IsAttachmentRequired = false
            };
            if (notificationTypeID.HasValue)
            {
                if (notificationTypeID != -1)
                {
                    var notificationType = NotificationTypeTable.GetItem(_db, notificationTypeID.Value);
                    if (notificationType != null)
                    {

                        result =
                        new
                        {
                            IsHtmlContent = notificationType.AllowHTMLContent != null ? (bool)notificationType.AllowHTMLContent : false,
                            IsAttachmentRequired = notificationType.IsAttachmentRequired != null ? (bool)notificationType.IsAttachmentRequired : false
                        };


                    }
                }
            }

            return Json(result);
        }

        public JsonResult GetAllMulticolumncomboboxAsset(string text, int? sourceTransactionID)
        {
            base.TraceLog("DataService GetAllMulticolumncomboboxAsset", $"{SessionUser.Current.Username} -GetAllMulticolumncomboboxAsset requested.{text}");
            
            if (!string.IsNullOrEmpty(text))
                text = text.ToUpper();

            var qry = AssetNewView.GetAllItems(_db).Where(a => a.StatusID == (int)StatusValue.Active /*&& a.LocationType != null*/);

            if (sourceTransactionID != null)
            {
                qry = (from q in qry
                       join t in _db.TransactionLineItemTable on q.AssetID equals t.AssetID
                       where t.TransactionID == (int)sourceTransactionID
                       select q);
            }

            if (!string.IsNullOrEmpty(text))
            {
                qry = qry.Where(c => c.AssetCode.ToUpper().Contains(text.ToUpper()) || c.Barcode.ToUpper().Contains(text.ToUpper()));
            }

            var lists = (from b in qry
                         orderby b.AssetCode, b.Barcode
                         select new
                         {
                             AssetCode = b.AssetCode,
                             AssetDescription = b.AssetDescription,
                             Barcode = b.Barcode,
                             AssetID = b.AssetID,
                             LocationType = b.LocationType,
                             CategoryType = b.CategoryType,
                             Category = b.CategoryHierarchy,
                             Location = b.LocationHierarchy,
                             DisplayValue = b.AssetCode + '-' + b.Barcode
                         }
                  );


            var list = lists.Take(maxRecordsPerRequest);

            return Json(list, new Newtonsoft.Json.JsonSerializerSettings());
        }

        public JsonResult GetAllPONumber(string text)
        {
            base.TraceLog("DataService GetAllPONumber", $"{SessionUser.Current.Username} -GetAllPONumber requested.{text}");
            if (!string.IsNullOrEmpty(text))
                text = text.ToUpper();

            var qry = ZDOF_PurchaseOrderTable.GetAllItems(_db).Where(x => x.AssetsCreated == false);
            if (!string.IsNullOrEmpty(text))
            {
                qry = qry.Where(c => c.PO_NUMBER.ToUpper().Contains(text.ToUpper()));
            }

            var lists = (from b in qry
                         orderby b.PO_NUMBER
                         select new
                         {
                             PO_NUMBER = b.PO_NUMBER,
                             VENDOR_NAME = b.VENDOR_NAME,
                             VENDOR_NUMBER = b.VENDOR_NUMBER,
                             PO_HEADER_ID = b.PO_HEADER_ID,
                             PO_DATE = b.PO_DATE//string.IsNullOrEmpty(b.PO_DATE + "") ? "" : b.PO_DATE.Value.ToString(CultureHelper.ConfigureDateFormat)
                         }
                       ).Distinct();
            var list = lists.Take(maxRecordsPerRequest);
            return Json(list, new Newtonsoft.Json.JsonSerializerSettings());
        }

        public JsonResult GetAllPOLineitemNumber(string poNumber, string text)
        {
            base.TraceLog("DataService GetAllPOLineitemNumber", $"{SessionUser.Current.Username} -GetAllPOLineitemNumber requested.{poNumber},{text}");
            var poLineQry = ZDOF_PurchaseOrderTable.GetAllItems(_db);
            bool allowMoreThanPOQty = true; //AppConfigurationManager.GetValue<bool>(AppConfigurationManager.AllowMoreQtyThenPOQty);

            if (allowMoreThanPOQty)
            {
                poLineQry = poLineQry.Where(x => x.PO_HEADER_ID == poNumber);
            }
            else
            {
                poLineQry = poLineQry.Where(x => x.AssetsCreated == false && x.PO_HEADER_ID == poNumber);
            }
            if (!string.IsNullOrEmpty(text))
            {
                poLineQry = poLineQry.Where(c => (c.LINE_NUM.ToUpper().Contains(text.ToUpper()) || c.ITEM_DESCRIPTION.ToUpper().Contains(text.ToUpper())
                                || c.QUANTITY.ToString().Contains(text.ToUpper())));
            }

            var qry = (from b in poLineQry
                       select new
                       {
                           LINE_NUM = b.LINE_NUM,
                           ITEM_DESCRIPTION = b.PO_LINE_DESC,
                           QUANTITY = b.QUANTITY, //- b.GeneratedAssetQty,
                           PO_LINE_ID = b.PO_LINE_ID,
                           PO_HEADER_ID = b.PO_HEADER_ID,
                           DisplayText = b.LINE_NUM + " - " + b.PO_LINE_DESC
                       }).Distinct();

            var list = qry.Take(maxRecordsPerRequest);
            return Json(list, new Newtonsoft.Json.JsonSerializerSettings());

        }
        public ActionResult GetAssetPOType(string poLineItemID)
        {
            base.TraceLog("DataService GetAssetPOType", $"{SessionUser.Current.Username} -GetAssetPOType requested.{poLineItemID}");
            //  GenerateAssetFromPOModel.All().Clear();
            var result = ZDOF_PurchaseOrderTable.GetAllItems(_db).Where(a => a.PO_LINE_ID == poLineItemID).FirstOrDefault();
            if (result != null)
            {
                string type = string.Empty;
                if (string.Compare(result.VENDOR_SITE_CODE.ToUpper(), "ASSET") == 0)
                {
                    type = "18";
                }
                else
                {
                    type = "19";
                }
                return Json(new { Success = true, assetType = type, qty = result.QUANTITY, prodName = result.PO_LINE_DESC, generateQty = result.GeneratedAssetQty, purchaseOrderID = result.ZDOFPurchaseOrderID }, new Newtonsoft.Json.JsonSerializerSettings());
            }
            else
            {
                return Json(new { Success = false }, new Newtonsoft.Json.JsonSerializerSettings());
            }
        }

        public ActionResult GetHierarchDetails(int id, string type)
        {
            base.TraceLog("DataService GetHierarchDetails", $"{SessionUser.Current.Username} -GetHierarchDetails requested.{id},{type}");
            string result = string.Empty;

            if (string.Compare(type, "Category") == 0)
            {
                result = (from b in _db.CategoryTable where b.CategoryID == id select b.CategoryName).FirstOrDefault();
            }
            else
            {

                result = (from b in _db.LocationTable where b.LocationID == id select b.LocationName).FirstOrDefault();
            }

            return Json(new { Success = true, hierarchy = result }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        public JsonResult GetCategoryListForDDTree(int? id, int? typeID)
        {
            base.TraceLog("DataService GetCategoryListForDDTree", $"{SessionUser.Current.Username} -GetCategoryListForDDTree requested.");
          //  _db.EnableInstanceQueryLog = true;
            string attribute = "YES";
            string Attribute1 = "p";
            if (typeID.HasValue)
            {
                if (typeID == 19)
                {
                    attribute = "No";
                }
            }

            List<int> allparentnode = new List<int>();
            IQueryable<CategoryTable> category = CategoryTable.GetAllItems(_db);
            bool mapping = false;
            if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.UserCategoryMapping))
            {
                var userLocation = UserCategoryMappingTable.GetAllUserItems(_db, SessionUser.Current.UserID).Where(a=>a.PersonID== SessionUser.Current.UserID);
                var location = userLocation.Where(a=>a.StatusID!=(int)StatusValue.DeletedOLD || a.StatusID!=(int)StatusValue.Deleted).Select(a => a.CategoryID).ToList();

                category = CategoryTable.GetAllItems(_db).Where(a => (from b in _db.CategoryNewView where location.Contains((int)b.MappedCategoryID) || b.MappedCategoryID == null select (int)b.CategoryID)
                .Contains(a.CategoryID));
                mapping = true;
            }

            var categories = from e in category
                             where (id.HasValue ? e.ParentCategoryID == id : e.ParentCategoryID == null) &&

                            (typeID.HasValue ? (e.Attribute1 == Attribute1 || e.Attribute1 == attribute) : 1 == 1)
                             orderby e.CategoryName
                             select new
                             {
                                 id = e.CategoryID,
                                 Name = e.CategoryName,
                                 hasChildren = (from q in _db.CategoryTable
                                                where (q.ParentCategoryID == e.CategoryID)
                                                select q
                                                ).Count() > 0
                             };

            return Json(categories.ToList());
        }

        public JsonResult GetLocationListForDDTree(int? id)
        {
            try
            {
                base.TraceLog("DataService GetLocationListForDDTree", $"{SessionUser.Current.Username} -GetLocationListForDDTree requested.");
                List<int> allparentnode = new List<int>();
                IQueryable<LocationNewView> Loc = LocationNewView.GetAllItems(_db);

                if ((AppConfigurationManager.GetValue<bool>(AppConfigurationManager.UserLocationMapping)) && (id.HasValue))
                {
                    var userLocation = UserLocationMappingTable.GetAllUserItems(_db, SessionUser.Current.UserID).Where(a=>a.PersonID== SessionUser.Current.UserID);
                    Loc = from c in LocationNewView.GetAllItems(_db)
                          join d in userLocation on c.MappedLocationID equals d.LocationID
                          //where c.ParentLocationID == id.Value
                          select c;
                }
                else
                {
                    Loc = from c in LocationNewView.GetAllItems(_db)
                              //where c.ParentLocationID == null
                          select c;
                }

                var categories = from e in Loc.Distinct()
                                 where (id.HasValue) ? e.ParentLocationID == id.Value : e.ParentLocationID == null
                                    && e.StatusID != (int)StatusValue.Deleted
                                    && e.StatusID != (int)StatusValue.DeletedOLD
                                 orderby e.LocationName
                                 select new
                                 {
                                     id = e.LocationID,
                                     Name = e.LocationName + " (" + e.LocationCode + ")",
                                     hasChildren = (from q in _db.LocationTable
                                                    where (q.ParentLocationID == e.LocationID)
                                                    select q).Count() > 0
                                 };

                var list = categories.ToList();
                return Json(list);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ex);
            }
        }

        [HttpPost]
        public object CheckBarcodePrefixDetails(string categoryCode = null, string locationCode = null, string departmentCode = null, string sectionCode = null, string customPrefixLength = null, string customPrefix = null)
        {
            base.TraceLog("DataService CheckBarcodePrefixDetails", $"{SessionUser.Current.Username} -CheckBarcodePrefixDetails requested.");
            bool result1 = BarcodeConstructTable.checkPrefixData(AMSContext.CreateNewContext(), categoryCode, locationCode, departmentCode, sectionCode, customPrefixLength, customPrefix);
            return Json(new { datas = result1 }, new Newtonsoft.Json.JsonSerializerSettings());

        }
        public ActionResult GetDashboardDBObjectsForMCombobox()
        {
            try
            {
                base.TraceLog("DataService GetDashboardDBObjectsForMCombobox", $"{SessionUser.Current.Username} -GetDashboardDBObjectsForMCombobox requested.");
                using (var db = AMSContext.CreateNewContext())
                {
                    var result = vwDashboardTemplateObjects.GetAllItems(db);
                    var list = from b in result
                               orderby b.ObjectName
                               select new
                               {
                                   ObjectID = b.ObjectID,
                                   ObjectType = b.ObjectType.Trim(),
                                   ObjectName = b.ObjectName.Trim(),

                                   DisplayValue = b.ObjectName.Trim() + " (" + b.ObjectType.Trim() + ")"
                               };

                    return Json(list.ToList());
                }
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ex);
            }

            return Json("Error");
        }

        public ActionResult GetAllDashboardTemplate(string text)
        {
            try
            {
                base.TraceLog("DataService GetAllDashboardTemplate", $"{SessionUser.Current.Username} -GetAllDashboardTemplate requested.");
                text = !string.IsNullOrEmpty(text) ? text.Trim() : text;
                var dt = DashboardTemplateTable.GetAllItems(_db);
                if (!string.IsNullOrEmpty(text))
                    dt = dt.Where(b => b.DashboardTemplateName.Contains(text) || b.QueryString.Contains(text));

                var list = from b in dt
                           orderby b.DashboardTemplateName
                           select new
                           {
                               DashboardTemplateName = b.DashboardTemplateName.Trim(),
                               ProcedureName = b.QueryString.Trim(),
                               DashboardTemplateID = b.DashboardTemplateID
                               //DisplayValue = b.WarehouseCode.Trim() + " - " + b.WarehouseName.Trim()
                           };

                return Json(list.Take(maxRecordsPerRequest));
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ex);
            }

            return Json("Error");
        }
        public JsonResult GetAvaliblityDashboardcolumns(int dashboardTemplateID)
        {
            base.TraceLog("DataService GetAvaliblityDashboardcolumns", $"{SessionUser.Current.Username} -GetAvaliblityDashboardcolumns requested.");
            var result = (from b in DashboardTemplateTable.GetAllDashboardTemplateFields(_db, dashboardTemplateID)
                          orderby b.DisplayTitle
                          select new TextValuePair<string, int>
                          {
                              Text = b.DisplayTitle,
                              Value = b.DashboardTemplateFieldID
                          }).ToList();

            var _list1 = new SelectList(result, "Value", "Text", null);
            return Json(new { availableFields = _list1 });
        }

        public JsonResult GetDashboardTemplateFields(int? templateID, string text = null)
        {
            base.TraceLog("DataService GetDashboardTemplateFields", $"{SessionUser.Current.Username} -GetDashboardTemplateFields requested.");
            AMSContext db = AMSContext.CreateNewContext();
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();
            if (templateID.HasValue)
            {
                var qry = DashboardTemplateTable.GetAllDashboardTemplateFields(db, (int)templateID);

                list = (from b in qry
                        orderby b.FieldName
                        select new TextValuePair<string, int>
                        {
                            Text = b.FieldName,
                            Value = b.DashboardTemplateFieldID
                        }).ToList();
            }

            return Json(new SelectList(list, "Value", "Text"), new Newtonsoft.Json.JsonSerializerSettings());
        }

        public JsonResult GetLocationTypeFields(int? moduleID = null, string text = null)
        {
            base.TraceLog("DataService GetLocationTypeFields", $"{SessionUser.Current.Username} -GetLocationTypeFields requested.");
            AMSContext db = AMSContext.CreateNewContext();
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();
            IQueryable<LocationTypeTable> items = LocationTypeTable.GetAllItems(db);
            if (moduleID.HasValue && moduleID.Value == (int)ApproveModuleValue.InternalAssetTransfer)
            {
                items = items.Where(a => a.LocationTypeName == "Head Quarters");

            }
            list = (from b in items
                    orderby b.LocationTypeName
                    select new TextValuePair<string, int>
                    {
                        Text = b.LocationTypeName,
                        Value = b.LocationTypeID
                    }).ToList();
            return Json(new SelectList(list, "Value", "Text"), new Newtonsoft.Json.JsonSerializerSettings());
        }

        public object CheckRights(int entityName, int type)
        {
            base.TraceLog("DataService CheckRights", $"{SessionUser.Current.Username} -CheckRights requested.");
            string rightName = EntityTable.GetItem(AMSContext.CreateNewContext(), entityName).EntityName;
            if (string.Compare(rightName, "POLineItemUpdate") == 0 || string.Compare(rightName, "CustomAsset") == 0)
            {
                rightName = "Asset";
            }
            if (string.Compare(rightName, "UserApprovalRoleMapping") == 0 || string.Compare(rightName, "UserLocationMapping") == 0 || string.Compare(rightName, "UserCategoryMapping") == 0 || string.Compare(rightName, "Custodian") == 0)
            {
                rightName = "UserMaster";
            }
            if (string.Compare(rightName, "ImportAssetTransfer") == 0 )
            {
                rightName = "InternalAssetTransfer";
            }
            if (string.Compare(rightName, "BulkDisposalAsset") == 0)
            {
                rightName = "AssetRetirement";
            }
            if (type == 1)
            {
                bool check = base.HasRights(rightName, UserRightValue.Create);
                return Json(new { Result = "Success", rights = check == true ? 1 : 0 }, new Newtonsoft.Json.JsonSerializerSettings());
            }
            else
            {
                bool check = base.HasRights(rightName, UserRightValue.Edit);
                return Json(new { Result = "Success", rights = check == true ? 1 : 0 }, new Newtonsoft.Json.JsonSerializerSettings());
            }


        }
        public object CheckTemplate(int template)
        {
            base.TraceLog("DataService CheckTemplate", $"{SessionUser.Current.Username} -CheckTemplate requested.");
            var list = ImportFormatNewTable.GetItem(_db, template);
            return Json(new { Result = "Success", appID = list.EntityID,typeID=list.ImportTemplateTypeID }, new Newtonsoft.Json.JsonSerializerSettings());
        }
        public JsonResult ProductDetailsAgainstCategory(string text,int? categoryID=null)
        {
            base.TraceLog("DataService GetAllSecondLevelLocationList", $"{SessionUser.Current.Username} -GetAllSecondLevelLocationList requested.text-{text}");

            IQueryable<ProductTable> dbQry = (from b in _db.ProductTable.Include("Category") where b.StatusID == (int)StatusValue.Active select b);
            if(categoryID.HasValue)
            {
                dbQry=dbQry.Where(a=>a.CategoryID==categoryID.Value);
            }
            

            if (!string.IsNullOrEmpty(text))
            {
                text = text.ToUpper();

                dbQry = dbQry.Where(c =>
                            c.ProductCode.ToUpper().Contains(text.ToUpper())
                            || c.ProductName.ToUpper().Contains(text.ToUpper())
                            || c.VirtualBarcode.ToUpper().Contains(text.ToUpper())
                            );
            }

            var qry = (from b in dbQry
                       select new
                       {
                           ProductCode = b.ProductCode,
                           ProductName = b.ProductName,
                           CategoryName = b.Category.CategoryName,
                           CatalogueImage = string.Concat("/", b.CatalogueImage.Replace("\\", "/")),
                           VirtualBarcode = b.VirtualBarcode,
                           ProductID = b.ProductID
                       }).Distinct().ToList();

            var list = qry.Take(maxRecordsPerRequest);
            return Json(list, new Newtonsoft.Json.JsonSerializerSettings());
        }
        public JsonResult GetAllAssetPONumber(string text)
        {
            if (!string.IsNullOrEmpty(text))
                text = text.ToUpper();

            var qry = AssetTable.GetAllAssets(_db).Where(x => x.StatusID == (int)StatusValue.Active
                                && x.PONumber != null && x.InvoiceNo != null && x.DOFPO_LINE_NUM != null)
                .Select(b => new
                {
                    PO_NUMBER = b.PONumber,
                    InvoiceNo = b.InvoiceNo,
                    dofpo = b.DOFPO_LINE_NUM,
                }).Distinct();

            if (!string.IsNullOrEmpty(text) && qry.Count() != 0)
            {
                qry = qry.Where(c => c.PO_NUMBER.ToUpper().Contains(text.ToUpper()));
            }

            var list = qry.Take(maxRecordsPerRequest);
            return Json(list, new Newtonsoft.Json.JsonSerializerSettings());
        }
        public async Task<JsonResult> GetAllMassAdditionalPONumber(string text)
        {
            DataAccess data = new DataAccess();
            SqlConnection conn = new SqlConnection(AMSContext.ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "Select * from ZDOF_MassAdditionView";
            var qry = data.GetDateTable(cmd);

            if (!string.IsNullOrEmpty(text))
                    text = text.ToUpper();

            var dataList = qry.AsEnumerable()
                           .Select(row => new
                           {
                               PO_NUMBER = row.Field<string?>("PONumber"),
                               InvoiceNo = row.Field<int?>("InvoiceLineNo"),
                               POLINENO = row.Field<int?>("POLineNo"),
                               ItemDesc = row.Field<string?>("ItemDescription"),
                               AssetCost = row.Field<decimal?>("AssetCost"),
                               id = row.Field<int>("MassAdditionID"),
                               // Add more columns as needed
                           })
                           .Distinct();


            if (!string.IsNullOrEmpty(text))
            {
                dataList = dataList.Where(c =>
                    (!string.IsNullOrEmpty(c.PO_NUMBER) && c.PO_NUMBER.Contains(text)) ||
                    (!string.IsNullOrEmpty(c.ItemDesc) && c.ItemDesc.Contains(text))
                );
            }

            var list =  dataList.Take(maxRecordsPerRequest).ToList();

                return Json(list);
            }

        public JsonResult GetAllAttribute(string tableName, string dataType)
        {
            AMSContext db = AMSContext.CreateNewContext();

            var qry = AttributeDefinitionTable.GetTableWiseAttribute(db, tableName, dataType, true);

            List<TextValuePair<string, string>> list = new List<TextValuePair<string, string>>();

            list = (from b in qry
                    select new TextValuePair<string, string>
                    {
                        Text = b,
                        Value = b,

                    }).ToList();

            return Json(new SelectList(list, "Value", "Text"));
        }
        public JsonResult GetDropdownData(string fieldName)
        {
            AMSContext _db = AMSContext.CreateNewContext();
                 var modelType = GetEntityObjectType(fieldName);
            var selectList = EntityHelper.GetSelectList(modelType, _db, SessionUser.Current.UserID);
                var data = EntityHelper.GetSelectList(modelType, _db, SessionUser.Current.UserID);
            return Json(data);
        }
    }
}
