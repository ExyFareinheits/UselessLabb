using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UselessLabb.Data;
using UselessLabb.Models;

namespace UselessLabb.Pages.Books
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DeleteModel(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult OnGet() => RedirectToPage("./Index");

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);

            if (book != null)
            {
                // Delete cover image if exists
                if (!string.IsNullOrEmpty(book.CoverImage))
                {
                    var filePath = ResolveUploadPath(book.CoverImage);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        private static string ResolveUploadPath(string publicPath)
        {
            var uploadsRoot = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "UselessLabb",
                "uploads");

            var relativePath = publicPath
                .TrimStart('/')
                .Replace("uploads/", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace('/', Path.DirectorySeparatorChar);

            return Path.Combine(uploadsRoot, relativePath);
        }
    }
}
