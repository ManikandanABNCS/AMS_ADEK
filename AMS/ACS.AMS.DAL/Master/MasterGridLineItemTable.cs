using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Data;
using ACS.AMS.DAL.DBContext;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel
{
   
    public partial class MasterGridNewLineItemTable : BaseEntityObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        ///        
        public static IQueryable<MasterGridNewLineItemTable> GetAllFields(AMSContext entityObject,string masterGridName, string tableName = null)
        {
           // AMSContext entityObject = AMSContext.CreateNewContext();

            var result = ACS.AMS.DAL.DBModel.UserGridNewColumnTable.GetUserColumns(masterGridName);
            IQueryable<MasterGridNewLineItemTable> Value = (from b in entityObject.MasterGridNewLineItemTable
                                                         where b.MasterGrid.MasterGridName == masterGridName // && b.IsGridColumn == true
                                                         orderby b.OrderID
                                                         select b);
            return Value;
            //List<MasterGridNewLineItemTable> gridItemList = new List<MasterGridNewLineItemTable>();
            //foreach (var d in Value)
            //{
            //    MasterGridNewLineItemTable gridItem = new MasterGridNewLineItemTable();
            //    gridItem.MasterGridLineItemID = d.MasterGridLineItemID;
            //    gridItem.MasterGridID = d.MasterGridID;
            //    gridItem.OrderID = d.OrderID;
            //    //gridItem.IsGridColumn = result.Where(e => e.TargetIndexLineItemID == d.TargetIndexLineItemID).Count() > 0;
            //    gridItem.DisplayName = d.DisplayName;
            //    gridItem.FieldName = d.FieldName;
            //    gridItem.Width = d.Width;
            //    //gridItem.IsLanguageTwoColumn = d.IsLanguageTwoColumn;
            //    gridItem.Format = d.Format;
            //    gridItemList.Add(gridItem);
            //}
            //return gridItemList.AsQueryable();
        }

        public static IQueryable<MasterGridNewLineItemTable> GetMasterGridLineItems(AMSContext db, string ControllerName)
        {
            return (from b in db.MasterGridNewLineItemTable
                    where b.MasterGrid.MasterGridName == ControllerName
                    select b).AsQueryable();
        }

        public static SelectList GetAvailableGridColumns( string controllerName)
        {
            AMSContext _db = AMSContext.CreateNewContext();
            IQueryable<MasterGridNewLineItemTable> allFields1 = MasterGridNewLineItemTable.GetAllFields(_db, controllerName);
            //commented
            var userSelectFields = ACS.AMS.DAL.DBModel.UserGridNewColumnTable.GetUserGridDetails(_db, controllerName);//.Select(a => a.MasterGridLineItemID).ToList();
            var allFields = (from b in allFields1 join c in userSelectFields on b.MasterGridLineItemID equals c.MasterGridLineItemID
                             into jk from subgroup in jk.DefaultIfEmpty()
                             where subgroup == null
                             select new { b.DisplayName, b.MasterGridLineItemID, b.FieldName });
            //var allFields = allFields1.Where(a => !userSelectFields.Contains(a.MasterGridLineItemID));


            var result = (from b in allFields
                          orderby b.FieldName
                          select new TextValuePair<string, string>
                          {
                             
                              Text = b.DisplayName,
                              Value = b.MasterGridLineItemID + ""
                          }).ToList();
            return new SelectList(result, "Value", "Text"); ;
        }
        public static SelectList GetSelectedGridColumns( string controllerName)
        {
            AMSContext _db = AMSContext.CreateNewContext();
            var value = ACS.AMS.DAL.DBModel.UserGridNewColumnTable.GetUserGridColumns(_db,controllerName);

            var result = (from b in value
                          select new TextValuePair<string, string>
                          {
                              Text = Language.GetString(b.MasterGridLineItem.DisplayName),
                              //Text = b.MasterGridLineItem.DisplayName,
                              Value = b.MasterGridLineItemID + ""
                          }).ToList();

           
            return new SelectList(result, "Value", "Text");
        }

    }
}
