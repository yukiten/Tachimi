using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tachimi.Data;

namespace Tachimi.Pages.Viewing
{
    public class CreateModel : PageModel
    {
        private readonly Tachimi.Data.ApplicationDbContext _context;

        public CreateModel(Tachimi.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public View View { get; set; }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Views.Add(View);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
