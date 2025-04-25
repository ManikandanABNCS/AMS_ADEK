using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.AMS.DAL
{
    public static class SessionUserHelper
    {
        public static ISessionUser CurrentSessionUser { get; set; }

        public static string UserName => CurrentSessionUser.UserName;

        public static int UserID => CurrentSessionUser.UserID;

        public static bool IsAuthenticated => CurrentSessionUser.IsAuthenticated;

        public static int LanguageID => CurrentSessionUser.LanguageID;   

        public static int CompanyID => CurrentSessionUser.CompanyID;
    }

    public interface ISessionUser
    {
        string UserName { get; }

        int UserID { get; }

        bool IsAuthenticated { get; }

        int LanguageID { get;  }
        int CompanyID { get; }
    }
}
