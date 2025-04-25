using ACS.LMS.WebApp.Models.Mobile;
using System.Collections.Generic;

namespace ACS.AMS.WebApp.Models.Mobile
{
    public class GetAllSectionsResponse : BaseResponse
    {
        public GetAllSectionsResponse()
        {

        }

        public List<SectionData> Sections { get; set; }
    }
}
