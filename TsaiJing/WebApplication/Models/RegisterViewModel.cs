using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsaiJing.WebApplication.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "帳號")]
        public string Account { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 的長度至少必須為 {2} 個字元。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "確認密碼")]
        [Compare("Password", ErrorMessage = "密碼和確認密碼不相符。")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "電話")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "生日")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Birthday { get; set; }

        [Display(Name = "職業")]
        public string Career { get; set; }

        [Required]
        [Display(Name = "地址")]
        public string Address { get; set; }

        [Display(Name = "身份證字號")]
        public string IdCardNumber { get; set; }

        [Required]
        [Display(Name = "介紹人")]
        public string Introducer { get; set; }

        [Display(Name = "身高")]
        public int? Height { get; set; }

        [Display(Name = "體重")]
        public int? Weight { get; set; }

        [Display(Name = "E-mail")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "LineID")]
        public string LineId { get; set; }
    }
}
