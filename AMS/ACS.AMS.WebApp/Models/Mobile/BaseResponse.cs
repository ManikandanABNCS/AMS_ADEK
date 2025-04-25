namespace ACS.AMS.WebApp.Models.Mobile
{
    public class BaseResponse
    {
        public BaseResponse()
        {

        }

        public string Message { get; set; }

        public bool IsSuccess { get; set; } = true;

        public bool IsDataSaved { get; set; } = true;

        public long TransactionID { get; set; }

        public string TransactionNo { get; set; }
    }
}
