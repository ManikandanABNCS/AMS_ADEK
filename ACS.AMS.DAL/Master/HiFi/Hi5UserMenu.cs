using ACS.AMS.DAL.DBContext;
using ACS.AMS.DAL.DBModel;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ACS.AMS.DAL.Master.HiFi
{
   public  class Hi5UserMenu
    {
		public List<Hi5UserMenu> ChildItems { get; set; }

		public string Image { get; set; }

		public object MenuID { get; set; }

		public string MenuName
		{
			get;
			set;
		}

		public int ModuleID
		{
			get;
			set;
		}

		public int OrderNo
		{
			get;
			set;
		}

		public string RightName
		{
			get;
			set;
		}

		public string ShortcutKeys
		{
			get;
			set;
		}

		public string TargetObject
		{
			get;
			set;
		}

		public Hi5UserMenu()
		{
			this.ChildItems = new List<Hi5UserMenu>();
		}

		public Hi5UserMenu(string menuName, string rightName, string targetObject, int orderNo, string shortcutKeys, string image) : this(menuName, rightName, targetObject, orderNo, shortcutKeys, image, 0)
		{
		}

		public Hi5UserMenu(string menuName, string rightName, string targetObject, int orderNo, string shortcutKeys, string image, int moduleID) : this()
		{
			this.MenuName = menuName;
			this.RightName = rightName;
			this.TargetObject = targetObject;
			this.OrderNo = orderNo;
			this.ShortcutKeys = shortcutKeys;
			this.Image = image;
			this.ModuleID = moduleID;
		}

	}
}
