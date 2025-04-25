using ACS.AMS.DAL.DBContext;
using ACS.AMS.DAL.DBModel;

namespace ACS.AMS.WebApp.Classes
{
    public static class CodeGenerationHelper
    {
        private static Dictionary<string, object> threadObjects = new Dictionary<string, object>();

        public static string GetNextCode(string codeName)
        {
            if(!threadObjects.ContainsKey(codeName))
            {
                lock (threadObjects)
                {
                    threadObjects.Add(codeName, new object());
                }
            }

            using (var db = AMSContext.CreateNewContext())
            {
                var currentThreadObject = threadObjects[codeName];
                lock (currentThreadObject)
                {
                    var itm = (from b in db.EntityCodeTable
                               where b.EntityCodeName == codeName
                               select b).FirstOrDefault();

                    if (itm == null)
                    {
                        //create a new code with default values
                        itm = new EntityCodeTable()
                        {
                            EntityCodeName = codeName,
                            LastUsedNo = 0,
                            CodeFormatString = "{0:00000}",
                        };
                        
                        db.EntityCodeTable.Add(itm);
                        db.SaveChanges();
                    }

                    itm.LastUsedNo = itm.LastUsedNo + 1;

                    //format the string
                    var codeString = "" + itm.CodePrefix.Replace("#DateTime.Now.Year#", DateTime.Now.Year.ToString());
                    codeString += string.Format(itm.CodeFormatString, itm.LastUsedNo, DateTime.Now.Year);
                    codeString += itm.CodeSuffix;

                    db.SaveChanges();

                    return codeString;
                }
            }
        }
    }
}
