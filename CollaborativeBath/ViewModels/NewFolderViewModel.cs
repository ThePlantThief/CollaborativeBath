using CollaborativeBath.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CollaborativeBath.ViewModels
{
    public class NewFolderViewModel
    {
        public Folder Parent { get; set; }
        public string Title { get; set; }
        public FolderType Type { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }
    }
}