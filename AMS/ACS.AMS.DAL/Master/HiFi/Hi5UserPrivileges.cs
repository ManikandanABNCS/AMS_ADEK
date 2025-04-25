using ACS.AMS.DAL.DBContext;
using ACS.AMS.DAL.DBModel;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ACS.AMS.DAL.Master.HiFi
{
    public class Hi5UserPrivileges 
    {
        public bool IsDirty
        {
            get;
            set;
        }
		public string RightName
		{
			get;
			set;
		}
		public int ValueType
		{
			get;
			set;
		}
		public Hi5UserPrivileges()
        {
        }

       

    }

    public class RightModel
    {
        public int RightID { get; set; }
        public int RightValue { get; set; }

    }
}
