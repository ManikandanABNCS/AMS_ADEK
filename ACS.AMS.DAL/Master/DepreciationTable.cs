using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ACS.AMS.DAL.DBModel
{
    public partial class DepreciationTable : BaseEntityObject

    {

        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            return true;
        }
        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            return true;
        }
        public static async Task<List<prc_GetPeriodWithDateResult>> GetPeriodWithData(AMSContext _db, int companyID)
        {
            try
            {
                var res = await _db.GetProcedures().prc_GetPeriodWithDateAsync(companyID);
                return res;
            }
            catch (Exception ex)

            {
                ApplicationErrorLogTable.SaveException(ex);
                throw ex;
            }
        }
        public static IQueryable<PeriodModel> GetAllPeriodWithData(AMSContext db, int companyID)
        {
            var res = GetPeriodWithData(db, companyID);
            var list = (from b in res.Result
                        select new PeriodModel()
                        {
                            PeriodID = b.PeriodID,
                            StartDate = b.StartDate,
                            EndDate = b.EndDate,
                            PeriodName = b.PeriodName,
                            lastField = b.lastField,
                            nextRecord = b.nextRecord,
                            Status = b.Status,
                            PeriodYear = b.periodYear
                        }).ToList();
            return list.AsQueryable();
        }

        public static async Task<prc_DepreciationResult> SaveDepreciation(AMSContext _db, int periodID,int companyID,int userID)
        {
            try
            {
                var res = await _db.GetProcedures().prc_DepreciationAsync(periodID, userID, companyID);
                return res.FirstOrDefault();
            }
            catch (Exception ex)

            {
                ApplicationErrorLogTable.SaveException(ex);
                throw ex;
            }
        }
        public static async Task<List<prc_DepreciationApprovalResult>> DepreciationApproval(AMSContext _db, int userID,  int companyID)
        {
            try
            {
                var res = await _db.GetProcedures().prc_DepreciationApprovalAsync(userID, companyID);
                return res;
            }
            catch (Exception ex)

            {
                ApplicationErrorLogTable.SaveException(ex);
                throw ex;
            }
        }
        public static IQueryable<PeriodModel> GetAllDepreciationApproval(AMSContext db, int companyID,int userID)
        {
            var res = DepreciationApproval(db, userID,companyID);
            var list = (from b in res.Result
                        select new PeriodModel()
                        {
                            PeriodID = b.PeriodID,
                            StartDate = b.StartDate,
                            EndDate = b.EndDate,
                            PeriodName = b.PeriodName,
                            Status = b.status,
                            depreciationID = b.DepreciationID
                        }).ToList();
            return list.AsQueryable();
        }
        public static bool validateApprovedProcess(AMSContext db, int? periodID, int companyID)
        {
            IQueryable<DepreciationTable> result = (from b in db.DepreciationTable
                                                    where b.StatusID == (int)StatusValue.Active
                                                    && (b.IsDepreciationApproval == null || b.IsDepreciationApproval == false) && b.CompanyID == companyID
                                                    select b);
            if (periodID.HasValue)
            {
                int pID = (int)periodID;
                result = result.Where(a => a.PeriodID == pID);
            }

            if (result.Count() > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool ValidationPeriodUndo(AMSContext db, int periodID,int companyID)
        {
            int result = (from b in db.DepreciationTable
                          where b.PeriodID > periodID && b.StatusID == (int)StatusValue.Active
                          && (b.IsDeletedApproval == null || b.IsDeletedApproval == false) && b.CompanyID == companyID
                          select b).Count();
            if (result > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool validateApprovedProcess(AMSContext db, int? periodID,int? companyID)
        {
            IQueryable<DepreciationTable> result = (from b in db.DepreciationTable
                                                    where b.StatusID == (int)StatusValue.Active
                                                    && (b.IsDepreciationApproval == null || b.IsDepreciationApproval == false) && b.CompanyID == (int)companyID
                                                    select b);
            if (periodID.HasValue)
            {
                int pID = (int)periodID;
                result = result.Where(a => a.PeriodID == pID);
            }

            if (result.Count() > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static DepreciationTable GetDeprecation(AMSContext entityObject, int periodID, int? depreciationID = null,int? companyID=null)
        {
            if (depreciationID.HasValue)
            {
                var result = (from b in entityObject.DepreciationTable
                              where b.PeriodID == periodID && b.DepreciationID == (int)depreciationID && (b.IsReclassification == null || b.IsReclassification == false)
                              && b.CompanyID == SessionUserHelper.CompanyID
                              select b).FirstOrDefault();
                return result;
            }
            else
            {
                var result = (from b in entityObject.DepreciationTable
                              where b.PeriodID == periodID && (b.StatusID != (int)StatusValue.Deleted || b.StatusID != (int)StatusValue.DeletedOLD)
                              && (b.IsReclassification == null || b.IsReclassification == false)
                              && b.CompanyID == SessionUserHelper.CompanyID
                              select b).FirstOrDefault();
                return result;
            }

        }
    }
}
