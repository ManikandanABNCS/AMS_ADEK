using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter.Xml;

namespace ACS.AMS.DAL.DBModel
{
    public partial class NotificationTemplateNotificationTypeTable:BaseEntityObject
    {
        public static IQueryable<NotificationTemplateNotificationTypeTable> GetAllNotificationTemplateTypebyNotificationTemplateID(AMSContext db, int? notificationTemplateID)
        {

            var result = (from b in db.NotificationTemplateNotificationTypeTable
                          where b.StatusID == (int)StatusValue.Active && b.NotificationTemplateID == notificationTemplateID
                          orderby b.NotificationTemplateNotificationTypeID descending
                          select b);
            return result;
        }
    }
}
