using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UselessLabb.Data;
using UselessLabb.Models;

namespace UselessLabb.Pages.Books
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private const long MaxCoverSizeBytes = 2 * 1024 * 1024;
        private static readonly string[] AllowedCoverExtensions = [".jpg", ".jpeg", ".png", ".webp"];

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CreateModel(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public Book Book { get; set; } = new Book();

        [BindProperty]
        public IFormFile? CoverImageFile { get; set; }

        public SelectList GenresList { get; set; } = default!;
        public SelectList PublishersList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadSelectListsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ValidateCoverFile();

            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync();
                return Page();
            }

            if (CoverImageFile != null)
            {
                try
                {
                    Book.CoverImage = await SaveCoverAsync(CoverImageFile);
                }
                catch
                {
                    ModelState.AddModelError("CoverImageFile", "Не вдалося зберегти обкладинку. Спробуйте інше зображення.");
                    await LoadSelectListsAsync();
                    return Page();
                }
            }

            _context.Books.Add(Book);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private void ValidateCoverFile()
        {
            if (CoverImageFile == null)
            {
                return;
            }

            var extension = Path.GetExtension(CoverImageFile.FileName).ToLowerInvariant();
            if (!AllowedCoverExtensions.Contains(extension))
            {
                ModelState.AddModelError("CoverImageFile", "Дозволені формати: JPG, JPEG, PNG, WEBP.");
            }

            if (CoverImageFile.Length == 0 || CoverImageFile.Length > MaxCoverSizeBytes)
            {
                ModelState.AddModelError("CoverImageFile", "Розмір файлу має бути від 1 байта до 2MB.");
            }
        }

        private async Task<string> SaveCoverAsync(IFormFile file)
        {
            var webRoot = _webHostEnvironment.WebRootPath;
            if (string.IsNullOrWhiteSpace(webRoot))
            {
                webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var uploadsFolder = Path.Combine(webRoot, "uploads", "covers");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid():N}{Path.GetExtension(file.FileName).ToLowerInvariant()}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);

            return $"/uploads/covers/{uniqueFileName}";
        }

        private async Task LoadSelectListsAsync()
        {
            var genres = await _context.Genres.ToListAsync();
            var publishers = await _context.Publishers.ToListAsync();

            GenresList = new SelectList(genres, "Id", "Name");
            PublishersList = new SelectList(publishers, "Id", "Name");
        }
    }
}
