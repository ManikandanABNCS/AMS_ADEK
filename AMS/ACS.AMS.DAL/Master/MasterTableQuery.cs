using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ACS.AMS.DAL;
using Microsoft.EntityFrameworkCore;
using Castle.Components.DictionaryAdapter.Xml;
using ACS.AMS.DAL.DBContext;
using ACS.AMS.DAL;

namespace ACS.AMS.DAL.DBModel
{
    public partial class MasterTableQuery
    {

        public static void deleteAssetTransactionLineItemTable(AMSContext db, int assetTransactionID)
        {
            var result = (from b in db.AssetTransactionLineItemTable
                          where b.AssetTransactionID == assetTransactionID
                          select b);
            if (result.Count() > 0)
            {
                foreach (var res in result)
                {
                    res.StatusID = (int)StatusValue.Deleted;
                }
                db.SaveChanges();
                // db.Dispose();

            }
        }

        public static void deleteTransactionLineItemTable(AMSContext db, int transactionID)
        {
            var result = (from b in db.TransactionLineItemTable
                          where b.TransactionID == transactionID
                          select b);
            if (result.Count() > 0)
            {
                foreach (var res in result)
                {
                    res.StatusID = (int)StatusValue.Deleted;
                }
                db.SaveChanges();
                // db.Dispose();

            }
        }
        public static void deleteTransactionScheduleTable(AMSContext db, int transactionID)
        {
            var result = (from b in db.TransactionScheduleTable
                          where b.TransactionID == transactionID
                          select b);
            if (result.Count() > 0)
            {
                foreach (var res in result)
                {
                    res.StatusID = (int)StatusValue.Deleted;
                }
                db.SaveChanges();
                // db.Dispose();

            }
        }
    }
}