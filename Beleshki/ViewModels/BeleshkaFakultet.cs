using Beleshki.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Beleshki.ViewModels
{
    public class BeleshkaFakultet
    {
        public IList<Beleshka> Beleshkii { get; set; }
       // public IList<Beleshka> Iminja { get; set; }
        public SelectList Fakulteti { get; set; }
        public string searchFakultet { get; set; }
        //public string searchIme { get; set; }
        public string searchPredmet { get; set; }
    }
}
