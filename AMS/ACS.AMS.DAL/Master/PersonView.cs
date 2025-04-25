using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ACS.AMS.DAL;


namespace ACS.AMS.DAL.DBModel
{
   public partial class PersonView
    {
        public static IQueryable<PersonView> GetAllPerson(AMSContext db)
        {
            return db.PersonView.AsQueryable();
        }
    }
}
