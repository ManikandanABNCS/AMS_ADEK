using System;
using System.Collections.Generic;
using System.Text;

namespace ACS.AMS.DAL
{
    public class AuditLogPropertyValue
    {
        public AuditLogPropertyValue(object oldValue, object newValue, object id)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
            this.PrimaryID = id;
        }

        public object OldValue { get; set; }

        public object NewValue { get; set; }

        public object PrimaryID { get; set; }
    }
}
