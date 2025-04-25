using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using ACS.WebAppPageGenerator.Models.SystemModels;

namespace ACS.AMS.DAL.DBModel
{
    public partial class LocationTable : BaseEntityObject
    {
        public static IQueryable<LocationTable> GetAllLocations(AMSContext db, bool includeInactiveItems = false)
        {
            var result = from b in db.LocationTable.Include("ParentLocation")
                         where b.StatusID != (int)StatusValue.Deleted && b.StatusID != (int)StatusValue.DeletedOLD &&
                            (includeInactiveItems == false ? b.StatusID < (int)StatusValue.Inactive : b.StatusID != (int)StatusValue.Deleted)
                       // && b.ParentLocation.ParentLocationID == null
                         orderby b.LocationCode
                         select b;
            return result;
        }

        public override PageFieldModelCollection GetCreateScreenControls(string subpageName, int userID)
        {
            var allList = base.GetCreateScreenControls(subpageName, userID);

            PageFieldModelCollection allowedCols = new PageFieldModelCollection()
            {
                new PageFieldModel() { FieldName = "ParentLocationID", IsMandatory = false },
                new PageFieldModel() { FieldName = "LocationCode", IsMandatory = true },
                new PageFieldModel() { FieldName = "LocationName", IsMandatory = true },
            };

            if (AppConfigurationManager.GetValue<bool>("IsMandatoryLocationType"))
            {
                allowedCols.Add(new PageFieldModel() { FieldName = "LocationTypeID", IsMandatory = true , IsMasterCreation = false });
            }
            else
            {
                var data = LocationTypeTable.GetAllLocationType(AMSContext.CreateNewContext(), "All");
                allowedCols.Add(new PageFieldModel() { FieldName = "LocationTypeID", IsHidden = true, DefaultValue = (int)data.LocationTypeID , IsMasterCreation = false });
            }
           

            //keep only allowed items, remove remaining
            //allList = allList.Where(b => (from c in allowedCols select c).Contains(b.FieldName) ).ToList();

            PageFieldModelCollection newList = new PageFieldModelCollection();
            foreach (var c in allowedCols)
            {
                var itm = allList.Where(b => string.Compare(b.FieldName, c.FieldName, true) == 0).FirstOrDefault();
                if (itm != null)
                {
                    itm.IsMandatory = c.IsMandatory;
                    itm.IsHidden = c.IsHidden;
                    itm.DefaultValue = c.DefaultValue;
                    itm.ControlType = c.ControlType;
                    itm.ControlName = c.ControlName;
                    itm.DisplayLabel = string.IsNullOrEmpty(c.DisplayLabel) ? c.FieldName : c.DisplayLabel;
                    itm.DataReadScriptFunctionName = c.DataReadScriptFunctionName;
                    itm.IsMasterCreation = c.IsMasterCreation;
                    newList.Add(itm);
                }
            }

            return newList;
        }

        public static int GetLocationType(AMSContext db, string locationType)
        {
            var res = (from b in db.LocationTypeTable where b.LocationTypeName == locationType select b).FirstOrDefault();
            return res.LocationTypeID;
        }

        public static LocationTable GetLocationCode(AMSContext db,string locationCode)
        {
            var result=(from b in db.LocationTable where b.LocationCode==locationCode && b.ParentLocationID==null select b).FirstOrDefault();
            return result;
        }

        public static bool ValidateLocationType(AMSContext db, string locationCode, int transactionID)
        {
           
            var transacrion = TransactionLineItemTable.GetTransactionLineItems(db, transactionID).FirstOrDefault();
            var loc=LocationTable.GetItem(db, (int)transacrion.ToLocationID);   

            var result = (from b in db.LocationTable
                          where b.LocationCode == locationCode 
                          && b.StatusID == (int)StatusValue.Active
                          select b).FirstOrDefault();
            if (result != null)
            {
                
                var lvls = LocationNewView.GetAllItems(db).Where(b => b.LocationID == result.LocationID && b.Level2ID == loc.LocationID).FirstOrDefault();
                if (lvls != null)
                {
                    if (lvls.Level >= 3)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        public static bool ValidateThirdLocationCode(AMSContext db, string locationCode,int transactionID)
        {
            
            var result = (from b in db.LocationTable
                          where b.LocationCode == locationCode 
                          && b.StatusID == (int)StatusValue.Active
                          select b).FirstOrDefault();
            if(result!=null)
            {
                var lvls = LocationNewView.GetItem(db, result.LocationID);
                if(lvls.Level>=3)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
        public static LocationTable GetThirdLocationCode(AMSContext db, string locationCode)
        {
            var result = (from b in db.LocationTable where b.LocationCode == locationCode 
                          && b.StatusID==(int)StatusValue.Active  select b).FirstOrDefault();
            
            return result;
        }

        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.LocationTable
                                  where b.LocationCode == this.LocationCode
                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();
            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("LocationCode Code already exists ", this.LocationCode);
                args.FieldName = "LocationCode";
                return false;
            }
            if (AppConfigurationManager.GetValue<bool>(AppConfigurationManager.IsMandatoryLocationType))
            {
                int level = AppConfigurationManager.GetValue<int>(AppConfigurationManager.PreferredLevelLocationMapping);

                if (this.ParentLocationID == null && this.LocationTypeID != null && level>1)
                {
                    args.ErrorMessage = string.Format("Location Type can be changed only at the Preferred Level", this.LocationCode);
                    args.FieldName = "LocationCode";
                    return false;
                }

                if (this.ParentLocationID != null)
                {
                    var loc = (from b in args.NewDB.LocationNewView where b.LocationID == this.ParentLocationID select b).FirstOrDefault();
                    if (loc != null)
                    {
                        if (loc.ParentLocationID != null && this.LocationTypeID != null)
                        {
                            if ((level - 1) != loc.Level)
                            {
                                args.ErrorMessage = string.Format("Location Type can be changed only at the Preferred Level", this.LocationCode);
                                args.FieldName = "LocationCode";

                                return false;
                            }
                        }
                        else if (loc.ParentLocationID == null && this.LocationTypeID == null)
                        {
                            if ((level - 1) == loc.Level)
                            {
                                args.ErrorMessage = string.Format("Location Type mandatory for Preferred Level Location", this.LocationCode);
                                args.FieldName = "LocationCode";

                                return false;
                            }
                        }
                    }
                }
            }

            if (this.IsStoreLocation == null)
                this.IsStoreLocation = false;

            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.LocationTable
                                  where (b.LocationCode == this.LocationCode && b.StatusID == (int)StatusValue.Active
                                  && b.LocationID != this.LocationID)
                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Location Code already exists", this.LocationCode);
                args.FieldName = "LocationCode";
                return false;
            }

            if (this.ParentLocationID == null && this.LocationTypeID != null)
            {
                args.ErrorMessage = string.Format("Second Level Location only LocationType is mandatory ", this.LocationCode);
                args.FieldName = "LocationCode";
                return false;
            }

            //if (this.ParentLocationID != null)
            //{
            //    var loc = (from b in args.NewDB.LocationTable where b.LocationID == this.ParentLocationID select b).FirstOrDefault();
            //    if (loc.ParentLocationID != null && this.LocationTypeID != null)
            //    {
            //        args.ErrorMessage = string.Format("Second Level Location only LocationType is mandatory ", this.LocationCode);
            //        args.FieldName = "LocationCode";
            //        return false;
            //    }
            //    else if (loc.ParentLocationID == null && this.LocationTypeID == null)
            //    {
            //        args.ErrorMessage = string.Format("Second Level Location required LocationType Please select it ", this.LocationCode);
            //        args.FieldName = "LocationCode";
            //        return false;
            //    }
            //}

            if (this.IsStoreLocation == null)
                this.IsStoreLocation = false;

            return true;
        }

        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.AssetTable
                                  where (b.LocationID == this.LocationID)
                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("references found", this.LocationID);
                args.FieldName = "LocationCode";
                return false;
            }
            return true;
        }

        //internal override bool ValidateObject(ValidateEventArgs<AMSContext> args)
        //{
        //    switch (args.State)
        //    {
        //        case EntityObjectState.New:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.LocationTable
        //                                      where b.LocationCode == this.LocationCode
        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();
        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("LocationCode Code already exists ", this.LocationCode);
        //                    args.FieldName = "LocationCode";
        //                    return false;
        //                }
        //               if(this.ParentLocationID==null && this.LocationTypeID!=null)
        //                {
        //                    args.ErrorMessage = string.Format("Second Level Location only LocationType is mandatory ", this.LocationCode);
        //                    args.FieldName = "LocationCode";
        //                    return false;
        //                }
        //                if (this.ParentLocationID != null)
        //                {
        //                    var loc = (from b in args.NewDB.LocationTable where b.LocationID == this.ParentLocationID select b).FirstOrDefault();
        //                    if(loc.ParentLocationID!=null && this.LocationTypeID!=null )
        //                    {
        //                        args.ErrorMessage = string.Format("Second Level Location only LocationType is mandatory ", this.LocationCode);
        //                        args.FieldName = "LocationCode";
        //                        return false;
        //                    }
        //                    else if(loc.ParentLocationID==null && this.LocationTypeID==null)
        //                    {
        //                        args.ErrorMessage = string.Format("Second Level Location required LocationType Please select it ", this.LocationCode);
        //                        args.FieldName = "LocationCode";
        //                        return false;
        //                    }
        //                }

        //                return true;
        //            }

        //        case EntityObjectState.Edit:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.LocationTable
        //                                      where (b.LocationCode == this.LocationCode && b.StatusID == (int)StatusValue.Active
        //                                      && b.LocationID != this.LocationID)
        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Location Code already exists ", this.LocationCode);
        //                    args.FieldName = "LocationCode";
        //                    return false;
        //                }

        //                if (this.ParentLocationID == null && this.LocationTypeID != null)
        //                {
        //                    args.ErrorMessage = string.Format("Second Level Location only LocationType is mandatory ", this.LocationCode);
        //                    args.FieldName = "LocationCode";
        //                    return false;
        //                }

        //                if (this.ParentLocationID != null)
        //                {
        //                    var loc = (from b in args.NewDB.LocationTable where b.LocationID == this.ParentLocationID select b).FirstOrDefault();
        //                    if (loc.ParentLocationID != null && this.LocationTypeID != null)
        //                    {
        //                        args.ErrorMessage = string.Format("Second Level Location only LocationType is mandatory ", this.LocationCode);
        //                        args.FieldName = "LocationCode";
        //                        return false;
        //                    }
        //                    else if (loc.ParentLocationID == null && this.LocationTypeID == null)
        //                    {
        //                        args.ErrorMessage = string.Format("Second Level Location required LocationType Please select it ", this.LocationCode);
        //                        args.FieldName = "LocationCode";
        //                        return false;
        //                    }
        //                }

        //                return true;
        //            }

        //        case EntityObjectState.Delete:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.AssetTable
        //                                      where (b.LocationID == this.LocationID)
        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("references found", this.LocationID);
        //                    args.FieldName = "LocationCode";
        //                    return false;
        //                }
        //                return true;
        //            }
        //    }
        //    return true;
        //}

        public static async Task<prc_GetSecondLevelLocationValueResult> Getresult(AMSContext _db, int locationID,string assetIDs)
        {
            try
            {
                var res = await _db.GetProcedures().prc_GetSecondLevelLocationValueAsync(locationID,assetIDs);
                return res.FirstOrDefault();
            }
            catch (Exception ex)

            {
                ApplicationErrorLogTable.SaveException(ex);
                throw ex;
            }
        }
        public  static int? GetSecondLevelID(AMSContext _db,int locationID)
        {
            var res = (from b in _db.LocationNewView where b.LocationID == locationID select b.Level2ID).FirstOrDefault();
            return res;
        }
        public static async Task<List<prc_ExportToLocationResult>> GetExportExceo(AMSContext _db, int userID,string filterData)
        {
            try
            {
                var res = await _db.GetProcedures().prc_ExportToLocationAsync(userID, filterData);
                return res;
            }
            catch (Exception ex)

            {
                ApplicationErrorLogTable.SaveException(ex);
                throw ex;
            }
        }
        public static async Task<List<prc_ExportToCategoryResult>> GetExportCategoryExceo(AMSContext _db, int userID,string filterData)
        {
            try
            {
                var res = await _db.GetProcedures().prc_ExportToCategoryAsync(userID, filterData);
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
