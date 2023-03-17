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
    public class DetailsModel : PageModel
    {
        private readonly Tachimi.Data.ApplicationDbContext _context;

        public DetailsModel(Tachimi.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public View View { get; set; }

        public string RoomName;
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
                RoomName = view.Id.ToString();
            }
            return Page();
        }
    }
}
