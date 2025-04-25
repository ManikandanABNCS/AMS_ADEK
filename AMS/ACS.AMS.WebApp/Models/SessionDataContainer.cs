namespace ACS.AMS.WebApp.Models
{
    public class SessionDataContainer
    {
        //private static string sessionContainerName = "__SessionContainer__";
        private static object sessionObject = new object();
        private static Dictionary<string, object> sessionObjects = new Dictionary<string, object>();

       

        public static void SetSessionObject<T>(int pageID, T sessionObject) where T: class
        {
            lock(sessionObject)
            {
                string key = $"{AppHttpContext.Current.Session.Id}_{pageID}";
                sessionObjects.Add(key, sessionObject);
            }
        }

        public static T GetSessionObject<T>(int pageID) where T : class
        {
            lock (sessionObject)
            {
                string key = $"{AppHttpContext.Current.Session.Id}_{pageID}";

                if(sessionObjects.ContainsKey(key))
                    return sessionObjects[key] as T;

                return default(T);
            }
        }

        public static void RemoveSessionObject(int pageID)
        {
            lock (sessionObject)
            {
                string key = $"{AppHttpContext.Current.Session.Id}_{pageID}";

                if (sessionObjects.ContainsKey(key))
                    sessionObjects.Remove(key);
            }
        }



        public static void SetSessionObjectWithName<T>(string pageName, int pageID, T sessionObject) where T : class
        {
            lock (sessionObject)
            {
                string key = $"{AppHttpContext.Current.Session.Id}_{pageName}_{pageID}";
                sessionObjects.Add(key, sessionObject);
            }
        }

        public static T GetSessionObjectWithName<T>(string pageName,int pageID) where T : class
        {
            lock (sessionObject)
            {
                string key = $"{AppHttpContext.Current.Session.Id}_{pageName}_{pageID}";

                if (sessionObjects.ContainsKey(key))
                    return sessionObjects[key] as T;

                return default(T);
            }
        }

        public static void RemoveSessionObjectWithName(string pageName, int pageID)
        {
            lock (sessionObject)
            {
                string key = $"{AppHttpContext.Current.Session.Id}_{pageName}_{pageID}";

                if (sessionObjects.ContainsKey(key))
                    sessionObjects.Remove(key);
            }
        }
    }
}