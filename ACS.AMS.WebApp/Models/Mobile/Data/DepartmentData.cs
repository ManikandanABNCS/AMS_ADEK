namespace ACS.LMS.WebApp.Models.Mobile
{
    public class DepartmentData
    {
        public DepartmentData()
        {

        }

        public int DepartmentID { get; set; }

        public string DepartmentCode { get; set; }

        public string DepartmentName { get; set; }

        public override string ToString()
        {
            return $"{DepartmentCode} ({DepartmentName})";
        }
    }
}
