namespace ACS.AMS.WebApp.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public Exception Error { get; set; }

        public string GetException()
        {
            if (Error != null)
            {
                if (WebApplication.Create().Environment.IsDevelopment())
                {
                    var str = Error.Message;

                    return str;
                }
            }

            return "";
        }
    }
}
