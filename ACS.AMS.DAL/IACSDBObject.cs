using ACS.AMS.DAL.DBContext;
using ACS.AMS.DAL.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.AMS.DAL
{
    public interface IACSDBObject
    {
        IQueryable<BaseEntityObject> GetAllItems(AMSContext _db, bool includeInactiveItems = true);

        IQueryable<BaseEntityObject> GetAllUserItems(AMSContext _db, int userID, bool includeInactiveItems = true);

        BaseEntityObject GetItemByID(AMSContext _db, long itemID);

        bool DeleteObject();
    }
}
