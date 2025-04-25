using System;
using System.Collections.Generic;
using System.Text;

namespace ACS.AMS.DAL
{
    public class ValidationException : Exception
    {
        public ValidationException(string error) : base(error)
        {

        }
        public ValidationException(string message, object entityObject, string fieldName) : base(message)
        {
            EntityObject = entityObject;
            FieldName = fieldName;
        }
        /// <summary>
        /// 
        /// </summary>
        public object EntityObject { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string FieldName { get; private set; }
    }
}
