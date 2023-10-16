using System.ComponentModel.DataAnnotations;

namespace Beleshki.Models
{
    public class Predmet
    {
        public int Id { get; set; }

        [Display(Name = "Predmet")]
        public string PredmetIme { get; set; }

        [StringLength(20)]
        public string Kod { get; set;}
        public int? Krediti { get; set;}
        public string? Institut { get; set; }

        [Display(Name = "Studiska godina")]
        public string? StudiskaGodina { get; set; }

        public ICollection<Beleshka>? Beleshki { get; set; }
        public ICollection<PredmetFakultet>? predmetFakulteti { get; set; }

    }
}
