using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Tachimi.Data;
using Tachimi.Utilities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public View View { get; set; }


        [BindProperty]
        public string InputPassword { get; set; }

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

            var viewToUpdate = await _context.Views.FindAsync(id);

            if (viewToUpdate == null)
            {
                return NotFound();
            }

            // 現在のユーザーのIDを取得（nullの場合は未ログイン）
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 現在のユーザーのIDと作成者IDが一致しない場合、エラーを表示
            if (viewToUpdate.CreatorId != currentUserId)
            {
                ModelState.AddModelError(string.Empty, "You are not authorized to edit this item.");
                return Page();
            }

            // 入力されたパスワードが空またはnullでないことを確認
            if (string.IsNullOrEmpty(InputPassword))
            {
                ModelState.AddModelError(string.Empty, "Password is required.");
                return Page();
            }

            // パスワードが一致するか確認
            if (!PasswordHasher.VerifyPassword(InputPassword, viewToUpdate.Salt, viewToUpdate.Password))
            {
                ModelState.AddModelError(string.Empty, "Incorrect password.");
                return Page();
            }

            // 画像ファイルをバイト配列に変換してモデルに格納
            if (View.ImageFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await View.ImageFile.CopyToAsync(memoryStream);
                    viewToUpdate.Image = memoryStream.ToArray();
                }
            }

            if (await TryUpdateModelAsync<View>(
                viewToUpdate,
                "View",
                v => v.Title, v => v.Genre, v => v.Medium, v => v.Live, v => v.Host, v => v.Description, v => v.Hashtags))
            {
                viewToUpdate.Description = View.Description;
                viewToUpdate.Hashtags = View.Hashtags;

                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
