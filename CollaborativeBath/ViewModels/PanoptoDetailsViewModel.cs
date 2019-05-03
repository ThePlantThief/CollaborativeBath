using CollaborativeBath.Models;

namespace CollaborativeBath.ViewModels
{
    public class PanoptoDetailsViewModel
    {
        public Panopto Panopto { get; set; }
        public Breadcrumb[] Breadcrumbs { get; set; }

        public PanoptoDetailsViewModel(Panopto panopto, Breadcrumb[] breadcrumbs)
        {
            Panopto = panopto;
            Breadcrumbs = breadcrumbs;
        }
    }
}