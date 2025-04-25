using ACS.LMS.WebApp.Models.Mobile;
using System.Collections.Generic;

namespace ACS.AMS.WebApp.Models.Mobile
{
    public class GetAllManufacturerResponse : BaseResponse
    {
        public GetAllManufacturerResponse()
        {

        }

        public List<ManufacturerData> Manufacturers { get; set; }
    }
}
