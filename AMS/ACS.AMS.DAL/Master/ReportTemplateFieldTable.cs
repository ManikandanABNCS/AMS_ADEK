using ACS.AMS.DAL.DBContext;
using ACS.AMS.DAL.Models.ReportTemplate;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.AMS.DAL.DBModel
{
    public partial class ReportTemplateFieldTable : BaseEntityObject
    {
        public static IQueryable<ReportTemplateFieldTable> GetAllReportTemplateFields(AMSContext db, int reportTemplateID)
        {
            var result = (from b in db.ReportTemplateFieldTable
                          where b.StatusID == (int)StatusValue.Active
                                && b.ReportTemplateID == reportTemplateID
                          select b);

            return result; // new SelectList(result, "Value", "Text");
        }
        public static IQueryable<ReportColumnModel> GetAllColumns(AMSContext db, int reportID)
        {
            var result = (from b in db.ReportFieldTable
                              //join c in db.UserReportDisplayColumnTables on b.ReportLineItemID equals c.ReportLineItemID
                          where b.ReportID == reportID
                          //&& b.StatusID == (int)StatusValue.Active
                          select new ReportColumnModel
                          {
                              ColumName = b.DisplayTitle,
                              Width = b.FieldWidth,
                              DisplayOrder = b.DisplayOrderID + ""
                          });

            return result;
        }
        public static SelectList GetAllReportFieldsForEdit(AMSContext db, int reportTemplateID)
        {
            var result = (from b in db.ReportTemplateFieldTable
                          where b.StatusID == (int)StatusValue.Active
                          && b.ReportTemplateID == reportTemplateID
                          orderby b.DisplayTitle
                          select new TextValuePair<string, int>
                          {
                              Text = b.DisplayTitle,
                              Value = b.ReportTemplateFieldID
                          }).ToList();

            return new SelectList(result, "Value", "Text");
        }

        public static List<ReportTemplateFieldTable> GetAllReportFields(AMSContext db, int reportTemplateID)
        {
            var result = (from b in db.ReportTemplateFieldTable
                          where b.StatusID == (int)StatusValue.Active
                          && b.ReportTemplateID == reportTemplateID
                          orderby b.DisplayTitle
                          select b).ToList();

            return result;
        }
       
        public static IQueryable<ReportGroupFieldTable> GetLineItemFromGroupTable(AMSContext db, int ReportID)
        {
            var result = from b in db.ReportGroupFieldTable
                         where b.ReportID == ReportID
                         select b;
            return result;
        }
        public static void DeleteExistingGrouplineItem(AMSContext db, IQueryable<ReportGroupFieldTable> lineItem)
        {
            foreach (ReportGroupFieldTable item in lineItem)
            {
                item.StatusID = (int)StatusValue.Deleted;
                //db.Remove(item);
                //db.SaveChanges();
            }
            db.SaveChanges();
        }

        public static void DeleteExistingFieldlineItem(AMSContext db, IQueryable<ReportFieldTable> lineItem)
        {
            foreach(ReportFieldTable item in lineItem)
            {
                item.StatusID = (int)StatusValue.Deleted;
               // db.Remove(item);
                //db.SaveChanges();
            }
            db.SaveChanges();
           
        }
        public static IQueryable<ReportFieldTable> GetColumnLineItemFromFieldTable(AMSContext db, int ReportID)
        {
            var result = from b in db.ReportFieldTable
                         where b.ReportID == ReportID
                         select b;
            return result;
        }
        public static SelectList GetAvailableGroupFieldsForEdit(AMSContext db, int reportID)
        {
            var reportTemplateID = (from b in db.ReportTable
                                    where b.ReportID == reportID
                                    select b.ReportTemplateID).FirstOrDefault();

            var result = (from b in ReportTemplateFieldTable.GetAllItems(db)
                          where !(from d in db.ReportGroupFieldTable
                                  where d.ReportID == reportID
                                    && d.StatusID != (int)StatusValue.Deleted && b.StatusID != (int)StatusValue.DeletedOLD
                                  select d.ReportTemplateFieldID).Contains(b.ReportTemplateFieldID)
                                  && b.ReportTemplateID == reportTemplateID
                          orderby b.DisplayTitle
                          select new TextValuePair<string, int>
                          {
                              Text = b.DisplayTitle,
                              Value = b.ReportTemplateFieldID
                          }).ToList();

            return new SelectList(result, "Value", "Text");
        }

        public static SelectList GetSelectedGroupFields(AMSContext db, int reportID)
        {
            var result = (from b in db.ReportTemplateFieldTable
                          join c in db.ReportGroupFieldTable on b.ReportTemplateFieldID equals c.ReportTemplateFieldID
                          where c.ReportID == reportID
                          orderby c.ReportTemplateFieldID
                          select new TextValuePair<string, int>
                          {
                              Text = b.DisplayTitle,
                              Value = b.ReportTemplateFieldID
                          }).ToList();

            return new SelectList(result, "Value", "Text");
        }

        public static SelectList GetAvailableReportFieldsForEdit(AMSContext model, int reportID)
        {
            var reportTemplateID = (from b in model.ReportTable
                                    where b.ReportID == reportID
                                    select b.ReportTemplateID).FirstOrDefault();
            var result = (from b in model.ReportTemplateFieldTable
                          where !(from d in model.ReportFieldTable
                                  where d.ReportID == reportID
                                  select d.ReportTemplateFieldID).Contains(b.ReportTemplateFieldID)
                                  && b.ReportTemplateID == reportTemplateID
                          orderby b.DisplayTitle
                          select new TextValuePair<string, int>
                          {
                              Text = b.DisplayTitle,
                              Value = b.ReportTemplateFieldID
                          }).ToList();

            return new SelectList(result, "Value", "Text"); ;
        }
        public static SelectList GetSelectedReportFieldDatas(AMSContext db, int ReportID)
        {
            var result = (from b in db.ReportTemplateFieldTable
                          join c in db.ReportFieldTable on b.ReportTemplateFieldID equals c.ReportTemplateFieldID
                          where c.ReportID == ReportID
                          orderby c.DisplayOrderID
                          select new TextValuePair<string, string>
                          {
                              Text = b.DisplayTitle,
                              Value = b.ReportTemplateFieldID + "@" + c.FieldWidth
                          }).ToList();

            return new SelectList(result, "Value", "Text");
        }
        public static SelectList GetAvailableFieldsForEdit(AMSContext model, int reportID)
        {
            var reportTemplateID = (from b in model.ReportTable
                                    where b.ReportID == reportID
                                    select b.ReportTemplateID).FirstOrDefault();

            var result = (from b in model.ReportTemplateFieldTable
                          where !(from d in model.ReportGroupFieldTable
                                  where d.ReportID == reportID
                                  select d.ReportTemplateFieldID).Contains(b.ReportTemplateFieldID) && b.ReportTemplateID == reportTemplateID
                          orderby b.DisplayTitle
                          select new TextValuePair<string, int>
                          {
                              Text = b.DisplayTitle,
                              Value = b.ReportTemplateFieldID
                          }).ToList();

            return new SelectList(result, "Value", "Text");
        }
    }
}