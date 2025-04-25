using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.Specialized;

namespace ACS.AMS.DAL
{
    public class DataUtilities
    {
        
        private static StringCollection ignoreCopyObjectList = new StringCollection();

        /// <summary>
        /// 
        /// </summary>
        static DataUtilities()
        {
            ignoreCopyObjectList.Add("CreatedBy".ToUpper());
            ignoreCopyObjectList.Add("CreatedDateTime".ToUpper());
            ignoreCopyObjectList.Add("LastModifiedBy".ToUpper());
            ignoreCopyObjectList.Add("LastModifiedDate".ToUpper());
            ignoreCopyObjectList.Add("LastModifiedDateTime".ToUpper());
            ignoreCopyObjectList.Add("StatusID".ToUpper());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <param name="destObject"></param>
        public static void CopyObject(object sourceObject, object destObject, bool copyBasicProperties = false)
        {
            PropertyDescriptorCollection propCollection = TypeDescriptor.GetProperties(destObject.GetType());

         //   ObjectContext.GetObjectType(sourceObject.GetType()).GetHashCode();
            if (sourceObject.GetType().GetHashCode() == destObject.GetType().GetHashCode())
            {
                foreach (PropertyDescriptor prop in propCollection)
                {
                    if (prop.IsReadOnly) continue;
                    if (prop.PropertyType.IsGenericType)
                    {
                        var arg = prop.PropertyType.GetGenericArguments();
                        if((arg.Length > 0) && (arg[0].IsClass))
                            continue;
                    }
                    if ((prop.PropertyType != typeof(string)) && (prop.PropertyType.IsClass)) continue;

                    //dont copy the created, modified date and user
                    if (!copyBasicProperties)
                    {
                        if (ignoreCopyObjectList.Contains(prop.Name.ToUpper())) continue;
                    }

                    prop.SetValue(destObject, prop.GetValue(sourceObject));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceObject"></param>
        /// <param name="destObject"></param>
        public static void CopyObjects<T>(IEnumerable<T> sourceObject, IList<T> destObject, bool copyBasicProperties = false)
        {
            Type objType = typeof(T);

            foreach (T tbl in sourceObject)
            {
                T newFI = Activator.CreateInstance<T>();
                CopyObject(tbl, newFI, copyBasicProperties);

                destObject.Add(newFI);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceObject"></param>
        /// <returns></returns>
        public static List<T> CloneObjects<T>(IEnumerable<T> sourceObject, bool copyBasicProperties = false)
        {
            List<T> destObject = new List<T>();

            CopyObjects<T>(sourceObject, destObject, copyBasicProperties);

            return destObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="destobject"></param>
        /// <param name="namePrefix"></param>
        public static void CopyCollectionValues(NameValueCollection srcCollection, object destObject, string namePrefix, bool preventFromException = false)
        {
            PropertyDescriptorCollection propCollection = TypeDescriptor.GetProperties(destObject.GetType());

            foreach (PropertyDescriptor prop in propCollection)
            {
                string nm = namePrefix + prop.Name;
                if (srcCollection[nm] != null)
                {
                    string strVal = srcCollection[nm];
                    //check it is boolean type
                    if (prop.PropertyType == typeof(bool))
                    {
                        if (strVal.Contains(","))
                            strVal = strVal.Substring(0, strVal.IndexOf(","));
                    }

                    try
                    {
                        object val = prop.Converter.ConvertFrom(strVal);
                        prop.SetValue(destObject, val);
                    }
                    catch (Exception ex)
                    {
                        if (!preventFromException)
                            throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// Function replaces the single quote into \' and \ into \\. so the data will displayed properly in grids.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects"></param>
        public static void CheckAndTrimStringValues<T>(IEnumerable<T> objects)
        {
            PropertyDescriptorCollection propCollection = TypeDescriptor.GetProperties(typeof(T));

            foreach (PropertyDescriptor prop in propCollection)
            {
                if (prop.IsReadOnly) continue;

                if (prop.PropertyType == typeof(string))
                {
                    foreach (T tbl in objects)
                    {
                        string val = Convert.ToString(prop.GetValue(tbl));

                        //replace the \ into \\
                        val = val.Replace(@"\", @"\\");
                        //replace the ' into \'
                        val = val.Replace("'", @"\'");

                        prop.SetValue(tbl, val);
                    }
                }
            }
        }
    }
}
