using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ACS.AMS.DAL.DBModel
{
    public partial class LocationTypeTable
    {
        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.LocationTypeTable
                                  where b.LocationTypeCode == this.LocationTypeCode

                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();
            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Location Type Code already exists ", this.LocationTypeCode);
                args.FieldName = "LocationTypeCode";
                return false;
            }
            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.LocationTypeTable
                                  where (b.LocationTypeCode == this.LocationTypeCode && b.StatusID == (int)StatusValue.Active
                                  && b.LocationTypeID != this.LocationTypeID)

                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Location Type Code already exists", this.LocationTypeID);
                args.FieldName = "LocationType";
                return false;
            }
            return true;
        }
        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.LocationTable
                                  where (b.LocationTypeID == this.LocationTypeID)
                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Location Type references found", this.LocationTypeID);
                args.FieldName = "LocationTypeID";
                return false;
            }
            var checkworkDuplicate = (from b in args.NewDB.ApproveWorkflowTable
                                      where (b.FromLocationTypeID == this.LocationTypeID)
                                      && b.StatusID == (int)StatusValue.Active
                                      select b).Count();

            if (checkworkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Location Type references found", this.LocationTypeID);
                args.FieldName = "LocationTypeID";
                return false;
            }
            var checkworkToDuplicate = (from b in args.NewDB.ApproveWorkflowTable
                                        where (b.ToLocationTypeID == this.LocationTypeID)
                                        && b.StatusID == (int)StatusValue.Active
                                        select b).Count();

            if (checkworkToDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Location Type references found", this.LocationTypeID);
                args.FieldName = "LocationTypeID";
                return false;
            }
            var ApprovalWorkFlowTo = (from b in args.NewDB.ApprovalHistoryTable
                                      where (b.ToLocationTypeID == this.LocationTypeID)
                                      && b.StatusID == (int)StatusValue.Active
                                      select b).Count();

            if (ApprovalWorkFlowTo > 0)
            {
                args.ErrorMessage = string.Format("Location Type references found", this.LocationTypeID);
                args.FieldName = "LocationTypeID";
                return false;
            }
            var ApprovalWorkFlowFrom = (from b in args.NewDB.ApprovalHistoryTable
                                        where (b.FromLocationTypeID == this.LocationTypeID)
                                        && b.StatusID == (int)StatusValue.Active
                                        select b).Count();

            if (ApprovalWorkFlowFrom > 0)
            {
                args.ErrorMessage = string.Format("Location Type references found", this.LocationTypeID);
                args.FieldName = "LocationTypeID";
                return false;
            }
            return true;
        }
        public static LocationTypeTable GetAllLocationType(AMSContext _db, string locationType)
        {
            var query=(from b in _db.LocationTypeTable where b.LocationTypeName==locationType && b.StatusID==(int)StatusValue.Active select b).FirstOrDefault();
           //if(query==null)
           // {
           //     insertAllType(_db);
           //     GetAllLocationType(_db, "All");
           // }
            
            return query;

        }
        public static LocationTypeTable insertAllType(AMSContext _db)
        {
            LocationTypeTable type = new LocationTypeTable();
            type.LocationTypeCode = "All";
            type.LocationTypeName = "All";
            type.StatusID = (int)StatusValue.Active;
            _db.Add(type);
            _db.SaveChanges();
            _db.Entry(type).Reload();
            return type;
        }
        //internal override bool ValidateObject(ValidateEventArgs<AMSContext> args)
        //{
        //    switch (args.State)
        //    {
        //        case EntityObjectState.New:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.LocationTypeTable
        //                                      where b.LocationTypeCode == this.LocationTypeCode

        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();
        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Location Type already exists ", this.LocationTypeCode);
        //                    args.FieldName = "LocationTypeCode";
        //                    return false;
        //                }
        //                return true;
        //            }

        //        case EntityObjectState.Edit:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.LocationTypeTable
        //                                      where (b.LocationTypeCode == this.LocationTypeCode && b.StatusID == (int)StatusValue.Active
        //                                      && b.LocationTypeID != this.LocationTypeID)

        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Location Type already exists", this.LocationTypeID);
        //                    args.FieldName = "LocationType";
        //                    return false;
        //                }

        //                return true;
        //            }

        //        case EntityObjectState.Delete:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.LocationTable
        //                                      where (b.LocationTypeID == this.LocationTypeID)
        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Location Type references found", this.LocationTypeID);
        //                    args.FieldName = "LocationTypeID";
        //                    return false;
        //                }
        //                var checkworkDuplicate = (from b in args.NewDB.ApproveWorkflowTable
        //                                      where (b.FromLocationTypeID == this.LocationTypeID)
        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();

        //                if (checkworkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Location Type references found", this.LocationTypeID);
        //                    args.FieldName = "LocationTypeID";
        //                    return false;
        //                }
        //                var checkworkToDuplicate = (from b in args.NewDB.ApproveWorkflowTable
        //                                          where (b.ToLocationTypeID == this.LocationTypeID)
        //                                          && b.StatusID == (int)StatusValue.Active
        //                                          select b).Count();

        //                if (checkworkToDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Location Type references found", this.LocationTypeID);
        //                    args.FieldName = "LocationTypeID";
        //                    return false;
        //                }
        //                var ApprovalWorkFlowTo = (from b in args.NewDB.ApprovalHistoryTable
        //                                            where (b.ToLocationTypeID == this.LocationTypeID)
        //                                            && b.StatusID == (int)StatusValue.Active
        //                                            select b).Count();

        //                if (ApprovalWorkFlowTo > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Location Type references found", this.LocationTypeID);
        //                    args.FieldName = "LocationTypeID";
        //                    return false;
        //                }
        //                var ApprovalWorkFlowFrom = (from b in args.NewDB.ApprovalHistoryTable
        //                                          where (b.FromLocationTypeID == this.LocationTypeID)
        //                                          && b.StatusID == (int)StatusValue.Active
        //                                          select b).Count();

        //                if (ApprovalWorkFlowFrom > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Location Type references found", this.LocationTypeID);
        //                    args.FieldName = "LocationTypeID";
        //                    return false;
        //                }
        //                return true;
        //            }
        //    }
        //    return true;
        //}
    }
}
