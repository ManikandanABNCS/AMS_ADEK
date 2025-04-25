using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACS.AMS.DAL
{
    //public class FieldTypeValue
    public enum FieldTypeValue
    {
        //public const string String = "String";
        //public const string MultilineString = "MultilineString";
        //public const string NumericInteger = "Numeric-Integer";
        //public const string NumericDecimal = "Numeric-Decimal";
        //public const string Date = "Date";
        //public const string Time = "Time";
        //public const string DateTime = "DateTime";
        //public const string YesNo = "Yes/No";
        //public const string Currency = "Currency";
        //public const string Dropdown = "Dropdown";
        //public const string ComboBox = "ComboBox";

          Integer = 1,
        Float = 2,
        String = 3,
        BigString = 4,
        Date = 5,
        Time = 6,
        DateTime = 7,
        Integer_Rage = 8,
        Float_Range = 9,
        Date_Range = 10,
        Dropdown = 11,
        Hierarchical=12,
        ComboBox = 13,
        Currency=14
    }
}
