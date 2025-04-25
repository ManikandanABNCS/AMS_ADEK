using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Diagnostics.Eventing.Reader;
using Microsoft.EntityFrameworkCore;
using Azure.Core;
using System.Data;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace ACS.AMS.DAL.DBModel
{
    public partial class TransactionTable
    {
        public static async Task<prc_AssetTransferForApprovalResult> Getresult(AMSContext _db, int approvalHistoryID, int userID, int? companyID)
        {
            try
            {
                var res = await _db.GetProcedures().prc_AssetTransferForApprovalAsync(userID, approvalHistoryID, companyID, 1);
                return res.FirstOrDefault();
            }
            catch (Exception ex)

            {
                ApplicationErrorLogTable.SaveException(ex);
                throw ex;
            }
        }
        public static async Task<prc_AssetRetirementForApprovalResult> GetRetirementresult(AMSContext _db, int approvalHistoryID, int userID, int? companyID)
        {
            try
            {
                var res = await _db.GetProcedures().prc_AssetRetirementForApprovalAsync(userID, approvalHistoryID, companyID, 1);
                return res.FirstOrDefault();
            }
            catch (Exception ex)

            {
                ApplicationErrorLogTable.SaveException(ex);
                throw ex;
            }
        }
        public static TransactionTable GetTransaction(AMSContext _db,int transactionID)
        {
            var result=(from b in _db.TransactionTable.Include("TransactionType")
                        where b.StatusID!=(int)StatusValue.Deleted && b.TransactionID==transactionID
                        select b ).FirstOrDefault();
            return result;
        }

        //public static async Task<prc_ValidateAssetCategoryMappingResult> GetValidationresult(AMSContext _db, string FromAssetID, int? ToLocationID=null, int? FromLocationTypeID=null,int? ToLocationTypeID=null,int? approvalWorkflowID=null)
        //{
        //    try
        //    {
        //        var res = await _db.GetProcedures().prc_ValidateAssetCategoryMappingAsync(FromAssetID,ToLocationID,FromLocationTypeID,ToLocationTypeID,approvalWorkflowID);
        //        return res.FirstOrDefault();
        //    }
        //    catch (Exception ex)

        //    {
        //        ApplicationErrorLogTable.SaveException(ex);
        //        throw ex;
        //    }
        //}

        public static async Task<string> GetTransactionResult(AMSContext _db, string FromAssetID, int? ToLocationID = null, int? moduleID = null, string rightName = null)
        {
            OutputParameter<int?> errorID = new OutputParameter<int?>();
            OutputParameter<string> errorMsg = new OutputParameter<string>();

            var res = await _db.GetProcedures().prc_ValidateForTransactionAsync(FromAssetID, ToLocationID, moduleID, rightName, errorID, errorMsg);
            var id = errorID.Value;
            string errorMessge = string.Empty;
            if (id > 0)
            {
                errorMessge = errorMsg.Value;
                return errorMessge;
            }
            return errorMessge;
        }
        public static async Task<string> GetTransactionRetriementResult(AMSContext _db, string Barcode,int userID)
        {
            OutputParameter<int?> errorID = new OutputParameter<int?>();
            OutputParameter<string> errorMsg = new OutputParameter<string>();

            var res = await _db.GetProcedures().Prc_ValidateBulkExcelRetirementAsync(Barcode, userID,  errorID, errorMsg);
            var id = errorID.Value;
            string errorMessge = string.Empty;
            if (id > 0)
            {
                errorMessge = errorMsg.Value;
                return errorMessge;
            }
            return errorMessge;
        }
        public static async Task<List<Prc_ValidateUserWiseTransactionsResult>> GetUserValidationResult(AMSContext _db,  int userID, int? moduleID)
        {
            try
            {
                var res = await _db.GetProcedures().Prc_ValidateUserWiseTransactionsAsync(userID, moduleID);
                return res;
            }
            catch (Exception ex)

            {
                ApplicationErrorLogTable.SaveException(ex);
                throw ex;
            }
        }

        public static TransactionTable GetTransactionData(AMSContext _db,int approvalhistoryID)
        {
            var res = ApprovalHistoryTable.GetItem(_db, approvalhistoryID);
            var value = (from b in _db.TransactionTable
                         where b.TransactionID == res.TransactionID
                         select b
                        ).FirstOrDefault();
            return value;
        }

        public static async Task<string> GetAssetResult(AMSContext _db, int locationID, int CategoryID)
        {

            OutputParameter<int?> errorID = new OutputParameter<int?>();
            OutputParameter<string> errorMsg = new OutputParameter<string>();

            var res = await _db.GetProcedures().Prc_ValidateAssetAdditionAsync(locationID, CategoryID, errorID, errorMsg);
            var id = errorID.Value;
            string errorMessge = string.Empty;
            if (id > 0)
            {
                errorMessge = errorMsg.Value;
                return errorMessge;
            }
            return errorMessge;

        }
        public static async Task<string> AssetValidationResult(AMSContext _db, int userID, string dataProcessedBy, int CategoryID, string CategoryCode = null, string LocationCode = null, int? locationID = null, int? departmentID = null, string departmentCode = null, string serialNo = null, string manufacturerCode = null, int? manufacturerID = null)
        {

            OutputParameter<int?> errorID = new OutputParameter<int?>();
            OutputParameter<string> errorMsg = new OutputParameter<string>();

            var res = await _db.GetProcedures().Prc_AssetCreationValidationAsync(userID, CategoryCode, LocationCode, CategoryID, locationID, departmentID,departmentCode , serialNo, manufacturerCode, manufacturerID, dataProcessedBy, errorID, errorMsg);
            var id = errorID.Value;
            string errorMessge = string.Empty;
            if (id > 0)
            {
                errorMessge = errorMsg.Value;
                return errorMessge;
            }
            return errorMessge;

        }
        public static async Task<string> AssetModificationValidationResult(AMSContext _db, int userID,int assetID, string dataProcessedBy, int CategoryID, string CategoryCode = null, string LocationCode = null, int? locationID = null, int? departmentID = null, string departmentCode = null, string serialNo = null, string manufacturerCode = null, int? manufacturerID = null)
        {
            OutputParameter<int?> errorID = new OutputParameter<int?>();
            OutputParameter<string> errorMsg = new OutputParameter<string>();

            var res = await _db.GetProcedures().Prc_AssetModificationValidationAsync(userID,assetID, CategoryCode, LocationCode, CategoryID, locationID, departmentID, departmentCode, serialNo, manufacturerCode, manufacturerID, dataProcessedBy, errorID, errorMsg);
            var id = errorID.Value;
            string errorMessge = string.Empty;
            if (id > 0)
            {
                errorMessge = errorMsg.Value;
                return errorMessge;
            }
            return errorMessge;
        }
        public static async Task<string> AssetTransferValidationResult(AMSContext _db, int userID, int assetID, string dataProcessedBy,  string LocationCode = null, int? locationID = null, int? departmentID = null, string departmentCode = null)
        {
            OutputParameter<int?> errorID = new OutputParameter<int?>();
            OutputParameter<string> errorMsg = new OutputParameter<string>();

            var res = await _db.GetProcedures().Prc_AssetTransferValidationAsync(userID, assetID, LocationCode, locationID, departmentID, departmentCode, dataProcessedBy, errorID, errorMsg);
            var id = errorID.Value;
            string errorMessge = string.Empty;
            if (id > 0)
            {
                errorMessge = errorMsg.Value;
                return errorMessge;
            }
            return errorMessge;
        }
        public static async Task<string> AssetRetirementValidationResult(AMSContext _db, int userID, int assetID, string dataProcessedBy)
        {
            OutputParameter<int?> errorID = new OutputParameter<int?>();
            OutputParameter<string> errorMsg = new OutputParameter<string>();

            var res = await _db.GetProcedures().Prc_AssetRetirementValidationAsync(userID, assetID, dataProcessedBy, errorID, errorMsg);
            var id = errorID.Value;
            string errorMessge = string.Empty;
            if (id > 0)
            {
                errorMessge = errorMsg.Value;
                return errorMessge;
            }
            return errorMessge;
        }
        public static async Task<string> GetBulkAssetResult(AMSContext _db, string locationCode, string CategoryCode)
        {
            OutputParameter<int?> errorID = new OutputParameter<int?>();
            OutputParameter<string> errorMsg = new OutputParameter<string>();

            var res = await _db.GetProcedures().Prc_ValidateBulkAssetAdditionAsync(locationCode, CategoryCode,  errorID, errorMsg);
            var id = errorID.Value;
            string errorMessge = string.Empty; 
            
            if(id>0)
            {
                errorMessge = errorMsg.Value;
                return errorMessge;
            }

            return errorMessge;    
        }

        public static async Task<prc_GetAssetDetailsResult> GetAssetDetails(AMSContext _db, int locationID,int categoryID)
        {
            try
            {
                var res = await _db.GetProcedures().prc_GetAssetDetailsAsync(locationID,categoryID);
                return res.FirstOrDefault();
            }
            catch (Exception ex)

            {
                ApplicationErrorLogTable.SaveException(ex);
                throw ex;
            }
        }
        //public static async Task<prc_GetCategoryTypeResult> GetCategoryTyperesult(AMSContext _db,  int CategoryID)
        //{
        //    try
        //    {
        //        var res = await _db.GetProcedures().prc_GetCategoryTypeAsync(CategoryID);
        //        return res.FirstOrDefault();
        //    }
        //    catch (Exception ex)

        //    {
        //        ApplicationErrorLogTable.SaveException(ex);
        //        throw ex;
        //    }
        //}

        public static void SaveTransactiondata(AMSContext _db,List<string> asset,int userID,int moduleID,string transactionNo)
        {
            TransactionTable tran = new TransactionTable();
            tran.CreatedBy = userID;
            tran.CreatedFrom = "Web";
            tran.CreatedDateTime = System.DateTime.Now;
            tran.TransactionTypeID = moduleID;
            tran.TransactionNo = transactionNo;
            tran.StatusID = (int)StatusValue.WaitingForApproval;
            tran.PostingStatusID = (int)PostingStatusValue.WorkInProgress;
            tran.TransactionDate = System.DateTime.Now;
            _db.Add(tran);

            //var secondlevelID=(from b in _db.)
            List<int> categoryTypeID = new List<int>();
            int? locId=0;
            int? loctypeId=0;
            foreach (var barcode in asset)
            {
                var assets = AssetTable.GetAssetBarcode(_db, barcode);

                var loc = GetAssetDetails(_db, (int)assets.LocationID, assets.CategoryID);
                locId = loc.Result.L2LocationID;
                loctypeId = loc.Result.LocationTypeID;
                int? cateTypeID = loc.Result.CategoryTypeID;
                categoryTypeID.Add((int)cateTypeID);

                TransactionLineItemTable lineitem = new TransactionLineItemTable();
                lineitem.AssetID = assets.AssetID;
                lineitem.Transaction = tran;
                lineitem.CreatedBy = userID;
                lineitem.CreatedDateTime = System.DateTime.Now;
                lineitem.FromLocationID = locId;

                lineitem.StatusID = (int)StatusValue.WaitingForApproval;
                _db.Add(lineitem);

                assets.StatusID = (int)StatusValue.WaitingForApproval;
                
            }
            

            _db.SaveChanges();
            _db.Entry(tran).Reload();

            var category = categoryTypeID.Distinct().ToList();
            int TypeID = category.Distinct().FirstOrDefault();
            if (category.Count()>1)
            {

                var res = CategoryTypeTable.GetAllItems(_db).Where(a => a.IsAllCategoryType == true).FirstOrDefault();
                TypeID = res.CategoryTypeID;
            }


            ApproveWorkflowLineItemTable.ApproveHistoryMaintanance(_db, (int)loctypeId, tran.TransactionID, moduleID, userID, (int)locId, null, null, TypeID);


        }

        //public static Tuple<string, string,int?,int?, int?> ValidationRequestMaintenances(AMSContext _db, List<int> assetID)
        //{
        //    string errorMessage = string.Empty;
        //    // Tuple<string, string, int> validation = new Tuple<string, string, int>();
        //    Tuple<string, string, int?,int?, int?> validation;
        //    int? categoryTypeID = null;
        //    int? fromLocationTypeID = null;
        //    int? mappedLoationID = null;
        //    var list = AssetNewView.GetAllItems(_db).Where(a => assetID.Contains(a.AssetID));
        //    var assetIDs = String.Join(",", list.Select(u => u.AssetID));
        //    if (list.Select(a => a.LocationType).Distinct().Count()>1)
        //    {
        //        errorMessage = "Selected Asset have different location type please select same location type.";
        //        validation = new Tuple<string, string, int?, int?, int?>(errorMessage, assetIDs, categoryTypeID, fromLocationTypeID, mappedLoationID);
        //        return validation;
        //    }
        //    if (list.Select(a => a.MappedLocationID).Distinct().Count() > 1)
        //    {
        //        errorMessage = "Selected Asset have different Mapped Location please select same second level location .";
        //        validation = new Tuple<string, string, int?, int?, int?>(errorMessage, assetIDs, categoryTypeID, fromLocationTypeID, mappedLoationID);
        //        return validation;
        //    }
        //     fromLocationTypeID = LocationTable.GetLocationType(_db, list.Select(a => a.LocationType).FirstOrDefault());
        //     mappedLoationID = list.Select(a => a.MappedLocationID).FirstOrDefault();
        //    var category = list.Select(a => a.CategoryType).Distinct();

        //    if (category.Count() == 1)
        //    {
        //        foreach (var str in category.ToList())
        //        {
        //            categoryTypeID = CategoryTypeTable.GetCategoryTypeDetails(_db, str).CategoryTypeID;
        //        }

        //    }
        //    else
        //    {
        //        var res = CategoryTypeTable.GetAllItems(_db).Where(a => a.IsAllCategoryType == true).FirstOrDefault();
        //        if (res == null)
        //        {
        //             errorMessage="Selected Assets have both IT and NON-IT so CategoryType-All Should be created";
        //             validation = new Tuple<string, string, int?,int?, int?>(errorMessage, assetIDs, categoryTypeID,fromLocationTypeID, mappedLoationID);
        //            return validation;
        //        }
        //        categoryTypeID = res.CategoryTypeID;
        //    }
        //     validation = new Tuple<string, string, int?,int?, int?>(errorMessage, assetIDs, categoryTypeID,fromLocationTypeID, mappedLoationID);
        //    return validation;

        //}

        public static Tuple<int?, int?, int?, int?> GetApprovalHistoryList(AMSContext _db, List<int> assetID, int? toLocationID = null, int? moduleID = null)
        {
            var list = AssetNewView.GetAllItems(_db).Where(a => assetID.Contains(a.AssetID));

            Tuple<int?, int?, int?, int?> GetRequiredValues;
            int fromLocationTypeID = 0;
            bool type = AppConfigurationManager.GetValue<bool>(AppConfigurationManager.IsMandatoryLocationType);
            if (type)
            {
                fromLocationTypeID = LocationTable.GetLocationType(_db, list.Select(a => a.LocationType).FirstOrDefault());
            }
            else
            {
                fromLocationTypeID = LocationTypeTable.GetAllLocationType(_db, "All").LocationTypeID;
            }
           

                int? toLocationTypeID = null;
            if (type)
            {
                if (toLocationID.HasValue)
                {
                    toLocationTypeID = LocationTable.GetItem(_db, (int)toLocationID).LocationTypeID;
                    if (moduleID.HasValue)
                    {
                        if (moduleID == (int)ApproveModuleValue.InternalAssetTransfer)
                        {
                            toLocationTypeID = LocationNewView.GetItem(_db, (int)toLocationID).LocationTypeID;
                        }
                    }
                }
            }
            else
            {
                if (toLocationID.HasValue)
                {
                    toLocationTypeID = fromLocationTypeID;
                }
            }
            int? FromLocationL2 = list.Select(a => a.MappedLocationID).FirstOrDefault();
            var category = list.Select(a => a.CategoryType).Distinct();
            int? categoryTypeID = null;
            if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.IsMandatoryCategoryType))
            {
                if (category.Count() == 1)
                {
                    categoryTypeID = CategoryTypeTable.GetCategoryTypeDetails(_db, category.FirstOrDefault()).CategoryTypeID;
                }
                else
                {
                    var res = CategoryTypeTable.GetAllItems(_db).Where(a => a.IsAllCategoryType == true).FirstOrDefault();
                    categoryTypeID = res.CategoryTypeID;
                }
            }
            else
            {
                categoryTypeID = CategoryTypeTable.GetCategoryTypeDetails(_db, "All").CategoryTypeID;
            }
            GetRequiredValues = new Tuple<int?, int?, int?, int?>(FromLocationL2, fromLocationTypeID, toLocationTypeID, categoryTypeID);
            return GetRequiredValues;
        }
        public static async Task<List<prc_ImportTemplateUserAgainstListResult>> GetUserImportTemplate(AMSContext _db, int userID)
        {
            try
            {
                var res = await _db.GetProcedures().prc_ImportTemplateUserAgainstListAsync(userID);
                return res;
            }
            catch (Exception ex)

            {
                ApplicationErrorLogTable.SaveException(ex);
                throw ex;
            }
        }

    }
}
