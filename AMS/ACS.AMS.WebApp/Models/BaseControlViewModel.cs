using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACS.AMS.WebApp.Models
{
    public class BaseControlViewModel
    {
        public string ControlName { get; set; }

        public object Value { get; set; }

        public string Text { get; set; }

        public string ValueFieldName { get; set; }

        //public bool IsMandatory { get; set; } = false;

        public BaseControlViewModel()
        {

        }

        public BaseControlViewModel(string fieldName, object val)
        {
            ControlName = fieldName;
            Value = val;
        }

    }
}