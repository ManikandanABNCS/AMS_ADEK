using ACS.AMS.WebApp.Products.Mobile;
using ACS.LMS.WebApp.Models.Mobile;
using System.Collections.Generic;

namespace ACS.AMS.WebApp.Models.Mobile
{
    public class GetAllProductsResponse : BaseResponse
    {
        public GetAllProductsResponse()
        {

        }

        public List<ProductData> Products { get; set; }
    }
}
