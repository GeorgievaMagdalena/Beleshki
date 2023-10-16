using Microsoft.AspNetCore.Mvc.Rendering;
using Beleshki.Models;
using System.Collections.Generic;

namespace Beleshki.ViewModels
{
    public class PredmetFakultetEditViewModel
    {
        public Fakultet Fakultet { get; set; }
        public IEnumerable<int>? SelectedPredmeti { get; set; }
        public IEnumerable<SelectListItem>? PredmetiLista { get; set; }
    }
}
