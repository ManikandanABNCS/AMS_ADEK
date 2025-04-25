using System.Collections.Generic;

namespace ACS.AMS.WebApp.Models.Mobile
{
    public class GetAllAssetConditionsResponse : BaseResponse
    {
        public GetAllAssetConditionsResponse()
        {

        }

        public List<AssetConditionData> AssetConditions { get; set; }
    }
}
