using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACS.AMS.DAL.DBContext;

namespace ACS.AMS.DAL.DBModel
{
    public partial class DynamicColumnRequiredEntityTable : BaseEntityObject
    {
        #region Static Methods
        /// <summary>
        /// This method Get all Application Pages name 
        /// </summary>
        /// <param name="entityObject"></param>
        /// <param name="checkStatus"></param>        
        /// <returns></returns>
        public static IQueryable<DynamicColumnRequiredEntityTable> GetAllDynamicColumnRequiredEntitys()
        {
            AMSContext db = AMSContext.CreateNewContext();
            return GetAllDynamicColumnRequiredEntitys(db);
        }
        public static IQueryable<DynamicColumnRequiredEntityTable> GetAllDynamicColumnRequiredEntitys(AMSContext db, bool includeInactiveItems = false)
        {
            return from b in db.DynamicColumnRequiredEntityTable
                   select b;
        }

        public static IQueryable<DynamicColumnRequiredEntityTable> GetAllPages(AMSContext db, bool checkStatus = false)
        {
            //var oldStrategy = PrepareDBFetchStrategy(db);
            IQueryable<DynamicColumnRequiredEntityTable> result = (from b in db.DynamicColumnRequiredEntityTable
                                                                   where b.DynamicColumnRequiredName != "Status" && b.DynamicColumnRequiredName != "Barcode" && b.DynamicColumnRequiredName != "ReceiptNumber"
                                                                   orderby b.DynamicColumnRequiredName
                                                                   select b);
            if (checkStatus)
                result = result.Where(a => a.isEnableExcel == true);
            //Based on License feature pages will load
            //AppLicenseManager key = new AppLicenseManager();
            //var license = key.GetAllColumns(db);
            //if (license != null)
            //{
            //    LicenseValidation validateKey = new LicenseValidation(license.LicenseCode);
            //    //if (!validateKey.EnableDepreciationModule())
            //    if (!validateKey.ValidateModule(LicenseFeaturesValue.DepreciationModule))
            //    {
            //        result = result.Where(a => !a.DynamicColumnRequiredName.Contains("Depreciation"));
            //    }
            //}
            //  db.FetchStrategy = oldStrategy;
            return result;
        }

        public static DynamicColumnRequiredEntityTable GetDynamicColumnsForEntity(AMSContext db, string tableName)
        {
            string dynamicColumnRequiredName=tableName.Replace("Table","");
            return (from b in db.DynamicColumnRequiredEntityTable
                    where b.EntityName == tableName && b.DynamicColumnRequiredName==dynamicColumnRequiredName
                    select b).FirstOrDefault();
        }
        #endregion


        /// <summary>
        /// validate the Application Name already exists or not
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        /// 
        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            return true;
        }
        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            return true;
        }
        //internal override bool ValidateObject(ValidateEventArgs<AMSContext> args)
        //{
        //    //validate the AreaCode already exists or not
        //    if (args.State == EntityObjectState.New)
        //    {
        //    }
        //    else
        //    {
        //        if (args.State == EntityObjectState.Edit)
        //        {
        //        }
        //    }
        //    return true;
        //}

        //public static IQueryable<ImportTemplateTypeTable> ExcelTempalteTypes(AMSContext db)
        //{
        //    var result = (from b in db.ImportTemplateTypeTables
        //                  select b);
        //    return result;
        //}

    }
}
