using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.AMS.DAL.DBSchema
{
    public class DBObjectSchema
    {
        public List<SchemaFieldInfo> Columns { get; set; }

        public List<SchemaFieldInfo> Parameters { get; set; }
    }

    public class SchemaFieldInfo
    {
        public string Name { get; set; }

        public Type DataType { get; set; }
    }

    //public class ParameterInfo
    //{
    //    public string ParameterName { get; set; }

    //    public Type DataType { get; set; }
    //}
}
