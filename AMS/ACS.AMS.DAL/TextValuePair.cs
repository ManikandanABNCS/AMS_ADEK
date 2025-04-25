using System;
using System.Collections.Generic;
using System.Text;

namespace ACS.AMS.DAL
{
    public class TextValuePair<TextType, ValueType>
    {
        public TextValuePair()
        {

        }

        public TextType Text { get; set; }

        public ValueType Value { get; set; }

    }
}
