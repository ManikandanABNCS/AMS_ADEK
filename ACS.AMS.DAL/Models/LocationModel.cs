using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACS.AMS.DAL.DBContext;
using ACS.AMS.DAL.DBModel;
namespace ACS.AMS.DAL.Models
{
    public class LocationModel
    {
        public int id { get; set; }
        public string Name { get; set; }
        public bool hasChildren { get; set; }
        public string spriteCssClass { get; set; }
        public IList<LocationTable> ChildItems { get; set; }
        public IList<LocationDescriptionModel> Description { get; set; }

        public static List<LocationModel> GetLocationItems(string masters)
        {
            AMSContext entityObject = AMSContext.CreateNewContext();
            List<LocationTable> item;
            List<LocationDescriptionTable> lineitem;
            item = LocationTable.GetAllLocations(entityObject, true).ToList();
           // lineitem = LocationDescriptionTable.GetAllLocationDescriptions(entityObject).ToList();
            List<LocationDescriptionModel> desc = new List<LocationDescriptionModel>();
            desc = LocationDescriptionTable.GetItemsForViewing(item.AsQueryable());
           
            List<LocationModel> list = new List<LocationModel>();
            //LocationModel dummy = new LocationModel();
            //dummy.id = 0;
            //dummy.Name = "Location";
            //dummy.hasChildren = false;
            //dummy.spriteCssClass = "html";
            //dummy.ChildItems = new List<LocationTable>();
            //dummy.Description = new List<LocationDescriptionModel>();
            //list.Add(dummy);

            var result = item.Where(x => x.ParentLocationID == null).Select(cate => new LocationModel
            {
                id = desc.Where(a => a.LocationID == cate.LocationID).Select(a => a.LocationID).FirstOrDefault(),//employee.LocationID,
                Name = desc.Where(a => a.LocationID == cate.LocationID).Select(a => a.LocationName).FirstOrDefault(),
                hasChildren = cate.InverseParentLocation.Any(x => x.StatusID != (int)StatusValue.Deleted && x.StatusID!=(int)StatusValue.DeletedOLD ),
                spriteCssClass = cate.InverseParentLocation.Count(x => x.StatusID != (int)StatusValue.Deleted && x.StatusID != (int)StatusValue.DeletedOLD) > 0 ? "rootfolder" : "html",
                 ChildItems = cate.InverseParentLocation.Where(x => x.StatusID != (int)StatusValue.Deleted && x.StatusID != (int)StatusValue.DeletedOLD).ToList(),
                Description = desc
            }).OrderBy(x => x.Name).ToList();


            var res = list.Concat(result).ToList();
            return res;
        }

    }
    public class LocationDescriptionModel
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; }
    }
}
