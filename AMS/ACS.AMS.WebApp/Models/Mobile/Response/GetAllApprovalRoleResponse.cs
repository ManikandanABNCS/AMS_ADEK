using System.Collections.Generic;

namespace ACS.AMS.WebApp.Models.Mobile
{
    public class GetAllApprovalRoleResponse : BaseResponse
    {
        public GetAllApprovalRoleResponse()
        {

        }

        public List<ApprovalRoleData> ApprovalRole { get; set; }
    }
}
