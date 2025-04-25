using System.Collections.Generic;

namespace ACS.AMS.WebApp.Models.Mobile
{
    public class GetAllLocationsResponse : BaseResponse
    {
        public GetAllLocationsResponse()
        {

        }

        public List<LocationData> Locations { get; set; }
    }
}
