using System.Collections.Generic;

namespace ACS.AMS.WebApp.Models.Mobile
{
    public class GetAllCustodiansResponse : BaseResponse
    {
        public GetAllCustodiansResponse()
        {

        }

        public List<CustodianData> Custodians { get; set; }
    }
}
