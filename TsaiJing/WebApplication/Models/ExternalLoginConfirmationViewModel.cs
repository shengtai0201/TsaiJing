using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsaiJing.WebApplication.Models
{
    public class ExternalLoginConfirmationViewModel
    {
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

        //[Required]
        //[Display(Name = "技術指導")]
        //public string GuidanceId { get; set; }

        //public IUserService UserService { get; set; }

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
