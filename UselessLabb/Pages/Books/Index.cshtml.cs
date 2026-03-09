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

        public async Task OnGetAsync()
        {
            Books = await _context.Books
                .Include(b => b.Genre)
                .Include(b => b.Publisher)
                .ToListAsync();
        }
    }
}
