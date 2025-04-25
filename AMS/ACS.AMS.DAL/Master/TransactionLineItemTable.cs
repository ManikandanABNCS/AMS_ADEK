using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.AMS.DAL.DBModel
{
    public partial class TransactionLineItemTable
    {
        public static IQueryable<TransactionLineItemTable> GetTransactionLineItems(AMSContext _db,int transactionID)
        {
            var result = (from b in _db.TransactionLineItemTable.Include("Asset").Include("Status").Include("TransferType")
                          where b.TransactionID == transactionID && b.StatusID == (int)StatusValue.WaitingForApproval
                          select b);
            return result;
        }
        public static IQueryable<TransactionLineItemTable> GetLineItems(AMSContext _db, int typeID)
        {
            var list = (from b in _db.TransactionTable where b.TransactionTypeID == typeID select b.TransactionID).ToList();
            var result = (from b in _db.TransactionLineItemTable.Include("Asset").Include("Status").Include("TransferType")
                          where  b.StatusID != (int)StatusValue.Deleted && list.Contains(b.TransactionID)
                          select b);
            return result;
        }
        public static List<int> GetUserBasedTransactionID(AMSContext _db,int userID, int? typeID)
        {
            List<int> userBasedLocation = PersonTable.GetUserBasedLocationList(_db, userID).Select(a => a.LocationID).ToList();
            IQueryable<TransactionTable> query = TransactionTable.GetAllItems(_db);
            List<int> list = new List<int>();
            if(typeID.HasValue)
            {
                list= query.Where(a=>a.TransactionTypeID==(int)typeID).Select(a=>a.TransactionID).ToList();
            }
            else
            {
                list = query.Select(a => a.TransactionID).ToList();
            }

            var result = (from b in _db.TransactionLineItemTable.Include("Asset").Include("Status").Include("TransferType")
                                                           join c in _db.LocationNewView  on b.FromLocationID equals c.LocationID
                                                           where b.StatusID != (int)StatusValue.Deleted && list.Contains(b.TransactionID)
                                                           &&  (userBasedLocation.Contains((int)c.Level2ID) || userBasedLocation.Contains((int)b.ToLocationID))
                                                           select b.TransactionID).ToList();

            return result;

            }
        public static IQueryable<TransactionLineItemTable> TransactionLineItems(AMSContext _db, int transactionID)
        {
            var result = (from b in _db.TransactionLineItemTable.Include("Asset").Include("Status").Include("ToLocation").Include("TransferType")
                          where b.TransactionID == transactionID 
                          select b);
            return result;
        }
        public static TransactionLineItemTable LineItems(AMSContext _db, int transactionID,int assetID)
        {
            var result = (from b in _db.TransactionLineItemTable.Include("Asset").Include("Status").Include("ToLocation").Include("TransferType")
                          where b.TransactionID == transactionID && b.AssetID==assetID
                          select b).FirstOrDefault();
            return result;
        }
        public static bool ValidateDisposalExcel(AMSContext db, int transactionID, List<int> assetID)
        {
            List<int> lineitem = TransactionLineItemTable.GetTransactionLineItems(db, transactionID).Select(a => a.AssetID).ToList();
            var missedIDS = lineitem.Except(assetID).ToList();
            if (missedIDS.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
