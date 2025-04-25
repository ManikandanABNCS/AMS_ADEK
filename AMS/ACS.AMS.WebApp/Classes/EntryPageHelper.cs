using ACS.AMS.DAL;
using ACS.AMS.DAL.DBContext;
using ACS.AMS.DAL.DBModel;
using ACS.AMS.WebApp.Models;
using ACS.WebAppPageGenerator.Models.SystemModels;
using DocumentFormat.OpenXml.Office2010.Word;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection;
//using Telerik.SvgIcons;

namespace ACS.AMS.WebApp
{
	public class EntryPageHelper
    {
        public EntryPageHelper(BasePageModel model)
        {
            Model = model;
            GridName = "DetailsGrid";
        }

        public BasePageModel Model { get; set; }

        public string GridName { get; set; }

        public IUrlHelper Url { get; set; }

        private string PageName { get => Model.PageName; }

        private string PrimaryKeyFieldName { get => Model.EntityInstance.GetPrimaryKeyFieldName(); }

        public bool AddControlsInSingleColumnForLessItems { get; set; } = true;

        public static List<string> commonFields = new List<string>()
        {
            "CreatedBy",
            "CreatedDateTime",
            "LastModifyBy",
            "LastModifiedBy",
            "LastModifiedDate",
            "LastModifiedDateTime",
            "CurrentPageID",
            "StatusID"
        };
        public static List<string> AuditLogFields = new List<string>()
        {
            "CreatedBy",
            "CreatedDateTime",
            "LastModifyBy",
            "LastModifiedBy",
            "LastModifiedDate",
            "LastModifiedDateTime",
        };
        public static List<string> noNeedToDisplayFields = new List<string>()
        {
            "CurrencyStartDate",
            "CurrencyEndDate",
            "TransactionSubType",
            "CreatedFrom",
             "SourceDocumentNo"
        };

        public static List<string> noNeedToDisplayFields1 = new List<string>()
        {
            "TransactionSubTypeID",
            "SourceTransactionID",
             "TransactionTypeID",
            "CreatedFrom",
            "SourceDocumentNo",
            "TransactionValue",
            "PostingStatusID",
            "VerifiedBy",
            "VerifiedDateTime",
                "PostedBy",
            "PostedDateTime",
            "TransactionStartDate",
                 "TransactionEndDate"
        };

        public static List<string> noNeedToDisplayFieldsAssetMaintenanceclosure = new List<string>()
        {
            "TransactionSubTypeID",
            "SourceTransactionID",
             "TransactionTypeID",
            "CreatedFrom",
            "SourceDocumentNo",
            "TransactionValue",
            "VerifiedBy",
            "VerifiedDateTime",
             "PostedBy",
            "PostedDateTime",
            "TransactionStartDate",
                 "TransactionEndDate"
        };

        public static List<string> ProductFields = new List<string>()
        {
            "IsInventoryItem",
            "IsSerialNumberRequired",
            "UOMID",
            "ReorderLevelQuantity",
            "Price"
        };

        public static List<string> LocationFields = new List<string>()
        {
            "LocationID",
            "ParentLocationID",
            "LocationCode",
            "LocationName",
            "StampPath",
            "LocationTypeID",
        };

        public static List<string> CategoryFields = new List<string>()
        {
            "CategoryID",
            "ParentCategoryID",
            "CategoryCode",
            "CategoryName",
            "CategoryTypeID"
        };

        public static List<string> TreeViewReferenceID = new List<string>()
        {
            "CategoryID",
            "ParentLocationID",
            "ParentCategoryID",
            "CategoryID",
        };

        public static List<string> AssetFields = new List<string>()
        {
            "CreateFromHHT",
            "ProjectID",
            "WIPAssetID",
            "MappedAssetID",
            "MailAlert",
            "IsTransfered",
            "TransferTypeID",
            "AssetApproval",
            "InsertedToOracle",
            "DistributionID",
            "ClassificationID",
            "CompanyID",
            "SyncDateTime",
            "QFAssetCode",
            "DOFPO_LINE_NUM",
            "DOFMASS_ADDITION_ID",
            "ERPUpdateType",
            "DOFPARENT_MASS_ADDITION_ID",
            "DOFFIXED_ASSETS_UNITS",
            "zDOF_Asset_Updated",
            "DOF_MASS_SPLIT_EXECUTED",
            "AllowTransfer",
            "ConsultantID",
            "LastScannedLocationID",
            "IsScanned",
            "NetbookValue",
            "zDOFAssetNumber",
            "zDOF_UpdateErrorOccurred",
            "DisposalReferenceNo",
            "DisposedDateTime",
            "DisposedRemarks",
            "DisposalValue",
            "DisposalTypeID",
            "CurrentCost",
            "ProceedofSales",
            "SoldTo",
            "RetirementTypeID",
            "CostOfRemoval"
        };

        public string FirstFieldName { get; private set; }
        private void AddMultiLanguageTextBox(IHtmlHelper<BasePageModel> htmlHelper, RazorPage page, bool readOnlyPage, Type entityInstanceType,
                bool allowMultipleCols, string propertyValue)
        {
            var languageEntityInstanceType = EntityHelper.GetLanguageTable(entityInstanceType);
            string languagePropertyName = languageEntityInstanceType.Name;
            languagePropertyName = languagePropertyName.Substring(0, languagePropertyName.Length - 5);

            if (!allowMultipleCols)
            {
                page.WriteLiteral($"<div class=\"row\">");
            }

            page.WriteLiteral($"<div class=\"col-xl-4\">");


            page.WriteLiteral($"<label for=\"fullname\">");

            page.Write(HtmlHelperLabelExtensions.Label(htmlHelper, Language.GetFieldName(languagePropertyName)));
            page.WriteLiteral($"&nbsp;&nbsp;<span style=\"color: red\">*<span>");

            page.WriteLiteral($"</label>");

            if (readOnlyPage)
            {
                var dHtmlAttr = new { Class = "k-textbox", disabled = "disabled", style = "width: 100%;" };
                page.Write(htmlHelper.TextBox("dValue", propertyValue + "", dHtmlAttr));
            }
            else
            {
                var htmlAttr = new { Class = "k-textbox", maxlength = 100, style = "width: 100%;" };
                page.Write(htmlHelper.Kendo().TextBox()
                                    .Value(propertyValue + "")
                                    .Name(languagePropertyName)
                                    .HtmlAttributes(htmlAttr));

                page.WriteLiteral("<p class=\"errmsg\">");
                page.Write(HtmlHelperValidationExtensions.ValidationMessage(htmlHelper, languagePropertyName, "", new { @class = "text-danger" }));
                page.WriteLiteral("</p>");
            }

            page.WriteLiteral($"</div>");

            if (!allowMultipleCols)
            {
                page.WriteLiteral($"</div>");
            }
        }

        private string GetLabel(string fieldName)
        {
            if (Model != null)
            {
                var field = Model.PageFields.Where(b => string.Compare(fieldName, b.FieldName, true) == 0).FirstOrDefault();
                if (field != null)
                {
                    return field.DisplayLabel;
                }
            }

            string fieldLabelTitle = fieldName;
            if (fieldLabelTitle.EndsWith("ID"))
                fieldLabelTitle = fieldLabelTitle.Substring(0, fieldLabelTitle.Length - 2);

            return fieldLabelTitle;
        }

        private string GetLineItemLabel(string fieldName)
        {
            var model = Model as TransactionEntryPageModel;
            if (model != null)
            {
                var field = model.TransactionLineFields.Where(b => string.Compare(fieldName, b.FieldName, true) == 0).FirstOrDefault();
                if (field != null)
                {
                    return field.DisplayLabel;
                }
            }

            string fieldLabelTitle = fieldName;
            if (fieldLabelTitle.EndsWith("ID"))
                fieldLabelTitle = fieldLabelTitle.Substring(0, fieldLabelTitle.Length - 2);

            return fieldLabelTitle;
        }

        #region Dynamic Grid

        public void AddDynamicGrid(IHtmlHelper<BasePageModel> htmlHelper, RazorPage page, string procedureName, string param1Name, object param1Value)
        {
            var dt = AMSContext.GetDataTable(procedureName, new Dictionary<string, object>()
            {
                { param1Name, param1Value }
            });

            page.WriteLiteral("<Table style='text-align: left;margin-bottom: 0px;margin: 0 auto;' class='table-bordered asset-tab'>");


            page.WriteLiteral("<TR>");
            foreach(DataColumn col in dt.Columns)
            {
                page.WriteLiteral($"<TH style='width: 25%;background-color: #0e5790;color:#fff'>{col.ColumnName}</TH>");
            }
            page.WriteLiteral("<TR>");

            foreach(DataRow dataRow in dt.Rows)
            {
                page.WriteLiteral("<TR>");
                foreach (DataColumn col in dt.Columns)
                {
                    page.WriteLiteral($"<TD>{dataRow[col.ColumnName]}</TD>");
                }
                page.WriteLiteral("<TR>");
            }

            page.WriteLiteral("</Table>");
        }

        #endregion

        public virtual void CreatePageControls(IHtmlHelper<BasePageModel> htmlHelper, RazorPage page, bool readOnlyPage = false,bool QuickCreation=false)
        {
            try
            {
                bool transactionEntryPage = false;
                List<PageFieldModel> transactionFields = null;
                if (Model is TransactionEntryPageModel)
                {
                    transactionFields = (Model as TransactionEntryPageModel).PageFields;

                    AddControlsInSingleColumnForLessItems = false;
                    transactionEntryPage = true;
                }
            
                if (Model.PageFields.Count > 0)
                {
                    if (Model.PageFields[0] is QueryPageFieldModel)
                    {
                        var field = Model.PageFields[0] as QueryPageFieldModel;
                        AddDynamicGrid(htmlHelper, page, field.ProcedureName, field.Param1Name, this.Model.EntityInstance.GetPrimaryKeyValue());

                        return;
                    }
                    if (Model.PageFields[0] is CataloguePageFieldModel)
                    {
                        if (string.Compare(PageName, "Asset") == 0)
                        {
                            var viewName = "MasterViews/CatalogueDetails";
                            object assetID = this.Model.EntityInstance.GetPrimaryKeyValue();
                            if (assetID + "" != "0")
                            {
                                HtmlHelperPartialExtensions.RenderPartial(htmlHelper, viewName, (int)assetID);
                                return;
                            }

                        }

                    }
                }

                //TODO: Load below data from Database
                string _rightName = PageName;
                var gridColumnIndexName = PageName;// "Color"; 

                //get the list of fields requires controls
                var entityInstanceType = Model.EntityInstance.GetType();
                var fieldLists = entityInstanceType.GetProperties();
                //var mIdentity = ModelMetadataIdentity.ForType(entityInstanceType);
                //var provider = new DataAnnotationsModelValidator;

                using (AMSContext db = AMSContext.CreateNewContext())
                {
                   

                    bool allowMultipleCols = Model.PageFields.Count > 5;
                    if (AddControlsInSingleColumnForLessItems == false) allowMultipleCols = true;

                    if (allowMultipleCols)
                    {
                        page.WriteLiteral($"<div class=\"row\">");
                    }

                 
                    int fieldIndex = 0;
                    if (Model.PageFields.Count > 0)
                    {
                        FirstFieldName = Model.PageFields[0].FieldName;
                        foreach (var field in Model.PageFields)
                        {
                          

                            bool controlRendered = false;

                            //process the values
                            //var converter = TypeDescriptor.GetConverter(field.PropertyType);
                            var propertyValue = Model.EntityInstance.GetFieldValue(field.FieldName);

                            if (field.IsHidden)
                            {
                                propertyValue = propertyValue + "" == "0" ? field.DefaultValue : propertyValue;
                                if (string.Compare(field.FieldName, "Year") == 0)
                                {
                                    if (propertyValue+"" == "0")
                                    {
                                        var StartDate = PeriodTable.GetStartDate(db);
                                        int year = StartDate.Year;
                                        propertyValue = year + "";
                                    }
                                    page.Write(htmlHelper.Hidden(field.FieldName, propertyValue + ""));
                                }
                                else
                                {
                                    page.Write(htmlHelper.Hidden(field.FieldName, propertyValue + ""));
                                }
                                controlRendered = true;
                                continue;
                            }

                            if (!allowMultipleCols)
                            {
                                page.WriteLiteral($"<div class=\"row\">");
                            }

                            page.WriteLiteral($"<div class=\"col-xl-4\">");


                            page.WriteLiteral($"<label for=\"fullname\">");
                          
                            page.Write(HtmlHelperLabelExtensions.Label(htmlHelper, Language.GetFieldName(field.DisplayLabel)));

                            if (field.IsMandatory)
                            {
                                page.WriteLiteral($"<span style=\"color: red\">*<span>");
                            }

                            page.WriteLiteral($"</label>");
                            //}

                            //var value = converter.ConvertFromString(propertyValue + "");
                            var partialViewName = "";
                            switch (field.FieldName)
                            {
                                case "CategoryID":
                                    //case "ParentCategoryID":
                                    partialViewName = "MasterViews/DefaultCategory";
                                    break;
                                case "ProductID":
                                    //case "ParentCategoryID":
                                    partialViewName = "MasterViews/DefaultProductWithCatalogue";
                                    break;
                                case "ParentCategoryID":
                                    //case "ParentCategoryID":
                                    partialViewName = "MasterViews/DefaultParentCategory";
                                    break;
                                case "LocationID":
                                    //case "ParentLocationID":
                                    partialViewName = "MasterViews/DefaultLocation";
                                    break;
                                case "ParentLocationID":
                                    //case "ParentLocationID":
                                    partialViewName = "MasterViews/DefaultParentLocation";
                                    break;
                                case "EmployeeID":
                                    //case "ParentLocationID":
                                    partialViewName = "MasterViews/DefaultEmployee";
                                    break;

                                case "DelegatedEmployeeID":
                                    //case "ParentLocationID":
                                    partialViewName = "MasterViews/DefaultDelegatedEmployee";
                                    break;

                                case "CategoryTypeID":
                                    //case "ParentLocationID":
                                    partialViewName = "MasterViews/DefaultCategoryTypeID";
                                    break;

                                case "PostingStatusID":
                                    //case "ParentLocationID":
                                    propertyValue = propertyValue + "" == "0" ? (int)PostingStatusValue.WorkInProgress + "" : propertyValue + "";
                                    partialViewName = "MasterViews/DefaultPostingStatusID";
                                    break;
                            }

                            if (string.IsNullOrEmpty(partialViewName) == false)
                            {
                                HtmlHelperPartialExtensions.RenderPartial(htmlHelper, partialViewName, propertyValue + "" == "0" ? "" : propertyValue + "");
                                if (!readOnlyPage && !QuickCreation && field.IsMasterCreation)
                                {
                                    page.WriteLiteral($"<a style=\"min-width: 25px;\" onclick=\"addMastePageClicked('" + field.FieldName.Replace("ID", "") + "')\" ><img src = '/css/images/AddNewRecordIcon.png' alt='icon' style='Width:20px;height:25px' /></a>");
                                }
                                page.WriteLiteral("<p class=\"errmsg\">");
                                var attrNew = new Dictionary<string, object>()
                                        {
                                            { "class", "text-danger" },
                                            { "id", field.FieldName + "Validator" }
                                        };
                                page.Write(HtmlHelperValidationExtensions.ValidationMessage(htmlHelper, field.FieldName, "", attrNew));
                                page.WriteLiteral("</p>");

                                controlRendered = true;
                            }
                            else
                            {
                                RenderControl(htmlHelper, page, db, readOnlyPage, entityInstanceType, field, field.PropertyType, propertyValue,
                                    field.StringMaxLength, Model.PageFields, false, QuickCreation);
                            }

                            page.WriteLiteral($"</div>");

                            if (!allowMultipleCols)
                            {
                                page.WriteLiteral($"</div>");
                            }

                            fieldIndex++;
                        }

                      
                    }
                    if (allowMultipleCols)
                    {
                        page.WriteLiteral($"</div>");
                    }
                    //if (transactionEntryPage)
                    //{
                    //    AddTransactionLineItems(htmlHelper, page, db, readOnlyPage);
                    //}
                }
            }
            catch (Exception ex)
            {
                ApplicationErrorLogTable.SaveException(ex);
                throw new Exception("Internal Server Error");
            }
        }

        //private void RenderControl(IHtmlHelper<BasePageModel> htmlHelper, RazorPage page, AMSContext db, bool readOnlyPage, Type entityInstanceType,
        //     PropertyInfo field, Type propertyType, object propertyValue, int stringMaxLength, List<PageFieldModel> pageFieldModels,
        //     bool istreeView = false, bool hiddenfield = false)
        //{
        //    RenderControl(htmlHelper, page, db, readOnlyPage, entityInstanceType, field, propertyType, propertyValue, 
        //        stringMaxLength, pageFieldModels, istreeView, hiddenfield);
        //}
        public virtual void CreatePageControlsForAuditLog(IHtmlHelper<BasePageModel> htmlHelper, RazorPage page, bool readOnlyPage = false)
        {
            try
            {
                bool transactionEntryPage = false;
                List<PageFieldModel> transactionFields = null;
                if (Model is TransactionEntryPageModel)
                {
                    transactionFields = (Model as TransactionEntryPageModel).PageFields;

                    AddControlsInSingleColumnForLessItems = false;
                    transactionEntryPage = true;
                }

                //TODO: Load below data from Database
                string _rightName = PageName;
                var gridColumnIndexName = PageName;// "Color"; 

                //get the list of fields requires controls
                var entityInstanceType = Model.EntityInstance.GetType();
                var fieldLists = entityInstanceType.GetProperties();
                //var mIdentity = ModelMetadataIdentity.ForType(entityInstanceType);
                //var provider = new DataAnnotationsModelValidator;

                using (AMSContext db = AMSContext.CreateNewContext())
                {
                    List<PropertyInfo> allowedFields = new List<PropertyInfo>();
                    foreach (var field in fieldLists)
                    {
                        var propertyType = field.PropertyType;
                        string fieldLabelTitle = field.Name;

                        //its a non null field
                        if (Nullable.GetUnderlyingType(field.PropertyType) != null)
                        {
                            propertyType = Nullable.GetUnderlyingType(field.PropertyType);
                        }

                        if ((propertyType != typeof(string)) && (propertyType.IsClass) && (propertyType.BaseType != typeof(System.Array)))
                            continue;
                        if (field.Name == PrimaryKeyFieldName || AuditLogFields.Contains(field.Name))
                        {
                            allowedFields.Add(field);
                        }
                    }

                    bool allowMultipleCols = allowedFields.Count > 5;
                    if (AddControlsInSingleColumnForLessItems == false) allowMultipleCols = true;

                    if (allowMultipleCols)
                    {
                        page.WriteLiteral($"<div class=\"row\">");
                    }
                    int fieldIndex = 0;
                    if (allowedFields.Count > 0)
                    {
                        FirstFieldName = allowedFields[0].Name;
                        foreach (var field in allowedFields.Where(c => c.Name != PrimaryKeyFieldName))
                        {
                            var customAtts = field.CustomAttributes;

                            int stringMaxLength = 2000;
                            bool isRequired = false;

                            var propertyType = field.PropertyType;
                            string fieldLabelTitle = GetLabel(field.Name);

                            //its a non null field
                            if (Nullable.GetUnderlyingType(field.PropertyType) == null)
                            {
                                isRequired = true;
                            }
                            else
                            {
                                propertyType = Nullable.GetUnderlyingType(field.PropertyType);
                            }

                            if (propertyType == typeof(string))
                                isRequired = false;

                            foreach (var customAttribute in customAtts)
                            {
                                if (customAttribute.AttributeType == typeof(StringLengthAttribute))
                                {
                                    stringMaxLength = (int)customAttribute.ConstructorArguments[0].Value;
                                }

                                if (customAttribute.AttributeType == typeof(RequiredAttribute))
                                {
                                    isRequired = true;
                                }

                                if (customAttribute.AttributeType == typeof(DisplayNameAttribute))
                                {
                                    fieldLabelTitle = customAttribute.ConstructorArguments[0].Value + "";
                                }

                                if (customAttribute.AttributeType.Name == "NullableAttribute")
                                {
                                    isRequired = false;
                                }
                            }


                            if (!allowMultipleCols)
                            {
                                page.WriteLiteral($"<div class=\"row\">");
                            }

                            page.WriteLiteral($"<div class=\"col-xl-4\">");




                            page.WriteLiteral($"<label for=\"fullname\"><b>");

                            bool hiddenField = false;

                            if (((string.Compare(PageName, "Asset") == 0 && string.Compare(field.Name, "ProductID") == 0))
                                || string.Compare(field.Name, "Year") == 0)

                            {
                                hiddenField = true;
                            }
                            else
                            {
                                page.Write(HtmlHelperLabelExtensions.Label(htmlHelper, Language.GetFieldName(fieldLabelTitle) + ":"));
                                page.WriteLiteral($"</b></label>");
                                var propertyValue = field.GetValue(Model.EntityInstance);
                                if (propertyValue != null)
                                {
                                    if (propertyType == typeof(DateTime))
                                    {
                                        DateTime? newValue = null;
                                        if ((propertyValue != null) && (propertyValue.GetType() == typeof(DateTime)))
                                        {
                                            newValue = (DateTime)propertyValue;

                                            if (newValue != null)
                                            {
                                                // String.Format("{0:" + CultureHelper.ConfigureDateFormat + "}", newValue)
                                                page.Write(HtmlHelperLabelExtensions.Label(htmlHelper, String.Format("{0:" + CultureHelper.ConfigureAuditLogDateFormat + "}", newValue)));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        int userId = int.Parse(propertyValue + "");
                                        string name = PersonTable.GetPersonUserName(db, userId);
                                        page.Write(HtmlHelperLabelExtensions.Label(htmlHelper, name + ""));
                                    }

                                }
                            }

                            page.WriteLiteral($"</div>");

                            if (!allowMultipleCols)
                            {
                                page.WriteLiteral($"</div>");
                            }

                            fieldIndex++;
                        }
                        foreach (var field in allowedFields.Where(c => c.Name == PrimaryKeyFieldName))
                        {
                            var customAtts = field.CustomAttributes;
                            var propertyType = field.PropertyType;
                            var propertyValue = field.GetValue(Model.EntityInstance);

                            page.WriteLiteral($"<div class=\"col-xl-4\">");
                            page.WriteLiteral($"<a class='k-button-icontext' href='javascript:SingleAuditLogReport(\"" + propertyValue + "\",\"" + entityInstanceType.Name + "\")'>" + Language.GetString(entityInstanceType.Name.Replace("Table", "")) + " Audit Log</a>");
                            page.WriteLiteral($"</div>");
                        }
                    }


                    if (allowMultipleCols)
                    {
                        page.WriteLiteral($"</div>");
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationErrorLogTable.SaveException(ex);
                throw new Exception("Internal Server Error");
            }
        }
            
            private void RenderControl(IHtmlHelper<BasePageModel> htmlHelper, RazorPage page, AMSContext db, bool readOnlyPage, Type entityInstanceType,
             PageFieldModel field, Type propertyType, object propertyValue, int stringMaxLength, List<PageFieldModel> pageFieldModels,
             bool istreeView = false,bool QuickCreation=false)
        {
            string currentFieldName = field.FieldName;

            bool controlRendered = false;
            bool periodPage = false;
            int year = DateTime.Now.Year;
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;
            if (string.Compare(PageName, "Period") == 0)
            {
                StartDate = PeriodTable.GetStartDate(db);
                EndDate= StartDate.AddMonths(1).AddDays(-1);
                year = StartDate.Year;
                periodPage = true;
            }

            if (istreeView)
            {
                page.Write(htmlHelper.Kendo().TreeView().Name("treeView").LoadOnDemand(false)
                    .Events(e => { e.Select("treeViewSelect"); }).DataTextField("text")
                    .Items(menu =>
                    {
                        DisplayHelper helper = new DisplayHelper(null, "", "");

                        if (string.Compare(PageName, "Category") == 0 || string.Compare(PageName, "Product") == 0)
                        {
                            helper.AddTreeViewItems(menu);
                        }
                        else
                        {
                            helper.AddTreeViewLocationItems(menu);
                        }

                    }
                    ));
                controlRendered = true;
            }

            if (pageFieldModels != null)
            {
                var fieldModel = pageFieldModels.Where(b => string.Compare(b.FieldName, currentFieldName, true) == 0).FirstOrDefault();
                if ((fieldModel != null) && (fieldModel.ControlType == PageControlTypes.MultiColumnComboBox))
                {
                    RenderMultiColumnDropDown(htmlHelper, page, db, readOnlyPage, fieldModel, propertyValue);

                    controlRendered = true;
                }
                if ((fieldModel != null) && (fieldModel.ControlType == PageControlTypes.DocUpload))
                {
                    if (!readOnlyPage)
                    {
                        page.Write(htmlHelper.Kendo().Upload()
                                                 .Name("UploadDoc")
                                                 .Async(a => a
                                                 .Save("DocumentUpload", "MasterPage", new { currentPageID = Model.EntityInstance.CurrentPageID, PageName = PageName })
                                                 .Remove("DocumentRemove", "MasterPage", new { currentPageID = Model.EntityInstance.CurrentPageID, PageName = PageName })
                                                 .AutoUpload(true)
                                                     ).Multiple(false)
                                                 .Events(events => events
                                                 .Success("OnSuccessUpload")
                                                 )
                                                 .Validation(validation => validation
                                               .AllowedExtensions(".txt", ".doc", ".docx", ".pdf", ".xls", ".xlsx")

                                                 )
                                                 );
                    }

                   // page.Write(htmlHelper.Hidden(field.FieldName));
                    if (propertyValue != null)
                    {
                        string[] arryDocID1 = propertyValue.ToString().Split("FileStoragePath");
                        var url = "/FileStoragePath" + arryDocID1[1];
                        string displayName = url.Split(new[] { '\\', '/' }).Last();
                

                        page.Write(htmlHelper.ActionLink(displayName, "DownloadFile", new { fileName = arryDocID1[1] }));
                        page.Write(htmlHelper.Hidden(field.FieldName, propertyValue));
                    }
                    else
                    {
                        page.Write(htmlHelper.Hidden(field.FieldName));
                        //page.WriteLiteral("<img id='StampImage' src='/FileStoreagePath/StampPath/' width='150' height='100' style='margin-top:5px;display:none'>");

                    }
                    controlRendered = true;
                }
                if ((fieldModel != null) && (fieldModel.ControlType == PageControlTypes.ImageUpload))
                {
                    if (!readOnlyPage)
                    {
                        page.Write(htmlHelper.Kendo().Upload()
                                             .Name("ImageDoc")
                                             .Async(a => a
                                             .Save("ImageUpload", "MasterPage", new { currentPageID = Model.EntityInstance.CurrentPageID,PageName=PageName })
                                             .Remove("ImageRemove", "MasterPage", new { currentPageID = Model.EntityInstance.CurrentPageID, PageName = PageName })
                                             .AutoUpload(true)
                                                 ).Multiple(false)
                                             .Events(events => events
                                             .Success("OnImageSuccessUpload")
                                             )
                                             .Validation(validation => validation
                                               .AllowedExtensions(".jpg", ".jpeg", ".png", ".gif")

                                             )
                                             );
                    }

                    // page.Write(htmlHelper.Hidden(field.FieldName));
                    if (propertyValue != null)
                    {
                        string[] arryDocID1 = propertyValue.ToString().Split("FileStoragePath");
                        var url = "/FileStoragePath" + arryDocID1[1];
                        string displayName = url.Split(new[] { '\\', '/' }).Last();

                        page.WriteLiteral("<img id='ImageID' src='" + url + "' alt='No Image' width='150' height='100'>");
                        if (!readOnlyPage )
                        {
                            page.WriteLiteral("   <a> <label id = \"imageRemove\" style = \"font-weight: normal\" onclick = \"Clearimage();\" title = \"remove\" ><u> remove </u></label></a>");
                        }
                        //page.Write(htmlHelper.ActionLink(displayName, "DownloadFile", new { fileName = arryDocID1[1] }));
                        page.Write(htmlHelper.Hidden(field.FieldName, propertyValue));
                    }
                    else
                    {
                        page.Write(htmlHelper.Hidden(field.FieldName));
                        //page.WriteLiteral("<img id='StampImage' src='/FileStoreagePath/StampPath/' width='150' height='100' style='margin-top:5px;display:none'>");

                    }
                    controlRendered = true;
                }
                if ((fieldModel != null) && (fieldModel.ControlType == PageControlTypes.TextArea) && (controlRendered == false))
                {
                    
                        var htmlAttr = new { Class = "k-textbox", maxlength = stringMaxLength, style = "width: 100%; " };
                        page.Write(htmlHelper.Kendo().TextArea()
                            .Value(propertyValue + "")
                            .Name(currentFieldName).Rows(4)
                          .HtmlAttributes(htmlAttr));
                    
                    controlRendered = true;
                }
            }

            if (controlRendered == false)
            {
                if (!TreeViewReferenceID.Contains(currentFieldName))
                {
                    var foreignKeyProperty = EntityPropertyHelper.GetForeignKeyField(entityInstanceType, currentFieldName);
                    if (foreignKeyProperty != null)
                    {
                        var foreignkeyTableType = foreignKeyProperty.PropertyType;
                        var selectList = EntityHelper.GetSelectList(foreignkeyTableType, db, SessionUser.Current.UserID);

                        page.Write(htmlHelper.Kendo().DropDownList()
                        .OptionLabel($"Select {Language.GetString(currentFieldName)}")
                        .BindTo(selectList)
                         .DataSource(ds => ds.Read(r => r.Action("GetDropdownData", "DataService", new {fieldName= field.FieldName.Replace("ID", "") })))
                        .Name(currentFieldName)
                        .Value(propertyValue + "").Enable(!readOnlyPage));

                        if (!readOnlyPage && !QuickCreation && field.IsMasterCreation)
                        {
                            page.WriteLiteral($"<a style=\"min-width: 25px;\" onclick=\"addMastePageClicked('" + field.FieldName.Replace("ID", "") + "')\" ><img src = '/css/images/AddNewRecordIcon.png' alt='icon' style='Width:20px;height:25px' /></a>");
                        }
                        controlRendered = true;
                    }
                }
                else if (string.Compare(PageName, "Asset") == 0)
                {
                    page.WriteLiteral("<div class='input-group'>");
                    var htmlAttr = new { Class = "form-control", @onkeydown = "ValidateKey(event);", @onblur = "ongroupBoxchangeevent(this.value,'" + currentFieldName + "');", style = "width: 90%;" };
                    string value = string.Empty;
                    if (propertyValue != null)
                    {
                        value = CategoryTable.GetName(db, (int)propertyValue, currentFieldName);
                    }

                    page.Write(htmlHelper.TextBox(currentFieldName.Replace("ID", "Name"), value + "", htmlAttr));
                    page.WriteLiteral("<span class='input-group-btn'>");
                    if (!readOnlyPage)
                    {
                        if (string.Compare(currentFieldName, "CategoryID") == 0)
                        {
                            page.WriteLiteral(" <button class=\"btn btn-add\" type=\"button\" onclick=\"onPopupOpenWindow(2,'SelectCategory')\"><i class=\"fa fa-hand-o-up\"></i></button>");
                        }
                        else
                        {
                            page.WriteLiteral(" <button class=\"btn btn-add\" type=\"button\" onclick=\"onPopupOpenWindow(1,'SelectLocation')\"><i class=\"fa fa-hand-o-up\"></i></button>");
                        }
                    }

                    page.WriteLiteral("</span>");
                    page.WriteLiteral("</div>");
                    if (string.Compare(currentFieldName, "Year") == 0)
                    {
                        if(propertyValue=="0")
                        {
                            propertyValue = year + "";
                        }
                        page.Write(htmlHelper.Hidden(currentFieldName, propertyValue + ""));
                    }
                    else
                    {
                        page.Write(htmlHelper.Hidden(currentFieldName, propertyValue + ""));
                    }
                    controlRendered = true;
                }
                else
                {
                    string value = string.Empty;
                    if (propertyValue != null)
                    {
                        value = CategoryTable.GetName(db, (int)propertyValue, currentFieldName);
                    }

                    var dHtmlAttr = new { Class = "k-textbox", maxlength = stringMaxLength, @readonly = "readonly", style = "width: 100%;" };
                    page.Write(htmlHelper.Kendo().TextBox()
                                       .Value(value + "")
                                       .Name("NameValue")
                                       .HtmlAttributes(dHtmlAttr));
                    page.Write(htmlHelper.Hidden(currentFieldName, propertyValue + ""));
                    controlRendered = true;
                }
            }

            if (controlRendered == false)
            {
                if (propertyType == typeof(bool))
                {
                    var isChecked = false;
                    if ((propertyValue != null) && (propertyValue.GetType() == typeof(bool)))
                    {
                        isChecked = (bool)propertyValue;
                    }
                    page.WriteLiteral($"<br/>");
                    page.Write(htmlHelper.Kendo().Switch().Checked(isChecked)
                            .Messages(m => m.Checked("Yes").Unchecked("No"))
                            .Name(currentFieldName));
                }
                else
                {
                    if (propertyType == typeof(DateTime))
                    {
                        DateTime? newValue = DateTime.Now;
                        if ((propertyValue != null) && (propertyValue.GetType() == typeof(DateTime)))
                        {
                            newValue = (DateTime)propertyValue;
                            //if (newValue == DateTime.MinValue)
                            //{
                            //    newValue = DateTime.Now;
                            //}
                        }
                        else
                            newValue = null;
                        page.WriteLiteral($"<br/>");
                        if (periodPage)
                        {
                            if (string.Compare(currentFieldName, "StartDate") == 0)
                            {
                                DateTime defaultVal = new DateTime();
                                if ((DateTime)propertyValue == defaultVal)
                                {
                                    propertyValue = StartDate;
                                }
                                page.Write(htmlHelper.Kendo().DatePicker()
                                     .Name(currentFieldName + "Val")
                                     .Format(CultureHelper.ConfigureDateFormat)
                                     .Value((DateTime)propertyValue).Enable(false));

                                page.Write(htmlHelper.Hidden(currentFieldName, (DateTime)propertyValue));
                            }
                            else
                            {
                                if (string.Compare(currentFieldName, "EndDate") == 0)
                                {
                                    DateTime defaultVal = new DateTime();
                                    if ((DateTime)propertyValue == defaultVal)
                                    {
                                        propertyValue = EndDate;
                                    }
                                    page.Write(htmlHelper.Kendo().DatePicker()
                                   .Name(currentFieldName)
                                    .Events(e => e.Change("onEndDatePicker"))
                                   .Format(CultureHelper.ConfigureDateFormat)
                                   .Value((DateTime)propertyValue).Enable(!readOnlyPage));
                                }
                            }
                        }
                        else
                        {
                            if (currentFieldName == "TransactionDate")
                            {
                                page.Write(htmlHelper.Kendo().DatePicker()
                                .Name(currentFieldName)
                                    .Format(CultureHelper.ConfigureDateFormat).Max(DateTime.Now).Min(DateTime.Now.AddDays(-60))
                                .Value(newValue).Enable(!readOnlyPage));
                            }
                            else
                            {
                                page.Write(htmlHelper.Kendo().DatePicker()
                              .Name(currentFieldName)
                              .Format(CultureHelper.ConfigureDateFormat)
                               .Value(newValue).Enable(!readOnlyPage));
                            }

                        }
                    }
                    else
                    {
                        var htmlAttr = new { Class = "k-textbox", maxlength = stringMaxLength, style = "width: 100%;" };
                        if (readOnlyPage)
                        {
                            if ((string.Compare(currentFieldName, "ErrorMessage", true) == 0) || (string.Compare(currentFieldName, "StackTrace", true) == 0))
                            {
                                var dHtmlAttr = new { Class = "k-textbox", style = "width: 100%;" };
                                page.Write(htmlHelper.Kendo().TextArea()
                                       .Value(propertyValue + "")
                                       .Name(currentFieldName).Rows(7)
                                       .HtmlAttributes(dHtmlAttr));
                            }
                            else if (string.Compare(currentFieldName, "StampPath", true) != 0)
                            {
                                var dHtmlAttr = new { Class = "k-textbox", disabled = "disabled", style = "width: 100%;" };
                                page.Write(htmlHelper.TextBox("dValue", propertyValue + "", dHtmlAttr));
                            }
                            else if (string.Compare(currentFieldName, "StampPath", true) == 0)

                            {
                                if (propertyValue != null)
                                {
                                    string[] arryDocID1 = propertyValue.ToString().Split("FileStoragePath");
                                    var url = "/FileStoragePath" + arryDocID1[1];
                                    page.WriteLiteral("<img id='StampImage' src='" + url + "' alt='No Image' width='150' height='100'>");
                                    if (!readOnlyPage)
                                    {
                                        page.WriteLiteral("   <a> <label id = \"imageRemove\" style = \"font-weight: normal\" onclick = \"Clearimage();\" title = \"remove\" ><u> remove </u></label></a>");
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (propertyType.IsValueType)
                            {
                                if ((propertyValue != null) && (propertyValue.GetType() == typeof(int)))
                                {
                                    var value = 0;
                                    value = (int)propertyValue;

                                    //handle meta data here
                                    page.Write(htmlHelper.Kendo().IntegerTextBox()
                                    .Name(currentFieldName)
                                    .Value(value).Min(0)
                                    .HtmlAttributes(htmlAttr));

                                }
                                else
                                {
                                    double value = 0;
                                    double.TryParse(propertyValue + "", out value);

                                    //handle meta data here
                                    page.Write(htmlHelper.Kendo().NumericTextBox()
                                        .Name(currentFieldName)
                                        .Value(value).Min(0)
                                        .HtmlAttributes(htmlAttr));

                                }
                            }
                            else
                            {
                                //handle meta data here
                                if (string.Compare(PageName, "Asset", true) == 0 && string.Compare(currentFieldName, "AssetDescriptionNA", true) == 0)
                                {
                                    var dHtmlAttr = new { Class = "k-textbox", maxlength = stringMaxLength, @readonly = "readonly", style = "width: 100%;height: 200px !important;" };

                                    page.Write(htmlHelper.Kendo().TextArea()
                                        .Value(propertyValue + "")
                                        .Name(currentFieldName).Rows(4)
                                        .HtmlAttributes(dHtmlAttr));
                                    //if (string.Compare(currentFieldName, "AssetDescription", true) == 0  )
                                    //{
                                    //    page.WriteLiteral(" <button class=\"btn btn-add\" type=\"button\" onclick=\"UpdateDesc()\"><i class=\"fas fa-edit\"></i>Edit</button>");
                                    //    //var productValue = field.GetValue("ProductID");


                                    //}
                                }
                                else if (string.Compare(currentFieldName, "TransactionNo", true) == 0)
                                {
                                    var dHtmlAttr = new { Class = "k-textbox", maxlength = stringMaxLength, disabled = "disabled", style = "width: 100%;" };
                                    propertyValue = propertyValue == null ? "AUTO" : propertyValue;
                                    page.Write(htmlHelper.Kendo().TextBox()
                                       .Value(propertyValue + "")
                                       .Name(currentFieldName)
                                       .HtmlAttributes(dHtmlAttr));
                                }
                                else if (string.Compare(currentFieldName, "StampPath", true) == 0)
                                {
                                    if (!readOnlyPage)
                                    {
                                        page.Write(htmlHelper.Kendo().Upload()
                                            .Name(currentFieldName + "1")
                                          .Async(a => a
                                            .Save(currentFieldName + "Upload", "Person", new { currentPageID = Model.EntityInstance.CurrentPageID })
                                            .Remove(currentFieldName + "Remove", "Person", new { currentPageID = Model.EntityInstance.CurrentPageID })
                                            .AutoUpload(true)
                                                ).Multiple(false)
                                            .Events(events => events
                                            .Success("OnSuccessUpload")
                                            )
                                            .Validation(validation => validation
                                            .AllowedExtensions(".jpg", ".jpeg", ".png", ".gif")

                                            )
                                            );
                                    }
                                    page.Write(htmlHelper.Hidden(currentFieldName));
                                    page.Write(htmlHelper.Hidden("rootPath"));
                                    if (propertyValue != null)
                                    {
                                        string[] arryDocID1 = propertyValue.ToString().Split("FileStoragePath");
                                        var url = "/FileStoragePath" + arryDocID1[1];
                                        page.WriteLiteral("<img id='StampImage' src='" + url + "' alt='No Image' width='150' height='100'>");
                                        if (!readOnlyPage)
                                        {
                                            page.WriteLiteral("   <a> <label id = \"imageRemove\" style = \"font-weight: normal\" onclick = \"Clearimage();\" title = \"remove\" ><u> remove </u></label></a>");
                                        }
                                    }
                                    else

                                    {
                                        page.WriteLiteral("<img id='StampImage' src='/FileStoreagePath/StampPath/' width='150' height='100' style='margin-top:5px;display:none'>");

                                    }
                                }
                                else
                                {
                                    var codehtmlAttr = new { Class = "k-textbox", maxlength = stringMaxLength, style = "width: 100%;", @onblur = "removeSpace(this.value,this.id);", @onkeypress = "removeSpecialCharacter(event);" };
                                    if (currentFieldName.EndsWith("Code"))
                                    {
                                        page.Write(htmlHelper.Kendo().TextBox()
                                          .Value(propertyValue + "")
                                          .Name(currentFieldName)
                                          .HtmlAttributes(codehtmlAttr));
                                    }
                                    else
                                    {
                                        page.Write(htmlHelper.Kendo().TextBox()
                                            .Value(propertyValue + "")
                                            .Name(currentFieldName)
                                            .HtmlAttributes(htmlAttr));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (!istreeView)
            {
                page.WriteLiteral("<p class=\"errmsg\">");
                var attrNew = new Dictionary<string, object>()
                {
                    { "class", "text-danger" },
                    { "id", field.FieldName + "Validator" }
                };
                page.Write(HtmlHelperValidationExtensions.ValidationMessage(htmlHelper, currentFieldName, "", attrNew));
                page.WriteLiteral("</p>");
            }

        }

        private void RenderMultiColumnDropDown(IHtmlHelper<BasePageModel> htmlHelper, RazorPage page, AMSContext db, bool readOnlyPage,
         PageFieldModel model, object propertyValue)
        {
            if (propertyValue == null)
                propertyValue = "";
            var fieldModel = new ScreenControlViewModel(ASelectionControlQueryTable.GetItem(db, model.ControlName), model.FieldName)
            {
                //IsMandatory = itm.IsMandatory
                Value = propertyValue
            };
            fieldModel.ChangeScriptFunctionName = model.ChangeMethodName;
            fieldModel.PlaceholderText = "Select " + model.DisplayLabel;
            //viewFile = "/Views/Shared/ReportFields/DynamicFilterControl.ascx";
            fieldModel.ParentFieldName = model.CascadeFrom;
            fieldModel.DataReadScriptFunctionName = model.DataReadScriptFunctionName;

            //page.Write(ctrl);

            htmlHelper.RenderPartial("/Views/Shared/CommonControls/DefaultMultiColumnComboBox.cshtml", fieldModel);
        }

        public virtual void CreateNotificationPageControls(IHtmlHelper<BasePageModel> htmlHelper, RazorPage page, bool readOnlyPage = false)
        {
            //TODO: Load below data from Database
            string _rightName = PageName;
            var gridColumnIndexName = PageName;// "Color"; 

            //get the list of fields requires controls
            var entityInstanceType = Model.EntityInstance.GetType();
            var fieldLists = entityInstanceType.GetProperties();
            // Type EntityLineItemInstance= entityInstanceType;


            List<PropertyInfo> allowedFields = new List<PropertyInfo>();
            foreach (var field in fieldLists)
            {
                var propertyType = field.PropertyType;
                string fieldLabelTitle = field.Name;

                //its a non null field
                if (Nullable.GetUnderlyingType(field.PropertyType) != null)
                {
                    propertyType = Nullable.GetUnderlyingType(field.PropertyType);
                }

                if ((propertyType != typeof(string)) && (propertyType.IsClass))
                    continue;

                if (propertyType.IsGenericType) continue;
                if (field.Name == PrimaryKeyFieldName) continue;
                if (commonFields.Contains(field.Name)) continue;
                if (field.Name.StartsWith("Attribute")) continue;

                allowedFields.Add(field);
            }
            if (string.Compare(PageName, "NotificationTemplate") == 0)
            {
                var EntityLineItemInstance = Activator.CreateInstance<NotificationReportAttachmentTable>().GetType();
                var LineLists = EntityLineItemInstance.GetProperties();
                string linePrimaryKeyID = EntityLineItemInstance.Name.Replace("Table", "ID");
                foreach (var line in LineLists)
                {
                    var propertyType = line.PropertyType;
                    string fieldLabelTitle = line.Name;

                    //its a non null field
                    if (Nullable.GetUnderlyingType(line.PropertyType) != null)
                    {
                        propertyType = Nullable.GetUnderlyingType(line.PropertyType);
                    }

                    if ((propertyType != typeof(string)) && (propertyType.IsClass))
                        continue;

                    if (propertyType.IsGenericType) continue;
                    if (line.Name == linePrimaryKeyID) continue;
                    if (line.Name == "NotificationTemplateID") continue;
                    if (commonFields.Contains(line.Name)) continue;
                    if (line.Name.StartsWith("Attribute")) continue;

                    allowedFields.Add(line);
                }

            }
            bool allowMultipleCols = allowedFields.Count > 5;
            if (AddControlsInSingleColumnForLessItems == false) allowMultipleCols = true;

            if (allowMultipleCols)
            {
                page.WriteLiteral($"<div class=\"row\">");
            }

            if (allowedFields.Count > 0)
            {
                FirstFieldName = allowedFields[0].Name;
                foreach (var field in allowedFields)
                {
                    var customAtts = field.CustomAttributes;

                    int stringMaxLength = 1000;
                    bool isRequired = false;

                    var propertyType = field.PropertyType;
                    string fieldLabelTitle = field.Name;
                    if (fieldLabelTitle.EndsWith("ID"))
                        fieldLabelTitle = fieldLabelTitle.Substring(0, fieldLabelTitle.Length - 2);

                    //its a non null field
                    if (Nullable.GetUnderlyingType(field.PropertyType) == null)
                    {
                        isRequired = true;
                    }
                    else
                    {
                        propertyType = Nullable.GetUnderlyingType(field.PropertyType);
                    }

                    foreach (var customAttribute in customAtts)
                    {
                        if (customAttribute.AttributeType == typeof(StringLengthAttribute))
                        {
                            stringMaxLength = (int)customAttribute.ConstructorArguments[0].Value;
                        }

                        if (customAttribute.AttributeType == typeof(RequiredAttribute))
                        {
                            isRequired = true;
                        }

                        if (customAttribute.AttributeType == typeof(DisplayNameAttribute))
                        {
                            fieldLabelTitle = customAttribute.ConstructorArguments[0].Value + "";
                        }

                        if (customAttribute.AttributeType.Name == "NullableAttribute")
                        {
                            isRequired = false;
                        }
                    }

                    if (!allowMultipleCols)
                    {
                        page.WriteLiteral($"<div class=\"row\">");
                    }


                    if (field.Name == "TemplateHeaderBodyContent" || field.Name == "TemplateDetailsBodyContent" || field.Name == "TemplateFooterBodyContent")
                    {
                        page.WriteLiteral($"<div id=\"div" + fieldLabelTitle + "\" class=\"col-xl-12\">");
                    }
                    else
                    {
                        page.WriteLiteral($"<div id=\"div" + fieldLabelTitle + "\" class=\"col-xl-4\">");
                    }

                    page.WriteLiteral($"<label for=\"fullname\">");

                    page.Write(HtmlHelperLabelExtensions.Label(htmlHelper, Language.GetFieldName(fieldLabelTitle)));

                    if (isRequired)
                    {
                        page.WriteLiteral($"&nbsp;&nbsp;<span style=\"color: red\">*<span>");
                    }

                    page.WriteLiteral($"</label>");

                    bool controlRendered = false;

                    //process the values
                    //var converter = TypeDescriptor.GetConverter(field.PropertyType);
                    object propertyValue = new object();
                    var keyProperty = Model.EntityInstance.GetType().GetProperty(PrimaryKeyFieldName);
                    int primaryID = (int)keyProperty.GetValue(Model.EntityInstance);

                    if (string.Compare(field.DeclaringType.Name, string.Concat(Model.PageName, "Table")) == 0)
                    {
                        propertyValue = field.GetValue(Model.EntityInstance);
                    }
                    else
                    {

                        var LineItemInstance = Activator.CreateInstance<NotificationReportAttachmentTable>();
                        if (primaryID > 0)
                        {
                            var reportAttachment = NotificationReportAttachmentTable.GetReportAttachment(AMSContext.CreateNewContext(), primaryID);
                            if (reportAttachment != null)
                            {
                                propertyValue = field.GetValue(reportAttachment);
                            }
                            else
                            {
                                propertyValue = null;
                            }
                        }

                        else
                        {
                            propertyValue = field.GetValue(LineItemInstance);
                        }
                    }

                    var partialViewName = "";
                    var ModelName = "";
                    bool txtRead = false;
                    if (field.Name.Contains("DestinationField"))
                    {
                        txtRead = false;
                    }
                    switch (field.Name)
                    {
                        case "NotificationModuleID":
                            partialViewName = "MasterViews/DefaultNotificationModule";
                            break;
                        case "TemplateHeaderBodyContent":
                            ModelName = "TemplateHeaderBodyContent";
                            partialViewName = "MasterViews/TemplateHeaderContent";
                            break;
                        case "TemplateDetailsBodyContent":
                            ModelName = "TemplateDetailsBodyContent";
                            partialViewName = "MasterViews/TemplateBodyContent";
                            break;
                        case "TemplateFooterBodyContent":
                            ModelName = "TemplateFooterBodyContent";
                            partialViewName = "MasterViews/TemplateFooterContent";
                            break;
                        case "NotificationTypeID":
                            partialViewName = "MasterViews/DefaultNotificationType";
                            break;
                        case "EmailSignatureID":
                            partialViewName = "MasterViews/EmailSignature";
                            break;
                        case "AttachmentFormatID":
                            partialViewName = "MasterViews/ReportAttachment";
                            break;
                        case "ReportID":
                            partialViewName = "MasterViews/UserReport";
                            break;
                        case "SourceField1":
                            ModelName = "SourceField1";
                            partialViewName = "MasterViews/UserReportFilter";
                            break;
                        case "SourceField2":
                            ModelName = "SourceField2";
                            partialViewName = "MasterViews/UserReportFilter";
                            break;
                        case "SourceField3":
                            ModelName = "SourceField3";
                            partialViewName = "MasterViews/UserReportFilter";
                            break;

                    }

                    if (string.IsNullOrEmpty(partialViewName) == false)
                    {

                        if (field.Name.Contains("SourceField"))
                        {
                            Tuple<string, string> objName = new Tuple<string, string>(ModelName, propertyValue + "");
                            HtmlHelperPartialExtensions.RenderPartial(htmlHelper, partialViewName, objName);
                            controlRendered = true;
                        }
                        else
                        {
                            HtmlHelperPartialExtensions.RenderPartial(htmlHelper, partialViewName, propertyValue + "");
                            controlRendered = true;
                        }

                    }

                    if (controlRendered == false)
                    {
                        if (propertyType == typeof(bool))
                        {
                            var isChecked = false;
                            if (propertyValue != null)
                            {
                                if (propertyValue.GetType() == typeof(bool))
                                {
                                    isChecked = (bool)propertyValue;
                                }
                            }

                            page.WriteLiteral($"<br/>");
                            page.Write(htmlHelper.Kendo().CheckBox().Checked(isChecked)
                                    .Name(field.Name));
                        }
                        else
                        {
                            var htmlAttr = new { Class = "k-textbox", maxlength = stringMaxLength, style = "width: 100%;" };
                            if (readOnlyPage)
                            {
                                var dHtmlAttr = new { Class = "k-textbox", disabled = "disabled", style = "width: 100%;" };
                                page.Write(htmlHelper.TextBox("dValue", propertyValue + "", dHtmlAttr));
                            }
                            else if (txtRead)
                            {
                                var dHtmlAttr = new { Class = "k-textbox", @readonly = "readonly", style = "width: 100%;" };
                                page.Write(htmlHelper.TextBox("dValue", propertyValue + "", dHtmlAttr));
                            }
                            else
                            {
                                if (propertyType.IsValueType)
                                {
                                    if (propertyValue.GetType() == typeof(int))
                                    {
                                        var value = 0;
                                        value = (int)propertyValue;

                                        //handle meta data here
                                        page.Write(htmlHelper.Kendo().IntegerTextBox()
                                            .Name(field.Name)
                                            .Value(value).Min(0)
                                            .HtmlAttributes(htmlAttr));
                                    }
                                    else
                                    {
                                        double value = 0;
                                        double.TryParse(propertyValue + "", out value);

                                        //handle meta data here
                                        page.Write(htmlHelper.Kendo().NumericTextBox()
                                            .Name(field.Name)
                                            .Value(value).Min(0)
                                            .HtmlAttributes(htmlAttr));
                                    }
                                }
                                else
                                {

                                    page.Write(htmlHelper.Kendo().TextBox()
                                        .Value(propertyValue + "")
                                        .Name(field.Name)
                                        .HtmlAttributes(htmlAttr));

                                }
                            }

                        }
                    }

                    page.WriteLiteral("<p class=\"errmsg\">");
                    page.Write(HtmlHelperValidationExtensions.ValidationMessage(htmlHelper, field.Name, "", new { @class = "text-danger" }));
                    page.WriteLiteral("</p>");

                    page.WriteLiteral($"</div>");

                    if (!allowMultipleCols)
                    {
                        page.WriteLiteral($"</div>");
                    }
                }
            }

            if (allowMultipleCols)
            {
                page.WriteLiteral($"</div>");
            }
        }

        public virtual void CreateTreeviewPageControls(IHtmlHelper<BasePageModel> htmlHelper, RazorPage page, bool readOnlyPage = false, bool istreeView = false)
        {
            bool transactionEntryPage = false;
            List<PageFieldModel> transactionFields = null;
            if (Model is TransactionEntryPageModel)
            {
                transactionFields = (Model as TransactionEntryPageModel).PageFields;

                AddControlsInSingleColumnForLessItems = false;
                transactionEntryPage = true;
            }

            //TODO: Load below data from Database
            string _rightName = PageName;
            var gridColumnIndexName = PageName;// "Color"; 

            //get the list of fields requires controls
            var entityInstanceType = Model.EntityInstance.GetType();
            var fieldLists = entityInstanceType.GetProperties();
            //var mIdentity = ModelMetadataIdentity.ForType(entityInstanceType);
            //var provider = new DataAnnotationsModelValidator;

            using (AMSContext db = AMSContext.CreateNewContext())
            {
                List<PropertyInfo> allowedFields = new List<PropertyInfo>();
                foreach (var field in fieldLists)
                {
                    var propertyType = field.PropertyType;
                    string fieldLabelTitle = field.Name;

                    //its a non null field
                    if (Nullable.GetUnderlyingType(field.PropertyType) != null)
                    {
                        propertyType = Nullable.GetUnderlyingType(field.PropertyType);
                    }

                    if ((propertyType != typeof(string)) && (propertyType.IsClass))
                        continue;
                    if (propertyType.IsGenericType) continue;
                    if (field.Name == PrimaryKeyFieldName) continue;
                    if (commonFields.Contains(field.Name)) continue;
                    if (ProductFields.Contains(field.Name)) continue;
                    if (field.Name.StartsWith("Attribute")) continue;

                    if (transactionFields != null)
                    {
                        if (transactionFields.Where(b => string.Compare(field.Name, b.FieldName, true) == 0).Any())
                            allowedFields.Add(field);
                    }
                    else
                    {
                        allowedFields.Add(field);
                    }
                }

                bool allowMultipleCols = allowedFields.Count > 5;
                if (AddControlsInSingleColumnForLessItems == false) allowMultipleCols = true;

                if (allowMultipleCols)
                {
                    page.WriteLiteral($"<div class=\"row\">");
                }

                page.WriteLiteral($"<div class=\"treeview_div\">");
                page.WriteLiteral($"<div class=\"nav-second-level\">");
                RenderControl(htmlHelper, page, db, readOnlyPage, entityInstanceType, null as PageFieldModel, null, null, 100, Model.PageFields, true);
                page.WriteLiteral($"</div>");
                page.WriteLiteral($"</div>");
                if (!istreeView)
                {
                    int fieldIndex = 0;
                    if (allowedFields.Count > 0)
                    {
                        FirstFieldName = allowedFields[0].Name;
                        foreach (var field in allowedFields)
                        {
                            var customAtts = field.CustomAttributes;

                            int stringMaxLength = 1000;
                            bool isRequired = false;

                            var propertyType = field.PropertyType;
                            string fieldLabelTitle = GetLabel(field.Name);

                            //its a non null field
                            if (Nullable.GetUnderlyingType(field.PropertyType) == null)
                            {
                                isRequired = true;
                            }
                            else
                            {
                                propertyType = Nullable.GetUnderlyingType(field.PropertyType);
                            }

                            if (propertyType == typeof(string))
                                isRequired = false;

                            foreach (var customAttribute in customAtts)
                            {
                                if (customAttribute.AttributeType == typeof(StringLengthAttribute))
                                {
                                    stringMaxLength = (int)customAttribute.ConstructorArguments[0].Value;
                                }

                                if (customAttribute.AttributeType == typeof(RequiredAttribute))
                                {
                                    isRequired = true;
                                }

                                if (customAttribute.AttributeType == typeof(DisplayNameAttribute))
                                {
                                    fieldLabelTitle = customAttribute.ConstructorArguments[0].Value + "";
                                }

                                if (customAttribute.AttributeType.Name == "NullableAttribute")
                                {
                                    isRequired = false;
                                }
                            }

                            if (!allowMultipleCols)
                            {
                                page.WriteLiteral($"<div class=\"row\">");
                            }

                            page.WriteLiteral($"<div class=\"col-xl-4\">");


                            page.WriteLiteral($"<label for=\"fullname\">");

                            page.Write(HtmlHelperLabelExtensions.Label(htmlHelper, Language.GetFieldName(fieldLabelTitle)));
                            if (isRequired)
                            {
                                page.WriteLiteral($"&nbsp;&nbsp;<span style=\"color: red\">*<span>");
                            }

                            page.WriteLiteral($"</label>");

                            bool controlRendered = false;
                            var propertyValue = field.GetValue(Model.EntityInstance);

                            var partialViewName = "";

                            if (string.IsNullOrEmpty(partialViewName) == false)
                            {
                                HtmlHelperPartialExtensions.RenderPartial(htmlHelper, partialViewName, propertyValue + "");
                                controlRendered = true;
                            }

                            //RenderControl(htmlHelper, page, db, readOnlyPage, entityInstanceType, field, propertyType, propertyValue, stringMaxLength, Model.PageFields);
                            throw new NotImplementedException();

                            page.WriteLiteral($"</div>");

                            if (!allowMultipleCols)
                            {
                                page.WriteLiteral($"</div>");
                            }

                            fieldIndex++;
                        }
                    }
                    if (allowMultipleCols)
                    {
                        page.WriteLiteral($"</div>");
                    }
                }

            }
        }

    

    }
}