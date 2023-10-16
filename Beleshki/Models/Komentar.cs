using System.ComponentModel.DataAnnotations;

namespace Beleshki.Models
{
    public class Komentar
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string? StudentIme { get; set; }

        [StringLength(500)]
        public string komentar { get; set; }

        [Range(1,5)]
        public int? Ocenka { get; set; }

        public int BeleshkaId { get; set; }
        public Beleshka? Beleshka { get; set; }
    }
}
