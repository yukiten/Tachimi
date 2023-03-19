using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.IO;
using Tachimi.Data;
using Tachimi.Utilities;

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

            View.CreatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            View.Salt = Convert.ToBase64String(salt);

            // パスワードをハッシュ化
            View.Password = PasswordHasher.HashPassword(View.Password, View.Salt);

            // 画像ファイルをバイト配列に変換してモデルに格納
            if (View.ImageFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await View.ImageFile.CopyToAsync(memoryStream);
                    View.Image = memoryStream.ToArray();
                }
            }

            _context.Views.Add(View);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
