using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACS.AMS.WebApp
{
    public class DynamicFieldValueModel
    {
        public string ControlName { get; set; }

        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public DateTime? DTValue1 { get; set; }
        public DateTime? DTValue2 { get; set; }

        public decimal? MinValue { get; set; }

        public decimal? MaxValue { get; set; }

        public int? NoofDigit { get; set; }
        public string fieldIDSuffix { get; set; }
        public string displayName { get; set; }

    }
}