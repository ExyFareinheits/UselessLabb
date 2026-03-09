using System.ComponentModel.DataAnnotations;

namespace UselessLabb.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва жанру є обов'язковою")]
        [StringLength(50, ErrorMessage = "Назва жанру не може перевищувати 50 символів")]
        [Display(Name = "Назва жанру")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Опис не може перевищувати 500 символів")]
        [Display(Name = "Опис")]
        public string? Description { get; set; }

        public ICollection<Book>? Books { get; set; }
    }
}
