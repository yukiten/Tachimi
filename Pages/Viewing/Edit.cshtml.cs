using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tachimi.Data;

namespace Tachimi.Pages.Viewing
{
    public class EditModel : PageModel
    {
        private readonly Tachimi.Data.ApplicationDbContext _context;

        public EditModel(Tachimi.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public View View { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Views == null)
            {
                return NotFound();
            }

            var view =  await _context.Views.FirstOrDefaultAsync(m => m.Id == id);
            if (view == null)
            {
                return NotFound();
            }
            View = view;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(View).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ViewExists(View.Id))
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

        private bool ViewExists(int id)
        {
          return _context.Views.Any(e => e.Id == id);
        }
    }
}
