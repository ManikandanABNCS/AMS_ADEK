using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using ACS.AMS.DAL;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;

namespace ACS.AMS.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public static class AppContext
    {

        static AppContext()
        {
                      
        }

        public static string[] UploadFileExtensions
        {
            get
            {
                return new string[] { ".png", ".jpg", ".jpeg" };
            }
        }
    }
}