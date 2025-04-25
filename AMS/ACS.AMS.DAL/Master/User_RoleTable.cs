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
    public partial class User_RoleTable:BaseEntityObject
    {
        public static IQueryable<User_RoleTable> GetAllRole(AMSContext db)
        {
            return db.User_RoleTable.Where(c=>c.IsDeleted==false).AsQueryable();
        }
        public static User_RoleTable GetRole(AMSContext db,string roleName)
        {
            var result = (from b in db.User_RoleTable where b.RoleName == roleName select b).FirstOrDefault();
            return result;
        }

        public static User_RoleTable GetRoleDetails(AMSContext db, int roleID)
        {
            var result = (from b in db.User_RoleTable where b.RoleID == roleID select b).FirstOrDefault();
            return result;
        }
        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            if (args.State == EntityObjectState.New)
            {
                var existingItem = (from b in args.NewDB.User_RoleTable
                                    where b.IsDeleted != true
                                    select new { RoleName = b.RoleName }).FirstOrDefault();
                if (existingItem != null)
                {
                    if (string.Compare(existingItem.RoleName, this.RoleName.Trim(), true) == 0)
                    {
                        args.FieldName = "RoleName"; //this is an parent table - so add ItemName_ infront of each fields
                        args.ErrorMessage = string.Format(Language.GetErrorMessage("RoleNameAlreadyExists"), Language.GetFieldName("RoleName", CultureHelper.EnglishCultureSymbol), RoleName);
                        return false;
                    }
                }

            }
            return true;
        }
        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            if (args.State == EntityObjectState.Edit)
            {
                var existingItem = (from b in args.NewDB.User_RoleTable
                                    where b.RoleID != this.RoleID && b.IsDeleted != true
                                    select new { RoleName = b.RoleName }).FirstOrDefault();
                if (existingItem != null)
                {
                    if (string.Compare(existingItem.RoleName, this.RoleName.Trim(), true) == 0)
                    {
                        args.FieldName = "RoleName"; //this is an parent table - so add ItemName_ infront of each fields
                        args.ErrorMessage = string.Format(Language.GetErrorMessage("RoleNameAlreadyExists"), Language.GetFieldName("RoleName", CultureHelper.EnglishCultureSymbol), RoleName);
                        return false;
                    }
                }

            }
            return true;
        }
        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            return true;
        }

        //internal override bool ValidateObject(ValidateEventArgs<AMSContext> args)
        //{
        //    if (args.State == EntityObjectState.New)
        //    {
        //        var existingItem = (from b in args.NewDB.User_RoleTable
        //                            where b.IsDeleted != true 
        //                            select new { RoleName = b.RoleName }).FirstOrDefault();
        //        if (existingItem != null)
        //        {
        //            if (string.Compare(existingItem.RoleName, this.RoleName.Trim(), true) == 0)
        //            {
        //                args.FieldName = "RoleName"; //this is an parent table - so add ItemName_ infront of each fields
        //                args.ErrorMessage = string.Format(Language.GetErrorMessage("RoleNameAlreadyExists"), Language.GetFieldName("RoleName", CultureHelper.EnglishCultureSymbol), RoleName);
        //                return false;
        //            }
        //        }

        //    }
        //    else
        //    {
        //        if (this.IsDeleted == true)
        //        {
                   

        //        }
        //        else
        //        {
        //            if (args.State == EntityObjectState.Edit)
        //            {
        //                var existingItem = (from b in args.NewDB.User_RoleTable
        //                                    where b.RoleID!=this.RoleID && b.IsDeleted != true
        //                                    select new { RoleName = b.RoleName }).FirstOrDefault();
        //                if (existingItem != null)
        //                {
        //                    if (string.Compare(existingItem.RoleName, this.RoleName.Trim(), true) == 0)
        //                    {
        //                        args.FieldName = "RoleName"; //this is an parent table - so add ItemName_ infront of each fields
        //                        args.ErrorMessage = string.Format(Language.GetErrorMessage("RoleNameAlreadyExists"), Language.GetFieldName("RoleName", CultureHelper.EnglishCultureSymbol), RoleName);
        //                        return false;
        //                    }
        //                }
                       
        //            }
        //        }
        //    }
        //    return true;
        //}
    }
}
