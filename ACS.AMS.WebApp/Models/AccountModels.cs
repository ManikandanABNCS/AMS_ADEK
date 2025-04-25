using ACS.AMS.DAL;
using ACS.AMS.DAL.DataAnnotations;
using ACS.AMS.DAL.DBModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ACS.AMS.WebApp.Models
{

    [PropertiesMustMatch("NewPassword", "ConfirmPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public class ChangePasswordModel
    {
        [DisplayName("Old Password")]
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [DisplayName("New Password")]
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DisplayName("Confirm Password")]
        [Required]
        //  [Compare("NewPassword")]
        [CompareAttribute("NewPassword", ErrorMessage = "Password doesn't match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        //  [VivensasRequired]
        [DisplayName("User name")]
        public string UserName { get; set; }

        //  [VivensasRequired]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        public int AccountingYearID { get; set; }

        [DisplayName("Remember me")]
        public bool RememberMe { get; set; }

        [DisplayName("Forgot Password")]
        public bool ForgotPassword { get; set; }
    }

    [PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "The password and confirmation password do not match.")]
    public class RegisterModel
    {
        public RegisterModel()
        {
            Person = new PersonTable();
           
        }

        public PersonTable Person { get; set; }     

        [Required]
        [StringLength(25)]
        public string UserName { get; set; }

        [Required]
        // [RegularExpression("[A-Za-z0-9]*", ErrorMessage = "Invalid Password")]
        [DataType(DataType.Password)]
        [StringLength(20)]
        public string Password { get; set; }

        [Required]
        //[RegularExpression("[A-Za-z0-9]*", ErrorMessage = "InvalidConfirmPassword")]
        [DataType(DataType.Password)]
        [StringLength(20)]
        //  [Compare("NewPassword")]
        [CompareAttribute("Password", ErrorMessage = "Password doesn't match.")]
        public string ConfirmPassword { get; set; }

        public int CurrentPageID { get; set; }

        //public string CompanyID { get; set; }

    }
    [PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "The password and confirmation password do not match.")]
    public class EditRegisterModel
    {

        public EditRegisterModel()
        {
            Person = new PersonTable();
        }

        public PersonTable Person { get; set; }

        //[RegularExpression("[A-Za-z0-9]*", ErrorMessage = "Invalid Username")]
        [StringLength(25)]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [StringLength(20)]
        public string? Password { get; set; }


        [DataType(DataType.Password)]
        [StringLength(20)]
        public string? ConfirmPassword { get; set; }
       
        public int CurrentPageID { get; set; }
        //public string CompanyID { get; set; }
    }
    public class UserRightList
    {

        public int RightID { get; set; }
        public string RightName { get; set; }
        public string RightDescription { get; set; }
        public int ValueType { get; set; }
        public int? RightGroupID { get; set; }
        public string RightGroupName { get; set; }
    }
    public class RolesModel
    {
        /// <summary>
        /// 
        /// </summary>
        public Object RoleID { get; set; }

        [Required]
        [DisplayName("Role Name")]
        public string RoleName { get; set; }

        [Required]
        [DisplayName("Person Name")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Active Status")]
        public string ActiveStatus
        {
            get
            {
                return IsActive ? Language.GetString("Active") : Language.GetString("Inactive");
            }
        }

        [Required]
        [DisplayName("IsActive")]
        public bool IsActive { get; set; }

        [Required]
        [DisplayName("Display Role")]
        public bool DisplayRole { get; set; }

    }

    #region Services




    #endregion



}
