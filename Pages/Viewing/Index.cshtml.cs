using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tachimi.Data;

namespace Tachimi.Pages.Viewing
{
    public class IndexModel : PageModel
    {
        private readonly Tachimi.Data.ApplicationDbContext _context;

        public IndexModel(Tachimi.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<View> View { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Views != null)
            {
                View = await _context.Views.ToListAsync();
            }
        }
    }
}
