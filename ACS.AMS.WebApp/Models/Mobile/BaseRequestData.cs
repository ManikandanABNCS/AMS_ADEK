namespace ACS.AMS.WebApp.Models.Mobile
{
    public partial class BaseRequestData
    {
        public BaseRequestData()
        {
            InitParameters();
        }

        partial void InitParameters();

        public string DeviceSerialNo { get; set; }

        public int UserID { get; set; }

       // public int WarehouseID { get; set; }
    }
    public partial class BaseRequestData1
    {
        public BaseRequestData1()
        {
            InitParameters();
        }

        partial void InitParameters();

        public string DeviceSerialNo { get; set; }

        public string UserCode { get; set; }

    }
}
