using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TsaiJing.WebApplication.Models
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string CustomerName { get; set; }
        public SelectListItem Consultant { get; set; }
        public SelectListItem Role { get; set; }
        public bool LockedOut { get; set; }
    }
}
