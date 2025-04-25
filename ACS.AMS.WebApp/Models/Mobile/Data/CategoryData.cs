namespace ACS.AMS.WebApp.Models.Mobile
{
    public class CategoryData
    {
        public CategoryData()
        {

        }

        public int CategoryID { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public int? parentCategoryID { get; set; }

        public override string ToString()
        {
            return $"{CategoryCode} ({CategoryName})";
        }
    }
}
