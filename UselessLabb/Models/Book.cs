using System.ComponentModel.DataAnnotations;

namespace UselessLabb.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва книги є обов'язковою")]
        [StringLength(200, ErrorMessage = "Назва не може перевищувати 200 символів")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Автор є обов'язковим")]
        [StringLength(100, ErrorMessage = "Ім'я автора не може перевищувати 100 символів")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "ISBN є обов'язковим")]
        [RegularExpression(@"^(?=(?:\D*\d){10}(?:(?:\D*\d){3})?$)[\d-]+$", ErrorMessage = "Невірний формат ISBN")]
        public string ISBN { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Дата публікації")]
        public DateTime? PublishDate { get; set; }

        [Range(0, 10000, ErrorMessage = "Ціна повинна бути між 0 та 10000")]
        [DataType(DataType.Currency)]
        [Display(Name = "Ціна")]
        public decimal Price { get; set; }

        [Display(Name = "Жанр")]
        public int? GenreId { get; set; }
        public Genre? Genre { get; set; }

        [Display(Name = "Видавництво")]
        public int? PublisherId { get; set; }
        public Publisher? Publisher { get; set; }

        [StringLength(1000, ErrorMessage = "Опис не може перевищувати 1000 символів")]
        [Display(Name = "Опис")]
        public string? Description { get; set; }

        [Display(Name = "Обкладинка")]
        public string? CoverImage { get; set; }
    }
}
