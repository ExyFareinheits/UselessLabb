using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UselessLabb.Data;
using UselessLabb.Models;

namespace UselessLabb.Pages.Books
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Book> Books { get; set; } = new List<Book>();

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public async Task OnGetAsync()
        {
            var booksQuery = _context.Books
                .Include(b => b.Genre)
                .Include(b => b.Publisher)
                .AsQueryable();

            if (!string.IsNullOrEmpty(SearchString))
            {
                var lowerSearch = SearchString.ToLower();
                booksQuery = booksQuery.Where(b => 
                    b.Title.ToLower().Contains(lowerSearch) || 
                    b.Author.ToLower().Contains(lowerSearch));
            }

            Books = await booksQuery.ToListAsync();
        }
    }
}
