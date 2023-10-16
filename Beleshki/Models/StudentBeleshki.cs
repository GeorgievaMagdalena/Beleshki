using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beleshki.Models
{
    public class StudentBeleshki
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string StudentIme { get; set; }

        public int BeleshkaId { get; set; }
        public Beleshka? Beleshka { get; set; }

        [NotMapped]
        public string? DownloadUrl => Beleshka?.DownloadUrl;
    }
}
