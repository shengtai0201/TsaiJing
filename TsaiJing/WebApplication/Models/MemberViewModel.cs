using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TsaiJing.WebApplication.Models
{
    public class MemberViewModel : CustomerBase
    {
        public string Mobile { get; set; }
        public string GuidanceName { get; set; }
        public string SuperviseName { get; set; }
        public SelectListItem MemberRole { get; set; }
        public DateTime? JoinDate { get; set; }
    }
}
