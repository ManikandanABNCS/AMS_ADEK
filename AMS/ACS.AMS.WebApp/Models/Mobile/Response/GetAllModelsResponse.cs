using System.Collections.Generic;

namespace ACS.AMS.WebApp.Models.Mobile
{
    public class GetAllModelsResponse : BaseResponse
    {
        public GetAllModelsResponse()
        {

        }

        public List<ModelData> Models { get; set; }
    }
}
