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
    public partial class ImportFormatLineItemNewTable
    {
        public static IQueryable<ImportFormatLineItemNewTable> GetImportFormatItems(AMSContext db, int ImportFormatID)
        {
            return from b in db.ImportFormatLineItemNewTable
                   where b.ImportFormatID == ImportFormatID 
                   select b;
        }
        public static void DeleteExistingFieldlineItem(AMSContext db, IQueryable<ImportFormatLineItemNewTable> lineItem)
        {
            foreach (ImportFormatLineItemNewTable item in lineItem)
            {
               
                db.Remove(item);
                //db.SaveChanges();
            }

        }
        public static IQueryable<ImportTemplateModel> GetImportMasterForImportFormat(AMSContext db, int importFormatID, int appPageID)
        {
            var _result = (from b in db.ImportFormatLineItemNewTable.Include("ImportFormat").Include("ImportFormat.Entity")
                           where b.ImportFormat.ImportFormatID == importFormatID && b.ImportFormat.EntityID == appPageID 
                           orderby b.DisplayOrderID ascending
                           select b);//.ToList();

            IQueryable<ImportTemplateModel> result = Enumerable.Empty<ImportTemplateModel>().AsQueryable();

            if (_result.Count() > 0)
            {
                // List<int> importID = _result.Select(a => a.ImportTemplateID).Distinct().ToList();
                //var  result1 = ImportTemplateTable.GetAllImportTemplates().Where(x=> importID.Contains(x.ImportTemplateID));
                var result1 = (from b in _result
                               select new
                               {
                                   ImportField = b.ImportTemplate.ImportField,
                                   AppPagesID = b.ImportTemplate.Entity.EntityName,
                                   IsForeignKey = b.ImportTemplate.IsForeignKey,
                                   IsMandatory = b.ImportTemplate.IsMandatory,
                                  // FieldFormat = b.ImportTemplate.FieldType.FieldTypeDesc,
                                   DataSize = b.ImportTemplate.DataSize,
                                   IsUnique = b.ImportTemplate.IsUnique,
                                   DispalyOrderID = b.DisplayOrderID,
                                  // FieldTypeID = b.ImportTemplate.FieldTypeID,
                                   ReferenceEntityID = b.ImportTemplate.ReferenceEntityID,
                                   ReferenceEntityName = b.ImportTemplate.Entity.EntityName
                               }).AsEnumerable().Select(x => new ImportTemplateModel
                               {
                                   ImportField = x.ImportField,
                                   EntityName = x.AppPagesID,
                                   IsForeignKey = x.IsForeignKey,
                                   IsMandatory = x.IsMandatory,
                                   DataSize = x.DataSize,
                                   IsUnique = x.IsUnique,
                                   DispalyOrderID = x.DispalyOrderID,
                                 //  FieldTypeID = x.FieldTypeID,
                                   ReferenceEntityID = x.ReferenceEntityID,
                                   ReferenceEntityName = x.ReferenceEntityName,
                                   LanguageImportField = Language.GetString(x.ImportField)
                               });

                return result1.AsQueryable();

            }
            return result;
        }
        public static IQueryable<ImportFormatLineItemNewTable> GetColumns(AMSContext db, int ImportFormatID)
        {
            var result = (from b in db.ImportFormatLineItemNewTable.Include("ImportFormat")
                          where b.ImportFormatID == ImportFormatID && b.ImportFormat.StatusID == (int)StatusValue.Active
                          //&& b.IsDynamicColumn == false
                          orderby b.DisplayOrderID
                          select b);

           

            return result;
        }

        public static bool ValidateAsset(AMSContext db, int importFormatID, int appPageID)
        {
            var _result = (from b in db.ImportFormatNewTable
                           where b.ImportFormatID == importFormatID && b.EntityID == appPageID
                           select b).FirstOrDefault();
            if (_result != null)
            {
                if (_result.ImportTemplateTypeID == 1)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public static IQueryable<string> MergeColumnName(IQueryable<ImportFormatLineItemNewTable> item1)
        {
            var result = (from b in item1 orderby b.DisplayOrderID select new { ImportField = b.ImportTemplate.ImportField, DispalyOrderID = b.DisplayOrderID });
           
            var _list = (from c in result orderby c.DispalyOrderID select c.ImportField);
            return _list;
        }


        public static IQueryable<ImportTemplateModel> GetImportFinalFormat(AMSContext db,  int appPageID)
        {
            var _result = (from b in db.ImportTemplateNewTable.Include("Entity")
                           where  b.EntityID == appPageID
                           orderby b.DispalyOrderID ascending
                           select b);//.ToList();

            IQueryable<ImportTemplateModel> result = Enumerable.Empty<ImportTemplateModel>().AsQueryable();

            if (_result.Count() > 0)
            {
               
                var result1 = (from b in _result
                               select new
                               {
                                   ImportField = b.ImportField,
                                   AppPagesID = b.Entity.EntityName,
                                   IsForeignKey = b.IsForeignKey,
                                   IsMandatory = b.IsMandatory,
                                   // FieldFormat = b.ImportTemplate.FieldType.FieldTypeDesc,
                                   DataSize = b.DataSize,
                                   IsUnique = b.IsUnique,
                                   DispalyOrderID = b.DispalyOrderID,
                                   // FieldTypeID = b.ImportTemplate.FieldTypeID,
                                   ReferenceEntityID = b.ReferenceEntityID,
                                   ReferenceEntityName = b.Entity.EntityName
                               }).AsEnumerable().Select(x => new ImportTemplateModel
                               {
                                   ImportField = x.ImportField,
                                   EntityName = x.AppPagesID,
                                   IsForeignKey = x.IsForeignKey,
                                   IsMandatory = x.IsMandatory,
                                   DataSize = x.DataSize,
                                   IsUnique = x.IsUnique,
                                   DispalyOrderID = x.DispalyOrderID,
                                   //  FieldTypeID = x.FieldTypeID,
                                   ReferenceEntityID = x.ReferenceEntityID,
                                   ReferenceEntityName = x.ReferenceEntityName,
                                   LanguageImportField = Language.GetString(x.ImportField)
                               });

                return result1.AsQueryable();

            }
            return result;
        }
    }
}
