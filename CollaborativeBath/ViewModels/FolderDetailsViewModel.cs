using CollaborativeBath.Models;

namespace CollaborativeBath.ViewModels
{
    public class FolderDetailsViewModel
    {
        public Folder Folder { get; set; }
        public Breadcrumb[] Breadcrumbs { get; set; }

        public bool Subscribed { get; set; }

        public FolderDetailsViewModel(Folder folder, Breadcrumb[] breadcrumbs, bool subbed)
        {
            Folder = folder;
            Breadcrumbs = breadcrumbs;
            Subscribed = subbed;
        }
    }
}