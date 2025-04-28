namespace IvanovaElenaAleksandrovnaKt_41_22.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? FoundationDate { get; set; }
        public int? HeadId { get; set; } // Ссылается на преподавателя
        public Teacher? Head { get; set; } // Навигационное свойство
        public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();

    }
}
