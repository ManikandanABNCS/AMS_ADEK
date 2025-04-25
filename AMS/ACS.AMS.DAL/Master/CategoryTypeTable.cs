using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Castle.DynamicProxy;

namespace ACS.AMS.DAL.DBModel
{
    public partial class CategoryTypeTable:BaseEntityObject
    {
        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.CategoryTypeTable
                                  where b.CategoryTypeCode == this.CategoryTypeCode

                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();
            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Category Type Code already exists ", this.CategoryTypeCode);
                args.FieldName = "CategoryTypeCode";
                return false;
            }
            var allCategory = (from b in args.NewDB.CategoryTypeTable
                                  where b.IsAllCategoryType == this.IsAllCategoryType
                                  && b.StatusID == (int)StatusValue.Active
                               select new { IsAllCategoryType = b.IsAllCategoryType }).FirstOrDefault();
            if (allCategory != null)
            {
                if (allCategory.IsAllCategoryType==true)
                {
                    args.ErrorMessage = string.Format("Is All Category allow only one time already added ", this.CategoryTypeCode);
                    args.FieldName = "CategoryTypeCode";
                    return false;
                }
            }
            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.CategoryTypeTable
                                  where (b.CategoryTypeCode == this.CategoryTypeCode && b.StatusID == (int)StatusValue.Active
                                  && b.CategoryTypeID != this.CategoryTypeID)

                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Category Type Code already exists", this.CategoryTypeID);
                args.FieldName = "CategoryType";
                return false;
            }
            var allCategory = (from b in args.NewDB.CategoryTypeTable
                               where b.IsAllCategoryType == this.IsAllCategoryType && b.CategoryTypeID!=this.CategoryTypeID
                               && b.StatusID == (int)StatusValue.Active
                               select new { IsAllCategoryType = b.IsAllCategoryType }).FirstOrDefault();
            if (allCategory != null)
            {
                if (allCategory.IsAllCategoryType == true)
                {
                    args.ErrorMessage = string.Format("Is All Category allow only one time already added ", this.CategoryTypeCode);
                    args.FieldName = "CategoryTypeCode";
                    return false;
                }
            }
            return true;
        }
        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.CategoryTable
                                  where (b.CategoryTypeID == this.CategoryTypeID)
                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Category Type references found", this.CategoryTypeID);
                args.FieldName = "CategoryTypeID";
                return false;
            }
            var checkRoleDuplicate = (from b in args.NewDB.UserApprovalRoleMappingTable
                                  where (b.CategoryTypeID == this.CategoryTypeID)
                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();

            if (checkRoleDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Category Type references found", this.CategoryTypeID);
                args.FieldName = "CategoryTypeID";
                return false;
            }

            var ApprovalWorkFlowTo = (from b in args.NewDB.ApprovalHistoryTable
                                      where (b.CategoryTypeID == this.CategoryTypeID)
                                      && b.StatusID == (int)StatusValue.Active
                                      select b).Count();

            if (ApprovalWorkFlowTo > 0)
            {
                args.ErrorMessage = string.Format("Category Type references found", this.CategoryTypeID);
                args.FieldName = "CategoryTypeID";
                return false;
            }
            
            return true;
        }
        
        public static CategoryTypeTable GetCategoryTypeDetails(AMSContext db, string categoryType)
        {
            var res = (from b in db.CategoryTypeTable where b.CategoryTypeName == categoryType && b.StatusID==(int)StatusValue.Active select b).FirstOrDefault();
            return res;
        }

        public static CategoryTypeTable insertAllType(AMSContext _db)
        {
            CategoryTypeTable type = new CategoryTypeTable();
            type.CategoryTypeCode = "All";
            type.CategoryTypeName = "All";
            type.StatusID = (int)StatusValue.Active;
            _db.Add(type);
            _db.SaveChanges();
            _db.Entry(type).Reload();
            return type;
        }
    }
}
