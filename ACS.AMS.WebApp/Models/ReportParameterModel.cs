using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACS.AMS.WebApp
{
    public class ReportParameterModel
    {
        public ReportParameterModel(string nm)
        {
            this.Name = nm;
            this.IsNullable = false;
        }

        public string Name { get; set; }

        public string DataType { get; set; }

        public bool IsNullable { get; set; }

        public override string ToString()
        {
            return $"{Name}, {DataType}";
        }
    }
}
