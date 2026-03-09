using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UselessLabb.Data;
using UselessLabb.Models;

namespace UselessLabb.Pages.Publishers
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Publisher> Publishers { get; set; } = new List<Publisher>();

        public async Task OnGetAsync()
        {
            Publishers = await _context.Publishers
                .Include(p => p.Books)
                .ToListAsync();
        }
    }
}
