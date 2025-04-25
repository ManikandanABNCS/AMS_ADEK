using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using ACS.AMS.DAL;

namespace ACS.DataAnnotations
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class ACSRegularExpressionAttribute : RegularExpressionAttribute
    {
        public ACSRegularExpressionAttribute(string pattern)
            : base(pattern)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            var errorMessageString = base.ErrorMessage;
            base.FormatErrorMessage(name);
            return Language.GetErrorMessage(errorMessageString);
            //return string.Format(CultureInfo.CurrentCulture, name, new object[] { name, base.Pattern });
        }
    }

   
}
