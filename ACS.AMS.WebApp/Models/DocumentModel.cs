namespace ACS.AMS.WebApp.Models
{

    public class DocumentModel
    {
        private static int idCount = 1;
        public DocumentModel()
        {

            DocumentID = -1;
            DocumentIdentityID = idCount++;

            if (idCount > int.MaxValue / 2)
                idCount = 1;
        }
        public int? DocumentIdentityID { get; set; }
        public int DocumentID { get; set; }

        public int CurrentPageID { get; set; }
        public string FullPath { get; set; }

        public string FileName { get; set; }
        public string DocumentName { get; set; }
        public string FileExtension { get; set; }
        public bool ModelRemoved { get; set; } = false;

    }

}
