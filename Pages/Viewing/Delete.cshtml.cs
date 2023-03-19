using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization; // 追加
using System.Security.Claims; // 追加
using Tachimi.Data;
using Tachimi.Utilities;
using Microsoft.AspNetCore.Identity;

namespace Tachimi.Pages.Viewing
{
    //[Authorize] // ページへのアクセスを認証済みユーザーに制限
    public class DeleteModel : PageModel
    {
        private readonly Tachimi.Data.ApplicationDbContext _context;

        public DeleteModel(Tachimi.Data.ApplicationDbContext context)
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
            var view = await _context.Views.FindAsync(id);

            if (view != null)
            {
                // 現在のユーザーのIDを取得（nullの場合は未ログイン）
                string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // 現在のユーザーのIDと作成者IDが一致しない場合、エラーを表示
                // ただし、一時的なアイテムの場合は、この制限を適用しない
                if (view.CreatorId != currentUserId && !view.IsTemporary)
                {
                    ModelState.AddModelError(string.Empty, "You are not authorized to delete this item.");
                    return Page();
                }

                // 入力されたパスワードが空またはnullでないことを確認
                if (string.IsNullOrEmpty(InputPassword))
                {
                    ModelState.AddModelError(string.Empty, "Password is required.");
                    return Page();
                }

                // パスワードが一致するか確認
                if (!PasswordHasher.VerifyPassword(InputPassword, view.Salt, view.Password))
                {
                    ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return Page();
                }

                View = view;
                _context.Views.Remove(View);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
