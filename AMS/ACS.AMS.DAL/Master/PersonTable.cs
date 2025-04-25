using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace ACS.AMS.DAL.DBModel
{
    public partial class PersonTable : BaseEntityObject
    {
        public static string GetPersonUserName(AMSContext db, int? userID)
        {
            if (userID != null)
            {
                var user = db.User_LoginUserTable.Where(b => b.UserID == (int)userID).FirstOrDefault();
                if (user != null)
                    return user.UserName;
                else
                    return "";
            }
            else
            {
                return "";
            }

        }
        public static PersonTable GetPerson(AMSContext db, string personcode)
        {
            return db.PersonTable.Where(b => b.PersonCode == personcode).FirstOrDefault();
        }

        public static PersonTable GetPersonBasedOnID(AMSContext db, int personID)
        {
            var result = (from b in db.PersonTable.Include("Department").Include("Designation").Include("Status")
                          where b.PersonID == personID
                          select b
                        ).FirstOrDefault();
            return result;

            // return db.PersonTable.Where(b => b.PersonID == personID).FirstOrDefault();
        }

        public static PersonTable GetPersonByEmail(AMSContext entityObject, string eMailID)
        {
            var result = (from b in entityObject.PersonTable
                          where b.EMailID == eMailID && b.StatusID != (int)StatusValue.Deleted && b.StatusID != (int)StatusValue.DeletedOLD
                          select b).FirstOrDefault();

            return result;
        }
        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.PersonTable
                                  where b.PersonCode == this.PersonCode
                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();
            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("PersonCode Code already exists ", this.PersonCode);
                args.FieldName = "PersonCode";
                return false;
            }
            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.PersonTable
                                  where (b.PersonCode == this.PersonCode && b.StatusID == (int)StatusValue.Active
                                  && b.PersonID != this.PersonID)
                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("Person Code already exists ", this.PersonCode);
                args.FieldName = "PersonCode";
                return false;
            }
            return true;
        }
        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            var checkDuplicate = (from b in args.NewDB.AssetTable
                                  where (b.CreatedBy == this.PersonID)
                                  && b.StatusID == (int)StatusValue.Active
                                  select b).Count();

            if (checkDuplicate > 0)
            {
                args.ErrorMessage = string.Format("references found", this.PersonID);
                args.FieldName = "PersonID";
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
        //                var checkDuplicate = (from b in args.NewDB.PersonTable
        //                                      where b.PersonCode == this.PersonCode
        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();
        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("PersonCode Code already exists ", this.PersonCode);
        //                    args.FieldName = "PersonCode";
        //                    return false;
        //                }
        //                return true;
        //            }

        //        case EntityObjectState.Edit:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.PersonTable
        //                                      where (b.PersonCode == this.PersonCode && b.StatusID == (int)StatusValue.Active
        //                                      && b.PersonID != this.PersonID)
        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("Person Code already exists ", this.PersonCode);
        //                    args.FieldName = "PersonCode";
        //                    return false;
        //                }

        //                return true;
        //            }

        //        case EntityObjectState.Delete:
        //            {
        //                var checkDuplicate = (from b in args.NewDB.AssetTable
        //                                      where (b.CreatedBy == this.PersonID)
        //                                      && b.StatusID == (int)StatusValue.Active
        //                                      select b).Count();

        //                if (checkDuplicate > 0)
        //                {
        //                    args.ErrorMessage = string.Format("references found", this.PersonID);
        //                    args.FieldName = "PersonID";
        //                    return false;
        //                }
        //                return true;
        //            }
        //    }
            
        //    return true;
        //}

        partial void OnDelete()
        {
            
        }

        public static IQueryable<UserApprovalRoleMappingTable> GetList(AMSContext _db, int userID)
        {
            var result = (from b in _db.UserApprovalRoleMappingTable where b.UserID == userID && b.StatusID == (int)StatusValue.Active select b);
            return result;
        }

        public static IQueryable<UserLocationMappingTable> GetUserBasedLocationList(AMSContext _db, int userID)
        {
            var result = (from b in _db.UserLocationMappingTable where b.PersonID == userID && b.StatusID == (int)StatusValue.Active select b);
            return result;
        }

        public static UserApprovalRoleMappingTable validateRole(AMSContext _db, int userID, int locationid, int approvalID)
        {
            var result = (from b in _db.UserApprovalRoleMappingTable
                          where b.UserID == userID
                          && b.StatusID == (int)StatusValue.Active
                          && b.LocationID == locationid && b.ApprovalRoleID == approvalID

                          select b).FirstOrDefault();
            return result;
        }

        public static void deleteApprovalRole(AMSContext db, int personID)
        {
            var result = (from b in db.UserApprovalRoleMappingTable
                          where b.UserID == personID && b.StatusID==(int)StatusValue.Active
                          select b);

            if (result.Count() > 0)
            {
                foreach (var res in result)
                {
                    res.StatusID = (int)StatusValue.Deleted;
                    //db.UserApprovalRoleMappingTable.Remove(res);
                }
                db.SaveChanges();

                // db.Dispose();
            }
        }
        
        public static void deleteCategoryLinteItem(AMSContext db, int personID)
        {
            var result = (from b in db.UserCategoryMappingTable
                          where b.PersonID == personID && b.StatusID==(int)StatusValue.Active
                          select b);
            if (result.Count() > 0)
            {
                foreach (var res in result)
                {
                    // db.UserCategoryMappingTable.Remove(res);
                    res.StatusID = (int)StatusValue.Deleted;
                }
                db.SaveChanges();

                // db.Dispose();

            }
        }

        public static void deleteLocationLinteItem(AMSContext db, int personID)
        {
            var result = (from b in db.UserLocationMappingTable
                          where b.PersonID == personID && b.StatusID == (int)StatusValue.Active
                          select b);
            if (result.Count() > 0)
            {
                foreach (var res in result)
                {
                    // db.UserLocationMappingTable.Remove(res);
                    res.StatusID = (int)StatusValue.Deleted;
                }
                db.SaveChanges();

                // db.Dispose();
            }
        }
        public static void deletecompanyLinteItem(AMSContext db, int personID)
        {
            var result = (from b in db.UserCompanyMappingTable
                          where b.UserID == personID && b.StatusID == (int)StatusValue.Active
                          select b);
            if (result.Count() > 0)
            {
                foreach (var res in result)
                {
                    // db.UserCompanyMappingTable.Remove(res);
                    res.StatusID = (int)StatusValue.Deleted;
                }
                db.SaveChanges();

                // db.Dispose();
            }
        }
        public static void deleteDepartmentLinteItem(AMSContext db, int personID)
        {
            var result = (from b in db.UserDepartmentMappingTable
                          where b.PersonID == personID && b.StatusID == (int)StatusValue.Active
                          select b);

            if (result.Count() > 0)
            {
                foreach (var res in result)
                {
                    //db.UserDepartmentMappingTable.Remove(res);
                    res.StatusID = (int)StatusValue.Deleted;
                }
                db.SaveChanges();

                // db.Dispose();

            }
        }

        public static IQueryable<UserApprovalRoleMappingTable> GetUserRoleMappingDetails(AMSContext db, int userID)
        {
            var result = (from b in db.UserApprovalRoleMappingTable where b.UserID == userID && b.StatusID==(int)StatusValue.Active select b);
            return result;
        }
        public static IQueryable<UserCompanyMappingTable> GetAllCompanyDetails(AMSContext db, int userID)
        {
            var result = UserCompanyMappingTable.GetAllItems(db).Where(a => a.UserID == userID && a.StatusID==(int)StatusValue.Active );
            return result;

        }
        public static List<int> GetMappingID(string objectID)
        {
            List<int> items = new List<int>();
            if (!string.IsNullOrEmpty(objectID))
            {
                string trimDepatID = objectID.TrimEnd(',');
                string[] arrindate = trimDepatID.Split(',');

                foreach (string s in arrindate)
                {
                    int val = 0;
                    if (int.TryParse(s, out val))
                        items.Add(val);
                }
            }
            else
            {
                items = null;
            }
            return items;
        }
    }
}