using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.AMS.DAL.Models
{
    public class ApproveWorkflowLineModel
    {
        public int ApproveWorkflowID { get; set; }
        public int ApprovalRoleID { get; set; }
        public string ApprovalRoleCode { get; set; }
        public string ApprovalRoleName { get; set; }
        public int OrderNo { get; set; }
        
    }
}
