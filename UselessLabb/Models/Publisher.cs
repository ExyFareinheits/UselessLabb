using System.ComponentModel.DataAnnotations;

namespace UselessLabb.Models
{
    public class Publisher
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва видавництва є обов'язковою")]
        [StringLength(200, ErrorMessage = "Назва не може перевищувати 200 символів")]
        [Display(Name = "Назва видавництва")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Адреса не може перевищувати 200 символів")]
        [Display(Name = "Адреса")]
        public string? Address { get; set; }

        [Phone(ErrorMessage = "Невірний формат телефону")]
        [Display(Name = "Телефон")]
        public string? Phone { get; set; }

        [EmailAddress(ErrorMessage = "Невірний формат email")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Url(ErrorMessage = "Невірний формат URL")]
        [Display(Name = "Веб-сайт")]
        public string? Website { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата заснування")]
        public DateTime? FoundedDate { get; set; }

        public ICollection<Book>? Books { get; set; }
    }
}
