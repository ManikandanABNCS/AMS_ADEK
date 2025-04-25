using ACS.AMS.DAL.DBContext;
using ACS.AMS.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.AMS.DAL.DBModel
{
    public partial class ApproveWorkflowLineItemTable
    {
        public static IQueryable<ApproveWorkflowLineItemTable> GetAllApproveWorkflowLineItems(AMSContext db, int approveWorkflowID)
        {
            return from b in db.ApproveWorkflowLineItemTable.Include("ApprovalRole").Include("Status").Include("ApproveWorkFlow")
                   where b.ApproveWorkFlowID == approveWorkflowID && b.StatusID == (int)StatusValue.Active && b.ApproveWorkFlow.StatusID==(int)StatusValue.Active
                   select b;
        }
        public static ApproveWorkflowLineItemTable GetApproveWorkflowLineItem(AMSContext db, int approveWorkflowID,int approveRoleID)
        {
            var result = (from b in db.ApproveWorkflowLineItemTable.Include("ApprovalRole").Include("Status").Include("ApproveWorkFlow")
                          where b.ApproveWorkFlowID == approveWorkflowID && b.ApprovalRoleID == approveRoleID && b.StatusID == (int)StatusValue.Active
                          && b.ApproveWorkFlow.StatusID == (int)StatusValue.Active
                          select b).FirstOrDefault();
            return result;
        }
        public static void ApproveHistoryMaintanance(AMSContext db,int fromtypeID,int transID,int types,int userID,int secondlvlFromID ,int? secondlevelToID=null, int? totypeID=null,int? categoryTypeID=null)
        {
            var result = (from b in db.ApproveWorkflowTable where b.ApproveModuleID == types && b.FromLocationTypeID == fromtypeID && b.ToLocationTypeID == totypeID && b.StatusID==(int)StatusValue.Active select b).FirstOrDefault();
            if(result!=null)
            {
                var lineitem = (from b in db.ApproveWorkflowLineItemTable where b.ApproveWorkFlowID == result.ApproveWorkflowID && b.StatusID==(int)StatusValue.Active select b).OrderBy(b => b.OrderNo);
                foreach(var  item in lineitem)
                {
                    ApprovalHistoryTable app = new ApprovalHistoryTable();
                    app.ApproveModuleID = types;
                    app.ApproveWorkFlowID = result.ApproveWorkflowID;
                    app.ToLocationTypeID = totypeID;
                    app.FromLocationTypeID = fromtypeID;
                    app.ApproveWorkFlowLineItemID = item.ApproveWorkFlowLineItemID;
                    app.OrderNo = item.OrderNo;
                    app.ApprovalRoleID = item.ApprovalRoleID;
                    app.StatusID = (int)StatusValue.WaitingForApproval;
                    app.TransactionID = transID;
                    app.CreatedBy = userID;
                    app.CreatedDateTime = System.DateTime.Now;
                    app.FromLocationID = secondlvlFromID;
                    app.ToLocationID = secondlevelToID;
                    app.CategoryTypeID = categoryTypeID;
                    db.Add(app);



                }
                db.SaveChanges();
            }

        }
        public static IQueryable<ApproveWorkflowLineModel> GetApprovalRoleDetails(AMSContext entityObject, int approveWorkflowID)
        {
            string update = string.Empty;
            var result = (from b in entityObject.ApproveWorkflowLineItemTable
                          join DesiDesc in entityObject.ApprovalRoleTable on new { ApprovalRoleID = (int)b.ApprovalRoleID }
                        equals new { ApprovalRoleID = DesiDesc.ApprovalRoleID } into Manu
                          from ManuDesc in Manu.DefaultIfEmpty()
                          where b.ApproveWorkFlow.StatusID != (int)StatusValue.Deleted && b.ApproveWorkFlowID == approveWorkflowID && b.StatusID == (int)StatusValue.Active
                          select new ApproveWorkflowLineModel
                          {
                              ApproveWorkflowID = b.ApproveWorkFlowID,
                              ApprovalRoleCode = ManuDesc.ApprovalRoleCode, //(b.AllowUpdate + "" == "true" ? b.DesignationID + "" + "@" + "TRUE" + "" : b.DesignationID + "" + "@" + "FALSE"),
                              ApprovalRoleName = ManuDesc.ApprovalRoleName,
                              ApprovalRoleID = (int)b.ApprovalRoleID,
                              OrderNo = b.OrderNo
                          });

            return result.AsQueryable();
        }
    }
}
