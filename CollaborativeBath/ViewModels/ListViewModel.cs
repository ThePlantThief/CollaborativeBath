using System.Collections.Generic;

namespace CollaborativeBath.ViewModels
{
    public class ListViewModel
    {
        public ICollection<CollaborativeBath.Models.Folder> List { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
}