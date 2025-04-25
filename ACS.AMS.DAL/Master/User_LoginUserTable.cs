using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ACS.AMS.DAL;
using System.Text;
using System.DirectoryServices.AccountManagement;


namespace ACS.AMS.DAL.DBModel
{
    public partial class User_LoginUserTable : BaseEntityObject
    {
        public static IQueryable<User_LoginUserTable> GetAllUser(AMSContext db)
        {
            return db.User_LoginUserTable.AsQueryable();
        }

        public static User_LoginUserTable GetUser(AMSContext db, string userName)
        {
            var result = (from b in db.User_LoginUserTable where b.UserName == userName select b).FirstOrDefault();
            return result;
        }

        public static User_LoginUserTable GetExistingUser(AMSContext db, int userID)
        {
            var result = (from b in db.User_LoginUserTable where b.UserID == userID select b).FirstOrDefault();
            return result;
        }

        public static string ValidateUser(AMSContext db, string userName, string password)
        {
            var userDetails = User_LoginUserTable.GetUser(db, userName);
            if (userDetails == null)
            {
                return "Enter valid Username and Password";
            }
            if (userDetails.IsDisabled == true || userDetails.IsLockedOut == true)
            {
                return "Enter valid Username and Password";
            }

            if (string.Compare(userName, "admin", true) != 0)
            {
                TraceLog("Validate Login", $"Login validation, User: {userName}");
                var person = PersonTable.GetItem(db, userDetails.UserID);
                // For AD
                if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.ActiveDirectoryEnabled) && string.Compare(person.CreatedFrom, "AD",true)==0)
                {
                    bool ValidateUser = User_LoginUserTable.ValidateADUser(userName, password);
                    TraceLog("Validate Login", $"AD Login validation, result: {ValidateUser}");

                    if (ValidateUser == false)
                    {
                        return "Enter valid Username and Password";
                    }
                    else
                    {
                        return "Success";
                    }
                }
                else
                {
                    string saltedPassword = EncryptPassword(password, userDetails.PasswordSalt);

                    if (userDetails.Password != saltedPassword)
                    {
                        TraceLog("Validate Login", $"Local Login validation, result: {false}");
                        return "Enter valid Username and Password";
                    }
                    else
                    {
                        return "Success";
                    }
                }
            }
            else
            {
                string saltedPassword = EncryptPassword(password, userDetails.PasswordSalt);

                if (userDetails.Password != saltedPassword)
                {
                    TraceLog("Validate Login", $"Local Login validation, result: {false}");

                    return "Enter valid Username and Password";
                }
                else
                {
                    return "Success";
                }
            }
        }

        public static string CheckOldPwd(AMSContext db, int userID, string oldPwd)
        {
            var result = (from b in db.User_LoginUserTable where b.UserID == userID select b).FirstOrDefault();
            string saltedPassword = EncryptPassword(oldPwd, result.PasswordSalt);
            if (result.Password != saltedPassword)
            {
                return "Wrong Old Password";
            }
            else
            {

                return "Success";
            }
        }
        private static string EncryptPassword(string password, string salt)
        {
            string saltPassword = password + salt;
            byte[] b = Encoding.Unicode.GetBytes(saltPassword);
            return Convert.ToBase64String(new System.Security.Cryptography.SHA1Managed().ComputeHash(b));
        }
        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            return true;
        }
        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            return true;
        }
        //internal override bool ValidateObject(ValidateEventArgs<AMSContext> args)
        //{
        //    return true;
        //}

        public static PrincipalContext GetPrincipalContext()
        {
            AMSContext _db = AMSContext.CreateNewContext();

            var domainName = (from b in _db.ConfigurationTable where b.ConfiguarationName == "ADDomainName" select b.ConfiguarationValue).FirstOrDefault();//AppConfigurationManager.GetValue<string>(AppConfigurationManager.ADDomainName);
            var adusername = (from b in _db.ConfigurationTable where b.ConfiguarationName == "ADUsername" select b.ConfiguarationValue).FirstOrDefault(); //AppConfigurationManager.GetValue<string>(AppConfigurationManager.ADUsername);
            var adPassword = (from b in _db.ConfigurationTable where b.ConfiguarationName == "ADPassword" select b.ConfiguarationValue).FirstOrDefault(); //AppConfigurationManager.GetValue<string>(AppConfigurationManager.ADPassword);

            PrincipalContext AD = null;

            if (string.IsNullOrEmpty(adusername))
                AD = new PrincipalContext(ContextType.Domain, domainName);
            else
                AD = new PrincipalContext(ContextType.Domain, domainName, adusername, adPassword);

            return AD;
        }

        public static bool ValidateADUser(string username, string password)
        {
            bool valid = false;
            try
            {
                using (PrincipalContext context = GetPrincipalContext())
                {
                    valid = context.ValidateCredentials(username, password);
                   // ApplicationErrorLogTable.SaveException(new Exception("Check Validation => " + username + valid.ToString()));
                }

                //ApplicationErrorLogTable.SaveException(new Exception("AD Uservalidation for " + username + ", result: " + valid));
            }
            catch (Exception ex)
            {
                ApplicationErrorLogTable.SaveException( ex);
            }
            return valid;
        }

    }
}