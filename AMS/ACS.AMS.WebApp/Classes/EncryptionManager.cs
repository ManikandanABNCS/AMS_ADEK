using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ACS.AMS.WebApp
{
    public class EncryptionManager
    {
        public static string EncryptPassword(string password)
        {
            byte[] b = Encoding.Unicode.GetBytes(password);
            return Convert.ToBase64String(new SHA1Managed().ComputeHash(b));
        }
    }
}
