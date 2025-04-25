using ACS.AMS.DAL;
using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using System.Data;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ACS.AMS.DAL
{
    public class ComboBoxHelper
    {
        public static List<int> StringToList(string commaString, char splitStr)
        {
            string[] arryAssetID = commaString.Split(new char[] { splitStr }, StringSplitOptions.RemoveEmptyEntries);
            int[] intIDS = Array.ConvertAll(arryAssetID, s => int.Parse(s));
            List<int> lst = intIDS.OfType<int>().ToList();
            return lst;
        }
        public static int[] StringToListDynamicGrid(string commaString, char splitStr)
        {
            string[] arryAssetID = commaString.Split(new char[] { splitStr }, StringSplitOptions.RemoveEmptyEntries);
            int[] intIDS = Array.ConvertAll(arryAssetID, s => int.Parse(s));
            return intIDS;
        }
        public static SelectList GetUserType(AMSContext context)
        {
            var qry = UserTypeTable.GetAllUserType(context);
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();

            foreach (var nm in qry)
            {
                TextValuePair<string, int> val = new TextValuePair<string, int>();
                val.Text = nm.UserType;
                val.Value = nm.UserTypeID;
                list.Add(val);
            }

            return new SelectList(list, "Value", "Text");
        }

        public static SelectList GetAllRolesForCombo(AMSContext model, string defaultItem = "")
        {
            var items = User_RoleTable.GetAllRole(model);
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();

            foreach (var nm in items)
            {
                TextValuePair<string, int> val = new TextValuePair<string, int>();
                val.Text = nm.RoleName;
                val.Value = nm.RoleID;
                list.Add(val);
            }

            if (!string.IsNullOrEmpty(defaultItem))
            {
                TextValuePair<string, int> val = new TextValuePair<string, int>();
                val.Value = 0;
                val.Text = defaultItem;
                list.Insert(0, val);
            }
            return new SelectList(list, "Value", "Text");
        }
        public static SelectList GetAvailableRole(AMSContext model, int? userID = null)
        {
            IQueryable<User_RoleTable> qry = User_RoleTable.GetAllRole(model);
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();
            if (userID.HasValue)
            {
                var oldList = (from b in model.User_UserRoleTable where b.UserID == userID select b);//select b.RoleID).ToList();
                var res = (from b in qry
                           join c in oldList on b.RoleID equals c.RoleID into hj
                           from subgroup in hj.DefaultIfEmpty()
                           where subgroup == null
                           select new { b.RoleID, b.RoleName });

                list = (from b in res
                        orderby b.RoleName
                        select new TextValuePair<string, int>
                        {
                            Text = b.RoleName,
                            Value = b.RoleID
                        }).ToList();

                return new SelectList(list, "Value", "Text");

                // qry = qry.Where(a => !oldList.Contains(a.RoleID));
            }
            list = (from b in qry
                    orderby b.RoleName
                    select new TextValuePair<string, int>
                    {
                        Text = b.RoleName,
                        Value = b.RoleID
                    }).ToList();

            return new SelectList(list, "Value", "Text");
        }
        public static SelectList GetSelectedRole(AMSContext model, int? userID = null)
        {
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();
            IQueryable<User_RoleTable> qry = User_RoleTable.GetAllRole(model);
            if (userID.HasValue)
            {
                var oldList = (from b in model.User_UserRoleTable where b.UserID == userID select b); //select b.RoleID).ToList();
                qry = (from b in qry join c in oldList on b.RoleID equals c.RoleID select b); //qry.Where(a => oldList.Contains(a.RoleID));
            }
            list = (from b in qry
                    select new TextValuePair<string, int>
                    {
                        Text = b.RoleName,
                        Value = b.RoleID
                    }).ToList();

            return new SelectList(list, "Value", "Text");

        }
        public static SelectList GetGender()
        {
            List<TextValuePair<string, string>> list = new List<TextValuePair<string, string>>();
            TextValuePair<string, string> val = new TextValuePair<string, string>();
            val.Value = "M";
            val.Text = "Male";
            list.Add(val);

            val = new TextValuePair<string, string>();
            val.Value = "F";
            val.Text = "Female";
            list.Add(val);
            return new SelectList(list, "Value", "Text");

        }
        public static SelectList GetAllStatuses(string defaultItem = "", bool includeDeleted = false)
        {
            using (AMSContext db = AMSContext.CreateNewContext())
            {
                var qry = StatusTable.GetAllStatuss(db, true);
                if (includeDeleted)
                    qry = StatusTable.GetAllStatusesIncludingDeleted(db);

                List<TextValuePair<string, int?>> list = new List<TextValuePair<string, int?>>();

                list = (from b in qry
                        orderby b.StatusID
                        select new TextValuePair<string, int?>
                        {
                            Text = b.Status,
                            Value = b.StatusID,
                        }).ToList();

                return new SelectList(list, "Value", "Text");
            }
        }
        public static SelectList GetAllApproveModule(AMSContext model, string defaultItem = "")
        {
            var items = ApproveModuleTable.GetAllItems(model);
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();

            foreach (var nm in items)
            {
                TextValuePair<string, int> val = new TextValuePair<string, int>();
                val.Text = nm.ModuleName;
                val.Value = nm.ApproveModuleID;
                list.Add(val);
            }

            if (!string.IsNullOrEmpty(defaultItem))
            {
                TextValuePair<string, int> val = new TextValuePair<string, int>();
                val.Value = 0;
                val.Text = defaultItem;
                list.Insert(0, val);
            }
            return new SelectList(list, "Value", "Text");
        }
        public static SelectList GetAllCategorytype(AMSContext model, string defaultItem = "",bool allowAllType=false)
        {
            IQueryable<CategoryTypeTable> items = CategoryTypeTable.GetAllItems(model);
            if(!allowAllType)
            {
                items= items.Where(a => a.IsAllCategoryType == false);
            }
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();

            foreach (var nm in items)
            {
                TextValuePair<string, int> val = new TextValuePair<string, int>();
                val.Text = nm.CategoryTypeName;
                val.Value = nm.CategoryTypeID;
                list.Add(val);
            }

            if (!string.IsNullOrEmpty(defaultItem))
            {
                TextValuePair<string, int> val = new TextValuePair<string, int>();
                val.Value = 0;
                val.Text = defaultItem;
                list.Insert(0, val);
            }
            return new SelectList(list, "Value", "Text");
        }
        public static SelectList GetAllLocationType(AMSContext model, string defaultItem = "",int? moduleID=null)
        {
            var items = LocationTypeTable.GetAllItems(model);
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();
            if(moduleID.HasValue)
            {
                items = items.Where(a => a.LocationTypeName == "Head Quarters");
            }
            foreach (var nm in items)
            {
                TextValuePair<string, int> val = new TextValuePair<string, int>();
                val.Text = nm.LocationTypeName;
                val.Value = nm.LocationTypeID;
                list.Add(val);
            }

            if (!string.IsNullOrEmpty(defaultItem))
            {
                TextValuePair<string, int> val = new TextValuePair<string, int>();
                val.Value = 0;
                val.Text = defaultItem;
                list.Insert(0, val);
            }
            return new SelectList(list, "Value", "Text");
        }

        public static SelectList GetAvailableApprovalRole(AMSContext model)
        {
            IQueryable<ApprovalRoleTable> qry = ApprovalRoleTable.GetAllItems(model);
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();

            list = (from b in qry
                    select new TextValuePair<string, int>
                    {
                        Text = b.ApprovalRoleName,
                        Value = b.ApprovalRoleID
                    }).ToList();

            return new SelectList(list, "Value", "Text");
        }

        public static SelectList GetSelectedApprovalRole(AMSContext model, int approveworkFlowID)
        {
            var selectedlist = (from b in model.ApproveWorkflowLineItemTable.Include("ApprovalRoleTable") where b.ApproveWorkFlowID == approveworkFlowID select b).OrderBy(a => a.OrderNo).ToList();

            IQueryable<ApprovalRoleTable> qry = ApprovalRoleTable.GetAllItems(model);
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();

            list = (from b in selectedlist
                        //where //selectedlist.Contains(b.ApprovalRoleID)
                    select new TextValuePair<string, int>
                    {
                        Text = b.ApprovalRole.ApprovalRoleName,
                        Value = b.ApprovalRole.ApprovalRoleID
                    }).ToList();

            return new SelectList(list, "Value", "Text");
        }
        public static SelectList GetAvailableApprovalRole(AMSContext model, int approveworkFlowID)
        {
            var selectedlist = (from b in model.ApproveWorkflowLineItemTable where b.ApproveWorkFlowID == approveworkFlowID && b.StatusID == (int)StatusValue.Active select b);// b.ApprovalRoleID).ToList();
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();
            IQueryable<ApprovalRoleTable> qry = ApprovalRoleTable.GetAllItems(model);
            var res = (from b in qry
                       join c in selectedlist on b.ApprovalRoleID equals c.ApprovalRoleID into gh
                       from subgroup in gh.DefaultIfEmpty()
                       where subgroup == null
                       select new { b.ApprovalRoleName, b.ApprovalRoleID });


            list = (from b in res
                   // where !selectedlist.Contains(b.ApprovalRoleID)
                    select new TextValuePair<string, int>
                    {
                        Text = b.ApprovalRoleName,
                        Value = b.ApprovalRoleID
                    }).ToList();

            return new SelectList(list, "Value", "Text");
           
        }

        public static SelectList GetAvailableLocation(AMSContext model, string page = null, string location = null, int? personID = null) //, string page, string loc = null, int? personID = null
        {   
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();
            var selectedlist = (from b in model.LocationForUserMappingView select b);
            if ((string.IsNullOrEmpty(location) && (page == "Edit" || page == "View")))
            {
                var result = (from b in model.UserLocationMappingTable where b.PersonID == (int)personID && b.StatusID == (int)StatusValue.Active select b);
                var res = (from b in selectedlist
                           join c in result on b.LocationID equals c.LocationID into gj
                           from subgroup in gj.DefaultIfEmpty()
                           where subgroup == null
                           select new
                           {
                               b.LocationID,
                               b.LocationCode,
                               b.SecondLevelLocationName,
                               
                           }); 
                list = (from b in res 
                        orderby b.SecondLevelLocationName
                        select new TextValuePair<string, int>
                        {
                            Text = string.Concat(b.LocationCode, '-', b.SecondLevelLocationName),
                            Value = b.LocationID
                        }).ToList();
              
                return new SelectList(list, "Value", "Text"); 

            }

            list = (from b in selectedlist 
                    orderby b.SecondLevelLocationName
                    select new TextValuePair<string, int>
                    {
                        Text = string.Concat(b.LocationCode,'-',b.SecondLevelLocationName),
                        Value = b.LocationID
                    }).ToList();
            //list = LocationDescriptionTable.GetItemsForListing(qry, MasterListingType.NameOnly);
            return new SelectList(list, "Value", "Text"); ;
        }

        public static SelectList GetAvailableCategory(AMSContext model, string page = null, string category = null, int? personID = null) //, string page, string loc = null, int? personID = null
        {
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();
            var selectedlist = (from b in model.SecondLevelCategoryTable  select b);
            if ((string.IsNullOrEmpty(category) && (page == "Edit" || page == "View")))
            {
               var result = (from b in model.UserCategoryMappingTable where b.PersonID == (int)personID && b.StatusID == (int)StatusValue.Active select b);// b.CategoryID).ToList();
                var res = (from b in selectedlist
                           join c in result on b.CategoryID equals c.CategoryID into gj
                           from subgroup in gj.DefaultIfEmpty()
                           where subgroup == null
                           select new { b.CategoryCode, b.CategoryID, b.SecondLevelCategoryName });

                //  selectedlist = selectedlist.Where(a => !result.Contains(a.CategoryID));
                list = (from b in res
                        orderby b.SecondLevelCategoryName
                        select new TextValuePair<string, int>
                        {
                            Text = string.Concat(b.CategoryCode, '-', b.SecondLevelCategoryName),//b.CategoryName,
                            Value = b.CategoryID
                        }).ToList();
                //list = LocationDescriptionTable.GetItemsForListing(qry, MasterListingType.NameOnly);
                return new SelectList(list, "Value", "Text"); ;
            }

            list = (from b in selectedlist
                    orderby b.SecondLevelCategoryName
                    select new TextValuePair<string, int>
                    {
                        Text = string.Concat(b.CategoryCode, '-', b.SecondLevelCategoryName),//b.CategoryName,
                        Value = b.CategoryID
                    }).ToList();
            //list = LocationDescriptionTable.GetItemsForListing(qry, MasterListingType.NameOnly);
            return new SelectList(list, "Value", "Text"); ;
        }

        public static SelectList GetSelectedCategory(AMSContext model, string page = null, string category = null, int? personID = null) //, string page, string loc = null, int? personID = null
        {
          
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();
            var selectedlist = (from b in model.SecondLevelCategoryTable select b);
            if ((string.IsNullOrEmpty(category) && (page == "Edit" || page == "View")))
            {
               var result = (from b in model.UserCategoryMappingTable where b.PersonID == (int)personID && b.StatusID == (int)StatusValue.Active select b); //select b.CategoryID).ToList();

                selectedlist = (from b in selectedlist join c in result on b.CategoryID equals c.CategoryID select b);
                //selectedlist = selectedlist.Where(a => result.Contains(a.CategoryID));

            }
            list = (from b in selectedlist
                    select new TextValuePair<string, int>
                    {
                        Text = string.Concat(b.CategoryCode, '-', b.SecondLevelCategoryName),
                        Value = b.CategoryID
                    }).ToList();
            return new SelectList(list, "Value", "Text"); 
        }
        
        public static SelectList GetAvailableDepartment(AMSContext model, string page = null, string depart = null, int? personID = null)
        {
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();
            var selectedlist = (from b in model.DepartmentTable where b.StatusID == (int)StatusValue.Active select b);
            if ((string.IsNullOrEmpty(depart) && (page == "Edit" || page == "View")))
            {
                var result = (from b in model.UserDepartmentMappingTable where b.PersonID == (int)personID && b.StatusID == (int)StatusValue.Active select b);// select b.DepartmentID).ToList();
                var res = (from b in selectedlist
                           join c in result on b.DepartmentID equals c.DepartmentID into kl
                           from subgroup in kl.DefaultIfEmpty()
                           where subgroup == null
                           select new { b.DepartmentName, b.DepartmentID });

                //  selectedlist = selectedlist.Where(a => !result.Contains(a.DepartmentID));
                list = (from b in res
                        orderby b.DepartmentName
                        select new TextValuePair<string, int>
                        {
                            Text = b.DepartmentName,
                            Value = b.DepartmentID
                        }).ToList();
                //list = LocationDescriptionTable.GetItemsForListing(qry, MasterListingType.NameOnly);
                return new SelectList(list, "Value", "Text");

            }

            list = (from b in selectedlist
                    orderby b.DepartmentName
                    select new TextValuePair<string, int>
                    {
                        Text = b.DepartmentName,
                        Value = b.DepartmentID
                    }).ToList();
            //list = LocationDescriptionTable.GetItemsForListing(qry, MasterListingType.NameOnly);
            return new SelectList(list, "Value", "Text"); 
        }
        public static SelectList GetSelectedDepartment(AMSContext model, string page = null, string depart = null, int? personID = null)
        {

            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();
            var selectedlist = (from b in model.DepartmentTable where b.StatusID == (int)StatusValue.Active select b);
            if ((string.IsNullOrEmpty(depart) && (page == "Edit" || page == "View")))
            {
                var result = (from b in model.UserDepartmentMappingTable where b.PersonID == (int)personID && b.StatusID == (int)StatusValue.Active select b);//select b.DepartmentID).ToList();
                                                                                                                                                          
                //  selectedlist = selectedlist.Where(a => result.Contains(a.DepartmentID));
                selectedlist = (from b in selectedlist join c in result on b.DepartmentID equals c.DepartmentID select b);
            }
            list = (from b in selectedlist
                    select new TextValuePair<string, int>
                    {
                        Text = b.DepartmentName,
                        Value = b.DepartmentID
                    }).ToList();
            //list = LocationDescriptionTable.GetItemsForListing(qry, MasterListingType.NameOnly);
            return new SelectList(list, "Value", "Text"); ;
        }

        public static SelectList GetSelectedLocation(AMSContext model, string page, string loc = null, int? personID = null)
        {
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();
            var selectedlist = (from b in model.LocationForUserMappingView select b);
            if ((string.IsNullOrEmpty(loc) && (page == "Edit" || page == "View")))
            {
                var result = (from b in model.UserLocationMappingTable where b.PersonID == (int)personID && b.StatusID == (int)StatusValue.Active select b);
                selectedlist = (from b in selectedlist join c in result on b.LocationID equals c.LocationID select b);  //selectedlist.Where(a => result.Contains(a.LocationID));

            }
            list = (from b in selectedlist
                    select new TextValuePair<string, int>
                    {
                        Text = string.Concat(b.LocationCode,'-',b.SecondLevelLocationName),
                        Value = b.LocationID
                    }).ToList();
            return new SelectList(list, "Value", "Text"); ;
        }

        public static SelectList GetFinalLevelDetils(AMSContext model,int userID, string defaultItem = "")
        {
     
            List<int> getUserApprovalList = PersonTable.GetUserRoleMappingDetails(model, userID).Select(a => (int)a.ApprovalRoleID).ToList();
            var items = FinalLevelRetirementView.GetAllItems(model).Where(a => getUserApprovalList.Contains(a.ApprovalRoleID)  && a.CreatedBy != userID);
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();

            foreach (var nm in items)
            {
                TextValuePair<string, int> val = new TextValuePair<string, int>();
                val.Text = nm.TransactionNo;
                val.Value = nm.TransactionID;
                list.Add(val);
            }

            if (!string.IsNullOrEmpty(defaultItem))
            {
                TextValuePair<string, int> val = new TextValuePair<string, int>();
                val.Value = 0;
                val.Text = defaultItem;
                list.Insert(0, val);
            }
            return new SelectList(list, "Value", "Text");
        }


        public static SelectList GetFinalTransferLevelDetils(AMSContext model, int userID, string defaultItem = "")
        {

            List<int> getUserApprovalList = PersonTable.GetUserRoleMappingDetails(model, userID).Select(a => (int)a.ApprovalRoleID).ToList();
            var items = FinalLevelTransferView.GetAllItems(model).Where(a => getUserApprovalList.Contains(a.ApprovalRoleID) && a.CreatedBy != userID);
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();

            foreach (var nm in items)
            {
                TextValuePair<string, int> val = new TextValuePair<string, int>();
                val.Text = nm.TransactionNo;
                val.Value = nm.TransactionID;
                list.Add(val);
            }

            if (!string.IsNullOrEmpty(defaultItem))
            {
                TextValuePair<string, int> val = new TextValuePair<string, int>();
                val.Value = 0;
                val.Text = defaultItem;
                list.Insert(0, val);
            }
            return new SelectList(list, "Value", "Text");
        }

        public static SelectList GetImportTemplateType(AMSContext model, string defaultItem = "")
        {
            var items = ImportTemplateTypeTable.GetAllItems(model);
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();

            foreach (var nm in items)
            {
                TextValuePair<string, int> val = new TextValuePair<string, int>();
                val.Text = nm.ImportTemplateType;
                val.Value = nm.ImportTemplateTypeID;
                list.Add(val);
            }

            if (!string.IsNullOrEmpty(defaultItem))
            {
                TextValuePair<string, int> val = new TextValuePair<string, int>();
                val.Value = 0;
                val.Text = defaultItem;
                list.Insert(0, val);
            }
            return new SelectList(list, "Value", "Text");
        }

        public static SelectList GetImportMaster(AMSContext model, string defaultItem = "")
        {
            var items = EntityTable.GetAllItems(model, true).Distinct().OrderBy(b => b.EntityName);
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();

            foreach (var nm in items)
            {
                TextValuePair<string, int> val = new TextValuePair<string, int>();
                val.Text = nm.EntityName;
                val.Value = nm.EntityID;
                list.Add(val);
            }

            if (!string.IsNullOrEmpty(defaultItem))
            {
                TextValuePair<string, int> val = new TextValuePair<string, int>();
                val.Value = 0;
                val.Text = defaultItem;
                list.Insert(0, val);
            }
            return new SelectList(list, "Value", "Text");
        }

        public static SelectList GetAllNotificationModules()
        {
            using (AMSContext db = AMSContext.CreateNewContext())
            {
                var qry = NotificationModuleTable.GetAllItems(db, false);
               // var res = NotificationTemplateTable.GetAllItems(db, false).Select(a => a.NotificationModuleID).ToList().Distinct();
                List<TextValuePair<string, int?>> list = new List<TextValuePair<string, int?>>();

                list = (from b in qry
                       // where !res.Contains(b.NotificationModuleID)
                        orderby b.NotificationModuleID
                        select new TextValuePair<string, int?>
                        {
                            Text = b.NotificationModule,
                            Value = b.NotificationModuleID,
                        }).ToList();

                return new SelectList(list, "Value", "Text");
            }
        }
        public static SelectList GetAllNotificationTypes()
        {
            using (AMSContext db = AMSContext.CreateNewContext())
            {
                var qry = NotificationTypeTable.GetAllItems(db, false);

                List<TextValuePair<string, int?>> list = new List<TextValuePair<string, int?>>();

                list = (from b in qry
                        orderby b.NotificationTypeID
                        select new TextValuePair<string, int?>
                        {
                            Text = b.NotificationType,
                            Value = b.NotificationTypeID,
                        }).ToList();

                return new SelectList(list, "Value", "Text");
            }
        }
        public static SelectList GetAllEmailSignature()
        {
            using (AMSContext db = AMSContext.CreateNewContext())
            {
                var qry = EmailSignatureTable.GetAllItems(db, false);

                List<TextValuePair<string, int?>> list = new List<TextValuePair<string, int?>>();

                list = (from b in qry
                        orderby b.EmailSignatureID
                        select new TextValuePair<string, int?>
                        {
                            Text = b.EmailSignatureContent,
                            Value = b.EmailSignatureID,
                        }).ToList();

                return new SelectList(list, "Value", "Text");
            }
        }
        public static SelectList GetAllAttachmentFormat()
        {
            using (AMSContext db = AMSContext.CreateNewContext())
            {
                var qry = AttachmentFormatTable.GetAllItems(db, false);

                List<TextValuePair<string, int?>> list = new List<TextValuePair<string, int?>>();

                list = (from b in qry
                        orderby b.AttachmentFormatID
                        select new TextValuePair<string, int?>
                        {
                            Text = b.AttachmentFormat,
                            Value = b.AttachmentFormatID,
                        }).ToList();

                return new SelectList(list, "Value", "Text");
            }
        }

        public static SelectList GetAllFieldTypes(string selectedValue = "")
        {
            AMSContext db = AMSContext.CreateNewContext();
            var qry = AFieldTypeTable.GetAllFieldTypes(db);

            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();

            list = (from b in qry.ToList()
                    orderby b.FieldTypeDesc
                    select new TextValuePair<string, int>
                    {
                        Text = Language.GetString(b.FieldTypeDesc),
                        Value = b.FieldTypeID,

                    }).ToList();

            return new SelectList(list, "Value", "Text", selectedValue);
        }
        public static SelectList GetAllReportTemplateCategories(string selectedValue = "")
        {
            AMSContext db = AMSContext.CreateNewContext();
            var qry = ReportTemplateCategoryTable.GetAllItems(db);

            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();

            list = (from b in qry.ToList()
                    orderby b.ReportTemplateCategoryName
                    select new TextValuePair<string, int>
                    {
                        Text = Language.GetString(b.ReportTemplateCategoryName),
                        Value = b.ReportTemplateCategoryID,

                    }).ToList();

            return new SelectList(list, "Value", "Text", selectedValue);
        }
        public static SelectList GetAllFilterFieldTypes(string selectedValue = "")
        {
            AMSContext db = AMSContext.CreateNewContext();
            var qry = ScreenFilterTypeTable.GetAllItems(db);

            List<TextValuePair<string, byte>> list = new List<TextValuePair<string, byte>>();

            list = (from b in qry.ToList()
                    orderby b.ScreenFilterTypeCode
                    select new TextValuePair<string, byte>
                    {
                        Text = Language.GetString(b.ScreenFilterTypeName),
                        Value = b.ScreenFilterTypeID,
                    }).ToList();

            return new SelectList(list, "Value", "Text", selectedValue);
        }

        public static SelectList GetAllReportPage(string defaultItem = "")
        {
            AMSContext db = AMSContext.CreateNewContext();
            var qry = ReportPaperSizeTable.GetAllItems(db);

            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();

            list = (from b in qry
                    select new TextValuePair<string, int>
                    {
                        Text = b.PaperSizeOrientation,
                        Value = b.ReportPaperSizeID,

                    }).ToList();

            if (!string.IsNullOrEmpty(defaultItem))
            {
                TextValuePair<string, int> val = new TextValuePair<string, int>();
                val.Value = 0;
                val.Text = defaultItem;
                list.Insert(0, val);
            }

            return new SelectList(list, "Value", "Text");
        }
        public static SelectList GetAllOperators(int fieldTypeID = 1, string selectedValue = "")
        {
            List<TextValuePair<string, string>> list = new List<TextValuePair<string, string>>();

            FieldTypeValue reqVal = (FieldTypeValue)fieldTypeID;
            switch (reqVal)
            {
                case FieldTypeValue.String:
                    list.Add(new TextValuePair<string, string>() { Value = "EqualTo", Text = "Is equal to" });
                    list.Add(new TextValuePair<string, string>() { Value = "NotEqualTo", Text = "Is not equal to" });
                    list.Add(new TextValuePair<string, string>() { Value = "StartsWith", Text = "Starts with" });
                    list.Add(new TextValuePair<string, string>() { Value = "Contains", Text = "Contains" });
                    list.Add(new TextValuePair<string, string>() { Value = "DoesNotContains", Text = "Does not contains" });
                    list.Add(new TextValuePair<string, string>() { Value = "EndsWith", Text = "Ends with" });
                    list.Add(new TextValuePair<string, string>() { Value = "In", Text = "In" });
                    list.Add(new TextValuePair<string, string>() { Value = "Isempty", Text = "Is empty" });
                    break;

                case FieldTypeValue.BigString:
                    list.Add(new TextValuePair<string, string>() { Value = "EqualTo", Text = "Is equal to" });
                    list.Add(new TextValuePair<string, string>() { Value = "NotEqualTo", Text = "Is not equal to" });
                    list.Add(new TextValuePair<string, string>() { Value = "StartsWith", Text = "Starts with" });
                    list.Add(new TextValuePair<string, string>() { Value = "Contains", Text = "Contains" });
                    list.Add(new TextValuePair<string, string>() { Value = "DoesNotContains", Text = "Does not contains" });
                    list.Add(new TextValuePair<string, string>() { Value = "EndsWith", Text = "Ends with" });
                    list.Add(new TextValuePair<string, string>() { Value = "Isempty", Text = "Is empty" });
                    break;

                case FieldTypeValue.Float:
                case FieldTypeValue.Integer:
                case FieldTypeValue.Currency:
                    list.Add(new TextValuePair<string, string>() { Value = "EqualTo", Text = "Is equal to" });
                    list.Add(new TextValuePair<string, string>() { Value = "NotEqualTo", Text = "Is not equal to" });
                    list.Add(new TextValuePair<string, string>() { Value = "GreaterThanOrEqualTo", Text = "Is greater than or equal to" });
                    list.Add(new TextValuePair<string, string>() { Value = "GreaterThan", Text = "Is greater than" });
                    list.Add(new TextValuePair<string, string>() { Value = "LessThanOrEqualTo", Text = "Is less than or equal to" });
                    list.Add(new TextValuePair<string, string>() { Value = "LessThan", Text = "Is less than" });
                    list.Add(new TextValuePair<string, string>() { Value = "Isempty", Text = "Is empty" });
                    break;

                case FieldTypeValue.Float_Range:
                case FieldTypeValue.Integer_Rage:
                    list.Add(new TextValuePair<string, string>() { Value = "Having", Text = "Having" });
                    list.Add(new TextValuePair<string, string>() { Value = "NotHaving", Text = "Not having" });
                    list.Add(new TextValuePair<string, string>() { Value = "GreaterThanOrEqualTo", Text = "Is greater than or equal to" });
                    list.Add(new TextValuePair<string, string>() { Value = "GreaterThan", Text = "Is greater than" });
                    list.Add(new TextValuePair<string, string>() { Value = "LessThanOrEqualTo", Text = "Is less than or equal to" });
                    list.Add(new TextValuePair<string, string>() { Value = "LessThan", Text = "Is less than" });
                    list.Add(new TextValuePair<string, string>() { Value = "Isempty", Text = "Is empty" });
                    break;

                case FieldTypeValue.Date:
                case FieldTypeValue.DateTime:
                case FieldTypeValue.Time:
                    list.Add(new TextValuePair<string, string>() { Value = "EqualTo", Text = "Is equal to" });
                    list.Add(new TextValuePair<string, string>() { Value = "NotEqualTo", Text = "Is not equal to" });
                    list.Add(new TextValuePair<string, string>() { Value = "AfterOrEqualTo", Text = "Is after or equal to" });
                    list.Add(new TextValuePair<string, string>() { Value = "After", Text = "Is after" });
                    list.Add(new TextValuePair<string, string>() { Value = "BeforeOrEqualTo", Text = "Is before or equal to" });
                    list.Add(new TextValuePair<string, string>() { Value = "Before", Text = "Is before" });
                    list.Add(new TextValuePair<string, string>() { Value = "Isempty", Text = "Is empty" });
                    break;

                case FieldTypeValue.Date_Range:
                    list.Add(new TextValuePair<string, string>() { Value = "Having", Text = "Having" });
                    list.Add(new TextValuePair<string, string>() { Value = "NotHaving", Text = "Not having" });
                    list.Add(new TextValuePair<string, string>() { Value = "AfterOrEqualTo", Text = "Is after or equal to" });
                    list.Add(new TextValuePair<string, string>() { Value = "After", Text = "Is after" });
                    list.Add(new TextValuePair<string, string>() { Value = "BeforeOrEqualTo", Text = "Is before or equal to" });
                    list.Add(new TextValuePair<string, string>() { Value = "Before", Text = "Is before" });
                    list.Add(new TextValuePair<string, string>() { Value = "Isempty", Text = "Is empty" });
                    break;
            }

            return new SelectList(list, "Value", "Text", selectedValue);
        }
        public static SelectList GetAllConditions(string defaultItem = "", string selectedValue = "")
        {
            List<TextValuePair<string, string>> list = new List<TextValuePair<string, string>>();

            TextValuePair<string, string> val = new TextValuePair<string, string>();
            val.Value = "AND";
            val.Text = "AND";
            list.Add(val);

            val = new TextValuePair<string, string>();
            val.Value = "OR";
            val.Text = "OR"; list.Add(val);

            return new SelectList(list, "Value", "Text", selectedValue);
        }
        public static SelectList GetDynamciReportTemplate(int reportTemplateID, string defaultItem = "")
        {
            AMSContext db = AMSContext.CreateNewContext();
            var qry = ReportTable.GetAllReports(db, reportTemplateID);

            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();

            list = (from b in qry
                    select new TextValuePair<string, int>
                    {
                        Text = b.ReportName,
                        Value = b.ReportID,

                    }).ToList();

            if (!string.IsNullOrEmpty(defaultItem))
            {
                TextValuePair<string, int> val = new TextValuePair<string, int>();
                val.Value = 0;
                val.Text = defaultItem;
                list.Insert(0, val);
            }

            return new SelectList(list, "Value", "Text");
        }

        public static SelectList GetReportFormat()
        {
            List<TextValuePair<string, string>> list = new List<TextValuePair<string, string>>();

            TextValuePair<string, string> val = new TextValuePair<string, string>();
            val.Value = "";
            val.Text = "Default";
            list.Add(val);

            val = new TextValuePair<string, string>();
            val.Value = "PDF";
            val.Text = "PDF";
            list.Add(val);

            val = new TextValuePair<string, string>();
            val.Value = "XLSX";
            val.Text = "Excel";
            list.Add(val);

            //val = new TextValuePair<string, string>();
            //val.Value = "ExcelWithoutPageHeader";
            //val.Text = "Excel Without Page Header/Footer";
            //list.Add(val);

            return new SelectList(list, "Value", "Text");
        }

        public static SelectList GetTransferType(string defaultItem = "")
        {
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();
            var lists = TransferTypeTable.GetAllItems(AMSContext.CreateNewContext(), false);

            list = (from b in lists
                    select new TextValuePair<string, int>
                    {
                        Text = b.TransferTypeName,
                        Value = b.TransferTypeID,

                    }).ToList();

            if (!string.IsNullOrEmpty(defaultItem))
            {
                TextValuePair<string, int> val = new TextValuePair<string, int>();
                val.Value = 0;
                val.Text = defaultItem;
                list.Insert(0, val);
            }
            return new SelectList(list, "Value", "Text");
        }
        public static SelectList GetAllAssetCondition(string defaultItem = "")
        {
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();
            var lists = AssetConditionTable.GetAllItems(AMSContext.CreateNewContext(), false);

            list = (from b in lists
                    select new TextValuePair<string, int>
                    {
                        Text = b.AssetConditionName,
                        Value = b.AssetConditionID,

                    }).ToList();

            if (!string.IsNullOrEmpty(defaultItem))
            {
                TextValuePair<string, int> val = new TextValuePair<string, int>();
                val.Value = 0;
                val.Text = defaultItem;
                list.Insert(0, val);
            }
            return new SelectList(list, "Value", "Text");
        }

        public static SelectList GetAvailableTemplateTableFields(AMSContext model, int moduleID, int? templateID = null)
        {
            IQueryable<NotificationModuleFieldTable> qry = NotificationModuleFieldTable.GetAllItems(model).Where(c => c.NotificationModuleID == moduleID);
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();
            if (templateID.HasValue)
            {
                var oldList = (from b in model.NotificationTemplateFieldTable where b.NotificationTemplateID == templateID select b);// select b.NotificationFieldID).ToList();
                var res = (from b in qry
                           join c in oldList on b.NotificationFieldID equals c.NotificationFieldID into gj
                           from subgroup in gj.DefaultIfEmpty()
                           where subgroup == null
                           select new { b.FieldName, b.NotificationFieldID });

                list = (from b in res
                        select new TextValuePair<string, int>
                        {
                            Text = b.FieldName,
                            Value = b.NotificationFieldID
                        }).ToList();

                return new SelectList(list, "Value", "Text");

            }
            list = (from b in qry
                    select new TextValuePair<string, int>
                    {
                        Text = b.FieldName,
                        Value = b.NotificationFieldID
                    }).ToList();

            return new SelectList(list, "Value", "Text");
        }
        public static SelectList GetSelectedTemplateTableFields(AMSContext model, int moduleID, int? templateID = null)
        {
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();
            //IQueryable<NotificationModuleFieldTable> qry = NotificationModuleFieldTable.GetAllItems(model).Where(c => c.NotificationModuleID == moduleID);
            if (templateID.HasValue)
            {
                var oldList = model.NotificationTemplateFieldTable.Include("NotificationField").Where(b => b.NotificationTemplateID == templateID)
                    .OrderBy(c => c.NotificationTemplateFieldID).ToList();
                //qry = qry.Where(a => oldList.Contains(a.NotificationFieldID));
                list = (from b in oldList
                        select new TextValuePair<string, int>
                        {
                            Text = b.NotificationField.FieldName,
                            Value = b.NotificationFieldID
                        }).ToList();
            }


            return new SelectList(list, "Value", "Text");

        }
        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        public static int ValidateApprovalRole(string[] roleIDs, int moduleID, AMSContext model)
        {
            string[] arryAssetID = roleIDs;
            int[] intIDS = Array.ConvertAll(arryAssetID, s => int.Parse(s));
            IQueryable<ApprovalRoleTable> Role = ApprovalRoleTable.GetAllItems(model);
            if(moduleID==(int)ApproveModuleValue.AssetTransfer)
            {
                Role = (from b in Role join c in intIDS on b.ApprovalRoleID equals c where b.UpdateDestinationLocationsForTransfer == true select b);//Role.Where(a => a.UpdateDestinationLocationsForTransfer == true && intIDS.Contains(a.ApprovalRoleID));
                return Role.Count();
            }
            else if(moduleID==(int)ApproveModuleValue.AssetRetirement)
            {
                Role = (from b in Role join c in intIDS on b.ApprovalRoleID equals c where b.UpdateRetirementDetailsForEachAssets == true select b);//Role.Where(a => a.UpdateRetirementDetailsForEachAssets == true && intIDS.Contains(a.ApprovalRoleID));
                return Role.Count();
            }
            return 0;
        }
        public static SelectList GetAvailableCompanies(AMSContext model, string page, string depart = null, int? personID = null)
        {
          
            var qry = CompanyTable.GetAllItems(model);
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();
            if (!string.IsNullOrEmpty(depart) && page == "Create")
            {
                List<int> ids = PersonTable.GetMappingID(depart);
                qry = qry.Where(a => !ids.Contains(a.CompanyID));
            }
            if ((string.IsNullOrEmpty(depart) && (page == "Edit" || page == "View")))
            {
                var result = UserCompanyMappingTable.GetAllItems(model).Where(x => x.UserID == personID);//.Select(b => b.CompanyID).ToList();
                var res=(from b in qry join c in result on b.CompanyID equals c.CompanyID into gj
                         from subgroup in gj.DefaultIfEmpty()
                         where subgroup == null
                         select new
                         {
                             b.CompanyID,
                             b.CompanyName
                         });

                list = (from b in res
                        orderby b.CompanyName
                        select new TextValuePair<string, int>
                        {
                            Text = b.CompanyName,
                            Value = b.CompanyID
                        }).ToList();

                return new SelectList(list, "Value", "Text");
            }
            if ((!string.IsNullOrEmpty(depart) && (page == "Edit")))
            {
                List<int> ids = PersonTable.GetMappingID(depart);
                qry = qry.Where(a => !ids.Contains(a.CompanyID));
            }

           
            list = (from b in qry
                    orderby b.CompanyName
                    select new TextValuePair<string, int>
                    {
                        Text = b.CompanyName,
                        Value = b.CompanyID
                    }).ToList();

            // list = CompanyDescriptionTable.GetItemsForListing(qry, MasterListingType.NameOnly);
            return new SelectList(list, "Value", "Text");
        }

        public static SelectList GetSelectedcompanies(AMSContext model, string page, string depart = null, int? personID = null)
        {
           
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();
            var qry = CompanyTable.GetAllItems(model);
            if (!string.IsNullOrEmpty(depart) && page == "Create")
            {
                List<int> ids = PersonTable.GetMappingID(depart);
                qry = qry.Where(a => ids.Contains(a.CompanyID));
            }
            if ((string.IsNullOrEmpty(depart) && (page == "Edit" || page == "View")))
            {
               var result = UserCompanyMappingTable.GetAllItems(model).Where(x => x.UserID == personID);//.Select(b => b.CompanyID).ToList();
                qry = (from b in qry join c in result on b.CompanyID equals c.CompanyID select b); //qry.Where(a => result.Contains(a.CompanyID));
            }
            if (!string.IsNullOrEmpty(depart) && page == "Edit")
            {
                List<int> ids = PersonTable.GetMappingID(depart);
                qry = qry.Where(a => ids.Contains(a.CompanyID));
            }
            list = (from b in qry
                    orderby b.CompanyName
                    select new TextValuePair<string, int>
                    {
                        Text = b.CompanyName,
                        Value = b.CompanyID
                    }).ToList();
            return new SelectList(list, "Value", "Text"); ;
        }
        public static SelectList GetLabelFormat(AMSContext context)
        {
            var qry = BarcodeFormatsTable.GetAllItems(context);
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();

            foreach (var nm in qry)
            {
                TextValuePair<string, int> val = new TextValuePair<string, int>();
                val.Text = nm.FormatName;
                val.Value = nm.FormatID;
                list.Add(val);
            }

            return new SelectList(list, "Value", "Text");
        }
        public static SelectList GetAllDashboardType(string defaultItem = "")
        {
            AMSContext db = AMSContext.CreateNewContext();
            var qry = DashboardTypeTable.GetAllItems(db);

            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();

            list = (from b in qry
                    select new TextValuePair<string, int>
                    {
                        Text = b.DashboardType,
                        Value = b.DashboardTypeID,

                    }).ToList();

            if (!string.IsNullOrEmpty(defaultItem))
            {
                TextValuePair<string, int> val = new TextValuePair<string, int>();
                val.Value = 0;
                val.Text = defaultItem;
                list.Insert(0, val);
            }

            return new SelectList(list, "Value", "Text");
        }
        public static SelectList GetAllDashboardFieldMapping(string defaultItem = "",int? dashboardTemplateID=null)
        {
            AMSContext db = AMSContext.CreateNewContext();
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();

            if (dashboardTemplateID.HasValue)
            {
                var qry = DashboardTemplateTable.GetAllDashboardTemplateFields(db, (int)dashboardTemplateID);
                list = (from b in qry
                        select new TextValuePair<string, int>
                        {
                            Text = b.FieldName,
                            Value = b.DashboardTemplateFieldID,

                        }).ToList();

                if (!string.IsNullOrEmpty(defaultItem))
                {
                    TextValuePair<string, int> val = new TextValuePair<string, int>();
                    val.Value = 0;
                    val.Text = defaultItem;
                    list.Insert(0, val);
                }
            }

            return new SelectList(list, "Value", "Text");
        }

        public static SelectList GetAllPostingStatus(string defaultItem = "", string selectedValue = "")
        {
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();

            TextValuePair<string, int> val = new TextValuePair<string, int>();
            val.Value = (int)PostingStatusValue.WorkInProgress;
            val.Text = "WorkInProgress";
            list.Add(val);

            TextValuePair<string, int> val1 = new TextValuePair<string, int>();
            val1.Value = (int)PostingStatusValue.CompletedByEndUser;
            val1.Text = "CompletedByEndUser";
            list.Add(val1);

            return new SelectList(list, "Value", "Text", selectedValue);
        }
        public static SelectList GetAllConfigurationValue(int appPages, string defaultItem = "")
        {
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();
            AMSContext _db = AMSContext.CreateNewContext();
            if ((int)EntityValues.LanguageTable == appPages)
            {
                var qry = LanguageTable.GetAllItems(_db,false);

                list = (from b in qry
                        select new TextValuePair<string, int>
                        {
                            Text = b.LanguageName,
                            Value = b.LanguageID,

                        }).ToList();

                if (!string.IsNullOrEmpty(defaultItem))
                {
                    TextValuePair<string, int> val = new TextValuePair<string, int>();
                    val.Value = 0;
                    val.Text = defaultItem;
                    list.Insert(0, val);
                }
            }
            else if ((int)EntityValues.PersonTable == appPages)
            {
                var qry = PersonTable.GetAllItems(_db, false).OrderBy(a => a.PersonFirstName);

                list = (from b in qry
                        select new TextValuePair<string, int>
                        {
                            Text = b.PersonFirstName + " " + b.PersonLastName,
                            Value = b.PersonID,

                        }).ToList();

                if (!string.IsNullOrEmpty(defaultItem))
                {
                    TextValuePair<string, int> val = new TextValuePair<string, int>();
                    val.Value = 0;
                    val.Text = defaultItem;
                    list.Insert(0, val);
                }

            }
            else if ((int)EntityValues.CategoryTable == appPages)
            {
                var qry = CategoryTable.GetAllItems(_db, false).Where(a=>a.ParentCategoryID==null);
                list = (from b in qry
                        select new TextValuePair<string, int>
                        {
                            Text = b.CategoryName,
                            Value = b.CategoryID,

                        }).ToList();
                if (!string.IsNullOrEmpty(defaultItem))
                {
                    TextValuePair<string, int> val = new TextValuePair<string, int>();
                    val.Value = 0;
                    val.Text = defaultItem;
                    list.Insert(0, val);
                }
            }
            else if((int)EntityValues.ManufacturerTable == appPages)
            {
                var qry = ManufacturerTable.GetAllItems(_db, false);
                list = (from b in qry
                        select new TextValuePair<string, int>
                        {
                            Text = b.ManufacturerCode + " " + b.ManufacturerName,
                            Value = b.ManufacturerID,

                        }).ToList();
                if (!string.IsNullOrEmpty(defaultItem))
                {
                    TextValuePair<string, int> val = new TextValuePair<string, int>();
                    val.Value = 0;
                    val.Text = defaultItem;
                    list.Insert(0, val);
                }
            }
            else
            {
                var qry = PersonTable.GetAllItems(_db, false).Where(p => p.EMailID != null).OrderBy(a => a.PersonFirstName);

                list = (from b in qry
                        select new TextValuePair<string, int>
                        {
                            Text = b.PersonFirstName + " " + b.PersonLastName,
                            Value = b.PersonID,

                        }).ToList();

                if (!string.IsNullOrEmpty(defaultItem))
                {
                    TextValuePair<string, int> val = new TextValuePair<string, int>();
                    val.Value = 0;
                    val.Text = defaultItem;
                    list.Insert(0, val);
                }
            }
            return new SelectList(list, "Value", "Text");
        }
        public static SelectList GetDynamciReportEntity(string defaultItem = "")
        {
            AMSContext _db = AMSContext.CreateNewContext();
            var qry = EntityActionTable.GetAllItems(_db).OrderBy(a => a.ActionName);

            List<TextValuePair<string, string>> list = new List<TextValuePair<string, string>>();

            list = (from b in qry.ToList()
                    select new TextValuePair<string, string>
                    {
                        Text = Language.GetString(b.ActionName),
                        Value = b.ActionName,

                    }).ToList();

            if (!string.IsNullOrEmpty(defaultItem))
            {
                TextValuePair<string, string> val = new TextValuePair<string, string>();
                val.Value = "0";
                val.Text = defaultItem;
                list.Insert(0, val);
            }

            return new SelectList(list, "Value", "Text");
        }
        public static string ValidateUnique(DataTable table)
        {
            string dupValue = string.Empty;

                var duplicateValues = (from row in table.AsEnumerable()
                                       let ID = row.Field<string>("Barcode")
                                       group row by new { ID } into grp
                                       where grp.Count() > 1
                                       select new
                                       {
                                           DupID = grp.Key.ID,
                                           count = grp.Count(),
                                           field = "Barcode"
                                       }).ToList();

                foreach (var dup in duplicateValues)
                {
                    dupValue += string.IsNullOrEmpty(dupValue) ? dup.DupID : (dupValue.LastIndexOf("\n") == dupValue.Length - 1) ? Environment.NewLine + dup.DupID : "," + dup.DupID;
                }

                if (duplicateValues.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dupValue))
                    {
                        dupValue += " In " + duplicateValues.FirstOrDefault().field + " are duplicated" + Environment.NewLine;
                    }
                }
            
            return dupValue;
        }

        public static SelectList GetTransactionAssignLevels(AMSContext model, int approvalHistoryID, string defaultItem = "")
        {
            var approvalHistory = ApprovalHistoryTable.GetItem(model, approvalHistoryID);
            var data = (from b in model.ApprovalHistoryTable where b.TransactionID == approvalHistory.TransactionID && b.ApproveModuleID == approvalHistory.ApproveModuleID && b.OrderNo < approvalHistory.OrderNo select b);

         
            List<TextValuePair<string, int>> list = new List<TextValuePair<string, int>>();

            foreach (var nm in data.ToList())
            {
                var RoleName = ApprovalRoleTable.GetItem(model, nm.ApprovalRoleID);
                TextValuePair<string, int> val = new TextValuePair<string, int>();
                val.Text = RoleName.ApprovalRoleName;
                val.Value = nm.OrderNo;
                list.Add(val);
            }

            if (!string.IsNullOrEmpty(defaultItem))
            {
                TextValuePair<string, int> val = new TextValuePair<string, int>();
                val.Value = 0;
                val.Text = defaultItem;
                list.Insert(0, val);
            }
            return new SelectList(list, "Value", "Text");
        }
    }
}
