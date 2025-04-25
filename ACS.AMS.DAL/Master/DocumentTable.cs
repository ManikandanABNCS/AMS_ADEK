using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ACS.AMS.DAL.DBModel
{
    public partial class DocumentTable
    {
        public static IQueryable<DocumentTable> GetDocumentDetails(AMSContext context, int objectKeyID, string types)
        {
            if (string.Compare(types, "AssetTransfer") == 0)
            {
                var result = (from b in context.DocumentTable where b.ObjectKeyID == objectKeyID && b.TransactionType == types select b);
                return result;
            }
            if (string.Compare(types, "AssetTransferApproval") == 0)
            {
                var apprvalids = ApprovalHistoryTable.GetItem(context, objectKeyID);
                var ids = (from b in context.ApprovalHistoryTable 
                           where (b.ApproveModuleID == (int)ApproveModuleValue.AssetTransfer || b.ApproveModuleID == (int)ApproveModuleValue.InternalAssetTransfer)
                           && b.TransactionID == apprvalids.TransactionID && b.StatusID == (int)StatusValue.Active select b.ApprovalHistoryID);

                var result = (from b in context.DocumentTable where ids.Contains(b.ObjectKeyID) && b.TransactionType == types select b);
                return result;


            }
            if (string.Compare(types, "AssetRetirement") == 0)
            {
                var result = (from b in context.DocumentTable where b.ObjectKeyID == objectKeyID && b.TransactionType == types select b);
                return result;
                
            }
            if (string.Compare(types, "AssetAddition") == 0)
            {
                var result = (from b in context.DocumentTable where b.ObjectKeyID == objectKeyID && b.TransactionType == types select b);
                return result;

            }
            if (string.Compare(types, "AssetMaintenanceRequest") == 0)
            {
                var result = (from b in context.DocumentTable where b.ObjectKeyID == objectKeyID && b.TransactionType == types select b);
                return result;

            }
            if (string.Compare(types, "AMCSchedule") == 0)
            {
                var result = (from b in context.DocumentTable where b.ObjectKeyID == objectKeyID && b.TransactionType == types select b);
                return result;

            }
            if (string.Compare(types, "AssetRetirementApproval") == 0)
            {
                var apprvalids = ApprovalHistoryTable.GetItem(context, objectKeyID);
                var ids = (from b in context.ApprovalHistoryTable
                           where b.ApproveModuleID == (int)ApproveModuleValue.AssetRetirement
                           && b.TransactionID == apprvalids.TransactionID && b.StatusID == (int)StatusValue.Active
                           select b.ApprovalHistoryID);

                var result = (from b in context.DocumentTable where ids.Contains(b.ObjectKeyID) && b.TransactionType == types select b);
                return result;
            }
            if (string.Compare(types, "AssetAdditionApproval") == 0)
            {
                var apprvalids = ApprovalHistoryTable.GetItem(context, objectKeyID);
                var ids = (from b in context.ApprovalHistoryTable
                           where b.ApproveModuleID == (int)ApproveModuleValue.AssetAddition
                           && b.TransactionID == apprvalids.TransactionID && b.StatusID == (int)StatusValue.Active
                           select b.ApprovalHistoryID);

                var result = (from b in context.DocumentTable where ids.Contains(b.ObjectKeyID) && b.TransactionType == types select b);
                return result;
            }
            if (string.Compare(types, "AssetMaintenanceRequestApproval") == 0)
            {
                var apprvalids = ApprovalHistoryTable.GetItem(context, objectKeyID);
                var ids = (from b in context.ApprovalHistoryTable
                           where b.ApproveModuleID == (int)ApproveModuleValue.AssetMaintenanceRequest
                           && b.TransactionID == apprvalids.TransactionID && b.StatusID == (int)StatusValue.Active
                           select b.ApprovalHistoryID);

                var result = (from b in context.DocumentTable where ids.Contains(b.ObjectKeyID) && b.TransactionType == types select b);
                return result;
            }
            if (string.Compare(types, "AMCScheduleApproval") == 0)
            {
                var apprvalids = ApprovalHistoryTable.GetItem(context, objectKeyID);
                var ids = (from b in context.ApprovalHistoryTable
                           where b.ApproveModuleID == (int)ApproveModuleValue.AMCSchedule
                           && b.TransactionID == apprvalids.TransactionID && b.StatusID == (int)StatusValue.Active
                           select b.ApprovalHistoryID);

                var result = (from b in context.DocumentTable where ids.Contains(b.ObjectKeyID) && b.TransactionType == types select b);
                return result;
            }
            return null;
        }
        public static void deleteDocument(AMSContext db, int tranID,string type)
        {
            var result = (from b in db.DocumentTable
                          where b.ObjectKeyID==tranID && b.TransactionType == type
                          select b);
            if (result.Count() > 0)
            {
                foreach (var res in result)
                {
                    res.StatusID = (int)StatusValue.Deleted;
                    //db.DocumentTable.Remove(res);
                }
                db.SaveChanges();

                // db.Dispose();

            }

        }
    }
}
