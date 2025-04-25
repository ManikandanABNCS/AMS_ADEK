using ACS.AMS.WebApp;

using ACS.AMS.DAL.DBModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACS.AMS.DAL.Master.HiFi;
using ACS.AMS.DAL;
using ACS.AMS.DAL.DBContext;

namespace ACS.AMS.WebApp
{
    /// <summary>
    /// 
    /// </summary>
    public class SessionUser
    {
        public string Username { get; set; }
        public int UserID { get; set; }

        public string PersonCode { get; private set; }
        public string PersonFirstName { get; private set; }
        public string PersonLastName { get; private set; }

        public int userType { get; set; }
        public int PersonID { get; set; }
        public string Culture { get; set; }
        public int CompanyID { get;  set; }

        public string CompanyName { get; private set; }
        public int LanguageID { get; set; }
       
        public IList<Hi5UserPrivileges> UserPrivilege { get; private set; }

        [JsonConstructor]
        public SessionUser() { }

        public SessionUser(string userName, int userID, int personID, int UserType, string culture, int languageID,int companyID)
        {
            this.Username = userName;
            this.UserID = userID;
            this.PersonID = personID;
            this.userType = UserType;
            this.Culture = culture;
            this.LanguageID = languageID;
            this.CompanyID = companyID;
            string compName = string.Empty;
            if (companyID > 0)
            {
                var compTable = CompanyTable.GetItem(AMSContext.CreateNewContext(), companyID);
                if (compTable != null)
                {
                    compName = compTable.CompanyName;
                }
            }
            this.CompanyName = compName;
        }

        public static SessionUser CreateSessionUserObject(string userName, int userID, int personID, int UserType, string culture, int LanguageID,int companyID)
        {
            //_httpContextAccessor = httpContext;
            //CultureHelper.setHttpContextAccessor(_httpContextAccessor);
            //Language.setHttpContextAccessor(_httpContextAccessor);
            SessionUser c1 = new SessionUser(userName, userID, userID, UserType, culture, LanguageID, companyID);

            var person = PersonTable.GetItem(AMSContext.CreateNewContext(), userID);
            if(person != null)
            {
                c1.PersonCode = person.PersonCode;
                c1.PersonFirstName = person.PersonFirstName;
                c1.PersonLastName = person.PersonLastName;
            }

            CurrentHttpContext.Session.SetComplexData("userObject", c1);

            return c1;
        }

        public static HttpContext CurrentHttpContext
        {
            get
            {
                return AppHttpContext.Current;
            }
        }

        public static bool IsAuthenticated
        {
            get
            {
                if (CurrentHttpContext == null) return false;
                if (CurrentHttpContext.Session == null) return false;

                return CurrentHttpContext.Session.GetComplexData<SessionUser>("userObject") != null;
            }
        }

        public static SessionUser Current
        {
            get
            {
                if (CurrentHttpContext == null) return null;
                if (CurrentHttpContext.Session == null) return null;

                return CurrentHttpContext.Session.GetComplexData<SessionUser>("userObject");
            }
        }

        public int GetNextPageID()
        {
            AMSContext.CurrentPageID++;
            return AMSContext.CurrentPageID;
        }
        public static int GetWithoutSessionNextPageID()
        {
            AMSContext.CurrentPageID++;
            return AMSContext.CurrentPageID;
        }

        public static bool HasRights(string rightName, UserRightValue right)
        {
            return HasRights(GetRightValue(rightName), right);
        }

        public static bool HasRights(int rightValue, UserRightValue right)
        {
            int requiredVal = (int)right;

            return (rightValue & requiredVal) == requiredVal;
        }

        public static int GetRightValue(string rightName)
        {
            if (IsAuthenticated == false)
                throw new InvalidSessionException();

            var p = SessionUser.Current.GetUserPrivileges();

            if (!p.Any(C => C.RightName.Contains(rightName)))
                return 0;

            int rVal = 0;

            var data = p.Where(C => string.Compare(C.RightName, rightName, true) == 0).OrderByDescending(c => c.ValueType).Select(c => c.ValueType).FirstOrDefault();


            if (int.TryParse(data + "", out rVal))
            {
                return rVal;
            }

            return 0;
        }
        
        public IList<Hi5UserPrivileges> GetUserPrivileges()
        {
            //
            if (UserPrivilege == null)
            {
                UserPrivilege = UserRightView.GetUserPrivileges(AMSContext.CreateNewContext(), Username);
            }
            return UserPrivilege;
        }
    }
}