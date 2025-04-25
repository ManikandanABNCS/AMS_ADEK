using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACS.AMS.DAL.DBContext;
using ACS.AMS.DAL.DBModel;

namespace ACS.AMS.DAL.Models
{
    public class CategoryModel
    {
        public int id { get; set; }
        public string Name { get; set; }
        public bool hasChildren { get; set; }
        public string spriteCssClass { get; set; }
        public IList<CategoryTable> ChildItems { get; set; }
        public IList<CategoryDescriptionModel> Description { get; set; }

        public static List<CategoryModel> GetCategoryItems(string masters)
        {
            AMSContext entityObject = AMSContext.CreateNewContext();

            List<CategoryTable> item;
            List<CategoryDescriptionTable> lineitem;
            item = CategoryTable.GetAllCategorys(entityObject, true).ToList();
            // lineitem = CategoryDescriptionTable.GetAllCategoryDescriptions(entityObject).ToList();
            List<CategoryDescriptionModel> desc = new List<CategoryDescriptionModel>();
            desc = CategoryDescriptionTable.GetItemsForViewing(item.AsQueryable());

            List<CategoryModel> list = new List<CategoryModel>();
            
            var result = item.Where(x => x.ParentCategoryID == null).Select(cate => new CategoryModel
            {
                id = desc.Where(a => a.CategoryID == cate.CategoryID).Select(a => a.CategoryID).FirstOrDefault(),//employee.LocationID,
                Name = desc.Where(a => a.CategoryID == cate.CategoryID).Select(a => a.CategoryName).FirstOrDefault(),
                hasChildren = cate.InverseParentCategory.Any(x => x.StatusID != (int)StatusValue.Deleted && x.StatusID != (int)StatusValue.DeletedOLD),
                spriteCssClass = cate.InverseParentCategory.Count(x => x.StatusID != (int)StatusValue.Deleted && x.StatusID != (int)StatusValue.DeletedOLD) > 0 ? "rootfolder" : "html",
                ChildItems = cate.InverseParentCategory.Where(x => x.StatusID != (int)StatusValue.Deleted && x.StatusID != (int)StatusValue.DeletedOLD).ToList(),
                Description = desc
            }).OrderBy(x => x.Name).ToList();

            var res = list.Concat(result).ToList();

            return res;
        }
    }

    public class CategoryDescriptionModel
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}
