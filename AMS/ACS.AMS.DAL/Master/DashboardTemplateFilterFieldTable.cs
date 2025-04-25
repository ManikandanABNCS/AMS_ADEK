using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.AMS.DAL.DBModel
{
    public partial class DashboardTemplateFilterFieldTable : BaseEntityObject
    {
        [NotMapped]
        public bool EnableFilter { get; set; }
        [NotMapped]
        public string FilterTypeDesc { get; set; }
        [NotMapped]
        public string FieldDataType { get; set; }



        //[NotMapped]
        //public string FieldName { get; set; }

        [NotMapped]
        public string FieldValue { get; set; }
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

    }
}