using ACS.AMS.DAL.DBContext;
using ACS.AMS.DAL.DBModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.AMS.DAL
{
    public static class EntityHelper
    {
        /// <summary>
        /// Gets the core entity object type for the requested page. Additional objects types are not considered here
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="indexScreen"></param>
        /// <returns></returns>
        public static Type GetEntityObjectType(string entityName,bool indexScreen=false)
        {
            if(entityName.Contains(':'))
            {
                int index = entityName.IndexOf(':');
                entityName = entityName.Substring(0, index);
            }

            if (indexScreen)
            {
                switch (entityName)
                {
                    case "Asset": entityName = "AssetNewView"; break;
                    case "MasterGridLineItem": entityName = "MasterGridNewLineItemTable"; break;
                    case "SearchByVirtualBarcode": entityName = "ProductTable"; break;
                    case "VirtualBarcodeMappedAsset": entityName = "AssetNewView"; break;
                }
            }
            else
            {
                switch (entityName)
                {                   
                    case "MasterGridLineItem": entityName = "MasterGridNewLineItemTable"; break;
                    case "Supplier": entityName = "PartyTable"; break;
                    case "Custodian": entityName = "User_LoginUserTable"; break;
                    case "SearchByVirtualBarcode": entityName = "ProductTable"; break;
                    case "VirtualBarcodeMappedAsset": entityName = "AssetNewView"; break;
                }
            }

            Type t2 = typeof(App_Applications);

            var objName = $"{t2.Namespace}.{entityName}Table";

            if (entityName.ToUpper().EndsWith("TABLE")|| entityName.ToUpper().EndsWith("VIEW"))
            {
                objName = $"{t2.Namespace}.{entityName}";
            }

            Type t = typeof(BaseEntityObject).Assembly.GetTypes().Where((t) => t.FullName == objName).FirstOrDefault();

            return t;
        }

        public static bool HasLanguageTable(string entityName)
        {
            return HasLanguageTable(GetEntityObjectType(entityName));
        }

        public static bool HasLanguageTable(Type type)
        {
            var tableName = type.Name;
            if (tableName.ToUpper().EndsWith("TABLE"))
            {
                var entityName = tableName.Substring(0, tableName.Length - 5);

                var langTableName = $"{entityName}DescriptionTable";
                var langEntityType = GetEntityObjectType(langTableName);

                return langEntityType != null;
            }

            return false;
        }

        public static Type GetLanguageTable(string entityName)
        {
            return GetLanguageTable(GetEntityObjectType(entityName));
        }

        public static Type GetLanguageTable(Type type)
        {
            var tableName = type.Name;

            if (tableName.ToUpper().EndsWith("TABLE"))
            {
                var entityName = tableName.Substring(0, tableName.Length - 5);

                var langTableName = $"{entityName}DescriptionTable";
                var langEntityType = GetEntityObjectType(langTableName);

                return langEntityType;
            }

            return null;
        }
          
        public static SelectList GetSelectList(Type type, AMSContext db, int userID)
        {
            var obj = Activator.CreateInstance(type) as IACSDBObject;
            var baseEntity = obj as BaseEntityObject;

            var allItems = obj.GetAllUserItems(db, userID);

            string valueField = baseEntity.GetPrimaryKeyFieldName();
            string displayField = type.GetProperties()[1].Name;

            string selectQuery = $"new ({valueField}, {displayField} as DisplayField)";
            string nameField = baseEntity.GetEntityNameFieldName();
            string orderBy = displayField;

            if (!string.IsNullOrEmpty(nameField))
            {
                selectQuery = $"new ({valueField}, {nameField} + \" (\" + {displayField} + \")\" as DisplayField)";
                orderBy = nameField;
            }

            var selectedValues = allItems.OrderBy(orderBy).Select(selectQuery, "");

            return new SelectList(selectedValues, valueField, "DisplayField");
        }
    }
}