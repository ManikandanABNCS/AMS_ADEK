using System;
using System.Collections.Generic;
using System.Text;

namespace ACS.AMS.DAL
{
    [Flags]
    public enum UserRightValue
    {
        None = 0,

        View = 1,

        Create = 2,

        Edit = 4,

        Delete = 8,

        ExportToCSV = 16,

        //ExportToPDF = 32,

        //ExportToExcel = 64,

        Details = 32
    }
}
