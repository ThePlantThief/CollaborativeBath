using System.Web;

namespace CollaborativeBath.ViewModels.Material
{
    public class FileUploadViewModel
    {
        public HttpPostedFileBase File { get; set; }
        public int FolderId { get; set; }
        public string Title { get; set; }
        public int Id { get; set; }
    }
}