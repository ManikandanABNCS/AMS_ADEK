using ACS.AMS.DAL.DBContext;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.AMS.DAL.DBModel
{
    public partial class ImportFormatNewTable
    {
        public static ImportFormatNewTable GetImportFormat(AMSContext db, int importFormatID)
        {
            var result = (from b in db.ImportFormatNewTable.Include("ImportTemplateType").Include("Status").Include("Entity")

                          where b.StatusID != (int)StatusValue.Deleted && b.ImportFormatID == importFormatID
                          select b).FirstOrDefault();
            return result;
        }
        public static IQueryable<ImportFormatNewTable> GetListOfFormatsForEntity(AMSContext db, int? id, string text = null,int? userID=null)
        {
            IQueryable<ImportFormatNewTable> result = (from b in db.ImportFormatNewTable.Include("ImportTemplateType").Include("Status").Include("Entity")
                                                    where b.StatusID == (int)StatusValue.Active 
                                                    select b);
            if (id.HasValue)
                result = result.Where(a => a.EntityID == id);
            if (!string.IsNullOrEmpty(text))
                result = result.Where(a => a.TamplateName.ToUpper().Contains(text.ToUpper()));
            var details = TransactionTable.GetUserImportTemplate(db, (int)userID);
          
            var ids = details.Result.AsQueryable();
            var formatId = (from b in ids select b.ImportFormatID).ToList();
            result = result.Where(a => formatId.Contains(a.ImportFormatID));
            
            return result;
        }
        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.ImportFormatNewTable
                                  where b.ImportTemplateTypeID == this.ImportTemplateTypeID
                                  && b.TamplateName == this.TamplateName
                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();
            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Tamplate Name already exists ", this.TamplateName);
                args.FieldName = "TamplateName";
                return false;
            }
            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.ImportFormatNewTable
                                  where (b.ImportTemplateTypeID == this.ImportTemplateTypeID && b.StatusID == (int)StatusValue.Active
                                    && b.TamplateName == this.TamplateName
                                  && b.ImportFormatID != this.ImportFormatID)

                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Tamplate Name already exists ", this.TamplateName);
                args.FieldName = "TamplateName";
                return false;
            }
            return true;
        }
        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            return true;
        }
        //internal override bool ValidateObject(ValidateEventArgs<AMSContext> args)
        //{
        //    switch (args.State)
        //    {
        //        case EntityObjectState.New:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.ImportFormatNewTable
        //                                      where b.ImportTemplateTypeID == this.ImportTemplateTypeID
        //                                      && b.TamplateName==this.TamplateName
        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();
        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Tamplate Name already exists ", this.TamplateName);
        //                    args.FieldName = "TamplateName";
        //                    return false;
        //                }
        //                return true;
        //            }

        //        case EntityObjectState.Edit:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.ImportFormatNewTable
        //                                      where (b.ImportTemplateTypeID == this.ImportTemplateTypeID && b.StatusID == (int)StatusValue.Active
        //                                        && b.TamplateName == this.TamplateName
        //                                      && b.ImportFormatID != this.ImportFormatID)

        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Tamplate Name already exists ", this.TamplateName);
        //                    args.FieldName = "TamplateName";
        //                    return false;
        //                }

        //                return true;
        //            }

        //        case EntityObjectState.Delete:
        //            {
                        

        //                return true;
        //            }
        //    }
        //    return true;
        //}
    }
}
