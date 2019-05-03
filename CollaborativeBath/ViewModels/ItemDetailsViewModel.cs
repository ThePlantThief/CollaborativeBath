using CollaborativeBath.Models;

namespace CollaborativeBath.ViewModels
{
    public class FileDetailsViewModel
    {
        public File File { get; set; }
        public Breadcrumb[] Breadcrumbs { get; set; }

        public int[] History { get; set; }

        public FileDetailsViewModel(File file, Breadcrumb[] breadcrumbs, int[] history)
        {
            File = file;
            Breadcrumbs = breadcrumbs;
            History = history;
        }
    }
}