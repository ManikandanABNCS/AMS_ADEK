using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.AMS.DAL
{
    public class DynamicReportFieldModel
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
    }

    public class ReportParmeterValue
    {
        public string name { get; set; }
        public string value { get; set; }
    }
}
