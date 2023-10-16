using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Beleshki.Models
{
    public class Fakultet
    {
        public int Id { get; set; }

        [Display(Name = "Fakultet")]
        public string FakultetIme { get; set; }

        [Display(Name = "Univerzitet")]
        public string UniverzitetIme { get; set; }

        [Display(Name = "Logo")]
        public string? LogoURL { get; set; }

        public ICollection<PredmetFakultet>? predmetiFakultet { get; set; }
    }
}
