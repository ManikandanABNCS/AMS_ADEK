using System;
using System.Collections.Generic;
using System.Text;
using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Castle.Components.DictionaryAdapter.Xml;
using System.Runtime.CompilerServices;
using ACS.WebAppPageGenerator.Models.SystemModels;

namespace ACS.AMS.DAL
{
    public class BaseEntityObject
    {
        private Dictionary<string, AuditLogPropertyValue> changeAudit = new Dictionary<string, AuditLogPropertyValue>();

        [NotMapped]
        public int CurrentPageID { get; set; }

        [NotMapped]
        public static int CurrentUserLanguageID { get; set; }
        
        public static List<string> auditLogNotRequiredProperies = new List<string>()
        {
            "CreatedDateTime",
            "CreatedBy",
            "LastModifyBy",
            "LastModifiedBy",
            "LastModifiedDateTime",
            "LastModifiedDate",
            "LastModifiedDatetime"
        };

        public static List<string> auditLogNotRequiredTables = new List<string>()
            {
                "APPLICATIONERRORLOGTABLE"
            };

        public BaseEntityObject()
        {
            CurrentUserLanguageID = 1;
        }

        public static IQueryable<object> ConvertToGridData<T>(IEnumerable<T> data, string columnIndexName = "", string primaryKeyField = "")
        {
            string entityName = typeof(T).Name;
            if ((string.IsNullOrEmpty(columnIndexName)) || (string.IsNullOrEmpty(primaryKeyField)))
            {
                BaseEntityObject obj = Activator.CreateInstance<T>() as BaseEntityObject;
                if (obj == null)
                    return data.AsQueryable() as IQueryable<object>;

                if (string.IsNullOrEmpty(columnIndexName))
                {
                    columnIndexName = entityName.Replace("Table", "");
                }
                if (string.IsNullOrEmpty(primaryKeyField))
                {
                    primaryKeyField = obj.GetPrimaryKeyFieldName();
                }
            }

            var columnNames = UserGridNewColumnTable.GetUserColumns(columnIndexName).Where(b => b.MasterGridLineItem.FieldName != primaryKeyField);
            if (!string.IsNullOrEmpty(primaryKeyField))
                columnNames = columnNames.Where(b => b.MasterGridLineItem.FieldName.ToUpper() != primaryKeyField.ToUpper());

            var columnList = "new (" + primaryKeyField;
            foreach (var itm in columnNames)
            {
                columnList += ", " + itm.MasterGridLineItem.FieldName + " as " + itm.MasterGridLineItem.FieldName.Replace('.', '_');
            }

            //Remove the last , and add )
            columnList = columnList + ")";

            var newQry = data.AsQueryable().Select(columnList, "") as IQueryable<object>;
            return newQry;
        }

        public virtual string GetPrimaryKeyFieldName()
        {
            return this.GetType().Name.Replace("Table", "ID");
        }

        public virtual string GetEntityCodeFieldName()
        {
            var codeField = this.GetType().Name.Replace("Table", "Code");

            if (IsFieldAvailable(codeField))
                return codeField;

            return string.Empty;
        }

        public virtual string GetEntityNameFieldName()
        {
            var nameField = this.GetType().Name.Replace("Table", "Name");

            if (IsFieldAvailable(nameField))
                return nameField;

            return string.Empty;
        }

        public virtual object GetPrimaryKeyValue()
        {
            return GetFieldValue(GetPrimaryKeyFieldName());
        }

        protected virtual IQueryable GetAllItemsQuery(AMSContext db)
        {
            return ((IACSDBObject) this).GetAllItems(db);
        }

        internal virtual void BeforeSave(AMSContext db)
        {

        }

        //internal virtual bool ValidateObject(ValidateEventArgs<AMSContext> args)
        //{
        //    return true;
        //}
        internal virtual bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            return true;
        }
        
        internal virtual bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            return true;
        }   

        internal virtual bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            return true;
        }

        public virtual PageFieldModelCollection GetCreateScreenControls(string subpageName, int userID)
        { 
            return PageGenerationHelper.CreateDefaultPage(this.GetType(), subpageName, userID); 
        }

        public virtual bool GetEditScreenControls(string subpageName, int userID)
        { return false; }

        public virtual bool GetDetailsScreenControls(string subpageName, int userID)
        { return false; }

        public virtual bool ValidateUniqueKey(AMSContext db)
        {
            var qry = GetAllItemsQuery(db);
            var type = this.GetType();

            //Get All unique fields
            //var entry = db.Entry(type);

            if (Attribute.IsDefined(type, typeof(IndexAttribute)))
            {
                var itms = IndexAttribute.GetCustomAttributes(type, typeof(IndexAttribute));
                foreach (IndexAttribute itm in itms )
                {
                    if (itm.IsUnique)
                    {
                        if(string.Compare(itm.Name, "uc_CompanyAssetcode") ==0 || string.Compare(itm.Name, "uc_CompanyBarcode") == 0)
                        {
                            continue;
                        }
                        var val = GetFieldValue(itm.PropertyNames[0]);
                        var nQry = qry.Where($"{itm.PropertyNames[0]} == @0 AND {GetPrimaryKeyFieldName()} <> @1", val, GetPrimaryKeyValue());

                        if(nQry.Any())
                        {
                            throw new ValidationException($"{itm.PropertyNames[0]} '{val}' already exists");
                        }
                    }
                }
            }

            return true;
        }

        public virtual void UpdateUniqueKey(AMSContext db)
        {
            var type = this.GetType();

            //Get All unique fields
            //var entry = db.Entry(type);

            if (Attribute.IsDefined(type, typeof(IndexAttribute)))
            {
                var itms = IndexAttribute.GetCustomAttributes(type, typeof(IndexAttribute));
                foreach (IndexAttribute itm in itms)
                {
                    if (itm.IsUnique)
                    {
                        PropertyDescriptorCollection propCollection = TypeDescriptor.GetProperties(this.GetType());
                        PropertyDescriptor prop = propCollection[itm.PropertyNames[0]];

                        if (prop.PropertyType == typeof(string))
                        {
                            var val = GetFieldValue(itm.PropertyNames[0]);

                            SetFieldValue(itm.PropertyNames[0], val + "_" + GetPrimaryKeyValue());
                        }
                    }
                }
            }
        }

        public virtual void ValidateReferenceData(AMSContext db)
        {
            var type = this.GetType();
        }

        public bool IsFieldAvailable(string fieldName)
        {
            PropertyDescriptorCollection propCollection = TypeDescriptor.GetProperties(this.GetType());
            PropertyDescriptor prop = propCollection[fieldName];

            return prop != null;
        }

        public object GetFieldValue(string fieldName)
        {
            PropertyDescriptorCollection propCollection = TypeDescriptor.GetProperties(this.GetType());
            PropertyDescriptor prop = propCollection[fieldName];

            if (prop != null)
            {
                return prop.GetValue(this);
            }

            return null;
        }

        public void SetFieldValue(string fieldName, object value)
        {
            PropertyDescriptorCollection propCollection = TypeDescriptor.GetProperties(this.GetType());
            PropertyDescriptor prop = propCollection[fieldName];

            if (prop != null)
            {
                prop.SetValue(this, value);
            }
        }

        internal virtual void AuditChange(string propertyName, object oldValue, object newValue, string primaryKeyID)
        {
            if (oldValue == newValue) return;
            if ((oldValue != null) && (newValue != null) && (oldValue.Equals(newValue))) return;

            if (changeAudit.ContainsKey(propertyName))
            {
                if (changeAudit[propertyName].OldValue == newValue)
                    changeAudit.Remove(propertyName);
                else
                    changeAudit[propertyName].NewValue = newValue;
            }
            else
                changeAudit.Add(propertyName, new AuditLogPropertyValue(oldValue, newValue, primaryKeyID));
        }

        //public string GetPrimaryKeyValue(AMSContext db, BaseEntityObject tableName)
        //{
        //    IEnumerable<string> primaryKeys = GetKeyPropertyNames(db, tableName.GetType());
        //    int i = 1;
        //    foreach (var keys in primaryKeys)
        //    {
        //        if (i == 1)
        //        {
        //            i = i + 1;
        //            return keys;
        //        }
        //    }
        //    return string.Empty;
        //}
        //protected static IEnumerable<string> GetKeyPropertyNames(AMSContext db, Type type)
        //{
        //    var objectContext = ((IObjectContextAdapter)db).ObjectContext;

        //    return GetKeyPropertyNames(type, objectContext.MetadataWorkspace);
        //}

        //private static IEnumerable<string> GetKeyPropertyNames(Type type, MetadataWorkspace workspace)
        //{
        //    EdmType edmType;

        //    if (workspace.TryGetType(type.Name, type.Namespace, DataSpace.OSpace, out edmType))
        //    {
        //        return edmType.MetadataProperties.Where(mp => mp.Name == "KeyMembers")
        //            .SelectMany(mp => mp.Value as ReadOnlyMetadataCollection<EdmMember>)
        //            .OfType<EdmProperty>().Select(edmProperty => edmProperty.Name);
        //    }

        //    return null;
        //}
        //protected virtual EntityKeyMember[] GetPrimaryKeys(AMSContext db, EntityEntry<BaseEntityObject> trand)
        //{
        //    var objectStateEntry = ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.GetObjectStateEntry(trand.Entity);
        //    return objectStateEntry.EntityKey.EntityKeyValues;



        //    return null;
        //}
        
        internal virtual void CreateAuditLogs(AMSContext db, int actionType, AuditLogTransactionTable trans, EntityEntry<BaseEntityObject> transData)
        {
            //get the metadata of the class
            //if (!AuditLogRequiredAttribute.IsAuditLogRequiredForClass(this.GetType()))
            //    return;
            // IEnumerable<string> primaryKeys = GetKeyPropertyNames(db, this.GetType());

            AuditLogTable logTable = new AuditLogTable();
            logTable.ActionType = actionType;
            int i = 1;
            //foreach (var keys in primaryKeys)
            //{
            //    if (i == 1)
            //    {
            //        i = i + 1;
            //        var val = GetFieldValue(keys);
            //        if (val != null)
            //            logTable.Auditedobjectkeyvalue1 = val + "";
            //        else
            logTable.AuditedObjectKeyValue1 = (int)GetPrimaryKeyValue()+"";
            //}

            logTable.AuditedObjectKeyValue2 = null;
            //    if (1 == 2)
            //    {
            //        var val = GetFieldValue(keys);
            //        i = i + 1;
            //        if (val != null)
            //            logTable.Auditedobjectkeyvalue2 = val + "";
            //    }

            //}

            if (this.GetType() != null)
                logTable.EntityName = this.GetType().Name;
            else
                logTable.EntityName = this.GetType().Name;

            logTable.AuditLogTransaction = trans;

            if ((actionType == 1) || (actionType == 3))
            {
                trans.AuditLogTable.Add(logTable);
                logTable.AuditLogTransaction = trans;

                return;
            }

            bool foundLineItems = false;
            foreach (var property in changeAudit.Keys)
            {
                if (auditLogNotRequiredProperies.Contains(property))
                    continue;

                //if (!AuditLogRequiredAttribute.IsAuditLogRequiredForProperty(this.GetType(), property))
                //    continue;

                AuditLogLineItemTable lineItem = new AuditLogLineItemTable();
                string str = property;
                string realStr = "";
                bool previousIsCapital = false;
                foreach (char c in str)
                {
                    if ((char.IsUpper(c)) && (!previousIsCapital))
                    {
                        realStr += " ";
                    }

                    previousIsCapital = char.IsUpper(c);

                    realStr += c;
                }
                str = realStr.Trim();

                lineItem.FieldName = str;
                lineItem.NewValue = changeAudit[property].NewValue + "" == "null" ? string.Empty : changeAudit[property].NewValue + "";
                lineItem.OldValue = changeAudit[property].OldValue + "" == "null" ? string.Empty : changeAudit[property].OldValue + "";
                lineItem.AuditLog = logTable;
                db.AuditLogLineItemTable.Add(lineItem);
                foundLineItems = true;
            }

            if (foundLineItems)
            {
                trans.AuditLogTable.Add(logTable);
                logTable.AuditLogTransaction = trans;
                db.AuditLogTable.Add(logTable);

            }

        }

        //public virtual BaseEntityObject GetItem(AMSContext _db, int? id)
        //{
        //    return null;
        //}

        //public virtual BaseEntityObject GetItem(AMSContext _db, int id)
        //{
        //    return null;
        //}

        //public virtual IQueryable<BaseEntityObject> GetAllItems(AMSContext _db, bool includeInactiveItems = true)
        //{
        //    return null;
        //}

        protected static void TraceLog(string functionality, string message, [CallerMemberName] string method = null,
            [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = -1)
        {
            var sessionID = "<NO_SESSION>";
            //if (base.HttpContext.Session != null)
            //    sessionID = base.HttpContext.Session.Id;

            message = $"SID: {sessionID}, {message}";
            LogHelper.AddTraceLog(functionality, message, method, filePath, lineNumber);
        }

    }
}
