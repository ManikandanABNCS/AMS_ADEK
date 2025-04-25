using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel
{
    public partial class UserCompanyMappingTable : BaseEntityObject
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
        
        public static IQueryable<UserCompanyMappingTable> GetCompanyForPersonID(AMSContext db, int personID)
        {
            return (from b in db.UserCompanyMappingTable
                    where b.UserID == personID && b.StatusID == (int)StatusValue.Active
                    select b);
        }
        
        public static IQueryable<UserDepartmentMappingTable> GetDepartmentForPersonID(AMSContext db, int personID)
        {
            return (from b in db.UserDepartmentMappingTable
                    where b.PersonID == personID && b.StatusID == (int)StatusValue.Active
                    select b);
        }

        public static IQueryable<UserCategoryMappingTable> GetCategoryForPersonID(AMSContext db, int personID)
        {
            return (from b in db.UserCategoryMappingTable
                    where b.PersonID == personID && b.StatusID == (int)StatusValue.Active
                    select b);
        }
        
        public static IQueryable<UserLocationMappingTable> GetLocationForPersonID(AMSContext db, int personID)
        {
            return (from b in db.UserLocationMappingTable
                    where b.PersonID == personID && b.StatusID == (int)StatusValue.Active
                    select b);
        }
    }
}

