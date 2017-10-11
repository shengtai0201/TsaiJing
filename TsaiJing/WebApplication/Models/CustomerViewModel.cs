using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TsaiJing.WebApplication.Models
{
    public class CustomerViewModel : CustomerBase
    {
        public string Career { get; set; }
        public string IdCardNumber { get; set; }
        public string ConsultantName { get; set; }
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public string LineId { get; set; }
        public string Remark { get; set; }
    }
}
