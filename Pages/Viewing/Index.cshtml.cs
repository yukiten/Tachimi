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
        public string CurrentFilter { get; set; }

        public async Task OnGetAsync(string searchHashtag)
        {
            CurrentFilter = searchHashtag;
            ViewData["HashtagFilter"] = searchHashtag;

            var views = await _context.Views.ToListAsync();
            View = FilterByHashtag(views, searchHashtag);
        }

        public IList<View> FilterByHashtag(IList<View> views, string hashtag)
        {
            if (string.IsNullOrEmpty(hashtag))
            {
                return views;
            }

            return views.Where(v => v.Hashtags.Split(' ').Any(h => h.Equals(hashtag, StringComparison.OrdinalIgnoreCase))).ToList();
        }

    }
}
