using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ACS.AMS.DAL;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
namespace ACS.AMS.DAL.DBModel
{
   public partial class UserTypeTable
    {
        public static IQueryable<UserTypeTable> GetAllUserType(AMSContext db)
        {
            return db.UserTypeTable.AsQueryable();
        }
    }
}
