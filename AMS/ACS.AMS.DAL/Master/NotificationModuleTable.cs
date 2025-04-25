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
    public partial class NotificationModuleTable
    {
        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.NotificationModuleTable
                                  where b.NotificationModule == this.NotificationModule

                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();
            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Notification Module already exists ", this.NotificationModuleID);
                args.FieldName = "NotificationModule";
                return false;
            }
            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.NotificationModuleTable
                                  where (b.NotificationModule == this.NotificationModule && b.StatusID == (int)StatusValue.Active
                                  && b.NotificationModuleID != this.NotificationModuleID)

                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Notification Module already exists ", this.NotificationModuleID);
                args.FieldName = "NotificationModule";
                return false;
            }
            return true;
        }
        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.NotificationModuleFieldTable
                                  where (b.NotificationModuleID == this.NotificationModuleID)
                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Notification Module references found", this.NotificationModuleID);
                args.FieldName = "NotificationModule";
                return false;
            }
            var checkTemplateDuplicate = (from b in args.NewDB.NotificationTemplateTable
                                          where (b.NotificationModuleID == this.NotificationModuleID)
                                          && b.StatusID == (int)StatusValue.Active
                                          select b).Count();

            if (checkTemplateDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Notification Template references found", this.NotificationModuleID);
                args.FieldName = "NotificationTemplate";
                return false;
            }
            var checkFieldDuplicate = (from b in args.NewDB.NotificationFieldTable
                                       where (b.NotificationModuleID == this.NotificationModuleID)
                                       && b.StatusID == (int)StatusValue.Active
                                       select b).Count();

            if (checkFieldDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Notification Field references found", this.NotificationModuleID);
                args.FieldName = "NotificationField";
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
        //                var checkDuplicate = (from b in args.NewDB.NotificationModuleTable
        //                                      where b.NotificationModule == this.NotificationModule

        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();
        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Notification Module already exists ", this.NotificationModuleID);
        //                    args.FieldName = "NotificationModule";
        //                    return false;
        //                }
        //                return true;
        //            }

        //        case EntityObjectState.Edit:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.NotificationModuleTable
        //                                      where (b.NotificationModule == this.NotificationModule && b.StatusID == (int)StatusValue.Active
        //                                      && b.NotificationModuleID != this.NotificationModuleID)
                                                   
        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Notification Module already exists ", this.NotificationModuleID);
        //                    args.FieldName = "NotificationModule";
        //                    return false;
        //                }

        //                return true;
        //            }

        //        case EntityObjectState.Delete:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.NotificationModuleFieldTable
        //                                      where (b.NotificationModuleID == this.NotificationModuleID)
        //                                      && b.StatusID==(int)StatusValue.Active
        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Notification Module references found", this.NotificationModuleID);
        //                    args.FieldName = "NotificationModule";
        //                    return false;
        //                }
        //                var checkTemplateDuplicate = (from b in args.NewDB.NotificationTemplateTable
        //                                      where (b.NotificationModuleID == this.NotificationModuleID)
        //                                      && b.StatusID == (int)StatusValue.Active
        //                                              select b).Count();

        //                if (checkTemplateDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Notification Template references found", this.NotificationModuleID);
        //                    args.FieldName = "NotificationTemplate";
        //                    return false;
        //                }
        //                var checkFieldDuplicate = (from b in args.NewDB.NotificationFieldTable
        //                                              where (b.NotificationModuleID == this.NotificationModuleID)
        //                                              && b.StatusID == (int)StatusValue.Active
        //                                              select b).Count();

        //                if (checkFieldDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Notification Field references found", this.NotificationModuleID);
        //                    args.FieldName = "NotificationField";
        //                    return false;
        //                }
        //                return true;
        //            }
        //    }
        //    return true;
        //}
    }
}
