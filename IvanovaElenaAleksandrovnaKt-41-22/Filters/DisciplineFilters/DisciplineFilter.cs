namespace IvanovaElenaAleksandrovnaKt_41_22.Filters.DisciplineFilters
{
    public class DisciplineFilter
    {
        public string? TeacherName { get; set; } // Имя преподавателя (FirstName + LastName)
        public int? MinHours { get; set; } // Минимальное количество часов
        public int? MaxHours { get; set; } // Максимальное количество часов
    }
}
