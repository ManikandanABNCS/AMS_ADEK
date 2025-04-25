using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.AMS.DAL.DBModel
{
    public partial class ApproveWorkflowTable:BaseEntityObject
    {
        public static IQueryable<ApproveWorkflowTable> GetAllApprovalWorkflow(AMSContext db)
        {
            var result = (from b in db.ApproveWorkflowTable.Include("ApproveModule").Include("FromLocationType").Include("ToLocationType")
                          where  b.StatusID != (int)StatusValue.Deleted select b);

            return result;
        }
        public static ApproveWorkflowTable GetApprovalWorkflow(AMSContext db,int approveWorkflowID)
        {
            var result = (from b in db.ApproveWorkflowTable.Include("ApproveModule").Include("FromLocationType").Include("ToLocationType")


                          where b.StatusID != (int)StatusValue.Deleted && b.ApproveWorkflowID==approveWorkflowID
                          select b).FirstOrDefault();
            return result;
        }
        public static bool ValidateWorkFlow(AMSContext db,int fromlocTypeID,int approvemoduleID,int? toLocTypeID=null )
        {
         
            //if(approvworkflowID.HasValue)
            //{
            //    var result = (from b in db.ApproveWorkflowTable
            //                  where b.ApproveWorkflowID == approvworkflowID && b.FromLocationTypeID == fromlocTypeID && b.ApproveModuleID == approvemoduleID
            //                  && b.ToLocationTypeID == toLocTypeID && b.StatusID != (int)StatusValue.Deleted //&& b.CategoryTypeID == (int)categorytypeID
            //                  select b).FirstOrDefault();

            //    if(result.CategoryTypeID.HasValue)
            //    {
            //        if(result.CategoryTypeID==(int)categorytypeID)
            //        {
            //            return false;
            //        }
            //        else
            //        {
            //            return true;
            //        }
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //    //if (result == 0)
            //    //{
            //    //    return false;
            //    //}
            //    //else
            //    //{
            //    //    return true;
            //    //}

            //}
            //else
            //{
               var result = (from b in db.ApproveWorkflowTable
                                                           where b.FromLocationTypeID == fromlocTypeID && b.ApproveModuleID == approvemoduleID
                                                           && b.ToLocationTypeID == toLocTypeID && b.StatusID != (int)StatusValue.Deleted //&& b.CategoryTypeID == (int)categorytypeID
                                                           select b).Count();
                if (result == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }

           // }

        }
        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.ApprovalHistoryTable
                                  where (b.ApproveWorkFlowID == this.ApproveWorkflowID)
                                  && b.StatusID == (int)StatusValue.WaitingForApproval
                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("references found", this.ApproveWorkflowID);
                args.FieldName = "ApproveWorkflowID";
                return false;
            }
           

            return true;
        }
    }
}
