using System.ComponentModel.DataAnnotations;

namespace ACS.AMS.WebApp.Models
{
    public class JWTLoginModel
    {
        public Version ClientAppVersion { get; set; }

        public string? Username { get; set; }

    
        public string? Password { get; set; }
    }
}
