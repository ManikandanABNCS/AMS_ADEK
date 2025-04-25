using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACS.AMS.DAL.DBModel
{
    public partial class ApplicationErrorLogTable
    {
        public static long SaveException(Exception ex)
        {
            return SaveException(AMSContext.CreateNewContext(), ex, "", ex.StackTrace);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="requestURL"></param>
        public static long SaveException(Exception ex, string requestURL)
        {
            return SaveException(AMSContext.CreateNewContext(), ex, requestURL, ex.StackTrace);
        }
        public static long SaveException(AMSContext db, Exception ex, string requestURL, string stackTrace)
        {
            //save the exceptions into the DB
            ApplicationErrorLogTable parent = null;
            ApplicationErrorLogTable errorParentObj = null;

            do
            {
                ApplicationErrorLogTable errorLog = new ApplicationErrorLogTable();
                db.Add(errorLog);

                errorLog.ErrorMessage = ex.Message;
                if (string.IsNullOrEmpty(stackTrace))
                    errorLog.StackTrace = ex.StackTrace;
                else
                    errorLog.StackTrace = stackTrace;

                if ((errorLog.StackTrace != null) && (errorLog.StackTrace.Length > 3999))
                    errorLog.StackTrace = errorLog.StackTrace.Substring(0, 3999);

                errorLog.URI = requestURL;
                errorLog.OccuredDateTime = DateTime.Now;
                parent = errorLog;
                if (errorParentObj == null)
                    errorParentObj = errorLog;

                ex = ex.InnerException;
            }
            while (ex != null);

            db.SaveChanges();
            //db.Dispose();

            return errorParentObj.ApplicationErrorLogID;
        }
        public static IQueryable<DBModel.ApplicationErrorLogTable> GetAllAppErrorLog(AMSContext db)
        {
            return db.ApplicationErrorLogTable.AsQueryable();
        }
        public static DBModel.ApplicationErrorLogTable GetApplicationError(AMSContext db, int appErrorLogID)
        {
            var result = (from b in db.ApplicationErrorLogTable where b.ApplicationErrorLogID == appErrorLogID select b).FirstOrDefault<DBModel.ApplicationErrorLogTable>();
            return result;
        }
    }
}
