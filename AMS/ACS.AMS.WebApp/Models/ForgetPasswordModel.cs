using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using ACS.AMS.WebApp   ;
using ACS.AMS.DAL.DataAnnotations;
using ACS.DataAnnotations;


namespace ACS.AMS.WebApp.Models
{
    [PropertiesMustMatch("NewPassword", "ConfirmPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public class NewChangePasswordModel
    {

        [Required(ErrorMessage = "New password is Required")]          
        [DataType(DataType.Password)]
        [DisplayName("New password ")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm new password is Required")]          
        [DataType(DataType.Password)]
        [DisplayName("Confirm new password ")]
        public string ConfirmPassword { get; set; }
    }
 
    public class ForgetPasswordModel
    {

        [Required(ErrorMessage = "Email is Required")]
        [ACSRegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid EmailID")]
        public string Email { get; set; }
    }
    //public class AuditLogReportModel
    //{
    //    public string FromDate { get; set; }
    //    public string ToDate { get; set; }
    //    public int UserID { get; set; }
    //    public String ActionName { get; set; }

    //    public string CompanyID { get; set; }
    //    public int LanguageID { get; set; }
    //}


}