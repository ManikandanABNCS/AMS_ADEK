using System;
using System.Collections.Generic;
using System.Text;

namespace ACS.AMS.DAL
{
    public class PeriodModel
    {
        public int PeriodID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PeriodName { get; set; }
        public string Status { get; set; }
        public int PeriodYear { get; set; }
        public int lastField { get; set; }
        public int nextRecord { get; set; }
        public int depreciationID { get; set; }
    }
}
