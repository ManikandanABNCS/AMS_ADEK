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
    public partial class ImportTemplateNewTable
    {
        public static SelectList GetAvailableFields(AMSContext model, int entityID, int type)
        {
            var result = (from b in model.ImportTemplateNewTable
                          where b.EntityID == entityID && b.IsDisplay == true && b.IsMandatory == false && b.ImportTemplateTypeID == type && !b.ImportField.Contains("Attribute")
                          select new
                          {
                              b.ImportField,
                              b.ImportTemplateID
                          }).AsEnumerable().Select(x => new
                          TextValuePair<string, int>
                          {
                              Text = Language.GetString(x.ImportField),
                              Value = x.ImportTemplateID
                          }).ToList();

            return new SelectList(result, "Value", "Text");
        }
        public static SelectList GetMandatoryFields(AMSContext model, int entityID, int type)
        {
            IQueryable<ImportTemplateNewTable> result = (from b in model.ImportTemplateNewTable
                                                      where b.EntityID == entityID && b.IsMandatory == true && b.IsDisplay == true && b.ImportTemplateTypeID == type
                                                      select b);
            //if ((AppConfigurationManager.GetValue<bool>(AppConfigurationManager.MasterCodeAutoGenerate)) && ((int)EntitiesValue.AssetTable != appPagesID) && type == 1)
            //{

            //    var code = result.Where(a => a.IsForeignKey == false && a.ImportField.Contains("Code")).Select(a => a.ImportField).FirstOrDefault();
            //    result = result.Where(a => !a.ImportField.Contains(code));
            //}

            var list = (from b in result
                        where !b.ImportField.Contains("Attribute")
                        select new
                        {
                            b.ImportField,
                            b.ImportTemplateID
                        }).AsEnumerable().Select(x => new
                        TextValuePair<string, int>
                        {
                            Text = Language.GetString(x.ImportField),
                            Value = x.ImportTemplateID
                        }).ToList();


            return new SelectList(list, "Value", "Text"); ;
        }
        public static int GetMandatoryCount(AMSContext model, string controlIDs, string type, int entityName)
        {
            string[] ColumnID = controlIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<int> ids = new List<int>();
            List<int> dynIds = new List<int>();
            foreach (string str in ColumnID)
            {
                string dynamicColumn = "Dyn_";
                if (dynamicColumn.Any(str.Contains))
                {
                    dynIds.Add(int.Parse(str.Remove(0, 4)));
                }
                else
                {
                    ids.Add(int.Parse(str));
                }
            }
            int ImportTemplateTypeID = int.Parse(type);
            var result = (from b in model.ImportTemplateNewTable
                          where b.IsMandatory == true && b.IsDisplay == true & b.ImportTemplateTypeID == ImportTemplateTypeID
                          select b);

            var _result1 = result.Where(a => ids.Contains(a.ImportTemplateID)).Count();

            //var dynamic = (from b in model.AttributeDefinitionTables
            //               where b.IsMandatory == true //&& b.ImportFormatTable.ImportTemplateTypeID == ImportTemplateTypeID 
            //               && b.DynamicColumnRequiredEntityID == apppageID && b.StatusID == (int)StatusValue.Active
            //               select b);

            //var _result2 = dynamic.Where(a => dynIds.Contains(a.AttributeDefinitionID)).Count();
            //  var _result = _result1 + _result2;
            var _result = _result1;
            return _result;
        }
        public static SelectList GetAvailableFieldsForEdit(AMSContext model, int excelID, int entityID, int? type)
        {
            IQueryable<ImportTemplateNewTable> result =(from b in model.ImportTemplateNewTable 
         
                                                      where !(from d in model.ImportFormatLineItemNewTable
                                                              where d.ImportFormatID == excelID //&& d.IsDynamicColumn == false
                                                              select d.ImportTemplateID).Contains(b.ImportTemplateID) && b.EntityID == entityID
                                                              && b.IsDisplay == true && b.ImportTemplateTypeID == type
                                                      select b);
               
            var list = (from b in result
                        select new
                        {
                            b.ImportField,
                            b.ImportTemplateID
                        }).AsEnumerable().Select(x => new
                        TextValuePair<string, string>
                        {
                            Text = Language.GetString(x.ImportField),
                            Value = x.ImportTemplateID + ""
                        }).ToList();

            return new SelectList(list, "Value", "Text");
        }
        public static SelectList GetSelectedDatas(AMSContext db, int importFormatID)
        {
            var result = (from c in db.ImportFormatLineItemNewTable.Include("ImportTemplate")
                          where c.ImportFormatID == importFormatID //&& c.IsDynamicColumn == false
                          orderby c.DisplayOrderID
                          select new
                          {
                              DisplayTitle = c.ImportTemplate.ImportField,
                              AttributeDefinitionID = c.ImportTemplate.ImportTemplateID + "",
                              DisplayOrderID = c.DisplayOrderID,
                              IsMandatory = c.ImportTemplate.IsMandatory
                          }).ToList();

            //var resultdynamic = (from c in db.ImportAttributeDefinitionTables.Include("AttributeDefinitionTable")
            //                     where c.ImportFormatID == importFormatID
            //                     orderby c.DisplayOrderID
            //                     select new
            //                     {
            //                         DisplayTitle = c.AttributeDefinitionTable.DisplayTitle,
            //                         AttributeDefinitionID = "Dyn_" + c.AttributeDefinitionTable.AttributeDefinitionID,
            //                         DisplayOrderID = c.DisplayOrderID,
            //                         IsMandatory = (c.IsMandatory.HasValue) ? c.IsMandatory.Value : false
            //                     }).ToList();

            //var importList = result.Concat(resultdynamic).OrderBy(a => a.DisplayOrderID).ToList();
            var importList = result.OrderBy(a => a.DisplayOrderID).ToList();
            var returnList = importList.Select(x => new TextValuePair<string, string>
            {
                Text = Language.GetString(x.DisplayTitle) + (x.IsMandatory ? "*" : ""),
                Value = x.AttributeDefinitionID + ""
            }).ToList();

           
                return new SelectList(returnList, "Value", "Text");
            
        }
        public static IQueryable<ImportTemplateNewTable> GetImportTemplateFields(AMSContext db, int appID, int type)
        {
            return from b in db.ImportTemplateNewTable.Include("ImportTemplateType").Include("Entity")
                   where b.EntityID == appID && b.ImportTemplateTypeID == type
                   select b;
        }
    }
}
