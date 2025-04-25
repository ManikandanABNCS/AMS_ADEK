using ACS.AMS.DAL.DBModel;

namespace ACS.AMS.WebApp.Models
{
    public class TransferAssetData
    {
        private static int idGenerator = -1;

        public TransferAssetData()
        {
            if (idGenerator < -100000)
                idGenerator = -1;

            id = idGenerator--;
        }

        public int id { get; set; }
        public string RoomCode { get; set; }
        public AssetTable Asset { get; set; }

      



    }
}
