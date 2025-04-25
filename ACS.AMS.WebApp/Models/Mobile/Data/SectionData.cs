namespace ACS.LMS.WebApp.Models.Mobile
{
    public class SectionData
    {
        public SectionData()
        {

        }

        public int SectionID { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public int DepartmentID { get; set; }
        
        public override string ToString()
        {
            return $"{SectionCode} ({SectionName})";
        }
    }
}
