using System.Collections.Generic;

namespace ACS.AMS.WebApp.Models.Mobile
{
    public class GetAllCategoriesResponse : BaseResponse
    {
        public GetAllCategoriesResponse()
        {

        }

        public List<CategoryData> Categories { get; set; }
    }
}
