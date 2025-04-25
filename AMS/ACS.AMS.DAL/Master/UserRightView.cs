using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ACS.AMS.DAL.Master.HiFi;
namespace ACS.AMS.DAL.DBModel
{
    public partial class UserRightView
    {
        public static IQueryable<UserRightView> GetAllRights(AMSContext db)
        {
            return db.UserRightView.AsQueryable();
        }
        public static IQueryable<User_UserRightView> GetAllRightValues(AMSContext db)
        {
            return db.User_UserRightView.AsQueryable();
        }
        public static IQueryable<User_RoleRightView> GetAllRoleRightValues(AMSContext db)
        {
            return db.User_RoleRightView.AsQueryable();
        }
		public static IList<Hi5UserPrivileges> GetUserPrivileges(AMSContext db, string username)
		{
			try
			{
				string upper = username;
				upper = upper.ToUpper();
				int num = (
					from b in db.User_LoginUserTable
					where b.UserName.ToUpper() == upper
					select b.UserID).FirstOrDefault<int>();

				var list = (
					from b in db.User_RoleRightTable
					where db.User_UserRoleTable.Where<User_UserRoleTable>((User_UserRoleTable ur) => ur.UserID == num).Select<User_UserRoleTable, int>((User_UserRoleTable ur) => ur.RoleID).Contains<int>(b.RoleID) && string.Compare(b.RightValue, "0") > 0
					select new RightModel {
						RightID=b.RightID,
						RightValue=int.Parse(b.RightValue)
					}
					).ToList();
				var list2 = (
					from b in db.User_UserRightTable
					where b.UserID == num && string.Compare(b.RightValue, "0") > 0
					select new RightModel { RightID= b.RightID, RightValue =int.Parse(b.RightValue) }).ToList();

				//var list3 = (from a in list
				//			 select new { a.RightID,a.RightValue})
    //                 .Union(from b in list2 select new { b.RightID,b.RightValue}).Select(new { c = }).ToList();


				var list3 = list.Union(list2);

				var list4 = (from a in db.User_RightTable
							 select a).ToList();

				List<Hi5UserPrivileges> User_RightTables = (
					from b in list4
					join c in list3 on b.RightID equals c.RightID
					select new Hi5UserPrivileges
					{
						RightName = b.RightName,
						ValueType = c.RightValue
					}).ToList();

				return User_RightTables;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
