using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACS.AMS.DAL.Master.HiFi;
namespace ACS.AMS.DAL.DBModel
{
    public partial class User_MenuTable:BaseEntityObject
    {
		public IQueryable<User_MenuTable> GetAllUserMenu(AMSContext db, int userId, int applicationID)
		{
			return db.User_MenuTable.AsQueryable();
		}

		public static List<User_MenuTable> GetAllUserMenus(AMSContext db, string userName)
		{
			string upper = userName;
			upper = upper.ToUpper();
			int num = (
				from b in db.User_LoginUserTable
				where b.UserName.ToUpper() == upper
				select b.UserID).FirstOrDefault<int>();
			List<int> list = (
				from b in db.User_RoleRightTable
				where db.User_UserRoleTable.Where<User_UserRoleTable>((User_UserRoleTable ur) => ur.UserID == num).Select<User_UserRoleTable, int>((User_UserRoleTable ur) => ur.RoleID).Contains<int>(b.RoleID) && string.Compare(b.RightValue, "0") > 0
				select b.RightID).ToList<int>();
			List<int> nums = (
				from b in db.User_UserRightTable
				where b.UserID == num && string.Compare(b.RightValue, "0") > 0
				select b.RightID).ToList<int>();
			IEnumerable<int> nums1 = nums.Union<int>(list).Distinct<int>();
			List<User_MenuTable> User_MenuTables = (
				from b in db.User_MenuTable
				select b).ToList<User_MenuTable>();
			List<User_MenuTable> list1 = (
				from b in User_MenuTables
				where (!b.RightID.HasValue ? true : nums1.Contains<int>(b.RightID.Value))
				orderby b.MenuName
				select b).ToList<User_MenuTable>();

			return list1;
		}

		public static List<Hi5UserMenu> GetUserMenus(AMSContext db, string username)
		{
			db.EnableInstanceQueryLog = false;
			List<User_MenuTable> allUserMenus = User_MenuTable.GetAllUserMenus(db, username);
			allUserMenus = allUserMenus.Where(b => b.ParentTransactionID >= 1 || b.TargetObject == "HandlerNull()").ToList();
			
            IEnumerable<Hi5UserMenu> hi5UserMenus = allUserMenus.Where<User_MenuTable>((User_MenuTable b) => 
			{
				int? nullable;
				int? nullable1;
				int? parentMenuID = b.ParentMenuID;
				if (parentMenuID.HasValue)
				{
					nullable1 = new int?(parentMenuID.GetValueOrDefault());
				}
				else
				{
					nullable = null;
					nullable1 = nullable;
				}
				nullable = nullable1;
				return !nullable.HasValue;
			}).OrderBy<User_MenuTable, int>((User_MenuTable b) => b.OrderNo).Select<User_MenuTable, Hi5UserMenu>((User_MenuTable b) => new Hi5UserMenu(b.MenuName, "", b.TargetObject, b.OrderNo, b.ShortCutKeys, b.Image)
			{
				MenuID = b.MenuID
			});

			List<Hi5UserMenu> list = hi5UserMenus.ToList<Hi5UserMenu>();
			foreach (Hi5UserMenu hi5UserMenu in list)
			{
				AddChildMenus(hi5UserMenu, allUserMenus);
			}

			return list;
		}

		private static void AddChildMenus(Hi5UserMenu menuItem, List<User_MenuTable> allItems)
		{
			int num1 = 0;
			int.TryParse(string.Concat(menuItem.MenuID), out num1);
			IEnumerable<Hi5UserMenu> hi5UserMenus = allItems.Where<User_MenuTable>((User_MenuTable b) => {
				int? parentMenuID = b.ParentMenuID;
				int num = num1;
				return (parentMenuID.GetValueOrDefault() != num ? false : parentMenuID.HasValue);
			}).OrderBy<User_MenuTable, int>((User_MenuTable b) => b.OrderNo).Select<User_MenuTable, Hi5UserMenu>((User_MenuTable b) => new Hi5UserMenu(b.MenuName, "", b.TargetObject, b.OrderNo, b.ShortCutKeys, b.Image)
			{
				MenuID = b.MenuID
			});
			List<Hi5UserMenu> list = hi5UserMenus.ToList<Hi5UserMenu>();
			menuItem.ChildItems.AddRange(list);
			foreach (Hi5UserMenu hi5UserMenu in list)
			{
				AddChildMenus(hi5UserMenu, allItems);
			}
		}
	}
}
