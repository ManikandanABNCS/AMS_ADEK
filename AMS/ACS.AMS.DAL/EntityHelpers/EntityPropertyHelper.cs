using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ACS.AMS.DAL
{
    public static class EntityPropertyHelper
    {
        public static PropertyInfo GetForeignKeyField(Type entityType, string propertyName)
        {
            var prop = from b in entityType.GetProperties()
                       where Attribute.IsDefined(b, typeof(ForeignKeyAttribute))
                       select b;

            foreach(var p in prop)
            {
                ForeignKeyAttribute fKey = Attribute.GetCustomAttributes(p, typeof(ForeignKeyAttribute)).FirstOrDefault() as ForeignKeyAttribute;
                if (fKey != null)
                {
                    if (string.Compare(fKey.Name, propertyName, true) == 0) 
                        return p;
                }
            }

            return null;
        }
    }
}
