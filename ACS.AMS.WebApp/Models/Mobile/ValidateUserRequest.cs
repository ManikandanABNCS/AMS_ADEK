namespace ACS.AMS.WebApp.Models.Mobile
{
    public class ValidateUserRequest : BaseRequestData
    {
        public ValidateUserRequest()
        {

        }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
