using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UselessLabb.Data;
using UselessLabb.Models;

namespace UselessLabb.Pages.Genres
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Genre> Genres { get; set; } = new List<Genre>();

        public async Task OnGetAsync()
        {
            Genres = await _context.Genres
                .Include(g => g.Books)
                .ToListAsync();
        }
    }
}
