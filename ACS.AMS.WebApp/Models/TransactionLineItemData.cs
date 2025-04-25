namespace ACS.AMS.WebApp.Models
{
    public class TransactionLineItemData
    {
        private static int idGenerator = -1;

        public TransactionLineItemData()
        {
            if(idGenerator < -100000)
                idGenerator = -1;

            TransactionLineItemID = idGenerator--;
        }

        public int TransactionLineItemID { get; set; }

        public int ItemID { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public int? FromBinID { get; set; }

        public string FromBinCode { get; set; }

        public int? ToBinID { get; set; }

        public string ToBinCode { get; set; }

        public decimal TotalQty { get; set; }

        public decimal Qty { get; set; }

        public decimal PendingQty { get; set; }

        public int UOMID { get; set; }

        public string UOMCode { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalAmount { get => UnitPrice * Qty; }
    }
}
