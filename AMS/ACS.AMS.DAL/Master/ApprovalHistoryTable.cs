using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ACS.AMS.DAL.DBModel
{
    public partial class ApprovalHistoryTable
    {
        public static ApprovalHistoryTable GetWorkflowID(AMSContext db, int requestID, int approveModuleID, int? approvalRoleID = null, int? objectkeyID1 = null)
        {
            IQueryable<ApprovalHistoryTable> docs = (from b in db.ApprovalHistoryTable where b.ApprovalHistoryID == requestID && (b.StatusID == (int)StatusValue.WaitingForApproval || b.StatusID == (int)StatusValue.ReApproval) select b);

            if (approvalRoleID.HasValue)
            {
                docs = docs.Where(a => a.ApprovalRoleID == approvalRoleID);
            }
            if (objectkeyID1.HasValue)
            {
                docs = docs.Where(a => a.ObjectKeyID1 == objectkeyID1);
            }


            return docs.OrderBy(a => a.OrderNo).FirstOrDefault();
        }

        public static int GetWorkflowDetailsworkflowMax(AMSContext db, int requestID)
        {
            var obj = (from b in db.ApprovalHistoryTable
                       where b.ApprovalHistoryID == requestID
                       select b).FirstOrDefault();
            var maxObjID = (from b in db.ApprovalHistoryTable where b.ApproveModuleID == obj.ApproveModuleID && b.TransactionID == obj.TransactionID select b.ApprovalHistoryID).Max();
            return maxObjID;
        }
        public static ApprovalHistoryTable LastLevelData(AMSContext db,  int type, int transationID)
        {
            var result = (from b in db.ApprovalHistoryTable
                          where b.TransactionID == transationID && b.ApproveModuleID == type  && b.StatusID == (int)StatusValue.WaitingForApproval
                          select b
                         ).FirstOrDefault();
            return result;
        }

        public static List<ApprovalHistoryTable> SameLevelUserList(AMSContext db, int approvalHistoryID, int type, int transationID)
        {
            var list = (from b in db.ApprovalHistoryTable
                        where b.TransactionID == transationID && b.ApproveModuleID==type
                           && b.StatusID == (int)StatusValue.WaitingForApproval && b.ApprovalHistoryID!=approvalHistoryID
                            //&& b.appovehis != approvalHistoryID
                        select b).ToList();
            return list;
        }
        public static List<ApprovalHistoryTable> SameLevelApprovedUserList(AMSContext db, int approvalHistoryID, int order,int type, int transationID)
        {
            var currentLEvel = (from b in db.ApprovalHistoryTable where b.ApprovalHistoryID == approvalHistoryID select b).FirstOrDefault();

            var list = (from b in db.ApprovalHistoryTable
                        where b.TransactionID == transationID && b.ApproveModuleID == type
                            && (b.OrderNo>=order && b.OrderNo<currentLEvel.OrderNo)
                        //&& b.appovehis != approvalHistoryID
                        select b).ToList();

            return list;
        }
        public static ApprovalHistoryTable GetWorkflowDetailsworkflowPrevious(AMSContext db, int requestID)
        {
            var obj = (from b in db.ApprovalHistoryTable
                       where b.ApprovalHistoryID == requestID
                       select b).FirstOrDefault();
            var maxObjID = (from b in db.ApprovalHistoryTable where b.ApproveModuleID == obj.ApproveModuleID && b.TransactionID == obj.TransactionID select b.OrderNo).Max();

            var previousMaxObjID = (from b in db.ApprovalHistoryTable where b.ApproveModuleID == obj.ApproveModuleID && b.TransactionID == obj.TransactionID
                                    && b.OrderNo == maxObjID - 1 select b).FirstOrDefault();
            return previousMaxObjID;
        }
        public static ApprovalHistoryTable GetTransaction(AMSContext db, int transactionid,int moduleID)
        {
            var list = (from b in db.ApprovalHistoryTable
                        where b.TransactionID == transactionid && b.ApproveModuleID== moduleID
                        //&& b.appovehis != approvalHistoryID
                        select b).FirstOrDefault();
            return list;
        }

        public static ApprovalHistoryTable GetApproval(AMSContext db, int approvalHistoryID)
        {
            var list = (from b in db.ApprovalHistoryTable.Include("ApproveModule")
                        where b.ApprovalHistoryID == approvalHistoryID 
                        //&& b.appovehis != approvalHistoryID
                        select b).FirstOrDefault();
            return list;
        }

        public static bool GethistoryValidation(AMSContext db,int approveworkflowID)
        {
            var list = (from b in db.ApprovalHistoryTable where b.ApproveWorkFlowID == approveworkflowID && b.StatusID == (int)StatusValue.WaitingForApproval select b).Count();
            if(list > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool GetMappingValidation(AMSContext db, int locationID,int roleID)
        {
            var list = (from b in db.ApprovalHistoryTable where b.ApprovalRoleID == roleID
                        &&(b.FromLocationID==locationID || b.ToLocationID==locationID)
                        && b.StatusID == (int)StatusValue.WaitingForApproval select b).Count();
            if (list > 0)
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
