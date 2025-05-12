using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IvanovaElenaAleksandrovnaKt_41_22.Tests.Helpers.Validators
{
    public static class AcademicDegreeValidator
    {
        /// <summary>
        /// Проверяет, соответствует ли название учёной степени шаблону:
        /// - Начинается с заглавной буквы
        /// - После пробела — слово с маленькой буквы
        /// Пример: "Кандидат наук"
        /// </summary>
        public static bool IsValidName(string name)
        {
            // Регулярное выражение:
            // ^ — начало строки
            // [А-Я] — первая буква заглавная
            // [а-я]+ — остальные буквы строчные
            // \s — пробел
            // [а-я]+ — второе слово только строчные
            // $ — конец строки
            return Regex.IsMatch(name, @"^[А-Я][а-я]+\s[а-я]+$");
        }
    }

}
