using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACS.AMS.DAL.DBModel
{
    public partial class BarcodeConstructTable : BaseEntityObject

    {
      
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
        //    return true;
        //}
        public static BarcodeConstructSequenceTable CompareData(AMSContext db, string categoryCode = null, string locCode = null,
                               string depCode = null, string secCode = null, string customPrefix = null)
        {
            var result = (from b in db.BarcodeConstructSequenceTable
                              // where b.CategoryCode == categoryCode && b.Locationcode == locCode
                          select b);
            if (!string.IsNullOrEmpty(categoryCode))
                result = result.Where(a => a.CategoryCode == categoryCode);
            else
                result = result.Where(a => a.CategoryCode == string.Empty);

            if (!string.IsNullOrEmpty(locCode))
                result = result.Where(a => a.Locationcode == locCode);
            else
                result = result.Where(a => a.Locationcode == string.Empty);

            if (!string.IsNullOrEmpty(depCode))
                result = result.Where(a => a.DepartmentCode == depCode);
            else
                result = result.Where(a => a.DepartmentCode == string.Empty);

            if (!string.IsNullOrEmpty(secCode))
                result = result.Where(a => a.SectionCode == secCode);
            else
                result = result.Where(a => a.SectionCode == string.Empty);
            if (!string.IsNullOrEmpty(customPrefix))
                result = result.Where(a => a.CustomCode == customPrefix);
            else
                result = result.Where(a => a.CustomCode == string.Empty);

            return result.FirstOrDefault();
        }
        public static bool checkPrefixData(AMSContext db, string categoryCode = null, string locCode = null, string depCode = null,
                                    string secCode = null, string customPrefixLength = null, string customPrefix = null)
        {
            var result = (from b in db.BarcodeConstructTable
                          select b);
            if (!string.IsNullOrEmpty(categoryCode))
                result = result.Where(a => a.CategoryCode == categoryCode);
            else
                result = result.Where(a => a.CategoryCode == null);

            if (!string.IsNullOrEmpty(locCode))
                result = result.Where(a => a.LocationCode == locCode);
            else
                result = result.Where(a => a.LocationCode == null);

            if (!string.IsNullOrEmpty(depCode))
                result = result.Where(a => a.DepartmentCode == depCode);
            else
                result = result.Where(a => a.DepartmentCode == null);

            if (!string.IsNullOrEmpty(secCode))
                result = result.Where(a => a.SectionCode == secCode);
            else
                result = result.Where(a => a.SectionCode == null);
            if (!string.IsNullOrEmpty(customPrefixLength))
            {
                int Length = int.Parse(customPrefixLength);
                result = result.Where(a => a.CustomPrefixLength == Length);
            }
            else
                result = result.Where(a => a.CustomPrefixLength == null);
            if (!string.IsNullOrEmpty(customPrefix))
                result = result.Where(a => a.CustomPrefix == customPrefix);
            else
                result = result.Where(a => a.CustomPrefix == null);
            if (result.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
