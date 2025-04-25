using ACS.AMS.DAL;
using ACS.AMS.DAL.DBContext;
using ACS.AMS.DAL.DBModel;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using Org.BouncyCastle.Tls;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace ACS.AMS.WebApp.Classes
{
    public static class SystemDatabaseHelper
    {
        private static AMSContext GetDBContext()
        {
            return AMSContext.CreateNewContext();
        }

        public static void GenerateMasterGridColumns(Type dbEntityType, string gridName)
        {
            //Generate the columns 
            using (var db = GetDBContext())
            {
                var found = from b in db.MasterGridNewTable where b.MasterGridName == gridName select b;
                if (!found.Any())
                {
                    Type newEntityType = null;// EntityHelper.GetLanguageTable(dbEntityType);
                    //Table contains Language based description table
                    if (newEntityType == null)
                    {
                        newEntityType = dbEntityType;
                    }

                    //create master grid data
                    MasterGridNewTable MasterGridNewTable = new MasterGridNewTable()
                    {
                        MasterGridName = gridName,
                        EntityName = newEntityType.Name,
                    };

                    db.Add(MasterGridNewTable);
                    db.SaveChanges();
                }

                UpdateMasterGridColumns(dbEntityType, gridName);
            }
        }

        private static void UpdateMasterGridColumns(Type dbEntityType, string gridName)
        {
            //Generate the columns 
            using (var db = GetDBContext())
            {
                var found = from b in db.MasterGridNewTable where b.MasterGridName == gridName select b;
                if (found.Any() == false)
                {
                    GenerateMasterGridColumns(dbEntityType, gridName);

                    return;
                }

                //create master grid data
                MasterGridNewTable MasterGridNewTable = found.FirstOrDefault();

                //Create master grid line items
                var properties = dbEntityType.GetProperties();
                int cnt = 0;
                var lastOrderID = (from b in db.MasterGridNewLineItemTable where b.MasterGridID == MasterGridNewTable.MasterGridID select b.OrderID);
                if (lastOrderID.Any())
                    cnt = lastOrderID.Max();
                var entityName = MasterGridNewTable.EntityName;
                //if (!entityName.EndsWith("View") && !entityName.Contains("Location") && !entityName.Contains("Category") )
                //{ 
                //load the ForeignKey values
                List<string> foreignKeys = new List<string>();
                foreach (var property in properties)
                {
                    if (Attribute.IsDefined(property, typeof(ForeignKeyAttribute)))
                    {
                        ForeignKeyAttribute fKey = Attribute.GetCustomAttributes(property, typeof(ForeignKeyAttribute)).FirstOrDefault() as ForeignKeyAttribute;
                        if (fKey != null)
                        {
                            //Ignore created by column, so system will add username
                            if (string.Compare(fKey.Name, "CreatedBy", true) == 0) continue;
                            if (string.Compare(fKey.Name, "StatusID", true) == 0) continue;

                            foreignKeys.Add(fKey.Name);
                        }
                    }
                }

                string parentTableName = "";
                //var languageEntityType = EntityHelper.GetLanguageTable(dbEntityType);
                ////Table contains Language based description table
                //if (languageEntityType == null)
                //{
                //    languageEntityType = dbEntityType;
                //}
                //else
                //{
                //    //fetch the parent table (Remove table suffix)
                //    parentTableName = dbEntityType.Name;
                //    parentTableName = parentTableName.Substring(0, parentTableName.Length - 5);

                //    var propertyname = languageEntityType.Name;
                //    propertyname = propertyname.Substring(0, propertyname.Length - 5);
                //    var descProperty = languageEntityType.GetProperty(propertyname);
                //    if(descProperty != null) 
                //        CreateLineItem(db, "", descProperty, MasterGridNewTable.MasterGridID, ++cnt);
                //}

                foreach (var property in properties)
                {
                    if (foreignKeys.Contains(property.Name)) continue;

                    if (Attribute.IsDefined(property, typeof(NotMappedAttribute))) continue;
                    if (Attribute.IsDefined(property, typeof(KeyAttribute))) continue;

                    //Add the foreign key field tables
                    if ((Attribute.IsDefined(property, typeof(InversePropertyAttribute))) &&
                        (Attribute.IsDefined(property, typeof(ForeignKeyAttribute))))
                    {
                        //add the Code & Name properties
                        var pType = property.PropertyType.GetProperties();
                        var tableName = property.Name;
                        if (tableName.ToUpper().EndsWith("TABLE"))
                        {
                            tableName = tableName.Substring(0, tableName.Length - 5);
                        }

                        string codeField = $"{tableName}Code";
                        string nameField = $"{tableName}Name";

                        string parentEntityName = property.Name;
                        if (string.IsNullOrEmpty(parentTableName) == false)
                            parentEntityName = parentTableName + "." + parentEntityName;

                        var codeProperty = pType.Where(b => string.Compare(b.Name, codeField, true) == 0).FirstOrDefault();
                        if (codeProperty != null)
                        {
                            CreateLineItem(db, parentEntityName, codeProperty, MasterGridNewTable.MasterGridID, ++cnt);
                        }

                        var nameProperty = pType.Where(b => string.Compare(b.Name, nameField, true) == 0).FirstOrDefault();
                        if (nameProperty != null)
                        {
                            CreateLineItem(db, parentEntityName, nameProperty, MasterGridNewTable.MasterGridID, ++cnt);
                        }

                        continue;
                    }

                    if (Attribute.IsDefined(property, typeof(InversePropertyAttribute))) continue;
                    if (Attribute.IsDefined(property, typeof(ForeignKeyAttribute))) continue;

                    CreateLineItem(db, parentTableName, property, MasterGridNewTable.MasterGridID, ++cnt);
                }

                db.SaveChanges();
            //}
            
            }
        }

        private static void CreateLineItem(AMSContext db, string parentTableName, PropertyInfo property, int masterGridID, int cnt)
        {
            string parentReferenceName = "";
            if (string.IsNullOrEmpty(parentTableName) == false)
            {
                parentReferenceName = parentTableName + ".";
            }

            MasterGridNewLineItemTable lineItemTable = new MasterGridNewLineItemTable()
            {
                FieldName = parentReferenceName + property.Name,
                DisplayName = property.Name,
                IsDefault = !property.Name.StartsWith("Attribute") && cnt < 8,
                Width = 10,
                OrderID = cnt,
                ColumnType = property.PropertyType.Name
            };

            var propertyType = property.PropertyType;
            propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? propertyType;

            var columnTypeStr = propertyType.ToString();

            if (property.PropertyType == typeof(DateTime))
            {
                lineItemTable.Format = CultureHelper.DateTimeFormat;
            }

            switch (property.Name.ToUpper())
            {
                case "LASTMODIFYBY":
                case "LASTMODIFIEDDATE":
                case "LASTMODIFIEDDATETIME":
                    return;

                case "STATUSID":
                    lineItemTable.FieldName = parentReferenceName + "Status.Status";
                    lineItemTable.DisplayName = "Status";
                    columnTypeStr = "System.String";
                    break;

                case "CREATEDBY":
                    lineItemTable.FieldName = parentReferenceName + "CreatedByNavigation.UserName";
                    lineItemTable.DisplayName = "CreatedBy";
                    columnTypeStr = "System.String";
                    break;

                case "CREATEDDATETIME":
                case "CREATEDDATE":
                    lineItemTable.DisplayName = "CreatedOn";
                    break;
            }

            //avoid newly modified field names
            var fieldFound = (from b in db.MasterGridNewLineItemTable
                              where b.MasterGridID == masterGridID
                                 && b.FieldName == lineItemTable.FieldName
                              select b).FirstOrDefault();

            if (fieldFound != null)
            {
                //if (string.IsNullOrEmpty(fieldFound.ColumnType))
                fieldFound.ColumnType = columnTypeStr;
                //fieldFound.ForeignKeyDataServiceName = parentTableName;

                return;
            }

            //lineItemTable.ForeignKeyDataServiceName = parentTableName;
            lineItemTable.ColumnType = columnTypeStr;
            lineItemTable.MasterGridID = masterGridID;
            db.Add(lineItemTable);
        }
    }
}