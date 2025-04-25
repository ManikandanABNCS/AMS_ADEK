using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel
{    
    public partial class UserGridNewColumnTable :BaseEntityObject
    {
        public static void DeleteExistingColumnForUser(AMSContext db, int masterGridID, int[] masterGridLineItemID)
        {
            var result = (from b in db.UserGridNewColumnTable
                          where b.MasterGridID == masterGridID && b.UserID == db.CurrentUserID && !masterGridLineItemID.Contains(b.MasterGridLineItemID)
                          select b);

            if (result.Count() > 0)
            {
                db.Remove(result);
                db.SaveChanges();
            }
        }

        public static void DeleteExistingColumnForUser(AMSContext db, int tragetIndexID)
        {
            var result = (from b in db.UserGridNewColumnTable
                          where b.MasterGridID == tragetIndexID && b.UserID == db.CurrentUserID
                          select b).ToList();
            if (result.Count() > 0)
            {
                foreach (var UserGridNewColumnTable in result)
                {
                    db.UserGridNewColumnTable.Remove(UserGridNewColumnTable);
                }
                db.SaveChanges();
            }
        }
        public static IQueryable<UserGridNewColumnTable> GetUserGridColumns(AMSContext db ,string indexName)
        {
           // AMSContext db = AMSContext.CreateNewContext();
            IEnumerable<UserGridNewColumnTable> result = (from b in db.UserGridNewColumnTable.Include(c => c.MasterGridLineItem).Include(c => c.MasterGrid)
                                                       where b.MasterGrid.MasterGridName == indexName && b.UserID == SessionUserHelper.UserID
                                                 orderby b.OrderID
                                                 select b);

            return result.OrderBy(a => a.OrderID).AsQueryable();
        }

        public static List<UserGridNewColumnTable> GetUserColumns(string indexName)
        {
            AMSContext entityObject = AMSContext.CreateNewContext();
            IEnumerable<UserGridNewColumnTable> result = (from b in entityObject.UserGridNewColumnTable.Include(c=>c.MasterGridLineItem).Include(c => c.MasterGrid)
                          where b.MasterGrid.MasterGridName == indexName 
                          && b.UserID == SessionUserHelper.UserID
                          orderby b.OrderID
                          select b).ToList();

            if (result.Count() == 0)
            {
                result = (from b in entityObject.MasterGridNewLineItemTable
                               where b.MasterGrid.MasterGridName == indexName && b.IsDefault == true
                               select new
                               {
                                   b.MasterGridLineItemID,
                                   b.MasterGridID,
                                   b,
                                   b.MasterGrid
                               }).AsEnumerable().Select(x => new UserGridNewColumnTable
                               {
                                   MasterGridLineItemID = x.MasterGridLineItemID,
                                   MasterGridID = x.MasterGridID,
                                   MasterGridLineItem = x.b,
                                   MasterGrid = x.MasterGrid,
                                   OrderID = x.b.OrderID
                               });

                //               select b).ToList();

                //result = (from b in result1
                //          select new UserGridNewColumnTable
                //          {
                //              MasterGridLineItem = b
                //          }).ToList();
            }

            return result.OrderBy(a => a.OrderID).ToList();
        }

        public static bool CheckExistingColumnForUser(AMSContext db, int masterGridID, int masterGridLineItemID)
        {

            var result = (from b in db.UserGridNewColumnTable
                          where b.MasterGridID == masterGridID && b.UserID == db.CurrentUserID && b.MasterGridLineItemID == masterGridLineItemID
                          select b);


            if (result.Count() > 0)
            {
                return false;

            }
            else
            {
                return true;
            }
        }

        public static IQueryable<UserGridNewColumnTable> GetUserGridDetails(AMSContext entityObject,string indexName)
        {
           // AMSContext entityObject = AMSContext.CreateNewContext();
            return (from b in entityObject.UserGridNewColumnTable
                    where b.MasterGrid.MasterGridName == indexName && b.UserID == SessionUserHelper.UserID
                    select b);
        }
    }
}