using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ACS.AMS.DAL.DBModel
{
    public partial class NotificationReportAttachmentTable : BaseEntityObject
    {
        public static NotificationReportAttachmentTable GetReportAttachment(AMSContext context,int notificationTemplateID)
        {
            var result = (from b in context.NotificationReportAttachmentTable
                          where b.NotificationTemplateID == notificationTemplateID
                          select b).FirstOrDefault();
            return result;
        }
    }
}
