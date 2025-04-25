using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.AMS.DAL
{
    public class ImportTemplateModel
    {
        public string ImportField { get; set; }
        public string EntityName { get; set; }
        public bool IsForeignKey { get; set; }
        public bool IsMandatory { get; set; }
        public int DataSize { get; set; }
        public bool? IsUnique { get; set; }
        public int DispalyOrderID { get; set; }
        public int FieldTypeID { get; set; }
        public int? ReferenceEntityID { get; set; }
        public string ReferenceEntityName { get; set; }
        public string LanguageImportField { get; set; }
    }
}
