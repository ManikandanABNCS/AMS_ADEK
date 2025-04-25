using ACS.AMS.DAL;
using ACS.AMS.DAL.DBContext;
using ACS.AMS.DAL.DBModel;
using ACS.AMS.WebApp.Models;
using Oracle.ManagedDataAccess.Types;
using System.Data;

namespace ACS.AMS.WebApp.Classes
{
    public static class ApprovalHelper
    {
        private static Dictionary<string, object> threadObjects = new Dictionary<string, object>();

       public static bool SaveApproval(AMSContext _db, string type,string remarks,List<DocumentModel> doc,string typeName,ApprovalHistoryTable wItem,
                                        int historyID,int userID,int approvalmoduleID,int enableUpdate, TransferAssetDataModel transactionModel,int currentPageID,int levels=0)
        {
            bool flag = false;
            if (doc.Count > 0)
            {
                foreach (var item in doc)
                {
                    DocumentTable document = new DocumentTable();
                    document.FileName = item.FileName;
                    document.DocumentName = item.DocumentName;
                    document.FilePath = item.FullPath;
                    document.FileExtension = item.FileExtension;
                    document.TransactionType = typeName;
                    document.ObjectKeyID = wItem.ApprovalHistoryID;
                    _db.Add(document);
                }
            }
            if (string.Compare(type, "Approval") == 0)
            {
                var maxworkflowID = ApprovalHistoryTable.GetWorkflowDetailsworkflowMax(_db, historyID);
                var max = ApprovalHistoryTable.GetItem(_db, maxworkflowID);
                var previousID = ApprovalHistoryTable.GetWorkflowDetailsworkflowPrevious(_db, historyID);
                var results = TransactionApprovalView.GetAllItems(_db).Where(a => a.UserID == userID && a.ApprovalHistoryID == historyID).FirstOrDefault();
                if (results.ApprovalRoleID == max.ApprovalRoleID)
                {
                    wItem.StatusID = (int)StatusValue.Active;
                    wItem.LastModifiedDateTime = System.DateTime.Now;
                    wItem.LastModifiedBy = userID;
                    wItem.Remarks = remarks;

                    TransactionTable tramsType = TransactionTable.GetItem(_db, wItem.TransactionID);
                    tramsType.StatusID = (int)StatusValue.Active;
                    tramsType.PostingStatusID = (int)PostingStatusValue.CompletedByEndUser;
                    tramsType.VerifiedBy = SessionUser.Current.UserID;
                    tramsType.VerifiedDateTime = System.DateTime.Now;
                    tramsType.Remarks = remarks;

                    var lineitem = TransactionLineItemTable.GetTransactionLineItems(_db, wItem.TransactionID);
                    foreach (var line in lineitem.ToList())
                    {
                        TransactionLineItemTable oldline = TransactionLineItemTable.GetItem(_db, line.TransactionLineItemID);
                        oldline.StatusID = (int)StatusValue.Active;
                        if (enableUpdate == 0) //Enable update false
                        {
                            AssetTable OldAsset = AssetTable.GetItem(_db, oldline.AssetID);

                            if (approvalmoduleID == (int)ApproveModuleValue.AssetTransfer )
                            {
                                OldAsset.LocationID = oldline.RoomID;
                                OldAsset.CustodianID = oldline.CustodianID;
                                OldAsset.DepartmentID = oldline.DepartmentID;
                                OldAsset.SectionID = oldline.SectionID;
                                OldAsset.StatusID = (int)StatusValue.Active;

                            }
                            else if (approvalmoduleID == (int)ApproveModuleValue.AssetRetirement)
                            {
                                OldAsset.StatusID = (int)StatusValue.Disposed;
                                OldAsset.ProceedofSales = oldline.ProceedOfSales;
                                OldAsset.CostOfRemoval = oldline.CostOfRemoval;
                                OldAsset.DisposedDateTime = oldline.DisposalDate;
                                OldAsset.DisposalReferenceNo = oldline.DisposalReferencesNo;
                                OldAsset.DisposedRemarks = oldline.DisposalRemarks;
                                OldAsset.DisposalValue = oldline.DisposalValue;
                                OldAsset.RetirementTypeID = oldline.RetirementTypeID;
                            }
                            else if(approvalmoduleID == (int)ApproveModuleValue.InternalAssetTransfer)
                            {
                                OldAsset.LocationID = oldline.ToLocationID;
                                if(oldline.ToCustodianID.HasValue)
                                {
                                    OldAsset.CustodianID = oldline.ToCustodianID;
                                }
                                if (oldline.ToDepartmentID.HasValue)
                                {
                                    OldAsset.DepartmentID = oldline.ToDepartmentID;
                                }
                                if (oldline.ToSectionID.HasValue)
                                {
                                    OldAsset.SectionID = oldline.ToSectionID;
                                }
                                if (oldline.ToAssetConditionID.HasValue)
                                {
                                    OldAsset.AssetConditionID = oldline.ToAssetConditionID;
                                }
                              
                                OldAsset.StatusID = (int)StatusValue.Active;
                                

                            }
                            else
                            {
                                OldAsset.StatusID = (int)StatusValue.Active;
                            }
                        }
                    }
                    if (enableUpdate == 1)//enable update was true
                    {
                        var lineitemAsset = transactionModel;
                        if (approvalmoduleID == (int)ApproveModuleValue.AssetTransfer)
                        {
 
                            if (lineitemAsset.LineItems.Count() > 0)
                            {
                                foreach (var line in lineitemAsset.LineItems.Select(a => a.Asset).ToList())
                                {
                                    AssetTable OldAsset = AssetTable.GetItem(_db, line.AssetID);
                                    OldAsset.StatusID = (int)StatusValue.Active;
                                    OldAsset.LocationID = line.LocationID;
                                    OldAsset.DepartmentID = line.DepartmentID;
                                    OldAsset.CustodianID = line.CustodianID;
                                    OldAsset.SectionID = line.SectionID;

                                    var tranLine = lineitem.Where(a => a.AssetID == line.AssetID).FirstOrDefault();
                                    tranLine.RoomID = line.LocationID;
                                    tranLine.CustodianID = line.CustodianID;
                                    tranLine.DepartmentID = line.DepartmentID;
                                    tranLine.SectionID = line.SectionID;
                                }
                            }
                            else
                            {
                                foreach (var line in lineitem)
                                {
                                    AssetTable OldAsset = AssetTable.GetItem(_db, line.AssetID);
                                    OldAsset.StatusID = (int)StatusValue.Active;
                                    OldAsset.LocationID = line.RoomID;
                                    OldAsset.DepartmentID = line.DepartmentID;
                                    OldAsset.SectionID = line.SectionID;
                                    OldAsset.CustodianID = line.CustodianID;
                                }
                            }

                        }
                        if (approvalmoduleID == (int)ApproveModuleValue.AssetRetirement)
                        {
                            if (lineitemAsset.LineItems.Count() > 0)
                            {
                                foreach (var line in lineitemAsset.LineItems.Select(a => a.Asset).ToList())
                                {

                                    AssetTable OldAsset = AssetTable.GetItem(_db, line.AssetID);

                                    OldAsset.StatusID = (int)StatusValue.Disposed;
                                    OldAsset.DisposalReferenceNo = line.DisposalReferenceNo;
                                    OldAsset.DisposalValue = line.DisposalValue;
                                    OldAsset.DisposedRemarks = line.DisposedRemarks;
                                    OldAsset.DisposedDateTime = line.DisposedDateTime;
                                    OldAsset.ProceedofSales = line.ProceedofSales;
                                    OldAsset.CostOfRemoval = line.CostOfRemoval;
                                    OldAsset.DisposedDateTime = line.DisposedDateTime;
                                    OldAsset.RetirementTypeID = line.RetirementTypeID;

                                    var tranLine = lineitem.Where(a => a.AssetID == line.AssetID).FirstOrDefault();
                                    tranLine.CostOfRemoval = line.CostOfRemoval;
                                    tranLine.DisposalReferencesNo = line.DisposalReferenceNo;
                                    tranLine.DisposalValue = line.DisposalValue;
                                    tranLine.DisposalRemarks = line.DisposedRemarks;
                                    tranLine.ProceedOfSales = line.ProceedofSales;
                                    tranLine.DisposalDate = line.DisposedDateTime;
                                    tranLine.RetirementTypeID = line.RetirementTypeID;


                                }
                            }
                            else
                            {
                                foreach (var line in lineitem)
                                {
                                    AssetTable OldAsset = AssetTable.GetItem(_db, line.AssetID);

                                    OldAsset.StatusID = (int)StatusValue.Disposed;
                                    OldAsset.DisposalReferenceNo = line.DisposalReferencesNo;
                                    OldAsset.DisposalValue = line.DisposalValue;
                                    OldAsset.DisposedRemarks = line.DisposalRemarks;
                                    OldAsset.DisposedDateTime = line.DisposalDate;
                                    OldAsset.ProceedofSales = line.ProceedOfSales;
                                    OldAsset.CostOfRemoval = line.CostOfRemoval;
                                    //  OldAsset.DisposedDateTime = line.DisposedDateTime;
                                    OldAsset.RetirementTypeID = line.RetirementTypeID;
                                }
                            }
                        }

                    }
                }
                else
                {
                    if (previousID.ApprovalHistoryID == wItem.ApprovalHistoryID)
                    {
                        TransactionTable tramsType = TransactionTable.GetItem(_db, wItem.TransactionID);
                        tramsType.PostingStatusID = (int)PostingStatusValue.WaitingForFinalApproval;
                        tramsType.PostedBy = userID;
                        tramsType.PostedDateTime = System.DateTime.Now;
                    }
                    wItem.StatusID = (int)StatusValue.Active;
                    wItem.LastModifiedDateTime = System.DateTime.Now;
                    wItem.LastModifiedBy = userID;
                    wItem.Remarks = remarks;
                    if (enableUpdate == 1)//enable update was true
                    {
                        var lineitemAsset = transactionModel;
                        var lineitem = TransactionLineItemTable.GetTransactionLineItems(_db, wItem.TransactionID);
                        var getmodeldate = lineitemAsset.LineItems.Select(a => a.Asset).ToList();

                        if (approvalmoduleID == (int)ApproveModuleValue.AssetTransfer)
                        {
                            foreach (var line in lineitem)
                            {
                                var locationid = (from b in getmodeldate where b.AssetID == line.AssetID select b).FirstOrDefault();
                                if (locationid != null)
                                {
                                    line.RoomID = (int)locationid.LocationID;
                                    line.DepartmentID = locationid.DepartmentID;
                                    line.CustodianID = locationid.CustodianID;
                                    line.SectionID = locationid.SectionID;
                                }
                            }
                        }
                        if (approvalmoduleID == (int)ApproveModuleValue.AssetRetirement)
                        {
                            foreach (var line in lineitem)
                            {
                                var datas = (from b in getmodeldate where b.AssetID == line.AssetID select b).FirstOrDefault();
                                if (datas != null)
                                {
                                    line.ProceedOfSales = datas.ProceedofSales;
                                    line.CostOfRemoval = datas.CostOfRemoval;
                                    line.DisposalDate = datas.DisposedDateTime;
                                    line.DisposalReferencesNo = datas.DisposalReferenceNo;
                                    line.DisposalRemarks = datas.DisposedRemarks;
                                    line.DisposalValue = datas.DisposalValue;
                                    line.RetirementTypeID = datas.RetirementTypeID;

                                }
                            }
                        }
                    }

                }
            }
            else if (string.Compare(type, "Reject") == 0)
            {
                if (levels == 0)
                {

                    wItem.StatusID = (int)StatusValue.Rejected;
                    wItem.LastModifiedDateTime = System.DateTime.Now;
                    wItem.LastModifiedBy = userID;
                    wItem.Remarks = remarks;

                    var remainingUsersameLevel = ApprovalHistoryTable.SameLevelUserList(_db, wItem.ApprovalHistoryID, approvalmoduleID, wItem.TransactionID);
                    foreach (var remaining in remainingUsersameLevel.ToList())
                    {
                        ApprovalHistoryTable OldHistory = ApprovalHistoryTable.GetItem(_db, remaining.ApprovalHistoryID);
                        OldHistory.StatusID = (int)StatusValue.Inactive;
                        OldHistory.LastModifiedBy = userID;
                        OldHistory.LastModifiedDateTime = System.DateTime.Now;

                    }
                    TransactionTable tramsType = TransactionTable.GetItem(_db, wItem.TransactionID);
                    tramsType.StatusID = (int)StatusValue.Rejected;
                    tramsType.PostingStatusID = (int)PostingStatusValue.CompletedByEndUser;
                    tramsType.VerifiedBy = SessionUser.Current.UserID;
                    tramsType.VerifiedDateTime = System.DateTime.Now;
                    tramsType.Remarks = remarks;
                    var lineitem = TransactionLineItemTable.GetTransactionLineItems(_db, wItem.TransactionID);
                    foreach (var line in lineitem.ToList())
                    {
                        TransactionLineItemTable oldline = TransactionLineItemTable.GetItem(_db, line.TransactionLineItemID);
                        oldline.StatusID = (int)StatusValue.Rejected;

                        AssetTable OldAsset = AssetTable.GetItem(_db, oldline.AssetID);
                        OldAsset.StatusID = (int)StatusValue.Active;

                    }
                }
                else
                {
                    var remainingUsersameLevel = ApprovalHistoryTable.SameLevelApprovedUserList(_db, wItem.ApprovalHistoryID, levels, approvalmoduleID, wItem.TransactionID);
                    foreach (var remaining in remainingUsersameLevel.ToList())
                    {
                        ApprovalHistoryTable OldHistory = ApprovalHistoryTable.GetItem(_db, remaining.ApprovalHistoryID);
                        OldHistory.StatusID = (int)StatusValue.ReApproval;
                        OldHistory.LastModifiedBy = userID;
                        OldHistory.LastModifiedDateTime = System.DateTime.Now;
                        OldHistory.Remarks = remarks;

                        ApprovalTransactionHistoryTable history = new ApprovalTransactionHistoryTable();
                        history.ApprovalHistoryID = remaining.ApprovalHistoryID;
                        history.StatusID= (int)StatusValue.ReApproval;
                        history.CreatedBy = userID;
                        history.CreatedDateTime = System.DateTime.Now;
                        history.Remarks = remarks;
                        _db.Add(history);
                    }

                        wItem.StatusID = (int)StatusValue.WaitingForApproval;
                    wItem.LastModifiedDateTime = System.DateTime.Now;
                    wItem.LastModifiedBy = userID;
                    wItem.Remarks = remarks;
                }
            }

                return flag;
        }

        public static List<string> GetTransferDBColumnName()
        {
            List<string> prop = new List<string>()
                            {
                                "Barcode",
                                "RoomCode",
                                "OracleLocationID"
                            };
            return prop;

        }
        public static List<string> GetRetirementDBColumnName()
        {
            List<string> prop = new List<string>()
                            {
                                "Barcode",
                                "RetirementValue",
                                "RetirementReferencesNo",
                                "RetirementRemarks",
                                "ProceedOfSales",
                                "CostOfRemoval",
                                "RetirementTypeCode",
                                "RetirementDate"
                            };


            return prop;

        }

        public static string ValidateExcelSheet(DataTable table, List<string> dbColumns)
        {

            string retrunString = string.Empty;
            int outerCount = 0;

            foreach (var excelAvailabilityField in dbColumns)
            {
                //var data = res.Data.Rows[0][outerCount].ToString();
                string columnName = table.Columns[outerCount].ColumnName.ToString();
                if (columnName.Replace("\n", "").Replace(" ", "").Trim().ToUpper() == excelAvailabilityField.Replace(" ", "").Trim().ToUpper())
                {
                    outerCount++;
                    continue;
                }
                else
                {
                    retrunString = columnName + " Column mismatch, please Check it";
                    return retrunString;
                }
            }

            return retrunString;

        }
        public static string ValidateMandatory(DataTable table, List<string> mandatoryfields)
        {
            string retrunString = string.Empty;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    string columnName = table.Columns[j].ColumnName.ToString();
                    var valid = mandatoryfields.Where(a => a.Equals(columnName)).FirstOrDefault();
                    if (valid != null)
                    {
                        if (string.IsNullOrEmpty(table.Rows[i][j] + ""))
                        {
                            retrunString += columnName + ",";
                        }
                    }
                }
            }
            return retrunString;
        }
        public static string ValidateUnique(DataTable table, IQueryable<ImportTemplateModel> uniquefields)
        {
            string dupValue = string.Empty;

            foreach (var availField in uniquefields)
            {

                var duplicateValues = (from row in table.AsEnumerable()
                                       let ID = row.Field<string>(availField.ImportField)
                                       group row by new { ID } into grp
                                       where grp.Count() > 1
                                       select new
                                       {
                                           DupID = grp.Key.ID,
                                           count = grp.Count(),
                                           field = availField.ImportField
                                       }).ToList();

                foreach (var dup in duplicateValues)
                {
                    dupValue += string.IsNullOrEmpty(dupValue) ? dup.DupID : (dupValue.LastIndexOf("\n") == dupValue.Length - 1) ? Environment.NewLine + dup.DupID : "," + dup.DupID;
                }

                if (duplicateValues.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dupValue))
                    {
                        dupValue += " In " + duplicateValues.FirstOrDefault().field + Environment.NewLine;
                    }
                }
            }
            return dupValue;
        }
        //public static string ValidateExcelSheet(DataTable table, List<string> dbColumns)
        //{

        //    string retrunString = string.Empty;
        //    int outerCount = 0;
        //    int tablecnt = table.Columns.Count;
        //    if (table.Columns.Count == dbColumns.Count())
        //    {
        //        foreach (var excelAvailabilityField in dbColumns)
        //        {
        //            //var data = res.Data.Rows[0][outerCount].ToString();
        //            string columnName = table.Columns[outerCount].ColumnName.ToString();
        //            if (columnName.Replace("\n", "").Replace(" ", "").Trim().ToUpper() == excelAvailabilityField.Replace(" ", "").Trim().ToUpper())
        //            {
        //                outerCount++;
        //                continue;
        //            }
        //            else
        //            {
        //                retrunString = columnName + " Column not available in Datatable, please Check it";
        //                return retrunString;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        retrunString = " Column mismatched . Please download the sample format and fill the data upload it";
        //        return retrunString;
        //    }

        //    return retrunString;

        //}
        public static string ValidateValues(DataTable table)
        {
            string retrunString = string.Empty;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (string.IsNullOrEmpty(table.Rows[i][j] + ""))
                    {
                        retrunString = "Some fields are have empty please check its";
                        return retrunString;
                    }
                }
            }
            return retrunString;
        }
    }
}
