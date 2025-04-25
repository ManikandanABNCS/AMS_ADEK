using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ACS.AMS.WebApp.Models
{
    public class MultiColumnViewModel : BaseControlViewModel
    {
        public string PlaceholderText { get; set; }

        public string ParentFieldName { get; set; }

        public string DataReadScriptFunctionName { get; set; }

        public string DataReadControllerName { get; set; }

        public string DataReadActionName { get; set; }

        public object RouteValues { get; set; }

        public string ChangeScriptFunctionName { get; set; }

        public string SelectScriptFunctionName { get; set; }

        public MultiColumnViewModel()
        {

        }

        public MultiColumnViewModel(string fieldName, object val) : base(fieldName, val)
        {
            
        }
    }
}