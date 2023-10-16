using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beleshki.Models
{
    public class Beleshka
    {
        public int Id { get; set; }

        [Display(Name = "Ime")]
        public string BeleshkaIme { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DatumKreiranje { get; set; }

        [StringLength(100)]
        public string? Opis { get; set; }

        [Display(Name = "Download")]
        public string? DownloadUrl { get; set; }
        
        public int PredmetId { get; set; }
        public Predmet? Predmet { get; set; }

        public ICollection<StudentBeleshki>? studentiBeleshki { get; set; }
        public ICollection<Komentar>? komentari { get; set; }


        [NotMapped]
        public double Ocenka { get; set; }


        [NotMapped]
        [Display(Name = "Prosechna Ocenka")]
        public double PresmetajProsechnaOcenka
        {
            get
            {
                if (komentari == null || komentari.Count == 0)
                {
                    return 0.0;
                }

                double sum = 0;
                foreach (var komentar in komentari)
                {
                    sum += komentar.Ocenka ?? 0;
                }

                return Math.Round(sum / komentari.Count, 2);
            }
        }
    }
}
