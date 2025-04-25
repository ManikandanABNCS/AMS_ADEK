using ACS.LMS.WebApp.Models.Mobile;
using System.Collections.Generic;

namespace ACS.AMS.WebApp.Models.Mobile
{
    public class GetAllSuppliersResponse : BaseResponse
    {
        public GetAllSuppliersResponse()
        {

        }

        public List<SupplierData> Suppliers { get; set; }
    }
}
