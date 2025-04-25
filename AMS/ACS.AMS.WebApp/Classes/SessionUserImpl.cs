using ACS.AMS.DAL;

using ACS.AMS.DAL.DBModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACS.AMS.DAL.Master.HiFi;

namespace ACS.AMS.WebApp
{
    /// <summary>
    /// 
    /// </summary>
    public class SessionUserImpl : ISessionUser
    {
        public string UserName => SessionUser.Current.Username;

        public int UserID => SessionUser.Current.UserID;

        public bool IsAuthenticated => (SessionUser.IsAuthenticated);

        public int LanguageID=>SessionUser.Current.LanguageID;

        public int CompanyID=>SessionUser.Current.CompanyID;
    }
}