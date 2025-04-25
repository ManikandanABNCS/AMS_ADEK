using ACS.LMS.WebApp.Models.Mobile;
using System.Collections.Generic;

namespace ACS.AMS.WebApp.Models.Mobile
{
    public class GetAllDepartmentResponse : BaseResponse
    {
        public GetAllDepartmentResponse()
        {

        }

        public List<DepartmentData> Departments { get; set; }
    }
}
