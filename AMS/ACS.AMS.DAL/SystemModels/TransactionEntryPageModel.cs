using ACS.AMS.DAL;

namespace ACS.WebAppPageGenerator.Models.SystemModels
{
    public partial class TransactionEntryPageModel : EntryPageModel
    {
        public string LineItemPageName { get; set; }

        public BaseEntityObject EntityLineItemInstance { get; set; }

        public List<PageFieldModel> TransactionLineFields { get; set; } = new List<PageFieldModel>();
    }
}