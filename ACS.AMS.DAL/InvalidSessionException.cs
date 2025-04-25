using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACS.AMS.DAL
{
    public class InvalidSessionException : Exception
    {
        public InvalidSessionException()
        {

        }

        public InvalidSessionException(string message)
            : base(message)
        {

        }
    }
}
