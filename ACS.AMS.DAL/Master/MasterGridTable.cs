using ACS.AMS.DAL.DBContext;

namespace ACS.AMS.DAL.DBModel
{
    public partial class MasterGridNewTable
    {
        public static int GetTragetIndexID(AMSContext db, string targetName)
        {
            int result = (from b in db.MasterGridNewTable
                          where b.MasterGridName == targetName
                          select b.MasterGridID).FirstOrDefault();
            return result;
        }

    }
}
