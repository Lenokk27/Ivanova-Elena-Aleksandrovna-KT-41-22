namespace IvanovaElenaAleksandrovnaKt_41_22.Models
{
    public class Discipline
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Load> Loads { get; set; } = new List<Load>();
        //kkk
    }
}
