using ACS.AMS.DAL;
using ACS.AMS.DAL.DBContext;

namespace ACS.AMS.WebApp.Models
{
    public class DynamicEntityModel : BaseEntityObject, IACSDBObject
    {
        public DynamicEntityModel()  
        {
            
        }

        bool IACSDBObject.DeleteObject()
        {
            throw new NotImplementedException();
        }

        IQueryable<BaseEntityObject> IACSDBObject.GetAllItems(AMSContext _db, bool includeInactiveItems)
        {
            throw new NotImplementedException();
        }

        IQueryable<BaseEntityObject> IACSDBObject.GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems)
        {
            throw new NotImplementedException();
        }

        BaseEntityObject IACSDBObject.GetItemByID(AMSContext _db, long itemID)
        {
            throw new NotImplementedException();
        }
    }
}
