using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL
{
    public class ValidateEventArgs<T> where T : DbContext
    {
        public ValidateEventArgs(T currentDB, T newDB, EntityObjectState state)
        {
            NewDB = newDB;
            CurrentDB = currentDB;
            State = state;
        }

        /// <summary>
        /// 
        /// </summary>
        public T NewDB { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public T CurrentDB { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public EntityObjectState State { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string ErrorMessage { get; set; }

        public string FieldName { get; set; }
    }
}
