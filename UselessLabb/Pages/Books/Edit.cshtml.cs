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
    public class EditModel : PageModel
    {
        private const long MaxCoverSizeBytes = 2 * 1024 * 1024;
        private static readonly string[] AllowedCoverExtensions = [".jpg", ".jpeg", ".png", ".webp"];

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EditModel(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        [BindProperty]
        public IFormFile? CoverImageFile { get; set; }

        public SelectList GenresList { get; set; } = default!;
        public SelectList PublishersList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            Book = book;
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
                    var oldCover = Book.CoverImage;
                    Book.CoverImage = await SaveCoverAsync(CoverImageFile);

                    if (!string.IsNullOrEmpty(oldCover))
                    {
                        var webRoot = _webHostEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                        var oldFilePath = Path.Combine(webRoot, oldCover.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }
                }
                catch
                {
                    ModelState.AddModelError("CoverImageFile", "Не вдалося зберегти обкладинку. Спробуйте інше зображення.");
                    await LoadSelectListsAsync();
                    return Page();
                }
            }

            _context.Attach(Book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(Book.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

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

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
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
