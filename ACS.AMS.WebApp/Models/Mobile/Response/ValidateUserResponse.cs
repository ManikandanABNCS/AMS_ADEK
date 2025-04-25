namespace ACS.AMS.WebApp.Models.Mobile
{
    public class ValidateUserResponse : BaseResponse
    {
        public ValidateUserResponse()
        {

        }

        public int UserID { get; set; }

        public string Fullname { get; set; }

        public List<string> AllowedRights { get; set; }

        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public DateTime expiration { get; set; }

        public string token { get; set; }
    }
}
