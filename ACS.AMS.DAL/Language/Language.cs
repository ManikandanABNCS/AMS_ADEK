using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using ACS.AMS.DAL;
using System.Collections;
using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace ACS.AMS.DAL
{

    /// <summary>
    /// 
    /// </summary>
    public static class Language
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        static IHttpContextAccessor _httpContextAccessor;
        static Language()
        {

        }
        public static void setHttpContextAccessor(IHttpContextAccessor HttpContextAccessor)
        {
            _httpContextAccessor = HttpContextAccessor;
        }
        /// <summary>
        /// 
        /// </summary>
        public static string CurrentCultureSymbol
        {
            get
            {
                return Thread.CurrentThread.CurrentUICulture.Name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<string, LanguageItem> Cache
        {
            get
            {
                return _httpContextAccessor.HttpContext.Session.GetComplexData<Dictionary<string, LanguageItem>>("__LanguageContent__");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetCustomerName()
        {
            return GetCustomerName(CurrentCultureSymbol);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cultureSymbol"></param>
        /// <returns></returns>
        public static string GetCustomerName(string cultureSymbol)
        {
            return GetString("CentralTendersCommittee", cultureSymbol);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetCopyrightString()
        {
            return GetCopyrightString(CurrentCultureSymbol);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cultureSymbol"></param>
        /// <returns></returns>
        public static string GetCopyrightString(string cultureSymbol)
        {

            return GetString("©2022.AMS All Rights Reserved.(Version 1.0.0.1)", cultureSymbol);
            //return GetString(AppConfigurationManager.GetValue<string>(AppConfigurationManager.PageFooterContent), cultureSymbol);
        }

        public static string GetLoginCopyrightString()
        {
            return GetLoginCopyrightString(CurrentCultureSymbol);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cultureSymbol"></param>
        /// <returns></returns>
        public static string GetLoginCopyrightString(string cultureSymbol)
        {

            return GetString("©AMS.Smartidfy All Rights Reserved.(Version 1.0.0.1)", cultureSymbol);
            //return GetString(AppConfigurationManager.GetValue<string>(AppConfigurationManager.LoginPageFooterContent), cultureSymbol);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetToolTipText(string key)
        {
            if (string.Compare(CurrentCultureSymbol, CultureHelper.ArabicCultureSymbol, true) == 0)
            {
                return GetToolTipText(key, Thread.CurrentThread.CurrentUICulture.Name)
                    + " - " + GetToolTipText(key, CultureHelper.EnglishCultureSymbol);
            }
            else
            {
                return GetToolTipText(key, CurrentCultureSymbol);
            }
        }

        public static string GetToolTipText(string key, string cultureSymbol)
        {
            return GetString(key + "_Tooltip", cultureSymbol);
        }

        public static string GetPageContent(string key)
        {
            return GetString(key + "_PageContent", CurrentCultureSymbol);
        }

        public static string GetPageContent(string key, string cultureSymbol)
        {
            return GetString(key + "_PageContent", cultureSymbol);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetPageTitle(string key)
        {
            return GetString(key);// + "_PageTitle");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetErrorMessage(string key)
        {
            return GetString(key, LanguageContentType.MessageTexts);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetFieldName(string key)
        {
            return GetString(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cultureSymbol"></param>
        /// <returns></returns>
        public static string GetFieldName(string key, string cultureSymbol)
        {
            return GetString(key, cultureSymbol);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetEntityName(string key)
        {
            return GetString(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetButtonText(string key)
        {
            return GetString(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
       
        public static string GetString(string key)
        {
            return GetString(key, CurrentCultureSymbol, LanguageContentType.Grammer1);
            //return GetString(CurrentCultureSymbol, CurrentCultureSymbol, LanguageContentType.Grammer1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetString(string key, LanguageContentType type)
        {
            return GetString(key, CurrentCultureSymbol, type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string GetString(string key, string cultureSymbol)
        {
            return GetString(key, cultureSymbol, LanguageContentType.Grammer1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        /// 

        private static Dictionary<string, int> languageIDs = new Dictionary<string, int>();

        // have to remove command after crearting LanguageContent  in Master Folder
        public static string GetString(string key, string cultureSymbol, LanguageContentType type)
        {
            return ToLabelString(key);
            //try
            //{
            //    //ClearContents();
            //    //UnComment After Adding Language Table
            //    int languageID = 0;
            //    if (!string.IsNullOrEmpty(cultureSymbol))
            //    {
            //        if (languageIDs.ContainsKey(cultureSymbol))
            //            languageID = languageIDs[cultureSymbol];
            //        else
            //        {
            //            languageID = (from b in AMSContext.CreateNewContext().LanguageTable
            //                          where b.CultureSymbol == cultureSymbol
            //                          select b.LanguageID).FirstOrDefault();

            //            languageIDs.Add(cultureSymbol, languageID);
            //        }
            //    }

            //    //get the dictionary from cache
            //    Dictionary<string, LanguageItem> itm = _httpContextAccessor.HttpContext.Session.GetComplexData<Dictionary<string, LanguageItem>>("__LanguageContent__");//Cache["__LanguageContent__"] as Dictionary<string, LanguageItem>;
            //    AMSContext db = AMSContext.CreateNewContext();
            //    if (itm == null || itm.Count()==0)
            //    {
            //        //create the dictionary & add it into cache
            //        itm = new Dictionary<string, LanguageItem>(new CaseInsensitiveComparer());
            //        _httpContextAccessor.HttpContext.Session.SetComplexData("__LanguageContent__", itm);

            //        // Cache.Add("__LanguageContent__", itm, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.High, null);

            //        //load all data from DB
            //        var qry = LanguageContentTable.GetAllLanguageContentTable(db);

            //        foreach (var d in qry)
            //        {
            //            if (!itm.ContainsKey(d.MessageIdentification))
            //            {
            //                itm.Add(d.MessageIdentification, new LanguageItem(d));
            //            }
            //        }
            //    }

            //    LanguageItem languageItem;
            //    if (!itm.ContainsKey(key))
            //    {
            //        LanguageContentTable languageContent = null;

            //        lock (itm)
            //        {
            //            //key not found so add it into the DB
            //            // AMSContext db = AMSContext.CreateNewContext();
            //            string content = key;

            //            if (content.EndsWith("_PageTitle"))
            //                content = content.Replace("_PageTitle", "");

            //            if (content.EndsWith("_Error"))
            //                content = content.Replace("_Error", "");

            //            if (content.EndsWith("_Tooltip"))
            //                content = content.Replace("_Tooltip", "");

            //            if (content.EndsWith("_PageContent"))
            //                content = content.Replace("_PageContent", "");

            //            // content = content.MakeLabelText();
            //            content = content.Replace("_ ", "_");
            //            content = content.Replace("Coupon", "Voucher");
            //            content = content.Replace("coupon", "voucher");

            //            bool previousIsCapital = false;
            //            string realStr = "";
            //            foreach (char c in content)
            //            {
            //                if ((char.IsUpper(c)) && (!previousIsCapital))
            //                {
            //                    realStr += " ";
            //                }

            //                previousIsCapital = char.IsUpper(c);

            //                realStr += c;
            //            }
            //            content = realStr.Trim();

            //            LanguageContentTable lc = new LanguageContentTable();
            //            lc.MessageIdentification = key;
            //            lc.IsNewItem = true;
            //            lc.StatusID = (int)StatusValue.Active;
            //            lc.ContentType = (int)type;

            //            db.Add(lc);

            //            foreach (var language in LanguageTable.GetAllLanguageTable(db))
            //            {
            //                LanguageContentLineItemTable lclineitem = new LanguageContentLineItemTable();
            //                lclineitem.LanguageID = language.LanguageID;
            //                lclineitem.LanguageContentNavigation = lc;
            //                lclineitem.LanguageContent = content;

            //                db.Add(lclineitem);
            //            }

            //            db.SaveChanges();
            //            languageContent = lc;

            //            languageItem = new LanguageItem(languageContent);
            //            itm.Add(key, languageItem);
            //        }
            //    }
            //    else
            //    {
            //        languageItem = itm[key];
            //    }

            //    if (languageItem != null)
            //    {
            //        if (languageID > 0)
            //            return languageItem.LineItems[languageID];
            //        else
            //            return languageItem.LineItems[CultureHelper.CurrentLanguageID];
            //    }

            //    return key;
            //}
            //catch (Exception ex)
            //{
            //    AppErrorLogTable.SaveException(ex);
            //    throw ex;
            //}
        }

        public static void ClearContents()
        {
            Cache.Remove("__LanguageContent__");
        }
        private static string ToLabelString(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;

            string newStr = "" + str[0];
            bool prevIsBold = false;
            if (char.IsUpper(str[0]))
                prevIsBold = true;

            for (int i = 1; i < str.Length; i++)
            {
                if (char.IsUpper(str[i]))
                {
                    if (prevIsBold == false)
                    {
                        newStr += " ";
                    }

                    prevIsBold = true;
                }
                else
                {
                    prevIsBold = false;
                }

                newStr += str[i];
            }

            return newStr;
        }
    }

    ///
    public class CaseInsensitiveComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            bool res = string.Compare(x, y, true) == 0;
            return res;
        }

        public int GetHashCode(string obj)
        {
            return obj.ToString().ToLower().GetHashCode();
        }
    }
}
