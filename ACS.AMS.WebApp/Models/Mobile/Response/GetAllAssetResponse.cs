using System.Collections.Generic;

namespace ACS.AMS.WebApp.Models.Mobile
{
    public class GetAllAssetResponse : BaseResponse
    {
        public GetAllAssetResponse()
        {

        }

        public List<AssetData> Asset { get; set; }
    }
}
