using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ACS.AMS.DAL;
using ACS.AMS.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using ACS.WebAppPageGenerator.Models.SystemModels;

namespace ACS.AMS.DAL.DBModel
{
   public partial class PeriodTable : BaseEntityObject
    {
        public static PeriodTable GetLastPeriodWithoutYear(AMSContext db)
        {
            return (from b in db.PeriodTable
                    where b.StatusID != (int)StatusValue.Deleted && b.StatusID != (int)StatusValue.DeletedOLD
                    select b).OrderByDescending(i => i.EndDate).FirstOrDefault();
        }

        internal override bool ValidateNewObject(ValidateEventArgs<AMSContext> args)
        {
            var existingItem = (from b in args.NewDB.PeriodTable
                                where b.StatusID != (int)StatusValue.Deleted && b.StatusID != (int)StatusValue.DeletedOLD && b.PeriodName.ToUpper() == this.PeriodName.ToUpper() && b.Year == this.Year
                                select new { PeriodName = b.PeriodName, Year = b.Year }).FirstOrDefault();
            if (existingItem != null)
            {
                if (string.Compare(existingItem.PeriodName, this.PeriodName.Trim(), true) == 0)
                {
                    args.FieldName = "PeriodName"; //this is an parent table - so add ItemName_ infront of each fields
                    args.ErrorMessage = string.Format(Language.GetErrorMessage("PeriodNameAlreadyExists"), Language.GetFieldName("PeriodName", CultureHelper.EnglishCultureSymbol), PeriodName);
                    return false;
                }
            }

            if (this.StartDate.Year == this.Year && this.EndDate.Year == this.Year)
            {
            }
            else
            {
                args.ErrorMessage = string.Format(Language.GetErrorMessage("YouHaveSelectedWrongYear"), Language.GetFieldName("EndDate", CultureHelper.EnglishCultureSymbol), EndDate);
                return false;
            }

            var checkDate = (from b in args.NewDB.PeriodTable
                             join c in args.NewDB.DepreciationTable on b.PeriodID equals c.PeriodID
                             where c.StatusID == (int)StatusValue.Active
                             orderby c.DepreciationID descending
                             select b).FirstOrDefault();
            if (checkDate != null)
            {
                if (DateTime.Compare(checkDate.StartDate, this.EndDate) > 0)
                {
                    args.ErrorMessage = string.Format(Language.GetErrorMessage("YouCreatingPeriodForLessThanDepreciationPeriod"), Language.GetFieldName("EndDate", CultureHelper.EnglishCultureSymbol), EndDate);
                    return false;
                }
            }

            var allDate = PeriodTable.GetAllItems(args.NewDB);
            foreach (var chk in allDate)
            {
                int endStartDate = DateTime.Compare(chk.EndDate, this.StartDate);
                int startDate = DateTime.Compare(chk.StartDate, this.StartDate);
                int endDate = DateTime.Compare(chk.EndDate, this.EndDate);
                int startEndDate = DateTime.Compare(chk.StartDate, this.EndDate);
                if (DateTime.Compare(this.StartDate, this.EndDate) < 0)
                {
                    if ((endStartDate >= 0 && startDate <= 0) || (endDate >= 0 && startEndDate <= 0))
                    {
                        args.ErrorMessage = string.Format(Language.GetErrorMessage("PeriodDatesOverlapping"), Language.GetFieldName("EndDate", CultureHelper.EnglishCultureSymbol), EndDate);
                        return false;
                    }
                    else
                    {
                        if (DateTime.Compare(this.StartDate, chk.StartDate) < 0)
                        {
                            if (DateTime.Compare(chk.StartDate, this.EndDate) < 0)
                            {
                                args.ErrorMessage = string.Format(Language.GetErrorMessage("PeriodDatesOverlapping"), Language.GetFieldName("EndDate", CultureHelper.EnglishCultureSymbol), EndDate);
                                return false;
                            }
                        }

                    }
                }
            }

            var method = ConfigurationTable.GetConfigurationValue(args.NewDB, "DepreciationMethod");
            if (method != "MD")
            {
                if (this.EndDate != this.StartDate.AddMonths(1).AddDays(-1))
                {
                    args.ErrorMessage = string.Format(Language.GetErrorMessage("PleaseSelectTheEndOfTheMonth"), Language.GetFieldName("EndDate", CultureHelper.EnglishCultureSymbol), EndDate);
                    return false;
                }
            }
            return true;
        }

        internal override bool ValidateUpdateObject(ValidateEventArgs<AMSContext> args)
        {
            var existingItem = (from b in args.NewDB.PeriodTable
                                where b.PeriodName == this.PeriodName && b.Year == this.Year && b.PeriodID != this.PeriodID && b.StatusID != (int)StatusValue.Deleted && b.StatusID != (int)StatusValue.DeletedOLD
                                select new { PeriodName = b.PeriodName, Year = b.Year }).FirstOrDefault();
            if (existingItem != null)
            {
                if (string.Compare(existingItem.PeriodName, this.PeriodName.Trim(), true) == 0)
                {
                    args.FieldName = "PeriodName"; //this is an parent table - so add ItemName_ infront of each fields
                    args.ErrorMessage = string.Format(Language.GetErrorMessage("PeriodNameAlreadyExists"), Language.GetFieldName("PeriodName", CultureHelper.EnglishCultureSymbol), PeriodName);
                    return false;
                }

            }
            if (this.StartDate.Year == this.Year && this.EndDate.Year == this.Year)
            {
            }
            else
            {
                args.ErrorMessage = string.Format(Language.GetErrorMessage("YouHaveSelectedWrongYear"), Language.GetFieldName("EndDate", CultureHelper.EnglishCultureSymbol), EndDate);
                return false;
            }
            var method = ConfigurationTable.GetConfigurationValue(args.NewDB, "DepreciationMethod");
            if (method != "MD")
            {
                if (this.EndDate != this.StartDate.AddMonths(1).AddDays(-1))
                {
                    args.ErrorMessage = string.Format(Language.GetErrorMessage("PleaseSelectTheEndOftheMonth"), Language.GetFieldName("EndDate", CultureHelper.EnglishCultureSymbol), EndDate);
                    return false;
                }
            }

            if (DateTime.Compare(this.StartDate, this.EndDate) > 0)
            {
                args.ErrorMessage = string.Format(Language.GetErrorMessage("YouCreatingPeriodForLessThanDepreciationPeriod"), Language.GetFieldName("EndDate", CultureHelper.EnglishCultureSymbol), EndDate);
                return false;
            }

            var allDate = PeriodTable.GetAllItems(args.NewDB);

            foreach (var chk in allDate)
            {
                if (this.PeriodID != chk.PeriodID)
                {
                    int endStartDate = DateTime.Compare(chk.EndDate, this.StartDate);
                    int startDate = DateTime.Compare(chk.StartDate, this.StartDate);
                    int endDate = DateTime.Compare(chk.EndDate, this.EndDate);
                    int startEndDate = DateTime.Compare(chk.StartDate, this.EndDate);
                    if (DateTime.Compare(this.StartDate, this.EndDate) < 0)
                    {
                        if ((endStartDate >= 0 && startDate <= 0) || (endDate >= 0 && startEndDate <= 0))
                        {
                            args.ErrorMessage = string.Format(Language.GetErrorMessage("PeriodDatesOverlapping"), Language.GetFieldName("EndDate", CultureHelper.EnglishCultureSymbol), EndDate);
                            return false;
                        }
                        else
                        {
                            if (DateTime.Compare(this.StartDate, chk.StartDate) < 0)
                            {
                                if (DateTime.Compare(chk.StartDate, this.EndDate) < 0)
                                {
                                    args.ErrorMessage = string.Format(Language.GetErrorMessage("PeriodDatesOverlapping"), Language.GetFieldName("EndDate", CultureHelper.EnglishCultureSymbol), EndDate);
                                    return false;
                                }
                            }

                        }
                    }
                }
            }
            return true;
        }
        internal override bool ValidateDeleteObject(ValidateEventArgs<AMSContext> args)
        {
            var item = (from b in args.NewDB.DepreciationTable
                        where b.PeriodID == this.PeriodID && b.StatusID != (int)StatusValue.Deleted && b.StatusID != (int)StatusValue.DeletedOLD
                        select b).Count();

            if (item > 0)
            {

                args.ErrorMessage = string.Format(Language.GetErrorMessage("ReferenceFound"), Language.GetEntityName("DepreciationTable"));
                return false;
            }

            var PerItem = (from b in args.NewDB.PeriodTable
                           where b.StatusID != (int)StatusValue.Deleted && b.StatusID != (int)StatusValue.DeletedOLD
                           select b);
            if (PerItem.Count() > 0)
            {
                int _perItem = (from b in PerItem select b.PeriodID).Max();
                if (_perItem > this.PeriodID)
                {

                    args.ErrorMessage = string.Format(Language.GetErrorMessage("ShouldNotAllowToDeletePreviousRecord"), Language.GetEntityName("PeriodTable"));
                    return false;
                }
            }
            return true;
        }
        public static DateTime GetStartDate(AMSContext db)
        {
            var checkDate = PeriodTable.GetLastPeriodWithoutYear(db);
            if (checkDate != null)
            {
                return checkDate.EndDate.AddDays(1);
            }
            else
            {
                return AppConfigurationManager.GetValue<DateTime>(AppConfigurationManager.PeriodStartDate);
            }

        }

        public override PageFieldModelCollection GetCreateScreenControls(string subpageName, int userID)
        {
            var allList = base.GetCreateScreenControls(subpageName, userID);

            PageFieldModelCollection allowedCols = new PageFieldModelCollection()
            {
                new PageFieldModel() { FieldName = "PeriodName", IsMandatory = true },
                new PageFieldModel() { FieldName = "Year", IsMandatory = false,IsHidden=true },
                new PageFieldModel() { FieldName = "StartDate", IsMandatory = true },
                 new PageFieldModel() { FieldName = "EndDate", IsMandatory = true },
            };

            PageFieldModelCollection newList = new PageFieldModelCollection();
            foreach (var c in allowedCols)
            {
                var itm = allList.Where(b => string.Compare(b.FieldName, c.FieldName, true) == 0).FirstOrDefault();
                if (itm != null)
                {
                    itm.IsMandatory = c.IsMandatory;
                    itm.IsHidden = c.IsHidden;
                    newList.Add(itm);
                }
            }

            return newList;
        }
    }

}
