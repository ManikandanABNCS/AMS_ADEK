using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ACS.AMS.DAL;
using Microsoft.EntityFrameworkCore;
using Castle.Components.DictionaryAdapter.Xml;

namespace ACS.AMS.DAL.DBModel
{
    public partial class MasterTable
    {
      
        public static IQueryable<NotificationTemplateNotificationTypeTable> GetAllNotificationTemplateTypebyNotificationTemplateID(AMSContext db, int? notificationTemplateID)
        {

            var result = (from b in db.NotificationTemplateNotificationTypeTable
                          where b.StatusID == (int)StatusValue.Active && b.NotificationTemplateID == notificationTemplateID
                          orderby b.NotificationTemplateNotificationTypeID descending
                          select b);
            return result;
        }
        public static NotificationTemplateNotificationTypeTable GetNotificationTemplateTypebynotificationTemplateIDNotificationID(AMSContext db, int? notificationTemplateID, int notificationTypeID)
        {


            var result = (from b in db.NotificationTemplateNotificationTypeTable
                          where b.StatusID == (int)StatusValue.Active && b.NotificationTemplateID == notificationTemplateID
                          && b.NotificationTypeID == notificationTypeID
                          select b).FirstOrDefault();
            return result;

        }

        public static void deleteNotificationTemplateFieldTable(AMSContext db, int notificationTemplateID)
        {
            var result = (from b in db.NotificationTemplateFieldTable
                          where b.NotificationTemplateID == notificationTemplateID
                          select b);
            if (result.Count() > 0)
            {
                foreach (var res in result)
                {
                    res.StatusID = (int)StatusValue.Deleted;
                   // db.NotificationTemplateFieldTable.Remove(res);

                }
                db.SaveChanges();
                // db.Dispose();

            }
        }

        public static void deleteNotificationModuleFieldTable(AMSContext db, int notificationModuleID)
        {
            var result = (from b in db.NotificationModuleFieldTable
                          where b.NotificationModuleID == notificationModuleID && b.StatusID==(int)StatusValue.Active
                          select b);
            if (result.Count() > 0)
            {
                foreach (var res in result)
                {
                    res.StatusID = (int)StatusValue.Deleted;

                }
                db.SaveChanges();
                // db.Dispose();

            }
        }
        public static IQueryable<ReportFieldTable> GetAllReportFields(AMSContext db, int reportID)
        {
            var result = (from b in db.ReportFieldTable.Include("ReportTemplateField")
                          where b.StatusID == (int)StatusValue.Active
                          && b.ReportID == reportID
                          orderby b.DisplayOrderID
                          select b);

            return result;
        }
    }
}