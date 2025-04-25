namespace ACS.AMS.WebApp.Models.Mobile
{
    public class ApprovalRoleData
    {
        public ApprovalRoleData()
        {

        }
        public int ApprovalRoleID { get; set; }
        public string ApprovalRoleCode { get; set; }
        public string ApprovalRoleName { get; set; }
        public override string ToString()
        {
            return $"{ApprovalRoleCode} ({ApprovalRoleName})";
        }
    }
}
