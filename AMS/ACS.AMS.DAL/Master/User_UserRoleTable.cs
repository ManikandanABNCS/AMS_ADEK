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
    public partial class User_UserRoleTable:BaseEntityObject
    {
        public static IQueryable<User_UserRoleTable> GetAllUserRole(AMSContext db)
        {
            return db.User_UserRoleTable.AsQueryable();
        }

        public static User_UserRoleTable GetUserRole(AMSContext db, int roleID,int userID)
        {
            var result = (from b in db.User_UserRoleTable where b.RoleID==roleID && b.UserID==userID select b).FirstOrDefault();
            return result;
        }

        public static void deleteUserRole(AMSContext db, int userID)
        {
            var result = (from b in db.User_UserRoleTable
                          where b.UserID == userID
                          select b);
            if (result.Count() > 0)
            {
                foreach (var res in result)
                {
                    db.User_UserRoleTable.Remove(res);

                }
                db.SaveChanges();
                // db.Dispose();

            }
        }
        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            return true;
        }
    }
}
