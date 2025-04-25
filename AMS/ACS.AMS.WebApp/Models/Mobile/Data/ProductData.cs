namespace ACS.AMS.WebApp.Products.Mobile
{
    public class ProductData
    {
        public ProductData()
        {

        }
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int CategoryID { get; set; }
        public override string ToString()
        {
            return $"{ProductCode} ({ProductName})";
        }
    }
}
