using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACS.AMS.DAL.DBContext;
using Castle.Components.DictionaryAdapter.Xml;

namespace ACS.AMS.DAL.DBModel
{
    public partial class NotificationTemplateTable:BaseEntityObject
    {
        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.NotificationTemplateTable
                                  where b.NotificationModuleID == this.NotificationModuleID
                                  && b.TemplateCode == this.TemplateCode
                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();
            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Notification Template already exists for this notification", this.TemplateCode);
                args.FieldName = "TemplateCode";
                return false;
            }
            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.NotificationTemplateTable
                                  where (b.NotificationModuleID == this.NotificationModuleID && b.StatusID == (int)StatusValue.Active
                                  && b.TemplateCode == this.TemplateCode)
                                        && b.NotificationTemplateID != this.NotificationTemplateID
                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Notification Template already exists for this notification", this.TemplateCode);
                args.FieldName = "TemplateCode";
                return false;
            }
            return true;
        }
        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.NotificationInputTable
                                  where (b.NotificationTemplateID == this.NotificationTemplateID)

                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Notification Template references found", this.NotificationTemplateID);
                args.FieldName = "NotificationInput";
                return false;
            }
            return true;
        }
        //internal override bool ValidateObject(ValidateEventArgs<AMSContext> args)
        //{
        //    switch (args.State)
        //    {
        //        case EntityObjectState.New:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.NotificationTemplateTable
        //                                      where b.NotificationModuleID == this.NotificationModuleID 
        //                                      && b.TemplateCode == this.TemplateCode
        //                                      && b.StatusID==(int)StatusValue.Active
        //                                      select b).Count();
        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Notification Template already exists for this notification", this.TemplateCode);
        //                    args.FieldName = "TemplateCode";
        //                    return false;
        //                }
        //                return true;
        //            }

        //        case EntityObjectState.Edit:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.NotificationTemplateTable
        //                                      where (b.NotificationModuleID == this.NotificationModuleID && b.StatusID == (int)StatusValue.Active
        //                                      && b.TemplateCode == this.TemplateCode)
        //                                            && b.NotificationTemplateID != this.NotificationTemplateID
        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Notification Template already exists for this notification", this.TemplateCode);
        //                    args.FieldName = "TemplateCode";
        //                    return false;
        //                }

        //                return true;
        //            }

        //        case EntityObjectState.Delete:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.NotificationInputTable
        //                                      where (b.NotificationTemplateID == this.NotificationTemplateID)
                                                    
        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Notification Template references found", this.NotificationTemplateID);
        //                    args.FieldName = "NotificationInput";
        //                    return false;
        //                }
        //                return true;
        //            }
        //    }
        //    return true;
        //}
    }
}
