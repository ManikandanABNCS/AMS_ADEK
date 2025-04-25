using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ACS.AMS.DAL;

namespace ACS.AMS.DAL.DBModel
{
    public partial class ConfigurationTable:BaseEntityObject
    {

        public static IQueryable<ConfigurationTable> GetAllConfiguration(AMSContext db)
        {
            return db.ConfigurationTable.AsQueryable();
        }

        public static string GetConfigurationValue(AMSContext db, string configName)
        {
            var details = (from b in db.ConfigurationTable where b.CategoryName == configName select b.ConfiguarationValue).FirstOrDefault();
            return details;
        }

        public static List<ConfigurationCategoryNameModel> GetCategoryName(AMSContext db)
        {
            var list = (from b in db.ConfigurationTable select new ConfigurationCategoryNameModel { CategoryName = b.CategoryName}
                                                ).Distinct().ToList();
            return list;


        }

    }
}
