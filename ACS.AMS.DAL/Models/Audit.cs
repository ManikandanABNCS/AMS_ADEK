using System;
using System.Collections.Generic;
using System.Text;

namespace ACS.AMS.DAL
{
    public class Audit
    {
        public int Action { get; set; }
        public string TypeId { get; set; }
        public string PrimaryKeyName { get; set; }
        public string ColumnName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
