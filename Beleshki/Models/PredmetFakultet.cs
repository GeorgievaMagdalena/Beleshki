namespace Beleshki.Models
{
    public class PredmetFakultet
    {
        public int Id { get; set; }

        public int PredmetId { get; set; }
        public Predmet? Predmet { get; set; }

        public int FakultetId { get; set; }
        public Fakultet? Fakultet { get; set; }
    }
}
