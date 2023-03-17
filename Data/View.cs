using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Tachimi.Data
{
    public class View
    {
        public int Id { get; set; }
        [DisplayName("タイトル")]
        [Required(ErrorMessage = "{0}は必須です。")]
        public string Title { get; set; } = String.Empty;
        [DisplayName("ジャンル")]
        public string Genre { get; set; }
        [DisplayName("媒体")]
        [StringLength(20, ErrorMessage = "{0}は{1}文字以内で入力してください。")]
        public string Medium { get; set; } = String.Empty;
        [DisplayName("LIVE")]
        public bool Live { get; set; }
        [DisplayName("host")]
        public string Host { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
