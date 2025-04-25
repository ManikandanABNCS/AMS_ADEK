using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ACS.DataAnnotations;
using ACS.AMS.DAL;
using ACS.AMS.DAL.DBContext;
using ACS.AMS.DAL.DBModel;
namespace ACS.AMS.DAL.Models.ReportTemplate
{
    public class ReportColumnModel 
    {
        public int ID { get; set; }
        [Required]
        [DisplayName("Column Name Required ")]
        public string ColumName { get; set; }
        public int ColumnID { get; set; }

        [Required]
        [DisplayName("Width Required ")]
        [Range(0.01, 50.00)]
        public decimal Width { get; set; }

        public bool isSumField { get; set; }
        public int ReportColumnLineItemID { get; set; }
        public string Type { get; set; }
        public int ReportPageUnitId { get; set; }
        public string ReportPageUnit { get; set; }
        [Required]
        [DisplayName("Display Order Required ")]
        public string DisplayOrder { get; set; }

        //public static void ClearCurrentModel(int pageID)

        //{
        //    SessionDataContainer.RemoveSessionObject<ReportColumnModel>(pageID);
        //}

        //public static ReportColumnModel GetCurrentModel(int pageID)
        //{
        //    ReportColumnModel model = SessionDataContainer.GetSessionObject<ReportColumnModel>(pageID);
        //    if (model == null)
        //    {
        //        model = new ReportColumnModel();
        //        SessionDataContainer.SetSessionObject(pageID, model);
        //    }
        //    return model;
        //}

    }
    public class ReportTemplateFieldsDB
    {
        public string FieldName { get; set; }
        public string DataType { get; set; }
    }
}
