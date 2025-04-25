using System.Collections.Generic;

namespace ACS.AMS.WebApp.Models.Mobile
{
    public class GetAllCompaniesResponse : BaseResponse
    {
        public GetAllCompaniesResponse()
        {

        }

        public List<CompanyData> Companies { get; set; }
    }
}
