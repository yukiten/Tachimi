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
    public class DeleteModel : PageModel
    {
        private readonly Tachimi.Data.ApplicationDbContext _context;

        public DeleteModel(Tachimi.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public View View { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Views == null)
            {
                return NotFound();
            }

            var view = await _context.Views.FirstOrDefaultAsync(m => m.Id == id);

            if (view == null)
            {
                return NotFound();
            }
            else 
            {
                View = view;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Views == null)
            {
                return NotFound();
            }
            var view = await _context.Views.FindAsync(id);

            if (view != null)
            {
                View = view;
                _context.Views.Remove(View);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
