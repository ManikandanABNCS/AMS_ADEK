
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Globalization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
namespace ACS.AMS.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class CultureHelper
    {
        protected static IHttpContextAccessor _httpContextAccessor;
        private const string arabicCurrency = "QR";
        public const string ArabicCultureSymbol = "ar-QA";
        public const string EnglishCultureSymbol = "en-GB";

        public const string DefaultTheme = "DefaultTheme";
        public const string GreenTheme = "GreenTheme";
        public const string OrangeTheme = "OrangeTheme";
        public static int CurrentLanguageID = 1;

        //public  string sysFormat = CultureInfo.CurrentUICulture.DateTimeFormat.ToString();
        /// <summary>
        /// 
        /// </summary>
        [JsonConstructor]
        public CultureHelper()
        {

        }
        public static void setHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsEnglish
        {
            get
            {
                return CurrentCulture.Name != ArabicCultureSymbol;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static CultureInfo CurrentCulture
        {
            get
            {
                return Thread.CurrentThread.CurrentUICulture;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetCurrencyFormatString()
        {
            return GetCurrencySymbol() + "{0}";
        }

        /// <summary>
        /// 
        /// </summary>
        public static string DateFormat
        {
            get
            {
                return "dd/MM/yyyy";
            }
        }

        public static string DateFormatForMonth
        {
            get
            {
                return "MM/dd/yyyy";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string DateTimeFormat
        {
            get
            {
                return "dd/MM/yyyy hh:mm:ss";
            }
        }


        public static string DateTimeFormatForGrid
        {
            get
            {
                return "dd/MM/yyyy HH:mm:ss ";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string USDateFormat
        {
            get
            {
                return "dd/MM/yyyy";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string USDateTimeFormat
        {
            get
            {
                return "MM/dd/yyyy HH:mm:ss tt";

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string US12HDateTimeFormat
        {
            get
            {
                return "dd/MM/yyyy hh:mm:s tt";
            }
        }

        public static string TimeFormat
        {
            get
            {
                return "HH:mm:ss";
            }
        }

        //public static string SystemFormat
        //{
        //    get
        //    {
        //        return sysFormat;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetCurrencySymbol()
        {
            if (CultureHelper.IsEnglish)
                return arabicCurrency + " ";
            else
            {
                CultureInfo c = CultureInfo.GetCultureInfo(ArabicCultureSymbol);
                return c.NumberFormat.CurrencySymbol;
            }
        }

        public static bool ValidateCulture(string cultureName)
        {
            if (string.Compare(cultureName, ArabicCultureSymbol) == 0) return true;
            if (string.Compare(cultureName, EnglishCultureSymbol) == 0) return true;

            return false;
        }

        public static string ConfigureDateFormat
        {
          
            get
            {
                string format = AppConfigurationManager.GetValue<string>(AppConfigurationManager.SystemDateFormat);
                string value = "dd/MM/yyyy";
                switch (format)
                {
                    case "dd/MM/yyyy":
                        value= "dd/MM/yyyy";
                        break;
                    case "MM/dd/yyyy":
                        value = "MM/dd/yyyy";
                        break;
                    case "yyyy/MM/dd":
                        value = "yyyy/MM/dd";
                        break;
                }
                return value;
            }
        }
        public static string ConfigureAuditLogDateFormat
        {

            get
            {
                string format = AppConfigurationManager.GetValue<string>(AppConfigurationManager.SystemDateFormat);
                string value = "dd/MM/yyyy";
                switch (format)
                {
                    case "dd/MM/yyyy":
                        value = "dd/MM/yyyy HH:mm:ss";
                        break;
                    case "MM/dd/yyyy":
                        value = "MM/dd/yyyy HH:mm:ss";
                        break;
                    case "yyyy/MM/dd":
                        value = "yyyy/MM/dd HH:mm:ss";
                        break;
                }
                return value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static void SetCulture()
        {
            SetCulture(CultureHelper.EnglishCultureSymbol);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cultureName"></param>
        public static void SetCulture(string cultureName)
        {
            _httpContextAccessor.HttpContext.Session.SetString("__CULTURE__", cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo(cultureName);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetSessionCulture()
        {
            return _httpContextAccessor.HttpContext.Session.GetString("__CULTURE__").ToString();
            //return HttpContext.Current.Session["__CULTURE__"] + "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetBodyClass()
        {
            if (string.Compare(CurrentCulture.Name, ArabicCultureSymbol, true) == 0)
                return "k-rtl";
            else
                return "";
        }

        /// <summary>
        /// gets the name of the next culture (Not currently in)
        /// </summary>
        /// <returns></returns>
        public static string GetOtherCultureName()
        {
            if (CultureHelper.IsEnglish)
                return Language.GetString("Arabic", ArabicCultureSymbol);
            else
                return Language.GetString("English", EnglishCultureSymbol);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetOtherCultureSymbol()
        {
            if (CultureHelper.IsEnglish)
                return ArabicCultureSymbol;
            else
                return EnglishCultureSymbol;
        }

        public static string GetCurrentCultureSymbol()
        {
            if (CultureHelper.IsEnglish)
                return EnglishCultureSymbol;
            else
                return ArabicCultureSymbol;
        }

        public static void SetTheme(string themeName)
        {
            _httpContextAccessor.HttpContext.Session.SetString("_CurrentTheme_", themeName);
           // HttpContext.Current.Session["_CurrentTheme_"] = themeName;
        }
        public static string GetTheme()
            
        {
            if (string.Compare("GreenTheme", _httpContextAccessor.HttpContext.Session.GetString("_CurrentTheme_").ToString())==0)
               
            {
                return "GreenTheme";
            }
            else if (string.Compare("OrangeTheme", _httpContextAccessor.HttpContext.Session.GetString("_CurrentTheme_").ToString() )== 0)               
            {
                return "OrangeTheme";
            }
            else if (string.Compare("RedTheme", _httpContextAccessor.HttpContext.Session.GetString("_CurrentTheme_").ToString()) == 0)
            {
                return "RedTheme";
            }
            return "";
        }

      
    }
}
